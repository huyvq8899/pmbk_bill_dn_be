using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddBienlaiForPos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDo",
                table: "ThongBaoChiTietHoaDonRaSoats",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BienLaiId",
                table: "HoaDonDienTus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDo",
                table: "ThongBaoChiTietHoaDonRaSoats");

            migrationBuilder.DropColumn(
                name: "BienLaiId",
                table: "HoaDonDienTus");
        }
    }
}
