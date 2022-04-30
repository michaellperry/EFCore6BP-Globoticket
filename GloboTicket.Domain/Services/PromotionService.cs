using GloboTicket.Domain.Entities;
using GloboTicket.Domain.Models;
using NetTopologySuite.Geometries;

namespace GloboTicket.Domain.Services;

public class PromotionService
{
    private GloboTicketContext context;

    public PromotionService(GloboTicketContext context)
    {
        this.context = context;
    }

    public async Task<Show> BookShow(Guid showGuid, Guid venueGuid, Guid actGuid, DateTimeOffset date)
    {
        var venue = context.Set<Venue>().SingleOrDefault(v => v.VenueGuid == venueGuid);
        if (venue is null)
        {
            throw new ArgumentException($"No venue found for guid {venueGuid}");
        }

        var act = context.Set<Act>().SingleOrDefault(a => a.ActGuid == actGuid);
        if (act is null)
        {
            throw new ArgumentException($"No act found for guid {actGuid}");
        }

        var show = new Show(venue, act)
        {
            ShowGuid = showGuid,
            Date = date
        };

        await context.AddAsync(show);
        await context.SaveChangesAsync();

        return show;
    }

    public async Task<Venue> CreateVenue(Guid venueGuid, string name, string address, Point? location)
    {
        var venue = new Venue
        {
            VenueGuid = venueGuid,
            Name = name,
            Address = address,
            Location = location
        };

        await context.AddAsync(venue);
        await context.SaveChangesAsync();

        return venue;
    }

    public async Task<Act> CreateAct(Guid actGuid, string name)
    {
        var act = new Act
        {
            ActGuid = actGuid,
            Name = name
        };

        await context.AddAsync(act);
        await context.SaveChangesAsync();

        return act;
    }

    public Task<List<ShowResult>> FindShowsByDistanceAndDateRange(Point search, int miles, DateTimeOffset start, DateTimeOffset end)
    {
        throw new NotImplementedException();
    }
}