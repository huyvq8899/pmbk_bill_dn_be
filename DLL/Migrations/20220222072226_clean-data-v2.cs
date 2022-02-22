using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class cleandatav2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaThongDiep",
                table: "ThongDiepChungs",
                maxLength: 46,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 46);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepChungs",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 14);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaThongDiep",
                table: "ThongDiepChungs",
                maxLength: 46,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 46,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSoThue",
                table: "ThongDiepChungs",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 14,
                oldNullable: true);
        }
    }
}
