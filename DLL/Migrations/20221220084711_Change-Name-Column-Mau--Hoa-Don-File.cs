using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeNameColumnMauHoaDonFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaSoThue",
                table: "MauHoaDonFiles",
                newName: "MaSoThueGiaiPhap");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaSoThueGiaiPhap",
                table: "MauHoaDonFiles",
                newName: "MaSoThue");
        }
    }
}
