using GloboTicket.API.Commands;
using GloboTicket.API.Models;
using GloboTicket.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly PromotionService promotionService;
    private readonly RescheduleShowCommand rescheduleShowCommand;

    public ShowsController(RescheduleShowCommand updateShowCommand, PromotionService promotionService)
    {
        this.rescheduleShowCommand = updateShowCommand;
        this.promotionService = promotionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowModel>>> GetShows(
        [FromQuery] DateTimeOffset? start = null,
        [FromQuery] DateTimeOffset? end = null,
        [FromQuery] double? latitude = null,
        [FromQuery] double? longitude = null,
        [FromQuery] int? meters = null
    )
    {
        if (start is DateTimeOffset startVal &&
            end is DateTimeOffset endVal &&
            start <= end &&
            latitude is double latitudeVal &&
            longitude is double longitudeVal &&
            meters is int metersVal &&
            metersVal > 0)
        {
            var showResults = await promotionService.FindShowsByDistanceAndDateRange(
                GeographicLocation(latitudeVal, longitudeVal),
                metersVal,
                startVal,
                endVal
            );
            List<ShowModel> showModels = showResults
                .Select(showResult => new ShowModel
                {
                    ActName = showResult.ActName,
                    VenueName = showResult.VenueName,
                    VenueAddress = showResult.VenueAddress,
                    Date = showResult.Date,

                    HrefShow = Url.Action(nameof(UpdateShow), new { showGuid = showResult.ShowGuid })!
                })
                .ToList();
            return Ok(showModels);
        }
        else
        {
            return BadRequest();
        }
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
