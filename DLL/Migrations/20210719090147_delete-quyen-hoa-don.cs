using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class deletequyenhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "FunctionName",
                keyValues: new object[]
                {
                    "HoaDonDienTu",
                    "HoaDonXoaBo",
                    "HoaDonThayThe",
                    "HoaDonDieuChinh"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
