using GloboTicket.API.Models;
using GloboTicket.Domain;
using GloboTicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.API.Queries;

public class ListShowsQuery
{
    private readonly GloboTicketContext context;

    public ListShowsQuery(GloboTicketContext context)
    {
        this.context = context;
    }

    public async Task<List<Show>> Execute()
    {
        var shows = await context.Set<Show>()
            .Include(show => show.Act)
            .Include(show => show.Venue)
            .TagWithCallSite()
            .ToListAsync();

        return shows;
    }
}
