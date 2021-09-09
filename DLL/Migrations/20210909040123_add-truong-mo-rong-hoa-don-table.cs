using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtruongmoronghoadontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoRong",
                table: "MauHoaDonTuyChinhChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiaChiGiaoHang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiHanThanhToan",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TruongMoRongHoaDons",
                columns: table => new
                {
                    TruongMoRongHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    MauHoaDonTuyChinhChiTietId = table.Column<string>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongMoRongHoaDons", x => x.TruongMoRongHoaDonId);
                    table.ForeignKey(
                        name: "FK_TruongMoRongHoaDons_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TruongMoRongHoaDons_HoaDonDienTuId",
                table: "TruongMoRongHoaDons",
                column: "HoaDonDienTuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TruongMoRongHoaDons");

            migrationBuilder.DropColumn(
                name: "DoRong",
                table: "MauHoaDonTuyChinhChiTiets");

            migrationBuilder.DropColumn(
                name: "DiaChiGiaoHang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "ThoiHanThanhToan",
                table: "HoaDonDienTus");
        }
    }
}
