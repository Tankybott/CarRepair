using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWebsiteConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsiteConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortalHomeTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortalHomeText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MondayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    MondayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    TuesdayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    TuesdayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    WednesdayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    WednesdayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    ThursdayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    ThursdayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    FridayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    FridayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    SaturdayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    SaturdayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    SundayOpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    SundayCloseTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteConfigs", x => x.Id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteConfigs");
        }
    }
}
