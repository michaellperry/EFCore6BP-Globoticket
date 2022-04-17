using GloboTicket.API.Models;
using GloboTicket.Domain;
using GloboTicket.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly GloboTicketContext context;

    public ShowsController(GloboTicketContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowModel>>> GetShows()
    {
        var shows = await context.Set<Show>()
            .Select(show => new ShowModel
            {
                ActName = show.Act.Name,
                VenueName = show.Venue.Name,
                VenueAddress = show.Venue.Address,
                Date = show.Date
            })
            .ToListAsync();
        return Ok(shows);
    }
}
