using Kamino.Entities;
using Kamino.Models;
using Kamino.Repo.Npgsql;
using Kamino.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint.Controllers;

[Route("")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class ProfilesController(IDbContextFactory<NpgsqlContext> contextFactory)
    : ContextualController
{
    public async Task<IActionResult> Index()
    {
        return await Contextualize(() => IndexHtml(), () => IndexJson());
    }

    private async Task<IActionResult> IndexHtml()
    {
        return await Task.FromResult(View("index.html"));
    }

    private async Task<IActionResult> IndexJson()
    {
        var factory = new ProfileActivityModelFactory(Request.GetEndpoint());
        var model = await IndexModel(factory);

        return Json(Contextify(model));
    }

    private async Task<TModel> IndexModel<TModel>(ModelFactoryBase<Profile, TModel> factory)
    {
        using var context = contextFactory.CreateDbContext();

        var service = new ProfilesService(context);
        var model = await service.GetPublicProfileAsync(factory);

        return model;
    }
}
