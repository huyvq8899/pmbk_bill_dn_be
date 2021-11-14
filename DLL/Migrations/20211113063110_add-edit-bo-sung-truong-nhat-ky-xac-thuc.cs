using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addeditbosungtruongnhatkyxacthuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHetSoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHetSoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus");

            migrationBuilder.DropColumn(
                name: "SoLuongHoaDon",
                table: "NhatKyXacThucBoKyHieus");
        }
    }
}
