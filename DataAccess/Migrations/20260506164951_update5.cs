using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairImages");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Repairs",
                newName: "FinalPrice");

            migrationBuilder.AddColumn<bool>(
                name: "IsManagerComment",
                table: "RepairComments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManagerComment",
                table: "RepairComments");

            migrationBuilder.RenameColumn(
                name: "FinalPrice",
                table: "Repairs",
                newName: "Price");

            migrationBuilder.CreateTable(
                name: "RepairImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairId = table.Column<int>(type: "int", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairImages_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairImages_RepairId",
                table: "RepairImages",
                column: "RepairId");
        }
    }
}
