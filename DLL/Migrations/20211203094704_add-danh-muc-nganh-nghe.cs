using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddanhmucnganhnghe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NganhNghe",
                table: "HoSoHDDTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NganhNghe",
                table: "HoSoHDDTs");
        }
    }
}
