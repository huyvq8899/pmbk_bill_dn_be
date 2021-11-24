using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class deletetabletrangthaiguitokhai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "TrangThaiTiepNhan",
                table: "ThongDiepChungs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThaiTiepNhan",
                table: "ThongDiepChungs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrangThaiGuiToKhais",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FileXMLGui = table.Column<string>(nullable: true),
                    IdToKhai = table.Column<string>(nullable: true),
                    MaThongDiep = table.Column<string>(nullable: true),
                    NgayGioGui = table.Column<DateTime>(nullable: true),
                    NoiDungFileGui = table.Column<byte[]>(nullable: true),
                    TrangThaiGui = table.Column<int>(nullable: false),
                    TrangThaiTiepNhan = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiGuiToKhais", x => x.Id);
                });
        }
    }
}
