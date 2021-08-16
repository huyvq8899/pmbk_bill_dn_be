using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addtabletruongdulieuhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TruongDuLieuHoaDons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    STT = table.Column<int>(nullable: false),
                    MaTruong = table.Column<string>(nullable: true),
                    TenTruong = table.Column<string>(nullable: true),
                    TenTruongData = table.Column<string>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true),
                    IsChiTiet = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Default = table.Column<bool>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Align = table.Column<string>(nullable: true),
                    DefaultSTT = table.Column<int>(nullable: false),
                    DinhDangSo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongDuLieuHoaDons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TruongDuLieuHoaDons");
        }
    }
}
