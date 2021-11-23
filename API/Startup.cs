using API.Extentions;
using API.Providers;
using API.Services;
using AutoMapper;
using DLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Services.Helper;
using Services.Hubs;
using Services.Repositories.Implimentations;
using Services.Repositories.Implimentations.BaoCao;
using Services.Repositories.Implimentations.Config;
using Services.Repositories.Implimentations.DanhMuc;
using Services.Repositories.Implimentations.QuanLy;
using Services.Repositories.Implimentations.QuanLyHoaDon;
using Services.Repositories.Implimentations.QuyDinhKyThuat;
using Services.Repositories.Implimentations.TienIch;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.BaoCao;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLy;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.Repositories.Interfaces.TienIch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wework.Auguard;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Datacontext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                 o => o.MigrationsAssembly("DLL").UseRowNumberForPaging()), ServiceLifetime.Transient);

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            buiders =>
            {
                buiders
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.pmbk.vn", "http://localhost:4200", "http://localhost:4300");
            }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                     options.SerializerSettings.Formatting = Formatting.Indented;
                     options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                 }
            );

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hóa đơn điện tử",
                    Version = "v1",
                });

                c.CustomSchemaIds(i => i.FullName);

                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });
            services.AddAutoMapper();

            // Add thread host
            //services.AddHostedService<ConsumeScopedServiceHostedService>();
            //services.AddHostedService<BackgroundQueueOut>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthorizationHandler, BaseAuthorizationHandler>();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
            services.AddHostedService<TimedHostedService>();
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<IRoleRespositories, RoleRespositories>();
            services.AddScoped<IUserRespositories, UserRespositories>();
            services.AddScoped<IFunctionRespositories, FunctionRespositories>();
            services.AddScoped<IFunction_RoleRespositories, Function_RoleRespositories>();
            services.AddScoped<IPermissionRespositories, PermissionRespositories>();
            services.AddScoped<IFunction_UserRespositories, Function_UserRespositories>();
            services.AddScoped<IUser_RoleRespositories, User_RoleRespositories>();
            services.AddScoped<ITuyChonService, TuyChonService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IXMLInvoiceService, XMLInvoiceService>();
            services.AddScoped<IThietLapTruongDuLieuService, ThietLapTruongDuLieuService>();


            #region Danh mục
            services.AddScoped<IDoiTuongService, DoiTuongService>();
            services.AddScoped<IDonViTinhService, DonViTinhService>();
            services.AddScoped<IHangHoaDichVuService, HangHoaDichVuService>();
            services.AddScoped<ILoaiTienService, LoaiTienService>();
            services.AddScoped<IHoSoHDDTService, HoSoHDDTService>();
            services.AddScoped<IMauHoaDonService, MauHoaDonService>();
            services.AddScoped<IHinhThucThanhToanService, HinhThucThanhToanService>();
            services.AddScoped<IThongBaoPhatHanhService, ThongBaoPhatHanhService>();
            services.AddScoped<IThongBaoKetQuaHuyHoaDonService, ThongBaoKetQuaHuyHoaDonService>();
            services.AddScoped<IThongBaoDieuChinhThongTinHoaDonService, ThongBaoDieuChinhThongTinHoaDonService>();
            services.AddScoped<IQuyetDinhApDungHoaDonService, QuyetDinhApDungHoaDonService>();
            #endregion

            #region Tiện tích
            services.AddScoped<INhatKyTruyCapService, NhatKyTruyCapService>();
            services.AddScoped<INhatKyGuiEmailService, NhatKyGuiEmailService>();
            services.AddScoped<ITVanService, TVanService>();
            #endregion

            #region Hóa đơn điện tử
            services.AddScoped<IHoaDonDienTuService, HoaDonDienTuService>();
            services.AddScoped<IHoaDonDienTuChiTietService, HoaDonDienTuChiTietService>();
            services.AddScoped<IBienBanDieuChinhService, BienBanDieuChinhService>();
            services.AddScoped<ITraCuuService, TraCuuService>();
            services.AddScoped<IThongTinHoaDonService, ThongTinHoaDonService>();
            #endregion

            #region Thông báo gửi CQT
            services.AddScoped<IThongDiepGuiNhanCQTService, ThongDiepGuiNhanCQTService>();
            #endregion

            #region Báo cáo
            services.AddScoped<IBaoCaoService, BaoCaoService>();
            #endregion

            #region Quy định kỹ thuật
            services.AddScoped<IQuyDinhKyThuatService, QuyDinhKyThuatService>();
            services.AddScoped<IDuLieuGuiHDDTService, DuLieuGuiHDDTService>();
            #endregion

            #region Quản lý
            services.AddScoped<IBoKyHieuHoaDonService, BoKyHieuHoaDonService>();
            #endregion

            // bỏ dấu #
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });
            //
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["access_token"];

                          // If the request is for our hub...
                          var path = context.HttpContext.Request.Path;
                          if (!string.IsNullOrEmpty(accessToken) &&
                              (path.StartsWithSegments("/signalr")))
                          {
                              // Read the token out of the query string
                              context.Token = accessToken;
                          }
                          return Task.CompletedTask;
                      }
                  };

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                          .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });

            // chat with angular
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.WithOrigins("http://localhost:4200")
            //        //builder => builder.WithOrigins("https://ketoan.pmbk.vn")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});

            services.AddCors(options => options.AddPolicy("CorsPolicy",
            buiders =>
            {
                buiders
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.pmbk.vn", "http://localhost:4200", "http://localhost:4300");
            }));

            // chat with angular
            services.AddSignalR();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });

                app.UseHsts();
            }

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoice V1");
                });
            }

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            // Fix ERR_INVALID_HTTP_RESPONSE
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            //app.ConfigureExceptionHandler();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseDefaultFiles(); // them khi co controller fallback
            app.UseStaticFiles();
            app.UseCookiePolicy();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseAuthentication(); // con gấu

            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRHub>("/signalr");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                //// To learn more about options for serving an Angular SPA from ASP.NET Core,
                //// see https://go.microsoft.com/fwlink/?linkid=864501

                //spa.Options.SourcePath = "ClientApp";

                //if (env.IsDevelopment())
                //{
                //    spa.UseAngularCliServer(npmScript: "start");
                //}
            });
        }
    }


}
