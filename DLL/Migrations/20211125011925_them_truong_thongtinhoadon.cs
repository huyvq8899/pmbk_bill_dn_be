using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_truong_thongtinhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoaiTienId",
                table: "ThongTinHoaDons",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaTraCuu",
                table: "ThongTinHoaDons",
                maxLength: 34,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThanhTien",
                table: "ThongTinHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiTienId",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "MaTraCuu",
                table: "ThongTinHoaDons");

            migrationBuilder.DropColumn(
                name: "ThanhTien",
                table: "ThongTinHoaDons");
        }
    }
}
