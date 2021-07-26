using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychoncophatsinhnghiepvungoaite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "BoolCoPhatSinhNghiepVuNgoaiTe",
                        "Có phát sinh nghiệp vụ ngoại tệ",
                        "false"
                    }
                });

            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "BoolQuanLyNhanVienBanHangTrenHoaDon",
                        "Quản lý nhân viên bán hàng trên hóa đơn",
                        "false"
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TuyChons",
                keyColumn: "Ma",
                keyValues: new object[]
                {
                    "BoolQuanLyNhanVienBanHangTrenHoaDon",
                    "BoolCoPhatSinhNghiepVuNgoaiTe",
                });
        }
    }
}
