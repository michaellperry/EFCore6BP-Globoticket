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

    public async Task<List<ShowModel>> Execute()
    {
        return await context.Set<Show>()
            .Select(show => new ShowModel
            {
                ActName = show.Act.Name,
                VenueName = show.Venue.Name,
                VenueAddress = show.Venue.Address,
                Date = show.Date
            })
            .ToListAsync();
    }
}
