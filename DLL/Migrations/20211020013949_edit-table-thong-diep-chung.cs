using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class edittablethongdiepchung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThongDiepChungs_TaiLieuDinhKems_TaiLieuDinhKemId",
                table: "ThongDiepChungs");

            migrationBuilder.DropIndex(
                name: "IX_ThongDiepChungs_TaiLieuDinhKemId",
                table: "ThongDiepChungs");

            migrationBuilder.DropColumn(
                name: "TaiLieuDinhKemId",
                table: "ThongDiepChungs");

            migrationBuilder.AddColumn<string>(
                name: "MaThongDiep",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaLoaiThongDiep",
                table: "ThongDiepChungs",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaThongDiep",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.AlterColumn<string>(
                name: "MaLoaiThongDiep",
                table: "ThongDiepChungs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "TaiLieuDinhKemId",
                table: "ThongDiepChungs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThongDiepChungs_TaiLieuDinhKemId",
                table: "ThongDiepChungs",
                column: "TaiLieuDinhKemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongDiepChungs_TaiLieuDinhKems_TaiLieuDinhKemId",
                table: "ThongDiepChungs",
                column: "TaiLieuDinhKemId",
                principalTable: "TaiLieuDinhKems",
                principalColumn: "TaiLieuDinhKemId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
