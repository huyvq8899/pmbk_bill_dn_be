using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addbokyhieuhoadonchohoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoKyHieuHoaDonId",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_BoKyHieuHoaDonId",
                table: "HoaDonDienTus",
                column: "BoKyHieuHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                table: "HoaDonDienTus",
                column: "BoKyHieuHoaDonId",
                principalTable: "BoKyHieuHoaDons",
                principalColumn: "BoKyHieuHoaDonId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_BoKyHieuHoaDons_BoKyHieuHoaDonId",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_BoKyHieuHoaDonId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "BoKyHieuHoaDonId",
                table: "HoaDonDienTus");
        }
    }
}
