using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddiadanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaDanhs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaDanhs", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO DiaDanhs(Id, Ma, Ten)
                                    SELECT NEWID() AS Id, * FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                                      'Excel 12.0 Xml; HDR=YES; IMEX=1;
                                       Database=D:\GIT\DiaDanh.xlsx',
                                       [CQT$]);
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaDanhs");
        }
    }
}
