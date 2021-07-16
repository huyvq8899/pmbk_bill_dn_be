using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddBoSungMauHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocHoaDonMauCoBan",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocHoaDonMauCoChietKhau",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocHoaDonMauDangChuyenDoi",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocHoaDonMauNgoaiTe",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDaKy",
                table: "MauHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocHoaDonMauCoBan",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "DocHoaDonMauCoChietKhau",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "DocHoaDonMauDangChuyenDoi",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "DocHoaDonMauNgoaiTe",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "IsDaKy",
                table: "MauHoaDons");
        }
    }
}
