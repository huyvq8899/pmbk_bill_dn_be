using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class alterexecthemvaitro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"IF EXISTS (SELECT * FROM sys.objects WHERE type IN (N'P', N'PC' )  AND object_id = OBJECT_ID(N'[dbo].[exec_afterThemVaiTro]'))
							        BEGIN
								        DROP PROCEDURE [dbo].[exec_afterThemVaiTro]
							        END");

            migrationBuilder.Sql(
                @"CREATE PROCEDURE [dbo].[exec_afterThemVaiTro](
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
                END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
