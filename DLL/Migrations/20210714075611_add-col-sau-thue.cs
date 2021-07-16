using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcolsauthue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DonGiaSauThue",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThanhTienSauThue",
                table: "HoaDonDienTuChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonGiaSauThue",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "ThanhTienSauThue",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
