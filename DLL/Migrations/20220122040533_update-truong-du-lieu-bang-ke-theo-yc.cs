using DLL.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Updatetruongdulieubangketheoyc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM ThietLapTruongDuLieus WHERE LoaiTruongDuLieu = 0");

            var query = new ThietLapTruongDuLieuData().QueryUpdateTruongDuLieuTheoYeuCauCuaSep();
            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
