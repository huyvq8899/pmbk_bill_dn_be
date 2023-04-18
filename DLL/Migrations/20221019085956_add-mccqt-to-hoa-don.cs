using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addmccqttohoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MCCQT",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThongDiepChungId",
                table: "DuLieuGuiHDDTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MCCQT",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "ThongDiepChungId",
                table: "DuLieuGuiHDDTs");
        }
    }
}
