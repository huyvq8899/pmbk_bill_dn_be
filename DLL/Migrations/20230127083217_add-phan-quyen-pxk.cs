using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addphanquyenpxk : Migration
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
                    "PhieuXuatKho",
                    null,
                    "Phiếu xuất kho",
                    DateTime.Now,
                    true,
                    "Phiếu xuất kho",
                    33,
                    null
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
                }
                });

            migrationBuilder.Sql(@"INSERT INTO Function_ThaoTacs(FTID, RoleId, FunctionId, ThaoTacId, Active)
            SELECT NEWID() AS FTID, r.RoleId AS RoleId, f.FunctionId AS FunctionId, tt.ThaoTacId AS ThaoTacId, 0 AS Active
            FROM (SELECT * FROM Roles) r
            CROSS JOIN Functions f
            CROSS JOIN ThaoTacs tt
            WHERE (f.Type = N'Phiếu xuất kho' and tt.Ma Like N'PXK%')");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
