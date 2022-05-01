using GloboTicket.Domain.Entities;
using GloboTicket.Domain.Models;
using NetTopologySuite.Geometries;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Venue> CreateVenue(Guid venueGuid, string name, string address, Point? location, int seatingCapacity)
    {
        var venue = new Venue
        {
            VenueGuid = venueGuid,
            Name = name,
            Address = address,
            Location = location,
            SeatingCapacity = seatingCapacity
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

    public async Task<List<ShowResult>> FindShowsByDistanceAndDateRange(Point search, int meters, DateTimeOffset start, DateTimeOffset end)
    {
        var shows = await context.Set<Show>()
            .Include(s => s.Venue)
            .Include(s => s.Act)
            .Include(s => s.TicketSales)
            .Where(s =>
                s.Date >= start && s.Date < end &&
                (s.Venue.Location == null || s.Venue.Location.IsWithinDistance(search, meters)))
            .OrderBy(s => s.Venue.Location!.Distance(search))
            .ToListAsync();
        var showResults = shows
            .Select(s => new ShowResult
            {
                ShowGuid = s.ShowGuid,
                VenueName = s.Venue.Name,
                VenueAddress = s.Venue.Address,
                VenueLocation = s.Venue.Location,
                Distance = s.Venue.Location?.Distance(search),
                ActName = s.Act.Name,
                Date = s.Date,
                SeatsAvailable = s.Venue.SeatingCapacity - s.TicketSales.Sum(ts => ts.Quantity)
            })
            .ToList();
        return showResults;
    }

    public async Task RescheduleShow(Guid showGuid, DateTimeOffset date)
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