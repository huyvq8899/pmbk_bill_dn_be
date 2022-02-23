using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtiengiamdoanhthu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TyLePhanTramDoanhThu",
                table: "HoaDonDienTus",
                type: "decimal(6,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienGiam",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TongTienGiamQuyDoi",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TienGiam",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TienGiamQuyDoi",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(21,6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongTienGiam",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TongTienGiamQuyDoi",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TienGiam",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TienGiamQuyDoi",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.AlterColumn<decimal>(
                name: "TyLePhanTramDoanhThu",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,4)",
                oldNullable: true);
        }
    }
}
