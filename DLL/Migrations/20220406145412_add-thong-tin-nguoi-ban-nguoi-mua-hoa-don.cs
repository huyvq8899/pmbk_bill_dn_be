using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthongtinnguoibannguoimuahoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTheHienLyDoTrenHoaDon",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsThongTinNguoiBanHoacNguoiMua",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTheHienLyDoTrenHoaDon",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "IsThongTinNguoiBanHoacNguoiMua",
                table: "HoaDonDienTus");
        }
    }
}
