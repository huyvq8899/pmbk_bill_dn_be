using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changeforeignkeypqmhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhanQuyenMauHoaDons_MauHoaDons_MauHoaDonId",
                table: "PhanQuyenMauHoaDons");

            migrationBuilder.RenameColumn(
                name: "MauHoaDonId",
                table: "PhanQuyenMauHoaDons",
                newName: "BoKyHieuHoaDonId");

            migrationBuilder.RenameIndex(
                name: "IX_PhanQuyenMauHoaDons_MauHoaDonId",
                table: "PhanQuyenMauHoaDons",
                newName: "IX_PhanQuyenMauHoaDons_BoKyHieuHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhanQuyenMauHoaDons_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                table: "PhanQuyenMauHoaDons",
                column: "BoKyHieuHoaDonId",
                principalTable: "BoKyHieuHoaDons",
                principalColumn: "BoKyHieuHoaDonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhanQuyenMauHoaDons_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                table: "PhanQuyenMauHoaDons");

            migrationBuilder.RenameColumn(
                name: "BoKyHieuHoaDonId",
                table: "PhanQuyenMauHoaDons",
                newName: "MauHoaDonId");

            migrationBuilder.RenameIndex(
                name: "IX_PhanQuyenMauHoaDons_BoKyHieuHoaDonId",
                table: "PhanQuyenMauHoaDons",
                newName: "IX_PhanQuyenMauHoaDons_MauHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhanQuyenMauHoaDons_MauHoaDons_MauHoaDonId",
                table: "PhanQuyenMauHoaDons",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
