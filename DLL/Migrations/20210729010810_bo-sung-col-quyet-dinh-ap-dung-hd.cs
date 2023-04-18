using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Bosungcolquyetdinhapdunghd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoiDungDieu3",
                table: "QuyetDinhApDungHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoiDungDieu4",
                table: "QuyetDinhApDungHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoiDungDieu3",
                table: "QuyetDinhApDungHoaDons");

            migrationBuilder.DropColumn(
                name: "NoiDungDieu4",
                table: "QuyetDinhApDungHoaDons");
        }
    }
}
