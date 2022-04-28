using GloboTicket.API.Models;
using GloboTicket.API.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly ListShowsQuery listShowsQuery;

    public ShowsController(ListShowsQuery listShowsQuery)
    {
        this.listShowsQuery = listShowsQuery;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowModel>>> GetShows()
    {
        List<ShowModel> shows = await listShowsQuery.Execute();
        return Ok(shows);
    }
}
