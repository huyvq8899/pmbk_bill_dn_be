using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class AddDataPhanQuyenThongTinHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
              table: "Functions",
              columns: new string[] { "FunctionId", "FunctionName", "Title", "SubTitle", "CreatedDate", "Status", "Type", "STT", "ParentFunctionId" },
              values: new object[,]
              {
                {
                    Guid.NewGuid().ToString(),
                    "ThongTinHoaDon",
                    "Quản lý",
                    "Thông tin hóa đơn",
                    DateTime.Now,
                    true,
                    "Quản lý",
                    17,
                    "d2a0d7df-a832-4956-8e06-6f85b049ac9b"
                },
              });
            migrationBuilder.Sql(
                @"DELETE Function_ThaoTacs;

                INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
                SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
                FROM Roles r
                CROSS JOIN Functions f
                CROSS JOIN ThaoTacs tt
                WHERE (f.Type = N'Hóa đơn' and tt.Ma Like N'HD%')
                OR (f.Type = N'Hệ thống' and tt.Ma like N'SYS%')
				OR (f.Type = N'Quản lý' and tt.Ma like N'MNG%')
                OR (f.Type = N'Báo cáo' and tt.Ma like N'BC%')
                OR (f.Type = N'Tiện ích' and tt.Ma like N'TOOL%')
                OR (f.Type = N'Danh mục' and tt.Ma Like N'DM%')
                OR (f.Type = N'Phiếu xuất kho' and tt.Ma Like N'PXK%')
                OR (f.Type = N'Tiện ích' and tt.Ma Like N'NhacViec%')"
                );

            migrationBuilder.Sql(
                 @"ALTER PROCEDURE [dbo].[exec_afterThemVaiTro](
	               @RoleId NVARCHAR(36)
                      )
                      AS
                      BEGIN
                      INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
                      SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
                      FROM (SELECT * FROM Roles WHERE RoleId = @RoleId) r
                      CROSS JOIN Functions f
                      CROSS JOIN ThaoTacs tt
                      WHERE (f.Type = N'Hóa đơn' and tt.Ma Like N'HD%')
                      OR (f.Type = N'Hệ thống' and tt.Ma like N'SYS%')
                      OR (f.Type = N'Quản lý' and tt.Ma like N'MNG%')
                      OR (f.Type = N'Báo cáo' and tt.Ma like N'BC%')
                      OR (f.Type = N'Tiện ích' and tt.Ma like N'TOOL%')
                      OR (f.Type = N'Danh mục' and tt.Ma Like N'DM%')
                      OR (f.Type = N'Phiếu xuất kho' and tt.Ma Like N'PXK%')
                      OR (f.Type = N'Tiện ích' and tt.Ma Like N'NhacViec%')
                      END"
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
