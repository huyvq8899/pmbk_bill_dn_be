using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class UpdateDisableAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE AlertStartups SET [Status]=0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
