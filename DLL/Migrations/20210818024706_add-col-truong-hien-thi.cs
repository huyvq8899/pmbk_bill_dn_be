using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoltruonghienthi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenTruongHienThi",
                table: "TruongDuLieuMoRongs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenTruongHienThi",
                table: "TruongDuLieuMoRongs");
        }
    }
}
