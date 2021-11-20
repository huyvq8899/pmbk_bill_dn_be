using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoquanthue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoQuanThues",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    DiaChi = table.Column<string>(nullable: true),
                    MaCQTCapCuc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoQuanThues", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO CoQuanThues(Id, Ma, Ten, DiaChi, MaCQTCapCuc)
                                    SELECT NEWID() AS Id, * FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                                      'Excel 12.0 Xml; HDR=YES; IMEX=1;
                                       Database=D:\GIT\CQT.xlsx',
                                       [CQT$]);
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoQuanThues");

            migrationBuilder.Sql("Delete from CoQuanThues");
        }
    }
}
