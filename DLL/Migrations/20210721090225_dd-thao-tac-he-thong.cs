using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class ddthaotachethong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                                {
                                    Guid.NewGuid().ToString(),
                                    "SYS_FULL",
                                    "Toàn quyền",
                                    1
                                },
                                {
                                    Guid.NewGuid().ToString(),
                                    "SYS_VIEW",
                                    "Xem",
                                    2
                                },
                                {
                                    Guid.NewGuid().ToString(),
                                    "SYS_UPDATE",
                                    "Sửa",
                                    3
                                },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
