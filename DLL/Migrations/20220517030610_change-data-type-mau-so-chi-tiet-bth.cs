using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changedatatypemausochitietbth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MauSo",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MauSo",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(int),
                oldMaxLength: 1);
        }
    }
}
