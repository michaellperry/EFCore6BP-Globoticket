namespace GloboTicket.API.Models;

public class ShowModel
{
    public string HrefShow { get; init; } = "";
    public string HrefAct { get; init; } = "";
    public string HrefVenue { get; init; } = "";
    public DateTimeOffset? Date { get; init; }
}
