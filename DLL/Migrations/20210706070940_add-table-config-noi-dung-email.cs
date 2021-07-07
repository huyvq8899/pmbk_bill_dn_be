using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtableconfignoidungemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigNoiDungEmails",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LoaiEmail = table.Column<int>(nullable: false),
                    TieuDeEmail = table.Column<string>(nullable: true),
                    NoiDungEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigNoiDungEmails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigNoiDungEmails");
        }
    }
}
