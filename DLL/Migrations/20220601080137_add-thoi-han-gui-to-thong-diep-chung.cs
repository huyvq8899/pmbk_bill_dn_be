using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthoihanguitothongdiepchung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThoiHan",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaCQTCap",
                table: "ThongBaoChiTietHoaDonRaSoats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThoiHan",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "MaCQTCap",
                table: "ThongBaoChiTietHoaDonRaSoats");
        }
    }
}
