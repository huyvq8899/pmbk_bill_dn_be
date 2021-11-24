using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhtypethuegtgt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThueGTGT",
                table: "HangHoaDichVus",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ThueGTGT",
                table: "HangHoaDichVus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
