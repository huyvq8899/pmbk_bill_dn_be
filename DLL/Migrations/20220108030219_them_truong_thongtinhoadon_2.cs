using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_thongtinhoadon_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDaLapThongBao04",
                table: "ThongTinHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LanGui04",
                table: "ThongTinHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThongDiepGuiCQTId",
                table: "ThongTinHoaDons",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiGui04",
                table: "ThongTinHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDaLapThongBao04",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "LanGui04",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "ThongDiepGuiCQTId",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "TrangThaiGui04",
                table: "ThongTinHoaDons");
        }
    }
}
