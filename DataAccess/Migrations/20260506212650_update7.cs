using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostEstimationItems_Repairs_OverheadRepairId",
                table: "CostEstimationItems");

            migrationBuilder.DropIndex(
                name: "IX_CostEstimationItems_OverheadRepairId",
                table: "CostEstimationItems");

            migrationBuilder.DropColumn(
                name: "OverheadRepairId",
                table: "CostEstimationItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OverheadRepairId",
                table: "CostEstimationItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostEstimationItems_OverheadRepairId",
                table: "CostEstimationItems",
                column: "OverheadRepairId");

            migrationBuilder.AddForeignKey(
                name: "FK_CostEstimationItems_Repairs_OverheadRepairId",
                table: "CostEstimationItems",
                column: "OverheadRepairId",
                principalTable: "Repairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
