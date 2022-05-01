using FluentAssertions;
using GloboTicket.Domain.Entities;
using GloboTicket.Domain.Models;
using GloboTicket.Domain.Services;
using GloboTicket.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.IntegrationTest;

public class PromotionServicePersistenceTest
{
    private const string AppConnectionString = "GLOBOTICKET_APP_CONNECTION_STRING";

    private PromotionService promotionService;
    private SalesService salesService;

    public PromotionServicePersistenceTest()
    {
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureServices((context, serviceCollection) =>
        {
            var connectionString = Environment.GetEnvironmentVariable(AppConnectionString);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ApplicationException($"Please set the environment variable {AppConnectionString}");
            }
            serviceCollection.AddInfrastructure(connectionString, isDevelopment: true);
        });
        var host = builder.Build();

        promotionService = host.Services.GetRequiredService<PromotionService>();
        salesService = host.Services.GetRequiredService<SalesService>();
    }

    [Fact]
    public async Task CanPersistAShow()
    {
        Point location = GeographicLocation(32.7903953, -96.8124434);
        Venue venue = await GivenVenue(location: location);
        Act act = await GivenAct();
        DateTimeOffset date = DateTimeOffset.Parse("2022-07-27Z");

        Show show = await WhenBookShow(venue.VenueGuid, act.ActGuid, date);

        show.Venue.Name.Should().Be(venue.Name);
        show.Act.Name.Should().Be(act.Name);
        show.Venue.Location.Should().Be(location);
    }

    [Fact]
    public async Task CanFindShowByDistance()
    {
        Venue aac = await GivenVenue(
            name: "American Airlines Center",
            location: GeographicLocation(32.7903953, -96.8124434),
            seatingCapacity: 20_000);
        Venue sr = await GivenVenue(
            name: "The State Room",
            location: GeographicLocation(40.7552824, -111.8906558));
        Act act = await GivenAct();
        DateTimeOffset date = DateTimeOffset.Parse("2022-07-27Z");
        var show = await GivenShow(aac.VenueGuid, act.ActGuid, date);
        await GivenShow(sr.VenueGuid, act.ActGuid, date);
        await GivenTicketSale(show.ShowGuid, 3);

        Point search = GeographicLocation(33.0782868, -96.8104113);
        var shows = await WhenFindShowsByDistanceAndDateRange(search, 100_000, date, date.AddDays(1));
        shows.Count.Should().Be(1);
        shows[0].VenueName.Should().Be(aac.Name);
        shows[0].ActName.Should().Be(act.Name);
        shows[0].SeatsAvailable.Should().Be(aac.SeatingCapacity-3);
    }

    [Fact]
    public async Task GenerateTestData()
    {
        int venueCount = 1000;
        int actCount = 300;
        int ticketCount = 40;
        var random = new Random();

        var venues = new List<Venue>();
        for (var v = 0; v < venueCount; v++)
        {
            var venue = await GivenVenue(name: $"Venue{v}", location: GeographicLocation(
                random.NextDouble() * 20.0 + 30.0,
                random.NextDouble() * 40.0 - 120.0
            ));
            venues.Add(venue);
        }

        var acts = new List<Act>();
        for (var a = 0; a < actCount; a++)
        {
            var act = await GivenAct(name: $"Act{a}");
            acts.Add(act);
        }

        var pairs =
            from venue in venues
            from act in acts
            select (venue, act);

        var shows = new List<Show>();
        var today = DateTimeOffset.Now;
        foreach (var (venue, act) in pairs)
        {
            var show = await GivenShow(venue.VenueGuid, act.ActGuid, today.AddDays(random.Next(1, 365)));
            for (int t = 0; t < ticketCount; t++)
            {
                await GivenTicketSale(show.ShowGuid, random.Next(1, 4));
            }
        }
    }

    private async Task<Show> WhenBookShow(Guid venueGuid, Guid actGuid, DateTimeOffset date)
    {
        Guid showGuid = Guid.NewGuid();
        return await promotionService.BookShow(showGuid, venueGuid, actGuid, date);
    }

    private async Task<List<ShowResult>> WhenFindShowsByDistanceAndDateRange(Point search, int meters, DateTimeOffset start, DateTimeOffset end)
    {
        return await promotionService.FindShowsByDistanceAndDateRange(search, meters, start, end, showGuid => "");
    }

    private async Task<Act> GivenAct(
        string name = "The Testers"
    )
    {
        Guid actGuid = Guid.NewGuid();
        return await promotionService.CreateAct(actGuid, name);
    }

    private async Task<Venue> GivenVenue(
        string name = "Test Arena",
        string address = "100 Test Street, Testertown, TS 99999",
        Point? location = null,
        int seatingCapacity = 1000
    )
    {
        Guid venueGuid = Guid.NewGuid();
        return await promotionService.CreateVenue(venueGuid, name, address, location, seatingCapacity);
    }

    private async Task<Show> GivenShow(Guid venueGuid, Guid actGuid, DateTimeOffset date)
    {
        Guid showGuid = Guid.NewGuid();
        return await promotionService.BookShow(showGuid, venueGuid, actGuid, date);
    }

    private async Task<TicketSale> GivenTicketSale(Guid showGuid, int quantity)
    {
        Guid ticketSaleGuid = Guid.NewGuid();
        return await salesService.SellTickets(ticketSaleGuid, showGuid, quantity);
    }
}