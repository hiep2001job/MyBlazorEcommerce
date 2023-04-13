using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEcommerce.Server.Migrations
{
    public partial class FeaturedProductSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Featured",
                value: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Featured",
                value: true);
        }
    }
}
