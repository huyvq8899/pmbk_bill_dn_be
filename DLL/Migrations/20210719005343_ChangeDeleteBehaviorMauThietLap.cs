using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeDeleteBehaviorMauThietLap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.AddForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.AddForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
