using GloboTicket.Domain;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Infrastructure.Configuration;

internal class SqlGloboTicketContext : GloboTicketContext
{
    public SqlGloboTicketContext(DbContextOptions options) : base(options, new SqlModelConfiguration())
    {
    }
}
