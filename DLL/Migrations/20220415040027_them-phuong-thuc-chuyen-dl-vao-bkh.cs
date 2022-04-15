using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class themphuongthucchuyendlvaobkh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhuongThucChuyenDL",
                table: "BoKyHieuHoaDons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhuongThucChuyenDL",
                table: "BoKyHieuHoaDons");
        }
    }
}
