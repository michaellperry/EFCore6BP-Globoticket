using GloboTicket.API.Commands;
using GloboTicket.API.Models;
using GloboTicket.Domain.Models;
using GloboTicket.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShowsController : ControllerBase
{
    private readonly PromotionService promotionService;

    public ShowsController(PromotionService promotionService)
    {
        this.promotionService = promotionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowResult>>> GetShows(
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
                endVal,
                showGuid => Url.Action(nameof(UpdateShow), new { showGuid })!
            );
            return Ok(showResults);
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
            await promotionService.RescheduleShow(showGuid, date);
        }
        return Ok();
    }
}
