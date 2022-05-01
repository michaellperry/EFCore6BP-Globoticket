using System;
using GloboTicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Domain.Services;

public class SalesService
{
    private readonly GloboTicketContext context;

    public SalesService(GloboTicketContext context)
    {
        this.context = context;
    }

    public async Task<TicketSale> SellTickets(Guid ticketSaleGuid, Guid showGuid, int quantity)
    {
        var show = await context.Set<Show>()
            .SingleOrDefaultAsync(s => s.ShowGuid == showGuid);
        if (show is null)
        {
            throw new ArgumentException($"Show {showGuid} not found");
        }

        var ticketSale = new TicketSale(show)
        {
            TicketSaleGuid = ticketSaleGuid,
            Quantity = quantity
        };
        context.Add(ticketSale);
        await context.SaveChangesAsync();

        return ticketSale;
    }
}
