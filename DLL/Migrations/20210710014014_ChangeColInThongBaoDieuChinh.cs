using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeColInThongBaoDieuChinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThaiNop",
                table: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiHieuLuc",
                table: "ThongBaoDieuChinhThongTinHoaDons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThaiHieuLuc",
                table: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiNop",
                table: "ThongBaoDieuChinhThongTinHoaDons",
                nullable: false,
                defaultValue: 0);
        }
    }
}
