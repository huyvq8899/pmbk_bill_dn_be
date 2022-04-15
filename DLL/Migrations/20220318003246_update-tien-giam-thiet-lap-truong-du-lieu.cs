using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updatetiengiamthietlaptruongdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ThietLapTruongDuLieus
                SET MaTruong = 'HHDV 37', STT = 37
                WHERE TenCot = 'TyLePhanTramDoanhThu';

                UPDATE ThietLapTruongDuLieus
                SET MaTruong = 'HHDV 38', STT = 38
                WHERE TenCot = 'TienGiam';

                UPDATE ThietLapTruongDuLieus
                SET MaTruong = 'HHDV 39', STT = 39
                WHERE TenCot = 'TienGiamQuyDoi';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
