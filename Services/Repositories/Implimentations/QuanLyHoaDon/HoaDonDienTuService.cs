using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.QuanLyHoaDon;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Enums;
using Services.Helper;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
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
    public class HoaDonDienTuService : IHoaDonDienTuService
    {
        Datacontext _db;
        IMapper _mp;
        ILoaiTienService _LoaiTienService;
        IHttpContextAccessor _IHttpContextAccessor;
        IHostingEnvironment _hostingEnvironment;

        public HoaDonDienTuService(
            Datacontext datacontext,
            IMapper mapper,
            ILoaiTienService LoaiTienService,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment IHostingEnvironment)
        {
            _db = datacontext;
            _mp = mapper;
            _LoaiTienService = LoaiTienService;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = IHostingEnvironment;
        }

        private List<TrangThai> TrangThaiHoaDons = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = 1, Ten = "Hóa đơn gốc", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Hóa đơn xóa bỏ", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Hóa đơn thay thế", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Hóa đơn điều chỉnh", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Hóa đơn điều chỉnh tăng", TrangThaiChaId = 4, Level = 0 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Hóa đơn điều chỉnh giảm", TrangThaiChaId = 4, Level = 0 },
            new TrangThai(){ TrangThaiId = 7, Ten = "Hóa đơn điều chỉnh thông tin", TrangThaiChaId = 4, Level = 0 },
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
        };

        private List<TrangThai> TrangThaiGuiHoaDons = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = 1, Ten = "Chưa gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Đang gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Gửi hóa đơn cho khách hàng lỗi", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Đã gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Khách hàng đã xem hóa đơn", TrangThaiChaId = 4, Level = 0 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Khách hàng chưa xem hóa đơn", TrangThaiChaId = 4, Level = 0 },
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
        };

        private List<TrangThai> TreeTrangThais = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 1, Ten = "Chưa phát hành", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Đang phát hành", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Phát hành lỗi", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Đã phát hành", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Chưa gửi hóa đơn cho khách hàng", TrangThaiChaId = 4, Level = 2 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Đang gửi hóa đơn cho khách hàng", TrangThaiChaId = 4, Level = 2 },
            new TrangThai(){ TrangThaiId = 7, Ten = "Gửi hóa đơn cho khách hàng lỗi", TrangThaiChaId = 4, Level = 2 },
            new TrangThai(){ TrangThaiId = 8, Ten = "Đã gửi hóa đơn cho khách hàng", TrangThaiChaId = 4, Level = 2 },
        };

        public async Task<bool> CheckSoHoaDonAsync(string SoHoaDon) // 1: nvk, 2: qttu
        {
            bool result = _db.HoaDonDienTus
                .Any(x => x.SoHoaDon == SoHoaDon);

            return result;
        }

        //public async Task<string> CreateSoChungTuAsync()
        //{
        //    var config = await _db.ConfigTienTos.FirstOrDefaultAsync(x => x.MaChucNang == "HDDT");
        //    if (!string.IsNullOrEmpty(config.SoChungTu))
        //    {
        //        return config.SoChungTu.IncreaseSoChungTu();
        //    }
        //    else
        //    {
        //        return config.MaLoai + "00001";
        //    }
        //}

        public async Task<bool> DeleteAsync(string id)
        {
            var hoaDonChiTiets = await _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == id).ToListAsync();
            _db.HoaDonDienTuChiTiets.RemoveRange(hoaDonChiTiets);

            var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
            _db.HoaDonDienTus.Remove(entity);
            return await _db.SaveChangesAsync() == hoaDonChiTiets.Count + 1;
        }

        public async Task<ThamChieuModel> DeleteRangeHoaDonDienTuAsync(List<HoaDonDienTuViewModel> list)
        {
            ThamChieuModel result = new ThamChieuModel();
            result.List = new List<ThamChieuModel>();
            result.RemovedList = new List<ThamChieuModel>();

            foreach (var item in list)
            {
                result.RemovedList.Add(new ThamChieuModel
                {
                    ChungTuId = item.HoaDonDienTuId,
                    LoaiChungTu = item.LoaiHoaDon == 1 ? BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG : BusinessOfType.HOA_DON_BAN_HANG,
                    NgayHoaDon = item.NgayHoaDon,
                    SoChungTu = !string.IsNullOrEmpty(item.SoHoaDon) ? item.SoHoaDon : "<Chưa cấp số>"
                });
            }

            result.SoChungTuDuocXuLy = list.Count();
            result.SoChungTuThanhCong = result.RemovedList.Count();
            result.SoChungTuKhongThanhCong = result.List.Count();

            return result;
        }

        public async Task<List<HoaDonDienTuViewModel>> GetAllAsync()
        {
            IQueryable<HoaDonDienTuViewModel> query = _db.HoaDonDienTus
                .ProjectTo<HoaDonDienTuViewModel>(_mp.ConfigurationProvider);

            List<HoaDonDienTuViewModel> result = await query.ToListAsync();
            return result;
        }

        public async Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingAsync(HoaDonParams pagingParams)
        {
            IQueryable<HoaDonDienTuViewModel> query = _db.HoaDonDienTus
                .OrderByDescending(x => x.NgayHoaDon)
                .ThenByDescending(x => x.NgayLap)
                .ThenByDescending(x => x.SoHoaDon)
                .Select(hd => new HoaDonDienTuViewModel
                {
                    HoaDonDienTuId = hd.HoaDonDienTuId,
                    NgayHoaDon = hd.NgayHoaDon,
                    NgayLap = hd.NgayLap,
                    SoHoaDon = hd.SoHoaDon,
                    MauHoaDonId = hd.MauHoaDonId ?? string.Empty,
                    MauHoaDon = _mp.Map<MauHoaDonViewModel>(_db.MauHoaDons.FirstOrDefault(x=>x.MauHoaDonId == hd.MauHoaDonId)),
                    KhachHangId = hd.KhachHangId ?? string.Empty,
                    KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x=>x.DoiTuongId == hd.KhachHangId)),
                    NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                    NhanVienBanHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.NhanVienBanHangId)),
                    LoaiTienId = hd.LoaiTienId ?? string.Empty,
                    LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x=>x.LoaiTienId == hd.LoaiTienId)),
                    TyGia = hd.TyGia ?? 1,
                    TrangThai = hd.TrangThai,
                    TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                    MaTraCuu = hd.MaTraCuu,
                    TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                    KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                    SoLanChuyenDoi = hd.SoLanChuyenDoi,
                    LyDoXoaBo = hd.LyDoXoaBo,
                    LoaiHoaDon = hd.LoaiHoaDon,
                    LoaiChungTu = hd.LoaiChungTu,
                    TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x=>x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x=>x.ThanhTien)
                });

            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                string keyword = pagingParams.Keyword.ToUpper().ToTrim();

                query = query.Where(x => x.NgayHoaDon.Value.ToString("dd/MM/yyyy").Contains(keyword) ||
                                                    x.NgayLap.Value.ToString("dd/MM/yyyy").ToString().Contains(keyword) ||
                                                    x.SoHoaDon.ToString().Contains(keyword) ||
                                                    (x.KhachHang.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                                    (x.KhachHang.Ten ?? string.Empty).ToUpper().Contains(keyword) ||
                                                    x.KhachHang.Ten.Contains(keyword) ||
                                                    (x.NguoiLap.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                                    (x.NguoiLap.Ten ?? string.Empty).ToUpper().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
            {
                DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
                DateTime toDate = DateTime.Parse(pagingParams.ToDate);
                query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                        DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
            }

            if (pagingParams.TrangThaiHoaDonDienTu.HasValue && pagingParams.TrangThaiHoaDonDienTu != -1)
            {
                if (pagingParams.TrangThaiHoaDonDienTu != 4)
                { 
                    //không phải hoá đơn điều chỉnh, hoặc chọn từng loại hóa đơn điều chỉnh
                    query = query.Where(x => x.TrangThai == pagingParams.TrangThaiHoaDonDienTu);
                }
                else
                {
                    //là hóa đơn điều chỉnh
                    query = query.Where(x => x.TrangThai == 5 || x.TrangThai == 6 || x.TrangThai == 7);
                }
            }

            if (pagingParams.TrangThaiPhatHanh.HasValue)
            {
                query = query.Where(x => x.TrangThaiPhatHanh == pagingParams.TrangThaiHoaDonDienTu);
            }

            if (pagingParams.TrangThaiGuiHoaDon.HasValue)
            {
                query = query.Where(x => x.TrangThai == pagingParams.TrangThaiGuiHoaDon);
            }

            if (pagingParams.TrangThaiChuyenDoi.HasValue)
            {
                query = query.Where(x => pagingParams.TrangThaiChuyenDoi == false ? x.SoLanChuyenDoi == 0 : x.SoLanChuyenDoi != 0);
            }

            #region Filter and Sort
            if (pagingParams.Filter != null)
            {     
                if (pagingParams.Filter.NgayHoaDon.HasValue)
                {
                    query = query.Where(x => x.NgayHoaDon.Value.ToString("dd/MM/yyyy") == pagingParams.Filter.NgayHoaDon.Value.ToString("dd/MM/yyyy"));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.SoHoaDon))
                {
                    query = query.Where(x => x.SoHoaDon.ToUpper().ToUnSign().Contains(pagingParams.Filter.SoHoaDon.ToUnSign()) || x.SoHoaDon.ToUpper().Contains(pagingParams.Filter.SoHoaDon));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.MauHoaDon.TenMauSo))
                {
                    query = query.Where(x => x.MauHoaDon.TenMauSo.ToUpper().ToUnSign().Contains(pagingParams.Filter.MauHoaDon.TenMauSo.ToUnSign()) || x.MauHoaDon.TenMauSo.ToUpper().Contains(pagingParams.Filter.MauHoaDon.TenMauSo));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.MauHoaDon.MauSo))
                {
                    query = query.Where(x => x.MauHoaDon.MauSo.ToUpper().ToUnSign().Contains(pagingParams.Filter.MauHoaDon.MauSo.ToUnSign()) || x.MauHoaDon.MauSo.ToUpper().Contains(pagingParams.Filter.MauHoaDon.MauSo));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.Ma))
                {
                    query = query.Where(x => x.KhachHang.Ma.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.Ma.ToUnSign()) || x.KhachHang.Ma.ToUpper().Contains(pagingParams.Filter.KhachHang.Ma));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.Ten))
                {
                    query = query.Where(x => x.KhachHang.Ten.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.Ten.ToUnSign()) || x.KhachHang.Ten.ToUpper().Contains(pagingParams.Filter.KhachHang.Ten));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.DiaChi))
                {
                    query = query.Where(x => x.KhachHang.DiaChi.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.DiaChi.ToUnSign()) || x.KhachHang.DiaChi.ToUpper().Contains(pagingParams.Filter.KhachHang.DiaChi));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.MaSoThue))
                {
                    query = query.Where(x => x.KhachHang.MaSoThue.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.MaSoThue.ToUnSign()) || x.KhachHang.MaSoThue.ToUpper().Contains(pagingParams.Filter.KhachHang.MaSoThue));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.HoTenNguoiMuaHang))
                {
                    query = query.Where(x => x.KhachHang.HoTenNguoiMuaHang.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.HoTenNguoiMuaHang.ToUnSign()) || x.KhachHang.HoTenNguoiMuaHang.ToUpper().Contains(pagingParams.Filter.KhachHang.HoTenNguoiMuaHang));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.NhanVienBanHang.Ten))
                {
                    query = query.Where(x => x.NhanVienBanHang.Ten.ToUpper().ToUnSign().Contains(pagingParams.Filter.NhanVienBanHang.Ten.ToUnSign()) || x.NhanVienBanHang.Ten.ToUpper().Contains(pagingParams.Filter.NhanVienBanHang.Ten));
                }
                if (pagingParams.Filter.TongTienThanhToan.HasValue)
                {
                    query = query.Where(x => x.TongTienThanhToan.ToString().Contains(pagingParams.Filter.TongTienThanhToan.ToString()));
                }
                if (pagingParams.Filter.LoaiHoaDon.HasValue && pagingParams.Filter.LoaiHoaDon != -1)
                {
                    query = query.Where(x => x.LoaiHoaDon == pagingParams.Filter.LoaiHoaDon);
                }
                if (pagingParams.Filter.TrangThai.HasValue && pagingParams.Filter.TrangThai != -1)
                {
                    if (pagingParams.Filter.TrangThai != 4)
                    {
                        query = query.Where(x => x.TrangThai == pagingParams.Filter.TrangThai);
                    }
                    else
                    {
                        query = query.Where(x => x.TrangThai == 5 || x.TrangThai == 6 || x.TrangThai == 7);
                    }
                }
                if (pagingParams.Filter.TrangThaiPhatHanh.HasValue && pagingParams.Filter.TrangThaiPhatHanh != -1)
                {
                    query = query.Where(x => x.TrangThaiPhatHanh == pagingParams.Filter.TrangThaiPhatHanh);
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.MaTraCuu))
                {
                    query = query.Where(x => x.MaTraCuu.ToUpper().ToUnSign().Contains(pagingParams.Filter.MaTraCuu.ToUnSign()) || x.MaTraCuu.ToUpper().Contains(pagingParams.Filter.MaTraCuu));
                }
                if (pagingParams.Filter.TrangThaiGuiHoaDon.HasValue && pagingParams.Filter.TrangThaiGuiHoaDon != -1)
                {
                    if (pagingParams.Filter.TrangThaiGuiHoaDon != 4)
                    {
                        query = query.Where(x => x.TrangThaiGuiHoaDon == pagingParams.Filter.TrangThaiGuiHoaDon);
                    }
                    else
                    {
                        query = query.Where(x => x.TrangThaiGuiHoaDon == 5 || x.TrangThaiGuiHoaDon == 6);
                    }
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.HoTenNguoiNhanHD))
                {
                    query = query.Where(x => x.KhachHang.HoTenNguoiNhanHD.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.HoTenNguoiNhanHD.ToUnSign()) || x.KhachHang.HoTenNguoiNhanHD.ToUpper().Contains(pagingParams.Filter.KhachHang.HoTenNguoiNhanHD));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.EmailNguoiNhanHD))
                {
                    query = query.Where(x => x.KhachHang.EmailNguoiNhanHD.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.EmailNguoiNhanHD.ToUnSign()) || x.KhachHang.EmailNguoiNhanHD.ToUpper().Contains(pagingParams.Filter.KhachHang.EmailNguoiNhanHD));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.KhachHang.SoDienThoaiNguoiNhanHD))
                {
                    query = query.Where(x => x.KhachHang.SoDienThoaiNguoiNhanHD.ToUpper().ToUnSign().Contains(pagingParams.Filter.KhachHang.SoDienThoaiNguoiNhanHD.ToUnSign()) || x.KhachHang.SoDienThoaiNguoiNhanHD.ToUpper().Contains(pagingParams.Filter.KhachHang.SoDienThoaiNguoiNhanHD));
                }
                if (pagingParams.Filter.KhachHangDaNhan.HasValue)
                {
                    query = query.Where(x => x.KhachHangDaNhan == pagingParams.Filter.KhachHangDaNhan.HasValue);
                }
                if (pagingParams.Filter.SoLanChuyenDoi.HasValue)
                {
                    query = query.Where(x => x.SoLanChuyenDoi == pagingParams.Filter.SoLanChuyenDoi);
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.LyDoXoaBo))
                {
                    query = query.Where(x => x.LyDoXoaBo.ToUpper().ToUnSign().Contains(pagingParams.Filter.LyDoXoaBo.ToUnSign()) || x.LyDoXoaBo.ToUpper().Contains(pagingParams.Filter.LyDoXoaBo));
                }
                if (pagingParams.Filter.LoaiChungTu.HasValue)
                {
                    query = query.Where(x => x.LoaiChungTu == pagingParams.Filter.LoaiChungTu);
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.TaiLieuDinhKem))
                {
                    query = query.Where(x => x.TaiLieuDinhKem.ToUpper().ToUnSign().Contains(pagingParams.Filter.TaiLieuDinhKem.ToUnSign()) || x.TaiLieuDinhKem.ToUpper().Contains(pagingParams.Filter.TaiLieuDinhKem));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.NguoiLap.Ten))
                {
                    query = query.Where(x => x.NguoiLap.Ten.ToUpper().ToUnSign().Contains(pagingParams.Filter.NguoiLap.Ten.ToUnSign()) || x.NguoiLap.Ten.ToUpper().Contains(pagingParams.Filter.NguoiLap.Ten));
                }
                if (pagingParams.Filter.NgayLap.HasValue)
                {
                    query = query.Where(x => x.NgayLap.Value.ToString("dd/MM/yyyy").Contains(pagingParams.Filter.NgayLap.Value.ToString("dd/MM/yyyy")));
                }
            }
            if (!string.IsNullOrEmpty(pagingParams.SortKey))
            {
                if (pagingParams.SortKey == "NgayHoaDon" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayHoaDon);
                }
                if (pagingParams.SortKey == "NgayHoaDon" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayHoaDon);
                }

                if (pagingParams.SortKey == "NgayLap" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayLap);
                }
                if (pagingParams.SortKey == "NgayLap" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayLap);
                }

                if (pagingParams.SortKey == "SoHoaDon" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoHoaDon);
                }
                if (pagingParams.SortKey == "SoHoaDon" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoHoaDon);
                }

                if (pagingParams.SortKey == "TongTienThanhToan" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TongTienThanhToan);
                }
                if (pagingParams.SortKey == "TongTienThanhToan" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TongTienThanhToan);
                }

            }
            #endregion

            return await PagedList<HoaDonDienTuViewModel>
                    .CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
        }



        public async Task<HoaDonDienTuViewModel> GetByIdAsync(string id)
        {
            var result = from hd in _db.HoaDonDienTus
                         join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                         from mhd in tmpMauHoaDons.DefaultIfEmpty()
                         join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                         from kh in tmpKhachHangs.DefaultIfEmpty()
                         join nl in _db.DoiTuongs on hd.NguoiLapId equals nl.DoiTuongId into tmpNguoiLaps
                         from nl in tmpNguoiLaps.DefaultIfEmpty()
                         join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                         from lt in tmpLoaiTiens.DefaultIfEmpty()
                         where hd.HoaDonDienTuId == id
                         select new HoaDonDienTuViewModel
                         {
                             HoaDonDienTuId = hd.HoaDonDienTuId,
                             NgayHoaDon = hd.NgayHoaDon,
                             NgayLap = hd.NgayLap,
                             SoHoaDon = hd.SoHoaDon,
                             MauHoaDonId = mhd.MauHoaDonId ?? string.Empty,
                             MauHoaDon = mhd != null ? _mp.Map<MauHoaDonViewModel>(mhd) : null,
                             KhachHangId = kh.DoiTuongId ?? string.Empty,
                             KhachHang = kh != null ? _mp.Map<DoiTuongViewModel>(kh) : null,
                             LoaiTienId = lt.LoaiTienId ?? string.Empty,
                             LoaiTien = lt != null ? _mp.Map<LoaiTienViewModel>(lt) : null,
                             TyGia = hd.TyGia ?? 1,
                             TrangThai = hd.TrangThai,
                             TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                             MaTraCuu = hd.MaTraCuu,
                             TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                             KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                             SoLanChuyenDoi = hd.SoLanChuyenDoi,
                             LyDoXoaBo = hd.LyDoXoaBo,
                             LoaiHoaDon = hd.LoaiHoaDon,
                             LoaiChungTu = hd.LoaiChungTu,
                             HoaDonChiTiets = (
                                                from hdct in _db.HoaDonDienTuChiTiets
                                                join hd in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                                from hd in tmpHoaDons.DefaultIfEmpty()
                                                join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                                from vt in tmpHangHoas.DefaultIfEmpty()
                                                join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                                from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                                where hdct.HoaDonDienTuId == id
                                                orderby vt.Ma descending
                                                select new HoaDonDienTuChiTietViewModel
                                                {
                                                    HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                    HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                                                    HoaDon = hd != null ? _mp.Map<HoaDonDienTuViewModel>(hd) : null,
                                                    HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                                                    HangHoaDichVu = vt != null ? _mp.Map<HangHoaDichVuViewModel>(vt) : null,
                                                    HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                                    DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                                    DonViTinh = dvt != null ? _mp.Map<DonViTinhViewModel>(dvt) : null,
                                                    SoLuong = hdct.SoLuong,
                                                    DonGia = hdct.DonGia,
                                                    DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                                    ThanhTien = hdct.ThanhTien,
                                                    ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                                    TienChietKhau = hdct.TienChietKhau,
                                                    TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi,
                                                    TienThueGTGT = hdct.TienThueGTGT,
                                                    TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi,
                                                    SoLo = hdct.SoLo,
                                                    HanSuDung = hdct.HanSuDung,
                                                    SoKhung = hdct.SoKhung,
                                                    SoMay = hdct.SoMay
                                                }).ToList(),
                             TaiLieuDinhKem = hd.TaiLieuDinhKem
                         };


            return result.FirstOrDefault();
        }

        public async Task<HoaDonDienTuViewModel> InsertAsync(HoaDonDienTuViewModel model)
        {
            try
            {
                model.HoaDonDienTuId = Guid.NewGuid().ToString();
                model.HoaDonChiTiets = null;

                HoaDonDienTu entity = _mp.Map<HoaDonDienTu>(model);
                entity.CreatedDate = DateTime.Now;
                entity.ModifyDate = entity.CreatedDate;
                entity.CreatedBy = model.ActionUser.UserId;
                entity.ModifyBy = entity.CreatedBy;


                entity.SoHoaDon = entity.SoHoaDon.IncreaseSoChungTu();

                //var config = await _db.ConfigTienTos.FirstOrDefaultAsync(x => x.MaChucNang == "NVK");
                //config.SoChungTu = entity.SoHoaDon;

                entity.SoLanChuyenDoi = 0;

                //if (!string.IsNullOrEmpty(entity.ThamChieu))
                //{
                //    List<ThamChieuModel> thamChieuModels = JsonConvert.DeserializeObject<List<ThamChieuModel>>(entity.ThamChieu);
                //    foreach (ThamChieuModel thamChieuModel in thamChieuModels)
                //    {
                //        if (thamChieuModel.KieuChungTu == BusinessOfType.PHIEU_CHI)
                //        {
                //            PhieuChi phieuChi = await _db.PhieuChis.FirstOrDefaultAsync(x => x.PhieuChiId == thamChieuModel.ChungTuId);
                //            List<ThamChieuModel> phieuChiThamChieus = new List<ThamChieuModel>();
                //            if (!string.IsNullOrEmpty(phieuChi.ThamChieu))
                //            {
                //                phieuChiThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(phieuChi.ThamChieu);
                //                if (!phieuChiThamChieus.Any(x => x.ChungTuId == entity.HoaDonDienTuId))
                //                {
                //                    if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                    {
                //                        phieuChiThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                        });
                //                    }
                //                    else
                //                    {
                //                        phieuChiThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                        });
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                {
                //                    phieuChiThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                    });
                //                }
                //                else
                //                {
                //                    phieuChiThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                    });
                //                }
                //            }
                //            if (phieuChiThamChieus.Count > 0)
                //            {
                //                phieuChi.ThamChieu = JsonConvert.SerializeObject(phieuChiThamChieus);
                //            }
                //        }
                //        if (thamChieuModel.KieuChungTu == BusinessOfType.CHUNG_TU_BAN_HANG)
                //        {
                //            ChungTuBanHang chungTuBH = await _db.ChungTuBanHangs.FirstOrDefaultAsync(x => x.ChungTuBanHangId == thamChieuModel.ChungTuId);
                //            List<ThamChieuModel> chungTuBHThamChieus = new List<ThamChieuModel>();
                //            if (!string.IsNullOrEmpty(chungTuBH.ThamChieu))
                //            {
                //                chungTuBHThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(chungTuBH.ThamChieu);
                //                if (!chungTuBHThamChieus.Any(x => x.ChungTuId == entity.HoaDonDienTuId))
                //                {
                //                    if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                    {
                //                        chungTuBHThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                        });
                //                    }
                //                    else
                //                    {
                //                        chungTuBHThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                        });
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                {
                //                    chungTuBHThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                    });
                //                }
                //                else
                //                {
                //                    chungTuBHThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                    });
                //                }
                //            }
                //            if (chungTuBHThamChieus.Count > 0)
                //            {
                //                chungTuBH.ThamChieu = JsonConvert.SerializeObject(chungTuBHThamChieus);
                //            }
                //        }

                //        if (thamChieuModel.KieuChungTu == BusinessOfType.CHUNG_TU_GIAM_GIA_HANG_BAN)
                //        {
                //            ChungTuGiamGiaHangBan chungTuGGHB = await _db.ChungTuGiamGiaHangBans.FirstOrDefaultAsync(x => x.ChungTuGiamGiaHangBanId == thamChieuModel.ChungTuId);
                //            List<ThamChieuModel> chungTuGGHBThamChieus = new List<ThamChieuModel>();
                //            if (!string.IsNullOrEmpty(chungTuGGHB.ThamChieu))
                //            {
                //                chungTuGGHBThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(chungTuGGHB.ThamChieu);
                //                if (!chungTuGGHBThamChieus.Any(x => x.ChungTuId == entity.HoaDonDienTuId))
                //                {
                //                    if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                    {
                //                        chungTuGGHBThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                        });
                //                    }
                //                    else
                //                    {
                //                        chungTuGGHBThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                        });
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                {
                //                    chungTuGGHBThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                    });
                //                }
                //                else
                //                {
                //                    chungTuGGHBThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                    });
                //                }
                //            }
                //            if (chungTuGGHBThamChieus.Count > 0)
                //            {
                //                chungTuGGHB.ThamChieu = JsonConvert.SerializeObject(chungTuGGHBThamChieus);
                //            }
                //        }

                //        if (thamChieuModel.KieuChungTu == BusinessOfType.CHUNG_TU_TRA_LAI_HANG_MUA)
                //        {
                //            ChungTuTraLaiHangMua chungTuTLHM = await _db.ChungTuTraLaiHangMuas.FirstOrDefaultAsync(x => x.ChungTuTraLaiHangMuaId == thamChieuModel.ChungTuId);
                //            List<ThamChieuModel> chungTuTLHMThamChieus = new List<ThamChieuModel>();
                //            if (!string.IsNullOrEmpty(chungTuTLHM.ThamChieu))
                //            {
                //                chungTuTLHMThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(chungTuTLHM.ThamChieu);
                //                if (!chungTuTLHMThamChieus.Any(x => x.ChungTuId == entity.HoaDonDienTuId))
                //                {
                //                    if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                    {
                //                        chungTuTLHMThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                        });
                //                    }
                //                    else
                //                    {
                //                        chungTuTLHMThamChieus.Add(new ThamChieuModel
                //                        {
                //                            ChungTuId = entity.HoaDonDienTuId,
                //                            SoChungTu = entity.SoHoaDon,
                //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                        });
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                if (entity.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                //                {
                //                    chungTuTLHMThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
                //                    });
                //                }
                //                else
                //                {
                //                    chungTuTLHMThamChieus.Add(new ThamChieuModel
                //                    {
                //                        ChungTuId = entity.HoaDonDienTuId,
                //                        SoChungTu = entity.SoHoaDon,
                //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
                //                    });
                //                }
                //            }
                //            if (chungTuTLHMThamChieus.Count > 0)
                //            {
                //                chungTuTLHM.ThamChieu = JsonConvert.SerializeObject(chungTuTLHMThamChieus);
                //            }
                //        }
                //    }
                //}

                await _db.HoaDonDienTus.AddAsync(entity);
                await _db.SaveChangesAsync();
                var result = _mp.Map<HoaDonDienTuViewModel>(entity);
                return result;
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return null;
        }

        //public async Task<string> PrintChungTuKeToanAsync(ChungTuNghiepVuKhacViewModel model)
        //{
        //    Document doc = new Document();

        //    string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/samples/Chung_tu_ke_toan.doc");
        //    doc.LoadFromFile(docFolder);

        //    CoCauToChuc coCauToChuc = await _db.CoCauToChucs
        //                    .AsNoTracking()
        //                    .Where(x => int.Parse(x.CapToChuc) == 0)
        //                    .FirstOrDefaultAsync();

        //    var _thongTinIn = _db.CoCauToChuc_ThongTinInBaoCaos
        //                        .AsNoTracking()
        //                        .FirstOrDefault();
        //    doc.Replace("<DonVi>", _thongTinIn.TenDonVi ?? string.Empty, true, true);
        //    doc.Replace("<DiaChi>", _thongTinIn.DiaChi ?? string.Empty, true, true);

        //    if (model.IsChungTuNVK == true || model.IsChungTuQTTU == true || model.IsChungTuXLCLTG == true || model.IsChungTuXLCLTGTuDGLTKNT == true)
        //    {
        //        DoiTuong doiTuong = GetDoiTuongInChiTiet(model.ChungTuNghiepVuKhacChiTiets);

        //        doc.Replace("<TenDoiTuong>", doiTuong != null ? (doiTuong.Ten ?? string.Empty) : string.Empty, true, true);
        //        doc.Replace("<DiaChiDoiTuong>", doiTuong != null ? (doiTuong.DiaChi ?? string.Empty) : string.Empty, true, true);
        //    }
        //    if (model.IsChungTuBuTruCongNo == true)
        //    {
        //        doc.Replace("<TenDoiTuong>", model.DoiTuong.Ten ?? string.Empty, true, true);
        //        doc.Replace("<DiaChiDoiTuong>", model.DoiTuong.DiaChi ?? string.Empty, true, true);
        //    }
        //    if (model.IsChungTuXLCLTGTuTinhTGXQ == true)
        //    {
        //        doc.Replace("<TenDoiTuong>", string.Empty, true, true);
        //        doc.Replace("<DiaChiDoiTuong>", string.Empty, true, true);
        //    }

        //    doc.Replace("<DienGiai>", model.DienGiai ?? string.Empty, true, true);

        //    doc.Replace("<SoChungTu>", model.SoChungTu ?? string.Empty, true, true);
        //    doc.Replace("<NgayChungTu>", model.NgayChungTu.Value.ToString("dd/MM/yyyy") ?? string.Empty, true, true);

        //    List<CoCauToChuc_NguoiKy> coCauToChuc_NguoiKys = _db.CoCauToChuc_NguoiKys.ToList();
        //    var giamdoc = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "GIÁM ĐỐC");
        //    doc.Replace("<GiamDoc>", giamdoc.TieuDeNguoiKy, true, true);

        //    var ketoantruong = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "KẾ TOÁN TRƯỞNG");
        //    doc.Replace("<KeToanTruong>", ketoantruong.TieuDeNguoiKy, true, true);

        //    var nguoilapphieu = _db.CoCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "NGƯỜI LẬP PHIẾU");
        //    doc.Replace("<NguoiLapPhieu>", nguoilapphieu.TieuDeNguoiKy, true, true);

        //    if (coCauToChuc.InTenNguoiKy == true)
        //    {
        //        doc.Replace("<TenGiamDoc>", giamdoc.TenNguoiKy ?? string.Empty, true, true);
        //        doc.Replace("<TenKeToanTruong>", ketoantruong.TenNguoiKy ?? string.Empty, true, true);
        //        doc.Replace("<TenNguoiLapPhieu>", nguoilapphieu.TenNguoiKy ?? string.Empty, true, true);
        //    }
        //    else
        //    {
        //        doc.Replace("<TenGiamDoc>", string.Empty, true, true);
        //        doc.Replace("<TenKeToanTruong>", string.Empty, true, true);
        //        doc.Replace("<TenNguoiLapPhieu>", string.Empty, true, true);
        //    }

        //    List<ChungTuKeToanChiTietViewModel> groupChiTiet = new List<ChungTuKeToanChiTietViewModel>();
        //    if (model.IsChungTuNVK == true || model.IsChungTuBuTruCongNo == true || model.IsChungTuXLCLTG == true || model.IsChungTuXLCLTGTuDGLTKNT == true)
        //    {
        //        groupChiTiet = (from c in model.ChungTuNghiepVuKhacChiTiets
        //                        group c by new
        //                        {
        //                            c.DienGiai,
        //                            c.TaiKhoanNo,
        //                            c.TaiKhoanCo,
        //                        } into gcs
        //                        select new ChungTuKeToanChiTietViewModel()
        //                        {
        //                            DienGiai = gcs.Key.DienGiai,
        //                            GhiNo = gcs.Key.TaiKhoanNo,
        //                            GhiCo = gcs.Key.TaiKhoanCo,
        //                            ThanhTien = gcs.Sum(x => x.SoTien),
        //                        }).ToList();
        //    }

        //    if (model.IsChungTuXLCLTGTuTinhTGXQ == true)
        //    {
        //        groupChiTiet = (from c in model.ChungTuNghiepVuKhacChiTiets
        //                        group c by new
        //                        {
        //                            c.DienGiai,
        //                            c.TaiKhoanNo,
        //                            c.TaiKhoanCo,
        //                        } into gcs
        //                        select new ChungTuKeToanChiTietViewModel()
        //                        {
        //                            DienGiai = gcs.Key.DienGiai,
        //                            GhiNo = gcs.Key.TaiKhoanNo,
        //                            GhiCo = gcs.Key.TaiKhoanCo,
        //                            ThanhTien = gcs.Sum(x => x.ChenhLech),
        //                        }).ToList();
        //    }

        //    if (model.IsChungTuQTTU == true)
        //    {
        //        groupChiTiet = (from c in model.ChungTuNghiepVuKhacChiTiets
        //                        group c by new
        //                        {
        //                            c.DienGiai,
        //                            c.TaiKhoanNo,
        //                            c.TaiKhoanCo,
        //                        } into gcs
        //                        select new ChungTuKeToanChiTietViewModel()
        //                        {
        //                            DienGiai = gcs.Key.DienGiai,
        //                            GhiNo = gcs.Key.TaiKhoanNo,
        //                            GhiCo = gcs.Key.TaiKhoanCo,
        //                            ThanhTien = gcs.Sum(x => x.SoTien),
        //                        }).ToList();

        //        if (model.ChungTuNghiepVuKhacChiTiets.Sum(x => x.TienThueGTGT) != 0)
        //        {
        //            groupChiTiet.AddRange((from c in model.ChungTuNghiepVuKhacChiTiets
        //                                   group c by new
        //                                   {
        //                                       c.TKThueGTGT,
        //                                       c.TaiKhoanCo
        //                                   } into gcs
        //                                   select new ChungTuKeToanChiTietViewModel()
        //                                   {
        //                                       DienGiai = "Thuế GTGT",
        //                                       GhiNo = gcs.Key.TKThueGTGT,
        //                                       GhiCo = gcs.Key.TaiKhoanCo,
        //                                       ThanhTien = gcs.Sum(x => x.TienThueGTGT),
        //                                   }).ToList());
        //        }
        //    }

        //    int line = groupChiTiet.Count();
        //    Table table = null;
        //    Paragraph _par;
        //    string stt = string.Empty;
        //    foreach (Table tb in doc.Sections[0].Tables)
        //    {
        //        if (tb.Rows.Count > 0)
        //        {
        //            foreach (Paragraph par in tb.Rows[0].Cells[0].Paragraphs)
        //            {
        //                stt = par.Text;
        //            }
        //            if (stt.Contains("STT"))
        //            {
        //                table = tb;
        //                break;
        //            }
        //        }
        //    }

        //    int beginRow = 1;
        //    for (int i = 0; i < line - 1; i++)
        //    {
        //        // Clone row
        //        TableRow cl_row = table.Rows[beginRow].Clone();
        //        table.Rows.Insert(beginRow, cl_row);
        //    }

        //    TableRow row = null;
        //    for (int i = 0; i < line; i++)
        //    {
        //        row = table.Rows[i + beginRow];

        //        _par = row.Cells[0].Paragraphs[0];
        //        _par.Text = (i + 1).ToString();

        //        _par = row.Cells[1].Paragraphs[0];
        //        _par.Text = groupChiTiet[i].DienGiai;

        //        _par = row.Cells[2].Paragraphs[0];
        //        _par.Text = groupChiTiet[i].GhiNo;

        //        _par = row.Cells[3].Paragraphs[0];
        //        _par.Text = groupChiTiet[i].GhiCo;

        //        _par = row.Cells[4].Paragraphs[0];
        //        _par.Text = groupChiTiet[i].ThanhTien.Value.FormatQuanity();
        //    }

        //    doc.Replace("<TongThanhTien>", model.TongTienThanhToan.Value.FormatQuanity(), true, true);
        //    doc.Replace("<TongThanhTienBangChu>", model.TongTienThanhToan.Value.ConvertToInWord(), true, true);

        //    string pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf/");

        //    if (!Directory.Exists(pdfFolder))
        //    {
        //        Directory.CreateDirectory(pdfFolder);
        //    }

        //    string pdfFileName = $"chung-tu-ke-toan-{model.ChungTuNghiepVuKhacId}.pdf";
        //    doc.SaveToFile(pdfFolder + pdfFileName, FileFormat.PDF);
        //    return pdfFileName;
        //}

        public async Task<bool> UpdateAsync(HoaDonDienTuViewModel model)
        {
            model.HoaDonChiTiets = null;

            model.ModifyDate = DateTime.Now;
            model.ModifyBy = model.ActionUser.UserId;

            //if (!string.IsNullOrEmpty(model.ThamChieu))
            //{

            //    List<ThamChieuModel> thamChieuModels = JsonConvert.DeserializeObject<List<ThamChieuModel>>(model.ThamChieu);
            //    foreach (ThamChieuModel thamChieuModel in thamChieuModels)
            //    {
            //        if (thamChieuModel.KieuChungTu == BusinessOfType.PHIEU_CHI)
            //        {
            //            PhieuChi phieuChi = await _db.PhieuChis.FirstOrDefaultAsync(x => x.PhieuChiId == thamChieuModel.ChungTuId);
            //            List<ThamChieuModel> phieuChiThamChieus = new List<ThamChieuModel>();
            //            if (!string.IsNullOrEmpty(phieuChi.ThamChieu))
            //            {
            //                phieuChiThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(phieuChi.ThamChieu);
            //                if (!phieuChiThamChieus.Any(x => x.ChungTuId == model.HoaDonDienTuId))
            //                {
            //                    if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                    {
            //                        phieuChiThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                        });
            //                    }
            //                    else
            //                    {
            //                        phieuChiThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                        });
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                {
            //                    phieuChiThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                    });
            //                }
            //                else
            //                {
            //                    phieuChiThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                    });
            //                }
            //            }
            //            if (phieuChiThamChieus.Count > 0)
            //            {
            //                phieuChi.ThamChieu = JsonConvert.SerializeObject(phieuChiThamChieus);
            //            }
            //        }
            //        if (thamChieuModel.KieuChungTu == BusinessOfType.CHUNG_TU_BAN_HANG)
            //        {
            //            ChungTuBanHang chungTuBH = await _db.ChungTuBanHangs.FirstOrDefaultAsync(x => x.ChungTuBanHangId == thamChieuModel.ChungTuId);
            //            List<ThamChieuModel> chungTuBHThamChieus = new List<ThamChieuModel>();
            //            if (!string.IsNullOrEmpty(chungTuBH.ThamChieu))
            //            {
            //                chungTuBHThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(chungTuBH.ThamChieu);
            //                if (!chungTuBHThamChieus.Any(x => x.ChungTuId == model.HoaDonDienTuId))
            //                {
            //                    if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                    {
            //                        chungTuBHThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                        });
            //                    }
            //                    else
            //                    {
            //                        chungTuBHThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                        });
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                {
            //                    chungTuBHThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                    });
            //                }
            //                else
            //                {
            //                    chungTuBHThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                    });
            //                }
            //            }
            //            if (chungTuBHThamChieus.Count > 0)
            //            {
            //                chungTuBH.ThamChieu = JsonConvert.SerializeObject(chungTuBHThamChieus);
            //            }
            //        }

            //        if (thamChieuModel.KieuChungTu == BusinessOfType.CHUNG_TU_GIAM_GIA_HANG_BAN)
            //        {
            //            ChungTuGiamGiaHangBan chungTuGGHB = await _db.ChungTuGiamGiaHangBans.FirstOrDefaultAsync(x => x.ChungTuGiamGiaHangBanId == thamChieuModel.ChungTuId);
            //            List<ThamChieuModel> chungTuGGHBThamChieus = new List<ThamChieuModel>();
            //            if (!string.IsNullOrEmpty(chungTuGGHB.ThamChieu))
            //            {
            //                chungTuGGHBThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(chungTuGGHB.ThamChieu);
            //                if (!chungTuGGHBThamChieus.Any(x => x.ChungTuId == model.HoaDonDienTuId))
            //                {
            //                    if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                    {
            //                        chungTuGGHBThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                        });
            //                    }
            //                    else
            //                    {
            //                        chungTuGGHBThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                        });
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                {
            //                    chungTuGGHBThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                    });
            //                }
            //                else
            //                {
            //                    chungTuGGHBThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                    });
            //                }
            //            }
            //            if (chungTuGGHBThamChieus.Count > 0)
            //            {
            //                chungTuGGHB.ThamChieu = JsonConvert.SerializeObject(chungTuGGHBThamChieus);
            //            }
            //        }

            //        if (thamChieuModel.KieuChungTu == BusinessOfType.CHUNG_TU_TRA_LAI_HANG_MUA)
            //        {
            //            ChungTuTraLaiHangMua chungTuTLHM = await _db.ChungTuTraLaiHangMuas.FirstOrDefaultAsync(x => x.ChungTuTraLaiHangMuaId == thamChieuModel.ChungTuId);
            //            List<ThamChieuModel> chungTuTLHMThamChieus = new List<ThamChieuModel>();
            //            if (!string.IsNullOrEmpty(chungTuTLHM.ThamChieu))
            //            {
            //                chungTuTLHMThamChieus = JsonConvert.DeserializeObject<List<ThamChieuModel>>(chungTuTLHM.ThamChieu);
            //                if (!chungTuTLHMThamChieus.Any(x => x.ChungTuId == model.HoaDonDienTuId))
            //                {
            //                    if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                    {
            //                        chungTuTLHMThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                        });
            //                    }
            //                    else
            //                    {
            //                        chungTuTLHMThamChieus.Add(new ThamChieuModel
            //                        {
            //                            ChungTuId = model.HoaDonDienTuId,
            //                            SoChungTu = model.SoHoaDon,
            //                            KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                        });
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
            //                {
            //                    chungTuTLHMThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG
            //                    });
            //                }
            //                else
            //                {
            //                    chungTuTLHMThamChieus.Add(new ThamChieuModel
            //                    {
            //                        ChungTuId = model.HoaDonDienTuId,
            //                        SoChungTu = model.SoHoaDon,
            //                        KieuChungTu = BusinessOfType.HOA_DON_BAN_HANG
            //                    });
            //                }
            //            }
            //            if (chungTuTLHMThamChieus.Count > 0)
            //            {
            //                chungTuTLHM.ThamChieu = JsonConvert.SerializeObject(chungTuTLHMThamChieus);
            //            }
            //        }
            //    }
            //}

            HoaDonDienTu entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == model.HoaDonDienTuId);
            //_db.ChungTuNghiepVuKhacs.Update(entity);
            _db.Entry(entity).CurrentValues.SetValues(model);
            return await _db.SaveChangesAsync() > 0;
        }

        private string GetLinkExcelFile(string link)
        {
            var filename = "FilesUpload/excels/" + link;
            string url = "";
            if (_IHttpContextAccessor.HttpContext.Request.IsHttps)
            {
                url = "https://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            url = url + "/" + filename;
            return url;
        }

        //private dynamic SaveToExcelChungTuKeToan(ChungTuNghiepVuKhacViewModel model)
        //{
        //    string excelFileName = string.Empty;
        //    string excelPath = string.Empty;

        //    try
        //    {
        //        // Export excel
        //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

        //        if (!Directory.Exists(uploadFolder))
        //        {
        //            Directory.CreateDirectory(uploadFolder);
        //        }
        //        else
        //        {
        //            FileHelper.ClearFolder(uploadFolder);
        //        }

        //        excelFileName = $"chung-tu-ke-toan-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
        //        string excelFolder = $"FilesUpload/excels/{excelFileName}";
        //        excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

        //        // Excel
        //        string _sample = $"docs/samples/Chung_tu_ke_toan.xlsx";
        //        string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

        //        FileInfo file = new FileInfo(_path_sample);
        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            //Don vi
        //            var _thongTinIn = _db.CoCauToChuc_ThongTinInBaoCaos
        //                        .AsNoTracking()
        //                        .FirstOrDefault();
        //            worksheet.Cells[1, 1].Value = _thongTinIn.TenDonVi;
        //            worksheet.Cells[2, 1].Value = _thongTinIn.DiaChi;
        //            // From to time

        //            if (model.IsChungTuNVK == true || model.IsChungTuQTTU == true || model.IsChungTuXLCLTG == true || model.IsChungTuXLCLTGTuDGLTKNT == true)
        //            {
        //                DoiTuong doiTuong = GetDoiTuongInChiTiet(model.ChungTuNghiepVuKhacChiTiets);

        //                worksheet.Cells[8, 1].Value = string.Format("Tên: {0}", doiTuong != null ? doiTuong.Ten : string.Empty);
        //                worksheet.Cells[9, 1].Value = string.Format("Địa chỉ: {0}", doiTuong != null ? doiTuong.DiaChi : string.Empty);
        //            }
        //            if (model.IsChungTuBuTruCongNo == true)
        //            {
        //                worksheet.Cells[8, 1].Value = string.Format("Tên: {0}", model.DoiTuong.Ten);
        //                worksheet.Cells[9, 1].Value = string.Format("Địa chỉ: {0}", model.DoiTuong.DiaChi);
        //            }
        //            if (model.IsChungTuXLCLTGTuTinhTGXQ == true)
        //            {
        //                worksheet.Cells[8, 1].Value = string.Format("Tên: {0}", string.Empty);
        //                worksheet.Cells[9, 1].Value = string.Format("Địa chỉ: {0}", string.Empty);
        //            }

        //            worksheet.Cells[6, 5].Value = string.Format("Số: {0}", model.SoChungTu);
        //            worksheet.Cells[7, 5].Value = string.Format("Ngày: {0}", model.NgayChungTu.Value.ToString("dd/MM/yyyy"));
        //            worksheet.Cells[10, 1].Value = string.Format("Diễn giải: {0}", model.DienGiai);

        //            int begin_row = 13;
        //            int idx = begin_row;
        //            int stt = 1;

        //            List<ChungTuKeToanChiTietViewModel> groupChiTiet = new List<ChungTuKeToanChiTietViewModel>();
        //            if (model.IsChungTuNVK == true || model.IsChungTuBuTruCongNo == true || model.IsChungTuXLCLTG == true || model.IsChungTuXLCLTGTuDGLTKNT == true)
        //            {
        //                groupChiTiet = (from c in model.ChungTuNghiepVuKhacChiTiets
        //                                group c by new
        //                                {
        //                                    c.DienGiai,
        //                                    c.TaiKhoanNo,
        //                                    c.TaiKhoanCo,
        //                                } into gcs
        //                                select new ChungTuKeToanChiTietViewModel()
        //                                {
        //                                    DienGiai = gcs.Key.DienGiai,
        //                                    GhiNo = gcs.Key.TaiKhoanNo,
        //                                    GhiCo = gcs.Key.TaiKhoanCo,
        //                                    ThanhTien = gcs.Sum(x => x.SoTien),
        //                                }).ToList();
        //            }

        //            if (model.IsChungTuXLCLTGTuTinhTGXQ == true)
        //            {
        //                groupChiTiet = (from c in model.ChungTuNghiepVuKhacChiTiets
        //                                group c by new
        //                                {
        //                                    c.DienGiai,
        //                                    c.TaiKhoanNo,
        //                                    c.TaiKhoanCo,
        //                                } into gcs
        //                                select new ChungTuKeToanChiTietViewModel()
        //                                {
        //                                    DienGiai = gcs.Key.DienGiai,
        //                                    GhiNo = gcs.Key.TaiKhoanNo,
        //                                    GhiCo = gcs.Key.TaiKhoanCo,
        //                                    ThanhTien = gcs.Sum(x => x.ChenhLech),
        //                                }).ToList();
        //            }

        //            if (model.IsChungTuQTTU == true)
        //            {
        //                groupChiTiet = (from c in model.ChungTuNghiepVuKhacChiTiets
        //                                group c by new
        //                                {
        //                                    c.DienGiai,
        //                                    c.TaiKhoanNo,
        //                                    c.TaiKhoanCo,
        //                                } into gcs
        //                                select new ChungTuKeToanChiTietViewModel()
        //                                {
        //                                    DienGiai = gcs.Key.DienGiai,
        //                                    GhiNo = gcs.Key.TaiKhoanNo,
        //                                    GhiCo = gcs.Key.TaiKhoanCo,
        //                                    ThanhTien = gcs.Sum(x => x.SoTien),
        //                                }).ToList();

        //                if (model.ChungTuNghiepVuKhacChiTiets.Sum(x => x.TienThueGTGT) != 0)
        //                {
        //                    groupChiTiet.AddRange((from c in model.ChungTuNghiepVuKhacChiTiets
        //                                           group c by new
        //                                           {
        //                                               c.TKThueGTGT,
        //                                               c.TaiKhoanCo
        //                                           } into gcs
        //                                           select new ChungTuKeToanChiTietViewModel()
        //                                           {
        //                                               DienGiai = "Thuế GTGT",
        //                                               GhiNo = gcs.Key.TKThueGTGT,
        //                                               GhiCo = gcs.Key.TaiKhoanCo,
        //                                               ThanhTien = gcs.Sum(x => x.TienThueGTGT),
        //                                           }).ToList());
        //                }
        //            }

        //            int totalRows = groupChiTiet.Count();
        //            worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);

        //            foreach (ChungTuKeToanChiTietViewModel item in groupChiTiet)
        //            {
        //                worksheet.Cells[idx, 1].Value = stt.ToString();
        //                worksheet.Cells[idx, 2].Value = item.DienGiai;
        //                worksheet.Cells[idx, 2, idx, 3].Merge = true;
        //                worksheet.Cells[idx, 4].Value = item.GhiNo;
        //                worksheet.Cells[idx, 5].Value = item.GhiCo;
        //                worksheet.Cells[idx, 6].Value = item.ThanhTien.Value.ToString("#,##0");
        //                idx += 1;
        //                stt += 1;
        //            }

        //            worksheet.Cells[idx, 6].Value = string.Format("{0}", model.TongTienThanhToan.Value.ToString("#,##0"));
        //            worksheet.Cells[idx + 2, 1].Value = string.Format("Thành tiền bằng chữ: {0}", model.TongTienThanhToan.Value.ConvertToInWord());

        //            CoCauToChuc coCauToChuc = _db.CoCauToChucs
        //                   .AsNoTracking()
        //                   .Where(x => int.Parse(x.CapToChuc) == 0)
        //                   .FirstOrDefault();

        //            if (coCauToChuc.InTenNguoiKy == true)
        //            {
        //                List<CoCauToChuc_NguoiKy> coCauToChuc_NguoiKys = _db.CoCauToChuc_NguoiKys.ToList();
        //                var ketoantruong = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "KẾ TOÁN TRƯỞNG");
        //                var giamdoc = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "GIÁM ĐỐC");
        //                var nguoilapphieu = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "NGƯỜI LẬP PHIẾU");

        //                worksheet.Cells[idx + 12, 1].Value = giamdoc.TenNguoiKy;
        //                worksheet.Cells[idx + 12, 3].Value = ketoantruong.TenNguoiKy;
        //                worksheet.Cells[idx + 12, 6].Value = nguoilapphieu.TenNguoiKy;
        //            }

        //            package.SaveAs(new FileInfo(excelPath));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(string.Empty, ex);
        //    }

        //    return new
        //    {
        //        excelFileName,
        //        excelPath
        //    };
        //}

        //private DoiTuong GetDoiTuongInChiTiet(List<ChungTuNghiepVuKhacChiTietViewModel> chungTuNghiepVuKhacChiTiets)
        //{
        //    DoiTuong doiTuong = null;

        //    foreach (ChungTuNghiepVuKhacChiTietViewModel item in chungTuNghiepVuKhacChiTiets)
        //    {
        //        if (!string.IsNullOrEmpty(item.DoiTuongNoId))
        //        {
        //            doiTuong = _db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == item.DoiTuongNoId);
        //            break;
        //        }
        //        if (!string.IsNullOrEmpty(item.DoiTuongCoId))
        //        {
        //            doiTuong = _db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == item.DoiTuongCoId);
        //            break;
        //        }
        //    }

        //    return doiTuong;
        //}

        //public async Task<string> PrintQuyetToanTamUngAsync(ChungTuNghiepVuKhacViewModel model)
        //{
        //    Document doc = new Document();

        //    string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/samples/Giay_thanh_toan_tien_tam_ung.doc");
        //    doc.LoadFromFile(docFolder);

        //    CoCauToChuc coCauToChuc = await _db.CoCauToChucs
        //                    .AsNoTracking()
        //                    .Where(x => int.Parse(x.CapToChuc) == 0)
        //                    .FirstOrDefaultAsync();

        //    ConfigTienTo configTienTo = _db.ConfigTienTos.FirstOrDefault(x => x.TenChucNang.Trim() == "Chứng từ quyết toán tạm ứng");

        //    doc.Replace("<MauCT>", configTienTo != null ? configTienTo.MauSo ?? string.Empty : string.Empty, true, true);
        //    if (configTienTo.LayTheoCoCauToChuc)
        //    {
        //        doc.Replace("<VanBanPL>", coCauToChuc.ThongTu ?? string.Empty, true, true);
        //        doc.Replace("<NgayPhatHanh>", coCauToChuc.NgayPhatHanh ?? string.Empty, true, true);
        //    }
        //    else
        //    {
        //        doc.Replace("<VanBanPL>", configTienTo.ThongTu ?? string.Empty, true, true);
        //        doc.Replace("<NgayPhatHanh>", configTienTo.NgayPhatHanh ?? string.Empty, true, true);
        //    }

        //    var _thongTinIn = _db.CoCauToChuc_ThongTinInBaoCaos
        //                        .AsNoTracking()
        //                        .FirstOrDefault();

        //    doc.Replace("<DonVi>", _thongTinIn != null ? _thongTinIn.TenDonVi ?? string.Empty : string.Empty, true, true);
        //    doc.Replace("<DiaChi>", _thongTinIn != null ? _thongTinIn.DiaChi ?? string.Empty : string.Empty, true, true);

        //    doc.Replace("<SoChungTu>", model.SoChungTu ?? string.Empty, true, true);
        //    doc.Replace("<NgayChungTu>", model.NgayChungTu.Value.ToString("dd/MM/yyyy") ?? string.Empty, true, true);

        //    var doiTuong = await (from dt in _db.DoiTuongs
        //                          join dv in _db.CoCauToChucs on dt.DonVi equals dv.CoCauToChucId into tmpCoCauToChucs
        //                          from dv in tmpCoCauToChucs.DefaultIfEmpty()
        //                          where dt.DoiTuongId == model.NhanVienId
        //                          select new
        //                          {
        //                              TenDoiTuong = dt.Ten,
        //                              BoPhan = dv != null ? dv.TenDonVi : string.Empty
        //                          }).FirstOrDefaultAsync();

        //    doc.Replace("<TenDoiTuong>", doiTuong.TenDoiTuong ?? string.Empty, true, true);
        //    doc.Replace("<BoPhan>", doiTuong.BoPhan ?? string.Empty, true, true);

        //    List<ChungTuNghiepVuKhacChiTietViewModel> listChiTiet = new List<ChungTuNghiepVuKhacChiTietViewModel>();
        //    foreach (ChungTuNghiepVuKhacChiTietViewModel item in model.ChungTuNghiepVuKhacChiTiets)
        //    {
        //        listChiTiet.Add(item);

        //        if (item.TienThueGTGT != 0)
        //        {
        //            ChungTuNghiepVuKhacChiTietViewModel itemThue = new ChungTuNghiepVuKhacChiTietViewModel
        //            {
        //                TaiKhoanNo = item.TKThueGTGT,
        //                TaiKhoanCo = item.TaiKhoanCo,
        //                SoTien = item.TienThueGTGT,
        //            };
        //            listChiTiet.Add(itemThue);
        //        }
        //    }

        //    int line = listChiTiet.Count();
        //    Table table = null;
        //    Paragraph _par;
        //    string stt = string.Empty;
        //    foreach (Table tb in doc.Sections[0].Tables)
        //    {
        //        if (tb.Rows.Count > 0)
        //        {
        //            foreach (Paragraph par in tb.Rows[0].Cells[0].Paragraphs)
        //            {
        //                stt = par.Text;
        //            }
        //            if (stt.Contains("STT"))
        //            {
        //                table = tb;
        //                break;
        //            }
        //        }
        //    }

        //    int beginRow = 1;
        //    for (int i = 0; i < line - 1; i++)
        //    {
        //        // Clone row
        //        TableRow cl_row = table.Rows[beginRow].Clone();
        //        table.Rows.Insert(beginRow, cl_row);
        //    }

        //    TableRow row = null;
        //    for (int i = 0; i < line; i++)
        //    {
        //        row = table.Rows[i + beginRow];

        //        _par = row.Cells[0].Paragraphs[0];
        //        _par.Text = (i + 1).ToString();

        //        _par = row.Cells[1].Paragraphs[0];
        //        _par.Text = listChiTiet[i].TaiKhoanNo;

        //        _par = row.Cells[2].Paragraphs[0];
        //        _par.Text = listChiTiet[i].TaiKhoanCo;

        //        _par = row.Cells[3].Paragraphs[0];
        //        _par.Text = listChiTiet[i].SoTien.Value.FormatQuanity();

        //        _par = row.Cells[4].Paragraphs[0];
        //        _par.Text = model.DienGiai;
        //    }

        //    decimal tongSoTien = listChiTiet.Sum(x => x.SoTien).Value;
        //    doc.Replace("<TongSoTien>", tongSoTien.FormatQuanity(), true, true);
        //    if (model.QuyetToanChoTungLanTamUng.Value)
        //    {
        //        doc.Replace("<SoTienTamUng>", model.SoTienTamUng.Value.FormatQuanity(), true, true);
        //        doc.Replace("<SoTienDaChi>", tongSoTien.FormatQuanity(), true, true);

        //        if (model.SoTienThuaThieu >= 0)
        //        {
        //            doc.Replace("<SoTamUngChiKhongHet>", model.SoTienThuaThieu.Value.FormatQuanity() + " VND", true, true);
        //            doc.Replace("<SoChiVuotSoTamUng>", string.Empty, true, true);
        //        }
        //        else
        //        {
        //            doc.Replace("<SoChiVuotSoTamUng>", model.SoTienThuaThieu.Value.FormatQuanity() + " VND", true, true);
        //            doc.Replace("<SoTamUngChiKhongHet>", string.Empty, true, true);
        //        }
        //    }
        //    else
        //    {
        //        BaoCaoParams baoCaoParams = new BaoCaoParams
        //        {
        //            DoiTuongId = model.NhanVienId,
        //            TaiKhoan = listChiTiet.Select(x => x.TaiKhoanCo).FirstOrDefault(),
        //            TuNgay = model.NgayChungTu.Value.ToString("yyyy-MM-dd"),
        //            DenNgay = model.NgayChungTu.Value.ToString("yyyy-MM-dd")
        //        };

        //        List<TaiKhoanKeToanViewModel> taiKhoanKeToans = await _taiKhoanKeToanService.GetTKKeToanConByDanhSachLocTaiKhoanAsync(baoCaoParams.TaiKhoan);
        //        List<string> soTaiKhoanKeToans = taiKhoanKeToans.Select(x => x.SoTaiKhoan).ToList();
        //        List<SoCongNoDoiTuong> listSoCongNoDoiTuongAll = await _db.SoCongNoDoiTuongs.ToListAsync();
        //        List<SoCongNoDoiTuongViewModel> list = await _soCongNoDoiTuongService.GetListBaoCaoCongNoNhanVienAsync(listSoCongNoDoiTuongAll, soTaiKhoanKeToans, baoCaoParams, model.ChungTuNghiepVuKhacId);

        //        decimal SoTienTamUng = 0;
        //        if (list.Sum(x => x.NoCuoiKy) > 0)
        //        {
        //            SoTienTamUng = list.Sum(x => x.NoCuoiKy).Value;
        //            doc.Replace("<SoTienTamUng>", SoTienTamUng.FormatQuanity(), true, true);
        //        }
        //        if (list.Sum(x => x.CoCuoiKy) > 0)
        //        {
        //            SoTienTamUng = list.Sum(x => x.CoCuoiKy).Value * -1;
        //            doc.Replace("<SoTienTamUng>", SoTienTamUng.FormatQuanity(), true, true);
        //        }

        //        doc.Replace("<SoTienDaChi>", tongSoTien.FormatQuanity(), true, true);

        //        if (SoTienTamUng - tongSoTien >= 0)
        //        {
        //            doc.Replace("<SoTamUngChiKhongHet>", (SoTienTamUng - tongSoTien).FormatQuanity() + " VND", true, true);
        //            doc.Replace("<SoChiVuotSoTamUng>", string.Empty, true, true);
        //        }
        //        else
        //        {
        //            doc.Replace("<SoChiVuotSoTamUng>", (tongSoTien - SoTienTamUng).FormatQuanity() + " VND", true, true);
        //            doc.Replace("<SoTamUngChiKhongHet>", string.Empty, true, true);
        //        }
        //    }

        //    CoCauToChuc_NguoiKy giamdoc = _db.CoCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "GIÁM ĐỐC");
        //    doc.Replace("<GiamDoc>", giamdoc.TieuDeNguoiKy ?? string.Empty, true, true);

        //    CoCauToChuc_NguoiKy ketoantruong = _db.CoCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "KẾ TOÁN TRƯỞNG");
        //    doc.Replace("<KeToanTruong>", ketoantruong.TieuDeNguoiKy ?? string.Empty, true, true);

        //    if (coCauToChuc.InTenNguoiKy == true)
        //    {
        //        doc.Replace("<TenGiamDoc>", giamdoc.TenNguoiKy ?? string.Empty, true, true);
        //        doc.Replace("<TenKeToanTruong>", ketoantruong.TenNguoiKy ?? string.Empty, true, true);
        //    }
        //    else
        //    {
        //        doc.Replace("<TenGiamDoc>", string.Empty, true, true);
        //        doc.Replace("<TenKeToanTruong>", string.Empty, true, true);
        //    }

        //    string pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf/");

        //    if (!Directory.Exists(pdfFolder))
        //    {
        //        Directory.CreateDirectory(pdfFolder);
        //    }

        //    string pdfFileName = $"chung-tu-ke-toan-{model.ChungTuNghiepVuKhacId}.pdf";
        //    doc.SaveToFile(pdfFolder + pdfFileName, FileFormat.PDF);
        //    return pdfFileName;
        //}

        //public async Task<List<ChungTuNghiepVuKhacViewModelTemp>> ImportCTNVKAsync(IList<IFormFile> files, UserViewModel ActionUser)
        //{
        //    List<ChungTuNghiepVuKhacViewModelTemp> result = new List<ChungTuNghiepVuKhacViewModelTemp>();

        //    var upload = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
        //    var fileUrl = upload.InsertFileExcel(files);
        //    if (!string.IsNullOrEmpty(fileUrl))
        //    {
        //        FileInfo file = new FileInfo(fileUrl);
        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            // Open sheet1
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            // Get total all row
        //            int totalRows = worksheet.Dimension.Rows;
        //            // Begin row
        //            int begin_row = 2;

        //            List<DoiTuong> _doiTuongs = await _db.DoiTuongs.ToListAsync();
        //            List<TaiKhoanNganHang> _taiKhoanNganHangs = await _db.TaiKhoanNganHangs.ToListAsync();
        //            List<NganHang> _nganHangs = await _db.NganHangs.ToListAsync();
        //            List<TaiKhoanKeToan> _taiKhoanKeToans = await _db.TaiKhoanKeToans.ToListAsync();
        //            List<ChungTuNghiepVuKhac> _chungTuNghiepVuKhacs = await _db.ChungTuNghiepVuKhacs.ToListAsync();

        //            for (int i = begin_row; i <= totalRows; i++)
        //            {
        //                ChungTuNghiepVuKhacViewModelTemp item = new ChungTuNghiepVuKhacViewModelTemp();
        //                item.Row = i;
        //                item.IsChungTuNVK = true;
        //                item.IsChungTuQTTU = false;
        //                item.IsChungTuBuTruCongNo = false;
        //                item.HasError = true;

        //                item.SoChungTu = (worksheet.Cells[i, 3].Value ?? string.Empty).ToString().Trim();

        //                ChungTuNghiepVuKhacViewModelTemp copyData = result.FirstOrDefault(x => x.SoChungTu == item.SoChungTu);
        //                if (copyData != null)
        //                {
        //                    item = (ChungTuNghiepVuKhacViewModelTemp)copyData.Clone();
        //                    item.Row = i;
        //                    item.HasError = true;
        //                    item.StatusMessage = string.Empty;

        //                    ChungTuNghiepVuKhacViewModelTemp checkError = result.FirstOrDefault(x => x.SoChungTu == item.SoChungTu && x.HasError == true);
        //                    if (checkError != null)
        //                    {
        //                        item.StatusMessage = $"Dòng chi tiết liên quan (dòng số <{checkError.Row}>) bị lỗi.";
        //                    }
        //                }
        //                else
        //                {
        //                    #region NgayHachToan
        //                    item.NgayHachToan = (worksheet.Cells[i, 1].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.NgayHachToan))
        //                    {
        //                        item.StatusMessage = "<Ngày hạch toán> không được bỏ trống.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.NgayHachToan.IsValidDate() == false)
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <Ngày hạch toán> không hợp lệ.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage))
        //                    {
        //                        item.NgayHachToan = item.NgayHachToan.ParseExact().ToString("dd/MM/yyyy");
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && ActionUser.KyKeToanFromDate.IsValidDate() && ActionUser.KyKeToanToDate.IsValidDate())
        //                    {
        //                        DateTime NgayHachToan = item.NgayHachToan.ParseExact();
        //                        DateTime KyKeToanFromDate = DateTime.Parse(ActionUser.KyKeToanFromDate);
        //                        DateTime KyKeToanToDate = DateTime.Parse(ActionUser.KyKeToanToDate);
        //                        if (NgayHachToan > KyKeToanToDate || NgayHachToan < KyKeToanFromDate)
        //                        {
        //                            item.StatusMessage = $"Ngày hạch toán phải nằm trong kỳ kế toán hiện tại Từ ngày<{KyKeToanFromDate:dd/MM/yyyy}> - Đến ngày<{KyKeToanToDate:dd/MM/yyyy}>";
        //                        }
        //                    }
        //                    #endregion

        //                    #region NgayChungTu
        //                    item.NgayChungTu = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.NgayChungTu))
        //                    {
        //                        item.StatusMessage = "<Ngày chứng từ> không được bỏ trống.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.NgayChungTu.IsValidDate() == false)
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <Ngày chứng từ> không hợp lệ.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage))
        //                    {
        //                        item.NgayChungTu = item.NgayChungTu.ParseExact().ToString("dd/MM/yyyy");
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && ActionUser.KyKeToanFromDate.IsValidDate() && ActionUser.KyKeToanToDate.IsValidDate())
        //                    {
        //                        DateTime NgayChungTu = item.NgayChungTu.ParseExact();
        //                        DateTime KyKeToanFromDate = DateTime.Parse(ActionUser.KyKeToanFromDate);
        //                        DateTime KyKeToanToDate = DateTime.Parse(ActionUser.KyKeToanToDate);
        //                        if (NgayChungTu > KyKeToanToDate || NgayChungTu < KyKeToanFromDate)
        //                        {
        //                            item.StatusMessage = $"Ngày chứng từ phải nằm trong kỳ kế toán hiện tại Từ ngày<{KyKeToanFromDate:dd/MM/yyyy}> - Đến ngày<{KyKeToanToDate:dd/MM/yyyy}>";
        //                        }
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.NgayChungTu.ParseExact() > item.NgayHachToan.ParseExact())
        //                    {
        //                        item.StatusMessage = $"Ngày hạch toán phải lớn hơn hoặc bằng Ngày chứng từ <{item.NgayChungTu}>";

        //                    }
        //                    #endregion

        //                    #region SoChungTu
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.SoChungTu))
        //                    {
        //                        item.StatusMessage = "<Số chứng từ> không được bỏ trống.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage))
        //                    {
        //                        if (await CheckSoHoaDonAsync(item.SoChungTu))
        //                        {
        //                            item.StatusMessage = $"Số chứng từ <{item.SoChungTu}> đã tồn tại.";
        //                        }
        //                    }
        //                    #endregion

        //                    item.DienGiai = (worksheet.Cells[i, 4].Value ?? string.Empty).ToString().Trim();

        //                    #region HanThanhToan
        //                    item.HanThanhToan = (worksheet.Cells[i, 5].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.HanThanhToan) && item.HanThanhToan.IsValidDate() == false)
        //                    {
        //                        item.StatusMessage = "<Hạn thanh toán> không đúng định dạng.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.HanThanhToan))
        //                    {
        //                        item.HanThanhToan = item.HanThanhToan.ParseExact().ToString("dd/MM/yyyy");
        //                    }
        //                    #endregion

        //                    item.LoaiTien = (worksheet.Cells[i, 6].Value ?? string.Empty).ToString().Trim();
        //                    item.TyGia = (worksheet.Cells[i, 7].Value ?? string.Empty).ToString().Trim();
        //                }

        //                #region Hạch toán
        //                item.ChungTuNghiepVuKhacChiTiet.DienGiai = (worksheet.Cells[i, 8].Value ?? string.Empty).ToString().Trim();

        //                #region TaiKhoanNo
        //                item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo = (worksheet.Cells[i, 9].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo))
        //                {
        //                    item.StatusMessage = "<Tài khoản nợ> không được bỏ trống.";
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckSoTaiKhoan(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans) == false)
        //                    {
        //                        item.StatusMessage = $"TK Nợ <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaChiTieuTongHopAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = $"TK Nợ <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo}> là chỉ tiêu tổng hợp. Bạn vui lòng nhập lại chỉ tiêu khác.";
        //                    }
        //                }
        //                #endregion

        //                #region TaiKhoanCo
        //                item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo = (worksheet.Cells[i, 10].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo))
        //                {
        //                    item.StatusMessage = "<Tài khoản có> không được bỏ trống.";
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckSoTaiKhoan(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans) == false)
        //                    {
        //                        item.StatusMessage = $"TK Có <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaChiTieuTongHopAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = $"TK Có <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo}> là chỉ tiêu tổng hợp. Bạn vui lòng nhập lại chỉ tiêu khác.";
        //                    }
        //                }
        //                #endregion

        //                #region SoTien
        //                item.ChungTuNghiepVuKhacChiTiet.SoTien = (worksheet.Cells[i, 11].Value ?? "0").ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) &&
        //                    !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.SoTien) &&
        //                    !item.ChungTuNghiepVuKhacChiTiet.SoTien.IsValidCurrency())
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Số tiền> không hợp lệ.";
        //                }
        //                #endregion

        //                item.ChungTuNghiepVuKhacChiTiet.QuyDoi = (worksheet.Cells[i, 12].Value ?? string.Empty).ToString().Trim();

        //                #region DoiTuongNo
        //                item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo = (worksheet.Cells[i, 13].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = "<Đối tượng Nợ> không được bỏ trống.";
        //                    }
        //                }
        //                DoiTuongViewModel doiTuongNoChiTiet = null;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo))
        //                {
        //                    doiTuongNoChiTiet = await _doiTuongService.CheckMaOutObjectAsync(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo, _doiTuongs);
        //                    if (doiTuongNoChiTiet == null)
        //                    {
        //                        item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (doiTuongNoChiTiet != null)
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongNoId = doiTuongNoChiTiet.DoiTuongId;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongNo = doiTuongNoChiTiet.Ten;
        //                }
        //                else
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongNoId = null;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongNo = null;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.KHACH_HANG))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsKhachHang != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không phải khách hàng.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHA_CUNG_CAP))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhaCungCap != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không phải nhà cung cấp.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHAN_VIEN))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhanVien != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không phải nhân viên.";
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region DoiTuongCo
        //                item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo = (worksheet.Cells[i, 14].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = "<Đối tượng Có> không được bỏ trống.";
        //                    }
        //                }
        //                DoiTuongViewModel doiTuongCoChiTiet = null;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    doiTuongCoChiTiet = await _doiTuongService.CheckMaOutObjectAsync(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo, _doiTuongs);
        //                    if (doiTuongCoChiTiet == null)
        //                    {
        //                        item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (doiTuongCoChiTiet != null)
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongCoId = doiTuongCoChiTiet.DoiTuongId;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongCo = doiTuongCoChiTiet.Ten;
        //                }
        //                else
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongCoId = null;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongCo = null;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.KHACH_HANG))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsKhachHang != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không phải khách hàng.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHA_CUNG_CAP))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhaCungCap != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không phải nhà cung cấp.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHAN_VIEN))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhanVien != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không phải nhân viên.";
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region TaiKhoanNganHang
        //                item.ChungTuNghiepVuKhacChiTiet.SoTaiKhoanNganHang = (worksheet.Cells[i, 15].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.SoTaiKhoanNganHang))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.TAI_KHOAN_NGAN_HANG) ||
        //                    await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.TAI_KHOAN_NGAN_HANG))
        //                    {
        //                        item.StatusMessage = "<TK ngân hàng> không được bỏ trống.";
        //                    }
        //                }
        //                string taiKhoanNganHangChiTietId = string.Empty;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.SoTaiKhoanNganHang))
        //                {
        //                    taiKhoanNganHangChiTietId = await _taiKhoanNganHangService.CheckSoTKNganHangReturnIdAsync(item.ChungTuNghiepVuKhacChiTiet.SoTaiKhoanNganHang, _taiKhoanNganHangs);
        //                    if (string.IsNullOrEmpty(taiKhoanNganHangChiTietId))
        //                    {
        //                        item.StatusMessage = $"TK ngân hàng <{item.ChungTuNghiepVuKhacChiTiet.SoTaiKhoanNganHang}> không có trong danh mục.";
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(taiKhoanNganHangChiTietId))
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNganHangId = taiKhoanNganHangChiTietId;
        //                }
        //                else
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNganHangId = null;
        //                }
        //                #endregion
        //                #endregion

        //                #region Thuế
        //                item.ThueChiTiet.HasData = false;

        //                item.ThueChiTiet.DienGiai = (worksheet.Cells[i, 16].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.DienGiai) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }

        //                #region TKThueGTGT
        //                item.ThueChiTiet.TKThueGTGT = (worksheet.Cells[i, 17].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.TKThueGTGT) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.TKThueGTGT))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckSoTaiKhoan(item.ThueChiTiet.TKThueGTGT, _taiKhoanKeToans) == false)
        //                    {
        //                        item.StatusMessage = $"TK thuế GTGT <{item.ThueChiTiet.TKThueGTGT}> không có trong danh mục.";
        //                    }
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.TKThueGTGT))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaChiTieuTongHopAsync(item.ThueChiTiet.TKThueGTGT, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = $"TK thuế GTGT <{item.ThueChiTiet.TKThueGTGT}> là chỉ tiêu tổng hợp. Bạn vui lòng nhập lại chỉ tiêu khác.";
        //                    }
        //                }
        //                #endregion

        //                #region TienThueGTGT
        //                item.ThueChiTiet.TienThueGTGT = (worksheet.Cells[i, 18].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.TienThueGTGT) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                item.ThueChiTiet.TienThueGTGT = string.IsNullOrEmpty(item.ThueChiTiet.TienThueGTGT) ? "0" : item.ThueChiTiet.TienThueGTGT;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.TienThueGTGT) && !item.ThueChiTiet.TienThueGTGT.IsValidCurrency())
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Tiền thuế GTGT> không hợp lệ.";
        //                }
        //                #endregion

        //                #region PhanTramThueGTGT
        //                item.ThueChiTiet.PhanTramThueGTGT = (worksheet.Cells[i, 19].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.PhanTramThueGTGT) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                item.ThueChiTiet.PhanTramThueGTGT = string.IsNullOrEmpty(item.ThueChiTiet.PhanTramThueGTGT) ? "10" : item.ThueChiTiet.PhanTramThueGTGT;
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (item.ThueChiTiet.PhanTramThueGTGT != "0" && item.ThueChiTiet.PhanTramThueGTGT != "5" && item.ThueChiTiet.PhanTramThueGTGT != "10" && item.ThueChiTiet.PhanTramThueGTGT != "KCT")
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <% thuế GTGT> không hợp lệ.";
        //                    }
        //                }
        //                #endregion

        //                #region GiaTriHHDVChuaThue
        //                item.ThueChiTiet.GiaTriHHDVChuaThue = (worksheet.Cells[i, 20].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.GiaTriHHDVChuaThue) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                item.ThueChiTiet.GiaTriHHDVChuaThue = string.IsNullOrEmpty(item.ThueChiTiet.GiaTriHHDVChuaThue) ? "0" : item.ThueChiTiet.GiaTriHHDVChuaThue;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.GiaTriHHDVChuaThue) && !item.ThueChiTiet.GiaTriHHDVChuaThue.IsValidCurrency())
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Giá trị HHDV chưa thuế> không hợp lệ.";
        //                }
        //                #endregion

        //                #region NgayHoaDon
        //                item.ThueChiTiet.NgayHoaDon = (worksheet.Cells[i, 21].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon))
        //                {
        //                    item.ThueChiTiet.NgayHoaDon = item.NgayChungTu;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon) && item.ThueChiTiet.NgayHoaDon.IsValidDate() == false)
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Ngày hóa đơn> không hợp lệ.";
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon))
        //                {
        //                    item.ThueChiTiet.NgayHoaDon = item.ThueChiTiet.NgayHoaDon.ParseExact().ToString("dd/MM/yyyy");
        //                }
        //                #endregion

        //                item.ThueChiTiet.SoHoaDon = (worksheet.Cells[i, 22].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.SoHoaDon) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }

        //                #region MaDoiTuong
        //                item.ThueChiTiet.MaDoiTuong = (worksheet.Cells[i, 23].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.MaDoiTuong) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                if ((!string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo) || !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo)) && string.IsNullOrEmpty(item.ThueChiTiet.MaDoiTuong))
        //                {
        //                    string maDoiTuong = string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo) ?
        //                        item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo :
        //                        item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo;

        //                    bool existMa = await _doiTuongService.CheckTrungMa(maDoiTuong, _doiTuongs);
        //                    if (existMa)
        //                    {
        //                        item.ThueChiTiet.MaDoiTuong = maDoiTuong;
        //                    }
        //                }
        //                DoiTuongViewModel doiTuongThue = null;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.MaDoiTuong))
        //                {
        //                    doiTuongThue = await _doiTuongService.CheckMaOutObjectAsync(item.ThueChiTiet.MaDoiTuong, _doiTuongs);
        //                    if (doiTuongThue == null)
        //                    {
        //                        item.StatusMessage = $"Mã đối tượng thuế <{item.ThueChiTiet.MaDoiTuong}> không có trong danh mục.";
        //                    }
        //                }
        //                if (doiTuongThue != null)
        //                {
        //                    item.ThueChiTiet.DoiTuongId = doiTuongThue.DoiTuongId;
        //                }
        //                else
        //                {
        //                    item.ThueChiTiet.DoiTuongId = null;
        //                }
        //                #endregion

        //                item.ThueChiTiet.TenDoiTuong = (worksheet.Cells[i, 24].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.TenDoiTuong) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                item.ThueChiTiet.TenDoiTuong = string.IsNullOrEmpty(item.ThueChiTiet.TenDoiTuong) ? (doiTuongThue != null ? doiTuongThue.Ten : string.Empty) : item.ThueChiTiet.TenDoiTuong;

        //                item.ThueChiTiet.MaSoThueDoiTuong = (worksheet.Cells[i, 25].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.ThueChiTiet.MaSoThueDoiTuong) && item.ThueChiTiet.HasData == false)
        //                {
        //                    item.ThueChiTiet.HasData = true;
        //                }
        //                item.ThueChiTiet.MaSoThueDoiTuong = string.IsNullOrEmpty(item.ThueChiTiet.MaSoThueDoiTuong) ? (doiTuongThue != null ? doiTuongThue.MaSoThue : string.Empty) : item.ThueChiTiet.MaSoThueDoiTuong;
        //                #endregion

        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    item.StatusMessage = "<Hợp lệ>";
        //                    item.HasError = false;
        //                }

        //                result.Add(item);
        //            }
        //        }
        //        file.Delete();
        //    }

        //    return result;
        //}

        //public async Task<string> CreateFileImportCTNVKError(List<ChungTuNghiepVuKhacViewModelTemp> list)
        //{
        //    string excelFileName = string.Empty;
        //    string excelPath = string.Empty;

        //    try
        //    {
        //        // Export excel
        //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

        //        if (!Directory.Exists(uploadFolder))
        //        {
        //            Directory.CreateDirectory(uploadFolder);
        //        }
        //        else
        //        {
        //            FileHelper.ClearFolder(uploadFolder);
        //        }

        //        excelFileName = $"chung-tu-nghiep-vu-khac-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
        //        string excelFolder = $"FilesUpload/excels/{excelFileName}";
        //        excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

        //        // Excel
        //        string _sample = $"Template/Chung_Tu_Nghiep_Vu_Khac_Import.xlsx";
        //        string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

        //        FileInfo file = new FileInfo(_path_sample);
        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            // Open sheet1
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            int begin_row = 2;
        //            int i = begin_row;
        //            foreach (var item in list)
        //            {
        //                worksheet.Cells[i, 1].Value = item.NgayHachToan;
        //                worksheet.Cells[i, 2].Value = item.NgayChungTu;
        //                worksheet.Cells[i, 3].Value = item.SoChungTu;
        //                worksheet.Cells[i, 4].Value = item.DienGiai;
        //                worksheet.Cells[i, 5].Value = item.HanThanhToan;
        //                worksheet.Cells[i, 6].Value = item.LoaiTien;
        //                worksheet.Cells[i, 7].Value = item.TyGia;
        //                worksheet.Cells[i, 8].Value = item.ChungTuNghiepVuKhacChiTiet.DienGiai;
        //                worksheet.Cells[i, 9].Value = item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo;
        //                worksheet.Cells[i, 10].Value = item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo;
        //                worksheet.Cells[i, 11].Value = item.ChungTuNghiepVuKhacChiTiet.SoTien;
        //                worksheet.Cells[i, 12].Value = item.ChungTuNghiepVuKhacChiTiet.QuyDoi;
        //                worksheet.Cells[i, 13].Value = item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo;
        //                worksheet.Cells[i, 14].Value = item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo;
        //                worksheet.Cells[i, 15].Value = item.ChungTuNghiepVuKhacChiTiet.SoTaiKhoanNganHang;
        //                worksheet.Cells[i, 16].Value = item.ThueChiTiet.DienGiai;
        //                worksheet.Cells[i, 17].Value = item.ThueChiTiet.TKThueGTGT;
        //                worksheet.Cells[i, 18].Value = item.ThueChiTiet.TienThueGTGT;
        //                worksheet.Cells[i, 19].Value = item.ThueChiTiet.PhanTramThueGTGT;
        //                worksheet.Cells[i, 20].Value = item.ThueChiTiet.GiaTriHHDVChuaThue;
        //                worksheet.Cells[i, 21].Value = item.ThueChiTiet.NgayHoaDon;
        //                worksheet.Cells[i, 22].Value = item.ThueChiTiet.SoHoaDon;
        //                worksheet.Cells[i, 23].Value = item.ThueChiTiet.MaDoiTuong;
        //                worksheet.Cells[i, 24].Value = item.ThueChiTiet.TenDoiTuong;
        //                worksheet.Cells[i, 25].Value = item.ThueChiTiet.MaSoThueDoiTuong;

        //                i += 1;
        //            }
        //            package.SaveAs(new FileInfo(excelPath));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(string.Empty, ex);
        //    }

        //    return GetLinkExcelFile(excelFileName);
        //}

        //public async Task<List<ChungTuNghiepVuKhacViewModelTemp>> ImportCTQTTUAsync(IList<IFormFile> files, UserViewModel ActionUser)
        //{
        //    List<ChungTuNghiepVuKhacViewModelTemp> result = new List<ChungTuNghiepVuKhacViewModelTemp>();

        //    var upload = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
        //    var fileUrl = upload.InsertFileExcel(files);
        //    if (!string.IsNullOrEmpty(fileUrl))
        //    {
        //        FileInfo file = new FileInfo(fileUrl);
        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            // Open sheet1
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            // Get total all row
        //            int totalRows = worksheet.Dimension.Rows;
        //            // Begin row
        //            int begin_row = 2;

        //            List<DoiTuong> _doiTuongs = await _db.DoiTuongs.ToListAsync();
        //            List<TaiKhoanNganHang> _taiKhoanNganHangs = await _db.TaiKhoanNganHangs.ToListAsync();
        //            List<NganHang> _nganHangs = await _db.NganHangs.ToListAsync();
        //            List<TaiKhoanKeToan> _taiKhoanKeToans = await _db.TaiKhoanKeToans.ToListAsync();
        //            List<ChungTuNghiepVuKhac> _chungTuNghiepVuKhacs = await _db.ChungTuNghiepVuKhacs.ToListAsync();
        //            List<KhoanMucChiPhi> _khoanMucChiPhis = await _db.KhoanMucChiPhis.ToListAsync();

        //            for (int i = begin_row; i <= totalRows; i++)
        //            {
        //                ChungTuNghiepVuKhacViewModelTemp item = new ChungTuNghiepVuKhacViewModelTemp();
        //                item.Row = i;
        //                item.IsChungTuNVK = false;
        //                item.IsChungTuQTTU = true;
        //                item.IsChungTuBuTruCongNo = false;
        //                item.HasError = true;

        //                item.SoChungTu = (worksheet.Cells[i, 3].Value ?? string.Empty).ToString().Trim();

        //                ChungTuNghiepVuKhacViewModelTemp copyData = result.FirstOrDefault(x => x.SoChungTu == item.SoChungTu);
        //                if (copyData != null)
        //                {
        //                    item = (ChungTuNghiepVuKhacViewModelTemp)copyData.Clone();
        //                    item.Row = i;
        //                    item.HasError = true;
        //                    item.StatusMessage = string.Empty;

        //                    ChungTuNghiepVuKhacViewModelTemp checkError = result.FirstOrDefault(x => x.SoChungTu == item.SoChungTu && x.HasError == true);
        //                    if (checkError != null)
        //                    {
        //                        item.StatusMessage = $"Dòng chi tiết liên quan (dòng số <{checkError.Row}>) bị lỗi.";
        //                    }
        //                }
        //                else
        //                {
        //                    #region NgayHachToan
        //                    item.NgayHachToan = (worksheet.Cells[i, 1].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.NgayHachToan))
        //                    {
        //                        item.StatusMessage = "<Ngày hạch toán> không được bỏ trống.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.NgayHachToan.IsValidDate() == false)
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <Ngày hạch toán> không hợp lệ.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage))
        //                    {
        //                        item.NgayHachToan = item.NgayHachToan.ParseExact().ToString("dd/MM/yyyy");
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && ActionUser.KyKeToanFromDate.IsValidDate() && ActionUser.KyKeToanToDate.IsValidDate())
        //                    {
        //                        DateTime NgayHachToan = item.NgayHachToan.ParseExact();
        //                        DateTime KyKeToanFromDate = DateTime.Parse(ActionUser.KyKeToanFromDate);
        //                        DateTime KyKeToanToDate = DateTime.Parse(ActionUser.KyKeToanToDate);
        //                        if (NgayHachToan > KyKeToanToDate || NgayHachToan < KyKeToanFromDate)
        //                        {
        //                            item.StatusMessage = $"Ngày hạch toán phải nằm trong kỳ kế toán hiện tại Từ ngày<{KyKeToanFromDate:dd/MM/yyyy}> - Đến ngày<{KyKeToanToDate:dd/MM/yyyy}>";
        //                        }
        //                    }
        //                    #endregion

        //                    #region NgayChungTu
        //                    item.NgayChungTu = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.NgayChungTu))
        //                    {
        //                        item.StatusMessage = "<Ngày chứng từ> không được bỏ trống.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.NgayChungTu.IsValidDate() == false)
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <Ngày chứng từ> không hợp lệ.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage))
        //                    {
        //                        item.NgayChungTu = item.NgayChungTu.ParseExact().ToString("dd/MM/yyyy");
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && ActionUser.KyKeToanFromDate.IsValidDate() && ActionUser.KyKeToanToDate.IsValidDate())
        //                    {
        //                        DateTime NgayChungTu = item.NgayChungTu.ParseExact();
        //                        DateTime KyKeToanFromDate = DateTime.Parse(ActionUser.KyKeToanFromDate);
        //                        DateTime KyKeToanToDate = DateTime.Parse(ActionUser.KyKeToanToDate);
        //                        if (NgayChungTu > KyKeToanToDate || NgayChungTu < KyKeToanFromDate)
        //                        {
        //                            item.StatusMessage = $"Ngày chứng từ phải nằm trong kỳ kế toán hiện tại Từ ngày<{KyKeToanFromDate:dd/MM/yyyy}> - Đến ngày<{KyKeToanToDate:dd/MM/yyyy}>";
        //                        }
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.NgayChungTu.ParseExact() > item.NgayHachToan.ParseExact())
        //                    {
        //                        item.StatusMessage = $"Ngày hạch toán phải lớn hơn hoặc bằng Ngày chứng từ <{item.NgayChungTu}>";

        //                    }
        //                    #endregion

        //                    #region SoChungTu
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.SoChungTu))
        //                    {
        //                        item.StatusMessage = "<Số chứng từ> không được bỏ trống.";
        //                    }
        //                    if (string.IsNullOrEmpty(item.StatusMessage))
        //                    {
        //                        if (await CheckSoHoaDonAsync(item.SoChungTu))
        //                        {
        //                            item.StatusMessage = $"Số chứng từ <{item.SoChungTu}> đã tồn tại.";
        //                        }
        //                    }
        //                    #endregion

        //                    item.QuyetToanChoTungLanTamUng = (worksheet.Cells[i, 4].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && item.QuyetToanChoTungLanTamUng != "0" && item.QuyetToanChoTungLanTamUng != "1")
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <Quyết toán cho từng lần tạm ứng> không hợp lệ.";
        //                    }

        //                    #region MaNhanVien
        //                    item.MaNhanVien = (worksheet.Cells[i, 5].Value ?? string.Empty).ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.MaNhanVien))
        //                    {
        //                        item.StatusMessage = $"<Nhân viên> không được bỏ trống.";
        //                    }
        //                    string nhanVienId = string.Empty;
        //                    if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.MaNhanVien))
        //                    {
        //                        nhanVienId = await _doiTuongService.CheckMaOutId(item.MaNhanVien, 3, _doiTuongs);
        //                        if (string.IsNullOrEmpty(nhanVienId))
        //                        {
        //                            item.StatusMessage = $"Nhân viên <{item.MaNhanVien}> không có trong danh mục.";
        //                        }
        //                    }
        //                    if (!string.IsNullOrEmpty(nhanVienId))
        //                    {
        //                        item.NhanVienId = nhanVienId;
        //                    }
        //                    #endregion

        //                    #region SoTienQuyetToan
        //                    item.SoTienTamUng = (worksheet.Cells[i, 6].Value ?? "0").ToString().Trim();
        //                    if (string.IsNullOrEmpty(item.StatusMessage) &&
        //                        !string.IsNullOrEmpty(item.SoTienTamUng) &&
        //                        !item.SoTienTamUng.IsValidCurrency())
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <Số tiền tạm ứng> không hợp lệ.";
        //                    }
        //                    #endregion

        //                    item.SoTienTamUngQuyDoi = (worksheet.Cells[i, 7].Value ?? string.Empty).ToString().Trim();
        //                    item.DienGiai = (worksheet.Cells[i, 8].Value ?? string.Empty).ToString().Trim();
        //                    item.LoaiTien = (worksheet.Cells[i, 9].Value ?? string.Empty).ToString().Trim();
        //                    item.TyGia = (worksheet.Cells[i, 10].Value ?? string.Empty).ToString().Trim();
        //                }

        //                #region Hạch toán
        //                item.ChungTuNghiepVuKhacChiTiet.DienGiai = (worksheet.Cells[i, 11].Value ?? string.Empty).ToString().Trim();

        //                #region TaiKhoanNo
        //                item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo = (worksheet.Cells[i, 12].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo))
        //                {
        //                    item.StatusMessage = "<Tài khoản nợ> không được bỏ trống.";
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckSoTaiKhoan(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans) == false)
        //                    {
        //                        item.StatusMessage = $"TK Nợ <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaChiTieuTongHopAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = $"TK Nợ <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo}> là chỉ tiêu tổng hợp. Bạn vui lòng nhập lại chỉ tiêu khác.";
        //                    }
        //                }
        //                #endregion

        //                #region TaiKhoanCo
        //                item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo = (worksheet.Cells[i, 13].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo))
        //                {
        //                    item.StatusMessage = "<Tài khoản có> không được bỏ trống.";
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckSoTaiKhoan(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans) == false)
        //                    {
        //                        item.StatusMessage = $"TK Có <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaChiTieuTongHopAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = $"TK Có <{item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo}> là chỉ tiêu tổng hợp. Bạn vui lòng nhập lại chỉ tiêu khác.";
        //                    }
        //                }
        //                #endregion

        //                #region SoTien
        //                item.ChungTuNghiepVuKhacChiTiet.SoTien = (worksheet.Cells[i, 14].Value ?? "0").ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) &&
        //                    !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.SoTien) &&
        //                    !item.ChungTuNghiepVuKhacChiTiet.SoTien.IsValidCurrency())
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Số tiền> không hợp lệ.";
        //                }
        //                #endregion

        //                item.ChungTuNghiepVuKhacChiTiet.QuyDoi = (worksheet.Cells[i, 15].Value ?? string.Empty).ToString().Trim();

        //                item.ChungTuNghiepVuKhacChiTiet.DienGiaiThue = (worksheet.Cells[i, 16].Value ?? string.Empty).ToString().Trim();

        //                item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT = (worksheet.Cells[i, 17].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT))
        //                {
        //                    if (item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT != "0" && item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT != "5" && item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT != "10" && item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT != "KCT")
        //                    {
        //                        item.StatusMessage = "Dữ liệu cột <% thuế GTGT> không hợp lệ.";
        //                    }
        //                }

        //                item.ChungTuNghiepVuKhacChiTiet.TienThueGTGT = (worksheet.Cells[i, 18].Value ?? "0").ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.TienThueGTGT) && !item.ThueChiTiet.TienThueGTGT.IsValidCurrency())
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Tiền thuế GTGT> không hợp lệ.";
        //                }

        //                item.ChungTuNghiepVuKhacChiTiet.TienThueGTGTQuyDoi = (worksheet.Cells[i, 19].Value ?? string.Empty).ToString().Trim();

        //                #region TKThueGTGT
        //                item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT = (worksheet.Cells[i, 20].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckSoTaiKhoan(item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT, _taiKhoanKeToans) == false)
        //                    {
        //                        item.StatusMessage = $"TK thuế GTGT <{item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT}> không có trong danh mục.";
        //                    }
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaChiTieuTongHopAsync(item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = $"TK thuế GTGT <{item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT}> là chỉ tiêu tổng hợp. Bạn vui lòng nhập lại chỉ tiêu khác.";
        //                    }

        //                }
        //                #endregion

        //                #region DoiTuongNo
        //                item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo = (worksheet.Cells[i, 21].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = "<Đối tượng Nợ> không được bỏ trống.";
        //                    }
        //                }
        //                DoiTuongViewModel doiTuongNoChiTiet = null;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo))
        //                {
        //                    doiTuongNoChiTiet = await _doiTuongService.CheckMaOutObjectAsync(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo, _doiTuongs);
        //                    if (doiTuongNoChiTiet == null)
        //                    {
        //                        item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (doiTuongNoChiTiet != null)
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongNoId = doiTuongNoChiTiet.DoiTuongId;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongNo = doiTuongNoChiTiet.Ten;
        //                }
        //                else
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongNoId = null;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongNo = null;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.KHACH_HANG))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsKhachHang != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không phải khách hàng.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHA_CUNG_CAP))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhaCungCap != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không phải nhà cung cấp.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHAN_VIEN))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhanVien != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Nợ <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo}> không phải nhân viên.";
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region DoiTuongCo
        //                item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo = (worksheet.Cells[i, 22].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.MaNhanVien) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo = item.MaNhanVien;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans))
        //                    {
        //                        item.StatusMessage = "<Đối tượng Có> không được bỏ trống.";
        //                    }
        //                }
        //                DoiTuongViewModel doiTuongCoChiTiet = null;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    doiTuongCoChiTiet = await _doiTuongService.CheckMaOutObjectAsync(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo, _doiTuongs);
        //                    if (doiTuongCoChiTiet == null)
        //                    {
        //                        item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không có trong danh mục.";
        //                    }
        //                }
        //                if (doiTuongCoChiTiet != null)
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongCoId = doiTuongCoChiTiet.DoiTuongId;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongCo = doiTuongCoChiTiet.Ten;
        //                }
        //                else
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.DoiTuongCoId = null;
        //                    item.ChungTuNghiepVuKhacChiTiet.TenDoiTuongCo = null;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo))
        //                {
        //                    if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.KHACH_HANG))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsKhachHang != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không phải khách hàng.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHA_CUNG_CAP))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhaCungCap != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không phải nhà cung cấp.";
        //                        }
        //                    }
        //                    else if (await _taiKhoanKeToanService.CheckLaTaiKhoanDoiTuongAsync(item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo, _taiKhoanKeToans, DoiTuongTaiKhoan.NHAN_VIEN))
        //                    {
        //                        if (_doiTuongs.Any(x => x.IsNhanVien != true && x.Ma.ToUpper() == item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo.ToUpper()))
        //                        {
        //                            item.StatusMessage = $"Đối tượng Có <{item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo}> không phải nhân viên.";
        //                        }
        //                    }
        //                }
        //                #endregion

        //                item.ChungTuNghiepVuKhacChiTiet.MaKhoanMucChiPhi = (worksheet.Cells[i, 23].Value ?? string.Empty).ToString().Trim();
        //                string khoanMucChiPhiChiTietId = string.Empty;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ChungTuNghiepVuKhacChiTiet.MaKhoanMucChiPhi))
        //                {
        //                    khoanMucChiPhiChiTietId = await _khoanMucChiPhiService.CheckMaOutIdAsync(item.ChungTuNghiepVuKhacChiTiet.MaKhoanMucChiPhi, _khoanMucChiPhis);
        //                    if (string.IsNullOrEmpty(khoanMucChiPhiChiTietId))
        //                    {
        //                        item.StatusMessage = $"Khoản mục chi phí <{item.ChungTuNghiepVuKhacChiTiet.MaKhoanMucChiPhi}> không có trong danh mục.";
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(khoanMucChiPhiChiTietId))
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.KhoanMucChiPhiId = khoanMucChiPhiChiTietId;
        //                }
        //                else
        //                {
        //                    item.ChungTuNghiepVuKhacChiTiet.KhoanMucChiPhiId = null;
        //                }
        //                #endregion

        //                #region Thuế
        //                item.ThueChiTiet.DienGiai = (worksheet.Cells[i, 11].Value ?? string.Empty).ToString().Trim();

        //                item.ThueChiTiet.MauSoHoaDon = (worksheet.Cells[i, 24].Value ?? string.Empty).ToString().Trim();
        //                item.ThueChiTiet.KyHieuHoaDon = (worksheet.Cells[i, 25].Value ?? string.Empty).ToString().Trim();

        //                #region NgayHoaDon
        //                item.ThueChiTiet.NgayHoaDon = (worksheet.Cells[i, 26].Value ?? string.Empty).ToString().Trim();
        //                if (string.IsNullOrEmpty(item.StatusMessage) && string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon))
        //                {
        //                    item.ThueChiTiet.NgayHoaDon = item.NgayChungTu;
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon) && item.ThueChiTiet.NgayHoaDon.IsValidDate() == false)
        //                {
        //                    item.StatusMessage = "Dữ liệu cột <Ngày hóa đơn> không hợp lệ.";
        //                }
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.NgayHoaDon))
        //                {
        //                    item.ThueChiTiet.NgayHoaDon = item.ThueChiTiet.NgayHoaDon.ParseExact().ToString("dd/MM/yyyy");
        //                }
        //                #endregion

        //                item.ThueChiTiet.SoHoaDon = (worksheet.Cells[i, 27].Value ?? string.Empty).ToString().Trim();

        //                #region MaDoiTuong
        //                item.ThueChiTiet.MaDoiTuong = (worksheet.Cells[i, 28].Value ?? string.Empty).ToString().Trim();
        //                if (!string.IsNullOrEmpty(item.MaNhanVien) && string.IsNullOrEmpty(item.ThueChiTiet.MaDoiTuong))
        //                {
        //                    bool existMa = await _doiTuongService.CheckTrungMa(item.MaNhanVien, _doiTuongs);
        //                    if (existMa)
        //                    {
        //                        item.ThueChiTiet.MaDoiTuong = item.MaNhanVien;
        //                    }
        //                }
        //                DoiTuongViewModel doiTuongThue = null;
        //                if (string.IsNullOrEmpty(item.StatusMessage) && !string.IsNullOrEmpty(item.ThueChiTiet.MaDoiTuong))
        //                {
        //                    doiTuongThue = await _doiTuongService.CheckMaOutObjectAsync(item.ThueChiTiet.MaDoiTuong, _doiTuongs);
        //                    if (doiTuongThue == null)
        //                    {
        //                        item.StatusMessage = $"Mã đối tượng thuế <{item.ThueChiTiet.MaDoiTuong}> không có trong danh mục.";
        //                    }
        //                }
        //                if (doiTuongThue != null)
        //                {
        //                    item.ThueChiTiet.DoiTuongId = doiTuongThue.DoiTuongId;
        //                }
        //                else
        //                {
        //                    item.ThueChiTiet.DoiTuongId = null;
        //                }
        //                #endregion

        //                item.ThueChiTiet.TenDoiTuong = (worksheet.Cells[i, 29].Value ?? string.Empty).ToString().Trim();
        //                item.ThueChiTiet.TenDoiTuong = string.IsNullOrEmpty(item.ThueChiTiet.TenDoiTuong) ? (doiTuongThue != null ? doiTuongThue.Ten : string.Empty) : item.ThueChiTiet.TenDoiTuong;

        //                item.ThueChiTiet.MaSoThueDoiTuong = (worksheet.Cells[i, 30].Value ?? string.Empty).ToString().Trim();
        //                item.ThueChiTiet.MaSoThueDoiTuong = string.IsNullOrEmpty(item.ThueChiTiet.MaSoThueDoiTuong) ? (doiTuongThue != null ? doiTuongThue.MaSoThue : string.Empty) : item.ThueChiTiet.MaSoThueDoiTuong;

        //                item.ThueChiTiet.DiaChi = (worksheet.Cells[i, 31].Value ?? string.Empty).ToString().Trim();
        //                item.ThueChiTiet.DiaChi = string.IsNullOrEmpty(item.ThueChiTiet.DiaChi) ? (doiTuongThue != null ? doiTuongThue.DiaChi : string.Empty) : item.ThueChiTiet.DiaChi;
        //                #endregion

        //                if (string.IsNullOrEmpty(item.StatusMessage))
        //                {
        //                    item.StatusMessage = "<Hợp lệ>";
        //                    item.HasError = false;
        //                }

        //                result.Add(item);
        //            }
        //        }
        //        file.Delete();
        //    }

        //    return result;
        //}

        //public async Task<string> CreateFileImportCTQTTUError(List<ChungTuNghiepVuKhacViewModelTemp> list)
        //{
        //    string excelFileName = string.Empty;
        //    string excelPath = string.Empty;

        //    try
        //    {
        //        // Export excel
        //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

        //        if (!Directory.Exists(uploadFolder))
        //        {
        //            Directory.CreateDirectory(uploadFolder);
        //        }
        //        else
        //        {
        //            FileHelper.ClearFolder(uploadFolder);
        //        }

        //        excelFileName = $"chung-tu-quyet-toan-tam-ung-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
        //        string excelFolder = $"FilesUpload/excels/{excelFileName}";
        //        excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

        //        // Excel
        //        string _sample = $"Template/Chung_Tu_Quyet_Toan_Tam_Ung_Import.xlsx";
        //        string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

        //        FileInfo file = new FileInfo(_path_sample);
        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            // Open sheet1
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            int begin_row = 2;
        //            int i = begin_row;
        //            foreach (var item in list)
        //            {
        //                worksheet.Cells[i, 1].Value = item.NgayHachToan;
        //                worksheet.Cells[i, 2].Value = item.NgayChungTu;
        //                worksheet.Cells[i, 3].Value = item.SoChungTu;
        //                worksheet.Cells[i, 4].Value = item.QuyetToanChoTungLanTamUng;
        //                worksheet.Cells[i, 5].Value = item.MaNhanVien;
        //                worksheet.Cells[i, 6].Value = item.SoTienTamUng;
        //                worksheet.Cells[i, 7].Value = item.SoTienTamUngQuyDoi;
        //                worksheet.Cells[i, 8].Value = item.DienGiai;
        //                worksheet.Cells[i, 9].Value = item.LoaiTien;
        //                worksheet.Cells[i, 10].Value = item.TyGia;
        //                worksheet.Cells[i, 11].Value = item.ChungTuNghiepVuKhacChiTiet.DienGiai;
        //                worksheet.Cells[i, 12].Value = item.ChungTuNghiepVuKhacChiTiet.TaiKhoanNo;
        //                worksheet.Cells[i, 13].Value = item.ChungTuNghiepVuKhacChiTiet.TaiKhoanCo;
        //                worksheet.Cells[i, 14].Value = item.ChungTuNghiepVuKhacChiTiet.SoTien;
        //                worksheet.Cells[i, 15].Value = item.ChungTuNghiepVuKhacChiTiet.QuyDoi;
        //                worksheet.Cells[i, 16].Value = item.ChungTuNghiepVuKhacChiTiet.DienGiaiThue;
        //                worksheet.Cells[i, 17].Value = item.ChungTuNghiepVuKhacChiTiet.PhanTramThueGTGT;
        //                worksheet.Cells[i, 18].Value = item.ChungTuNghiepVuKhacChiTiet.TienThueGTGT;
        //                worksheet.Cells[i, 19].Value = item.ChungTuNghiepVuKhacChiTiet.TienThueGTGTQuyDoi;
        //                worksheet.Cells[i, 20].Value = item.ChungTuNghiepVuKhacChiTiet.TKThueGTGT;
        //                worksheet.Cells[i, 21].Value = item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongNo;
        //                worksheet.Cells[i, 22].Value = item.ChungTuNghiepVuKhacChiTiet.MaDoiTuongCo;
        //                worksheet.Cells[i, 23].Value = item.ChungTuNghiepVuKhacChiTiet.MaKhoanMucChiPhi;
        //                worksheet.Cells[i, 24].Value = item.ThueChiTiet.MauSoHoaDon;
        //                worksheet.Cells[i, 25].Value = item.ThueChiTiet.KyHieuHoaDon;
        //                worksheet.Cells[i, 26].Value = item.ThueChiTiet.NgayHoaDon;
        //                worksheet.Cells[i, 27].Value = item.ThueChiTiet.SoHoaDon;
        //                worksheet.Cells[i, 28].Value = item.ThueChiTiet.MaDoiTuong;
        //                worksheet.Cells[i, 29].Value = item.ThueChiTiet.TenDoiTuong;
        //                worksheet.Cells[i, 30].Value = item.ThueChiTiet.MaSoThueDoiTuong;
        //                worksheet.Cells[i, 31].Value = item.ThueChiTiet.DiaChi;

        //                i += 1;
        //            }
        //            package.SaveAs(new FileInfo(excelPath));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(string.Empty, ex);
        //    }

        //    return GetLinkExcelFile(excelFileName);
        //}

        //public async Task<ChungTuNghiepVuKhacViewModel> HelpPreviewMutipleById(string id)
        //{
        //    ChungTuNghiepVuKhacViewModel query = _db.ChungTuNghiepVuKhacs
        //        .Select(ctnvk => new ChungTuNghiepVuKhacViewModel
        //        {
        //            ChungTuNghiepVuKhacId = ctnvk.ChungTuNghiepVuKhacId,
        //            NgayHachToan = ctnvk.NgayHachToan,
        //            NgayChungTu = ctnvk.NgayChungTu,
        //            SoChungTu = ctnvk.SoChungTu,
        //            NhanVienId = ctnvk.NhanVienId,
        //            SoTienTamUng = ctnvk.SoTienTamUng,
        //            SoTienThuaThieu = ctnvk.SoTienThuaThieu,
        //            DienGiai = ctnvk.DienGiai,
        //            HanThanhToan = ctnvk.HanThanhToan,
        //            QuyetToanChoTungLanTamUng = ctnvk.QuyetToanChoTungLanTamUng,
        //            FileDinhKem = ctnvk.FileDinhKem,
        //            ThamChieu = ctnvk.ThamChieu,
        //            GhiSo = ctnvk.GhiSo,
        //            TongTienHang = ctnvk.TongTienHang,
        //            TienChietKhau = ctnvk.TienChietKhau,
        //            TienThueGTGT = ctnvk.TienThueGTGT,
        //            TongTienThanhToan = ctnvk.TongTienThanhToan,
        //            SoDaTraGiamTruHD = ctnvk.SoDaTraGiamTruHD,
        //            IsChungTuNVK = ctnvk.IsChungTuNVK,
        //            IsChungTuQTTU = ctnvk.IsChungTuQTTU,
        //            // chứng từ bù trừ
        //            DoiTuongId = ctnvk.DoiTuongId,
        //            TenDoiTuong = ctnvk.TenDoiTuong,
        //            LyDoBuTru = ctnvk.LyDoBuTru,
        //            TaiKhoanPhaiThu = ctnvk.TaiKhoanPhaiThu,
        //            TaiKhoanPhaiTra = ctnvk.TaiKhoanPhaiTra,
        //            NgayBuTru = ctnvk.NgayBuTru,
        //            IsChungTuBuTruCongNo = ctnvk.IsChungTuBuTruCongNo,
        //            IsBuTruKhongChiTiet = ctnvk.IsBuTruKhongChiTiet,
        //            //
        //            LoaiChungTu = ctnvk.IsChungTuBuTruCongNo == true ? "Chứng từ công nợ" : ctnvk.IsChungTuNVK == true ? "Chứng từ nghiệp vụ khác" : "Chứng từ quyết toán tạm ứng"
        //        }).FirstOrDefault(x => x.ChungTuNghiepVuKhacId == id);

        //    return query;
        //}

        //public async Task<string> PreviewMultiplePDFQuyetToanTamUng(PreviewMultipleViewModel previewMultipleVM)
        //{
        //    List<string> listPdfFiles = new List<string>();
        //    string mergedFilePath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/pdf/");

        //    foreach (string id in previewMultipleVM.listIds)
        //    {
        //        string xxx = string.Empty;
        //        ChungTuNghiepVuKhacViewModel chungTuNghiepVuKhacViewModel = await HelpPreviewMutipleById(id);
        //        if (chungTuNghiepVuKhacViewModel.IsChungTuQTTU.Value)
        //        {
        //            ChungTuNghiepVuKhacViewModel model = await GetByIdAsync(id);
        //            model.ActionUser = previewMultipleVM.ActionUser;
        //            xxx = await PrintQuyetToanTamUngAsync(model);
        //            string pdfFolder = $"FilesUpload/pdf/{xxx}";
        //            string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, pdfFolder);
        //            listPdfFiles.Add(pdfPath);
        //        }

        //    }

        //    string targetName = string.Empty;
        //    if (listPdfFiles.Count() == 0)
        //    {
        //        return targetName;
        //    }

        //    List<byte[]> listBytes = new List<byte[]>();

        //    for (int i = 0; i < listPdfFiles.Count; i++)
        //    {
        //        byte[] a = System.IO.File.ReadAllBytes(listPdfFiles[i]);
        //        listBytes.Add(a);
        //        if (File.Exists(listPdfFiles[i]))
        //        {
        //            File.Delete(listPdfFiles[i]);
        //        }
        //    }

        //    byte[] result = HeplerMergePDF.MergeFiles(listBytes); ;
        //    targetName = "MergePDF" + DateTime.Now.ToString("ddMMyyyyhhmmsstt") + ".pdf";
        //    System.IO.File.WriteAllBytes(mergedFilePath + targetName, result);
        //    return targetName;
        //}

        //public async Task<string> PreviewMultiplePDFChungTuKeToan(PreviewMultipleViewModel previewMultipleVM)
        //{
        //    List<string> listPdfFiles = new List<string>();
        //    string mergedFilePath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/pdf/");

        //    foreach (string id in previewMultipleVM.listIds)
        //    {
        //        string xxx = string.Empty;
        //        ChungTuNghiepVuKhacViewModel model = await GetByIdAsync(id);
        //        model.ActionUser = previewMultipleVM.ActionUser;
        //        xxx = await PrintChungTuKeToanAsync(model);
        //        string pdfFolder = $"FilesUpload/pdf/{xxx}";
        //        string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, pdfFolder);
        //        listPdfFiles.Add(pdfPath);
        //    }

        //    string targetName = string.Empty;
        //    if (listPdfFiles.Count() == 0)
        //    {
        //        return targetName;
        //    }

        //    List<byte[]> listBytes = new List<byte[]>();

        //    for (int i = 0; i < listPdfFiles.Count; i++)
        //    {
        //        byte[] a = System.IO.File.ReadAllBytes(listPdfFiles[i]);
        //        listBytes.Add(a);
        //        if (File.Exists(listPdfFiles[i]))
        //        {
        //            File.Delete(listPdfFiles[i]);
        //        }
        //    }

        //    byte[] result = HeplerMergePDF.MergeFiles(listBytes); ;
        //    targetName = "MergePDF" + DateTime.Now.ToString("ddMMyyyyhhmmsstt") + ".pdf";
        //    System.IO.File.WriteAllBytes(mergedFilePath + targetName, result);
        //    return targetName;
        //}

        public async Task<bool> DeleteFilePDF(string fileName)
        {
            try
            {
                string mergedFilePath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/pdf/{fileName}");
                if (File.Exists(mergedFilePath))
                {
                    File.Delete(mergedFilePath);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<TrangThai>> GetTrangThaiHoaDon(int? idCha = null)
        {
            List<TrangThai> result = new List<TrangThai>();

            var listParent = TrangThaiHoaDons.Where(x => x.TrangThaiChaId == idCha)
                .OrderBy(x => x.TrangThaiId)
                .ToList();

            if (listParent.Any())
            {
                foreach (var parent in listParent)
                {
                    result.Add(new TrangThai
                    {
                        TrangThaiId = parent.TrangThaiId,
                        Ten = parent.Ten,
                        TrangThaiChaId = parent.TrangThaiChaId,
                        Level = parent.Level,
                        IsParent = TrangThaiHoaDons.Count(x => x.TrangThaiChaId == parent.TrangThaiId) > 0,
                    });

                    result.AddRange(await GetTrangThaiHoaDon(parent.TrangThaiId));
                }
            }
            return result;
        }

        public async Task<List<TrangThai>> GetTrangThaiGuiHoaDon(int? idCha = null)
        {
            List<TrangThai> result = new List<TrangThai>();

            var listParent = TrangThaiGuiHoaDons.Where(x => x.TrangThaiChaId == idCha)
                .OrderBy(x => x.TrangThaiId)
                .ToList();

            if (listParent.Any())
            {
                foreach (var parent in listParent)
                {
                    result.Add(new TrangThai
                    {
                        TrangThaiId = parent.TrangThaiId,
                        Ten = parent.Ten,
                        TrangThaiChaId = parent.TrangThaiChaId,
                        Level = parent.Level,
                        IsParent = TrangThaiGuiHoaDons.Count(x => x.TrangThaiChaId == parent.TrangThaiId) > 0,
                    });

                    result.AddRange(await GetTrangThaiGuiHoaDon(parent.TrangThaiId));
                }
            }
            return result;
        }

        public async Task<List<TrangThai>> GetTreeTrangThai(int LoaiHD, DateTime fromDate, DateTime toDate, int? idCha = null)
        {
            List<TrangThai> result = new List<TrangThai>();

            var listParent = TreeTrangThais.Where(x => x.TrangThaiChaId == idCha)
                .OrderBy(x => x.TrangThaiId)
                .ToList();

            var hoaDons = await _db.HoaDonDienTus.Where(x => x.LoaiHoaDon == LoaiHD && x.NgayHoaDon.Value >= fromDate && x.NgayHoaDon <= toDate)
                                                 .ToListAsync();    
            if (listParent.Any())
            {
                foreach (var parent in listParent)
                {
                    var item = new TrangThai
                    {
                        TrangThaiId = parent.TrangThaiId,
                        Ten = parent.Ten,
                        TrangThaiChaId = parent.TrangThaiChaId,
                        Level = parent.Level,
                        IsParent = TrangThaiGuiHoaDons.Count(x => x.TrangThaiChaId == parent.TrangThaiId) > 0,
                        Children = TrangThaiHoaDons.Where(x => x.TrangThaiChaId == parent.TrangThaiId).ToList(),
                    };

                    if (parent.TrangThaiId == -1)
                    {
                        item.SoLuong = hoaDons.Count;
                    }
                    else if (parent.TrangThaiId >= 1 && parent.TrangThaiId <= 4)
                    {
                        item.SoLuong = hoaDons.Count(x => x.TrangThaiPhatHanh == parent.TrangThaiId);
                    }
                    else
                    {
                        if (parent.TrangThaiId != 8)
                        {
                            item.SoLuong = hoaDons.Count(x => x.TrangThaiPhatHanh == 4 && x.TrangThaiGuiHoaDon == parent.TrangThaiId - 4);
                        }
                        else
                        {
                            item.SoLuong = hoaDons.Count(x => x.TrangThaiPhatHanh == 4 && (x.TrangThaiGuiHoaDon == 5 || x.TrangThaiGuiHoaDon == 6));
                        }
                    }
                    result.Add(item);

                    result.AddRange(await GetTreeTrangThai(LoaiHD, fromDate, toDate, parent.TrangThaiId));
                }
            }
            return result;
        }

        //public async Task<string> ExportExcelBangKe(PagingParams pagingParams)
        //{
        //    try
        //    {
        //        IQueryable<ChungTuNghiepVuKhacViewModel> query = _db.ChungTuNghiepVuKhacs
        //        .OrderByDescending(x => x.NgayChungTu)
        //        .ThenByDescending(x => x.SoChungTu)
        //        .Select(ctnvk => new ChungTuNghiepVuKhacViewModel
        //        {
        //            ChungTuNghiepVuKhacId = ctnvk.ChungTuNghiepVuKhacId,
        //            NgayHachToan = ctnvk.NgayHachToan,
        //            NgayChungTu = ctnvk.NgayChungTu,
        //            SoChungTu = ctnvk.SoChungTu,
        //            NhanVienId = ctnvk.NhanVienId,
        //            SoTienTamUng = ctnvk.SoTienTamUng,
        //            SoTienThuaThieu = ctnvk.SoTienThuaThieu,
        //            DienGiai = ctnvk.DienGiai,
        //            HanThanhToan = ctnvk.HanThanhToan,
        //            QuyetToanChoTungLanTamUng = ctnvk.QuyetToanChoTungLanTamUng,
        //            FileDinhKem = ctnvk.FileDinhKem,
        //            ThamChieu = ctnvk.ThamChieu,
        //            GhiSo = ctnvk.GhiSo,
        //            TongTienHang = ctnvk.TongTienHang,
        //            TienChietKhau = ctnvk.TienChietKhau,
        //            TienThueGTGT = ctnvk.TienThueGTGT,
        //            TongTienThanhToan = ctnvk.TongTienThanhToan,
        //            SoDaTraGiamTruHD = ctnvk.SoDaTraGiamTruHD,
        //            IsChungTuNVK = ctnvk.IsChungTuNVK,
        //            IsChungTuQTTU = ctnvk.IsChungTuQTTU,
        //            IsChungTuXLCLTG = ctnvk.IsChungTuXLCLTG,
        //            // chứng từ bù trừ
        //            DoiTuongId = ctnvk.DoiTuongId,
        //            TenDoiTuong = ctnvk.TenDoiTuong,
        //            LyDoBuTru = ctnvk.LyDoBuTru,
        //            TaiKhoanPhaiThu = ctnvk.TaiKhoanPhaiThu,
        //            TaiKhoanPhaiTra = ctnvk.TaiKhoanPhaiTra,
        //            NgayBuTru = ctnvk.NgayBuTru,
        //            IsChungTuBuTruCongNo = ctnvk.IsChungTuBuTruCongNo,
        //            IsBuTruKhongChiTiet = ctnvk.IsBuTruKhongChiTiet,
        //            IsChungTuXLCLTGTuTinhTGXQ = ctnvk.IsChungTuXLCLTGTuTinhTGXQ,
        //            IsChungTuXLCLTGTuDGLTKNT = ctnvk.IsChungTuXLCLTGTuDGLTKNT,
        //            LoaiChungTu = ctnvk.GetTenLoaiChungTu()
        //        });

        //        if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
        //        {
        //            DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
        //            DateTime toDate = DateTime.Parse(pagingParams.ToDate);
        //            query = query.Where(x => DateTime.Parse(x.NgayChungTu.Value.ToString("yyyy-MM-dd")) >= fromDate &&
        //                                    DateTime.Parse(x.NgayChungTu.Value.ToString("yyyy-MM-dd")) <= toDate);
        //        }

        //        string excelFileName = string.Empty;
        //        string excelPath = string.Empty;

        //        // Export excel
        //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

        //        if (!Directory.Exists(uploadFolder))
        //        {
        //            Directory.CreateDirectory(uploadFolder);
        //        }
        //        else
        //        {
        //            FileHelper.ClearFolder(uploadFolder);
        //        }

        //        excelFileName = $"BANG_KE_CHUNG_TU_NGHIEP_VU_KHAC-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
        //        string excelFolder = $"FilesUpload/excels/{excelFileName}";
        //        excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

        //        // Excel
        //        string _sample = $"docs/samples/BangKeTongHop/BANG_KE_CHUNG_TU_NGHIEP_VU_KHAC.xlsx";
        //        string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
        //        FileInfo file = new FileInfo(_path_sample);
        //        string dateReport = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(pagingParams.FromDate).ToString("dd/MM/yyyy"), DateTime.Parse(pagingParams.ToDate).ToString("dd/MM/yyyy"));
        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            List<ChungTuNghiepVuKhacViewModel> list = query.OrderBy(x => x.NgayChungTu).ToList();
        //            // Open sheet1
        //            int totalRows = list.Count;

        //            // Begin row
        //            int begin_row = 8;

        //            // Open sheet1
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

        //            // Add Row
        //            worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);

        //            // Fill data
        //            int idx = begin_row;
        //            foreach (var it in list)
        //            {
        //                worksheet.Cells[idx, 1].Value = it.NgayHachToan.Value.ToString("dd/MM/yyyy");
        //                worksheet.Cells[idx, 2].Value = it.NgayChungTu.Value.ToString("dd/MM/yyyy");
        //                worksheet.Cells[idx, 3].Value = it.SoChungTu;
        //                worksheet.Cells[idx, 4].Value = it.DienGiai;
        //                worksheet.Cells[idx, 5].Value = it.TongTienThanhToan;
        //                worksheet.Cells[idx, 6].Value = it.LoaiChungTu;
        //                idx += 1;
        //            }
        //            worksheet.Cells[5, 1].Value = dateReport;
        //            //worksheet.Row(5).Style.Font.Color.SetColor(Color.Red);
        //            // Total
        //            worksheet.Row(idx).Style.Font.Bold = true;
        //            worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", list.Count);
        //            worksheet.Cells[idx, 5].Formula = string.Format("=SUM({0}:{1})", worksheet.Cells[begin_row, 5].Address, worksheet.Cells[idx - 1, 5].Address);

        //            //replace Text
        //            CoCauToChuc coCauToChuc = _db.CoCauToChucs
        //                        .AsNoTracking()
        //                        .Where(x => int.Parse(x.CapToChuc) == 0)
        //                        .FirstOrDefault();

        //            var _thongTinIn = _db.CoCauToChuc_ThongTinInBaoCaos
        //                        .AsNoTracking()
        //                        .FirstOrDefault();


        //            List<CoCauToChuc_NguoiKy> coCauToChuc_NguoiKys = _db.CoCauToChuc_NguoiKys.ToList();
        //            var giamdoc = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "GIÁM ĐỐC");

        //            var ketoantruong = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "KẾ TOÁN TRƯỞNG");

        //            var nguoilapphieu = coCauToChuc_NguoiKys.FirstOrDefault(x => x.ChucDanh.Trim().ToUpper() == "NGƯỜI LẬP PHIẾU");

        //            var query1 = from cell in worksheet.Cells["A:XFD"]
        //                         where cell.Value?.ToString().Contains("<NguoiLapPhieu>") == true ||
        //                         cell.Value?.ToString().Contains("<KeToanTruong>") == true ||
        //                         cell.Value?.ToString().Contains("<TenNguoiLapPhieu>") == true ||
        //                         cell.Value?.ToString().Contains("<TenKeToanTruong>") == true ||
        //                         cell.Value?.ToString().Contains("<TenGiamDoc>") == true ||
        //                         cell.Value?.ToString().Contains("<DonVi>") == true ||
        //                         cell.Value?.ToString().Contains("<DiaChi>") == true ||
        //                         cell.Value?.ToString().Contains("<GiamDoc>") == true
        //                         select cell;

        //            foreach (var cell in query1)
        //            {
        //                cell.Value = cell.Value.ToString().Replace("<NguoiLapPhieu>", nguoilapphieu.TieuDeNguoiKy ?? string.Empty);
        //                cell.Value = cell.Value.ToString().Replace("<KeToanTruong>", ketoantruong.TieuDeNguoiKy ?? string.Empty);
        //                cell.Value = cell.Value.ToString().Replace("<GiamDoc>", giamdoc.TieuDeNguoiKy ?? string.Empty);
        //                cell.Value = cell.Value.ToString().Replace("<GiamDoc>", giamdoc.TieuDeNguoiKy ?? string.Empty);
        //                cell.Value = cell.Value.ToString().Replace("<DonVi>", _thongTinIn.TenDonVi ?? string.Empty);
        //                cell.Value = cell.Value.ToString().Replace("<DiaChi>", _thongTinIn.DiaChi ?? string.Empty);
        //                if (coCauToChuc.InTenNguoiKy == true)
        //                {
        //                    cell.Value = cell.Value.ToString().Replace("<TenGiamDoc>", giamdoc.TenNguoiKy ?? string.Empty);
        //                    cell.Value = cell.Value.ToString().Replace("<TenKeToanTruong>", ketoantruong.TenNguoiKy ?? string.Empty);
        //                    cell.Value = cell.Value.ToString().Replace("<TenNguoiLapPhieu>", nguoilapphieu.TenNguoiKy ?? string.Empty);
        //                }
        //                else
        //                {
        //                    cell.Value = cell.Value.ToString().Replace("<TenGiamDoc>", string.Empty);
        //                    cell.Value = cell.Value.ToString().Replace("<TenKeToanTruong>", string.Empty);
        //                    cell.Value = cell.Value.ToString().Replace("<TenNguoiLapPhieu>", string.Empty);
        //                }
        //            }

        //            package.SaveAs(new FileInfo(excelPath));
        //        }

        //        return excelFileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model)
        //{
        //    TienLuiViewModel result = new TienLuiViewModel();
        //    if (string.IsNullOrEmpty(model.ChungTuId))
        //    {
        //        return result;
        //    }

        //    var list = await _db.ChungTuNghiepVuKhacs
        //        .OrderBy(x => x.NgayHachToan)
        //        .ThenBy(x => x.NgayChungTu)
        //        .ThenBy(x => x.SoChungTu)
        //        .Select(x => new ChungTuNghiepVuKhacViewModel
        //        {
        //            ChungTuNghiepVuKhacId = x.ChungTuNghiepVuKhacId,
        //        })
        //        .ToListAsync();

        //    var length = list.Count();
        //    var currentIndex = list.FindIndex(x => x.ChungTuNghiepVuKhacId == model.ChungTuId);
        //    if (currentIndex != -1)
        //    {
        //        if (currentIndex > 0)
        //        {
        //            result.TruocId = list[currentIndex - 1].ChungTuNghiepVuKhacId;
        //            result.VeDauId = list[0].ChungTuNghiepVuKhacId;
        //        }
        //        if (currentIndex < (length - 1))
        //        {
        //            result.SauId = list[currentIndex + 1].ChungTuNghiepVuKhacId;
        //            result.VeCuoiId = list[length - 1].ChungTuNghiepVuKhacId;
        //        }
        //    }

        //    return result;
        //}

    }
}
