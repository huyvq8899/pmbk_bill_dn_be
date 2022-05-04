using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.Filter;
using Services.Helper.Params.HoaDon;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class BangTongHopService : IBangTongHopService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQuyDinhKyThuatService _quyDinhKyThuatService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IXMLInvoiceService _xMLInvoiceService;
        private readonly ITVanService _ITVanService;

        public BangTongHopService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IXMLInvoiceService xMLInvoiceService,
            IQuyDinhKyThuatService quyDinhKyThuatService,
            ITVanService ITVanService,
            IMapper mp
        )
        {
            _db = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _xMLInvoiceService = xMLInvoiceService;
            _quyDinhKyThuatService = quyDinhKyThuatService;
            _ITVanService = ITVanService;
            _mp = mp;
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
        public async Task<List<BangTongHopDuLieuHoaDonChiTietViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        {
            try
            {
                IQueryable<BangTongHopDuLieuHoaDonChiTietViewModel> query = null;
                query = from hd in _db.HoaDonDienTus
                        join hdct in _db.HoaDonDienTuChiTiets on hd.HoaDonDienTuId equals hdct.HoaDonDienTuId into tmpHoaDons
                        from hdct in tmpHoaDons.DefaultIfEmpty()
                        join tbaoCT in _db.ThongDiepChiTietGuiCQTs on hd.HoaDonDienTuId equals tbaoCT.HoaDonDienTuId into tmpThongBaoChiTiets
                        from tbaoCT in tmpThongBaoChiTiets.DefaultIfEmpty()
                        join mhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals mhd.BoKyHieuHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        where hd.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu && mhd.HinhThucHoaDon == (int)HinhThucHoaDon.KhongCoMa && hd.TrangThai != 2
                        && !_db.BangTongHopDuLieuHoaDonChiTiets.Any(x => x.RefHoaDonDienTuId == hd.HoaDonDienTuId)
                        select new BangTongHopDuLieuHoaDonChiTietViewModel
                        {
                            MauSo = mhd.KyHieuMauSoHoaDon.ToString(),
                            BackupTrangThai = hd.BackUpTrangThai.HasValue ? hd.BackUpTrangThai.Value : (int?)null,
                            TrangThai = hd.TrangThai.HasValue ? hd.BackUpTrangThai.Value : (int?)null,
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
                            TrangThaiHoaDon = hd.TrangThai == 1 ? 0 : hd.TrangThai == 2 ? 0 : hd.TrangThai == 3 ? 3 : 2,
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
                            LoaiHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
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
                                                         select (int)hd1.HinhThucApDung).FirstOrDefault()) : (int?)null),
                            STBao = tbaoCT != null ? _db.ThongDiepGuiCQTs.Where(x => x.Id == tbaoCT.ThongDiepGuiCQTId).Select(x => x.SoThongBaoSaiSot).FirstOrDefault() : string.Empty,
                            NTBao = tbaoCT != null ? _db.ThongDiepGuiCQTs.Where(x => x.Id == tbaoCT.ThongDiepGuiCQTId).Select(x => x.NgayGui).FirstOrDefault() : (DateTime?)null,
                            RefHoaDonDienTuId = hd.HoaDonDienTuId
                        };

                query = query.GroupBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.ThueGTGT })
                    .Select(x => new BangTongHopDuLieuHoaDonChiTietViewModel
                    {
                        MauSo = x.Key.MauSo,
                        KyHieu = x.Key.KyHieu,
                        TrangThai = x.First().TrangThai,
                        BackupTrangThai = x.First().BackupTrangThai,
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
                        LoaiHoaDonLienQuan = x.First().LoaiHoaDonLienQuan,
                        TenLoaiHoaDonLienQuan = ((LADHDDT)x.First().LoaiHoaDonLienQuan).GetDescription(),
                    //STBao = x.First().STBao,
                    //NTBao = x.First().NTBao,
                        RefHoaDonDienTuId = x.First().RefHoaDonDienTuId
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
            catch(Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gửi bảng tổng hợp dữ liệu
        /// </summary>
        /// <param name="XMLUrl"></param>
        /// <param name="thongDiepChungId"></param>
        /// <param name="maThongDiep"></param>
        /// <param name="mst"></param>
        /// <returns></returns>
        public async Task<bool> GuiBangDuLieu(string thongDiepChungId, string maThongDiep, string mst)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            var dataXML = (await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == thongDiepChungId)).Content;
            var data = new GuiThongDiepData
            {
                MST = mst,
                MTDiep = maThongDiep,
                DataXML = dataXML
            };

            // Send to TVAN
            string strContent = await _ITVanService.TVANSendData("api/report/send", data.DataXML);

            if (string.IsNullOrEmpty(strContent))
            {
                return false;
            }

            var @params = new ThongDiepPhanHoiParams()
            {
                ThongDiepId = thongDiepChungId,
                DataXML = strContent,
                MST = mst,
                MLTDiep = 999,
                MTDiep = maThongDiep
            };

            return await _quyDinhKyThuatService.InsertThongDiepNhanAsync(@params);
        }


        /// <summary>
        /// Lưu dữ liệu ký bảng tổng hợp vào file datas
        /// </summary>
        /// <param name="encodedContent"></param>
        /// <param name="thongDiepId"></param>
        /// <returns></returns>
        public async Task<bool> LuuDuLieuKy(string encodedContent, string thongDiepId)
        {
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            if (!Directory.Exists(fullFolder))
            {
                Directory.CreateDirectory(fullFolder);
            }

            var base64EncodedBytes = System.Convert.FromBase64String(encodedContent);
            byte[] byteXML = Encoding.UTF8.GetBytes(encodedContent);
            string dataXML = TextHelper.Decompress(encodedContent);

            var fileData = new FileData
            {
                RefId = thongDiepId,
                Type = 1,
                IsSigned = true,
                DateTime = DateTime.Now,
                Content = dataXML,
                Binary = byteXML,
            };

            //var entity = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == thongDiepId);
            //if (entity != null) _db.FileDatas.Remove(entity);
            await _db.FileDatas.AddAsync(fileData);

            return await _db.SaveChangesAsync() > 0;
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
                            @params.NgayDuLieu.HasValue ? @params.NgayDuLieu.Value.ToString("dd/MM/yyyyy") :
                            $"0{@params.QuyDuLieu.Value}/{@params.NamDuLieu}" ;
                var lKDLieu = @params.ThangDuLieu.HasValue ? "T" :  @params.NgayDuLieu.HasValue ? "N ": "Q";
                foreach (var id in td400NewestId)
                {
                    var plainContent = await _db.FileDatas.Where(x => x.RefId == id && x.IsSigned == false).Select(x => x.Content).FirstOrDefaultAsync();
                    if (!string.IsNullOrEmpty(plainContent))
                    {
                        var td400 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(plainContent);
                        var dl = td400.DLieu.Where(x => x.DLBTHop.TTChung.KDLieu == kDLieu && x.DLBTHop.TTChung.LKDLieu == lKDLieu).ToList();
                        if (td400.DLieu.Any(x => x.DLBTHop.TTChung.KDLieu == kDLieu && x.DLBTHop.TTChung.LKDLieu == lKDLieu))
                        {
                            return (td400.DLieu.Where(x => x.DLBTHop.TTChung.KDLieu == kDLieu).Max(x => x.DLBTHop.TTChung.SBTHDLieu)) + 1;
                        }
                    }
                    else
                    {
                        var bth = _db.BangTongHopDuLieuHoaDons.Where(x => x.ThongDiepChungId == id).FirstOrDefault();
                        if (bth != null) return bth.SoBTHDLieu + 1;
                    }
                }
            }

            return 1;
        }

        /// <summary>
        /// Check xem có bảng tổng hợp lần đầu và đã gửi thành công cho CQT không
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<dynamic> CheckLanDau(BangTongHopParams3 @params)
        {
            IQueryable<string> tDiep400LDIds = from bth in _db.BangTongHopDuLieuHoaDons
                                               where bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.BangTongHopHopLe && bth.LanDau == true && !bth.BoSungLanThu.HasValue
                                               && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                               && bth.LHHoa == @params.LoaiHH
                                               select bth.Id;
            if (tDiep400LDIds.Any()) return new { rs = 1 };
            else
            {
                IQueryable<BangTongHopDuLieuHoaDon> tDiep400LD_SentIds = from bth in _db.BangTongHopDuLieuHoaDons
                                                        where bth.TrangThaiQuyTrinh != TrangThaiQuyTrinh_BangTongHop.ChuaGui && bth.LanDau == true && !bth.BoSungLanThu.HasValue
                                                        && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                        && bth.LHHoa == @params.LoaiHH
                                                        orderby bth.TrangThaiQuyTrinh descending
                                                        select bth;

                if (tDiep400LD_SentIds.Any()) return new { rs = 2, trangThaiQuyTrinh = tDiep400LD_SentIds.FirstOrDefault().TrangThaiQuyTrinh, tenTrangThaiQuyTrinh = tDiep400LD_SentIds.FirstOrDefault().TrangThaiQuyTrinh.GetDescription() };
                else
                {
                    IQueryable<string> tDiep400LD_UnSentIds = from bth in _db.BangTongHopDuLieuHoaDons
                                                            where bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.ChuaGui && bth.LanDau == true && !bth.BoSungLanThu.HasValue
                                                            && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                            && bth.LHHoa == @params.LoaiHH
                                                            select bth.Id;
                    if (tDiep400LD_UnSentIds.Any()) return new { rs = 1 };
                } 
            }

            return 0;
        }

        /// <summary>
        /// Tự động sinh số lần bổ sung với trường hợp bố sung. 
        /// Số lần bổ sung >= 1 và <=999
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<int> GetLanBoSung(BangTongHopParams3 @params)
        {
            IQueryable<int> tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                                where bth.LanDau == false
                                                && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                && bth.LHHoa == @params.LoaiHH
                                                && bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.BangTongHopHopLe
                                                orderby bth.BoSungLanThu descending
                                                select bth.BoSungLanThu.Value;

            if (!tDiep400BSNumbers.Any()) { return 1; }
            else
            {
                var bSLTMax = await tDiep400BSNumbers.FirstOrDefaultAsync();
                return bSLTMax + 1;
            }
        }

        /// <summary>
        /// Check xem có bảng tổng hợp sửa đổi lần thứ n và đã gửi thành công cho CQT không
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<dynamic> CheckBoSung(BangTongHopParams3 @params) 
        { 
            IQueryable<BangTongHopDuLieuHoaDon> tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                                                    where bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.BangTongHopHopLe
                                                                    && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                                    && bth.LHHoa == @params.LoaiHH
                                                                    && bth.LanDau == false
                                                                    && bth.BoSungLanThu == @params.SoLanBoSung
                                                                    select bth;

            if (tDiep400BSNumbers.Any()) { return new { rs = 1 }; }
            else
            {
                tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                    where bth.TrangThaiQuyTrinh != TrangThaiQuyTrinh_BangTongHop.ChuaGui
                                    && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                    && bth.LHHoa == @params.LoaiHH
                                    && bth.LanDau == false
                                    && bth.BoSungLanThu == @params.SoLanBoSung
                                    select bth;

                if (tDiep400BSNumbers.Any()) return new { rs = 2, trangThaiQuyTrinh = tDiep400BSNumbers.FirstOrDefault().TrangThaiQuyTrinh, tenTrangThaiQuyTrinh = tDiep400BSNumbers.FirstOrDefault().TrangThaiQuyTrinh.GetDescription() };
                else return new { rs = 0 };
            }
        }

        /// <summary>
        /// Tự động sinh số lần sửa đổi với trường hợp sửa đổi 
        /// Số lần sửa đổi >= 1 và <=999
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<int> GetLanSuaDoi(BangTongHopParams3 @params)
        {
            IQueryable<int> tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                                where bth.LanDau == true && bth.BoSungLanThu.HasValue
                                                && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                && bth.LHHoa == @params.LoaiHH
                                                && bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.BangTongHopHopLe
                                                orderby bth.BoSungLanThu descending
                                                select bth.BoSungLanThu.Value;

            if (!tDiep400BSNumbers.Any()) { return 1; }
            else
            {
                var bSLTMax = await tDiep400BSNumbers.FirstOrDefaultAsync();
                return bSLTMax + 1;
            }
        }

        /// <summary>
        /// Check xem có bảng tổng hợp sửa đổi lần thứ n và đã gửi thành công cho CQT không
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<dynamic> CheckSuaDoi(BangTongHopParams3 @params)
        {
            IQueryable<BangTongHopDuLieuHoaDon> tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                                where bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.BangTongHopHopLe
                                                && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                && bth.LHHoa == @params.LoaiHH
                                                && bth.LanDau == true && bth.BoSungLanThu.HasValue
                                                && bth.BoSungLanThu == @params.SoLanSuaDoi
                                                select bth;

            if (tDiep400BSNumbers.Any()) { return new { rs = 1 }; }
            else
            {
                tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                    where bth.TrangThaiQuyTrinh != TrangThaiQuyTrinh_BangTongHop.ChuaGui
                                    && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                    && bth.LHHoa == @params.LoaiHH
                                    select bth;
                if (tDiep400BSNumbers.Any()) return new { rs = 2, trangThaiQuyTrinh = tDiep400BSNumbers.FirstOrDefault().TrangThaiQuyTrinh, tenTrangThaiQuyTrinh = tDiep400BSNumbers.FirstOrDefault().TrangThaiQuyTrinh.GetDescription() };
                else return new { rs = 0 };
            }
        }

        /// <summary>
        /// Check xem có bảng tổng hợp sửa đổi lần n chưa gửi cqt
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<bool> CheckSuaDoiChuaGui(BangTongHopParams3 @params)
        {
            IQueryable<BangTongHopDuLieuHoaDon> tDiep400BSNumbers = from bth in _db.BangTongHopDuLieuHoaDons
                                                                    where bth.LanDau == true && bth.BoSungLanThu.HasValue
                                                                    && ((bth.NamDuLieu == @params.NamDuLieu && (bth.ThangDuLieu == @params.ThangDuLieu || bth.QuyDuLieu == @params.QuyDuLieu)) || (@params.NgayDuLieu.HasValue && bth.NgayDuLieu == @params.NgayDuLieu.Value))
                                                                    && bth.LHHoa == @params.LoaiHH
                                                                    && bth.BoSungLanThu == @params.SoLanSuaDoi
                                                                    && bth.TrangThaiQuyTrinh == TrangThaiQuyTrinh_BangTongHop.ChuaGui
                                                                    orderby bth.BoSungLanThu descending
                                                                    select bth;

            return tDiep400BSNumbers.Any();
        }

        /// <summary>
        /// Thêm mới bảng tổng hợp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BangTongHopDuLieuHoaDonViewModel> InsertBangTongHopDuLieuHoaDonAsync(BangTongHopDuLieuHoaDonViewModel model)
        {
            var chiTiets = model.ChiTiets;
            model.ChiTiets = null;
            var entity = _mp.Map<BangTongHopDuLieuHoaDon>(model);
            entity.Id = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = model.ActionUser.UserId;
            await _db.BangTongHopDuLieuHoaDons.AddAsync(entity);

            foreach (var item in chiTiets)
            {
                item.BangTongHopDuLieuHoaDonId = entity.Id;
            }

            var entitiesChiTiet = _mp.Map<List<BangTongHopDuLieuHoaDonChiTiet>>(chiTiets);
            await _db.BangTongHopDuLieuHoaDonChiTiets.AddRangeAsync(entitiesChiTiet);

            if(await _db.SaveChangesAsync() == chiTiets.Count + 1)
            {
                return _mp.Map<BangTongHopDuLieuHoaDonViewModel>(entity);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Cập nhật bảng tổng hợp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBangTongHopDuLieuHoaDonAsync(BangTongHopDuLieuHoaDonViewModel model)
        {
            if (model.ChiTiets != null && model.ChiTiets.Any())
            {
                var entityChiTiets = await _db.BangTongHopDuLieuHoaDonChiTiets.Where(x => x.BangTongHopDuLieuHoaDonId == model.Id).ToListAsync();
                if (entityChiTiets.Any())
                {
                    _db.BangTongHopDuLieuHoaDonChiTiets.RemoveRange(entityChiTiets);
                }

                var chiTiets = model.ChiTiets;
                model.ChiTiets = null;

                foreach (var item in chiTiets)
                {
                    item.BangTongHopDuLieuHoaDonId = model.Id;
                }

                var entitiesChiTiet = _mp.Map<List<BangTongHopDuLieuHoaDonChiTiet>>(chiTiets);
                await _db.BangTongHopDuLieuHoaDonChiTiets.AddRangeAsync(entitiesChiTiet);
            }

            if(model.ActionUser != null) model.ModifyBy = model.ActionUser.UserId;
            model.ModifyDate = DateTime.Now;
            var entity = _mp.Map<BangTongHopDuLieuHoaDon>(model);
            _db.BangTongHopDuLieuHoaDons.Update(entity);

            return await _db.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Xóa bảng tổng hợp
        /// </summary>
        /// <param name="BangTongHopId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBangTongHopDuLieuHoaDonAsync(string BangTongHopId)
        {
            var entityChiTiets = await _db.BangTongHopDuLieuHoaDonChiTiets.Where(x => x.BangTongHopDuLieuHoaDonId == BangTongHopId).ToListAsync();
            if (entityChiTiets.Any())
            {
                _db.BangTongHopDuLieuHoaDonChiTiets.RemoveRange(entityChiTiets);
            }

            var entity = await _db.BangTongHopDuLieuHoaDons.FirstOrDefaultAsync(x => x.Id == BangTongHopId);
            _db.BangTongHopDuLieuHoaDons.Remove(entity);

            return await _db.SaveChangesAsync() == entityChiTiets.Count + 1;
        }

        /// <summary>
        /// Lấy list tiêu chí tìm kiếm
        /// </summary>
        /// <returns></returns>
        public List<EnumModel> GetListTimKiemTheoBangTongHop()
        {
            BangTongHopSearch search = new BangTongHopSearch();
            var result = search.GetType().GetProperties()
                .Select(x => new EnumModel
                {
                    Value = x.Name,
                    Name = (x.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute).Name
                })
                .ToList();

            return result;
        }

        /// <summary>
        /// Lấy thông tin bảng tổng hợp theo Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BangTongHopDuLieuHoaDonViewModel> GetById(string Id)
        {
            IQueryable<BangTongHopDuLieuHoaDonViewModel> query = from bth in _db.BangTongHopDuLieuHoaDons
                                                                 where bth.Id == Id
                                                                 select new BangTongHopDuLieuHoaDonViewModel
                                                                 {
                                                                     Id = bth.Id,
                                                                     PhienBan = bth.PhienBan,
                                                                     MauSo = bth.MauSo,
                                                                     Ten = bth.Ten,
                                                                     SoBTHDLieu = bth.SoBTHDLieu,
                                                                     LoaiKyDuLieu = bth.LoaiKyDuLieu,
                                                                     KyDuLieu = bth.KyDuLieu,
                                                                     NamDuLieu = bth.NamDuLieu,
                                                                     ThangDuLieu = bth.ThangDuLieu,
                                                                     QuyDuLieu = bth.QuyDuLieu,
                                                                     NgayDuLieu = bth.NgayDuLieu,
                                                                     LanDau = bth.LanDau != null ? bth.LanDau : false,
                                                                     BoSungLanThu = bth.BoSungLanThu,
                                                                     NgayLap = bth.NgayLap,
                                                                     TenNNT = bth.TenNNT,
                                                                     MaSoThue = bth.MaSoThue,
                                                                     HDDIn = bth.HDDIn,
                                                                     LHHoa = bth.LHHoa,
                                                                     ThoiHanGui = bth.ThoiHanGui,
                                                                     NNT = bth.NNT,
                                                                     CreatedDate = bth.CreatedDate,
                                                                     CreatedBy = bth.CreatedBy,
                                                                     ChiTiets = (from ct in _db.BangTongHopDuLieuHoaDonChiTiets
                                                                                 where ct.BangTongHopDuLieuHoaDonId == bth.Id
                                                                                 select new BangTongHopDuLieuHoaDonChiTietViewModel
                                                                                 {
                                                                                     Id = ct.Id,
                                                                                     BangTongHopDuLieuHoaDonId = ct.BangTongHopDuLieuHoaDonId,
                                                                                     RefHoaDonDienTuId = ct.RefHoaDonDienTuId,
                                                                                     MauSo = ct.MauSo,
                                                                                     KyHieu = ct.KyHieu,
                                                                                     SoHoaDon = ct.SoHoaDon,
                                                                                     NgayHoaDon = ct.NgayHoaDon,
                                                                                     MaSoThue = ct.MaSoThue,
                                                                                     MaKhachHang = ct.MaKhachHang,
                                                                                     TenKhachHang = ct.TenKhachHang,
                                                                                     DiaChi = ct.DiaChi,
                                                                                     HoTenNguoiMuaHang = ct.HoTenNguoiMuaHang,
                                                                                     MaHang = ct.MaHang,
                                                                                     TenHang = ct.TenHang,
                                                                                     SoLuong = ct.SoLuong,
                                                                                     DonViTinh = ct.DonViTinh,
                                                                                     ThanhTien = ct.ThanhTien,
                                                                                     ThueGTGT = ct.ThueGTGT,
                                                                                     TienThueGTGT = ct.TienThueGTGT,
                                                                                     TongTienThanhToan = ct.TongTienThanhToan,
                                                                                     TrangThaiHoaDon = ct.TrangThaiHoaDon,
                                                                                     TenTrangThaiHoaDon = ((TrangThaiHoaDon)ct.TrangThaiHoaDon).GetDescription(),
                                                                                     LoaiHoaDonLienQuan = ct.LoaiHoaDonLienQuan,
                                                                                     MauSoHoaDonLienQuan = ct.MauSoHoaDonLienQuan,
                                                                                     KyHieuHoaDonLienQuan = ct.KyHieuHoaDonLienQuan,
                                                                                     SoHoaDonLienQuan = ct.SoHoaDonLienQuan,
                                                                                     NgayHoaDonLienQuan = ct.NgayHoaDonLienQuan,
                                                                                     LKDLDChinh = ct.LKDLDChinh,
                                                                                     KDLDChinh = ct.KDLDChinh,
                                                                                     STBao = ct.STBao,
                                                                                     NTBao = ct.NTBao,
                                                                                     GhiChu = ct.GhiChu
                                                                                 }).ToList()
                                                                 };
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lấy bảng kê dữ liệu bảng tổng hợp theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<PagedList<BangTongHopDuLieuHoaDonViewModel>> GetAllPagingBangTongHopAsync(BangTongHopDuLieuHoaDonParams @params)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            try
            {
                IQueryable<BangTongHopDuLieuHoaDonViewModel> query = from bth in _db.BangTongHopDuLieuHoaDons
                                                                     join tdc in _db.ThongDiepChungs on bth.ThongDiepChungId equals tdc.ThongDiepChungId into thongDiepChungTmp
                                                                     from tdc in thongDiepChungTmp.DefaultIfEmpty()
                                                                     where tdc == null || (tdc != null && tdc.ThongDiepGuiDi == true)
                                                                     select new BangTongHopDuLieuHoaDonViewModel
                                                                     {
                                                                         Id = bth.Id,
                                                                         PhienBan = bth.PhienBan,
                                                                         MauSo = bth.MauSo,
                                                                         Ten = bth.Ten,
                                                                         SoBTHDLieu = bth.SoBTHDLieu,
                                                                         LoaiKyDuLieu = bth.LoaiKyDuLieu,
                                                                         KyDuLieu = bth.KyDuLieu,
                                                                         NamDuLieu = bth.NamDuLieu,
                                                                         ThangDuLieu = bth.ThangDuLieu,
                                                                         QuyDuLieu = bth.QuyDuLieu,
                                                                         NgayDuLieu = bth.NgayDuLieu,
                                                                         LanDau = bth.LanDau != null ? bth.LanDau : false,
                                                                         BoSungLanThu = bth.BoSungLanThu,
                                                                         NgayLap = bth.NgayLap,
                                                                         TenNNT = bth.TenNNT,
                                                                         MaSoThue = bth.MaSoThue,
                                                                         HDDIn = bth.HDDIn,
                                                                         LHHoa = bth.LHHoa,
                                                                         TenLoaiHH = ((LHHoa)bth.LHHoa).GetDescription(),
                                                                         ThoiHanGui = bth.ThoiHanGui,
                                                                         NNT = bth.NNT,
                                                                         CreatedDate = bth.CreatedDate,
                                                                         CreatedBy = bth.CreatedBy,
                                                                         ThoiGianGui = tdc != null ? tdc.NgayGui : (DateTime?)null,
                                                                         ThongDiepChungId = tdc != null ? tdc.ThongDiepChungId : string.Empty,
                                                                         MaLoaiThongDiep = tdc != null ? tdc.MaLoaiThongDiep : (int?)null,
                                                                         TrangThaiGui = tdc != null ? (TrangThaiGuiThongDiep)tdc.TrangThaiGui : TrangThaiGuiThongDiep.ChuaGui,
                                                                         TenTrangThaiGui = tdc != null ? ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription() : TrangThaiGuiThongDiep.ChuaGui.GetDescription(),
                                                                         MaThongDiep = tdc != null ? tdc.MaThongDiep : string.Empty,
                                                                         TrangThaiQuyTrinh = bth.TrangThaiQuyTrinh,
                                                                         TenTrangThaiQuyTrinh = bth.TrangThaiQuyTrinh == null ? string.Empty : bth.TrangThaiQuyTrinh.GetDescription(),
                                                                         NguoiTao = _mp.Map<UserViewModel>(_db.Users.FirstOrDefault(x=>x.UserId == bth.CreatedBy)),
                                                                         NguoiCapNhat = _mp.Map<UserViewModel>(_db.Users.FirstOrDefault(x=>x.UserId == bth.ModifyBy))
                                                                     };

                if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
                {
                    DateTime fromDate = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime toDate = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    query = query.Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate);
                }

                // thông điệp nhận
                if (@params.LoaiHangHoa != -1 && @params.LoaiHangHoa != null)
                {
                    query = query.Where(x => x.LHHoa == @params.LoaiHangHoa);
                }

                if (@params.TrangThaiGui != -99 && @params.TrangThaiGui != null)
                {
                    query = query.Where(x => x.TrangThaiGui == (TrangThaiGuiThongDiep)@params.TrangThaiGui);
                }

                if (@params.TimKiemTheo != null)
                {
                    var timKiemTheo = @params.TimKiemTheo;
                    if (!string.IsNullOrEmpty(timKiemTheo.PhienBan))
                    {
                        var keyword = timKiemTheo.PhienBan.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.PhienBan) && x.PhienBan.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (timKiemTheo.SoBangTongHop.HasValue)
                    {
                        var keyword = timKiemTheo.SoBangTongHop;
                        query = query.Where(x => x.SoBTHDLieu == timKiemTheo.SoBangTongHop);
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                    {
                        var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaSoThue) && x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiep))
                    {
                        var keyword = timKiemTheo.MaThongDiep.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaThongDiep) && x.MaThongDiep.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (timKiemTheo.TenNNT != null)
                    {
                        var keyword = timKiemTheo.TenNNT.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.TenNNT) && x.TenNNT.ToUpper().ToTrim().Contains(keyword));
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
                            case nameof(@params.Filter.PhienBan):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.PhienBan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.SoBTHDLieu):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.SoBTHDLieu, filterCol, FilterValueType.Int);
                                break;
                            case nameof(@params.Filter.MauSo):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.MauSo, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaLoaiThongDiep):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.MaLoaiThongDiep, filterCol, FilterValueType.Decimal);
                                break;
                            case nameof(@params.Filter.MaThongDiep):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.MaThongDiep, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.Ten):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.Ten, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaSoThue):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.MaSoThue, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.LoaiKyDuLieu):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.LoaiKyDuLieu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.KyDuLieu):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.KyDuLieu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.TenNNT):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.TenNNT, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.LHHoa):
                                query = GenericFilterColumn<BangTongHopDuLieuHoaDonViewModel>.Query(query, x => x.LHHoa, filterCol, FilterValueType.Int);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(@params.SortKey))
                {
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.PhienBan);
                    }
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.PhienBan);
                    }

                    if (@params.SortKey == "SoBTHDLieu" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoBTHDLieu);
                    }
                    if (@params.SortKey == "SoBTHDLieu" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoBTHDLieu);
                    }


                    if (@params.SortKey == "MauSo" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MauSo);
                    }
                    if (@params.SortKey == "MauSo" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MauSo);
                    }

                    if (@params.SortKey == "Ten" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ten);
                    }
                    if (@params.SortKey == "Ten" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ten);
                    }

                    if (@params.SortKey == "NgayLap" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayLap);
                    }
                    if (@params.SortKey == "NgayLap" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayLap);
                    }

                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaThongDiep);
                    }
                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaThongDiep);
                    }

                    if (@params.SortKey == "TenNNT" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenNNT);
                    }
                    if (@params.SortKey == "TenNNT" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenNNT);
                    }

                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaSoThue);
                    }
                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaSoThue);
                    }
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedDate.Value);
                }
                #endregion

                var list = await query.ToListAsync();

                if (@params.PageSize == -1)
                {
                    @params.PageSize = await query.CountAsync();
                }

                return await PagedList<BangTongHopDuLieuHoaDonViewModel>
                     .CreateAsync(query, @params.PageNumber, @params.PageSize);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
