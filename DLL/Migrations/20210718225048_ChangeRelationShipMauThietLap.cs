using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeRelationShipMauThietLap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropIndex(
                name: "IX_MauHoaDonThietLapMacDinhs_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDonThietLapMacDinhs_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.DropIndex(
                name: "IX_MauHoaDonThietLapMacDinhs_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs");

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDonThietLapMacDinhs_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                unique: true,
                filter: "[MauHoaDonId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
