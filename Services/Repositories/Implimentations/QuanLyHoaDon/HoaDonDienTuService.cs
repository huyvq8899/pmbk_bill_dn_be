using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using MailKit.Net.Smtp;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using Services.ViewModels.Params;
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
        Datacontext _db;
        IMapper _mp;
        IHoSoHDDTService _HoSoHDDTService;
        IHoaDonDienTuChiTietService _HoaDonDienTuChiTietService;
        IMauHoaDonService _MauHoaDonService;
        IHttpContextAccessor _IHttpContextAccessor;
        IHostingEnvironment _hostingEnvironment;
        IConfiguration _configuration;
        IXMLInvoiceService _xMLInvoiceService;
        INhatKyGuiEmailService _nhatKyGuiEmailService;
        ITuyChonService _TuyChonService;
        IBienBanDieuChinhService _BienBanDieuChinhService;

        public HoaDonDienTuService(
            Datacontext datacontext,
            IMapper mapper,
            IMauHoaDonService MauHoaDonService,
            IHoSoHDDTService HoSoHDDTService,
            IHoaDonDienTuChiTietService HoaDonDienTuChiTietService,
            ITuyChonService TuyChonService,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment IHostingEnvironment,
            IConfiguration configuration,
            INhatKyGuiEmailService nhatKyGuiEmailService,
            IXMLInvoiceService xMLInvoiceService,
            IBienBanDieuChinhService bienBanDieuChinhService
        )
        {
            _db = datacontext;
            _mp = mapper;
            _HoSoHDDTService = HoSoHDDTService;
            _MauHoaDonService = MauHoaDonService;
            _HoaDonDienTuChiTietService = HoaDonDienTuChiTietService;
            _TuyChonService = TuyChonService;
            _IHttpContextAccessor = IHttpContextAccessor;
            _configuration = configuration;
            _xMLInvoiceService = xMLInvoiceService;
            _nhatKyGuiEmailService = nhatKyGuiEmailService;
            _hostingEnvironment = IHostingEnvironment;
            _BienBanDieuChinhService = bienBanDieuChinhService;
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
            new TrangThai(){ TrangThaiId = 0, Ten = "Chưa gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 1, Ten = "Đang gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Gửi hóa đơn cho khách hàng lỗi", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Đã gửi hóa đơn cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Khách hàng đã xem hóa đơn", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Khách hàng chưa xem hóa đơn", TrangThaiChaId = 4, Level = 1 },
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
            try
            {
                var hoaDonChiTiets = await _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == id).ToListAsync();
                _db.HoaDonDienTuChiTiets.RemoveRange(hoaDonChiTiets);

                var truongMoRongHoaDons = await _db.TruongMoRongHoaDons.Where(x => x.HoaDonDienTuId == id).ToListAsync();
                _db.TruongMoRongHoaDons.RemoveRange(truongMoRongHoaDons);

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
                else return false;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
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
                                                      join tbs1 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung1Id equals tbs1.Id into tmpTBS1
                                                      from tbs1 in tmpTBS1.DefaultIfEmpty()
                                                      join tbs2 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung2Id equals tbs2.Id into tmpTBS2
                                                      from tbs2 in tmpTBS2.DefaultIfEmpty()
                                                      join tbs3 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung3Id equals tbs3.Id into tmpTBS3
                                                      from tbs3 in tmpTBS3.DefaultIfEmpty()
                                                      join tbs4 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung4Id equals tbs4.Id into tmpTBS4
                                                      from tbs4 in tmpTBS4.DefaultIfEmpty()
                                                      join tbs5 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung5Id equals tbs5.Id into tmpTBS5
                                                      from tbs5 in tmpTBS5.DefaultIfEmpty()
                                                      join tbs6 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung6Id equals tbs6.Id into tmpTBS6
                                                      from tbs6 in tmpTBS6.DefaultIfEmpty()
                                                      join tbs7 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung7Id equals tbs7.Id into tmpTBS7
                                                      from tbs7 in tmpTBS7.DefaultIfEmpty()
                                                      join tbs8 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung8Id equals tbs8.Id into tmpTBS8
                                                      from tbs8 in tmpTBS8.DefaultIfEmpty()
                                                      join tbs9 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung9Id equals tbs9.Id into tmpTBS9
                                                      from tbs9 in tmpTBS9.DefaultIfEmpty()
                                                      join tbs10 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung10Id equals tbs10.Id into tmpTBS10
                                                      from tbs10 in tmpTBS9.DefaultIfEmpty()
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
                                                          TruongThongTinBoSung1 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs1.TenTruongHienThi,
                                                              DuLieu = tbs1.DuLieu
                                                          },
                                                          TruongThongTinBoSung2 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs2.TenTruongHienThi,
                                                              DuLieu = tbs2.DuLieu
                                                          },
                                                          TruongThongTinBoSung3 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs3.TenTruongHienThi,
                                                              DuLieu = tbs3.DuLieu
                                                          },
                                                          TruongThongTinBoSung4 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs4.TenTruongHienThi,
                                                              DuLieu = tbs4.DuLieu
                                                          },
                                                          TruongThongTinBoSung5 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs5.TenTruongHienThi,
                                                              DuLieu = tbs5.DuLieu
                                                          },
                                                          TruongThongTinBoSung6 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs6.TenTruongHienThi,
                                                              DuLieu = tbs6.DuLieu
                                                          },
                                                          TruongThongTinBoSung7 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs7.TenTruongHienThi,
                                                              DuLieu = tbs7.DuLieu
                                                          },
                                                          TruongThongTinBoSung8 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs8.TenTruongHienThi,
                                                              DuLieu = tbs8.DuLieu
                                                          },
                                                          TruongThongTinBoSung9 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs9.TenTruongHienThi,
                                                              DuLieu = tbs9.DuLieu
                                                          },
                                                          TruongThongTinBoSung10 = new TruongDuLieuMoRongViewModel
                                                          {
                                                              TenTruongHienThi = tbs10.TenTruongHienThi,
                                                              DuLieu = tbs10.DuLieu
                                                          },
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

            return PagedList<HoaDonDienTuViewModel>
                    .Create(query, pagingParams.PageNumber, pagingParams.PageSize);
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
                        join tbs1 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung1Id equals tbs1.Id into tmpTBS1
                        from tbs1 in tmpTBS1.DefaultIfEmpty()
                        join tbs2 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung2Id equals tbs2.Id into tmpTBS2
                        from tbs2 in tmpTBS2.DefaultIfEmpty()
                        join tbs3 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung3Id equals tbs3.Id into tmpTBS3
                        from tbs3 in tmpTBS3.DefaultIfEmpty()
                        join tbs4 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung4Id equals tbs4.Id into tmpTBS4
                        from tbs4 in tmpTBS4.DefaultIfEmpty()
                        join tbs5 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung5Id equals tbs5.Id into tmpTBS5
                        from tbs5 in tmpTBS5.DefaultIfEmpty()
                        join tbs6 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung6Id equals tbs6.Id into tmpTBS6
                        from tbs6 in tmpTBS6.DefaultIfEmpty()
                        join tbs7 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung7Id equals tbs7.Id into tmpTBS7
                        from tbs7 in tmpTBS7.DefaultIfEmpty()
                        join tbs8 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung8Id equals tbs8.Id into tmpTBS8
                        from tbs8 in tmpTBS8.DefaultIfEmpty()
                        join tbs9 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung9Id equals tbs9.Id into tmpTBS9
                        from tbs9 in tmpTBS9.DefaultIfEmpty()
                        join tbs10 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung10Id equals tbs10.Id into tmpTBS10
                        from tbs10 in tmpTBS9.DefaultIfEmpty()
                        where hd.HoaDonDienTuId == id
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
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
                            TruongThongTinBoSung1Id = hd.TruongThongTinBoSung1Id,
                            TruongThongTinBoSung1 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs1.Id,
                                DataId = tbs1.DataId,
                                TenTruong = tbs1.TenTruong,
                                TenTruongHienThi = tbs1.TenTruongHienThi,
                                DuLieu = tbs1.DuLieu,
                                HienThi = tbs1.HienThi
                            },
                            TruongThongTinBoSung2Id = hd.TruongThongTinBoSung2Id,
                            TruongThongTinBoSung2 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs2.Id,
                                DataId = tbs2.DataId,
                                TenTruong = tbs2.TenTruong,
                                TenTruongHienThi = tbs2.TenTruongHienThi,
                                DuLieu = tbs2.DuLieu,
                                HienThi = tbs2.HienThi
                            },
                            TruongThongTinBoSung3Id = hd.TruongThongTinBoSung3Id,
                            TruongThongTinBoSung3 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs3.Id,
                                DataId = tbs3.DataId,
                                TenTruong = tbs3.TenTruong,
                                TenTruongHienThi = tbs3.TenTruongHienThi,
                                DuLieu = tbs3.DuLieu,
                                HienThi = tbs3.HienThi
                            },
                            TruongThongTinBoSung4Id = hd.TruongThongTinBoSung4Id,
                            TruongThongTinBoSung4 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs4.Id,
                                DataId = tbs4.DataId,
                                TenTruong = tbs4.TenTruong,
                                TenTruongHienThi = tbs4.TenTruongHienThi,
                                DuLieu = tbs4.DuLieu,
                                HienThi = tbs4.HienThi
                            },
                            TruongThongTinBoSung5Id = hd.TruongThongTinBoSung5Id,
                            TruongThongTinBoSung5 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs5.Id,
                                DataId = tbs5.DataId,
                                TenTruong = tbs5.TenTruong,
                                TenTruongHienThi = tbs5.TenTruongHienThi,
                                DuLieu = tbs5.DuLieu,
                                HienThi = tbs5.HienThi,
                            },
                            TruongThongTinBoSung6Id = hd.TruongThongTinBoSung6Id,
                            TruongThongTinBoSung6 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs6.Id,
                                DataId = tbs6.DataId,
                                TenTruong = tbs6.TenTruong,
                                TenTruongHienThi = tbs6.TenTruongHienThi,
                                DuLieu = tbs6.DuLieu,
                                HienThi = tbs6.HienThi
                            },
                            TruongThongTinBoSung7Id = hd.TruongThongTinBoSung7Id,
                            TruongThongTinBoSung7 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs7.Id,
                                DataId = tbs7.DataId,
                                TenTruong = tbs7.TenTruong,
                                TenTruongHienThi = tbs7.TenTruongHienThi,
                                DuLieu = tbs7.DuLieu,
                                HienThi = tbs7.HienThi
                            },
                            TruongThongTinBoSung8Id = hd.TruongThongTinBoSung8Id,
                            TruongThongTinBoSung8 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs8.Id,
                                DataId = tbs8.DataId,
                                TenTruong = tbs8.TenTruong,
                                TenTruongHienThi = tbs8.TenTruongHienThi,
                                DuLieu = tbs8.DuLieu,
                                HienThi = tbs8.HienThi
                            },
                            TruongThongTinBoSung9Id = hd.TruongThongTinBoSung9Id,
                            TruongThongTinBoSung9 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs9.Id,
                                DataId = tbs9.DataId,
                                TenTruong = tbs9.TenTruong,
                                TenTruongHienThi = tbs9.TenTruongHienThi,
                                DuLieu = tbs9.DuLieu,
                                HienThi = tbs9.HienThi
                            },
                            TruongThongTinBoSung10Id = hd.TruongThongTinBoSung10Id,
                            TruongThongTinBoSung10 = new TruongDuLieuMoRongViewModel
                            {
                                Id = tbs10.Id,
                                DataId = tbs10.DataId,
                                TenTruong = tbs10.TenTruong,
                                TenTruongHienThi = tbs10.TenTruongHienThi,
                                DuLieu = tbs10.DuLieu,
                                HienThi = tbs10.HienThi
                            },
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
                                               join tmr1 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet1Id equals tmr1.Id into tmpTMR1
                                               from tmr1 in tmpTMR1.DefaultIfEmpty()
                                               join tmr2 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet2Id equals tmr2.Id into tmpTMR2
                                               from tmr2 in tmpTMR2.DefaultIfEmpty()
                                               join tmr3 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet3Id equals tmr3.Id into tmpTMR3
                                               from tmr3 in tmpTMR3.DefaultIfEmpty()
                                               join tmr4 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet4Id equals tmr4.Id into tmpTMR4
                                               from tmr4 in tmpTMR4.DefaultIfEmpty()
                                               join tmr5 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet5Id equals tmr5.Id into tmpTMR5
                                               from tmr5 in tmpTMR5.DefaultIfEmpty()
                                               join tmr6 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet6Id equals tmr6.Id into tmpTMR6
                                               from tmr6 in tmpTMR6.DefaultIfEmpty()
                                               join tmr7 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet7Id equals tmr7.Id into tmpTMR7
                                               from tmr7 in tmpTMR7.DefaultIfEmpty()
                                               join tmr8 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet8Id equals tmr8.Id into tmpTMR8
                                               from tmr8 in tmpTMR8.DefaultIfEmpty()
                                               join tmr9 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet9Id equals tmr9.Id into tmpTMR9
                                               from tmr9 in tmpTMR9.DefaultIfEmpty()
                                               join tmr10 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet10Id equals tmr10.Id into tmpTMR10
                                               from tmr10 in tmpTMR9.DefaultIfEmpty()
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
                                                   TruongMoRongChiTiet1 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr1.Id,
                                                       DataId = tmr1.DataId,
                                                       TenTruong = tmr1.TenTruong,
                                                       TenTruongHienThi = tmr1.TenTruongHienThi,
                                                       DuLieu = tmr1.DuLieu,
                                                       HienThi = tmr1.HienThi
                                                   },
                                                   TruongMoRongChiTiet2 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr2.Id,
                                                       DataId = tmr2.DataId,
                                                       TenTruong = tmr2.TenTruong,
                                                       TenTruongHienThi = tmr2.TenTruongHienThi,
                                                       DuLieu = tmr2.DuLieu,
                                                       HienThi = tmr2.HienThi
                                                   },
                                                   TruongMoRongChiTiet3 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr3.Id,
                                                       DataId = tmr3.DataId,
                                                       TenTruong = tmr3.TenTruong,
                                                       TenTruongHienThi = tmr3.TenTruongHienThi,
                                                       DuLieu = tmr3.DuLieu,
                                                       HienThi = tmr3.HienThi
                                                   },
                                                   TruongMoRongChiTiet4 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr4.Id,
                                                       DataId = tmr4.DataId,
                                                       TenTruong = tmr4.TenTruong,
                                                       TenTruongHienThi = tmr4.TenTruongHienThi,
                                                       DuLieu = tmr4.DuLieu,
                                                       HienThi = tmr4.HienThi
                                                   },
                                                   TruongMoRongChiTiet5 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr5.Id,
                                                       DataId = tmr5.DataId,
                                                       TenTruong = tmr5.TenTruong,
                                                       TenTruongHienThi = tmr5.TenTruongHienThi,
                                                       DuLieu = tmr5.DuLieu,
                                                       HienThi = tmr5.HienThi
                                                   },
                                                   TruongMoRongChiTiet6 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr6.Id,
                                                       DataId = tmr6.DataId,
                                                       TenTruong = tmr6.TenTruong,
                                                       TenTruongHienThi = tmr6.TenTruongHienThi,
                                                       DuLieu = tmr6.DuLieu,
                                                       HienThi = tmr6.HienThi
                                                   },
                                                   TruongMoRongChiTiet7 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr7.Id,
                                                       DataId = tmr7.DataId,
                                                       TenTruong = tmr7.TenTruong,
                                                       TenTruongHienThi = tmr7.TenTruongHienThi,
                                                       DuLieu = tmr7.DuLieu,
                                                       HienThi = tmr7.HienThi
                                                   },
                                                   TruongMoRongChiTiet8 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr8.Id,
                                                       DataId = tmr8.DataId,
                                                       TenTruong = tmr8.TenTruong,
                                                       TenTruongHienThi = tmr8.TenTruongHienThi,
                                                       DuLieu = tmr8.DuLieu,
                                                       HienThi = tmr8.HienThi
                                                   },
                                                   TruongMoRongChiTiet9 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr9.Id,
                                                       DataId = tmr9.DataId,
                                                       TenTruong = tmr9.TenTruong,
                                                       TenTruongHienThi = tmr9.TenTruongHienThi,
                                                       DuLieu = tmr9.DuLieu,
                                                       HienThi = tmr9.HienThi
                                                   },
                                                   TruongMoRongChiTiet10 = new TruongDuLieuMoRongViewModel
                                                   {
                                                       Id = tmr10.Id,
                                                       DataId = tmr10.DataId,
                                                       TenTruong = tmr10.TenTruong,
                                                       TenTruongHienThi = tmr10.TenTruongHienThi,
                                                       DuLieu = tmr10.DuLieu,
                                                       HienThi = tmr10.HienThi
                                                   },
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
            try
            {
                model.HoaDonDienTuId = Guid.NewGuid().ToString();
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
            try
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

        public async Task<string> CreateSoBienBanXoaBoHoaDon()
        {
            var result = string.Empty;
            try
            {
                var maxSoCT = await _db.BienBanXoaBos.Where(x => !string.IsNullOrEmpty(x.SoBienBan))
                                                    .MaxAsync(x => x.SoBienBan);
                if (!string.IsNullOrEmpty(maxSoCT))
                {
                    var number = maxSoCT.Substring(3);
                    var next = int.Parse(number) + 1;
                    result = "BBH" + next.ToString("00000");
                }
                else result = "BBH00001";
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
                        SoHoaDon = string.Empty,
                        ErrorMessage = "Chưa lập thông báo phát hành cho mẫu hóa đơn tương ứng, hoặc thông báo phát hành chưa được cơ quan thuế chấp nhận"
                    };
                }
                else if (thongBaoPhatHanh.ThongBaoPhatHanh.TrangThaiNop != TrangThaiNop.DaDuocChapNhan)
                {
                    var converMaxToInt = int.Parse(validMaxSoHoaDon);
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.ChuaDuocCQTChapNhan,
                        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.ToString("0000000") :
                                   (converMaxToInt + 1).ToString("0000000")) : thongBaoPhatHanh.TuSo.Value.ToString("0000000"),
                        ErrorMessage = "Chưa lập thông báo phát hành cho mẫu hóa đơn tương ứng, hoặc thông báo phát hành chưa được cơ quan thuế chấp nhận"
                    };
                }
                else if (thongBaoPhatHanh.NgayBatDauSuDung > hd.NgayHoaDon)
                {
                    var converMaxToInt = !string.IsNullOrEmpty(validMaxSoHoaDon) ? int.Parse(validMaxSoHoaDon) : 0;
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.NgayHoaDonNhoHonNgayBatDauSuDung,
                        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.ToString("0000000") :
                                   (converMaxToInt + 1).ToString("0000000")) : thongBaoPhatHanh.TuSo.Value.ToString("0000000"),
                        ErrorMessage = $"Ngày hóa đơn không được nhỏ hơn ngày bắt đầu sử dụng của hóa đơn trên thông báo phát hành hóa đơn <{thongBaoPhatHanh.NgayBatDauSuDung.Value.ToString("dd/MM/yyyy")}>"
                    };
                }
                else if (DateTime.Now.Date > hd.NgayHoaDon.Value.Date)
                {
                    var converMaxToInt = !string.IsNullOrEmpty(validMaxSoHoaDon) ? int.Parse(validMaxSoHoaDon) : 0;
                    return new KetQuaCapSoHoaDon
                    {
                        LoiTrangThaiPhatHanh = (int)LoiThongBaoPhatHanh.NgayHoaDonNhoHonNgayKy,
                        SoHoaDon = !string.IsNullOrEmpty(validMaxSoHoaDon) ? (converMaxToInt < thongBaoPhatHanh.TuSo ? thongBaoPhatHanh.TuSo.Value.ToString("0000000") :
                                   (converMaxToInt + 1).ToString("0000000")) : thongBaoPhatHanh.TuSo.Value.ToString("0000000"),
                        ErrorMessage = "Ngày hóa đơn không được nhỏ hơn ngày ký"
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
                                SoHoaDon = (converMaxToInt + 1).ToString("0000000"),
                                ErrorMessage = "Số hóa đơn vượt quá giới hạn đã đăng ký với cơ quan thuế hoặc thông tin không chính xác so với thông báo phát hành"
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
                                            ErrorMessage = "Hóa đơn có số nhỏ hơn không được có ngày lớn hơn ngày của hóa đơn có số lớn hơn"
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
                    SoHoaDon = string.Empty,
                    ErrorMessage = ex.Message
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

            try
            {
                var _tuyChons = await _TuyChonService.GetAllAsync();

                var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
                var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
                var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());

                var hoSoHDDT = await _HoSoHDDTService.GetDetailAsync();
                var mauHoaDon = await _MauHoaDonService.GetByIdAsync(hd.MauHoaDonId);

                var doc = MauHoaDonHelper.TaoMauHoaDonDoc(mauHoaDon, hd.GetBoMauHoaDonFromHoaDonDienTu(), hoSoHDDT, _hostingEnvironment, _IHttpContextAccessor, out int beginRow, !string.IsNullOrEmpty(hd.LyDoThayThe) || !string.IsNullOrEmpty(hd.LyDoDieuChinh));

                doc.Replace(LoaiChiTietTuyChonNoiDung.MauSo.GenerateWordKey(), hd.MauSo ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.KyHieu.GenerateWordKey(), hd.KyHieu ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoHoaDon.GenerateWordKey(), string.IsNullOrEmpty(hd.SoHoaDon) ? "<Chưa cấp số>" : hd.SoHoaDon, true, true);

                doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.HoTenNguoiMua.GenerateWordKey(), hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua.GenerateWordKey(), hd.KhachHang != null ? (hd.KhachHang.TenDonVi ?? string.Empty) : string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua.GenerateWordKey(), hd.MaSoThue ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua.GenerateWordKey(), hd.DiaChi ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.HinhThucThanhToan.GenerateWordKey(), hd.HinhThucThanhToan?.Ten ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua.GenerateWordKey(), hd.SoTaiKhoanNganHang ?? string.Empty, true, true);


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

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TyLeChietKhau.GenerateWordKey(), (models.Sum(x => x.TyLeChietKhau) / models.Count).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienChietKhau.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienChietKhau : hd.TongTienChietKhauQuyDoi).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TienThueGTGT.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienThueGTGT : hd.TongTienThueGTGTQuyDoi).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHang.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienHang : hd.TongTienHangQuyDoi).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.ThueSuatGTGT.GenerateWordKey(), models.Select(x => x.ThueGTGT).FirstOrDefault() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienThanhToan.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienThanhToan : hd.TongTienThanhToanQuyDoi).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienBangChu.GenerateWordKey(), soTienBangChu ?? string.Empty, true, true);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TyGia.GenerateWordKey(), (hd.TyGia.Value.FormatPriceTwoDecimal() + $" VND/{hd.MaLoaiTien}") ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.QuyDoi.GenerateWordKey(), (hd.TongTienThanhToanQuyDoi.Value.FormatPriceTwoDecimal() + " VND") ?? string.Empty, true, true);

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

                                row.Cells[5].Paragraphs[0].SetValuePar((hd.IsVND == true ? models[i].ThanhTienQuyDoi : models[i].ThanhTien).Value.FormatPriceTwoDecimal());
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

                                row.Cells[5].Paragraphs[0].SetValuePar((hd.IsVND == true ? models[i].ThanhTienQuyDoi : models[i].ThanhTien).Value.FormatPriceTwoDecimal());

                                row.Cells[6].Paragraphs[0].SetValuePar(models[i].ThueGTGT);

                                row.Cells[7].Paragraphs[0].SetValuePar((hd.IsVND == true ? models[i].TienThueGTGTQuyDoi : models[i].TienThueGTGT).Value.FormatPriceTwoDecimal());
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
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return new KetQuaConvertPDF()
            {
                FilePDF = path,
                FileXML = pathXML,
                PdfName = pdfFileName,
                XMLName = xmlFileName
            };
        }

        private string GetLinkFileUnsignedXML(string link)
        {
            var filename = "FilesUpload/xml/unsigned/" + link;
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

        public async Task<bool> CheckMaTraCuuAsync(string maTraCuu)
        {
            var result = await _db.HoaDonDienTus
                                    .Where(x => x.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.MaTraCuu == maTraCuu);

            return result != null ? true : false;
        }

        private async Task<string> ConvertHoaDonToHoaDonGiay(HoaDonDienTuViewModel hd, ParamsChuyenDoiThanhHDGiay @params)
        {
            var path = string.Empty;
            try
            {
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

                var doc = MauHoaDonHelper.TaoMauHoaDonDoc(mauHoaDon, hd.GetBoMauHoaDonFromHoaDonDienTu(false), hoSoHDDT, _hostingEnvironment, _IHttpContextAccessor, out int beginRow, !string.IsNullOrEmpty(hd.LyDoThayThe) || !string.IsNullOrEmpty(hd.LyDoDieuChinh));

                doc.Replace(LoaiChiTietTuyChonNoiDung.MauSo.GenerateWordKey(), hd.MauSo ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.KyHieu.GenerateWordKey(), hd.KyHieu ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoHoaDon.GenerateWordKey(), string.IsNullOrEmpty(hd.SoHoaDon) ? "<Chưa cấp số>" : hd.SoHoaDon, true, true);

                doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.HoTenNguoiMua.GenerateWordKey(), hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua.GenerateWordKey(), hd.KhachHang != null ? (hd.KhachHang.TenDonVi ?? string.Empty) : string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua.GenerateWordKey(), hd.MaSoThue ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua.GenerateWordKey(), hd.DiaChi ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.HinhThucThanhToan.GenerateWordKey(), hd.HinhThucThanhToan != null ? hd.HinhThucThanhToan.Ten : string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua.GenerateWordKey(), hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

                doc.Replace("<convertor>", @params.TenNguoiChuyenDoi ?? string.Empty, true, true);
                doc.Replace("<conversionDate>", @params.NgayChuyenDoi.Value.ToString("dd/MM/yyyy") ?? string.Empty, true, true);

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
                    Table table = null;
                    table = listTable[0];

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TyLeChietKhau.GenerateWordKey(), (models.Sum(x => x.TyLeChietKhau) / models.Count).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienChietKhau.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienChietKhauQuyDoi : hd.TongTienChietKhau).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TienThueGTGT.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienThueGTGTQuyDoi : hd.TongTienThueGTGT).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHang.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienHangQuyDoi : hd.TongTienHang).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.ThueSuatGTGT.GenerateWordKey(), models.Select(x => x.ThueGTGT).FirstOrDefault() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienThanhToan.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienThanhToanQuyDoi : hd.TongTienThanhToan).Value.FormatPriceTwoDecimal() ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienBangChu.GenerateWordKey(), (hd.IsVND == true ? hd.TongTienThanhToanQuyDoi : hd.TongTienThanhToan).Value.ConvertToInWord(_cachDocSo0HangChuc.ToLower(), _cachDocHangNghin.ToLower(), _hienThiSoChan) ?? string.Empty, true, true);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TyGia.GenerateWordKey(), (hd.TyGia.Value.FormatPriceTwoDecimal() + $" VND/{hd.MaLoaiTien}") ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.QuyDoi.GenerateWordKey(), (hd.TongTienThanhToanQuyDoi.Value.FormatPriceTwoDecimal() + " VND") ?? string.Empty, true, true);

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

                            row.Cells[5].Paragraphs[0].SetValuePar((hd.IsVND == true ? models[i].ThanhTien : models[i].ThanhTienQuyDoi).Value.FormatPriceTwoDecimal());
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

                            row.Cells[5].Paragraphs[0].SetValuePar((hd.IsVND == true ? models[i].ThanhTien : models[i].ThanhTienQuyDoi).Value.FormatPriceTwoDecimal());

                            row.Cells[6].Paragraphs[0].SetValuePar(models[i].ThueGTGT);

                            row.Cells[7].Paragraphs[0].SetValuePar((hd.IsVND == true ? models[i].TienThueGTGT : models[i].TienThueGTGTQuyDoi).Value.FormatPriceTwoDecimal());
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

                    var _objTrangThaiLuuTru = await GetTrangThaiLuuTru(_objHDDT.HoaDonDienTuId);
                    _objTrangThaiLuuTru = _objTrangThaiLuuTru != null ? _objTrangThaiLuuTru : new LuuTruTrangThaiFileHDDTViewModel();
                    if (string.IsNullOrEmpty(_objTrangThaiLuuTru.HoaDonDienTuId)) _objTrangThaiLuuTru.HoaDonDienTuId = _objHDDT.HoaDonDienTuId;

                    // PDF 
                    byte[] bytePDF = DataHelper.StringToByteArray(@param.DataPDF);
                    _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                    File.WriteAllBytes(Path.Combine(newSignedPdfFolder, newPdfFileName), _objTrangThaiLuuTru.PdfDaKy);

                    //xml
                    string xmlDeCode = DataHelper.Base64Decode(@param.DataXML);
                    byte[] byteXML = Encoding.UTF8.GetBytes(@param.DataXML);
                    _objTrangThaiLuuTru.XMLDaKy = byteXML;
                    File.WriteAllText(Path.Combine(newSignedXmlFolder, newXmlFileName), xmlDeCode);

                    _objTrangThaiLuuTru.PdfChuaKy = null;
                    _objTrangThaiLuuTru.XMLChuaKy = null;
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
                        _objTrangThaiLuuTru = _objTrangThaiLuuTru != null ? _objTrangThaiLuuTru : new LuuTruTrangThaiBBXBViewModel();
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
                        _objTrangThaiLuuTru.PdfChuaKy = null;
                        _objTrangThaiLuuTru.XMLChuaKy = null;
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        //_objTrangThaiLuuTru.XMLDaKy = Encoding.UTF8.GetBytes(@param.DataXML);
                        await this.UpdateTrangThaiLuuFileBBXB(_objTrangThaiLuuTru);

                        param.BienBan.FileDaKy = newPdfFileName;
                        if (param.TypeKy == 1004)
                            param.BienBan.NgayKyBenA = DateTime.Now;
                        else if (param.TypeKy == 1005)
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

                        if (param.TypeKy == 1004)
                            _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaGuiKH;
                        else
                            _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.KHDaKy;
                        await this.UpdateAsync(_objHDDT);

                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }


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
            catch (Exception ex)
            {
                return false;
            }
        }

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
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> SendEmailAsync(ParamsSendMail @params)
        {
            try
            {
                var hddt = await GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                var bbxb = await GetBienBanXoaBoHoaDon(@params.HoaDon.HoaDonDienTuId);
                var bbdc = await _BienBanDieuChinhService.GetByIdAsync(@params.HoaDon.BienBanDieuChinhId);

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
                        pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"pdf/signed/{bbdc.FileDaKy}");
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
                    messageBody = messageBody.Replace("##lydodieuchinh##", hddt.LyDoDieuChinhModel.ToString());
                }

                var _objHDDT = await this.GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                if (await this.SendEmailAsync(@params.ToMail, messageTitle, messageBody, pdfFilePath, @params.CC, @params.BCC))
                {
                    if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                        _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                        _objHDDT.DaGuiThongBaoXoaBoHoaDon = true;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                        _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChoKHKy;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    {
                        bbdc.TrangThaiBienBan = (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChoKhachHangKy;
                        await _BienBanDieuChinhService.UpdateAsync(bbdc);
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
            try
            {
                var entity = await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == bb.Id);
                _db.Entry<BienBanXoaBo>(entity).CurrentValues.SetValues(bb);
                if (await _db.SaveChangesAsync() > 0)
                {
                    var entityHD = await GetByIdAsync(entity.HoaDonDienTuId);
                    entityHD.LyDoXoaBo = entity.LyDoXoaBo;
                    return await UpdateAsync(entityHD);
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<BienBanXoaBoViewModel> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon @params)
        {
            try
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
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return null;
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

        public async Task<BienBanXoaBoViewModel> GetBienBanXoaBoById(string Id)
        {
            return _mp.Map<BienBanXoaBoViewModel>(await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<KetQuaConvertPDF> ConvertBienBanXoaHoaDon(BienBanXoaBoViewModel bb)
        {
            var path = string.Empty;
            var pathXML = string.Empty;
            try
            {
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
                            FileXML = Path.Combine(assetsFolder, $"pdf/signed/{_objBB.XMLDaKy}"),
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
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
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
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
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
                                MauSoBiDieuChinh = hdbdc.MauSo,
                                KyHieuBiDieuChinh = hdbdc.KyHieu,

                                BienBanDieuChinhId = bbdc.BienBanDieuChinhId,
                                TenNguoiNhanBienBan = kh != null ? kh.HoTenNguoiNhanHD : null,
                                EmailNguoiNhanBienBan = kh != null ? kh.EmailNguoiNhanHD : null,
                                SoDienThoaiNguoiNhanBienBan = kh != null ? kh.SoDienThoaiNguoiNhanHD : null,
                                LyDoDieuChinhBienBan = bbdc.LyDoDieuChinh,

                                HoaDonDieuChinhId = hddc != null ? hddc.HoaDonDienTuId : null,
                                TrangThaiHoaDonDieuChinh = hddc != null ? hddc.TrangThai : null,
                                TenTrangThaiHoaDonDieuChinh = hddc != null ? ((TrangThaiHoaDon)hddc.TrangThai).GetDescription() : string.Empty,
                                TenHinhThucHoaDonBiDieuChinh = hddc != null ? hddc.LyDoDieuChinh.GetTenHinhThucHoaDonBiDieuChinh() : string.Empty,
                                LyDoDieuChinh = hdbdc != null ? hddc.LyDoDieuChinh.GetNoiDungLyDoDieuChinh() : string.Empty,
                                LoaiDieuChinh = hdbdc != null ? hddc.LoaiDieuChinh : null,
                                TenLoaiDieuChinh = hddc != null ? ((LoaiDieuChinhHoaDon)hddc.LoaiDieuChinh).GetDescription() : string.Empty,
                                TrangThaiBienBanDieuChinh = bbdc.TrangThaiBienBan,
                                TenTrangThaiBienBanDieuChinh = ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription(),
                                MaTraCuuDieuChinh = hddc != null ? hddc.MaTraCuu : string.Empty,
                                LoaiHoaDonDieuChinh = hddc.LoaiHoaDon,
                                TenLoaiHoaDonDieuChinh = hddc != null ? ((LoaiHoaDon)hddc.LoaiHoaDon).GetDescription() : string.Empty,
                                NgayHoaDonDieuChinh = hddc != null ? hddc.NgayHoaDon : null,
                                SoHoaDonDieuChinh = hddc != null ? hddc.SoHoaDon : string.Empty,
                                MauSoDieuChinh = hddc != null ? hddc.MauSo : string.Empty,
                                KyHieuDieuChinh = hddc != null ? hddc.KyHieu : string.Empty,
                                MaKhachHangDieuChinh = hddc != null ? hddc.MaKhachHang : string.Empty,
                                TenKhachHangDieuChinh = hddc != null ? hddc.TenKhachHang : string.Empty,
                                MaSoThueDieuChinh = hddc != null ? hddc.MaSoThue : string.Empty,
                                NguoiMuaHangDieuChinh = hddc != null ? hddc.HoTenNguoiMuaHang : string.Empty,
                                NhanVienBanHangDieuChinh = hddc != null ? hddc.TenNhanVienBanHang : string.Empty,
                                LoaiTienId = hddc != null ? hddc.LoaiTienId : string.Empty,
                                MaLoaiTien = lt != null ? lt.Ma : "VND",
                                IsVND = lt != null ? (lt.Ma == "VND") : true,
                                TongTienThanhToan = hddc != null ? hddc.TongTienThanhToanQuyDoi : 0,
                                TrangThaiPhatHanhDieuChinh = hddc != null ? hddc.TrangThaiPhatHanh : null,
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
                             where (TrangThaiHoaDon)hddc.TrangThai == TrangThaiHoaDon.HoaDonDieuChinh && (string.IsNullOrEmpty(hddc.DieuChinhChoHoaDonId) || (!string.IsNullOrEmpty(hddc.DieuChinhChoHoaDonId) && !hoaDonDieuChinhIds.Contains(hddc.HoaDonDienTuId)))
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
                                 TenTrangThaiHoaDonDieuChinh = ((TrangThaiHoaDon)hddc.TrangThai).GetDescription(),
                                 TenHinhThucHoaDonBiDieuChinh = hddc.LyDoDieuChinh.GetTenHinhThucHoaDonBiDieuChinh(),
                                 LyDoDieuChinh = hddc.LyDoDieuChinh.GetNoiDungLyDoDieuChinh(),
                                 LoaiDieuChinh = hddc.LoaiDieuChinh,
                                 TenLoaiDieuChinh = ((LoaiDieuChinhHoaDon)hddc.LoaiDieuChinh).GetDescription(),
                                 TrangThaiBienBanDieuChinh = bbdc == null ? 0 : bbdc.TrangThaiBienBan,
                                 TenTrangThaiBienBanDieuChinh = bbdc == null ? LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan.GetDescription() : ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription(),
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
                                 IsVND = lt != null ? (lt.Ma == "VND") : true,
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
            if (@params.FilterColumns != null && @params.FilterColumns.Any())
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in @params.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        //case nameof(@params.Filter.SoHoaDon):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.SoHoaDon, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.MauSo):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.MauSo, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.KyHieu):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.KyHieu, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.MaKhachHang):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.MaKhachHang, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.TenKhachHang):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.TenKhachHang, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.DiaChi):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.DiaChi, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.MaSoThue):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.MaSoThue, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.HoTenNguoiMuaHang):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.HoTenNguoiMuaHang, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.TenNhanVienBanHang):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.TenNhanVienBanHang, filterCol, FilterValueType.String);
                        //    break;
                        //case nameof(@params.Filter.TongTienThanhToan):
                        //    query = GenericFilterColumn<BangKeHoaDonDieuChinh>.Query(query, x => x.TongTienThanhToan, filterCol, FilterValueType.Decimal);
                        //    break;
                        //default:
                        //    break;
                    }
                }
            }

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

        public async Task<string> XemHoaDonDongLoat(List<string> fileArray)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/merged";

            string outPutFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            for (int i = 0; i < fileArray.Count; i++)
            {
                fileArray[i] = Path.Combine(_hostingEnvironment.WebRootPath, fileArray[i]);
            }
            var fileName = FileHelper.MergePDF(fileArray.ToArray(), outPutFilePath);
            var path = Path.Combine(assetsFolder, fileName);
            return path;
        }

        public async Task<KetQuaConvertPDF> TaiHoaDon(HoaDonDienTuViewModel hoaDonDienTuViewModel)
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

                    filePdfName = $"{hoaDonDienTuViewModel.MauSo}_{hoaDonDienTuViewModel.KyHieu}_{hoaDonDienTuViewModel.SoHoaDon}_{DateTime.Now.ToString("dd/MM/yyyy")}.pdf";
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

                    fileXMLName = $"{hoaDonDienTuViewModel.MauSo}_{hoaDonDienTuViewModel.KyHieu}_{hoaDonDienTuViewModel.SoHoaDon}_{DateTime.Now.ToString("dd/MM/yyyy")}.xml";
                    fileXMLPath = Path.Combine(outPutFileXMLPath, fileXMLName.Replace("/", ""));
                    File.Copy(Path.Combine(srcPath, $"xml/signed/{hoaDonDienTuViewModel.XMLDaKy}"), fileXMLPath, true);
                    fileXMLPath = Path.Combine(fileXMLXPath, fileXMLName.Replace("/", ""));
                    fileXMLName = fileXMLName.Replace("/", "");
                }
            }
            catch (Exception ex)
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

        public async Task<List<TruongMoRongHoaDonViewModel>> GetTruongMoRongHoaDonAsync(string mauHoaDonId)
        {
            List<TruongMoRongHoaDonViewModel> result = new List<TruongMoRongHoaDonViewModel>();

            var listTuyChinhChiTiets = await _db.MauHoaDonTuyChinhChiTiets
                .Where(x => x.MauHoaDonId == mauHoaDonId && (x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua || x.Loai == LoaiTuyChinhChiTiet.ThongTinVeHangHoaDichVu) && x.LoaiChiTiet < 0)
                .GroupBy(x => x.LoaiChiTiet)
                .Select(x => new MauHoaDonTuyChinhChiTietViewModel
                {
                    KieuDuLieuThietLap = x.First().KieuDuLieuThietLap,
                    Loai = x.First().Loai,
                    LoaiChiTiet = x.Key,
                    Checked = x.First(y => y.IsParent == true).Checked,
                    STT = x.First(y => y.IsParent == true).STT,
                    Children = x.Where(y => y.IsParent == false)
                        .Select(y => new MauHoaDonTuyChinhChiTietViewModel
                        {
                            GiaTri = y.GiaTri,
                            GiaTriMacDinh = y.GiaTriMacDinh,
                            Loai = y.Loai,
                            LoaiChiTiet = y.LoaiChiTiet,
                            LoaiContainer = y.LoaiContainer,
                            STT = y.STT
                        })
                        .ToList()
                })
                .OrderBy(x => x.STT)
                .ToListAsync();

            foreach (var parent in listTuyChinhChiTiets)
            {
                var tieuDe = parent.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                var noiDung = parent.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.NoiDung);
                var tieuDeSongNgu = parent.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);

                result.Add(new TruongMoRongHoaDonViewModel
                {
                    TenTruong = tieuDe.GiaTri,
                    TenTruongTiengAnh = tieuDeSongNgu != null ? tieuDeSongNgu.GiaTri : null,
                    KieuDuLieu = parent.KieuDuLieuThietLap,
                    GiaTriMacDinh = noiDung.GiaTriMacDinh,
                    DoRong = parent.DoRong ?? 100,
                    IsHienThi = parent.Checked
                });
            }

            return result;
        }
    }
}