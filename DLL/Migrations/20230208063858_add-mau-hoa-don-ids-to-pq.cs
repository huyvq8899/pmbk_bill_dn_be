using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addmauhoadonidstopq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MauHoaDonIds",
                table: "PhanQuyenMauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhanQuyenMauHoaDonId",
                table: "MauHoaDons",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_MauHoaDons_PhanQuyenMauHoaDonId",
            //    table: "MauHoaDons",
            //    column: "PhanQuyenMauHoaDonId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_MauHoaDons_PhanQuyenMauHoaDons_PhanQuyenMauHoaDonId",
            //    table: "MauHoaDons",
            //    column: "PhanQuyenMauHoaDonId",
            //    principalTable: "PhanQuyenMauHoaDons",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MauHoaDons_PhanQuyenMauHoaDons_PhanQuyenMauHoaDonId",
                table: "MauHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_MauHoaDons_PhanQuyenMauHoaDonId",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "MauHoaDonIds",
                table: "PhanQuyenMauHoaDons");

            migrationBuilder.DropColumn(
                name: "PhanQuyenMauHoaDonId",
                table: "MauHoaDons");
        }
    }
}
