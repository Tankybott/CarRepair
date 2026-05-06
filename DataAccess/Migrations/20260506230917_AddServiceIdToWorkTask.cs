using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceIdToWorkTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ServiceId",
                table: "Tasks",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Services_ServiceId",
                table: "Tasks",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Services_ServiceId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ServiceId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Tasks");
        }
    }
}
