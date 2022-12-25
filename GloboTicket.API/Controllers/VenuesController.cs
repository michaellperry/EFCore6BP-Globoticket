using GloboTicket.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VenuesController : Controller
{
    [HttpGet]
    [Route("{venueGuid}")]
    public async Task<ActionResult<VenueModel>> GetVenue([FromRoute] Guid venueGuid)
    {
        throw new NotImplementedException();
    }
}
