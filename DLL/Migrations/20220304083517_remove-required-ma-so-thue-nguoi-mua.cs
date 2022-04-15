using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class removerequiredmasothuenguoimua : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 14,
                oldNullable: true);
        }
    }
}
