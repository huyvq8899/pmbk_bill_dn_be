using DLL.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtruongdulieumathangduocgiam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var query = new ThietLapTruongDuLieuData().QueryInsertIsMatHangDuocGiam();
            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
