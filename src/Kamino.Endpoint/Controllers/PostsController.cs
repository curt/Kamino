using Kamino.Entities;
using Kamino.Models;
using Kamino.Services;
using Medo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kamino.Endpoint.Controllers;

[Route("p")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class PostsController(IDbContextFactory<ApplicationContext> contextFactory) : ContextualController
{
    public async Task<IActionResult> Index()
    {
        return await Contextualize(() => IndexHtml(), () => IndexJson());
    }

    [Route("{id:regex(^[[1-9A-Za-z]]{{22}}$)}")]
    public async Task<IActionResult> Get(string id)
    {
        var guid = Uuid7.FromId22String(id).ToGuid();

        return await Contextualize(() => GetHtml(guid), () => GetJson(guid));
    }

    private async Task<IActionResult> IndexHtml()
    {
        var factory = new PostViewModelFactory(Request.GetEndpoint());
        var model = await IndexModel(factory);

        return View("index.html", model);
    }

    private async Task<IActionResult> IndexJson()
    {
        var factory = new PostActivityModelFactory(Request.GetEndpoint());
        var model = await IndexModel(factory);

        return Json(model.Select(m => Contextify(m)));
    }

    private async Task<IEnumerable<TModel>> IndexModel<TModel>(ModelFactoryBase<Post, TModel> factory)
    {
        using var context = contextFactory.CreateDbContext();

        var postsService = new PostsService(context);
        var model = await postsService.GetPublicPostsAsync(factory);

        return model;
    }

    private async Task<IActionResult> GetHtml(Guid id)
    {
        var factory = new PostViewModelFactory(Request.GetEndpoint());
        var model = await GetModel(id, factory);

        return View("get.html", model);
    }

    private async Task<IActionResult> GetJson(Guid id)
    {
        var factory = new PostActivityModelFactory(Request.GetEndpoint());
        var model = await GetModel(id, factory);

        return Json(Contextify(model));
    }

    private async Task<TModel> GetModel<TModel>(Guid id, ModelFactoryBase<Post, TModel> factory)
    {
        using var context = contextFactory.CreateDbContext();

        var postsService = new PostsService(context);
        var model = await postsService.GetSinglePublicPostByIdAsync(id, factory);

        return model;
    }
}
