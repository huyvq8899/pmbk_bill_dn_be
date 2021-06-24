using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class removeduplicatecolinchitiethd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThueGTGT",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TyLeChietKhau",
                table: "HoaDonDienTuChiTiets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ThueGTGT",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TyLeChietKhau",
                table: "HoaDonDienTuChiTiets",
                nullable: true);
        }
    }
}
