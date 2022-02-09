using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtylephantramdoanhthu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGiamTheoNghiQuyet",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TyLePhanTramDoanhThu",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGiamTheoNghiQuyet",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TyLePhanTramDoanhThu",
                table: "HoaDonDienTus");
        }
    }
}
