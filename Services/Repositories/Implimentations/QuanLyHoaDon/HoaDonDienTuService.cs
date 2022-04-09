﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
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
using Services.Helper.Constants;
using Services.Helper.Params.Filter;
using Services.Helper.Params.HeThong;
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
using Services.ViewModels.Import;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.TienIch;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;

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
        private readonly IDoiTuongService _doiTuongService;
        private readonly IHangHoaDichVuService _hangHoaDichVuService;
        private readonly ILoaiTienService _loaiTienService;
        private readonly IDonViTinhService _donViTinhService;
        private readonly IThongTinHoaDonService _thongTinHoaDonService;
        private readonly ITVanService _tVanService;
        private int timeToListenResTCT = 0;

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
            IXMLInvoiceService xMLInvoiceService,
            IBoKyHieuHoaDonService boKyHieuHoaDonService,
            IDoiTuongService doiTuongService,
            IHangHoaDichVuService hangHoaDichVuService,
            ILoaiTienService loaiTienService,
            IDonViTinhService donViTinhService,
            IThongTinHoaDonService thongTinHoaDonService,
            ITVanService tVanService
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
            _boKyHieuHoaDonService = boKyHieuHoaDonService;
            _doiTuongService = doiTuongService;
            _hangHoaDichVuService = hangHoaDichVuService;
            _loaiTienService = loaiTienService;
            _donViTinhService = donViTinhService;
            _thongTinHoaDonService = thongTinHoaDonService;
            _tVanService = tVanService;
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
            new TrangThai(){ TrangThaiId = 8, Ten = "Hóa đơn bị điều chỉnh", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 9, Ten = "Hóa đơn bị thay thế", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 10, Ten = "Hóa đơn gốc bị thay thế", TrangThaiChaId = 9, Level = 1 },
            new TrangThai(){ TrangThaiId = 11, Ten = "Hóa đơn thay thế bị thay thế", TrangThaiChaId = 9, Level = 1 },
            new TrangThai(){ TrangThaiId = 12, Ten = "Hóa đơn hủy", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 13, Ten = "Hóa đơn gốc bị hủy", TrangThaiChaId = 12, Level = 1 },
            new TrangThai(){ TrangThaiId = 14, Ten = "Hóa đơn thay thế bị hủy", TrangThaiChaId = 12, Level = 1 },
            new TrangThai(){ TrangThaiId = -1, Ten = "Tất cả", TrangThaiChaId = null, Level = 0 },
        };

        private readonly List<TrangThai> TrangThaiGuiHoaDons = new List<TrangThai>()
        {
            new TrangThai(){ TrangThaiId = 0, Ten = "Chưa gửi cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 1, Ten = "Đang gửi cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 2, Ten = "Gửi cho khách hàng lỗi", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 3, Ten = "Đã gửi cho khách hàng", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 4, Ten = "Khách hàng đã nhận", TrangThaiChaId = null, Level = 0 },
            new TrangThai(){ TrangThaiId = 5, Ten = "Khách hàng chưa ký", TrangThaiChaId = 4, Level = 1 },
            new TrangThai(){ TrangThaiId = 6, Ten = "Khách hàng đã ký", TrangThaiChaId = 4, Level = 1 },
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

        public async Task<bool> CheckSoHoaDonAsync(long? SoHoaDon) // 1: nvk, 2: qttu
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
                await uploadFile.DeleteFileRefTypeById(id, _db);

                var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
                _db.HoaDonDienTus.Remove(entity);
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public ThamChieuModel DeleteRangeHoaDonDienTuAsync(List<HoaDonDienTuViewModel> list)
        {
            ThamChieuModel result = new ThamChieuModel
            {
                List = new List<ThamChieuModel>(),
                RemovedList = new List<ThamChieuModel>()
            };

            foreach (var item in list)
            {
                result.RemovedList.Add(new ThamChieuModel
                {
                    ChungTuId = item.HoaDonDienTuId,
                    LoaiChungTu = item.LoaiHoaDon == 1 ? BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG : BusinessOfType.HOA_DON_BAN_HANG,
                    NgayHoaDon = item.NgayHoaDon,
                    SoChungTu = item.SoHoaDon
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
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            var hasFilterDate = false;
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
            {
                fromDate = DateTime.Parse(pagingParams.FromDate);
                toDate = DateTime.Parse(pagingParams.ToDate);
                hasFilterDate = true;
            }

            //đọc ra trước các hóa đơn để lấy ra hóa đơn thay thế, hóa đơn điều chỉnh tại mỗi dòng hóa đơn đang duyệt
            //mục đích thêm code này để hiển thị cột thông báo sai sót theo yêu của a Kiên
            //cột này hiển thị ở cả 4 tab hóa đơn
            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                         select new HoaDonDienTu
                                                         {
                                                             HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                             SoHoaDon = hoaDon.SoHoaDon,
                                                             ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                             DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                             NgayHoaDon = hoaDon.NgayHoaDon,
                                                             TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                             MaCuaCQT = hoaDon.MaCuaCQT,
                                                             ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                             TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                             LanGui04 = hoaDon.LanGui04,
                                                             IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                             CreatedDate = hoaDon.CreatedDate
                                                         }).ToListAsync();

            //đọc ra thông tin hóa đơn được nhập từ phần mềm khác, (được dùng để hiển thị cột thông tin sai sót ở hóa đơn điều chỉnh); việc đọc ra bảng này vì phải truy vấn thông tin với các hóa đơn được nhập từ phần mềm khác
            List<ThongTinHoaDon> listThongTinHoaDon = await (from hoaDon in _db.ThongTinHoaDons
                                                             where listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.Id) > 0
                                                             select new ThongTinHoaDon
                                                             {
                                                                 Id = hoaDon.Id,
                                                                 TrangThaiHoaDon = hoaDon.TrangThaiHoaDon,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04
                                                             }).ToListAsync();

            //đọc ra kỳ kế toán hiện tại
            //mục đích đọc ra là để hiển thị tình trạng quá hạn/trong hạn của mỗi hóa đơn theo yêu cầu của a Kiên
            var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            IQueryable<HoaDonDienTuViewModel> query = from hd in _db.HoaDonDienTus.Where(x => !hasFilterDate || (x.NgayHoaDon.Value.Date >= fromDate && x.NgayHoaDon.Value.Date <= toDate))
                                                      join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                      join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                                                      from kh in tmpKhachHangs.DefaultIfEmpty()
                                                      join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                                                      from nv in tmpNhanViens.DefaultIfEmpty()
                                                      join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                                      from lt in tmpLoaiTiens.DefaultIfEmpty()
                                                      join cb in _db.Users on hd.CreatedBy equals cb.UserId into tmpCreatedBys
                                                      from cb in tmpCreatedBys.DefaultIfEmpty()
                                                      join mb in _db.Users on hd.ModifyBy equals mb.UserId into tmpModifyBys
                                                      from mb in tmpModifyBys.DefaultIfEmpty()
                                                      where pagingParams.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)

                                                      select new HoaDonDienTuViewModel
                                                      {
                                                          ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, listThongTinHoaDon.FirstOrDefault(x => x.Id == hd.DieuChinhChoHoaDonId)),
                                                          ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                                          HoaDonThayTheDaDuocCapMa = bkhhd.HinhThucHoaDon != HinhThucHoaDon.CoMa || (!string.IsNullOrWhiteSpace(
                                                              listHoaDonDienTu.FirstOrDefault(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId).MaCuaCQT)),
                                                          HoaDonDieuChinhDaDuocCapMa = bkhhd.HinhThucHoaDon != HinhThucHoaDon.CoMa || (!string.IsNullOrWhiteSpace(
                                                              listHoaDonDienTu.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId).MaCuaCQT)),
                                                          HoaDonDienTuId = hd.HoaDonDienTuId,
                                                          NgayHoaDon = hd.NgayHoaDon,
                                                          NgayKy = hd.NgayKy,
                                                          SoHoaDon = hd.SoHoaDon, // <Chưa cấp số>
                                                          IsCoSoHoaDon = hd.SoHoaDon.HasValue,
                                                          MaCuaCQT = bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa ? (hd.MaCuaCQT ?? "") : string.Empty, // <Chưa cấp mã>
                                                          BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                                          MauSo = bkhhd.KyHieuMauSoHoaDon + string.Empty,
                                                          KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                                                          BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                                                          {
                                                              BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                                              KyHieu = bkhhd.KyHieu,
                                                              MauHoaDonId = bkhhd.MauHoaDonId,
                                                              HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                                              TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                                              UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                                              TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                                                              TrangThaiSuDung = bkhhd.TrangThaiSuDung
                                                          },
                                                          HinhThucHoaDon = (int)bkhhd.HinhThucHoaDon,
                                                          TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                                          UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                                                          TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                                                          IsHoaDonCoMa = bkhhd.KyHieu.IsHoaDonCoMa(),
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
                                                          MaKhachHang = hd.MaKhachHang ?? kh.Ma ?? string.Empty,
                                                          TenKhachHang = hd.TenKhachHang ?? kh.Ten ?? string.Empty,
                                                          MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                                                          DiaChi = hd.DiaChi,
                                                          HinhThucThanhToanId = hd.HinhThucThanhToanId,
                                                          TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
                                                          HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang ?? string.Empty,
                                                          SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                                          EmailNguoiMuaHang = hd.EmailNguoiMuaHang ?? string.Empty,
                                                          TenNganHang = hd.TenNganHang ?? string.Empty,
                                                          SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang ?? string.Empty,
                                                          HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD ?? string.Empty,
                                                          EmailNguoiNhanHD = hd.EmailNguoiNhanHD ?? string.Empty,
                                                          SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD ?? string.Empty,
                                                          LoaiTienId = lt.LoaiTienId ?? string.Empty,
                                                          IsVND = lt == null || lt.Ma == "VND",
                                                          LoaiTien = lt != null ? new LoaiTienViewModel
                                                          {
                                                              Ma = lt.Ma,
                                                              Ten = lt.Ten
                                                          }
                                                          : null,
                                                          TyGia = hd.TyGia ?? 1,
                                                          TrangThai = hd.TrangThai,
                                                          TenTrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
                                                          TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                                                          TenTrangThaiQuyTrinh = ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription(),
                                                          MaTraCuu = hd.MaTraCuu,
                                                          TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                                          KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                                                          SoLanChuyenDoi = hd.SoLanChuyenDoi ?? 0,
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
                                                          } : null,
                                                          TaiLieuDinhKem = hd.TaiLieuDinhKem,
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
                                                                                 Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                                                 Status = tldk.Status
                                                                             })
                                                                            .ToList(),
                                                          CreatedBy = hd.CreatedBy,
                                                          CreatedDate = hd.CreatedDate,
                                                          Status = hd.Status,
                                                          NgayXoaBo = hd.NgayXoaBo,
                                                          TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                                          TongTienThanhToan = hd.TongTienThanhToan,
                                                          TongTienThanhToanQuyDoi = hd.TongTienThanhToanQuyDoi,
                                                          //HinhThucDieuChinh = GetHinhThucDieuChinh(hd, _db.HoaDonDienTus.Any(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId), _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId) || _db.BienBanDieuChinhs.Any(x => x.HoaDonBiDieuChinhId == hd.HoaDonDienTuId)),
                                                          TrangThaiThoaThuan = hd.IsLapVanBanThoaThuan == true ? "Có thỏa thuận" : "Không thỏa thuận",
                                                          IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan,
                                                          ThongTinTao = GetThongTinChung(cb, hd.CreatedDate),
                                                          ThongTinCapNhat = GetThongTinChung(mb, hd.ModifyDate),
                                                          //DaLapHoaDonThayThe = _db.HoaDonDienTus.Any(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId),
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
                                                          IsNotCreateThayThe = hd.IsNotCreateThayThe,
                                                          HinhThucXoabo = hd.HinhThucXoabo,
                                                          BackUpTrangThai = hd.BackUpTrangThai,
                                                          IdHoaDonSaiSotBiThayThe = hd.IdHoaDonSaiSotBiThayThe,
                                                          TenNguoiTao = cb.UserName ?? string.Empty,
                                                          NgayTao = hd.CreatedDate,
                                                          TenNguoiCapNhat = mb.UserName ?? string.Empty,
                                                          NgayCapNhat = hd.ModifyDate,
                                                          DaBiDieuChinh = (from hd1 in _db.HoaDonDienTus
                                                                           join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                                           where hd1.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                                           && hd.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo
                                                                           && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                                           || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                                           )
                                                                           select hd1.HoaDonDienTuId).Any(),
                                                          IsLapHoaDonThayThe = (from hd1 in _db.HoaDonDienTus
                                                                                join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                                                where hd1.ThayTheChoHoaDonId == hd.HoaDonDienTuId
                                                                                && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                                                || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                                                )
                                                                                select hd1.HoaDonDienTuId).Any(),
                                                      };

            if (pagingParams.LocHoaDonCoSaiSotChuaLapTBao04.GetValueOrDefault())
            {
                query = query.Where(x => x.ThongBaoSaiSot != null && x.ThongBaoSaiSot.TrangThaiLapVaGuiThongBao == -2);
            }

            if (string.IsNullOrEmpty(pagingParams.SortValue))
            {
                query = query.OrderBy(x => x.IsCoSoHoaDon)
                    .ThenByDescending(x => x.NgayHoaDon.Value.Date)
                    .ThenByDescending(x => x.SoHoaDon)
                    .ThenByDescending(x => x.CreatedDate);
            }

            if (pagingParams.IsChuyenDoi == true)
            {
                query = query.Where(x => (x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) &&
                                            ((x.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) ||
                                            (x.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe) ||
                                            (x.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh)));
            }

            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                string keyword = pagingParams.Keyword.ToUpper().ToTrim();

                query = query.Where(x => x.SoHoaDon.ToString().Contains(keyword) || x.SoHoaDon.ToString().ToUnSign().Contains(keyword.ToUnSign()) ||
                                      x.MaKhachHang.ToUpper().Contains(keyword) || x.MaKhachHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                      x.TenKhachHang.ToUpper().Contains(keyword) || x.TenKhachHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                      x.HoTenNguoiMuaHang.ToUpper().Contains(keyword) || x.HoTenNguoiMuaHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                      x.MaSoThue.ToUpper().Contains(keyword));
            }

            //if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
            //{
            //    DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
            //    DateTime toDate = DateTime.Parse(pagingParams.ToDate);
            //    query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
            //                            DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
            //}

            if (!string.IsNullOrEmpty(pagingParams.KhachHangId))
            {
                query = query.Where(x => x.KhachHangId == pagingParams.KhachHangId);
            }

            if (pagingParams.TrangThaiHoaDonDienTu.HasValue && pagingParams.TrangThaiHoaDonDienTu != -1)
            {
                //không phải hoá đơn điều chỉnh, hoặc chọn từng loại hóa đơn điều chỉnh
                switch (pagingParams.TrangThaiHoaDonDienTu)
                {
                    case 1:
                    case 2:
                    case 3:
                        {
                            query = query.Where(x => x.TrangThai == pagingParams.TrangThaiHoaDonDienTu);
                            break;
                        }
                    case 4:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh);
                            break;
                        }
                    case 5:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh && x.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhTang);
                            break;
                        }
                    case 6:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh && x.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhGiam);
                            break;
                        }
                    case 7:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh && x.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin);
                            break;
                        }
                    case 8:
                        {
                            query = query.Where(x => x.DaBiDieuChinh == true);
                            break;
                        }
                    case 9:
                        {
                            query = query.Where(x => x.IsLapHoaDonThayThe == true);
                            break;
                        }
                    case 10:
                        {
                            query = query.Where(x => x.IsLapHoaDonThayThe == true && string.IsNullOrEmpty(x.ThayTheChoHoaDonId));
                            break;
                        }
                    case 11:
                        {
                            query = query.Where(x => x.IsLapHoaDonThayThe == true && !string.IsNullOrEmpty(x.ThayTheChoHoaDonId));
                            break;
                        }
                    case 12:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4));
                            break;
                        }
                    case 13:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4) && string.IsNullOrEmpty(x.ThayTheChoHoaDonId));
                            break;
                        }
                    case 14:
                        {
                            query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4) && !string.IsNullOrEmpty(x.ThayTheChoHoaDonId));
                            break;
                        }
                    default: break;
                }
            }

            if (pagingParams.HinhThucHoaDon.HasValue && pagingParams.HinhThucHoaDon != -1)
            {
                query = query.Where(x => x.HinhThucHoaDon == pagingParams.HinhThucHoaDon);
            }

            if (pagingParams.UyNhiemLapHoaDon.HasValue && pagingParams.UyNhiemLapHoaDon != -1)
            {
                query = query.Where(x => x.UyNhiemLapHoaDon == pagingParams.UyNhiemLapHoaDon);
            }

            if (pagingParams.LoaiHoaDon.HasValue && pagingParams.LoaiHoaDon != -1)
            {
                query = query.Where(x => x.LoaiHoaDon == pagingParams.LoaiHoaDon);
            }

            if (pagingParams.TrangThaiPhatHanh.HasValue && pagingParams.TrangThaiPhatHanh != -1)
            {
                query = query.Where(x => x.TrangThaiQuyTrinh == pagingParams.TrangThaiPhatHanh);
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
                    query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && x.DaLapHoaDonThayThe == true);
                }
                else if (pagingParams.TrangThaiXoaBo == 2)
                {
                    query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && x.DaLapHoaDonThayThe != true);
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
                    query = query.Where(x => (x.SoHoaDon + "").ToTrim().Contains(keyword));
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
                    var keyword = timKiemTheo.TenKhachHang.ToUnSign().ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang.ToUnSign().ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUnSign().ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUnSign().ToUpper().ToTrim().Contains(keyword));
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
                        case nameof(pagingParams.Filter.NgayHoaDon):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.NgayHoaDon.Value.ToString("yyyy-MM-dd"), filterCol, FilterValueType.DateTime);
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
                        case nameof(pagingParams.Filter.MaTraCuu):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MaTraCuu, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.MaCuaCQT):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.MaCuaCQT, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.FilterThongBaoSaiSot):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.ThongBaoSaiSot?.TenTrangThai, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.TenNguoiTao):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TenNguoiTao, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.NgayTao):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.NgayTao.Value.ToString("yyyy-MM-dd"), filterCol, FilterValueType.DateTime);
                            break;
                        case nameof(pagingParams.Filter.TenNguoiCapNhat):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.TenNguoiCapNhat, filterCol, FilterValueType.String);
                            break;
                        case nameof(pagingParams.Filter.NgayCapNhat):
                            query = GenericFilterColumn<HoaDonDienTuViewModel>.Query(query, x => x.NgayCapNhat.Value.ToString("yyyy-MM-dd"), filterCol, FilterValueType.DateTime);
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

            var allHoaDonDienTuIds = query.Select(x => x.HoaDonDienTuId).ToList();

            var result = PagedList<HoaDonDienTuViewModel>.Create(query, pagingParams.PageNumber, pagingParams.PageSize);
            result.AllItemIds = allHoaDonDienTuIds;

            #region xử lý trạng thái khác
            List<string> hoaDonDienTuIds = result.Items.Select(x => x.HoaDonDienTuId).ToList();
            List<string> boKyHieuHoaDonIds = result.Items.Select(x => x.BoKyHieuHoaDonId).ToList();

            var hoaDonDieuChinh_ThayThes = await _db.HoaDonDienTus
                .Where(x => hoaDonDienTuIds.Contains(x.ThayTheChoHoaDonId) || hoaDonDienTuIds.Contains(x.DieuChinhChoHoaDonId))
                .AsNoTracking()
                .ToListAsync();

            var bienBanDieuChinhs = await _db.BienBanDieuChinhs
                .Where(x => hoaDonDienTuIds.Contains(x.HoaDonBiDieuChinhId))
                .AsNoTracking()
                .ToListAsync();

            var hoaDonDienTu_BlockPhatHanhLais = await _db.HoaDonDienTus
                .Where(x => boKyHieuHoaDonIds.Contains(x.BoKyHieuHoaDonId) && x.SoHoaDon.HasValue)
                .Select(x => new HoaDonDienTuViewModel
                {
                    BoKyHieuHoaDonId = x.BoKyHieuHoaDonId,
                    SoHoaDon = x.SoHoaDon
                })
                .ToListAsync();

            var duLieuGuiHDDTs = await (from dlghd in _db.DuLieuGuiHDDTs
                                        join dlghdct in _db.DuLieuGuiHDDTChiTiets on dlghd.DuLieuGuiHDDTId equals dlghdct.DuLieuGuiHDDTId into tmpDLGHDCTs
                                        from dlghdct in tmpDLGHDCTs.DefaultIfEmpty()
                                        select new DuLieuGuiHDDTViewModel
                                        {
                                            DuLieuGuiHDDTId = dlghd.DuLieuGuiHDDTId,
                                            HoaDonDienTuId = dlghdct != null ? dlghdct.HoaDonDienTuId : dlghd.HoaDonDienTuId
                                        })
                                        .Where(x => hoaDonDienTuIds.Contains(x.HoaDonDienTuId))
                                        .GroupBy(x => x.HoaDonDienTuId)
                                        .Select(x => new DuLieuGuiHDDTViewModel
                                        {
                                            HoaDonDienTuId = x.Key,
                                            Count = x.Count()
                                        })
                                        .ToListAsync();

            foreach (var item in result.Items)
            {
                item.DaLapHoaDonThayThe = hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == item.HoaDonDienTuId);
                item.SoLanGuiCQT = duLieuGuiHDDTs.Where(x => x.HoaDonDienTuId == item.HoaDonDienTuId).Select(x => x.Count).FirstOrDefault();
                item.HinhThucDieuChinh = GetHinhThucDieuChinh(item, hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == item.HoaDonDienTuId), hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == item.HoaDonDienTuId) || bienBanDieuChinhs.Any(x => x.HoaDonBiDieuChinhId == item.HoaDonDienTuId));
                item.IsLapHoaDonThayThe = (item.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (item.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && item.DaLapHoaDonThayThe != true;
                item.IsLapHoaDonDieuChinh = (item.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && (item.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (item.TrangThaiGuiHoaDon >= (int)TrangThaiGuiHoaDon.DaGui) && !hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == item.HoaDonDienTuId);

                if (!item.NgayKy.HasValue ||
                    item.NgayKy.Value.Date != DateTime.Now.Date ||
                    item.IsHoaDonCoMa != true ||
                    ((item.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.GuiLoi) && (item.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa)))
                {
                    item.BlockPhatHanhLai = true;
                }
                else
                {
                    if (hoaDonDienTu_BlockPhatHanhLais.Any(x => x.SoHoaDon > item.SoHoaDon && x.BoKyHieuHoaDonId == item.BoKyHieuHoaDonId))
                    {
                        item.BlockPhatHanhLai = true;
                    }
                }
            }
            #endregion

            return result;
        }

        public async Task<HoaDonDienTuViewModel> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || id == "null" || id == "undefined") return null;

            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                         select new HoaDonDienTu
                                                         {
                                                             HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                             SoHoaDon = hoaDon.SoHoaDon,
                                                             ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                             DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                             NgayHoaDon = hoaDon.NgayHoaDon,
                                                             TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                             MaCuaCQT = hoaDon.MaCuaCQT,
                                                             ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                             TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                             LanGui04 = hoaDon.LanGui04,
                                                             IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                             CreatedDate = hoaDon.CreatedDate
                                                         }).ToListAsync();

            //đọc ra thông tin hóa đơn được nhập từ phần mềm khác, (được dùng để hiển thị cột thông tin sai sót ở hóa đơn điều chỉnh); việc đọc ra bảng này vì phải truy vấn thông tin với các hóa đơn được nhập từ phần mềm khác
            List<ThongTinHoaDon> listThongTinHoaDon = await (from hoaDon in _db.ThongTinHoaDons
                                                             where listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.Id) > 0
                                                             select new ThongTinHoaDon
                                                             {
                                                                 Id = hoaDon.Id,
                                                                 TrangThaiHoaDon = hoaDon.TrangThaiHoaDon,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04
                                                             }).ToListAsync();


            var query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                            //join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                            //from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        join bbdc_dc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc_dc.HoaDonDieuChinhId into tmpBienBanDieuChinh_DCs
                        from bbdc_dc in tmpBienBanDieuChinh_DCs.DefaultIfEmpty()
                        where hd.HoaDonDienTuId == id
                        select new HoaDonDienTuViewModel
                        {
                            ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, listThongTinHoaDon.FirstOrDefault(x => x.Id == hd.DieuChinhChoHoaDonId)),
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            HoaDonThayTheDaDuocCapMa = bkhhd.HinhThucHoaDon != HinhThucHoaDon.CoMa || (!string.IsNullOrWhiteSpace(
                                                              listHoaDonDienTu.FirstOrDefault(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId).MaCuaCQT)),
                            HoaDonDieuChinhDaDuocCapMa = bkhhd.HinhThucHoaDon != HinhThucHoaDon.CoMa || (!string.IsNullOrWhiteSpace(
                                                              listHoaDonDienTu.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId).MaCuaCQT)),

                            DaDieuChinh = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId),
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                                TrangThaiSuDung = bkhhd.TrangThaiSuDung
                            },
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauHoaDonId = mhd != null ? mhd.MauHoaDonId : null,
                            MauHoaDon = mhd != null ? new MauHoaDonViewModel
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                LoaiHoaDon = mhd.LoaiHoaDon,
                                LoaiThueGTGT = mhd.LoaiThueGTGT,
                            } : null,
                            MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                            KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                            KhachHangId = kh.DoiTuongId,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            DiaChi = hd.DiaChi,
                            MaNhanVienBanHang = hd.MaNhanVienBanHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            KhachHang = kh != null ?
                                        new DoiTuongViewModel
                                        {
                                            DoiTuongId = kh.DoiTuongId,
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
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
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
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            TenTrangThaiQuyTrinh = ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription(),
                            MaTraCuu = hd.MaTraCuu,
                            IsBuyerSigned = hd.IsBuyerSigned,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            TrangThaiGuiHoaDonNhap = hd.TrangThaiGuiHoaDonNhap,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi ?? 0,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            NgayXoaBo = hd.NgayXoaBo,
                            SoCTXoaBo = hd.SoCTXoaBo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
                            HinhThucXoabo = hd.HinhThucXoabo,
                            BackUpTrangThai = hd.BackUpTrangThai,
                            IdHoaDonSaiSotBiThayThe = hd.IdHoaDonSaiSotBiThayThe,
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
                            LyDoBiDieuChinh = bbdc != null ? bbdc.LyDoDieuChinh : null,
                            NhanVienBanHangId = hd.NhanVienBanHangId,
                            IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan,
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
                            TrangThaiBienBanDieuChinh = bbdc_dc != null ? bbdc_dc.TrangThaiBienBan : (bbdc != null ? bbdc.TrangThaiBienBan : (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                            ThoiHanThanhToan = hd.ThoiHanThanhToan,
                            DiaChiGiaoHang = hd.DiaChiGiaoHang,
                            BienBanDieuChinhId = bbdc_dc != null ? bbdc_dc.BienBanDieuChinhId : (bbdc != null ? bbdc.BienBanDieuChinhId : null),
                            //LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
                            GhiChuThayTheSaiSot = hd.GhiChuThayTheSaiSot,
                            HoaDonChiTiets = (
                                               from hdct in _db.HoaDonDienTuChiTiets
                                               join hd in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                               from hd in tmpHoaDons.DefaultIfEmpty()
                                               join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                               from vt in tmpHangHoas.DefaultIfEmpty()
                                               join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                               from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                               where hdct.HoaDonDienTuId == id
                                               orderby hdct.CreatedDate
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   STT = hdct.STT,
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   TinhChat = hdct.TinhChat,
                                                   TenTinhChat = ((TChat)(hdct.TinhChat)).GetDescription(),
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
                                                   TyLePhanTramDoanhThu = hdct.TyLePhanTramDoanhThu,
                                                   TienGiam = hdct.TienGiam ?? 0,
                                                   TienGiamQuyDoi = hdct.TienGiamQuyDoi ?? 0,
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
                            TongTienGiam = hd.TongTienGiam ?? 0,
                            TongTienGiamQuyDoi = hd.TongTienGiamQuyDoi ?? 0,
                            CreatedBy = hd.CreatedBy,
                            CreatedDate = hd.CreatedDate,
                            Status = hd.Status,
                            TrangThaiThoaThuan = hd.IsLapVanBanThoaThuan == true ? "Có thỏa thuận" : "Không thỏa thuận",
                            MaCuaCQT = hd.MaCuaCQT,
                            NgayKy = hd.NgayKy,
                            LoaiChietKhau = hd.LoaiChietKhau,
                            TyLeChietKhau = hd.TyLeChietKhau,
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon,
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            LoaiApDungHoaDonDieuChinh = 1,
                            IsGiamTheoNghiQuyet = hd.IsGiamTheoNghiQuyet,
                            TyLePhanTramDoanhThu = hd.TyLePhanTramDoanhThu ?? 0,
                            IsThongTinNguoiBanHoacNguoiMua = hd.IsThongTinNguoiBanHoacNguoiMua,
                            IsTheHienLyDoTrenHoaDon = hd.IsTheHienLyDoTrenHoaDon,
                            TrangThaiLanDieuChinhGanNhat = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId) ? _db.HoaDonDienTus.Where(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId).OrderByDescending(x => x.CreatedDate).FirstOrDefault().TrangThaiQuyTrinh : (int?)null,
                            MauSoHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                              join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                              where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                              orderby hddt.CreatedDate descending
                                                              select bkh.KyHieuMauSoHoaDon).FirstOrDefault(),
                            KyHieuHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                               join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                               where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                               orderby hddt.CreatedDate descending
                                                               select bkh.KyHieuHoaDon).FirstOrDefault(),
                            SoHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                           where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                           orderby hddt.CreatedDate descending
                                                           select hddt.SoHoaDon).FirstOrDefault(),
                            NgayHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                             where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                             orderby hddt.CreatedDate descending
                                                             select hddt.NgayHoaDon).FirstOrDefault(),
                        };

            var result = await query.FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }

            #region xử lý trạng thái khác

            var hoaDonDieuChinh_ThayThes = await _db.HoaDonDienTus
            .Where(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId || x.ThayTheChoHoaDonId == result.HoaDonDienTuId)
            .AsNoTracking()
            .ToListAsync();

            var bienBanDieuChinhs = await _db.BienBanDieuChinhs
                .Where(x => x.HoaDonBiDieuChinhId == result.HoaDonDienTuId)
                .AsNoTracking()
                .ToListAsync();

            result.DaLapHoaDonThayThe = hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == result.HoaDonDienTuId);
            result.HinhThucDieuChinh = GetHinhThucDieuChinh(result, hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == result.HoaDonDienTuId), hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId) || bienBanDieuChinhs.Any(x => x.HoaDonBiDieuChinhId == result.HoaDonDienTuId));
            result.IsLapHoaDonThayThe = (result.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (result.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && result.DaLapHoaDonThayThe != true;
            result.IsLapHoaDonDieuChinh = (result.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && (result.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (result.TrangThaiGuiHoaDon >= (int)TrangThaiGuiHoaDon.DaGui) && !hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId);
            #endregion

            if (!string.IsNullOrEmpty(result.LyDoDieuChinh) && result.LyDoDieuChinh.StartsWith("{"))
            {
                result.LyDoDieuChinhModel = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(result.LyDoDieuChinh);
            }
            else
            {
                if (string.IsNullOrEmpty(result.LyDoDieuChinh))
                {
                    if (!string.IsNullOrEmpty(result.BienBanDieuChinhId))
                    {
                        var bbdc = _db.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == result.BienBanDieuChinhId);
                        result.LyDoDieuChinhModel = new LyDoDieuChinhModel { LyDo = bbdc.LyDoDieuChinh };
                    }
                    else result.LyDoDieuChinhModel = null;
                }
                else result.LyDoDieuChinhModel = new LyDoDieuChinhModel { LyDo = result.LyDoDieuChinh };
            }

            if (result.LyDoDieuChinhModel != null)
            {
                result.LyDoDieuChinhModel.DieuChinhChoHoaDonId = result.DieuChinhChoHoaDonId;
            }
            result.TenTrangThaiLanDieuChinhGanNhat = result.TrangThaiLanDieuChinhGanNhat.HasValue ? ((TrangThaiQuyTrinh)result.TrangThaiLanDieuChinhGanNhat.Value).GetDescription() : string.Empty;
            return result;
        }

        public async Task<HoaDonDienTuViewModel> GetByIdAsync(long SoHoaDon, string KyHieuHoaDon, string KyHieuMauSoHoaDon)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            var query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                            //join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                            //from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        where (hd.SoHoaDon) == SoHoaDon && bkhhd.KyHieuHoaDon == KyHieuHoaDon && bkhhd.KyHieuMauSoHoaDon == int.Parse(KyHieuMauSoHoaDon)
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            DaDieuChinh = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId),
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                            },
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauHoaDonId = mhd != null ? mhd.MauHoaDonId : null,
                            MauHoaDon = mhd != null ? new MauHoaDonViewModel
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                LoaiHoaDon = mhd.LoaiHoaDon,
                                LoaiThueGTGT = mhd.LoaiThueGTGT,
                            } : null,
                            MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                            KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                            KhachHangId = kh.DoiTuongId,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            DiaChi = hd.DiaChi,
                            MaNhanVienBanHang = hd.MaNhanVienBanHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            KhachHang = kh != null ?
                                        new DoiTuongViewModel
                                        {
                                            DoiTuongId = kh.DoiTuongId,
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
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
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
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            TenTrangThaiQuyTrinh = ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription(),
                            MaTraCuu = hd.MaTraCuu,
                            IsBuyerSigned = hd.IsBuyerSigned,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            TrangThaiGuiHoaDonNhap = hd.TrangThaiGuiHoaDonNhap,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            NgayXoaBo = hd.NgayXoaBo,
                            SoCTXoaBo = hd.SoCTXoaBo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
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
                            IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan,
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
                            TrangThaiBienBanDieuChinh = bbdc != null ? bbdc.TrangThaiBienBan : null,
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
                                               where hdct.HoaDonDienTuId == hd.HoaDonDienTuId
                                               orderby hdct.CreatedDate
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   STT = hdct.STT,
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   TinhChat = hdct.TinhChat,
                                                   TenTinhChat = ((TChat)(hdct.TinhChat)).GetDescription(),
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
                            TrangThaiThoaThuan = hd.IsLapVanBanThoaThuan == true ? "Có thỏa thuận" : "Không thỏa thuận",
                            MaCuaCQT = hd.MaCuaCQT,
                            NgayKy = hd.NgayKy,
                            LoaiChietKhau = hd.LoaiChietKhau,
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon,
                            HinhThucXoabo = hd.HinhThucXoabo,
                            BackUpTrangThai = hd.BackUpTrangThai,
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                        };

            var result = await query.FirstOrDefaultAsync();

            #region xử lý trạng thái khác

            var hoaDonDieuChinh_ThayThes = await _db.HoaDonDienTus
                .Where(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId || x.ThayTheChoHoaDonId == result.HoaDonDienTuId)
                .AsNoTracking()
                .ToListAsync();

            var bienBanDieuChinhs = await _db.BienBanDieuChinhs
                .Where(x => x.HoaDonBiDieuChinhId == result.HoaDonDienTuId)
                .AsNoTracking()
                .ToListAsync();

            result.DaLapHoaDonThayThe = hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == result.HoaDonDienTuId);
            result.HinhThucDieuChinh = GetHinhThucDieuChinh(result, hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == result.HoaDonDienTuId), hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId) || bienBanDieuChinhs.Any(x => x.HoaDonBiDieuChinhId == result.HoaDonDienTuId));
            result.IsLapHoaDonThayThe = (result.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (result.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && result.DaLapHoaDonThayThe != true;
            result.IsLapHoaDonDieuChinh = (result.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && (result.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (result.TrangThaiGuiHoaDon >= (int)TrangThaiGuiHoaDon.DaGui) && !hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId);
            #endregion

            return result;
        }


        public async Task<HoaDonDienTuViewModel> InsertAsync(HoaDonDienTuViewModel model)
        {
            model.HoaDonDienTuId = Guid.NewGuid().ToString();
            model.HoaDonChiTiets = null;

            HoaDonDienTu entity = _mp.Map<HoaDonDienTu>(model);

            entity.TrangThai = (int)TrangThaiHoaDon.HoaDonGoc;
            entity.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.ChuaKyDienTu;
            entity.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.ChuaGui;

            if (!string.IsNullOrEmpty(model.BienBanDieuChinhId))
            {
                BienBanDieuChinh bienBanDieuChinh = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == model.BienBanDieuChinhId);
                if (bienBanDieuChinh != null)
                {
                    bienBanDieuChinh.HoaDonDieuChinhId = entity.HoaDonDienTuId;
                }

                // copy tài liệu đính kèm biên bản -> hóa đơn điều chỉnh
                var addedTaiLieuDinhKiems = await _db.TaiLieuDinhKems.Where(x => x.NghiepVuId == model.BienBanDieuChinhId)
                    .Select(x => new TaiLieuDinhKem
                    {
                        Status = true,
                        LoaiNghiepVu = RefType.HoaDonDienTu,
                        TenGoc = x.TenGoc,
                        TenGuid = x.TenGuid,
                        NghiepVuId = model.HoaDonDienTuId
                    })
                    .ToListAsync();

                await _db.TaiLieuDinhKems.AddRangeAsync(addedTaiLieuDinhKiems);
            }

            entity.NgayLap = DateTime.Now;

            if (!string.IsNullOrEmpty(entity.LyDoThayThe))
            {
                entity.TrangThai = (int)TrangThaiHoaDon.HoaDonThayThe;
            }

            if (!string.IsNullOrEmpty(entity.LyDoDieuChinh))
            {
                entity.TrangThai = (int)TrangThaiHoaDon.HoaDonDieuChinh;

                if (entity.LoaiDieuChinh != 3)
                {
                    entity.IsThongTinNguoiBanHoacNguoiMua = false;
                    entity.IsTheHienLyDoTrenHoaDon = false;
                }
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

            if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh && model.LoaiDieuChinh != 3)
            {
                model.IsThongTinNguoiBanHoacNguoiMua = false;
                model.IsTheHienLyDoTrenHoaDon = false;
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
                            item.SoLuong = hoaDons.Count(x => x.TrangThaiQuyTrinh == item.TrangThaiId || !x.TrangThaiQuyTrinh.HasValue);
                        }
                        else
                            item.SoLuong = hoaDons.Count(x => x.TrangThaiQuyTrinh == item.TrangThaiId);
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
            .Select(hd => new HoaDonDienTuViewModel
            {
                HoaDonDienTuId = hd.HoaDonDienTuId,
                NgayHoaDon = hd.NgayHoaDon,
                NgayLap = hd.CreatedDate,
                SoHoaDon = hd.SoHoaDon,
                IsCoSoHoaDon = hd.SoHoaDon.HasValue,
                MaCuaCQT = hd.MaCuaCQT,
                BoKyHieuHoaDon = _mp.Map<BoKyHieuHoaDonViewModel>(_db.BoKyHieuHoaDons.FirstOrDefault(x => x.BoKyHieuHoaDonId == hd.BoKyHieuHoaDonId)),
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
                TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                MaTraCuu = hd.MaTraCuu,
                TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                SoLanChuyenDoi = hd.SoLanChuyenDoi,
                LyDoXoaBo = hd.LyDoXoaBo,
                LoaiHoaDon = hd.LoaiHoaDon,
                LoaiChungTu = hd.LoaiChungTu,
                TongTienThanhToan = hd.TongTienThanhToan
            });

            if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
            {
                DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
                DateTime toDate = DateTime.Parse(pagingParams.ToDate);
                query = query.Where(x => DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                        DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate);
            }

            query = query.OrderBy(x => x.IsCoSoHoaDon)
                .ThenByDescending(x => x.NgayHoaDon.Value.Date)
                .ThenByDescending(x => x.SoHoaDon)
                .ThenByDescending(x => x.CreatedDate);
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
                List<HoaDonDienTuViewModel> list = await query.ToListAsync();
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
                    worksheet.Cells[idx, 3].Value = it.SoHoaDon;
                    worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MaCuaCQT) ? it.MaCuaCQT : "<Chưa cấp mã>";
                    worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.BoKyHieuHoaDon != null ? it.BoKyHieuHoaDon.KyHieuMauSoHoaDon.ToString() : string.Empty);
                    worksheet.Cells[idx, 6].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.BoKyHieuHoaDon != null ? it.BoKyHieuHoaDon.KyHieuHoaDon : string.Empty);
                    worksheet.Cells[idx, 7].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.KhachHang != null ? it.KhachHang.Ma : string.Empty);
                    worksheet.Cells[idx, 8].Value = !string.IsNullOrEmpty(it.TenKhachHang) ? it.TenKhachHang : (it.KhachHang != null ? it.KhachHang.Ten : string.Empty);
                    worksheet.Cells[idx, 9].Value = !string.IsNullOrEmpty(it.DiaChi) ? it.DiaChi : (it.KhachHang != null ? it.KhachHang.DiaChi : string.Empty);
                    worksheet.Cells[idx, 10].Value = !string.IsNullOrEmpty(it.MaSoThue) ? it.MaSoThue : (it.KhachHang != null ? it.KhachHang.MaSoThue : string.Empty);
                    worksheet.Cells[idx, 11].Value = !string.IsNullOrEmpty(it.HoTenNguoiMuaHang) ? it.HoTenNguoiMuaHang : (it.KhachHang != null ? it.KhachHang.HoTenNguoiMuaHang : string.Empty);
                    worksheet.Cells[idx, 12].Value = !string.IsNullOrEmpty(it.TenNhanVienBanHang) ? it.TenNhanVienBanHang : it.NhanVienBanHang != null ? it.NhanVienBanHang.Ten : string.Empty;
                    worksheet.Cells[idx, 13].Value = it.TongTienThanhToan;
                    worksheet.Cells[idx, 14].Value = it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng";
                    worksheet.Cells[idx, 15].Value = TrangThaiHoaDons.Where(x => x.TrangThaiId == it.TrangThai).Select(x => x.Ten).FirstOrDefault();
                    worksheet.Cells[idx, 16].Value = (it.TrangThaiQuyTrinh == 0 ? "Chưa phát hành" : (it.TrangThaiQuyTrinh == 1 ? "Đang phát hành" : (it.TrangThaiQuyTrinh == 2 ? "Phát hành lỗi" : "Đã phát hành")));
                    worksheet.Cells[idx, 17].Value = it.MaTraCuu;
                    worksheet.Cells[idx, 18].Value = TrangThaiGuiHoaDons.Where(x => x.TrangThaiId == it.TrangThaiGuiHoaDon).Select(x => x.Ten).FirstOrDefault();
                    worksheet.Cells[idx, 19].Value = it.KhachHang != null ? it.KhachHang.HoTenNguoiNhanHD : string.Empty;
                    worksheet.Cells[idx, 20].Value = it.KhachHang != null ? it.KhachHang.EmailNguoiNhanHD : string.Empty;
                    worksheet.Cells[idx, 21].Value = it.KhachHang != null ? it.KhachHang.SoDienThoaiNguoiNhanHD : string.Empty;
                    worksheet.Cells[idx, 22].Value = it.KhachHangDaNhan.HasValue ? (it.KhachHangDaNhan.Value ? "Đã nhận" : "Chưa nhận") : "Chưa nhận";
                    worksheet.Cells[idx, 23].Value = it.SoLanChuyenDoi;
                    worksheet.Cells[idx, 24].Value = it.LyDoXoaBo;
                    worksheet.Cells[idx, 25].Value = it.LoaiChungTu == 0 ? "Mua hàng" : it.LoaiChungTu == 1 ? "Bán hàng" : it.LoaiChungTu == 2 ? "Kho" : "Hóa đơn bách khoa";
                    worksheet.Cells[idx, 26].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                    worksheet.Cells[idx, 27].Value = it.NguoiLap != null ? it.NguoiLap.Ten : string.Empty;

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
            IQueryable<HoaDonDienTuViewModel> query = null;
            if (!string.IsNullOrEmpty(@params.HoaDonDienTuId))
            {
                query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join usr in _db.Users on hd.CreatedBy equals usr.UserId into tmpUser
                        from usr in tmpUser.DefaultIfEmpty()
                        orderby hd.NgayHoaDon, bkhhd.KyHieuHoaDon, bkhhd.KyHieuMauSoHoaDon, hd.SoHoaDon
                        where hd.HoaDonDienTuId == @params.HoaDonDienTuId
                        select new HoaDonDienTuViewModel()
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            NgayHoaDon = hd.NgayHoaDon,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                            },
                            HinhThucHoaDon = (int)bkhhd.HinhThucHoaDon,
                            TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauSo = bkhhd.KyHieuMauSoHoaDon.ToString(),
                            KyHieu = bkhhd.KyHieuHoaDon,
                            KhachHangId = hd.KhachHangId ?? string.Empty,
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
                            KhachHang = new DoiTuongViewModel
                            {
                                DoiTuongId = kh.DoiTuongId,
                                Ma = kh.Ma,
                                Ten = kh.Ten,
                                DiaChi = kh.DiaChi,
                                MaSoThue = kh.MaSoThue,
                                HoTenNguoiMuaHang = kh.HoTenNguoiMuaHang,
                                SoDienThoaiNguoiMuaHang = kh.SoDienThoaiNguoiMuaHang,
                                SoDienThoaiNguoiNhanHD = kh.SoDienThoaiNguoiMuaHang
                            },
                            MaSoThue = hd.MaSoThue,
                            TenKhachHang = hd.TenKhachHang,
                            EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                            TenNganHang = hd.TenNganHang,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                            SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                            NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                            NhanVienBanHang = new DoiTuongViewModel
                            {
                                DoiTuongId = nv.DoiTuongId,
                                Ma = nv.Ma,
                                Ten = nv.DoiTuongId
                            },
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            LoaiTienId = hd.LoaiTienId ?? string.Empty,
                            LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
                            TyGia = hd.TyGia ?? 1,
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            MaTraCuu = hd.MaTraCuu,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            LoaiChungTu = hd.LoaiChungTu,
                            TyLePhanTramDoanhThu = Math.Round(hd.TyLePhanTramDoanhThu ?? 0, 2),
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
                            orderby vt.CreatedDate
                            select new HoaDonDienTuChiTietViewModel
                            {
                                HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                                HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                                HangHoaDichVu = vt != null ?
                                new HangHoaDichVuViewModel
                                {
                                    HangHoaDichVuId = vt.HangHoaDichVuId,
                                    Ma = vt.Ma,
                                    Ten = vt.Ten
                                } : null,
                                MaHang = hdct.MaHang,
                                TenHang = hdct.TenHang,
                                TinhChat = hdct.TinhChat,
                                DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                DonViTinh = dvt != null ?
                                new DonViTinhViewModel
                                {
                                    DonViTinhId = dvt.DonViTinhId,
                                    Ten = dvt.Ten
                                } : null,
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
                                TienGiam = hdct.TienGiam,
                                TienGiamQuyDoi = hdct.TienGiamQuyDoi,
                                TongTienThanhToan = hdct.TongTienThanhToan,
                                TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi,
                                SoLo = hdct.SoLo,
                                HanSuDung = hdct.HanSuDung,
                                SoKhung = hdct.SoKhung,
                                SoMay = hdct.SoMay
                            })
                            .ToList(),
                            DaBiDieuChinh = (from hd1 in _db.HoaDonDienTus
                                             join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                             where hd1.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                             && hd.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo
                                             && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                             || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                             )
                                             select hd1.HoaDonDienTuId).Any(),
                            IsLapHoaDonThayThe = (from hd1 in _db.HoaDonDienTus
                                                  join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                  where hd1.ThayTheChoHoaDonId == hd.HoaDonDienTuId
                                                  && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                  || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                  )
                                                  select hd1.HoaDonDienTuId).Any(),
                            ActionUser = _mp.Map<UserViewModel>(usr)
                        };
            }
            else if (@params.HoaDonDienTuIds != null && @params.HoaDonDienTuIds.Any())
            {
                query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join usr in _db.Users on hd.CreatedBy equals usr.UserId into tmpUser
                        from usr in tmpUser.DefaultIfEmpty()
                        where @params.HoaDonDienTuIds.Contains(hd.HoaDonDienTuId)
                        orderby hd.NgayHoaDon, bkhhd.KyHieuHoaDon, bkhhd.KyHieuMauSoHoaDon, hd.SoHoaDon
                        select new HoaDonDienTuViewModel()
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            NgayHoaDon = hd.NgayHoaDon,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                            },
                            HinhThucHoaDon = (int)bkhhd.HinhThucHoaDon,
                            TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauSo = bkhhd.KyHieuMauSoHoaDon.ToString(),
                            KyHieu = bkhhd.KyHieuHoaDon,
                            KhachHangId = hd.KhachHangId ?? string.Empty,
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
                            KhachHang = new DoiTuongViewModel
                            {
                                DoiTuongId = kh.DoiTuongId,
                                Ma = kh.Ma,
                                Ten = kh.Ten,
                                DiaChi = kh.DiaChi,
                                MaSoThue = kh.MaSoThue,
                                HoTenNguoiMuaHang = kh.HoTenNguoiMuaHang,
                                SoDienThoaiNguoiMuaHang = kh.SoDienThoaiNguoiMuaHang,
                                SoDienThoaiNguoiNhanHD = kh.SoDienThoaiNguoiMuaHang
                            },
                            MaSoThue = hd.MaSoThue,
                            TenKhachHang = hd.TenKhachHang,
                            EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                            TenNganHang = hd.TenNganHang,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                            SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                            NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                            NhanVienBanHang = new DoiTuongViewModel
                            {
                                DoiTuongId = nv.DoiTuongId,
                                Ma = nv.Ma,
                                Ten = nv.DoiTuongId
                            },
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            LoaiTienId = hd.LoaiTienId ?? string.Empty,
                            LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
                            TyGia = hd.TyGia ?? 1,
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            MaTraCuu = hd.MaTraCuu,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            LoaiChungTu = hd.LoaiChungTu,
                            TyLePhanTramDoanhThu = Math.Round(hd.TyLePhanTramDoanhThu ?? 0, 2),
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
                            orderby vt.CreatedDate
                            select new HoaDonDienTuChiTietViewModel
                            {
                                HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                                HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                                HangHoaDichVu = vt != null ?
                                new HangHoaDichVuViewModel
                                {
                                    HangHoaDichVuId = vt.HangHoaDichVuId,
                                    Ma = vt.Ma,
                                    Ten = vt.Ten
                                } : null,
                                MaHang = hdct.MaHang,
                                TenHang = hdct.TenHang,
                                TinhChat = hdct.TinhChat,
                                DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                DonViTinh = dvt != null ?
                                new DonViTinhViewModel
                                {
                                    DonViTinhId = dvt.DonViTinhId,
                                    Ten = dvt.Ten
                                } : null,
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
                                TienGiam = hdct.TienGiam,
                                TienGiamQuyDoi = hdct.TienGiamQuyDoi,
                                TongTienThanhToan = hdct.TongTienThanhToan,
                                TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi,
                                SoLo = hdct.SoLo,
                                HanSuDung = hdct.HanSuDung,
                                SoKhung = hdct.SoKhung,
                                SoMay = hdct.SoMay
                            })
                            .ToList(),
                            DaBiDieuChinh = (from hd1 in _db.HoaDonDienTus
                                             join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                             where hd1.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                             && hd.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo
                                             && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                             || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                             )
                                             select hd1.HoaDonDienTuId).Any(),
                            IsLapHoaDonThayThe = (from hd1 in _db.HoaDonDienTus
                                                  join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                  where hd1.ThayTheChoHoaDonId == hd.HoaDonDienTuId
                                                  && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                  || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                  )
                                                  select hd1.HoaDonDienTuId).Any(),
                            ActionUser = _mp.Map<UserViewModel>(usr)
                        };
            }
            else
            {
                DateTime fromDate = DateTime.Parse(@params.TuNgay);
                DateTime toDate = DateTime.Parse(@params.DenNgay);
                query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join usr in _db.Users on hd.CreatedBy equals usr.UserId into tmpUser
                        from usr in tmpUser.DefaultIfEmpty()
                        orderby hd.NgayHoaDon, bkhhd.KyHieuHoaDon, bkhhd.KyHieuMauSoHoaDon, hd.SoHoaDon
                        where (hd.NgayHoaDon.Value >= fromDate && hd.NgayHoaDon.Value <= toDate)
                        && (@params.HinhThucHoaDon == (int)HinhThucHoaDon.TatCa || bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon)
                        && (@params.UyNhiemLapHoaDon == (int)HinhThucHoaDon.TatCa || bkhhd.UyNhiemLapHoaDon == (UyNhiemLapHoaDon)@params.UyNhiemLapHoaDon)
                        && (@params.LoaiHoaDon.Contains((int)LoaiHoaDon.TatCa) || @params.LoaiHoaDon.Contains(hd.LoaiHoaDon))
                        && (@params.TrangThaiQuyTrinh.Contains((int)TrangThaiQuyTrinh.TatCa) || @params.TrangThaiQuyTrinh.Contains(hd.TrangThaiQuyTrinh.Value))
                        && (@params.TrangThaiGuiHoaDon.Contains(-1) || @params.TrangThaiHoaDon.Contains(hd.TrangThaiGuiHoaDon.Value))
                        && (@params.TrangThaiChuyenDoi == -1 || @params.TrangThaiChuyenDoi == 0 ? hd.SoLanChuyenDoi == 0 : hd.SoLanChuyenDoi > 0)
                        && (@params.KhachHangId.Contains("-1") || @params.KhachHangId.Contains(hd.KhachHangId))
                        && (@params.BoKyHieuHoaDonId.Contains("-1") || @params.BoKyHieuHoaDonId.Contains(hd.BoKyHieuHoaDonId))
                        select new HoaDonDienTuViewModel()
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            NgayHoaDon = hd.NgayHoaDon,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                            },
                            HinhThucHoaDon = (int)bkhhd.HinhThucHoaDon,
                            TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauSo = bkhhd.KyHieuMauSoHoaDon.ToString(),
                            KyHieu = bkhhd.KyHieuHoaDon,
                            KhachHangId = hd.KhachHangId ?? string.Empty,
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
                            KhachHang = new DoiTuongViewModel
                            {
                                DoiTuongId = kh.DoiTuongId,
                                Ma = kh.Ma,
                                Ten = kh.Ten,
                                DiaChi = kh.DiaChi,
                                MaSoThue = kh.MaSoThue,
                                HoTenNguoiMuaHang = kh.HoTenNguoiMuaHang,
                                SoDienThoaiNguoiMuaHang = kh.SoDienThoaiNguoiMuaHang,
                                SoDienThoaiNguoiNhanHD = kh.SoDienThoaiNguoiMuaHang
                            },
                            MaSoThue = hd.MaSoThue,
                            TenKhachHang = hd.TenKhachHang,
                            EmailNguoiMuaHang = hd.EmailNguoiMuaHang,
                            TenNganHang = hd.TenNganHang,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang,
                            SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang,
                            NhanVienBanHangId = hd.NhanVienBanHangId ?? string.Empty,
                            NhanVienBanHang = new DoiTuongViewModel
                            {
                                DoiTuongId = nv.DoiTuongId,
                                Ma = nv.Ma,
                                Ten = nv.DoiTuongId
                            },
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            LoaiTienId = hd.LoaiTienId ?? string.Empty,
                            LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hd.LoaiTienId)),
                            TyGia = hd.TyGia ?? 1,
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            MaTraCuu = hd.MaTraCuu,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            LoaiChungTu = hd.LoaiChungTu,
                            TyLePhanTramDoanhThu = Math.Round(hd.TyLePhanTramDoanhThu ?? 0, 2),
                            TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien - x.TienChietKhau + x.TienThueGTGT),
                            DaBiDieuChinh = (from hd1 in _db.HoaDonDienTus
                                             join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                             where hd1.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                             && hd.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo
                                             && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                             || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                             )
                                             select hd1.HoaDonDienTuId).Any(),
                            IsLapHoaDonThayThe = (from hd1 in _db.HoaDonDienTus
                                                  join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                  where hd1.ThayTheChoHoaDonId == hd.HoaDonDienTuId
                                                  && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                  || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && (hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hd1.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                  )
                                                  select hd1.HoaDonDienTuId).Any(),
                            HoaDonChiTiets = (
                                from hdct in _db.HoaDonDienTuChiTiets
                                join hddt in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hddt.HoaDonDienTuId into tmpHoaDons
                                from hddt in tmpHoaDons.DefaultIfEmpty()
                                join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                from vt in tmpHangHoas.DefaultIfEmpty()
                                join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                where hdct.HoaDonDienTuId == hd.HoaDonDienTuId
                                orderby hdct.CreatedDate
                                select new HoaDonDienTuChiTietViewModel
                                {
                                    HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                    HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                                    HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                                    HangHoaDichVu = vt != null ?
                                    new HangHoaDichVuViewModel
                                    {
                                        HangHoaDichVuId = vt.HangHoaDichVuId,
                                        Ma = vt.Ma,
                                        Ten = vt.Ten
                                    } : null,
                                    MaHang = hdct.MaHang,
                                    TenHang = hdct.TenHang,
                                    TinhChat = hdct.TinhChat,
                                    DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                    DonViTinh = dvt != null ?
                                    new DonViTinhViewModel
                                    {
                                        DonViTinhId = dvt.DonViTinhId,
                                        Ten = dvt.Ten
                                    } : null,
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
                                    TienGiam = hdct.TienGiam,
                                    TienGiamQuyDoi = hdct.TienGiamQuyDoi,
                                    TongTienThanhToan = hdct.TongTienThanhToan,
                                    TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi,
                                    SoLo = hdct.SoLo,
                                    HanSuDung = hdct.HanSuDung,
                                    SoKhung = hdct.SoKhung,
                                    SoMay = hdct.SoMay
                                })
                                .ToList(),
                            ActionUser = _mp.Map<UserViewModel>(usr)
                        };
            }

            if (@params.TrangThaiHoaDon != null && @params.TrangThaiHoaDon.Any() && !@params.TrangThaiHoaDon.Contains(-1))
            {
                var trangThaiBasic = @params.TrangThaiHoaDon.Where(o => o <= 4).ToList();
                var trangThais = @params.TrangThaiHoaDon.ToList();
                query = query.Where(x => trangThaiBasic.Contains(x.TrangThai.Value)
                                    || (trangThais.Contains(5) && x.TrangThai.Value == (int)TrangThaiHoaDon.HoaDonDieuChinh && x.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhTang)
                                    || (trangThais.Contains(6) && x.TrangThai.Value == (int)TrangThaiHoaDon.HoaDonDieuChinh && x.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhGiam)
                                    || (trangThais.Contains(7) && x.TrangThai.Value == (int)TrangThaiHoaDon.HoaDonDieuChinh && x.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin)
                                    || (trangThais.Contains(8) && x.DaBiDieuChinh == true)
                                    || (trangThais.Contains(9) && x.IsLapHoaDonThayThe == true)
                                    || (trangThais.Contains(10) && x.IsLapHoaDonThayThe == true && string.IsNullOrEmpty(x.ThayTheChoHoaDonId))
                                    || (trangThais.Contains(11) && x.IsLapHoaDonThayThe == true && !string.IsNullOrEmpty(x.ThayTheChoHoaDonId))
                                    || (trangThais.Contains(12) && x.TrangThai.Value == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4))
                                    || (trangThais.Contains(13) && x.TrangThai.Value == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4) && string.IsNullOrEmpty(x.ThayTheChoHoaDonId))
                                    || (trangThais.Contains(14) && x.TrangThai.Value == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3 || x.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4) && !string.IsNullOrEmpty(x.ThayTheChoHoaDonId))
                                    );
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

            string excelFileName = $"BANG_KE_CHI_TIET_HOA_DON_DIEN_TU-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            string excelFolder = $"FilesUpload/excels/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            // Excel
            string _sample = $"docs/HoaDonDienTu/BANG_KE_CHI_TIET_HOA_DON_DIEN_TU.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
            FileInfo file = new FileInfo(_path_sample);
            var dateReport = string.Empty;
            if ((@params.HoaDonDienTuIds == null || (@params.HoaDonDienTuIds != null && !@params.HoaDonDienTuIds.Any())) && string.IsNullOrEmpty(@params.HoaDonDienTuId))
            {
                dateReport = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(@params.TuNgay).ToString("dd/MM/yyyy"), DateTime.Parse(@params.DenNgay).ToString("dd/MM/yyyy"));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                List<HoaDonDienTuViewModel> list = query.OrderBy(x => x.NgayHoaDon).ToList();
                // Open sheet1
                int totalRows = list.Sum(x => x.HoaDonChiTiets.Count);

                // Begin row
                int begin_row = 5;

                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Fill data
                int idx = begin_row;
                if (@params.Mode == 1)
                {
                    // Add Row
                    worksheet.InsertRow(begin_row + 1, totalRows, begin_row);
                    int count = 1;
                    foreach (var it in list)
                    {
                        foreach (var ct in it.HoaDonChiTiets)
                        {
                            worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[idx, 1].Value = count.ToString();
                            worksheet.Cells[idx, 2].Value = it.NgayHoaDon.Value.ToString("dd/MM/yyyy");
                            worksheet.Cells[idx, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[idx, 3].Value = it.SoHoaDon;
                            worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.BoKyHieuHoaDon != null ? it.BoKyHieuHoaDon.KyHieuMauSoHoaDon.ToString() : string.Empty);
                            worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.BoKyHieuHoaDon != null ? it.BoKyHieuHoaDon.KyHieuHoaDon : string.Empty);
                            worksheet.Cells[idx, 6].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.KhachHang != null ? it.KhachHang.Ma : string.Empty);
                            worksheet.Cells[idx, 7].Value = !string.IsNullOrEmpty(it.TenKhachHang) ? it.TenKhachHang : (it.KhachHang != null ? it.KhachHang.Ten : string.Empty);
                            worksheet.Cells[idx, 8].Value = !string.IsNullOrEmpty(it.DiaChi) ? it.DiaChi : (it.KhachHang != null ? it.KhachHang.DiaChi : string.Empty);
                            worksheet.Cells[idx, 9].Value = !string.IsNullOrEmpty(it.MaSoThue) ? it.MaSoThue : (it.KhachHang != null ? it.KhachHang.MaSoThue : string.Empty);
                            worksheet.Cells[idx, 10].Value = !string.IsNullOrEmpty(it.HoTenNguoiMuaHang) ? it.HoTenNguoiMuaHang : (it.KhachHang != null ? it.KhachHang.HoTenNguoiMuaHang : string.Empty);
                            worksheet.Cells[idx, 11].Value = it.TenHinhThucThanhToan;
                            worksheet.Cells[idx, 12].Value = it.LoaiTien != null ? it.LoaiTien.Ten : string.Empty;
                            worksheet.Cells[idx, 13].Value = it.TyGia ?? 1;
                            worksheet.Cells[idx, 14].Value = !string.IsNullOrEmpty(ct.MaHang) ? ct.MaHang : ((ct.HangHoaDichVu != null) ? ct.HangHoaDichVu.Ma : string.Empty);
                            worksheet.Cells[idx, 15].Value = !string.IsNullOrEmpty(ct.TenHang) ? ct.TenHang : ((ct.HangHoaDichVu != null) ? ct.HangHoaDichVu.Ten : string.Empty);
                            worksheet.Cells[idx, 16].Value = (ct.DonViTinh != null) ? ct.DonViTinh.Ten : string.Empty;
                            worksheet.Cells[idx, 17].Value = ct.SoLuong ?? 0;
                            worksheet.Cells[idx, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            worksheet.Cells[idx, 18].Value = ct.DonGia ?? 0;
                            worksheet.Cells[idx, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 19].Value = ct.ThanhTien ?? 0;
                            worksheet.Cells[idx, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 20].Value = ct.ThanhTienQuyDoi ?? 0;
                            worksheet.Cells[idx, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 21].Value = ct.TyLeChietKhau ?? 0;
                            worksheet.Cells[idx, 21].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 22].Value = ct.TienChietKhau ?? 0;
                            worksheet.Cells[idx, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 23].Value = ct.TienChietKhauQuyDoi ?? 0;
                            worksheet.Cells[idx, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 24].Value = ct.ThueGTGT != "KCT" ? ct.ThueGTGT.ToString() + "%" : "\\";
                            worksheet.Cells[idx, 24].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 25].Value = ct.TienThueGTGT ?? 0;
                            worksheet.Cells[idx, 25].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 26].Value = ct.TienThueGTGTQuyDoi ?? 0;
                            worksheet.Cells[idx, 26].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 27].Value = it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang ? (it.TyLePhanTramDoanhThu ?? 0) + "%" : "";
                            worksheet.Cells[idx, 27].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            if (it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang)
                                worksheet.Cells[idx, 28].Value = ct.TienGiam ?? 0;
                            else worksheet.Cells[idx, 28].Value = string.Empty;
                            worksheet.Cells[idx, 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 29].Value = ((ct.ThanhTien ?? 0) - (ct.TienChietKhau ?? 0) + (ct.TienThueGTGT ?? 0) - (ct.TienGiam ?? 0));
                            worksheet.Cells[idx, 29].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 30].Value = ((ct.ThanhTienQuyDoi ?? 0) - (ct.TienChietKhauQuyDoi ?? 0) + (ct.TienThueGTGTQuyDoi ?? 0) - (ct.TienGiamQuyDoi ?? 0));
                            worksheet.Cells[idx, 30].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            worksheet.Cells[idx, 31].Value = ct.TinhChat == (int)TChat.KhuyenMai ? "x" : string.Empty;
                            worksheet.Cells[idx, 31].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            worksheet.Cells[idx, 32].Value = string.Empty;
                            worksheet.Cells[idx, 33].Value = ct.SoLo;
                            worksheet.Cells[idx, 34].Value = ct.HanSuDung.HasValue ? ct.HanSuDung.Value.ToString("dd/MM/yyyy") : string.Empty;
                            worksheet.Cells[idx, 35].Value = ct.SoKhung;
                            worksheet.Cells[idx, 36].Value = ct.SoMay;
                            worksheet.Cells[idx, 37].Value = string.Empty;
                            worksheet.Cells[idx, 38].Value = string.Empty;
                            worksheet.Cells[idx, 39].Value = !string.IsNullOrEmpty(it.MaNhanVienBanHang) ? it.MaNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ma : string.Empty);
                            worksheet.Cells[idx, 40].Value = !string.IsNullOrEmpty(it.TenNhanVienBanHang) ? it.TenNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ten : string.Empty);
                            worksheet.Cells[idx, 41].Value = ((LoaiHoaDon)it.LoaiHoaDon).GetDescription();
                            worksheet.Cells[idx, 42].Value = TrangThaiHoaDons.Where(x => x.TrangThaiId == it.TrangThai).Select(x => x.Ten).FirstOrDefault();
                            worksheet.Cells[idx, 43].Value = ((TrangThaiQuyTrinh)it.TrangThaiQuyTrinh).GetDescription();
                            worksheet.Cells[idx, 44].Value = it.MaTraCuu;
                            worksheet.Cells[idx, 45].Value = it.LyDoXoaBo;
                            worksheet.Cells[idx, 46].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                            worksheet.Cells[idx, 46].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                            worksheet.Cells[idx, 47].Value = it.ActionUser != null ? it.ActionUser.FullName : string.Empty;

                            idx += 1;
                            count += 1;
                        }
                    }
                    worksheet.Cells[2, 1].Value = dateReport;
                    //worksheet.Row(5).Style.Font.Color.SetColor(Color.Red);
                    // Total
                    worksheet.Row(idx).Style.Font.Bold = true;
                    worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[idx, 2].Value = string.Format("Số dòng = {0}", totalRows);
                    worksheet.Cells[idx, 19].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien));
                    worksheet.Cells[idx, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 20].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi));
                    worksheet.Cells[idx, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 22].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhau));
                    worksheet.Cells[idx, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 23].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhauQuyDoi));
                    worksheet.Cells[idx, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 25].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGT));
                    worksheet.Cells[idx, 25].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 26].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGTQuyDoi));
                    worksheet.Cells[idx, 26].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 28].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienGiam));
                    worksheet.Cells[idx, 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 29].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien - y.TienChietKhau + y.TienThueGTGT - (y.TienGiam ?? 0)));
                    worksheet.Cells[idx, 29].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 30].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi - y.TienChietKhauQuyDoi + y.TienThueGTGTQuyDoi - (y.TienGiamQuyDoi ?? 0)));
                    worksheet.Cells[idx, 30].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    //replace Text

                }
                else
                {
                    var thueSuats = list.Select(x => x.HoaDonChiTiets.Select(o => o.ThueGTGT).Distinct().ToList()).Distinct().ToList();
                    var lstThueSuats = new List<string>();
                    foreach (var item in thueSuats)
                    {
                        foreach (var it in item)
                        {
                            lstThueSuats.Add(it);
                        }
                    }

                    lstThueSuats = lstThueSuats.Distinct().ToList();
                    foreach (var item in lstThueSuats)
                    {
                        var lstThue = list.Where(x => x.HoaDonChiTiets.Any(o => o.ThueGTGT == item)).ToList();
                        foreach (var itemThue in lstThue)
                        {
                            itemThue.HoaDonChiTiets = itemThue.HoaDonChiTiets.Where(x => x.ThueGTGT == item).ToList();
                        }
                        var total = lstThue.Sum(x => x.HoaDonChiTiets.Count);
                        worksheet.InsertRow(idx + 1, total + 2, idx);
                        worksheet.Cells[idx, 1, idx, 45].Merge = true;
                        var strThue = item == "KCT" ? "Hàng hóa dịch vụ không chịu thuế giá trị gia tăng (GTGT)" :
                                      item == "KHAC" ? "Hàng hóa dịch vụ chịu mức thuế suất trống và tiền thuế lớn hơn 0" :
                                      item == "KKKNT" ? "Hàng hóa dịch vụ chịu mức thuế suất trống và tiền thuế bằng 0" :
                                      $"Hàng hóa dịch vụ chịu mức thuế {item}%";
                        worksheet.Cells[idx, 1].Value = strThue;
                        idx += 1;
                        int count = 1;
                        foreach (var it in lstThue)
                        {
                            foreach (var ct in it.HoaDonChiTiets)
                            {
                                worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[idx, 1].Value = count.ToString();
                                worksheet.Cells[idx, 2].Value = it.NgayHoaDon.Value.ToString("dd/MM/yyyy");
                                worksheet.Cells[idx, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[idx, 3].Value = it.SoHoaDon;
                                worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.BoKyHieuHoaDon != null ? it.BoKyHieuHoaDon.KyHieuMauSoHoaDon.ToString() : string.Empty);
                                worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.BoKyHieuHoaDon != null ? it.BoKyHieuHoaDon.KyHieuHoaDon : string.Empty);
                                worksheet.Cells[idx, 6].Value = !string.IsNullOrEmpty(it.MaKhachHang) ? it.MaKhachHang : (it.KhachHang != null ? it.KhachHang.Ma : string.Empty);
                                worksheet.Cells[idx, 7].Value = !string.IsNullOrEmpty(it.TenKhachHang) ? it.TenKhachHang : (it.KhachHang != null ? it.KhachHang.Ten : string.Empty);
                                worksheet.Cells[idx, 8].Value = !string.IsNullOrEmpty(it.DiaChi) ? it.DiaChi : (it.KhachHang != null ? it.KhachHang.DiaChi : string.Empty);
                                worksheet.Cells[idx, 9].Value = !string.IsNullOrEmpty(it.MaSoThue) ? it.MaSoThue : (it.KhachHang != null ? it.KhachHang.MaSoThue : string.Empty);
                                worksheet.Cells[idx, 10].Value = !string.IsNullOrEmpty(it.HoTenNguoiMuaHang) ? it.HoTenNguoiMuaHang : (it.KhachHang != null ? it.KhachHang.HoTenNguoiMuaHang : string.Empty);
                                worksheet.Cells[idx, 11].Value = it.TenHinhThucThanhToan;
                                worksheet.Cells[idx, 12].Value = it.LoaiTien != null ? it.LoaiTien.Ten : string.Empty;
                                worksheet.Cells[idx, 13].Value = it.TyGia ?? 1;
                                worksheet.Cells[idx, 14].Value = !string.IsNullOrEmpty(ct.MaHang) ? ct.MaHang : ((ct.HangHoaDichVu != null) ? ct.HangHoaDichVu.Ma : string.Empty);
                                worksheet.Cells[idx, 15].Value = !string.IsNullOrEmpty(ct.TenHang) ? ct.TenHang : ((ct.HangHoaDichVu != null) ? ct.HangHoaDichVu.Ten : string.Empty);
                                worksheet.Cells[idx, 16].Value = (ct.DonViTinh != null) ? ct.DonViTinh.Ten : string.Empty;
                                worksheet.Cells[idx, 17].Value = ct.SoLuong ?? 0;
                                worksheet.Cells[idx, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 18].Value = ct.DonGia ?? 0;
                                worksheet.Cells[idx, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 19].Value = ct.ThanhTien ?? 0;
                                worksheet.Cells[idx, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 20].Value = ct.ThanhTienQuyDoi ?? 0;
                                worksheet.Cells[idx, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 21].Value = ct.TyLeChietKhau ?? 0;
                                worksheet.Cells[idx, 21].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 22].Value = ct.TienChietKhau ?? 0;
                                worksheet.Cells[idx, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 23].Value = ct.TienChietKhauQuyDoi ?? 0;
                                worksheet.Cells[idx, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 24].Value = ct.ThueGTGT != "KCT" ? ct.ThueGTGT.ToString() + "%" : "\\";
                                worksheet.Cells[idx, 24].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 25].Value = ct.TienThueGTGT ?? 0;
                                worksheet.Cells[idx, 25].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 26].Value = ct.TienThueGTGTQuyDoi ?? 0;
                                worksheet.Cells[idx, 26].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 27].Value = ct.ThanhTien ?? 0 - ct.TienChietKhau ?? 0 + ct.TienThueGTGT ?? 0;
                                worksheet.Cells[idx, 27].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                if (it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang)
                                    worksheet.Cells[idx, 28].Value = ct.TienGiam ?? 0;
                                else worksheet.Cells[idx, 28].Value = string.Empty;
                                worksheet.Cells[idx, 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 29].Value = ((ct.ThanhTien ?? 0) - (ct.TienChietKhau ?? 0) + (ct.TienThueGTGT ?? 0) - (ct.TienGiam ?? 0));
                                worksheet.Cells[idx, 29].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 30].Value = ((ct.ThanhTienQuyDoi ?? 0) - (ct.TienChietKhauQuyDoi ?? 0) + (ct.TienThueGTGTQuyDoi ?? 0) - (ct.TienGiamQuyDoi ?? 0));
                                worksheet.Cells[idx, 30].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 31].Value = ct.TinhChat == (int)TChat.KhuyenMai ? "x" : string.Empty;
                                worksheet.Cells[idx, 31].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                worksheet.Cells[idx, 32].Value = string.Empty;
                                worksheet.Cells[idx, 33].Value = ct.SoLo;
                                worksheet.Cells[idx, 34].Value = ct.HanSuDung.HasValue ? ct.HanSuDung.Value.ToString("dd/MM/yyyy") : string.Empty;
                                worksheet.Cells[idx, 35].Value = ct.SoKhung;
                                worksheet.Cells[idx, 36].Value = ct.SoMay;
                                worksheet.Cells[idx, 37].Value = string.Empty;
                                worksheet.Cells[idx, 38].Value = string.Empty;
                                worksheet.Cells[idx, 39].Value = !string.IsNullOrEmpty(it.MaNhanVienBanHang) ? it.MaNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ma : string.Empty);
                                worksheet.Cells[idx, 40].Value = !string.IsNullOrEmpty(it.TenNhanVienBanHang) ? it.TenNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ten : string.Empty);
                                worksheet.Cells[idx, 41].Value = ((LoaiHoaDon)it.LoaiHoaDon).GetDescription();
                                worksheet.Cells[idx, 42].Value = TrangThaiHoaDons.Where(x => x.TrangThaiId == it.TrangThai).Select(x => x.Ten).FirstOrDefault();
                                worksheet.Cells[idx, 43].Value = ((TrangThaiQuyTrinh)it.TrangThaiQuyTrinh).GetDescription();
                                worksheet.Cells[idx, 44].Value = it.MaTraCuu;
                                worksheet.Cells[idx, 45].Value = it.LyDoXoaBo;
                                worksheet.Cells[idx, 46].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                                worksheet.Cells[idx, 46].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                worksheet.Cells[idx, 47].Value = it.ActionUser != null ? it.ActionUser.FullName : string.Empty;

                                if (it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang)
                                    worksheet.Cells[idx, 28].Value = ct.TienGiam ?? 0;
                                else worksheet.Cells[idx, 28].Value = string.Empty;
                                worksheet.Cells[idx, 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 29].Value = ((ct.ThanhTien ?? 0) - (ct.TienChietKhau ?? 0) + (ct.TienThueGTGT ?? 0) - (ct.TienGiam ?? 0));
                                worksheet.Cells[idx, 29].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 30].Value = ((ct.ThanhTienQuyDoi ?? 0) - (ct.TienChietKhauQuyDoi ?? 0) + (ct.TienThueGTGTQuyDoi ?? 0) - (ct.TienGiamQuyDoi ?? 0));
                                worksheet.Cells[idx, 30].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                                worksheet.Cells[idx, 31].Value = ct.TinhChat == (int)TChat.KhuyenMai ? "x" : string.Empty;
                                worksheet.Cells[idx, 31].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                worksheet.Cells[idx, 32].Value = string.Empty;
                                worksheet.Cells[idx, 33].Value = ct.SoLo;
                                worksheet.Cells[idx, 34].Value = ct.HanSuDung.HasValue ? ct.HanSuDung.Value.ToString("dd/MM/yyyy") : string.Empty;
                                worksheet.Cells[idx, 35].Value = ct.SoKhung;
                                worksheet.Cells[idx, 36].Value = ct.SoMay;
                                worksheet.Cells[idx, 37].Value = string.Empty;
                                worksheet.Cells[idx, 38].Value = string.Empty;
                                worksheet.Cells[idx, 39].Value = !string.IsNullOrEmpty(it.MaNhanVienBanHang) ? it.MaNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ma : string.Empty);
                                worksheet.Cells[idx, 40].Value = !string.IsNullOrEmpty(it.TenNhanVienBanHang) ? it.TenNhanVienBanHang : (it.NhanVienBanHang != null ? it.NhanVienBanHang.Ten : string.Empty);
                                worksheet.Cells[idx, 41].Value = ((LoaiHoaDon)it.LoaiHoaDon).GetDescription();
                                worksheet.Cells[idx, 42].Value = TrangThaiHoaDons.Where(x => x.TrangThaiId == it.TrangThai).Select(x => x.Ten).FirstOrDefault();
                                worksheet.Cells[idx, 43].Value = ((TrangThaiQuyTrinh)it.TrangThaiQuyTrinh).GetDescription();
                                worksheet.Cells[idx, 44].Value = it.MaTraCuu;
                                worksheet.Cells[idx, 45].Value = it.LyDoXoaBo;
                                worksheet.Cells[idx, 46].Value = it.NgayLap.Value.ToString("dd/MM/yyyy");
                                worksheet.Cells[idx, 46].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                                worksheet.Cells[idx, 47].Value = it.ActionUser != null ? it.ActionUser.FullName : string.Empty;

                                idx += 1;
                                count += 1;
                            }
                        }
                        worksheet.Cells[2, 1].Value = dateReport;
                        //worksheet.Row(5).Style.Font.Color.SetColor(Color.Red);
                        // Total
                        worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                        worksheet.Row(idx).Style.Font.Bold = true;
                        worksheet.Cells[idx, 19].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien));
                        worksheet.Cells[idx, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 20].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi));
                        worksheet.Cells[idx, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 22].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhau));
                        worksheet.Cells[idx, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 23].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhauQuyDoi));
                        worksheet.Cells[idx, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 25].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGT));
                        worksheet.Cells[idx, 25].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 26].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGTQuyDoi));
                        worksheet.Cells[idx, 26].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 28].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienGiam));
                        worksheet.Cells[idx, 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 29].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien - y.TienChietKhau + y.TienThueGTGT - (y.TienGiam ?? 0)));
                        worksheet.Cells[idx, 29].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        worksheet.Cells[idx, 30].Value = lstThue.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi - y.TienChietKhauQuyDoi + y.TienThueGTGTQuyDoi - (y.TienGiamQuyDoi ?? 0)));
                        worksheet.Cells[idx, 30].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        //replace Text
                        idx += 1;
                    }

                    worksheet.Row(idx).Style.Font.Bold = true;
                    worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[idx, 2].Value = string.Format("Số dòng = {0}", totalRows);
                    worksheet.Cells[idx, 19].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien));
                    worksheet.Cells[idx, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 20].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi));
                    worksheet.Cells[idx, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 22].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhau));
                    worksheet.Cells[idx, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 23].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienChietKhauQuyDoi));
                    worksheet.Cells[idx, 23].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 25].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGT));
                    worksheet.Cells[idx, 25].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 26].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienThueGTGTQuyDoi));
                    worksheet.Cells[idx, 26].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    if (list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienGiam)) > 0)
                        worksheet.Cells[idx, 28].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.TienGiam));
                    else worksheet.Cells[idx, 28].Value = 0;
                    worksheet.Cells[idx, 28].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 29].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTien - y.TienChietKhau + y.TienThueGTGT));
                    worksheet.Cells[idx, 29].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    worksheet.Cells[idx, 30].Value = list.Sum(x => x.HoaDonChiTiets.Sum(y => y.ThanhTienQuyDoi - y.TienChietKhauQuyDoi + y.TienThueGTGTQuyDoi));
                    worksheet.Cells[idx, 30].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                }
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
            KetQuaCapSoHoaDon result = new KetQuaCapSoHoaDon();

            if (hd.SoHoaDon.HasValue)
            {
                result.SoHoaDon = hd.SoHoaDon;
                return result;
            }

            var maxSoHoaDon = await _db.HoaDonDienTus
                        .Where(x => x.BoKyHieuHoaDonId == hd.BoKyHieuHoaDonId && x.SoHoaDon.HasValue)
                        .DefaultIfEmpty()
                        .MaxAsync(x => x.SoHoaDon);

            if (maxSoHoaDon.HasValue)
            {
                result.SoHoaDon = maxSoHoaDon + 1;
            }
            else
            {
                var boKyHieuHoaDon = await _db.BoKyHieuHoaDons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == hd.BoKyHieuHoaDonId);

                result.SoHoaDon = boKyHieuHoaDon.SoBatDau;
            }

            return result;
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
            try
            {
                var path = string.Empty;
                var pathXML = string.Empty;
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string pdfFileName = string.Empty;
                string xmlFileName = string.Empty;

                if (hd.IsCapMa != true && hd.IsReloadSignedPDF != true && hd.BuyerSigned != true && (hd.TrangThaiQuyTrinh >= (int)TrangThaiQuyTrinh.DaKyDienTu) && (hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.GuiTCTNLoi) && (!string.IsNullOrEmpty(hd.FileDaKy) || !string.IsNullOrEmpty(hd.XMLDaKy)))
                {
                    // Check file exist to re-save
                    await RestoreFilesInvoiceSigned(hd.HoaDonDienTuId);

                    return new KetQuaConvertPDF
                    {
                        FilePDF = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}/{hd.FileDaKy}",
                        FileXML = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{hd.XMLDaKy}",
                    };
                }

                var _tuyChons = await _TuyChonService.GetAllAsync();

                var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
                var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
                var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());

                var hoSoHDDT = await _HoSoHDDTService.GetDetailAsync();
                var mauHoaDon = await _MauHoaDonService.GetByIdAsync(hd.BoKyHieuHoaDon.MauHoaDonId);
                hd.MauHoaDon = mauHoaDon;
                if (mauHoaDon.MauHoaDonTuyChinhChiTiets.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.KhuVucPhiThueQuan))
                {
                    hd.IsHoaDonChoTCCNTKPTQ = mauHoaDon.MauHoaDonTuyChinhChiTiets.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.KhuVucPhiThueQuan).Checked;
                }

                var docTuple = await _MauHoaDonService.GetDocForInvoiceAsync(mauHoaDon, hd.GetBoMauHoaDonFromHoaDonDienTu(), !string.IsNullOrEmpty(hd.LyDoThayThe) || !string.IsNullOrEmpty(hd.LyDoDieuChinh));
                var doc = docTuple.Item1;
                var beginRow = docTuple.Item2;

                doc.Replace(LoaiChiTietTuyChonNoiDung.MaCuaCQT.GenerateKeyTag(), hd.MaCuaCQT ?? string.Empty, true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.KyHieu.GenerateKeyTag(), (hd.MauSo + hd.KyHieu) ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoHoaDon.GenerateKeyTag(), !hd.SoHoaDon.HasValue ? "<Chưa cấp số>" : hd.SoHoaDon.ToString(), true, true);

                doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
                doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
                doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.HoTenNguoiMua.GenerateKeyTag(), hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua.GenerateKeyTag(), hd.TenKhachHang ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua.GenerateKeyTag(), hd.MaSoThue ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua.GenerateKeyTag(), hd.DiaChi ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.HinhThucThanhToan.GenerateKeyTag(), hd.TenHinhThucThanhToan ?? string.Empty, true, true);
                doc.Replace(LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua.GenerateKeyTag(), hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

                doc.Replace(LoaiChiTietTuyChonNoiDung.MaTraCuu.GenerateKeyTag(), hd.MaTraCuu ?? string.Empty, true, true);

                if (hd.IsCapMa == true || hd.IsPhatHanh == true || hd.IsReloadSignedPDF == true)
                {
                    if (hd.IsCapMa == true || hd.IsPhatHanh == true)
                    {
                        hd.NgayKy = DateTime.Now;
                    }

                    //ImageHelper.AddSignatureImageToDoc(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, hd.NgayKy);

                    ImageHelper.CreateSignatureBox(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, hd.NgayKy);
                }
                else
                {
                    if (!hd.NgayKy.HasValue)
                    {
                        doc.Replace("<digitalSignature>", string.Empty, true, true);
                    }
                    else
                    {
                        //ImageHelper.AddSignatureImageToDoc(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, hd.NgayKy);

                        ImageHelper.CreateSignatureBox(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, hd.NgayKy);
                    }
                }

                if (hd.BuyerSigned == true)
                {
                    if (hd.BuyerSigned == true)
                    {
                        hd.NgayNguoiMuaKy = DateTime.Now;
                    }
                    //ImageHelper.AddSignatureImageToDoc_Buyer(doc, hd.TenKhachHang, mauHoaDon.LoaiNgonNgu, hd.NgayNguoiMuaKy);

                    ImageHelper.CreateSignatureBox(doc, hd.TenKhachHang, mauHoaDon.LoaiNgonNgu, hd.NgayNguoiMuaKy);
                }
                else
                {
                    doc.Replace("<digitalSignature_Buyer>", string.Empty, true, true);
                }

                List<Table> listTable = new List<Table>();
                string stt = string.Empty;
                foreach (Table tb in doc.Sections[0].Tables)
                {
                    if (tb.Title == "tbl_hhdv")
                    {
                        listTable.Add(tb);
                        break;
                    }
                }

                var maLoaiTien = hd.LoaiTien.Ma == "VND" ? string.Empty : hd.LoaiTien.Ma;
                string soTienBangChu = hd.TongTienThanhToan.Value
                    .MathRoundNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE)
                    .ConvertToInWord(_cachDocSo0HangChuc.ToLower(), _cachDocHangNghin.ToLower(), _hienThiSoChan, hd.LoaiTien.Ma);
                List<HoaDonDienTuChiTietViewModel> models = await _HoaDonDienTuChiTietService.GetChiTietHoaDonAsync(hd.HoaDonDienTuId, true);

                int line = models.Count();
                if (line > 0)
                {
                    Table table = null;
                    int soDongTrang = 0;
                    if (listTable.Count > 0)
                    {
                        table = listTable[0];

                        // Check to insert to row detail order
                        soDongTrang = int.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.SoDongTrang).GiaTri);
                        if (line > soDongTrang)
                        {
                            int _cnt_rows = line - soDongTrang;
                            for (int i = 0; i < _cnt_rows; i++)
                            {
                                TableRow cl_row = table.Rows[beginRow + 1].Clone();
                                table.Rows.Insert(beginRow, cl_row);
                            }
                        }

                        if (hd.IsGiamTheoNghiQuyet == true && (hd.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang) && hd.TongTienGiam != 0)
                        {
                            var tblTongTien = table.Rows[table.Rows.Count - 1].Cells[0].Tables[0] as Table;

                            for (int i = 0; i < tblTongTien.Rows.Count; i++)
                            {
                                var find = tblTongTien.Rows[i].Cells[0].Paragraphs[0].Text;
                                if (find.Contains("<SoTienBangChu>"))
                                {
                                    TableRow cl_row = tblTongTien.Rows[i].Clone();
                                    var par = cl_row.Cells[0].Paragraphs[0];

                                    TextRange textRangeClone = null;
                                    foreach (DocumentObject obj in par.ChildObjects)
                                    {
                                        if (obj.DocumentObjectType == DocumentObjectType.TextRange)
                                        {
                                            textRangeClone = obj as TextRange;
                                            break;
                                        }
                                    }

                                    cl_row.Cells[0].Paragraphs.Clear();
                                    par = cl_row.Cells[0].AddParagraph();
                                    par.Format.AfterSpacing = 0;
                                    par.Format.LeftIndent = 1;
                                    par.Format.RightIndent = 1;

                                    string strTongTienGiam = hd.TongTienGiam.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                                    string tenLoaiTien = hd.LoaiTien.Ma.DocTenLoaiTien();

                                    TextRange textRange = par.AppendText($"Giảm {strTongTienGiam} {tenLoaiTien}, tương ứng 20% mức tỷ lệ % để tính thuế giá trị gia tăng theo Nghị quyết số 43/2022/QH15");
                                    textRange.CharacterFormat.FontSize = textRangeClone.CharacterFormat.FontSize;

                                    tblTongTien.Rows.Insert(i + 1, cl_row);
                                    break;
                                }
                            }

                        }
                    }

                    var thueGTGT = TextHelper.GetThueGTGTByNgayHoaDon(hd.NgayHoaDon.Value, models.Select(x => x.ThueGTGT ?? "0").FirstOrDefault());

                    var isAllKhuyenMai = models.Any(x => x.IsAllKhuyenMai == true);
                    var tienThueGTGT = string.Empty;
                    if (isAllKhuyenMai)
                    {
                        thueGTGT = "X";
                    }

                    if (thueGTGT == "X")
                    {
                        tienThueGTGT = thueGTGT;
                    }
                    else
                    {
                        tienThueGTGT = hd.TongTienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    }

                    var isDieuChinhThongTin = (hd.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (hd.LoaiDieuChinh == 3);

                    if (!isDieuChinhThongTin)
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TyLeChietKhau.GenerateKeyTag(), hd.TyLeChietKhau.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE) + "%", true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienChietKhau.GenerateKeyTag(), hd.TongTienChietKhau.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TienThueGTGT.GenerateKeyTag(), tienThueGTGT ?? string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHang.GenerateKeyTag(), hd.TongTienHang.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien), true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHangDaTruCK.GenerateKeyTag(), (hd.TongTienHang - hd.TongTienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.ThueSuatGTGT.GenerateKeyTag(), thueGTGT ?? string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienThanhToan.GenerateKeyTag(), hd.TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienBangChu.GenerateKeyTag(), soTienBangChu ?? string.Empty, true, true);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TyGia.GenerateKeyTag(), (hd.TyGia.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TY_GIA) + $" VND/{hd.MaLoaiTien}") ?? string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.QuyDoi.GenerateKeyTag(), (hd.TongTienThanhToanQuyDoi.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI) + " VND") ?? string.Empty, true, true);
                    }

                    #region Thuế chi tiết
                    if (models.Where(x => x.ThueGTGT == "KKKNT" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                    {
                        string thanhTienTruocThueKKKNT = models.Where(x => x.ThueGTGT == "KKKNT" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThueKKKNT = string.IsNullOrEmpty(thanhTienTruocThueKKKNT) ? string.Empty : "0";
                        string congTienThanhToanKKKNT = models.Where(x => x.ThueGTGT == "KKKNT" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueKKKNT, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueKKKNT, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanKKKNT, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }

                    if (models.Where(x => x.ThueGTGT == "KCT" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                    {
                        string thanhTienTruocThueKCT = models.Where(x => x.ThueGTGT == "KCT" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThueKCT = string.IsNullOrEmpty(thanhTienTruocThueKCT) ? string.Empty : "0";
                        string congTienThanhToanKCT = models.Where(x => x.ThueGTGT == "KCT" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueKCT, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueKCT, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanKCT, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }

                    if (models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                    {
                        string thanhTienTruocThue0 = models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThue0 = models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string congTienThanhToan0 = models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThue0, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThue0, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToan0, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }

                    if (models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                    {
                        string thanhTienTruocThue5 = models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThue5 = models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string congTienThanhToan5 = models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThue5, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThue5, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToan5, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }

                    if (models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                    {
                        if (hd.IsGiamTheoNghiQuyet == true && hd.MauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauNhieuThueSuat)
                        {
                            var tblTongTien = table.Rows[table.Rows.Count - 1].Cells[0].Tables[0] as Table;

                            for (int i = 0; i < tblTongTien.Rows.Count; i++)
                            {
                                if (tblTongTien.Rows[i].Cells[1].Paragraphs.Count > 0)
                                {
                                    var find = tblTongTien.Rows[i].Cells[1].Paragraphs[0].Text;
                                    bool flag = false;
                                    if (find.Contains(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue)))
                                    {
                                        var par = tblTongTien.Rows[i].Cells[0].Paragraphs[0];
                                        foreach (DocumentObject obj in par.ChildObjects)
                                        {
                                            if (obj.DocumentObjectType == DocumentObjectType.TextRange)
                                            {
                                                var textRange = obj as TextRange;
                                                textRange.Text = textRange.Text.Replace("10%", "8%");
                                                flag = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag)
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        string thanhTienTruocThue10 = models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThue10 = models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string congTienThanhToan10 = models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThue10, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThue10, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToan10, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }

                    if (models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                    {
                        string thanhTienTruocThueKHAC = models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThueKHAC = models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string congTienThanhToanKHAC = models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueKHAC, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueKHAC, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanKHAC, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }

                    if (!isDieuChinhThongTin)
                    {
                        string thanhTienTruocThueTong = (hd.TongTienHang - hd.TongTienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string tienThueTong = hd.TongTienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                        string congTienThanhToanTong = hd.TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueTong, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueTong, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanTong, true, true);
                    }
                    else
                    {
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                        doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(hd.LyDoThayThe))
                    {
                        string lyDoThayThe = JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe).ToString();
                        if (!string.IsNullOrEmpty(hd.LyDoThayTheModel.LyDo))
                        {
                            lyDoThayThe += "\n" + hd.LyDoThayTheModel.LyDo;
                        }
                        doc.Replace("<reason>", lyDoThayThe ?? string.Empty, true, true);
                    }

                    if (!string.IsNullOrEmpty(hd.LyDoDieuChinh))
                    {
                        string lyDoDieuChinh = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh).ToString();
                        if (hd.IsThongTinNguoiBanHoacNguoiMua == true && hd.IsTheHienLyDoTrenHoaDon == true && !string.IsNullOrEmpty(hd.LyDoDieuChinhModel.LyDo))
                        {
                            lyDoDieuChinh += "\n" + hd.LyDoDieuChinhModel.LyDo;
                        }
                        doc.Replace("<reason>", lyDoDieuChinh ?? string.Empty, true, true);
                    }

                    if (table != null)
                    {
                        TableRow row = null;

                        for (int i = 0; i < line; i++)
                        {
                            row = table.Rows[i + beginRow];
                            int col = row.Cells.Count;

                            for (int j = 0; j < col; j++)
                            {
                                var par = row.Cells[j].Paragraphs[0];

                                par.SetValuePar2(models[i].STT + "", LoaiChiTietTuyChonNoiDung.STT);
                                par.SetValuePar2(models[i].TenHang, LoaiChiTietTuyChonNoiDung.TenHangHoaDichVu);

                                // Chiết khấu thương mại
                                // Ghi chú/diễn giải
                                if (models[i].TinhChat == 4 || isDieuChinhThongTin)
                                {
                                    continue;
                                }
                                else
                                {
                                    par.SetValuePar2(models[i].DonViTinh?.Ten, LoaiChiTietTuyChonNoiDung.DonViTinh);
                                    par.SetValuePar2(models[i].SoLuong.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.SO_LUONG), LoaiChiTietTuyChonNoiDung.SoLuong);
                                    par.SetValuePar2(models[i].DonGia.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.DON_GIA_QUY_DOI : LoaiDinhDangSo.DON_GIA_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.DonGia);
                                    par.SetValuePar2(models[i].ThanhTien.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.ThanhTien);
                                    par.SetValuePar2(models[i].TyLeChietKhau.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE), LoaiChiTietTuyChonNoiDung.TyLeChietKhauHHDV);
                                    par.SetValuePar2(models[i].TienChietKhau.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.TienChietKhauHHDV);
                                    par.SetValuePar2((models[i].ThanhTien - models[i].TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.ThanhTienDaTruCKHHDV);

                                    string thueGTGTHHDV = string.Empty;
                                    string tienThueHHDV = string.Empty;
                                    //if (models[i].ThueGTGT == "KCT" || models[i].ThueGTGT == "KKKNT")
                                    //{
                                    //    thueGTGTHHDV = "\\";
                                    //    tienThueHHDV = "\\";
                                    //}
                                    //else
                                    //{
                                    //    thueGTGTHHDV = models[i].ThueGTGT;
                                    //    tienThueHHDV = models[i].TienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                                    //}

                                    thueGTGTHHDV = models[i].ThueGTGT;
                                    tienThueHHDV = models[i].TienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                                    if (models[i].TinhChat == 2) // km
                                    {
                                        thueGTGTHHDV = "X";
                                        tienThueHHDV = "X";
                                    }
                                    else
                                    {
                                        if ((models[i].ThanhTien - models[i].TienChietKhau) == 0)
                                        {
                                            tienThueHHDV = string.Empty;
                                        }
                                        par.SetValuePar2(models[i].TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien), LoaiChiTietTuyChonNoiDung.ThanhTienSauThueHHDV);
                                    }

                                    par.SetValuePar2(thueGTGTHHDV, LoaiChiTietTuyChonNoiDung.ThueSuatHHDV);
                                    par.SetValuePar2(tienThueHHDV, LoaiChiTietTuyChonNoiDung.TienThueHHDV);
                                }
                            }
                        }
                    }

                    doc.ClearKeyTag();
                }
                else
                {
                    MauHoaDonHelper.CreatePreviewFileDoc(doc, mauHoaDon, _IHttpContextAccessor);
                }

                string fullPdfFolder;
                string fullXmlFolder;
                if (hd.IsCapMa == true || hd.IsPhatHanh == true || hd.IsReloadSignedPDF == true || hd.BuyerSigned == true)
                {
                    fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}");
                    fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}");
                    #region create folder
                    if (!Directory.Exists(fullPdfFolder))
                    {
                        Directory.CreateDirectory(fullPdfFolder);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hd.FileDaKy))
                        {
                            string oldFilePath = Path.Combine(fullPdfFolder, hd.FileDaKy);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }
                    }

                    if (hd.IsCapMa == true || hd.IsPhatHanh == true)
                    {
                        if (!Directory.Exists(fullXmlFolder))
                        {
                            Directory.CreateDirectory(fullXmlFolder);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(hd.XMLDaKy))
                            {
                                string oldFilePath = Path.Combine(fullXmlFolder, hd.XMLDaKy);
                                if (File.Exists(oldFilePath))
                                {
                                    File.Delete(oldFilePath);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_UNSIGN}");
                    fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}");
                    #region create folder
                    if (!Directory.Exists(fullPdfFolder))
                    {
                        Directory.CreateDirectory(fullPdfFolder);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hd.FileChuaKy))
                        {
                            string oldFilePath = Path.Combine(fullPdfFolder, hd.FileChuaKy);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }
                    }

                    if (!Directory.Exists(fullXmlFolder))
                    {
                        Directory.CreateDirectory(fullXmlFolder);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hd.XMLChuaKy))
                        {
                            string oldFilePath = Path.Combine(fullXmlFolder, hd.XMLChuaKy);
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }
                    }
                    #endregion
                }

                var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == hd.HoaDonDienTuId);

                if (hd.IsCapMa == true || hd.IsPhatHanh == true || hd.IsReloadSignedPDF == true || hd.BuyerSigned == true)
                {
                    pdfFileName = $"{hd.BoKyHieuHoaDon.KyHieu}-{hd.SoHoaDon}-{Guid.NewGuid()}.pdf";
                    entity.FileDaKy = pdfFileName;

                    if (hd.IsCapMa == true)
                    {
                        xmlFileName = $"{hd.BoKyHieuHoaDon.KyHieu}-{hd.SoHoaDon}-{Guid.NewGuid()}.xml";
                        entity.XMLDaKy = xmlFileName;
                        entity.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.CQTDaCapMa;
                        entity.MaCuaCQT = hd.MaCuaCQT;
                    }
                    else
                    {
                        if (hd.IsPhatHanh == true)
                        {
                            xmlFileName = Path.GetFileNameWithoutExtension(pdfFileName) + ".xml";
                        }
                        else
                        {
                            xmlFileName = entity.XMLDaKy;
                        }
                    }
                }
                else
                {
                    pdfFileName = $"{hd.BoKyHieuHoaDon.KyHieu}-{Guid.NewGuid()}.pdf";
                    xmlFileName = $"{hd.BoKyHieuHoaDon.KyHieu}-{Guid.NewGuid()}.xml";

                    entity.FileChuaKy = pdfFileName;
                    entity.XMLChuaKy = xmlFileName;
                }

                string fullPdfFilePath = Path.Combine(fullPdfFolder, pdfFileName);
                string fullXmlFilePath = Path.Combine(fullXmlFolder, xmlFileName);

                hd.HoaDonChiTiets = models;
                hd.SoTienBangChu = soTienBangChu;
                //doc.SaveToFile(fullPdfFilePath, Spire.Doc.FileFormat.PDF);
                doc.SaveToPDF(fullPdfFilePath, _hostingEnvironment, mauHoaDon.LoaiNgonNgu);

                if (hd.IsCapMa == true || hd.IsReloadSignedPDF == true || hd.BuyerSigned == true)
                {
                    if (hd.IsCapMa == true)
                    {
                        File.WriteAllText(fullXmlFilePath, hd.DataXML);
                    }
                }
                else
                {
                    await _xMLInvoiceService.CreateXMLInvoice(fullXmlFilePath, hd);
                }

                if (hd.IsCapMa == true || hd.IsPhatHanh == true || hd.IsReloadSignedPDF == true)
                {
                    if (hd.IsReloadSignedPDF == true || hd.IsPhatHanh == true)
                    {
                        await UpdateFileDataPdfForHDDT(hd.HoaDonDienTuId, fullPdfFilePath);
                    }
                    else
                    {
                        await UpdateFileDataForHDDT(hd.HoaDonDienTuId, fullPdfFilePath, fullXmlFilePath);
                    }
                }

                await _db.SaveChangesAsync();

                if (hd.IsBuyerSigned == true || hd.IsPhatHanh == true)
                {
                    path = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}/{pdfFileName}";
                    pathXML = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{xmlFileName}";
                }
                else
                {
                    path = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_UNSIGN}/{pdfFileName}";
                    pathXML = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}/{xmlFileName}";
                }

                return new KetQuaConvertPDF()
                {
                    FilePDF = path,
                    FileXML = pathXML,
                    PdfName = pdfFileName,
                    XMLName = xmlFileName,
                    XMLBase64 = File.Exists(fullXmlFilePath) ? TextHelper.Compress(File.ReadAllText(fullXmlFilePath)) : null
                    //PDFBase64 = fullPdfFilePath.EncodeFile()
                };
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return null;
        }

        public KetQuaConvertPDF ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd, string dataBaseName)
        {
            try
            {
                return new KetQuaConvertPDF
                {
                    FilePDF = $"FilesUpload/{dataBaseName}/{ManageFolderPath.PDF_SIGNED}/{hd.FileDaKy}",
                    FileXML = $"FilesUpload/{dataBaseName}/{ManageFolderPath.XML_SIGNED}/{hd.XMLDaKy}",
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FileReturn> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay @params)
        {
            try
            {
                var _objHDDT = await GetByIdAsync(@params.HoaDonDienTuId);
                var fileReturn = await ConvertHoaDonToHoaDonGiay(_objHDDT, @params);

                _objHDDT.SoLanChuyenDoi += 1;
                await UpdateAsync(_objHDDT);

                return fileReturn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CheckMaTraCuuAsync(string maTraCuu)
        {
            var result = await _db.HoaDonDienTus
                                    .Where(x => x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.MaTraCuu == maTraCuu);

            return result != null;
        }

        private async Task<FileReturn> ConvertHoaDonToHoaDonGiay(HoaDonDienTuViewModel hd, ParamsChuyenDoiThanhHDGiay @params)
        {
            var path = string.Empty;

            var _tuyChons = await _TuyChonService.GetAllAsync();

            var _cachDocSo0HangChuc = _tuyChons.Where(x => x.Ma == "CachDocSo0OHangChuc").Select(x => x.GiaTri).FirstOrDefault();
            var _cachDocHangNghin = _tuyChons.Where(x => x.Ma == "CachDocSoTienOHangNghin").Select(x => x.GiaTri).FirstOrDefault();
            var _hienThiSoChan = bool.Parse(_tuyChons.Where(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").Select(x => x.GiaTri).FirstOrDefault());
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            var hoSoHDDT = await _HoSoHDDTService.GetDetailAsync();
            var mauHoaDon = await _MauHoaDonService.GetByIdAsync(hd.BoKyHieuHoaDon.MauHoaDonId);

            var docTuple = await _MauHoaDonService.GetDocForInvoiceAsync(mauHoaDon, hd.GetBoMauHoaDonFromHoaDonDienTu(false), !string.IsNullOrEmpty(hd.LyDoThayThe) || !string.IsNullOrEmpty(hd.LyDoDieuChinh));
            var doc = docTuple.Item1;
            var beginRow = docTuple.Item2;

            doc.Replace(LoaiChiTietTuyChonNoiDung.MaCuaCQT.GenerateKeyTag(), hd.MaCuaCQT ?? string.Empty, true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.KyHieu.GenerateKeyTag(), (hd.MauSo + hd.KyHieu) ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.SoHoaDon.GenerateKeyTag(), !hd.SoHoaDon.HasValue ? "<Chưa cấp số>" : hd.SoHoaDon.ToString(), true, true);

            doc.Replace("<dd>", hd.NgayHoaDon.Value.Day.ToString() ?? DateTime.Now.Day.ToString(), true, true);
            doc.Replace("<mm>", hd.NgayHoaDon.Value.Month.ToString() ?? DateTime.Now.Month.ToString(), true, true);
            doc.Replace("<yyyy>", hd.NgayHoaDon.Value.Year.ToString() ?? DateTime.Now.Year.ToString(), true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.HoTenNguoiMua.GenerateKeyTag(), hd.HoTenNguoiMuaHang ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua.GenerateKeyTag(), hd.TenKhachHang ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua.GenerateKeyTag(), hd.MaSoThue ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua.GenerateKeyTag(), hd.DiaChi ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.HinhThucThanhToan.GenerateKeyTag(), hd.TenHinhThucThanhToan ?? string.Empty, true, true);
            doc.Replace(LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua.GenerateKeyTag(), hd.SoTaiKhoanNganHang ?? string.Empty, true, true);

            doc.Replace(LoaiChiTietTuyChonNoiDung.MaTraCuu.GenerateKeyTag(), hd.MaTraCuu ?? string.Empty, true, true);

            doc.Replace("<convertor>", @params.NguoiChuyenDoi ?? string.Empty, true, true);
            doc.Replace("<conversionDateValue>", @params.NgayChuyenDoi.Value.ToString("dd/MM/yyyy") ?? string.Empty, true, true);

            //ImageHelper.AddSignatureImageToDoc(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, hd.NgayKy.Value);

            ImageHelper.CreateSignatureBox(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, hd.NgayKy);

            if (hd.IsBuyerSigned == true)
            {
                //ImageHelper.AddSignatureImageToDoc_Buyer(doc, hd.TenKhachHang, mauHoaDon.LoaiNgonNgu, hd.NgayNguoiMuaKy.Value);

                ImageHelper.CreateSignatureBox(doc, hd.TenKhachHang, mauHoaDon.LoaiNgonNgu, hd.NgayNguoiMuaKy);
            }
            else
            {
                doc.Replace("<digitalSignature_Buyer>", string.Empty, true, true);
            }

            List<Table> listTable = new List<Table>();
            string stt = string.Empty;
            foreach (Table tb in doc.Sections[0].Tables)
            {
                if (tb.Title == "tbl_hhdv")
                {
                    listTable.Add(tb);
                    break;
                }
            }

            var maLoaiTien = hd.LoaiTien.Ma == "VND" ? string.Empty : hd.LoaiTien.Ma;
            List<HoaDonDienTuChiTietViewModel> models = await _HoaDonDienTuChiTietService.GetChiTietHoaDonAsync(hd.HoaDonDienTuId, true);
            string soTienBangChu = hd.TongTienThanhToan.Value
               .MathRoundNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE)
               .ConvertToInWord(_cachDocSo0HangChuc.ToLower(), _cachDocHangNghin.ToLower(), _hienThiSoChan, hd.LoaiTien.Ma);

            int line = models.Count();
            if (line > 0)
            {
                Table table = null;
                table = listTable[0];

                // Check to insert to row detail order
                var soDongTrang = int.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.SoDongTrang).GiaTri);
                if (line > soDongTrang)
                {
                    int _cnt_rows = line - soDongTrang;
                    for (int i = 0; i < _cnt_rows; i++)
                    {
                        TableRow cl_row = table.Rows[beginRow + 1].Clone();
                        table.Rows.Insert(beginRow, cl_row);
                    }
                }

                if (hd.IsGiamTheoNghiQuyet == true && (hd.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHang) && hd.TongTienGiam != 0)
                {
                    var tblTongTien = table.Rows[table.Rows.Count - 1].Cells[0].Tables[0] as Table;

                    for (int i = 0; i < tblTongTien.Rows.Count; i++)
                    {
                        var find = tblTongTien.Rows[i].Cells[0].Paragraphs[0].Text;
                        if (find.Contains("<SoTienBangChu>"))
                        {
                            TableRow cl_row = tblTongTien.Rows[i].Clone();
                            var par = cl_row.Cells[0].Paragraphs[0];

                            TextRange textRangeClone = null;
                            foreach (DocumentObject obj in par.ChildObjects)
                            {
                                if (obj.DocumentObjectType == DocumentObjectType.TextRange)
                                {
                                    textRangeClone = obj as TextRange;
                                    break;
                                }
                            }

                            cl_row.Cells[0].Paragraphs.Clear();
                            par = cl_row.Cells[0].AddParagraph();
                            par.Format.AfterSpacing = 0;
                            par.Format.LeftIndent = 1;
                            par.Format.RightIndent = 1;

                            string strTongTienGiam = hd.TongTienGiam.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                            string tenLoaiTien = hd.LoaiTien.Ma.DocTenLoaiTien();

                            TextRange textRange = par.AppendText($"Giảm {strTongTienGiam} {tenLoaiTien}, tương ứng 20% mức tỷ lệ % để tính thuế giá trị gia tăng theo Nghị quyết số 43/2022/QH15");
                            textRange.CharacterFormat.FontSize = textRangeClone.CharacterFormat.FontSize;

                            tblTongTien.Rows.Insert(i + 1, cl_row);
                            break;
                        }
                    }

                }

                var thueGTGT = TextHelper.GetThueGTGTByNgayHoaDon(hd.NgayHoaDon.Value, models.Select(x => x.ThueGTGT ?? "0").FirstOrDefault());

                var isAllKhuyenMai = models.Any(x => x.IsAllKhuyenMai == true);
                var tienThueGTGT = string.Empty;
                if (isAllKhuyenMai)
                {
                    thueGTGT = "X";
                }

                if (thueGTGT == "X")
                {
                    tienThueGTGT = thueGTGT;
                }
                else
                {
                    tienThueGTGT = hd.TongTienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                }

                var isDieuChinhThongTin = (hd.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (hd.LoaiDieuChinh == 3);

                if (!isDieuChinhThongTin)
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TyLeChietKhau.GenerateKeyTag(), hd.TyLeChietKhau.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE) + "%", true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienChietKhau.GenerateKeyTag(), hd.TongTienChietKhau.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TienThueGTGT.GenerateKeyTag(), tienThueGTGT ?? "0", true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHang.GenerateKeyTag(), hd.TongTienHang.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.CongTienHangDaTruCK.GenerateKeyTag(), (hd.TongTienHang - hd.TongTienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.ThueSuatGTGT.GenerateKeyTag(), thueGTGT ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienThanhToan.GenerateKeyTag(), hd.TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.SoTienBangChu.GenerateKeyTag(), soTienBangChu ?? string.Empty, true, true);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TyGia.GenerateKeyTag(), (hd.TyGia.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TY_GIA) + $" VND/{hd.MaLoaiTien}") ?? string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.QuyDoi.GenerateKeyTag(), (hd.TongTienThanhToanQuyDoi.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI) + " VND") ?? string.Empty, true, true);
                }

                if (models.Where(x => x.ThueGTGT == "KKKNT" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                {
                    string thanhTienTruocThueKKKNT = models.Where(x => x.ThueGTGT == "KKKNT" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThueKKKNT = string.IsNullOrEmpty(thanhTienTruocThueKKKNT) ? string.Empty : "0";
                    string congTienThanhToanKKKNT = models.Where(x => x.ThueGTGT == "KKKNT" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueKKKNT, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueKKKNT, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanKKKNT, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

                if (models.Where(x => x.ThueGTGT == "KCT" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                {
                    string thanhTienTruocThueKCT = models.Where(x => x.ThueGTGT == "KCT" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThueKCT = string.IsNullOrEmpty(thanhTienTruocThueKCT) ? string.Empty : "0";
                    string congTienThanhToanKCT = models.Where(x => x.ThueGTGT == "KCT" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueKCT, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueKCT, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanKCT, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienKhongChiuThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

                if (models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                {
                    string thanhTienTruocThue0 = models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThue0 = models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string congTienThanhToan0 = models.Where(x => x.ThueGTGT == "0" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThue0, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThue0, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToan0, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat0.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

                if (models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                {
                    string thanhTienTruocThue5 = models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThue5 = models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string congTienThanhToan5 = models.Where(x => x.ThueGTGT == "5" && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThue5, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThue5, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToan5, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat5.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

                if (models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                {
                    if (hd.IsGiamTheoNghiQuyet == true && hd.MauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauNhieuThueSuat)
                    {
                        var tblTongTien = table.Rows[table.Rows.Count - 1].Cells[0].Tables[0] as Table;

                        for (int i = 0; i < tblTongTien.Rows.Count; i++)
                        {
                            var find = tblTongTien.Rows[i].Cells[1].Paragraphs[0].Text;
                            bool flag = false;
                            if (find.Contains(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue)))
                            {
                                var par = tblTongTien.Rows[i].Cells[0].Paragraphs[0];
                                foreach (DocumentObject obj in par.ChildObjects)
                                {
                                    if (obj.DocumentObjectType == DocumentObjectType.TextRange)
                                    {
                                        var textRange = obj as TextRange;
                                        textRange.Text = textRange.Text.Replace("10%", "8%");
                                        flag = true;
                                        break;
                                    }
                                }
                            }
                            if (flag)
                            {
                                break;
                            }
                        }

                    }

                    string thanhTienTruocThue10 = models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThue10 = models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string congTienThanhToan10 = models.Where(x => (x.ThueGTGT == "10" || x.ThueGTGT == "8") && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThue10, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThue10, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToan10, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuat10.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

                if (models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Any() && !isDieuChinhThongTin)
                {
                    string thanhTienTruocThueKHAC = models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Sum(x => x.ThanhTien - x.TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThueKHAC = models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Sum(x => x.TienThueGTGT).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string congTienThanhToanKHAC = models.Where(x => x.IsThueKhac == true && x.IsHangKhongTinhTien != true).Sum(x => x.TongTienThanhToan).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueKHAC, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueKHAC, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanKHAC, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

                if (!isDieuChinhThongTin)
                {
                    string thanhTienTruocThueTong = (hd.TongTienHang - hd.TongTienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string tienThueTong = hd.TongTienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                    string congTienThanhToanTong = hd.TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), thanhTienTruocThueTong, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), tienThueTong, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), congTienThanhToanTong, true, true);
                }
                else
                {
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.ThanhTienTruocThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.TienThue), string.Empty, true, true);
                    doc.Replace(LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT.GenerateKeyTagTongHopThueGTGT(MauHoaDonHelper.LoaiTongHopThueGTGT.CongTienThanhToan), string.Empty, true, true);
                }

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

                TableRow row = null;
                for (int i = 0; i < line; i++)
                {
                    row = table.Rows[i + beginRow];
                    int col = row.Cells.Count;

                    for (int j = 0; j < col; j++)
                    {
                        var par = row.Cells[j].Paragraphs[0];

                        par.SetValuePar2(models[i].STT + "", LoaiChiTietTuyChonNoiDung.STT);
                        par.SetValuePar2(models[i].TenHang, LoaiChiTietTuyChonNoiDung.TenHangHoaDichVu);

                        // Chiết khấu thương mại
                        // Ghi chú/diễn giải
                        if (models[i].TinhChat == 4 || isDieuChinhThongTin)
                        {
                            continue;
                        }
                        else
                        {
                            par.SetValuePar2(models[i].DonViTinh?.Ten, LoaiChiTietTuyChonNoiDung.DonViTinh);
                            par.SetValuePar2(models[i].SoLuong.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.SO_LUONG), LoaiChiTietTuyChonNoiDung.SoLuong);
                            par.SetValuePar2(models[i].DonGia.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.DON_GIA_QUY_DOI : LoaiDinhDangSo.DON_GIA_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.DonGia);
                            par.SetValuePar2(models[i].ThanhTien.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.ThanhTien);
                            par.SetValuePar2(models[i].TyLeChietKhau.Value.FormatNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE), LoaiChiTietTuyChonNoiDung.TyLeChietKhauHHDV);
                            par.SetValuePar2(models[i].TienChietKhau.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.TienChietKhauHHDV);
                            par.SetValuePar2((models[i].ThanhTien - models[i].TienChietKhau).Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, false, maLoaiTien), LoaiChiTietTuyChonNoiDung.ThanhTienDaTruCKHHDV);

                            string thueGTGTHHDV = string.Empty;
                            string tienThueHHDV = string.Empty;
                            //if (models[i].ThueGTGT == "KCT" || models[i].ThueGTGT == "KKKNT")
                            //{
                            //    thueGTGTHHDV = "\\";
                            //    tienThueHHDV = "\\";
                            //}
                            //else
                            //{
                            //    thueGTGTHHDV = models[i].ThueGTGT;
                            //    tienThueHHDV = models[i].TienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);
                            //}

                            thueGTGTHHDV = models[i].ThueGTGT;
                            tienThueHHDV = models[i].TienThueGTGT.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien);

                            if (models[i].TinhChat == 2) // km
                            {
                                thueGTGTHHDV = "X";
                                tienThueHHDV = "X";
                            }
                            else
                            {
                                if ((models[i].ThanhTien - models[i].TienChietKhau) == 0)
                                {
                                    tienThueHHDV = string.Empty;
                                }
                                par.SetValuePar2(models[i].TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hd.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien), LoaiChiTietTuyChonNoiDung.ThanhTienSauThueHHDV);
                            }

                            par.SetValuePar2(thueGTGTHHDV, LoaiChiTietTuyChonNoiDung.ThueSuatHHDV);
                            par.SetValuePar2(tienThueHHDV, LoaiChiTietTuyChonNoiDung.TienThueHHDV);
                        }
                    }
                }

                doc.ClearKeyTag();
            }
            else
            {
                MauHoaDonHelper.CreatePreviewFileDoc(doc, mauHoaDon, _IHttpContextAccessor);
            }

            var pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(pdfFolder))
            {
                Directory.CreateDirectory(pdfFolder);
            }

            string pdfFileName = $"{hd.KyHieu}-{hd.SoHoaDon}-{Guid.NewGuid()}.pdf";
            string pdfPath = Path.Combine(pdfFolder, pdfFileName);
            //doc.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);
            doc.SaveToPDF(pdfPath, _hostingEnvironment, mauHoaDon.LoaiNgonNgu);
            path = Path.Combine(pdfFolder, pdfFileName);

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

            byte[] fileByte = File.ReadAllBytes(path);
            if (@params.IsKeepFile == true)
            {
                @params.FilePath = path;
            }
            else
            {
                File.Delete(path);
            }

            return new FileReturn
            {
                Bytes = fileByte,
                ContentType = MimeTypes.GetMimeType(path),
                FileName = Path.GetFileName(path)
            };
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

                var _objHDDT = await GetByIdAsync(param.HoaDonDienTuId);
                if (_objHDDT != null)
                {
                    string oldSignedXmlPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{_objHDDT.XMLDaKy}");
                    if (File.Exists(oldSignedXmlPath))
                    {
                        File.Delete(oldSignedXmlPath);
                    }

                    string newXmlFileName = $"{_objHDDT.BoKyHieuHoaDon.KyHieu}-{param.HoaDon.SoHoaDon}-{Guid.NewGuid()}.xml";
                    string newSignedXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}");
                    if (!Directory.Exists(newSignedXmlFolder))
                    {
                        Directory.CreateDirectory(newSignedXmlFolder);
                    }

                    //xml
                    string xmlDeCode = TextHelper.Decompress(@param.DataXML);
                    string newSignedXmlFullPath = Path.Combine(newSignedXmlFolder, newXmlFileName);
                    File.WriteAllText(newSignedXmlFullPath, xmlDeCode);

                    if (param.HoaDon.ActionUser != null)
                    {
                        _objHDDT.ActionUser = param.HoaDon.ActionUser;
                    }

                    _objHDDT.XMLDaKy = newXmlFileName;
                    _objHDDT.NgayKy = DateTime.Now;
                    _objHDDT.SoHoaDon = param.HoaDon.SoHoaDon;
                    _objHDDT.MaTraCuu = param.HoaDon.MaTraCuu;
                    _objHDDT.NgayHoaDon = param.HoaDon.NgayHoaDon;

                    var hasBangTongHop = await _boKyHieuHoaDonService.HasChuyenTheoBangTongHopDuLieuHDDTAsync(_objHDDT.BoKyHieuHoaDonId);
                    if (param.IsBuyerSigned != true)
                    {
                        var checkDaDungHetSLHD = await _boKyHieuHoaDonService.CheckDaHetSoLuongHoaDonAsync(_objHDDT.BoKyHieuHoaDonId, _objHDDT.SoHoaDon);
                        if (checkDaDungHetSLHD) // đã dùng hết
                        {
                            await _boKyHieuHoaDonService.XacThucBoKyHieuHoaDonAsync(new NhatKyXacThucBoKyHieuViewModel
                            {
                                BoKyHieuHoaDonId = _objHDDT.BoKyHieuHoaDonId,
                                TrangThaiSuDung = TrangThaiSuDung.HetHieuLuc,
                                LoaiHetHieuLuc = LoaiHetHieuLuc.XuatHetSoHoaDon,
                                SoLuongHoaDon = _objHDDT.SoHoaDon,
                                NgayHoaDon = _objHDDT.NgayHoaDon
                            });
                        }

                        // tích chuyển sang bảng tổng hợp thì không gửi tới CQT ngay
                        if (hasBangTongHop)
                        {
                            _objHDDT.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.DaKyDienTu;
                            param.TrangThaiQuyTrinh = _objHDDT.TrangThaiQuyTrinh;
                        }
                        else
                        {
                            // ko tích chuyển sang bảng tổng hợp mới gửi thông điệp
                            #region create thông điêp
                            DuLieuGuiHDDT duLieuGuiHDDT = new DuLieuGuiHDDT
                            {
                                DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                                HoaDonDienTuId = param.HoaDonDienTuId
                            };
                            await _db.DuLieuGuiHDDTs.AddAsync(duLieuGuiHDDT);

                            var thongDiep200 = new ThongDiepChung
                            {
                                ThongDiepChungId = Guid.NewGuid().ToString(),
                                PhienBan = param.HoaDon.TTChungThongDiep.PBan,
                                MaNoiGui = param.HoaDon.TTChungThongDiep.MNGui,
                                MaNoiNhan = param.HoaDon.TTChungThongDiep.MNNhan,
                                MaLoaiThongDiep = int.Parse(param.HoaDon.TTChungThongDiep.MLTDiep),
                                MaThongDiep = param.HoaDon.TTChungThongDiep.MTDiep,
                                SoLuong = param.HoaDon.TTChungThongDiep.SLuong,
                                IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId,
                                NgayGui = DateTime.Now,
                                TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                                MaSoThue = param.HoaDon.TTChungThongDiep.MST,
                                ThongDiepGuiDi = true,
                                Status = true,
                            };
                            await _db.ThongDiepChungs.AddAsync(thongDiep200);
                            await _db.SaveChangesAsync();
                            #endregion

                            // send to CQT
                            var sendResult = await SendDuLieuHoaDonToCQT(newSignedXmlFullPath);

                            _objHDDT.TrangThaiQuyTrinh = sendResult.trangThaiQuyTrinh;
                            string xmlContent999 = sendResult.xmlContent999;
                            param.TrangThaiQuyTrinh = _objHDDT.TrangThaiQuyTrinh;

                            int trangThaiGui;
                            if (_objHDDT.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiTCTNLoi)
                            {
                                trangThaiGui = (int)TrangThaiGuiThongDiep.GuiTCTNLoi;
                            }
                            else if (_objHDDT.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi)
                            {
                                trangThaiGui = (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                            }
                            else
                            {
                                trangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                            }

                            thongDiep200.TrangThaiGui = trangThaiGui;

                            #region create file data and thong diep phan hoi 999
                            List<FileData> fileDatas = new List<FileData>
                            {
                                new FileData
                                {
                                    RefId = thongDiep200.ThongDiepChungId,
                                    Type = 1,
                                    DateTime = DateTime.Now,
                                    Binary = File.ReadAllBytes(newSignedXmlFullPath),
                                    Content = File.ReadAllText(newSignedXmlFullPath),
                                    FileName = newXmlFileName,
                                    IsSigned = true
                                }
                            };
                            if (!string.IsNullOrEmpty(xmlContent999))
                            {
                                // add 999
                                var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(xmlContent999);
                                var thongDiep999 = new ThongDiepChung
                                {
                                    ThongDiepChungId = Guid.NewGuid().ToString(),
                                    PhienBan = tDiep999.TTChung.PBan,
                                    MaNoiGui = tDiep999.TTChung.MNGui,
                                    MaNoiNhan = tDiep999.TTChung.MNNhan,
                                    MaLoaiThongDiep = int.Parse(tDiep999.TTChung.MLTDiep),
                                    MaThongDiep = tDiep999.TTChung.MTDiep,
                                    MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu,
                                    MaSoThue = tDiep999.TTChung.MST,
                                    SoLuong = tDiep999.TTChung.SLuong,
                                    ThongDiepGuiDi = false,
                                    TrangThaiGui = tDiep999.DLieu.TBao.TTTNhan == (int)TTTNhan.KhongLoi ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi,
                                    NgayThongBao = DateTime.Now,
                                    Status = true,
                                };
                                await _db.ThongDiepChungs.AddAsync(thongDiep999);

                                fileDatas.Add(new FileData
                                {
                                    RefId = thongDiep999.ThongDiepChungId,
                                    Type = 1,
                                    DateTime = DateTime.Now,
                                    Binary = Encoding.UTF8.GetBytes(xmlContent999),
                                    Content = xmlContent999,
                                });
                            }

                            await _db.FileDatas.AddRangeAsync(fileDatas);
                            await _db.SaveChangesAsync();
                            #endregion
                        }

                        //thêm bản ghi vào bảng xóa bỏ hóa đơn đối với cấp mã cho hóa đơn thay thế
                        if (!string.IsNullOrWhiteSpace(_objHDDT.ThayTheChoHoaDonId) && _objHDDT.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu)
                        {
                            var _objHDDTBiThayThe = await GetByIdAsync(_objHDDT.ThayTheChoHoaDonId);
                            if (_objHDDTBiThayThe != null)
                            {
                                if (_objHDDTBiThayThe.HinhThucXoabo == null && _objHDDTBiThayThe.TrangThai != 2)
                                {
                                    _objHDDTBiThayThe.NgayXoaBo = DateTime.Now;
                                    _objHDDTBiThayThe.LyDoXoaBo = _objHDDT.LyDoXoaBo;
                                    _objHDDTBiThayThe.IsNotCreateThayThe = null;
                                    if (_objHDDTBiThayThe.TrangThai == 1) //hóa đơn gốc
                                    {
                                        _objHDDTBiThayThe.HinhThucXoabo = 2;
                                    }
                                    else if (_objHDDTBiThayThe.TrangThai == 3) //hóa đơn thay thế
                                    {
                                        _objHDDTBiThayThe.HinhThucXoabo = 5;
                                    }
                                    else
                                    {
                                        _objHDDTBiThayThe.HinhThucXoabo = 2;
                                    }
                                    _objHDDTBiThayThe.BackUpTrangThai = _objHDDTBiThayThe.TrangThai;
                                    _objHDDTBiThayThe.SoCTXoaBo = "XHD-" + (_objHDDTBiThayThe.MauSo ?? "") + "-" + (_objHDDTBiThayThe.KyHieu ?? "") + "-" + (_objHDDTBiThayThe.SoHoaDon + "") + "-" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

                                    //nếu nó chưa bị xóa bỏ
                                    //thì thực hiện xóa bỏ hóa đơn cho nó
                                    ParamXoaBoHoaDon paramXoaBoHoaDon = new ParamXoaBoHoaDon
                                    {
                                        HoaDon = _objHDDTBiThayThe,
                                        OptionalSend = 1
                                    };

                                    await XoaBoHoaDon(paramXoaBoHoaDon);
                                }
                            }
                        }
                    }
                    else
                    {
                        _objHDDT.XMLDaKy = newXmlFileName;
                        _objHDDT.IsBuyerSigned = true;
                        _objHDDT.NgayNguoiMuaKy = DateTime.Now;
                    }

                    await UpdateFileDataXmlForHDDT(_objHDDT.HoaDonDienTuId, newSignedXmlFullPath);
                    await UpdateAsync(_objHDDT);
                }
            }

            return true;
        }

        public async Task<bool> WaitForTCTResonseAsync(string id)
        {
            timeToListenResTCT = 0;
            await SetInterval(id);
            return timeToListenResTCT < 60;
        }

        private async Task SetInterval(string id)
        {
            await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
            timeToListenResTCT += 3;

            var hoaDon = await (from hddt in _db.HoaDonDienTus
                                join bkhhd in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                where hddt.HoaDonDienTuId == id
                                select new HoaDonDienTuViewModel
                                {
                                    HoaDonDienTuId = hddt.HoaDonDienTuId,
                                    TrangThaiQuyTrinh = hddt.TrangThaiQuyTrinh,
                                    BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                                    {
                                        HinhThucHoaDon = bkhhd.HinhThucHoaDon
                                    }
                                })
                                .FirstOrDefaultAsync();

            if (timeToListenResTCT >= 60)
            {
                var laHoaDonGuiTCTNLoi = await CheckLaHoaDonGuiTCTNLoiAsync(id);
                if (laHoaDonGuiTCTNLoi)
                {
                    var hddt = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
                    hddt.FileDaKy = null;
                    hddt.XMLDaKy = null;
                    hddt.NgayKy = null;
                    hddt.SoHoaDon = null;
                    hddt.MaTraCuu = null;

                    var fileDatas = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == true).ToListAsync();
                    if (fileDatas.Any())
                    {
                        _db.FileDatas.RemoveRange(fileDatas);
                    }

                    var nhatKyXacThucBKHs = await _db.NhatKyXacThucBoKyHieus
                        .Where(x => x.BoKyHieuHoaDonId == hddt.BoKyHieuHoaDonId)
                        .OrderByDescending(x => x.CreatedDate)
                        .Take(2)
                        .ToListAsync();

                    if (nhatKyXacThucBKHs.Any(x => x.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc && x.LoaiHetHieuLuc == LoaiHetHieuLuc.XuatHetSoHoaDon))
                    {
                        var nhatKyCuoiCung = nhatKyXacThucBKHs[0];

                        if (nhatKyCuoiCung.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc && nhatKyCuoiCung.LoaiHetHieuLuc == LoaiHetHieuLuc.XuatHetSoHoaDon)
                        {
                            _db.NhatKyXacThucBoKyHieus.Remove(nhatKyCuoiCung);

                            if (nhatKyXacThucBKHs.Count == 2)
                            {
                                var boKyHieuHD = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == hddt.BoKyHieuHoaDonId);
                                boKyHieuHD.TrangThaiSuDung = nhatKyXacThucBKHs[1].TrangThaiSuDung;
                            }
                        }
                    }

                    await _db.SaveChangesAsync();
                }
                return;
            }
            else
            {
                if (hoaDon.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                {
                    var hasThongDiepGui = await (from dlghd in _db.DuLieuGuiHDDTs
                                                 join tdg in _db.ThongDiepChungs on dlghd.DuLieuGuiHDDTId equals tdg.IdThamChieu
                                                 join hddt in _db.HoaDonDienTus on dlghd.HoaDonDienTuId equals hddt.HoaDonDienTuId
                                                 where hddt.HoaDonDienTuId == id
                                                 orderby dlghd.CreatedDate descending
                                                 select tdg.ThongDiepChungId).AnyAsync();

                    if (!hasThongDiepGui) // Nếu không có thông điệp gửi
                    {
                        var xmlFileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Type == 1 && x.RefId == id && x.IsSigned == true);
                        if (xmlFileData != null)
                        {
                            var xmlContent = Encoding.Default.GetString(xmlFileData.Binary);

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xmlContent);
                            XmlNode ttChung = doc.SelectSingleNode("/TDiep/TTChung");

                            var PBan = ttChung.SelectSingleNode("descendant::PBan").InnerText;
                            var MNGui = ttChung.SelectSingleNode("descendant::MNGui").InnerText;
                            var MNNhan = ttChung.SelectSingleNode("descendant::MNNhan").InnerText;
                            var MLTDiep = ttChung.SelectSingleNode("descendant::MLTDiep").InnerText;
                            var MTDiep = ttChung.SelectSingleNode("descendant::MTDiep").InnerText;
                            var MST = ttChung.SelectSingleNode("descendant::MST").InnerText;
                            var SLuong = ttChung.SelectSingleNode("descendant::SLuong").InnerText;

                            int trangThaiGui;
                            var tt999 = await _db.TransferLogs.AsNoTracking().FirstOrDefaultAsync(x => x.MTDTChieu == MTDiep && x.MLTDiep == 999 && x.Type == 3);
                            if (tt999 != null)
                            {
                                var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(tt999.XMLData);
                                if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                                {
                                    trangThaiGui = (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                                }
                                else
                                {
                                    trangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                                }
                            }
                            else
                            {
                                trangThaiGui = (int)TrangThaiGuiThongDiep.GuiTCTNLoi;
                            }

                            var duLieuGuiHDDT = await _db.DuLieuGuiHDDTs
                               .Where(x => x.HoaDonDienTuId == id)
                               .OrderByDescending(x => x.CreatedDate)
                               .AsNoTracking()
                               .FirstOrDefaultAsync();

                            ThongDiepChung thongDiepChung = new ThongDiepChung
                            {
                                ThongDiepChungId = Guid.NewGuid().ToString(),
                                PhienBan = PBan,
                                MaNoiGui = MNGui,
                                MaNoiNhan = MNNhan,
                                MaLoaiThongDiep = int.Parse(MLTDiep),
                                MaThongDiep = MTDiep,
                                SoLuong = int.Parse(SLuong),
                                IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId,
                                NgayGui = duLieuGuiHDDT.CreatedDate,
                                TrangThaiGui = trangThaiGui,
                                MaSoThue = MST,
                                ThongDiepGuiDi = true,
                                Status = true,
                            };
                            await _db.ThongDiepChungs.AddAsync(thongDiepChung);

                            var fileData = new FileData
                            {
                                RefId = thongDiepChung.ThongDiepChungId,
                                Type = 1,
                                DateTime = DateTime.Now,
                                Binary = xmlFileData.Binary,
                                Content = xmlContent,
                                FileName = xmlFileData.FileName,
                                IsSigned = true
                            };

                            await _db.FileDatas.AddAsync(fileData);
                            await _db.SaveChangesAsync();
                        }
                    }
                }
            }

            if (hoaDon != null)
            {
                if (hoaDon.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                {
                    if ((hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) || (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa))
                    {
                        return;
                    }
                }
                else
                {
                    if ((hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe) || (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonKhongHopLe))
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }

            await SetInterval(id);
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
                string assetsFolder = $"FilesUpload/{databaseName}";
                var objHSDetail = await _HoSoHDDTService.GetDetailAsync();

                if (!string.IsNullOrEmpty(param.BienBan.HoaDonDienTuId))
                {
                    var _objHDDT = await this.GetByIdAsync(param.BienBan.HoaDonDienTuId);
                    if (_objHDDT != null)
                    {
                        //// Delete file if exist
                        //string oldSignedPdfPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{_objHDDT.FileDaKy}");
                        //if (File.Exists(oldSignedPdfPath))
                        //{
                        //    File.Delete(oldSignedPdfPath);
                        //}

                        string newPdfFileName = $"BBXB-{Guid.NewGuid()}.pdf";
                        string newSignedPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.PDF_SIGNED);
                        if (!Directory.Exists(newSignedPdfFolder))
                        {
                            Directory.CreateDirectory(newSignedPdfFolder);
                        }

                        var _objTrangThaiLuuTru = await GetTrangThaiLuuTruBBXB(param.BienBan.Id);
                        _objTrangThaiLuuTru = _objTrangThaiLuuTru ?? new LuuTruTrangThaiBBXBViewModel();
                        if (string.IsNullOrEmpty(_objTrangThaiLuuTru.BienBanXoaBoId)) _objTrangThaiLuuTru.BienBanXoaBoId = param.BienBan.Id;

                        // PDF 
                        string signedPdfPath = Path.Combine(newSignedPdfFolder, newPdfFileName);
                        byte[] bytePDF = Convert.FromBase64String(@param.DataPDF);
                        _objTrangThaiLuuTru.PdfDaKy = bytePDF;
                        string newSignedPdfFullPath = Path.Combine(newSignedPdfFolder, newPdfFileName);
                        File.WriteAllBytes(newSignedPdfFullPath, _objTrangThaiLuuTru.PdfDaKy);

                        await UpdateTrangThaiLuuFileBBXB(_objTrangThaiLuuTru);

                        param.BienBan.FileDaKy = newPdfFileName;
                        if (param.TypeKy == 10)
                            param.BienBan.NgayKyBenA = DateTime.Now;
                        else if (param.TypeKy == 11)
                            param.BienBan.NgayKyBenB = DateTime.Now;
                        else return false;

                        var entity = _db.BienBanXoaBos.FirstOrDefault(x => x.Id == param.BienBan.Id);
                        if (entity != null)
                        {
                            _db.Entry(entity).CurrentValues.SetValues(param.BienBan);
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
                        return await UpdateAsync(_objHDDT);
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        [Obsolete]
        public async Task<bool> SendEmail(HoaDonDienTuViewModel hddt, string TenNguoiNhan = "", string ToMail = "")
        {
            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}";
                string pdfFilePath = string.Empty;

                if (hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu)
                {
                    pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{hddt.FileDaKy}");
                    if (!File.Exists(pdfFilePath))
                    {
                        hddt.IsReloadSignedPDF = true;
                        var convertPdf = await ConvertHoaDonToFilePDF(hddt);
                        pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPdf.FilePDF);
                    }
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
                messageBody = messageBody.Replace("##so##", hddt.SoHoaDon.HasValue ? hddt.SoHoaDon.ToString() : "<Chưa cấp số>");
                messageBody = messageBody.Replace("##mauso##", hddt.MauSo);
                messageBody = messageBody.Replace("##kyhieu##", hddt.KyHieu);
                messageBody = messageBody.Replace("##matracuu##", hddt.MaTraCuu);

                var _objHDDT = await this.GetByIdAsync(hddt.HoaDonDienTuId);

                string[] fileUrls = new string[] { pdfFilePath };
                if (await SendEmailAsync(ToMail ?? hddt.EmailNguoiNhanHD, messageTitle, messageBody, fileUrls))
                {
                    _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = hddt.MauSo,
                        KyHieu = hddt.KyHieu,
                        So = hddt.SoHoaDon + "",
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
                        So = hddt.SoHoaDon + "",
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
                Tracert.WriteLog(ex.Message);
                return false;
            }
        }

        [Obsolete]
        private async Task<bool> SendEmailAsync(string toMail, string subject, string message, string[] fileUrl = null, string cc = "", string bcc = "")
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
                if (fileUrl.Length != 0)
                {
                    foreach (var item in fileUrl)
                        if (!string.IsNullOrEmpty(item)) bodyBuilder.Attachments.Add(item);
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

                        return true;
                    }
                    catch (System.Net.Mail.SmtpFailedRecipientsException)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return false;
            }

        }

        [Obsolete]
        public async Task<bool> SendEmailThongBaoSaiThongTinAsync(ParamsSendMailThongBaoSaiThongTin @params)
        {
            //Method này để gửi email thông tin sai sót không phải lập lại hóa đơn cho khách hàng
            try
            {
                var maLoaiTien = @params.MaLoaiTien;
                var _tuyChons = await _TuyChonService.GetAllAsync();

                var banMauEmail = _mp.Map<ConfigNoiDungEmailViewModel>(await _db.ConfigNoiDungEmails.Where(x => x.LoaiEmail == (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon).FirstOrDefaultAsync());

                var salerVM = await _HoSoHDDTService.GetDetailAsync();

                string messageTitle = banMauEmail.TieuDeEmail;
                messageTitle = messageTitle.Replace("##tendonvi##", salerVM != null ? salerVM.TenDonVi : "");
                messageTitle = messageTitle.Replace("##tenkhachhang##", @params.TenKhachHang);

                string messageBody = banMauEmail.NoiDungEmail;
                messageBody = messageBody.Replace("##tendonvi##", salerVM != null ? salerVM.TenDonVi : "");
                messageBody = messageBody.Replace("##tennguoinhan##", @params.TenNguoiNhan);
                messageBody = messageBody.Replace("##so##", @params.SoHoaDon.ToString().Replace("<", "&lt;").Replace(">", "&gt;"));
                messageBody = messageBody.Replace("##mauso##", @params.MauHoaDon);
                messageBody = messageBody.Replace("##kyhieu##", @params.KyHieuHoaDon);
                messageBody = messageBody.Replace("##ngayhoadon##", @params.NgayHoaDon.Value.ToString("dd/MM/yyyy"));
                messageBody = messageBody.Replace("##tongtien##", @params.TongTienThanhToan.GetValueOrDefault().FormatNumberByTuyChon(_tuyChons, (maLoaiTien == "VND") ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty);

                messageBody = messageBody.Replace("##hotennguoimuahang_sai##", @params.HoTenNguoiMuaHang_Sai?.Replace("<", "&lt;").Replace(">", "&gt;"));
                messageBody = messageBody.Replace("##hotennguoimuahang_dung##", @params.HoTenNguoiMuaHang_Dung);
                messageBody = messageBody.Replace("##tendonvi_sai##", @params.TenDonVi_Sai?.Replace("<", "&lt;").Replace(">", "&gt;"));
                messageBody = messageBody.Replace("##tendonvi_dung##", @params.TenDonVi_Dung);
                messageBody = messageBody.Replace("##diachi_sai##", @params.DiaChi_Sai?.Replace("<", "&lt;").Replace(">", "&gt;"));
                messageBody = messageBody.Replace("##diachi_dung##", @params.DiaChi_Dung);

                if (@params.TichChon_HoTenNguoiMuaHang.Value)
                {
                    messageBody = messageBody.Replace("##displayhotennguoimuahang##", "table-row");
                }
                else
                {
                    messageBody = messageBody.Replace("##displayhotennguoimuahang##", "none");
                }

                if (@params.TichChon_TenDonVi.Value)
                {
                    messageBody = messageBody.Replace("##displaytendonvi##", "table-row");
                }
                else
                {
                    messageBody = messageBody.Replace("##displaytendonvi##", "none");
                }

                if (@params.TichChon_DiaChi.Value)
                {
                    messageBody = messageBody.Replace("##displaydiachi##", "table-row");
                }
                else
                {
                    messageBody = messageBody.Replace("##displaydiachi##", "none");
                }

                //insert nhật ký
                var nhatKyGuiEmailViewModel = new NhatKyGuiEmailViewModel
                {
                    NhatKyGuiEmailId = Guid.NewGuid().ToString(),
                    MauSo = @params.MauHoaDon,
                    KyHieu = @params.KyHieuHoaDon,
                    So = @params.SoHoaDon + "",
                    Ngay = @params.NgayHoaDon,
                    TrangThaiGuiEmail = TrangThaiGuiEmail.DangGuiChoKhachHang,
                    LoaiEmail = LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
                    EmailNguoiNhan = @params.EmailCuaNguoiNhan,
                    TenNguoiNhan = @params.TenNguoiNhan,
                    TieuDeEmail = messageTitle,
                    RefId = @params.HoaDonDienTuId,
                    RefType = RefType.HoaDonDienTu,
                    CreatedBy = @params.UserId,
                    ModifyBy = @params.UserId
                };
                await _nhatKyGuiEmailService.InsertAsync(nhatKyGuiEmailViewModel);

                //update nhật ký
                var updateNhatKyGuiEmail = await _db.NhatKyGuiEmails.FirstOrDefaultAsync(x => x.NhatKyGuiEmailId == nhatKyGuiEmailViewModel.NhatKyGuiEmailId);

                var ketQuaGuiEmail = await SendEmailAsync(@params.EmailCuaNguoiNhan, messageTitle, messageBody, new string[] { }, @params.EmailCCNguoiNhan, @params.EmailBCCNguoiNhan);

                if (updateNhatKyGuiEmail != null)
                {
                    if (ketQuaGuiEmail)
                    {
                        updateNhatKyGuiEmail.TrangThaiGuiEmail = TrangThaiGuiEmail.DaGui;

                        //đánh dấu hóa đơn đã gửi thông báo sai thông tin
                        var updateHoaDon = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == @params.HoaDonDienTuId);
                        if (updateHoaDon != null)
                        {
                            updateHoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD = DateTime.Now;
                            //reset trạng thái 04
                            updateHoaDon.IsDaLapThongBao04 = false;
                            updateHoaDon.TrangThaiGui04 = -2;
                            updateHoaDon.ThongDiepGuiCQTId = null;
                        }

                        //cập nhật thông tin khách hàng
                        var khachHang = await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == @params.KhachHangId);
                        if (khachHang != null)
                        {
                            khachHang.HoTenNguoiNhanHD = @params.TenNguoiNhan;
                            khachHang.EmailNguoiNhanHD = @params.EmailCuaNguoiNhan;
                        }

                        //thêm bản ghi thông báo sai thông tin
                        ThongBaoSaiThongTin thongBaoSaiThongTin = new ThongBaoSaiThongTin
                        {
                            Id = Guid.NewGuid().ToString(),
                            DoiTuongId = @params.KhachHangId,
                            HoaDonDienTuId = @params.HoaDonDienTuId,
                            HoTenNguoiMuaHang_Sai = @params.HoTenNguoiMuaHang_Sai,
                            HoTenNguoiMuaHang_Dung = @params.HoTenNguoiMuaHang_Dung,
                            TenDonVi_Dung = @params.TenDonVi_Dung,
                            TenDonVi_Sai = @params.TenDonVi_Sai,
                            DiaChi_Dung = @params.DiaChi_Dung,
                            DiaChi_Sai = @params.DiaChi_Sai,
                            EmailCuaNguoiNhan = @params.EmailCuaNguoiNhan,
                            EmailBCCCuaNguoiNhan = @params.EmailBCCNguoiNhan,
                            EmailCCCuaNguoiNhan = @params.EmailCCNguoiNhan,
                            TenNguoiNhan = @params.TenNguoiNhan,
                            SDTCuaNguoiNhan = @params.SoDienThoaiNguoiNhan,
                            CreatedDate = DateTime.Now,
                            ModifyDate = DateTime.Now
                        };
                        await _db.ThongBaoSaiThongTins.AddAsync(thongBaoSaiThongTin);
                    }
                    else
                    {
                        updateNhatKyGuiEmail.TrangThaiGuiEmail = TrangThaiGuiEmail.GuiLoi;
                    }
                    await _db.SaveChangesAsync();
                }

                return ketQuaGuiEmail;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Obsolete]
        public async Task<bool> SendEmailThongTinHoaDonAsync(ParamsSendMailThongTinHoaDon @params)
        {
            //Method này để gửi email thông báo biên bản hủy hóa đơn cho các hóa đơn khác
            try
            {
                var thongTinHoaDon = await _db.ThongTinHoaDons.FirstOrDefaultAsync(x => x.Id == @params.ThongTinHoaDonId);
                var loaiTien = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == ((thongTinHoaDon != null) ? thongTinHoaDon.LoaiTienId : null));
                var maLoaiTien = loaiTien?.Ma;

                var bbxb = await GetBienBanXoaBoHoaDon(@params.ThongTinHoaDonId);
                var _tuyChons = await _TuyChonService.GetAllAsync();

                var banMauEmail = _mp.Map<ConfigNoiDungEmailViewModel>(await _db.ConfigNoiDungEmails.Where(x => x.LoaiEmail == @params.LoaiEmail).FirstOrDefaultAsync());

                var salerVM = await _HoSoHDDTService.GetDetailAsync();

                string messageTitle = banMauEmail.TieuDeEmail;
                messageTitle = messageTitle.Replace("##tendonvi##", salerVM != null ? salerVM.TenDonVi : "");
                messageTitle = messageTitle.Replace("##so##", thongTinHoaDon.SoHoaDon);
                messageTitle = messageTitle.Replace("##tenkhachhang##", bbxb.TenKhachHang);

                string messageBody = banMauEmail.NoiDungEmail;
                messageBody = messageBody.Replace("##tendonvi##", salerVM != null ? salerVM.TenDonVi : "");
                messageBody = messageBody.Replace("##tenkhachhang##", bbxb.TenKhachHang);
                messageBody = messageBody.Replace("##so##", thongTinHoaDon.SoHoaDon);
                messageBody = messageBody.Replace("##mauso##", thongTinHoaDon.MauSoHoaDon);
                messageBody = messageBody.Replace("##kyhieu##", thongTinHoaDon.KyHieuHoaDon);
                messageBody = messageBody.Replace("##lydohuy##", bbxb.LyDoXoaBo);
                messageBody = messageBody.Replace("##ngayhoadon##", thongTinHoaDon.NgayHoaDon.Value.ToString("dd/MM/yyyy"));
                messageBody = messageBody.Replace("##tongtien##", thongTinHoaDon.ThanhTien.GetValueOrDefault().FormatNumberByTuyChon(_tuyChons, (maLoaiTien == "VND") ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, maLoaiTien) ?? string.Empty);
                messageBody = messageBody.Replace("##duongdanbienban##", @params.Link + "/xem-chi-tiet-bbxb/" + bbxb.Id);

                string[] pdfFilePath = null;

                if (await SendEmailAsync(@params.ToMail, messageTitle, messageBody, pdfFilePath, @params.CC, @params.BCC))
                {
                    thongTinHoaDon.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChoKHKy;
                    _db.ThongTinHoaDons.Update(thongTinHoaDon);
                    await _db.SaveChangesAsync();

                    /*
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
                    */

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = thongTinHoaDon.MauSoHoaDon,
                        KyHieu = thongTinHoaDon.KyHieuHoaDon,
                        So = thongTinHoaDon.SoHoaDon,
                        Ngay = thongTinHoaDon.NgayHoaDon,
                        TrangThaiGuiEmail = TrangThaiGuiEmail.DaGui,
                        LoaiEmail = (LoaiEmail)@params.LoaiEmail,
                        EmailNguoiNhan = @params.ToMail,
                        TenNguoiNhan = bbxb.TenNguoiNhan,
                        TieuDeEmail = messageTitle,
                        RefId = thongTinHoaDon.Id,
                        RefType = RefType.HoaDonDienTu
                    });
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Obsolete]
        public async Task<bool> SendEmailAsync(ParamsSendMail @params)
        {
            try
            {
                var isSystem = true;
                var hddt = await GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                if (hddt == null)
                {
                    hddt = await _thongTinHoaDonService.GetById(@params.HoaDon.HoaDonDienTuId);
                    isSystem = false;
                }
                var bbxb = await GetBienBanXoaBoHoaDon(@params.HoaDon.HoaDonDienTuId);
                var _tuyChons = await _TuyChonService.GetAllAsync();
                BienBanDieuChinh bbdc = null;

                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}";

                string pdfFilePath = string.Empty;
                string xmlFilePath = string.Empty;
                if (isSystem == false || hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                {
                    if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                    {
                        pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{hddt.FileDaKy}");
                        xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.XML_SIGNED}/{hddt.XMLDaKy}");
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                    {
                        if (hddt.TrangThaiBienBanXoaBo > (int)TrangThaiBienBanXoaBo.ChuaKy)
                            pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{bbxb.FileDaKy}");
                        else
                        {
                            var convertPDF = await ConvertBienBanXoaHoaDon(bbxb);
                            pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FilePDF);
                            xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FileXML);
                        }
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    {
                        bbdc = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == @params.BienBanDieuChinhId);
                        if (bbdc.TrangThaiBienBan == (int)TrangThaiBienBanXoaBo.ChuaKy)
                        {
                            pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_UNSIGN}/{bbdc.FileChuaKy}");
                            xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.XML_UNSIGN}/{bbdc.XMLChuaKy}");
                        }
                        else
                        {
                            pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{bbdc.FileDaKy}");
                            xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.XML_SIGNED}/{bbdc.XMLDaKy}");
                        }
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                    {
                        pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{hddt.FileDaKy}");
                    }
                    else
                    {
                        pdfFilePath = string.Empty;
                        xmlFilePath = string.Empty;
                    }
                }
                else
                {
                    var convertPDF = await ConvertHoaDonToFilePDF(hddt);
                    pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FilePDF);
                    xmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, convertPDF.FileXML);
                }

                var banMauEmail = await _db.ConfigNoiDungEmails.Where(x => x.LoaiEmail == @params.LoaiEmail && x.IsDefault != true)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                var salerVM = await _HoSoHDDTService.GetDetailAsync();

                string messageTitle = banMauEmail.TieuDeEmail;
                messageTitle = messageTitle.Replace("##tendonvi##", salerVM.TenDonVi);
                messageTitle = messageTitle.Replace("##loaihoadon##", @params.HoaDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");
                messageTitle = messageTitle.Replace("##so##", hddt.SoHoaDon.HasValue ? hddt.SoHoaDon.ToString() : "<Chưa cấp số>");
                messageTitle = messageTitle.Replace("##tenkhachhang##", hddt.TenKhachHang);

                string messageBody = banMauEmail.NoiDungEmail;
                string TenNguoiNhan = !string.IsNullOrEmpty(@params.TenNguoiNhan) ? @params.TenNguoiNhan : (@params.HoaDon.HoTenNguoiNhanHD ?? string.Empty);
                messageBody = messageBody.Replace("##tendonvi##", salerVM.TenDonVi);
                messageBody = messageBody.Replace("##loaihoadon##", @params.HoaDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng");
                messageBody = messageBody.Replace("##tennguoinhan##", TenNguoiNhan);
                messageBody = messageBody.Replace("##tenkhachhang##", TenNguoiNhan);
                messageBody = messageBody.Replace("##so##", !@params.HoaDon.SoHoaDon.HasValue ? "<Chưa cấp số>" : @params.HoaDon.SoHoaDon.ToString());
                messageBody = messageBody.Replace("##mauso##", @params.HoaDon.MauSo);
                messageBody = messageBody.Replace("##kyhieu##", @params.HoaDon.KyHieu);
                messageBody = messageBody.Replace("##ngayhoadon##", hddt.NgayHoaDon.Value.ToString("dd/MM/yyyy"));
                messageBody = messageBody.Replace("##matracuu##", @params.HoaDon.MaTraCuu);
                messageBody = messageBody.Replace("##linktracuu##", @params.LinkTraCuu);

                if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                {
                    messageBody = messageBody.Replace("##lydohuy##", bbxb.LyDoXoaBo);
                    messageBody = messageBody.Replace("##tongtien##", hddt.TongTienThanhToan.Value.FormatNumberByTuyChon(_tuyChons, hddt.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, hddt.MaLoaiTien) ?? string.Empty);
                    messageBody = messageBody.Replace("##duongdanbienban##", @params.Link + "/xem-chi-tiet-bbxb/" + bbxb.Id);
                }
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                {
                    messageBody = messageBody.Replace("##lydoxoahoadon##", @params.HoaDon.LyDoXoaBo);
                }
                else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                {
                    messageBody = messageBody.Replace("##lydodieuchinh##", bbdc.LyDoDieuChinh);
                    messageBody = messageBody.Replace("##tongtien##", (hddt.TongTienThanhToan ?? 0).FormatNumberByTuyChon(_tuyChons, hddt.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true, hddt.MaLoaiTien) ?? string.Empty);
                    messageBody = messageBody.Replace("##duongdanbienban##", @params.Link + "/xem-chi-tiet-bbdc/" + bbdc.BienBanDieuChinhId);
                }

                var _objHDDT = await this.GetByIdAsync(@params.HoaDon.HoaDonDienTuId);
                if (_objHDDT == null)
                {
                    _objHDDT = await _thongTinHoaDonService.GetById(@params.HoaDon.HoaDonDienTuId);
                }

                string[] fileUrls = new string[] { };
                if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon && !string.IsNullOrEmpty(pdfFilePath) && !string.IsNullOrEmpty(xmlFilePath) && _objHDDT.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu)
                {
                    fileUrls = new string[] { pdfFilePath, xmlFilePath };
                }
                else
                {
                    fileUrls = new string[] { pdfFilePath };
                }

                if (await SendEmailAsync(@params.ToMail, messageTitle, messageBody, fileUrls, @params.CC, @params.BCC))
                {
                    if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoPhatHanhHoaDon)
                    {
                        if (_objHDDT.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu)
                        {
                            _objHDDT.TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.DaGui;
                            _objHDDT.KhachHangDaNhan = true;
                        }
                        else
                        {
                            _objHDDT.TrangThaiGuiHoaDonNhap = (int)TrangThaiGuiHoaDon.DaGui;
                        }
                    }
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoXoaBoHoaDon)
                        _objHDDT.DaGuiThongBaoXoaBoHoaDon = true;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon)
                        _objHDDT.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChoKHKy;
                    else if (@params.LoaiEmail == (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon)
                    {
                        bbdc.TrangThaiBienBan = (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChoKhachHangKy;
                    }

                    await _nhatKyGuiEmailService.InsertAsync(new NhatKyGuiEmailViewModel
                    {
                        MauSo = hddt.MauSo,
                        KyHieu = hddt.KyHieu,
                        So = hddt.SoHoaDon + "",
                        Ngay = hddt.NgayHoaDon,
                        TrangThaiGuiEmail = TrangThaiGuiEmail.DaGui,
                        LoaiEmail = (LoaiEmail)@params.LoaiEmail,
                        EmailNguoiNhan = @params.ToMail,
                        TenNguoiNhan = TenNguoiNhan,
                        TieuDeEmail = messageTitle,
                        RefId = hddt.HoaDonDienTuId,
                        RefType = RefType.HoaDonDienTu
                    });

                    if (isSystem) await this.UpdateAsync(_objHDDT);
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
                        So = hddt.SoHoaDon + "",
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
                Tracert.WriteLog(ex.Message);
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
                                                     CreatedDate = x.CreatedDate,
                                                     NgayGio = x.NgayGio,
                                                     LoaiThaoTac = x.LoaiThaoTac,
                                                     HanhDong = ((LoaiThaoTac)x.LoaiThaoTac).GetDescription(),
                                                     DiaChiIp = x.DiaChiIp,
                                                     MoTa = x.MoTa,
                                                     NguoiThucHien = _mp.Map<UserViewModel>(_db.Users.FirstOrDefault(u => u.UserId == x.NguoiThucHienId))
                                                 })
                                                 .ToListAsync();
        }

        public async Task<BienBanXoaBoViewModel> GetBienBanXoaBoHoaDon(string HoaDonDienTuId)
        {
            try
            {
                string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                //kiểm tra xem có HoaDonDienTuId trong bảng HoaDonDienTus và bảng ThongTinHoaDons
                var hoaDon = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == HoaDonDienTuId);
                if (hoaDon != null)
                {
                    var query = from bbxb in _db.BienBanXoaBos
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
                                    NgayKyBenB = bbxb.NgayKyBenB,
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
                                        HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                        SoHoaDon = hoaDon.SoHoaDon,
                                        NgayHoaDon = hoaDon.NgayHoaDon,
                                        MauSo = hoaDon.MauSo,
                                        KyHieu = hoaDon.KyHieu,
                                        TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                           where tldk.NghiepVuId == (bbxb != null ? bbxb.Id : null)
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
                                                           }).ToList(),
                                    },
                                };
                    var result = await query.FirstOrDefaultAsync();
                    return result;
                }
                else
                {
                    var thongTinHoaDon = await _db.ThongTinHoaDons.FirstOrDefaultAsync(x => x.Id == HoaDonDienTuId);
                    if (thongTinHoaDon != null)
                    {
                        var query = from bbxb in _db.BienBanXoaBos
                                    where bbxb.ThongTinHoaDonId == HoaDonDienTuId
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
                                        NgayKyBenB = bbxb.NgayKyBenB,
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
                                            HoaDonDienTuId = thongTinHoaDon.Id,
                                            StrSoHoaDon = thongTinHoaDon.SoHoaDon,
                                            NgayHoaDon = thongTinHoaDon.NgayHoaDon,
                                            MauSo = thongTinHoaDon.MauSoHoaDon,
                                            KyHieu = thongTinHoaDon.KyHieuHoaDon,
                                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                               where tldk.NghiepVuId == (bbxb != null ? bbxb.Id : null)
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
                                                               }).ToList(),
                                        },
                                    };

                        var result = await query.FirstOrDefaultAsync();
                        return result;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }


        }

        public async Task<bool> CapNhatBienBanXoaBoHoaDon(BienBanXoaBoViewModel bb)
        {
            var f = false;
            try
            {
                var entity = await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == bb.Id);
                _db.Entry<BienBanXoaBo>(entity).CurrentValues.SetValues(bb);
                var flag = await _db.SaveChangesAsync();
                if (flag > 0)
                {
                    //nếu bb.ThongTinHoaDonId = null thì mới cập nhật vào bảng hóa đơn
                    //còn nếu bb.ThongTinHoaDonId != null thì chỉ là cập nhật cho hóa đơn ngoài hệ thống
                    if (!string.IsNullOrWhiteSpace(bb.ThongTinHoaDonId)) return true;

                    //var entityHD = await GetByIdAsync(entity.HoaDonDienTuId);
                    //entityHD.LyDoXoaBo = entity.LyDoXoaBo;
                    //return await UpdateAsync(entityHD);
                    f = true;
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }
            return f;

        }

        [Obsolete]
        public async Task<BienBanXoaBoViewModel> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon @params)
        {
            var entity = _mp.Map<BienBanXoaBo>(@params.Data);
            if (@params.Data.KhachHangId != null)
            {
                var khachHang = _mp.Map<DoiTuongViewModel>(await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == @params.Data.KhachHangId));
                entity.TenNguoiNhan = khachHang.HoTenNguoiNhanHD;
                entity.EmailNguoiNhan = khachHang.EmailNguoiNhanHD;
                entity.SoDienThoaiNguoiNhan = khachHang.SoDienThoaiNguoiNhanHD;
            }
            await _db.BienBanXoaBos.AddAsync(entity);

            if (string.IsNullOrWhiteSpace(@params.Data.ThongTinHoaDonId) == false)
            {
                var entityHD = _db.ThongTinHoaDons.FirstOrDefault(x => x.Id == @params.Data.ThongTinHoaDonId);
                entityHD.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaKy;
                _db.ThongTinHoaDons.Update(entityHD);
            }
            else
            {
                var entityHD = _db.HoaDonDienTus.FirstOrDefault(x => x.HoaDonDienTuId == @params.Data.HoaDonDienTuId);
                //entityHD.LyDoXoaBo = entity.LyDoXoaBo;
                if (entityHD.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo) entityHD.NgayXoaBo = DateTime.Now;
                entityHD.TrangThaiBienBanXoaBo = 1;
                _db.HoaDonDienTus.Update(entityHD);
            }

            var effect = await _db.SaveChangesAsync();

            if (effect > 0)
            {
                if (@params.OptionalSendData == 1)
                {
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(@params.Data.ThongTinHoaDonId) == false)
                    {
                        await SendEmailThongTinHoaDonAsync(
                            new ParamsSendMailThongTinHoaDon
                            {
                                ThongTinHoaDonId = @params.Data.ThongTinHoaDonId,
                                LoaiEmail = (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon
                            });
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
            }

            return _mp.Map<BienBanXoaBoViewModel>(entity);
        }

        public async Task<bool> DeleteBienBanXoaHoaDon(string Id)
        {
            try
            {
                BienBanXoaBo entity = _db.BienBanXoaBos.FirstOrDefault(x => x.Id == Id);
                if (entity != null)
                {
                    LuuTruTrangThaiBBXB entityLuuTruTrangThaiBBXB = await _db.LuuTruTrangThaiBBXBs.Where(x => x.BienBanXoaBoId == Id).FirstOrDefaultAsync();
                    if (entityLuuTruTrangThaiBBXB != null) _db.LuuTruTrangThaiBBXBs.Remove(entityLuuTruTrangThaiBBXB);

                    _db.BienBanXoaBos.Remove(entity);

                    return await _db.SaveChangesAsync() > 0;
                }

            }
            catch (Exception)
            {
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
            var pdfFileName = string.Empty;
            string xmlFileName = string.Empty;

            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}";
            string fullPdfFolder = "";

            if (bb != null)
            {

                var _objHD = await GetByIdAsync(bb.HoaDonDienTuId);
                var _objBB = await GetBienBanXoaBoById(bb.Id);
                if (_objHD.TrangThaiBienBanXoaBo >= 2 && !string.IsNullOrEmpty(_objBB.FileDaKy) && (bb.IsKhachHangKy == false || bb.IsKhachHangKy == null))
                {
                    return new KetQuaConvertPDF
                    {
                        FilePDF = Path.Combine(assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{_objBB.FileDaKy}"),
                    };
                }
                var signA = bb.NgayKyBenA == null ? "(Ký, đóng dấu, ghi rõ họ và tên)" : "(Chữ ký số, chữ ký điện tử)";
                var signB = bb.NgayKyBenB == null ? "(Ký, đóng dấu, ghi rõ họ và tên)" : "(Chữ ký số, chữ ký điện tử)";
                string tenDonViA = bb.TenCongTyBenA ?? string.Empty;
                string tenDonViB = bb.TenKhachHang ?? string.Empty;
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/HoaDonXoaBo/Bien_ban_huy_hoa_don.doc");
                doc.LoadFromFile(docFolder);

                doc.Replace("<CompanyName>", tenDonViA, true, true);
                doc.Replace("<Address>", bb.DiaChiBenA ?? string.Empty, true, true);
                doc.Replace("<Taxcode>", bb.MaSoThueBenA ?? string.Empty, true, true);
                doc.Replace("<Tel>", bb.SoDienThoaiBenA ?? string.Empty, true, true);
                doc.Replace("<Representative>", bb.DaiDienBenA ?? string.Empty, true, true);
                doc.Replace("<Position>", bb.ChucVuBenA ?? string.Empty, true, true);


                doc.Replace("<CustomerCompany>", tenDonViB, true, true);
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


                doc.Replace("<reason>", bb.LyDoXoaBo ?? string.Empty, true, true);
                doc.Replace("<thongtu>", bb.ThongTu ?? string.Empty, true, true);
                doc.Replace("<txtSignA>", signA ?? string.Empty, true, true);
                doc.Replace("<txtSignB>", signB ?? string.Empty, true, true);

                if (bb.NgayKyBenA != null)
                {
                    //var tenKySo = tenDonViA.GetTenKySo();
                    //var signatureImage = ImageHelper.CreateImageSignature(tenKySo.Item1, tenKySo.Item2, LoaiNgonNgu.TiengViet, bb.NgayKyBenA);

                    //TextSelection selection = doc.FindString("<digitalSignatureA>", true, true);
                    //if (selection != null)
                    //{
                    //    DocPicture pic = new DocPicture(doc);
                    //    pic.LoadImage(signatureImage);
                    //    pic.Width = pic.Width * 48 / 100;
                    //    pic.Height = pic.Height * 48 / 100;

                    //    var range = selection.GetAsOneRange();
                    //    var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                    //    range.OwnerParagraph.ChildObjects.Insert(index, pic);
                    //    range.OwnerParagraph.ChildObjects.Remove(range);
                    //}

                    ImageHelper.CreateSignatureBox(doc, tenDonViA, bb.NgayKyBenA, "<digitalSignatureA>");
                }
                else
                {
                    doc.Replace("<digitalSignatureA>", string.Empty, true, true);
                }
                if (bb.NgayKyBenB != null)
                {
                    //var tenKySo = tenDonViB.GetTenKySo();
                    //var signatureImage = ImageHelper.CreateImageSignature(tenKySo.Item1, tenKySo.Item2, LoaiNgonNgu.TiengViet, bb.NgayKyBenB);

                    //TextSelection selection = doc.FindString("<digitalSignatureB>", true, true);
                    //if (selection != null)
                    //{
                    //    DocPicture pic = new DocPicture(doc);
                    //    pic.LoadImage(signatureImage);
                    //    pic.Width = pic.Width * 48 / 100;
                    //    pic.Height = pic.Height * 48 / 100;

                    //    var range = selection.GetAsOneRange();
                    //    var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                    //    range.OwnerParagraph.ChildObjects.Insert(index, pic);
                    //    range.OwnerParagraph.ChildObjects.Remove(range);
                    //}

                    ImageHelper.CreateSignatureBox(doc, tenDonViB, bb.NgayKyBenB, "<digitalSignatureB>");
                }
                else
                {
                    doc.Replace("<digitalSignatureB>", string.Empty, true, true);
                }

                //fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.PDF_UNSIGN);
                fullPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_UNSIGN}");
                #region create folder
                if (!Directory.Exists(fullPdfFolder))
                {
                    Directory.CreateDirectory(fullPdfFolder);
                }
                else
                {
                    if (!string.IsNullOrEmpty(bb.FileChuaKy))
                    {
                        string oldFilePath = Path.Combine(fullPdfFolder, bb.FileChuaKy);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                }
                #endregion

                pdfFileName = $"BBXB-{Guid.NewGuid()}.pdf";
                var entity = await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == bb.Id);
                entity.FileChuaKy = pdfFileName;
                await _db.SaveChangesAsync();

                //doc.SaveToFile(Path.Combine(fullPdfFolder, pdfFileName), Spire.Doc.FileFormat.PDF);
                doc.SaveToPDF(Path.Combine(fullPdfFolder, pdfFileName), _hostingEnvironment, LoaiNgonNgu.TiengViet);

                path = Path.Combine(assetsFolder, ManageFolderPath.PDF_UNSIGN, pdfFileName);
            }
            string fullPdfFilePath = Path.Combine(fullPdfFolder, pdfFileName);

            return new KetQuaConvertPDF
            {
                FilePDF = path,
                PdfName = pdfFileName,
                XMLName = xmlFileName,
                PDFBase64 = fullPdfFilePath.EncodeFile()
            };
        }

        public async Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonThayTheAsync(HoaDonThayTheParams @params)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            //query ra các file đính kèm (đọc ra file đính kèm trước để ko truy cập vào database nhiều lần)
            var listTaiLieuDinhKems = await _db.TaiLieuDinhKems.ToListAsync();

            //đọc ra trước các hóa đơn để lấy ra hóa đơn thay thế, hóa đơn điều chỉnh tại mỗi dòng hóa đơn đang duyệt
            //mục đích thêm code này để hiển thị cột thông báo sai sót theo yêu của a Kiên
            //cột này hiển thị ở cả 4 tab hóa đơn
            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                         select new HoaDonDienTu
                                                         {
                                                             HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                             SoHoaDon = hoaDon.SoHoaDon,
                                                             ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                             DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                             NgayHoaDon = hoaDon.NgayHoaDon,
                                                             TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                             MaCuaCQT = hoaDon.MaCuaCQT,
                                                             ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                             TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                             LanGui04 = hoaDon.LanGui04,
                                                             IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                             CreatedDate = hoaDon.CreatedDate
                                                         }).ToListAsync();

            //đọc ra kỳ kế toán hiện tại
            //mục đích đọc ra là để hiển thị tình trạng quá hạn/trong hạn của mỗi hóa đơn theo yêu cầu của a Kiên
            var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            //đọc ra danh sách ủy nhiệm lập hóa đơn
            var queryBoKyHieuHoaDon = await (from boKyHieuHoaDon in _db.BoKyHieuHoaDons
                                             select new DLL.Entity.QuanLy.BoKyHieuHoaDon
                                             {
                                                 BoKyHieuHoaDonId = boKyHieuHoaDon.BoKyHieuHoaDonId,
                                                 HinhThucHoaDon = boKyHieuHoaDon.HinhThucHoaDon,
                                                 KyHieuMauSoHoaDon = boKyHieuHoaDon.KyHieuMauSoHoaDon,
                                                 KyHieuHoaDon = boKyHieuHoaDon.KyHieuHoaDon,
                                                 UyNhiemLapHoaDon = boKyHieuHoaDon.UyNhiemLapHoaDon,
                                                 KyHieu1 = boKyHieuHoaDon.KyHieu1,
                                                 KyHieu56 = boKyHieuHoaDon.KyHieu56,
                                             }).ToListAsync();

            //query ra các id hóa đơn đã bị thay thế
            var listHoaDonBiThayTheIds = listHoaDonDienTu.Select(x => x.ThayTheChoHoaDonId).Distinct();

            //query ra các hóa đơn thay thế
            var query = from hd in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbxb in _db.BienBanXoaBos on hd.HoaDonDienTuId equals bbxb.HoaDonDienTuId into tmpBienBanXoaBos
                        from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                        join bkhhd in queryBoKyHieuHoaDon on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieuHoaDon
                        from bkhhd in tmpBoKyHieuHoaDon.DefaultIfEmpty()
                        where hd.NgayHoaDon.Value.Date >= fromDate && hd.NgayHoaDon.Value.Date <= toDate
                        && string.IsNullOrWhiteSpace(hd.ThayTheChoHoaDonId) == false //hiện ra các hóa đơn thay thế
                        && listHoaDonBiThayTheIds.Contains(hd.HoaDonDienTuId) == false //và loại ra những hóa đơn đã bị thay thế
                        && @params.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)

                        orderby hd.NgayHoaDon descending, hd.SoHoaDon descending
                        select new HoaDonDienTuViewModel
                        {
                            ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, null),
                            ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                            Key = Guid.NewGuid().ToString(),
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                            BienBanXoaBoId = bbxb != null ? bbxb.Id : null,
                            NgayBienBanXoaBo = bbxb != null ? bbxb.NgayBienBan : null,
                            LyDoThayThe = hd.LyDoThayThe,
                            LoaiApDungHoaDonCanThayThe = 1,
                            TenHinhThucHoaDonCanThayThe = ((HinhThucHoaDonCanThayThe)1).GetDescription(), //mặc định luôn loại 1
                            NgayXoaBo = hd.NgayXoaBo,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            TenTrangThaiBienBanXoaBo = ((TrangThaiBienBanXoaBo)hd.TrangThaiBienBanXoaBo).GetDescription(),
                            //TrangThai = hd.TrangThai,
                            //TenTrangThaiHoaDon = hd.TrangThai.HasValue ? ((TrangThaiHoaDon)hd.TrangThai).GetDescription() : string.Empty,
                            HinhThucXoabo = hd.HinhThucXoabo,
                            TrangThai = 3, //mặc định là thay thế
                            TenTrangThaiHoaDon = "Thay thế",
                            DienGiaiTrangThaiHoaDon = HoaDonHelper.GetDienGiaiTrangThaiHoaDon(hd.HinhThucXoabo, hd.TrangThaiGuiHoaDon),
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            TenTrangThaiQuyTrinh = hd.TrangThaiQuyTrinh.HasValue ? ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription() : string.Empty,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                            MaTraCuu = hd.MaTraCuu,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                            NgayHoaDon = hd.NgayHoaDon,
                            SoHoaDon = hd.SoHoaDon,
                            IsCoSoHoaDon = hd.SoHoaDon.HasValue,
                            MaCuaCQT = (bkhhd != null) ? ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hd.MaCuaCQT ?? "<Chưa cấp mã>") : "") : "",
                            MauSo = (bkhhd != null) ? bkhhd.KyHieuMauSoHoaDon.ToString() : "",
                            KyHieu = (bkhhd != null) ? (bkhhd.KyHieuHoaDon ?? "") : "",
                            KhachHangId = hd.KhachHangId,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            MaSoThue = hd.MaSoThue,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            LoaiTienId = hd.LoaiTienId,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            TongTienThanhToan = hd.TongTienThanhToanQuyDoi,
                            DaLapHoaDonThayThe = false,
                            TenUyNhiemLapHoaDon = (bkhhd != null) ? bkhhd.UyNhiemLapHoaDon.GetDescription() : "",
                            TaiLieuDinhKems = (from tldk in listTaiLieuDinhKems
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
                                                   Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                   Status = tldk.Status
                                               }).ToList(),
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                KyHieu1 = bkhhd.KyHieu1,
                                KyHieu56 = bkhhd.KyHieu56,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                            }
                        };

            //query hóa đơn xóa bỏ ở bảng hóa đơn chính
            var queryXoaBo = from hd in _db.HoaDonDienTus
                             join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                             from lt in tmpLoaiTiens.DefaultIfEmpty()
                             join bbxb in _db.BienBanXoaBos on hd.HoaDonDienTuId equals bbxb.HoaDonDienTuId into tmpBienBanXoaBos
                             from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                             join bkhhd in queryBoKyHieuHoaDon on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieuHoaDon
                             from bkhhd in tmpBoKyHieuHoaDon.DefaultIfEmpty()
                             where hd.HinhThucXoabo != null
                             select new HoaDonDienTuViewModel
                             {
                                 ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, null),
                                 ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                 Key = Guid.NewGuid().ToString(),
                                 HoaDonDienTuId = hd.HoaDonDienTuId,
                                 ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                                 BienBanXoaBoId = bbxb != null ? bbxb.Id : null,
                                 NgayBienBanXoaBo = bbxb != null ? bbxb.NgayBienBan : null,
                                 LyDoThayThe = string.Empty,
                                 LoaiApDungHoaDonCanThayThe = 1,
                                 TenHinhThucHoaDonCanThayThe = ((HinhThucHoaDonCanThayThe)1).GetDescription(), //mặc định luôn loại 1
                                 NgayXoaBo = hd.NgayXoaBo,
                                 LyDoXoaBo = hd.LyDoXoaBo,
                                 TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                 TenTrangThaiBienBanXoaBo = ((TrangThaiBienBanXoaBo)hd.TrangThaiBienBanXoaBo).GetDescription(),
                                 //TrangThai = hd.TrangThai,
                                 //TenTrangThaiHoaDon = hd.TrangThai.HasValue ? ((TrangThaiHoaDon)hd.TrangThai).GetDescription() : string.Empty,
                                 HinhThucXoabo = hd.HinhThucXoabo,
                                 TrangThai = 2, //2 = hóa đơn xóa bỏ
                                 TenTrangThaiHoaDon = (string.IsNullOrWhiteSpace(hd.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hd.DieuChinhChoHoaDonId)) ? "Hóa đơn gốc" : "Thay thế",
                                 DienGiaiTrangThaiHoaDon = "Bị thay thế",
                                 TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                                 TenTrangThaiQuyTrinh = hd.TrangThaiQuyTrinh.HasValue ? ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription() : string.Empty,
                                 TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                 TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                                 MaTraCuu = hd.MaTraCuu,
                                 LoaiHoaDon = hd.LoaiHoaDon,
                                 TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                 NgayHoaDon = hd.NgayHoaDon,
                                 SoHoaDon = hd.SoHoaDon,
                                 IsCoSoHoaDon = hd.SoHoaDon.HasValue,
                                 MaCuaCQT = (bkhhd != null) ? ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hd.MaCuaCQT ?? "<Chưa cấp mã>") : "") : "",
                                 MauSo = (bkhhd != null) ? bkhhd.KyHieuMauSoHoaDon.ToString() : "",
                                 KyHieu = (bkhhd != null) ? (bkhhd.KyHieuHoaDon ?? "") : "",
                                 KhachHangId = hd.KhachHangId,
                                 MaKhachHang = hd.MaKhachHang,
                                 TenKhachHang = hd.TenKhachHang,
                                 MaSoThue = hd.MaSoThue,
                                 HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                                 TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                 LoaiTienId = hd.LoaiTienId,
                                 MaLoaiTien = lt != null ? lt.Ma : "VND",
                                 TongTienThanhToan = hd.TongTienThanhToanQuyDoi,
                                 DaLapHoaDonThayThe = true,
                                 TenUyNhiemLapHoaDon = (bkhhd != null) ? bkhhd.UyNhiemLapHoaDon.GetDescription() : "",
                                 CreatedDate = hd.CreatedDate,
                                 TaiLieuDinhKems = (from tldk in listTaiLieuDinhKems
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
                                                        Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                        Status = tldk.Status
                                                    }).ToList(),
                                 BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                                 {
                                     BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                     KyHieu = bkhhd.KyHieu,
                                     MauHoaDonId = bkhhd.MauHoaDonId,
                                     KyHieu1 = bkhhd.KyHieu1,
                                     KyHieu56 = bkhhd.KyHieu56,
                                     HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                     TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                     UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                     TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                                 }
                             };

            //query hóa đơn xóa bỏ từ bảng nhập thông tin khác
            var queryXoaBoBangNgoai = from hd in _db.ThongTinHoaDons
                                      join bbxb in _db.BienBanXoaBos on hd.Id equals bbxb.ThongTinHoaDonId into tmpBienBanXoaBos
                                      from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                                      join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                      from lt in tmpLoaiTiens.DefaultIfEmpty()
                                      select new HoaDonDienTuViewModel
                                      {
                                          ThongBaoSaiSot = GetCotThongBaoSaiSotHoaDon32(hd,
                                          XacDinhHoaDonDienTuLienQuan(listHoaDonDienTu, hd.Id, "thayThe"), "thayThe", null, queryBoKyHieuHoaDon),
                                          ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                          Key = Guid.NewGuid().ToString(),
                                          HoaDonDienTuId = hd.Id,
                                          BienBanXoaBoId = bbxb.Id ?? null,
                                          NgayBienBanXoaBo = bbxb.NgayBienBan ?? null,
                                          LyDoThayThe = string.Empty,
                                          LoaiApDungHoaDonCanThayThe = hd.HinhThucApDung,
                                          TenHinhThucHoaDonCanThayThe = ((HinhThucHoaDonCanThayThe)hd.HinhThucApDung).GetDescription(),
                                          NgayXoaBo = bbxb.NgayBienBan ?? null,
                                          LyDoXoaBo = bbxb.LyDoXoaBo ?? null,
                                          TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                          TenTrangThaiBienBanXoaBo = ((TrangThaiBienBanXoaBo)hd.TrangThaiBienBanXoaBo).GetDescription(),
                                          TrangThai = 2, //hóa đơn xóa bỏ
                                          TenTrangThaiHoaDon = (hd.TrangThaiHoaDon != null) ? ((TrangThaiHoaDonNgoaiHeThong)hd.TrangThaiHoaDon).GetDescription() : "Hóa đơn xóa bỏ",
                                          DienGiaiTrangThaiHoaDon = "Bị thay thế",
                                          TrangThaiQuyTrinh = -1,//mặc định
                                          TenTrangThaiQuyTrinh = "",//mặc định
                                          TrangThaiGuiHoaDon = 3,//mặc định
                                          TenTrangThaiGuiHoaDon = ((LoaiTrangThaiGuiHoaDon)3).GetDescription(),//mặc định
                                          MaTraCuu = hd.MaTraCuu,//mặc định
                                          LoaiHoaDon = 0, //mặc định
                                          TenLoaiHoaDon = "",//mặc định (tên loại có thể xem bổ sung sau nếu có)
                                          NgayHoaDon = hd.NgayHoaDon,
                                          StrSoHoaDon = hd.SoHoaDon,
                                          MaCuaCQT = hd.MaCQTCap,
                                          MauSo = hd.MauSoHoaDon,
                                          KyHieu = hd.KyHieuHoaDon,
                                          KhachHangId = null,
                                          MaKhachHang = "",//mặc định
                                          TenKhachHang = "",//mặc định
                                          MaSoThue = "",//mặc định
                                          HoTenNguoiMuaHang = "",//mặc định
                                          TenNhanVienBanHang = "",//mặc định
                                          LoaiTienId = hd.LoaiTienId,//mặc định
                                          MaLoaiTien = lt.Ma,//mặc định
                                          TongTienThanhToan = hd.ThanhTien, //mặc định,
                                          DaLapHoaDonThayThe = true,
                                          CreatedDate = hd.CreatedDate,
                                          HoaDonNgoaiHeThong = true,
                                          TaiLieuDinhKems = (from tldk in listTaiLieuDinhKems
                                                             where tldk.NghiepVuId == hd.Id
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
                                                             }).ToList()
                                      };

            //query hóa đơn gốc chưa xóa bỏ nhưng đã lập hóa đơn thay thế
            var queryHDDaLapTTChuaXoaBo = from hd in _db.HoaDonDienTus
                                          join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                          from lt in tmpLoaiTiens.DefaultIfEmpty()
                                          join bbxb in _db.BienBanXoaBos on hd.HoaDonDienTuId equals bbxb.HoaDonDienTuId into tmpBienBanXoaBos
                                          from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                                          join bkhhd in queryBoKyHieuHoaDon on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieuHoaDon
                                          from bkhhd in tmpBoKyHieuHoaDon.DefaultIfEmpty()
                                          where listHoaDonBiThayTheIds.Contains(hd.HoaDonDienTuId) == true
                                          && hd.TrangThai != 2
                                          select new HoaDonDienTuViewModel
                                          {
                                              ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, null),
                                              ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                              Key = Guid.NewGuid().ToString(),
                                              HoaDonDienTuId = hd.HoaDonDienTuId,
                                              ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                                              BienBanXoaBoId = bbxb != null ? bbxb.Id : null,
                                              NgayBienBanXoaBo = bbxb != null ? bbxb.NgayBienBan : null,
                                              LyDoThayThe = hd.LyDoThayThe,
                                              LoaiApDungHoaDonCanThayThe = 1,
                                              TenHinhThucHoaDonCanThayThe = ((HinhThucHoaDonCanThayThe)1).GetDescription(), //mặc định luôn loại 1
                                              NgayXoaBo = hd.NgayXoaBo,
                                              LyDoXoaBo = hd.LyDoXoaBo,
                                              TenTrangThaiBienBanXoaBo = ((TrangThaiBienBanXoaBo)hd.TrangThaiBienBanXoaBo).GetDescription(),
                                              //TrangThai = hd.TrangThai,
                                              //TenTrangThaiHoaDon = hd.TrangThai.HasValue ? ((TrangThaiHoaDon)hd.TrangThai).GetDescription() : string.Empty,
                                              HinhThucXoabo = hd.HinhThucXoabo,
                                              TrangThai = 1, //mặc định là HĐ gốc
                                              TenTrangThaiHoaDon = (string.IsNullOrWhiteSpace(hd.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hd.DieuChinhChoHoaDonId)) ? "Hóa đơn gốc" : "Thay thế",
                                              DienGiaiTrangThaiHoaDon = "Bị thay thế",
                                              TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                                              TenTrangThaiQuyTrinh = hd.TrangThaiQuyTrinh.HasValue ? ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription() : string.Empty,
                                              TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                              TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                                              MaTraCuu = hd.MaTraCuu,
                                              LoaiHoaDon = hd.LoaiHoaDon,
                                              TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                              TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                              NgayHoaDon = hd.NgayHoaDon,
                                              SoHoaDon = hd.SoHoaDon,
                                              IsCoSoHoaDon = hd.SoHoaDon.HasValue,
                                              MaCuaCQT = (bkhhd != null) ? ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hd.MaCuaCQT ?? "<Chưa cấp mã>") : "") : "",
                                              MauSo = (bkhhd != null) ? bkhhd.KyHieuMauSoHoaDon.ToString() : "",
                                              KyHieu = (bkhhd != null) ? (bkhhd.KyHieuHoaDon ?? "") : "",
                                              KhachHangId = hd.KhachHangId,
                                              MaKhachHang = hd.MaKhachHang,
                                              TenKhachHang = hd.TenKhachHang,
                                              MaSoThue = hd.MaSoThue,
                                              HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                                              TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                              LoaiTienId = hd.LoaiTienId,
                                              MaLoaiTien = lt != null ? lt.Ma : "VND",
                                              TongTienThanhToan = hd.TongTienThanhToanQuyDoi,
                                              DaLapHoaDonThayThe = false,
                                              TenUyNhiemLapHoaDon = (bkhhd != null) ? bkhhd.UyNhiemLapHoaDon.GetDescription() : "",
                                              TaiLieuDinhKems = (from tldk in listTaiLieuDinhKems
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
                                                                     Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                                     Status = tldk.Status
                                                                 }).ToList(),
                                              BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                                              {
                                                  BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                                  KyHieu = bkhhd.KyHieu,
                                                  MauHoaDonId = bkhhd.MauHoaDonId,
                                                  KyHieu1 = bkhhd.KyHieu1,
                                                  KyHieu56 = bkhhd.KyHieu56,
                                                  HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                                  TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                                  UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                                  TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                                              }
                                          };

            if (@params.TrangThaiQuyTrinh != -1)
            {
                query = query.Where(x => x.TrangThaiQuyTrinh.HasValue && x.TrangThaiQuyTrinh == @params.TrangThaiQuyTrinh);
            }

            if (@params.LoaiTrangThaiGuiHoaDon != -1)
            {
                query = query.Where(x => x.TrangThaiGuiHoaDon.HasValue && x.TrangThaiGuiHoaDon == @params.LoaiTrangThaiGuiHoaDon);
            }

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrWhiteSpace(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon != null && x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon != null && x.SoHoaDon.ToString().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue != null && x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHang != null && x.MaKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang != null && x.TenKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang != null && x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
            }
            else
            {
                //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                if (string.IsNullOrWhiteSpace(@params.TimKiemBatKy) == false)
                {
                    @params.TimKiemBatKy = @params.TimKiemBatKy.ToUpper().ToTrim();
                    query = query.Where(x =>
                        (x.TenLoaiHoaDon != null && x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.SoHoaDon != null && x.SoHoaDon.ToString().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.MaSoThue != null && x.MaSoThue.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.MaKhachHang != null && x.MaKhachHang.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.TenKhachHang != null && x.TenKhachHang.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.HoTenNguoiMuaHang != null && x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(@params.TimKiemBatKy))
                    );
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
                    case nameof(@params.Filter.TenTrangThaiQuyTrinh):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiQuyTrinh);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiQuyTrinh);
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
            else
            {
                query = query.OrderByDescending(x => x.NgayHoaDon.Value.Date)
                    .ThenByDescending(x => x.MauSo)
                    .ThenBy(x => x.BoKyHieuHoaDon.KyHieu1)
                    .ThenBy(x => x.BoKyHieuHoaDon.KyHieu56)
                    .ThenBy(x => x.IsCoSoHoaDon)
                    .ThenByDescending(x => x.SoHoaDon);
            }
            #endregion

            var listThayThe = await query.ToListAsync();
            var listXoaBo = await (queryXoaBo.Union(queryXoaBoBangNgoai)).ToListAsync();
            var listHDDaLapTTChuaXoaBo = await queryHDDaLapTTChuaXoaBo.ToListAsync();

            foreach (var item in listThayThe)
            {
                if (listXoaBo.Any(x => x.HoaDonDienTuId == item.ThayTheChoHoaDonId && x.HoaDonDienTuId != item.HoaDonDienTuId))
                {
                    //điều kiện: x.HoaDonDienTuId != item.HoaDonDienTuId để đảm bảo ko xuất hiện hóa đơn thay thế (đã bị xóa bỏ)
                    //trong danh sách hóa đơn xóa bỏ
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
                    //order by lại danh sách hóa đơn xóa bỏ
                    item.Children = item.Children.OrderByDescending(x => x.NgayXoaBo != null ? x.NgayXoaBo : x.CreatedDate).ToList();
                }
                if (listHDDaLapTTChuaXoaBo.Any(x => x.HoaDonDienTuId == item.ThayTheChoHoaDonId))
                {
                    //trong danh sách hóa đơn chưa xóa bỏ
                    item.Children = new List<HoaDonDienTuViewModel>();

                    var hoaDon = listHDDaLapTTChuaXoaBo.Where(x => x.HoaDonDienTuId == item.ThayTheChoHoaDonId).ToList();
                    Queue<HoaDonDienTuViewModel> queue2 = new Queue<HoaDonDienTuViewModel>(hoaDon);
                    while (queue2.Count() != 0)
                    {
                        var dequeue2 = queue2.Dequeue();
                        item.Children.Insert(0, dequeue2);
                        if (!string.IsNullOrEmpty(dequeue2.ThayTheChoHoaDonId) && listHDDaLapTTChuaXoaBo.Any(x => x.HoaDonDienTuId == dequeue2.ThayTheChoHoaDonId))
                        {
                            var hoaDonXoaBoInQueues = listHDDaLapTTChuaXoaBo.Where(x => x.HoaDonDienTuId == dequeue2.ThayTheChoHoaDonId).ToList();
                            foreach (var child in hoaDonXoaBoInQueues)
                            {
                                queue2.Enqueue(child);
                            }
                        }
                        else if (!string.IsNullOrEmpty(dequeue2.ThayTheChoHoaDonId) && listXoaBo.Any(x => x.HoaDonDienTuId == dequeue2.ThayTheChoHoaDonId))
                        {
                            var hoaDonXoaBoInQueues = listXoaBo.Where(x => x.HoaDonDienTuId == dequeue2.ThayTheChoHoaDonId).ToList();
                            foreach (var child in hoaDonXoaBoInQueues)
                            {
                                queue2.Enqueue(child);
                            }
                        }
                    }

                    //order by lại danh sách hóa đơn xóa bỏ
                    item.Children = item.Children.OrderByDescending(x => x.SoHoaDon).ToList();
                }
            }

            //hiển thị cây phả hệ lại
            List<CayThayTheViewModel> listCayThayTheViewModel = new List<CayThayTheViewModel>();

            foreach (var item in listThayThe)
            {
                if (item.Children != null)
                {
                    if (item.Children.Count > 0)
                    {
                        listCayThayTheViewModel.Add(new CayThayTheViewModel
                        {
                            HoaDonDienTuChaId = item.HoaDonDienTuId,
                            HoaDonDienTuId = item.Children[0].HoaDonDienTuId,
                            CreatedDate = item.CreatedDate
                        });
                    }
                }
            }

            List<string> listDuyNhat = listCayThayTheViewModel.Select(x => x.HoaDonDienTuId).Distinct().ToList();

            foreach (var item in listDuyNhat)
            {
                var listtam = listCayThayTheViewModel.Where(x => x.HoaDonDienTuId == item).OrderByDescending(y => y.CreatedDate).ToList();

                if (listtam.Count > 1)
                {
                    var caydautien = listThayThe.Where(x => x.HoaDonDienTuId == listtam[0].HoaDonDienTuChaId).FirstOrDefault();
                    var caydautien123 = listThayThe.Where(x => x.HoaDonDienTuId == listtam[0].HoaDonDienTuChaId).FirstOrDefault();
                    var caycuoicung = listThayThe.Where(x => x.HoaDonDienTuId == listtam[listtam.Count - 1].HoaDonDienTuChaId).FirstOrDefault();

                    List<HoaDonDienTuViewModel> listtamthoi = new List<HoaDonDienTuViewModel>();

                    List<HoaDonDienTuViewModel> cacConCuoi = new List<HoaDonDienTuViewModel>();

                    var listtamthoitiep = listtam.Where(x => x.HoaDonDienTuChaId != caydautien.HoaDonDienTuId).ToList();
                    for (int i = 0; i < listtamthoitiep.Count(); i++)
                    {
                        var giatritam = listThayThe.Where(x => x.HoaDonDienTuId == listtamthoitiep[i].HoaDonDienTuChaId).FirstOrDefault();

                        if (i != listtamthoitiep.Count() - 1)
                        {
                            //nếu ko phải item cuối thì remove Children
                            giatritam.Children = null;
                        }
                        else
                        {
                            cacConCuoi = giatritam.Children;
                            giatritam.Children = null;
                        }

                        //remove
                        listThayThe.Remove(giatritam);

                        giatritam.IsChildThayThe = true;
                        listtamthoi.Add(giatritam);
                    }


                    caydautien.Children = listtamthoi;
                    //add đến con

                    var caccon = caycuoicung.Children;
                    if (cacConCuoi != null)
                    {
                        //caycuoicung.Children = null;
                        foreach (var item2 in cacConCuoi)
                        {
                            caydautien.Children.Add(item2);
                        }
                        // listThayThe.Remove(caydautien123);
                        //add lai
                        //listThayThe.Add(caydautien);
                    }

                }
            }

            //sap xep lai
            //  listThayThe = listThayThe.OrderByDescending(x => x.NgayHoaDon).ThenByDescending(y => y.SoHoaDon).ToList();

            //sap xep cac con

            //foreach (var item in listThayThe)
            //{
            //    if (string.IsNullOrEmpty(item.MaTraCuu))
            //    {
            //    item.Children = item.Children.OrderByDescending(x => x.CreatedDate).ToList();
            //    }
            //}

            return PagedList<HoaDonDienTuViewModel>
                    .CreateAsyncWithList(listThayThe, @params.PageNumber, @params.PageSize);
        }

        public List<EnumModel> GetLoaiTrangThaiPhatHanhs()
        {
            List<EnumModel> enums = ((TrangThaiQuyTrinh[])Enum.GetValues(typeof(TrangThaiQuyTrinh)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).OrderBy(x => x.Value).ToList();

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
                _objHDDT.IsNotCreateThayThe = @params.HoaDon.IsNotCreateThayThe;
                _objHDDT.HinhThucXoabo = @params.HoaDon.HinhThucXoabo;
                if (@params.HoaDon.TrangThaiBienBanXoaBo == -10) _objHDDT.TrangThaiBienBanXoaBo = @params.HoaDon.TrangThaiBienBanXoaBo;
                _objHDDT.BackUpTrangThai = @params.HoaDon.BackUpTrangThai;
                _objHDDT.TrangThai = (int)TrangThaiHoaDon.HoaDonXoaBo;

                if (await this.UpdateAsync(_objHDDT))
                {

                    if (_objHDDT.TrangThai == 2)
                    {
                        var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                        string assetsFolder = $"FilesUpload/{databaseName}";
                        var pdfPath = Path.Combine(assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{_objHDDT.FileDaKy}");
                        string pdfFilePath = Path.Combine(_hostingEnvironment.WebRootPath, pdfPath);
                        //thêm ảnh đã bị xóa vào file pdf
                        if (@File.Exists(pdfFilePath))
                        {
                            string mauHoaDonImg = Path.Combine(_hostingEnvironment.WebRootPath, "images/template/dabixoabo.png");
                            PdfDocument pdfDoc = new PdfDocument();
                            pdfDoc.LoadFromFile(pdfFilePath);
                            PdfImage image = PdfImage.FromFile(mauHoaDonImg);

                            int pdfPageCount = pdfDoc.Pages.Count;
                            for (int i = 0; i < pdfPageCount; i++)
                            {
                                PdfPageBase page = pdfDoc.Pages[i];
                                page.Canvas.SetTransparency(0.7f, 0.7f, PdfBlendMode.Normal);
                                page.Canvas.DrawImage(image, new PointF(130, 270), new SizeF(350, 350));
                            }

                            pdfDoc.SaveToFile(pdfFilePath);
                            pdfDoc.Close();
                        }
                    }
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
                                SoHoaDonDieuChinh = hddc.SoHoaDon,
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
                                TrangThaiPhatHanhDieuChinh = hddc.TrangThaiQuyTrinh,
                                TenTrangThaiPhatHanhDieuChinh = hddc.TrangThaiQuyTrinh.HasValue ? ((LoaiTrangThaiPhatHanh)hddc.TrangThaiQuyTrinh).GetDescription() : string.Empty,
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
                                                       Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
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
                                 SoHoaDonBiDieuChinh = hdbdc.SoHoaDon,
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
                                 TrangThaiPhatHanhDieuChinh = hddc.TrangThaiQuyTrinh,
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
                                                        Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
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

            if (@params.LoaiTrangThaiPhatHanh != TrangThaiQuyTrinh.TatCa)
            {
                query = query.Where(x => x.TrangThaiPhatHanhDieuChinh.HasValue && (TrangThaiQuyTrinh)x.TrangThaiPhatHanhDieuChinh == @params.LoaiTrangThaiPhatHanh);
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
                    query = query.Where(x => x.SoHoaDonBiDieuChinh.ToString().ToTrim().Contains(keyword) || x.SoHoaDonDieuChinh.ToString().ToTrim().Contains(keyword));
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

        public async Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonDieuChinhAsync_New(HoaDonDieuChinhParams @params)
        {
            try
            {
                string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

                DateTime fromDate = DateTime.Parse(@params.FromDate);
                DateTime toDate = DateTime.Parse(@params.ToDate);

                //đọc ra trước các hóa đơn để lấy ra hóa đơn thay thế, hóa đơn điều chỉnh tại mỗi dòng hóa đơn đang duyệt
                //mục đích thêm code này để hiển thị cột thông báo sai sót theo yêu của a Kiên
                //cột này hiển thị ở cả 4 tab hóa đơn
                //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
                List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                             select new HoaDonDienTu
                                                             {
                                                                 HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                                 SoHoaDon = hoaDon.SoHoaDon,
                                                                 ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                                 DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                                 NgayHoaDon = hoaDon.NgayHoaDon,
                                                                 TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                                 MaCuaCQT = hoaDon.MaCuaCQT,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 CreatedDate = hoaDon.CreatedDate
                                                             }).ToListAsync();

                //đọc ra thông tin hóa đơn được nhập từ phần mềm khác, (được dùng để hiển thị cột thông tin sai sót ở hóa đơn điều chỉnh); việc đọc ra bảng này vì phải truy vấn thông tin với các hóa đơn được nhập từ phần mềm khác
                List<ThongTinHoaDon> listThongTinHoaDon = await (from hoaDon in _db.ThongTinHoaDons
                                                                 where listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.Id) > 0
                                                                 select new ThongTinHoaDon
                                                                 {
                                                                     Id = hoaDon.Id,
                                                                     TrangThaiHoaDon = hoaDon.TrangThaiHoaDon,
                                                                     IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                     LanGui04 = hoaDon.LanGui04,
                                                                     ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                     TrangThaiGui04 = hoaDon.TrangThaiGui04
                                                                 }).ToListAsync();

                //đọc ra kỳ kế toán hiện tại
                //mục đích đọc ra là để hiển thị tình trạng quá hạn/trong hạn của mỗi hóa đơn theo yêu cầu của a Kiên
                var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

                var queryBB = _db.BienBanDieuChinhs.ToList();
                var query = from hd in _db.HoaDonDienTus
                            join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                            join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                            join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpBienBanDieuChinhs
                            from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                            join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                            from lt in tmpLoaiTiens.DefaultIfEmpty()
                            where ((TrangThaiHoaDon)hd.TrangThai) == TrangThaiHoaDon.HoaDonGoc && (_db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId) || bbdc != null)
                            && hd.NgayHoaDon.Value.Date >= fromDate && hd.NgayHoaDon.Value.Date <= toDate
                            && @params.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                            orderby hd.NgayHoaDon, hd.SoHoaDon descending
                            select new HoaDonDienTuViewModel
                            {
                                ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, listThongTinHoaDon.FirstOrDefault(x => x.Id == hd.DieuChinhChoHoaDonId)),
                                ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                Key = Guid.NewGuid().ToString(),
                                DaDieuChinh = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId),
                                DaBiDieuChinh = (from tt in _db.HoaDonDienTus
                                                 join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                 where tt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                 && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && tt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                 || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && tt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu && hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DangKyDienTu && hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KyDienTuLoi))
                                                 select tt.HoaDonDienTuId).Any(),
                                IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan ?? false,
                                HoaDonDienTuId = hd.HoaDonDienTuId,
                                LoaiApDungHoaDonDieuChinh = hd.LoaiApDungHoaDonDieuChinh.HasValue && hd.LoaiApDungHoaDonDieuChinh != 0 ? hd.LoaiApDungHoaDonDieuChinh : 1,
                                TenHinhThucHoaDonBiDieuChinh = hd.LoaiApDungHoaDonDieuChinh.HasValue && hd.LoaiApDungHoaDonDieuChinh != 0 ? ((LADHDDT)hd.LoaiApDungHoaDonDieuChinh).GetDescription() : LADHDDT.HinhThuc1.GetDescription(),
                                //LyDoDieuChinhModel = (!string.IsNullOrEmpty(hd.LyDoDieuChinh) || bbdc != null) ? JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh) : new LyDoDieuChinhModel { LyDo = bbdc != null ? bbdc.LyDoDieuChinh : string.Empty },
                                BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : string.Empty,
                                TrangThaiBienBanDieuChinh = bbdc != null ? bbdc.TrangThaiBienBan : (int)(LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                                TenTrangThaiBienBanDieuChinh = bbdc != null ? ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription() : LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan.GetDescription(),
                                TrangThai = hd.TrangThai,
                                DieuChinhChoHoaDonId = hd.DieuChinhChoHoaDonId,
                                HoaDonDieuChinhId = bbdc != null ? bbdc.HoaDonDieuChinhId : string.Empty,
                                LoaiDieuChinh = hd.LoaiDieuChinh,
                                TenTrangThaiHoaDon = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId) ? "Hóa đơn đã lập điều chỉnh" : "Hóa đơn chưa lập điều chỉnh",
                                TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                                TenTrangThaiQuyTrinh = hd.TrangThaiQuyTrinh.HasValue ? ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription() : string.Empty,
                                TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                                MaTraCuu = hd.MaTraCuu,
                                LapTuPMGP = true,
                                LoaiHoaDon = hd.LoaiHoaDon,
                                TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                NgayHoaDon = hd.NgayHoaDon,
                                SoHoaDon = hd.SoHoaDon,
                                MaCuaCQT = hd.MaCuaCQT ?? string.Empty,
                                MauSo = bkhhd != null ? bkhhd.KyHieuMauSoHoaDon.ToString() ?? string.Empty : hd.MauSo,
                                KyHieu = bkhhd != null ? bkhhd.KyHieuHoaDon ?? string.Empty : hd.KyHieu,
                                MaKhachHang = hd.MaKhachHang,
                                TenKhachHang = hd.TenKhachHang,
                                MaSoThue = hd.MaSoThue,
                                HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                                TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD,
                                EmailNguoiNhanHD = hd.EmailNguoiNhanHD,
                                SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD,
                                LoaiTienId = hd.LoaiTienId,
                                MaLoaiTien = lt != null ? lt.Ma : "VND",
                                TongTienThanhToan = hd.TongTienThanhToanQuyDoi,
                                NgayLapBienBanDieuChinh = bbdc != null ? bbdc.NgayBienBan : null,
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
                                                       Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                       Status = tldk.Status
                                                   })
                                                   .ToList(),
                                BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    KyHieu = bkhhd.KyHieu,
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                    TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                    UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                    TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                                },
                                CreatedDate = hd.CreatedDate
                            };

                var queryHDCu = from hd in _db.ThongTinHoaDons
                                join bbdc in _db.BienBanDieuChinhs on hd.Id equals bbdc.HoaDonBiDieuChinhId into tmpBienBanDieuChinhs
                                from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                                join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                from lt in tmpLoaiTiens.DefaultIfEmpty()
                                where (_db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.Id) || bbdc != null)
                                && hd.NgayHoaDon.Value.Date >= fromDate && hd.NgayHoaDon.Value.Date <= toDate && ((TrangThaiHoaDon)hd.TrangThaiHoaDon) == TrangThaiHoaDon.HoaDonGoc
                                orderby hd.NgayHoaDon, hd.SoHoaDon descending
                                select new HoaDonDienTuViewModel
                                {
                                    Key = Guid.NewGuid().ToString(),
                                    Loai = "Bị điều chỉnh",
                                    TrangThai = hd.TrangThaiHoaDon.HasValue && hd.TrangThaiHoaDon != 0 ? hd.TrangThaiHoaDon : (int)TrangThaiHoaDon.HoaDonGoc,
                                    DaBiDieuChinh = (from tt in _db.HoaDonDienTus
                                                     join bkh in _db.BoKyHieuHoaDons on tt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                     where tt.DieuChinhChoHoaDonId == hd.Id
                                                     && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && tt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                     || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && tt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu && tt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DangKyDienTu && tt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KyDienTuLoi))
                                                     select tt.HoaDonDienTuId).Any(),
                                    HoaDonDienTuId = hd.Id,
                                    LoaiApDungHoaDonDieuChinh = (int)hd.HinhThucApDung,
                                    HoaDonDieuChinhId = bbdc != null ? bbdc.HoaDonDieuChinhId : string.Empty,
                                    TenHinhThucHoaDonBiDieuChinh = ((LADHDDT)hd.HinhThucApDung).GetDescription(),
                                    BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : string.Empty,
                                    TrangThaiBienBanDieuChinh = bbdc != null ? bbdc.TrangThaiBienBan : (int)(LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                                    TenTrangThaiBienBanDieuChinh = bbdc != null ? ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription() : LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan.GetDescription(),
                                    DaDieuChinh = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.Id),
                                    NgayHoaDon = hd.NgayHoaDon,
                                    StrSoHoaDon = hd.SoHoaDon,
                                    MaCuaCQT = hd.MaCQTCap ?? string.Empty,
                                    LapTuPMGP = false,
                                    MauSo = hd.MauSoHoaDon,
                                    KyHieu = hd.KyHieuHoaDon,
                                    NgayLapBienBanDieuChinh = bbdc != null ? bbdc.NgayBienBan : null,
                                    TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                       where tldk.NghiepVuId == hd.Id
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
                                    LoaiTienId = hd.LoaiTienId,
                                    MaLoaiTien = lt != null ? lt.Ma : "VND",
                                    TongTienThanhToan = hd.ThanhTien,
                                    CreatedDate = hd.CreatedDate
                                };

                var queryDieuChinh = from hd in _db.HoaDonDienTus
                                     join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into boKyHieuHoaDons
                                     from bkhhd in boKyHieuHoaDons.DefaultIfEmpty()
                                     join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                                     from mhd in tmpMauHoaDons.DefaultIfEmpty()
                                     join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonDieuChinhId into tmpBienBanDieuChinhs
                                     from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                                     join hddc in _db.HoaDonDienTus on hd.HoaDonDienTuId equals hddc.DieuChinhChoHoaDonId into tmpHoaDonDieuChinhs
                                     from hddc in tmpHoaDonDieuChinhs.DefaultIfEmpty()
                                     join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                     from lt in tmpLoaiTiens.DefaultIfEmpty()
                                     where ((!string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) && hd.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh)) && hddc == null
                                     select new HoaDonDienTuViewModel
                                     {
                                         ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, listThongTinHoaDon.FirstOrDefault(x => x.Id == hd.DieuChinhChoHoaDonId)),
                                         ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                         Key = Guid.NewGuid().ToString(),
                                         HoaDonDienTuId = hd.HoaDonDienTuId,
                                         DaDieuChinh = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId),
                                         DieuChinhChoHoaDonId = hd.DieuChinhChoHoaDonId,
                                         IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan,
                                         LoaiApDungHoaDonDieuChinh = hd.LoaiApDungHoaDonDieuChinh ?? (int)LADHDDT.HinhThuc1,
                                         TenHinhThucHoaDonBiDieuChinh = hd.LoaiApDungHoaDonDieuChinh.HasValue ? ((LADHDDT)hd.LoaiApDungHoaDonDieuChinh).GetDescription() : LADHDDT.HinhThuc1.GetDescription(),
                                         BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : string.Empty,
                                         LyDoDieuChinh = hd.LyDoDieuChinh,
                                         LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                                         TrangThaiBienBanDieuChinh = bbdc != null ? bbdc.TrangThaiBienBan : (int)(LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                                         TenTrangThaiBienBanDieuChinh = bbdc != null && bbdc.TrangThaiBienBan.HasValue ? ((LoaiTrangThaiBienBanDieuChinhHoaDon)bbdc.TrangThaiBienBan).GetDescription() : LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan.GetDescription(),
                                         TrangThai = hd.TrangThai,
                                         TenTrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
                                         TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                                         TenTrangThaiQuyTrinh = hd.TrangThaiQuyTrinh.HasValue ? ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription() : string.Empty,
                                         TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                                         TenTrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon.HasValue ? ((LoaiTrangThaiGuiHoaDon)hd.TrangThaiGuiHoaDon).GetDescription() : string.Empty,
                                         MaTraCuu = hd.MaTraCuu,
                                         LoaiHoaDon = hd.LoaiHoaDon,
                                         TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                         NgayLapBienBanDieuChinh = bbdc != null ? bbdc.NgayBienBan : null,
                                         LoaiDieuChinh = hd.LoaiDieuChinh,
                                         TenLoaiDieuChinh = ((LoaiDieuChinhHoaDon)hd.LoaiDieuChinh).GetDescription(),
                                         NgayHoaDon = hd.NgayHoaDon,
                                         SoHoaDon = hd.SoHoaDon,
                                         MaCuaCQT = hd.MaCuaCQT,
                                         MauSo = bkhhd != null ? bkhhd.KyHieuMauSoHoaDon.ToString() ?? string.Empty : hd.MauSo,
                                         KyHieu = bkhhd != null ? bkhhd.KyHieuHoaDon ?? string.Empty : hd.KyHieu,
                                         MaKhachHang = hd.MaKhachHang,
                                         TenKhachHang = hd.TenKhachHang,
                                         MaSoThue = hd.MaSoThue,
                                         HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                                         TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                         HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD,
                                         EmailNguoiNhanHD = hd.EmailNguoiNhanHD,
                                         SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD,
                                         LoaiTienId = hd.LoaiTienId,
                                         MaLoaiTien = lt != null ? lt.Ma : "VND",
                                         TongTienThanhToan = hd.TongTienThanhToanQuyDoi,
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
                                                                Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                                Status = tldk.Status
                                                            })
                                                   .ToList(),
                                         CreatedDate = hd.CreatedDate,
                                         BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                                         {
                                             BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                             KyHieu = bkhhd.KyHieu,
                                             KyHieu1 = bkhhd.KyHieu1,
                                             KyHieu56 = bkhhd.KyHieu56,
                                             MauHoaDonId = bkhhd.MauHoaDonId,
                                             HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                             TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                             UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                             TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                                         },
                                     };



                var listBiDieuChinh = await query.ToListAsync();
                var listBiDieuChinhCu = await queryHDCu.ToListAsync();
                var listDieuChinh = await queryDieuChinh.ToListAsync();
                var listHoaDonBDC = listBiDieuChinh.Union(listBiDieuChinhCu);

                //loại các hóa đơn bị điều chỉnh xuất hiện 2 lần
                var idsTrung = listHoaDonBDC.Where(x => listHoaDonBDC.Count(o => o.HoaDonDienTuId == x.HoaDonDienTuId) > 1).Select(x => x.HoaDonDienTuId).Distinct().ToList();
                listHoaDonBDC = listHoaDonBDC.Where(x => (idsTrung.Contains(x.HoaDonDienTuId) && (string.IsNullOrEmpty(x.HoaDonDieuChinhId) || listHoaDonBDC.Where(o => o.HoaDonDienTuId == x.HoaDonDienTuId).All(o => !string.IsNullOrEmpty(o.HoaDonDieuChinhId))) || !idsTrung.Contains(x.HoaDonDienTuId))).DistinctBy(x => x.HoaDonDienTuId).ToList();

                foreach (var item in listHoaDonBDC)
                {
                    if (listDieuChinh.Any(x => x.DieuChinhChoHoaDonId == item.HoaDonDienTuId))
                    {
                        item.Children = new List<HoaDonDienTuViewModel>();

                        var hoaDonDieuChinhs = listDieuChinh.Where(x => x.DieuChinhChoHoaDonId == item.HoaDonDienTuId).ToList();
                        Queue<HoaDonDienTuViewModel> queue = new Queue<HoaDonDienTuViewModel>(hoaDonDieuChinhs);
                        while (queue.Count() != 0)
                        {
                            var dequeue = queue.Dequeue();
                            dequeue.DaLapDieuChinh = true;
                            item.Children.Insert(0, dequeue);
                            //if (!string.IsNullOrEmpty(dequeue.DieuChinhChoHoaDonId))
                            //{
                            //    var a = 1;
                            //}
                            //if (listHoaDonBDC.Any(x => x.HoaDonDienTuId == dequeue.DieuChinhChoHoaDonId))
                            //{
                            //    var hoaDonDieuChinhInQueues = listHoaDonBDC.Where(x => x.HoaDonDienTuId == dequeue.DieuChinhChoHoaDonId).ToList();
                            //    foreach (var child in hoaDonDieuChinhInQueues)
                            //    {
                            //        queue.Enqueue(child);
                            //    }
                            //}
                        }

                        var idx = 0;
                        for (idx = 0; idx < item.Children.Count; idx++)
                        {
                            if (idx == 0)
                            {
                                var bbdc = _db.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == item.Children[idx].BienBanDieuChinhId);
                                if (bbdc != null && bbdc.HoaDonDieuChinhId == item.Children[idx].HoaDonDienTuId)
                                {
                                    item.Children[idx].TrangThaiBienBanDieuChinhTmp = item.Children[idx].TrangThaiBienBanDieuChinh;
                                    item.Children[idx].BienBanDieuChinhIdTmp = item.Children[idx].BienBanDieuChinhId;
                                    item.Children[idx].LyDoDieuChinhModelTmp = new LyDoDieuChinhModel { LyDo = bbdc.LyDoDieuChinh };
                                    item.Children[idx].TenTrangThaiBienBanDieuChinhTmp = item.Children[idx].TenTrangThaiBienBanDieuChinh;
                                    item.Children[idx].NgayLapBienBanDieuChinhTmp = item.Children[idx].NgayLapBienBanDieuChinh;
                                }
                                else
                                {
                                    item.Children[idx].TrangThaiBienBanDieuChinhTmp = item.Children[idx].TrangThaiBienBanDieuChinh;
                                    item.Children[idx].BienBanDieuChinhIdTmp = item.Children[idx].BienBanDieuChinhId;
                                    item.Children[idx].LyDoDieuChinhModelTmp = item.Children[idx].LyDoDieuChinhModel;
                                    item.Children[idx].TenTrangThaiBienBanDieuChinhTmp = item.Children[idx].TenTrangThaiBienBanDieuChinh;
                                    item.Children[idx].NgayLapBienBanDieuChinhTmp = item.Children[idx].NgayLapBienBanDieuChinh;
                                }
                            }
                            else
                            {
                                var bbdc = _db.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == item.Children[idx].BienBanDieuChinhId);
                                if (bbdc != null && bbdc.HoaDonDieuChinhId == item.Children[idx].HoaDonDienTuId)
                                {
                                    item.Children[idx].TrangThaiBienBanDieuChinhTmp = item.Children[idx].TrangThaiBienBanDieuChinh;
                                    item.Children[idx].BienBanDieuChinhIdTmp = item.Children[idx].BienBanDieuChinhId;
                                    item.Children[idx].LyDoDieuChinhModelTmp = new LyDoDieuChinhModel { LyDo = bbdc.LyDoDieuChinh };
                                    item.Children[idx].TenTrangThaiBienBanDieuChinhTmp = item.Children[idx].TenTrangThaiBienBanDieuChinh;
                                    item.Children[idx].NgayLapBienBanDieuChinhTmp = item.Children[idx].NgayLapBienBanDieuChinh;
                                }
                                else
                                {
                                    item.Children[idx].TrangThaiBienBanDieuChinhTmp = item.Children[idx].TrangThaiBienBanDieuChinh;
                                    item.Children[idx].BienBanDieuChinhIdTmp = item.Children[idx].BienBanDieuChinhId;
                                    item.Children[idx].LyDoDieuChinhModelTmp = item.Children[idx].LyDoDieuChinhModel;
                                    item.Children[idx].TenTrangThaiBienBanDieuChinhTmp = item.Children[idx].TenTrangThaiBienBanDieuChinh;
                                    item.Children[idx].NgayLapBienBanDieuChinhTmp = item.Children[idx].NgayLapBienBanDieuChinh;
                                }
                            }
                        }

                        item.Children = item.Children.OrderBy(x => x.CreatedDate).ToList();

                        if (idx == item.Children.Count)
                        {
                            //var bbdc = _db.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == item.Children[idx - 1].BienBanDieuChinhId);
                            //if (bbdc != null && string.IsNullOrEmpty(bbdc.HoaDonDieuChinhId))
                            //{
                            //    item.Children.Add(new HoaDonDienTuViewModel
                            //    {
                            //        ThongBaoSaiSot = null,
                            //        TaiLieuDinhKems = new List<TaiLieuDinhKemViewModel>(),
                            //        DaDieuChinh = false,
                            //        TenTrangThaiBienBanDieuChinhTmp = item.Children[idx - 1].TenTrangThaiBienBanDieuChinh,
                            //        BienBanDieuChinhIdTmp = item.Children[idx - 1].BienBanDieuChinhId,
                            //        LyDoDieuChinhModelTmp = new LyDoDieuChinhModel { LyDo = bbdc.LyDoDieuChinh },
                            //        TrangThaiBienBanDieuChinhTmp = item.Children[idx - 1].TrangThaiBienBanDieuChinh
                            //    });
                            //}

                            var bbdcChuaCoHoaDons = _db.BienBanDieuChinhs.Where(x => x.BienBanDieuChinhId != item.Children[idx - 1].BienBanDieuChinhId && x.HoaDonBiDieuChinhId == item.HoaDonDienTuId && string.IsNullOrEmpty(x.HoaDonDieuChinhId)).ToList();
                            if (bbdcChuaCoHoaDons.Any())
                            {
                                foreach (var it in bbdcChuaCoHoaDons)
                                    item.Children.Add(new HoaDonDienTuViewModel
                                    {
                                        Key = Guid.NewGuid().ToString(),
                                        ThongBaoSaiSot = null,
                                        DaDieuChinh = false,
                                        TenTrangThaiBienBanDieuChinhTmp = ((LoaiTrangThaiBienBanDieuChinhHoaDon)it.TrangThaiBienBan).GetDescription(),
                                        DieuChinhChoHoaDonId = item.HoaDonDienTuId,
                                        BienBanDieuChinhIdTmp = it.BienBanDieuChinhId,
                                        LyDoDieuChinhModelTmp = new LyDoDieuChinhModel { LyDo = it.LyDoDieuChinh },
                                        TrangThaiBienBanDieuChinhTmp = it.TrangThaiBienBan,
                                        NgayLapBienBanDieuChinhTmp = it.NgayBienBan,
                                        TenKhachHang = it.TenDonViBenB,
                                        HoTenNguoiMuaHang = it.DaiDienBenB,
                                        MaSoThue = it.MaSoThueBenB
                                    });
                            }
                        }

                        item.TenTrangThaiBienBanDieuChinhTmp = string.Empty;
                        item.LyDoDieuChinhModelTmp = null;
                    }
                    else if (!string.IsNullOrEmpty(item.BienBanDieuChinhId))
                    {
                        var bbdc = _db.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == item.BienBanDieuChinhId);
                        item.Children = new List<HoaDonDienTuViewModel>
                        {
                            new HoaDonDienTuViewModel
                            {
                                Key = Guid.NewGuid().ToString(),
                                DaDieuChinh = false,
                                BienBanDieuChinhIdTmp = item.BienBanDieuChinhId,
                                DieuChinhChoHoaDonId = item.HoaDonDienTuId,
                                TenTrangThaiBienBanDieuChinhTmp = item.TenTrangThaiBienBanDieuChinh,
                                LyDoDieuChinhModelTmp = new LyDoDieuChinhModel(){ LyDo = bbdc.LyDoDieuChinh },
                                TrangThaiBienBanDieuChinhTmp = item.TrangThaiBienBanDieuChinh,
                                NgayLapBienBanDieuChinhTmp = item.NgayLapBienBanDieuChinh,
                                TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                   where tldk.NghiepVuId == item.BienBanDieuChinhId
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
                                    TenKhachHang = bbdc.TenDonViBenB,
                                    HoTenNguoiMuaHang = bbdc.DaiDienBenB,
                                    MaSoThue = bbdc.MaSoThueBenB
                            }
                        };

                        var bbdcChuaCoHoaDons = _db.BienBanDieuChinhs.Where(x => x.BienBanDieuChinhId != item.BienBanDieuChinhId && x.HoaDonBiDieuChinhId == item.HoaDonDienTuId && string.IsNullOrEmpty(x.HoaDonDieuChinhId)).ToList();
                        if (bbdcChuaCoHoaDons.Any())
                        {
                            foreach (var it in bbdcChuaCoHoaDons)
                                item.Children.Add(new HoaDonDienTuViewModel
                                {
                                    Key = Guid.NewGuid().ToString(),
                                    ThongBaoSaiSot = null,
                                    TaiLieuDinhKems = new List<TaiLieuDinhKemViewModel>(),
                                    DaDieuChinh = false,
                                    TenTrangThaiBienBanDieuChinhTmp = ((LoaiTrangThaiBienBanDieuChinhHoaDon)it.TrangThaiBienBan).GetDescription(),
                                    DieuChinhChoHoaDonId = item.HoaDonDienTuId,
                                    BienBanDieuChinhIdTmp = it.BienBanDieuChinhId,
                                    LyDoDieuChinhModelTmp = new LyDoDieuChinhModel { LyDo = it.LyDoDieuChinh },
                                    TrangThaiBienBanDieuChinhTmp = it.TrangThaiBienBan,
                                    NgayLapBienBanDieuChinhTmp = it.NgayBienBan,
                                    TenKhachHang = it.TenDonViBenB,
                                    HoTenNguoiMuaHang = it.DaiDienBenB,
                                    MaSoThue = it.MaSoThueBenB
                                });
                        }

                        item.TenTrangThaiBienBanDieuChinhTmp = string.Empty;
                        item.LyDoDieuChinhModelTmp = null;
                    }
                }

                if (@params.LoaiTrangThaiHoaDonDieuChinh != LoaiTrangThaiHoaDonDieuChinh.TatCa)
                {
                    if (@params.LoaiTrangThaiHoaDonDieuChinh == LoaiTrangThaiHoaDonDieuChinh.ChuaLap)
                    {
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => string.IsNullOrEmpty(o.HoaDonDienTuId)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => string.IsNullOrEmpty(x.HoaDonDienTuId)).ToList();
                        }
                    }
                    else if (@params.LoaiTrangThaiHoaDonDieuChinh == LoaiTrangThaiHoaDonDieuChinh.DaLap)
                    {
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => !string.IsNullOrEmpty(o.HoaDonDienTuId)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => !string.IsNullOrEmpty(x.HoaDonDienTuId)).ToList();
                        }
                    }
                    else
                    {
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.LoaiDieuChinh == (int)@params.LoaiTrangThaiHoaDonDieuChinh));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.LoaiDieuChinh == (int)@params.LoaiTrangThaiHoaDonDieuChinh).ToList();
                        }
                    }
                }

                if (@params.LoaiTrangThaiPhatHanh != TrangThaiQuyTrinh.TatCa)
                {
                    listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.TrangThaiQuyTrinh == (int)@params.LoaiTrangThaiPhatHanh));
                    foreach (var item in listHoaDonBDC)
                    {
                        item.Children = item.Children.Where(x => x.TrangThaiQuyTrinh == (int)@params.LoaiTrangThaiPhatHanh).ToList();
                    }
                }

                if (@params.LoaiTrangThaiBienBanDieuChinhHoaDon != LoaiTrangThaiBienBanDieuChinhHoaDon.TatCa)
                {
                    listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.TrangThaiBienBanDieuChinhTmp == (int)@params.LoaiTrangThaiBienBanDieuChinhHoaDon));
                    foreach (var item in listHoaDonBDC)
                    {
                        item.Children = item.Children.Where(x => x.TrangThaiBienBanDieuChinhTmp == (int)@params.LoaiTrangThaiBienBanDieuChinhHoaDon).ToList();
                    }
                }

                if ((int)@params.TrangThaiGuiHoaDon != -1)
                {
                    listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.TrangThaiGuiHoaDon == (int)@params.TrangThaiGuiHoaDon));
                    foreach (var item in listHoaDonBDC)
                    {
                        item.Children = item.Children.Where(x => x.TrangThaiGuiHoaDon == (int)@params.TrangThaiGuiHoaDon).ToList();
                    }
                }

                if (@params.TimKiemTheo != null)
                {
                    var timKiemTheo = @params.TimKiemTheo;
                    if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                    {
                        var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                    {
                        var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.MauSo.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.MauSo.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                    {
                        var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.KyHieu.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.KyHieu.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                    {
                        var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                    {
                        var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.MaSoThue.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                    {
                        var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.MaKhachHang.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.MaKhachHang.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                    {
                        var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.TenKhachHang.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.TenKhachHang.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                    {
                        var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                        listHoaDonBDC = listHoaDonBDC.Where(x => x.Children.Any(o => o.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword)));
                        foreach (var item in listHoaDonBDC)
                        {
                            item.Children = item.Children.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword)).ToList();
                        }
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
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.SoHoaDon, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MauSo):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.MauSo, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.KyHieu):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.KyHieu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaKhachHang):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.MaKhachHang, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.TenKhachHang):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.TenKhachHang, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.DiaChi):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.DiaChi, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaSoThue):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.MaSoThue, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.HoTenNguoiMuaHang):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.HoTenNguoiMuaHang, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.TenNhanVienBanHang):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.TenNhanVienBanHang, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.TongTienThanhToan):
                                listHoaDonBDC = GenericFilterColumn<HoaDonDienTuViewModel>.Query(listHoaDonBDC, x => x.TongTienThanhToan, filterCol, FilterValueType.Decimal);
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
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenTrangThaiHoaDon);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenTrangThaiHoaDon);
                            }
                            break;
                        case nameof(@params.Filter.TenHinhThucHoaDonBiDieuChinh):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenHinhThucHoaDonBiDieuChinh);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenHinhThucHoaDonBiDieuChinh);
                            }
                            break;
                        case nameof(@params.Filter.LyDoDieuChinh):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.LyDoDieuChinh);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.LyDoDieuChinh);
                            }
                            break;
                        case nameof(@params.Filter.TenTrangThaiBienBanDieuChinh):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenTrangThaiBienBanDieuChinh);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenTrangThaiBienBanDieuChinh);
                            }
                            break;
                        case nameof(@params.Filter.TenTrangThaiPhatHanh):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenTrangThaiPhatHanh);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenTrangThaiPhatHanh);
                            }
                            break;
                        case nameof(@params.Filter.MaTraCuu):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.MaTraCuu);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.MaTraCuu);
                            }
                            break;
                        case nameof(@params.Filter.TenLoaiHoaDon):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenLoaiHoaDon);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenLoaiHoaDon);
                            }
                            break;
                        case nameof(@params.Filter.NgayHoaDon):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.NgayHoaDon);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.NgayHoaDon);
                            }
                            break;
                        case nameof(@params.Filter.SoHoaDon):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.SoHoaDon);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.SoHoaDon);
                            }
                            break;
                        case nameof(@params.Filter.MauSo):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.MauSo);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.MauSo);
                            }
                            break;
                        case nameof(@params.Filter.KyHieu):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.KyHieu);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.KyHieu);
                            }
                            break;
                        case nameof(@params.Filter.MaKhachHang):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.MaKhachHang);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.MaKhachHang);
                            }
                            break;
                        case nameof(@params.Filter.TenKhachHang):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenKhachHang);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenKhachHang);
                            }
                            break;
                        case nameof(@params.Filter.MaSoThue):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.MaSoThue);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.MaSoThue);
                            }
                            break;
                        case nameof(@params.Filter.HoTenNguoiMuaHang):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.HoTenNguoiMuaHang);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.HoTenNguoiMuaHang);
                            }
                            break;
                        case nameof(@params.Filter.TenNhanVienBanHang):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TenNhanVienBanHang);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TenNhanVienBanHang);
                            }
                            break;
                        case nameof(@params.Filter.MaLoaiTien):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.MaLoaiTien);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.MaLoaiTien);
                            }
                            break;
                        case nameof(@params.Filter.TongTienThanhToan):
                            if (@params.SortValue == "ascend")
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderBy(x => x.TongTienThanhToan);
                            }
                            else
                            {
                                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.TongTienThanhToan);
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                foreach (var item in listHoaDonBDC)
                {
                    item.Children = item.Children.OrderBy(x => x.NgayHoaDon.HasValue ? x.NgayHoaDon : x.NgayLapBienBanDieuChinhTmp)
                                                    .ThenBy(x => x.MauSo)
                                                    .ThenBy(x => x.BoKyHieuHoaDon != null ? x.BoKyHieuHoaDon.KyHieu1 : null)
                                                    .ThenBy(x => x.BoKyHieuHoaDon != null ? x.BoKyHieuHoaDon.KyHieu56 : null)
                                                    .ThenBy(x => x.SoHoaDon).ToList();
                }

                listHoaDonBDC = listHoaDonBDC.OrderByDescending(x => x.Children[x.Children.Count - 1].NgayHoaDon.HasValue ? x.Children[x.Children.Count - 1].NgayHoaDon : x.Children[x.Children.Count - 1].NgayLapBienBanDieuChinhTmp)
                                                .ThenByDescending(x => x.Children[x.Children.Count - 1].MauSo)
                                                .ThenByDescending(x => x.Children[x.Children.Count - 1].BoKyHieuHoaDon != null ? x.Children[x.Children.Count - 1].BoKyHieuHoaDon.KyHieu1 : null)
                                                .ThenByDescending(x => x.Children[x.Children.Count - 1].BoKyHieuHoaDon != null ? x.Children[x.Children.Count - 1].BoKyHieuHoaDon.KyHieu56 : null)
                                                .ThenByDescending(x => x.Children[x.Children.Count - 1].SoHoaDon);

                var res = listHoaDonBDC.ToList();
                return PagedList<HoaDonDienTuViewModel>
                        .CreateAsyncWithList(listHoaDonBDC, @params.PageNumber, @params.PageSize);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }
        }

        public List<EnumModel> GetLoaiTrangThaiBienBanDieuChinhHoaDons()
        {
            List<EnumModel> enums = ((LoaiTrangThaiBienBanDieuChinhHoaDon[])Enum.GetValues(typeof(LoaiTrangThaiBienBanDieuChinhHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).OrderBy(x => x.Value).ToList();
            return enums;
        }

        public List<TrangThaiHoaDonDieuChinh> GetTrangThaiHoaDonDieuChinhs()
        {
            return new List<TrangThaiHoaDonDieuChinh>
            {
                new TrangThaiHoaDonDieuChinh { Key = -1, Name = "Tất cả", ParentId = null, Level = 0 },
                new TrangThaiHoaDonDieuChinh { Key = 0, Name = "Hóa đơn chưa lập điều chỉnh", ParentId = null, Level = 0 },
                new TrangThaiHoaDonDieuChinh { Key = -2, Name = "Hóa đơn đã lập điều chỉnh", ParentId = null, IsParent = true, Level = 0 },
                new TrangThaiHoaDonDieuChinh { Key = 1, Name = "Hóa đơn điều chỉnh tăng", ParentId = -2, Level = 1 },
                new TrangThaiHoaDonDieuChinh { Key = 2, Name = "Hóa đơn điều chỉnh giảm", ParentId = -2, Level = 1 },
                new TrangThaiHoaDonDieuChinh { Key = 3, Name = "Hóa đơn điều chỉnh thông tin", ParentId = -2, Level = 1 },
            };
        }

        /// <summary>
        /// GetListHoaDonXoaBoCanThayTheAsync trả về danh sách các hóa đơn xóa bỏ cần thay thế
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonXoaBoCanThayTheAsync(HoaDonThayTheParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var listTatCaHoaDon = await _db.HoaDonDienTus.ToListAsync();
            var listBoKyHieuHoaDon = await _db.BoKyHieuHoaDons.ToListAsync();

            var query = from hddt in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bkhhd in listBoKyHieuHoaDon on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        where hddt.NgayHoaDon.Value.Date >= fromDate && hddt.NgayHoaDon.Value.Date <= toDate
                        //&& (hddt.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2 || hddt.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5)

                        && string.IsNullOrWhiteSpace(hddt.DieuChinhChoHoaDonId) && listTatCaHoaDon.Count(x => x.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId) == 0
                        && (hddt.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2 || hddt.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5 || hddt.TrangThai == 1 || (hddt.TrangThai == 3 && hddt.TrangThaiGuiHoaDon > 2))
                        //nếu HĐ có mã CQT thì lấy HĐ đã cấp số
                        //nếu HĐ KHÔNG có mã CQT thì trạng thái quy trình không phải là <Chưa ký điện tử>; <Đang Ký điện tử>, <Ký điện tử lỗi:>
                        && ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa && hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                            || (bkhhd.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && hddt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu && hddt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DangKyDienTu && hddt.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KyDienTuLoi))

                        //Lấy hóa đơn đã gửi khách hàng thì mới cho lập thay thế
                        && hddt.TrangThaiGuiHoaDon > 2
                        //không cho chọn lại hóa đơn nếu đã tồn tại hóa đơn thay thế không bị lỗi cấp mã
                        && ((listTatCaHoaDon.Where(x => x.ThayTheChoHoaDonId == hddt.HoaDonDienTuId).OrderByDescending(y => y.CreatedDate).Take(1).Where(z => (TrangThaiQuyTrinh)z.TrangThaiQuyTrinh == TrangThaiQuyTrinh.GuiLoi || (TrangThaiQuyTrinh)z.TrangThaiQuyTrinh == TrangThaiQuyTrinh.KhongDuDieuKienCapMa).Count() > 0)
                        || listTatCaHoaDon.Count(x => x.ThayTheChoHoaDonId == hddt.HoaDonDienTuId) == 0)

                        //đồng thời hóa đơn thay thế ko được phép phát hành lại nữa
                        && KiemTraHoaDonThayTheKhongDuocPhatHanhLai(hddt.HoaDonDienTuId, listBoKyHieuHoaDon, listTatCaHoaDon)

                        && @params.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                        orderby hddt.NgayHoaDon descending, hddt.SoHoaDon descending
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            BackUpTrangThai = hddt.BackUpTrangThai,
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            HinhThucXoabo = hddt.HinhThucXoabo,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            NgayHoaDon = hddt.NgayHoaDon,
                            SoHoaDon = hddt.SoHoaDon,
                            NgayXoaBo = hddt.NgayXoaBo,
                            MaCuaCQT = (bkhhd != null) ? ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hddt.MaCuaCQT ?? "<Chưa cấp mã>") : "") : "",
                            MauSo = (bkhhd != null) ? bkhhd.KyHieuMauSoHoaDon.ToString() : "",
                            KyHieu = (bkhhd != null) ? (bkhhd.KyHieuHoaDon ?? "") : "",
                            KhachHangId = hddt.KhachHangId,
                            MaKhachHang = hddt.MaKhachHang ?? string.Empty,
                            TenKhachHang = hddt.TenKhachHang ?? string.Empty,
                            DiaChi = hddt.DiaChi ?? string.Empty,
                            MaSoThue = hddt.MaSoThue ?? string.Empty,
                            HoTenNguoiMuaHang = hddt.HoTenNguoiMuaHang ?? string.Empty,
                            LoaiTienId = hddt.LoaiTienId,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            TongTienThanhToanQuyDoi = hddt.TongTienThanhToanQuyDoi,
                            TenUyNhiemLapHoaDon = (bkhhd != null) ? bkhhd.UyNhiemLapHoaDon.GetDescription() : "",
                            IsLapVanBanThoaThuan = hddt.IsLapVanBanThoaThuan,
                            LyDoXoaBo = hddt.LyDoXoaBo,
                            LyDoThayThe = hddt.LyDoThayThe
                        };

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.LoaiHoaDon))
                {
                    var keyword = timKiemTheo.LoaiHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenLoaiHoaDon != null && x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon != null && x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                {
                    var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue != null && x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaKhachHang))
                {
                    var keyword = timKiemTheo.MaKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.MaKhachHang != null && x.MaKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenKhachHang))
                {
                    var keyword = timKiemTheo.TenKhachHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenKhachHang != null && x.TenKhachHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiMuaHang))
                {
                    var keyword = timKiemTheo.NguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang != null && x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
            }
            else
            {
                //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                if (string.IsNullOrWhiteSpace(@params.TimKiemBatKy) == false)
                {
                    @params.TimKiemBatKy = @params.TimKiemBatKy.ToUpper().ToTrim();
                    query = query.Where(x =>
                        (x.TenLoaiHoaDon != null && x.TenLoaiHoaDon.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.SoHoaDon != null && x.SoHoaDon.ToString().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.MaSoThue != null && x.MaSoThue.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.MaKhachHang != null && x.MaKhachHang.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.TenKhachHang != null && x.TenKhachHang.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.HoTenNguoiMuaHang != null && x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(@params.TimKiemBatKy))
                    );
                }
            }
            return query.ToList();
        }

        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonCanDieuChinhAsync(HoaDonDieuChinhParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var listHoaDonDaLapThayTheIds = await _db.HoaDonDienTus.Where(x => _db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId))
                                                                    .Select(x => x.HoaDonDienTuId).Distinct().ToListAsync();
            var listHoaDonDaLapBBDCs = await _db.BienBanDieuChinhs.Select(x => x.HoaDonBiDieuChinhId).Distinct().ToListAsync();

            var query = from hddt in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bkhhd in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieuHoaDons
                        from bkhhd in tmpBoKyHieuHoaDons.DefaultIfEmpty()
                            //join bbdc in _db.BienBanDieuChinhs on hddt.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpHoaDonBiDieuChinhs
                            //from bbdc in tmpHoaDonBiDieuChinhs.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where hddt.NgayHoaDon.Value.Date >= fromDate && hddt.NgayHoaDon <= toDate && ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && ((TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.DaKyDienTu || (TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.HoaDonHopLe)) || (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa && (TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.CQTDaCapMa)) &&
                        (((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonGoc) && ((TrangThaiGuiHoaDon)hddt.TrangThaiGuiHoaDon >= TrangThaiGuiHoaDon.DaGui || _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId) || listHoaDonDaLapBBDCs.Contains(hddt.HoaDonDienTuId)))
                        && (hddt.TrangThaiBienBanXoaBo == (int)TrangThaiBienBanXoaBo.ChuaLap)
                        && !listHoaDonDaLapThayTheIds.Contains(hddt.HoaDonDienTuId)
                        && @params.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            DaBiDieuChinh = (from hd in _db.HoaDonDienTus
                                             join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                             where hd.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId
                                             && ((bkh.HinhThucHoaDon == HinhThucHoaDon.CoMa && hd.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                             || (bkh.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu && hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DangKyDienTu && hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KyDienTuLoi))
                                             select hd.HoaDonDienTuId).Any(),
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            IsLapVanBanThoaThuan = hddt.IsLapVanBanThoaThuan,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            MauSo = bkhhd.KyHieuMauSoHoaDon.ToString() ?? string.Empty,
                            KyHieu = bkhhd.KyHieuHoaDon,
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
                            //BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : null,
                            //LyDoDieuChinh = bbdc != null ? bbdc.LyDoDieuChinh : null,
                            TongTienThanhToanQuyDoi = hddt.TongTienThanhToanQuyDoi,
                            NgayKy = hddt.NgayKy,
                            IsHoaDonCoMa = bkhhd.KyHieu.IsHoaDonCoMa(),
                            TrangThaiQuyTrinh = hddt.TrangThaiQuyTrinh,
                            TrangThaiLanDieuChinhGanNhat = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId) ? _db.HoaDonDienTus.Where(x => x.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId).OrderByDescending(x => x.CreatedDate).FirstOrDefault().TrangThaiQuyTrinh : (int?)null,
                            MauSoHoaDonLanDieuChinhGanNhat = (from hd in _db.HoaDonDienTus
                                                              join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                              where hd.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId
                                                              orderby hd.CreatedDate descending
                                                              select bkh.KyHieuMauSoHoaDon).FirstOrDefault(),
                            KyHieuHoaDonLanDieuChinhGanNhat = (from hd in _db.HoaDonDienTus
                                                               join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                               where hd.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId
                                                               orderby hd.CreatedDate descending
                                                               select bkh.KyHieuHoaDon).FirstOrDefault(),
                            SoHoaDonLanDieuChinhGanNhat = (from hd in _db.HoaDonDienTus
                                                           where hd.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId
                                                           orderby hd.CreatedDate descending
                                                           select hd.SoHoaDon).FirstOrDefault(),
                            NgayHoaDonLanDieuChinhGanNhat = (from hd in _db.HoaDonDienTus
                                                             where hd.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId
                                                             orderby hd.CreatedDate descending
                                                             select hd.NgayHoaDon).FirstOrDefault(),

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
                    query = query.Where(x => x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword));
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
            List<string> boKyHieuHoaDonIds = result.Select(x => x.BoKyHieuHoaDonId).ToList();

            var hoaDonDienTu_BlockPhatHanhLais = await _db.HoaDonDienTus
                .Where(x => boKyHieuHoaDonIds.Contains(x.BoKyHieuHoaDonId) && x.SoHoaDon.HasValue)
                .Select(x => new HoaDonDienTuViewModel
                {
                    BoKyHieuHoaDonId = x.BoKyHieuHoaDonId,
                    SoHoaDon = x.SoHoaDon
                })
                .ToListAsync();

            foreach (var item in result)
            {
                if (!item.NgayKy.HasValue ||
                item.NgayKy.Value.Date != DateTime.Now.Date ||
                item.IsHoaDonCoMa != true ||
                ((item.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.GuiLoi) && (item.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa)))
                {
                    item.BlockPhatHanhLai = true;
                }
                else
                {
                    if (hoaDonDienTu_BlockPhatHanhLais.Any(x => x.SoHoaDon > item.SoHoaDon && x.BoKyHieuHoaDonId == item.BoKyHieuHoaDonId))
                    {
                        item.BlockPhatHanhLai = true;
                    }
                }

                item.TenTrangThaiLanDieuChinhGanNhat = item.TrangThaiLanDieuChinhGanNhat.HasValue ? ((TrangThaiQuyTrinh)item.TrangThaiLanDieuChinhGanNhat).GetDescription() : string.Empty;
            }

            result = result.Where(x => x.BlockPhatHanhLai == true).ToList();
            return result;
        }

        public async Task<List<HoaDonDienTuViewModel>> GetAllListHoaDonLienQuan(string Id, DateTime ngayTao)
        {
            var query = from hddt in _db.HoaDonDienTus
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _db.BienBanDieuChinhs on hddt.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpHoaDonBiDieuChinhs
                        from bbdc in tmpHoaDonBiDieuChinhs.DefaultIfEmpty()
                        join bbdc_1 in _db.BienBanDieuChinhs on hddt.HoaDonDienTuId equals bbdc_1.HoaDonDieuChinhId into tmpHoaDonDieuChinhs
                        from bbdc_1 in tmpHoaDonDieuChinhs.DefaultIfEmpty()
                        join bkhhd in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieuHoaDons
                        from bkhhd in tmpBoKyHieuHoaDons.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where hddt.DieuChinhChoHoaDonId == Id && hddt.NgayHoaDon < ngayTao
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            BoKyHieuHoaDon = _mp.Map<BoKyHieuHoaDonViewModel>(bkhhd),
                            Loai = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hddt.HoaDonDienTuId) ? "Bị điều chỉnh" : string.Empty,
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            IsLapVanBanThoaThuan = hddt.IsLapVanBanThoaThuan,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hddt.MauHoaDonId,
                            TrangThaiGuiHoaDon = hddt.TrangThaiGuiHoaDon ?? (int)TrangThaiGuiHoaDon.ChuaGui,
                            MauSo = bkhhd.KyHieuMauSoHoaDon.ToString() ?? string.Empty,
                            KyHieu = bkhhd.KyHieuHoaDon,
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
                            TongTienThanhToanQuyDoi = hddt.TongTienThanhToanQuyDoi,
                            TrangThaiBienBanDieuChinh = bbdc_1 != null ? bbdc_1.TrangThaiBienBan : (bbdc != null ? bbdc.TrangThaiBienBan : (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                            BienBanDieuChinhId = bbdc_1 != null ? bbdc_1.BienBanDieuChinhId : (bbdc != null ? bbdc.BienBanDieuChinhId : null),
                            NgayKy = hddt.NgayKy,
                        };

            return await query.ToListAsync();
        }

        public async Task<bool> GetStatusDaThayTheHoaDon(string HoaDonId)
        {
            return await _db.HoaDonDienTus.AnyAsync(x => x.ThayTheChoHoaDonId == HoaDonId);
        }

        public FileReturn XemHoaDonDongLoat(List<string> listPdfFiles)
        {
            string outPutFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(outPutFilePath))
            {
                Directory.CreateDirectory(outPutFilePath);
            }

            for (int i = 0; i < listPdfFiles.Count; i++)
            {
                listPdfFiles[i] = Path.Combine(_hostingEnvironment.WebRootPath, listPdfFiles[i]);
            }

            // File PDF output
            string fileName = $"{Guid.NewGuid()}.pdf";
            string filePath = Path.Combine(outPutFilePath, fileName);

            // Meger file pdf
            bool res = FileHelper.MergePDF(listPdfFiles, filePath);

            //string fileName = $"{Guid.NewGuid()}.pdf";
            //string filePath = Path.Combine(outPutFilePath, fileName);
            //using (var targetDoc = new PdfSharp.Pdf.PdfDocument())
            //{
            //    foreach (var pdf in listPdfFiles)
            //    {
            //        using (var pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
            //        {
            //            for (var i = 0; i < pdfDoc.PageCount; i++)
            //                targetDoc.AddPage(pdfDoc.Pages[i]);
            //        }
            //    }
            //    targetDoc.Save(filePath);
            //}

            byte[] fileByte = File.ReadAllBytes(filePath);
            File.Delete(filePath);

            return new FileReturn
            {
                Bytes = fileByte,
                ContentType = MimeTypes.GetMimeType(filePath),
                FileName = fileName
            };
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
                if (hoaDonDienTuViewModel.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DaKyDienTu && hoaDonDienTuViewModel.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.CQTDaCapMa)
                {
                }
                else
                {
                    string assetsFolder = $"FilesUpload/{databaseName}";

                    filePdfPath = Path.Combine(assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{hoaDonDienTuViewModel.FileDaKy}");
                    filePdfName = hoaDonDienTuViewModel.FileDaKy;

                    fileXMLPath = Path.Combine(assetsFolder, $"{ManageFolderPath.XML_SIGNED}/{hoaDonDienTuViewModel.XMLDaKy}");
                    fileXMLName = hoaDonDienTuViewModel.XMLDaKy;
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

                    worksheet.Cells[idx, 3].Value = it.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGT ? "Hóa đơn GTGT" : "Hóa đơn bán hàng";

                    worksheet.Cells[idx, 4].Value = !string.IsNullOrEmpty(it.MauSo) ? it.MauSo : (it.MauHoaDon != null ? it.MauHoaDon.MauSo : string.Empty);
                    worksheet.Cells[idx, 5].Value = !string.IsNullOrEmpty(it.KyHieu) ? it.KyHieu : (it.MauHoaDon != null ? it.MauHoaDon.KyHieu : string.Empty);
                    worksheet.Cells[idx, 6].Value = it.NgayHoaDon.HasValue ? it.NgayHoaDon.Value.ToString("dd/MM/yyyy") : string.Empty;
                    worksheet.Cells[idx, 7].Value = it.SoHoaDon;

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
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrEmpty(@params.FromDate))
            {
                fromDate = DateTime.Parse(@params.FromDate);
            }
            if (!string.IsNullOrEmpty(@params.ToDate))
            {
                toDate = DateTime.Parse(@params.ToDate);
            }

            var query = from hddt in _db.HoaDonDienTus
                        join tddl in _db.DuLieuGuiHDDTChiTiets on hddt.HoaDonDienTuId equals tddl.HoaDonDienTuId into tmpTDDLs
                        from tddl in tmpTDDLs.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId
                        join mhd in _db.MauHoaDons on hddt.MauHoaDonId equals mhd.MauHoaDonId
                        where
                        (
                            (
                            fromDate != null && toDate != null && string.IsNullOrEmpty(@params.HoaDonDienTuId) &&
                            hddt.NgayHoaDon.Value >= fromDate.Value && hddt.NgayHoaDon.Value <= toDate.Value
                            )
                            ||
                            (
                            fromDate == null && toDate == null && !string.IsNullOrEmpty(@params.HoaDonDienTuId) &&
                            hddt.HoaDonDienTuId == @params.HoaDonDienTuId
                            )
                        )
                        && ((TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.DaKyDienTu) && tddl == null &&
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
            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrEmpty(@params.FromDate))
            {
                fromDate = DateTime.Parse(@params.FromDate);
            }
            if (!string.IsNullOrEmpty(@params.ToDate))
            {
                toDate = DateTime.Parse(@params.ToDate);
            }

            var query = from hddt in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId
                        where
                        (
                            (
                            fromDate != null && toDate != null && string.IsNullOrEmpty(@params.HoaDonDienTuId) &&
                            hddt.NgayHoaDon.Value >= fromDate.Value && hddt.NgayHoaDon.Value <= toDate.Value
                            )
                            ||
                            (
                            fromDate == null && toDate == null && !string.IsNullOrEmpty(@params.HoaDonDienTuId) &&
                            hddt.HoaDonDienTuId == @params.HoaDonDienTuId
                            )
                        )
                        &&
                        (((TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.DaKyDienTu) ||
                        ((TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.GuiLoi) ||
                        ((TrangThaiQuyTrinh)hddt.TrangThaiQuyTrinh == TrangThaiQuyTrinh.KhongDuDieuKienCapMa)) &&
                        bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa &&
                        (((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonGoc) ||
                        ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonThayThe) ||
                        ((TrangThaiHoaDon)hddt.TrangThai == TrangThaiHoaDon.HoaDonDieuChinh))
                        orderby hddt.NgayHoaDon, hddt.SoHoaDon
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hddt.HoaDonDienTuId,
                            TrangThai = hddt.TrangThai,
                            TenTrangThaiHoaDon = hddt.TrangThai.HasValue ? ((TrangThaiHoaDon)hddt.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hddt.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                            BoKyHieuHoaDonId = hddt.BoKyHieuHoaDonId,
                            MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                            KyHieu = bkhhd.KyHieuHoaDon,
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

        public async Task<List<HoaDonDienTuViewModel>> GetDSXoaBoChuaLapThayTheAsync()
        {
            var query = from hd in _db.HoaDonDienTus
                        select new HoaDonDienTuViewModel
                        {
                            DaLapHoaDonThayThe = _db.HoaDonDienTus.Any(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId),
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.NgayLap,
                            SoHoaDon = hd.SoHoaDon,
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
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
                            HinhThucXoabo = hd.HinhThucXoabo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
                        };
            return await query.ToListAsync();
        }
        public async Task<List<HoaDonDienTuViewModel>> GetHoaDonDaLapBbChuaXoaBoAsync()
        {
            var query = from hd in _db.HoaDonDienTus
                        where hd.TrangThaiBienBanXoaBo > 0 && hd.TrangThai != 2
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.NgayLap,
                            SoHoaDon = hd.SoHoaDon,
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
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
                            HinhThucXoabo = hd.HinhThucXoabo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
                        };
            return await query.ToListAsync();
        }
        public async Task<List<HoaDonDienTuViewModel>> GetDSHdDaXoaBo(HoaDonParams pagingParams)
        {
            try
            {
                string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

                //đọc ra trước các hóa đơn để lấy ra hóa đơn thay thế, hóa đơn điều chỉnh tại mỗi dòng hóa đơn đang duyệt
                //mục đích thêm code này để hiển thị cột thông báo sai sót theo yêu của a Kiên
                //cột này hiển thị ở cả 4 tab hóa đơn
                //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
                List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                             select new HoaDonDienTu
                                                             {
                                                                 HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                                 SoHoaDon = hoaDon.SoHoaDon,
                                                                 ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                                 DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                                 NgayHoaDon = hoaDon.NgayHoaDon,
                                                                 TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                                 MaCuaCQT = hoaDon.MaCuaCQT,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 CreatedDate = hoaDon.CreatedDate
                                                             }).ToListAsync();

                //đọc ra kỳ kế toán hiện tại
                //mục đích đọc ra là để hiển thị tình trạng quá hạn/trong hạn của mỗi hóa đơn theo yêu cầu của a Kiên
                var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

                IQueryable<HoaDonDienTuViewModel> query = from hd in _db.HoaDonDienTus
                                                          join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                          join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                                                          from mhd in tmpMauHoaDons.DefaultIfEmpty()
                                                          join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                                                          from kh in tmpKhachHangs.DefaultIfEmpty()
                                                          join bbxb in _db.BienBanXoaBos on hd.HoaDonDienTuId equals bbxb.HoaDonDienTuId into tmpBienBanXoaBos
                                                          from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                                                              //join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                                                              //from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                                                          join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                                                          from nv in tmpNhanViens.DefaultIfEmpty()
                                                          join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                                                          from nl in tmpNguoiLaps.DefaultIfEmpty()
                                                          join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                                          from lt in tmpLoaiTiens.DefaultIfEmpty()
                                                          where (hd.TrangThai == 2 || hd.TrangThaiBienBanXoaBo > 0) && hd.NgayXoaBo != null && pagingParams.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                                                          orderby hd.NgayXoaBo.Value.Date descending, hd.NgayHoaDon.Value.Date descending, bkhhd.UyNhiemLapHoaDon descending, bkhhd.KyHieuMauSoHoaDon descending, bkhhd.KyHieuHoaDon descending, hd.SoHoaDon descending, hd.NgayLap.Value.Date descending
                                                          select new HoaDonDienTuViewModel
                                                          {
                                                              ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, null),
                                                              ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                                              HoaDonDienTuId = hd.HoaDonDienTuId,
                                                              NgayHoaDon = hd.NgayHoaDon,
                                                              BienBanXoaBoId = bbxb != null ? bbxb.Id : null,
                                                              NgayBienBanXoaBo = bbxb != null ? bbxb.NgayBienBan : null,
                                                              NgayLap = hd.NgayLap,
                                                              NguoiLap = nl != null ? new DoiTuongViewModel
                                                              {
                                                                  Ma = nl.Ma,
                                                                  Ten = nl.Ten
                                                              }
                                                                                    : null,
                                                              SoHoaDon = hd.SoHoaDon,
                                                              MauHoaDonId = mhd.MauHoaDonId ?? string.Empty,
                                                              MauSo = bkhhd.KyHieuMauSoHoaDon + string.Empty,
                                                              KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                                                              MaCuaCQT = bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa ? (hd.MaCuaCQT ?? "<Chưa cấp mã>") : string.Empty,
                                                              HinhThucHoaDon = (int)bkhhd.HinhThucHoaDon,
                                                              TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
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
                                                              TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                                              MaKhachHang = hd.MaKhachHang ?? string.Empty,
                                                              TenKhachHang = hd.TenKhachHang ?? string.Empty,
                                                              MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                                                              SoCTXoaBo = hd.SoCTXoaBo,
                                                              DiaChi = hd.DiaChi,
                                                              HinhThucThanhToanId = hd.HinhThucThanhToanId,
                                                              TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
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
                                                              TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
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
                                                              TaiLieuDinhKem = hd.TaiLieuDinhKem,
                                                              CreatedBy = hd.CreatedBy,
                                                              CreatedDate = hd.CreatedDate,
                                                              Status = hd.Status,
                                                              NgayXoaBo = hd.NgayXoaBo,
                                                              TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                                              DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon,
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
                                                              IsNotCreateThayThe = hd.IsNotCreateThayThe,
                                                              HinhThucXoabo = hd.HinhThucXoabo,
                                                              BackUpTrangThai = hd.BackUpTrangThai,
                                                              TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                                                 where tldk.NghiepVuId == (hd != null ? hd.HoaDonDienTuId : null)
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
                                                                                 }).ToList()
                                                          };



                if (!string.IsNullOrEmpty(pagingParams.GiaTri))
                {
                    string keyword = pagingParams.GiaTri.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon.ToString().ToUpper().Contains(keyword) || x.SoHoaDon.ToString().ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                            x.MaKhachHang.ToUpper().Contains(keyword) || x.MaKhachHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                            x.TenKhachHang.ToUpper().Contains(keyword) || x.TenKhachHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                            x.HoTenNguoiMuaHang.ToUpper().Contains(keyword) || x.HoTenNguoiMuaHang.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                            x.MaSoThue.ToUpper().Contains(keyword));
                }

                if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
                {
                    DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
                    DateTime toDate = DateTime.Parse(pagingParams.ToDate);
                    query = query.Where(x => DateTime.Parse(x.NgayXoaBo.Value.ToString("yyyy-MM-dd")) >= fromDate &&
                                            DateTime.Parse(x.NgayXoaBo.Value.ToString("yyyy-MM-dd")) <= toDate);
                }
                if (pagingParams.TrangThaiBienBanXoaBo.HasValue && pagingParams.TrangThaiBienBanXoaBo != -1)
                {
                    query = query.Where(x => x.TrangThaiBienBanXoaBo == pagingParams.TrangThaiBienBanXoaBo);
                }

                if (pagingParams.TrangThaiXoaBo.HasValue && pagingParams.TrangThaiXoaBo != -1)
                {
                    if (pagingParams.TrangThaiXoaBo == 0)//Hóa đơn đã xóa bỏ
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo);
                    }
                    else if (pagingParams.TrangThaiXoaBo == 1)//Hóa đơn xóa bỏ đã lập thay thế
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && _db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId));
                    }
                    else if (pagingParams.TrangThaiXoaBo == 2)
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && !_db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId)
                                             && ((x.IsNotCreateThayThe == false || x.HinhThucXoabo == 2 || x.HinhThucXoabo == 5)));
                    }
                    else if (pagingParams.TrangThaiXoaBo == 3)//Hóa đơn chưa xóa bỏ
                    {
                        query = query.Where(x => (x.TrangThai == 1 || (x.TrangThai == 3 && x.TrangThaiGuiHoaDon > 2)) && x.TrangThaiBienBanXoaBo > 0);// || (x.TrangThai == 4 && (x.TrangThaiGuiHoaDon == 0 || x.TrangThaiGuiHoaDon == 1 || x.TrangThaiGuiHoaDon == 2))
                    }
                    else if (pagingParams.TrangThaiXoaBo == 4)
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.IsNotCreateThayThe == true || (x.HinhThucXoabo != 2 && x.HinhThucXoabo != 5)));
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
                        query = query.Where(x => x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword));
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

                List<HoaDonDienTuViewModel> result = await query.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
                return null;
            }
        }
        public async Task<List<HoaDonDienTuViewModel>> GetDSHoaDonDeXoaBo(HoaDonParams pagingParams)
        {
            try
            {
                string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                List<string> hoaDonBiDieuChinhIds = null;
                List<string> hoaDonDieuChinhIdsDaLapBBDC = null;
                if ((pagingParams.TrangThaiXoaBo == 103) || (pagingParams.TrangThaiXoaBo == 100))//filter data for view HĐ cần xóa bỏ
                {
                    //Kiểm tra xem hóa đơn đã được chọn để lập biên bản điều chỉnh hoặc đã được chọn để lập hóa đơn điều chỉnh ?
                    //  queryLeft là lấy HĐ lập biên bản điều chỉnh bảng BienBanDieuChinhs
                    // lập hóa đơn điều chỉnh 
                    var queryLeft = from hdbdc in _db.HoaDonDienTus
                                    join bbdc in _db.BienBanDieuChinhs on hdbdc.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId
                                    join hddc in _db.HoaDonDienTus on bbdc.HoaDonDieuChinhId equals hddc.HoaDonDienTuId into tmpHoaDonDieuChinhs
                                    from hddc in tmpHoaDonDieuChinhs.DefaultIfEmpty()
                                    select new BangKeHoaDonDieuChinh
                                    {
                                        HoaDonBiDieuChinhId = hdbdc.HoaDonDienTuId
                                    };

                    hoaDonDieuChinhIdsDaLapBBDC = await queryLeft.Where(x => !string.IsNullOrEmpty(x.HoaDonBiDieuChinhId)).Select(x => x.HoaDonBiDieuChinhId).ToListAsync();



                    var queryLeft2 = (from hd in _db.HoaDonDienTus
                                      where hd.DieuChinhChoHoaDonId != ""
                                      select new HoaDonDienTuViewModel
                                      {
                                          DieuChinhChoHoaDonId = hd.DieuChinhChoHoaDonId,
                                      }).Distinct();

                    hoaDonBiDieuChinhIds = await queryLeft2.Where(x => !string.IsNullOrEmpty(x.DieuChinhChoHoaDonId)).Select(x => x.DieuChinhChoHoaDonId).ToListAsync();


                }

                //đọc ra trước các hóa đơn để lấy ra hóa đơn thay thế, hóa đơn điều chỉnh tại mỗi dòng hóa đơn đang duyệt
                //mục đích thêm code này để hiển thị cột thông báo sai sót theo yêu của a Kiên
                //cột này hiển thị ở cả 4 tab hóa đơn
                //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
                List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                             select new HoaDonDienTu
                                                             {
                                                                 HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                                 SoHoaDon = hoaDon.SoHoaDon,
                                                                 ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                                 DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                                 NgayHoaDon = hoaDon.NgayHoaDon,
                                                                 TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                                 MaCuaCQT = hoaDon.MaCuaCQT,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 CreatedDate = hoaDon.CreatedDate
                                                             }).ToListAsync();

                //đọc ra kỳ kế toán hiện tại
                //mục đích đọc ra là để hiển thị tình trạng quá hạn/trong hạn của mỗi hóa đơn theo yêu cầu của a Kiên
                var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

                IQueryable<HoaDonDienTuViewModel> query = from hd in _db.HoaDonDienTus
                                                          join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                          join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                                                          from mhd in tmpMauHoaDons.DefaultIfEmpty()
                                                          join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                                                          from kh in tmpKhachHangs.DefaultIfEmpty()
                                                              //join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                                                              //from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                                                          join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                                                          from nv in tmpNhanViens.DefaultIfEmpty()
                                                          join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                                                          from nl in tmpNguoiLaps.DefaultIfEmpty()
                                                          join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                                                          from lt in tmpLoaiTiens.DefaultIfEmpty()
                                                          where pagingParams.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                                                          orderby hd.NgayXoaBo.Value.Date descending, hd.NgayHoaDon.Value.Date descending, bkhhd.UyNhiemLapHoaDon descending, bkhhd.KyHieuMauSoHoaDon descending, bkhhd.KyHieuHoaDon descending, hd.SoHoaDon descending, hd.NgayLap.Value.Date descending
                                                          select new HoaDonDienTuViewModel
                                                          {
                                                              ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hd, bkhhd, listHoaDonDienTu, null),
                                                              ThongDiepGuiCQTId = hd.ThongDiepGuiCQTId,
                                                              HoaDonDienTuId = hd.HoaDonDienTuId,
                                                              NgayHoaDon = hd.NgayHoaDon,
                                                              NgayLap = hd.NgayLap,
                                                              NguoiLap = nl != null ? new DoiTuongViewModel
                                                              {
                                                                  Ma = nl.Ma,
                                                                  Ten = nl.Ten
                                                              }
                                                                                    : null,
                                                              SoHoaDon = hd.SoHoaDon,
                                                              MauHoaDonId = mhd.MauHoaDonId ?? string.Empty,
                                                              MauSo = bkhhd.KyHieuMauSoHoaDon + string.Empty,
                                                              KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                                                              MaCuaCQT = bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa ? (hd.MaCuaCQT ?? "<Chưa cấp mã>") : string.Empty,
                                                              HinhThucHoaDon = (int)bkhhd.HinhThucHoaDon,
                                                              TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
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
                                                              TenNhanVienBanHang = hd.TenNhanVienBanHang,
                                                              MaKhachHang = hd.MaKhachHang ?? string.Empty,
                                                              TenKhachHang = hd.TenKhachHang ?? string.Empty,
                                                              MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                                                              SoCTXoaBo = hd.SoCTXoaBo,
                                                              DiaChi = hd.DiaChi,
                                                              HinhThucThanhToanId = hd.HinhThucThanhToanId,
                                                              TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
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
                                                              TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
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
                                                              TaiLieuDinhKem = hd.TaiLieuDinhKem,
                                                              CreatedBy = hd.CreatedBy,
                                                              CreatedDate = hd.CreatedDate,
                                                              Status = hd.Status,
                                                              NgayXoaBo = hd.NgayXoaBo,
                                                              TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                                                              DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon,
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
                                                              IsNotCreateThayThe = hd.IsNotCreateThayThe,
                                                              HinhThucXoabo = hd.HinhThucXoabo,
                                                              BackUpTrangThai = hd.BackUpTrangThai,
                                                              TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                                                 where tldk.NghiepVuId == (hd != null ? hd.HoaDonDienTuId : null)
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
                                                                                 }).ToList()
                                                          };



                if (!string.IsNullOrEmpty(pagingParams.GiaTri))
                {
                    string keyword = pagingParams.GiaTri.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon.ToString().ToUpper().Contains(keyword) || x.SoHoaDon.ToString().ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
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


                if (pagingParams.TrangThaiPhatHanh.HasValue && pagingParams.TrangThaiPhatHanh != -1)
                {
                    if (pagingParams.TrangThaiPhatHanh == 3)
                    {
                        //nếu HĐ có mã CQT thì lấy HĐ đã cấp số
                        //nếu HĐ KHÔNG có mã CQT thì trạng thái quy trình không phải là <Chưa ký điện tử>; <Đang Ký điện tử>, <Ký điện tử lỗi:>
                        query = query.Where(x => (x.HinhThucHoaDon == (int)HinhThucHoaDon.CoMa && x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                            || (x.HinhThucHoaDon == (int)HinhThucHoaDon.KhongCoMa && x.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu && x.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DangKyDienTu && x.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KyDienTuLoi));
                    }
                }

                if (pagingParams.TrangThaiBienBanXoaBo.HasValue && pagingParams.TrangThaiBienBanXoaBo != -1)
                {
                    query = query.Where(x => x.TrangThaiBienBanXoaBo == pagingParams.TrangThaiBienBanXoaBo);
                }

                if (pagingParams.TrangThaiXoaBo.HasValue && pagingParams.TrangThaiXoaBo != -1)
                {
                    if (pagingParams.TrangThaiXoaBo == 0)//Hóa đơn đã xóa bỏ
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo);
                    }
                    else if (pagingParams.TrangThaiXoaBo == 1)//Hóa đơn xóa bỏ đã lập thay thế
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && _db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId));
                    }
                    else if (pagingParams.TrangThaiXoaBo == 2)
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && !_db.HoaDonDienTus.Any(o => o.ThayTheChoHoaDonId == x.HoaDonDienTuId)
                                             && ((x.IsNotCreateThayThe == false || x.HinhThucXoabo == 2 || x.HinhThucXoabo == 5)));
                    }
                    else if (pagingParams.TrangThaiXoaBo == 3)//Hóa đơn chưa xóa bỏ
                    {
                        query = query.Where(x => (x.TrangThai == 1 || (x.TrangThai == 3 && x.TrangThaiGuiHoaDon > 2)) && x.TrangThaiBienBanXoaBo > 0);// || (x.TrangThai == 4 && (x.TrangThaiGuiHoaDon == 0 || x.TrangThaiGuiHoaDon == 1 || x.TrangThaiGuiHoaDon == 2))
                        if (pagingParams.TrangThaiPhatHanh.HasValue && pagingParams.TrangThaiPhatHanh == -1)
                        {
                            query = query.Where(x => (x.HinhThucHoaDon == (int)HinhThucHoaDon.CoMa && x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                            || (x.HinhThucHoaDon == (int)HinhThucHoaDon.KhongCoMa && x.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaKyDienTu && x.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.DangKyDienTu && x.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KyDienTuLoi));
                        }
                    }
                    else if (pagingParams.TrangThaiXoaBo == 4)
                    {
                        query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo && (x.IsNotCreateThayThe == true || (x.HinhThucXoabo != 2 && x.HinhThucXoabo != 5)));
                    }
                    else if (pagingParams.TrangThaiXoaBo == 100)//điều kiên riêng của list hóa đơn lập biên bản
                    {
                        var notSelectHDId = hoaDonBiDieuChinhIds.Union(hoaDonDieuChinhIdsDaLapBBDC);
                        if (notSelectHDId != null)
                        {
                            query = query.Where(x => ((x.TrangThai == 1 && x.TrangThaiGuiHoaDon > 2) || (x.TrangThai == 3 && x.TrangThaiGuiHoaDon > 2)) && notSelectHDId.All(x2 => x.HoaDonDienTuId != x2));
                        }
                        else
                        {
                            query = query.Where(x => ((x.TrangThai == 1 && x.TrangThaiGuiHoaDon > 2) || (x.TrangThai == 3 && x.TrangThaiGuiHoaDon > 2)));
                        }
                    }
                    else if (pagingParams.TrangThaiXoaBo == 103)//điều kiên riêng của list hóa đơn xóa bỏ
                    {
                        var notSelectHDId = hoaDonBiDieuChinhIds.Union(hoaDonDieuChinhIdsDaLapBBDC);
                        if (notSelectHDId != null)
                        {
                            query = query.Where(x => (x.TrangThai == 1 || (x.TrangThai == 3 && x.TrangThaiGuiHoaDon > 2)) && notSelectHDId.All(x2 => x.HoaDonDienTuId != x2));
                        }
                        else
                        {
                            query = query.Where(x => (x.TrangThai == 1 || (x.TrangThai == 3 && x.TrangThaiGuiHoaDon > 2)));// || (x.TrangThai == 4 && (x.TrangThaiGuiHoaDon == 0 || x.TrangThaiGuiHoaDon == 1 || x.TrangThaiGuiHoaDon == 2))
                        }
                    }
                }
                else if (pagingParams.TrangThaiXoaBo.HasValue && pagingParams.TrangThaiXoaBo == -1)
                {
                    query = query.Where(x => x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo || x.TrangThaiBienBanXoaBo > 0);
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
                        query = query.Where(x => x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword));
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

                List<HoaDonDienTuViewModel> result = await query.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
                return null;
            }
        }
        private string GetThongTinChung(User user, DateTime? dateTime)
        {
            List<string> result = new List<string>();

            if (user != null)
            {
                result.Add(user.UserName);
            }

            if (dateTime.HasValue)
            {
                result.Add(dateTime.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            }

            return string.Join(" ", result);
        }

        private string GetHinhThucDieuChinh(HoaDonDienTuViewModel model, bool isHoaDonXoaBoDaBiThayThe, bool isHoaDonBiDieuChinh)
        {
            TrangThaiQuyTrinh trangThaiQuyTrinh = (TrangThaiQuyTrinh)model.TrangThaiQuyTrinh;
            TrangThaiHoaDon trangThaiHoaDon = (TrangThaiHoaDon)model.TrangThai;

            if ((trangThaiQuyTrinh == TrangThaiQuyTrinh.ChuaKyDienTu ||
                    trangThaiQuyTrinh == TrangThaiQuyTrinh.KyDienTuLoi ||
                    trangThaiQuyTrinh == TrangThaiQuyTrinh.DangKyDienTu) &&
                (trangThaiHoaDon == TrangThaiHoaDon.HoaDonGoc ||
                    trangThaiHoaDon == TrangThaiHoaDon.HoaDonThayThe))
            {
                return string.Empty;
            }

            if (trangThaiQuyTrinh != TrangThaiQuyTrinh.ChuaKyDienTu &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.KyDienTuLoi &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.DangKyDienTu &&
                (trangThaiHoaDon == TrangThaiHoaDon.HoaDonGoc || trangThaiHoaDon == TrangThaiHoaDon.HoaDonThayThe) &&
                !isHoaDonBiDieuChinh)
            {
                return "Chưa điều chỉnh";
            }

            if (trangThaiHoaDon == TrangThaiHoaDon.HoaDonDieuChinh)
            {
                return "Không điều chỉnh";
            }

            if ((trangThaiHoaDon == TrangThaiHoaDon.HoaDonXoaBo && !isHoaDonXoaBoDaBiThayThe))
            {
                return "Hủy";
            }

            if (trangThaiQuyTrinh != TrangThaiQuyTrinh.ChuaKyDienTu &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.KyDienTuLoi &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.DangKyDienTu &&
                (trangThaiHoaDon == TrangThaiHoaDon.HoaDonGoc || trangThaiHoaDon == TrangThaiHoaDon.HoaDonThayThe) &&
                isHoaDonBiDieuChinh)
            {
                return "Điều chỉnh";
            }

            if (trangThaiHoaDon == TrangThaiHoaDon.HoaDonXoaBo) // thay thế
            {
                return "Thay thế";
            }

            return string.Empty;
        }

        private async Task<(int trangThaiQuyTrinh, string xmlContent999)> SendDuLieuHoaDonToCQT(string xmlFilePath)
        {
            string fileBody = File.ReadAllText(xmlFilePath); // relative path;
            var status = (int)TrangThaiQuyTrinh.GuiLoi;

            // Send to TVAN
            string strContent = await _tVanService.TVANSendData("api/invoice/send", fileBody);
            if (!string.IsNullOrEmpty(strContent))
            {
                var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(strContent);
                if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                {
                    status = (int)TrangThaiQuyTrinh.GuiKhongLoi;
                }
            }
            else
            {
                status = (int)TrangThaiQuyTrinh.GuiTCTNLoi;
            }

            return (status, strContent);
        }

        /// <summary>
        /// link blob
        /// </summary>
        /// <param name="fileArray"></param>
        /// <returns></returns>
        public FileReturn XemHoaDonDongLoat2(List<string> fileArray)
        {
            string outPutFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(outPutFilePath))
            {
                Directory.CreateDirectory(outPutFilePath);
            }

            string fileName = $"{Guid.NewGuid()}.pdf";
            string filePath = Path.Combine(outPutFilePath, fileName);
            FileHelper.MergePDF(fileArray, filePath);

            byte[] fileByte = File.ReadAllBytes(filePath);
            File.Delete(filePath);

            return new FileReturn
            {
                Bytes = fileByte,
                ContentType = MimeTypes.GetMimeType(filePath),
                FileName = fileName
            };
        }

        public async Task UpdateTrangThaiQuyTrinhAsync(string id, TrangThaiQuyTrinh status)
        {
            var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
            if (entity != null)
            {
                entity.TrangThaiQuyTrinh = (int)status;
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> RemoveDigitalSignatureAsync(string id)
        {
            var entity = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
            if (entity != null)
            {
                entity.TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.GuiTCTNLoi;
                entity.XMLDaKy = null;
                entity.FileDaKy = null;

                var fileDatas = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == true).ToListAsync();
                _db.FileDatas.RemoveRange(fileDatas);

                var result = await _db.SaveChangesAsync();
                return result > 0;
            }

            return false;
        }

        private async Task UpdateFileDataForHDDT(string id, string fullPdfFilePath, string fullXmlFilePath)
        {
            var oldFileDatas = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == true).ToListAsync();
            _db.FileDatas.RemoveRange(oldFileDatas);

            var fileDatas = new List<FileData>
            {
                new FileData {
                    RefId = id,
                    Type = 2,
                    DateTime = DateTime.Now,
                    Binary = File.ReadAllBytes(fullPdfFilePath),
                    FileName = Path.GetFileName(fullPdfFilePath),
                    IsSigned = true
                },
                new FileData {
                    RefId = id,
                    Type = 1,
                    DateTime = DateTime.Now,
                    Binary = File.ReadAllBytes(fullXmlFilePath),
                    FileName = Path.GetFileName(fullXmlFilePath),
                    IsSigned = true
                }
            };
            await _db.FileDatas.AddRangeAsync(fileDatas);
        }

        private async Task UpdateFileDataPdfForHDDT(string id, string fullPdfFilePath)
        {
            var oldFileDatas = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == true && x.Type == 2).ToListAsync();
            _db.FileDatas.RemoveRange(oldFileDatas);

            var fileData = new FileData
            {
                RefId = id,
                Type = 2,
                DateTime = DateTime.Now,
                Binary = File.ReadAllBytes(fullPdfFilePath),
                FileName = Path.GetFileName(fullPdfFilePath),
                IsSigned = true
            };
            await _db.FileDatas.AddAsync(fileData);
        }

        private async Task UpdateFileDataXmlForHDDT(string id, string xml)
        {
            var oldFileDatas = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == true && x.Type == 1).ToListAsync();
            _db.FileDatas.RemoveRange(oldFileDatas);

            var fileData = new FileData
            {
                RefId = id,
                Type = 1,
                DateTime = DateTime.Now,
                Binary = File.ReadAllBytes(xml),
                FileName = Path.GetFileName(xml),
                IsSigned = true
            };
            await _db.FileDatas.AddAsync(fileData);
        }

        public async Task<ReloadPDFResult> ReloadPDFAsync(ReloadPDFParams @params)
        {
            if (@params.Password != "Bk9108vn")
            {
                return new ReloadPDFResult
                {
                    Status = false,
                    Message = "Sai mật khẩu"
                };
            }

            List<string> result = new List<string>();

            if (@params.SoHoaDon.HasValue && !string.IsNullOrEmpty(@params.KyHieu))
            {
                var entityId = await (from hddt in _db.HoaDonDienTus
                                      join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                      where bkh.KyHieu == @params.KyHieu && hddt.SoHoaDon == @params.SoHoaDon
                                      orderby hddt.SoHoaDon
                                      select hddt.HoaDonDienTuId).FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(entityId))
                {
                    var hddt = await GetByIdAsync(entityId);
                    hddt.IsReloadSignedPDF = true;
                    await ConvertHoaDonToFilePDF(hddt);

                    result.Add(hddt.BoKyHieuHoaDon.KyHieu + "-" + hddt.SoHoaDon);
                }
                else
                {
                    return new ReloadPDFResult
                    {
                        Status = false,
                        Message = "Ký hiệu/Số hóa đơn ko đúng"
                    };
                }
            }
            else if (!@params.SoHoaDon.HasValue && !string.IsNullOrEmpty(@params.KyHieu))
            {
                var entityIds = await (from hddt in _db.HoaDonDienTus
                                       join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                       where bkh.KyHieu == @params.KyHieu && hddt.SoHoaDon.HasValue
                                       orderby hddt.SoHoaDon
                                       select hddt.HoaDonDienTuId).ToListAsync();

                if (entityIds.Any())
                {
                    foreach (var item in entityIds)
                    {
                        var hddt = await GetByIdAsync(item);
                        hddt.IsReloadSignedPDF = true;
                        await ConvertHoaDonToFilePDF(hddt);

                        result.Add(hddt.BoKyHieuHoaDon.KyHieu + "-" + hddt.SoHoaDon);
                    }
                }
                else
                {
                    return new ReloadPDFResult
                    {
                        Status = false,
                        Message = "Ký hiệu ko đúng"
                    };
                }
            }
            else
            {
                var entityIds = await (from hddt in _db.HoaDonDienTus
                                       join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                       where hddt.SoHoaDon.HasValue
                                       orderby hddt.SoHoaDon
                                       select hddt.HoaDonDienTuId).ToListAsync();

                foreach (var item in entityIds)
                {
                    var hddt = await GetByIdAsync(item);
                    hddt.IsReloadSignedPDF = true;
                    await ConvertHoaDonToFilePDF(hddt);

                    result.Add(hddt.BoKyHieuHoaDon.KyHieu + "-" + hddt.SoHoaDon);
                }
            }

            return new ReloadPDFResult
            {
                Status = true,
                Message = "Thành công: " + string.Join(", ", result)
            };
        }

        private async Task<bool> RestoreFilesInvoiceSigned(string RefId)
        {
            bool res = false;

            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

                // Check folder XML                
                string folder = $"{_hostingEnvironment.WebRootPath}/FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Check folder XML
                folder = $"{_hostingEnvironment.WebRootPath}/FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                // Get list data file.
                string pullPath = string.Empty;
                var listDatas = await _db.FileDatas.Where(o => o.IsSigned == true && o.RefId == RefId).ToListAsync();
                foreach (var it in listDatas)
                {
                    if (it.Type == 1)            // XML
                    {
                        pullPath = $"{_hostingEnvironment.WebRootPath}/FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/{it.FileName}";
                    }
                    else if (it.Type == 2)       // PDF
                    {
                        pullPath = $"{_hostingEnvironment.WebRootPath}/FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}/{it.FileName}";
                    }

                    // Get File XML
                    if (!File.Exists(pullPath))
                    {
                        File.WriteAllBytes(pullPath, it.Binary);
                    }
                }

                res = true;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return res;
        }

        public async Task<FileReturn> DowloadXMLAsync(string id)
        {
            var file = await _db.FileDatas
                .OrderByDescending(x => x.IsSigned)
                .FirstOrDefaultAsync(x => x.RefId == id);

            if (file == null)
            {
                return null;
            }

            var dic = Path.Combine(_hostingEnvironment.WebRootPath, "temp");
            if (!Directory.Exists(dic))
            {
                Directory.CreateDirectory(dic);
            }

            var filePath = Path.Combine(dic, $"{Guid.NewGuid()}.xml");
            await File.WriteAllBytesAsync(filePath, file.Binary);

            if (file != null)
            {
                return new FileReturn
                {
                    Bytes = file.Binary,
                    ContentType = MimeTypes.GetMimeType(filePath),
                    FileName = Path.GetFileName(filePath),
                };
            }

            return null;
        }

        public async Task<NhapKhauResult> ImportHoaDonAsync(NhapKhauParams @params)
        {
            NhapKhauResult nhapKhauResult = new NhapKhauResult();
            List<HoaDonDienTuImport> result = new List<HoaDonDienTuImport>();
            var formFile = @params.Files[0];

            if (@params.FileType == 1)
            {
                using (var stream = new MemoryStream())
                {
                    await formFile.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var countSheet = package.Workbook.Worksheets;
                        ExcelWorksheet worksheet = null;

                        // ignore error sheet
                        for (int i = 0; i < countSheet.Count; i++)
                        {
                            worksheet = package.Workbook.Worksheets[i];
                            if (worksheet.Dimension == null)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Get total all row
                        int totalRows = worksheet.Dimension.Rows;
                        int numCol = worksheet.Dimension.Columns;

                        int rowStart = worksheet.Dimension.Start.Row;
                        int rowEnd = worksheet.Dimension.End.Row;
                        string cellRange = rowStart.ToString() + ":" + rowEnd.ToString();
                        var searchCell = from cell in worksheet.Cells[cellRange]
                                         where cell.Value?.ToString().Contains("Tính chất") == true
                                         select cell.Start.Row;

                        // Begin row
                        int begin_row = searchCell.Last() + 1;

                        var khachHangs = await _db.DoiTuongs.Where(x => x.IsKhachHang == true).AsNoTracking().ToListAsync();
                        var nhanViens = await _db.DoiTuongs.Where(x => x.IsNhanVien == true).AsNoTracking().ToListAsync();
                        var boKyHieuHoaDons = await _db.BoKyHieuHoaDons.AsNoTracking().ToListAsync();
                        var hhdvs = await _db.HangHoaDichVus.AsNoTracking().ToListAsync();
                        var donViTinhs = await _db.DonViTinhs.AsNoTracking().ToListAsync();
                        var loaiTiens = await _db.LoaiTiens.AsNoTracking().ToListAsync();
                        var loaiThueSuat = await (from mhd in _db.MauHoaDons
                                                  join bkhhd in _db.BoKyHieuHoaDons on mhd.MauHoaDonId equals bkhhd.MauHoaDonId
                                                  where bkhhd.BoKyHieuHoaDonId == @params.BoKyHieuHoaDonId
                                                  select mhd.LoaiThueGTGT).FirstOrDefaultAsync();
                        var _tuyChons = await _TuyChonService.GetAllAsync();
                        var tienVND = _mp.Map<LoaiTienViewModel>(loaiTiens.FirstOrDefault(x => x.Ma == "VND"));

                        string formatRequired = "<{0}> không được bỏ trống.";
                        string formatValid = "Dữ liệu cột <{0}> không hợp lệ.";
                        string formatExists = "{0} <{1}> không có trong danh mục.";

                        var truongDLHDExcels = new List<TruongDLHDExcel>();
                        var enumTruongDLHDs = new TruongDLHDExcel().GetTruongDLHDExcels();

                        // declare thue by so thu thu hoa don
                        Dictionary<int, List<string>> thuePairs = new Dictionary<int, List<string>>();
                        // declare tyle % doanh thu by so thu thu hoa don
                        Dictionary<int, List<decimal>> tyLePhanTramDoanhThuPairs = new Dictionary<int, List<decimal>>();

                        for (int i = 3; i <= numCol; i++)
                        {
                            var maTruong = (worksheet.Cells[begin_row - 2, i].Value ?? string.Empty).ToString();
                            var tenTruong = (worksheet.Cells[begin_row - 1, i].Value ?? string.Empty).ToString();

                            if (!string.IsNullOrEmpty(maTruong))
                            {
                                var findEnum = enumTruongDLHDs.FirstOrDefault(x => x.NameOfKey == maTruong);
                                if (findEnum != null)
                                {
                                    var maEnum = (MaTruongDLHDExcel)findEnum.Value;
                                    int nhomThongTin = 1;

                                    if (maEnum >= MaTruongDLHDExcel.HHDV2)
                                    {
                                        nhomThongTin = 2;
                                    }

                                    truongDLHDExcels.Add(new TruongDLHDExcel(maEnum)
                                    {
                                        Ma = maEnum,
                                        ColIndex = i,
                                        TenTruong = findEnum.Name,
                                        NhomThongTin = nhomThongTin,
                                        TenTruongExcel = tenTruong,
                                        TenEnum = findEnum.NameOfKey
                                    });
                                }
                            }
                        }

                        for (int i = begin_row; i <= totalRows; i++)
                        {
                            HoaDonDienTuImport item = new HoaDonDienTuImport
                            {
                                Row = i,
                                HasError = true,
                                STT = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString().ParseInt(),
                                BoKyHieuHoaDonId = @params.BoKyHieuHoaDonId,
                                LoaiHoaDon = @params.LoaiHoaDon,
                                IsVND = true,
                                TyGia = 1,
                                LoaiTienId = tienVND.LoaiTienId
                            };

                            var copyData = result.FirstOrDefault(x => x.STT == item.STT);

                            if (copyData != null)
                            {
                                item = (HoaDonDienTuImport)copyData.Clone();
                                item.Row = i;
                                item.HasError = true;
                                item.ErrorMessage = string.Empty;

                                var checkError = result.FirstOrDefault(x => x.STT == item.STT && x.HasError == true);

                                if (checkError != null)
                                {
                                    item.IsMainError = false;
                                    item.ErrorMessage = $"Dòng chi tiết liên quan (dòng số <{checkError.Row}>) bị lỗi.";
                                }
                            }
                            else
                            {
                                #region Số thứ tự hóa đơn
                                string stt = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString().Trim();
                                if (string.IsNullOrEmpty(item.ErrorMessage) && string.IsNullOrEmpty(stt))
                                {
                                    item.ErrorMessage = string.Format(formatRequired, "Số thứ tự hóa đơn");
                                }
                                var checkSTTHoaDon = stt.IsValidInt(out int outputSTTHoaDon);
                                if (string.IsNullOrEmpty(item.ErrorMessage) && !checkSTTHoaDon)
                                {
                                    item.ErrorMessage = string.Format(formatValid, "Số thứ tự hóa đơn");
                                }
                                if (checkSTTHoaDon)
                                {
                                    item.STT = outputSTTHoaDon;
                                }
                                #endregion

                                var nhomThongTinNguoiMua = truongDLHDExcels.Where(x => x.NhomThongTin == 1).ToList();
                                DoiTuongViewModel khachHang = null;
                                LoaiTienViewModel loaiTien = null;

                                foreach (var group in nhomThongTinNguoiMua)
                                {
                                    switch (group.Ma)
                                    {
                                        case MaTruongDLHDExcel.NVBANHANG:
                                            item.MaNhanVienBanHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            DoiTuongViewModel nhanVien = null;
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !string.IsNullOrEmpty(item.MaNhanVienBanHang))
                                            {
                                                nhanVien = _doiTuongService.CheckMaOutObject(item.MaNhanVienBanHang, nhanViens, false);
                                                if (nhanVien == null)
                                                {
                                                    item.ErrorMessage = string.Format(formatExists, group.TenTruong, item.MaNhanVienBanHang);
                                                }
                                            }
                                            if (nhanVien != null)
                                            {
                                                item.NhanVienBanHangId = nhanVien.DoiTuongId;
                                            }
                                            break;
                                        case MaTruongDLHDExcel.NGAYHOADON:
                                            item.StrNgayHoaDon = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.NgayHoaDon = worksheet.Cells[i, group.ColIndex].Value.ParseExactCellDate(out bool isValidNgayHoaDon);
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && string.IsNullOrEmpty(item.StrNgayHoaDon))
                                            {
                                                item.ErrorMessage = string.Format(formatRequired, group.TenTruong);
                                            }
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !isValidNgayHoaDon)
                                            {
                                                item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                            }
                                            break;
                                        case MaTruongDLHDExcel.NM1:
                                            item.HoTenNguoiMuaHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            break;
                                        case MaTruongDLHDExcel.NM2:
                                            item.MaKhachHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            khachHang = _doiTuongService.CheckMaOutObject(item.MaKhachHang, khachHangs, true);
                                            if (khachHang != null)
                                            {
                                                item.KhachHangId = khachHang.DoiTuongId;
                                            }
                                            break;
                                        case MaTruongDLHDExcel.NM3:
                                            item.TenKhachHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.TenKhachHang = string.IsNullOrEmpty(item.TenKhachHang) ? khachHang?.Ten : item.TenKhachHang;
                                            break;
                                        case MaTruongDLHDExcel.NM4:
                                            item.DiaChi = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.DiaChi = string.IsNullOrEmpty(item.DiaChi) ? khachHang?.DiaChi : item.DiaChi;
                                            break;
                                        case MaTruongDLHDExcel.NM5:
                                            item.MaSoThue = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            if (string.IsNullOrEmpty(item.MaSoThue) && khachHang != null)
                                            {
                                                item.MaSoThue = khachHang.MaSoThue ?? string.Empty;
                                            }
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !string.IsNullOrEmpty(item.MaSoThue) && !item.MaSoThue.CheckValidMaSoThue())
                                            {
                                                item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                            }
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !string.IsNullOrEmpty(item.MaSoThue) && string.IsNullOrEmpty(item.TenKhachHang))
                                            {
                                                item.ErrorMessage = "Bắt buộc phải nhập thông tin <Tên khách hàng> khi đã có thông tin <Mã số thuế>.";
                                            }
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !string.IsNullOrEmpty(item.MaSoThue) && string.IsNullOrEmpty(item.DiaChi))
                                            {
                                                item.ErrorMessage = "Bắt buộc phải nhập thông tin <Địa chỉ> khi đã có thông tin <Mã số thuế>.";
                                            }
                                            break;
                                        case MaTruongDLHDExcel.NM6:
                                            item.HinhThucThanhToanId = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.HinhThucThanhToanId = string.IsNullOrEmpty(item.HinhThucThanhToanId) ? "3" : item.HinhThucThanhToanId;
                                            bool checkHinhThucThanhToan = int.TryParse(item.HinhThucThanhToanId, out int hinhThucThanhToan);
                                            if (string.IsNullOrEmpty(item.ErrorMessage))
                                            {
                                                if (!checkHinhThucThanhToan || (checkHinhThucThanhToan && (hinhThucThanhToan < 1 || hinhThucThanhToan > 6)))
                                                {
                                                    item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                                }
                                            }
                                            break;
                                        case MaTruongDLHDExcel.NM7:
                                            item.EmailNguoiMuaHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.EmailNguoiMuaHang = string.IsNullOrEmpty(item.EmailNguoiMuaHang) ? khachHang?.EmailNguoiMuaHang : item.EmailNguoiMuaHang;
                                            break;
                                        case MaTruongDLHDExcel.NM8:
                                            item.SoDienThoaiNguoiMuaHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.SoDienThoaiNguoiMuaHang = string.IsNullOrEmpty(item.SoDienThoaiNguoiMuaHang) ? khachHang?.SoDienThoaiNguoiMuaHang : item.SoDienThoaiNguoiMuaHang;
                                            break;
                                        case MaTruongDLHDExcel.NM9:
                                            item.SoTaiKhoanNganHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.SoTaiKhoanNganHang = string.IsNullOrEmpty(item.SoTaiKhoanNganHang) ? khachHang?.SoTaiKhoanNganHang : item.SoTaiKhoanNganHang;
                                            break;
                                        case MaTruongDLHDExcel.NM10:
                                            item.TenNganHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            item.TenNganHang = string.IsNullOrEmpty(item.TenNganHang) ? khachHang?.TenNganHang : item.TenNganHang;
                                            break;
                                        case MaTruongDLHDExcel.LOAITIEN:
                                            item.MaLoaiTien = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            if (string.IsNullOrEmpty(item.MaLoaiTien))
                                            {
                                                loaiTien = tienVND;
                                            }
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !string.IsNullOrEmpty(item.MaLoaiTien))
                                            {
                                                loaiTien = _loaiTienService.CheckMaOutObject(item.MaLoaiTien, loaiTiens);
                                                if (loaiTien == null)
                                                {
                                                    item.ErrorMessage = string.Format(formatExists, group.TenTruong, item.MaLoaiTien);
                                                }
                                            }
                                            if (loaiTien != null)
                                            {
                                                item.LoaiTienId = loaiTien.LoaiTienId;
                                                item.IsVND = loaiTien.Ma == "VND";
                                            }
                                            break;
                                        case MaTruongDLHDExcel.TYGIA:
                                            string tyGia = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                            if (string.IsNullOrEmpty(tyGia) && loaiTien != null)
                                            {
                                                tyGia = loaiTien.TyGiaQuyDoi.ToString();
                                            }
                                            var checkValidTyGia = tyGia.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.TY_GIA, out decimal outputTyGia);
                                            if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTyGia)
                                            {
                                                item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                            }
                                            item.TyGia = outputTyGia.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TY_GIA);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                if (string.IsNullOrEmpty(item.ErrorMessage) && string.IsNullOrEmpty(item.HoTenNguoiMuaHang) && string.IsNullOrEmpty(item.TenKhachHang))
                                {
                                    item.ErrorMessage = "Bạn bắt buộc phải nhập ít nhất một trong hai thông tin <Tên khách hàng> hoặc <Người mua hàng>";
                                }
                            }

                            var nhomThongTinHHDVs = truongDLHDExcels.Where(x => x.NhomThongTin == 2).ToList();
                            HangHoaDichVuViewModel hangHoaDichVu = null;

                            #region Tính chất
                            string strTinhChat = (worksheet.Cells[i, 1].Value ?? string.Empty).ToString().Trim();
                            strTinhChat = string.IsNullOrEmpty(strTinhChat) ? "1" : strTinhChat;
                            bool checkTinhChat = int.TryParse(strTinhChat, out int tinhChat);
                            if (string.IsNullOrEmpty(item.ErrorMessage))
                            {
                                if (!checkTinhChat || (checkTinhChat && (tinhChat < 1 || tinhChat > 4)))
                                {
                                    item.ErrorMessage = string.Format(formatValid, "Tính chất");
                                }
                            }
                            if (checkTinhChat)
                            {
                                item.HoaDonChiTiet.TinhChat = tinhChat;
                            }
                            #endregion

                            foreach (var group in nhomThongTinHHDVs)
                            {
                                switch (group.Ma)
                                {
                                    case MaTruongDLHDExcel.HHDV2:
                                        item.HoaDonChiTiet.MaHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        hangHoaDichVu = _hangHoaDichVuService.CheckMaOutObject(item.HoaDonChiTiet.MaHang, hhdvs);
                                        if (hangHoaDichVu != null)
                                        {
                                            item.HoaDonChiTiet.HangHoaDichVuId = hangHoaDichVu.HangHoaDichVuId;
                                        }
                                        break;
                                    case MaTruongDLHDExcel.HHDV3:
                                        item.HoaDonChiTiet.TenHang = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && string.IsNullOrEmpty(item.HoaDonChiTiet.TenHang))
                                        {
                                            item.ErrorMessage = string.Format(formatRequired, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TenHang = string.IsNullOrEmpty(item.HoaDonChiTiet.TenHang) ? hangHoaDichVu?.Ten : item.HoaDonChiTiet.TenHang;
                                        break;
                                    case MaTruongDLHDExcel.HHDV4:
                                        break;
                                    case MaTruongDLHDExcel.HHDV5:
                                        break;
                                    case MaTruongDLHDExcel.HHDV6:
                                        item.HoaDonChiTiet.TenDonViTinh = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        DonViTinhViewModel donViTinh = _donViTinhService.CheckTenOutObject(item.HoaDonChiTiet.TenDonViTinh, donViTinhs);
                                        if (donViTinh != null)
                                        {
                                            item.HoaDonChiTiet.DonViTinhId = donViTinh.DonViTinhId;
                                        }
                                        break;
                                    case MaTruongDLHDExcel.HHDV7:
                                        string soLuong = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidSoLuong = soLuong.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.SO_LUONG, out decimal outputSoLuong);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidSoLuong)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.SoLuong = outputSoLuong.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.SO_LUONG);
                                        break;
                                    case MaTruongDLHDExcel.HHDV9:
                                        string donGia = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidDonGia = donGia.IsValidCurrencyOutput(_tuyChons, (item.IsVND == true ? LoaiDinhDangSo.DON_GIA_QUY_DOI : LoaiDinhDangSo.DON_GIA_NGOAI_TE), out decimal outputDonGia);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidDonGia)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.DonGia = outputDonGia.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.DON_GIA_QUY_DOI : LoaiDinhDangSo.DON_GIA_NGOAI_TE);
                                        break;
                                    case MaTruongDLHDExcel.HHDV11:
                                        string thanhTien = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidThanhTien = thanhTien.IsValidCurrencyOutput(_tuyChons, (item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE), out decimal outputThanhTien);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidThanhTien)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.ThanhTien = outputThanhTien.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                                        break;
                                    case MaTruongDLHDExcel.HHDV12:
                                        string thanhTienQuyDoi = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidThanhTienQuyDoi = thanhTienQuyDoi.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI, out decimal outputThanhTienQuyDoi);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidThanhTienQuyDoi)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.ThanhTienQuyDoi = outputThanhTienQuyDoi.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);
                                        break;
                                    case MaTruongDLHDExcel.HHDV13:
                                        string tyLeCK = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTyLeCK = tyLeCK.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.HESO_TYLE, out decimal outputTyLeCK);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTyLeCK)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TyLeChietKhau = outputTyLeCK.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE);

                                        var intTyLeChietKhau = (int)item.HoaDonChiTiet.TyLeChietKhau;
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && intTyLeChietKhau.ToString().Count() > 2)
                                        {
                                            item.ErrorMessage = "Đối với các hóa đơn có thông tin chiết khấu thì <Tỷ lệ chiết khấu> phải nhỏ hơn 100%. Vui lòng kiểm tra lại!";
                                        }
                                        break;
                                    case MaTruongDLHDExcel.HHDV14:
                                        string tienCK = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTienCK = tienCK.IsValidCurrencyOutput(_tuyChons, (item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE), out decimal outputTienCK);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTienCK)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TienChietKhau = outputTienCK.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                                        break;
                                    case MaTruongDLHDExcel.HHDV15:
                                        string tienCKQuyDoi = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTienCKQuyDoi = tienCKQuyDoi.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI, out decimal outputTienCKQuyDoi);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTienCKQuyDoi)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TienChietKhauQuyDoi = outputTienCKQuyDoi.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);
                                        break;
                                    case MaTruongDLHDExcel.HHDV16:
                                        item.HoaDonChiTiet.ThueGTGT = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && string.IsNullOrEmpty(item.HoaDonChiTiet.ThueGTGT))
                                        {
                                            item.ErrorMessage = string.Format(formatRequired, group.TenTruong);
                                        }
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !item.HoaDonChiTiet.ThueGTGT.CheckValidThueGTGT())
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && item.HoaDonChiTiet.ThueGTGT == "8") // check giảm thuế 8% trong khoảng 01/02/2022 đến 31/12/2022
                                        {
                                            var monthOfNgayHoaDon = item.NgayHoaDon.Value.Month;
                                            var yearOfNgayHoaDon = item.NgayHoaDon.Value.Year;

                                            if (!(monthOfNgayHoaDon >= 2 && monthOfNgayHoaDon <= 12 && yearOfNgayHoaDon == 2022))
                                            {
                                                item.ErrorMessage = "Thuế suất 8% áp dụng trong thời gian từ 01/02/2022 đến 31/12/2022";
                                            }
                                        }
                                        if (string.IsNullOrEmpty(item.ErrorMessage)) // check TH 
                                        {
                                            if (thuePairs.ContainsKey(item.STT)) // nếu là thuế tiếp theo trong hóa đơn thì KT
                                            {
                                                var thues = thuePairs[item.STT];

                                                // Nếu là 1 thuế suất thì set các thuế còn lại = thuế dòng đầu tiền
                                                if (loaiThueSuat == LoaiThueGTGT.MauMotThueSuat)
                                                {
                                                    item.HoaDonChiTiet.ThueGTGT = thues[0];
                                                }
                                                else // Nếu là nhiều thuế suất
                                                {
                                                    thues.Add(item.HoaDonChiTiet.ThueGTGT);

                                                    // Nếu có thuế 8% + thuế khác 8% thì báo
                                                    if (thues.Contains("8") && thues.Distinct().ToList().Count > 1)
                                                    {
                                                        item.ErrorMessage = "Người dùng phải lập hóa đơn riêng cho hàng hóa dịch vụ được giảm thuế giá trị gia tăng (thuế suất 8%)";
                                                    }

                                                    thuePairs.Remove(item.STT);
                                                    thuePairs.Add(item.STT, thues);
                                                }
                                            }
                                            else // nếu là thuế của dòng đầu tiên trong hóa đơn thì add vào dic
                                            {
                                                thuePairs.Add(item.STT, new List<string> { item.HoaDonChiTiet.ThueGTGT });
                                            }
                                        }
                                        break;
                                    case MaTruongDLHDExcel.HHDV17:
                                        string tienThueGTGT = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTienThueGTGT = tienThueGTGT.IsValidCurrencyOutput(_tuyChons, (item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE), out decimal outputTienThueGTGT);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTienThueGTGT)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TienThueGTGT = outputTienThueGTGT.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                                        break;
                                    case MaTruongDLHDExcel.HHDV18:
                                        string tienThueGTGTQuyDoi = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTienThueGTGTQuyDoi = tienThueGTGTQuyDoi.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI, out decimal outputTienThueGTGTQuyDoi);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTienThueGTGTQuyDoi)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TienThueGTGTQuyDoi = outputTienThueGTGTQuyDoi.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);
                                        break;
                                    case MaTruongDLHDExcel.HHDV37:
                                        string tyLePhanTramDoanhThu = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTyLePhanTramDoanThu = tyLePhanTramDoanhThu.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.HESO_TYLE, out decimal outputTyLePhanTramDoanhThu);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTyLePhanTramDoanThu)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TyLePhanTramDoanhThu = outputTyLePhanTramDoanhThu.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE);

                                        if (string.IsNullOrEmpty(item.ErrorMessage) && item.HoaDonChiTiet.TyLePhanTramDoanhThu != 0) // check giảm thuế GTGT trong khoảng 01/02/2022 đến 31/12/2022
                                        {
                                            var monthOfNgayHoaDon = item.NgayHoaDon.Value.Month;
                                            var yearOfNgayHoaDon = item.NgayHoaDon.Value.Year;

                                            if (!(monthOfNgayHoaDon >= 2 && monthOfNgayHoaDon <= 12 && yearOfNgayHoaDon == 2022))
                                            {
                                                item.ErrorMessage = "Giảm thuế GTGT áp dụng trong thời gian từ 01/02/2022 đến 31/12/2022";
                                            }
                                        }
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && item.HoaDonChiTiet.TyLePhanTramDoanhThu != 0)
                                        {
                                            if (tyLePhanTramDoanhThuPairs.ContainsKey(item.STT)) // nếu là phần trăm tiếp theo trong hóa đơn thì KT
                                            {
                                                var tyLePhanTramDoanhThuPair = tyLePhanTramDoanhThuPairs[item.STT];

                                                tyLePhanTramDoanhThuPair.Add(item.HoaDonChiTiet.TyLePhanTramDoanhThu.Value);

                                                // Nếu là 1 hóa đơn chọn nhiều tỷ lệ % doanh thu khác nhau
                                                if (tyLePhanTramDoanhThuPair.Distinct().ToList().Count > 1)
                                                {
                                                    item.ErrorMessage = "Người dùng phải lập hóa đơn riêng cho hàng hóa dịch vụ được giảm thuế giá trị gia tăng và riêng cho từng Tỷ lệ % trên doanh thu";
                                                }

                                                tyLePhanTramDoanhThuPairs.Remove(item.STT);
                                                tyLePhanTramDoanhThuPairs.Add(item.STT, tyLePhanTramDoanhThuPair);
                                            }
                                            else // nếu là tỷ lệ của dòng đầu tiên trong hóa đơn thì add vào dic
                                            {
                                                tyLePhanTramDoanhThuPairs.Add(item.STT, new List<decimal> { item.HoaDonChiTiet.TyLePhanTramDoanhThu.Value });
                                            }
                                        }
                                        break;
                                    case MaTruongDLHDExcel.HHDV38:
                                        string tienGiam = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTienGiam = tienGiam.IsValidCurrencyOutput(_tuyChons, (item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE), out decimal outputTienGiam);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTienGiam)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TienGiam = outputTienGiam.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                                        break;
                                    case MaTruongDLHDExcel.HHDV39:
                                        string tienGiamQuyDoi = (worksheet.Cells[i, group.ColIndex].Value ?? string.Empty).ToString().Trim();
                                        var checkValidTienGiamQuyDoi = tienGiamQuyDoi.IsValidCurrencyOutput(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI, out decimal outputTienGiamQuyDoi);
                                        if (string.IsNullOrEmpty(item.ErrorMessage) && !checkValidTienGiamQuyDoi)
                                        {
                                            item.ErrorMessage = string.Format(formatValid, group.TenTruong);
                                        }
                                        item.HoaDonChiTiet.TienGiamQuyDoi = outputTienGiamQuyDoi.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (string.IsNullOrEmpty(item.ErrorMessage))
                            {
                                var checkHoaDon = await CheckHoaDonPhatHanhAsync(new ParamPhatHanhHD
                                {
                                    SkipCheckHetHieuLucTrongKhoang = true,
                                    SkipChecNgayKyLonHonNgayHoaDon = true,
                                    IsPhatHanh = false,
                                    HoaDon = new HoaDonDienTuViewModel
                                    {
                                        NgayHoaDon = item.NgayHoaDon,
                                        BoKyHieuHoaDonId = item.BoKyHieuHoaDonId,
                                        LoaiHoaDon = item.LoaiHoaDon,
                                        HoaDonChiTiets = new List<HoaDonDienTuChiTietViewModel>(),
                                        IsVND = item.IsVND
                                    }
                                });

                                if (checkHoaDon != null && checkHoaDon.IsYesNo != true)
                                {
                                    item.ErrorMessage = Regex.Replace(checkHoaDon.ErrorMessage, "<.*?>", string.Empty);
                                    item.HasError = true;
                                }
                                else
                                {
                                    item.ErrorMessage = "<Hợp lệ>";
                                    item.HasError = false;
                                }
                            }
                            else
                            {
                                if (item.IsMainError == null)
                                {
                                    item.IsMainError = true;
                                }
                            }

                            result.Add(item);
                        }

                        // Quét lại error
                        List<HoaDonDienTuImport> listError = new List<HoaDonDienTuImport>();
                        foreach (HoaDonDienTuImport item in result)
                        {
                            var errorItem = result.FirstOrDefault(x => x.STT == item.STT && x.HasError == true);
                            if (errorItem != null && !listError.Any(x => x.STT == item.STT))
                            {
                                listError.Add(errorItem);
                            }

                            if (listError.Any(x => x.STT == item.STT) && item.HasError != true)
                            {
                                var errorItem2 = listError.FirstOrDefault(x => x.STT == item.STT);
                                item.HasError = true;
                                item.ErrorMessage = $"Dòng chi tiết liên quan (dòng số <{errorItem2.Row}>) bị lỗi.";
                            }
                        }

                        nhapKhauResult.ListResult = result;
                        nhapKhauResult.ListTruongDuLieu = truongDLHDExcels;
                    }
                }
            }
            else
            {
                using (var reader = new StreamReader(formFile.OpenReadStream()))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(reader);

                    XmlNodeList hDons = doc.SelectNodes("/TDiep/DLieu/HDon");

                    foreach (XmlNode item in hDons)
                    {
                        var strLoaiHoaDon = item.SelectSingleNode("descendant::KHMSHDon").InnerText;
                        var dscks = item.SelectSingleNode("DSCKS");
                        dscks.ParentNode.RemoveChild(dscks);

                        if (Enum.TryParse(strLoaiHoaDon, out LoaiHoaDon loaiHoaDon))
                        {
                            var dataXML = item.OuterXml;
                            var signatureTime = dscks.SelectSingleNode("NBan").SelectSingleNode("descendant::SigningTime");
                            DateTime? ngayKy;
                            if (signatureTime != null)
                            {
                                ngayKy = DateTime.Parse($"{signatureTime.InnerText.Replace("T", " ")}");
                            }

                            switch (loaiHoaDon)
                            {
                                case LoaiHoaDon.HoaDonGTGT:
                                    var hoaDonGTGT = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HDon>(dataXML);
                                    break;
                                case LoaiHoaDon.HoaDonBanHang:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            return nhapKhauResult;
        }

        public async Task<bool> InsertImportHoaDonAsync(List<HoaDonDienTuImport> data)
        {
            var tuyChons = await _TuyChonService.GetAllAsync();

            var group = data.GroupBy(x => x.STT)
                .Select(x => new HoaDonDienTuViewModel
                {
                    NgayHoaDon = x.First().NgayHoaDon,
                    BoKyHieuHoaDonId = x.First().BoKyHieuHoaDonId,
                    KhachHangId = x.First().KhachHangId,
                    MaKhachHang = x.First().MaKhachHang,
                    TenKhachHang = x.First().TenKhachHang,
                    DiaChi = x.First().DiaChi,
                    MaSoThue = x.First().MaSoThue,
                    HoTenNguoiMuaHang = x.First().HoTenNguoiMuaHang,
                    SoDienThoaiNguoiMuaHang = x.First().SoDienThoaiNguoiMuaHang,
                    EmailNguoiMuaHang = x.First().EmailNguoiMuaHang,
                    TenNganHang = x.First().TenNganHang,
                    SoTaiKhoanNganHang = x.First().SoTaiKhoanNganHang,
                    HinhThucThanhToanId = x.First().HinhThucThanhToanId,
                    NhanVienBanHangId = x.First().NhanVienBanHangId,
                    MaNhanVienBanHang = x.First().MaNhanVienBanHang,
                    TenNhanVienBanHang = x.First().TenNhanVienBanHang,
                    LoaiTienId = x.First().LoaiTienId,
                    TyGia = x.First().TyGia,
                    TrangThai = (int)TrangThaiHoaDon.HoaDonGoc,
                    TrangThaiQuyTrinh = (int)TrangThaiQuyTrinh.ChuaKyDienTu,
                    TrangThaiGuiHoaDon = (int)TrangThaiGuiHoaDon.ChuaGui,
                    LoaiHoaDon = x.First().LoaiHoaDon ?? 1,
                    IsVND = x.First().IsVND,
                    LoaiChietKhau = LoaiChietKhau.TheoMatHang,
                    HoaDonChiTiets = x.Select(y => new HoaDonDienTuChiTietViewModel
                    {
                        HangHoaDichVuId = y.HoaDonChiTiet.HangHoaDichVuId,
                        MaHang = y.HoaDonChiTiet.MaHang,
                        TenHang = y.HoaDonChiTiet.TenHang,
                        TinhChat = y.HoaDonChiTiet.TinhChat,
                        DonViTinhId = y.HoaDonChiTiet.DonViTinhId,
                        TenDonViTinh = y.HoaDonChiTiet.TenDonViTinh,
                        DonGia = y.HoaDonChiTiet.DonGia,
                        SoLuong = y.HoaDonChiTiet.SoLuong,
                        ThanhTien = y.HoaDonChiTiet.ThanhTien ?? 0,
                        ThanhTienQuyDoi = y.HoaDonChiTiet.ThanhTienQuyDoi ?? 0,
                        TyLeChietKhau = y.HoaDonChiTiet.TyLeChietKhau ?? 0,
                        TienChietKhau = y.HoaDonChiTiet.TienChietKhau ?? 0,
                        TienChietKhauQuyDoi = y.HoaDonChiTiet.TienChietKhauQuyDoi ?? 0,
                        ThueGTGT = y.HoaDonChiTiet.ThueGTGT.ConvertThueExcetToDB(),
                        TienThueGTGT = y.HoaDonChiTiet.TienThueGTGT ?? 0,
                        TienThueGTGTQuyDoi = y.HoaDonChiTiet.TienThueGTGTQuyDoi ?? 0,
                        TyLePhanTramDoanhThu = y.HoaDonChiTiet.TyLePhanTramDoanhThu ?? 0,
                        TienGiam = y.HoaDonChiTiet.TienGiam ?? 0,
                        TienGiamQuyDoi = y.HoaDonChiTiet.TienGiamQuyDoi ?? 0,
                    }).ToList()
                });

            var addedHDDTList = new List<HoaDonDienTu>();
            var addedDoiTuongList = new List<DoiTuong>();
            var addedHHDVList = new List<HangHoaDichVu>();
            var addedDVTList = new List<DonViTinh>();

            foreach (var item in group)
            {
                switch ((LoaiHoaDon)item.LoaiHoaDon)
                {
                    case LoaiHoaDon.HoaDonGTGT:
                        item.IsGiamTheoNghiQuyet = item.HoaDonChiTiets.All(x => x.ThueGTGT == "8");
                        break;
                    case LoaiHoaDon.HoaDonBanHang:
                        item.IsGiamTheoNghiQuyet = item.HoaDonChiTiets.All(x => x.TyLePhanTramDoanhThu != 0);
                        break;
                    case LoaiHoaDon.HoaDonBanTaiSanCong:
                        break;
                    case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                        break;
                    case LoaiHoaDon.CacLoaiHoaDonKhac:
                        break;
                    case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(item.MaKhachHang) && string.IsNullOrEmpty(item.KhachHangId))
                {
                    var addedKhachHangItem = addedDoiTuongList.FirstOrDefault(x => x.Ma.ToUpper() == item.MaKhachHang.ToUpper());

                    if (addedKhachHangItem != null)
                    {
                        item.KhachHangId = addedKhachHangItem.DoiTuongId;
                    }
                    else
                    {
                        string doiTuongId = Guid.NewGuid().ToString();
                        item.KhachHangId = doiTuongId;

                        addedDoiTuongList.Add(new DoiTuong
                        {
                            DoiTuongId = doiTuongId,
                            LoaiKhachHang = 2,
                            MaSoThue = item.MaSoThue,
                            Ma = item.MaKhachHang,
                            Ten = item.TenKhachHang,
                            DiaChi = item.DiaChi,
                            SoTaiKhoanNganHang = item.SoTaiKhoanNganHang,
                            TenNganHang = item.TenNganHang,
                            HoTenNguoiMuaHang = item.HoTenNguoiMuaHang,
                            EmailNguoiMuaHang = item.EmailNguoiMuaHang,
                            SoDienThoaiNguoiMuaHang = item.SoDienThoaiNguoiMuaHang,
                            IsKhachHang = true,
                            Status = true
                        });
                    }
                }

                int stt = 1;
                foreach (var detail in item.HoaDonChiTiets)
                {
                    if (!string.IsNullOrEmpty(detail.MaHang) && string.IsNullOrEmpty(detail.HangHoaDichVuId))
                    {
                        var addedHHDVItem = addedHHDVList.FirstOrDefault(x => x.Ma.ToUpper() == detail.MaHang.ToUpper());

                        if (addedHHDVItem != null)
                        {
                            detail.HangHoaDichVuId = addedHHDVItem.HangHoaDichVuId;
                        }
                        else
                        {
                            string hhdvId = Guid.NewGuid().ToString();
                            detail.HangHoaDichVuId = hhdvId;

                            addedHHDVList.Add(new HangHoaDichVu
                            {
                                HangHoaDichVuId = hhdvId,
                                Ma = detail.MaHang,
                                Ten = detail.TenHang,
                                DonViTinhId = detail.DonViTinhId,
                                DonGiaBan = detail.DonGia,
                                ThueGTGT = detail.ThueGTGT,
                                TyLeChietKhau = detail.TyLeChietKhau,
                                Status = true
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(detail.TenDonViTinh) && string.IsNullOrEmpty(detail.DonViTinhId))
                    {
                        var addedDVTItem = addedDVTList.FirstOrDefault(x => x.Ten.ToUpper() == detail.TenDonViTinh.ToUpper());

                        if (addedDVTItem != null)
                        {
                            detail.DonViTinhId = addedDVTItem.DonViTinhId;
                        }
                        else
                        {
                            string donViTinhId = Guid.NewGuid().ToString();
                            detail.DonViTinhId = donViTinhId;

                            addedDVTList.Add(new DonViTinh
                            {
                                DonViTinhId = donViTinhId,
                                Ten = detail.TenDonViTinh,
                                Status = true
                            });
                        }
                    }

                    if (detail.TinhChat == 1 || detail.TinhChat == 2)
                    {
                        detail.STT = stt;
                        stt += 1;
                    }
                    else
                    {
                        if (detail.TinhChat == 3)
                        {
                            detail.TyLeChietKhau = 0;
                            detail.TienChietKhau = 0;
                            detail.TienChietKhauQuyDoi = 0;
                        }
                        else
                        {
                            detail.SoLuong = 0;
                            detail.DonGia = 0;
                            detail.DonGiaSauThue = 0;
                            detail.ThanhTien = 0;
                            detail.ThanhTienQuyDoi = 0;
                            detail.ThanhTienSauThue = 0;
                            detail.ThanhTienSauThueQuyDoi = 0;
                            detail.TyLeChietKhau = 0;
                            detail.TienChietKhau = 0;
                            detail.TienChietKhauQuyDoi = 0;
                            detail.TienThueGTGT = 0;
                            detail.TienThueGTGTQuyDoi = 0;
                        }
                    }

                    detail.DonGiaSauThue = detail.DonGiaSauThue ?? 0;
                    detail.ThanhTienSauThue = detail.ThanhTienSauThue ?? 0;

                    if (item.IsVND == true)
                    {
                        detail.ThanhTienQuyDoi = detail.ThanhTien;
                        detail.TienChietKhauQuyDoi = detail.TienChietKhau;
                        detail.TienThueGTGTQuyDoi = detail.TienThueGTGT;
                        detail.ThanhTienSauThueQuyDoi = detail.ThanhTienSauThue;
                        detail.TienGiamQuyDoi = detail.TienGiam;
                    }

                    detail.TongTienThanhToan = detail.ThanhTien - detail.TienChietKhau - detail.TienGiam + detail.TienThueGTGT;
                    detail.TongTienThanhToanQuyDoi = detail.ThanhTienQuyDoi - detail.TienChietKhauQuyDoi - detail.TienGiamQuyDoi + detail.TienThueGTGTQuyDoi;
                }

                var listToSum = item.HoaDonChiTiets.Where(x => x.TinhChat == 1 || x.TinhChat == 3).ToList();
                var listToSum2 = item.HoaDonChiTiets.Where(x => x.TinhChat == 1).ToList();

                item.TongTienHang = listToSum.Sum(x => x.TinhChat == 1 ? x.ThanhTien : (-x.ThanhTien));
                item.TongTienHangQuyDoi = listToSum.Sum(x => x.TinhChat == 1 ? x.ThanhTienQuyDoi : (-x.ThanhTienQuyDoi));
                item.TongTienChietKhau = listToSum2.Sum(x => x.TienChietKhau);
                item.TongTienChietKhauQuyDoi = listToSum2.Sum(x => x.TienChietKhauQuyDoi);
                item.TongTienThueGTGT = listToSum.Sum(x => x.TinhChat == 1 ? x.TienThueGTGT : (-x.TienThueGTGT));
                item.TongTienThueGTGTQuyDoi = listToSum.Sum(x => x.TinhChat == 1 ? x.TienThueGTGTQuyDoi : (-x.TienThueGTGTQuyDoi));
                item.TongTienGiam = listToSum2.Sum(x => x.TienGiam);
                item.TongTienGiamQuyDoi = listToSum2.Sum(x => x.TienGiamQuyDoi);
                item.TongTienThanhToan = item.TongTienHang - item.TongTienChietKhau - item.TongTienGiam + item.TongTienThueGTGT;
                item.TongTienThanhToanQuyDoi = item.TongTienHangQuyDoi - item.TongTienChietKhauQuyDoi - item.TongTienGiamQuyDoi + item.TongTienThueGTGTQuyDoi;
                item.TyLeChietKhau = 0;

                if (item.TongTienHang != 0)
                {
                    item.TyLeChietKhau = (item.TongTienChietKhau * 100 / item.TongTienHang).Value.MathRoundNumberByTuyChon(tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                }

                var entity = _mp.Map<HoaDonDienTu>(item);
                addedHDDTList.Add(entity);
            }

            await _db.DoiTuongs.AddRangeAsync(addedDoiTuongList);
            await _db.HangHoaDichVus.AddRangeAsync(addedHHDVList);
            await _db.DonViTinhs.AddRangeAsync(addedDVTList);
            await _db.HoaDonDienTus.AddRangeAsync(addedHDDTList);
            var reuslt = await _db.SaveChangesAsync();
            return reuslt > 0;
        }

        public FileReturn CreateFileImportHoaDonError(NhapKhauResult result)
        {
            // Export excel
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/temp");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string excelFileName = $"hoa-don-error-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            string excelFolder = $"FilesUpload/temp/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            var list = result.ListResult;
            var loaiHoaDon = list[0].LoaiHoaDon;

            // Excel
            string _sample = $"docs/Template/ImportHoaDon/{(loaiHoaDon == 1 ? "Hoa_Don_Gia_Tri_Gia_Tang_Import" : "Hoa_Don_Ban_Hang_Import")}.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int numCol = result.ListTruongDuLieu.Max(x => x.ColIndex);

                worksheet.InsertColumn(numCol + 1, 1, numCol);
                worksheet.Cells[15, numCol + 1].Value = "";
                worksheet.Cells[16, numCol + 1].Value = "Thông tin kỹ thuật";
                worksheet.Column(numCol + 1).Width = 50;

                int begin_row = 17;
                int idx = begin_row;
                int iTemp = begin_row;

                for (int i = 3; i <= numCol; i++)
                {
                    var truongDL = result.ListTruongDuLieu.FirstOrDefault(x => x.ColIndex == i);
                    if (truongDL != null)
                    {
                        worksheet.Cells[15, i].Value = truongDL.TenEnum;
                        worksheet.Cells[16, i].Value = truongDL.TenTruongExcel;
                    }
                }

                foreach (var item in list)
                {
                    item.Row = iTemp;
                    iTemp += 1;
                }

                List<HoaDonDienTuImport> listError = new List<HoaDonDienTuImport>();
                foreach (var item in list)
                {
                    worksheet.Cells[idx, 1].Value = item.HoaDonChiTiet.TinhChat;
                    worksheet.Cells[idx, 2].Value = item.STT;

                    for (int j = 3; j <= numCol + 1; j++)
                    {
                        var truongDL = result.ListTruongDuLieu.FirstOrDefault(x => x.ColIndex == j);

                        if (j != (numCol + 1))
                        {
                            switch (truongDL.Ma)
                            {
                                case MaTruongDLHDExcel.NVBANHANG:
                                    worksheet.Cells[idx, j].Value = item.MaNhanVienBanHang;
                                    break;
                                case MaTruongDLHDExcel.NGAYHOADON:
                                    worksheet.Cells[idx, j].Value = item.NgayHoaDon.Value.ToString("dd/MM/yyyy");
                                    break;
                                case MaTruongDLHDExcel.NM1:
                                    worksheet.Cells[idx, j].Value = item.HoTenNguoiMuaHang;
                                    break;
                                case MaTruongDLHDExcel.NM2:
                                    break;
                                case MaTruongDLHDExcel.NM3:
                                    worksheet.Cells[idx, j].Value = item.TenKhachHang;
                                    break;
                                case MaTruongDLHDExcel.NM4:
                                    worksheet.Cells[idx, j].Value = item.DiaChi;
                                    break;
                                case MaTruongDLHDExcel.NM5:
                                    worksheet.Cells[idx, j].Value = item.MaSoThue;
                                    break;
                                case MaTruongDLHDExcel.NM6:
                                    worksheet.Cells[idx, j].Value = item.HinhThucThanhToanId;
                                    break;
                                case MaTruongDLHDExcel.NM7:
                                    worksheet.Cells[idx, j].Value = item.EmailNguoiMuaHang;
                                    break;
                                case MaTruongDLHDExcel.NM8:
                                    break;
                                case MaTruongDLHDExcel.NM9:
                                    worksheet.Cells[idx, j].Value = item.SoTaiKhoanNganHang;
                                    break;
                                case MaTruongDLHDExcel.NM10:
                                    worksheet.Cells[idx, j].Value = item.TenNganHang;
                                    break;
                                case MaTruongDLHDExcel.LOAITIEN:
                                    worksheet.Cells[idx, j].Value = item.MaLoaiTien;
                                    break;
                                case MaTruongDLHDExcel.TYGIA:
                                    worksheet.Cells[idx, j].Value = item.TyGia;
                                    break;
                                case MaTruongDLHDExcel.HHDV2:
                                    break;
                                case MaTruongDLHDExcel.HHDV3:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.TenHang;
                                    break;
                                case MaTruongDLHDExcel.HHDV4:
                                    break;
                                case MaTruongDLHDExcel.HHDV5:
                                    break;
                                case MaTruongDLHDExcel.HHDV6:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.TenDonViTinh;
                                    break;
                                case MaTruongDLHDExcel.HHDV7:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.SoLuong;
                                    break;
                                case MaTruongDLHDExcel.HHDV9:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.DonGia;
                                    break;
                                case MaTruongDLHDExcel.HHDV11:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.ThanhTien;
                                    break;
                                case MaTruongDLHDExcel.HHDV12:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.ThanhTienQuyDoi;
                                    break;
                                case MaTruongDLHDExcel.HHDV13:
                                    break;
                                case MaTruongDLHDExcel.HHDV14:
                                    break;
                                case MaTruongDLHDExcel.HHDV15:
                                    break;
                                case MaTruongDLHDExcel.HHDV16:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.ThueGTGT;
                                    break;
                                case MaTruongDLHDExcel.HHDV17:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.TienThueGTGT;
                                    break;
                                case MaTruongDLHDExcel.HHDV18:
                                    worksheet.Cells[idx, j].Value = item.HoaDonChiTiet.TienThueGTGTQuyDoi;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            var errorItem = list.FirstOrDefault(x => x.STT == item.STT && x.IsMainError == true);
                            if (errorItem != null && !listError.Any(x => x.STT == item.STT))
                            {
                                listError.Add(errorItem);
                            }

                            if (listError.Any(x => x.STT == item.STT) && item.IsMainError != true)
                            {
                                var errorItem2 = listError.FirstOrDefault(x => x.STT == item.STT);
                                item.ErrorMessage = $"Dòng chi tiết liên quan (dòng số <{errorItem2.Row}>) bị lỗi.";
                            }

                            worksheet.Cells[idx, j].Value = item.ErrorMessage;
                            worksheet.Cells[idx, j].Style.Font.Color.SetColor(Color.Red);
                        }
                    }

                    idx += 1;
                }

                package.SaveAs(new FileInfo(excelPath));
            }

            byte[] bytes = File.ReadAllBytes(excelPath);
            File.Delete(excelPath);

            return new FileReturn
            {
                Bytes = bytes,
                ContentType = MimeTypes.GetMimeType(excelPath),
                FileName = Path.GetFileName(excelPath),
            };
        }

        public string GetNgayHienTai()
        {
            //API này lấy ngày hiện tại của hệ thống
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public async Task<ReloadXmlResult> ReloadXMLAsync(ReloadXmlParams @params)
        {
            using (var reader = new StreamReader(@params.File.OpenReadStream()))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                string KHMSHDon = doc.SelectSingleNode("/TDiep/DLieu/HDon/DLHDon/TTChung/KHMSHDon").InnerText;
                string KHHDon = doc.SelectSingleNode("/TDiep/DLieu/HDon/DLHDon/TTChung/KHHDon").InnerText;
                string SHDon = doc.SelectSingleNode("/TDiep/DLieu/HDon/DLHDon/TTChung/SHDon").InnerText;
                string kyHieu = KHMSHDon + KHHDon;
                string fileName = $"{kyHieu}-{SHDon}-{Guid.NewGuid()}.xml";
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, fileName);

                var boKyHieuHoaDon = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.KyHieu == kyHieu);
                if (boKyHieuHoaDon != null)
                {
                    var hoaDonDienTu = await _db.HoaDonDienTus
                        .FirstOrDefaultAsync(x => x.SoHoaDon == long.Parse(SHDon) && x.BoKyHieuHoaDonId == boKyHieuHoaDon.BoKyHieuHoaDonId);

                    if (hoaDonDienTu != null)
                    {
                        hoaDonDienTu.XMLDaKy = fileName;
                        doc.Save(filePath);

                        var fileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == hoaDonDienTu.HoaDonDienTuId && x.Type == 1 && x.IsSigned == true);
                        if (fileData == null)
                        {
                            fileData = new FileData
                            {
                                RefId = hoaDonDienTu.HoaDonDienTuId,
                                Type = 1,
                                DateTime = DateTime.Now,
                                Binary = File.ReadAllBytes(filePath),
                                FileName = fileName,
                                IsSigned = true
                            };

                            await _db.FileDatas.AddAsync(fileData);
                        }
                        else
                        {
                            fileData.Binary = File.ReadAllBytes(filePath);
                            fileData.FileName = fileName;
                        }

                        var result = await _db.SaveChangesAsync();
                        return new ReloadXmlResult
                        {
                            Status = result > 0
                        };
                    }
                    else
                    {
                        return new ReloadXmlResult
                        {
                            Status = false,
                            Message = "Bộ ký hiệu không tồn tại."
                        };
                    }
                }
                else
                {
                    return new ReloadXmlResult
                    {
                        Status = false,
                        Message = "Bộ ký hiệu không tồn tại."
                    };
                }
            }
        }

        /// <summary>
        /// KiemTraHoaDonDaLapTBaoCoSaiSot kiểm tra hóa đơn đã lập thông báo có sai sót 04 hay chưa
        /// </summary>
        /// <param name="hoaDonDienTuId"></param>
        /// <returns></returns>
        public async Task<KetQuaKiemTraLapTBao04ViewModel> KiemTraHoaDonDaLapTBaoCoSaiSotAsync(string hoaDonDienTuId)
        {
            var listHoaDon = await _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hoaDonDienTuId || x.DieuChinhChoHoaDonId == hoaDonDienTuId).ToListAsync();
            var hoaDon = listHoaDon.FirstOrDefault(x => x.HoaDonDienTuId == hoaDonDienTuId);
            ThongTinHoaDonRutGonViewModel hoaDonDieuChinh = null;

            if (hoaDon != null)
            {
                var valid = false;
                for (var i = 0; i < 1; i++)
                {
                    if (string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId)) //nếu là hóa đơn gốc
                    {
                        /* theo yêu cầu thì sẽ bỏ trường hợp này
                        if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1
                           || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4
                           || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc6
                           // bỏ điều kiện này || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2
                           || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                        { //nếu là hủy do sai sót
                            valid = true;
                        }
                        */

                        //nếu ko phải lập lại hóa đơn
                        if (!valid && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                        {
                            valid = true;
                        }

                        //nếu là hóa đơn gốc bị điều chỉnh
                        if (!valid && listHoaDon.Count(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId) > 0)
                        {
                            var hoaDonDieuChinhTemp = listHoaDon.Where(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(x => x.CreatedDate)?.Take(1)?.FirstOrDefault();
                            if (hoaDonDieuChinhTemp != null)
                            {
                                var boKyHieuHoaDon = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == hoaDonDieuChinhTemp.BoKyHieuHoaDonId);
                                hoaDonDieuChinh = new ThongTinHoaDonRutGonViewModel
                                {
                                    MauSoHoaDon = boKyHieuHoaDon.KyHieuMauSoHoaDon.ToString(),
                                    KyHieuHoaDon = boKyHieuHoaDon.KyHieuHoaDon ?? "",
                                    SoHoaDon = hoaDonDieuChinhTemp.SoHoaDon + "",
                                    NgayHoaDon = hoaDonDieuChinhTemp.NgayHoaDon?.ToString("yyyy-MM-dd")
                                };
                            }

                            valid = true;
                        }
                        break;
                    }

                    //nếu là hóa đơn thay thế
                    if (!string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId))
                    {
                        /* theo yêu cầu bỏ điều kiện này
                        if ( // bỏ điều kiện này hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5 ||
                            hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                        {
                            valid = true;
                        }
                        */

                        //nếu ko phải lập lại hóa đơn
                        if (!valid && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                        {
                            valid = true;
                        }
                        break;
                    }

                    //nếu là hóa đơn điều chỉnh
                    if (!string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))
                    {
                        //nếu ko phải lập lại hóa đơn
                        if (hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                        {
                            valid = true;
                        }
                        break;
                    }
                }

                if (valid == false)
                {
                    return new KetQuaKiemTraLapTBao04ViewModel
                    {
                        IsDaGuiThongBao = true,
                        IsDaLapThongBao = true,
                        HoaDonDieuChinh = hoaDonDieuChinh
                    };
                    //Ghi chú: IsDaGuiThongBao, IsDaLapThongBao = true để không bị thông báo nữa
                }
                else
                {
                    return new KetQuaKiemTraLapTBao04ViewModel
                    {
                        IsDaGuiThongBao = (hoaDon.TrangThaiGui04.GetValueOrDefault() > (int)TrangThaiGuiThongDiep.ChuaGui),
                        IsDaLapThongBao = (hoaDon.IsDaLapThongBao04.GetValueOrDefault() == true),
                        HoaDonDieuChinh = hoaDonDieuChinh
                    };
                }
            }

            return null;
        }

        //Method này để hiển thị dữ liệu ở cột thông báo sai sót (đối với hóa đơn được nhập từ phần mềm khác)
        private CotThongBaoSaiSotViewModel GetCotThongBaoSaiSotHoaDon32(ThongTinHoaDon thongTinHoaDon, HoaDonDienTu hoaDon, string phanLoaiKiemTraHoaDon, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon, List<DLL.Entity.QuanLy.BoKyHieuHoaDon> queryBoKyHieuHoaDon = null)
        {
            //phanLoaiKiemTraHoaDon: thayThe = hiển thị ở tab thay thế; dieuChinh = hiển thị ở tab điều chỉnh
            if (boKyHieuHoaDon == null)
            {
                if (queryBoKyHieuHoaDon != null && hoaDon != null) //với hóa đơn 32 thì xác định bộ ký hiệu qua queryBoKyHieuHoaDon
                {
                    boKyHieuHoaDon = queryBoKyHieuHoaDon.FirstOrDefault(x => x.BoKyHieuHoaDonId == hoaDon.BoKyHieuHoaDonId);
                }
            }

            //nếu phanLoaiKiemTraHoaDon == "dieuChinh" thì dùng để kiểm tra cho hóa đơn điều chỉnh
            if (phanLoaiKiemTraHoaDon == "dieuChinh")
            {
                if (thongTinHoaDon != null)
                {
                    if (hoaDon.DieuChinhChoHoaDonId == thongTinHoaDon.Id)
                    {
                        if (thongTinHoaDon.IsDaLapThongBao04.GetValueOrDefault()) //nếu đã lập thông báo 04
                        {
                            if (thongTinHoaDon.TrangThaiGui04.GetValueOrDefault() == (int)TrangThaiGuiThongDiep.ChuaGui || thongTinHoaDon.TrangThaiGui04 == null) //nếu là chưa gửi
                            {
                                var dienGiaiChiTietTrangThai = "";
                                if (thongTinHoaDon.TrangThaiHoaDon.GetValueOrDefault() == 1) //hóa đơn gốc
                                {
                                    dienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc bị điều chỉnh";
                                }
                                else if (thongTinHoaDon.TrangThaiHoaDon.GetValueOrDefault() == 4) //hóa đơn điều chỉnh
                                {
                                    dienGiaiChiTietTrangThai = "&nbsp;|&nbsp;HĐ điều chỉnh bị điều chỉnh";
                                }

                                return new CotThongBaoSaiSotViewModel
                                {
                                    ThongDiepGuiCQTId = thongTinHoaDon.ThongDiepGuiCQTId,
                                    TrangThaiLapVaGuiThongBao = (int)TrangThaiGuiThongDiep.ChuaGui, //chưa gửi thông báo
                                    DienGiaiChiTietTrangThai = dienGiaiChiTietTrangThai,
                                    TenTrangThai = TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                                    //IsTrongHan = XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon),
                                    IsHoaDonNgoaiHeThong = true
                                };
                            }
                            else //nếu là đã gửi
                            {
                                //đã gửi thì có định dạng là Lần gửi | trạng thái gửi | trong hạn/quá hạn
                                TrangThaiGuiThongDiep trangThaiGuiThongDiep = (TrangThaiGuiThongDiep)thongTinHoaDon.TrangThaiGui04.GetValueOrDefault();

                                var dienGiaiTrangThaiGui = "";
                                if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHDKhongHopLe)
                                {
                                    dienGiaiTrangThaiGui = "Hóa đơn không hợp lệ";
                                }
                                else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan)
                                {
                                    dienGiaiTrangThaiGui = "CQT không tiếp nhận";
                                }
                                else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon)
                                {
                                    dienGiaiTrangThaiGui = "CQT đã tiếp nhận";
                                }
                                else
                                {
                                    dienGiaiTrangThaiGui = trangThaiGuiThongDiep.GetDescription();
                                }

                                return new CotThongBaoSaiSotViewModel
                                {
                                    ThongDiepGuiCQTId = thongTinHoaDon.ThongDiepGuiCQTId,
                                    TrangThaiLapVaGuiThongBao = thongTinHoaDon.TrangThaiGui04.GetValueOrDefault(),
                                    DienGiaiChiTietTrangThai = dienGiaiTrangThaiGui,
                                    TenTrangThai = dienGiaiTrangThaiGui,
                                    LanGui = "Lần gửi " + thongTinHoaDon.LanGui04.GetValueOrDefault().ToString(),
                                    //IsTrongHan = (thongTinHoaDon.TrangThaiGui04.GetValueOrDefault() > -1) ? true : false,
                                    IsHoaDonNgoaiHeThong = true
                                    //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                                };
                            }
                        }
                        else //nếu chưa lập thông báo 04
                        {
                            //kiểm tra xem hóa đơn điều chỉnh đã được cấp mã hay chưa
                            var daDuocCapMa = false;
                            if (hoaDon != null)
                            {
                                daDuocCapMa = (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDon.SoHoaDon.HasValue);
                            }

                            if (thongTinHoaDon.TrangThaiHoaDon.GetValueOrDefault() == 1) //hóa đơn gốc
                            {
                                if (daDuocCapMa)
                                {
                                    return new CotThongBaoSaiSotViewModel
                                    {
                                        HoaDonDienTuId = thongTinHoaDon.Id,
                                        TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                        TenTrangThai = "Chưa lập thông báo",
                                        DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc bị điều chỉnh",
                                        //IsTrongHan = XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon),
                                        IsHoaDonNgoaiHeThong = true
                                    };
                                }
                                else
                                {
                                    return null;
                                }

                                /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                                return new CotThongBaoSaiSotViewModel
                                {
                                    HoaDonDienTuId = thongTinHoaDon.Id,
                                    TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                    TenTrangThai = "Chưa lập thông báo",
                                    DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc bị điều chỉnh",
                                    IsTrongHan = ((daDuocCapMa) ? XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon) : null),
                                    IsHoaDonNgoaiHeThong = true
                                };
                                */
                            }
                            else if (thongTinHoaDon.TrangThaiHoaDon.GetValueOrDefault() == 4) //hóa đơn điều chỉnh
                            {
                                if (daDuocCapMa)
                                {
                                    return new CotThongBaoSaiSotViewModel
                                    {
                                        HoaDonDienTuId = thongTinHoaDon.Id,
                                        TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                        TenTrangThai = "Chưa lập thông báo",
                                        DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;HĐ điều chỉnh bị điều chỉnh",
                                        //IsTrongHan = XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon),
                                        IsHoaDonNgoaiHeThong = true
                                    };
                                }
                                else
                                {
                                    return null;
                                }

                                /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                                //thực tế sẽ ko có trường hợp HĐ điều chỉnh bị điều chỉnh
                                return new CotThongBaoSaiSotViewModel
                                {
                                    HoaDonDienTuId = thongTinHoaDon.Id,
                                    TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                    TenTrangThai = "Chưa lập thông báo",
                                    DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;HĐ điều chỉnh bị điều chỉnh",
                                    IsTrongHan = ((daDuocCapMa) ? XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon) : null),
                                    IsHoaDonNgoaiHeThong = true
                                };
                                */
                            }
                        }
                    }
                }
            }
            else
            {
                //đối với hóa đơn bị thay thế thì chỉ cần kiểm tra hóa đơn bị thay thế
                if (thongTinHoaDon.IsDaLapThongBao04.GetValueOrDefault()) //nếu đã lập thông báo 04
                {
                    if (thongTinHoaDon.TrangThaiGui04.GetValueOrDefault() == (int)TrangThaiGuiThongDiep.ChuaGui || thongTinHoaDon.TrangThaiGui04 == null) //nếu là chưa gửi
                    {

                        return new CotThongBaoSaiSotViewModel
                        {
                            ThongDiepGuiCQTId = thongTinHoaDon.ThongDiepGuiCQTId,
                            TrangThaiLapVaGuiThongBao = (int)TrangThaiGuiThongDiep.ChuaGui, //chưa gửi thông báo
                            DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới", //diễn giải đều giống nhau cả
                            TenTrangThai = TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                            //IsTrongHan = XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon),
                            IsHoaDonNgoaiHeThong = true
                        };
                    }
                    else //nếu là đã gửi
                    {
                        //đã gửi thì có định dạng là Lần gửi | trạng thái gửi | trong hạn/quá hạn
                        TrangThaiGuiThongDiep trangThaiGuiThongDiep = (TrangThaiGuiThongDiep)thongTinHoaDon.TrangThaiGui04.GetValueOrDefault();

                        var dienGiaiTrangThaiGui = "";
                        if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHDKhongHopLe)
                        {
                            dienGiaiTrangThaiGui = "Hóa đơn không hợp lệ";
                        }
                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan)
                        {
                            dienGiaiTrangThaiGui = "CQT không tiếp nhận";
                        }
                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon)
                        {
                            dienGiaiTrangThaiGui = "CQT đã tiếp nhận";
                        }
                        else
                        {
                            dienGiaiTrangThaiGui = trangThaiGuiThongDiep.GetDescription();
                        }

                        return new CotThongBaoSaiSotViewModel
                        {
                            ThongDiepGuiCQTId = thongTinHoaDon.ThongDiepGuiCQTId,
                            TrangThaiLapVaGuiThongBao = thongTinHoaDon.TrangThaiGui04.GetValueOrDefault(),
                            DienGiaiChiTietTrangThai = dienGiaiTrangThaiGui,
                            TenTrangThai = dienGiaiTrangThaiGui,
                            LanGui = "Lần gửi " + thongTinHoaDon.LanGui04.GetValueOrDefault().ToString(),
                            //IsTrongHan = (thongTinHoaDon.TrangThaiGui04.GetValueOrDefault() > -1) ? true : false,
                            IsHoaDonNgoaiHeThong = true
                            //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                        };
                    }
                }
                else //nếu chưa lập thông báo 04
                {
                    //kiểm tra xem hóa đơn thay thế đã được cấp mã hay chưa
                    var daDuocCapMa = false;
                    if (hoaDon != null)
                    {
                        daDuocCapMa = (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDon.SoHoaDon.HasValue);
                    }

                    if (thongTinHoaDon.TrangThaiHoaDon.GetValueOrDefault() == 1) //hóa đơn gốc
                    {
                        if (daDuocCapMa)
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                HoaDonDienTuId = thongTinHoaDon.Id,
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới",
                                //IsTrongHan = XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon),
                                IsHoaDonNgoaiHeThong = true
                            };
                        }
                        else
                        {
                            return null;
                        }

                        /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                        return new CotThongBaoSaiSotViewModel
                        {
                            HoaDonDienTuId = thongTinHoaDon.Id,
                            TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                            TenTrangThai = "Chưa lập thông báo",
                            DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới",
                            IsTrongHan = ((daDuocCapMa) ? XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon) : null),
                            IsHoaDonNgoaiHeThong = true
                        };
                        */
                    }
                    else if (thongTinHoaDon.TrangThaiHoaDon.GetValueOrDefault() == 3) //hóa đơn thay thế
                    {
                        if (daDuocCapMa)
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                HoaDonDienTuId = thongTinHoaDon.Id,
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới",
                                //IsTrongHan = XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon),
                                IsHoaDonNgoaiHeThong = true
                            };
                        }
                        else
                        {
                            return null;
                        }

                        /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                        return new CotThongBaoSaiSotViewModel
                        {
                            HoaDonDienTuId = thongTinHoaDon.Id,
                            TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                            TenTrangThai = "Chưa lập thông báo",
                            DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới",
                            IsTrongHan = ((daDuocCapMa) ? XacDinhTrongHanHoaDon32(tuyChonKyKeKhai, thongTinHoaDon, hoaDon, bienBanXoaBo, boKyHieuHoaDon) : null),
                            IsHoaDonNgoaiHeThong = true
                        };
                        */
                    }
                }
            }

            return null;
        }

        //Method này để hiển thị dữ liệu ở cột thông báo sai sót (đối với hóa đơn hệ thống)
        private CotThongBaoSaiSotViewModel GetCotThongBaoSaiSot(string tuyChonKyKeKhai, HoaDonDienTu hoaDonParam, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon, List<HoaDonDienTu> listHoaDonDienTu, ThongTinHoaDon thongTinHoaDon)
        {
            HoaDonDienTu hoaDon = hoaDonParam;
            //Kiểm tra hóa đơn điều chỉnh cho hóa đơn được nhập từ phần mềm khác
            //Nếu là điều chỉnh cho hóa đơn được nhập từ phần mềm khác thì trả về thông tin sai sót luôn (nếu có != null)
            if (!string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))
            {
                var thongTinSaiSot = GetCotThongBaoSaiSotHoaDon32(thongTinHoaDon, hoaDon, "dieuChinh", boKyHieuHoaDon);
                if (thongTinSaiSot != null)
                {
                    return thongTinSaiSot;
                }
            }

            if (string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId)
                && hoaDon.HinhThucXoabo == null && listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId) == 0 && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD == null)
            {
                //nếu là hóa đơn gốc chưa bị xóa bỏ, chưa bị điều chỉnh, chưa gửi thông báo sai sót cho khách hàng
                return null;
            }

            if (!string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId)
                && hoaDon.HinhThucXoabo == null && listHoaDonDienTu.Count(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId) == 0 && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD == null)
            {
                //nếu là hóa đơn thay thế chưa bị xóa bỏ, chưa bị thay thế, chưa gửi thông báo sai sót cho khách hàng
                return null;
            }

            //nếu là hóa đơn gốc bị điều chỉnh và ko có gửi email sai thông tin thì sẽ ko hiển thị thông tin gì cả
            //vì dòng thông tin sẽ hiển thị ở hóa đơn điều chỉnh
            if (string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId)
                && listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId) > 0
                && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId)) //nếu không phải là hóa đơn điều chỉnh
            {
                if (hoaDon.IsDaLapThongBao04 != true) //nếu chưa lập thông báo 04
                {
                    //nếu là hóa đơn gốc
                    if (string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId)
                        && string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))
                    {
                        if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1
                        || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4
                        || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc6)
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hủy do sai sót",
                                //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                            };
                        }

                        if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2)
                        {
                            //kiểm tra xem hóa đơn thay thế đã được cấp mã hay chưa
                            var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
                            var daDuocCapMa = false;
                            if (hoaDonThayThe != null)
                            {
                                daDuocCapMa = (hoaDonThayThe.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDonThayThe.SoHoaDon.HasValue);
                            }

                            if (daDuocCapMa)
                            {
                                //nếu là hóa đơn gốc chọn hình thức xóa bỏ là HinhThuc2
                                return new CotThongBaoSaiSotViewModel
                                {
                                    TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                    TenTrangThai = "Chưa lập thông báo",
                                    DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế",
                                    //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                                };
                            }
                            else
                            {
                                return null;
                            }

                            /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                            //nếu là hóa đơn gốc chọn hình thức xóa bỏ là HinhThuc2
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế",
                                IsTrongHan = ((daDuocCapMa)? XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu): null)
                            };
                            */
                        }

                        if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                        {
                            //nếu là hóa đơn gốc chọn hình thức xóa bỏ là HinhThuc3
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hủy theo lý do phát sinh",
                                //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                            };
                        }

                        if (hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Không phải lập lại hóa đơn",
                                IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu),
                                IsCoGuiEmailSaiThongTin = true
                            };
                        }
                    }

                    //nếu là hóa đơn thay thế
                    if (!string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId))
                    {
                        if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5)
                        {
                            //kiểm tra xem hóa đơn thay thế đã được cấp mã hay chưa
                            var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
                            var daDuocCapMa = false;
                            if (hoaDonThayThe != null)
                            {
                                daDuocCapMa = (hoaDonThayThe.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDonThayThe.SoHoaDon.HasValue);
                            }

                            if (daDuocCapMa)
                            {
                                return new CotThongBaoSaiSotViewModel
                                {
                                    TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                    TenTrangThai = "Chưa lập thông báo",
                                    DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới",
                                    //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                                };
                            }
                            else
                            {
                                return null;
                            }

                            /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                            //nếu là hóa đơn gốc chọn hình thức xóa bỏ là HinhThuc5
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Xóa để lập thay thế mới",
                                IsTrongHan = ((daDuocCapMa) ? XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu) : null)
                            };
                            */
                        }

                        if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                        {
                            //nếu là hóa đơn gốc chọn hình thức xóa bỏ là HinhThuc3
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hủy theo lý do phát sinh",
                                //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                            };
                        }

                        if (hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Không phải lập lại hóa đơn",
                                IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                            };
                        }
                    }
                }
                else //nếu đã lập thông báo 04
                {
                    //kiểm tra chỉ hiển thị tình trạng với trường hợp gửi thông báo không phải lập lại hóa đơn
                    var hienThiTinhTrang = (GetDienGiaiChiTietTrangThai(hoaDon, null) == "&nbsp;|&nbsp;Không phải lập lại hóa đơn");

                    if (hoaDon.TrangThaiGui04.GetValueOrDefault() == (int)TrangThaiGuiThongDiep.ChuaGui || hoaDon.TrangThaiGui04 == null) //nếu là chưa gửi
                    {
                        return new CotThongBaoSaiSotViewModel
                        {
                            TrangThaiLapVaGuiThongBao = (int)TrangThaiGuiThongDiep.ChuaGui, //chưa gửi thông báo
                            TenTrangThai = TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                            DienGiaiChiTietTrangThai = GetDienGiaiChiTietTrangThai(hoaDon, null),
                            IsTrongHan = (hienThiTinhTrang) ? XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu) : null
                        };
                    }
                    else //nếu là đã gửi
                    {
                        //đã gửi thì có định dạng là Lần gửi | trạng thái gửi | trong hạn/quá hạn
                        TrangThaiGuiThongDiep trangThaiGuiThongDiep = (TrangThaiGuiThongDiep)hoaDon.TrangThaiGui04.GetValueOrDefault();

                        var dienGiaiTrangThaiGui = "";
                        if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHDKhongHopLe || trangThaiGuiThongDiep == TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe)
                        {
                            dienGiaiTrangThaiGui = "Hóa đơn không hợp lệ";
                        }
                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan)
                        {
                            dienGiaiTrangThaiGui = "CQT không tiếp nhận";
                        }
                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon)
                        {
                            dienGiaiTrangThaiGui = "CQT đã tiếp nhận";
                        }
                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.GoiDuLieuHopLe)
                        {
                            dienGiaiTrangThaiGui = "Hóa đơn hợp lệ";
                        }
                        else
                        {
                            dienGiaiTrangThaiGui = trangThaiGuiThongDiep.GetDescription();
                        }

                        return new CotThongBaoSaiSotViewModel
                        {
                            TrangThaiLapVaGuiThongBao = hoaDon.TrangThaiGui04.GetValueOrDefault(),
                            TenTrangThai = dienGiaiTrangThaiGui,
                            DienGiaiChiTietTrangThai = dienGiaiTrangThaiGui,
                            LanGui = "Lần gửi " + hoaDon.LanGui04.GetValueOrDefault().ToString(),
                            IsTrongHan = (hienThiTinhTrang) ? ((bool?)((hoaDon.TrangThaiGui04.GetValueOrDefault() > -1))) : null
                            //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                        };
                    }
                }
            }
            else
            {
                //nếu là hóa đơn điều chỉnh
                //bước 1: kiểm tra 04 của hóa đơn điều chỉnh, nếu nó có thì không hiển thị 04 của hóa đơn bị điều chỉnh nữa
                if (hoaDon.IsDaLapThongBao04 == true || hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                {
                    if (hoaDon.IsDaLapThongBao04 == true)
                    {
                        //kiểm tra chỉ hiển thị tình trạng với trường hợp gửi thông báo không phải lập lại hóa đơn
                        var hienThiTinhTrang = (GetDienGiaiChiTietTrangThai(hoaDon, null) == "&nbsp;|&nbsp;Không phải lập lại hóa đơn");

                        if (hoaDon.TrangThaiGui04.GetValueOrDefault() == (int)TrangThaiGuiThongDiep.ChuaGui || hoaDon.TrangThaiGui04 == null) //nếu là chưa gửi
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = (int)TrangThaiGuiThongDiep.ChuaGui, //chưa gửi thông báo
                                DienGiaiChiTietTrangThai = GetDienGiaiChiTietTrangThai(hoaDon, null),
                                TenTrangThai = TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                                IsTrongHan = (hienThiTinhTrang) ? XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu) : null
                            };
                        }
                        else //nếu là đã gửi
                        {
                            //đã gửi thì có định dạng là Lần gửi | trạng thái gửi | trong hạn/quá hạn
                            TrangThaiGuiThongDiep trangThaiGuiThongDiep = (TrangThaiGuiThongDiep)hoaDon.TrangThaiGui04.GetValueOrDefault();

                            var dienGiaiTrangThaiGui = "";
                            if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHDKhongHopLe || trangThaiGuiThongDiep == TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe)
                            {
                                dienGiaiTrangThaiGui = "Hóa đơn không hợp lệ";
                            }
                            else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan)
                            {
                                dienGiaiTrangThaiGui = "CQT không tiếp nhận";
                            }
                            else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon)
                            {
                                dienGiaiTrangThaiGui = "CQT đã tiếp nhận";
                            }
                            else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.GoiDuLieuHopLe)
                            {
                                dienGiaiTrangThaiGui = "Hóa đơn hợp lệ";
                            }
                            else
                            {
                                dienGiaiTrangThaiGui = trangThaiGuiThongDiep.GetDescription();
                            }

                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = hoaDon.TrangThaiGui04.GetValueOrDefault(),
                                TenTrangThai = dienGiaiTrangThaiGui,
                                DienGiaiChiTietTrangThai = dienGiaiTrangThaiGui,
                                LanGui = "Lần gửi " + hoaDon.LanGui04.GetValueOrDefault().ToString(),
                                IsTrongHan = (hienThiTinhTrang) ? ((bool?)((hoaDon.TrangThaiGui04.GetValueOrDefault() > -1))) : null
                                //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu)
                            };
                        }
                    }

                    if (hoaDon.IsDaLapThongBao04 != true)
                    {
                        if (hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                        {
                            return new CotThongBaoSaiSotViewModel
                            {
                                TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                TenTrangThai = "Chưa lập thông báo",
                                DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Không phải lập lại hóa đơn",
                                IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu),
                                IsCoGuiEmailSaiThongTin = true
                            };
                        }
                    }
                }
                else
                {
                    //nếu như chưa có 04 của hóa đơn điều chỉnh thì
                    //kiểm tra đến hóa đơn gốc bị điều chỉnh

                    //truy ra hóa đơn bị điều chỉnh của nó là hóa đơn nào để hiển thị thông tin
                    //nếu hóa đơn điều chỉnh thỏa mãn điều kiện thì mới hiện ra thôn tin
                    if ((hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa) || (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa))
                    {
                        var hoaDonBiDieuChinh = listHoaDonDienTu.FirstOrDefault(x => x.HoaDonDienTuId == hoaDon.DieuChinhChoHoaDonId);
                        if (hoaDonBiDieuChinh != null) //nếu có hóa đơn bị điều chỉnh thì hiện ra thông tin
                        {
                            //nếu là hóa đơn gốc
                            if (string.IsNullOrWhiteSpace(hoaDonBiDieuChinh.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hoaDonBiDieuChinh.DieuChinhChoHoaDonId))
                            {
                                if (hoaDonBiDieuChinh.IsDaLapThongBao04 == true)
                                {
                                    //kiểm tra chỉ hiển thị tình trạng với trường hợp gửi thông báo không phải lập lại hóa đơn
                                    var hienThiTinhTrang = (GetDienGiaiChiTietTrangThai(null, hoaDonBiDieuChinh) == "&nbsp;|&nbsp;Không phải lập lại hóa đơn");

                                    if (hoaDonBiDieuChinh.TrangThaiGui04.GetValueOrDefault() == (int)TrangThaiGuiThongDiep.ChuaGui || hoaDonBiDieuChinh.TrangThaiGui04 == null) //nếu là chưa gửi
                                    {
                                        return new CotThongBaoSaiSotViewModel
                                        {
                                            HoaDonDienTuId = hoaDonBiDieuChinh.HoaDonDienTuId,
                                            ThongDiepGuiCQTId = hoaDonBiDieuChinh.ThongDiepGuiCQTId,
                                            TrangThaiLapVaGuiThongBao = (int)TrangThaiGuiThongDiep.ChuaGui, //chưa gửi thông báo
                                            DienGiaiChiTietTrangThai = GetDienGiaiChiTietTrangThai(null, hoaDonBiDieuChinh),
                                            TenTrangThai = TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                                            IsTrongHan = (hienThiTinhTrang) ? XacDinhTrongHan(tuyChonKyKeKhai, hoaDonBiDieuChinh, boKyHieuHoaDon, listHoaDonDienTu) : null
                                        };
                                    }
                                    else //nếu là đã gửi
                                    {
                                        //đã gửi thì có định dạng là Lần gửi | trạng thái gửi | trong hạn/quá hạn
                                        TrangThaiGuiThongDiep trangThaiGuiThongDiep = (TrangThaiGuiThongDiep)hoaDonBiDieuChinh.TrangThaiGui04.GetValueOrDefault();

                                        var dienGiaiTrangThaiGui = "";
                                        if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHDKhongHopLe || trangThaiGuiThongDiep == TrangThaiGuiThongDiep.GoiDuLieuKhongHopLe)
                                        {
                                            dienGiaiTrangThaiGui = "Hóa đơn không hợp lệ";
                                        }
                                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CoHoaDonCQTKhongTiepNhan)
                                        {
                                            dienGiaiTrangThaiGui = "CQT không tiếp nhận";
                                        }
                                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon)
                                        {
                                            dienGiaiTrangThaiGui = "CQT đã tiếp nhận";
                                        }
                                        else if (trangThaiGuiThongDiep == TrangThaiGuiThongDiep.GoiDuLieuHopLe)
                                        {
                                            dienGiaiTrangThaiGui = "Hóa đơn hợp lệ";
                                        }
                                        else
                                        {
                                            dienGiaiTrangThaiGui = trangThaiGuiThongDiep.GetDescription();
                                        }

                                        return new CotThongBaoSaiSotViewModel
                                        {
                                            HoaDonDienTuId = hoaDonBiDieuChinh.HoaDonDienTuId,
                                            ThongDiepGuiCQTId = hoaDonBiDieuChinh.ThongDiepGuiCQTId,
                                            TrangThaiLapVaGuiThongBao = hoaDonBiDieuChinh.TrangThaiGui04.GetValueOrDefault(),
                                            TenTrangThai = dienGiaiTrangThaiGui,
                                            DienGiaiChiTietTrangThai = dienGiaiTrangThaiGui,
                                            LanGui = "Lần gửi " + hoaDonBiDieuChinh.LanGui04.GetValueOrDefault().ToString(),
                                            IsTrongHan = (hienThiTinhTrang) ? ((bool?)((hoaDonBiDieuChinh.TrangThaiGui04.GetValueOrDefault() > -1))) : null
                                            //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDonBiDieuChinh, boKyHieuHoaDon, listHoaDonDienTu)
                                        };
                                    }
                                }

                                if (hoaDonBiDieuChinh.IsDaLapThongBao04 != true)
                                {
                                    //kiểm tra xem hóa đơn điều chỉnh đã được cấp mã hay chưa
                                    var hoaDonDieuChinh = listHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoaDonBiDieuChinh.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
                                    var daDuocCapMa = false;
                                    if (hoaDonDieuChinh != null)
                                    {
                                        daDuocCapMa = (hoaDonDieuChinh.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDonDieuChinh.SoHoaDon.HasValue);
                                    }

                                    if (daDuocCapMa)
                                    {
                                        return new CotThongBaoSaiSotViewModel
                                        {
                                            HoaDonDienTuId = hoaDonBiDieuChinh.HoaDonDienTuId,
                                            ThongDiepGuiCQTId = hoaDonBiDieuChinh.ThongDiepGuiCQTId,
                                            TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                            TenTrangThai = "Chưa lập thông báo",
                                            DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc bị điều chỉnh",
                                            //IsTrongHan = XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu),
                                            IsHoaDonDieuChinh = true //là hóa đơn điều chỉnh vì dòng thông báo sai sót này nằm ở hóa đơn điều chỉnh
                                        };
                                    }
                                    else
                                    {
                                        return null;
                                    }

                                    /* điều chỉnh theo yêu cầu: không hiển thị ra dòng thông tin này nữa nếu chưa được cấp mã
                                    return new CotThongBaoSaiSotViewModel
                                    {
                                        HoaDonDienTuId = hoaDonBiDieuChinh.HoaDonDienTuId,
                                        ThongDiepGuiCQTId = hoaDonBiDieuChinh.ThongDiepGuiCQTId,
                                        TrangThaiLapVaGuiThongBao = -2, //chưa lập thông báo
                                        TenTrangThai = "Chưa lập thông báo",
                                        DienGiaiChiTietTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc bị điều chỉnh",
                                        IsTrongHan = ((daDuocCapMa) ? XacDinhTrongHan(tuyChonKyKeKhai, hoaDon, boKyHieuHoaDon, listHoaDonDienTu) : null),
                                        IsHoaDonDieuChinh = true //là hóa đơn điều chỉnh vì dòng thông báo sai sót này nằm ở hóa đơn điều chỉnh
                                    };
                                    */
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        //Method này để xác định trong hạn/quá hạn
        private bool? XacDinhTrongHan(string tuyChonKyKeKhai, HoaDonDienTu hoaDon, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon, List<HoaDonDienTu> listHoaDonDienTu)
        {
            DateTime? ngayDoiChieu = null;

            //nếu là hóa đơn gốc
            if (string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))
            {
                if (ngayDoiChieu == null && hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1)
                {
                    ngayDoiChieu = hoaDon.NgayXoaBo;
                }

                if (ngayDoiChieu == null && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                {
                    ngayDoiChieu = hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD;
                }

                if (ngayDoiChieu == null && hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2)
                {
                    var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();

                    if (hoaDonThayThe != null)
                    {
                        if ((hoaDonThayThe.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa) || (hoaDonThayThe.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa))
                        {
                            ngayDoiChieu = hoaDonThayThe.NgayHoaDon;
                        }
                    }
                }

                if (ngayDoiChieu == null && hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                {
                    ngayDoiChieu = hoaDon.NgayXoaBo;
                }

                if (ngayDoiChieu == null && listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId) > 0)
                {
                    //nếu hóa đơn bị điều chỉnh
                    var hoaDonDieuChinh = listHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();

                    if (hoaDonDieuChinh != null)
                    {
                        if ((hoaDonDieuChinh.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa) || (hoaDonDieuChinh.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa))
                        {
                            ngayDoiChieu = hoaDonDieuChinh.NgayHoaDon;
                        }
                    }
                }
            }

            //nếu là hóa đơn thay thế
            if (!string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId))
            {
                if (ngayDoiChieu == null && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                {
                    ngayDoiChieu = hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD;
                }

                if (ngayDoiChieu == null && hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5)
                {
                    var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();

                    if (hoaDonThayThe != null)
                    {
                        ngayDoiChieu = hoaDonThayThe.NgayHoaDon;
                    }
                }

                if (ngayDoiChieu == null && hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                {
                    ngayDoiChieu = hoaDon.NgayXoaBo;
                }
            }

            //nếu là hóa đơn điều chỉnh
            if (!string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))
            {
                if (ngayDoiChieu == null &&
                    (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1
                   || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4
                   || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc6))
                {
                    ngayDoiChieu = hoaDon.NgayXoaBo;
                }

                if (ngayDoiChieu == null && hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
                {
                    ngayDoiChieu = hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD;
                }

                if (ngayDoiChieu == null && hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
                {
                    ngayDoiChieu = hoaDon.NgayXoaBo;
                }
            }

            if (ngayDoiChieu != null)
            {
                DateTime? ngayCuoiCung = null;
                if (tuyChonKyKeKhai == "Thang")
                {
                    ngayCuoiCung = ngayDoiChieu.Value.GetLastDayOfMonth(); //ngày cuối cùng của tháng
                }
                else if (tuyChonKyKeKhai == "Quy")
                {
                    ngayCuoiCung = GetLastDateInQuarter(ngayDoiChieu.Value);
                }

                if (ngayCuoiCung != null)
                {
                    //so sánh với ngày hiện tại
                    if (DateTime.Now > ngayCuoiCung.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return null;
            }
        }

        //Method này xác định trong hạn/quá hạn với hóa đơn 32
        //private bool? XacDinhTrongHanHoaDon32(string tuyChonKyKeKhai, ThongTinHoaDon thongTinHoaDon, HoaDonDienTu hoaDon, BienBanXoaBo bienBanXoaBo, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon)
        //{
        //    DateTime? ngayDoiChieu = null;
        //    if (thongTinHoaDon != null && hoaDon != null && boKyHieuHoaDon != null)
        //    {
        //        for (var i = 0; i < 1; i++)
        //        {
        //            if (!string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId))
        //            {
        //                if (hoaDon.ThayTheChoHoaDonId == thongTinHoaDon.Id)
        //                {
        //                    if ((hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa) || (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa))
        //                    {
        //                        ///lấy ngày biên bản xóa bỏ làm đối chiếu
        //                        ngayDoiChieu = bienBanXoaBo?.NgayBienBan;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (!string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))
        //            {
        //                if (hoaDon.DieuChinhChoHoaDonId == thongTinHoaDon.Id)
        //                {
        //                    if ((hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa) || (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi && boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa))
        //                    {
        //                        //lấy ngày điều chỉnh làm đối chiếu
        //                        ngayDoiChieu = hoaDon?.NgayHoaDon;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (ngayDoiChieu != null)
        //    {
        //        DateTime? ngayCuoiCung = null;
        //        if (tuyChonKyKeKhai == "Thang")
        //        {
        //            ngayCuoiCung = ngayDoiChieu.Value.GetLastDayOfMonth(); //ngày cuối cùng của tháng
        //        }
        //        else if (tuyChonKyKeKhai == "Quy")
        //        {
        //            ngayCuoiCung = GetLastDateInQuarter(ngayDoiChieu.Value);
        //        }

        //        if (ngayCuoiCung != null)
        //        {
        //            //so sánh với ngày hiện tại
        //            if (DateTime.Now > ngayCuoiCung.Value)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //Method này để xác định ngày cuối cùng của quý của ngày đã nhập
        private DateTime GetLastDateInQuarter(DateTime inputDate)
        {
            int thang = inputDate.Month;
            if (thang <= 3)
            {
                return new DateTime(inputDate.Year, 3, 1).GetLastDayOfMonth();
            }
            else if (thang > 3 && thang <= 6)
            {
                return new DateTime(inputDate.Year, 6, 1).GetLastDayOfMonth();
            }
            else if (thang > 6 && thang <= 9)
            {
                return new DateTime(inputDate.Year, 9, 1).GetLastDayOfMonth();
            }
            else if (thang > 9 && thang <= 12)
            {
                return new DateTime(inputDate.Year, 12, 1).GetLastDayOfMonth();
            }

            return inputDate;
        }

        //Method này xác định hóa đơn điện tử liên quan đến hóa đơn bị thay thế/bị điều chỉnh
        private HoaDonDienTu XacDinhHoaDonDienTuLienQuan(List<HoaDonDienTu> listHoaDonDienTu, string hoaDonDienTuId, string phanLoaiKiemTraHoaDon)
        {
            if (phanLoaiKiemTraHoaDon == "thayThe")
            {
                var hoaDon = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDonDienTuId).OrderByDescending(y => y.CreatedDate)?.Take(1); //lấy bản ghi hóa đơn thay thế mới nhất
                if (hoaDon != null)
                {
                    return hoaDon.FirstOrDefault();
                }
            }
            else if (phanLoaiKiemTraHoaDon == "dieuChinh")
            {
                var hoaDon = listHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoaDonDienTuId).OrderByDescending(y => y.CreatedDate)?.Take(1); //lấy bản ghi hóa đơn điều chỉnh mới nhất
                if (hoaDon != null)
                {
                    return hoaDon.FirstOrDefault();
                }
            }

            return null;
        }

        public async Task<ReloadXmlResult> InsertThongDiepChungAsync(ReloadXmlParams @params)
        {
            using (var reader = new StreamReader(@params.File.OpenReadStream()))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);

                string PBan = doc.SelectSingleNode("/TDiep/TTChung/PBan").InnerText;
                string MNGui = doc.SelectSingleNode("/TDiep/TTChung/MNGui").InnerText;
                string MNNhan = doc.SelectSingleNode("/TDiep/TTChung/MNNhan").InnerText;
                string MLTDiep = doc.SelectSingleNode("/TDiep/TTChung/MLTDiep").InnerText;
                string MTDiep = doc.SelectSingleNode("/TDiep/TTChung/MTDiep").InnerText;
                string MST = doc.SelectSingleNode("/TDiep/TTChung/MST").InnerText;
                string SLuong = doc.SelectSingleNode("/TDiep/TTChung/SLuong").InnerText;
                string KHMSHDon = doc.SelectSingleNode("/TDiep/DLieu/HDon/DLHDon/TTChung/KHMSHDon").InnerText;
                string KHHDon = doc.SelectSingleNode("/TDiep/DLieu/HDon/DLHDon/TTChung/KHHDon").InnerText;
                string SHDon = doc.SelectSingleNode("/TDiep/DLieu/HDon/DLHDon/TTChung/SHDon").InnerText;

                XmlNode hDon = doc.SelectSingleNode("/TDiep/DLieu/HDon");
                string SigningTime = hDon.SelectSingleNode("descendant::SigningTime").InnerText;

                var boKyHieuHoaDon = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.KyHieuMauSoHoaDon.ToString() == KHMSHDon && x.KyHieuHoaDon == KHHDon);
                var hddt = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == boKyHieuHoaDon.BoKyHieuHoaDonId && x.SoHoaDon == long.Parse(SHDon));

                DuLieuGuiHDDT duLieuGuiHDDT = new DuLieuGuiHDDT
                {
                    DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                    HoaDonDienTuId = hddt.HoaDonDienTuId,
                    CreatedBy = hddt.CreatedBy
                };
                await _db.DuLieuGuiHDDTs.AddAsync(duLieuGuiHDDT);

                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string newXmlFileName = $"{KHMSHDon}{KHHDon}-{SHDon}-{Guid.NewGuid()}.pdf";
                string newSignedXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}");
                string newSignedXmlFullPath = Path.Combine(newSignedXmlFolder, newXmlFileName);

                ThongDiepChung thongDiepChung = new ThongDiepChung
                {
                    ThongDiepChungId = Guid.NewGuid().ToString(),
                    PhienBan = PBan,
                    MaNoiGui = MNGui,
                    MaNoiNhan = MNNhan,
                    MaLoaiThongDiep = int.Parse(MLTDiep),
                    MaThongDiep = MTDiep,
                    SoLuong = int.Parse(SLuong),
                    IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId,
                    NgayGui = DateTime.Parse(SigningTime),
                    TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                    MaSoThue = MST,
                    ThongDiepGuiDi = true,
                    Status = true,
                    FileXML = newXmlFileName,
                    CreatedBy = hddt.CreatedBy
                };
                await _db.ThongDiepChungs.AddAsync(thongDiepChung);
                await _db.SaveChangesAsync();
                return new ReloadXmlResult
                {
                    Status = true
                };
            }
        }

        public async Task<KetQuaCapSoHoaDon> CheckHoaDonPhatHanhAsync(ParamPhatHanhHD @param)
        {
            var keKhaiThueGTGT = await _TuyChonService.GetDetailAsync("KyKeKhaiThueGTGT");

            var canhBaoHDChenhLech = await _TuyChonService.GetDetailAsync("CanhBaoHDChenhLech");

            var tuyChons = await _TuyChonService.GetAllAsync();

            var boKyHieuHoaDon = await (from bkh in _db.BoKyHieuHoaDons
                                        join tdg in _db.ThongDiepChungs on bkh.ThongDiepId equals tdg.ThongDiepChungId
                                        where bkh.BoKyHieuHoaDonId == @param.HoaDon.BoKyHieuHoaDonId
                                        select new BoKyHieuHoaDonViewModel
                                        {
                                            BoKyHieuHoaDonId = bkh.BoKyHieuHoaDonId,
                                            KyHieu = bkh.KyHieu,
                                            KyHieu23Int = int.Parse(bkh.KyHieu23),
                                            TrangThaiSuDung = bkh.TrangThaiSuDung,
                                            ThongDiepChung = new ThongDiepChungViewModel
                                            {
                                                ThongDiepChungId = tdg.ThongDiepChungId,
                                                MaLoaiThongDiep = tdg.MaLoaiThongDiep,
                                                MaThongDiep = tdg.MaThongDiep,
                                                NgayThongBao = tdg.NgayThongBao,
                                            },
                                            NhatKyXacThucBoKyHieus = (from nk in _db.NhatKyXacThucBoKyHieus
                                                                      where nk.BoKyHieuHoaDonId == bkh.BoKyHieuHoaDonId
                                                                      orderby nk.CreatedDate
                                                                      select new NhatKyXacThucBoKyHieuViewModel
                                                                      {
                                                                          TrangThaiSuDung = nk.TrangThaiSuDung,
                                                                          LoaiHetHieuLuc = nk.LoaiHetHieuLuc,
                                                                          ThoiDiemChapNhan = nk.ThoiDiemChapNhan
                                                                      })
                                                                      .ToList()
                                        })
                                        .FirstOrDefaultAsync();

            string kyKeKhai = keKhaiThueGTGT.GiaTri == "Thang" ? "tháng" : "quý";
            var hoaDon = param.HoaDon;

            if (hoaDon != null && boKyHieuHoaDon != null)
            {
                var ngayHoaDon = hoaDon.NgayHoaDon.Value.Date;

                if (boKyHieuHoaDon.TrangThaiSuDung == TrangThaiSuDung.ChuaXacThuc && param.IsPhatHanh == true)
                {
                    return new KetQuaCapSoHoaDon
                    {
                        TitleMessage = "Kiểm tra lại",
                        ErrorMessage = $"Không thể phát hành hóa đơn khi trạng thái sử dụng của ký hiệu &lt;{boKyHieuHoaDon.KyHieu}&gt; là <strong>Chưa xác thực</strong>. " +
                                        $"Bạn cần xác thực sử dụng trước khi phát hành. Vui lòng kiểm tra lại."
                    };
                }

                if (boKyHieuHoaDon.TrangThaiSuDung == TrangThaiSuDung.NgungSuDung)
                {
                    return new KetQuaCapSoHoaDon
                    {
                        TitleMessage = "Kiểm tra lại",
                        ErrorMessage = param.IsPhatHanh == true ?
                                        $"Không thể phát hành hóa đơn khi trạng thái sử dụng của ký hiệu &lt;{boKyHieuHoaDon.KyHieu}&gt; là <strong>Ngừng sử dụng</strong>. Vui lòng kiểm tra lại!" :
                                        $"Ký hiệu &lt;{boKyHieuHoaDon.KyHieu}&gt; đang có trạng thái sử dụng là <strong>Ngừng sử dụng</strong>. Vui lòng kiểm tra lại!"
                    };
                }

                if (boKyHieuHoaDon.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                {
                    var lastNKXT = boKyHieuHoaDon.NhatKyXacThucBoKyHieus.LastOrDefault();

                    if (lastNKXT.LoaiHetHieuLuc == LoaiHetHieuLuc.XuatHetSoHoaDon)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Ký hiệu &lt;{boKyHieuHoaDon.KyHieu}&gt; đã phát hành đến số tối đa {lastNKXT.SoLuongHoaDon}. Vui lòng kiểm tra lại."
                        };
                    }
                    else if (lastNKXT.LoaiHetHieuLuc == LoaiHetHieuLuc.ThoiDiemCuoiNam)
                    {
                        var yearOfSysten = int.Parse(DateTime.Now.ToString("yy"));

                        if (boKyHieuHoaDon.KyHieu23Int < yearOfSysten)
                        {
                            if (keKhaiThueGTGT.GiaTri == "Thang")
                            {
                                var thoiDiem = DateTime.Parse($"{DateTime.Now.Year}-01-20");

                                if (DateTime.Now.Date <= thoiDiem)
                                {
                                    if (param.SkipCheckHetHieuLucTrongKhoang != true)
                                    {
                                        return new KetQuaCapSoHoaDon
                                        {
                                            IsAcceptHetHieuLucTrongKhoang = true,
                                            IsYesNo = true,
                                            TitleMessage = param.IsPhatHanh != true ? "Lập hóa đơn điện tử" : "Phát hành hóa đơn điện tử",
                                            ErrorMessage = $"Ký hiệu {boKyHieuHoaDon.KyHieu} được sử dụng cho hóa đơn lập có ngày hóa đơn thuộc năm 20{boKyHieuHoaDon.KyHieu23Int}. " +
                                                            $"Thời điểm hiện tại là năm 20{yearOfSysten}. Bạn có muốn tiếp tục {(param.IsPhatHanh == true ? "phát hành" : "lập")} hóa đơn này không?"
                                        };
                                    }
                                }
                                else
                                {
                                    return new KetQuaCapSoHoaDon
                                    {
                                        TitleMessage = "Kiểm tra lại",
                                        ErrorMessage = $"Ký hiệu {boKyHieuHoaDon.KyHieu} được sử dụng cho hóa đơn lập có ngày hóa đơn thuộc năm 20{boKyHieuHoaDon.KyHieu23Int}. " +
                                                        $"Thời điểm hiện tại là năm 20{yearOfSysten} và đã quá thời hạn kê khai thuế GTGT của kỳ kê khai thuế GTGT tháng 12/20{boKyHieuHoaDon.KyHieu23Int}. " +
                                                        $"Vui lòng kiểm tra lại!"
                                    };
                                }
                            }
                            else
                            {
                                var thoiDiem = DateTime.Parse($"{DateTime.Now.Year}-01-31");

                                if (DateTime.Now.Date <= thoiDiem)
                                {
                                    if (param.SkipCheckHetHieuLucTrongKhoang != true)
                                    {
                                        return new KetQuaCapSoHoaDon
                                        {
                                            IsAcceptHetHieuLucTrongKhoang = true,
                                            IsYesNo = true,
                                            TitleMessage = param.IsPhatHanh != true ? "Lập hóa đơn điện tử" : "Phát hành hóa đơn điện tử",
                                            ErrorMessage = $"Ký hiệu {boKyHieuHoaDon.KyHieu} được sử dụng cho hóa đơn lập có ngày hóa đơn thuộc năm 20{boKyHieuHoaDon.KyHieu23Int}. " +
                                                            $"Thời điểm hiện tại là năm 20{yearOfSysten}. Bạn có muốn tiếp tục {(param.IsPhatHanh == true ? "phát hành" : "lập")} hóa đơn này không?"
                                        };
                                    }
                                }
                                else
                                {
                                    return new KetQuaCapSoHoaDon
                                    {
                                        TitleMessage = "Kiểm tra lại",
                                        ErrorMessage = $"Ký hiệu {boKyHieuHoaDon.KyHieu} được sử dụng cho hóa đơn lập có ngày hóa đơn thuộc năm 20{boKyHieuHoaDon.KyHieu23Int}. " +
                                                    $"Thời điểm hiện tại là năm 20{yearOfSysten} và đã quá thời hạn kê khai thuế GTGT của kỳ kê khai thuế GTGT quý 4/20{boKyHieuHoaDon.KyHieu23Int}. " +
                                                    $"Vui lòng kiểm tra lại!"
                                    };
                                }
                            }
                        }
                    }
                }

                var thongDiepMoiNhat = await _db.ThongDiepChungs
                    .Where(x => x.MaLoaiThongDiep == boKyHieuHoaDon.ThongDiepChung.MaLoaiThongDiep &&
                                x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan &&
                                x.NgayThongBao > boKyHieuHoaDon.ThongDiepChung.NgayThongBao)
                    .OrderByDescending(x => x.NgayThongBao)
                    .FirstOrDefaultAsync();

                if (thongDiepMoiNhat != null && ngayHoaDon >= thongDiepMoiNhat.NgayThongBao.Value.Date)
                {
                    return new KetQuaCapSoHoaDon
                    {
                        TitleMessage = "Kiểm tra lại",
                        ErrorMessage = $"Hệ thống tìm thấy có tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử có mã thông điệp gửi &lt;{thongDiepMoiNhat.MaThongDiep}&gt; " +
                        $"đã được cơ quan thuế chấp nhận ngày {thongDiepMoiNhat.NgayThongBao.Value:dd/MM/yyyy} có thông tin về Hình thức hóa đơn và Loại hóa đơn phù hợp với ký hiệu &lt;{boKyHieuHoaDon.KyHieu}&gt; " +
                        $"nhưng chưa được liên kết với ký hiệu này. Vui lòng kiểm tra lại!"
                    };
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (param.IsPhatHanh == true)
                {
                    if (DateTime.Now.Date < ngayHoaDon)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Ngày ký điện tử (Ngày hiện tại) đang nhỏ hơn ngày hóa đơn <span class='colorChuYTrongThongBao'><b>{ngayHoaDon:dd/MM/yyyy}</b></span>. Vui lòng kiểm tra lại!"
                        };
                    }

                    if (DateTime.Now.Date > ngayHoaDon && param.SkipChecNgayKyLonHonNgayHoaDon != true)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            IsAcceptNgayKyLonHonNgayHoaDon = true,
                            IsYesNo = true,
                            TitleMessage = "Phát hành hóa đơn",
                            ErrorMessage = $"Ngày ký điện tử (Ngày hiện tại) đang lớn hơn ngày hóa đơn <span class='colorChuYTrongThongBao'><b>{ngayHoaDon:dd/MM/yyyy}</b></span>. Bạn có muốn tiếp tục phát hành không?"
                        };
                    }

                    var canhBaoHoaDonChenhLech = bool.Parse(canhBaoHDChenhLech.GiaTri);
                    if (canhBaoHoaDonChenhLech)
                    {
                        foreach (var item in hoaDon.HoaDonChiTiets)
                        {
                            // get thành tiền gốc theo công thức
                            var thanhTienGoc = (item.SoLuong * item.DonGia).Value.MathRoundNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                            if (item.ThanhTien != thanhTienGoc)
                            {
                                var strThanhTien = item.ThanhTien.Value.FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);
                                var strThanhTienGoc = thanhTienGoc.FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);
                                var strChenhLech = Math.Abs(thanhTienGoc - item.ThanhTien.Value).FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);

                                if (param.SkipCheckChenhLechThanhTien != true)
                                {
                                    return new KetQuaCapSoHoaDon
                                    {
                                        IsYesNo = true,
                                        IsCoCanhBaoChenhLechThanhTien = true,
                                        TitleMessage = "Phát hành hóa đơn",
                                        ErrorMessage = $"Thành tiền &lt;{strThanhTien}&gt; khác Số lượng * Đơn giá &lt;{strThanhTienGoc}&gt;, chênh lệch &lt;{strChenhLech}&gt;. Bạn có muốn tiếp tục phát hành không?"
                                    };
                                }
                            }

                            // get tiền chiết khấu gốc theo công thức
                            var tienChietKhauGoc = (item.ThanhTien * item.TyLeChietKhau / 100).Value.MathRoundNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                            if (item.TienChietKhau != tienChietKhauGoc)
                            {
                                var strTienChietKhau = item.TienChietKhau.Value.FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);
                                var strTienChietKhauGoc = tienChietKhauGoc.FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);
                                var strChenhLech = Math.Abs(tienChietKhauGoc - item.TienChietKhau.Value).FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);

                                if (param.SkipCheckChenhLechTienChietKhau != true)
                                {
                                    return new KetQuaCapSoHoaDon
                                    {
                                        IsYesNo = true,
                                        IsCoCanhBaoChenhLechTienChietKhau = true,
                                        TitleMessage = "Phát hành hóa đơn",
                                        ErrorMessage = $"Tiền chiết khấu &lt;{strTienChietKhau}&gt; khác Thành tiền * Tỷ lệ chiết khấu &lt;{strTienChietKhauGoc}&gt;, chênh lệch &lt;{strChenhLech}&gt;. Bạn có muốn tiếp tục phát hành không?"
                                    };
                                }
                            }

                            if (!string.IsNullOrEmpty(item.ThueGTGT))
                            {
                                decimal thueGTGT = 0;
                                if (item.ThueGTGT.CheckValidNumber() || item.ThueGTGT.Contains("KHAC"))
                                {
                                    if (item.ThueGTGT.Contains("KHAC"))
                                    {
                                        thueGTGT = item.ThueGTGT.Split(":")[1].ConvertStringToDecimal();
                                    }
                                    else
                                    {
                                        thueGTGT = item.ThueGTGT.ConvertStringToDecimal();
                                    }
                                }

                                // get tiền thuế GTGT gốc theo công thức
                                var tienThueGTGTGoc = ((item.ThanhTien - item.TienChietKhau) * thueGTGT / 100).Value.MathRoundNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);
                                if (item.TienThueGTGT != tienThueGTGTGoc)
                                {
                                    var strTienThueGTGT = item.TienThueGTGT.Value.FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);
                                    var strTienThueGTGTGoc = tienThueGTGTGoc.FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);
                                    var strChenhLech = Math.Abs(tienThueGTGTGoc - item.TienThueGTGT.Value).FormatNumberByTuyChon(tuyChons, hoaDon.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE, true);

                                    if (param.SkipCheckChenhLechTienThueGTGT != true)
                                    {
                                        return new KetQuaCapSoHoaDon
                                        {
                                            IsYesNo = true,
                                            IsCoCanhBaoChenhLechTienThueGTGT = true,
                                            TitleMessage = "Phát hành hóa đơn",
                                            ErrorMessage = $"Tiền thuế GTGT &lt;{strTienThueGTGT}&gt; khác (Thành tiền - Tiền chiết khấu) * Thuế suất GTGT &lt;{strTienThueGTGTGoc}&gt;, chênh lệch &lt;{strChenhLech}&gt;. Bạn có muốn tiếp tục phát hành không?"
                                        };
                                    }
                                }
                            }
                        }
                    }
                }

                var nextMonth = ngayHoaDon.AddMonths(1);
                var currentKy = string.Empty;
                DateTime thoiHanKyKeKhai;

                if (keKhaiThueGTGT.GiaTri == "Thang")
                {
                    thoiHanKyKeKhai = DateTime.Parse($"{nextMonth.Year}-{nextMonth.Month}-20");
                    currentKy = ngayHoaDon.ToString("MM/yyyy");
                }
                else
                {
                    int quarterNumber = (ngayHoaDon.Month - 1) / 3 + 1;
                    currentKy = $"0{quarterNumber}/{ngayHoaDon.Year}";
                    thoiHanKyKeKhai = ngayHoaDon.Date
                       .AddDays(1 - ngayHoaDon.Day)
                       .AddMonths(3 - (ngayHoaDon.Month - 1) % 3)
                       .AddDays(-1).AddMonths(1);
                }

                if (DateTime.Now.Date > thoiHanKyKeKhai)
                {
                    if (param.IsPhatHanh == true)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Bạn đang lựa chọn kỳ kê khai thuế GTGT là kê khai theo {kyKeKhai}. " +
                            $"Hóa đơn đang thực hiện phát hành có Ký hiệu {boKyHieuHoaDon.KyHieu} ngày hóa đơn {ngayHoaDon:dd/MM/yyyy} thuộc kỳ kê khai thuế GTGT " +
                            $"{kyKeKhai} {currentKy} có thời hạn kê khai là thời điểm {thoiHanKyKeKhai:dd/MM/yyyy} 23:59:59. Thời điểm hiện tại đã quá thời hạn kê khai thuế GTGT " +
                            $"{kyKeKhai} {currentKy}. Vui lòng kiểm tra lại!"
                        };
                    }
                    else
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Bạn đang lựa chọn kỳ kê khai thuế GTGT là kê khai theo {kyKeKhai}. " +
                            $"Hóa đơn đang lập có ngày hóa đơn {ngayHoaDon:dd/MM/yyyy} thuộc kỳ kê khai thuế GTGT " +
                            $"{kyKeKhai} {currentKy} có thời hạn kê khai là thời điểm {thoiHanKyKeKhai:dd/MM/yyyy} 23:59:59. Thời điểm hiện tại đã quá thời hạn kê khai thuế GTGT " +
                            $"{kyKeKhai} {currentKy}. Vui lòng kiểm tra lại!"
                        };
                    }
                }

                // get thông tin hóa đơn
                var thongTinHoaDons = await _db.QuanLyThongTinHoaDons
                    .Where(x => ((int)x.STT) == hoaDon.LoaiHoaDon && x.LoaiThongTin == 2)
                    .OrderBy(x => x.STT)
                    .AsNoTracking()
                    .ToListAsync();

                // Nếu ĐÃ phát sinh khoảng thời gian < Tạm ngừng sử dụng > thì Ngày hóa đơn LỚN HƠN hoặc BẰNG < Đến ngày > ở dòng dữ liệu mới nhất
                if (thongTinHoaDons.Any(x => x.LoaiThongTinChiTiet == LoaiThongTinChiTiet.TamNgungSuDung))
                {
                    var ngayKetThucTamNgungHDDT = thongTinHoaDons.LastOrDefault().DenNgayTamNgungSuDung.Value.Date;
                    if (ngayHoaDon < ngayKetThucTamNgungHDDT)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Ngày hóa đơn phải lớn hơn hoặc bằng ngày kết thúc thời gian tạm ngừng sử dụng hóa đơn điện tử là ngày <strong>{ngayKetThucTamNgungHDDT:dd/MM/yyyy}</strong>. Vui lòng kiểm tra lại!"
                        };
                    }
                }
                // Nếu CHƯA phát sinh khoảng thời gian <Tạm ngừng sử dụng> thì Ngày hóa đơn LỚN HƠN hoặc BẰNG <Ngày bắt đầu sử dụng>
                else
                {
                    var ngayBatDauSuDung = thongTinHoaDons.FirstOrDefault().NgayBatDauSuDung.Value.Date;
                    if (ngayHoaDon < ngayBatDauSuDung)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Ngày hóa đơn phải lớn hơn hoặc bằng ngày bắt đầu sử dụng hóa đơn điện tử là ngày <strong>{ngayBatDauSuDung:dd/MM/yyyy}</strong>. Vui lòng kiểm tra lại!"
                        };
                    }
                }

                //if (boKyHieuHoaDon.NhatKyXacThucBoKyHieus.Any(x => x.ThoiDiemChapNhan.HasValue))
                //{
                //    var ngayChapNhanTK = boKyHieuHoaDon.NhatKyXacThucBoKyHieus
                //         .Where(x => x.ThoiDiemChapNhan.HasValue)
                //         .Select(x => x.ThoiDiemChapNhan.Value.Date)
                //         .FirstOrDefault();

                //    if (ngayHoaDon < ngayChapNhanTK)
                //    {
                //        return new KetQuaCapSoHoaDon
                //        {
                //            TitleMessage = "Kiểm tra lại",
                //            ErrorMessage = $"Ngày hóa đơn không được nhỏ hơn ngày CQT chấp nhận tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử là ngày {ngayChapNhanTK:dd/MM/yyyy}. Vui lòng kiểm tra lại!"
                //        };
                //    }
                //}

                var hoaDonLonNhat = await _db.HoaDonDienTus
                    .Where(x => x.BoKyHieuHoaDonId == hoaDon.BoKyHieuHoaDonId && x.SoHoaDon.HasValue)
                    .Select(x => new HoaDonDienTuViewModel
                    {
                        HoaDonDienTuId = x.HoaDonDienTuId,
                        SoHoaDon = x.SoHoaDon,
                        NgayHoaDon = x.NgayHoaDon.Value.Date
                    })
                    .OrderByDescending(x => x.NgayHoaDon)
                    .FirstOrDefaultAsync();

                if (hoaDonLonNhat != null && ngayHoaDon < hoaDonLonNhat.NgayHoaDon)
                {
                    return new KetQuaCapSoHoaDon
                    {
                        TitleMessage = "Kiểm tra lại",
                        ErrorMessage = $"Ngày hóa đơn không nhỏ hơn ngày hóa đơn của hóa đơn có số hóa đơn lớn nhất là hóa đơn có ký hiệu " +
                                        $"<span class = 'colorChuYTrongThongBao'><b>{boKyHieuHoaDon.KyHieu}</b></span> số <span class = 'colorChuYTrongThongBao'><b>{hoaDonLonNhat.SoHoaDon}</b></span> ngày <span class = 'colorChuYTrongThongBao'><b>{hoaDonLonNhat.NgayHoaDon:dd/MM/yyyy}</b></span>. " +
                                        $"Vui lòng kiểm tra lại!"
                    };
                }

                if (param.IsPhatHanh == true)
                {
                    var checkHasHoaDonChuaCapSoBefore = await _db.HoaDonDienTus
                    .AnyAsync(x => x.BoKyHieuHoaDonId == hoaDon.BoKyHieuHoaDonId && !x.SoHoaDon.HasValue && x.NgayHoaDon.Value.Date < ngayHoaDon);

                    if (checkHasHoaDonChuaCapSoBefore)
                    {
                        return new KetQuaCapSoHoaDon
                        {
                            IsCoHoaDonNhoHonHoaDonDangPhatHanh = true,
                            TitleMessage = "Kiểm tra lại",
                            ErrorMessage = $"Bạn đang thực hiện phát hành hóa đơn có ký hiệu <span class = 'colorChuYTrongThongBao'><b>{boKyHieuHoaDon.KyHieu}</b></span> ngày <span class = 'colorChuYTrongThongBao'><b>{ngayHoaDon:dd/MM/yyyy}</b></span>. " +
                                            $"Tồn tại hóa đơn có ký hiệu <span class = 'colorChuYTrongThongBao'><b>{boKyHieuHoaDon.KyHieu}</b></span> số <span class = 'colorChuYTrongThongBao'><b>&lt;Chưa cấp số&gt;</b></span> có ngày hóa đơn nhỏ hơn ngày hóa đơn của hóa đơn này. " +
                                            $"Vui lòng kiểm tra lại!"
                        };
                    }
                }
            }

            return null;
        }

        public async Task<bool> UpdateNgayHoaDonBangNgayHoaDonPhatHanhAsync(HoaDonDienTuViewModel model)
        {
            var listHoaDonCoNgayHDNhoHon = await _db.HoaDonDienTus
                .Where(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId && !x.SoHoaDon.HasValue && x.NgayHoaDon.Value.Date < model.NgayHoaDon)
                .ToListAsync();

            foreach (var item in listHoaDonCoNgayHDNhoHon)
            {
                item.NgayHoaDon = model.NgayHoaDon;
            }

            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// GetListHoaDonSaiSotCanThayTheAsync đọc ra danh sách các hóa đơn bị hủy để lập hóa đơn gốc mới, và theo điều kiện 
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<HoaDonDienTuViewModel>> GetListHoaDonSaiSotCanThayTheAsync(HoaDonThayTheParams @params)
        {
            DateTime fromDate = DateTime.Parse(@params.FromDate);
            DateTime toDate = DateTime.Parse(@params.ToDate);

            var listTatCaHoaDon = await _db.HoaDonDienTus.ToListAsync();

            var query = from thongDiep in _db.ThongDiepChungs
                        join thongDiep300 in _db.ThongDiepGuiCQTs on thongDiep.IdThamChieu equals thongDiep300.Id
                        join thongDiepChiTiet300 in _db.ThongDiepChiTietGuiCQTs on thongDiep300.Id equals thongDiepChiTiet300.ThongDiepGuiCQTId
                        join hoaDon in listTatCaHoaDon on thongDiepChiTiet300.HoaDonDienTuId equals hoaDon.HoaDonDienTuId
                        join bkhhd in _db.BoKyHieuHoaDons on hoaDon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                        join lt in _db.LoaiTiens on hoaDon.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        where
                        hoaDon.NgayHoaDon.Value.Date >= fromDate && hoaDon.NgayHoaDon.Value.Date <= toDate
                        && thongDiep.MaLoaiThongDiep == 300 && thongDiepChiTiet300.PhanLoaiHDSaiSot == 1
                        && (hoaDon.TrangThaiGui04 == (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe ||
                        hoaDon.TrangThaiGui04 == (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon)
                        && (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1 && string.IsNullOrWhiteSpace(hoaDon.ThayTheChoHoaDonId) && string.IsNullOrWhiteSpace(hoaDon.DieuChinhChoHoaDonId))

                        //không cho chọn lại hóa đơn nếu đã tồn tại hóa đơn gốc thay thế không bị lỗi cấp mã
                        && ((listTatCaHoaDon.Where(x => x.IdHoaDonSaiSotBiThayThe == hoaDon.HoaDonDienTuId).OrderByDescending(y => y.CreatedDate).Take(1).Where(z => (TrangThaiQuyTrinh)z.TrangThaiQuyTrinh == TrangThaiQuyTrinh.GuiLoi || (TrangThaiQuyTrinh)z.TrangThaiQuyTrinh == TrangThaiQuyTrinh.KhongDuDieuKienCapMa).Count() > 0)
                        || listTatCaHoaDon.Count(x => x.IdHoaDonSaiSotBiThayThe == hoaDon.HoaDonDienTuId) == 0)

                        && @params.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                        orderby hoaDon.NgayHoaDon descending, hoaDon.SoHoaDon descending
                        select new HoaDonDienTuViewModel
                        {
                            MaThongDiep = thongDiep.MaThongDiep,
                            HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                            TrangThai = hoaDon.BackUpTrangThai,
                            TenTrangThaiHoaDon = hoaDon.TrangThai.HasValue ? ((TrangThaiHoaDon)hoaDon.TrangThai).GetDescription() : string.Empty,
                            LoaiHoaDon = hoaDon.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hoaDon.LoaiHoaDon).GetDescription(),
                            MauHoaDonId = hoaDon.MauHoaDonId,
                            NgayHoaDon = hoaDon.NgayHoaDon,
                            SoHoaDon = hoaDon.SoHoaDon,
                            NgayXoaBo = hoaDon.NgayXoaBo,
                            MaCuaCQT = (bkhhd != null) ? ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoaDon.MaCuaCQT ?? "<Chưa cấp mã>") : "") : "",
                            MauSo = (bkhhd != null) ? bkhhd.KyHieuMauSoHoaDon.ToString() : "",
                            KyHieu = (bkhhd != null) ? (bkhhd.KyHieuHoaDon ?? "") : "",
                            KhachHangId = hoaDon.KhachHangId,
                            MaKhachHang = hoaDon.MaKhachHang ?? string.Empty,
                            TenKhachHang = hoaDon.TenKhachHang ?? string.Empty,
                            DiaChi = hoaDon.DiaChi ?? string.Empty,
                            MaSoThue = hoaDon.MaSoThue ?? string.Empty,
                            HoTenNguoiMuaHang = hoaDon.HoTenNguoiMuaHang ?? string.Empty,
                            LoaiTienId = hoaDon.LoaiTienId,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            TongTienThanhToanQuyDoi = hoaDon.TongTienThanhToanQuyDoi,
                            TenUyNhiemLapHoaDon = (bkhhd != null) ? bkhhd.UyNhiemLapHoaDon.GetDescription() : "",
                            IsLapVanBanThoaThuan = hoaDon.IsLapVanBanThoaThuan
                        };

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.MauSo))
                {
                    var keyword = timKiemTheo.MauSo.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.SoHoaDon != null && x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NgayHoaDon))
                {
                    var keyword = timKiemTheo.NgayHoaDon.ToTrim();
                    query = query.Where(x => x.NgayHoaDon != null && x.NgayHoaDon.Value.ToString("dd/MM/yyyy").Contains(keyword));
                }
            }
            else
            {
                //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                if (string.IsNullOrWhiteSpace(@params.TimKiemBatKy) == false)
                {
                    @params.TimKiemBatKy = @params.TimKiemBatKy.ToUpper().ToTrim();
                    query = query.Where(x =>
                        (x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.SoHoaDon != null && x.SoHoaDon.ToString().ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.NgayHoaDon != null && x.NgayHoaDon.Value.ToString("dd/MM/yyyy").Contains(@params.TimKiemBatKy))
                    );
                }
            }

            return query.ToList();
        }

        /// <summary>
        /// ThongKeSoLuongHoaDonSaiSotChuaLapThongBaoAsync thống kê số lượng hóa đơn sai sót chưa lập 04
        /// </summary>
        /// <param name="coThongKeSoLuong"></param>
        /// <returns></returns>
        public async Task<ThongKeSoLuongHoaDonCoSaiSotViewModel> ThongKeSoLuongHoaDonSaiSotChuaLapThongBaoAsync(byte coThongKeSoLuong)
        {
            var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            DateTime fromDate = DateTime.Parse("2021-11-21"); //cố định như vậy theo file yêu cầu
            DateTime toDate = DateTime.Now;

            if (tuyChonKyKeKhai == "Thang") //ngày cuối cùng của tháng
            {
                toDate = DateTime.Now.GetLastDayOfMonth();
            }
            else if (tuyChonKyKeKhai == "Quy") //ngày cuối cùng của quý
            {
                int thang = DateTime.Now.Month;
                int nam = DateTime.Now.Year;
                if (thang <= 3)
                {
                    toDate = new DateTime(nam, 3, 1).GetLastDayOfMonth();
                }
                else if (thang > 3 && thang <= 6)
                {
                    toDate = new DateTime(nam, 6, 1).GetLastDayOfMonth();
                }
                else if (thang > 6 && thang <= 9)
                {
                    toDate = new DateTime(nam, 9, 1).GetLastDayOfMonth();
                }
                else if (thang > 9 && thang <= 12)
                {
                    toDate = new DateTime(nam, 12, 1).GetLastDayOfMonth();
                }
            }

            int thongKeSoLuong = 0;
            if (coThongKeSoLuong == 1)
            {
                //đọc ra trước các hóa đơn để lấy ra hóa đơn thay thế, hóa đơn điều chỉnh tại mỗi dòng hóa đơn đang duyệt
                List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                             select new HoaDonDienTu
                                                             {
                                                                 HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                                 SoHoaDon = hoaDon.SoHoaDon,
                                                                 ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                                 DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                                 NgayHoaDon = hoaDon.NgayHoaDon,
                                                                 TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                                 MaCuaCQT = hoaDon.MaCuaCQT,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 CreatedDate = hoaDon.CreatedDate
                                                             }).ToListAsync();

                //đọc ra thông tin hóa đơn được nhập từ phần mềm khác, (được dùng để hiển thị cột thông tin sai sót ở hóa đơn điều chỉnh); việc đọc ra bảng này vì phải truy vấn thông tin với các hóa đơn được nhập từ phần mềm khác
                List<ThongTinHoaDon> listThongTinHoaDon = await (from hoaDon in _db.ThongTinHoaDons
                                                                 where listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.Id) > 0
                                                                 select new ThongTinHoaDon
                                                                 {
                                                                     Id = hoaDon.Id,
                                                                     TrangThaiHoaDon = hoaDon.TrangThaiHoaDon,
                                                                     IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                     LanGui04 = hoaDon.LanGui04,
                                                                     ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                     TrangThaiGui04 = hoaDon.TrangThaiGui04
                                                                 }).ToListAsync();

                var queryThongKe = (from hoaDon in _db.HoaDonDienTus
                                    join bkhhd in _db.BoKyHieuHoaDons on hoaDon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                    where hoaDon.NgayHoaDon.Value.Date >= fromDate
                                     && hoaDon.NgayHoaDon.Value.Date <= toDate
                                    select new HoaDonDienTuViewModel
                                    {
                                        ThongBaoSaiSot = GetCotThongBaoSaiSot(tuyChonKyKeKhai, hoaDon, bkhhd, listHoaDonDienTu, listThongTinHoaDon.FirstOrDefault(x => x.Id == hoaDon.DieuChinhChoHoaDonId))
                                    }).ToList();

                thongKeSoLuong = queryThongKe.Count(x => x.ThongBaoSaiSot != null && x.ThongBaoSaiSot.TrangThaiLapVaGuiThongBao == -2); //TrangThaiLapVaGuiThongBao = -2: chưa lập thông báo
            }

            return new ThongKeSoLuongHoaDonCoSaiSotViewModel
            {
                TuNgay = fromDate.ToString("yyyy-MM-dd"),
                DenNgay = toDate.ToString("yyyy-MM-dd"),
                SoLuong = thongKeSoLuong,
                IsDaLapThongBao04 = false //do điều kiện nên chỉ có IsDaLapThongBao04 = false
            };
        }

        /// <summary>
        /// KiemTraSoLanGuiEmailSaiSotAsync trả về số lần gửi email theo loại sai sót
        /// </summary>
        /// <param name="hoaDonDienTuId"></param>
        /// <param name="loaiSaiSot">phân loại sai sót</param>
        /// <returns></returns>
        public async Task<int> KiemTraSoLanGuiEmailSaiSotAsync(string hoaDonDienTuId, byte loaiSaiSot)
        {
            //loaiSaiSot: 0 = họ tên, 1 = đơn vị, 2 = địa chỉ
            int soLanGui = 0;
            switch (loaiSaiSot)
            {
                case 0:
                    soLanGui = await _db.ThongBaoSaiThongTins.CountAsync(x => x.HoTenNguoiMuaHang_Dung != x.HoTenNguoiMuaHang_Sai && x.HoaDonDienTuId == hoaDonDienTuId);
                    break;
                case 1:
                    soLanGui = await _db.ThongBaoSaiThongTins.CountAsync(x => x.TenDonVi_Dung != x.TenDonVi_Sai && x.HoaDonDienTuId == hoaDonDienTuId);
                    break;
                case 2:
                    soLanGui = await _db.ThongBaoSaiThongTins.CountAsync(x => x.DiaChi_Dung != x.DiaChi_Sai && x.HoaDonDienTuId == hoaDonDienTuId);
                    break;
                default:
                    break;
            }

            return soLanGui;
        }

        /// <summary>
        /// KiemTraHoaDonThayTheDaDuocCapMaAsync trả về thông báo cho biết hóa đơn thay thế đã được cấp mã hay chưa
        /// </summary>
        /// <param name="hoaDonDienTuId">là id của hóa đơn thay thế gần nhất để phát hành</param>
        /// <returns></returns>
        public async Task<string> KiemTraHoaDonThayTheDaDuocCapMaAsync(string hoaDonDienTuId)
        {
            string cauThongBao = null;
            var hoaDonThayTheHienTai = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == hoaDonDienTuId);
            if (string.IsNullOrWhiteSpace(hoaDonThayTheHienTai.ThayTheChoHoaDonId))
            {
                return null; //nếu ko phải là hóa đơn thay thế thì bỏ qua
            }

            var hoaDonThayTheKhac = await _db.HoaDonDienTus.Where(x => x.ThayTheChoHoaDonId == hoaDonThayTheHienTai.ThayTheChoHoaDonId && x.HoaDonDienTuId != hoaDonDienTuId).OrderByDescending(x => x.CreatedDate)?.Take(1).FirstOrDefaultAsync();

            if (hoaDonThayTheKhac != null)
            {
                if (hoaDonThayTheKhac.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDonThayTheKhac.SoHoaDon.HasValue)
                {
                    var hoaDonBiThayThe = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == hoaDonThayTheHienTai.ThayTheChoHoaDonId);
                    if (hoaDonBiThayThe != null)
                    {
                        var queryBoKyHieu = await _db.BoKyHieuHoaDons.Where(x => x.BoKyHieuHoaDonId == hoaDonBiThayThe.BoKyHieuHoaDonId || x.BoKyHieuHoaDonId == hoaDonThayTheKhac.BoKyHieuHoaDonId).ToListAsync();

                        var boKyHieuHoaDonBiThayThe = queryBoKyHieu.FirstOrDefault(x => x.BoKyHieuHoaDonId == hoaDonBiThayThe.BoKyHieuHoaDonId);
                        var boKyHieuHoaDonThayTheKhac = queryBoKyHieu.FirstOrDefault(x => x.BoKyHieuHoaDonId == hoaDonThayTheKhac.BoKyHieuHoaDonId);

                        cauThongBao = "Hóa đơn được chọn để lập hóa đơn thay thế là hóa đơn có ký hiệu <strong class = 'colorChuYTrongThongBao'>" + boKyHieuHoaDonBiThayThe.KyHieu + "</strong> số <strong class = 'colorChuYTrongThongBao'>" + hoaDonBiThayThe.SoHoaDon + "</strong> ngày <strong class = 'colorChuYTrongThongBao'>" + hoaDonBiThayThe.NgayHoaDon.Value.ToString("dd/MM/yyyy") + "</strong> đã được thay thế bởi hóa đơn có ký hiệu <strong class = 'colorChuYTrongThongBao'>" + boKyHieuHoaDonThayTheKhac.KyHieu + "</strong> số <strong class = 'colorChuYTrongThongBao'>" + hoaDonThayTheKhac.SoHoaDon + "</strong> ngày <strong class = 'colorChuYTrongThongBao'>" + hoaDonThayTheKhac.NgayHoaDon.Value.ToString("dd/MM/yyyy") + "</strong>. Vui lòng kiểm tra lại.";
                    }
                }
            }

            return cauThongBao;
        }

        //Method này kiểm tra xem hóa đơn thay thế có được phép phát hành lại ko
        private bool KiemTraHoaDonThayTheKhongDuocPhatHanhLai(string hoaDonDienTuId, List<DLL.Entity.QuanLy.BoKyHieuHoaDon> listBoKyHieuHoaDon, List<HoaDonDienTu> listHoaDon)
        {
            var hoaDonThayThe = listHoaDon.Where(x => x.ThayTheChoHoaDonId == hoaDonDienTuId).OrderByDescending(y => y.CreatedDate).Take(1).FirstOrDefault();

            if (hoaDonThayThe != null)
            {
                if (!hoaDonThayThe.NgayKy.HasValue ||
                    hoaDonThayThe.NgayKy.Value.Date != DateTime.Now.Date ||
                    listBoKyHieuHoaDon.FirstOrDefault(x => x.BoKyHieuHoaDonId == hoaDonThayThe.BoKyHieuHoaDonId).KyHieu.IsHoaDonCoMa() != true ||
                    ((hoaDonThayThe.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.GuiLoi) && (hoaDonThayThe.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.KhongDuDieuKienCapMa)))
                {
                    return true; //true: không được phép phát hành lại
                }
                else
                {
                    if (hoaDonThayThe.SoHoaDon.HasValue)
                    {
                        if (listHoaDon.Any(x => x.SoHoaDon.HasValue && x.SoHoaDon > hoaDonThayThe.SoHoaDon && x.BoKyHieuHoaDonId == hoaDonThayThe.BoKyHieuHoaDonId))
                        {
                            return true; //true: không được phép phát hành lại
                        }
                    }

                    return false; //false: được phép phát hành lại
                }
            }

            return true; //mặc định trả về true, đây là điều kiện hợp lệ khi kết hợp với điều kiện AND
        }

        /// <summary>
        /// Kiểm tra xem hóa đơn đã có phản hồi từ cqt chưa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckDaPhatSinhThongDiepTruyenNhanVoiCQTAsync(string id)
        {
            var maThongDiepGuiMoiNhat = await (from dlghhdt in _db.DuLieuGuiHDDTs
                                               join tdg in _db.ThongDiepChungs on dlghhdt.DuLieuGuiHDDTId equals tdg.IdThamChieu
                                               where dlghhdt.HoaDonDienTuId == id
                                               orderby tdg.NgayGui descending
                                               select tdg.MaThongDiep).FirstOrDefaultAsync();

            var result = await _db.TransferLogs.AnyAsync(x => !string.IsNullOrEmpty(x.MTDTChieu) && x.MTDTChieu == maThongDiepGuiMoiNhat);
            return result;
        }

        public async Task<bool> CheckLaHoaDonGuiTCTNLoiAsync(string id)
        {
            var result = await (from tdg in _db.ThongDiepChungs
                                join dlghddt in _db.DuLieuGuiHDDTs on tdg.IdThamChieu equals dlghddt.DuLieuGuiHDDTId
                                join hddt in _db.HoaDonDienTus on dlghddt.HoaDonDienTuId equals hddt.HoaDonDienTuId
                                where hddt.HoaDonDienTuId == id
                                orderby tdg.CreatedDate descending
                                select tdg).FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            return result.TrangThaiGui == (int)TrangThaiGuiThongDiep.GuiTCTNLoi;
        }

        public async Task<int> GetTrangThaiQuyTrinhByIdAsync(string id)
        {
            var result = await _db.HoaDonDienTus
                .Where(x => x.HoaDonDienTuId == id)
                .Select(x => x.TrangThaiQuyTrinh)
                .FirstOrDefaultAsync();

            return result.Value;
        }

        /// <summary>
        /// Đổi danh sách id to item hóa đơn theo bảng kê
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        public IEnumerable<HoaDonDienTuViewModel> SortListSelected(HoaDonParams pagingParams)
        {
            var listIdFilter = pagingParams.HoaDonDienTuIds
                .Where(x => pagingParams.HoaDonDienTus.Select(y => y.HoaDonDienTuId).Contains(x))
                .ToList();

            foreach (var id in listIdFilter)
            {
                var item = pagingParams.HoaDonDienTus.FirstOrDefault(x => x.HoaDonDienTuId == id);
                yield return item;
            }
        }

        /// <summary>
        /// Get value of node MTDiep in xml signed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetMaThongDiepInXMLSignedByIdAsync(string id)
        {
            //var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}");

            //var fileData = await _db.HoaDonDienTus.AsNoTracking().FirstOrDefaultAsync(x => x.HoaDonDienTuId == id);
            var fileData = await _db.FileDatas.AsNoTracking()
                .OrderByDescending(x => x.DateTime)
                .FirstOrDefaultAsync(x => x.RefId == id && x.Type == 1);

            if (fileData == null)
            {
                return null;
            }

            // string filePath = Path.Combine(fullXmlFolder, fileData.XMLDaKy);
            XmlDocument doc = new XmlDocument();
            string xml = Encoding.UTF8.GetString(fileData.Binary);
            doc.LoadXml(xml);

            var result = doc.SelectSingleNode("/TDiep/TTChung/MTDiep").InnerText;
            return result;
        }

        //Method này xác định diễn giải trạng thái chi tiết của hóa đơn sau khi đã lập thông báo 04
        private string GetDienGiaiChiTietTrangThai(HoaDonDienTu hoaDon, HoaDonDienTu hoaDonBiDieuChinh)
        {
            if (hoaDonBiDieuChinh != null) //ưu tiên, nếu truyền vào tham số hoaDonBiDieuChinh != null thì return ngay
            {
                return "&nbsp;|&nbsp;Hóa đơn gốc bị điều chỉnh";
            }

            if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1
                       || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4
                       || hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc6)
            {
                return "&nbsp;|&nbsp;Hủy do sai sót";
            }

            if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2)
            {
                return "&nbsp;|&nbsp;Xóa để lập thay thế";
            }

            if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3)
            {
                return "&nbsp;|&nbsp;Hủy theo lý do phát sinh";
            }

            if (hoaDon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5)
            {
                return "&nbsp;|&nbsp;Xóa để lập thay thế mới";
            }

            if (hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null)
            {
                return "&nbsp;|&nbsp;Không phải lập lại hóa đơn";
            }

            return "";
        }

        public async Task<List<TaiLieuDinhKemViewModel>> GetTaiLieuDinhKemsByIdAsync(string id)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            var result = await (from tldk in _db.TaiLieuDinhKems
                                where tldk.NghiepVuId == id
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
                                .ToListAsync();

            return result;
        }

        public async Task<HoaDonDienTuViewModel> GetHoaDonByThayTheChoHoaDonIdAsync(string id)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            var query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                            //join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                            //from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                        join nv in _db.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        join bbdc_dc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc_dc.HoaDonDieuChinhId into tmpBienBanDieuChinh_DCs
                        from bbdc_dc in tmpBienBanDieuChinh_DCs.DefaultIfEmpty()
                        where hd.ThayTheChoHoaDonId == id
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            DaDieuChinh = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId),
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                                TrangThaiSuDung = bkhhd.TrangThaiSuDung
                            },
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauHoaDonId = mhd != null ? mhd.MauHoaDonId : null,
                            MauHoaDon = mhd != null ? new MauHoaDonViewModel
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                LoaiHoaDon = mhd.LoaiHoaDon,
                                LoaiThueGTGT = mhd.LoaiThueGTGT,
                            } : null,
                            MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                            KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                            KhachHangId = kh.DoiTuongId,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            DiaChi = hd.DiaChi,
                            MaNhanVienBanHang = hd.MaNhanVienBanHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            KhachHang = kh != null ?
                                        new DoiTuongViewModel
                                        {
                                            DoiTuongId = kh.DoiTuongId,
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
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
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
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            TenTrangThaiQuyTrinh = ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription(),
                            MaTraCuu = hd.MaTraCuu,
                            IsBuyerSigned = hd.IsBuyerSigned,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            TrangThaiGuiHoaDonNhap = hd.TrangThaiGuiHoaDonNhap,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            NgayXoaBo = hd.NgayXoaBo,
                            SoCTXoaBo = hd.SoCTXoaBo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
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
                            IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan,
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
                            TrangThaiBienBanDieuChinh = bbdc_dc != null ? bbdc_dc.TrangThaiBienBan : (bbdc != null ? bbdc.TrangThaiBienBan : (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                            ThoiHanThanhToan = hd.ThoiHanThanhToan,
                            DiaChiGiaoHang = hd.DiaChiGiaoHang,
                            BienBanDieuChinhId = bbdc_dc != null ? bbdc_dc.BienBanDieuChinhId : (bbdc != null ? bbdc.BienBanDieuChinhId : null),
                            LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
                            IdHoaDonSaiSotBiThayThe = hd.IdHoaDonSaiSotBiThayThe,
                            GhiChuThayTheSaiSot = hd.GhiChuThayTheSaiSot,
                            HoaDonChiTiets = (
                                               from hdct in _db.HoaDonDienTuChiTiets
                                               join hd in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                               from hd in tmpHoaDons.DefaultIfEmpty()
                                               join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                               from vt in tmpHangHoas.DefaultIfEmpty()
                                               join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                               from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                               where hdct.HoaDonDienTuId == id
                                               orderby hdct.CreatedDate
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   STT = hdct.STT,
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   TinhChat = hdct.TinhChat,
                                                   TenTinhChat = ((TChat)(hdct.TinhChat)).GetDescription(),
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
                                                   TyLePhanTramDoanhThu = hdct.TyLePhanTramDoanhThu,
                                                   TienGiam = hdct.TienGiam ?? 0,
                                                   TienGiamQuyDoi = hdct.TienGiamQuyDoi ?? 0,
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
                            TongTienGiam = hd.TongTienGiam ?? 0,
                            TongTienGiamQuyDoi = hd.TongTienGiamQuyDoi ?? 0,
                            CreatedBy = hd.CreatedBy,
                            CreatedDate = hd.CreatedDate,
                            Status = hd.Status,
                            TrangThaiThoaThuan = hd.IsLapVanBanThoaThuan == true ? "Có thỏa thuận" : "Không thỏa thuận",
                            MaCuaCQT = hd.MaCuaCQT,
                            NgayKy = hd.NgayKy,
                            LoaiChietKhau = hd.LoaiChietKhau,
                            TyLeChietKhau = hd.TyLeChietKhau,
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon,
                            HinhThucXoabo = hd.HinhThucXoabo,
                            BackUpTrangThai = hd.BackUpTrangThai,
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            LoaiApDungHoaDonDieuChinh = 1,
                            IsGiamTheoNghiQuyet = hd.IsGiamTheoNghiQuyet,
                            TyLePhanTramDoanhThu = hd.TyLePhanTramDoanhThu ?? 0,
                            TrangThaiLanDieuChinhGanNhat = _db.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId) ? _db.HoaDonDienTus.Where(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId).OrderByDescending(x => x.CreatedDate).FirstOrDefault().TrangThaiQuyTrinh : (int?)null,
                            MauSoHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                              join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                              where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                              orderby hddt.CreatedDate descending
                                                              select bkh.KyHieuMauSoHoaDon).FirstOrDefault(),
                            KyHieuHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                               join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                               where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                               orderby hddt.CreatedDate descending
                                                               select bkh.KyHieuHoaDon).FirstOrDefault(),
                            SoHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                           where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                           orderby hddt.CreatedDate descending
                                                           select hddt.SoHoaDon).FirstOrDefault(),
                            NgayHoaDonLanDieuChinhGanNhat = (from hddt in _db.HoaDonDienTus
                                                             where hddt.DieuChinhChoHoaDonId == hd.HoaDonDienTuId
                                                             orderby hddt.CreatedDate descending
                                                             select hddt.NgayHoaDon).FirstOrDefault(),
                        };

            var result = await query.FirstOrDefaultAsync();
            if (result != null) result.TenTrangThaiLanDieuChinhGanNhat = result.TrangThaiLanDieuChinhGanNhat.HasValue ? ((TrangThaiQuyTrinh)result.TrangThaiLanDieuChinhGanNhat.Value).GetDescription() : string.Empty;
            return result;
        }

    }
}