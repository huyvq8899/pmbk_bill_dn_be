using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class themtuychonchophepnhapsoam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'BoolChoPhepNhapSoAm' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('BoolChoPhepNhapSoAm', N'Cho phép được ghi dấu âm (-) các nội dung về giá trị trên hóa đơn đối với hóa đơn có trạng thái hóa đơn là Hóa đơn gốc và Hóa đơn thay thế ', 'false')
                                END");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
