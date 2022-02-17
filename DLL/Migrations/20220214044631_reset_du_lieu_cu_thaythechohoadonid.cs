using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class reset_du_lieu_cu_thaythechohoadonid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"update [dbo].[HoaDonDienTus] set ThayTheChoHoaDonId = null, LyDoThayThe = null  where trangthai = 1 and ThayTheChoHoaDonId is not null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
