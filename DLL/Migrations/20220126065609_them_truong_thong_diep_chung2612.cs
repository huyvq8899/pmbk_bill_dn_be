using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_thong_diep_chung2612 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdTDiepTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTDiepTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "NgayTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "SoTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs");
        }
    }
}
