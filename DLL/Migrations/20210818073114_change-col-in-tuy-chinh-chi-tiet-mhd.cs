using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changecolintuychinhchitietmhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TuyChonChiTiet",
                table: "MauHoaDonTuyChinhChiTiets",
                newName: "TuyChinhChiTiet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TuyChinhChiTiet",
                table: "MauHoaDonTuyChinhChiTiets",
                newName: "TuyChonChiTiet");
        }
    }
}
