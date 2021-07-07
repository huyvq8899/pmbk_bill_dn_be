using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class ChangeColThongBaoPhatHanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChiTruSo",
                table: "ThongBaoPhatHanhs");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "ThongBaoPhatHanhs");

            migrationBuilder.RenameColumn(
                name: "TenDonViPhatHanh",
                table: "ThongBaoPhatHanhs",
                newName: "DienThoai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DienThoai",
                table: "ThongBaoPhatHanhs",
                newName: "TenDonViPhatHanh");

            migrationBuilder.AddColumn<string>(
                name: "DiaChiTruSo",
                table: "ThongBaoPhatHanhs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "ThongBaoPhatHanhs",
                nullable: true);
        }
    }
}
