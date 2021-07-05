using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addrangechophepphathanhhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChoPhepPhatHanhMax",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChoPhepPhatHanhMin",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoPhepPhatHanhMax",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "ChoPhepPhatHanhMin",
                table: "MauHoaDons");
        }
    }
}
