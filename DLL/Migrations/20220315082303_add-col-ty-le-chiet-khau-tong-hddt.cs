using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoltylechietkhautonghddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TyLeChietKhau",
                table: "HoaDonDienTus",
                type: "decimal(6,4)",
                nullable: true);

            var query = @"DECLARE @heSoTyLe int;
            SET @heSoTyLe = CAST((select GiaTri from TuyChons WHERE Ma = 'IntDinhDangSoThapPhanHeSoTyLe') as int);
            UPDATE HoaDonDienTus SET TyLeChietKhau = ROUND(TongTienChietKhau * 100 / IIF(TongTienHang = 0, 1, TongTienHang), @heSoTyLe) WHERE TyLeChietKhau IS NULL";

            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TyLeChietKhau",
                table: "HoaDonDienTus");
        }
    }
}
