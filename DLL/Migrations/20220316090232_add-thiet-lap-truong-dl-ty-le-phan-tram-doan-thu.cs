using DLL.Entity.Config;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthietlaptruongdltylephantramdoanthu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var query = new ThietLapTruongDuLieu().QueryInsertTyLePhanTramDoanhThu();
            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
