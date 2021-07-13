using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class RemoveColKyHieuThongBaoHuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KyHieu",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KyHieu",
                table: "ThongBaoKetQuaHuyHoaDonChiTiets",
                nullable: true);
        }
    }
}
