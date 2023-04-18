using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addmauhoadontuychinhchitiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MauHoaDonTuyChinhChiTiets",
                columns: table => new
                {
                    MauHoaDonTuyChinhChiTietId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true),
                    TuyChonChiTiet = table.Column<string>(nullable: true),
                    TenTiengAnh = table.Column<string>(nullable: true),
                    KieuDuLieuThietLap = table.Column<int>(nullable: false),
                    Loai = table.Column<int>(nullable: false),
                    LoaiChiTiet = table.Column<int>(nullable: false),
                    LoaiContainer = table.Column<int>(nullable: false),
                    IsParent = table.Column<bool>(nullable: true),
                    Checked = table.Column<bool>(nullable: true),
                    Disabled = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDonTuyChinhChiTiets", x => x.MauHoaDonTuyChinhChiTietId);
                    table.ForeignKey(
                        name: "FK_MauHoaDonTuyChinhChiTiets_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDonTuyChinhChiTiets_MauHoaDonId",
                table: "MauHoaDonTuyChinhChiTiets",
                column: "MauHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MauHoaDonTuyChinhChiTiets");
        }
    }
}
