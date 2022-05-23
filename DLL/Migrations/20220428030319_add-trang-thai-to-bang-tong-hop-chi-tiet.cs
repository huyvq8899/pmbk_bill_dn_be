using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtrangthaitobangtonghopchitiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BackupTrangThai",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "BangTongHopDuLieuHoaDonChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupTrangThai",
                table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "BangTongHopDuLieuHoaDonChiTiets");
        }
    }
}
