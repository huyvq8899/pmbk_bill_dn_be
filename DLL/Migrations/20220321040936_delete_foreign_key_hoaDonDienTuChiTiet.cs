using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class delete_foreign_key_hoaDonDienTuChiTiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_DonViTinhs_DonViTinhId",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_DonViTinhId",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.AlterColumn<string>(
                name: "DonViTinhId",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DonViTinhId",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_DonViTinhId",
                table: "HoaDonDienTuChiTiets",
                column: "DonViTinhId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_DonViTinhs_DonViTinhId",
                table: "HoaDonDienTuChiTiets",
                column: "DonViTinhId",
                principalTable: "DonViTinhs",
                principalColumn: "DonViTinhId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
