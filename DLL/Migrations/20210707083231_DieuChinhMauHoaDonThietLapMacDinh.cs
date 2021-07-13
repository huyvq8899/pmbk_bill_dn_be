using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class DieuChinhMauHoaDonThietLapMacDinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ma",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.AddColumn<int>(
                name: "Loai",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loai",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.AddColumn<string>(
                name: "Ma",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: true);
        }
    }
}
