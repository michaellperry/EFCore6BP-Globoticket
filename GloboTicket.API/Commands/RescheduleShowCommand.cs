using GloboTicket.Domain;
using GloboTicket.Domain.Entities;

namespace GloboTicket.API.Commands;

public class RescheduleShowCommand
{
    private readonly GloboTicketContext context;

    public RescheduleShowCommand(GloboTicketContext context)
    {
        this.context = context;
    }

    public async Task Execute(Guid showGuid, DateTimeOffset date)
    {
        var show = context.Set<Show>().SingleOrDefault(s => s.ShowGuid == showGuid);
        if (show is null)
        {
            throw new ArgumentException($"No show found for guid {showGuid}");
        }

        show.Date = date;
        await context.SaveChangesAsync();
    }
}
