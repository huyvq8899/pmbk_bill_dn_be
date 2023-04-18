using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changeondeletenhatkythaotac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyThaoTacHoaDons_HoaDonDienTus_HoaDonDienTuId",
                table: "NhatKyThaoTacHoaDons");

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyThaoTacHoaDons_HoaDonDienTus_HoaDonDienTuId",
                table: "NhatKyThaoTacHoaDons",
                column: "HoaDonDienTuId",
                principalTable: "HoaDonDienTus",
                principalColumn: "HoaDonDienTuId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhatKyThaoTacHoaDons_HoaDonDienTus_HoaDonDienTuId",
                table: "NhatKyThaoTacHoaDons");

            migrationBuilder.AddForeignKey(
                name: "FK_NhatKyThaoTacHoaDons_HoaDonDienTus_HoaDonDienTuId",
                table: "NhatKyThaoTacHoaDons",
                column: "HoaDonDienTuId",
                principalTable: "HoaDonDienTus",
                principalColumn: "HoaDonDienTuId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
