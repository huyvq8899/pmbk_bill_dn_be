using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddangkyuynhiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DangKyUyNhiems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdToKhai = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    TLHDon = table.Column<string>(maxLength: 100, nullable: true),
                    KHMSHDon = table.Column<int>(nullable: true),
                    KHHDon = table.Column<string>(maxLength: 6, nullable: true),
                    MST = table.Column<string>(maxLength: 14, nullable: true),
                    TTChuc = table.Column<string>(maxLength: 400, nullable: true),
                    MDich = table.Column<string>(maxLength: 255, nullable: true),
                    TNgay = table.Column<string>(nullable: true),
                    DNgay = table.Column<string>(nullable: true),
                    PThuc = table.Column<int>(nullable: false),
                    THTTTKhac = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyUyNhiems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKyUyNhiems");
        }
    }
}
