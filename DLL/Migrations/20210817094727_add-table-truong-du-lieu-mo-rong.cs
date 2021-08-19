using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtabletruongdulieumorong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TruongDuLieuMoRongs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DataId = table.Column<string>(nullable: true),
                    TenTruong = table.Column<string>(nullable: true),
                    DuLieu = table.Column<string>(nullable: true),
                    HienThi = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongDuLieuMoRongs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TruongDuLieuMoRongs");
        }
    }
}
