using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class updatesttthaotac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ThaoTacs
                SET STT=14
                WHERE Ma = 'HD_CRASH';

                UPDATE ThaoTacs
                SET STT=15
                WHERE Ma = 'HD_REPLACE';

                UPDATE ThaoTacs
                SET STT=16
                WHERE Ma = 'HD_ADJUST';
            ");

            migrationBuilder.Sql(@"
                UPDATE ThaoTacs
                SET STT=9
                WHERE Ma = 'PXK_DOWNLOAD';

                UPDATE ThaoTacs
                SET STT=10
                WHERE Ma = 'PXK_CONVERT';

                UPDATE ThaoTacs
                SET STT=11
                WHERE Ma = 'PXK_DETAIL';

                UPDATE ThaoTacs
                SET STT=12
                WHERE Ma = 'PXK_SEND';

                UPDATE ThaoTacs
                SET STT=13
                WHERE Ma = 'PXK_FAILURE_NOTIFICATION';

                UPDATE ThaoTacs
                SET STT=14
                WHERE Ma = 'PXK_CRASH';

                UPDATE ThaoTacs
                SET STT=15
                WHERE Ma = 'PXK_REPLACE';

                UPDATE ThaoTacs
                SET STT=16
                WHERE Ma = 'PXK_ADJUST';
            ");


            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "HD_FAILURE_NOTIFICATION",
                        "Thông báo sai sót không phải lập lại hóa đơn",
                        13
                    },

                    {
                        Guid.NewGuid().ToString(),
                        "PXK_SENDCQT",
                        "Gửi phiếu xuất kho đến cơ quan thuế",
                        13
                    }
                });

            migrationBuilder.Sql(@"INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
            SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
            FROM (SELECT * FROM Roles) r
            CROSS JOIN Functions f
            CROSS JOIN ThaoTacs tt
            WHERE (f.Type = N'Hóa đơn' and tt.Ma = N'HD_FAILURE_NOTIFICATION')
            OR (f.Type=N'Phiếu xuất kho' and tt.Ma = N'PXK_SENDCQT')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
