using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addsuacqt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE CoQuanThues Set MaCQTCapCuc = Ma
                WHERE MaCQTCapCuc IS NULL
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
