using GloboTicket.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add configuration sources
if (builder.Environment.IsDevelopment())
{
    builder.Configuration
        .AddUserSecrets<Program>();
}

// Add services to the container.
string connectionString = builder.Configuration
    .GetConnectionString("GloboTicketConnection");
builder.Services.AddInfrastructure(connectionString);

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
