using DLL.Configurations;
using DLL.Configurations.Config;
using DLL.Configurations.DanhMuc;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
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
        // Chat
        public DbSet<Function> Functions { get; set; }
        public DbSet<Function_Role> Function_Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Function_User> Function_Users { get; set; }
        public DbSet<User_Role> User_Roles { get; set; }
        public DbSet<ThaoTac> ThaoTacs { get; set; }
        public DbSet<Function_ThaoTac> Function_ThaoTacs { get; set; }
        public DbSet<TuyChon> TuyChons { get; set; }

        #region Danh mục
        public DbSet<DoiTuong> DoiTuongs { get; set; }
        public DbSet<DonViTinh> DonViTinhs { get; set; }
        public DbSet<HangHoaDichVu> HangHoaDichVus { get; set; }
        public DbSet<LoaiTien> LoaiTiens { get; set; }
        public DbSet<HoSoHDDT> HoSoHDDTs { get; set; }
        public DbSet<MauHoaDon> MauHoaDons { get; set; }
        public DbSet<ThongBaoPhatHanh> ThongBaoPhatHanhs { get; set; }
        public DbSet<ThongBaoPhatHanhChiTiet> ThongBaoPhatHanhChiTiets { get; set; }
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

            #region Danh mục
            modelBuilder.AddConfiguration(new DoiTuongConfiguration());
            modelBuilder.AddConfiguration(new DonViTinhConfiguration());
            modelBuilder.AddConfiguration(new HangHoaDichVuConfiguration());
            modelBuilder.AddConfiguration(new LoaiTienConfiguration());
            modelBuilder.AddConfiguration(new HoSoHDDTConfiguration());
            modelBuilder.AddConfiguration(new MauHoaDonConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoPhatHanhConfiguration());
            modelBuilder.AddConfiguration(new ThongBaoPhatHanhChiTietConfiguration());
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
                ThongTinChung changedOrAddedItem = item.Entity as ThongTinChung;
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