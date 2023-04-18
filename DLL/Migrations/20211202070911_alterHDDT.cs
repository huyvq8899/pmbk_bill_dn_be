using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class alterHDDT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotCreateThayThe",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotCreateThayThe",
                table: "HoaDonDienTus");
            migrationBuilder.DropColumn(
                name: "IsNotCreateBienBan",
                table: "HoaDonDienTus");
        }

    }
}
