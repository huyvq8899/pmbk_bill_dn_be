using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class deletefunctiontinhhinhsudunghoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Functions",
                keyColumn: "FunctionName",
                keyValue: "TinhHinhSuDungHoaDon"

            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
