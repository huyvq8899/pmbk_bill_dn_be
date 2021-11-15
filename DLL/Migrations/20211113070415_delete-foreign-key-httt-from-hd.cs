using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class deleteforeignkeyhtttfromhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_HinhThucThanhToans_HinhThucThanhToanId",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_HinhThucThanhToanId",
                table: "HoaDonDienTus");

            migrationBuilder.AlterColumn<string>(
                name: "HinhThucThanhToanId",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HinhThucThanhToanId",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_HinhThucThanhToanId",
                table: "HoaDonDienTus",
                column: "HinhThucThanhToanId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_HinhThucThanhToans_HinhThucThanhToanId",
                table: "HoaDonDienTus",
                column: "HinhThucThanhToanId",
                principalTable: "HinhThucThanhToans",
                principalColumn: "HinhThucThanhToanId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
