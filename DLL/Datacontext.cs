using DLL.Configurations;
using DLL.Configurations.Config;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.Config;
using DLL.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DLL
{
    public class Datacontext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        IConfiguration _config;

        public Datacontext(DbContextOptions<Datacontext> options,
             IHttpContextAccessor httpContextAccessor, IConfiguration IConfig) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = IConfig;
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
    }
}