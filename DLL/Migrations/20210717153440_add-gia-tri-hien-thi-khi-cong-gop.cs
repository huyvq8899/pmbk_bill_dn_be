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
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fMaHang"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fTenHang"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fDVT"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoLuong"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fDonGiaSauThue"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fDonGia"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fThanhTienSauThue"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fTyLeChietKhau"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fMaQuyCach"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoLo"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fHanSuDung"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoKhung"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoMay"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fXuatBanPhi"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fGhiChu"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fMaNhanVien"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fTenNhanVien"
                    }
                },
                 column: "Status",
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
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fMaHang"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fTenHang"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fDVT"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoLuong"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fDonGiaSauThue"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fDonGia"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fThanhTienSauThue"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fTyLeChietKhau"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fMaQuyCach"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoLo"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fHanSuDung"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoKhung"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fSoMay"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fXuatBanPhi"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fGhiChu"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fMaNhanVien"
                    },
                    {
                        "0fd15f22-f2cb-498f-916c-78bdb9fcdc85",
                        "fTenNhanVien"
                    }
                 },
                  column: "Status",
                  values: new object[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }
              );
        }
    }
}
