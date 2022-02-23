using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_du_lieu_2512 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChungTuLienQuan",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 80,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NhatKyGuiEmailId",
                table: "ThongBaoSaiThongTins",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailTBaoSaiSotKhongPhaiLapHDId",
                table: "HoaDonDienTus",
                maxLength: 36,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NhatKyGuiEmailId",
                table: "ThongBaoSaiThongTins");

            migrationBuilder.DropColumn(
                name: "EmailTBaoSaiSotKhongPhaiLapHDId",
                table: "HoaDonDienTus");

            migrationBuilder.AlterColumn<string>(
                name: "ChungTuLienQuan",
                table: "ThongDiepChiTietGuiCQTs",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 80,
                oldNullable: true);
        }
    }
}
