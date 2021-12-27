using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addquyenxacthucsudung : Migration
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
                    "MNG_CONFIRM",
                    "Xác thực sử dụng",
                    7
                },
           });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
