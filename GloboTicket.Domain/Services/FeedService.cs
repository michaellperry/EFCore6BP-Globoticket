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

    public IAsyncEnumerable<ShowInfo> ListShows()
    {
        var shows = context.Set<Show>()
            .Select(show => new ShowInfo
            {
                ShowGuid = show.ShowGuid,
                ActGuid = show.Act.ActGuid,
                VenueGuid = show.Venue.VenueGuid,
                Date = show.Date
            })
            .AsAsyncEnumerable();
        return shows;
    }
}
