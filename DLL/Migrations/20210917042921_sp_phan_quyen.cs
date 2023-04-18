using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class sp_phan_quyen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE [dbo].[Functions]
                                    SET [ParentFunctionId] = (SELECT top(1) FunctionId
                                                                FROM Functions as f   
                                   WHERE (f.SubTitle = [dbo].[Functions].Title and[dbo].[Functions].Title is not null and (f.Title is null or f.Title<> f.SubTitle)) and[dbo].[Functions].FunctionId != f.FunctionId)");
            migrationBuilder.Sql(
                                @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'  AND TABLE_NAME = 'Function_ThaoTacs1')
								BEGIN
								CREATE TABLE [dbo].[Function_ThaoTacs1](
                                	[FTID] [nvarchar](450) NOT NULL,
                                	[FunctionId] [nvarchar](36) NULL,
                                	[ThaoTacId] [nvarchar](450) NULL, 
                                	[PermissionId] [nvarchar](36) NULL,
                                	[Active] [bit] NOT NULL,
                                	[RoleId] [nvarchar](max) NULL) 
								END"
                                );

            migrationBuilder.Sql(@"DELETE Function_ThaoTacs1;
                                 INSERT INTO Function_ThaoTacs1(FTID, FunctionId, ThaoTacId, PermissionId, Active, RoleId)
                                 SELECT [FTID]
                                       ,[FunctionId] 
                                	    ,[ThaoTacId]
                                		,[PermissionId]
                                       ,[Active]     
                                		,[RoleId]	
                                 FROM [dbo].[Function_ThaoTacs]");

            migrationBuilder.Sql(
                @"DELETE Function_ThaoTacs;

                INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
                SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
                FROM Roles r
                CROSS JOIN Functions f
                CROSS JOIN ThaoTacs tt
                WHERE (f.Type = N'Hóa đơn' and tt.Ma Like N'HD%')
                OR (f.Type = N'Hệ thống' and tt.Ma like N'SYS%')
                OR (f.Type = N'Báo cáo' and tt.Ma like N'BC%')
                OR (f.Type = N'Tiện ích' and tt.Ma like N'TOOL%')
                OR (f.Type = N'Danh mục' and tt.Ma Like N'DM%')"
                );

            migrationBuilder.Sql(@"UPDATE Function_ThaoTacs SET Active =  CASE WHEN (SELECT Top(1) Active FROM Function_ThaoTacs1 f WHERE f.FunctionId = FunctionId and f.ThaoTacId = ThaoTacId and f.PermissionId = PermissionId and f.RoleId = RoleId) IS NULL THEN 0
																		 WHEN (SELECT Top(1) Active FROM Function_ThaoTacs1 f WHERE f.FunctionId = FunctionId and f.ThaoTacId = ThaoTacId and f.PermissionId = PermissionId and f.RoleId = RoleId) IS NOT NULL THEN (SELECT Top(1) Active FROM Function_ThaoTacs1 f WHERE f.FunctionId = FunctionId and f.ThaoTacId = ThaoTacId and f.PermissionId = PermissionId and f.RoleId = RoleId)
																		 END");
            migrationBuilder.Sql("DROP TABLE Function_ThaoTacs1");

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
                OR (f.Type = N'Báo cáo' and tt.Ma like N'BC%')
                OR (f.Type = N'Tiện ích' and tt.Ma like N'TOOL%')
                OR (f.Type = N'Danh mục' and tt.Ma Like N'DM%')

                END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
