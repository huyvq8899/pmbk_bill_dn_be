using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updateloaiapdunghddc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoaiApDungHoaDonDieuChinh",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoaiApDungHoaDonDieuChinh",
                table: "HoaDonDienTus",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
