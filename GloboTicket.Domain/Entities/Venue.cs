using NetTopologySuite.Geometries;

namespace GloboTicket.Domain.Entities;

public class Venue
{
    public int VenueId { get; set; }

    public Guid VenueGuid { get; set; }
    public string Name { get; set; } = "";
    public string? Address { get; set; }

    public Point? Location { get; set; }
}