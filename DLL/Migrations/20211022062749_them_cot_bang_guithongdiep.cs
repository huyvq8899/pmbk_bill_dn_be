using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_cot_bang_guithongdiep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FileDinhKem",
                table: "ThongDiepGuiCQTs",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DaKyGuiCQT",
                table: "ThongDiepGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileXMLDaKy",
                table: "ThongDiepGuiCQTs",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaKyGuiCQT",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "FileXMLDaKy",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.AlterColumn<string>(
                name: "FileDinhKem",
                table: "ThongDiepGuiCQTs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
