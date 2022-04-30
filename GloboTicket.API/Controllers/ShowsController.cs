using GloboTicket.API.Commands;
using GloboTicket.API.Models;
using GloboTicket.API.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly ListShowsQuery listShowsQuery;
    private readonly RescheduleShowCommand rescheduleShowCommand;

    public ShowsController(ListShowsQuery listShowsQuery, RescheduleShowCommand updateShowCommand)
    {
        this.listShowsQuery = listShowsQuery;
        this.rescheduleShowCommand = updateShowCommand;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowModel>>> GetShows()
    {
        List<ShowModel> shows = await listShowsQuery.Execute();
        return Ok(shows);
    }

    [HttpPut]
    [Route("{showGuid}")]
    public async Task<ActionResult> UpdateShow([FromRoute]Guid showGuid, [FromBody] ShowModel updatedShow)
    {
        await rescheduleShowCommand.Execute(showGuid, updatedShow.Date);
        return Ok();
    }
}
