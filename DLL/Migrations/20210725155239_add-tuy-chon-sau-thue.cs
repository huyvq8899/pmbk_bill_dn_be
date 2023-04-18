using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonsauthue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "PhatSinhBanHangTheoDGSauThue",
                        "Phát sinh bán hàng theo đơn giá sau thuế",
                        "false"
                    },
                    {
                        "TinhTienTheoDGSauThue",
                        "Tính tiền theo đơn giá sau thuế",
                        "false"
                    },
                    {
                        "TinhTienTheoSLvaDGSauThue",
                        "Tính tiền theo số lượng và đơn giá sau thuế",
                        "false"
                    },
                    {
                        "TinhSLTheoDGvaTienSauThue",
                        "Tính số lượng theo đơn giá và tiền sau thuế",
                        "false"
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
                    "PhatSinhBanHangTheoDGSauThue",
                    "TinhTienTheoDGSauThue",
                    "TinhTienTheoSLvaDGSauThue",
                    "TinhSLTheoDGvaTienSauThue",
                });
        }
    }
}
