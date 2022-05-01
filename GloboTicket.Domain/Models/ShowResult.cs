using NetTopologySuite.Geometries;

namespace GloboTicket.Domain.Models;

public class ShowResult
{
    public string HrefShow { get; set; } = "";
    public string VenueName { get; set; } = "";
    public string? VenueAddress { get; set; }
    public Point? VenueLocation { get; set; }
    public double? Distance { get; set; }
    public string ActName { get; set; } = "";
    public DateTimeOffset Date { get; set; }
    public int SeatsAvailable { get; set; }
}