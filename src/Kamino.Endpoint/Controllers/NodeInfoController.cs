using Microsoft.AspNetCore.Mvc;

namespace Kamino.Endpoint.Controllers;

[Route("")]
[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, NoStore = false)]
public class NodeInfoController : Controller
{
    [HttpGet(".well-known/nodeinfo")]
    public ActionResult Index()
    {
        var model = new Dictionary<string, object>(
            [
                new(
                    "links",
                    new Dictionary<string, object?>(
                        [
                            new("rel", "http://nodeinfo.diaspora.software/ns/schema/2.0"),
                            new(
                                "href",
                                Url.ActionLink(action: "Get", values: new { Version = "2.0" })
                            ),
                        ]
                    )
                ),
            ]
        );

        return Json(model);
    }

    [HttpGet("nodeinfo/{version}")]
    public ActionResult Get(string version)
    {
        if (version != "2.0")
        {
            return NotFound();
        }

        var model = new Dictionary<string, object>(
            [
                new("version", "2.0"),
                new("protocols", new List<string>(["activitypub"])),
                new(
                    "software",
                    new Dictionary<string, string>(
                        [
                            new("name", "Kamino"),
                            new("version", ThisAssembly.AssemblyInformationalVersion),
                        ]
                    )
                ),
                new(
                    "services",
                    new Dictionary<string, List<string>>([new("outbound", []), new("inbound", [])])
                ),
            ]
        );

        return Json(model);
    }
}
