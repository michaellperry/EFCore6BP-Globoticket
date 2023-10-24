﻿namespace GloboTicket.Domain.Entities;

public class Show
{
    public int ShowId { get; set; }

    public Guid ShowGuid { get; set; }
    public Venue Venue { get; set; }
    public Act Act { get; set; }
    public DateTimeOffset Date { get; set; }
}