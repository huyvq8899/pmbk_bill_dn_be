using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcotmauhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HinhThucHoaDon",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UyNhiemLapHoaDon",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhThucHoaDon",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "UyNhiemLapHoaDon",
                table: "MauHoaDons");
        }
    }
}
