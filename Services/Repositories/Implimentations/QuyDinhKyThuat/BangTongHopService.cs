using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class BangTongHopService : IBangTongHopService
    {
        private readonly Datacontext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IXMLInvoiceService _xMLInvoiceService;
        private readonly ITVanService _ITVanService;

        public BangTongHopService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IXMLInvoiceService xMLInvoiceService,
            ITVanService ITVanService
        )
        {
            _db = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _xMLInvoiceService = xMLInvoiceService;
            _ITVanService = ITVanService;
        }

        /// <summary>
        /// Tạo file xml bảng tổng hợp. Trả về tên file xml
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public string CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folderPath = $"FilesUpload/{databaseName}/BangTongHopDuLieu/unsigned";
            string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }
            else
            {
                Directory.Delete(fullFolderPath, true);
                Directory.CreateDirectory(fullFolderPath);
            }

            string fileName = $"{Guid.NewGuid()}.xml";
            string filePath = Path.Combine(fullFolderPath, fileName);
            _xMLInvoiceService.CreateBangTongHopDuLieu(filePath, @params);
            return fileName;
        }

        /// <summary>
        /// Lấy dữ liệu bảng tổng hợp gửi đến TVAN
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<TongHopDuLieuHoaDonGuiCQTViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        {
            IQueryable<TongHopDuLieuHoaDonGuiCQTViewModel> query = null;
            query = from hd in _db.HoaDonDienTus
                    join hdct in _db.HoaDonDienTuChiTiets on hd.HoaDonDienTuId equals hdct.HoaDonDienTuId into tmpHoaDons
                    from hdct in tmpHoaDons.DefaultIfEmpty()
                    join mhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals mhd.BoKyHieuHoaDonId into tmpMauHoaDons
                    from mhd in tmpMauHoaDons.DefaultIfEmpty()
                    join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                    from dvt in tmpDonViTinhs.DefaultIfEmpty()
                    where hd.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu && mhd.HinhThucHoaDon == (int)HinhThucHoaDon.KhongCoMa
                    select new TongHopDuLieuHoaDonGuiCQTViewModel
                    {
                        MauSo = mhd.KyHieuMauSoHoaDon.ToString(),
                        KyHieu = mhd.KyHieuHoaDon,
                        SoHoaDon = hd.SoHoaDon,
                        NgayHoaDon = hd.NgayHoaDon,
                        MaSoThue = hd.MaSoThue,
                        MaKhachHang = hd.MaKhachHang,
                        TenKhachHang = hd.TenKhachHang,
                        HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                        MaHang = hdct.MaHang,
                        TenHang = hdct.TenHang,
                        SoLuong = hdct.SoLuong,
                        DonViTinh = dvt.Ten ?? string.Empty,
                        ThanhTien = hdct.ThanhTien,
                        ThueGTGT = hdct.ThueGTGT,
                        TienThueGTGT = hdct.TienThueGTGT,
                        TongTienThanhToan = hdct.TongTienThanhToan,
                        TrangThaiHoaDon = hd.TrangThai,
                        TenTrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
                        MauSoHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
                                               (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
                                               (from hd1 in _db.HoaDonDienTus
                                                join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
                                                select bkh.KyHieuMauSoHoaDon.ToString()).FirstOrDefault()
                                                : (from hd1 in _db.ThongTinHoaDons
                                                   where hd1.Id == hd.DieuChinhChoHoaDonId
                                                   select hd1.MauSoHoaDon).FirstOrDefault()
                                                ) :
                                            !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
                                            (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
                                            (
                                            from hd1 in _db.HoaDonDienTus
                                            join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                            where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
                                            select bkh.KyHieuMauSoHoaDon.ToString()).FirstOrDefault()
                                            : (from hd1 in _db.ThongTinHoaDons
                                               where hd1.Id == hd.ThayTheChoHoaDonId
                                               select hd1.MauSoHoaDon).FirstOrDefault()
                                            )
                                            : null,
                        KyHieuHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
                                               (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
                                               (from hd1 in _db.HoaDonDienTus
                                                join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                                where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
                                                select bkh.KyHieuHoaDon.ToString()).FirstOrDefault()
                                                : (from hd1 in _db.ThongTinHoaDons
                                                   where hd1.Id == hd.DieuChinhChoHoaDonId
                                                   select hd1.KyHieuHoaDon).FirstOrDefault()
                                                ) :
                                            !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
                                            (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
                                            (
                                            from hd1 in _db.HoaDonDienTus
                                            join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
                                            where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
                                            select bkh.KyHieuHoaDon.ToString()).FirstOrDefault()
                                            : (from hd1 in _db.ThongTinHoaDons
                                               where hd1.Id == hd.ThayTheChoHoaDonId
                                               select hd1.KyHieuHoaDon).FirstOrDefault()
                                            )
                                            : null,
                        SoHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
                                                (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
                                                    ((from hd1 in _db.HoaDonDienTus
                                                      where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
                                                      select hd1.SoHoaDon).FirstOrDefault() + "") :
                                                     (from hd1 in _db.ThongTinHoaDons
                                                      where hd1.Id == hd.DieuChinhChoHoaDonId
                                                      select hd1.SoHoaDon).FirstOrDefault()) :
                                            !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
                                                (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
                                                    ((from hd1 in _db.HoaDonDienTus
                                                      where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
                                                      select hd1.SoHoaDon).FirstOrDefault() + "") :
                                                     (from hd1 in _db.ThongTinHoaDons
                                                      where hd1.Id == hd.ThayTheChoHoaDonId
                                                      select hd1.SoHoaDon).FirstOrDefault()) : null,
                        NgayHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
                                                (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
                                                    (from hd1 in _db.HoaDonDienTus
                                                     where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
                                                     select hd1.NgayHoaDon).FirstOrDefault() :
                                                     (from hd1 in _db.ThongTinHoaDons
                                                      where hd1.Id == hd.DieuChinhChoHoaDonId
                                                      select hd1.NgayHoaDon).FirstOrDefault()) :
                                            !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
                                                (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
                                                    (from hd1 in _db.HoaDonDienTus
                                                     where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
                                                     select hd1.NgayHoaDon).FirstOrDefault() :
                                                     (from hd1 in _db.ThongTinHoaDons
                                                      where hd1.Id == hd.ThayTheChoHoaDonId
                                                      select hd1.NgayHoaDon).FirstOrDefault()) : null,
                        LoaiApDungHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
                                                    (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
                                                    (int)LADHDDT.HinhThuc1 :
                                                    (from hd1 in _db.ThongTinHoaDons
                                                     where hd1.Id == hd.DieuChinhChoHoaDonId
                                                     select (int)hd1.HinhThucApDung).FirstOrDefault()
                                                    ) : (!string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
                                                    (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
                                                    (int)LADHDDT.HinhThuc1 :
                                                    (from hd1 in _db.ThongTinHoaDons
                                                     where hd1.Id == hd.ThayTheChoHoaDonId
                                                     select (int)hd1.HinhThucApDung).FirstOrDefault()) : (int?)null)
                    };

            query = query.GroupBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.ThueGTGT })
                .Select(x => new TongHopDuLieuHoaDonGuiCQTViewModel
                {
                    MauSo = x.Key.MauSo,
                    KyHieu = x.Key.KyHieu,
                    SoHoaDon = x.Key.SoHoaDon,
                    NgayHoaDon = x.First().NgayHoaDon,
                    MaSoThue = x.First().MaSoThue,
                    MaKhachHang = x.First().MaKhachHang,
                    TenKhachHang = x.First().TenKhachHang,
                    HoTenNguoiMuaHang = x.First().HoTenNguoiMuaHang,
                    TrangThaiHoaDon = x.First().TrangThaiHoaDon,
                    TenTrangThaiHoaDon = x.First().TenTrangThaiHoaDon,
                    SoLuong = x.Sum(o => o.SoLuong),
                    TenHang = x.Select(o => o.TenHang).Distinct().Join(", "),
                    ThueGTGT = x.Key.ThueGTGT.CheckIsInteger() ? $"{x.Key.ThueGTGT}%" : x.Key.ThueGTGT,
                    ThanhTien = x.Sum(o => o.ThanhTien),
                    TienThueGTGT = x.Sum(o => o.TienThueGTGT),
                    TongTienThanhToan = x.Sum(o => o.TongTienThanhToan),
                    MauSoHoaDonLienQuan = x.First().MauSoHoaDonLienQuan,
                    KyHieuHoaDonLienQuan = x.First().KyHieuHoaDonLienQuan,
                    SoHoaDonLienQuan = x.First().SoHoaDonLienQuan,
                    NgayHoaDonLienQuan = x.First().NgayHoaDonLienQuan,
                    LoaiApDungHoaDonLienQuan = x.First().LoaiApDungHoaDonLienQuan
                });


            if (!string.IsNullOrEmpty(@params.TuNgay) && !string.IsNullOrEmpty(@params.DenNgay))
            {
                DateTime fromDate = DateTime.ParseExact(@params.TuNgay, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(@params.DenNgay + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                query = query.Where(x => x.NgayHoaDon >= fromDate && x.NgayHoaDon <= toDate);
            }

            query = query.OrderBy(x => x.NgayHoaDon)
            .ThenBy(x => x.MauSo)
            .ThenBy(x => x.KyHieu)
            .ThenBy(x => x.SoHoaDon);

            var result = await query.ToListAsync();
            return result;
        }

        /// <summary>
        /// Gửi bảng tổng hợp dữ liệu
        /// </summary>
        /// <param name="XMLUrl"></param>
        /// <param name="thongDiepChungId"></param>
        /// <param name="maThongDiep"></param>
        /// <param name="mst"></param>
        /// <returns></returns>
        public async Task<bool> GuiBangDuLieu(string XMLUrl, string thongDiepChungId, string maThongDiep, string mst)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            var fullXMLFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

            var data = new GuiThongDiepData
            {
                MST = mst,
                MTDiep = maThongDiep,
                DataXML = File.ReadAllText(Path.Combine(fullXMLFolder, XMLUrl))
            };
            await _ITVanService.TVANSendData("api/report/send", data.DataXML);
            return true;
        }


        /// <summary>
        /// Lưu dữ liệu ký bảng tổng hợp vào file datas
        /// </summary>
        /// <param name="encodedContent"></param>
        /// <param name="thongDiepId"></param>
        /// <returns></returns>
        public string LuuDuLieuKy(string encodedContent, string thongDiepId)
        {
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            if (!Directory.Exists(fullFolder))
            {
                Directory.CreateDirectory(fullFolder);
            }

            var fullXMLFile = Path.Combine(fullFolder, fileName);
            File.WriteAllText(fullXMLFile, encodedContent);
            return fileName;
        }

        /// <summary>
        /// Generate tự động số bảng tổng hợp dựa trên các bảng tổng hợp đã gửi
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<int> GetSoBangTongHopDuLieu(BangTongHopParams2 @params)
        {
            IQueryable<string> tDiep400Ids = from td in _db.ThongDiepChungs
                                             where td.MaLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue && td.TrangThaiGui != (int)TrangThaiGuiThongDiep.ChuaGui
                                             orderby td.CreatedDate descending
                                             select td.ThongDiepChungId;
            var td400NewestId = await tDiep400Ids.ToListAsync();
            if (td400NewestId.Any())
            {
                var kDLieu = @params.ThangDuLieu.HasValue ? (@params.ThangDuLieu < 10 ? $"0${@params.ThangDuLieu.Value}/{@params.NamDuLieu}" : $"{@params.ThangDuLieu.Value}/{@params.NamDuLieu}") :
                            $"0{@params.QuyDuLieu.Value}/{@params.NamDuLieu}";
                var lKDLieu = @params.ThangDuLieu.HasValue ? "T" : "Q";
                foreach (var id in td400NewestId)
                {
                    var plainContent = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == false).Select(x => x.Content).FirstOrDefaultAsync();
                    var td400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContent);
                    var dl = td400.DLieu.Where(x => x.DLBTHop.TTChung.KDLieu == kDLieu && x.DLBTHop.TTChung.LKDLieu == lKDLieu).ToList();
                    if (td400.DLieu.Any(x => x.DLBTHop.TTChung.KDLieu == kDLieu && x.DLBTHop.TTChung.LKDLieu == lKDLieu))
                    {
                        return (td400.DLieu.Where(x => x.DLBTHop.TTChung.KDLieu == kDLieu).Max(x => x.DLBTHop.TTChung.SBTHDLieu)) + 1;
                    }
                }
            }

            return 1;
        }
    }
}
