using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addbokyhieudangphathanhtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoKyHieuDangPhatHanhs",
                columns: table => new
                {
                    BoKyHieuHoaDonId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoKyHieuDangPhatHanhs", x => x.BoKyHieuHoaDonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoKyHieuDangPhatHanhs");
        }
    }
}
