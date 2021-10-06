using DLL.Configurations;
using DLL.Configurations.Config;
using DLL.Configurations.DanhMuc;
using DLL.Configurations.TienIch;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.BaoCao;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.TienIch;
using DLL.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DLL
{
    public class Datacontext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Datacontext(DbContextOptions<Datacontext> options,
             IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public virtual DbSet<ViewThaoTac> ViewThaoTacs { get; set; }
        public DbSet<KyKeToan> KyKeToans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PhanQuyenMauHoaDon> PhanQuyenMauHoaDons { get; set; }
        // Chat
        public DbSet<Function> Functions { get; set; }
        public DbSet<Function_Role> Function_Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Function_User> Function_Users { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<ThaoTac> ThaoTacs { get; set; }
        public DbSet<Function_ThaoTac> Function_ThaoTacs { get; set; }
        public DbSet<TuyChon> TuyChons { get; set; }
        public DbSet<ConfigNoiDungEmail> ConfigNoiDungEmails { get; set; }
        public DbSet<ThietLapTruongDuLieu> ThietLapTruongDuLieus { get; set; }

        #region Danh mục
        public DbSet<DoiTuong> DoiTuongs { get; set; }
        public DbSet<DonViTinh> DonViTinhs { get; set; }
        public DbSet<HangHoaDichVu> HangHoaDichVus { get; set; }
        public DbSet<LoaiTien> LoaiTiens { get; set; }
        public DbSet<MauHoaDon> MauHoaDons { get; set; }
        public DbSet<MauHoaDonThietLapMacDinh> MauHoaDonThietLapMacDinhs { get; set; }
        public DbSet<MauHoaDonTuyChinhChiTiet> MauHoaDonTuyChinhChiTiets { get; set; }

        public DbSet<HoSoHDDT> HoSoHDDTs { get; set; }
        public DbSet<HinhThucThanhToan> HinhThucThanhToans { get; set; }
        public DbSet<ThongBaoPhatHanh> ThongBaoPhatHanhs { get; set; }
        public DbSet<ThongBaoPhatHanhChiTiet> ThongBaoPhatHanhChiTiets { get; set; }
        public DbSet<ThongBaoKetQuaHuyHoaDon> ThongBaoKetQuaHuyHoaDons { get; set; }
        public DbSet<ThongBaoKetQuaHuyHoaDonChiTiet> ThongBaoKetQuaHuyHoaDonChiTiets { get; set; }
        public DbSet<ThongBaoDieuChinhThongTinHoaDon> ThongBaoDieuChinhThongTinHoaDons { get; set; }
        public DbSet<ThongBaoDieuChinhThongTinHoaDonChiTiet> ThongBaoDieuChinhThongTinHoaDonChiTiets { get; set; }
        public DbSet<QuyetDinhApDungHoaDon> QuyetDinhApDungHoaDons { get; set; }
        public DbSet<QuyetDinhApDungHoaDonDieu1> QuyetDinhApDungHoaDonDieu1s { get; set; }
        public DbSet<QuyetDinhApDungHoaDonDieu2> QuyetDinhApDungHoaDonDieu2s { get; set; }
        public DbSet<TaiLieuDinhKem> TaiLieuDinhKems { get; set; }
        #endregion

        #region Tiện tích
        public DbSet<NhatKyTruyCap> NhatKyTruyCaps { get; set; }
        public DbSet<NhatKyGuiEmail> NhatKyGuiEmails { get; set; }
        #endregion

        #region Hóa đơn
        public DbSet<HoaDonDienTu> HoaDonDienTus { get; set; }
        public DbSet<HoaDonDienTuChiTiet> HoaDonDienTuChiTiets { get; set; }
        public DbSet<NhatKyThaoTacHoaDon> NhatKyThaoTacHoaDons { get; set; }
        public DbSet<LuuTruTrangThaiFileHDDT> LuuTruTrangThaiFileHDDTs { get; set; }
        public DbSet<ThongTinChuyenDoi> ThongTinChuyenDois { get; set; }
        public DbSet<BienBanXoaBo> BienBanXoaBos { get; set; }
        public DbSet<LuuTruTrangThaiBBXB> LuuTruTrangThaiBBXBs { get; set; }
        public DbSet<BienBanDieuChinh> BienBanDieuChinhs { get; set; }
        public DbSet<LuuTruTrangThaiBBDT> LuuTruTrangThaiBBDTs { get; set; }
        #endregion

        #region Báo cáo
        public DbSet<NghiepVu> NghiepVus { get; set; }
        public DbSet<TruongDuLieu> TruongDuLieus { get; set; }
        public DbSet<BaoCaoTinhHinhSuDungHoaDon> BaoCaoTinhHinhSuDungHoaDons { get; set; }
        public DbSet<BaoCaoTinhHinhSuDungHoaDonChiTiet> BaoCaoTinhHinhSuDungHoaDonChiTiets { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddConfiguration(new KyKeToanConfiguration());
            modelBuilder.AddConfiguration(new RoleConfiguration());
            modelBuilder.AddConfiguration(new UserConfiguration());
            modelBuilder.AddConfiguration(new FunctionConfiguration());
            modelBuilder.AddConfiguration(new Function_RoleConfiguration());
            modelBuilder.AddConfiguration(new PermissionConfiguration());
            modelBuilder.AddConfiguration(new Function_UserConfiguration());
            modelBuilder.AddConfiguration(new User_RoleConfiguration());
            modelBuilder.AddConfiguration(new TuyChonConfiguration());
            modelBuilder.AddConfiguration(new ThietLapTruongDuLieuConfiguration());

            #region Danh mục
            modelBuilder.AddConfiguration(new DoiTuongConfiguration());
            modelBuilder.AddConfiguration(new DonViTinhConfiguration());
            modelBuilder.AddConfiguration(new HangHoaDichVuConfiguration());
            modelBuilder.AddConfiguration(new LoaiTienConfiguration());
            modelBuilder.AddConfiguration(new HoSoHDDTConfiguration());
            modelBuilder.AddConfiguration(new MauHoaDonConfiguration());
            modelBuilder.AddConfiguration(new MauHoaDonThietLapMacDinhConfiguration());
            modelBuilder.AddConfiguration(new MauHoaDonTuyChinhChiTietConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoPhatHanhConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoPhatHanhChiTietConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoKetQuaHuyHoaDonConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoKetQuaHuyHoaDonChiTietConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoDieuChinhThongTinHoaDonConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoDieuChinhThongTinHoaDonChiTietConfiguration());
            modelBuilder.AddConfiguration(new QuyetDinhApDungHoaDonConfiguration());
            modelBuilder.AddConfiguration(new QuyetDinhApDungHoaDonDieu1Configuration());
            modelBuilder.AddConfiguration(new QuyetDinhApDungHoaDonDieu2Configuration());
            modelBuilder.AddConfiguration(new TaiLieuDinhKemConfiguration());
            #endregion

            #region Tiện tích
            modelBuilder.AddConfiguration(new NhatKyTruyCapConfiguration());
            modelBuilder.AddConfiguration(new NhatKyGuiEmailConfiguration());
            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // optionsBuilder.EnableSensitiveDataLogging(true);
            var connectionString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.CONNECTION_STRING)?.Value;

            if (!string.IsNullOrEmpty(connectionString))
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditEntities()
        {
            IEnumerable<EntityEntry> entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added ||
                                                                                    x.State == EntityState.Modified);

            string nameIdentifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string currentUserId = null;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                currentUserId = nameIdentifier;
            }

            foreach (EntityEntry item in entities)
            {
                ThongTinChung changedOrAddedItem = (ThongTinChung)item.Entity;
                DateTime now = DateTime.Now;

                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        if (changedOrAddedItem.CreatedBy == null && currentUserId != null)
                        {
                            changedOrAddedItem.CreatedBy = currentUserId;
                        }

                        changedOrAddedItem.CreatedDate = now;
                    }

                    if (changedOrAddedItem.ModifyBy == null && currentUserId != null)
                    {
                        changedOrAddedItem.ModifyBy = currentUserId;
                    }

                    changedOrAddedItem.ModifyDate = now;
                }
            }
        }
    }
}