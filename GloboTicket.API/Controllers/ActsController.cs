using GloboTicket.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

public class ActsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("{actGuid}")]
    public async Task<ActionResult<ActModel>> GetAct([FromRoute] Guid actGuid)
    {
        throw new NotImplementedException();
    }
}
