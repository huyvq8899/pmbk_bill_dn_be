using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Add_custom_key_tuy_chinh_chi_tiet_mhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomKey",
                table: "MauHoaDonTuyChinhChiTiets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomKey",
                table: "MauHoaDonTuyChinhChiTiets");
        }
    }
}
