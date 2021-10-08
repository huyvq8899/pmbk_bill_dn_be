using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addthongtutobbxb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThongTu",
                table: "BienBanXoaBos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThongTu",
                table: "BienBanXoaBos");
        }
    }
}
