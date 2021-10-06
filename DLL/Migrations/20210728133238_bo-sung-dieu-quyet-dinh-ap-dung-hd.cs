using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Bosungdieuquyetdinhapdunghd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Dieu1",
                table: "QuyetDinhApDungHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dieu2",
                table: "QuyetDinhApDungHoaDons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dieu1",
                table: "QuyetDinhApDungHoaDons");

            migrationBuilder.DropColumn(
                name: "Dieu2",
                table: "QuyetDinhApDungHoaDons");
        }
    }
}
