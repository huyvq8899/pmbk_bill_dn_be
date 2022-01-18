using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class deletekhoangoaibbdc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BienBanDieuChinhs_HoaDonDienTus_HoaDonDieuChinhId",
                table: "BienBanDieuChinhs");

            migrationBuilder.DropIndex(
                name: "IX_BienBanDieuChinhs_HoaDonDieuChinhId",
                table: "BienBanDieuChinhs");

            migrationBuilder.AlterColumn<string>(
                name: "HoaDonDieuChinhId",
                table: "BienBanDieuChinhs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HoaDonDieuChinhId",
                table: "BienBanDieuChinhs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BienBanDieuChinhs_HoaDonDieuChinhId",
                table: "BienBanDieuChinhs",
                column: "HoaDonDieuChinhId");

            migrationBuilder.AddForeignKey(
                name: "FK_BienBanDieuChinhs_HoaDonDienTus_HoaDonDieuChinhId",
                table: "BienBanDieuChinhs",
                column: "HoaDonDieuChinhId",
                principalTable: "HoaDonDienTus",
                principalColumn: "HoaDonDienTuId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
