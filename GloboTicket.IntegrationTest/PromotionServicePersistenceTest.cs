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
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.IntegrationTest;

public class PromotionServicePersistenceTest
{
    private const string AppConnectionString = "GLOBOTICKET_APP_CONNECTION_STRING";

    private PromotionService promotionService;

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
            serviceCollection.AddInfrastructure(connectionString);
        });
        var host = builder.Build();

        promotionService = host.Services.GetRequiredService<PromotionService>();
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
    public async Task CanFindVenueByDistance()
    {
        Venue aac = await GivenVenue(
            name: "American Airlines Center",
            location: GeographicLocation(32.7903953, -96.8124434));
        Venue sr = await GivenVenue(
            name: "The State Room",
            location: GeographicLocation(40.7552824, -111.8906558));
        Act act = await GivenAct();
        DateTimeOffset date = DateTimeOffset.Parse("2022-07-27Z");
        await GivenShow(aac.VenueGuid, act.ActGuid, date);
        await GivenShow(sr.VenueGuid, act.ActGuid, date);

        Point search = GeographicLocation(33.0782868, -96.8104113);
        var venues = await WhenFindShowsByDistanceAndDateRange(search, 50, date, date.AddDays(1));
    }

    private async Task<Show> WhenBookShow(Guid venueGuid, Guid actGuid, DateTimeOffset date)
    {
        Guid showGuid = Guid.NewGuid();
        return await promotionService.BookShow(showGuid, venueGuid, actGuid, date);
    }

    private async Task<List<ShowResult>> WhenFindShowsByDistanceAndDateRange(Point search, int miles, DateTimeOffset start, DateTimeOffset end)
    {
        return await promotionService.FindShowsByDistanceAndDateRange(search, miles, start, end);
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
        Point? location = null
    )
    {
        Guid venueGuid = Guid.NewGuid();
        return await promotionService.CreateVenue(venueGuid, name, address, location);
    }

    private async Task<Show> GivenShow(Guid venueGuid, Guid actGuid, DateTimeOffset date)
    {
        Guid showGuid = Guid.NewGuid();
        return await promotionService.BookShow(showGuid, venueGuid, actGuid, date);
    }
}