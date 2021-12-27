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

            migrationBuilder.Sql(@"
            INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
                        SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
                        FROM Roles r
                        CROSS JOIN Functions f
                        CROSS JOIN ThaoTacs tt
                        WHERE  (f.Type = N'Quản lý' and tt.Ma = N'MNG_CONFIRM')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
