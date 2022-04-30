using GloboTicket.API.Commands;
using GloboTicket.API.Models;
using GloboTicket.API.Queries;
using GloboTicket.Domain.Entities;
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
        List<Show> shows = await listShowsQuery.Execute();
        List<ShowModel> showModels = shows
            .Select(show => new ShowModel
            {
                ActName = show.Act.Name,
                VenueName = show.Venue.Name,
                VenueAddress = show.Venue.Address,
                Date = show.Date,

                HrefShow = Url.Action(nameof(UpdateShow), new { showGuid = show.ShowGuid })!
            })
            .ToList();
        return Ok(showModels);
    }

    [HttpPatch]
    [Route("{showGuid}")]
    public async Task<ActionResult> UpdateShow([FromRoute]Guid showGuid, [FromBody] ShowPatchModel showPatch)
    {
        if (showPatch.Date is DateTimeOffset date)
        {
            await rescheduleShowCommand.Execute(showGuid, date);
        }
        return Ok();
    }
}
