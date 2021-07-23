using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthietlapmacdinhtotruongdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultSTT",
                table: "TruongDuLieus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DinhDangSo",
                table: "TruongDuLieus",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultSTT",
                table: "TruongDuLieus");

            migrationBuilder.DropColumn(
                name: "DinhDangSo",
                table: "TruongDuLieus");
        }
    }
}
