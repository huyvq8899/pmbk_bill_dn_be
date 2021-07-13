using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddNhatKyTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NhatKyGuiEmails",
                columns: table => new
                {
                    NhatKyGuiEmailId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MauSo = table.Column<string>(nullable: true),
                    KyHieu = table.Column<string>(nullable: true),
                    So = table.Column<string>(nullable: true),
                    Ngay = table.Column<string>(nullable: true),
                    TrangThaiGuiEmail = table.Column<int>(nullable: false),
                    TenNguoiNhan = table.Column<string>(nullable: true),
                    EmailNguoiNhan = table.Column<string>(nullable: true),
                    RefId = table.Column<string>(nullable: true),
                    RefType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyGuiEmails", x => x.NhatKyGuiEmailId);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyTruyCaps",
                columns: table => new
                {
                    NhatKyTruyCapId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    DoiTuongThaoTac = table.Column<string>(nullable: true),
                    HanhDong = table.Column<string>(nullable: true),
                    ThamChieu = table.Column<string>(nullable: true),
                    MoTaChiTiet = table.Column<string>(nullable: true),
                    DiaChiIP = table.Column<string>(nullable: true),
                    TenMayTinh = table.Column<string>(nullable: true),
                    RefFile = table.Column<string>(nullable: true),
                    RefId = table.Column<string>(nullable: true),
                    RefType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyTruyCaps", x => x.NhatKyTruyCapId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NhatKyGuiEmails");

            migrationBuilder.DropTable(
                name: "NhatKyTruyCaps");
        }
    }
}
