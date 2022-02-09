using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_hoadonsaisot_ghichu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChuThayTheSaiSot",
                table: "HoaDonDienTus",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdHoaDonSaiSotBiThayThe",
                table: "HoaDonDienTus",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChuThayTheSaiSot",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "IdHoaDonSaiSotBiThayThe",
                table: "HoaDonDienTus");
        }
    }
}
