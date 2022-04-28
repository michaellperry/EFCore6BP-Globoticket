using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloboTicket.Infrastructure.Migrations
{
    public partial class AddTicketSalesAndSeatingCapacity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Venue",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SeatingCapacity",
                table: "Venue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TicketSale",
                columns: table => new
                {
                    TicketSaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketSaleGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSale", x => x.TicketSaleId);
                    table.ForeignKey(
                        name: "FK_TicketSale_Show_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Show",
                        principalColumn: "ShowId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketSale_ShowId",
                table: "TicketSale",
                column: "ShowId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketSale");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Venue");

            migrationBuilder.DropColumn(
                name: "SeatingCapacity",
                table: "Venue");
        }
    }
}
