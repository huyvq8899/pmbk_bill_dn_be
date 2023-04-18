using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtrangthaiguihoadonmtt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThaiGuiHoaDonMTT",
                table: "HoaDonDienTus",
                defaultValue: 3,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThaiGuiHoaDonMTT",
                table: "HoaDonDienTus");
        }
    }
}
