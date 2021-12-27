using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_phanloaihdsaisot_macdinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PhanLoaiHDSaiSotMacDinh",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhanLoaiHDSaiSotMacDinh",
                table: "ThongDiepChiTietGuiCQTs");
        }
    }
}
