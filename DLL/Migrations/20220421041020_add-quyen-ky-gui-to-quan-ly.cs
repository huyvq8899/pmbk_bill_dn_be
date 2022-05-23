using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addquyenkyguitoquanly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE Function_ThaoTacs WHERE ThaoTacId IN
                                    (Select ThaoTacId From ThaoTacs Where Ma = 'MNG_SEND')");


            migrationBuilder.Sql(@"DELETE ThaoTacs WHERE Ma = 'MNG_SEND'");
            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "MNG_SEND",
                        "Ký và gửi",
                        8
                    },
            });

            migrationBuilder.Sql(@"
            INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
                        SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
                        FROM Roles r
                        CROSS JOIN Functions f
                        CROSS JOIN ThaoTacs tt
                        WHERE  (f.Type = N'Quản lý' and tt.Ma = N'MNG_SEND')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
