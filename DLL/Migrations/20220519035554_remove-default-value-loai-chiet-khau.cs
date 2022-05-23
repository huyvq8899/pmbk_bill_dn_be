using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class removedefaultvalueloaichietkhau : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoaiChietKhau",
                table: "HoaDonDienTus",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LoaiChietKhau",
                table: "HoaDonDienTus",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int));
        }
    }
}
