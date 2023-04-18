using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addeditthaotac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ThaoTacs
                SET STT=9
                WHERE Ma = 'HD_DOWNLOAD';

                UPDATE ThaoTacs
                SET STT=10
                WHERE Ma = 'HD_CONVERT';

                UPDATE ThaoTacs
                SET STT=11
                WHERE Ma = 'HD_DETAIL';

                UPDATE ThaoTacs
                SET STT=12
                WHERE Ma = 'HD_SEND';

                UPDATE ThaoTacs
                SET STT=13
                WHERE Ma = 'HD_CRASH';

                UPDATE ThaoTacs
                SET STT=14
                WHERE Ma = 'HD_REPLACE';

                UPDATE ThaoTacs
                SET STT=15
                WHERE Ma = 'HD_ADJUST';
            ");
            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                {
                    Guid.NewGuid().ToString(),
                    "HD_SENDCQT",
                    "Gửi hóa đơn đến cơ quan thuế",
                    8
                }
                });
            migrationBuilder.Sql(@"INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
            SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
            FROM (SELECT * FROM Roles) r
            CROSS JOIN Functions f
            CROSS JOIN ThaoTacs tt
            WHERE (f.Type = N'Hóa đơn' and tt.Ma = N'HD_SENDCQT')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
