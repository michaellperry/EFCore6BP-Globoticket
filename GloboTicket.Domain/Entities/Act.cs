namespace GloboTicket.Domain.Entities;

public class Act
{
    public int ActId { get; set; }

    public Guid ActGuid { get; set; }
    public string Name { get; set; } = "";
}