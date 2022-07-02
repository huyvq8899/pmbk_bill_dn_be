using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addmaloaitiencolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaLoaiTien",
                table: "HoaDonDienTus",
                maxLength: 3,
                nullable: true);

            migrationBuilder.Sql("update HoaDonDienTus set MaLoaiTien = (select Ma from LoaiTiens where LoaiTienId = HoaDonDienTus.LoaiTienId)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaLoaiTien",
                table: "HoaDonDienTus");
        }
    }
}
