using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_gui04loai3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "HinhThucTBaoHuyGiaiTrinhKhac",
                table: "ThongDiepGuiCQTs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTBaoHuyGiaiTrinhKhacCuaNNT",
                table: "ThongDiepGuiCQTs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HinhThucTBaoHuyGiaiTrinhKhac",
                table: "ThongDiepGuiCQTs");

            migrationBuilder.DropColumn(
                name: "IsTBaoHuyGiaiTrinhKhacCuaNNT",
                table: "ThongDiepGuiCQTs");
        }
    }
}
