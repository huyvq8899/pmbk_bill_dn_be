using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongBaoDieuChinhThongTinHoaDonChiTiets",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets");

            migrationBuilder.AlterColumn<string>(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTietId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThongBaoDieuChinhThongTinHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongBaoDieuChinhThongTinHoaDonChiTiets",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "ThongBaoDieuChinhThongTinHoaDonChiTietId");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_ThongBaoDieuChinhThongTinHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "ThongBaoDieuChinhThongTinHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongBaoDieuChinhThongTinHoaDonChiTiets",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_ThongBaoDieuChinhThongTinHoaDonChiTiets_ThongBaoDieuChinhThongTinHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets");

            migrationBuilder.AlterColumn<string>(
                name: "ThongBaoDieuChinhThongTinHoaDonId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ThongBaoDieuChinhThongTinHoaDonChiTietId",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongBaoDieuChinhThongTinHoaDonChiTiets",
                table: "ThongBaoDieuChinhThongTinHoaDonChiTiets",
                column: "ThongBaoDieuChinhThongTinHoaDonId");
        }
    }
}
