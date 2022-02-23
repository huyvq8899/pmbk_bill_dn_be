using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Addcolandtableforbokyhieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaSoThueBenUyNhiem",
                table: "BoKyHieuHoaDons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MauHoaDonXacThucs",
                columns: table => new
                {
                    MauHoaDonXacThucId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NhatKyXacThucBoKyHieuId = table.Column<string>(nullable: true),
                    FileByte = table.Column<byte[]>(nullable: true),
                    FileType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDonXacThucs", x => x.MauHoaDonXacThucId);
                    table.ForeignKey(
                        name: "FK_MauHoaDonXacThucs_NhatKyXacThucBoKyHieus_NhatKyXacThucBoKyHieuId",
                        column: x => x.NhatKyXacThucBoKyHieuId,
                        principalTable: "NhatKyXacThucBoKyHieus",
                        principalColumn: "NhatKyXacThucBoKyHieuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDonXacThucs_NhatKyXacThucBoKyHieuId",
                table: "MauHoaDonXacThucs",
                column: "NhatKyXacThucBoKyHieuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MauHoaDonXacThucs");

            migrationBuilder.DropColumn(
                name: "MaSoThueBenUyNhiem",
                table: "BoKyHieuHoaDons");
        }
    }
}
