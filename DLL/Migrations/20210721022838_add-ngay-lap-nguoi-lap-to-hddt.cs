using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addngaylapnguoilaptohddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileChuaKy",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLChuaKy",
                table: "BienBanXoaBos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLDaKy",
                table: "BienBanXoaBos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileChuaKy",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "XMLChuaKy",
                table: "BienBanXoaBos");

            migrationBuilder.DropColumn(
                name: "XMLDaKy",
                table: "BienBanXoaBos");
        }
    }
}
