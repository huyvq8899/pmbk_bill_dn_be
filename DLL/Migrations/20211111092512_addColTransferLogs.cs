using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addColTransferLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MNGui",
                table: "TransferLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MNNhan",
                table: "TransferLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MNGui",
                table: "TransferLogs");

            migrationBuilder.DropColumn(
                name: "MNNhan",
                table: "TransferLogs");
        }
    }
}
