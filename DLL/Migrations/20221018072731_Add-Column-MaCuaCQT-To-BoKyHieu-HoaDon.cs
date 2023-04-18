using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddColumnMaCuaCQTToBoKyHieuHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaCuaCQTToKhaiChapNhan",
                table: "BoKyHieuHoaDons",
                maxLength: 23,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaCuaCQTToKhaiChapNhan",
                table: "BoKyHieuHoaDons");
        }
    }
}
