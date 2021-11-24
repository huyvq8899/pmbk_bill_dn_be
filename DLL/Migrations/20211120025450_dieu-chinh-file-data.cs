using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhfiledata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenFileChietKhau",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenFileChuyenDoi",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenFileNgoaiTe",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenFileTheHien",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FileDatas",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSigned",
                table: "FileDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenFileChietKhau",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "TenFileChuyenDoi",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "TenFileNgoaiTe",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "TenFileTheHien",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FileDatas");

            migrationBuilder.DropColumn(
                name: "IsSigned",
                table: "FileDatas");
        }
    }
}
