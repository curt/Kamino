using Kamino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint.Controllers;

public abstract class ContextualController : Controller
{
    private static readonly string[] ActivityMediaTypes = [
        "application/ld+json",
        "application/activity+json",
        "application/json"
    ];

    protected async Task<IActionResult> Contextualize(Func<Task<IActionResult>> html, Func<Task<IActionResult>> json)
    {
        var acceptHeaders = Request.GetTypedHeaders().Accept;
        var jsonAcceptHeader = acceptHeaders.FirstOrDefault(x => ActivityMediaTypes.Contains(x.MediaType.Value));

        if (jsonAcceptHeader != null)
        {
            Response.ContentType = jsonAcceptHeader.MediaType.Buffer;
            return await json();
        }

        return await html();
    }

    protected TModel Contextify<TModel>(TModel core) where TModel : ActivityModelBase
    {
        core.JsonLdContext = "https://www.w3.org/ns/activitystreams";

        return core;
    }
}
