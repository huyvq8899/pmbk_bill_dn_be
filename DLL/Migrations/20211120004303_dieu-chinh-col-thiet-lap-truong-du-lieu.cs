using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhcolthietlaptruongdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietLapTruongDuLieus_MauHoaDons_MauHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.DropIndex(
                name: "IX_ThietLapTruongDuLieus_MauHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.RenameColumn(
                name: "MauHoaDonId",
                table: "ThietLapTruongDuLieus",
                newName: "GiaTri");

            migrationBuilder.AlterColumn<string>(
                name: "GiaTri",
                table: "ThietLapTruongDuLieus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BoKyHieuHoaDonId",
                table: "ThietLapTruongDuLieus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThietLapTruongDuLieus_BoKyHieuHoaDonId",
                table: "ThietLapTruongDuLieus",
                column: "BoKyHieuHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThietLapTruongDuLieus_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                table: "ThietLapTruongDuLieus",
                column: "BoKyHieuHoaDonId",
                principalTable: "BoKyHieuHoaDons",
                principalColumn: "BoKyHieuHoaDonId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThietLapTruongDuLieus_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.DropIndex(
                name: "IX_ThietLapTruongDuLieus_BoKyHieuHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.DropColumn(
                name: "BoKyHieuHoaDonId",
                table: "ThietLapTruongDuLieus");

            migrationBuilder.RenameColumn(
                name: "GiaTri",
                table: "ThietLapTruongDuLieus",
                newName: "MauHoaDonId");

            migrationBuilder.AlterColumn<string>(
                name: "MauHoaDonId",
                table: "ThietLapTruongDuLieus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
