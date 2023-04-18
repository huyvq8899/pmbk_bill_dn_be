using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addthoihangui : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiHanGui",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true,
                defaultValue: null);
            migrationBuilder.AddColumn<string>(
                name: "STBao",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true,
                defaultValue: null);
            migrationBuilder.AddColumn<int>(
                name: "STT",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
