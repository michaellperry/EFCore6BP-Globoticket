using FluentAssertions;
using GloboTicket.Domain.Entities;
using GloboTicket.Domain.Services;
using GloboTicket.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
        Venue venue = await GivenVenue();
        Act act = await GivenAct();
        DateTimeOffset date = DateTimeOffset.Parse("2022-07-27Z");

        Show show = await WhenBookShow(venue, act, date);

        show.Venue.Name.Should().Be(venue.Name);
        show.Act.Name.Should().Be(act.Name);
    }

    private async Task<Show> WhenBookShow(Venue venue, Act act, DateTimeOffset date)
    {
        Guid showGuid = Guid.NewGuid();
        return await promotionService.BookShow(showGuid, venue, act, date);
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
        string address = "100 Test Street, Testertown, TS 99999"
    )
    {
        Guid venueGuid = Guid.NewGuid();
        return await promotionService.CreateVenue(venueGuid, name, address);
    }
}