using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addphuongthucchuyendlnhatkyxacthuc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhuongThucChuyenDL",
                table: "NhatKyXacThucBoKyHieus",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhuongThucChuyenDL",
                table: "NhatKyXacThucBoKyHieus");
        }
    }
}
