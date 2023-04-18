using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class dieuchinhcolbokyhieuhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoKyHieuHoaDons_DangKyUyNhiems_DangKyUyNhiemId",
                table: "BoKyHieuHoaDons");

            //migrationBuilder.DropColumn(
            //    name: "TenTrangThaiGui",
            //    table: "ThongDiepChungs");

            migrationBuilder.RenameColumn(
                name: "DangKyUyNhiemId",
                table: "BoKyHieuHoaDons",
                newName: "ThongDiepChungId");

            migrationBuilder.RenameIndex(
                name: "IX_BoKyHieuHoaDons_DangKyUyNhiemId",
                table: "BoKyHieuHoaDons",
                newName: "IX_BoKyHieuHoaDons_ThongDiepChungId");

            //migrationBuilder.AddColumn<string>(
            //    name: "PhuongPhapTinhThueGTGT",
            //    table: "HoSoHDDTs",
            //    nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoKyHieuHoaDons_ThongDiepChungs_ThongDiepChungId",
                table: "BoKyHieuHoaDons",
                column: "ThongDiepChungId",
                principalTable: "ThongDiepChungs",
                principalColumn: "ThongDiepChungId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoKyHieuHoaDons_ThongDiepChungs_ThongDiepChungId",
                table: "BoKyHieuHoaDons");

            //migrationBuilder.DropColumn(
            //    name: "PhuongPhapTinhThueGTGT",
            //    table: "HoSoHDDTs");

            migrationBuilder.RenameColumn(
                name: "ThongDiepChungId",
                table: "BoKyHieuHoaDons",
                newName: "DangKyUyNhiemId");

            migrationBuilder.RenameIndex(
                name: "IX_BoKyHieuHoaDons_ThongDiepChungId",
                table: "BoKyHieuHoaDons",
                newName: "IX_BoKyHieuHoaDons_DangKyUyNhiemId");

            //migrationBuilder.AddColumn<string>(
            //    name: "TenTrangThaiGui",
            //    table: "ThongDiepChungs",
            //    nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoKyHieuHoaDons_DangKyUyNhiems_DangKyUyNhiemId",
                table: "BoKyHieuHoaDons",
                column: "DangKyUyNhiemId",
                principalTable: "DangKyUyNhiems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
