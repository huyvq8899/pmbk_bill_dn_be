using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changetypesttthongtinhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "STT",
                table: "QuanLyThongTinHoaDons",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "STT",
                table: "QuanLyThongTinHoaDons",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
