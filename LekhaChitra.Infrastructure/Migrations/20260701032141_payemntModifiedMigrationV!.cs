using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LekhaChitra.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class payemntModifiedMigrationV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AddedDate",
                table: "Transactions",
                column: "AddedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AddedDate",
                table: "Payments",
                column: "AddedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_AddedDate",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_AddedDate",
                table: "Payments");
        }
    }
}
