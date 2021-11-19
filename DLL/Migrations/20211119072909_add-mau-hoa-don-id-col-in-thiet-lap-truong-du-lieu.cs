using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addmauhoadonidcolinthietlaptruongdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MauHoaDonId",
                table: "ThietLapTruongDuLieus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThietLapTruongDuLieus_MauHoaDonId",
                table: "ThietLapTruongDuLieus",
                column: "MauHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThietLapTruongDuLieus_MauHoaDons_MauHoaDonId",
                table: "ThietLapTruongDuLieus",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietLapTruongDuLieus_MauHoaDons_MauHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.DropIndex(
                name: "IX_ThietLapTruongDuLieus_MauHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.DropColumn(
                name: "MauHoaDonId",
                table: "ThietLapTruongDuLieus");
        }
    }
}
