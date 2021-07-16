using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddTongTien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TongTienChietKhau",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienChietKhauQuyDoi",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienHangQuyDoi",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienThanhToan",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienThueGTGT",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienThueGTGTQuyDoi",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienThanhToan",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTuChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongTienChietKhau",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienChietKhauQuyDoi",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienHangQuyDoi",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienThanhToan",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienThueGTGT",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienThueGTGTQuyDoi",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienThanhToan",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
