using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addloaihoadontotruongdl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoaiHoaDon",
                table: "TruongDuLieuHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoaiHoaDon",
                table: "ThietLapTruongDuLieuMoRongs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiHoaDon",
                table: "TruongDuLieuHoaDons");

            migrationBuilder.DropColumn(
                name: "LoaiHoaDon",
                table: "ThietLapTruongDuLieuMoRongs");
        }
    }
}
