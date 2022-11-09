using DLL.Entity.QuanLyHoaDon;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuConfiguration : DbEntityConfiguration<HoaDonDienTu>
    {
        public override void Configure(EntityTypeBuilder<HoaDonDienTu> entity)
        {
            entity.Property(r => r.TyLeChietKhau).HasColumnType("decimal(6,4)");
            entity.Property(r => r.TyGia).HasColumnType("decimal(7,2)");
            entity.Property(r => r.TongTienHang).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienChietKhau).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienThueGTGT).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienThanhToan).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienHangQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienChietKhauQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienThueGTGTQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienThanhToanQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(c => c.ThongDiepGuiCQTId).HasMaxLength(36);
            entity.Property(c => c.IdHoaDonSaiSotBiThayThe).HasMaxLength(36);
            entity.Property(c => c.GhiChuThayTheSaiSot).HasMaxLength(500);

            entity.Property(r => r.TyLePhanTramDoanhThu).HasColumnType("decimal(6,4)");
            entity.Property(r => r.TongTienGiam).HasColumnType("decimal(21,6)");
            entity.Property(r => r.TongTienGiamQuyDoi).HasColumnType("decimal(21,6)");
            entity.Property(c => c.EmailTBaoSaiSotKhongPhaiLapHDId).HasMaxLength(36);
            entity.Property(c => c.MaLoaiTien).HasMaxLength(3);

            entity.Property(c => c.CanCuSo).HasMaxLength(255);
            entity.Property(c => c.Cua).HasMaxLength(400);
            entity.Property(c => c.DienGiai).HasMaxLength(400);
            entity.Property(c => c.DiaChiKhoXuatHang).HasMaxLength(400);
            entity.Property(c => c.HoTenNguoiXuatHang).HasMaxLength(100);
            entity.Property(c => c.HopDongVanChuyenSo).HasMaxLength(255);
            entity.Property(c => c.TenNguoiVanChuyen).HasMaxLength(100);
            entity.Property(c => c.PhuongThucVanChuyen).HasMaxLength(50);
            entity.Property(c => c.DiaChiKhoNhanHang).HasMaxLength(400);
            entity.Property(c => c.HoTenNguoiNhanHang).HasMaxLength(100);

            entity.Property(r => r.SoLuong).HasColumnType("decimal(21,6)");
            entity.Property(c => c.SoXe).HasMaxLength(256);
            entity.Property(c => c.SoGhe).HasMaxLength(256);
            entity.Property(c => c.MaTraCuu).HasMaxLength(256);
            entity.Property(c => c.MaCuaCQT).HasMaxLength(256);
            entity.Property(c => c.SoTuyen).HasMaxLength(256);
            entity.Property(c => c.ThueGTGT).HasMaxLength(16);

            entity.HasOne(u => u.Xe)
            .WithMany(s => s.HoaDonDienTus)
            .HasForeignKey(sc => sc.XeId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
