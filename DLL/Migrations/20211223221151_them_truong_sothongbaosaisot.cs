using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Them_truong_sothongbaosaisot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoThongBaoSaiSot",
                table: "ThongDiepGuiCQTs",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThongDiepGuiCQTId",
                table: "HoaDonDienTus",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoThongBaoSaiSot",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "ThongDiepGuiCQTId",
                table: "HoaDonDienTus");
        }
    }
}
