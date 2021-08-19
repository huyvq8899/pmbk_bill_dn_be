using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addtablethietlaptruongdlmorong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HienThi",
                table: "TruongDuLieuMoRongs");

            migrationBuilder.CreateTable(
                name: "ThietLapTruongDuLieuMoRongs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TenTruong = table.Column<string>(nullable: true),
                    TenTruongHienThi = table.Column<string>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true),
                    HienThi = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietLapTruongDuLieuMoRongs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ThietLapTruongDuLieuMoRongs",
                columns: new string[] { "Id", "TenTruong", "TenTruongHienThi", "GhiChu", "HienThi" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 1",
                        "Trường thông tin bổ sung 1",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 2",
                        "Trường thông tin bổ sung 2",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 3",
                        "Trường thông tin bổ sung 3",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 4",
                        "Trường thông tin bổ sung 4",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 5",
                        "Trường thông tin bổ sung 5",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 6",
                        "Trường thông tin bổ sung 6",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 7",
                        "Trường thông tin bổ sung 7",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 8",
                        "Trường thông tin bổ sung 8",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 9",
                        "Trường thông tin bổ sung 9",
                        string.Empty,
                        true
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 10",
                        "Trường thông tin bổ sung 10",
                        string.Empty,
                        true
                    },

                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThietLapTruongDuLieuMoRongs");

            migrationBuilder.AddColumn<bool>(
                name: "HienThi",
                table: "TruongDuLieuMoRongs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("Delete from ThietLapTruongDuLieuMoRongs");
        }
    }
}
