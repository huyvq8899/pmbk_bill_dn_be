using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class update_truong_sothongbaosaisot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SoThongBaoSaiSot",
                table: "ThongDiepGuiCQTs",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 15,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SoThongBaoSaiSot",
                table: "ThongDiepGuiCQTs",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);
        }
    }
}
