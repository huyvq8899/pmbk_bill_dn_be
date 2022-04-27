using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcachthehiensotientuychon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'CachTheHienSoTienBangChu' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('CachTheHienSoTienBangChu', N'Cách thể hiện dòng Số tiền bằng chữ trên hóa đơn điều chỉnh', '1');
                                END");

            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'CachTheHienSoTienThueLaKCT' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('CachTheHienSoTienThueLaKCT', N'Cách thể hiện Số tiền thuế trên bản thể hiện hóa đơn điện tử khi thuế suất GTGT là KCT', '0');
                                END");

            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'CachTheHienSoTienThueLaKKKNT' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('CachTheHienSoTienThueLaKKKNT', N'Cách thể hiện Số tiền thuế trên bản thể hiện hóa đơn điện tử khi thuế suất GTGT là KKKNT', '0');
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
