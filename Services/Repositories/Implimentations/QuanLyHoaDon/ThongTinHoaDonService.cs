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
using Services.Helper.Constants;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
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

        /// <summary>
        /// CheckTrungHoaDonHeThong kiểm tra trùng hóa đơn hệ thống (trường hợp 1)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CheckTrungHoaDonHeThongAsync(ThongTinHoaDon model)
        {
            var query = await (from hddt in _db.HoaDonDienTus
                         join bkhhd in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieuHoaDon from bkhhd in tmpBoKyHieuHoaDon.DefaultIfEmpty()
                         where hddt.MaCuaCQT.TrimToUpper() == model.MaCQTCap.TrimToUpper() &&
                         (bkhhd == null || (bkhhd != null && bkhhd.KyHieuMauSoHoaDon.ToString() == model.MauSoHoaDon.TrimToUpper())) && 
                         (bkhhd == null || (bkhhd != null && bkhhd.KyHieuHoaDon.TrimToUpper() == model.KyHieuHoaDon.TrimToUpper())) && 
                         hddt.SoHoaDon.TrimToUpper() == model.SoHoaDon.TrimToUpper() &&
                         hddt.MaTraCuu.TrimToUpper() == model.MaTraCuu.TrimToUpper() &&
                         hddt.NgayHoaDon.Value.Date == model.NgayHoaDon.Value.Date
                         select hddt.SoHoaDon
                        ).ToListAsync();

            return query.Count > 0;
        }

        /// <summary>
        /// CheckTrungThongTinAsync kiểm tra trùng hóa đơn thay thế và điều chỉnh trường hợp 2,3,4
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ThongTinHoaDonViewModel> CheckTrungThongTinAsync(ThongTinHoaDon param)
        {
            //kiểm tra xem đã có hóa đơn điều chỉnh cho hóa đơn đó chưa
            var queryHoaDonDieuChinh = await (from thongTinHD in _db.ThongTinHoaDons
                               join hoaDon in _db.HoaDonDienTus.Where(x => string.IsNullOrWhiteSpace(x.DieuChinhChoHoaDonId) == false) on thongTinHD.Id equals hoaDon.DieuChinhChoHoaDonId
                               where
                                 thongTinHD.MauSoHoaDon.TrimToUpper() == param.MauSoHoaDon.TrimToUpper()
                                 && thongTinHD.KyHieuHoaDon.TrimToUpper() == param.KyHieuHoaDon.TrimToUpper()
                                 && thongTinHD.SoHoaDon.TrimToUpper() == param.SoHoaDon.TrimToUpper() 
                                 select new ThongTinHoaDonViewModel
                               {
                                   Id = thongTinHD.Id,
                                   MauSoHoaDon = thongTinHD.MauSoHoaDon,
                                   KyHieuHoaDon = thongTinHD.KyHieuHoaDon,
                                   SoHoaDon = thongTinHD.SoHoaDon,
                                   NgayHoaDon = thongTinHD.NgayHoaDon.Value.ToString("dd/MM/yyyy"),
                                   LoaiHinhThuc = 2
                               }).FirstOrDefaultAsync();

            if (queryHoaDonDieuChinh == null)
            {
                //nếu chưa có hóa đơn điều chỉnh nào liên quan thì kiểm tra chính trong hóa đơn thay thế
                //thì chỉ cần kiểm tra trong bảng ThongTinHoaDons là được
                var queryThongTinHoaDon = await (from thongTinHD in _db.ThongTinHoaDons 
                                                 join hoaDon in _db.HoaDonDienTus.Where(x => string.IsNullOrWhiteSpace(x.ThayTheChoHoaDonId) == false) on thongTinHD.Id equals hoaDon.ThayTheChoHoaDonId 
                                                 where 
                                                    thongTinHD.MauSoHoaDon.TrimToUpper() == param.MauSoHoaDon.TrimToUpper()
                                                    && thongTinHD.KyHieuHoaDon.TrimToUpper() == param.KyHieuHoaDon.TrimToUpper()
                                                    && thongTinHD.SoHoaDon.TrimToUpper() == param.SoHoaDon.TrimToUpper()
                                                 select new ThongTinHoaDonViewModel
                                                  {
                                                      Id = thongTinHD.Id,
                                                      MauSoHoaDon = thongTinHD.MauSoHoaDon,
                                                      KyHieuHoaDon = thongTinHD.KyHieuHoaDon,
                                                      SoHoaDon = thongTinHD.SoHoaDon,
                                                      NgayHoaDon = thongTinHD.NgayHoaDon.Value.ToString("dd/MM/yyyy"),
                                                      LoaiHinhThuc = 1
                                                  }).FirstOrDefaultAsync();

                if (queryThongTinHoaDon != null)
                {
                    return queryThongTinHoaDon;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return queryHoaDonDieuChinh;
            }
        }

        public async Task<bool> CheckTrungThongTinThayTheAsync(ThongTinHoaDon param)
        {
            //kiểm tra xem đã có hóa đơn thay thế cho hóa đơn đó chưa
            var queryHoaDonThayThe = await (from thongTinHD in _db.ThongTinHoaDons
                                              join hoaDon in _db.HoaDonDienTus.Where(x => string.IsNullOrWhiteSpace(x.DieuChinhChoHoaDonId) == false) on thongTinHD.Id equals hoaDon.DieuChinhChoHoaDonId
                                              where
                                                thongTinHD.MauSoHoaDon.TrimToUpper() == param.MauSoHoaDon.TrimToUpper()
                                                && thongTinHD.KyHieuHoaDon.TrimToUpper() == param.KyHieuHoaDon.TrimToUpper()
                                                && thongTinHD.SoHoaDon.TrimToUpper() == param.SoHoaDon.TrimToUpper()
                                              select new ThongTinHoaDonViewModel
                                              {
                                                  MauSoHoaDon = thongTinHD.MauSoHoaDon,
                                                  KyHieuHoaDon = thongTinHD.KyHieuHoaDon,
                                                  SoHoaDon = thongTinHD.SoHoaDon,
                                                  NgayHoaDon = thongTinHD.NgayHoaDon.Value.ToString("dd/MM/yyyy"),
                                                  LoaiHinhThuc = 2
                                              }).FirstOrDefaultAsync();

            return queryHoaDonThayThe != null;
        }

        public async Task<bool> CheckTrungThongTinDieuChinhAsync(ThongTinHoaDon param)
        {
            //kiểm tra xem đã có hóa đơn điều chỉnh cho hóa đơn đó chưa
            var queryHoaDonDieuChinh = await (from thongTinHD in _db.ThongTinHoaDons
                                              join hoaDon in _db.HoaDonDienTus.Where(x => string.IsNullOrWhiteSpace(x.DieuChinhChoHoaDonId) == false) on thongTinHD.Id equals hoaDon.DieuChinhChoHoaDonId
                                              where
                                                thongTinHD.MauSoHoaDon.TrimToUpper() == param.MauSoHoaDon.TrimToUpper()
                                                && thongTinHD.KyHieuHoaDon.TrimToUpper() == param.KyHieuHoaDon.TrimToUpper()
                                                && thongTinHD.SoHoaDon.TrimToUpper() == param.SoHoaDon.TrimToUpper()
                                              select new ThongTinHoaDonViewModel
                                              {
                                                  MauSoHoaDon = thongTinHD.MauSoHoaDon,
                                                  KyHieuHoaDon = thongTinHD.KyHieuHoaDon,
                                                  SoHoaDon = thongTinHD.SoHoaDon,
                                                  NgayHoaDon = thongTinHD.NgayHoaDon.Value.ToString("dd/MM/yyyy"),
                                                  LoaiHinhThuc = 2
                                              }).FirstOrDefaultAsync();

            return queryHoaDonDieuChinh != null;
        }

        public async Task<HoaDonDienTuViewModel> GetById(string Id)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

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
                                                LoaiApDungHoaDonCanThayThe = x.HinhThucApDung,
                                                MaTraCuu = x.MaTraCuu,
                                                BienBanDieuChinhId = _db.BienBanDieuChinhs.Where(o => o.HoaDonBiDieuChinhId == Id).Select(o => o.BienBanDieuChinhId).FirstOrDefault(),
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
                                                                       Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                                       Status = tldk.Status
                                                                   })
                                                   .ToList(),
                                            })
                                            .FirstOrDefaultAsync();
        }
    }
}
