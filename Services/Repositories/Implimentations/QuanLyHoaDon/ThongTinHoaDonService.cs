using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class ThongTinHoaDonService : IThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _IHttpContextAccessor;

        public ThongTinHoaDonService(Datacontext datacontext, IMapper mapper, 
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor IHttpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _IHttpContextAccessor = IHttpContextAccessor;
        }

        public async Task<ThongTinHoaDon> InsertAsync(ThongTinHoaDon model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = DateTime.Now;
            model.ModifyDate = DateTime.Now;
            await _db.ThongTinHoaDons.AddAsync(model);
            var result = await _db.SaveChangesAsync() > 0;
            if (result)
            {
                return model;
            }
            else
            {
                return null;
            }
        }

        public async Task<ThongTinHoaDon> UpdateAsync(ThongTinHoaDon model)
        {
            model.ModifyDate = DateTime.Now;
            var entity = await _db.ThongTinHoaDons.FirstOrDefaultAsync(x => x.Id == model.Id);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            if (result)
            {
                return model;
            }
            else
            {
                return null;
            }
        }

        public async Task<HoaDonDienTuViewModel> GetById(string Id)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            return await _db.ThongTinHoaDons.Where(x => x.Id == Id)
                                            .Select(x => new HoaDonDienTuViewModel
                                            {
                                                HoaDonDienTuId = x.Id,
                                                MauSo = x.MauSoHoaDon,
                                                KyHieu = x.KyHieuHoaDon,
                                                MaCuaCQT = x.MaCQTCap,
                                                NgayHoaDon = x.NgayHoaDon,
                                                SoHoaDon = x.SoHoaDon,
                                                LoaiApDungHoaDonDieuChinh = x.HinhThucApDung,
                                                BienBanDieuChinhId = _db.BienBanDieuChinhs.Where(o=>o.HoaDonBiDieuChinhId == Id).Select(o=>o.BienBanDieuChinhId).FirstOrDefault(),
                                                TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                                   where tldk.NghiepVuId == x.Id
                                                                   orderby tldk.CreatedDate
                                                                   select new TaiLieuDinhKemViewModel
                                                                   {
                                                                       TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                                       NghiepVuId = tldk.NghiepVuId,
                                                                       LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                                       TenGoc = tldk.TenGoc,
                                                                       TenGuid = tldk.TenGuid,
                                                                       CreatedDate = tldk.CreatedDate,
                                                                       Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{x.Id}\FileAttach", tldk.TenGuid),
                                                                       Status = tldk.Status
                                                                   })
                                                   .ToList(),
                                            })
                                            .FirstOrDefaultAsync();
        }
    }
}
