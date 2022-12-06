using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addphanquyenticket : Migration
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
                    "PhieuXuatKho",
                    null,
                    "Phiếu xuất kho",
                    DateTime.Now,
                    true,
                    "Phiếu xuất kho",
                    26
                  },
                  {
                    Guid.NewGuid().ToString(),
                    "Ve",
                    null,
                    "Vé",
                    DateTime.Now,
                    true,
                    "Vé",
                    27
                  },
              });

            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_FULL",
                        "Toàn quyền",
                        1
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_VIEW",
                        "Xem",
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_CREATE",
                        "Thêm/Nhân bản/Nhập khẩu",
                        3
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_UPDATE",
                        "Sửa",
                        4
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_DELETE",
                        "Xóa",
                        5
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_EXPORT",
                        "Xuất khẩu",
                        6
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_PUBLISH",
                        "Phát hành phiếu xuất kho",
                        7
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_DOWNLOAD",
                        "Tải phiếu xuất kho",
                        8
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_CONVERT",
                        "Chuyển thành phiếu xuất kho giấy",
                        9
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_DETAIL",
                        "Xuất khẩu chi tiết phiếu xuất kho",
                        10
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_SEND",
                        "Gửi phiếu xuất kho cho khách hàng/Gửi phiếu xuất kho nháp cho khách hàng",
                        11
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_FAILURE_NOTIFICATION",
                        "Thông báo sai sót không phải lập lại phiếu xuất kho",
                        12
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_CRASH",
                        "Xóa bỏ phiếu xuất kho",
                        13
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_REPLACE",
                        "Lập phiếu xuất kho thay thế",
                        14
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "PXK_ADJUST",
                        "Lập phiếu xuất kho điều chỉnh",
                        15
                    },
                    ////////////////////////////////////////////////////
                    {
                        Guid.NewGuid().ToString(),
                        "TK_FULL",
                        "Toàn quyền",
                        1
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_VIEW",
                        "Xem",
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_CREATE",
                        "Thêm/Nhân bản/Nhập khẩu",
                        3
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_UPDATE",
                        "Sửa",
                        4
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_DELETE",
                        "Xóa",
                        5
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_EXPORT",
                        "Xuất khẩu",
                        6
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_PUBLISH",
                        "Xuất vé",
                        7
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_DOWNLOAD",
                        "Tải vé",
                        8
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_CONVERT",
                        "Chuyển thành vé giấy",
                        9
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_DETAIL",
                        "Xuất khẩu chi tiết vé",
                        10
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_SEND",
                        "Gửi vé cho khách hàng",
                        11
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_FAILURE_NOTIFICATION",
                        "Thông báo sai sót không phải lập lại vé",
                        12
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_CRASH",
                        "Xóa bỏ vé",
                        13
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_REPLACE",
                        "Lập vé thay thế",
                        14
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "TK_ADJUST",
                        "Lập vé điều chỉnh",
                        15
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
                OR (f.Type = N'Vé' and tt.Ma Like N'TK%')"
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
                      OR (f.Type = N'Vé' and tt.Ma Like N'TK%')
                      END"
             );

            /// Thêm function phiếu xuất kho => update lại stt
            migrationBuilder.Sql(" Update Functions set STT = 28 where FunctionName = 'BaoCao' ");
            migrationBuilder.Sql(" Update Functions set STT = 29 where FunctionName = 'TienIch' ");
            migrationBuilder.Sql(" Update Functions set STT = 30 where FunctionName = 'NhatKyGuiEmail'");
            migrationBuilder.Sql(" Update Functions set STT = 31 where FunctionName = 'NhatKyTruyCap'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
