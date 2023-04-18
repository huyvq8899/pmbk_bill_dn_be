using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addisnewthongdiepsaisot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "ThongDiepGuiCQTs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "HinhThucHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "HinhThucHoaDon",
                table: "ThongDiepChiTietGuiCQTs");
        }
    }
}
