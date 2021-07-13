using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddTaiLieuDinhKem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaiLieuDinhKem",
                table: "ThongBaoKetQuaHuyHoaDons");

            migrationBuilder.DropColumn(
                name: "TaiLieuDinhKem",
                table: "ThongBaoDieuChinhThongTinHoaDons");

            migrationBuilder.CreateTable(
                name: "TaiLieuDinhKems",
                columns: table => new
                {
                    TaiLieuDinhKemId = table.Column<string>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NghiepVuId = table.Column<string>(nullable: true),
                    LoaiNghiepVu = table.Column<int>(nullable: false),
                    TenGoc = table.Column<string>(nullable: true),
                    TenGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiLieuDinhKems", x => x.TaiLieuDinhKemId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaiLieuDinhKems");

            migrationBuilder.AddColumn<string>(
                name: "TaiLieuDinhKem",
                table: "ThongBaoKetQuaHuyHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaiLieuDinhKem",
                table: "ThongBaoDieuChinhThongTinHoaDons",
                nullable: true);
        }
    }
}
