using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updatecolstruongdlhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update TruongDuLieuHoaDons set TenTruongHienThi = TenTruong");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
