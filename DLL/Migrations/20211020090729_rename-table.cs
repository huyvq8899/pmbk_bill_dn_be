using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class renametable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_HoaDonDienTus_HoaDonDienTuId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongDiepGuiDuLieuHDDTs",
                table: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongDiepGuiDuLieuHDDTChiTiets",
                table: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.RenameTable(
                name: "ThongDiepGuiDuLieuHDDTs",
                newName: "DuLieuGuiHDDTs");

            migrationBuilder.RenameTable(
                name: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "DuLieuGuiHDDTChiTiets");

            migrationBuilder.RenameIndex(
                name: "IX_ThongDiepGuiDuLieuHDDTChiTiets_HoaDonDienTuId",
                table: "DuLieuGuiHDDTChiTiets",
                newName: "IX_DuLieuGuiHDDTChiTiets_HoaDonDienTuId");

            migrationBuilder.RenameIndex(
                name: "IX_ThongDiepGuiDuLieuHDDTChiTiets_DuLieuGuiHDDTId",
                table: "DuLieuGuiHDDTChiTiets",
                newName: "IX_DuLieuGuiHDDTChiTiets_DuLieuGuiHDDTId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DuLieuGuiHDDTs",
                table: "DuLieuGuiHDDTs",
                column: "DuLieuGuiHDDTId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DuLieuGuiHDDTChiTiets",
                table: "DuLieuGuiHDDTChiTiets",
                column: "DuLieuGuiHDDTChiTietId");

            migrationBuilder.AddForeignKey(
                name: "FK_DuLieuGuiHDDTChiTiets_DuLieuGuiHDDTs_DuLieuGuiHDDTId",
                table: "DuLieuGuiHDDTChiTiets",
                column: "DuLieuGuiHDDTId",
                principalTable: "DuLieuGuiHDDTs",
                principalColumn: "DuLieuGuiHDDTId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DuLieuGuiHDDTChiTiets_HoaDonDienTus_HoaDonDienTuId",
                table: "DuLieuGuiHDDTChiTiets",
                column: "HoaDonDienTuId",
                principalTable: "HoaDonDienTus",
                principalColumn: "HoaDonDienTuId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuLieuGuiHDDTChiTiets_DuLieuGuiHDDTs_DuLieuGuiHDDTId",
                table: "DuLieuGuiHDDTChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_DuLieuGuiHDDTChiTiets_HoaDonDienTus_HoaDonDienTuId",
                table: "DuLieuGuiHDDTChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DuLieuGuiHDDTs",
                table: "DuLieuGuiHDDTs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DuLieuGuiHDDTChiTiets",
                table: "DuLieuGuiHDDTChiTiets");

            migrationBuilder.RenameTable(
                name: "DuLieuGuiHDDTs",
                newName: "ThongDiepGuiDuLieuHDDTs");

            migrationBuilder.RenameTable(
                name: "DuLieuGuiHDDTChiTiets",
                newName: "ThongDiepGuiDuLieuHDDTChiTiets");

            migrationBuilder.RenameIndex(
                name: "IX_DuLieuGuiHDDTChiTiets_HoaDonDienTuId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "IX_ThongDiepGuiDuLieuHDDTChiTiets_HoaDonDienTuId");

            migrationBuilder.RenameIndex(
                name: "IX_DuLieuGuiHDDTChiTiets_DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                newName: "IX_ThongDiepGuiDuLieuHDDTChiTiets_DuLieuGuiHDDTId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongDiepGuiDuLieuHDDTs",
                table: "ThongDiepGuiDuLieuHDDTs",
                column: "DuLieuGuiHDDTId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongDiepGuiDuLieuHDDTChiTiets",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "DuLieuGuiHDDTChiTietId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_ThongDiepGuiDuLieuHDDTs_DuLieuGuiHDDTId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "DuLieuGuiHDDTId",
                principalTable: "ThongDiepGuiDuLieuHDDTs",
                principalColumn: "DuLieuGuiHDDTId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThongDiepGuiDuLieuHDDTChiTiets_HoaDonDienTus_HoaDonDienTuId",
                table: "ThongDiepGuiDuLieuHDDTChiTiets",
                column: "HoaDonDienTuId",
                principalTable: "HoaDonDienTus",
                principalColumn: "HoaDonDienTuId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
