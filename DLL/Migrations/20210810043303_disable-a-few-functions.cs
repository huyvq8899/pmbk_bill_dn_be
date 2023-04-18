using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class disableafewfunctions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaNhanVien",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NhanVienBanHangId",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenNhanVien",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentFunctionId",
                table: "Functions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Function_ThaoTacs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_UserId",
                table: "Function_ThaoTacs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Function_ThaoTacs_Users_UserId",
                table: "Function_ThaoTacs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Function_ThaoTacs_Users_UserId",
                table: "Function_ThaoTacs");

            migrationBuilder.DropIndex(
                name: "IX_Function_ThaoTacs_UserId",
                table: "Function_ThaoTacs");

            migrationBuilder.DropColumn(
                name: "MaNhanVien",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "NhanVienBanHangId",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TenNhanVien",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "ParentFunctionId",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Function_ThaoTacs");
        }
    }
}
