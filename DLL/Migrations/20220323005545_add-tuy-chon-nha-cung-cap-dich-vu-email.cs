using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonnhacungcapdichvuemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'NhaCungCapDichVuEmail' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('NhaCungCapDichVuEmail', N'Nhà cung cấp dịch vụ email', '1')
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
