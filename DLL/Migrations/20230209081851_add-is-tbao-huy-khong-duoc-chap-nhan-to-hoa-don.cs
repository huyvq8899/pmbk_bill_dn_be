using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addistbaohuykhongduocchapnhantohoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_MauHoaDons_PhanQuyenMauHoaDons_PhanQuyenMauHoaDonId",
            //    table: "MauHoaDons");

            //migrationBuilder.DropIndex(
            //    name: "IX_MauHoaDons_PhanQuyenMauHoaDonId",
            //    table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "PhanQuyenMauHoaDonId",
                table: "MauHoaDons");

            migrationBuilder.AddColumn<bool>(
                name: "IsTBaoHuyKhongDuocChapNhan",
                table: "HoaDonDienTus",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTBaoHuyKhongDuocChapNhan",
                table: "HoaDonDienTus");

            migrationBuilder.AddColumn<string>(
                name: "PhanQuyenMauHoaDonId",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDons_PhanQuyenMauHoaDonId",
                table: "MauHoaDons",
                column: "PhanQuyenMauHoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_MauHoaDons_PhanQuyenMauHoaDons_PhanQuyenMauHoaDonId",
                table: "MauHoaDons",
                column: "PhanQuyenMauHoaDonId",
                principalTable: "PhanQuyenMauHoaDons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
