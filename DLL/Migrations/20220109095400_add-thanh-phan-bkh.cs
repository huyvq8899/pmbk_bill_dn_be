using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthanhphanbkh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KyHieu1",
                table: "DangKyUyNhiems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu23",
                table: "DangKyUyNhiems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu4",
                table: "DangKyUyNhiems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu56",
                table: "DangKyUyNhiems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KyHieu1",
                table: "DangKyUyNhiems");

            migrationBuilder.DropColumn(
                name: "KyHieu23",
                table: "DangKyUyNhiems");

            migrationBuilder.DropColumn(
                name: "KyHieu4",
                table: "DangKyUyNhiems");

            migrationBuilder.DropColumn(
                name: "KyHieu56",
                table: "DangKyUyNhiems");
        }
    }
}
