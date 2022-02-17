using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class reset_trang_thai_gui_300_hoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
update [dbo].[HoaDonDienTus] set TrangThaiGui04 = null, ThongDiepGuiCQTId = null where (IsDaLapThongBao04 is null or IsDaLapThongBao04 = 0) and TrangThaiGui04 >= -1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
