using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentValidation.Results;
using Kamino.Entities;
using Kamino.Repo.Npgsql;
using Kamino.Shared.Models;
using Kamino.Shared.Validators;
using Medo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SevenKilo.HttpSignatures;

namespace Kamino.Shared.Services;

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
        var activityActorUri = activity.GetUri("actor")!;

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
        var actorUri = actor.GetUri("id")!;
        var keyOwnerUri = keyProvider.Owner!;

        if (!(actorUri == keyOwnerUri && actorUri == activityActorUri))
        {
            logger.LogWarning(
                "Mismatch between activity actor and actor identifier for '{keyId}'.",
                keyId
            );
            throw new BadRequestException();
        }

        switch (activity.GetString("type"))
        {
            case "Create":
                await CreateAsync(activity, actor);
                break;
            case "Follow":
                await FollowAsync(activity, actor);
                break;
            case "Like":
                await LikeAsync(activity, actorUri);
                break;
            case "Ping":
                await PingAsync(activity, actor);
                break;
            case "Pong":
                await PongAsync(activity, actor);
                break;
            case "Undo":
                await UndoAsync(activity, actorUri);
                break;
        }
    }

    internal async Task CreateAsync(JsonObject activity, JsonObject actor)
    {
        await Task.Run(() => { });
    }

    internal async Task LikeAsync(JsonObject activity, Uri actorUri)
    {
        var activityUri = NormalizeIdentifier(activity, "id", "like");
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
                "Like already found for actor '{actorUri}' and object '{objectUri}'.",
                actorUri,
                objectUri
            );
        }
    }

    internal async Task FollowAsync(JsonObject activity, JsonObject actor)
    {
        var activityUri = NormalizeIdentifier(activity, "id", "follow");
        var actorUri = NormalizeIdentifier(activity, "actor");
        var objectUri = NormalizeIdentifier(activity, "object");
        var acceptUri = GenerateLocalIdentifier("accept/follow");
        var actorInbox = actor.GetUri("inbox")!;

        using var context = contextFactory.CreateDbContext();

        var match = await context
            .Follows.Where(e => e.ActorUri == actorUri && e.ObjectUri == objectUri)
            .AnyAsync();

        if (!match)
        {
            var follow = new Follow
            {
                ActivityUri = activityUri,
                AcceptUri = acceptUri,
                ActorUri = actorUri,
                ObjectUri = objectUri,
            };
            context.Add(follow);

            if (await context.SaveChangesAsync() > 0)
            {
                logger.LogInformation(
                    "Follow added for actor '{actorUri}' and object '{objectUri}'.",
                    actorUri,
                    objectUri
                );
            }

            var response = new FollowAcceptOutboundModel(follow);
            var http = new SignedHttpPostService(
                contextFactory,
                httpClientFactory,
                accessor.HttpContext!.Request.GetEndpoint()
            );
            await http.PostAsync(actorInbox, response);
        }
        else
        {
            logger.LogInformation(
                "Follow already found for actor '{actorUri}' and object '{objectUri}'.",
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
        var actorInbox = actor.GetUri("inbox")!;

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

            if (await context.SaveChangesAsync() > 0)
            {
                logger.LogInformation("Ping added for activity '{activityUri}'.", activityUri);
            }

            var response = new PongOutboundModel(pong);
            var http = new SignedHttpPostService(
                contextFactory,
                httpClientFactory,
                accessor.HttpContext!.Request.GetEndpoint()
            );
            await http.PostAsync(actorInbox, response);
        }
        else
        {
            logger.LogInformation("Ping already found for activity '{activityUri}'.", activityUri);
        }
    }

    internal async Task PongAsync(JsonObject activity, JsonObject actor)
    {
        await Task.Run(() => { });
    }

    internal async Task UndoAsync(JsonObject activity, Uri actorUri)
    {
        var undone = false;
        var actObjectUri = NormalizeIdentifier(activity, "object");

        if (actObjectUri != null)
        {
            undone =
                undone
                || await UndoLikeAsync(
                    f => f.ActorUri == actorUri && f.ActivityUri == actObjectUri,
                    $"activity '{actObjectUri}'"
                )
                || await UndoFollowAsync(
                    f => f.ActorUri == actorUri && f.ActivityUri == actObjectUri,
                    $"activity '{actObjectUri}'"
                );
        }

        var obj = activity["object"]!;
        if (!undone && obj.GetValueKind() == JsonValueKind.Object)
        {
            var objObjectUri = NormalizeIdentifier(obj.AsObject(), "object");
            var objActorUri = NormalizeIdentifier(obj.AsObject(), "actor");

            if (!undone && actorUri == objActorUri)
            {
                switch (obj.GetString("type"))
                {
                    case "Like":
                        await UndoLikeAsync(
                            f => f.ActorUri == objActorUri && f.ObjectUri == objObjectUri,
                            $"actor '{objActorUri}' and '{objObjectUri}'"
                        );
                        break;
                    case "Follow":
                        await UndoFollowAsync(
                            f => f.ActorUri == objActorUri && f.ObjectUri == objObjectUri,
                            $"actor '{objActorUri}' and '{objObjectUri}'"
                        );
                        break;
                }
            }
        }
    }

    private async Task<bool> UndoLikeAsync(Expression<Func<Like, bool>> predicate, string msg) =>
        await UndoEntityAsync(c => c.Likes, predicate, msg);

    private async Task<bool> UndoFollowAsync(
        Expression<Func<Follow, bool>> predicate,
        string msg
    ) => await UndoEntityAsync(c => c.Follows, predicate, msg);

    private async Task<bool> UndoEntityAsync<T>(
        Func<NpgsqlContext, DbSet<T>> dbSet,
        Expression<Func<T, bool>> predicate,
        string msg
    )
        where T : class
    {
        using var context = contextFactory.CreateDbContext();

        var entity = await dbSet.Invoke(context).Where(predicate).FirstOrDefaultAsync();

        if (entity != null)
        {
            context.Remove(entity);
            if (await context.SaveChangesAsync() > 0)
            {
                logger.LogInformation("{t} removed for {msg}.", typeof(T).Name, msg);

                return true;
            }
        }

        return false;
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

    private string GetSignatureHeader(Dictionary<string, string> headers)
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
        string? str = null;
        var node = obj[property];
        if (node != null)
        {
            if (node.GetValueKind() == JsonValueKind.Object)
            {
                str = node.GetString("id") ?? node.GetString("href");
            }
            else if (node.GetValueKind() == JsonValueKind.String)
            {
                str = node.GetString();
            }
        }
        if (path != null)
        {
            str ??= GenerateLocalIdentifier(path).ToString();
        }

        return str != null ? new Uri(str) : null;
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
