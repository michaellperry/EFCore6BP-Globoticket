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
    public ActionResult<IAsyncEnumerable<ShowInfo>> ShowsFeed()
    {
        var shows = feedService.ListShows();
        return Ok(shows);
    }
}
