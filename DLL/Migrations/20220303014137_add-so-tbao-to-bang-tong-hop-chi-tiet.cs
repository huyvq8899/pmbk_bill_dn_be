using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addsotbaotobangtonghopchitiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "STBao",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AlterColumn<bool>(
                name: "HDDIn",
                table: "BangTongHopDuLieuHoaDons",
                oldClrType: typeof(int)

                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.DropColumn(
                name: "STBao",
                table: "BangTongHopDuLieuHoaDonChiTiets");
        }
    }
}
