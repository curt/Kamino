using System.Text.Json;
using System.Text.Json.Nodes;
using FluentValidation.Results;
using Kamino.Entities;
using Kamino.Models;
using Kamino.Repo.Npgsql;
using Kamino.Validators;
using Medo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SevenKilo.HttpSignatures;

namespace Kamino.Services;

public class InboxService(
    IDbContextFactory<NpgsqlContext> contextFactory,
    IConfiguration configuration,
    ILogger<InboxService> logger,
    IHttpContextAccessor accessor,
    IHttpClientFactory httpClientFactory
) : IInboxService
{
    public async Task ReceiveAsync(JsonObject activity)
    {
        ValidateInboundActivity(activity);
        PrenormalizeInboundActivity(activity);
        var activityActorUri = new Uri(activity["actor"]!.ToString());

        // TODO: Check actor blocklist.

        var signature = GetSignatureHeader(ParseHeaders());
        var signatureModel = GetSignatureModel(signature);
        ValidateSignatureModel(signatureModel);

        var keyProvider = new KeyProvider(logger, httpClientFactory);
        var keyId = signatureModel.KeyId;

        if (configuration.GetValue("HttpSignatures:Required", true))
        {
            var verifier = new Signature(keyProvider);
            var request = new InboundVerificationRequest(accessor);
            var result = await verifier.VerifyAsync(request);

            if (result.Errors.Any())
            {
                logger.LogWarning("Activity failed signature verification.");
                throw new BadRequestException();
            }
        }
        else
        {
            var _ =
                await keyProvider.GetKeyModelByKeyIdAsync(keyId) ?? throw new BadRequestException();
        }

        var actor = keyProvider.Actor!;
        var actorUri = new Uri(actor["id"]!.ToString());
        var keyOwnerUri = keyProvider.Owner!;

        if (!(keyOwnerUri == actorUri && actorUri == activityActorUri))
        {
            logger.LogWarning(
                "Mismatch between activity actor and actor identifier for '{keyId}'.",
                keyId
            );
            throw new BadRequestException();
        }

        switch (activity["type"]!.ToString())
        {
            case "Create":
                await CreateAsync(activity, actor);
                break;
            case "Like":
                await LikeAsync(activity, actor);
                break;
            case "Ping":
                await PingAsync(activity, actor);
                break;
            case "Pong":
                await PongAsync(activity, actor);
                break;
        }
    }

    internal async Task CreateAsync(JsonObject activity, JsonObject actor)
    {
        await Task.Run(() => { });
    }

    internal async Task LikeAsync(JsonObject activity, JsonObject actor)
    {
        var activityUri = NormalizeIdentifier(activity, "id", "activity");
        var actorUri = NormalizeIdentifier(activity, "actor");
        var objectUri = NormalizeIdentifier(activity, "object");

        using var context = contextFactory.CreateDbContext();

        var match = await context
            .Likes.Where(e => e.ActorUri == actorUri && e.ObjectUri == objectUri)
            .AnyAsync();

        if (!match)
        {
            var like = new Like
            {
                ActivityUri = activityUri,
                ActorUri = actorUri,
                ObjectUri = objectUri,
            };

            context.Add(like);
            await context.SaveChangesAsync();
        }
        else
        {
            logger.LogInformation(
                "Like already found in repository for actor '{actorUri}' and object '{objectUri}'.",
                actorUri,
                objectUri
            );
        }
    }

    internal async Task PingAsync(JsonObject activity, JsonObject actor)
    {
        var activityUri = NormalizeIdentifier(activity, "id", "ping");
        var actorUri = NormalizeIdentifier(activity, "actor");
        var toUri = NormalizeIdentifier(activity, "to");
        var actorInbox = new Uri(actor["inbox"]!.ToString());

        using var context = contextFactory.CreateDbContext();

        var match = await context.Pings.Where(e => e.ActivityUri == activityUri).AnyAsync();

        if (!match)
        {
            var ping = new Ping
            {
                ActivityUri = activityUri,
                ActorUri = actorUri,
                ToUri = toUri,
            };
            var pong = new Pong { ActivityUri = GenerateLocalIdentifier("pong"), Ping = ping };
            context.Add(pong);
            await context.SaveChangesAsync();
            var response = new PongOutboundModel(pong);
            // TODO: Response pong.
            var post = new SignedHttpPostService(
                contextFactory,
                httpClientFactory,
                accessor.HttpContext!.Request.GetEndpoint()
            );
            await post.PostAsync(actorInbox, response);
        }
        else
        {
            logger.LogInformation(
                "Ping already found in repository for activity '{activityUri}'.",
                activityUri
            );
        }
    }

    internal async Task PongAsync(JsonObject activity, JsonObject actor)
    {
        await Task.Run(() => { });
    }

    private Dictionary<string, string> ParseHeaders()
    {
        return accessor
                .HttpContext?.Request.Headers.SelectMany(h =>
                    h.Value.Select(v => new KeyValuePair<string, string>(
                        h.Key.ToLowerInvariant(),
                        v ?? string.Empty
                    ))
                )
                .ToDictionary() ?? [];
    }

    private string GetSignatureHeader(IDictionary<string, string> headers)
    {
        if (!headers.TryGetValue("signature", out var signature))
        {
            logger.LogWarning("'Signature' header not found among request headers.");
            throw new BadRequestException();
        }

        return signature;
    }

    private SignatureModel GetSignatureModel(string signature)
    {
        var result = SignatureParser.Parse(signature, out var signatureModel);

        if (result)
        {
            return signatureModel!;
        }

        logger.LogWarning(
            "Failed to retrieve signature model: {errors}.",
            string.Join(" ", result.Errors)
        );

        throw new BadRequestException();
    }

    private void ValidateSignatureModel(SignatureModel signatureModel)
    {
        var validator = new SignatureModelValidator();
        var result = validator.Validate(signatureModel);
        ValidateResult(result, "Signature header failed validation: {errors}.");
    }

    private void ValidateInboundActivity(JsonObject inboundActivity)
    {
        var validator = new InboundActivityValidator();
        var result = validator.Validate(inboundActivity);
        ValidateResult(result, "Inbound activity failed validation: {errors}.");
    }

    private void ValidateResult(ValidationResult result, string message)
    {
        if (!result.IsValid)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.ErrorMessage));
            logger.LogWarning(message, errors);
            throw new BadRequestException();
        }
    }

    private void PrenormalizeInboundActivity(JsonObject inboundActivity)
    {
        var actor = inboundActivity["actor"]!;

        if (actor.GetValueKind() != JsonValueKind.String)
        {
            var id = actor["id"] ?? actor["href"];

            if (id != null && id.GetValueKind() == JsonValueKind.String)
            {
                inboundActivity["actor"] = id;
            }
            else
            {
                logger.LogWarning("Inbound activity lacks normalizable actor identifier.");
                throw new BadRequestException();
            }
        }
    }

    private static Uri? NormalizeIdentifier(JsonObject obj, string property, string? path = null)
    {
        var node = obj[property];
        if (node != null)
        {
            if (node.GetValueKind() == JsonValueKind.Object)
            {
                obj[property] = node["id"]?.ToString() ?? node["href"]?.ToString();
            }
        }
        if (path != null)
        {
            obj[property] ??= GenerateLocalIdentifier(path).ToString();
        }

        return obj[property] != null ? new Uri(obj[property]!.ToString()) : null;
    }

    private static Uri GenerateLocalIdentifier(string context)
    {
        context = context.Trim('/');
        var authority = Constants.InternalHost;
        var date = DateTime.UtcNow.Year;
        var id = Uuid7.NewUuid7().ToId22String();
        return new Uri($"tag:{authority},{date}:{context}/{id}");
    }
}
