using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using MailKit.Net.Smtp;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class HoaDonDienTuService : IHoaDonDienTuService
    {
        Datacontext _db;
        IMapper _mp;
        ILoaiTienService _LoaiTienService;
        IHoSoHDDTService _HoSoHDDTService;
        IHoaDonDienTuChiTietService _HoaDonDienTuChiTietService;
        IMauHoaDonService _MauHoaDonService;
        IHttpContextAccessor _IHttpContextAccessor;
        IHostingEnvironment _hostingEnvironment;
        ITuyChonService _TuyChonService;
        IUserRespositories _UserRespositories;

        public HoaDonDienTuService(
            Datacontext datacontext,
            IMapper mapper,
            ILoaiTienService LoaiTienService,
            IMauHoaDonService MauHoaDonService,
            IHoSoHDDTService HoSoHDDTService,
            IHoaDonDienTuChiTietService HoaDonDienTuChiTietService,
            ITuyChonService TuyChonService,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment IHostingEnvironment,
            IUserRespositories UserRespositories
        )
        {
            _db = datacontext;
            _mp = mapper;
            _LoaiTienService = LoaiTienService;
            _HoSoHDDTService = HoSoHDDTService;
            _MauHoaDonService = MauHoaDonService;
            _HoaDonDienTuChiTietService = HoaDonDienTuChiTietService;
            _TuyChonService = TuyChonService;
            _UserRespositories = UserRespositories;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = IHostingEnvironment;
        }

        private List<TrangThai> TrangThaiHoaDons = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = 1, Ten = "Hóa đơn gốc", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Hóa đơn xóa bỏ", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Hóa đơn thay thế", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Hóa đơn điều chỉnh", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Hóa đơn điều chỉnh tăng", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Hóa đơn điều chỉnh giảm", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = 7, Ten = "Hóa đơn điều chỉnh thông tin", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
        };

        private List<TrangThai> TrangThaiGuiHoaDons = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = 1, Ten = "Chưa gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Đang gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Gửi hóa đơn cho khách hàng lỗi", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Đã gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Khách hàng đã xem hóa đơn", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Khách hàng chưa xem hóa đơn", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
        };

        private List<TrangThai> TreeTrangThais = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 0, Ten = "Chưa phát hành", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 1, Ten = "Đang phát hành", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Phát hành lỗi", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Đã phát hành", TrangThaiChaId = -1, Level = 1 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Chưa gửi hóa đơn cho khách hàng", TrangThaiChaId = 3, Level = 2 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Đang gửi hóa đơn cho khách hàng", TrangThaiChaId = 3, Level = 2 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Gửi hóa đơn cho khách hàng lỗi", TrangThaiChaId = 3, Level = 2 },
            new TrangThai(){ TrangThaiId = 7, Ten = "Đã gửi hóa đơn cho khách hàng", TrangThaiChaId = 3, Level = 2 },
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
                    MauHoaDon = _mp.Map<MauHoaDonViewModel>(_db.MauHoaDons.FirstOrDefault(x => x.MauHoaDonId == hd.MauHoaDonId)),
                    MauSo = hd.MauSo,
                    KyHieu = hd.KyHieu,
                    KhachHangId = hd.KhachHangId ?? string.Empty,
                    KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.KhachHangId)),
                    MaKhachHang = hd.MaKhachHang,
                    TenKhachHang = hd.TenKhachHang,
                    SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                    EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                    HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                    DiaChi = hd.DiaChi,
                    MaSoThue = hd.MaSoThue,
                    TenNganHang = hd.TenNganHang,
                    SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                    NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                    NhanVienBanHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.NhanVienBanHangId)),
                    TenNhanVienBanHang = hd.TenNhanVienBanHang,
                    LoaiTienId = hd.LoaiTienId ?? string.Empty,
                    LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
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
                    TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien ?? 0 - x.TienChietKhau ?? 0 + x.TienThueGTGT ?? 0)
                });

            var list = query.ToList();
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

            if (pagingParams.TrangThaiPhatHanh.HasValue && pagingParams.TrangThaiPhatHanh != -1)
            {
                query = query.Where(x => x.TrangThaiPhatHanh == pagingParams.TrangThaiHoaDonDienTu);
            }

            if (pagingParams.TrangThaiGuiHoaDon.HasValue && pagingParams.TrangThaiGuiHoaDon != -1)
            {
                query = query.Where(x => x.TrangThai == pagingParams.TrangThaiGuiHoaDon);
            }

            if (pagingParams.TrangThaiChuyenDoi.HasValue && pagingParams.TrangThaiChuyenDoi != -1)
            {
                query = query.Where(x => pagingParams.TrangThaiChuyenDoi == 0 ? x.SoLanChuyenDoi == 0 : x.SoLanChuyenDoi != 0);
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
                if (!string.IsNullOrEmpty(pagingParams.Filter.KyHieu))
                {
                    query = query.Where(x => x.KyHieu.ToUpper().ToUnSign().Contains(pagingParams.Filter.KyHieu.ToUnSign()) || x.KyHieu.ToUpper().Contains(pagingParams.Filter.KyHieu));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.MauSo))
                {
                    query = query.Where(x => x.MauSo.ToUpper().ToUnSign().Contains(pagingParams.Filter.MauSo.ToUnSign()) || x.MauSo.ToUpper().Contains(pagingParams.Filter.MauSo));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.MaKhachHang))
                {
                    query = query.Where(x => x.MaKhachHang.ToUpper().ToUnSign().Contains(pagingParams.Filter.MaKhachHang.ToUnSign()) || x.MaKhachHang.ToUpper().Contains(pagingParams.Filter.MaKhachHang));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.TenKhachHang))
                {
                    query = query.Where(x => x.TenKhachHang.ToUpper().ToUnSign().Contains(pagingParams.Filter.TenKhachHang.ToUnSign()) || x.KhachHang.Ten.ToUpper().Contains(pagingParams.Filter.TenKhachHang));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.DiaChi))
                {
                    query = query.Where(x => x.DiaChi.ToUpper().ToUnSign().Contains(pagingParams.Filter.DiaChi.ToUnSign()) || x.DiaChi.ToUpper().Contains(pagingParams.Filter.DiaChi));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.MaSoThue))
                {
                    query = query.Where(x => x.MaSoThue.ToUpper().ToUnSign().Contains(pagingParams.Filter.MaSoThue.ToUnSign()) || x.KhachHang.MaSoThue.ToUpper().Contains(pagingParams.Filter.KhachHang.MaSoThue));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.HoTenNguoiMuaHang))
                {
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToUnSign().Contains(pagingParams.Filter.HoTenNguoiMuaHang.ToUnSign()) || x.HoTenNguoiMuaHang.ToUpper().Contains(pagingParams.Filter.HoTenNguoiMuaHang));
                }
                if (!string.IsNullOrEmpty(pagingParams.Filter.TenNhanVienBanHang))
                {
                    query = query.Where(x => x.TenNhanVienBanHang.ToUpper().ToUnSign().Contains(pagingParams.Filter.TenNhanVienBanHang.ToUnSign()) || x.TenNhanVienBanHang.ToUpper().Contains(pagingParams.Filter.TenNhanVienBanHang));
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
            var query = from hd in _db.HoaDonDienTus
                        join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                        from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
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
                            MauSo = hd.MauSo ?? mhd.MauSo,
                            KyHieu = hd.KyHieu ?? mhd.KyHieu,
                            KhachHangId = kh.DoiTuongId ?? string.Empty,
                            KhachHang = kh != null ? _mp.Map<DoiTuongViewModel>(kh) : null,
                            MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                            HinhThucThanhToanId = hd.HinhThucThanhToanId,
                            HinhThucThanhToan = httt != null ? _mp.Map<HinhThucThanhToanViewModel>(httt) : null,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang ?? string.Empty,
                            SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang ?? string.Empty,
                            EmailNguoiMuaHang = hd.EmailNguoiMuaHang ?? string.Empty,
                            TenNganHang = hd.TenNganHang ?? string.Empty,
                            SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang ?? string.Empty,
                            HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD ?? string.Empty,
                            EmailNguoiNhanHD = hd.EmailNguoiNhanHD ?? string.Empty,
                            SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD ?? string.Empty,
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
                            NhanVienBanHangId = hd.NhanVienBanHangId,
                            NhanVienBanHang = nv != null ? _mp.Map<DoiTuongViewModel>(nv) : null,
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
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                                   DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                                   DonViTinh = dvt != null ? _mp.Map<DonViTinhViewModel>(dvt) : null,
                                                   SoLuong = hdct.SoLuong,
                                                   DonGia = hdct.DonGia,
                                                   DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                                   ThanhTien = hdct.ThanhTien,
                                                   ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                                   TyLeChietKhau = hdct.TyLeChietKhau,
                                                   TienChietKhau = hdct.TienChietKhau,
                                                   TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi,
                                                   ThueGTGT = hdct.ThueGTGT,
                                                   TienThueGTGT = hdct.TienThueGTGT,
                                                   TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi,
                                                   TongTienThanhToan = hdct.TongTienThanhToan,
                                                   TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi,
                                                   SoLo = hdct.SoLo,
                                                   HanSuDung = hdct.HanSuDung,
                                                   SoKhung = hdct.SoKhung,
                                                   SoMay = hdct.SoMay
                                               }).ToList(),
                            TaiLieuDinhKem = hd.TaiLieuDinhKem,
                            TongTienHang = hd.TongTienHang,
                            TongTienHangQuyDoi = hd.TongTienHangQuyDoi,
                            TongTienChietKhau = hd.TongTienChietKhau,
                            TongTienChietKhauQuyDoi = hd.TongTienChietKhauQuyDoi,
                            TongTienThueGTGT = hd.TongTienThueGTGT,
                            TongTienThueGTGTQuyDoi = hd.TongTienThueGTGTQuyDoi,
                            TongTienThanhToan = hd.TongTienThanhToan,
                            TongTienThanhToanQuyDoi = hd.TongTienThanhToanQuyDoi,
                            CreatedBy = hd.CreatedBy,
                            CreatedDate = hd.CreatedDate,
                            Status = hd.Status,
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
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
                entity.NgayLap = entity.CreatedDate.Value;

                entity.TrangThai = (int)TrangThaiHoaDon.HoaDonGoc;
                entity.TrangThaiPhatHanh = (int)TrangThaiPhatHanh.ChuaPhatHanh;
                entity.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.ChuaGui;

                var _khachHang = _mp.Map<DoiTuongViewModel>(await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == entity.KhachHangId));
                entity.MaKhachHang = _khachHang.Ma;
                entity.HoTenNguoiNhanHD = _khachHang.HoTenNguoiNhanHD;
                entity.EmailNguoiNhanHD = _khachHang.EmailNguoiNhanHD;
                entity.SoDienThoaiNguoiNhanHD = _khachHang.SoDienThoaiNguoiNhanHD;

                var _nhanVienBanHang = _mp.Map<DoiTuongViewModel>(await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == entity.NhanVienBanHangId));
                if (_nhanVienBanHang != null)
                {
                    entity.MaNhanVienBanHang = _nhanVienBanHang.Ma;
                    entity.TenNhanVienBanHang = _nhanVienBanHang.Ten;
                }
                else
                {
                    entity.MaNhanVienBanHang = string.Empty;
                    entity.TenNhanVienBanHang = string.Empty;
                }

                entity.SoLanChuyenDoi = 0;

                await _db.HoaDonDienTus.AddAsync(entity);
                await _db.SaveChangesAsync();

                var _nktc = new NhatKyThaoTacHoaDonViewModel
                {
                    HoaDonDienTuId = entity.HoaDonDienTuId,
                    LoaiThaoTac = (int)LoaiThaoTac.LapHoaDon,
                    KhachHangId = entity.KhachHangId,
                    MoTa = "Thêm hóa đơn " + (entity.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "GTGT" : "Bán hàng") + " mới vào " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " cho khách hàng " + entity.TenKhachHang,
                    NguoiThucHienId = model.ActionUser.UserId,
                };

                await ThemNhatKyThaoTacHoaDonAsync(_nktc);
                var result = _mp.Map<HoaDonDienTuViewModel>(entity);
                return result;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(HoaDonDienTuViewModel model)
        {
            try
            {
                model.HoaDonChiTiets = null;

                model.ModifyDate = DateTime.Now;
                model.ModifyBy = model.ActionUser.UserId;
                model.NgayLap = model.CreatedDate;

                var _khachHang = _mp.Map<DoiTuongViewModel>(await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == model.KhachHangId));
                model.MaKhachHang = _khachHang.Ma;
                model.HoTenNguoiNhanHD = _khachHang.HoTenNguoiNhanHD;
                model.EmailNguoiNhanHD = _khachHang.EmailNguoiNhanHD;
                model.SoDienThoaiNguoiNhanHD = _khachHang.SoDienThoaiNguoiNhanHD;

                var _nhanVienBanHang = _mp.Map<DoiTuongViewModel>(await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == model.NhanVienBanHangId));
                if (_nhanVienBanHang != null)
                {
                    model.MaNhanVienBanHang = _nhanVienBanHang.Ma;
                    model.TenNhanVienBanHang = _nhanVienBanHang.Ten;
                }
                else
                {
                    model.MaNhanVienBanHang = string.Empty;
                    model.TenNhanVienBanHang = string.Empty;
                }


                HoaDonDienTu entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == model.HoaDonDienTuId);
                //_db.ChungTuNghiepVuKhacs.Update(entity);
                _db.Entry(entity).CurrentValues.SetValues(model);
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<bool> UpdateRangeAsync(List<HoaDonDienTuViewModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    model.HoaDonChiTiets = null;

                    model.ModifyDate = DateTime.Now;
                    model.ModifyBy = model.ActionUser.UserId;
                }

                var ids = models.Select(x => x.HoaDonDienTuId).ToList();
                var entities = await _db.HoaDonDienTus.Where(x => ids.Contains(x.HoaDonDienTuId)).ToListAsync();
                //_db.ChungTuNghiepVuKhacs.Update(entity);
                _db.Entry(entities).CurrentValues.SetValues(models);
                return await _db.SaveChangesAsync() == models.Count;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                return false;
            }
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

            var listParent = TreeTrangThais
                .OrderBy(x => x.TrangThaiId)
                .ToList();

            var hoaDons = await _db.HoaDonDienTus.Where(x => (x.LoaiHoaDon == LoaiHD || LoaiHD == -1) && x.NgayHoaDon.Value >= fromDate && x.NgayHoaDon <= toDate)
                                                 .ToListAsync();
            foreach (var item in listParent)
            {
                item.Children = TreeTrangThais.Where(x => x.TrangThaiChaId == item.TrangThaiId).OrderBy(x => x.TrangThaiId).ToList();
                if (item.TrangThaiId == -1)
                {
                    item.SoLuong = hoaDons.Count();
                }
                else if (item.TrangThaiId != 4)
                {
                    if (item.TrangThaiId < 4)
                    {
                        if (item.TrangThaiId == 0)
                        {
                            item.SoLuong = hoaDons.Count(x => x.TrangThaiPhatHanh == item.TrangThaiId || !x.TrangThaiPhatHanh.HasValue);
                        }
                        else
                            item.SoLuong = hoaDons.Count(x => x.TrangThaiPhatHanh == item.TrangThaiId);
                    }
                    else item.SoLuong = hoaDons.Count(x => x.TrangThaiGuiHoaDon == item.TrangThaiId - 4);
                }
                else
                {
                    item.SoLuong = hoaDons.Count(x => x.TrangThaiGuiHoaDon >= 0 && x.TrangThaiGuiHoaDon <= 4);
                }
            }

            // Get tree accounts
            var byIdLookup = listParent.ToLookup(i => i.TrangThaiChaId);
            foreach (var item in listParent)
            {
                item.Key = item.TrangThaiId;
                item.Children = byIdLookup[item.TrangThaiId].ToList();
            }

            listParent = listParent.Where(x => x.TrangThaiChaId == null).ToList();
            return listParent;
        }

        public async Task<string> ExportExcelBangKe(HoaDonParams pagingParams)
        {
            try
            {
                IQueryable<HoaDonDienTuViewModel> query = _db.HoaDonDienTus
                .OrderByDescending(x => x.NgayHoaDon)
                .ThenByDescending(x => x.SoHoaDon)
                .Select(hd => new HoaDonDienTuViewModel
                {
                    HoaDonDienTuId = hd.HoaDonDienTuId,
                    NgayHoaDon = hd.NgayHoaDon,
                    NgayLap = hd.NgayLap,
                    SoHoaDon = hd.SoHoaDon,
                    MauHoaDonId = hd.MauHoaDonId ?? string.Empty,
                    MauHoaDon = _mp.Map<MauHoaDonViewModel>(_db.MauHoaDons.FirstOrDefault(x => x.MauHoaDonId == hd.MauHoaDonId)),
                    MauSo = hd.MauSo,
                    KyHieu = hd.KyHieu,
                    KhachHangId = hd.KhachHangId ?? string.Empty,
                    KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.KhachHangId)),
                    TenKhachHang = hd.TenKhachHang,
                    EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                    TenNganHang = hd.TenNganHang,
                    HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                    SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                    SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                    NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                    NhanVienBanHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.NhanVienBanHangId)),
                    TenNhanVienBanHang = hd.TenNhanVienBanHang,
                    LoaiTienId = hd.LoaiTienId ?? string.Empty,
                    LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
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
                    TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien ?? 0 - x.TienChietKhau ?? 0 + x.TienThueGTGT ?? 0)
                });

                if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
                {
                    DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
                    DateTime toDate = DateTime.Parse(pagingParams.ToDate);
                    query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                            DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
                }

                string excelFileName = string.Empty;
                string excelPath = string.Empty;

                // Export excel
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                else
                {
                    FileHelper.ClearFolder(uploadFolder);
                }

                excelFileName = $"BANG_KE_HOA_DON_DIEN_TU-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"docs/HoaDonDienTu/BANG_KE_HOA_DON_DIEN_TU.xlsx";
                string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
                FileInfo file = new FileInfo(_path_sample);
                string dateReport = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(pagingParams.FromDate).ToString("dd/MM/yyyy"), DateTime.Parse(pagingParams.ToDate).ToString("dd/MM/yyyy"));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    List<HoaDonDienTuViewModel> list = query.OrderBy(x => x.NgayHoaDon).ToList();
                    // Open sheet1
                    int totalRows = list.Count;

                    // Begin row
                    int begin_row = 5;

                    // Open sheet1
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Add Row
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);

                    // Fill data
                    int idx = begin_row;
                    int count = 1;
                    foreach (var it in list)
                    {
                        worksheet.Cells[idx, 1].Value = count.ToString();
                        worksheet.Cells[idx, 2].Value = it.NgayHoaDon.Value.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 3].Value = !string.IsNullOrEmpty(it.SoHoaDon) ? it.SoHoaDon : "<Chưa cấp số>";
                        worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.MauHoaDon != null ? it.MauHoaDon.MauSo : string.Empty);
                        worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.MauHoaDon != null ? it.MauHoaDon.KyHieu : string.Empty);
                        worksheet.Cells[idx, 6].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.MauHoaDon != null ? it.KhachHang.Ma : string.Empty);
                        worksheet.Cells[idx, 7].Value = !string.IsNullOrEmpty(it.TenKhachHang) ? it.TenKhachHang : (it.KhachHang != null ? it.KhachHang.Ten : string.Empty);
                        worksheet.Cells[idx, 8].Value = !string.IsNullOrEmpty(it.DiaChi) ? it.DiaChi : (it.KhachHang != null ? it.KhachHang.DiaChi : string.Empty);
                        worksheet.Cells[idx, 9].Value = !string.IsNullOrEmpty(it.MaSoThue) ? it.MaSoThue : (it.KhachHang != null ? it.KhachHang.MaSoThue : string.Empty);
                        worksheet.Cells[idx, 10].Value = !string.IsNullOrEmpty(it.HoTenNguoiMuaHang) ? it.HoTenNguoiMuaHang : (it.KhachHang != null ? it.KhachHang.HoTenNguoiMuaHang : string.Empty);
                        worksheet.Cells[idx, 11].Value = !string.IsNullOrEmpty(it.TenNhanVienBanHang) ? it.TenNhanVienBanHang : it.NhanVienBanHang != null ? it.NhanVienBanHang.Ten : string.Empty;
                        worksheet.Cells[idx, 12].Value = it.TongTienThanhToan;
                        worksheet.Cells[idx, 13].Value = it.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG ? "Hóa đơn GTGT" : "Hóa đơn bán hàng";
                        worksheet.Cells[idx, 14].Value = TrangThaiHoaDons.Where(x => x.TrangThaiId == it.TrangThai).Select(x => x.Ten).FirstOrDefault();
                        worksheet.Cells[idx, 15].Value = (it.TrangThaiPhatHanh == 0 ? "Chưa phát hành" : (it.TrangThaiPhatHanh == 1 ? "Đang phát hành" : (it.TrangThaiPhatHanh == 2 ? "Phát hành lỗi" : "Đã phát hành")));
                        worksheet.Cells[idx, 16].Value = it.MaTraCuu;
                        worksheet.Cells[idx, 17].Value = TrangThaiGuiHoaDons.Where(x => x.TrangThaiId == it.TrangThaiGuiHoaDon).Select(x => x.Ten).FirstOrDefault();
                        worksheet.Cells[idx, 18].Value = it.KhachHang != null ? it.KhachHang.HoTenNguoiNhanHD : string.Empty;
                        worksheet.Cells[idx, 19].Value = it.KhachHang != null ? it.KhachHang.EmailNguoiNhanHD : string.Empty;
                        worksheet.Cells[idx, 20].Value = it.KhachHang != null ? it.KhachHang.SoDienThoaiNguoiNhanHD : string.Empty;
                        worksheet.Cells[idx, 21].Value = it.KhachHangDaNhan.HasValue ? (it.KhachHangDaNhan.Value ? "Đã nhận" : "Chưa nhận") : "Chưa nhận";
                        worksheet.Cells[idx, 22].Value = it.SoLanChuyenDoi;
                        worksheet.Cells[idx, 23].Value = it.LyDoXoaBo;
                        worksheet.Cells[idx, 24].Value = it.LoaiChungTu == 0 ? "Mua hàng" : it.LoaiChungTu == 1 ? "Bán hàng" : it.LoaiChungTu == 2 ? "Kho" : "Hóa đơn bách khoa";
                        worksheet.Cells[idx, 25].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 26].Value = it.NguoiLap != null ? it.NguoiLap.Ten : string.Empty;

                        idx += 1;
                        count += 1;
                    }
                    worksheet.Cells[2, 1].Value = dateReport;
                    //worksheet.Row(5).Style.Font.Color.SetColor(Color.Red);
                    // Total
                    worksheet.Row(idx).Style.Font.Bold = true;
                    worksheet.Cells[idx, 2].Value = string.Format("Số dòng = {0}", list.Count);

                    //replace Text


                    package.SaveAs(new FileInfo(excelPath));
                }

                return excelFileName;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params)
        {
            try
            {
                var arrMauHoaDon = @params.MauSo != "-1" ? @params.MauSo.Split(";").ToList() : new List<string>();
                var arrKyHieuHoaDon = @params.KyHieu != "-1" ? @params.KyHieu.Split(";").ToList() : new List<string>();
                var arrKhongDuocChon = @params.KhongDuocChon.Split(";");
                var arrTrangThaiKhongChon = new List<int>();
                foreach (var _it in arrKhongDuocChon)
                {
                    arrTrangThaiKhongChon.Add(_it.ParseInt());
                }

                IQueryable<HoaDonDienTuViewModel> query = _db.HoaDonDienTus
                .Where(x => (x.KhachHangId == @params.KhachHangId || @params.KhachHangId == "" || @params.KhachHangId == null)
                      && (x.LoaiHoaDon == @params.LoaiHoaDon || @params.LoaiHoaDon == -1)
                      && (arrMauHoaDon.Contains(x.MauSo) || @params.KyHieu == "-1")
                      && (arrKyHieuHoaDon.Contains(x.KyHieu) || @params.MauSo == "-1")
                      && x.NgayHoaDon >= DateTime.Parse(@params.TuNgay) && x.NgayHoaDon <= DateTime.Parse(@params.DenNgay)
                      && (x.TrangThai == @params.TrangThaiHoaDon || @params.TrangThaiHoaDon == -1)
                      && (x.TrangThaiPhatHanh == @params.TrangThaiPhatHanh || @params.TrangThaiPhatHanh == -1)
                      && (x.TrangThaiGuiHoaDon == @params.TrangThaiGuiHoaDon || @params.TrangThaiGuiHoaDon == -1)
                      && ((x.SoLanChuyenDoi == 0 && @params.TrangThaiChuyenDoi == 0) || (x.SoLanChuyenDoi > 0 && @params.TrangThaiChuyenDoi == 1)
                      || @params.TrangThaiChuyenDoi == -1)
                      && (!arrTrangThaiKhongChon.Contains(x.TrangThai.Value))
                      )
                .OrderByDescending(x => x.NgayHoaDon)
                .ThenByDescending(x => x.SoHoaDon)
                .Select(hd => new HoaDonDienTuViewModel
                {
                    HoaDonDienTuId = hd.HoaDonDienTuId,
                    NgayHoaDon = hd.NgayHoaDon,
                    NgayLap = hd.NgayLap,
                    SoHoaDon = hd.SoHoaDon,
                    MauHoaDonId = hd.MauHoaDonId ?? string.Empty,
                    MauHoaDon = _mp.Map<MauHoaDonViewModel>(_db.MauHoaDons.FirstOrDefault(x => x.MauHoaDonId == hd.MauHoaDonId)),
                    MauSo = hd.MauSo,
                    KyHieu = hd.KyHieu,
                    KhachHangId = hd.KhachHangId ?? string.Empty,
                    KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.KhachHangId)),
                    TenKhachHang = hd.TenKhachHang,
                    EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                    TenNganHang = hd.TenNganHang,
                    HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                    SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                    SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                    NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                    NhanVienBanHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.NhanVienBanHangId)),
                    TenNhanVienBanHang = hd.TenNhanVienBanHang,
                    LoaiTienId = hd.LoaiTienId ?? string.Empty,
                    LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
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
                    TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien - x.TienChietKhau + x.TienThueGTGT),
                    HoaDonChiTiets = (
                                                from hdct in _db.HoaDonDienTuChiTiets
                                                join hddt in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hddt.HoaDonDienTuId into tmpHoaDons
                                                from hddt in tmpHoaDons.DefaultIfEmpty()
                                                join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                                from vt in tmpHangHoas.DefaultIfEmpty()
                                                join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                                from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                                where hdct.HoaDonDienTuId == hd.HoaDonDienTuId
                                                orderby vt.Ma descending
                                                select new HoaDonDienTuChiTietViewModel
                                                {
                                                    HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                    HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                                                    HoaDon = hd != null ? _mp.Map<HoaDonDienTuViewModel>(hd) : null,
                                                    HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                                                    HangHoaDichVu = vt != null ? _mp.Map<HangHoaDichVuViewModel>(vt) : null,
                                                    MaHang = hdct.MaHang,
                                                    TenHang = hdct.TenHang,
                                                    HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                                    DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                                    DonViTinh = dvt != null ? _mp.Map<DonViTinhViewModel>(dvt) : null,
                                                    SoLuong = hdct.SoLuong,
                                                    DonGia = hdct.DonGia,
                                                    DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                                    ThanhTien = hdct.ThanhTien,
                                                    ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                                    TyLeChietKhau = hdct.TyLeChietKhau,
                                                    TienChietKhau = hdct.TienChietKhau,
                                                    TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi,
                                                    ThueGTGT = hdct.ThueGTGT,
                                                    TienThueGTGT = hdct.TienThueGTGT,
                                                    TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi,
                                                    SoLo = hdct.SoLo,
                                                    HanSuDung = hdct.HanSuDung,
                                                    SoKhung = hdct.SoKhung,
                                                    SoMay = hdct.SoMay
                                                }).ToList(),
                });


                string excelFileName = string.Empty;
                string excelPath = string.Empty;

                // Export excel
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                else
                {
                    FileHelper.ClearFolder(uploadFolder);
                }

                excelFileName = $"BANG_KE_CHI_TIET_HOA_DON_DIEN_TU-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"docs/HoaDonDienTu/BANG_KE_CHI_TIET_HOA_DON_DIEN_TU.xlsx";
                string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
                FileInfo file = new FileInfo(_path_sample);
                string dateReport = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(@params.TuNgay).ToString("dd/MM/yyyy"), DateTime.Parse(@params.DenNgay).ToString("dd/MM/yyyy"));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    List<HoaDonDienTuViewModel> list = query.OrderBy(x => x.NgayHoaDon).ToList();
                    // Open sheet1
                    int totalRows = list.Sum(x => x.HoaDonChiTiets.Count);

                    // Begin row
                    int begin_row = 5;

                    // Open sheet1
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Add Row
                    worksheet.InsertRow(begin_row + 1, totalRows, begin_row);

                    // Fill data
                    int idx = begin_row;
                    int count = 1;
                    foreach (var it in list)
                    {
                        foreach (var ct in it.HoaDonChiTiets)
                        {
                            worksheet.Cells[idx, 1].Value = count.ToString();
                            worksheet.Cells[idx, 2].Value = it.NgayHoaDon.Value.ToString("dd/MM/yyyy");
                            worksheet.Cells[idx, 3].Value = !string.IsNullOrEmpty(it.SoHoaDon) ? it.SoHoaDon : "<Chưa cấp số>";
                            worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.MauHoaDon != null ? it.MauHoaDon.MauSo : string.Empty);
                            worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.MauHoaDon != null ? it.MauHoaDon.KyHieu : string.Empty);
                            worksheet.Cells[idx, 6].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.MauHoaDon != null ? it.KhachHang.Ma : string.Empty);
                            worksheet.Cells[idx, 7].Value = !string.IsNullOrEmpty(it.TenKhachHang) ? it.TenKhachHang : (it.KhachHang != null ? it.KhachHang.Ten : string.Empty);
                            worksheet.Cells[idx, 8].Value = !string.IsNullOrEmpty(it.DiaChi) ? it.DiaChi : (it.KhachHang != null ? it.KhachHang.DiaChi : string.Empty);
                            worksheet.Cells[idx, 9].Value = !string.IsNullOrEmpty(it.MaSoThue) ? it.MaSoThue : (it.KhachHang != null ? it.KhachHang.MaSoThue : string.Empty);
                            worksheet.Cells[idx, 10].Value = !string.IsNullOrEmpty(it.HoTenNguoiMuaHang) ? it.HoTenNguoiMuaHang : (it.KhachHang != null ? it.KhachHang.HoTenNguoiMuaHang : string.Empty);
                            worksheet.Cells[idx, 11].Value = it.HinhThucThanhToan != null ? it.HinhThucThanhToan.Ten : string.Empty;
                            worksheet.Cells[idx, 12].Value = it.LoaiTien != null ? it.LoaiTien.Ten : string.Empty;
                            worksheet.Cells[idx, 13].Value = it.TyGia ?? 1;
                            worksheet.Cells[idx, 14].Value = !string.IsNullOrEmpty(ct.MaHang) ? ct.MaHang : ((ct.HangHoaDichVu != null) ? ct.HangHoaDichVu.Ma : string.Empty);
                            worksheet.Cells[idx, 15].Value = !string.IsNullOrEmpty(ct.TenHang) ? ct.TenHang : ((ct.HangHoaDichVu != null) ? ct.HangHoaDichVu.Ten : string.Empty);
                            worksheet.Cells[idx, 16].Value = (ct.DonViTinh != null) ? ct.DonViTinh.Ten : string.Empty;
                            worksheet.Cells[idx, 17].Value = ct.SoLuong ?? 0;
                            worksheet.Cells[idx, 18].Value = ct.DonGia ?? 0;
                            worksheet.Cells[idx, 19].Value = ct.ThanhTien ?? 0;
                            worksheet.Cells[idx, 20].Value = ct.ThanhTienQuyDoi ?? 0;
                            worksheet.Cells[idx, 21].Value = ct.TyLeChietKhau ?? 0;
                            worksheet.Cells[idx, 22].Value = ct.TienChietKhau ?? 0;
                            worksheet.Cells[idx, 23].Value = ct.TienChietKhauQuyDoi ?? 0;
                            worksheet.Cells[idx, 24].Value = ct.ThueGTGT != "KCT" ? ct.ThueGTGT.ToString() + "%" : "\\";
                            worksheet.Cells[idx, 25].Value = ct.TienThueGTGT ?? 0;
                            worksheet.Cells[idx, 26].Value = ct.TienThueGTGTQuyDoi ?? 0;
                            worksheet.Cells[idx, 27].Value = ct.ThanhTien ?? 0 - ct.TienChietKhau ?? 0 + ct.TienThueGTGT ?? 0;
                            worksheet.Cells[idx, 28].Value = ct.ThanhTienQuyDoi ?? 0 - ct.TienChietKhauQuyDoi ?? 0 + ct.TienThueGTGTQuyDoi ?? 0;
                            worksheet.Cells[idx, 29].Value = ct.HangKhuyenMai == true ? "x" : string.Empty;
                            worksheet.Cells[idx, 30].Value = string.Empty;
                            worksheet.Cells[idx, 31].Value = ct.SoLo;
                            worksheet.Cells[idx, 32].Value = ct.HanSuDung.HasValue ? ct.HanSuDung.Value.ToString("dd/MM/yyyy") : string.Empty;
                            worksheet.Cells[idx, 33].Value = ct.SoKhung;
                            worksheet.Cells[idx, 34].Value = ct.SoMay;
                            worksheet.Cells[idx, 35].Value = string.Empty;
                            worksheet.Cells[idx, 36].Value = string.Empty;
                            worksheet.Cells[idx, 37].Value = !string.IsNullOrEmpty(it.MaNhanVienBanHang) ? it.MaNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ma : string.Empty);
                            worksheet.Cells[idx, 38].Value = !string.IsNullOrEmpty(it.TenNhanVienBanHang) ? it.TenNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ten : string.Empty);
                            worksheet.Cells[idx, 39].Value = it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : (it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang ? "Hóa đơn bán hàng" : it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonXoaBo ? "Hóa đơn xóa bỏ" : it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonDieuChinh ? "Hóa đơn điều chỉnh" : "Hóa đơn thay thế");
                            worksheet.Cells[idx, 40].Value = TrangThaiHoaDons.Where(x => x.TrangThaiId == it.TrangThai).Select(x => x.Ten).FirstOrDefault();
                            worksheet.Cells[idx, 41].Value = (it.TrangThaiPhatHanh == 0 ? "Chưa phát hành" : (it.TrangThaiPhatHanh == 1 ? "Đang phát hành" : (it.TrangThaiPhatHanh == 2 ? "Phát hành lỗi" : "Đã phát hành")));
                            worksheet.Cells[idx, 42].Value = it.MaTraCuu;
                            worksheet.Cells[idx, 43].Value = it.LyDoXoaBo;
                            worksheet.Cells[idx, 44].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                            worksheet.Cells[idx, 45].Value = it.NguoiLap != null ? it.NguoiLap.Ten : string.Empty;

                            idx += 1;
                            count += 1;
                        }
                    }
                    worksheet.Cells[2, 1].Value = dateReport;
                    //worksheet.Row(5).Style.Font.Color.SetColor(Color.Red);
                    // Total
                    worksheet.Row(idx).Style.Font.Bold = true;
                    worksheet.Cells[idx, 2].Value = string.Format("Số dòng = {0}", list.Count);
                    worksheet.Cells[idx, 19].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien));
                    worksheet.Cells[idx, 20].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi));
                    worksheet.Cells[idx, 22].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhau));
                    worksheet.Cells[idx, 23].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhauQuyDoi));
                    worksheet.Cells[idx, 25].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGT));
                    worksheet.Cells[idx, 26].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGTQuyDoi));
                    worksheet.Cells[idx, 27].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien - y.TienChietKhau + y.TienThueGTGT));
                    worksheet.Cells[idx, 28].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi - y.TienChietKhauQuyDoi + y.TienThueGTGTQuyDoi));
                    //replace Text


                    package.SaveAs(new FileInfo(excelPath));
                }

                return GetLinkExcelFile(excelFileName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model)
        {
            TienLuiViewModel result = new TienLuiViewModel();
            if (string.IsNullOrEmpty(model.ChungTuId))
            {
                return result;
            }

            var list = await _db.HoaDonDienTus
                .Select(x => new HoaDonDienTuViewModel
                {
                    HoaDonDienTuId = x.HoaDonDienTuId,
                    NgayHoaDon = x.NgayHoaDon,
                    SoHoaDon = x.SoHoaDon
                })
                .OrderBy(x => x.NgayHoaDon)
                .ThenBy(x => x.SoHoaDon)
                .ToListAsync();

            var length = list.Count();
            var currentIndex = list.FindIndex(x => x.HoaDonDienTuId == model.ChungTuId);
            if (currentIndex != -1)
            {
                if (currentIndex > 0)
                {
                    result.TruocId = list[currentIndex - 1].HoaDonDienTuId;
                    result.VeDauId = list[0].HoaDonDienTuId;
                }
                if (currentIndex < (length - 1))
                {
                    result.SauId = list[currentIndex + 1].HoaDonDienTuId;
                    result.VeCuoiId = list[length - 1].HoaDonDienTuId;
                }
            }

            return result;
        }

        public async Task<string> CreateSoCTXoaBoHoaDon()
        {
            var result = string.Empty;
            try
            {
                var maxSoCT = await _db.HoaDonDienTus.Where(x => !string.IsNullOrEmpty(x.SoCTXoaBo))
                                                    .MaxAsync(x => x.SoCTXoaBo);
                if (!string.IsNullOrEmpty(maxSoCT))
                {
                    var number = maxSoCT.Substring(3);
                    var next = int.Parse(number) + 1;
                    result = "XHĐ" + next.ToString("00000");
                }
                else result = "XHĐ00001";
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return result;
        }

        public async Task<KetQuaCapSoHoaDon> CreateSoHoaDon(HoaDonDienTuViewModel hd)
        {
            try
            {
                var validMaxSoHoaDon = _db.HoaDonDienTus
                                        .Where(x => x.MauHoaDonId == hd.MauHoaDonId && !string.IsNullOrEmpty(x.SoHoaDon))
                                        .Max(x => x.SoHoaDon);

                var thongBaoPhatHanh = await _db.ThongBaoPhatHanhChiTiets
                                       .Include(x => x.ThongBaoPhatHanh)
                                       .Where(x => x.MauHoaDonId == hd.MauHoaDonId)
                                       .OrderByDescending(x => x.NgayBatDauSuDung)
                                       .FirstOrDefaultAsync();

                if (thongBaoPhatHanh == null)
                {
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.ChuaTimThayThongBaoPhatHanh,
                        SoHoaDon = string.Empty
                    };
                }
                else if (thongBaoPhatHanh.ThongBaoPhatHanh.TrangThaiNop != TrangThaiNop.DaDuocChapNhan)
                {
                    var converMaxToInt = int.Parse(validMaxSoHoaDon);
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.ChuaDuocCQTChapNhan,
                        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.ToString("0000000") :
                                   (converMaxToInt + 1).ToString("0000000")) : thongBaoPhatHanh.TuSo.Value.ToString("0000000")
                    };
                }
                else if (thongBaoPhatHanh.NgayBatDauSuDung > hd.NgayHoaDon)
                {
                    var converMaxToInt = int.Parse(validMaxSoHoaDon);
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.NgayHoaDonNhoHonNgayBatDauSuDung,
                        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.ToString("0000000") :
                                   (converMaxToInt + 1).ToString("0000000")) : thongBaoPhatHanh.TuSo.Value.ToString("0000000"),
                        ErrorMessage = $"Ngày hóa đơn không được nhỏ hơn ngày bắt đầu sử dụng của hóa đơn trên thông báo phát hành hóa đơn <{thongBaoPhatHanh.NgayBatDauSuDung.Value.ToString("dd/MM/yyyy")}>"
                    };
                }
                else if (DateTime.Now > hd.NgayHoaDon)
                {
                    var converMaxToInt = int.Parse(validMaxSoHoaDon);
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.NgayHoaDonNhoHonNgayKy,
                        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.ToString("0000000") :
                                   (converMaxToInt + 1).ToString("0000000")) : thongBaoPhatHanh.TuSo.Value.ToString("0000000")
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(validMaxSoHoaDon))
                    {
                        var converMaxToInt = int.Parse(validMaxSoHoaDon);
                        if (converMaxToInt < thongBaoPhatHanh.TuSo)
                        {
                            return new KetQuaCapSoHoaDon
                            {
                                LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                                SoHoaDon = thongBaoPhatHanh.TuSo.Value.ToString("0000000")
                            };
                        }
                        else if (converMaxToInt >= thongBaoPhatHanh.DenSo)
                        {
                            return new KetQuaCapSoHoaDon
                            {
                                LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.SoHoaDonVuotQuaGioiHanDangKy,
                                SoHoaDon = (converMaxToInt + 1).ToString("0000000")
                            };
                        }
                        else
                        {
                            var _hdNgayNhoHon = _db.HoaDonDienTus.Where(x => x.NgayHoaDon < hd.NgayHoaDon && x.MauHoaDonId == hd.MauHoaDonId && !string.IsNullOrEmpty(x.SoHoaDon)).ToList();
                            if (_hdNgayNhoHon.Any())
                            {
                                foreach (var item in _hdNgayNhoHon)
                                {
                                    if (int.Parse(item.SoHoaDon) > int.Parse(validMaxSoHoaDon) + 1)
                                    {
                                        return new KetQuaCapSoHoaDon
                                        {
                                            LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.SoHoaDonNhoHonSoHoaDonTruocDo,
                                            SoHoaDon = (converMaxToInt + 1).ToString("0000000"),
                                        };
                                    }
                                }

                                return new KetQuaCapSoHoaDon
                                {
                                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                                    SoHoaDon = (converMaxToInt + 1).ToString("0000000")
                                };
                            }
                            else
                            {
                                return new KetQuaCapSoHoaDon
                                {
                                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                                    SoHoaDon = (converMaxToInt + 1).ToString("0000000")
                                };
                            }
                        }
                    }
                    else
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                            SoHoaDon = thongBaoPhatHanh.TuSo.Value.ToString("0000000")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                return new KetQuaCapSoHoaDon
                {
                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.LoiKhac,
                    SoHoaDon = string.Empty
                };
            }
        }

        public async Task<ResultParams> CapPhatSoHoaDon(HoaDonDienTuViewModel hd, string soHoaDon)
        {
            var result = new ResultParams();
            try
            {
                hd.SoHoaDon = soHoaDon;
                var updateRes = await this.UpdateAsync(hd);
                if (updateRes)
                {
                    result = new ResultParams
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    result = new ResultParams
                    {
                        Success = false,
                        ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                    };
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                result = new ResultParams
                {
                    Success = false,
                    ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                };
            }
            return result;
        }

        public async Task<ResultParams> CapPhatSoHoaDonHangLoat(List<HoaDonDienTuViewModel> hd, List<string> soHoaDon)
        {
            var result = new ResultParams();
            try
            {
                for (int i = 0; i < hd.Count; i++)
                {
                    hd[i].SoHoaDon = soHoaDon[i];
                }
                var updateRes = await this.UpdateRangeAsync(hd);
                if (updateRes)
                {
                    result = new ResultParams
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    result = new ResultParams
                    {
                        Success = false,
                        ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                    };
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                result = new ResultParams
                {
                    Success = false,
                    ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                };
            }
            return result;
        }

        public async Task<List<ChiTietMauHoaDon>> GetListChiTietByMauHoaDon(string mauHoaDonId)
        {
            var result = new List<ChiTietMauHoaDon>();
            try
            {
                var mhd = _mp.Map<MauHoaDonViewModel>(await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == mauHoaDonId));
                var listBanMau = new List<BanMauHoaDon>();
                string jsonFolder = Path.Combine(_hostingEnvironment.WebRootPath, "jsons/mau-hoa-don.json");
                using (StreamReader r = new StreamReader(jsonFolder))
                {
                    string json = r.ReadToEnd();
                    listBanMau = JsonConvert.DeserializeObject<List<BanMauHoaDon>>(json);
                }

                var banMau = listBanMau.FirstOrDefault(x => x.TenBanMau == mhd.TenBoMau);
                if (banMau != null) result = banMau.ChiTiets;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return result;
        }

        public async Task<string> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd)
        {
            var path = string.Empty;
            try
            {
                var _tuyChons = await _TuyChonService.GetAllAsync();

                var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
                var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
                var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());

                var _banMauHD = await _MauHoaDonService.GetChiTietByMauHoaDon(hd.MauHoaDonId);
                var _thongTinNguoiBan = await _HoSoHDDTService.GetDetailAsync();
                if (_thongTinNguoiBan != null)
                {
                    Document doc = new Document();
                    string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/MauHoaDon/{_banMauHD.File}");
                    doc.LoadFromFile(docFolder);

                    doc.Replace("<CompanyName>", _thongTinNguoiBan.TenDonVi ?? string.Empty, true, true);
                    doc.Replace("<taxcode>", _thongTinNguoiBan.MaSoThue ?? string.Empty, true, true);
                    doc.Replace("<Address>", _thongTinNguoiBan.DiaChi ?? string.Empty, true, true);
                    doc.Replace("<Tel>", _thongTinNguoiBan.SoDienThoaiLienHe ?? string.Empty, true, true);
                    doc.Replace("<Bankaccount>", _thongTinNguoiBan.SoTaiKhoanNganHang ?? string.Empty, true, true);

                    doc.Replace("<numberSample>", hd.MauSo ?? string.Empty, true, true);
                    doc.Replace("<sign>", hd.KyHieu ?? string.Empty, true, true);
                    doc.Replace("<orderNumber>", hd.SoHoaDon ?? "<Chưa cấp số>", true, true);

                    doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                    doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                    doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);


                    doc.Replace("<customerName>", hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
                    doc.Replace("<customerCompany>", hd.KhachHang.TenDonVi ?? string.Empty, true, true);
                    doc.Replace("<customerTaxCode>", hd.MaSoThue ?? string.Empty, true, true);
                    doc.Replace("<customerAddress>", hd.DiaChi ?? string.Empty, true, true);
                    doc.Replace("<kindOfPayment>", hd.HinhThucThanhToan.Ten ?? string.Empty, true, true);
                    doc.Replace("<accountNumber>", hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

                    List<Table> listTable = new List<Table>();
                    Paragraph _par;
                    string stt = string.Empty;
                    foreach (Table tb in doc.Sections[0].Tables)
                    {
                        if (tb.Rows.Count > 0)
                        {
                            foreach (Paragraph par in tb.Rows[0].Cells[0].Paragraphs)
                            {
                                stt = par.Text;
                            }
                            if (stt.ToTrim().ToUpper().Contains("STT"))
                            {
                                listTable.Add(tb);
                                continue;
                            }
                        }
                    }

                    List<HoaDonDienTuChiTietViewModel> models = await _HoaDonDienTuChiTietService.GetChiTietHoaDonAsync(hd.HoaDonDienTuId);
                    int line = models.Count();
                    if (line > 0)
                    {
                        int beginRow = 1;
                        Table table = null;
                        table = listTable[0];
                        doc.Replace("<vatAmount>", models.Sum(x => x.TienThueGTGT.Value).FormatPriceTwoDecimal() ?? string.Empty, true, true);
                        doc.Replace("<totalAmount>", models.Sum(x => x.ThanhTien.Value).FormatQuanity() ?? string.Empty, true, true);
                        doc.Replace("<vatRate>", models.Select(x => x.ThueGTGT).FirstOrDefault() ?? string.Empty, true, true);
                        doc.Replace("<totalPayment>", (hd.TongTienThanhToan ?? 0).FormatPriceTwoDecimal() ?? string.Empty, true, true);
                        doc.Replace("<amountInWords>", (hd.TongTienThanhToan ?? 0).ConvertToInWord(_cachDocSo0HangChuc, _cachDocHangNghin, _hienThiSoChan) ?? string.Empty, true, true);


                        for (int i = 0; i < line - 1; i++)
                        {
                            // Clone row
                            TableRow cl_row = table.Rows[1].Clone();
                            table.Rows.Insert(1, cl_row);
                        }

                        TableRow row = null;
                        if (_banMauHD.LoaiThueGTGT == 1)
                        {
                            for (int i = 0; i < line; i++)
                            {
                                row = table.Rows[i + beginRow];

                                _par = row.Cells[0].Paragraphs[0];
                                _par.Text = (i + 1).ToString();

                                _par = row.Cells[1].Paragraphs[0];
                                _par.Text = models[i].TenHang ?? string.Empty;

                                _par = row.Cells[2].Paragraphs[0];
                                _par.Text = models[i].DonViTinh?.Ten ?? string.Empty;

                                _par = row.Cells[3].Paragraphs[0];
                                _par.Text = models[i].SoLuong.Value.FormatQuanity() ?? string.Empty;

                                _par = row.Cells[4].Paragraphs[0];
                                _par.Text = models[i].DonGia.Value.FormatPriceTwoDecimal() ?? string.Empty;

                                _par = row.Cells[5].Paragraphs[0];
                                _par.Text = models[i].ThanhTien.Value.FormatPriceTwoDecimal() ?? string.Empty;
                            }
                        }
                        else
                        {

                            for (int i = 0; i < line; i++)
                            {
                                row = table.Rows[i + beginRow];

                                _par = row.Cells[0].Paragraphs[0];
                                _par.Text = (i + 1).ToString();

                                _par = row.Cells[1].Paragraphs[0];
                                _par.Text = models[i].TenHang ?? string.Empty;

                                _par = row.Cells[2].Paragraphs[0];
                                _par.Text = models[i].DonViTinh?.Ten ?? string.Empty;

                                _par = row.Cells[3].Paragraphs[0];
                                _par.Text = models[i].SoLuong.Value.FormatQuanity() ?? string.Empty;

                                _par = row.Cells[4].Paragraphs[0];
                                _par.Text = models[i].DonGia.Value.FormatPriceTwoDecimal() ?? string.Empty;

                                _par = row.Cells[5].Paragraphs[0];
                                _par.Text = models[i].ThanhTien.Value.FormatPriceTwoDecimal() ?? string.Empty;

                                _par = row.Cells[6].Paragraphs[0];
                                _par.Text = models[i].ThueGTGT ?? string.Empty;

                                _par = row.Cells[7].Paragraphs[0];
                                _par.Text = models[i].TienThueGTGT.Value.FormatPriceTwoDecimal() ?? string.Empty;

                            }
                        }
                    }
                    else
                    {
                        doc.Replace("<totalPayment>", string.Empty, true, true);
                        doc.Replace("<amountInWords>", string.Empty, true, true);
                        doc.Replace("<vatAmount>", string.Empty, true, true);
                        doc.Replace("<vatRate>", string.Empty, true, true);
                        doc.Replace("<totalAmount>", string.Empty, true, true);
                    }

                    doc.Replace("<codeSearch>", hd.MaTraCuu ?? string.Empty, true, true);
                    doc.Replace("<linkSearch>", "hoadonbachkhoa.pmbk.vn", true, true);

                    var pdfFolder = string.Empty;

                    if (hd.TrangThaiPhatHanh != 3)
                        pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf/unsigned/");
                    else
                    {
                        pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf/signed/");
                    }

                    var pdfFileName = (hd.LoaiHoaDon == 1 ? "Hoa_Don_GTGT_" : "Hoa_don_ban_hang_") + (!string.IsNullOrEmpty(hd.SoHoaDon) ? hd.SoHoaDon : string.Empty
                        ) + hd.NgayHoaDon.Value.ToString("yyyyMMddhhmmss") + ".pdf";
                    if (!Directory.Exists(pdfFolder))
                    {
                        Directory.CreateDirectory(pdfFolder);
                    }


                    doc.SaveToFile(pdfFolder + pdfFileName, FileFormat.PDF);
                    if (hd.TrangThaiPhatHanh != (int)TrangThaiPhatHanh.DaPhatHanh)
                    {
                        path = GetLinkFileUnsignedPdf(pdfFileName);
                    }
                    else
                    {
                        path = GetLinkFileSignedPdf(pdfFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return path;
        }

        private string GetLinkFileUnsignedPdf(string link)
        {
            var filename = "FilesUpload/pdf/unsigned/" + link;
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

        private string GetLinkFileSignedPdf(string link)
        {
            var filename = "FilesUpload/pdf/signed/" + link;
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

        private string GetLinkFile(string link)
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

        public async Task<KetQuaChuyenDoi> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay @params)
        {
            try
            {
                var _objHDDT = await GetByIdAsync(@params.HoaDonDienTuId);
                if (_objHDDT != null)
                {
                    var pathPdf = await ConvertHoaDonToHoaDonGiay(_objHDDT);
                    if (!string.IsNullOrEmpty(pathPdf))
                    {
                        _objHDDT.SoLanChuyenDoi += 1;
                        if (await UpdateAsync(_objHDDT))
                        {
                            var _objThongTinChuyenDoi = new ThongTinChuyenDoiViewModel
                            {
                                HoaDonDienTuId = @params.HoaDonDienTuId,
                                NgayChuyenDoi = DateTime.Now,
                                NguoiChuyenDoiId = @params.NguoiChuyenDoiId
                            };

                            await _db.ThongTinChuyenDois.AddAsync(_mp.Map<ThongTinChuyenDoi>(_objThongTinChuyenDoi));
                            await _db.SaveChangesAsync();

                            return new KetQuaChuyenDoi
                            {
                                ThanhCong = true,
                                Loi = string.Empty,
                                PathFile = pathPdf
                            };
                        }
                        else
                        {
                            return new KetQuaChuyenDoi
                            {
                                ThanhCong = false,
                                Loi = "Không cập nhật được trạng thái hóa đơn",
                                PathFile = string.Empty
                            };
                        }
                    }
                    else
                    {
                        return new KetQuaChuyenDoi
                        {
                            ThanhCong = false,
                            Loi = "Không chuyển được sang dạng hóa đơn giấy",
                            PathFile = string.Empty
                        };
                    }
                }
                else
                {
                    return new KetQuaChuyenDoi
                    {
                        ThanhCong = false,
                        Loi = "Không tìm được hóa đơn",
                        PathFile = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                return new KetQuaChuyenDoi
                {
                    ThanhCong = false,
                    Loi = "Bị lỗi khi chuyển đổi hóa đơn, đề nghị liên lạc với admin",
                    PathFile = string.Empty
                };
            }
        }

        private async Task<string> ConvertHoaDonToHoaDonGiay(HoaDonDienTuViewModel hd)
        {
            var path = string.Empty;
            try
            {
                var _tuyChons = await _TuyChonService.GetAllAsync();

                var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
                var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
                var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());

                var _banMauHD = await _MauHoaDonService.GetChiTietByMauHoaDon(hd.MauHoaDonId);
                var _thongTinNguoiBan = await _HoSoHDDTService.GetDetailAsync();
                if (_thongTinNguoiBan != null)
                {
                    Document doc = new Document();
                    string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/MauChuyenDoi/{_banMauHD.File}");
                    doc.LoadFromFile(docFolder);

                    doc.Replace("<CompanyName>", _thongTinNguoiBan.TenDonVi ?? string.Empty, true, true);
                    doc.Replace("<TaxCode>", _thongTinNguoiBan.MaSoThue ?? string.Empty, true, true);
                    doc.Replace("<Address>", _thongTinNguoiBan.DiaChi ?? string.Empty, true, true);
                    doc.Replace("<Tel>", _thongTinNguoiBan.SoDienThoaiLienHe ?? string.Empty, true, true);
                    doc.Replace("<BankNumber>", _thongTinNguoiBan.SoTaiKhoanNganHang ?? string.Empty, true, true);

                    doc.Replace("<numberSample>", hd.MauSo ?? string.Empty, true, true);
                    doc.Replace("<sign>", hd.KyHieu ?? string.Empty, true, true);
                    doc.Replace("<orderNumber>", hd.SoHoaDon ?? "<Chưa cấp số>", true, true);

                    doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                    doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                    doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);


                    doc.Replace("<customerName>", hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
                    doc.Replace("<customerCompany>", hd.KhachHang.TenDonVi ?? string.Empty, true, true);
                    doc.Replace("<customerTaxCode>", hd.MaSoThue ?? string.Empty, true, true);
                    doc.Replace("<customerAddress>", hd.DiaChi ?? string.Empty, true, true);
                    doc.Replace("<kindOfPayment>", hd.HinhThucThanhToan.Ten ?? string.Empty, true, true);
                    doc.Replace("<accountNumber>", hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

                    List<Table> listTable = new List<Table>();
                    Paragraph _par;
                    string stt = string.Empty;
                    foreach (Table tb in doc.Sections[0].Tables)
                    {
                        if (tb.Rows.Count > 0)
                        {
                            foreach (Paragraph par in tb.Rows[0].Cells[0].Paragraphs)
                            {
                                stt = par.Text;
                            }
                            if (stt.ToTrim().ToUpper().Contains("STT"))
                            {
                                listTable.Add(tb);
                                continue;
                            }
                        }
                    }

                    List<HoaDonDienTuChiTietViewModel> models = await _HoaDonDienTuChiTietService.GetChiTietHoaDonAsync(hd.HoaDonDienTuId);
                    int line = models.Count();
                    if (line > 0)
                    {
                        int beginRow = 1;
                        Table table = null;
                        table = listTable[0];
                        doc.Replace("<vatAmount>", models.Sum(x => x.TienThueGTGT.Value).FormatPriceTwoDecimal() ?? string.Empty, true, true);
                        doc.Replace("<totalAmount>", models.Sum(x => x.ThanhTien.Value).FormatQuanity() ?? string.Empty, true, true);
                        doc.Replace("<vatRate>", models.Select(x => x.ThueGTGT).FirstOrDefault() ?? string.Empty, true, true);
                        doc.Replace("<totalPayment>", hd.TongTienThanhToan.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                        doc.Replace("<amountInWords>", hd.TongTienThanhToan.Value.ConvertToInWord(_cachDocSo0HangChuc, _cachDocHangNghin, _hienThiSoChan) ?? string.Empty, true, true);


                        for (int i = 0; i < line - 1; i++)
                        {
                            // Clone row
                            TableRow cl_row = table.Rows[1].Clone();
                            table.Rows.Insert(1, cl_row);
                        }

                        TableRow row = null;
                        if (_banMauHD.LoaiThueGTGT == 1)
                        {
                            for (int i = 0; i < line; i++)
                            {
                                row = table.Rows[i + beginRow];

                                _par = row.Cells[0].Paragraphs[0];
                                _par.Text = (i + 1).ToString();

                                _par = row.Cells[1].Paragraphs[0];
                                _par.Text = models[i].TenHang ?? string.Empty;

                                _par = row.Cells[2].Paragraphs[0];
                                _par.Text = models[i].DonViTinh?.Ten ?? string.Empty;

                                _par = row.Cells[3].Paragraphs[0];
                                _par.Text = models[i].SoLuong.Value.FormatQuanity() ?? string.Empty;

                                _par = row.Cells[4].Paragraphs[0];
                                _par.Text = models[i].DonGia.Value.FormatPriceTwoDecimal() ?? string.Empty;

                                _par = row.Cells[5].Paragraphs[0];
                                _par.Text = models[i].ThanhTien.Value.FormatPriceTwoDecimal() ?? string.Empty;
                            }
                        }
                        else
                        {

                            for (int i = 0; i < line; i++)
                            {
                                row = table.Rows[i + beginRow];

                                _par = row.Cells[0].Paragraphs[0];
                                _par.Text = (i + 1).ToString();

                                _par = row.Cells[1].Paragraphs[0];
                                _par.Text = models[i].TenHang ?? string.Empty;

                                _par = row.Cells[2].Paragraphs[0];
                                _par.Text = models[i].DonViTinh?.Ten ?? string.Empty;

                                _par = row.Cells[3].Paragraphs[0];
                                _par.Text = models[i].SoLuong.Value.FormatQuanity() ?? string.Empty;

                                _par = row.Cells[4].Paragraphs[0];
                                _par.Text = models[i].DonGia.Value.FormatPriceTwoDecimal() ?? string.Empty;

                                _par = row.Cells[5].Paragraphs[0];
                                _par.Text = models[i].ThanhTien.Value.FormatPriceTwoDecimal() ?? string.Empty;

                                _par = row.Cells[6].Paragraphs[0];
                                _par.Text = models[i].ThueGTGT ?? string.Empty;

                                _par = row.Cells[7].Paragraphs[0];
                                _par.Text = models[i].TienThueGTGT.Value.FormatPriceTwoDecimal() ?? string.Empty;

                            }
                        }
                    }
                    else
                    {
                        doc.Replace("<totalPayment>", string.Empty, true, true);
                        doc.Replace("<amountInWords>", string.Empty, true, true);
                        doc.Replace("<vatAmount>", string.Empty, true, true);
                        doc.Replace("<vatRate>", string.Empty, true, true);
                        doc.Replace("<totalAmount>", string.Empty, true, true);
                    }

                    doc.Replace("<codeSearch>", hd.MaTraCuu ?? string.Empty, true, true);
                    doc.Replace("<linkSearch>", "hoadonbachkhoa.pmbk.vn", true, true);

                    var pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf_convert");

                    var pdfFileName = (hd.LoaiHoaDon == 1 ? "Hoa_Don_GTGT_" : "Hoa_don_ban_hang_") + (!string.IsNullOrEmpty(hd.SoHoaDon) ? hd.SoHoaDon : string.Empty
                        ) + hd.NgayHoaDon.Value.ToString("yyyyMMddhhmmss");
                    if (!Directory.Exists(pdfFolder))
                    {
                        Directory.CreateDirectory(pdfFolder);
                    }


                    doc.SaveToFile(pdfFolder + pdfFileName, FileFormat.PDF);

                    var modelNK = new NhatKyThaoTacHoaDonViewModel
                    {
                        HoaDonDienTuId = hd.HoaDonDienTuId,
                        NgayGio = DateTime.Now,
                        KhachHangId = hd.KhachHangId,
                        LoaiThaoTac = (int)LoaiThaoTac.ChuyenThanhHoaDonGiay,
                        MoTa = "Đã chuyển hóa đơn số " + hd.SoHoaDon + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + " thành hóa đơn giấy",
                        HasError = false,
                        ErrorMessage = "",
                        DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                    };

                    await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return path;
        }


        public async Task<bool> ThemNhatKyThaoTacHoaDonAsync(NhatKyThaoTacHoaDonViewModel model)
        {
            try
            {
                var entity = _mp.Map<NhatKyThaoTacHoaDon>(model);
                await _db.AddAsync<NhatKyThaoTacHoaDon>(entity);
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return false;
        }

        public async Task<LuuTruTrangThaiFileHDDTViewModel> GetTrangThaiLuuTru(string HoaDonDienTuId)
        {
            var result = await _db.LuuTruTrangThaiFileHDDTs.Where(x => x.HoaDonDienTuId == HoaDonDienTuId)
                                                    .FirstOrDefaultAsync();

            return _mp.Map<LuuTruTrangThaiFileHDDTViewModel>(result);
        }

        public async Task<bool> UpdateTrangThaiLuuFileHDDT(LuuTruTrangThaiFileHDDTViewModel model)
        {
            try
            {
                var entity = await _db.LuuTruTrangThaiFileHDDTs.Where(x => x.HoaDonDienTuId == model.HoaDonDienTuId)
                                                    .FirstOrDefaultAsync();

                if (entity != null)
                {
                    _db.Entry(entity).CurrentValues.SetValues(model);
                }
                else
                {
                    entity = _mp.Map<LuuTruTrangThaiFileHDDT>(model);
                    await _db.LuuTruTrangThaiFileHDDTs.AddAsync(entity);
                }
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return false;
        }

        public async Task<bool> UpdateTrangThaiLuuFileBBXB(LuuTruTrangThaiBBXBViewModel model)
        {
            try
            {
                var entity = await _db.LuuTruTrangThaiBBXBs.Where(x => x.BienBanXoaBoId == model.BienBanXoaBoId)
                                                    .FirstOrDefaultAsync();

                if (entity != null)
                {
                    _db.Entry(entity).CurrentValues.SetValues(model);
                }
                else
                {
                    entity = _mp.Map<LuuTruTrangThaiBBXB>(model);
                    await _db.LuuTruTrangThaiBBXBs.AddAsync(entity);
                }
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return false;
        }


        public async Task GateForWebSocket(ParamPhatHanhHD param)
        {
            try
            {
                if (!string.IsNullOrEmpty(param.HoaDonDienTuId))
                {
                    var _objHDDT = await this.GetByIdAsync(param.HoaDonDienTuId);
                    if (_objHDDT != null)
                    {
                        // Delete file if exist
                        if (!string.IsNullOrEmpty(_objHDDT.FileDaKy))
                        {
                            FileHelper.DeleteFile(Path.Combine(_hostingEnvironment.ContentRootPath, $"Assets/uploaded/pdf/signed/instances/{_objHDDT.FileDaKy}"));
                        }

                        var _sampleFile = await _MauHoaDonService.GetChiTietByMauHoaDon(_objHDDT.MauHoaDonId);
                        // Create name file.
                        String pre = new String(_objHDDT.FileDaKy.Where(Char.IsLetterOrDigit).ToArray());
                        string newFileName = $"{pre}_{param.HoaDon.SoHoaDon}_{Guid.NewGuid()}.pdf";

                        _objHDDT.FileDaKy = newFileName;
                        _objHDDT.TrangThaiPhatHanh = (int)TrangThaiPhatHanh.DaPhatHanh;
                        _objHDDT.SoHoaDon = param.HoaDon.SoHoaDon;

                        await this.UpdateAsync(_objHDDT);

                        var _objTrangThaiLuuTru = await GetTrangThaiLuuTru(_objHDDT.HoaDonDienTuId);
                        _objTrangThaiLuuTru = _objTrangThaiLuuTru != null ? _objTrangThaiLuuTru : new LuuTruTrangThaiFileHDDTViewModel();
                        if (string.IsNullOrEmpty(_objTrangThaiLuuTru.HoaDonDienTuId)) _objTrangThaiLuuTru.HoaDonDienTuId = _objHDDT.HoaDonDienTuId;

                        // PDF 
                        string signedPdfFolder = $"Assets/uploaded/pdf/signed/instances/{newFileName}";
                        string signedPdfPath = Path.Combine(_hostingEnvironment.ContentRootPath, signedPdfFolder);
                        byte[] bytePDF = DataHelper.StringToByteArray(@param.DataPDF);
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        System.IO.File.WriteAllBytes(signedPdfPath, _objTrangThaiLuuTru.PdfDaKy);

                        //xml
                        string signedXmlFolder = $"Assets/uploaded/xml/signed/{newFileName.Replace(".pdf", ".xml")}";
                        string signedXmlPath = Path.Combine(_hostingEnvironment.ContentRootPath, signedXmlFolder);
                        string xmlDeCode = DataHelper.Base64Decode(@param.DataXML);
                        System.IO.File.WriteAllText(signedXmlPath, xmlDeCode);
                        _objTrangThaiLuuTru.PdfChuaKy = null;
                        _objTrangThaiLuuTru.XMLChuaKy = null;
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        _objTrangThaiLuuTru.XMLDaKy = Encoding.UTF8.GetBytes(@param.DataXML);
                        await this.UpdateTrangThaiLuuFileHDDT(_objTrangThaiLuuTru);

                        //nhật ký thao tác hóa đơn
                        var modelNK = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                            NgayGio = DateTime.Now,
                            KhachHangId = _objHDDT.KhachHangId,
                            LoaiThaoTac = (int)LoaiThaoTac.PhatHanhHoaDon,
                            MoTa = "Đã phát hành hóa đơn số " + _objHDDT.SoHoaDon + " ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            HasError = false,
                            ErrorMessage = string.Empty,
                            DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                        };

                        await ThemNhatKyThaoTacHoaDonAsync(modelNK);

                        if (param.TuDongGuiMail)
                        {
                            if (_objHDDT.KhachHang != null && !string.IsNullOrEmpty(_objHDDT.KhachHang.EmailNguoiNhanHD) && _objHDDT.KhachHang.EmailNguoiNhanHD.IsValidEmail() && await SendEmail(param.HoaDon))
                            {
                                modelNK = new NhatKyThaoTacHoaDonViewModel
                                {
                                    HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                                    NgayGio = DateTime.Now,
                                    KhachHangId = _objHDDT.KhachHangId,
                                    LoaiThaoTac = (int)LoaiThaoTac.GuiHoaDon,
                                    MoTa = "Đã gửi thông báo phát hành hóa đơn số " + _objHDDT.SoHoaDon + " cho khách hàng " + _objHDDT.HoTenNguoiNhanHD + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                                    HasError = false,
                                    ErrorMessage = string.Empty,
                                    DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                                };

                                await ThemNhatKyThaoTacHoaDonAsync(modelNK);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                return;
            }
        }

        public async Task<LuuTruTrangThaiBBXBViewModel> GetTrangThaiLuuTruBBXB(string BienBanXoaBoId)
        {
            var result = await _db.LuuTruTrangThaiBBXBs.Where(x => x.BienBanXoaBoId == BienBanXoaBoId)
                                                    .FirstOrDefaultAsync();

            return _mp.Map<LuuTruTrangThaiBBXBViewModel>(result);
        }

        public async Task GateForWebSocket(ParamKyBienBanHuyHoaDon param)
        {
            try
            {
                if (!string.IsNullOrEmpty(param.BienBan.HoaDonDienTuId))
                {
                    var _objHDDT = await this.GetByIdAsync(param.BienBan.HoaDonDienTuId);
                    if (_objHDDT != null)
                    {
                        // Delete file if exist
                        if (!string.IsNullOrEmpty(param.BienBan.FileDaKy))
                        {
                            FileHelper.DeleteFile(Path.Combine(_hostingEnvironment.ContentRootPath, $"Assets/uploaded/pdf/signed/instances/{param.BienBan.FileDaKy}"));
                        }

                        var _sampleFile = await _MauHoaDonService.GetChiTietByMauHoaDon(_objHDDT.MauHoaDonId);
                        // Create name file.
                        String pre = new String(param.BienBan.FileDaKy.Where(Char.IsLetterOrDigit).ToArray());
                        string newFileName = $"{pre}_{_objHDDT.SoHoaDon}_{Guid.NewGuid()}.pdf";

                        param.BienBan.FileDaKy = newFileName;
                        param.BienBan.NgayKyBenA = DateTime.Now;
                        var entity = _db.BienBanXoaBos.FirstOrDefault(x => x.Id == param.BienBan.Id);
                        if (entity != null)
                        {
                            _db.Entry<BienBanXoaBo>(entity).CurrentValues.SetValues(param.BienBan);
                        }
                        else
                        {
                            if (param.BienBan.Id == "")
                            {
                                param.BienBan.Id = Guid.NewGuid().ToString();
                            }

                            _db.BienBanXoaBos.Add(_mp.Map<BienBanXoaBo>(param.BienBan));
                            _db.SaveChanges();
                        }

                        _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaGuiKH;
                        await this.UpdateAsync(_objHDDT);

                        var _objTrangThaiLuuTru = await GetTrangThaiLuuTruBBXB(param.BienBan.Id);
                        _objTrangThaiLuuTru = _objTrangThaiLuuTru != null ? _objTrangThaiLuuTru : new LuuTruTrangThaiBBXBViewModel();
                        if (string.IsNullOrEmpty(_objTrangThaiLuuTru.BienBanXoaBoId)) _objTrangThaiLuuTru.BienBanXoaBoId = param.BienBan.Id;

                        // PDF 
                        string signedPdfFolder = $"Assets/uploaded/pdf/signed/bienBanXoaBo/{newFileName}";
                        string signedPdfPath = Path.Combine(_hostingEnvironment.ContentRootPath, signedPdfFolder);
                        byte[] bytePDF = DataHelper.StringToByteArray(@param.DataPDF);
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        System.IO.File.WriteAllBytes(signedPdfPath, _objTrangThaiLuuTru.PdfDaKy);

                        //xml
                        string signedXmlFolder = $"Assets/uploaded/xml/signed/{newFileName.Replace(".pdf", ".xml")}";
                        string signedXmlPath = Path.Combine(_hostingEnvironment.ContentRootPath, signedXmlFolder);
                        string xmlDeCode = DataHelper.Base64Decode(@param.DataXML);
                        System.IO.File.WriteAllText(signedXmlPath, xmlDeCode);
                        _objTrangThaiLuuTru.PdfChuaKy = null;
                        _objTrangThaiLuuTru.XMLChuaKy = null;
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        _objTrangThaiLuuTru.XMLDaKy = Encoding.UTF8.GetBytes(@param.DataXML);
                        await this.UpdateTrangThaiLuuFileBBXB(_objTrangThaiLuuTru);
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                return;
            }
        }


        public async Task<bool> SendEmail(HoaDonDienTuViewModel hddt, string TenNguoiNhan = "", string ToMail = "")
        {
            try
            {
                string pdfFileFolder = $"Assets/uploaded/pdf/signed/instances/{hddt.FileDaKy}";
                string pdfFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, pdfFileFolder);

                var banMauEmail = _mp.Map<ConfigNoiDungEmailViewModel>(await _db.ConfigNoiDungEmails.Where(x => x.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon).FirstOrDefaultAsync());

                var salerVM = await _HoSoHDDTService.GetDetailAsync();

                string messageTitle = banMauEmail.TieuDeEmail;
                messageTitle = messageTitle.Replace("##tendonvi##", salerVM.TenDonVi);
                messageTitle = messageTitle.Replace("##loaihoadon##", hddt.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");

                string messageBody = banMauEmail.NoiDungEmail;
                messageBody = messageBody.Replace("##tendonvi##", salerVM.TenDonVi);
                messageBody = messageBody.Replace("##loaihoadon##", hddt.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");
                messageBody = messageBody.Replace("##tennguoinhan##", TenNguoiNhan ?? (hddt.HoTenNguoiNhanHD ?? string.Empty));
                messageBody = messageBody.Replace("##so##", hddt.SoHoaDon ?? "<Chưa cấp số>");
                messageBody = messageBody.Replace("##mauso##", hddt.MauSo);
                messageBody = messageBody.Replace("##kyhieu##", hddt.KyHieu);
                messageBody = messageBody.Replace("##matracuu##", hddt.MaTraCuu);

                var _objHDDT = await this.GetByIdAsync(hddt.HoaDonDienTuId);
                if (await this.SendEmailAsync(ToMail ?? hddt.EmailNguoiNhanHD, messageTitle, messageBody, pdfFilePath))
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;
                }
                else
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.GuiLoi;
                }
                var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == hddt.HoaDonDienTuId);
                _db.Entry<HoaDonDienTu>(entity).CurrentValues.SetValues(_objHDDT);
                return await _db.SaveChangesAsync() > 0;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> SendEmailAsync(string toMail, string subject, string message, string fileUrl = null, string cc = "", string bcc = "")
        {
            var _configuration = await _TuyChonService.GetAllAsync();

            string fromMail = _configuration.Where(x => x.Ma == "TenDangNhapEmail").Select(x => x.GiaTri).FirstOrDefault();
            string fromName = _configuration.Where(x => x.Ma == "TenNguoiGui").Select(x => x.GiaTri).FirstOrDefault(); ;
            string CC = cc;
            string BCC = bcc;
            string password = _configuration.Where(x => x.Ma == "MatKhauEmail").Select(x => x.GiaTri).FirstOrDefault();
            string host = _configuration.Where(x => x.Ma == "TenMayChu").Select(x => x.GiaTri).FirstOrDefault();
            int port = int.Parse(_configuration.Where(x => x.Ma == "SoCong").Select(x => x.GiaTri).FirstOrDefault());
            bool enableSSL = true;

            MimeMessage mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(fromName ?? string.Empty, fromMail));

            // send multi users
            List<string> toMails = toMail.Split(',', ';').Distinct().ToList();
            List<string> ccs = CC.Split(',', ';').Distinct().ToList();
            List<string> bccs = BCC.Split(',', ';').Distinct().ToList();

            InternetAddressList list = new InternetAddressList();
            foreach (string to in toMails)
            {
                list.Add(new MailboxAddress(to));
            }
            mimeMessage.To.AddRange(list);

            InternetAddressList listCC = new InternetAddressList();
            foreach (string _cc in ccs)
            {
                listCC.Add(new MailboxAddress(_cc));
            }
            mimeMessage.Cc.AddRange(listCC);

            InternetAddressList listBCC = new InternetAddressList();
            foreach (string _bcc in bccs)
            {
                listBCC.Add(new MailboxAddress(_bcc));
            }
            mimeMessage.Bcc.AddRange(listBCC);

            mimeMessage.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            if (!string.IsNullOrEmpty(fileUrl))
            {
                bodyBuilder.Attachments.Add(fileUrl);
            }
            bodyBuilder.HtmlBody = message;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(host, port, enableSSL);
                    await client.AuthenticateAsync(fromMail, password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                catch (System.Net.Mail.SmtpFailedRecipientsException)
                {
                    return false;
                }

                return true;
            }
        }

        public async Task<bool> SendEmailAsync(ParamsSendMail @params)
        {
            try
            {
                string pdfFileFolder = string.Empty;
                if (!string.IsNullOrEmpty(@params.HoaDon.SoHoaDon))
                    pdfFileFolder = $"Assets/uploaded/pdf/signed/instances/{@params.HoaDon.FileDaKy}";
                else
                    pdfFileFolder = $"Assets/uploaded/pdf/signed/instances/{@params.HoaDon.FileChuaKy}";

                string pdfFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, pdfFileFolder);

                var banMauEmail = _mp.Map<ConfigNoiDungEmailViewModel>(await _db.ConfigNoiDungEmails.Where(x => x.LoaiEmail == @params.LoaiEmail).FirstOrDefaultAsync());

                var salerVM = await _HoSoHDDTService.GetDetailAsync();

                string messageTitle = banMauEmail.TieuDeEmail;
                messageTitle = messageTitle.Replace("##tendonvi##", salerVM.TenDonVi);
                messageTitle = messageTitle.Replace("##loaihoadon##", @params.HoaDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");

                string messageBody = banMauEmail.NoiDungEmail;
                string TenNguoiNhan = !string.IsNullOrEmpty(@params.TenNguoiNhan) ? @params.TenNguoiNhan : (@params.HoaDon.HoTenNguoiNhanHD ?? string.Empty);
                messageBody = messageBody.Replace("##tendonvi##", salerVM.TenDonVi);
                messageBody = messageBody.Replace("##loaihoadon##", @params.HoaDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");
                messageBody = messageBody.Replace("##tennguoinhan##", TenNguoiNhan);
                messageBody = messageBody.Replace("##so##", @params.HoaDon.SoHoaDon ?? "<Chưa cấp số>");
                messageBody = messageBody.Replace("##mauso##", @params.HoaDon.MauSo);
                messageBody = messageBody.Replace("##kyhieu##", @params.HoaDon.KyHieu);
                messageBody = messageBody.Replace("##matracuu##", @params.HoaDon.MaTraCuu);

                if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                {
                    messageBody = messageBody.Replace("##lydohuy##", @params.HoaDon.LyDoXoaBo);
                }
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                {
                    messageBody = messageBody.Replace("##lydoxoahoadon##", @params.HoaDon.LyDoXoaBo);
                }

                var _objHDDT = await this.GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                if (await this.SendEmailAsync(@params.ToMail, messageTitle, messageBody, pdfFilePath, @params.CC, @params.BCC))
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;
                    var modelNK = new NhatKyThaoTacHoaDonViewModel
                    {
                        HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                        NgayGio = DateTime.Now,
                        KhachHangId = _objHDDT.KhachHangId,
                        LoaiThaoTac = (int)LoaiThaoTac.GuiHoaDon,
                        MoTa = "Đã gửi hóa đơn " + _objHDDT.SoHoaDon ?? string.Empty + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                        HasError = false,
                        ErrorMessage = "",
                        DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                    };

                    await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == @params.HoaDon.HoaDonDienTuId);
                    _db.Entry<HoaDonDienTu>(entity).CurrentValues.SetValues(_objHDDT);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.GuiLoi;

                    var modelNK = new NhatKyThaoTacHoaDonViewModel
                    {
                        HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                        NgayGio = DateTime.Now,
                        KhachHangId = _objHDDT.KhachHangId,
                        LoaiThaoTac = (int)LoaiThaoTac.GuiHoaDon,
                        MoTa = "Đã gửi lỗi hóa đơn " + _objHDDT.SoHoaDon ?? string.Empty + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                        HasError = true,
                        ErrorMessage = "Lỗi khi gửi hóa đơn",
                        DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                    };

                    await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == @params.HoaDon.HoaDonDienTuId);
                    _db.Entry<HoaDonDienTu>(entity).CurrentValues.SetValues(_objHDDT);
                    await _db.SaveChangesAsync();
                    return false;
                }

            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<List<NhatKyThaoTacHoaDonViewModel>> XemLichSuHoaDon(string HoaDonDienTuId)
        {
            return await _db.NhatKyThaoTacHoaDons.Where(x => x.HoaDonDienTuId == HoaDonDienTuId)
                                                 .Select(x => new NhatKyThaoTacHoaDonViewModel
                                                 {
                                                     HoaDonDienTuId = x.HoaDonDienTuId,
                                                     KhachHangId = x.KhachHangId,
                                                     KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(y => y.DoiTuongId == x.KhachHangId)),
                                                     NgayGio = x.NgayGio,
                                                     LoaiThaoTac = x.LoaiThaoTac,
                                                     HanhDong = ((LoaiThaoTac)x.LoaiThaoTac).GetDescription(),
                                                     DiaChiIp = x.DiaChiIp,
                                                     MoTa = x.MoTa
                                                 })
                                                 .ToListAsync();
        }

        public async Task<BienBanXoaBoViewModel> GetBienBanXoaBoHoaDon(string HoaDonDienTuId)
        {
            try
            {
                var result = _mp.Map<BienBanXoaBoViewModel>(await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.HoaDonDienTuId == HoaDonDienTuId));
                return result;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return null;
        }

        public async Task<bool> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon @params)
        {
            try
            {
                var entity = _mp.Map<BienBanXoaBo>(@params.Data);
                await _db.BienBanXoaBos.AddAsync(entity);

                var entityHD = _db.HoaDonDienTus.FirstOrDefault(x => x.HoaDonDienTuId == @params.Data.HoaDonDienTuId);
                entityHD.LyDoXoaBo = entity.LyDoXoaBo;

                if (await _db.SaveChangesAsync() > 0)
                {
                    if (@params.OptionalSendData == 1)
                    {
                        return true;
                    }
                    else
                    {
                        var _objHD = await this.GetByIdAsync(@params.Data.HoaDonDienTuId);
                        var _params = new ParamsSendMail
                        {
                            HoaDon = _objHD,
                            LoaiEmail = (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon
                        };

                        if (await this.SendEmailAsync(_params))
                        {
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<bool> DeleteBienBanXoaHoaDon(string Id)
        {
            try
            {
                BienBanXoaBo entity = _db.BienBanXoaBos.FirstOrDefault(x => x.Id == Id);
                _db.BienBanXoaBos.Remove(entity);
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<string> ConvertBienBanXoaHoaDon(BienBanXoaBoViewModel bb)
        {
            var path = string.Empty;
            try
            {
                if (bb != null)
                {
                    var _objHD = await GetByIdAsync(bb.HoaDonDienTuId);
                    Document doc = new Document();
                    string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/HoaDonXoaBo/Bien_ban_huy_hoa_don.docx");
                    doc.LoadFromFile(docFolder);

                    doc.Replace("<CompanyName>", bb.TenCongTyBenA ?? string.Empty, true, true);
                    doc.Replace("<Address>", bb.DiaChiBenA ?? string.Empty, true, true);
                    doc.Replace("<Taxcode>", bb.MaSoThueBenA ?? string.Empty, true, true);
                    doc.Replace("<Tel>", bb.SoDienThoaiBenA ?? string.Empty, true, true);
                    doc.Replace("<Representative>", bb.DaiDienBenA ?? string.Empty, true, true);
                    doc.Replace("<Position>", bb.ChucVuBenA ?? string.Empty, true, true);


                    doc.Replace("<CustomerCompany>", bb.TenKhachHang ?? string.Empty, true, true);
                    doc.Replace("<CustomerAddress>", bb.DiaChi ?? string.Empty, true, true);
                    doc.Replace("<CustomerTaxcode>", bb.MaSoThue ?? string.Empty, true, true);
                    doc.Replace("<CustomerTel>", bb.SoDienThoai ?? string.Empty, true, true);
                    doc.Replace("<CustomerRepresentative>", bb.DaiDien ?? string.Empty, true, true);
                    doc.Replace("<CustomerPosition>", bb.ChucVu ?? string.Empty, true, true);

                    var description = "Hai bên thống nhất lập biên bản này để thu hồi và xóa bỏ hóa đơn có mẫu số " + _objHD.MauSo + " ký hiệu " + _objHD.KyHieu + " số " + _objHD.SoHoaDon + " ngày " + _objHD.NgayHoaDon.Value.ToString("dd/MM/yyyy") + " mã tra cứu " + _objHD.MaTraCuu + " theo quy định";
                    doc.Replace("<Description>", description, true, true);


                    doc.Replace("<dd>", bb.NgayBienBan.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                    doc.Replace("<mm>", bb.NgayBienBan.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                    doc.Replace("<yyyy>", bb.NgayBienBan.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);


                    doc.Replace("<reason>", _objHD.LyDoXoaBo ?? string.Empty, true, true);

                    var pdfFolder = string.Empty;

                    if (bb.SoBienBan != "")
                        pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf/unsigned");
                    else
                    {
                        pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf/signed");
                    }

                    var pdfFileName = "Bien_ban_huy_hoa_don" + (_objHD.LoaiHoaDon == 1 ? "_Hoa_Don_GTGT_" : "_Hoa_don_ban_hang_") + _objHD.SoHoaDon
                       + _objHD.NgayHoaDon.Value.ToString("yyyyMMddhhmmss");
                    if (!Directory.Exists(pdfFolder))
                    {
                        Directory.CreateDirectory(pdfFolder);
                    }


                    doc.SaveToFile(pdfFolder + pdfFileName, FileFormat.PDF);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return path;
        }

        public async Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonThayTheAsync(HoaDonThayTheParams @params)
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
                    MauHoaDon = _mp.Map<MauHoaDonViewModel>(_db.MauHoaDons.FirstOrDefault(x => x.MauHoaDonId == hd.MauHoaDonId)),
                    MauSo = hd.MauSo,
                    KyHieu = hd.KyHieu,
                    KhachHangId = hd.KhachHangId ?? string.Empty,
                    KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.KhachHangId)),
                    MaKhachHang = hd.MaKhachHang,
                    TenKhachHang = hd.TenKhachHang,
                    SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                    EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                    HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                    DiaChi = hd.DiaChi,
                    MaSoThue = hd.MaSoThue,
                    TenNganHang = hd.TenNganHang,
                    SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                    NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                    NhanVienBanHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.NhanVienBanHangId)),
                    TenNhanVienBanHang = hd.TenNhanVienBanHang,
                    LoaiTienId = hd.LoaiTienId ?? string.Empty,
                    LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
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
                    TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien ?? 0 - x.TienChietKhau ?? 0 + x.TienThueGTGT ?? 0)
                });

            if (!string.IsNullOrEmpty(@params.Keyword))
            {
                string keyword = @params.Keyword.ToUpper().ToTrim();

                query = query.Where(x => x.NgayHoaDon.Value.ToString("dd/MM/yyyy").Contains(keyword) ||
                                        x.NgayLap.Value.ToString("dd/MM/yyyy").ToString().Contains(keyword) ||
                                        x.SoHoaDon.ToString().Contains(keyword) ||
                                        (x.KhachHang.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.KhachHang.Ten ?? string.Empty).ToUpper().Contains(keyword) ||
                                        x.KhachHang.Ten.Contains(keyword) ||
                                        (x.NguoiLap.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.NguoiLap.Ten ?? string.Empty).ToUpper().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                DateTime fromDate = DateTime.Parse(@params.FromDate);
                DateTime toDate = DateTime.Parse(@params.ToDate);
                query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                        DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
            }

            if (@params.TimKiemTheo != null)
            {
                if (!string.IsNullOrEmpty(@params.TimKiemTheo.MauSo))
                {
                    var keyword = @params.TimKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.TimKiemTheo.KyHieu))
                {
                    //var keyword = @params.TimKiemTheo.KyHieu.ToUpper().ToTrim();
                    //query = query.Where(x => x..ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            return await PagedList<HoaDonDienTuViewModel>
                    .CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public List<EnumModel> GetLoaiTrangThaiPhatHanhs()
        {
            List<EnumModel> enums = ((LoaiTrangThaiPhatHanh[])Enum.GetValues(typeof(LoaiTrangThaiPhatHanh)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetLoaiTrangThaiGuiHoaDons()
        {
            List<EnumModel> enums = ((LoaiTrangThaiGuiHoaDon[])Enum.GetValues(typeof(LoaiTrangThaiGuiHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListTimKiemTheoHoaDonThayThe()
        {
            HoaDonThayTheSearch search = new HoaDonThayTheSearch();
            var result = search.GetType().GetProperties()
                .Select(x => new EnumModel
                {
                    Value = x.Name,
                    Name = (x.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute).Name
                })
                .ToList();

            return result;
        }

        public List<EnumModel> GetListHinhThucHoaDonCanThayThe()
        {
            List<EnumModel> enums = ((HinhThucHoaDonCanThayThe[])Enum.GetValues(typeof(HinhThucHoaDonCanThayThe)))
               .Select(c => new EnumModel()
               {
                   Value = (int)c,
                   Name = c.GetDescription()
               }).ToList();
            return enums;
        }

        public async Task<bool> XoaBoHoaDon(ParamXoaBoHoaDon @params)
        {
            try
            {
                var _objHDDT = _mp.Map<HoaDonDienTuViewModel>(await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == @params.HoaDon.HoaDonDienTuId));
                if (_objHDDT != null)
                {
                    _objHDDT.SoCTXoaBo = @params.HoaDon.SoCTXoaBo;
                    _objHDDT.NgayXoaBo = @params.HoaDon.NgayXoaBo;
                    _objHDDT.LyDoXoaBo = @params.HoaDon.LyDoXoaBo;
                    _objHDDT.TrangThai = (int)TrangThaiHoaDon.HoaDonXoaBo;

                    if (await this.UpdateAsync(_objHDDT))
                    {
                        if (@params.OptionalSend == 1)
                        {
                            return true;
                        }
                        else
                        {
                            var _objHD = await this.GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                            var _params = new ParamsSendMail
                            {
                                HoaDon = _objHD,
                                LoaiEmail = (int)LoaiEmail.ThongBaoXoaBoHoaDon
                            };

                            if (await this.SendEmailAsync(_params))
                            {
                                return true;
                            }

                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonDieuChinhAsync(HoaDonDieuChinhParams @params)
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
                    MauHoaDon = _mp.Map<MauHoaDonViewModel>(_db.MauHoaDons.FirstOrDefault(x => x.MauHoaDonId == hd.MauHoaDonId)),
                    MauSo = hd.MauSo,
                    KyHieu = hd.KyHieu,
                    KhachHangId = hd.KhachHangId ?? string.Empty,
                    KhachHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.KhachHangId)),
                    MaKhachHang = hd.MaKhachHang,
                    TenKhachHang = hd.TenKhachHang,
                    SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                    EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                    HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                    DiaChi = hd.DiaChi,
                    MaSoThue = hd.MaSoThue,
                    TenNganHang = hd.TenNganHang,
                    SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                    NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                    NhanVienBanHang = _mp.Map<DoiTuongViewModel>(_db.DoiTuongs.FirstOrDefault(x => x.DoiTuongId == hd.NhanVienBanHangId)),
                    TenNhanVienBanHang = hd.TenNhanVienBanHang,
                    LoaiTienId = hd.LoaiTienId ?? string.Empty,
                    LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
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
                    TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien ?? 0 - x.TienChietKhau ?? 0 + x.TienThueGTGT ?? 0)
                });

            if (!string.IsNullOrEmpty(@params.Keyword))
            {
                string keyword = @params.Keyword.ToUpper().ToTrim();

                query = query.Where(x => x.NgayHoaDon.Value.ToString("dd/MM/yyyy").Contains(keyword) ||
                                        x.NgayLap.Value.ToString("dd/MM/yyyy").ToString().Contains(keyword) ||
                                        x.SoHoaDon.ToString().Contains(keyword) ||
                                        (x.KhachHang.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.KhachHang.Ten ?? string.Empty).ToUpper().Contains(keyword) ||
                                        x.KhachHang.Ten.Contains(keyword) ||
                                        (x.NguoiLap.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.NguoiLap.Ten ?? string.Empty).ToUpper().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                DateTime fromDate = DateTime.Parse(@params.FromDate);
                DateTime toDate = DateTime.Parse(@params.ToDate);
                query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                        DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
            }

            if (@params.TimKiemTheo != null)
            {
                if (!string.IsNullOrEmpty(@params.TimKiemTheo.MauSo))
                {
                    var keyword = @params.TimKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.TimKiemTheo.KyHieu))
                {
                    //var keyword = @params.TimKiemTheo.KyHieu.ToUpper().ToTrim();
                    //query = query.Where(x => x..ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            return await PagedList<HoaDonDienTuViewModel>
                    .CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public List<EnumModel> GetLoaiTrangThaiBienBanDieuChinhHoaDons()
        {
            List<EnumModel> enums = ((LoaiTrangThaiBienBanDieuChinhHoaDon[])Enum.GetValues(typeof(LoaiTrangThaiBienBanDieuChinhHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<TrangThaiHoaDonDieuChinh> GetTrangThaiHoaDonDieuChinhs()
        {
            return new List<TrangThaiHoaDonDieuChinh>
            {
                new TrangThaiHoaDonDieuChinh { Key = -1, Name = "Tất cả", ParentId = null, Level = 0 },
                new TrangThaiHoaDonDieuChinh { Key = 0, Name = "Hóa đơn chưa lập điều chỉnh", ParentId = null, Level = 0 },
                new TrangThaiHoaDonDieuChinh { Key = -2, Name = "Hóa đơn đã lập điều chỉnh", ParentId = null, Level = 0 },
                new TrangThaiHoaDonDieuChinh { Key = 1, Name = "Hóa đơn điều chỉnh tăng", ParentId = -2, Level = 1 },
                new TrangThaiHoaDonDieuChinh { Key = 2, Name = "Hóa đơn điều chỉnh giảm", ParentId = -2, Level = 1 },
                new TrangThaiHoaDonDieuChinh { Key = 3, Name = "Hóa đơn điều chỉnh thông tin", ParentId = -2, Level = 1 },
            };
        }
    }
}
