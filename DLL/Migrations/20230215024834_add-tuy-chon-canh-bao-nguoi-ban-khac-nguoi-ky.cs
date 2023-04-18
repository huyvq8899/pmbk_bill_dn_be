using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychoncanhbaonguoibankhacnguoiky : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolCanhBaoNguoiBanKhacNguoiKy' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('BoolCanhBaoNguoiBanKhacNguoiKy', N'Cảnh báo thông tin tên người bán khác thông tin người ký khi phát hành hóa đơn', 'false')
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
