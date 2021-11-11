using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_cot_saisot_rasoat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "XMLData",
                table: "ThongDiepGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "XMLData",
                table: "ThongBaoHoaDonRaSoats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XMLData",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "XMLData",
                table: "ThongBaoHoaDonRaSoats");
        }
    }
}
