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
            await _dataContext.ToKhaiDangKyThongTins.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai)
        {
            var _entity = _mp.Map<DuLieuKyToKhai>(kTKhai);
            await _dataContext.DuLieuKyToKhais.AddAsync(_entity);
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

        }
    }
}
