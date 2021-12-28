using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addresetphanquyen4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete Function_ThaoTacs");

            migrationBuilder.Sql("Delete ThaoTacs");
            migrationBuilder.Sql("Delete Functions");
            migrationBuilder.InsertData(
                table: "Functions",
                columns: new string[] { "FunctionId", "FunctionName", "Title", "SubTitle", "CreatedDate", "Status", "Type", "STT" },
                values: new object[,]
                {
                {
                    Guid.NewGuid().ToString(),
                    "HeThong",
                    null,
                    "Hệ thống",
                    DateTime.Now,
                    true,
                    "Hệ thống",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThongTinNguoiNopThue",
                    "Hệ thống",
                    "Thông tin người nộp thuế",
                    DateTime.Now,
                    true,
                    "Hệ thống",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "TuyChonChung",
                    "Hệ thống",
                    "Tùy chọn chung",
                    DateTime.Now,
                    true,
                    "Hệ thống",
                    3
                },
                {
                    Guid.NewGuid().ToString(),
                    "DinhDangSo",
                    "Hệ thống",
                    "Định dạng số",
                    DateTime.Now,
                    true,
                    "Hệ thống",
                    4
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThongTinTaiNguyen",
                    "Hệ thống",
                    "Thông tin tài nguyên",
                    DateTime.Now,
                    false,
                    "Hệ thống",
                    5
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThietLapCongCuKy",
                    "Hệ thống",
                    "Thiết lập công cụ ký",
                    DateTime.Now,
                    true,
                    "Hệ thống",
                    6
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThietLapEmailGuiHoaDon",
                    "Hệ thống",
                    "Thiết lập email gửi hóa đơn",
                    DateTime.Now,
                    true,
                    "Hệ thống",
                    7
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThietLapSMSGuiHoaDon",
                    "Hệ thống",
                    "Thiết lập SMS gửi hóa đơn",
                    DateTime.Now,
                    false,
                    "Hệ thống",
                    8
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThietLapXacNhanCTS",
                    "Hệ thống",
                    "Thiết lập xác nhận bằng chứng thư số",
                    DateTime.Now,
                    false,
                    "Hệ thống",
                    9
                },
                {
                    Guid.NewGuid().ToString(),
                    "PheDuyetHoaDon",
                    "Hệ thống",
                    "Phê duyệt hóa đơn",
                    DateTime.Now,
                    false,
                    "Hệ thống",
                    10
                },
                {
                    Guid.NewGuid().ToString(),
                    "QuanLy",
                    null,
                    "Quản lý",
                    DateTime.Now,
                    true,
                    "Quản lý",
                    11
                },
                {
                    Guid.NewGuid().ToString(),
                    "MauHoaDon",
                    "Quản lý",
                    "Mẫu hóa đơn",
                    DateTime.Now,
                    true,
                    "Quản lý",
                    12
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThongDiepGui",
                    "Quản lý",
                    "Thông điệp gửi",
                    DateTime.Now,
                    true,
                    "Quản lý",
                    13
                },
                {
                    Guid.NewGuid().ToString(),
                    "ThongDiepNhan",
                    "Quản lý",
                    "Thông điệp nhận",
                    DateTime.Now,
                    true,
                    "Quản lý",
                    14
                },
                {
                    Guid.NewGuid().ToString(),
                    "BoKyHieuHoaDon",
                    "Quản lý",
                    "Bộ ký hiệu hóa đơn",
                    DateTime.Now,
                    true,
                    "Quản lý",
                    15
                },
                {
                    Guid.NewGuid().ToString(),
                    "QuyTrinhPheDuyetHoaDon",
                    "Quản lý",
                    "Quy trình phê duyệt hóa đơn",
                    DateTime.Now,
                    false,
                    "Quản lý",
                    16
                },
                {
                    Guid.NewGuid().ToString(),
                    "DanhMuc",
                    null,
                    "Danh mục",
                    DateTime.Now,
                    true,
                    "Danh mục",
                    16
                },
                {
                    Guid.NewGuid().ToString(),
                    "CoCauToChuc",
                    "Danh mục",
                    "Cơ cấu tổ chức",
                    DateTime.Now,
                    false,
                    "Danh mục",
                    17
                },
                {
                    Guid.NewGuid().ToString(),
                    "ChucDanh",
                    "Danh mục",
                    "Chức danh",
                    DateTime.Now,
                    false,
                    "Danh mục",
                    18
                },
                {
                    Guid.NewGuid().ToString(),
                    "KhachHang",
                    "Danh mục",
                    "Khách hàng",
                    DateTime.Now,
                    true,
                    "Danh mục",
                    19
                },
                {
                    Guid.NewGuid().ToString(),
                    "NhanVien",
                    "Danh mục",
                    "Nhân viên",
                    DateTime.Now,
                    true,
                    "Danh mục",
                    20
                },
                {
                    Guid.NewGuid().ToString(),
                    "HangHoaDichVu",
                    "Danh mục",
                    "Hàng hóa, dịch vụ",
                    DateTime.Now,
                    true,
                    "Danh mục",
                    21
                },
                {
                    Guid.NewGuid().ToString(),
                    "DonViTinh",
                    "Danh mục",
                    "Đơn vị tính",
                    DateTime.Now,
                    true,
                    "Danh mục",
                    22
                },
                {
                    Guid.NewGuid().ToString(),
                    "NhomHangHoaDichVu",
                    "Danh mục",
                    "Nhóm hàng hóa, dịch vụ",
                    DateTime.Now,
                    false,
                    "Danh mục",
                    23
                },
                {
                    Guid.NewGuid().ToString(),
                    "LoaiTien",
                    "Danh mục",
                    "Loại tiền",
                    DateTime.Now,
                    true,
                    "Danh mục",
                    24
                },
                {
                    Guid.NewGuid().ToString(),
                    "HoaDon",
                    null,
                    "Hóa đơn",
                    DateTime.Now,
                    true,
                    "Hóa đơn",
                    25
                },
                {
                    Guid.NewGuid().ToString(),
                    "BaoCao",
                    null,
                    "Báo cáo",
                    DateTime.Now,
                    false,
                    "Báo cáo",
                    26
                },
                {
                    Guid.NewGuid().ToString(),
                    "TienIch",
                    null,
                    "Tiện ích",
                    DateTime.Now,
                    true,
                    "Tiện ích",
                    27
                },
                {
                    Guid.NewGuid().ToString(),
                    "NhatKyGuiEmail",
                    "Tiện ích",
                    "Nhật ký gửi email",
                    DateTime.Now,
                    true,
                    "Tiện ích",
                    28
                },
                {
                    Guid.NewGuid().ToString(),
                    "NhatKyTruyCap",
                    "Tiện ích",
                    "Nhật ký truy cập",
                    DateTime.Now,
                    true,
                    "Tiện ích",
                    29
                },
                });

            migrationBuilder.InsertData(
                table: "ThaoTacs",
                columns: new string[] { "ThaoTacId", "Ma", "Ten", "STT" },
                values: new object[,]
                {
                {
                    Guid.NewGuid().ToString(),
                    "SYS_FULL",
                    "Toàn quyền",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "SYS_VIEW",
                    "Xem",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "SYS_UPDATE",
                    "Sửa",
                    3
                },
                {
                    Guid.NewGuid().ToString(),
                    "MNG_FULL",
                    "Toàn quyền",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "MNG_VIEW",
                    "Xem",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "MNG_CREATE",
                    "Thêm",
                    3
                },
                {
                    Guid.NewGuid().ToString(),
                    "MNG_UPDATE",
                    "Sửa",
                    4
                },
                {
                    Guid.NewGuid().ToString(),
                    "MNG_DELETE",
                    "Xóa",
                    5
                },
                {
                    Guid.NewGuid().ToString(),
                    "MNG_EXPORT",
                    "Xuất khẩu",
                    6
                },
                {
                    Guid.NewGuid().ToString(),
                    "DM_FULL",
                    "Toàn quyền",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "DM_VIEW",
                    "Xem",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "DM_CREATE",
                    "Thêm",
                    3
                },
                {
                    Guid.NewGuid().ToString(),
                    "DM_UPDATE",
                    "Sửa",
                    4
                },
                {
                    Guid.NewGuid().ToString(),
                    "DM_DELETE",
                    "Xóa",
                    5
                },
                {
                    Guid.NewGuid().ToString(),
                    "DM_EXPORT",
                    "Xuất khẩu",
                    6
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_FULL",
                    "Toàn quyền",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_VIEW",
                    "Xem",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_CREATE",
                    "Thêm/Nhân bản/Nhập khẩu",
                    3
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_UPDATE",
                    "Sửa",
                    4
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_DELETE",
                    "Xóa",
                    5
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_EXPORT",
                    "Xuất khẩu",
                    6
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_PUBLISH",
                    "Phát hành hóa đơn",
                    7
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_DOWNLOAD",
                    "Tải hóa đơn",
                    8
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_CONVERT",
                    "Chuyển thành hóa đơn giấy",
                    9
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_DETAIL",
                    "Xuất khẩu chi tiết hóa đơn",
                    10
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_SEND",
                    "Gửi hóa đơn cho khách hàng/Gửi hóa đơn nháp cho khách hàng",
                    11
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_FAILURE_NOTIFICATION",
                    "Thông báo sai sót không phải lập lại hóa đơn",
                    12
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_CRASH",
                    "Xóa bỏ hóa đơn",
                    13
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_REPLACE",
                    "Lập hóa đơn thay thế",
                    14
                },
                {
                    Guid.NewGuid().ToString(),
                    "HD_ADJUST",
                    "Lập hóa đơn điều chỉnh",
                    15
                },
                {
                    Guid.NewGuid().ToString(),
                    "BC_FULL",
                    "Toàn quyền",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "BC_VIEW",
                    "Xem",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "BC_CREATE",
                    "Thêm",
                    3
                },
                {
                    Guid.NewGuid().ToString(),
                    "BC_UPDATE",
                    "Sửa",
                    4
                },
                {
                    Guid.NewGuid().ToString(),
                    "BC_DELETE",
                    "Xóa",
                    5
                },
                {
                    Guid.NewGuid().ToString(),
                    "BC_EXPORT",
                    "Xuất khẩu",
                    6
                },
                {
                    Guid.NewGuid().ToString(),
                    "TOOL_FULL",
                    "Toàn quyền",
                    1
                },
                {
                    Guid.NewGuid().ToString(),
                    "TOOL_VIEW",
                    "Xem",
                    2
                },
                {
                    Guid.NewGuid().ToString(),
                    "TOOL_EXPORT",
                    "Xuất khẩu",
                    3
                },
                });

            migrationBuilder.Sql(@"UPDATE [dbo].[Functions]
                                SET [Status] = 0
                                WHERE Type=N'Báo cáo'");


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
            OR (f.Type = N'Quản lý' and tt.Ma like N'MNG%')
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
            OR (f.Type = N'Quản lý' and tt.Ma like N'MNG%')
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
