using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Them_dulieu_truongthongbao_macdinh3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM [dbo].[ThietLapTruongDuLieus] where ThietLapTruongDuLieuId = 'bcf3ca2c-126c-493e-929b-9ea150733eeb'
GO
INSERT [dbo].[ThietLapTruongDuLieus] ([ThietLapTruongDuLieuId], [MaTruong], [TenCot], [TenTruong], [TenTruongHienThi], [LoaiHoaDon], [LoaiTruongDuLieu], [KieuDuLieu], [GhiChu], [DoRong], [STT], [HienThi], [GiaTri], [BoKyHieuHoaDonId]) VALUES (N'bcf3ca2c-126c-493e-929b-9ea150733eeb', N'', N'ThongTinSaiSot', N'Thông báo hóa đơn điện tử có sai sót', N'Thông báo hóa đơn điện tử có sai sót', 0, 0, 1, N'', 360, 41, 1, NULL, NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
