using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class editforeignkeytk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuLieuKyToKhais_ToKhaiDangKyThongTins_ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais");

            migrationBuilder.DropForeignKey(
                name: "FK_TrangThaiGuiToKhais_ToKhaiDangKyThongTins_ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropIndex(
                name: "IX_TrangThaiGuiToKhais_ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropIndex(
                name: "IX_DuLieuKyToKhais_ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais");

            migrationBuilder.DropColumn(
                name: "ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrangThaiGuiToKhais_ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais",
                column: "ToKhaiDangKyThongTinId");

            migrationBuilder.CreateIndex(
                name: "IX_DuLieuKyToKhais_ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais",
                column: "ToKhaiDangKyThongTinId");

            migrationBuilder.AddForeignKey(
                name: "FK_DuLieuKyToKhais_ToKhaiDangKyThongTins_ToKhaiDangKyThongTinId",
                table: "DuLieuKyToKhais",
                column: "ToKhaiDangKyThongTinId",
                principalTable: "ToKhaiDangKyThongTins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrangThaiGuiToKhais_ToKhaiDangKyThongTins_ToKhaiDangKyThongTinId",
                table: "TrangThaiGuiToKhais",
                column: "ToKhaiDangKyThongTinId",
                principalTable: "ToKhaiDangKyThongTins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
