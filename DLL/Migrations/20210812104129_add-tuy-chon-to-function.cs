using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class Addtuychontofunction : Migration
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
                        "TuyChon",
                        "Hệ thống",
                        "Tùy chọn",
                        DateTime.Now,
                        true,
                        "Hệ thống",
                        21
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "QuyetDinhApDungHoaDon",
                        "Quản lý hóa đơn điện tử",
                        "Quyết định áp dụng hóa đơn",
                        DateTime.Now,
                        true,
                        "Danh mục",
                        22
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
