using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addrefhoadondientutobangtonghop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefHoaDonDienTuId",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefHoaDonDienTuId",
                table: "BangTongHopDuLieuHoaDonChiTiets");
        }
    }
}
