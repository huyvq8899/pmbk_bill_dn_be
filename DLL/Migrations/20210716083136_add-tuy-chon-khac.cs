using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonkhac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "CanhBaoKhiNhapMaSoThueKhongHopLe",
                        "Cảnh báo khi nhập mã số thuế không hợp lệ",
                        "KhongCB"
                    },
                    {
                        "KyNopBaoCaoTinhHinhSuDungHoaDon",
                        "Kỳ nộp báo cáo tình hình sử dụng hóa đơn",
                        "Quy"
                    },
                    {
                        "KyKeKhaiThueGTGT",
                        "Kỳ kê khai thuế GTGT",
                        "Thang"
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
