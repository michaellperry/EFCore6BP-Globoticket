using FluentAssertions;
using GloboTicket.Domain;
using GloboTicket.Domain.Entities;
using GloboTicket.Domain.Services;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.UnitTest;

public class PromotionServiceTest
{
    private readonly PromotionService promotionService;

    public PromotionServiceTest()
    {
        var options = new DbContextOptionsBuilder<GloboTicketContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new GloboTicketContext(options, new TestModelConfiguration());
        promotionService = new PromotionService(context);
    }

    [Fact]
    public async Task CanBookAShow()
    {
        Point location = new Point(-96.8124434, 32.7903953);
        Venue venue = await GivenVenue(location: location);
        Act act = await GivenAct();
        DateTimeOffset date = DateTimeOffset.Parse("2022-07-27Z");

        Show show = await WhenBookShow(venue.VenueGuid, act.ActGuid, date);

        show.Venue.Name.Should().Be(venue.Name);
        show.Act.Name.Should().Be(act.Name);
        show.Venue.Location.Should().Be(location);
    }

    private async Task<Show> WhenBookShow(Guid venueGuid, Guid actGuid, DateTimeOffset date)
    {
        Guid showGuid = Guid.NewGuid();
        return await promotionService.BookShow(showGuid, venueGuid, actGuid, date);
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
}