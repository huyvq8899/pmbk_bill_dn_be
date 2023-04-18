using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonnopthue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "TuyChons",
               columns: new string[] { "Ma", "Ten", "GiaTri" },
               values: new object[,]
               {
                        {
                            "IsNopThueTheoThongTu1032014BTC",
                            "Nộp thuế theo quy định tại Điều 11, Thông tư số 103/2014/TT-BTC",
                            "false"
                        },
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
