using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class DoiTuongService : IDoiTuongService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public DoiTuongService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == id);
            _db.DoiTuongs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<DoiTuongViewModel>> GetAllAsync(DoiTuongParams @params = null)
        {
            var query = _db.DoiTuongs.AsQueryable();

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    @params.Keyword = @params.Keyword.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(@params.Keyword) ||
                                            x.Ten.ToUpper().ToTrim().Contains(@params.Keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(@params.Keyword.ToUpper()));
                }

                if (@params.LoaiDoiTuong.HasValue == true)
                {
                    query = query.Where(x => @params.LoaiDoiTuong == 1 ? (x.IsKhachHang == true) : (x.IsNhanVien == true));
                }
            }

            var result = await query
                .ProjectTo<DoiTuongViewModel>(_mp.ConfigurationProvider)
                .AsNoTracking()
                .OrderBy(x => x.Ma)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<DoiTuongViewModel>> GetAllPagingAsync(DoiTuongParams @params)
        {
            var query = _db.DoiTuongs
                .OrderBy(x => x.Ma)
                .Select(x => new DoiTuongViewModel
                {
                    DoiTuongId = x.DoiTuongId,
                    LoaiKhachHang = x.LoaiKhachHang,
                    MaSoThue = x.MaSoThue ?? string.Empty,
                    Ma = x.Ma ?? string.Empty,
                    Ten = x.Ten ?? string.Empty,
                    DiaChi = x.DiaChi ?? string.Empty,
                    SoTaiKhoanNganHang = x.SoTaiKhoanNganHang ?? string.Empty,
                    TenNganHang = x.TenNganHang ?? string.Empty,
                    ChiNhanh = x.ChiNhanh ?? string.Empty,
                    HoTenNguoiMuaHang = x.HoTenNguoiMuaHang ?? string.Empty,
                    EmailNguoiMuaHang = x.EmailNguoiMuaHang ?? string.Empty,
                    SoDienThoaiNguoiMuaHang = x.SoDienThoaiNguoiMuaHang ?? string.Empty,
                    HoTenNguoiNhanHD = x.HoTenNguoiNhanHD ?? string.Empty,
                    EmailNguoiNhanHD = x.EmailNguoiNhanHD ?? string.Empty,
                    SoDienThoaiNguoiNhanHD = x.SoDienThoaiNguoiNhanHD ?? string.Empty,
                    ChucDanh = x.ChucDanh ?? string.Empty,
                    TenDonVi = x.TenDonVi ?? string.Empty,
                    Email = x.Email ?? string.Empty,
                    SoDienThoai = x.SoDienThoai ?? string.Empty,
                    IsKhachHang = x.IsKhachHang,
                    IsNhanVien = x.IsNhanVien,
                    Status = true
                });

            if (@params.LoaiKhachHang.HasValue == true && (@params.LoaiKhachHang == 1 || @params.LoaiKhachHang == 2))
            {
                query = query.Where(x => x.LoaiKhachHang == @params.LoaiKhachHang);
            }

            if (@params.LoaiDoiTuong.HasValue == true)
            {
                query = query.Where(x => @params.LoaiDoiTuong == 1 ? (x.IsKhachHang == true) : (x.IsNhanVien == true));
            }

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.Ma))
                {
                    var keyword = @params.Filter.Ma.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            return await PagedList<DoiTuongViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<DoiTuongViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.DoiTuongs.AsNoTracking().FirstOrDefaultAsync(x => x.DoiTuongId == id);
            var result = _mp.Map<DoiTuongViewModel>(entity);
            return result;
        }

        public async Task<DoiTuongViewModel> InsertAsync(DoiTuongViewModel model)
        {
            var entity = _mp.Map<DoiTuong>(model);
            await _db.DoiTuongs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<DoiTuongViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(DoiTuongViewModel model)
        {
            var entity = await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == model.DoiTuongId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
