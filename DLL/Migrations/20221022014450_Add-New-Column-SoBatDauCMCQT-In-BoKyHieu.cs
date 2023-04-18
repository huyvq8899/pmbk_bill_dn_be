using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddNewColumnSoBatDauCMCQTInBoKyHieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SoBatDauCMCQT",
                table: "BoKyHieuHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoBatDauCMCQT",
                table: "BoKyHieuHoaDons");
        }
    }
}
