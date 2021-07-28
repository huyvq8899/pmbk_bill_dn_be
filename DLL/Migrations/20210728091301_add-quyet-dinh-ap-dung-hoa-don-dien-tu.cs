using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addquyetdinhapdunghoadondientu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuyetDinhApDungHoaDons",
                columns: table => new
                {
                    QuyetDinhApDungHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NguoiDaiDienPhapLuat = table.Column<string>(nullable: true),
                    ChucDanh = table.Column<string>(nullable: true),
                    SoQuyetDinh = table.Column<string>(nullable: true),
                    NgayQuyetDinh = table.Column<DateTime>(nullable: true),
                    CanCuDeBanHanhQuyetDinh = table.Column<string>(nullable: true),
                    HasMayTinh = table.Column<bool>(nullable: true),
                    HasMayIn = table.Column<bool>(nullable: true),
                    HasChungTuSo = table.Column<bool>(nullable: true),
                    Dieu3 = table.Column<string>(nullable: true),
                    Dieu4 = table.Column<string>(nullable: true),
                    Dieu5 = table.Column<int>(nullable: true),
                    NgayHieuLuc = table.Column<DateTime>(nullable: true),
                    CoQuanThue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhApDungHoaDons", x => x.QuyetDinhApDungHoaDonId);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhApDungHoaDonDieu1s",
                columns: table => new
                {
                    QuyetDinhApDungHoaDonDieu1Id = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    QuyetDinhApDungHoaDonId = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true),
                    Checked = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhApDungHoaDonDieu1s", x => x.QuyetDinhApDungHoaDonDieu1Id);
                    table.ForeignKey(
                        name: "FK_QuyetDinhApDungHoaDonDieu1s_QuyetDinhApDungHoaDons_QuyetDinhApDungHoaDonId",
                        column: x => x.QuyetDinhApDungHoaDonId,
                        principalTable: "QuyetDinhApDungHoaDons",
                        principalColumn: "QuyetDinhApDungHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuyetDinhApDungHoaDonDieu2s",
                columns: table => new
                {
                    QuyetDinhApDungHoaDonDieu2Id = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    QuyetDinhApDungHoaDonId = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    MucDichSuDung = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyetDinhApDungHoaDonDieu2s", x => x.QuyetDinhApDungHoaDonDieu2Id);
                    table.ForeignKey(
                        name: "FK_QuyetDinhApDungHoaDonDieu2s_QuyetDinhApDungHoaDons_QuyetDinhApDungHoaDonId",
                        column: x => x.QuyetDinhApDungHoaDonId,
                        principalTable: "QuyetDinhApDungHoaDons",
                        principalColumn: "QuyetDinhApDungHoaDonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuyetDinhApDungHoaDonDieu1s_QuyetDinhApDungHoaDonId",
                table: "QuyetDinhApDungHoaDonDieu1s",
                column: "QuyetDinhApDungHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_QuyetDinhApDungHoaDonDieu2s_QuyetDinhApDungHoaDonId",
                table: "QuyetDinhApDungHoaDonDieu2s",
                column: "QuyetDinhApDungHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuyetDinhApDungHoaDonDieu1s");

            migrationBuilder.DropTable(
                name: "QuyetDinhApDungHoaDonDieu2s");

            migrationBuilder.DropTable(
                name: "QuyetDinhApDungHoaDons");
        }
    }
}
