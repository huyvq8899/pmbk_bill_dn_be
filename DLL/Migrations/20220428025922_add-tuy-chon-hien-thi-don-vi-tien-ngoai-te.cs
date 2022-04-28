using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonhienthidonvitienngoaite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolHienThiDonViTienNgoaiTeTrenHoaDon' )
                                BEGIN
									declare @parentValue nvarchar(450);
									SELECT @parentValue = GiaTri FROM TuyChons WHERE Ma = 'BoolCoPhatSinhNghiepVuNgoaiTe';
                                    INSERT INTO TuyChons VALUES ('BoolHienThiDonViTienNgoaiTeTrenHoaDon', N'Hiển thị đơn vị tiền ngoại tệ trên hóa đơn', @parentValue);
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
