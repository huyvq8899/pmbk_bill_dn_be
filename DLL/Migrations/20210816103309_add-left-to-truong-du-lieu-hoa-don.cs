using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addlefttotruongdulieuhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsChiTiet",
                table: "TruongDuLieuHoaDons",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeft",
                table: "TruongDuLieuHoaDons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Left",
                table: "TruongDuLieuHoaDons",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLeft",
                table: "TruongDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "Left",
                table: "TruongDuLieuHoaDons");

            migrationBuilder.AlterColumn<string>(
                name: "IsChiTiet",
                table: "TruongDuLieuHoaDons",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
