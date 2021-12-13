using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changecolumntypedecimalhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TyGia",
                table: "HoaDonDienTus",
                type: "decimal(7,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThueGTGTQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThueGTGT",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToan",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienHangQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienHang",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienChietKhauQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienChietKhau",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TyLeChietKhau",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(6,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToan",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienThueGTGTQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienThueGTGT",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienChietKhauQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienChietKhau",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTienSauThueQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTienSauThue",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTienQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTien",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SoLuong",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DonGiaSauThue",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DonGiaQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DonGia",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TyGia",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThueGTGTQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThueGTGT",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToan",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienHangQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienHang",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienChietKhauQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienChietKhau",
                table: "HoaDonDienTus",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TyLeChietKhau",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToanQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TongTienThanhToan",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienThueGTGTQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienThueGTGT",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienChietKhauQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TienChietKhau",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTienSauThueQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTienSauThue",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTienQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ThanhTien",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SoLuong",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DonGiaSauThue",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DonGiaQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DonGia",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(18,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(21,6)",
                oldNullable: true);
        }
    }
}
