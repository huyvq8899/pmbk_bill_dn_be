using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updategiatridocsotientuychon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE TuyChons SET GiaTri = N'linh' WHERE Ma = 'CachDocSo0OHangChuc' AND GiaTri = N'Linh';
                UPDATE TuyChons SET GiaTri = N'lẻ' WHERE Ma = 'CachDocSo0OHangChuc' AND GiaTri = N'Lẻ';
                UPDATE TuyChons SET GiaTri = N'nghìn' WHERE Ma = 'CachDocSoTienOHangNghin' AND GiaTri = N'Nghìn';
                UPDATE TuyChons SET GiaTri = N'ngàn' WHERE Ma = 'CachDocSoTienOHangNghin' AND GiaTri = N'Ngàn';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
