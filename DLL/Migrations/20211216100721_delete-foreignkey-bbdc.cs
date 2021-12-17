using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class deleteforeignkeybbdc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BienBanDieuChinhs_HoaDonDienTus_HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs");

            migrationBuilder.DropIndex(
                name: "IX_BienBanDieuChinhs_HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs");

            migrationBuilder.AlterColumn<string>(
                name: "HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BienBanDieuChinhs_HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs",
                column: "HoaDonBiDieuChinhId");

            migrationBuilder.AddForeignKey(
                name: "FK_BienBanDieuChinhs_HoaDonDienTus_HoaDonBiDieuChinhId",
                table: "BienBanDieuChinhs",
                column: "HoaDonBiDieuChinhId",
                principalTable: "HoaDonDienTus",
                principalColumn: "HoaDonDienTuId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
