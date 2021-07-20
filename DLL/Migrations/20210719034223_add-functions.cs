using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addfunctions : Migration
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
                        "HinhThucThanhToan",
                        "Danh mục",
                        "Hình thức thanh toán",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        24
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
