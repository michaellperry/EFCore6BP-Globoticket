using GloboTicket.Domain;
using GloboTicket.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedController : Controller
{
    private readonly GloboTicketContext context;

    public FeedController(GloboTicketContext context)
    {
        this.context = context;
    }

    [HttpGet]
    [Route("shows")]
    public ActionResult<IAsyncEnumerable<ShowInfo>> ShowsFeed()
    {
        var shows = context.Set<Show>()
            .Select(show => new ShowInfo(
                show.ShowGuid,
                show.Act.ActGuid,
                show.Venue.VenueGuid,
                show.Date
            ))
            .AsAsyncEnumerable();
        return Ok(shows);
    }

    [HttpGet]
    [Route("list")]
    public ActionResult<List<ShowInfo>> ShowList()
    {
        var shows = context.Set<Show>()
            .Select(show => new ShowInfo(
                show.ShowGuid,
                show.Act.ActGuid,
                show.Venue.VenueGuid,
                show.Date
            ))
            .ToList();
        return Ok(shows);
    }

    [HttpGet]
    [Route("enumerable")]
    public ActionResult<IEnumerable<ShowInfo>> ShowEnumerable()
    {
        var shows = context.Set<Show>()
            .Select(show => new ShowInfo(
                show.ShowGuid,
                show.Act.ActGuid,
                show.Venue.VenueGuid,
                show.Date
            ))
            .AsEnumerable();
        return Ok(shows);
    }
}

public class ShowInfo
{
    public Guid ShowGuid { get; }
    public Guid ActGuid { get; }
    public Guid VenueGuid { get; }
    public DateTimeOffset Date { get; }

    public ShowInfo(Guid showGuid, Guid actGuid, Guid venueGuid, DateTimeOffset date)
    {
        ShowGuid = showGuid;
        ActGuid = actGuid;
        VenueGuid = venueGuid;
        Date = date;
    }

    public override bool Equals(object? obj)
    {
        return obj is ShowInfo other &&
               ShowGuid.Equals(other.ShowGuid) &&
               ActGuid.Equals(other.ActGuid) &&
               VenueGuid.Equals(other.VenueGuid) &&
               Date.Equals(other.Date);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ShowGuid, ActGuid, VenueGuid, Date);
    }
}