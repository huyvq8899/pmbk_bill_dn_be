using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addgiatrihienthikhiconggop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                 table: "TruongDuLieus",
                 keyColumns: new string[] { "NghiepVuId", "MaTruong" },
                keyValues: new object[,]
                {
                    {
                        "nv2",
                        "fMaHang"
                    },
                    {
                        "nv2",
                        "fTenHang"
                    },
                    {
                        "nv2",
                        "fDVT"
                    },
                    {
                        "nv2",
                        "fSoLuong"
                    },
                    {
                        "nv2",
                        "fDonGiaSauThue"
                    },
                    {
                        "nv2",
                        "fDonGia"
                    },
                    {
                        "nv2",
                        "fThanhTienSauThue"
                    },
                    {
                        "nv2",
                        "fTyLeChietKhau"
                    },
                    {
                        "nv2",
                        "fMaQuyCach"
                    },
                    {
                        "nv2",
                        "fSoLo"
                    },
                    {
                        "nv2",
                        "fHanSuDung"
                    },
                    {
                        "nv2",
                        "fSoKhung"
                    },
                    {
                        "nv2",
                        "fSoMay"
                    },
                    {
                        "nv2",
                        "fXuatBanPhi"
                    },
                    {
                        "nv2",
                        "fGhiChu"
                    },
                    {
                        "nv2",
                        "fMaNhanVien"
                    },
                    {
                        "nv2",
                        "fTenNhanVien"
                    }
                },
                 column: "HienThiKhiCongGop",
                 values: new object[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false }
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                  table: "TruongDuLieus",
                  keyColumns: new string[] { "NghiepVuId", "MaTruong" },
                 keyValues: new object[,]
                 {
                    {
                        "nv2",
                        "fMaHang"
                    },
                    {
                        "nv2",
                        "fTenHang"
                    },
                    {
                        "nv2",
                        "fDVT"
                    },
                    {
                        "nv2",
                        "fSoLuong"
                    },
                    {
                        "nv2",
                        "fDonGiaSauThue"
                    },
                    {
                        "nv2",
                        "fDonGia"
                    },
                    {
                        "nv2",
                        "fThanhTienSauThue"
                    },
                    {
                        "nv2",
                        "fTyLeChietKhau"
                    },
                    {
                        "nv2",
                        "fMaQuyCach"
                    },
                    {
                        "nv2",
                        "fSoLo"
                    },
                    {
                        "nv2",
                        "fHanSuDung"
                    },
                    {
                        "nv2",
                        "fSoKhung"
                    },
                    {
                        "nv2",
                        "fSoMay"
                    },
                    {
                        "nv2",
                        "fXuatBanPhi"
                    },
                    {
                        "nv2",
                        "fGhiChu"
                    },
                    {
                        "nv2",
                        "fMaNhanVien"
                    },
                    {
                        "nv2",
                        "fTenNhanVien"
                    }
                 },
                  column: "HienThiKhiCongGop",
                  values: new object[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }
              );
        }
    }
}
