using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class AddLogTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileDatas",
                columns: table => new
                {
                    FileDataId = table.Column<string>(maxLength: 36, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Binary = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDatas", x => x.FileDataId);
                });

            migrationBuilder.CreateTable(
                name: "TransferLogs",
                columns: table => new
                {
                    TransferLogId = table.Column<string>(maxLength: 36, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    MLTDiep = table.Column<int>(nullable: false),
                    MTDiep = table.Column<string>(nullable: true),
                    MTDTChieu = table.Column<string>(nullable: true),
                    XMLData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferLogs", x => x.TransferLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDatas");

            migrationBuilder.DropTable(
                name: "TransferLogs");
        }
    }
}
