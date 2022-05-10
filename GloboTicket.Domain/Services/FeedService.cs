using GloboTicket.Domain.Entities;
using GloboTicket.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Domain.Services;

public class FeedService
{
    private readonly GloboTicketContext context;

    public FeedService(GloboTicketContext context)
    {
        this.context = context;
    }

    public async Task<List<ShowInfo>> ListShows()
    {
        var shows = await context.Set<Show>()
            .Select(show => new ShowInfo
            {
                ShowGuid = show.ShowGuid,
                ActGuid = show.Act.ActGuid,
                VenueGuid = show.Venue.VenueGuid,
                Date = show.Date
            })
            .ToListAsync();
        return shows;
    }
}
