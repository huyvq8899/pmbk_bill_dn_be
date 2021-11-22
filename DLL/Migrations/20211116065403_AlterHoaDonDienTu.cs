using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AlterHoaDonDienTu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotCreateBienBan",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotCreateBienBan",
                table: "HoaDonDienTus");
        }
    }
}
