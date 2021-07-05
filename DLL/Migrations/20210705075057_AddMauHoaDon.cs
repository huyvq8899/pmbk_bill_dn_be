using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddMauHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MauHoaDons",
                columns: table => new
                {
                    MauHoaDonId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    SoThuTu = table.Column<int>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    TenBoMau = table.Column<string>(nullable: true),
                    FileMau = table.Column<string>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    LoaiMauHoaDon = table.Column<int>(nullable: false),
                    LoaiThueGTGT = table.Column<int>(nullable: false),
                    LoaiNgonNgu = table.Column<int>(nullable: false),
                    LoaiKhoGiay = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauHoaDons", x => x.MauHoaDonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MauHoaDons");
        }
    }
}
