namespace GloboTicket.Domain.Models;

public class ShowInfo
{
    public Guid ShowGuid { get; init; }
    public Guid ActGuid { get; init; }
    public Guid VenueGuid { get; init; }
    public DateTimeOffset Date { get; init; }
}