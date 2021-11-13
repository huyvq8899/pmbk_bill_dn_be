using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Constants;
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
using Services.Helper.Params.Filter;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLy;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.TienIch;
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
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHoSoHDDTService _HoSoHDDTService;
        private readonly IHoaDonDienTuChiTietService _HoaDonDienTuChiTietService;
        private readonly IMauHoaDonService _MauHoaDonService;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IXMLInvoiceService _xMLInvoiceService;
        private readonly INhatKyGuiEmailService _nhatKyGuiEmailService;
        private readonly ITuyChonService _TuyChonService;
        private readonly IBoKyHieuHoaDonService _boKyHieuHoaDonService;

        public HoaDonDienTuService(
            Datacontext datacontext,
            IMapper mapper,
            IMauHoaDonService MauHoaDonService,
            IHoSoHDDTService HoSoHDDTService,
            IHoaDonDienTuChiTietService HoaDonDienTuChiTietService,
            ITuyChonService TuyChonService,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment IHostingEnvironment,
            INhatKyGuiEmailService nhatKyGuiEmailService,
            IXMLInvoiceService xMLInvoiceService
        )
        {
            _db = datacontext;
            _mp = mapper;
            _HoSoHDDTService = HoSoHDDTService;
            _MauHoaDonService = MauHoaDonService;
            _HoaDonDienTuChiTietService = HoaDonDienTuChiTietService;
            _TuyChonService = TuyChonService;
            _IHttpContextAccessor = IHttpContextAccessor;
            _xMLInvoiceService = xMLInvoiceService;
            _nhatKyGuiEmailService = nhatKyGuiEmailService;
            _hostingEnvironment = IHostingEnvironment;
        }

        private readonly List<TrangThai> TrangThaiHoaDons = new List<TrangThai>()
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

        private readonly List<TrangThai> TrangThaiGuiHoaDons = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = 0, Ten = "Chưa gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 1, Ten = "Đang gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Gửi hóa đơn cho khách hàng lỗi", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Đã gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Khách hàng đã xem hóa đơn", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Khách hàng chưa xem hóa đơn", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
        };

        private readonly List<TrangThai> TreeTrangThais = new List<TrangThai>()
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
            return await _db.HoaDonDienTus
                .AnyAsync(x => x.SoHoaDon == SoHoaDon);
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

            var nhatKyThaoTacHoaDons = await _db.NhatKyThaoTacHoaDons.Where(x => x.HoaDonDienTuId == id).ToListAsync();
            _db.NhatKyThaoTacHoaDons.RemoveRange(nhatKyThaoTacHoaDons);

            if (await _db.SaveChangesAsync() == hoaDonChiTiets.Count + nhatKyThaoTacHoaDons.Count)
            {
                UploadFile uploadFile = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
                await uploadFile.DeleteFileRefTypeById(id, RefType.HoaDonDienTu, _db);

                var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
                _db.HoaDonDienTus.Remove(entity);
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public ThamChieuModel DeleteRangeHoaDonDienTuAsync(List<HoaDonDienTuViewModel> list)
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
            IQueryable<HoaDonDienTuViewModel> query = from hd in _db.HoaDonDienTus
                                                      join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                                                      from mhd in tmpMauHoaDons.DefaultIfEmpty()
                                                      join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                                                      from kh in tmpKhachHangs.DefaultIfEmpty()
                                                      join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                                                      from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                                                      join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                                                      from nv in tmpNhanViens.DefaultIfEmpty()
                                                      join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                                                      from nl in tmpNguoiLaps.DefaultIfEmpty()
                                                      join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                                      from lt in tmpLoaiTiens.DefaultIfEmpty()
                                                      orderby hd.MauSo descending, hd.KyHieu, hd.NgayHoaDon.Value.Date descending, hd.NgayLap.Value.Date descending, hd.SoHoaDon ascending
                                                      select new HoaDonDienTuViewModel
                                                      {
                                                          HoaDonDienTuId = hd.HoaDonDienTuId,
                                                          NgayHoaDon = hd.NgayHoaDon,
                                                          NgayLap = hd.NgayLap,
                                                          NguoiLap = nl != null ? new DoiTuongViewModel
                                                          {
                                                              Ma = nl.Ma,
                                                              Ten = nl.Ten
                                                          }
                                                                                : null,
                                                          SoHoaDon = hd.SoHoaDon ?? "<Chưa cấp số>",
                                                          MaCuaCQT = hd.MaCuaCQT ?? string.Empty,
                                                          MauHoaDonId = mhd.MauHoaDonId ?? string.Empty,
                                                          MauSo = hd.MauSo ?? mhd.MauSo,
                                                          KyHieu = hd.KyHieu ?? mhd.KyHieu,
                                                          KhachHangId = kh.DoiTuongId,
                                                          KhachHang = kh != null ?
                                                                      new DoiTuongViewModel
                                                                      {
                                                                          Ma = kh.Ma,
                                                                          Ten = kh.Ten,
                                                                          MaSoThue = kh.MaSoThue,
                                                                          HoTenNguoiMuaHang = kh.HoTenNguoiMuaHang,
                                                                          SoDienThoaiNguoiMuaHang = kh.SoDienThoaiNguoiMuaHang,
                                                                          EmailNguoiMuaHang = kh.EmailNguoiMuaHang,
                                                                          HoTenNguoiNhanHD = kh.HoTenNguoiNhanHD,
                                                                          SoDienThoaiNguoiNhanHD = kh.SoDienThoaiNguoiNhanHD,
                                                                          EmailNguoiNhanHD = kh.EmailNguoiNhanHD,
                                                                          SoTaiKhoanNganHang = kh.SoTaiKhoanNganHang
                                                                      }
                                                                      : null,
                                                          MaKhachHang = hd.MaKhachHang ?? string.Empty,
                                                          TenKhachHang = hd.TenKhachHang ?? string.Empty,
                                                          MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                                                          DiaChi = hd.DiaChi,
                                                          HinhThucThanhToanId = hd.HinhThucThanhToanId,
                                                          HinhThucThanhToan = httt != null ?
                                                                                    new HinhThucThanhToanViewModel
                                                                                    {
                                                                                        Ten = httt.Ten
                                                                                    }
                                                                                    : null,
                                                          HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang ?? string.Empty,
                                                          SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                                          EmailNguoiMuaHang = hd.EmailNguoiMuaHang ?? string.Empty,
                                                          TenNganHang = hd.TenNganHang ?? string.Empty,
                                                          SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang ?? string.Empty,
                                                          HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD ?? string.Empty,
                                                          EmailNguoiNhanHD = hd.EmailNguoiNhanHD ?? string.Empty,
                                                          SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD ?? string.Empty,
                                                          LoaiTienId = lt.LoaiTienId ?? string.Empty,
                                                          LoaiTien = lt != null ? new LoaiTienViewModel
                                                          {
                                                              Ma = lt.Ma,
                                                              Ten = lt.Ten
                                                          }
                                                          : null,
                                                          TyGia = hd.TyGia ?? 1,
                                                          TrangThai = hd.TrangThai,
                                                          TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                                                          MaTraCuu = hd.MaTraCuu,
                                                          TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                                          KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                                                          SoLanChuyenDoi = hd.SoLanChuyenDoi,
                                                          LyDoXoaBo = hd.LyDoXoaBo,
                                                          FileChuaKy = hd.FileChuaKy,
                                                          FileDaKy = hd.FileDaKy,
                                                          LoaiHoaDon = hd.LoaiHoaDon,
                                                          TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                                          LoaiChungTu = hd.LoaiChungTu,
                                                          ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                                                          LyDoThayThe = hd.LyDoThayThe,
                                                          DieuChinhChoHoaDonId = hd.DieuChinhChoHoaDonId,
                                                          LyDoDieuChinh = hd.LyDoDieuChinh,
                                                          LoaiDieuChinh = hd.LoaiDieuChinh,
                                                          NhanVienBanHangId = hd.NhanVienBanHangId,
                                                          NhanVienBanHang = nv != null ? new DoiTuongViewModel
                                                          {
                                                              Ma = nv.Ma,
                                                              Ten = nv.Ten
                                                          }
                                                          : null,
                                                          HoaDonChiTiets = (
                                                                             from hdct in _db.HoaDonDienTuChiTiets
                                                                             join hd in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                                                             from hd in tmpHoaDons.DefaultIfEmpty()
                                                                             join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                                                             from vt in tmpHangHoas.DefaultIfEmpty()
                                                                             join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                                                             from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                                                             where hdct.HoaDonDienTuId == hd.HoaDonDienTuId
                                                                             orderby vt.Ma descending
                                                                             select new HoaDonDienTuChiTietViewModel
                                                                             {
                                                                                 HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                                                 HoaDonDienTuId = hd.HoaDonDienTuId,
                                                                                 HangHoaDichVuId = vt.HangHoaDichVuId,
                                                                                 MaHang = !string.IsNullOrEmpty(hdct.MaHang) ? hdct.MaHang : vt.Ma,
                                                                                 TenHang = !string.IsNullOrEmpty(hdct.TenHang) ? hdct.TenHang : vt.Ten,
                                                                                 HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                                                                 DonViTinhId = dvt.DonViTinhId,
                                                                                 DonViTinh = dvt != null ? new DonViTinhViewModel
                                                                                 {
                                                                                     Ten = dvt.Ten
                                                                                 }
                                                                                 : null,
                                                                                 SoLuong = hdct.SoLuong,
                                                                                 DonGia = hdct.DonGia,
                                                                                 DonGiaSauThue = hdct.DonGiaSauThue,
                                                                                 DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                                                                 ThanhTien = hdct.ThanhTien,
                                                                                 ThanhTienSauThue = hdct.ThanhTienSauThue,
                                                                                 ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                                                                 ThanhTienSauThueQuyDoi = hdct.ThanhTienSauThueQuyDoi,
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
                                                          CreatedBy = hd.CreatedBy,
                                                          CreatedDate = hd.CreatedDate,
                                                          Status = hd.Status,
                                                          NgayXoaBo = hd.NgayXoaBo,
                                                          TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                                          TongTienThanhToan = hd.TongTienThanhToan,
                                                          TongTienThanhToanQuyDoi = hd.TongTienThanhToanQuyDoi,
                                                          DaLapHoaDonThayThe = _db.HoaDonDienTus.Any(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId),
                                                          TruongThongTinBoSung1 = hd.TruongThongTinBoSung1,
                                                          TruongThongTinBoSung2 = hd.TruongThongTinBoSung2,
                                                          TruongThongTinBoSung3 = hd.TruongThongTinBoSung3,
                                                          TruongThongTinBoSung4 = hd.TruongThongTinBoSung4,
                                                          TruongThongTinBoSung5 = hd.TruongThongTinBoSung5,
                                                          TruongThongTinBoSung6 = hd.TruongThongTinBoSung6,
                                                          TruongThongTinBoSung7 = hd.TruongThongTinBoSung7,
                                                          TruongThongTinBoSung8 = hd.TruongThongTinBoSung8,
                                                          TruongThongTinBoSung9 = hd.TruongThongTinBoSung9,
                                                          TruongThongTinBoSung10 = hd.TruongThongTinBoSung10,
                                                      };


            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                string keyword = pagingParams.Keyword.ToUpper().ToTrim();

                //query = query.Where(x => x.NgayHoaDon.Value.ToString("dd/MM/yyyy").Contains(keyword) ||
                //                          x.NgayLap.Value.ToString("dd/MM/yyyy").ToString().Contains(keyword) ||
                //                          x.SoHoaDon.ToString().Contains(keyword) ||
                //                          (x.KhachHang.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                //                          (x.KhachHang.Ten ?? string.Empty).ToUpper().Contains(keyword) ||
                //                          x.KhachHang.Ten.Contains(keyword) ||
                //                          (x.NguoiLap.Ten ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                //                          (x.NguoiLap.Ten ?? string.Empty).ToUpper().Contains(keyword));

                query = query.Where(x => x.SoHoaDon.ToUpper().Contains(keyword) || x.SoHoaDon.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.MaKhachHang.ToUpper().Contains(keyword) || x.MaKhachHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.TenKhachHang.ToUpper().Contains(keyword) || x.TenKhachHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.HoTenNguoiMuaHang.ToUpper().Contains(keyword) || x.HoTenNguoiMuaHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.MaSoThue.ToUpper().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
            {
                DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
                DateTime toDate = DateTime.Parse(pagingParams.ToDate);
                query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                        DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
            }

            if (!string.IsNullOrEmpty(pagingParams.KhachHangId))
            {
                query = query.Where(x => x.KhachHangId == pagingParams.KhachHangId);
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
                query = query.Where(x => x.TrangThaiPhatHanh == pagingParams.TrangThaiPhatHanh);
            }

            if (pagingParams.TrangThaiGuiHoaDon.HasValue && pagingParams.TrangThaiGuiHoaDon != -1)
            {
                query = query.Where(x => x.TrangThaiGuiHoaDon == pagingParams.TrangThaiGuiHoaDon);
            }

            if (pagingParams.TrangThaiChuyenDoi.HasValue && pagingParams.TrangThaiChuyenDoi != -1)
            {
                query = query.Where(x => pagingParams.TrangThaiChuyenDoi == 0 ? x.SoLanChuyenDoi == 0 : x.SoLanChuyenDoi != 0);
            }

            if (pagingParams.TrangThaiBienBanXoaBo.HasValue && pagingParams.TrangThaiBienBanXoaBo != -1)
            {
                query = query.Where(x => x.TrangThaiBienBanXoaBo == pagingParams.TrangThaiBienBanXoaBo);
            }

            if (pagingParams.TrangThaiXoaBo.HasValue && pagingParams.TrangThaiXoaBo != -1)
            {
                if (pagingParams.TrangThaiXoaBo == 0)
                {
                    query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo);
                }
                else if (pagingParams.TrangThaiXoaBo == 1)
                {
                    query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && _db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId));
                }
                else if (pagingParams.TrangThaiXoaBo == 2)
                {
                    query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && !_db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId));
                }
                else
                {
                    query = query.Where(x => x.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo);
                }
            }


            if (pagingParams.TimKiemTheo != null)
            {
                var timKiemTheo = pagingParams.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
            }

            #region Filter and Sort
            if (pagingParams.FilterColumns != null && pagingParams.FilterColumns.Any())
            {
                pagingParams.FilterColumns = pagingParams.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in pagingParams.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        case nameof(pagingParams.Filter.SoHoaDon):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.SoHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.MauSo):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MauSo, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.KyHieu):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.KyHieu, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.MaKhachHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MaKhachHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.TenKhachHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TenKhachHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.DiaChi):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.DiaChi, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.MaSoThue):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MaSoThue, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.HoTenNguoiMuaHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.HoTenNguoiMuaHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.TenNhanVienBanHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TenNhanVienBanHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.TongTienThanhToan):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TongTienThanhToan, filterCol, FilterValueType.Decimal);
                            break;
                        default:
                            break;
                    }
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

                if (pagingParams.SortKey == "NgayXoaBo" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayXoaBo);
                }
                if (pagingParams.SortKey == "NgayXoaBo" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayXoaBo);
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

                if (pagingParams.SortKey == "MauSoHoaDon" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MauSo);
                }
                if (pagingParams.SortKey == "MauSoHoaDon" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MauSo);
                }

                if (pagingParams.SortKey == "KyHieuHoaDon" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.KyHieu);
                }
                if (pagingParams.SortKey == "KyHieuHoaDon" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.KyHieu);
                }

                if (pagingParams.SortKey == "MauSoHoaDon" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MauSo);
                }
                if (pagingParams.SortKey == "MauSoHoaDon" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MauSo);
                }

                if (pagingParams.SortKey == "TenKhachHang" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenKhachHang);
                }
                if (pagingParams.SortKey == "TenKhachHang" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenKhachHang);
                }

                if (pagingParams.SortKey == "MaSoThue" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MaSoThue);
                }
                if (pagingParams.SortKey == "MaSoThue" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MaSoThue);
                }

                if (pagingParams.SortKey == "HoTenNguoiMuaHang" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.HoTenNguoiMuaHang);
                }
                if (pagingParams.SortKey == "HoTenNguoiMuaHang" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.HoTenNguoiMuaHang);
                }

                if (pagingParams.SortKey == "NVBanHang" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenNhanVienBanHang);
                }
                if (pagingParams.SortKey == "NVBanHang" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenNhanVienBanHang);
                }

                if (pagingParams.SortKey == "LoaiTien" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MaLoaiTien);
                }
                if (pagingParams.SortKey == "LoaiTien" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MaLoaiTien);
                }


                if (pagingParams.SortKey == "MaTraCuu" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MaTraCuu);
                }
                if (pagingParams.SortKey == "MaTraCuu" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MaTraCuu);
                }


                if (pagingParams.SortKey == "TenNguoiNhan" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.HoTenNguoiNhanHD);
                }
                if (pagingParams.SortKey == "TenNguoiNhan" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.HoTenNguoiNhanHD);
                }

                if (pagingParams.SortKey == "EmailNguoiNhan" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.EmailNguoiNhanHD);
                }
                if (pagingParams.SortKey == "EmailNguoiNhan" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.EmailNguoiNhanHD);
                }

                if (pagingParams.SortKey == "SoDienThoaiNguoiNhan" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoDienThoaiNguoiNhanHD);
                }
                if (pagingParams.SortKey == "SoDienThoaiNguoiNhan" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoDienThoaiNguoiNhanHD);
                }

                if (pagingParams.SortKey == "SoLanChuyenDoi" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoLanChuyenDoi);
                }
                if (pagingParams.SortKey == "SoLanChuyenDoi" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoLanChuyenDoi);
                }

                if (pagingParams.SortKey == "LyDoXoaBo" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.LyDoXoaBo);
                }
                if (pagingParams.SortKey == "LyDoXoaBo" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.LyDoXoaBo);
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
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string folder = $@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{id}\FileAttach";

            var query = from hd in _db.HoaDonDienTus
                        join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                        from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        where hd.HoaDonDienTuId == id
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauHoaDonId = mhd.MauHoaDonId ?? string.Empty,
                            MauHoaDon = mhd != null ? _mp.Map<MauHoaDonViewModel>(mhd) : null,
                            MauSo = hd.MauSo ?? mhd.MauSo,
                            KyHieu = hd.KyHieu ?? mhd.KyHieu,
                            KhachHangId = kh.DoiTuongId,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            DiaChi = hd.DiaChi,
                            MaNhanVienBanHang = hd.MaNhanVienBanHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            KhachHang = kh != null ?
                                        new DoiTuongViewModel
                                        {
                                            Ma = kh.Ma,
                                            Ten = kh.Ten,
                                            MaSoThue = kh.MaSoThue,
                                            HoTenNguoiMuaHang = kh.HoTenNguoiMuaHang,
                                            SoDienThoaiNguoiMuaHang = kh.SoDienThoaiNguoiMuaHang,
                                            EmailNguoiMuaHang = kh.EmailNguoiMuaHang,
                                            HoTenNguoiNhanHD = kh.HoTenNguoiNhanHD,
                                            SoDienThoaiNguoiNhanHD = kh.SoDienThoaiNguoiNhanHD,
                                            EmailNguoiNhanHD = kh.EmailNguoiNhanHD,
                                            SoTaiKhoanNganHang = kh.SoTaiKhoanNganHang
                                        }
                                        : null,
                            MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                            HinhThucThanhToanId = hd.HinhThucThanhToanId,
                            HinhThucThanhToan = httt != null ?
                                                new HinhThucThanhToanViewModel
                                                {
                                                    Ten = httt.Ten
                                                }
                                                : null,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang ?? string.Empty,
                            SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang ?? string.Empty,
                            EmailNguoiMuaHang = hd.EmailNguoiMuaHang ?? string.Empty,
                            TenNganHang = hd.TenNganHang ?? string.Empty,
                            SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang ?? string.Empty,
                            HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD ?? string.Empty,
                            EmailNguoiNhanHD = hd.EmailNguoiNhanHD ?? string.Empty,
                            SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD ?? string.Empty,
                            LoaiTienId = lt.LoaiTienId ?? string.Empty,
                            LoaiTien = lt != null ? new LoaiTienViewModel
                            {
                                Ma = lt.Ma,
                                Ten = lt.Ten
                            } : null,
                            TyGia = hd.TyGia ?? 1,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            IsVND = lt == null || (lt.Ma == "VND"),
                            TrangThai = hd.TrangThai,
                            TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                            MaTraCuu = hd.MaTraCuu,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            FileChuaKy = hd.FileChuaKy,
                            FileDaKy = hd.FileDaKy,
                            XMLChuaKy = hd.XMLChuaKy,
                            XMLDaKy = hd.XMLDaKy,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                            LoaiChungTu = hd.LoaiChungTu,
                            ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                            LyDoThayThe = hd.LyDoThayThe,
                            DieuChinhChoHoaDonId = hd.DieuChinhChoHoaDonId,
                            LyDoDieuChinh = hd.LyDoDieuChinh,
                            LoaiDieuChinh = hd.LoaiDieuChinh,
                            NhanVienBanHangId = hd.NhanVienBanHangId,
                            NhanVienBanHang = nv != null ? new DoiTuongViewModel
                            {
                                Ma = nv.Ma,
                                Ten = nv.Ten
                            } : null,
                            TruongThongTinBoSung1 = hd.TruongThongTinBoSung1,
                            TruongThongTinBoSung2 = hd.TruongThongTinBoSung2,
                            TruongThongTinBoSung3 = hd.TruongThongTinBoSung3,
                            TruongThongTinBoSung4 = hd.TruongThongTinBoSung4,
                            TruongThongTinBoSung5 = hd.TruongThongTinBoSung5,
                            TruongThongTinBoSung6 = hd.TruongThongTinBoSung6,
                            TruongThongTinBoSung7 = hd.TruongThongTinBoSung7,
                            TruongThongTinBoSung8 = hd.TruongThongTinBoSung8,
                            TruongThongTinBoSung9 = hd.TruongThongTinBoSung9,
                            TruongThongTinBoSung10 = hd.TruongThongTinBoSung10,
                            ThoiHanThanhToan = hd.ThoiHanThanhToan,
                            DiaChiGiaoHang = hd.DiaChiGiaoHang,
                            BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : null,
                            LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
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
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                                   DonViTinhId = dvt.DonViTinhId,
                                                   DonViTinh = dvt != null ? new DonViTinhViewModel
                                                   {
                                                       Ten = dvt.Ten
                                                   } : null,
                                                   SoLuong = hdct.SoLuong,
                                                   DonGia = hdct.DonGia,
                                                   DonGiaSauThue = hdct.DonGiaSauThue,
                                                   DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                                   ThanhTien = hdct.ThanhTien,
                                                   ThanhTienSauThue = hdct.ThanhTienSauThue,
                                                   ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                                   ThanhTienSauThueQuyDoi = hdct.ThanhTienSauThueQuyDoi,
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
                                                   SoMay = hdct.SoMay,
                                                   NhanVienBanHangId = hdct.NhanVienBanHangId,
                                                   MaNhanVien = hdct.MaNhanVien,
                                                   TenNhanVien = hdct.TenNhanVien,
                                                   XuatBanPhi = hdct.XuatBanPhi,
                                                   GhiChu = hdct.GhiChu,
                                                   TruongMoRongChiTiet1 = hdct.TruongMoRongChiTiet1,
                                                   TruongMoRongChiTiet2 = hdct.TruongMoRongChiTiet2,
                                                   TruongMoRongChiTiet3 = hdct.TruongMoRongChiTiet3,
                                                   TruongMoRongChiTiet4 = hdct.TruongMoRongChiTiet4,
                                                   TruongMoRongChiTiet5 = hdct.TruongMoRongChiTiet5,
                                                   TruongMoRongChiTiet6 = hdct.TruongMoRongChiTiet6,
                                                   TruongMoRongChiTiet7 = hdct.TruongMoRongChiTiet7,
                                                   TruongMoRongChiTiet8 = hdct.TruongMoRongChiTiet8,
                                                   TruongMoRongChiTiet9 = hdct.TruongMoRongChiTiet9,
                                                   TruongMoRongChiTiet10 = hdct.TruongMoRongChiTiet10
                                               }).ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == hd.HoaDonDienTuId
                                               orderby tldk.CreatedDate
                                               select new TaiLieuDinhKemViewModel
                                               {
                                                   TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                   NghiepVuId = tldk.NghiepVuId,
                                                   LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                   TenGoc = tldk.TenGoc,
                                                   TenGuid = tldk.TenGuid,
                                                   CreatedDate = tldk.CreatedDate,
                                                   Link = _IHttpContextAccessor.GetDomain() + Path.Combine(folder, tldk.TenGuid),
                                                   Status = tldk.Status
                                               })
                                               .ToList(),
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
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon
                        };

            var result = await query.FirstOrDefaultAsync();
            result.TongTienThanhToan = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToan ?? 0);
            result.TongTienThanhToanQuyDoi = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToanQuyDoi ?? 0);
            return result;
        }

        public async Task<HoaDonDienTuViewModel> InsertAsync(HoaDonDienTuViewModel model)
        {
            model.HoaDonDienTuId = Guid.NewGuid().ToString();
            model.HoaDonChiTiets = null;

            HoaDonDienTu entity = _mp.Map<HoaDonDienTu>(model);

            entity.TrangThai = (int)TrangThaiHoaDon.HoaDonGoc;
            entity.TrangThaiPhatHanh = (int)TrangThaiPhatHanh.ChuaPhatHanh;
            entity.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.ChuaGui;

            if (!string.IsNullOrEmpty(model.BienBanDieuChinhId))
            {
                BienBanDieuChinh bienBanDieuChinh = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == model.BienBanDieuChinhId);
                if (bienBanDieuChinh != null)
                {
                    bienBanDieuChinh.HoaDonDieuChinhId = entity.HoaDonDienTuId;
                }
            }

            entity.NgayLap = DateTime.Now;

            if (!string.IsNullOrEmpty(entity.LyDoThayThe))
            {
                entity.TrangThai = (int)TrangThaiHoaDon.HoaDonThayThe;
            }

            if (!string.IsNullOrEmpty(entity.LyDoDieuChinh))
            {
                entity.TrangThai = (int)TrangThaiHoaDon.HoaDonDieuChinh;
            }

            var _khachHang = await _db.DoiTuongs.AsNoTracking().FirstOrDefaultAsync(x => x.DoiTuongId == entity.KhachHangId);
            if (_khachHang != null)
            {
                entity.MaKhachHang = _khachHang.Ma;
                entity.HoTenNguoiNhanHD = _khachHang.HoTenNguoiNhanHD;
                entity.EmailNguoiNhanHD = _khachHang.EmailNguoiNhanHD;
                entity.SoDienThoaiNguoiNhanHD = _khachHang.SoDienThoaiNguoiNhanHD;
            }
            else
            {
                entity.MaKhachHang = string.Empty;
                entity.HoTenNguoiNhanHD = string.Empty;
                entity.EmailNguoiNhanHD = string.Empty;
                entity.SoDienThoaiNguoiNhanHD = string.Empty;
            }

            var _nhanVienBanHang = await _db.DoiTuongs.AsNoTracking().FirstOrDefaultAsync(x => x.DoiTuongId == entity.NhanVienBanHangId);
            if (_nhanVienBanHang != null)
            {
                entity.MaNhanVienBanHang = _nhanVienBanHang.Ma;
            }
            else
            {
                entity.MaNhanVienBanHang = string.Empty;
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

        public async Task<bool> UpdateAsync(HoaDonDienTuViewModel model)
        {
            model.HoaDonChiTiets = null;

            var _khachHang = await _db.DoiTuongs.AsNoTracking().FirstOrDefaultAsync(x => x.DoiTuongId == model.KhachHangId);
            if (_khachHang != null)
            {
                model.MaKhachHang = _khachHang.Ma;
                model.HoTenNguoiNhanHD = _khachHang.HoTenNguoiNhanHD;
                model.EmailNguoiNhanHD = _khachHang.EmailNguoiNhanHD;
                model.SoDienThoaiNguoiNhanHD = _khachHang.SoDienThoaiNguoiNhanHD;
            }
            else
            {
                model.MaKhachHang = string.Empty;
                model.HoTenNguoiNhanHD = string.Empty;
                model.EmailNguoiNhanHD = string.Empty;
                model.SoDienThoaiNguoiNhanHD = string.Empty;
            }

            var _nhanVienBanHang = await _db.DoiTuongs.AsNoTracking().FirstOrDefaultAsync(x => x.DoiTuongId == model.NhanVienBanHangId);
            if (_nhanVienBanHang != null)
            {
                model.MaNhanVienBanHang = _nhanVienBanHang.Ma;
            }
            else
            {
                model.MaNhanVienBanHang = string.Empty;
            }

            var _mauHoaDon = await _db.MauHoaDons.AsNoTracking().FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId);
            if (_mauHoaDon != null)
            {
                model.MauSo = _mauHoaDon.MauSo;
                model.KyHieu = _mauHoaDon.KyHieu;
            }
            else
            {
                model.MauSo = string.Empty;
                model.KyHieu = string.Empty;
            }

            HoaDonDienTu entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == model.HoaDonDienTuId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRangeAsync(List<HoaDonDienTuViewModel> models)
        {
            foreach (var model in models)
            {
                model.HoaDonChiTiets = null;
                model.TruongThongTinBoSung1 = null;
                model.TruongThongTinBoSung2 = null;
                model.TruongThongTinBoSung3 = null;
                model.TruongThongTinBoSung4 = null;
                model.TruongThongTinBoSung5 = null;
                model.TruongThongTinBoSung6 = null;
                model.TruongThongTinBoSung7 = null;
                model.TruongThongTinBoSung8 = null;
                model.TruongThongTinBoSung9 = null;
                model.TruongThongTinBoSung10 = null;

                model.ModifyDate = DateTime.Now;
                model.ModifyBy = model.ActionUser.UserId;
            }

            var ids = models.Select(x => x.HoaDonDienTuId).ToList();
            var entities = await _db.HoaDonDienTus.Where(x => ids.Contains(x.HoaDonDienTuId)).ToListAsync();
            //_db.ChungTuNghiepVuKhacs.Update(entity);
            _db.Entry(entities).CurrentValues.SetValues(models);
            return await _db.SaveChangesAsync() == models.Count;
        }

        private string GetLinkExcelFile(string link)
        {
            var filename = "FilesUpload/excels/" + link;
            string url;
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
                else
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
                    else item.SoLuong = hoaDons.Count(x => x.TrangThaiGuiHoaDon == (item.TrangThaiId - 4));
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
            IQueryable<HoaDonDienTuViewModel> query = _db.HoaDonDienTus
            .OrderByDescending(x => x.NgayHoaDon)
            .ThenByDescending(x => x.SoHoaDon)
            .Select(hd => new HoaDonDienTuViewModel
            {
                HoaDonDienTuId = hd.HoaDonDienTuId,
                NgayHoaDon = hd.NgayHoaDon,
                NgayLap = hd.CreatedDate,
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

            string excelFileName = $"BANG_KE_HOA_DON_DIEN_TU-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            string excelFolder = $"FilesUpload/excels/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            // Excel
            string _sample = $"docs/HoaDonDienTu/BANG_KE_HOA_DON_DIEN_TU.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
            FileInfo file = new FileInfo(_path_sample);
            string dateReport = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(pagingParams.FromDate).ToString("dd/MM/yyyy"), DateTime.Parse(pagingParams.ToDate).ToString("dd/MM/yyyy"));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                List<HoaDonDienTuViewModel> list = await query.OrderBy(x => x.NgayHoaDon).ToListAsync();
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
                    worksheet.Cells[idx, 6].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.KhachHang != null ? it.KhachHang.Ma : string.Empty);
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

        public async Task<string> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params)
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
                NgayLap = hd.CreatedDate,
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

            string excelFileName = $"BANG_KE_CHI_TIET_HOA_DON_DIEN_TU-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            string excelFolder = $"FilesUpload/excels/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            // Excel
            string _sample = $"docs/HoaDonDienTu/BANG_KE_CHI_TIET_HOA_DON_DIEN_TU.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
            FileInfo file = new FileInfo(_path_sample);
            string dateReport = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(@params.TuNgay).ToString("dd/MM/yyyy"), DateTime.Parse(@params.DenNgay).ToString("dd/MM/yyyy"));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                List<HoaDonDienTuViewModel> list = await query.OrderBy(x => x.NgayHoaDon).ToListAsync();
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
                        worksheet.Cells[idx, 39].Value = it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng";
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

            var maxSoCT = await _db.HoaDonDienTus.Where(x => !string.IsNullOrEmpty(x.SoCTXoaBo))
                                                .MaxAsync(x => x.SoCTXoaBo);
            if (!string.IsNullOrEmpty(maxSoCT))
            {
                var number = maxSoCT.Substring(3);
                var next = int.Parse(number) + 1;
                result = "XHĐ" + next.ToString("00000");
            }
            else result = "XHĐ00001";

            return result;
        }

        public async Task<string> CreateSoBienBanXoaBoHoaDon()
        {
            var result = string.Empty;

            var maxSoCT = await _db.BienBanXoaBos.Where(x => !string.IsNullOrEmpty(x.SoBienBan))
                                                .MaxAsync(x => x.SoBienBan);
            if (!string.IsNullOrEmpty(maxSoCT))
            {
                var number = maxSoCT.Substring(3);
                var next = int.Parse(number) + 1;
                result = "BBH" + next.ToString("00000");
            }
            else result = "BBH00001";

            return result;
        }

        public async Task<KetQuaCapSoHoaDon> CreateSoHoaDon(HoaDonDienTuViewModel hd)
        {
            try
            {
                var validMaxSoHoaDon = _db.HoaDonDienTus
                                        .Where(x => x.BoKyHieuHoaDonId == hd.BoKyHieuHoaDonId && !string.IsNullOrEmpty(x.SoHoaDon))
                                        .DefaultIfEmpty()
                                        .Max(x => x.SoHoaDon);

                if (string.IsNullOrEmpty(validMaxSoHoaDon))
                {
                    validMaxSoHoaDon = "0";
                }

                return new KetQuaCapSoHoaDon
                {
                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                    SoHoaDon = (int.Parse(validMaxSoHoaDon) + 1).ToString()
                };

                //var thongBaoPhatHanh = await _db.ThongBaoPhatHanhChiTiets
                //                       .Include(x => x.ThongBaoPhatHanh)
                //                       .Where(x => x.MauHoaDonId == hd.MauHoaDonId)
                //                       .OrderByDescending(x => x.NgayBatDauSuDung)
                //                       .FirstOrDefaultAsync();

                //var mauHoaDon = await _db.MauHoaDons.AsNoTracking().FirstOrDefaultAsync(x => x.MauHoaDonId == hd.MauHoaDonId);

                //if (thongBaoPhatHanh == null)
                //{
                //    return new KetQuaCapSoHoaDon
                //    {
                //        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.ChuaTimThayThongBaoPhatHanh,
                //        SoHoaDon = string.Empty,
                //        ErrorMessage = "Chưa lập thông báo phát hành cho mẫu hóa đơn tương ứng, hoặc thông báo phát hành chưa được cơ quan thuế chấp nhận"
                //    };
                //}
                //else if (thongBaoPhatHanh.ThongBaoPhatHanh.TrangThaiNop != TrangThaiNop.DaDuocChapNhan)
                //{
                //    var converMaxToInt = int.Parse(validMaxSoHoaDon);
                //    return new KetQuaCapSoHoaDon
                //    {
                //        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.ChuaDuocCQTChapNhan,
                //        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung) :
                //                   (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)) : thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung),
                //        ErrorMessage = "Chưa lập thông báo phát hành cho mẫu hóa đơn tương ứng, hoặc thông báo phát hành chưa được cơ quan thuế chấp nhận"
                //    };
                //}
                //else if (thongBaoPhatHanh.NgayBatDauSuDung > hd.NgayHoaDon)
                //{
                //    var converMaxToInt = !string.IsNullOrEmpty(validMaxSoHoaDon) ? int.Parse(validMaxSoHoaDon) : 0;
                //    return new KetQuaCapSoHoaDon
                //    {
                //        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.NgayHoaDonNhoHonNgayBatDauSuDung,
                //        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt >= thongBaoPhatHanh.TuSo ? (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung) :
                //                   thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)) : thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung),
                //        ErrorMessage = $"Ngày hóa đơn không được nhỏ hơn ngày bắt đầu sử dụng của hóa đơn trên thông báo phát hành hóa đơn <{thongBaoPhatHanh.NgayBatDauSuDung.Value:dd/MM/yyyy}>"
                //    };
                //}
                //else if (DateTime.Now.Date > hd.NgayHoaDon.Value.Date)
                //{
                //    var converMaxToInt = !string.IsNullOrEmpty(validMaxSoHoaDon) ? int.Parse(validMaxSoHoaDon) : 0;
                //    return new KetQuaCapSoHoaDon
                //    {
                //        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.NgayHoaDonNhoHonNgayKy,
                //        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung) :
                //                   (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)) : thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung),
                //        ErrorMessage = "Ngày hóa đơn không được nhỏ hơn ngày ký"
                //    };
                //}
                //else
                //{
                //    if (!string.IsNullOrEmpty(validMaxSoHoaDon))
                //    {
                //        var converMaxToInt = int.Parse(validMaxSoHoaDon);
                //        if (converMaxToInt < thongBaoPhatHanh.TuSo)
                //        {
                //            return new KetQuaCapSoHoaDon
                //            {
                //                LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                //                SoHoaDon = thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)
                //            };
                //        }
                //        else if (converMaxToInt >= thongBaoPhatHanh.DenSo)
                //        {
                //            return new KetQuaCapSoHoaDon
                //            {
                //                LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.SoHoaDonVuotQuaGioiHanDangKy,
                //                SoHoaDon = (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung),
                //                ErrorMessage = "Số hóa đơn vượt quá giới hạn đã đăng ký với cơ quan thuế hoặc thông tin không chính xác so với thông báo phát hành"
                //            };
                //        }
                //        else
                //        {
                //            var _hdNgayNhoHon = _db.HoaDonDienTus.Where(x => x.NgayHoaDon < hd.NgayHoaDon && x.MauHoaDonId == hd.MauHoaDonId && !string.IsNullOrEmpty(x.SoHoaDon)).ToList();
                //            if (_hdNgayNhoHon.Any())
                //            {
                //                foreach (var item in _hdNgayNhoHon)
                //                {
                //                    if (int.Parse(item.SoHoaDon) > int.Parse(validMaxSoHoaDon) + 1)
                //                    {
                //                        return new KetQuaCapSoHoaDon
                //                        {
                //                            LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.SoHoaDonNhoHonSoHoaDonTruocDo,
                //                            SoHoaDon = (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung),
                //                            ErrorMessage = "Hóa đơn có số nhỏ hơn không được có ngày lớn hơn ngày của hóa đơn có số lớn hơn"
                //                        };
                //                    }
                //                }

                //                return new KetQuaCapSoHoaDon
                //                {
                //                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                //                    SoHoaDon = (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)
                //                };
                //            }
                //            else
                //            {
                //                return new KetQuaCapSoHoaDon
                //                {
                //                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                //                    SoHoaDon = (converMaxToInt + 1).GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)
                //                };
                //            }
                //        }
                //    }
                //    else
                //    {
                //        return new KetQuaCapSoHoaDon
                //        {
                //            LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.KhongLoi,
                //            SoHoaDon = thongBaoPhatHanh.TuSo.Value.GetFormatSoHoaDon(mauHoaDon.QuyDinhApDung)
                //        };
                //    }
                //}
            }
            catch (Exception)
            {
                return new KetQuaCapSoHoaDon
                {
                    LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.LoiKhac,
                    SoHoaDon = string.Empty
                };
            }
        }

        public async Task<ResultParams> CapPhatSoHoaDon(HoaDonDienTuViewModel hd, string soHoaDon)
        {
            try
            {
                hd.SoHoaDon = soHoaDon;
                var updateRes = await this.UpdateAsync(hd);
                if (updateRes)
                {
                    return new ResultParams
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    return new ResultParams
                    {
                        Success = false,
                        ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultParams
                {
                    Success = false,
                    ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                };
            }
        }

        public async Task<ResultParams> CapPhatSoHoaDonHangLoat(List<HoaDonDienTuViewModel> hd, List<string> soHoaDon)
        {
            try
            {
                for (int i = 0; i < hd.Count; i++)
                {
                    hd[i].SoHoaDon = soHoaDon[i];
                }
                var updateRes = await this.UpdateRangeAsync(hd);
                if (updateRes)
                {
                    return new ResultParams
                    {
                        Success = true,
                        ErrorMessage = string.Empty
                    };
                }
                else
                {
                    return new ResultParams
                    {
                        Success = false,
                        ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultParams
                {
                    Success = false,
                    ErrorMessage = "Có lỗi xảy ra trong quá trình cập nhật hóa đơn"
                };
            }
        }

        public async Task<List<ChiTietMauHoaDon>> GetListChiTietByMauHoaDon(string mauHoaDonId)
        {
            var result = new List<ChiTietMauHoaDon>();

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

            return result;
        }

        public async Task<KetQuaConvertPDF> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd)
        {
            var path = string.Empty;
            var pathXML = string.Empty;
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{hd.HoaDonDienTuId}";
            string pdfFileName = string.Empty;
            string xmlFileName = string.Empty;

            if (hd.TrangThaiPhatHanh == 3 && !string.IsNullOrEmpty(hd.FileDaKy) || !string.IsNullOrEmpty(hd.XMLDaKy))
            {
                return new KetQuaConvertPDF
                {
                    FilePDF = Path.Combine(assetsFolder, $"pdf/signed/{hd.FileDaKy}"),
                    FileXML = Path.Combine(assetsFolder, $"xml/signed/{hd.XMLDaKy}"),
                };
            }

            var _tuyChons = await _TuyChonService.GetAllAsync();

            var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
            var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
            var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());

            var hoSoHDDT = await _HoSoHDDTService.GetDetailAsync();
            var mauHoaDon = await _MauHoaDonService.GetByIdAsync(hd.MauHoaDonId);
            hd.MauHoaDon = mauHoaDon;

            var doc = MauHoaDonHelper.TaoMauHoaDonDoc(mauHoaDon, hd.GetBoMauHoaDonFromHoaDonDienTu(), _hostingEnvironment, _IHttpContextAccessor, out int beginRow, !string.IsNullOrEmpty(hd.LyDoThayThe) || !string.IsNullOrEmpty(hd.LyDoDieuChinh));

            doc.Replace(LoaiChiTietTuyChonNoiDung.MaCuaCQT.GenerateKeyTag(), string.Empty, true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.MauSo.GenerateKeyTag(), hd.MauSo ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.KyHieu.GenerateKeyTag(), hd.KyHieu ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.SoHoaDon.GenerateKeyTag(), string.IsNullOrEmpty(hd.SoHoaDon) ? "<Chưa cấp số>" : hd.SoHoaDon, true, true);

            doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
            doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
            doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.HoTenNguoiMua.GenerateKeyTag(), hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua.GenerateKeyTag(), hd.KhachHang != null ? (hd.KhachHang.TenDonVi ?? string.Empty) : string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua.GenerateKeyTag(), hd.MaSoThue ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua.GenerateKeyTag(), hd.DiaChi ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.HinhThucThanhToan.GenerateKeyTag(), hd.HinhThucThanhToan?.Ten ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua.GenerateKeyTag(), hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

            List<Table> listTable = new List<Table>();
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

            string soTienBangChu = (hd.IsVND == true ? hd.TongTienThanhToan : hd.TongTienThanhToanQuyDoi).Value.ConvertToInWord(_cachDocSo0HangChuc.ToLower(), _cachDocHangNghin.ToLower(), _hienThiSoChan);
            List<HoaDonDienTuChiTietViewModel> models = await _HoaDonDienTuChiTietService.GetChiTietHoaDonAsync(hd.HoaDonDienTuId);

            int line = models.Count();
            if (line > 0)
            {
                Table table = null;
                if (listTable.Count > 0)
                {
                    table = listTable[0];
                }

                var thueGTGT = TextHelper.GetThueGTGTByNgayHoaDon(hd.NgayHoaDon.Value, models.Select(x => x.ThueGTGT).FirstOrDefault());

                doc.Replace(LoaiChiTietTuyChonNoiDung.TyLeChietKhau.GenerateKeyTag(), (models.Sum(x => x.TyLeChietKhau) / models.Count).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienChietKhau.GenerateKeyTag(), hd.TongTienChietKhau.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.TienThueGTGT.GenerateKeyTag(), (thueGTGT == "\\" ? "\\" : hd.TongTienThueGTGT.Value.FormatPriceTwoDecimal()) ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHang.GenerateKeyTag(), hd.TongTienHang.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.ThueSuatGTGT.GenerateKeyTag(), thueGTGT ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienThanhToan.GenerateKeyTag(), hd.TongTienThanhToan.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienBangChu.GenerateKeyTag(), soTienBangChu ?? string.Empty, true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.TyGia.GenerateKeyTag(), (hd.TyGia.Value.FormatPriceTwoDecimal() + $" VND/{hd.MaLoaiTien}") ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.QuyDoi.GenerateKeyTag(), (hd.TongTienThanhToanQuyDoi.Value.FormatPriceTwoDecimal() + " VND") ?? string.Empty, true, true);

                if (!string.IsNullOrEmpty(hd.LyDoThayThe))
                {
                    LyDoThayTheModel lyDoThayThe = JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe);
                    doc.Replace("<reason>", lyDoThayThe.ToString() ?? string.Empty, true, true);
                }

                if (!string.IsNullOrEmpty(hd.LyDoDieuChinh))
                {
                    LyDoDieuChinhModel lyDoDieuChinh = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh);
                    doc.Replace("<reason>", lyDoDieuChinh.ToString() ?? string.Empty, true, true);
                }

                if (table != null)
                {
                    for (int i = 0; i < line - 1; i++)
                    {
                        // Clone row
                        TableRow cl_row = table.Rows[1].Clone();
                        table.Rows.Insert(1, cl_row);
                    }

                    TableRow row = null;
                    if (mauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauMotThueSuat)
                    {
                        for (int i = 0; i < line; i++)
                        {
                            row = table.Rows[i + beginRow];

                            row.Cells[0].Paragraphs[0].SetValuePar((i + 1).ToString());

                            row.Cells[1].Paragraphs[0].SetValuePar(models[i].TenHang);

                            row.Cells[2].Paragraphs[0].SetValuePar(models[i].DonViTinh?.Ten);

                            row.Cells[3].Paragraphs[0].SetValuePar(models[i].SoLuong.Value.FormatQuanity());

                            row.Cells[4].Paragraphs[0].SetValuePar(models[i].DonGia.Value.FormatPriceTwoDecimal());

                            row.Cells[5].Paragraphs[0].SetValuePar(models[i].ThanhTien.Value.FormatPriceTwoDecimal());
                        }
                    }
                    else
                    {

                        for (int i = 0; i < line; i++)
                        {
                            row = table.Rows[i + beginRow];

                            row.Cells[0].Paragraphs[0].SetValuePar((i + 1).ToString());

                            row.Cells[1].Paragraphs[0].SetValuePar(models[i].TenHang);

                            row.Cells[2].Paragraphs[0].SetValuePar(models[i].DonViTinh?.Ten);

                            row.Cells[3].Paragraphs[0].SetValuePar(models[i].SoLuong.Value.FormatQuanity());

                            row.Cells[4].Paragraphs[0].SetValuePar(models[i].DonGia.Value.FormatPriceTwoDecimal());

                            row.Cells[5].Paragraphs[0].SetValuePar(models[i].ThanhTien.Value.FormatPriceTwoDecimal());

                            row.Cells[6].Paragraphs[0].SetValuePar(models[i].ThueGTGT);

                            row.Cells[7].Paragraphs[0].SetValuePar(models[i].TienThueGTGT.Value.FormatPriceTwoDecimal());
                        }
                    }
                }
            }
            else
            {
                MauHoaDonHelper.CreatePreviewFileDoc(doc, mauHoaDon, _IHttpContextAccessor);
            }

            var fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/unsigned");
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"xml/unsigned");
            #region create folder
            if (!Directory.Exists(fullPdfFolder))
            {
                Directory.CreateDirectory(fullPdfFolder);
            }
            else
            {
                string[] files = Directory.GetFiles(fullPdfFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

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

            pdfFileName = $"{Guid.NewGuid()}.pdf";
            xmlFileName = $"{Guid.NewGuid()}.xml";

            hd.HoaDonChiTiets = models;
            hd.SoTienBangChu = soTienBangChu;
            doc.SaveToFile(Path.Combine(fullPdfFolder, $"{pdfFileName}"), FileFormat.PDF);
            await _xMLInvoiceService.CreateXMLInvoice(Path.Combine(fullXmlFolder, $"{xmlFileName}"), hd);

            path = Path.Combine(assetsFolder, $"pdf/unsigned", $"{pdfFileName}");
            pathXML = Path.Combine(assetsFolder, $"xml/unsigned", $"{xmlFileName}");
            doc.Close();

            return new KetQuaConvertPDF()
            {
                FilePDF = path,
                FileXML = pathXML,
                PdfName = pdfFileName,
                XMLName = xmlFileName
            };
        }

        public async Task<KetQuaChuyenDoi> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay @params)
        {
            try
            {
                var _objHDDT = await GetByIdAsync(@params.HoaDonDienTuId);
                if (_objHDDT != null)
                {
                    var pathPdf = await ConvertHoaDonToHoaDonGiay(_objHDDT, @params);
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
            catch (Exception)
            {
                return new KetQuaChuyenDoi
                {
                    ThanhCong = false,
                    Loi = "Bị lỗi khi chuyển đổi hóa đơn, đề nghị liên lạc với admin",
                    PathFile = string.Empty
                };
            }
        }

        public async Task<bool> CheckMaTraCuuAsync(string maTraCuu)
        {
            var result = await _db.HoaDonDienTus
                                    .Where(x => x.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.MaTraCuu == maTraCuu);

            return result != null;
        }

        private async Task<string> ConvertHoaDonToHoaDonGiay(HoaDonDienTuViewModel hd, ParamsChuyenDoiThanhHDGiay @params)
        {
            var path = string.Empty;

            var _tuyChons = await _TuyChonService.GetAllAsync();

            var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
            var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
            var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var taxCode = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{hd.HoaDonDienTuId}/pdf/convertion";

            var hoSoHDDT = await _HoSoHDDTService.GetDetailAsync();
            var mauHoaDon = await _MauHoaDonService.GetByIdAsync(hd.MauHoaDonId);

            var doc = MauHoaDonHelper.TaoMauHoaDonDoc(mauHoaDon, hd.GetBoMauHoaDonFromHoaDonDienTu(false), _hostingEnvironment, _IHttpContextAccessor, out int beginRow, !string.IsNullOrEmpty(hd.LyDoThayThe) || !string.IsNullOrEmpty(hd.LyDoDieuChinh));

            doc.Replace(LoaiChiTietTuyChonNoiDung.MaCuaCQT.GenerateKeyTag(), string.Empty, true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.MauSo.GenerateKeyTag(), hd.MauSo ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.KyHieu.GenerateKeyTag(), hd.KyHieu ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.SoHoaDon.GenerateKeyTag(), string.IsNullOrEmpty(hd.SoHoaDon) ? "<Chưa cấp số>" : hd.SoHoaDon, true, true);

            doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
            doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
            doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.HoTenNguoiMua.GenerateKeyTag(), hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua.GenerateKeyTag(), hd.KhachHang != null ? (hd.KhachHang.TenDonVi ?? string.Empty) : string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua.GenerateKeyTag(), hd.MaSoThue ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua.GenerateKeyTag(), hd.DiaChi ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.HinhThucThanhToan.GenerateKeyTag(), hd.HinhThucThanhToan != null ? hd.HinhThucThanhToan.Ten : string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua.GenerateKeyTag(), hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

            doc.Replace("<convertor>", @params.TenNguoiChuyenDoi ?? string.Empty, true, true);
            doc.Replace("<conversionDate>", @params.NgayChuyenDoi.Value.ToString("dd/MM/yyyy") ?? string.Empty, true, true);

            List<Table> listTable = new List<Table>();
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
                Table table = null;
                table = listTable[0];

                var thueGTGT = TextHelper.GetThueGTGTByNgayHoaDon(hd.NgayHoaDon.Value, models.Select(x => x.ThueGTGT).FirstOrDefault());

                doc.Replace(LoaiChiTietTuyChonNoiDung.TyLeChietKhau.GenerateKeyTag(), (models.Sum(x => x.TyLeChietKhau) / models.Count).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienChietKhau.GenerateKeyTag(), hd.TongTienChietKhau.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.TienThueGTGT.GenerateKeyTag(), (thueGTGT == "\\" ? "\\" : hd.TongTienThueGTGT.Value.FormatPriceTwoDecimal()) ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHang.GenerateKeyTag(), hd.TongTienHang.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.ThueSuatGTGT.GenerateKeyTag(), thueGTGT ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienThanhToan.GenerateKeyTag(), hd.TongTienThanhToan.Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienBangChu.GenerateKeyTag(), hd.TongTienThanhToan.Value.ConvertToInWord(_cachDocSo0HangChuc.ToLower(), _cachDocHangNghin.ToLower(), _hienThiSoChan) ?? string.Empty, true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.TyGia.GenerateKeyTag(), (hd.TyGia.Value.FormatPriceTwoDecimal() + $" VND/{hd.MaLoaiTien}") ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.QuyDoi.GenerateKeyTag(), (hd.TongTienThanhToanQuyDoi.Value.FormatPriceTwoDecimal() + " VND") ?? string.Empty, true, true);

                if (!string.IsNullOrEmpty(hd.LyDoThayThe))
                {
                    LyDoThayTheModel lyDoThayThe = JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe);
                    doc.Replace("<reason>", lyDoThayThe.ToString() ?? string.Empty, true, true);
                }

                if (!string.IsNullOrEmpty(hd.LyDoDieuChinh))
                {
                    LyDoDieuChinhModel lyDoDieuChinh = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh);
                    doc.Replace("<reason>", lyDoDieuChinh.ToString() ?? string.Empty, true, true);
                }

                for (int i = 0; i < line - 1; i++)
                {
                    // Clone row
                    TableRow cl_row = table.Rows[1].Clone();
                    table.Rows.Insert(1, cl_row);
                }

                TableRow row = null;
                if (mauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauMotThueSuat)
                {
                    for (int i = 0; i < line; i++)
                    {
                        row = table.Rows[i + beginRow];

                        row.Cells[0].Paragraphs[0].SetValuePar((i + 1).ToString());

                        row.Cells[1].Paragraphs[0].SetValuePar(models[i].TenHang);

                        row.Cells[2].Paragraphs[0].SetValuePar(models[i].DonViTinh?.Ten);

                        row.Cells[3].Paragraphs[0].SetValuePar(models[i].SoLuong.Value.FormatQuanity());

                        row.Cells[4].Paragraphs[0].SetValuePar(models[i].DonGia.Value.FormatPriceTwoDecimal());

                        row.Cells[5].Paragraphs[0].SetValuePar(models[i].ThanhTien.Value.FormatPriceTwoDecimal());
                    }
                }
                else
                {

                    for (int i = 0; i < line; i++)
                    {
                        row = table.Rows[i + beginRow];

                        row.Cells[0].Paragraphs[0].SetValuePar((i + 1).ToString());

                        row.Cells[1].Paragraphs[0].SetValuePar(models[i].TenHang);

                        row.Cells[2].Paragraphs[0].SetValuePar(models[i].DonViTinh?.Ten);

                        row.Cells[3].Paragraphs[0].SetValuePar(models[i].SoLuong.Value.FormatQuanity());

                        row.Cells[4].Paragraphs[0].SetValuePar(models[i].DonGia.Value.FormatPriceTwoDecimal());

                        row.Cells[5].Paragraphs[0].SetValuePar(models[i].ThanhTien.Value.FormatPriceTwoDecimal());

                        row.Cells[6].Paragraphs[0].SetValuePar(models[i].ThueGTGT);

                        row.Cells[7].Paragraphs[0].SetValuePar(models[i].TienThueGTGT.Value.FormatPriceTwoDecimal());
                    }
                }
            }
            else
            {
                MauHoaDonHelper.CreatePreviewFileDoc(doc, mauHoaDon, _IHttpContextAccessor);
            }

            var pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            if (!Directory.Exists(pdfFolder))
            {
                Directory.CreateDirectory(pdfFolder);
            }
            else
            {
                string[] files = Directory.GetFiles(pdfFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }

            string pdfFileName = $"{Guid.NewGuid()}.pdf";
            string pdfPath = Path.Combine(pdfFolder, pdfFileName);
            doc.SaveToFile(pdfPath, FileFormat.PDF);
            USBTokenSign uSBTokenSign = new USBTokenSign(_mp.Map<HoSoHDDTViewModel>(hoSoHDDT), _hostingEnvironment);
            uSBTokenSign.DigitalSignaturePDF(pdfPath, hd.NgayHoaDon.Value);
            path = Path.Combine(assetsFolder, pdfFileName);

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

            return path;
        }


        public async Task<bool> ThemNhatKyThaoTacHoaDonAsync(NhatKyThaoTacHoaDonViewModel model)
        {
            var entity = _mp.Map<NhatKyThaoTacHoaDon>(model);
            await _db.AddAsync<NhatKyThaoTacHoaDon>(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<LuuTruTrangThaiFileHDDTViewModel> GetTrangThaiLuuTru(string HoaDonDienTuId)
        {
            var result = await _db.LuuTruTrangThaiFileHDDTs.Where(x => x.HoaDonDienTuId == HoaDonDienTuId)
                                                    .FirstOrDefaultAsync();

            return _mp.Map<LuuTruTrangThaiFileHDDTViewModel>(result);
        }

        public async Task<bool> UpdateTrangThaiLuuFileHDDT(LuuTruTrangThaiFileHDDTViewModel model)
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

        public async Task<bool> UpdateTrangThaiLuuFileBBXB(LuuTruTrangThaiBBXBViewModel model)
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

        [Obsolete]
        public async Task<bool> GateForWebSocket(ParamPhatHanhHD param)
        {
            if (!string.IsNullOrEmpty(param.HoaDonDienTuId))
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{param.HoaDonDienTuId}";

                var _objHDDT = await GetByIdAsync(param.HoaDonDienTuId);
                if (_objHDDT != null)
                {
                    string oldSignedPdfPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{_objHDDT.FileDaKy}");
                    string oldSignedXmlPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{_objHDDT.XMLDaKy}");
                    if (File.Exists(oldSignedPdfPath))
                    {
                        File.Delete(oldSignedPdfPath);
                    }
                    if (File.Exists(oldSignedXmlPath))
                    {
                        File.Delete(oldSignedXmlPath);
                    }

                    // Create name file.
                    string pre = string.Empty;
                    if (!string.IsNullOrEmpty(_objHDDT.FileDaKy))
                    {
                        pre = new String(_objHDDT.FileDaKy.Where(Char.IsLetterOrDigit).ToArray());
                    }

                    string newPdfFileName = !string.IsNullOrEmpty(pre) ? $"{pre}_{param.HoaDon.SoHoaDon}_{Guid.NewGuid()}.pdf" : $"{param.HoaDon.SoHoaDon}_{Guid.NewGuid()}.pdf";
                    string newXmlFileName = newPdfFileName.Replace(".pdf", ".xml");
                    string newSignedPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed");
                    string newSignedXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"xml/signed");
                    if (!Directory.Exists(newSignedPdfFolder))
                    {
                        Directory.CreateDirectory(newSignedPdfFolder);
                    }
                    if (!Directory.Exists(newSignedXmlFolder))
                    {
                        Directory.CreateDirectory(newSignedXmlFolder);
                    }

                    _objHDDT.ActionUser = param.HoaDon.ActionUser;
                    _objHDDT.FileDaKy = newPdfFileName;
                    _objHDDT.XMLDaKy = newXmlFileName;
                    _objHDDT.TrangThaiPhatHanh = (int)TrangThaiPhatHanh.DaPhatHanh;
                    _objHDDT.SoHoaDon = param.HoaDon.SoHoaDon;
                    _objHDDT.MaTraCuu = param.HoaDon.MaTraCuu;
                    _objHDDT.NgayHoaDon = param.HoaDon.NgayHoaDon;
                    await UpdateAsync(_objHDDT);

                    var checkDaDungHetSLHD = await _boKyHieuHoaDonService.CheckDaHetSoLuongHoaDonAsync(_objHDDT.BoKyHieuHoaDonId, _objHDDT.SoHoaDon);
                    if (checkDaDungHetSLHD) // đã dùng hết
                    {
                        await _boKyHieuHoaDonService.XacThucBoKyHieuHoaDonAsync(new NhatKyXacThucBoKyHieuViewModel
                        {
                            BoKyHieuHoaDonId = _objHDDT.BoKyHieuHoaDonId,
                            TrangThaiSuDung = TrangThaiSuDung.HetHieuLuc,
                            IsHetSoLuongHoaDon = true,
                            SoLuongHoaDon = int.Parse(_objHDDT.SoHoaDon)
                        });
                    }

                    var _objTrangThaiLuuTru = await GetTrangThaiLuuTru(_objHDDT.HoaDonDienTuId);
                    _objTrangThaiLuuTru = _objTrangThaiLuuTru ?? new LuuTruTrangThaiFileHDDTViewModel();
                    if (string.IsNullOrEmpty(_objTrangThaiLuuTru.HoaDonDienTuId)) _objTrangThaiLuuTru.HoaDonDienTuId = _objHDDT.HoaDonDienTuId;

                    // PDF 
                    //byte[] bytePDF = DataHelper.StringToByteArray(@param.DataPDF);
                    //_objTrangThaiLuuTru.PdfDaKy = bytePDF;
                    //File.WriteAllBytes(Path.Combine(newSignedPdfFolder, newPdfFileName), _objTrangThaiLuuTru.PdfDaKy);

                    //xml
                    string xmlDeCode = DataHelper.Base64Decode(@param.DataXML);
                    byte[] byteXML = Encoding.UTF8.GetBytes(@param.DataXML);
                    _objTrangThaiLuuTru.XMLDaKy = byteXML;
                    File.WriteAllText(Path.Combine(newSignedXmlFolder, newXmlFileName), xmlDeCode);

                    await UpdateTrangThaiLuuFileHDDT(_objTrangThaiLuuTru);

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
                        if (!string.IsNullOrEmpty(param.HoaDon.EmailNguoiNhanHD) && param.HoaDon.EmailNguoiNhanHD.IsValidEmail() && await SendEmail(param.HoaDon))
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

            return true;
        }

        public async Task<LuuTruTrangThaiBBXBViewModel> GetTrangThaiLuuTruBBXB(string BienBanXoaBoId)
        {
            var result = await _db.LuuTruTrangThaiBBXBs.Where(x => x.BienBanXoaBoId == BienBanXoaBoId)
                                                    .FirstOrDefaultAsync();

            return _mp.Map<LuuTruTrangThaiBBXBViewModel>(result);
        }

        public async Task<bool> GateForWebSocket(ParamKyBienBanHuyHoaDon param)
        {
            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonXoaBo);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{param.BienBan.HoaDonDienTuId}";
                var objHSDetail = await _HoSoHDDTService.GetDetailAsync();

                if (!string.IsNullOrEmpty(param.BienBan.HoaDonDienTuId))
                {
                    var _objHDDT = await this.GetByIdAsync(param.BienBan.HoaDonDienTuId);
                    if (_objHDDT != null)
                    {
                        // Delete file if exist
                        string oldSignedPdfPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{_objHDDT.FileDaKy}");
                        string oldSignedXmlPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{_objHDDT.XMLDaKy}");
                        if (File.Exists(oldSignedPdfPath))
                        {
                            File.Delete(oldSignedPdfPath);
                        }
                        if (File.Exists(oldSignedXmlPath))
                        {
                            File.Delete(oldSignedXmlPath);
                        }

                        // Create name file.
                        string pre = string.Empty;
                        if (!string.IsNullOrEmpty(_objHDDT.FileDaKy))
                        {
                            pre = new String(_objHDDT.FileDaKy.Where(Char.IsLetterOrDigit).ToArray());
                        }

                        string newPdfFileName = !string.IsNullOrEmpty(pre) ? $"{pre}_{param.BienBan.SoBienBan}_{Guid.NewGuid()}.pdf" : $"{param.BienBan.SoBienBan}_{Guid.NewGuid()}.pdf";
                        string newXmlFileName = newPdfFileName.Replace(".pdf", ".xml");
                        string newSignedPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed");
                        string newSignedXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"xml/signed");
                        if (!Directory.Exists(newSignedPdfFolder))
                        {
                            Directory.CreateDirectory(newSignedPdfFolder);
                        }
                        if (!Directory.Exists(newSignedXmlFolder))
                        {
                            Directory.CreateDirectory(newSignedXmlFolder);
                        }

                        var _objTrangThaiLuuTru = await GetTrangThaiLuuTruBBXB(param.BienBan.Id);
                        _objTrangThaiLuuTru = _objTrangThaiLuuTru ?? new LuuTruTrangThaiBBXBViewModel();
                        if (string.IsNullOrEmpty(_objTrangThaiLuuTru.BienBanXoaBoId)) _objTrangThaiLuuTru.BienBanXoaBoId = param.BienBan.Id;

                        // PDF 
                        string signedPdfPath = Path.Combine(newSignedPdfFolder, newPdfFileName);
                        byte[] bytePDF = DataHelper.StringToByteArray(@param.DataPDF);
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        System.IO.File.WriteAllBytes(signedPdfPath, _objTrangThaiLuuTru.PdfDaKy);

                        //xml
                        string signedXmlPath = Path.Combine(newSignedXmlFolder, newXmlFileName);
                        //string xmlDeCode = DataHelper.Base64Decode(@param.DataXML);
                        //System.IO.File.WriteAllText(signedXmlPath, xmlDeCode);
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        //_objTrangThaiLuuTru.XMLDaKy = Encoding.UTF8.GetBytes(@param.DataXML);
                        await this.UpdateTrangThaiLuuFileBBXB(_objTrangThaiLuuTru);

                        param.BienBan.FileDaKy = newPdfFileName;
                        if (param.TypeKy == 10)
                            param.BienBan.NgayKyBenA = DateTime.Now;
                        else if (param.TypeKy == 11)
                            param.BienBan.NgayKyBenB = DateTime.Now;
                        else return false;

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

                        if (param.TypeKy == 10)
                            _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaGuiKH;
                        else
                            _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.KHDaKy;
                        await this.UpdateAsync(_objHDDT);

                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [Obsolete]
        public async Task<bool> SendEmail(HoaDonDienTuViewModel hddt, string TenNguoiNhan = "", string ToMail = "")
        {
            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
                string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{hddt.HoaDonDienTuId}";
                string pdfFilePath = string.Empty;
                if (hddt.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh)
                {
                    pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{hddt.FileDaKy}");
                }
                else
                {
                    KetQuaConvertPDF convertPDF = await ConvertHoaDonToFilePDF(hddt);
                    pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FilePDF);
                }

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

                if (await SendEmailAsync(ToMail ?? hddt.EmailNguoiNhanHD, messageTitle, messageBody, pdfFilePath))
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = hddt.MauSo,
                        KyHieu = hddt.KyHieu,
                        So = hddt.SoHoaDon,
                        Ngay = hddt.NgayHoaDon,
                        TrangThaiGuiEmail = TrangThaiGuiEmail.DaGui,
                        LoaiEmail = LoaiEmail.ThongBaoPhatHanhHoaDon,
                        EmailNguoiNhan = ToMail ?? hddt.EmailNguoiNhanHD,
                        TenNguoiNhan = TenNguoiNhan ?? hddt.HoTenNguoiNhanHD ?? string.Empty,
                        TieuDeEmail = messageTitle,
                        RefId = hddt.HoaDonDienTuId,
                        RefType = RefType.HoaDonDienTu
                    });
                }
                else
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.GuiLoi;

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = hddt.MauSo,
                        KyHieu = hddt.KyHieu,
                        So = hddt.SoHoaDon,
                        Ngay = hddt.NgayHoaDon,
                        TrangThaiGuiEmail = TrangThaiGuiEmail.GuiLoi,
                        LoaiEmail = LoaiEmail.ThongBaoPhatHanhHoaDon,
                        EmailNguoiNhan = ToMail ?? hddt.EmailNguoiNhanHD,
                        TenNguoiNhan = TenNguoiNhan ?? hddt.HoTenNguoiNhanHD ?? string.Empty,
                        TieuDeEmail = messageTitle,
                        RefId = hddt.HoaDonDienTuId,
                        RefType = RefType.HoaDonDienTu
                    });
                }

                var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == hddt.HoaDonDienTuId);
                _db.Entry<HoaDonDienTu>(entity).CurrentValues.SetValues(_objHDDT);
                return await _db.SaveChangesAsync() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        [Obsolete]
        private async Task<bool> SendEmailAsync(string toMail, string subject, string message, string fileUrl = null, string cc = "", string bcc = "")
        {
            try
            {
                var _configuration = await _TuyChonService.GetAllAsync();

                string fromMail = _configuration.Where(x => x.Ma == "TenDangNhapEmail").Select(x => x.GiaTri).FirstOrDefault();
                string fromName = _configuration.Where(x => x.Ma == "TenNguoiGui").Select(x => x.GiaTri).FirstOrDefault();
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
                List<string> ccs = !string.IsNullOrEmpty(CC) ? CC.Split(',', ';').Distinct().ToList() : new List<string>();
                List<string> bccs = !string.IsNullOrEmpty(BCC) ? BCC.Split(',', ';').Distinct().ToList() : new List<string>();

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
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        [Obsolete]
        public async Task<bool> SendEmailAsync(ParamsSendMail @params)
        {
            try
            {
                var hddt = await GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                var bbxb = await GetBienBanXoaBoHoaDon(@params.HoaDon.HoaDonDienTuId);
                BienBanDieuChinh bbdc = null;

                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = string.Empty;
                string assetsFolder = string.Empty;
                if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                    loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                    loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonXoaBo);
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.BienBanDieuChinh);

                if (!string.IsNullOrEmpty(loaiNghiepVu)) assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{hddt.HoaDonDienTuId}";
                string pdfFilePath = string.Empty;
                if (hddt.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh)
                {
                    if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                        pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{hddt.FileDaKy}");
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                    {
                        if (hddt.TrangThaiBienBanXoaBo > (int)TrangThaiBienBanXoaBo.ChuaKy)
                            pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{bbxb.FileDaKy}");
                        else
                        {
                            var convertPDF = await ConvertBienBanXoaHoaDon(bbxb);
                            pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FilePDF);
                        }
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    {
                        bbdc = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == @params.BienBanDieuChinhId);
                        assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{@params.BienBanDieuChinhId}";
                        pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{bbdc.FileDaKy}");
                    }
                    else pdfFilePath = string.Empty;
                }
                else
                {
                    var convertPDF = await ConvertHoaDonToFilePDF(hddt);
                    pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FilePDF);
                }

                var banMauEmail = _mp.Map<ConfigNoiDungEmailViewModel>(await _db.ConfigNoiDungEmails.Where(x => x.LoaiEmail == @params.LoaiEmail).FirstOrDefaultAsync());

                var salerVM = await _HoSoHDDTService.GetDetailAsync();

                string messageTitle = banMauEmail.TieuDeEmail;
                messageTitle = messageTitle.Replace("##tendonvi##", salerVM.TenDonVi);
                messageTitle = messageTitle.Replace("##loaihoadon##", @params.HoaDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");
                messageTitle = messageTitle.Replace("##so##", hddt.SoHoaDon);
                messageTitle = messageTitle.Replace("##tenkhachhang##", hddt.TenKhachHang);

                string messageBody = banMauEmail.NoiDungEmail;
                string TenNguoiNhan = !string.IsNullOrEmpty(@params.TenNguoiNhan) ? @params.TenNguoiNhan : (@params.HoaDon.HoTenNguoiNhanHD ?? string.Empty);
                messageBody = messageBody.Replace("##tendonvi##", salerVM.TenDonVi);
                messageBody = messageBody.Replace("##loaihoadon##", @params.HoaDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");
                messageBody = messageBody.Replace("##tennguoinhan##", TenNguoiNhan);
                messageBody = messageBody.Replace("##tenkhachhang##", TenNguoiNhan);
                messageBody = messageBody.Replace("##so##", string.IsNullOrEmpty(@params.HoaDon.SoHoaDon) ? "<Chưa cấp số>" : @params.HoaDon.SoHoaDon);
                messageBody = messageBody.Replace("##mauso##", @params.HoaDon.MauSo);
                messageBody = messageBody.Replace("##kyhieu##", @params.HoaDon.KyHieu);
                messageBody = messageBody.Replace("##matracuu##", @params.HoaDon.MaTraCuu);
                messageBody = messageBody.Replace("##link##", @params.Link);

                if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                {
                    messageBody = messageBody.Replace("##lydohuy##", bbxb.LyDoXoaBo);
                    messageBody = messageBody.Replace("##ngayhoadon##", hddt.NgayHoaDon.Value.ToString("dd/MM/yyyy"));
                    messageBody = messageBody.Replace("##tongtien##", hddt.TongTienThanhToan.Value.ToString());
                    messageBody = messageBody.Replace("##duongdanbienban##", @params.Link + "/xem-chi-tiet-bbxb/" + bbxb.Id);
                }
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                {
                    messageBody = messageBody.Replace("##lydoxoahoadon##", @params.HoaDon.LyDoXoaBo);
                }
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                {
                    messageBody = messageBody.Replace("##lydodieuchinh##", bbdc.LyDoDieuChinh);
                    messageBody = messageBody.Replace("##ngayhoadon##", hddt.NgayHoaDon.Value.ToString("dd/MM/yyyy"));
                    messageBody = messageBody.Replace("##tongtien##", hddt.TongTienThanhToan.Value.ToString());
                    messageBody = messageBody.Replace("##duongdanbienban##", @params.Link + "/xem-chi-tiet-bbdc/" + bbdc.BienBanDieuChinhId);
                }

                var _objHDDT = await this.GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                if (await SendEmailAsync(@params.ToMail, messageTitle, messageBody, pdfFilePath, @params.CC, @params.BCC))
                {
                    if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                    {
                        _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;
                        _objHDDT.KhachHangDaNhan = true;
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                        _objHDDT.DaGuiThongBaoXoaBoHoaDon = true;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                        _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChoKHKy;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    {
                        bbdc.TrangThaiBienBan = (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChoKhachHangKy;
                    }

                    if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                    {
                        var modelNK = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                            NgayGio = DateTime.Now,
                            KhachHangId = _objHDDT.KhachHangId,
                            LoaiThaoTac = (int)LoaiThaoTac.GuiThongBaoPhatHanhHoaDon,
                            MoTa = "Đã gửi thông báo phát hành hóa đơn " + _objHDDT.SoHoaDon ?? string.Empty + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            HasError = false,
                            ErrorMessage = "",
                            DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                        };
                        await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                    {
                        var modelNK = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                            NgayGio = DateTime.Now,
                            KhachHangId = _objHDDT.KhachHangId,
                            LoaiThaoTac = (int)LoaiThaoTac.GuiThongBaoXoaBoHoaDon,
                            MoTa = "Đã gửi thông báo xóa bỏ hóa đơn " + _objHDDT.SoHoaDon ?? string.Empty + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            HasError = false,
                            ErrorMessage = "",
                            DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                        };
                        await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                    {
                        var modelNK = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                            NgayGio = DateTime.Now,
                            KhachHangId = _objHDDT.KhachHangId,
                            LoaiThaoTac = (int)LoaiThaoTac.GuiThongBaoXoaBoHoaDon,
                            MoTa = "Đã gửi hóa đơn " + _objHDDT.SoHoaDon ?? string.Empty + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            HasError = false,
                            ErrorMessage = "",
                            DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                        };
                        await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    {
                        var modelNK = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                            NgayGio = DateTime.Now,
                            KhachHangId = _objHDDT.KhachHangId,
                            LoaiThaoTac = (int)LoaiThaoTac.GuiThongBaoDieuChinhHoaDon,
                            MoTa = "Đã gửi thông báo điều chỉnh hóa đơn " + _objHDDT.SoHoaDon ?? string.Empty + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            HasError = false,
                            ErrorMessage = "",
                            DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                        };
                        await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    }
                    else
                    {
                        var modelNK = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = _objHDDT.HoaDonDienTuId,
                            NgayGio = DateTime.Now,
                            KhachHangId = _objHDDT.KhachHangId,
                            LoaiThaoTac = _objHDDT.SoHoaDon != "" ? (int)LoaiThaoTac.GuiHoaDon : (int)LoaiThaoTac.GuiHoaDonNhap,
                            MoTa = "Đã gửi hóa đơn " + _objHDDT.SoHoaDon ?? "<Chưa cấp số>" + " cho khách hàng " + TenNguoiNhan + ", ngày giờ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            HasError = false,
                            ErrorMessage = "",
                            DiaChiIp = NhatKyThaoTacHoaDonHelper.GetLocalIPAddress()
                        };
                        await ThemNhatKyThaoTacHoaDonAsync(modelNK);
                    }

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = hddt.MauSo,
                        KyHieu = hddt.KyHieu,
                        So = hddt.SoHoaDon,
                        Ngay = hddt.NgayHoaDon,
                        TrangThaiGuiEmail = TrangThaiGuiEmail.DaGui,
                        LoaiEmail = (LoaiEmail)@params.LoaiEmail,
                        EmailNguoiNhan = @params.ToMail,
                        TenNguoiNhan = TenNguoiNhan,
                        TieuDeEmail = messageTitle,
                        RefId = hddt.HoaDonDienTuId,
                        RefType = RefType.HoaDonDienTu
                    });

                    await this.UpdateAsync(_objHDDT);
                    return true;
                }
                else
                {
                    if (!@params.LoaiEmail.HasValue || @params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
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
                    }

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = hddt.MauSo,
                        KyHieu = hddt.KyHieu,
                        So = hddt.SoHoaDon,
                        Ngay = hddt.NgayHoaDon,
                        TrangThaiGuiEmail = TrangThaiGuiEmail.GuiLoi,
                        LoaiEmail = (LoaiEmail)@params.LoaiEmail,
                        EmailNguoiNhan = @params.ToMail,
                        TenNguoiNhan = TenNguoiNhan,
                        TieuDeEmail = messageTitle,
                        RefId = hddt.HoaDonDienTuId,
                        RefType = RefType.HoaDonDienTu
                    });

                    await UpdateAsync(_objHDDT);
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
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
            var query = from bbxb in _db.BienBanXoaBos
                        join hddt in _db.HoaDonDienTus on bbxb.HoaDonDienTuId equals hddt.HoaDonDienTuId
                        where bbxb.HoaDonDienTuId == HoaDonDienTuId
                        select new BienBanXoaBoViewModel
                        {
                            Id = bbxb.Id,
                            HoaDonDienTuId = bbxb.HoaDonDienTuId,
                            NgayBienBan = bbxb.NgayBienBan,
                            SoBienBan = bbxb.SoBienBan,
                            KhachHangId = bbxb.KhachHangId,
                            ThongTu = bbxb.ThongTu,
                            TenKhachHang = bbxb.TenKhachHang,
                            DiaChi = bbxb.DiaChi,
                            MaSoThue = bbxb.MaSoThue,
                            SoDienThoai = bbxb.SoDienThoai,
                            DaiDien = bbxb.DaiDien,
                            ChucVu = bbxb.ChucVu,
                            TenCongTyBenA = bbxb.TenCongTyBenA,
                            DiaChiBenA = bbxb.DiaChiBenA,
                            MaSoThueBenA = bbxb.MaSoThueBenA,
                            SoDienThoaiBenA = bbxb.SoDienThoaiBenA,
                            DaiDienBenA = bbxb.DaiDienBenA,
                            ChucVuBenA = bbxb.ChucVuBenA,
                            NgayKyBenA = bbxb.NgayKyBenA,
                            LyDoXoaBo = bbxb.LyDoXoaBo,
                            FileDaKy = bbxb.FileDaKy,
                            FileChuaKy = bbxb.FileChuaKy,
                            XMLChuaKy = bbxb.XMLChuaKy,
                            XMLDaKy = bbxb.XMLDaKy,
                            TenNguoiNhan = bbxb.TenNguoiNhan,
                            EmailNguoiNhan = bbxb.EmailNguoiNhan,
                            SoDienThoaiNguoiNhan = bbxb.SoDienThoaiNguoiNhan,
                            HoaDonDienTu = new HoaDonDienTuViewModel
                            {
                                HoaDonDienTuId = hddt.HoaDonDienTuId,
                                SoHoaDon = hddt.SoHoaDon,
                                NgayHoaDon = hddt.NgayHoaDon,
                                MauSo = hddt.MauSo,
                                KyHieu = hddt.KyHieu
                            },
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> CapNhatBienBanXoaBoHoaDon(BienBanXoaBoViewModel bb)
        {
            var entity = await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == bb.Id);
            _db.Entry<BienBanXoaBo>(entity).CurrentValues.SetValues(bb);
            if (await _db.SaveChangesAsync() > 0)
            {
                var entityHD = await GetByIdAsync(entity.HoaDonDienTuId);
                entityHD.LyDoXoaBo = entity.LyDoXoaBo;
                return await UpdateAsync(entityHD);
            }

            return false;
        }

        [Obsolete]
        public async Task<BienBanXoaBoViewModel> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon @params)
        {
            var entity = _mp.Map<BienBanXoaBo>(@params.Data);

            var khachHang = _mp.Map<DoiTuongViewModel>(await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == @params.Data.KhachHangId));
            entity.TenNguoiNhan = khachHang.HoTenNguoiNhanHD;
            entity.EmailNguoiNhan = khachHang.EmailNguoiNhanHD;
            entity.SoDienThoaiNguoiNhan = khachHang.SoDienThoaiNguoiNhanHD;

            await _db.BienBanXoaBos.AddAsync(entity);

            var entityHD = _db.HoaDonDienTus.FirstOrDefault(x => x.HoaDonDienTuId == @params.Data.HoaDonDienTuId);
            entityHD.LyDoXoaBo = entity.LyDoXoaBo;
            entityHD.TrangThaiBienBanXoaBo = 1;
            _db.HoaDonDienTus.Update(entityHD);
            var effect = await _db.SaveChangesAsync();

            if (effect > 0)
            {
                if (@params.OptionalSendData == 1)
                {
                }
                else
                {
                    var _objHD = await this.GetByIdAsync(@params.Data.HoaDonDienTuId);
                    var _params = new ParamsSendMail
                    {
                        HoaDon = _objHD,
                        LoaiEmail = (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon
                    };

                    await this.SendEmailAsync(_params);
                }
            }

            return _mp.Map<BienBanXoaBoViewModel>(entity);
        }

        public async Task<bool> DeleteBienBanXoaHoaDon(string Id)
        {
            BienBanXoaBo entity = _db.BienBanXoaBos.FirstOrDefault(x => x.Id == Id);
            _db.BienBanXoaBos.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<BienBanXoaBoViewModel> GetBienBanXoaBoById(string Id)
        {
            return _mp.Map<BienBanXoaBoViewModel>(await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<KetQuaConvertPDF> ConvertBienBanXoaHoaDon(BienBanXoaBoViewModel bb)
        {
            var path = string.Empty;
            var pathXML = string.Empty;

            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonXoaBo);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{bb.HoaDonDienTuId}";

            if (bb != null)
            {
                var _objHD = await GetByIdAsync(bb.HoaDonDienTuId);
                var _objBB = await GetBienBanXoaBoById(bb.Id);
                if (_objHD.TrangThaiBienBanXoaBo >= 2 && !string.IsNullOrEmpty(_objBB.FileDaKy))
                {
                    return new KetQuaConvertPDF
                    {
                        FilePDF = Path.Combine(assetsFolder, $"pdf/signed/{_objBB.FileDaKy}"),
                        FileXML = Path.Combine(assetsFolder, $"xml/signed/{_objBB.XMLDaKy}"),
                    };
                }
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/HoaDonXoaBo/Bien_ban_huy_hoa_don.doc");
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


                doc.Replace("<date>", bb.NgayBienBan.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                doc.Replace("<month>", bb.NgayBienBan.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                doc.Replace("<year>", bb.NgayBienBan.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);


                doc.Replace("<reason>", _objHD.LyDoXoaBo ?? string.Empty, true, true);

                var fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/unsigned");
                var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"xml/unsigned");
                #region create folder
                if (!Directory.Exists(fullPdfFolder))
                {
                    Directory.CreateDirectory(fullPdfFolder);
                }
                else
                {
                    string[] files = Directory.GetFiles(fullPdfFolder);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }

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

                string pdfFileName = $"{Guid.NewGuid()}.pdf";
                string xmlFileName = $"{Guid.NewGuid()}.xml";

                doc.SaveToFile(Path.Combine(fullPdfFolder, $"{pdfFileName}"), FileFormat.PDF);
                await _xMLInvoiceService.CreateXMLBienBan(Path.Combine(fullXmlFolder, $"{xmlFileName}"), bb);

                path = Path.Combine(assetsFolder, $"pdf/unsigned", $"{pdfFileName}");
                pathXML = Path.Combine(assetsFolder, $"xml/unsigned", $"{xmlFileName}");
            }

            return new KetQuaConvertPDF
            {
                FilePDF = path,
                FileXML = pathXML
            };
        }

        public async Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonThayTheAsync(HoaDonThayTheParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var query = from hd in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        where hd.NgayHoaDon.Value.Date >= fromDate && hd.NgayHoaDon.Value.Date <= toDate && (((TrangThaiHoaDon)hd.TrangThai) == TrangThaiHoaDon.HoaDonThayThe)
                        orderby hd.NgayHoaDon, hd.SoHoaDon descending
                        select new HoaDonDienTuViewModel
                        {
                            Key = Guid.NewGuid().ToString(),
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                            LyDoThayThe = hd.LyDoThayThe,
                            TenHinhThucHoaDonCanThayThe = hd.LyDoThayThe.GetTenHinhThucHoaDonCanThayThe(),
                            NgayXoaBo = hd.NgayXoaBo,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            TenTrangThaiBienBanXoaBo = string.Empty,
                            TrangThai = hd.TrangThai,
                            TenTrangThaiHoaDon = hd.TrangThai.HasValue ? ((TrangThaiHoaDon)hd.TrangThai).GetDescription() : string.Empty,
                            TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                            TenTrangThaiPhatHanh = hd.TrangThaiPhatHanh.HasValue ? ((LoaiTrangThaiPhatHanh)hd.TrangThaiPhatHanh).GetDescription() : string.Empty,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                            MaTraCuu = hd.MaTraCuu,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                            NgayHoaDon = hd.NgayHoaDon,
                            SoHoaDon = hd.SoHoaDon,
                            MaCuaCQT = hd.MaCuaCQT ?? string.Empty,
                            MauSo = hd.MauSo,
                            KyHieu = hd.KyHieu,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            MaSoThue = hd.MaSoThue,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            LoaiTienId = hd.LoaiTienId,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            TongTienThanhToan = hd.TongTienThanhToanQuyDoi
                        };

            var queryXoaBo = from hd in _db.HoaDonDienTus
                             join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                             from lt in tmpLoaiTiens.DefaultIfEmpty()
                             join bbxb in _db.BienBanXoaBos on hd.HoaDonDienTuId equals bbxb.HoaDonDienTuId into tmpBienBanXoaBos
                             from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                             where hd.NgayHoaDon.Value.Date >= fromDate && hd.NgayHoaDon.Value.Date <= toDate && ((TrangThaiHoaDon)hd.TrangThai) == TrangThaiHoaDon.HoaDonXoaBo
                             select new HoaDonDienTuViewModel
                             {
                                 Key = Guid.NewGuid().ToString(),
                                 HoaDonDienTuId = hd.HoaDonDienTuId,
                                 BienBanXoaBoId = bbxb != null ? bbxb.Id : null,
                                 LyDoThayThe = string.Empty,
                                 TenHinhThucHoaDonCanThayThe = string.Empty,
                                 NgayXoaBo = hd.NgayXoaBo,
                                 LyDoXoaBo = hd.LyDoXoaBo,
                                 TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                 TenTrangThaiBienBanXoaBo = ((TrangThaiBienBanXoaBo)hd.TrangThaiBienBanXoaBo).GetDescription(),
                                 TrangThai = hd.TrangThai,
                                 TenTrangThaiHoaDon = hd.TrangThai.HasValue ? ((TrangThaiHoaDon)hd.TrangThai).GetDescription() : string.Empty,
                                 TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                                 TenTrangThaiPhatHanh = hd.TrangThaiPhatHanh.HasValue ? ((LoaiTrangThaiPhatHanh)hd.TrangThaiPhatHanh).GetDescription() : string.Empty,
                                 TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                 TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                                 MaTraCuu = hd.MaTraCuu,
                                 LoaiHoaDon = hd.LoaiHoaDon,
                                 TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                 NgayHoaDon = hd.NgayHoaDon,
                                 SoHoaDon = hd.SoHoaDon,
                                 MaCuaCQT = hd.MaCuaCQT,
                                 MauSo = hd.MauSo,
                                 KyHieu = hd.KyHieu,
                                 MaKhachHang = hd.MaKhachHang,
                                 TenKhachHang = hd.TenKhachHang,
                                 MaSoThue = hd.MaSoThue,
                                 HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                                 TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                 LoaiTienId = hd.LoaiTienId,
                                 MaLoaiTien = lt != null ? lt.Ma : "VND",
                                 TongTienThanhToan = hd.TongTienThanhToanQuyDoi
                             };

            if (@params.LoaiTrangThaiPhatHanh != LoaiTrangThaiPhatHanh.TatCa)
            {
                query = query.Where(x => x.TrangThaiPhatHanh.HasValue && (LoaiTrangThaiPhatHanh)x.TrangThaiPhatHanh == @params.LoaiTrangThaiPhatHanh);
            }

            if (@params.LoaiTrangThaiGuiHoaDon != LoaiTrangThaiGuiHoaDon.TatCa)
            {
                query = query.Where(x => x.TrangThaiGuiHoaDon.HasValue && (LoaiTrangThaiGuiHoaDon)x.TrangThaiGuiHoaDon == @params.LoaiTrangThaiGuiHoaDon);
            }

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
            }

            #region Filter and Sort
            if (@params.FilterColumns != null && @params.FilterColumns.Any())
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in @params.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        case nameof(@params.Filter.SoHoaDon):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.SoHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.MauSo):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MauSo, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.KyHieu):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.KyHieu, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.MaKhachHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MaKhachHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenKhachHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TenKhachHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.DiaChi):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.DiaChi, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.MaSoThue):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MaSoThue, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.HoTenNguoiMuaHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.HoTenNguoiMuaHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenNhanVienBanHang):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TenNhanVienBanHang, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TongTienThanhToan):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TongTienThanhToan, filterCol, FilterValueType.Decimal);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                switch (@params.SortKey)
                {
                    case nameof(@params.Filter.TenTrangThaiHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiHoaDon);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiHoaDon);
                        }
                        break;
                    case nameof(@params.Filter.TenHinhThucHoaDonCanThayThe):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenHinhThucHoaDonCanThayThe);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenHinhThucHoaDonCanThayThe);
                        }
                        break;
                    case nameof(@params.Filter.NgayXoaBo):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.NgayXoaBo);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.NgayXoaBo);
                        }
                        break;
                    case nameof(@params.Filter.LyDoXoaBo):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.LyDoXoaBo);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.LyDoXoaBo);
                        }
                        break;
                    case nameof(@params.Filter.TenTrangThaiBienBanXoaBo):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiBienBanXoaBo);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiBienBanXoaBo);
                        }
                        break;
                    case nameof(@params.Filter.TenTrangThaiPhatHanh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiPhatHanh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiPhatHanh);
                        }
                        break;
                    case nameof(@params.Filter.TenTrangThaiGuiHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiGuiHoaDon);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiGuiHoaDon);
                        }
                        break;
                    case nameof(@params.Filter.MaTraCuu):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaTraCuu);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaTraCuu);
                        }
                        break;
                    case nameof(@params.Filter.TenLoaiHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenLoaiHoaDon);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenLoaiHoaDon);
                        }
                        break;
                    case nameof(@params.Filter.NgayHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.NgayHoaDon);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.NgayHoaDon);
                        }
                        break;
                    case nameof(@params.Filter.SoHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.SoHoaDon);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SoHoaDon);
                        }
                        break;
                    case nameof(@params.Filter.MauSo):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MauSo);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MauSo);
                        }
                        break;
                    case nameof(@params.Filter.KyHieu):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.KyHieu);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.KyHieu);
                        }
                        break;
                    case nameof(@params.Filter.MaKhachHang):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaKhachHang);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaKhachHang);
                        }
                        break;
                    case nameof(@params.Filter.TenKhachHang):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenKhachHang);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenKhachHang);
                        }
                        break;
                    case nameof(@params.Filter.MaSoThue):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaSoThue);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaSoThue);
                        }
                        break;
                    case nameof(@params.Filter.HoTenNguoiMuaHang):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.HoTenNguoiMuaHang);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.HoTenNguoiMuaHang);
                        }
                        break;
                    case nameof(@params.Filter.TenNhanVienBanHang):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenNhanVienBanHang);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenNhanVienBanHang);
                        }
                        break;
                    case nameof(@params.Filter.MaLoaiTien):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaLoaiTien);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaLoaiTien);
                        }
                        break;
                    case nameof(@params.Filter.TongTienThanhToan):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TongTienThanhToan);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TongTienThanhToan);
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion

            var listThayThe = await query.ToListAsync();
            var listXoaBo = await queryXoaBo.ToListAsync();

            foreach (var item in listThayThe)
            {
                if (!string.IsNullOrEmpty(item.ThayTheChoHoaDonId) && listXoaBo.Any(x => x.HoaDonDienTuId == item.ThayTheChoHoaDonId))
                {
                    item.Children = new List<HoaDonDienTuViewModel>();

                    var hoaDonXoaBos = listXoaBo.Where(x => x.HoaDonDienTuId == item.ThayTheChoHoaDonId).ToList();
                    Queue<HoaDonDienTuViewModel> queue = new Queue<HoaDonDienTuViewModel>(hoaDonXoaBos);
                    while (queue.Count() != 0)
                    {
                        var dequeue = queue.Dequeue();
                        item.Children.Insert(0, dequeue);
                        if (!string.IsNullOrEmpty(dequeue.ThayTheChoHoaDonId) && listXoaBo.Any(x => x.HoaDonDienTuId == dequeue.ThayTheChoHoaDonId))
                        {
                            var hoaDonXoaBoInQueues = listXoaBo.Where(x => x.HoaDonDienTuId == dequeue.ThayTheChoHoaDonId).ToList();
                            foreach (var child in hoaDonXoaBoInQueues)
                            {
                                queue.Enqueue(child);
                            }
                        }
                    }
                }
            }

            return PagedList<HoaDonDienTuViewModel>
                    .CreateAsyncWithList(listThayThe, @params.PageNumber, @params.PageSize);
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

        [Obsolete]
        public async Task<bool> XoaBoHoaDon(ParamXoaBoHoaDon @params)
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
                        _objHD.HoTenNguoiNhanHD = @params.HoaDon.HoTenNguoiNhanHD;
                        _objHD.EmailNguoiNhanHD = @params.HoaDon.EmailNguoiNhanHD;
                        _objHD.SoDienThoaiNguoiNhanHD = @params.HoaDon.SoDienThoaiNguoiNhanHD;

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

            return false;
        }

        public async Task<PagedList<BangKeHoaDonDieuChinh>> GetAllPagingHoaDonDieuChinhAsync(HoaDonDieuChinhParams @params)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);

            var queryLeft = from hdbdc in _db.HoaDonDienTus
                            join bbdc in _db.BienBanDieuChinhs on hdbdc.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId
                            join hddc in _db.HoaDonDienTus on bbdc.HoaDonDieuChinhId equals hddc.HoaDonDienTuId into tmpHoaDonDieuChinhs
                            from hddc in tmpHoaDonDieuChinhs.DefaultIfEmpty()
                            join kh in _db.DoiTuongs on hdbdc.KhachHangId equals kh.DoiTuongId into tmpDoiTuongs
                            from kh in tmpDoiTuongs.DefaultIfEmpty()
                            join lt in _db.LoaiTiens on hddc.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                            from lt in tmpLoaiTiens.DefaultIfEmpty()
                            select new BangKeHoaDonDieuChinh
                            {
                                HoaDonBiDieuChinhId = hdbdc.HoaDonDienTuId,
                                MaTraCuuBiDieuChinh = hdbdc.MaTraCuu,
                                LoaiHoaDonBiDieuChinh = hdbdc.LoaiHoaDon,
                                TenLoaiHoaDonBiDieuChinh = ((LoaiHoaDon)hdbdc.LoaiHoaDon).GetDescription(),
                                NgayHoaDonBiDieuChinh = hdbdc.NgayHoaDon,
                                SoHoaDonBiDieuChinh = hdbdc.SoHoaDon,
                                MaCQTCapBiDieuChinh = hdbdc.MaCuaCQT ?? string.Empty,
                                MauSoBiDieuChinh = hdbdc.MauSo,
                                KyHieuBiDieuChinh = hdbdc.KyHieu,

                                BienBanDieuChinhId = bbdc.BienBanDieuChinhId,
                                TenNguoiNhanBienBan = kh.HoTenNguoiNhanHD,
                                EmailNguoiNhanBienBan = kh.EmailNguoiNhanHD,
                                SoDienThoaiNguoiNhanBienBan = kh.SoDienThoaiNguoiNhanHD,
                                LyDoDieuChinhBienBan = bbdc.LyDoDieuChinh,

                                HoaDonDieuChinhId = hddc.HoaDonDienTuId,
                                TrangThaiHoaDonDieuChinh = hddc.TrangThai,
                                TenTrangThaiHoaDonDieuChinh = (hddc.TrangThai != null) ? ((TrangThaiHoaDon)hddc.TrangThai).GetDescription() : string.Empty,
                                TenHinhThucHoaDonBiDieuChinh = hddc != null ? hddc.LyDoDieuChinh.GetTenHinhThucHoaDonBiDieuChinh() : string.Empty,
                                LyDoDieuChinh = hdbdc != null ? hddc.LyDoDieuChinh.GetNoiDungLyDoDieuChinh() : string.Empty,
                                LoaiDieuChinh = hdbdc != null ? hddc.LoaiDieuChinh : null,
                                TenLoaiDieuChinh = hddc != null ? (hddc.LoaiDieuChinh != null ? ((LoaiDieuChinhHoaDon)hddc.LoaiDieuChinh).GetDescription() : string.Empty) : string.Empty,
                                TrangThaiBienBanDieuChinh = bbdc.TrangThaiBienBan,
                                TenTrangThaiBienBanDieuChinh = (bbdc.TrangThaiBienBan != null) ? ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription() : string.Empty,
                                MaTraCuuDieuChinh = hddc != null ? hddc.MaTraCuu : string.Empty,
                                LoaiHoaDonDieuChinh = hddc.LoaiHoaDon,
                                TenLoaiHoaDonDieuChinh = hddc != null ? ((LoaiHoaDon)hddc.LoaiHoaDon).GetDescription() : string.Empty,
                                NgayHoaDonDieuChinh = hddc.NgayHoaDon.Value,
                                SoHoaDonDieuChinh = hddc != null ? hddc.SoHoaDon : string.Empty,
                                MaCQTCapDieuChinh = hddc != null ? (hddc.MaCuaCQT ?? string.Empty) : string.Empty,
                                MauSoDieuChinh = hddc != null ? hddc.MauSo : string.Empty,
                                KyHieuDieuChinh = hddc != null ? hddc.KyHieu : string.Empty,
                                MaKhachHangDieuChinh = hddc != null ? hddc.MaKhachHang : string.Empty,
                                TenKhachHangDieuChinh = hddc != null ? hddc.TenKhachHang : string.Empty,
                                MaSoThueDieuChinh = hddc != null ? hddc.MaSoThue : string.Empty,
                                NguoiMuaHangDieuChinh = hddc != null ? hddc.HoTenNguoiMuaHang : string.Empty,
                                NhanVienBanHangDieuChinh = hddc != null ? hddc.TenNhanVienBanHang : string.Empty,
                                LoaiTienId = hddc != null ? hddc.LoaiTienId : string.Empty,
                                MaLoaiTien = lt != null ? lt.Ma : "VND",
                                IsVND = lt == null || (lt.Ma == "VND"),
                                TongTienThanhToan = hddc != null ? hddc.TongTienThanhToanQuyDoi : 0,
                                TrangThaiPhatHanhDieuChinh = hddc.TrangThaiPhatHanh,
                                TenTrangThaiPhatHanhDieuChinh = hddc.TrangThaiPhatHanh.HasValue ? ((LoaiTrangThaiPhatHanh)hddc.TrangThaiPhatHanh).GetDescription() : string.Empty,
                                TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                   where tldk.NghiepVuId == (hddc != null ? hddc.HoaDonDienTuId : null)
                                                   orderby tldk.CreatedDate
                                                   select new TaiLieuDinhKemViewModel
                                                   {
                                                       TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                       NghiepVuId = tldk.NghiepVuId,
                                                       LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                       TenGoc = tldk.TenGoc,
                                                       TenGuid = tldk.TenGuid,
                                                       CreatedDate = tldk.CreatedDate,
                                                       Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{(hddc != null ? hddc.HoaDonDienTuId : null)}\FileAttach", tldk.TenGuid),
                                                       Status = tldk.Status
                                                   })
                                                    .ToList(),
                            };

            var hoaDonDieuChinhIds = await queryLeft.Where(x => !string.IsNullOrEmpty(x.HoaDonDieuChinhId)).Select(x => x.HoaDonDieuChinhId).ToListAsync();

            var queryRight = from hddc in _db.HoaDonDienTus
                             join bbdc in _db.BienBanDieuChinhs on hddc.HoaDonDienTuId equals bbdc.HoaDonDieuChinhId into tmpHoaDonDieuChinhs
                             from bbdc in tmpHoaDonDieuChinhs.DefaultIfEmpty()
                             join lt in _db.LoaiTiens on hddc.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                             from lt in tmpLoaiTiens.DefaultIfEmpty()
                             join hdbdc in _db.HoaDonDienTus on hddc.DieuChinhChoHoaDonId equals hdbdc.HoaDonDienTuId into tmpHoaDonBiDieuChinhs
                             from hdbdc in tmpHoaDonBiDieuChinhs.DefaultIfEmpty()
                             join kh in _db.DoiTuongs on hdbdc.KhachHangId equals kh.DoiTuongId into tmpDoiTuongs
                             from kh in tmpDoiTuongs.DefaultIfEmpty()
                             where (TrangThaiHoaDon)hddc.TrangThai.GetValueOrDefault() == TrangThaiHoaDon.HoaDonDieuChinh && (string.IsNullOrEmpty(hddc.DieuChinhChoHoaDonId) || (!string.IsNullOrEmpty(hddc.DieuChinhChoHoaDonId) && !hoaDonDieuChinhIds.Contains(hddc.HoaDonDienTuId)))
                             select new BangKeHoaDonDieuChinh
                             {
                                 HoaDonBiDieuChinhId = hdbdc != null ? hdbdc.HoaDonDienTuId : null,
                                 MaTraCuuBiDieuChinh = hdbdc != null ? hdbdc.MaTraCuu : string.Empty,
                                 LoaiHoaDonBiDieuChinh = hdbdc.LoaiHoaDon,
                                 TenLoaiHoaDonBiDieuChinh = hdbdc != null ? (((LoaiHoaDon)hdbdc.LoaiHoaDon).GetDescription()) : string.Empty,
                                 NgayHoaDonBiDieuChinh = hdbdc != null ? hdbdc.NgayHoaDon : null,
                                 SoHoaDonBiDieuChinh = hdbdc != null ? hdbdc.SoHoaDon : string.Empty,
                                 MauSoBiDieuChinh = hdbdc != null ? hdbdc.MauSo : string.Empty,
                                 KyHieuBiDieuChinh = hdbdc != null ? hdbdc.KyHieu : string.Empty,

                                 BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : null,
                                 TenNguoiNhanBienBan = kh != null ? kh.HoTenNguoiNhanHD : null,
                                 EmailNguoiNhanBienBan = kh != null ? kh.EmailNguoiNhanHD : null,
                                 SoDienThoaiNguoiNhanBienBan = kh != null ? kh.SoDienThoaiNguoiNhanHD : null,
                                 LyDoDieuChinhBienBan = bbdc != null ? bbdc.LyDoDieuChinh : null,

                                 HoaDonDieuChinhId = hddc.HoaDonDienTuId,
                                 TrangThaiHoaDonDieuChinh = hddc.TrangThai,
                                 TenTrangThaiHoaDonDieuChinh = (hddc.TrangThai != null) ? ((TrangThaiHoaDon)hddc.TrangThai).GetDescription() : string.Empty,
                                 TenHinhThucHoaDonBiDieuChinh = hddc.LyDoDieuChinh.GetTenHinhThucHoaDonBiDieuChinh(),
                                 LyDoDieuChinh = hddc.LyDoDieuChinh.GetNoiDungLyDoDieuChinh(),
                                 LoaiDieuChinh = hddc.LoaiDieuChinh,
                                 TenLoaiDieuChinh = (hddc.LoaiDieuChinh != null) ? ((LoaiDieuChinhHoaDon)hddc.LoaiDieuChinh).GetDescription() : string.Empty,
                                 TrangThaiBienBanDieuChinh = bbdc == null ? 0 : bbdc.TrangThaiBienBan,
                                 TenTrangThaiBienBanDieuChinh = bbdc == null ? LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan.GetDescription() : (bbdc.TrangThaiBienBan != null ? ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription() : string.Empty),
                                 MaTraCuuDieuChinh = hddc.MaTraCuu,
                                 LoaiHoaDonDieuChinh = hddc.LoaiHoaDon,
                                 TenLoaiHoaDonDieuChinh = ((LoaiHoaDon)hddc.LoaiHoaDon).GetDescription(),
                                 NgayHoaDonDieuChinh = hddc.NgayHoaDon,
                                 SoHoaDonDieuChinh = hddc.SoHoaDon,
                                 MauSoDieuChinh = hddc.MauSo,
                                 KyHieuDieuChinh = hddc.KyHieu,
                                 MaKhachHangDieuChinh = hddc.MaKhachHang,
                                 TenKhachHangDieuChinh = hddc.TenKhachHang,
                                 MaSoThueDieuChinh = hddc.MaSoThue,
                                 NguoiMuaHangDieuChinh = hddc.HoTenNguoiMuaHang,
                                 NhanVienBanHangDieuChinh = hddc.TenNhanVienBanHang,
                                 LoaiTienId = hddc.LoaiTienId,
                                 MaLoaiTien = lt != null ? lt.Ma : "VND",
                                 IsVND = lt == null || (lt.Ma == "VND"),
                                 TongTienThanhToan = hddc.TongTienThanhToanQuyDoi,
                                 TrangThaiPhatHanhDieuChinh = hddc.TrangThaiPhatHanh,
                                 TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                    where tldk.NghiepVuId == hddc.HoaDonDienTuId
                                                    orderby tldk.CreatedDate
                                                    select new TaiLieuDinhKemViewModel
                                                    {
                                                        TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                        NghiepVuId = tldk.NghiepVuId,
                                                        LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                        TenGoc = tldk.TenGoc,
                                                        TenGuid = tldk.TenGuid,
                                                        CreatedDate = tldk.CreatedDate,
                                                        Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{hddc.HoaDonDienTuId}\FileAttach", tldk.TenGuid),
                                                        Status = tldk.Status
                                                    })
                                                    .ToList(),
                             };

            var query = queryLeft.Union(queryRight)
                .OrderBy(x => x.NgayHoaDonBiDieuChinh.HasValue == true ? x.NgayHoaDonBiDieuChinh : x.NgayHoaDonDieuChinh).OrderByDescending(x => x.SoHoaDonBiDieuChinh)
                .AsQueryable();

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                DateTime fromDate = DateTime.Parse(@params.FromDate);
                DateTime toDate = DateTime.Parse(@params.ToDate);
                query = query.Where(x => DateTime.Parse((x.NgayHoaDonBiDieuChinh.HasValue == true ? x.NgayHoaDonBiDieuChinh : x.NgayHoaDonDieuChinh).Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                        DateTime.Parse((x.NgayHoaDonBiDieuChinh.HasValue == true ? x.NgayHoaDonBiDieuChinh : x.NgayHoaDonDieuChinh).Value.ToString("yyyy-MM-dd")) <= toDate);
            }

            if (@params.LoaiTrangThaiHoaDonDieuChinh != LoaiTrangThaiHoaDonDieuChinh.TatCa)
            {
                if (@params.LoaiTrangThaiHoaDonDieuChinh == LoaiTrangThaiHoaDonDieuChinh.ChuaLap)
                {
                    query = query.Where(x => string.IsNullOrEmpty(x.HoaDonDieuChinhId));
                }
                else if (@params.LoaiTrangThaiHoaDonDieuChinh == LoaiTrangThaiHoaDonDieuChinh.DaLap)
                {
                    query = query.Where(x => !string.IsNullOrEmpty(x.HoaDonDieuChinhId));
                }
                else
                {
                    query = query.Where(x => x.LoaiDieuChinh == (int)@params.LoaiTrangThaiHoaDonDieuChinh);
                }
            }

            if (@params.LoaiTrangThaiPhatHanh != LoaiTrangThaiPhatHanh.TatCa)
            {
                query = query.Where(x => x.TrangThaiPhatHanhDieuChinh.HasValue && (LoaiTrangThaiPhatHanh)x.TrangThaiPhatHanhDieuChinh == @params.LoaiTrangThaiPhatHanh);
            }

            if (@params.LoaiTrangThaiBienBanDieuChinhHoaDon != LoaiTrangThaiBienBanDieuChinhHoaDon.TatCa)
            {
                query = query.Where(x => x.TrangThaiBienBanDieuChinh.HasValue && (LoaiTrangThaiBienBanDieuChinhHoaDon)x.TrangThaiBienBanDieuChinh == @params.LoaiTrangThaiBienBanDieuChinhHoaDon);
            }

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDonBiDieuChinh.ToUpper().ToTrim().Contains(keyword) || x.TenLoaiHoaDonDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSoBiDieuChinh.ToUpper().ToTrim().Contains(keyword) || x.MauSoDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieuBiDieuChinh.ToUpper().ToTrim().Contains(keyword) || x.KyHieuDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDonBiDieuChinh.ToUpper().ToTrim().Contains(keyword) || x.SoHoaDonDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThueDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHangDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHangDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.NguoiMuaHangDieuChinh.ToUpper().ToTrim().Contains(keyword));
                }
            }

            #region Filter and Sort
            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                BangKeHoaDonDieuChinh model = new BangKeHoaDonDieuChinh();

                switch (@params.SortKey)
                {
                    case nameof(model.MaTraCuuBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaTraCuuBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaTraCuuBiDieuChinh);
                        }
                        break;
                    case nameof(model.TenLoaiHoaDonBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenLoaiHoaDonBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenLoaiHoaDonBiDieuChinh);
                        }
                        break;
                    case nameof(model.NgayHoaDonBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.NgayHoaDonBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.NgayHoaDonBiDieuChinh);
                        }
                        break;
                    case nameof(model.SoHoaDonBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.SoHoaDonBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SoHoaDonBiDieuChinh);
                        }
                        break;
                    case nameof(model.MauSoBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MauSoBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MauSoBiDieuChinh);
                        }
                        break;
                    case nameof(model.KyHieuBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.KyHieuBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.KyHieuBiDieuChinh);
                        }
                        break;
                    case nameof(model.TenTrangThaiHoaDonDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiHoaDonDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiHoaDonDieuChinh);
                        }
                        break;
                    case nameof(model.TenHinhThucHoaDonBiDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenHinhThucHoaDonBiDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenHinhThucHoaDonBiDieuChinh);
                        }
                        break;
                    case nameof(model.LyDoDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.LyDoDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.LyDoDieuChinh);
                        }
                        break;
                    case nameof(model.TenTrangThaiBienBanDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiBienBanDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiBienBanDieuChinh);
                        }
                        break;
                    case nameof(model.MaTraCuuDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaTraCuuDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaTraCuuDieuChinh);
                        }
                        break;
                    case nameof(model.TenLoaiHoaDonDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenLoaiHoaDonDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenLoaiHoaDonDieuChinh);
                        }
                        break;
                    case nameof(model.NgayHoaDonDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.NgayHoaDonDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.NgayHoaDonDieuChinh);
                        }
                        break;
                    case nameof(model.SoHoaDonDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.SoHoaDonDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SoHoaDonDieuChinh);
                        }
                        break;
                    case nameof(model.MauSoDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MauSoDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MauSoDieuChinh);
                        }
                        break;
                    case nameof(model.KyHieuDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.KyHieuDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.KyHieuDieuChinh);
                        }
                        break;
                    case nameof(model.MaKhachHangDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaKhachHangDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaKhachHangDieuChinh);
                        }
                        break;
                    case nameof(model.TenKhachHangDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenKhachHangDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenKhachHangDieuChinh);
                        }
                        break;
                    case nameof(model.MaSoThueDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaSoThueDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaSoThueDieuChinh);
                        }
                        break;
                    case nameof(model.NguoiMuaHangDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.NguoiMuaHangDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.NguoiMuaHangDieuChinh);
                        }
                        break;
                    case nameof(model.NhanVienBanHangDieuChinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.NhanVienBanHangDieuChinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.NhanVienBanHangDieuChinh);
                        }
                        break;
                    case nameof(model.MaLoaiTien):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MaLoaiTien);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MaLoaiTien);
                        }
                        break;
                    case nameof(model.TongTienThanhToan):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TongTienThanhToan);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TongTienThanhToan);
                        }
                        break;
                    default:
                        break;
                }
            }
            #endregion

            return await PagedList<BangKeHoaDonDieuChinh>
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

        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonXoaBoCanThayTheAsync(HoaDonThayTheParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var listHoaDonBiThayTheIds = await _db.HoaDonDienTus
                .Where(x => (TrangThaiHoaDon)x.TrangThai == TrangThaiHoaDon.HoaDonThayThe && !string.IsNullOrEmpty(x.ThayTheChoHoaDonId))
                .Select(x => x.ThayTheChoHoaDonId)
                .ToListAsync();

            var query = from hddt in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where hddt.NgayHoaDon.Value.Date >= fromDate && hddt.NgayHoaDon <= toDate &&
                        (TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonXoaBo && !listHoaDonBiThayTheIds.Contains(hddt.HoaDonDienTuId)
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            TenTrangThaiHoaDon = ((TrangThaiHoaDon)hddt.TrangThai).GetDescription(),
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = hddt.KyHieu,
                            NgayHoaDon = hddt.NgayHoaDon,
                            SoHoaDon = hddt.SoHoaDon,
                            MaCuaCQT = hddt.MaCuaCQT,
                            KhachHangId = hddt.KhachHangId,
                            MaKhachHang = hddt.MaKhachHang ?? string.Empty,
                            TenKhachHang = hddt.TenKhachHang ?? string.Empty,
                            DiaChi = hddt.DiaChi ?? string.Empty,
                            MaSoThue = hddt.MaSoThue ?? string.Empty,
                            HoTenNguoiMuaHang = hddt.HoTenNguoiMuaHang ?? string.Empty,
                            LoaiTienId = hddt.LoaiTienId,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            TongTienThanhToanQuyDoi = hddt.TongTienThanhToanQuyDoi
                        };

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
            }

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonCanDieuChinhAsync(HoaDonDieuChinhParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var query = from hddt in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _db.BienBanDieuChinhs on hddt.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpHoaDonBiDieuChinhs
                        from bbdc in tmpHoaDonBiDieuChinhs.DefaultIfEmpty()
                        join hddc in _db.HoaDonDienTus on hddt.HoaDonDienTuId equals hddc.ThayTheChoHoaDonId into tmpHoaDonDieuChinhs
                        from hddc in tmpHoaDonDieuChinhs.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where hddt.NgayHoaDon.Value.Date >= fromDate && hddt.NgayHoaDon <= toDate && bbdc == null && hddc == null && ((TrangThaiPhatHanh)hddt.TrangThaiPhatHanh == TrangThaiPhatHanh.DaPhatHanh) &&
                        ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonGoc || (TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonThayThe)
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = hddt.KyHieu,
                            NgayHoaDon = hddt.NgayHoaDon,
                            SoHoaDon = hddt.SoHoaDon,
                            MaCuaCQT = hddt.MaCuaCQT ?? string.Empty,
                            KhachHangId = hddt.KhachHangId,
                            MaKhachHang = hddt.MaKhachHang ?? string.Empty,
                            TenKhachHang = hddt.TenKhachHang ?? string.Empty,
                            DiaChi = hddt.DiaChi ?? string.Empty,
                            MaSoThue = hddt.MaSoThue ?? string.Empty,
                            HoTenNguoiMuaHang = hddt.HoTenNguoiMuaHang ?? string.Empty,
                            LoaiTienId = hddt.LoaiTienId,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            MaTraCuu = hddt.MaTraCuu,
                            TongTienThanhToanQuyDoi = hddt.TongTienThanhToanQuyDoi
                        };

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
            }

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<bool> GetStatusDaThayTheHoaDon(string HoaDonId)
        {
            return await _db.HoaDonDienTus.AnyAsync(x => x.ThayTheChoHoaDonId == HoaDonId);
        }

        public string XemHoaDonDongLoat(List<string> listPdfFiles)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/merged";

            string outPutFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            for (int i = 0; i < listPdfFiles.Count; i++)
            {
                listPdfFiles[i] = Path.Combine(_hostingEnvironment.WebRootPath, listPdfFiles[i]);
            }
            //var fileName = FileHelper.MergePDF(fileArray, outPutFilePath);
            //var path = Path.Combine(assetsFolder, fileName);

            string targetName = $"{outPutFilePath}{Guid.NewGuid()}.pdf";
            FileHelper.MergePDF(listPdfFiles, targetName);

            return targetName;
        }

        public KetQuaConvertPDF TaiHoaDon(HoaDonDienTuViewModel hoaDonDienTuViewModel)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            var filePdfPath = "";
            var fileXMLPath = "";
            var filePdfName = "";
            var fileXMLName = "";

            try
            {
                if (hoaDonDienTuViewModel.TrangThaiPhatHanh != (int)TrangThaiPhatHanh.DaPhatHanh)
                {
                }
                else
                {
                    string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
                    string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/downloaded";
                    string assetsFolder_src = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{hoaDonDienTuViewModel.HoaDonDienTuId}";
                    string filePdfXPath = Path.Combine(assetsFolder, "pdf");
                    string outPutFilePdfPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder + "/pdf");
                    if (!Directory.Exists(outPutFilePdfPath))
                    {
                        Directory.CreateDirectory(outPutFilePdfPath);
                    }

                    filePdfName = $"{hoaDonDienTuViewModel.MauSo}_{hoaDonDienTuViewModel.KyHieu}_{hoaDonDienTuViewModel.SoHoaDon}_{DateTime.Now:dd/MM/yyyy}.pdf";
                    filePdfPath = Path.Combine(outPutFilePdfPath, filePdfName.Replace("/", ""));
                    var srcPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder_src);
                    File.Copy(Path.Combine(srcPath, $"pdf/signed/{hoaDonDienTuViewModel.FileDaKy}"), filePdfPath, true);
                    filePdfPath = Path.Combine(filePdfXPath, filePdfName.Replace("/", ""));
                    filePdfName = filePdfName.Replace("/", "");

                    string outPutFileXMLPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder + "/xml");
                    string fileXMLXPath = Path.Combine(assetsFolder, "xml");
                    if (!Directory.Exists(outPutFileXMLPath))
                    {
                        Directory.CreateDirectory(outPutFileXMLPath);
                    }

                    fileXMLName = $"{hoaDonDienTuViewModel.MauSo}_{hoaDonDienTuViewModel.KyHieu}_{hoaDonDienTuViewModel.SoHoaDon}_{DateTime.Now:dd/MM/yyyy}.xml";
                    fileXMLPath = Path.Combine(outPutFileXMLPath, fileXMLName.Replace("/", ""));
                    File.Copy(Path.Combine(srcPath, $"xml/signed/{hoaDonDienTuViewModel.XMLDaKy}"), fileXMLPath, true);
                    fileXMLPath = Path.Combine(fileXMLXPath, fileXMLName.Replace("/", ""));
                    fileXMLName = fileXMLName.Replace("/", "");
                }
            }
            catch (Exception)
            {
                filePdfPath = "";
                fileXMLPath = "";
                filePdfName = "";
                fileXMLName = "";
            }
            return new KetQuaConvertPDF
            {
                FilePDF = filePdfPath,
                FileXML = fileXMLPath,
                PdfName = filePdfName,
                XMLName = fileXMLName
            };
        }

        public async Task<List<string>> GetError(string hoaDonDienTuId, int loaiLoi)
        {
            var result = new List<string>();

            result = await _db.NhatKyThaoTacHoaDons.Where(x => x.HoaDonDienTuId == hoaDonDienTuId && x.LoaiThaoTac == loaiLoi)
                                                    .Select(x => x.ErrorMessage)
                                                    .ToListAsync();

            return result;
        }

        public string ExportErrorFile(List<HoaDonDienTuViewModel> listError, int action)
        {
            string excelFileName = string.Empty;

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

            if (action == 1)
            {
                excelFileName = $"HOA-DON-PHAT-HANH-LOI-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            }
            else if (action == 2)
            {
                excelFileName = $"HOA-DON-GUI-LOI-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            }
            string excelFolder = $"FilesUpload/excels/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            // Excel
            string _sample = $"docs/HoaDonDienTu/BANG_KE_HOA_DON_DIEN_TU.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                List<HoaDonDienTuViewModel> list = listError.OrderBy(x => x.NgayHoaDon).ToList();
                // Open sheet1
                int totalRows = list.Count;

                // Begin row
                int begin_row = 2;

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
                    if (action == 1) // phát hành
                        worksheet.Cells[idx, 2].Value = "Phát hành lỗi";
                    else
                        worksheet.Cells[idx, 2].Value = "Gửi hóa đơn cho khách hàng lỗi";

                    worksheet.Cells[idx, 3].Value = it.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG ? "Hóa đơn GTGT" : "Hóa đơn bán hàng";

                    worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.MauHoaDon != null ? it.MauHoaDon.MauSo : string.Empty);
                    worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.MauHoaDon != null ? it.MauHoaDon.KyHieu : string.Empty);
                    worksheet.Cells[idx, 6].Value = it.NgayHoaDon.HasValue ? it.NgayHoaDon.Value.ToString("dd/MM/yyyy") : string.Empty;
                    worksheet.Cells[idx, 7].Value = !string.IsNullOrEmpty(it.SoHoaDon) ? it.SoHoaDon : "<Chưa cấp số>";

                    worksheet.Cells[idx, 8].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.KhachHang != null ? it.KhachHang.Ma : string.Empty);
                    worksheet.Cells[idx, 9].Value = !string.IsNullOrEmpty(it.TenKhachHang) ? it.TenKhachHang : (it.KhachHang != null ? it.KhachHang.Ten : string.Empty);
                    worksheet.Cells[idx, 10].Value = !string.IsNullOrEmpty(it.MaSoThue) ? it.MaSoThue : (it.KhachHang != null ? it.KhachHang.MaSoThue : string.Empty);
                    worksheet.Cells[idx, 11].Value = !string.IsNullOrEmpty(it.HoTenNguoiMuaHang) ? it.HoTenNguoiMuaHang : (it.KhachHang != null ? it.KhachHang.HoTenNguoiMuaHang : string.Empty);
                    worksheet.Cells[idx, 12].Value = it.LoaiTien != null ? it.LoaiTien.Ma : string.Empty;
                    worksheet.Cells[idx, 13].Value = it.TongTienThanhToan;
                    worksheet.Cells[idx, 14].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                    worksheet.Cells[idx, 15].Value = it.NguoiLap != null ? it.NguoiLap.Ten : string.Empty;

                    idx += 1;
                    count += 1;
                }
                //worksheet.Row(5).Style.Font.Color.SetColor(Color.Red);
                // Total

                worksheet.Row(idx).Style.Font.Bold = true;
                worksheet.Cells[idx, 1, idx, 2].Merge = true;
                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", list.Count);
                worksheet.Cells[idx, 13].Value = list.Sum(x => x.TongTienThanhToan);

                //replace Text


                package.SaveAs(new FileInfo(excelPath));
            }

            return excelFileName;
        }

        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonKhongMaAsync(HoaDonParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var query = from hddt in _db.HoaDonDienTus
                        join tddl in _db.DuLieuGuiHDDTChiTiets on hddt.HoaDonDienTuId equals tddl.HoaDonDienTuId into tmpTDDLs
                        from tddl in tmpTDDLs.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where hddt.NgayHoaDon.Value.Date >= fromDate && hddt.NgayHoaDon <= toDate && ((TrangThaiPhatHanh)hddt.TrangThaiPhatHanh == TrangThaiPhatHanh.DaPhatHanh) && tddl == null &&
                        hddt.KyHieu.IsHoaDonCoMa() == false &&
                        (((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonGoc) || ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonThayThe) || ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonDieuChinh))
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            MauSo = hddt.MauSo,
                            KyHieu = hddt.KyHieu,
                            NgayHoaDon = hddt.NgayHoaDon,
                            SoHoaDon = hddt.SoHoaDon,
                            KhachHangId = hddt.KhachHangId,
                            MaKhachHang = hddt.MaKhachHang ?? string.Empty,
                            TenKhachHang = hddt.TenKhachHang ?? string.Empty,
                            DiaChi = hddt.DiaChi ?? string.Empty,
                            MaSoThue = hddt.MaSoThue ?? string.Empty,
                            HoTenNguoiMuaHang = hddt.HoTenNguoiMuaHang ?? string.Empty,
                            LoaiTienId = hddt.LoaiTienId,
                            MaLoaiTien = lt.Ma,
                            MaTraCuu = hddt.MaTraCuu,
                            XMLDaKy = hddt.XMLDaKy,
                            TongTienThanhToan = hddt.TongTienThanhToan
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonCanCapMaAsync(HoaDonParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var query = from hddt in _db.HoaDonDienTus
                        join td in _db.DuLieuGuiHDDTs on hddt.HoaDonDienTuId equals td.HoaDonDienTuId into tmpThongDieps
                        from td in tmpThongDieps.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where hddt.NgayHoaDon.Value.Date >= fromDate && hddt.NgayHoaDon <= toDate && ((TrangThaiPhatHanh)hddt.TrangThaiPhatHanh == TrangThaiPhatHanh.DaPhatHanh) && td == null &&
                        hddt.KyHieu.IsHoaDonCoMa() == true &&
                        (((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonGoc) || ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonThayThe) || ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonDieuChinh))
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            MauSo = hddt.MauSo,
                            KyHieu = hddt.KyHieu,
                            NgayHoaDon = hddt.NgayHoaDon,
                            SoHoaDon = hddt.SoHoaDon,
                            KhachHangId = hddt.KhachHangId,
                            MaKhachHang = hddt.MaKhachHang ?? string.Empty,
                            TenKhachHang = hddt.TenKhachHang ?? string.Empty,
                            DiaChi = hddt.DiaChi ?? string.Empty,
                            MaSoThue = hddt.MaSoThue ?? string.Empty,
                            HoTenNguoiMuaHang = hddt.HoTenNguoiMuaHang ?? string.Empty,
                            LoaiTienId = hddt.LoaiTienId,
                            MaLoaiTien = lt.Ma,
                            MaTraCuu = hddt.MaTraCuu,
                            XMLDaKy = hddt.XMLDaKy,
                            TongTienThanhToan = hddt.TongTienThanhToan
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<ViewModels.QuanLy.DanhSachRutGonBoKyHieuHoaDonViewModel>> GetDSRutGonBoKyHieuHoaDonAsync()
        {
            var query = from boKyHieuHD in _db.BoKyHieuHoaDons
                        select new ViewModels.QuanLy.DanhSachRutGonBoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = boKyHieuHD.BoKyHieuHoaDonId,
                            KyHieu = boKyHieuHD.KyHieu,
                            UyNhiemLapHoaDon = boKyHieuHD.UyNhiemLapHoaDon
                        };
            return await query.ToListAsync();
        }
    }
}