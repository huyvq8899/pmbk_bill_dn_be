using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonxulysaisothoadonkhoitaotumttduatrenlsguiemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolChoPhepXuLySaiSotMTTDuaTrenLichSuGuiEmail' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('BoolChoPhepXuLySaiSotMTTDuaTrenLichSuGuiEmail', N'Cho phép xử lý hóa đơn điện tử khởi tạo từ máy tính có sai sót căn cứ vào lịch sử gửi hóa đơn khách hàng trên phần mềm giải pháp', 'false')
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
