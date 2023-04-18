using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddFileMauHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaSoThue",
                table: "MauHoaDonFiles",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayXacThuc",
                table: "MauHoaDonFiles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MauFile_HoaDons",
                columns: table => new
                {
                    MauFile_HoaDonId = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(maxLength: 36, nullable: true),
                    MauHoaDonFileId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauFile_HoaDons", x => x.MauFile_HoaDonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HopDongHoaDons");

            migrationBuilder.DropTable(
                name: "MauFile_HoaDons");

            migrationBuilder.DropColumn(
                name: "MaSoThue",
                table: "MauHoaDonFiles");

            migrationBuilder.DropColumn(
                name: "NgayXacThuc",
                table: "MauHoaDonFiles");
        }
    }
}
