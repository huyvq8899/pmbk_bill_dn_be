using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class themcanhbaoCanhBaoHDChenhLech : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TuyChons",
                columns: new string[] { "Ma", "Ten", "GiaTri" },
                values: new object[,]
                {
                    {
                        "CanhBaoHDChenhLech",
                        "Cảnh báo khi phát hành hóa đơn có số liệu phát sinh chênh lệch",
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
                    "CanhBaoHDChenhLech",
                });
        }
    }
}
