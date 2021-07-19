using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addfunctiontinhhinhsudunghoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Functions",
                columns: new string[] { "FunctionId", "FunctionName", "Title", "SubTitle", "CreatedDate", "Status", "Type", "STT" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "TinhHinhSuDungHoaDon",
                        "Hóa đơn",
                        "Tình hình sử dụng hóa đơn",
                        DateTime.Now,
                        true,
                        "Hóa đơn",
                        25
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
