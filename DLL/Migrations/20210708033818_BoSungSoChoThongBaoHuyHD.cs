using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class BoSungSoChoThongBaoHuyHD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "So",
                table: "ThongBaoKetQuaHuyHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "So",
                table: "ThongBaoKetQuaHuyHoaDons");
        }
    }
}
