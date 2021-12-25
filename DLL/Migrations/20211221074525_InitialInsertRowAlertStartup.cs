using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class InitialInsertRowAlertStartup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AlertStartups",
                columns: new string[] { "Id", "Title", "SubTitle", "Link", "Content", "Status", "CreatedDate", "CreatedBy", "ModifyDate", "ModifyBy" },
                values: new object[,]
                {
                    {
                       Guid.NewGuid().ToString(),
                        "Khuyến nghị người nộp thuế thực hiện đúng quy định",
                        "",
                        "",
                        "",
                        true,
                        DateTime.Now,
                        Guid.NewGuid().ToString(),
                        DateTime.Now,
                        Guid.NewGuid().ToString()
                    }
                });
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
