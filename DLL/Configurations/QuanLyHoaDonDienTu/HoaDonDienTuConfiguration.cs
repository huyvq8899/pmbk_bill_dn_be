using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuConfiguration : DbEntityConfiguration<HoaDonDienTu>
    {
        public override void Configure(EntityTypeBuilder<HoaDonDienTu> entity)
        {
            entity.Property(c => c.LoaiChietKhau).HasDefaultValue(LoaiChietKhau.TheoMatHang);

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
        }
    }
}
