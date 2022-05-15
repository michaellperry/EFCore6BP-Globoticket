using GloboTicket.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

public class VenuesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("{venueGuid}")]
    public async Task<ActionResult<VenueModel>> GetVenue([FromRoute] Guid venueGuid)
    {
        throw new NotImplementedException();
    }
}
