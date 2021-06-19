using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class RenameColInHHVT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiaChi",
                table: "HangHoaDichVus",
                newName: "MoTa");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoTa",
                table: "HangHoaDichVus",
                newName: "DiaChi");
        }
    }
}
