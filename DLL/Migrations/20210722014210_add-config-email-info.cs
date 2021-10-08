using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addconfigemailinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "TuyChons",
               columns: new string[] { "Ma", "Ten", "GiaTri" },
               values: new object[,]
               {
                    {
                        "TenDangNhapEmail",
                        "Tên đăng nhập email",
                        "hotro@pmbk.vn"
                    },
                    {
                        "TenNguoiGui",
                        "Tên người gửi",
                        "HÓA ĐƠN BÁCH KHOA"
                    },
                    {
                        "MatKhauEmail",
                        "Mật khẩu email gửi",
                        "pmbk@2019"
                    },
                    {
                        "TenMayChu",
                        "Tên máy chủ",
                        "mail9096.maychuemail.com"
                    },
                    {
                        "SoCong",
                        "Số cổng",
                        "465"
                    },
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TuyChons",
                keyColumn: "Ma",
                keyValues: new object[]
                {
                    "TenDangNhapEmail",
                    "TenNguoiGui",
                    "MatKhauEmail",
                    "TenMayChu",
                    "SoCong"
                });
        }
    }
}
