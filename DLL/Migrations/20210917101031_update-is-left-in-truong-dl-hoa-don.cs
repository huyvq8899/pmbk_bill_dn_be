using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updateisleftintruongdlhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
               table: "TruongDuLieuHoaDons",
               keyColumn: "MaTruong",
               keyValues: new string[]
               {
                    "NHD",
                    "SHD",
                    "MSHD",
                    "KHHD",
                    "HHDV 1",
                    "HHDV 2",
                    "HHDV 3"
               },
               column: "IsLeft",
               values: new object[]
               {
                   true,
                   true,
                   true,
                   true,
                   true,
                   true,
                   true
               }
               );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
