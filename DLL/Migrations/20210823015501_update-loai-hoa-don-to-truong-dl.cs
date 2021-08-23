using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class updateloaihoadontotruongdl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update TruongDuLieuHoaDons " +
                                "Set LoaiHoaDon = 0 Where STT >=1 and STT<=26 and IsChiTiet = 0");
            migrationBuilder.Sql("Update TruongDuLieuHoaDons " +
                    "Set LoaiHoaDon = 1 Where STT >=27 and STT<=36");
            migrationBuilder.InsertData(
                    table: "TruongDuLieuHoaDons",
                    columns: new string[] {
                                    "Id",
                                    "STT",
                                    "MaTruong",
                                    "TenTruong",
                                    "TenTruongData",
                                    "GhiChu",
                                    "IsMoRong",
                                    "IsChiTiet",
                                    "Status",
                                    "Default",
                                    "Size",
                                    "Align",
                                    "DefaultSTT",
                                    "DinhDangSo",
                                    "LoaiHoaDon"
                    },
                    values: new object[,] {
                    {
                        Guid.NewGuid().ToString(),
                        27,
                        "TTBS1",
                        "Trường thông tin bổ sung 1",
                        "TruongThongTinBoSung1",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        27,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        28,
                        "TTBS2",
                        "Trường thông tin bổ sung 2",
                        "TruongThongTinBoSung2",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        28,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        29,
                        "TTBS3",
                        "Trường thông tin bổ sung 3",
                        "TruongThongTinBoSung3",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        29,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        30,
                        "TTBS4",
                        "Trường thông tin bổ sung 4",
                        "TruongThongTinBoSung4",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        30,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        31,
                        "TTBS5",
                        "Trường thông tin bổ sung 5",
                        "TruongThongTinBoSung5",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        31,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        32,
                        "TTBS6",
                        "Trường thông tin bổ sung 6",
                        "TruongThongTinBoSung6",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        30,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        33,
                        "TTBS7",
                        "Trường thông tin bổ sung 7",
                        "TruongThongTinBoSung7",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        33,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        34,
                        "TTBS8",
                        "Trường thông tin bổ sung 8",
                        "TruongThongTinBoSung8",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        34,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        35,
                        "TTBS9",
                        "Trường thông tin bổ sung 9",
                        "TruongThongTinBoSung9",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        35,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        36,
                        "TTBS10",
                        "Trường thông tin bổ sung 10",
                        "TruongThongTinBoSung10",
                        "",
                        false,
                        false,
                        false,
                        false,
                        150,
                        "left",
                        36,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        27,
                        "HHDV 27",
                        "Trường mở rộng chi tiết số 1",
                        "TruongMoRongChiTiet1",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        27,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        28,
                        "HHDV 28",
                        "Trường mở rộng chi tiết số 2",
                        "TruongMoRongChiTiet2",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        28,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        29,
                        "HHDV 29",
                        "Trường mở rộng chi tiết số 3",
                        "TruongMoRongChiTiet3",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        29,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        30,
                        "HHDV 30",
                        "Trường mở rộng chi tiết số 4",
                        "TruongMoRongChiTiet4",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        30,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        31,
                        "HHDV 31",
                        "Trường mở rộng chi tiết số 5",
                        "TruongMoRongChiTiet5",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        31,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        32,
                        "HHDV 32",
                        "Trường mở rộng chi tiết số 6",
                        "TruongMoRongChiTiet6",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        32,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        33,
                        "HHDV 33",
                        "Trường mở rộng chi tiết số 7",
                        "TruongMoRongChiTiet7",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        33,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        34,
                        "HHDV 34",
                        "Trường mở rộng chi tiết số 8",
                        "TruongMoRongChiTiet8",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        34,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        35,
                        "HHDV 35",
                        "Trường mở rộng chi tiết số 9",
                        "TruongMoRongChiTiet9",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        35,
                        false,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        36,
                        "HHDV 36",
                        "Trường mở rộng chi tiết số 10",
                        "TruongMoRongChiTiet10",
                        "",
                        true,
                        true,
                        false,
                        false,
                        150,
                        "left",
                        36,
                        false,
                        2
                    },
            });

            migrationBuilder.Sql("Update ThietLapTruongDuLieuMoRongs " +
                    "Set LoaiHoaDon = 1");
            migrationBuilder.InsertData(
                table: "ThietLapTruongDuLieuMoRongs",
                columns: new string[] { "Id", "TenTruong", "TenTruongHienThi", "GhiChu", "HienThi", "LoaiHoaDon" },
                values: new object[,]
                {
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 1",
                        "Trường thông tin bổ sung 1",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 2",
                        "Trường thông tin bổ sung 2",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 3",
                        "Trường thông tin bổ sung 3",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 4",
                        "Trường thông tin bổ sung 4",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 5",
                        "Trường thông tin bổ sung 5",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 6",
                        "Trường thông tin bổ sung 6",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 7",
                        "Trường thông tin bổ sung 7",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 8",
                        "Trường thông tin bổ sung 8",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 9",
                        "Trường thông tin bổ sung 9",
                        string.Empty,
                        true,
                        2
                    },
                    {
                        Guid.NewGuid().ToString(),
                        "Trường thông tin bổ sung 10",
                        "Trường thông tin bổ sung 10",
                        string.Empty,
                        true,
                        2
                    },

                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
