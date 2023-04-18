using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class TestDelFKMauHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoKyHieuHoaDons_MauHoaDons_MauHoaDonId",
                table: "BoKyHieuHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_BoKyHieuHoaDons_MauHoaDonId",
                table: "BoKyHieuHoaDons");

            migrationBuilder.AlterColumn<string>(
                name: "MauHoaDonId",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MauHoaDonId",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BoKyHieuHoaDons_MauHoaDonId",
                table: "BoKyHieuHoaDons",
                column: "MauHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoKyHieuHoaDons_MauHoaDons_MauHoaDonId",
                table: "BoKyHieuHoaDons",
                column: "MauHoaDonId",
                principalTable: "MauHoaDons",
                principalColumn: "MauHoaDonId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
