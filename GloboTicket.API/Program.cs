using GloboTicket.Infrastructure;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add configuration sources
if (builder.Environment.IsDevelopment())
{
    builder.Configuration
        .AddUserSecrets<Program>();
    IdentityModelEventSource.ShowPII = true;
}

// Add services to the container.
string connectionString = builder.Configuration
    .GetConnectionString("GloboTicketConnection")
    ?? throw new InvalidOperationException("Please provide a value for connection string GloboTicketConnection.");

builder.Services.AddInfrastructure(connectionString, builder.Environment.IsDevelopment());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
