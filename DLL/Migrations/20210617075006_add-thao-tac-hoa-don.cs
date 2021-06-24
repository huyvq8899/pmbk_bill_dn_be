using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addthaotachoadon : Migration
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
                        "Thêm",
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
                        "HD_PUBLISH",
                        "Phát hành hóa đơn",
                        6
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HD_DOWNLOAD",
                        "Tải hóa đơn",
                        7
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HD_CONVERT",
                        "Chuyển thành hóa đơn giấy",
                        8
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HD_SEND",
                        "Gửi hóa đơn cho khách hàng",
                        9
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HD_CRASH",
                        "Xóa bỏ hóa đơn",
                        10
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HD_REPLACE",
                        "Lập hóa đơn thay thế",
                        11
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "HD_ADJUST",
                        "Lập hóa đơn điều chỉnh",
                        12
                    },
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
