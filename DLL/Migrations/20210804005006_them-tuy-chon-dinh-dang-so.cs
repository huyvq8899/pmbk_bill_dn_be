using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class themtuychondinhdangso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "IntDinhDangSoThapPhanTienQuyDoi",
                        "Định dạng số thập phân tiền quy đổi",
                        "0"
                    },
                    {
                        "IntDinhDangSoThapPhanTienNgoaiTe",
                        "Định dạng số thập phân tiền ngoại tệ",
                        "2"
                    },
                    {
                        "IntDinhDangSoThapPhanDonGiaQuyDoi",
                        "Định dạng số thập phân đơn giá quy đổi",
                        "2"
                    },
                    {
                        "IntDinhDangSoThapPhanDonGiaNgoaiTe",
                        "Định dạng số thập phân đơn giá ngoại tệ",
                        "2"
                    },
                    {
                        "IntDinhDangSoThapPhanSoLuong",
                        "Định dạng số thập phân số lượng",
                        "2"
                    },
                    {
                        "IntDinhDangSoThapPhanTyGia",
                        "Định dạng số thập phân tỷ giá",
                        "2"
                    },
                    {
                        "IntDinhDangSoThapPhanTyLePhanBo",
                        "Định dạng số thập phân tỷ lệ phân bổ",
                        "10"
                    },
                    {
                        "IntDinhDangSoThapPhanHeSoTyLe",
                        "Định dạng số thập phân hệ số tỷ lệ",
                        "2"
                    },
                    {
                        "IntDinhDangSoThapPhanSoCong",
                        "Định dạng số thập phân số cộng",
                        "0"
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TuyChons",
                keyColumn: "Ma",
                keyValues: new object[] {
                    "IntDinhDangSoThapPhanTienQuyDoi",
                    "IntDinhDangSoThapPhanTienNgoaiTe",
                    "IntDinhDangSoThapPhanDonGiaQuyDoi",
                    "IntDinhDangSoThapPhanDonGiaNgoaiTe",
                    "IntDinhDangSoThapPhanSoLuong",
                    "IntDinhDangSoThapPhanTyGia",
                    "IntDinhDangSoThapPhanTyLePhanBo",
                    "IntDinhDangSoThapPhanHeSoTyLe",
                    "IntDinhDangSoThapPhanSoCong"
                });
        }
    }
}
