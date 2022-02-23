using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addchungthusosudung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChungThuSoSuDungs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    STT = table.Column<int>(nullable: true),
                    TTChuc = table.Column<string>(maxLength: 400, nullable: false),
                    Seri = table.Column<string>(maxLength: 40, nullable: false),
                    TNgay = table.Column<string>(nullable: false),
                    DNgay = table.Column<string>(nullable: false),
                    HThuc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChungThuSoSuDungs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChungThuSoSuDungs");
        }
    }
}
