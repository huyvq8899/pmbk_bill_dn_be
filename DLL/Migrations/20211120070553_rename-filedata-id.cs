using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class renamefiledataid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileDataId",
                table: "FileDatas",
                newName: "RefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefId",
                table: "FileDatas",
                newName: "FileDataId");
        }
    }
}
