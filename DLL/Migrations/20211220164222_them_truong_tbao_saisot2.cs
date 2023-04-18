using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_tbao_saisot2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChungTuLienQuan",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DienGiaiTrangThai",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiHoaDon",
                table: "ThongDiepChiTietGuiCQTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChungTuLienQuan",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "DienGiaiTrangThai",
                table: "ThongDiepChiTietGuiCQTs");

            migrationBuilder.DropColumn(
                name: "TrangThaiHoaDon",
                table: "ThongDiepChiTietGuiCQTs");
        }
    }
}
