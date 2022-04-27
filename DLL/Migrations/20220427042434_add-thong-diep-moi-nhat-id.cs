using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthongdiepmoinhatid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThongDiepMoiNhatId",
                table: "BoKyHieuHoaDons",
                maxLength: 36,
                nullable: true);

            migrationBuilder.Sql("UPDATE BoKyHieuHoaDons SET ThongDiepMoiNhatId = ThongDiepId WHERE ThongDiepMoiNhatId IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThongDiepMoiNhatId",
                table: "BoKyHieuHoaDons");
        }
    }
}
