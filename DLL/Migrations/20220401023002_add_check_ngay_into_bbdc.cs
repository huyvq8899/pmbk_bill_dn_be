using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class add_check_ngay_into_bbdc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCheckNgay",
                table: "BienBanDieuChinhs",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCheckNgay",
                table: "BienBanDieuChinhs");
        }
    }
}
