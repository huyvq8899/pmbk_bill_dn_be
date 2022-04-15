using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoltylephantramdoanthuhddtct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TyLePhanTramDoanhThu",
                table: "HoaDonDienTuChiTiets",
                type: "decimal(6,4)",
                nullable: true);

            var query = @"
                UPDATE hddtct
                SET TyLePhanTramDoanhThu = IIF(hddt.TyLePhanTramDoanhThu IS NULL, 0, hddt.TyLePhanTramDoanhThu)
                FROM HoaDonDienTuChiTiets hddtct
                JOIN HoaDonDienTus hddt ON hddtct.HoaDonDienTuId = hddt.HoaDonDienTuId
                WHERE hddtct.TyLePhanTramDoanhThu IS NULL;
            ";

            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TyLePhanTramDoanhThu",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
