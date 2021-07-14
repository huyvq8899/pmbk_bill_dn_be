using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class renamecolsinhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenMauSo",
                table: "HoaDonDienTus",
                newName: "KyHieu");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KyHieu",
                table: "HoaDonDienTus",
                newName: "TenMauSo");
        }
    }
}
