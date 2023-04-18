using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddColTableThongDiepChung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "MauSoTBaoPhanHoiCuaCQT",
               table: "ThongDiepChungs",
               maxLength: 255,
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MauSoTBaoPhanHoiCuaCQT",
                table: "ThongDiepChungs");
        }
    }
}
