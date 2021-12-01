using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class editcoltrangthaihoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiHoaDon",
                table: "ThongTinHoaDons",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TrangThaiHoaDon",
                table: "ThongTinHoaDons",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
