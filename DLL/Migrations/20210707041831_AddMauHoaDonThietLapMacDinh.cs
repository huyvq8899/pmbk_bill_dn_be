using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddMauHoaDonThietLapMacDinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MauHoaDonThietLapMacDinhs",
                columns: table => new
                {
                    MauHoaDonThietLapMacDinhId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true),
                    Top = table.Column<string>(nullable: true),
                    Left = table.Column<string>(nullable: true),
                    Width = table.Column<string>(nullable: true),
                    Height = table.Column<string>(nullable: true),
                    Opacity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDonThietLapMacDinhs", x => x.MauHoaDonThietLapMacDinhId);
                    table.ForeignKey(
                        name: "FK_MauHoaDonThietLapMacDinhs_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDonThietLapMacDinhs_MauHoaDonId",
                table: "MauHoaDonThietLapMacDinhs",
                column: "MauHoaDonId",
                unique: true,
                filter: "[MauHoaDonId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MauHoaDonThietLapMacDinhs");
        }
    }
}
