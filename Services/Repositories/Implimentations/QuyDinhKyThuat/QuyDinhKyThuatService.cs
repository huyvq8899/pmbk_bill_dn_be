﻿using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.HoaDonDienTu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class QuyDinhKyThuatService : IQuyDinhKyThuatService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mp;

        public QuyDinhKyThuatService(Datacontext dataContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IConfiguration configuration, IMapper mp)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _mp = mp;
        }

        public async Task<bool> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
        {
            var _entity = _mp.Map<ToKhaiDangKyThongTin>(tKhai);
            string folderName = tKhai.NhanUyNhiem == true ? "QuyDinhKyThuatHDDT_PhanII_I_2" : "QuyDinhKyThuatHDDT_PhanII_I_1";
            string assetsFolder = $"FilesUpload/QuyDinhKyThuat/{folderName}/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var fullXmlName = Path.Combine(fullXmlFolder, tKhai.FileXMLChuaKy);
            //string xmlDeCode = DataHelper.Base64Decode(fullXmlName);
            byte[] byteXML = Encoding.UTF8.GetBytes(fullXmlName);
            _entity.ContentXMLChuaKy = byteXML;
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayTao = DateTime.Now;
            await _dataContext.ToKhaiDangKyThongTins.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai)
        {
            var _entity = _mp.Map<DuLieuKyToKhai>(kTKhai);
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayKy = DateTime.Now;
            byte[] byteXML = Encoding.UTF8.GetBytes(kTKhai.NoiDungKy);
            _entity.NoiDungKy = byteXML;
            var _entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == kTKhai.IdToKhai);
            var fileName = Guid.NewGuid().ToString();
            string assetsFolder = _entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_1/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_2/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            #region create folder
            if (!Directory.Exists(fullXmlFolder))
            {
                Directory.CreateDirectory(fullXmlFolder);
            }
            else
            {
                string[] files = Directory.GetFiles(fullXmlFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            #endregion
            var fullXMLFile = Path.Combine(fullXmlFolder, fileName);
            File.WriteAllText(fullXMLFile, kTKhai.NoiDungKy);
            _entity.FileXMLDaKy = fileName;
            await _dataContext.DuLieuKyToKhais.AddAsync(_entity);

       
            if (!_entityTK.SignedStatus)
            {
                _entityTK.SignedStatus = true;
                _dataContext.Update(_entityTK);
            }
            else
            {
                _entityTK.FileXMLChuaKy = fileName;
                _dataContext.Update(_entityTK);
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> LuuTrangThaiGuiToKhai(TrangThaiGuiToKhaiViewModel tThai)
        {
            var _entity = _mp.Map<TrangThaiGuiToKhai>(tThai);
            await _dataContext.TrangThaiGuiToKhais.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<ToKhaiDangKyThongTinViewModel>> GetPagingAsync(PagingParams @params)
        {
            IQueryable<ToKhaiDangKyThongTinViewModel> query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join dlKy in _dataContext.DuLieuKyToKhais on tk.Id equals dlKy.IdToKhai into tmpDLKy
                        from dlKy in tmpDLKy.DefaultIfEmpty()
                        join ttGui in _dataContext.TrangThaiGuiToKhais on tk.Id equals ttGui.IdToKhai into tmpTTGui
                        from ttGui in tmpTTGui.DefaultIfEmpty()
                        select new ToKhaiDangKyThongTinViewModel
                        {
                            Id = tk.Id,
                            NgayTao = tk.NgayTao,
                            NhanUyNhiem = tk.NhanUyNhiem,
                            LoaiUyNhiem = tk.NhanUyNhiem ? tk.LoaiUyNhiem : null,
                            SignedStatus = tk.SignedStatus,
                            NgayKy = dlKy != null ? dlKy.NgayKy : null,
                            NgayGui = ttGui != null ? ttGui.ThoiGianGui : null,
                            TrangThaiGui = ttGui != null ? ttGui.TrangThaiGui.GetDescription() : string.Empty,
                            TrangThaiTiepNhan = ttGui != null ? ttGui.TrangThaiTiepNhan.GetDescription() : string.Empty
                        };

            query = query.GroupBy(x => new { x.Id })
                        .Select(x => new ToKhaiDangKyThongTinViewModel
                        {
                            Id = x.Key.Id,
                            NgayTao = x.First().NgayTao,
                            NhanUyNhiem = x.First().NhanUyNhiem,
                            LoaiUyNhiem = x.First().LoaiUyNhiem,
                            SignedStatus = x.First().SignedStatus,
                            NgayKy = x.OrderByDescending(y => y.NgayKy).Select(z => z.NgayKy).FirstOrDefault(),
                            NgayGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.NgayGui).FirstOrDefault(),
                            TrangThaiGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiGui).FirstOrDefault(),
                            TrangThaiTiepNhan = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiTiepNhan).FirstOrDefault(),
                        });

            var _list = await query.ToListAsync();

            if(!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                DateTime fromDate = DateTime.Parse(@params.FromDate);
                DateTime toDate = DateTime.Parse(@params.ToDate);
                query = query.Where(x => x.NgayTao >= fromDate &&
                                        x.NgayTao <= toDate);
            }

            return await PagedList<ToKhaiDangKyThongTinViewModel>
                .CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id)
        {
            var query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join dlKy in _dataContext.DuLieuKyToKhais on tk.Id equals dlKy.IdToKhai into tmpDLKy
                        from dlKy in tmpDLKy.DefaultIfEmpty()
                        join ttGui in _dataContext.TrangThaiGuiToKhais on tk.Id equals ttGui.IdToKhai into tmpTTGui
                        from ttGui in tmpTTGui.DefaultIfEmpty()
                        select new ToKhaiDangKyThongTinViewModel
                        {
                            Id = tk.Id,
                            NgayTao = tk.NgayTao,
                            IsThemMoi = tk.IsThemMoi,
                            FileXMLChuaKy = tk.FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.FromByteArray<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(tk.ContentXMLChuaKy),
                            ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.FromByteArray<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(tk.ContentXMLChuaKy),
                            NhanUyNhiem = tk.NhanUyNhiem,
                            LoaiUyNhiem = tk.LoaiUyNhiem,
                            SignedStatus = tk.SignedStatus,
                            NgayKy = dlKy != null ? dlKy.NgayKy : null,
                            NgayGui = ttGui != null ? ttGui.ThoiGianGui : null,
                            TrangThaiGui = ttGui != null ? ttGui.TrangThaiGui.GetDescription() : string.Empty,
                            TrangThaiTiepNhan = ttGui != null ? ttGui.TrangThaiTiepNhan.GetDescription() : string.Empty
                        };

            query = query.GroupBy(x => new { x.Id })
                        .Select(x => new ToKhaiDangKyThongTinViewModel
                        {
                            Id = x.Key.Id,
                            NgayTao = x.First().NgayTao,
                            NhanUyNhiem = x.First().NhanUyNhiem,
                            LoaiUyNhiem = x.First().LoaiUyNhiem,
                            SignedStatus = x.First().SignedStatus,
                            FileXMLChuaKy = x.First().FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = x.First().ToKhaiKhongUyNhiem,
                            ToKhaiUyNhiem = x.First().ToKhaiUyNhiem,
                            NgayKy = x.OrderByDescending(y => y.NgayKy).Select(z => z.NgayKy).FirstOrDefault(),
                            NgayGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.NgayGui).FirstOrDefault(),
                            TrangThaiGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiGui).FirstOrDefault(),
                            TrangThaiTiepNhan = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiTiepNhan).FirstOrDefault(),
                        });

            var data = await query.FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }
    }
}