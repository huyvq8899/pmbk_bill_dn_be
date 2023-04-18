using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Add_quy_dinh_ky_thuat_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToKhaiDangKyThongTins",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, maxLength: 36),
                    FileXMLChuaKy = table.Column<string>(nullable: true),
                    ContentXMLChuaKy = table.Column<byte[]>(nullable: true),
                    SignedStatus = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToKhaiDangKyThongTins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuLieuKyToKhais",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdToKhai = table.Column<string>(nullable: true, maxLength: 36),
                    ToKhaiDangKyThongTinId = table.Column<string>(nullable: true, maxLength: 36),
                    FileXMLDaKy = table.Column<string>(nullable: true),
                    NoiDungKy = table.Column<byte[]>(nullable: true),
                    MST = table.Column<string>(nullable: true),
                    Seri = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuLieuKyToKhais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuLieuKyToKhais_ToKhaiDangKyThongTins_ToKhaiDangKyThongTinId",
                        column: x => x.IdToKhai,
                        principalTable: "ToKhaiDangKyThongTins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiGuiToKhais",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdToKhai = table.Column<string>(nullable: true, maxLength: 36),
                    ToKhaiDangKyThongTinId = table.Column<string>(nullable: true, maxLength: 36),
                    MNGui = table.Column<string>(nullable: true),
                    MNNhan = table.Column<string>(nullable: true),
                    MLTDiep = table.Column<string>(nullable: true),
                    MTDiep = table.Column<string>(nullable: true),
                    MTDTChieu = table.Column<string>(nullable: true),
                    MST = table.Column<string>(nullable: true),
                    SLuong = table.Column<int>(nullable: false),
                    TrangThaiGui = table.Column<int>(nullable: false),
                    TrangThaiTiepNhan = table.Column<int>(nullable: false),
                    FileXMLGui = table.Column<string>(nullable: true),
                    NoiDungFileGui = table.Column<byte[]>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiGuiToKhais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrangThaiGuiToKhais_ToKhaiDangKyThongTins_ToKhaiDangKyThongTinId",
                        column: x => x.IdToKhai,
                        principalTable: "ToKhaiDangKyThongTins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuLieuKyToKhais_ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais",
                column: "IdToKhai");

            migrationBuilder.CreateIndex(
                name: "IX_TrangThaiGuiToKhais_ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais",
                column: "IdToKhai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuLieuKyToKhais");

            migrationBuilder.DropTable(
                name: "TrangThaiGuiToKhais");

            migrationBuilder.DropTable(
                name: "ToKhaiDangKyThongTins");
        }
    }
}
