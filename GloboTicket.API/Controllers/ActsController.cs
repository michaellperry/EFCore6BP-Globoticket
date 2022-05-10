using GloboTicket.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActsController : Controller
{
    [HttpGet]
    [Route("{actGuid}")]
    public async Task<ActionResult<ActModel>> GetAct([FromRoute] Guid actGuid)
    {
        throw new NotImplementedException();
    }
}
