using GloboTicket.API.Models;
using GloboTicket.Domain.Models;
using GloboTicket.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedController : Controller
{
    private readonly FeedService feedService;

    public FeedController(FeedService feedService)
    {
        this.feedService = feedService;
    }

    [HttpGet]
    [Route("shows")]
    public ActionResult<IAsyncEnumerable<ShowItem>> ShowsFeed()
    {
        var shows = feedService.ListShows();
        var showItems = shows.Select(show => new ShowItem
        {
            HrefShow = Url.Action(
                "GetShow",
                "Shows",
                new { showGuid = show.ShowGuid })!,
            HrefAct = Url.Action(
                "GetAct",
                "Acts",
                new { actGuid = show.ActGuid })!,
            HrefVenue = Url.Action(
                "GetVenue",
                "Venues",
                new { venueGuid = show.VenueGuid })!,
            Date = show.Date
        });
        return Ok(showItems);
    }
}
