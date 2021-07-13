using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class DieuChinhColThietLapMacDinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropColumn(
                name: "Left",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropColumn(
                name: "Opacity",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropColumn(
                name: "Top",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "MauHoaDonThietLapMacDinhs",
                newName: "GiaTriBoSung");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GiaTriBoSung",
                table: "MauHoaDonThietLapMacDinhs",
                newName: "Width");

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Left",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Opacity",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Top",
                table: "MauHoaDonThietLapMacDinhs",
                nullable: true);
        }
    }
}
