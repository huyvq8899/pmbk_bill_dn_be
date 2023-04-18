using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class themdiachivaobbxb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "BienBanXoaBos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "BienBanXoaBos");
        }
    }
}
