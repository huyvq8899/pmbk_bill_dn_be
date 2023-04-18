using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonlapHDMTTvaCPXNHDCTguichoKH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolChoPhepLapHDDTMTT' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('BoolChoPhepLapHDDTMTT', N'Cho phép lập hóa đơn điện tử khởi tạo từ máy tính tiền (Hóa đơn gốc) trên phần mềm giải pháp', 'true')
                                END");
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolChoPhepXacNhanHDDaGuiKhachHang' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('BoolChoPhepXacNhanHDDaGuiKhachHang', N'Cho phép xác nhận hóa đơn/chứng từ đã gửi cho khách hàng/người nộp thuế', 'true')
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
