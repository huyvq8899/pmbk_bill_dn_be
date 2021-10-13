using AutoMapper;
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
            string xmlDeCode = DataHelper.Base64Decode(tKhai.FileXMLChuaKy);
            byte[] byteXML = Encoding.UTF8.GetBytes(tKhai.FileXMLChuaKy);
            _entity.ContentXMLChuaKy = byteXML;
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayTao = DateTime.Now;
            await _dataContext.ToKhaiDangKyThongTins.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai)
        {
            var _entity = _mp.Map<DuLieuKyToKhai>(kTKhai);
            await _dataContext.DuLieuKyToKhais.AddAsync(_entity);

            var _entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == kTKhai.IdToKhai);
            if (!_entityTK.SignedStatus)
            {
                _entityTK.SignedStatus = true;
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
            var query = from tk in _dataContext.ToKhaiDangKyThongTins
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
                            NgayKy = dlKy.NgayKy ?? null,
                            NgayGui = ttGui.ThoiGianGui ?? null,
                            TrangThaiGui = ttGui.TrangThaiGui.GetDescription() ?? string.Empty,
                            TrangThaiTiepNhan = ttGui.TrangThaiTiepNhan.GetDescription() ?? string.Empty
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
                            FileXMLChuaKy = tk.FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.FromByteArray<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(tk.ContentXMLChuaKy),
                            ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.FromByteArray<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(tk.ContentXMLChuaKy),
                            NhanUyNhiem = tk.NhanUyNhiem,
                            LoaiUyNhiem = tk.LoaiUyNhiem,
                            SignedStatus = tk.SignedStatus,
                            NgayKy = dlKy.NgayKy ?? null,
                            NgayGui = ttGui.ThoiGianGui ?? null,
                            TrangThaiGui = ttGui.TrangThaiGui.GetDescription() ?? string.Empty,
                            TrangThaiTiepNhan = ttGui.TrangThaiTiepNhan.GetDescription() ?? string.Empty
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

            return await query.FirstOrDefaultAsync();
        }
    }
}
