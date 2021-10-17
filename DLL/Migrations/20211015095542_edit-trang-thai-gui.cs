using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class edittrangthaigui : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MLTDiep",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "MNGui",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "MNNhan",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "MST",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "MTDTChieu",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "MTDiep",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.DropColumn(
                name: "SLuong",
                table: "TrangThaiGuiToKhais");

            migrationBuilder.RenameColumn(
                name: "ThoiGianGui",
                table: "TrangThaiGuiToKhais",
                newName: "NgayGioGui");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NgayGioGui",
                table: "TrangThaiGuiToKhais",
                newName: "ThoiGianGui");

            migrationBuilder.AddColumn<string>(
                name: "MLTDiep",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MNGui",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MNNhan",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MST",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MTDTChieu",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MTDiep",
                table: "TrangThaiGuiToKhais",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SLuong",
                table: "TrangThaiGuiToKhais",
                nullable: false,
                defaultValue: 0);
        }
    }
}
