﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addgiatrimacdinhtuychonchitietmhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiaTriMacDinh",
                table: "MauHoaDonTuyChinhChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaTriMacDinh",
                table: "MauHoaDonTuyChinhChiTiets");
        }
    }
}
