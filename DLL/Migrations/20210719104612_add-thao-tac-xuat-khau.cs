﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addthaotacxuatkhau : Migration
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
                        "DM_EXPORT",
                        "Xuất khẩu",
                        6
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "BC_EXPORT",
                        "Xuất khẩu",
                        5
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TOOL_EXPORT",
                        "Xuất khẩu",
                        2
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
