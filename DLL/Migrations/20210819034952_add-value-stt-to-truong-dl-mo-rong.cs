using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addvaluestttotruongdlmorong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ThietLapTruongDuLieuMoRongs",
                column: "STT",
                values: new object[]
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    10
                },
                keyColumn: "TenTruong",
                keyValues: new object[]
                {
                    "Trường thông tin bổ sung 1",
                    "Trường thông tin bổ sung 2",
                    "Trường thông tin bổ sung 3",
                    "Trường thông tin bổ sung 4",
                    "Trường thông tin bổ sung 5",
                    "Trường thông tin bổ sung 6",
                    "Trường thông tin bổ sung 7",
                    "Trường thông tin bổ sung 8",
                    "Trường thông tin bổ sung 9",
                    "Trường thông tin bổ sung 10",
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
