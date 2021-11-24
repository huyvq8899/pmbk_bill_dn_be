using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class bosungcolthongdiepchung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaNoiGui",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaNoiNhan",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiepThamChieu",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhienBan",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoLuong",
                table: "ThongDiepChungs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaNoiGui",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "MaNoiNhan",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "MaThongDiepThamChieu",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "PhienBan",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "ThongDiepChungs");
        }
    }
}
