using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changesohoadontypetolong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SoHoaDon",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SoToiDa",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SoLonNhatDaLapDenHienTai",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SoBatDau",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoHoaDon",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SoToiDa",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SoLonNhatDaLapDenHienTai",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SoBatDau",
                table: "BoKyHieuHoaDons",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
