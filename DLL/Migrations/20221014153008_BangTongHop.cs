using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class BangTongHop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DVTTe",
                table: "BangTongHopDuLieuHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TGTKhac",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TGia",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TTPhi",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DVTTe",
                table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "TGTKhac",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "TGia",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "TTPhi",
                table: "BangTongHopDuLieuHoaDonChiTiets");
        }
    }
}
