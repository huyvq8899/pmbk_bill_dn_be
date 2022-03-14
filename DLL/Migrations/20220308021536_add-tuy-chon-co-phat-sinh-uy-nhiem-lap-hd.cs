using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychoncophatsinhuynhiemlaphd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolCoPhatSinhUyNhiemLapHoaDon' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('BoolCoPhatSinhUyNhiemLapHoaDon', N'Có phát sinh ủy nhiệm lập hóa đơn', 'false')
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
