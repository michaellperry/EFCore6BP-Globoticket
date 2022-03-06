using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloboTicket.Infrastructure.Migrations
{
    public partial class CreateAppRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE ROLE globoticket_app AUTHORIZATION db_securityadmin");
            migrationBuilder.Sql("ALTER ROLE db_datareader ADD MEMBER globoticket_app");
            migrationBuilder.Sql("ALTER ROLE db_datawriter ADD MEMBER globoticket_app");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER ROLE db_datawriter DROP MEMBER globoticket_app");
            migrationBuilder.Sql("ALTER ROLE db_datareader DROP MEMBER globoticket_app");
            migrationBuilder.Sql("DROP ROLE globoticket_app");
        }
    }
}
