using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class renameandaddcolquyetdinhapdunghd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasChungTuSo",
                table: "QuyetDinhApDungHoaDons",
                newName: "HasChungThuSo");

            migrationBuilder.AddColumn<bool>(
                name: "Disabled",
                table: "QuyetDinhApDungHoaDonDieu1s",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disabled",
                table: "QuyetDinhApDungHoaDonDieu1s");

            migrationBuilder.RenameColumn(
                name: "HasChungThuSo",
                table: "QuyetDinhApDungHoaDons",
                newName: "HasChungTuSo");
        }
    }
}
