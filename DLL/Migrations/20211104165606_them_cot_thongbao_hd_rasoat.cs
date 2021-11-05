using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_cot_thongbao_hd_rasoat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThongBaoHoaDonRaSoatId",
                table: "ThongDiepGuiCQTs",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiep",
                table: "ThongBaoHoaDonRaSoats",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThongBaoHoaDonRaSoatId",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "MaThongDiep",
                table: "ThongBaoHoaDonRaSoats");
        }
    }
}
