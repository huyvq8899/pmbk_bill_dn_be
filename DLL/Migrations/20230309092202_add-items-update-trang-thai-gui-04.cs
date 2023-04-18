using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class additemsupdatetrangthaigui04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChungTuLienQuan",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUpdated04",
                table: "HoaDonDienTus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PhanLoaiSaiSot04",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChungTuLienQuan",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "IsUpdated04",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "PhanLoaiSaiSot04",
                table: "HoaDonDienTus");
        }
    }
}
