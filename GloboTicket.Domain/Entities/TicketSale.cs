using System;

namespace GloboTicket.Domain.Entities;

public class TicketSale
{
    public int TicketSaleId { get; set; }
    public Guid TicketSaleGuid { get; set; }
    public Show Show { get; set; }
    public int ShowId { get; set; }
    public int Quantity { get; set; }

    public TicketSale(Show show)
    {
        Show = show;
    }

    public TicketSale() : this(null!)
    {
    }
}
