using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychoncanhbaokhongchonnhanvienbanhang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                    table: "TuyChons",
                    columns: new string[] { "Ma", "Ten", "GiaTri" },
                    values: new object[,]
                    {
                        {
                            "BoolCanhBaoKhiKhongChonNhanVienBanHangTrenHoaDon",
                            "Cảnh báo khi không chọn nhân viên bán hàng trên hóa đơn",
                            "false"
                        }
                    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TuyChons",
                keyColumn: "Ma",
                keyValue: "BoolCanhBaoKhiKhongChonNhanVienBanHangTrenHoaDon"
                );
        }
    }
}
