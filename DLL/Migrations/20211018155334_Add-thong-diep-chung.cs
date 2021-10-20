using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addthongdiepchung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongDiepChungs",
                columns: table => new
                {
                    ThongDiepChungId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MaThongDiep = table.Column<string>(nullable: true),
                    MaLoaiThongDiep = table.Column<string>(nullable: true),
                    ThongDiepGuiDi = table.Column<bool>(nullable: false),
                    HinhThuc = table.Column<int>(nullable: true),
                    LanThu = table.Column<int>(nullable: false),
                    LanGui = table.Column<int>(nullable: false),
                    TrangThaiGui = table.Column<int>(nullable: false),
                    TrangThaiTiepNhan = table.Column<int>(nullable: false),
                    NoiNhan = table.Column<string>(nullable: true),
                    NgayGui = table.Column<DateTime>(nullable: true),
                    TaiLieuDinhKemId = table.Column<string>(nullable: true),
                    IdThamChieu = table.Column<string>(nullable: true),
                    IdThongDiepGoc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongDiepChungs", x => x.ThongDiepChungId);
                    table.ForeignKey(
                        name: "FK_ThongDiepChungs_TaiLieuDinhKems_TaiLieuDinhKemId",
                        column: x => x.TaiLieuDinhKemId,
                        principalTable: "TaiLieuDinhKems",
                        principalColumn: "TaiLieuDinhKemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepChungs_TaiLieuDinhKemId",
                table: "ThongDiepChungs",
                column: "TaiLieuDinhKemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongDiepChungs");
        }
    }
}
