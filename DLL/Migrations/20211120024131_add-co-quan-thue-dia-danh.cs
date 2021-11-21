using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoquanthuediadanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoQuanThueCapCuc_DiaDanhs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MaCQT = table.Column<string>(nullable: true),
                    MaDiaDanh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoQuanThueCapCuc_DiaDanhs", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT INTO CoQuanThueCapCuc_DiaDanhs(Id, MaCQT, MaDiaDanh)
                                    SELECT NEWID() AS Id, * FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                                      'Excel 12.0 Xml; HDR=YES; IMEX=1;
                                       Database=D:\GIT\CQT_DiaDanh.xlsx',
                                       [CQT$]);
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoQuanThueCapCuc_DiaDanhs");
        }
    }
}
