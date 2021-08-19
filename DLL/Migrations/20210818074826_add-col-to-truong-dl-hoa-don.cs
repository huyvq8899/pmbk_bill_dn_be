using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoltotruongdlhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMoRong",
                table: "TruongDuLieuHoaDons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TenTruongHienThi",
                table: "TruongDuLieuHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMoRong",
                table: "TruongDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "TenTruongHienThi",
                table: "TruongDuLieuHoaDons");
        }
    }
}
