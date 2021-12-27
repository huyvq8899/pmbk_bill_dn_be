using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class them_dulieu_truongthongbao_macdinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM [dbo].[ThietLapTruongDuLieus] where ThietLapTruongDuLieuId = 'bcf3ca2c-126c-493e-929b-9ea150733eeb'
GO
INSERT [dbo].[ThietLapTruongDuLieus] ([ThietLapTruongDuLieuId], [MaTruong], [TenCot], [TenTruong], [TenTruongHienThi], [LoaiHoaDon], [LoaiTruongDuLieu], [KieuDuLieu], [GhiChu], [DoRong], [STT], [HienThi], [GiaTri], [BoKyHieuHoaDonId]) VALUES (N'bcf3ca2c-126c-493e-929b-9ea150733eeb', N'', N'ThongBaoSaiSot', N'Thông báo sai sót', N'Thông báo sai sót', 0, 0, 1, N'', 360, 41, 1, NULL, NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
