using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class RemoveColsInDoiTuong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "DoiTuongs");

            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "DoiTuongs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DoiTuongs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "DoiTuongs",
                nullable: true);
        }
    }
}
