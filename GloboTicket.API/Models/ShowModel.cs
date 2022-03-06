namespace GloboTicket.API.Models
{
    public class ShowModel
    {
        public string ActName { get; set; } = "";
        public string VenueName { get; set; } = "";
        public string VenueAddress { get; set; } = "";
        public DateTimeOffset Date { get; set; }
    }
}
