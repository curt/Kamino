using Kamino.Models;
using Kamino.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SevenKilo.HttpSignatures;
using System.Text.Json;

namespace Kamino.Services;

public class InboxService(ILogger<InboxService> logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory) : IInboxService
{
    public void Receive(ObjectInboxModel inboxModel)
    {
        var signature = GetSignatureHeader(ParseHeaders());
        var signatureModel = GetSignatureModel(signature);

        ValidateSignatureModel(signatureModel);
        ValidateActivity(inboxModel);

        // TODO: normalize inbound activity

        var keyProvider = new KeyProvider(httpClientFactory);
        var key = keyProvider.Get(signatureModel.KeyId);

        if (key == null)
        {
            return; // TODO: handle rejection
        }

        // TODO: reject if actor does not match key owner
    }

    private Dictionary<string, string> ParseHeaders()
    {
        return accessor.HttpContext?.Request.Headers
            .SelectMany
            (
                h => h.Value.Select
                (
                    v => new KeyValuePair<string, string>
                    (
                        h.Key.ToLowerInvariant(),
                        v ?? string.Empty
                    )
                )
            ).ToDictionary() ?? [];

    }

    private byte[] ParseBody()
    {
        var body = new MemoryStream();
        accessor.HttpContext?.Request.BodyReader.AsStream().CopyTo(body);
        return body.ToArray();
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

        logger.LogWarning("Failed to retrieve signature model: {errors}", string.Join(" ", result.Errors));
        throw new BadRequestException();
    }

    private void ValidateSignatureModel(SignatureModel signatureModel)
    {
        var validator = new SignatureModelValidator();
        var result = validator.Validate(signatureModel);

        if (!result.IsValid)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.ErrorMessage));
            logger.LogWarning("Signature header failed validation: {errors}", errors);
            throw new BadRequestException();
        }
    }

    private void ValidateActivity(ObjectInboxModel activity)
    {
        var validator = new ObjectInboxModelValidator();
        var result = validator.Validate(activity);

        if (!result.IsValid)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.ErrorMessage));
            logger.LogWarning("Activity not in valid format: {errors}", errors);
            throw new BadRequestException();
        }
    }

    // private static ObjectInboxModel NormalizeActivity(JsonElement activity)
    // {
    //     var model = new ObjectInboxModel()
    //     {
    //         Id = activity.GetStringProperty("id"),
    //         Type = activity.GetStringProperty("type"),
    //         Actor = NormalizeDisjointObject(activity, "actor")
    //     };

    //     return model;
    // }

    // private static ObjectInboxModel? NormalizeDisjointObject(JsonElement element, string propertyName)
    // {
    //     if (element.TryGetProperty(propertyName, out var innerElement))
    //     {
    //         var obj = new ObjectInboxModel();

    //         if (innerElement.ValueKind == JsonValueKind.String)
    //         {
    //             obj.Id = innerElement.GetString();
    //         }

    //         if (innerElement.ValueKind == JsonValueKind.Object)
    //         {
    //             obj.Id = innerElement.GetStringProperty("id") ?? innerElement.GetStringProperty("href");
    //             obj.Type = innerElement.GetStringProperty("type");
    //         }

    //         return obj;
    //     }

    //     return null;
    // }
}
