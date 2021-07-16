using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "CachDocSo0OHangChuc",
                        "Cách đọc số 0 ở hàng chục",
                        "Linh"
                    },
                    {
                        "CachDocSoTienOHangNghin",
                        "Cách đọc số tiền ở hàng nghìn",
                        "Nghìn"
                    },
                    {
                        "BoolHienThiTuChanKhiDocSoTien",
                        "Hiển thị từ chẵn khi đọc số tiền",
                        "false"
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
