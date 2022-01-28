using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLL.Configurations.QuanLyHoaDonDienTu
{
    public class ThongBaoSaiThongTinConfiguration : DbEntityConfiguration<ThongBaoSaiThongTin>
    {
        public override void Configure(EntityTypeBuilder<ThongBaoSaiThongTin> entity)
        {
            entity.HasKey(c => new { c.Id });
            entity.Property(c => c.Id).HasMaxLength(36);
            entity.Property(c => c.DoiTuongId).HasMaxLength(36);
            entity.Property(c => c.HoaDonDienTuId).HasMaxLength(36);

            entity.Property(c => c.HoTenNguoiMuaHang_Sai).HasMaxLength(200);
            entity.Property(c => c.HoTenNguoiMuaHang_Dung).HasMaxLength(200);
            entity.Property(c => c.TenDonVi_Sai).HasMaxLength(1000);
            entity.Property(c => c.TenDonVi_Dung).HasMaxLength(1000);
            entity.Property(c => c.DiaChi_Dung).HasMaxLength(1000);
            entity.Property(c => c.DiaChi_Sai).HasMaxLength(1000);

            entity.Property(c => c.TenNguoiNhan).HasMaxLength(200);
            entity.Property(c => c.EmailCuaNguoiNhan).HasMaxLength(300);
            entity.Property(c => c.EmailCCCuaNguoiNhan).HasMaxLength(300);
            entity.Property(c => c.EmailBCCCuaNguoiNhan).HasMaxLength(300);
            entity.Property(c => c.SDTCuaNguoiNhan).HasMaxLength(100);

            entity.Property(c => c.CreatedBy).HasMaxLength(36);
            entity.Property(c => c.ModifyBy).HasMaxLength(36);
            entity.Property(c => c.NhatKyGuiEmailId).HasMaxLength(36);
        }
    }
}
