using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addlinkxmltohddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "XMLChuaKy",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XMLDaKy",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XMLChuaKy",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "XMLDaKy",
                table: "HoaDonDienTus");
        }
    }
}
