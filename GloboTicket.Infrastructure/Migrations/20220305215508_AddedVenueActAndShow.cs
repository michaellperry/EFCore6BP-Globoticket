using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GloboTicket.Infrastructure.Migrations
{
    public partial class AddedVenueActAndShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Act",
                columns: table => new
                {
                    ActId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Act", x => x.ActId);
                    table.UniqueConstraint("AK_Act_ActGuid", x => x.ActGuid);
                });

            migrationBuilder.CreateTable(
                name: "Venue",
                columns: table => new
                {
                    VenueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venue", x => x.VenueId);
                    table.UniqueConstraint("AK_Venue_VenueGuid", x => x.VenueGuid);
                });

            migrationBuilder.CreateTable(
                name: "Show",
                columns: table => new
                {
                    ShowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    ActId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Show", x => x.ShowId);
                    table.UniqueConstraint("AK_Show_ShowGuid", x => x.ShowGuid);
                    table.ForeignKey(
                        name: "FK_Show_Act_ActId",
                        column: x => x.ActId,
                        principalTable: "Act",
                        principalColumn: "ActId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Show_Venue_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venue",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Show_ActId",
                table: "Show",
                column: "ActId");

            migrationBuilder.CreateIndex(
                name: "IX_Show_VenueId",
                table: "Show",
                column: "VenueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Show");

            migrationBuilder.DropTable(
                name: "Act");

            migrationBuilder.DropTable(
                name: "Venue");
        }
    }
}
