using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changepkfiledatatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileDatas",
                table: "FileDatas");

            migrationBuilder.AlterColumn<string>(
                name: "RefId",
                table: "FileDatas",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "FileDataId",
                table: "FileDatas",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileDatas",
                table: "FileDatas",
                column: "FileDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileDatas",
                table: "FileDatas");

            migrationBuilder.DropColumn(
                name: "FileDataId",
                table: "FileDatas");

            migrationBuilder.AlterColumn<string>(
                name: "RefId",
                table: "FileDatas",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileDatas",
                table: "FileDatas",
                column: "RefId");
        }
    }
}
