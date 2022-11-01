using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.HoaDonSaiSot;
using Services.Helper.Params.Filter;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.ThongDiepGuiNhanCQT;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class ThongDiepGuiNhanCQTService : IThongDiepGuiNhanCQTService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITVanService _ITVanService;
        private readonly int MaLoaiThongDiep = 300;

        public ThongDiepGuiNhanCQTService(Datacontext db,
            IMapper mp,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            ITVanService tvanService
        )
        {
            _db = db;
            _mp = mp;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _ITVanService = tvanService;
        }

        /// <summary>
        /// TaoSoThongBaoSaiSot tạo số thông báo sai sót khi thêm mới
        /// </summary>
        /// <returns></returns>
        public async Task<string> TaoSoThongBaoSaiSotAsync()
        {
            var maxSoThongBao = await _db.ThongDiepGuiCQTs.MaxAsync(x => int.Parse(x.SoThongBaoSaiSot ?? "0"));
            return string.Format("TBSS{0}", (maxSoThongBao + 1));
        }

        /// <summary>
        /// GetThongDiepGuiCQTByIdAsync trả về bản ghi thông điệp gửi CQT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ThongDiepGuiCQTViewModel> GetThongDiepGuiCQTByIdAsync(DataByIdParams @params)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string fileContainerPath = $"FilesUpload/{databaseName}";
            var querySoLanGuiCQT = await _db.ThongDiepChiTietGuiCQTs.ToListAsync();

            var queryDetail = _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == @params.ThongDiepGuiCQTId);

            var queryDetailThongBaoRaSoat = from chiTiet in _db.ThongBaoChiTietHoaDonRaSoats
                                            select new ThongBaoChiTietHoaDonRaSoatViewModel
                                            {
                                                Id = chiTiet.Id,
                                                LoaiApDungHD = chiTiet.LoaiApDungHD,
                                                LyDoRaSoat = chiTiet.LyDoRaSoat
                                            };

            var query = from thongDiep in _db.ThongDiepGuiCQTs
                        join raSoat in _db.ThongBaoHoaDonRaSoats on thongDiep.ThongBaoHoaDonRaSoatId equals raSoat.Id into
                        tmpRaSoat
                        from raSoat in tmpRaSoat.DefaultIfEmpty()
                        where thongDiep.Id == @params.ThongDiepGuiCQTId
                        select new ThongDiepGuiCQTViewModel
                        {
                            Id = thongDiep.Id,
                            SoThongBaoSaiSot = thongDiep.SoThongBaoSaiSot,
                            DaiDienNguoiNopThue = thongDiep.DaiDienNguoiNopThue,
                            DaKyGuiCQT = thongDiep.DaKyGuiCQT,
                            DiaDanh = thongDiep.DiaDanh,
                            FileDinhKem = thongDiep.FileDinhKem,
                            FileXMLDaKy = thongDiep.FileXMLDaKy,
                            LoaiThongBao = (thongDiep.IsTBaoHuyGiaiTrinhKhacCuaNNT == true) ? ((byte)3) : (string.IsNullOrWhiteSpace(thongDiep.ThongBaoHoaDonRaSoatId) ? (byte)1 : (byte)2),
                            MaCoQuanThue = thongDiep.MaCoQuanThue,
                            MaDiaDanh = thongDiep.MaDiaDanh,
                            MaSoThue = thongDiep.MaSoThue,
                            MaThongDiep = thongDiep.MaThongDiep,
                            NgayGui = thongDiep.NgayGui,
                            NgayLap = thongDiep.NgayLap,
                            NguoiNopThue = thongDiep.NguoiNopThue,
                            NTBCCQT = raSoat.NgayThongBao,
                            SoTBCCQT = raSoat.SoThongBaoCuaCQT,
                            TenCoQuanThue = thongDiep.TenCoQuanThue,
                            ThongBaoHoaDonRaSoatId = thongDiep.ThongBaoHoaDonRaSoatId,
                            CreatedDate = thongDiep.CreatedDate,
                            FileContainerPath = fileContainerPath,
                            IsTBaoHuyGiaiTrinhKhacCuaNNT = thongDiep.IsTBaoHuyGiaiTrinhKhacCuaNNT,
                            HinhThucTBaoHuyGiaiTrinhKhac = thongDiep.HinhThucTBaoHuyGiaiTrinhKhac,
                            ThongDiepChiTietGuiCQTs = (from chiTiet in queryDetail
                                                       orderby chiTiet.STT
                                                       select new ThongDiepChiTietGuiCQTViewModel
                                                       {
                                                           SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.ChungTuLienQuan == chiTiet.ChungTuLienQuan).OrderBy(x => x.CreatedDate).Select(y => y.ThongDiepGuiCQTId).Distinct().ToList().IndexOf(thongDiep.Id) + 1,
                                                           Id = chiTiet.Id,
                                                           ThongDiepGuiCQTId = chiTiet.ThongDiepGuiCQTId,
                                                           HoaDonDienTuId = chiTiet.HoaDonDienTuId,
                                                           MaCQTCap = chiTiet.MaCQTCap,
                                                           MauHoaDon = chiTiet.MauHoaDon,
                                                           KyHieuHoaDon = chiTiet.KyHieuHoaDon,
                                                           SoHoaDon = chiTiet.SoHoaDon,
                                                           NgayLapHoaDon = chiTiet.NgayLapHoaDon,
                                                           LoaiApDungHoaDon = chiTiet.LoaiApDungHoaDon,
                                                           PhanLoaiHDSaiSot = chiTiet.PhanLoaiHDSaiSot,
                                                           PhanLoaiHDSaiSotMacDinh = chiTiet.PhanLoaiHDSaiSotMacDinh,
                                                           LyDo = chiTiet.LyDo,
                                                           STT = chiTiet.STT,
                                                           ThongBaoChiTietHDRaSoatId = chiTiet.ThongBaoChiTietHDRaSoatId,
                                                           CreatedDate = chiTiet.CreatedDate,
                                                           CreatedBy = chiTiet.CreatedBy,
                                                           ModifyDate = chiTiet.ModifyDate,
                                                           ModifyBy = chiTiet.ModifyBy,
                                                           LoaiApDungHD = (string.IsNullOrWhiteSpace(chiTiet.ThongBaoChiTietHDRaSoatId) == false) ? queryDetailThongBaoRaSoat.FirstOrDefault(x => x.Id == chiTiet.ThongBaoChiTietHDRaSoatId).LoaiApDungHD : ((byte)0),
                                                           LyDoRaSoat = (string.IsNullOrWhiteSpace(chiTiet.ThongBaoChiTietHDRaSoatId) == false) ? queryDetailThongBaoRaSoat.FirstOrDefault(x => x.Id == chiTiet.ThongBaoChiTietHDRaSoatId).LyDoRaSoat : string.Empty,
                                                           ChungTuLienQuan = chiTiet.ChungTuLienQuan,
                                                           DienGiaiTrangThai = chiTiet.DienGiaiTrangThai,
                                                           TrangThaiHoaDon = chiTiet.TrangThaiHoaDon,
                                                           LaHoaDonNgoaiHeThong = thongDiep.IsTBaoHuyGiaiTrinhKhacCuaNNT
                                                       }
                                                      ).ToList()
                        };

            var result = await query.FirstOrDefaultAsync();

            if (@params.IsTraVeThongDiepChung)
            {
                //nếu có cần trả về id thông điệp chung
                var thongDiepChungId = await (from thongDiep in _db.ThongDiepChungs
                                              where thongDiep.MaLoaiThongDiep == MaLoaiThongDiep && thongDiep.ThongDiepGuiDi && thongDiep.IdThamChieu == result.Id
                                              select thongDiep.ThongDiepChungId).FirstOrDefaultAsync();
                result.ThongDiepChungId = thongDiepChungId;
            }
            return result;
        }

        /// <summary>
        /// KiemTraTrungThongBaoHoaDonSaiSot kiểm tra đã lập thông báo sai sót cho hóa đơn điện tử hay chưa
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<ThongBaoSaiSotSearch>> KiemTraHoaDonDaLapThongBaoSaiSotAsync(List<ThongBaoSaiSotSearch> @params)
        {
            var query = await (from hoadon in _db.ThongDiepChiTietGuiCQTs
                               where
                               @params.Count(x => x.MauHoaDon.TrimToUpper() == hoadon.MauHoaDon.TrimToUpper() &&
                               x.KyHieuHoaDon.TrimToUpper() == hoadon.KyHieuHoaDon.TrimToUpper() &&
                               x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.TrimToUpper() &&
                               x.NgayLapHoaDon == hoadon.NgayLapHoaDon.Value.ToString("yyyy-MM-dd")
                               ) > 0
                               select new ThongBaoSaiSotSearch
                               {
                                   MauHoaDon = hoadon.MauHoaDon,
                                   KyHieuHoaDon = hoadon.KyHieuHoaDon,
                                   SoHoaDon = hoadon.SoHoaDon,
                                   NgayLapHoaDon = hoadon.NgayLapHoaDon.Value.ToString("yyyy-MM-dd")
                               }).ToListAsync();
            return query;
        }

        /// <summary>
        /// KiemTraHoaDonDaNhapTrungVoiHoaDonHeThong kiểm tra hóa đơn đã nhập (trường hợp gửi thông báo 04 của NNT (Khác)) 
        /// bị trùng với các hóa đơn trong hệ thống
        /// trong trường hợp này trên giao diện đã chọn Hủy/Giải trình
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<HoaDonHeThongViewModel>> KiemTraHoaDonDaNhapTrungVoiHoaDonHeThongAsync(List<ThongBaoSaiSotSearch> @params)
        {
            var query = await (from hoadon in _db.HoaDonDienTus
                               join bkhhd in _db.BoKyHieuHoaDons on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                               select new HoaDonHeThongViewModel
                               {
                                   HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                   MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                   KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                   SoHoaDon = hoadon.SoHoaDon,
                                   NgayLapHoaDon = hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                   ThayTheChoHoaDonId = hoadon.ThayTheChoHoaDonId,
                                   DieuChinhChoHoaDonId = hoadon.DieuChinhChoHoaDonId
                               }).ToListAsync();

            var listIdHoaDonBiThayThe = (from hoadon in query
                                         where string.IsNullOrWhiteSpace(hoadon.ThayTheChoHoaDonId) == false
                                         select hoadon.ThayTheChoHoaDonId).ToList();
            var listHoaDonBiThayThe = query.Where(x => listIdHoaDonBiThayThe.Contains(x.HoaDonDienTuId)).ToList();


            var listIdHoaDonBiDieuChinh = (from hoadon in query
                                           where string.IsNullOrWhiteSpace(hoadon.DieuChinhChoHoaDonId) == false
                                           select hoadon.DieuChinhChoHoaDonId).ToList();
            var listHoaDonBiDieuChinh = query.Where(x => listIdHoaDonBiDieuChinh.Contains(x.HoaDonDienTuId)).ToList();

            //kiểm tra trùng chung với hóa đơn hệ thống
            var listTrungHoaDonHeThong = (from hoadon in query
                                          where
                          @params.Count(x => x.MauHoaDon.TrimToUpper() == hoadon.MauHoaDon.TrimToUpper() &&
                          x.KyHieuHoaDon.TrimToUpper() == hoadon.KyHieuHoaDon.TrimToUpper() &&
                          x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.ToString().TrimToUpper() &&
                          x.NgayLapHoaDon == hoadon.NgayLapHoaDon) > 0
                                          select new HoaDonHeThongViewModel
                                          {
                                              HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                              MauHoaDon = hoadon.MauHoaDon,
                                              KyHieuHoaDon = hoadon.KyHieuHoaDon,
                                              SoHoaDon = hoadon.SoHoaDon,
                                              NgayLapHoaDon = hoadon.NgayLapHoaDon,
                                              PhanLoaiTrungHoaDon = 1
                                          }).ToList();

            //kiểm tra trùng với hóa đơn bị thay thế thì hiển thị ra hóa đơn thay thế liên quan
            var listTrungHoaDonBiThayThe = (from hoadon in listHoaDonBiThayThe
                                            where
                          @params.Count(x => x.MauHoaDon.TrimToUpper() == hoadon.MauHoaDon.TrimToUpper() &&
                          x.KyHieuHoaDon.TrimToUpper() == hoadon.KyHieuHoaDon.TrimToUpper() &&
                          x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.ToString().TrimToUpper() &&
                          x.NgayLapHoaDon == hoadon.NgayLapHoaDon) > 0
                                            select new HoaDonHeThongViewModel
                                            {
                                                HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                MauHoaDon = hoadon.MauHoaDon,
                                                KyHieuHoaDon = hoadon.KyHieuHoaDon,
                                                SoHoaDon = hoadon.SoHoaDon,
                                                NgayLapHoaDon = hoadon.NgayLapHoaDon,
                                                PhanLoaiTrungHoaDon = 2,

                                                MauHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.MauHoaDon,
                                                KyHieuHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.KyHieuHoaDon,
                                                SoHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.SoHoaDon,
                                                NgayLapHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.NgayLapHoaDon
                                            }).ToList();

            //kiểm tra trùng với hóa đơn bị điều chỉnh thì hiển thị ra hóa đơn điều chỉnh liên quan
            var listTrungHoaDonBiDieuChinh = (from hoadon in listHoaDonBiDieuChinh
                                              where
                          @params.Count(x => x.MauHoaDon.TrimToUpper() == hoadon.MauHoaDon.TrimToUpper() &&
                          x.KyHieuHoaDon.TrimToUpper() == hoadon.KyHieuHoaDon.TrimToUpper() &&
                          x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.ToString().TrimToUpper() &&
                          x.NgayLapHoaDon == hoadon.NgayLapHoaDon) > 0
                                              select new HoaDonHeThongViewModel
                                              {
                                                  HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                  MauHoaDon = hoadon.MauHoaDon,
                                                  KyHieuHoaDon = hoadon.KyHieuHoaDon,
                                                  SoHoaDon = hoadon.SoHoaDon,
                                                  NgayLapHoaDon = hoadon.NgayLapHoaDon,
                                                  PhanLoaiTrungHoaDon = 3,

                                                  MauHoaDonDieuChinh = query.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hoadon.HoaDonDienTuId)?.MauHoaDon,
                                                  KyHieuHoaDonDieuChinh = query.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hoadon.HoaDonDienTuId)?.KyHieuHoaDon,
                                                  SoHoaDonDieuChinh = query.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hoadon.HoaDonDienTuId)?.SoHoaDon,
                                                  NgayLapHoaDonDieuChinh = query.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hoadon.HoaDonDienTuId)?.NgayLapHoaDon
                                              }).ToList();

            return (listTrungHoaDonHeThong.Union(listTrungHoaDonBiThayThe).Union(listTrungHoaDonBiDieuChinh)).ToList();
        }

        /// <summary>
        /// GetListHoaDonSaiSotAsync trả về danh sách các hóa đơn sai sót
        /// </summary>
        /// <param name="params"></param>
        /// <returns>List<HoaDonSaiSotViewModel></returns>
        public async Task<List<HoaDonSaiSotViewModel>> GetListHoaDonSaiSotAsync(HoaDonSaiSotParams @params)
        {
            try
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;
                List<HoaDonDienTu> queryHoaDonDienTu = new List<HoaDonDienTu>();
                List<HoaDonSaiSotViewModel> query = new List<HoaDonSaiSotViewModel>();

                var hoaDonDienTus = await _db.HoaDonDienTus.ToListAsync();

                //query để đếm số lần gửi hóa đơn tới CQT
                var querySoLanGuiCQT = await (from thongDiep in _db.ThongDiepGuiCQTs
                                              join thongDiepChiTiet in _db.ThongDiepChiTietGuiCQTs on thongDiep.Id equals thongDiepChiTiet.ThongDiepGuiCQTId
                                              where thongDiep.DaKyGuiCQT == true
                                              select new SoLanGuiHoaDonToiCQT_ViewModel { ThongDiepGuiCQTId = thongDiep.Id, HoaDonDienTuId = thongDiepChiTiet.HoaDonDienTuId }).ToListAsync();

                var queryBoKyHieuHoaDon = await (from boKyHieuHoaDon in _db.BoKyHieuHoaDons
                                                 where boKyHieuHoaDon.PhuongThucChuyenDL == PhuongThucChuyenDL.CDDu //chỉ lấy dữ bộ ký hiệu với phương thức chuyển dữ liệu từng hóa đơn
                                                 select new DLL.Entity.QuanLy.BoKyHieuHoaDon
                                                 {
                                                     BoKyHieuHoaDonId = boKyHieuHoaDon.BoKyHieuHoaDonId,
                                                     HinhThucHoaDon = boKyHieuHoaDon.HinhThucHoaDon,
                                                     KyHieuMauSoHoaDon = boKyHieuHoaDon.KyHieuMauSoHoaDon,
                                                     KyHieuHoaDon = boKyHieuHoaDon.KyHieuHoaDon
                                                 }).ToListAsync();

                if (string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                {
                    if (!string.IsNullOrWhiteSpace(@params.FromDate))
                    {
                        fromDate = DateTime.Parse(@params.FromDate);
                    }
                    if (!string.IsNullOrWhiteSpace(@params.ToDate))
                    {
                        toDate = DateTime.Parse(@params.ToDate);
                    }
                    queryHoaDonDienTu = await _db.HoaDonDienTus.Include(x => x.BoKyHieuHoaDon).Where(x => x.BoKyHieuHoaDon.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon
                                            && (DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null) && (DateTime.Parse(x.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)
                                        ).ToListAsync();
                }
                else
                {
                    queryHoaDonDienTu = await (from hoadon in _db.HoaDonDienTus.Include(x => x.BoKyHieuHoaDon)
                                               where hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId
                                               || hoadon.ThayTheChoHoaDonId == @params.LapTuHoaDonDienTuId
                                               //|| hoadon.DieuChinhChoHoaDonId == @params.LapTuHoaDonDienTuId
                                               select hoadon).ToListAsync();
                    if (queryHoaDonDienTu.Count > 0 && @params.IsTBaoHuyGiaiTrinhKhacCuaNNT != true)
                    {
                        @params.TrangThaiGuiHoaDon = (int)(queryHoaDonDienTu.FirstOrDefault(x => x.HoaDonDienTuId == @params.LapTuHoaDonDienTuId)?.TrangThaiGuiHoaDon.GetValueOrDefault());
                    }
                }

                if (@params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                {
                    if (!string.IsNullOrWhiteSpace(@params.FromDate) && string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                    {
                        fromDate = DateTime.Parse(@params.FromDate);
                    }
                    if (!string.IsNullOrWhiteSpace(@params.ToDate) && string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                    {
                        toDate = DateTime.Parse(@params.ToDate);
                    }

                    query = await (from hoadon in _db.ThongTinHoaDons
                                   from hoaDonHeThong in queryHoaDonDienTu
                                   join bkhhd in queryBoKyHieuHoaDon on hoaDonHeThong.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                   join bbxb in _db.BienBanXoaBos on hoadon.Id equals bbxb.ThongTinHoaDonId into tmpBienBanXoaBos
                                   from bbxb in tmpBienBanXoaBos.DefaultIfEmpty()
                                   where
                                   (hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon
                                   && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe
                                   && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GuiKhongLoi
                                   && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChoPhanHoi
                                   && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChuaGui
                                   )
                                   && (hoadon.Id == hoaDonHeThong.ThayTheChoHoaDonId || hoadon.Id == hoaDonHeThong.DieuChinhChoHoaDonId)
                                   &&
                                   (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                   && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null) && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.Id == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                                   && bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon

                                   //nếu là hóa đơn bị thay thế thì hóa đơn thay thế đó phải được cấp mã rồi
                                   && ((!string.IsNullOrWhiteSpace(hoaDonHeThong.ThayTheChoHoaDonId) && hoaDonHeThong.SoHoaDon.HasValue && (hoaDonHeThong.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa || hoaDonHeThong.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hoaDonHeThong.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe)) || string.IsNullOrWhiteSpace(hoaDonHeThong.ThayTheChoHoaDonId))

                                   //nếu là hóa đơn bị điều chỉnh thì hóa đơn điều chỉnh đó phải được cấp mã rồi
                                   && ((!string.IsNullOrWhiteSpace(hoaDonHeThong.DieuChinhChoHoaDonId) && hoaDonHeThong.SoHoaDon.HasValue && (hoaDonHeThong.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa || hoaDonHeThong.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu || hoaDonHeThong.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe)) || string.IsNullOrWhiteSpace(hoaDonHeThong.DieuChinhChoHoaDonId))

                                   select new HoaDonSaiSotViewModel
                                   {
                                       SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.Id).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                       HoaDonDienTuId = hoadon.Id,
                                       ChungTuLienQuan =
                                                    string.Format("{0}-{1}-{2};{3}", (bkhhd.KyHieuMauSoHoaDon.ToString() + (bkhhd.KyHieuHoaDon ?? "")), hoaDonHeThong.SoHoaDon, hoaDonHeThong.NgayHoaDon.Value.ToString("dd/MM/yyyy"), hoaDonHeThong.HoaDonDienTuId),
                                       TrangThaiHoaDon = hoadon.TrangThaiHoaDon.GetValueOrDefault(),
                                       DienGiaiTrangThai = "&nbsp;|&nbsp;" + GetDienGiaiTrangThaiHoaDonKhacCuaNNT(hoaDonHeThong.ThayTheChoHoaDonId, hoaDonHeThong.DieuChinhChoHoaDonId),
                                       PhanLoaiHDSaiSot = !string.IsNullOrWhiteSpace(hoaDonHeThong.DieuChinhChoHoaDonId) ? (byte)2 : (byte)3,
                                       PhanLoaiHDSaiSotMacDinh = !string.IsNullOrWhiteSpace(hoaDonHeThong.DieuChinhChoHoaDonId) ? (byte)2 : (byte)3,
                                       LoaiApDungHDDT = hoadon.HinhThucApDung,
                                       TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)hoadon.HinhThucApDung).GetDescription(),
                                       MaCQTCap = hoadon.MaCQTCap,
                                       MauHoaDon = hoadon.MauSoHoaDon,
                                       KyHieuHoaDon = hoadon.KyHieuHoaDon,
                                       StrSoHoaDon = hoadon.SoHoaDon,
                                       NgayLapHoaDon = hoadon.NgayHoaDon,
                                       LoaiSaiSotDeTimKiem = 0,
                                       LyDo = bbxb.LyDoXoaBo,
                                       LaHoaDonNgoaiHeThong = true
                                   }).ToListAsync();
                }
                else
                {
                    //listEmail lưu danh sách các email đã gửi
                    List<string> listEmail = await (from email in _db.NhatKyGuiEmails
                                                    where email.TrangThaiGuiEmail != 0 && ((int)email.TrangThaiGuiEmail != 2)
                                                    && !string.IsNullOrWhiteSpace(email.So)
                                                    select email.RefId).ToListAsync();

                    List<ThongBaoSaiThongTin> queryThongBaoSaiThongTin = new List<ThongBaoSaiThongTin>();
                    if (string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                    {
                        queryThongBaoSaiThongTin = await _db.ThongBaoSaiThongTins.ToListAsync();
                    }
                    else
                    {
                        queryThongBaoSaiThongTin = await _db.ThongBaoSaiThongTins.Where(x => x.HoaDonDienTuId == @params.LapTuHoaDonDienTuId).ToListAsync();
                    }

                    if (!string.IsNullOrWhiteSpace(@params.FromDate) && string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                    {
                        fromDate = DateTime.Parse(@params.FromDate);
                    }
                    if (!string.IsNullOrWhiteSpace(@params.ToDate) && string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                    {
                        toDate = DateTime.Parse(@params.ToDate);
                    }

                    var queryHoaDonHuyChuaGui = from hoadon in queryHoaDonDienTu
                                                join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                where
                                                (hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon
                                                && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe
                                                && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GuiKhongLoi
                                                && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChoPhanHoi
                                                && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChuaGui
                                                )
                                                && !listEmail.Contains(hoadon.HoaDonDienTuId)
                                        && bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon
                                        && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                        && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null) &&
                                                (hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1
                                                || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4
                                                || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc6)
                                                && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                                                select new HoaDonSaiSotViewModel
                                                {
                                                    SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                                    HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                    ChungTuLienQuan = string.Format("{0};{1}", (hoadon.SoCTXoaBo ?? ""), "XHD-" + hoadon.HoaDonDienTuId),
                                                    TrangThaiHoaDon = 2,
                                                    DienGiaiTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc (SS)",
                                                    PhanLoaiHDSaiSot = 1,
                                                    PhanLoaiHDSaiSotMacDinh = 1,
                                                    LoaiApDungHDDT = 1,
                                                    TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                                    MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                                    MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                                    KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                                    SoHoaDon = hoadon.SoHoaDon,
                                                    NgayLapHoaDon = hoadon.NgayHoaDon,
                                                    LoaiSaiSotDeTimKiem = 2, //hủy hóa đơn do sai sót dựa trên giao diện
                                                    LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin, ""),
                                                    IdsChungTuLienQuan = GetIdChungTuLienQuan(hoadon.HoaDonDienTuId, queryHoaDonDienTu, false)
                                                };
                    query = queryHoaDonHuyChuaGui.ToList();


                    var queryHoaDonHuy = from hoadon in queryHoaDonDienTu
                                         join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                         where
                                         (hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GuiKhongLoi
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChoPhanHoi
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChuaGui
                                         )
                                 && listEmail.Contains(hoadon.HoaDonDienTuId)
                                 && bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon
                                 && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                 && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null) &&
                                         (hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2
                                         || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3
                                         || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5)
                                         &&
                                         ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))

                                            //nếu chọn HinhThuc2 hoặc HinhThuc5 thì hóa đơn thay thế phải được cấp mã rồi 
                                            && (queryHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId && x.SoHoaDon.HasValue && (hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2 || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5)
                                                && (x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa || x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe)).OrderByDescending(y => y.CreatedDate).Take(1).FirstOrDefault() != null || (hoadon.HinhThucXoabo != (int)HinhThucXoabo.HinhThuc2 && hoadon.HinhThucXoabo != (int)HinhThucXoabo.HinhThuc5))

                                         select new HoaDonSaiSotViewModel
                                         {
                                             SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                             HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                             ChungTuLienQuan = XacDinhSoChungTuLienQuan("huy_va_thaythe", XacDinhTrangThaiHoaDon(hoadon, bkhhd, queryHoaDonDienTu), hoadon, queryHoaDonDienTu, bkhhd),
                                             TrangThaiHoaDon = XacDinhTrangThaiHoaDon(hoadon, bkhhd, queryHoaDonDienTu),
                                             DienGiaiTrangThai = GetDienGiaiTrangThai(hoadon, bkhhd, queryHoaDonDienTu),
                                             PhanLoaiHDSaiSot = (byte)GetGoiY(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                             PhanLoaiHDSaiSotMacDinh = (byte)GetGoiY(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                             LoaiApDungHDDT = 1,
                                             TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                             MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                             MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                             KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                             SoHoaDon = hoadon.SoHoaDon,
                                             NgayLapHoaDon = hoadon.NgayHoaDon,
                                             LoaiSaiSotDeTimKiem = XacDinhLoaiSaiSotDuaTrenGiaoDien(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo),
                                             LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin, ""),
                                             IdsChungTuLienQuan = GetIdChungTuLienQuan(hoadon.HoaDonDienTuId, queryHoaDonDienTu, false),
                                             LoaiDieuChinh = hoadon.LoaiDieuChinh
                                         };
                    var lstHuy = queryHoaDonHuy.ToList();

                    var queryThamChieuHoaDonSaiThongTin = await (from hoadon in _db.NhatKyGuiEmails
                                                                 where hoadon.LoaiEmail == LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon && !string.IsNullOrWhiteSpace(hoadon.So)
                                                                 && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.RefId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                                                                 select hoadon.RefId).ToListAsync();
                    var queryHoaDonSaiThongTin = from hoadon in queryHoaDonDienTu
                                                 join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                 where
                                                 (hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GuiKhongLoi
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChoPhanHoi
                                         && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChuaGui
                                         ) && listEmail.Contains(hoadon.HoaDonDienTuId)
                                 && bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon
                                 && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                 && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null) && queryThamChieuHoaDonSaiThongTin.Contains(hoadon.HoaDonDienTuId)
                                 && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
                                                 select new HoaDonSaiSotViewModel
                                                 {
                                                     SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                                     HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                     ChungTuLienQuan = hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD != null ? string.Format("{0}{1};{2}", "TBSSTT-Email-", hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD.Value.ToString("dd/MM/yyyy HH:mm:ss"), (hoadon.EmailTBaoSaiSotKhongPhaiLapHDId ?? "")) : "",
                                                     TrangThaiHoaDon = XacDinhTrangThaiHoaDon(hoadon, bkhhd, queryHoaDonDienTu),
                                                     DienGiaiTrangThai = "",
                                                     PhanLoaiHDSaiSot = 4,
                                                     PhanLoaiHDSaiSotMacDinh = 4,
                                                     LoaiApDungHDDT = 1,
                                                     TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                                     LaThongTinSaiSot = true,
                                                     MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                                     MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                                     KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                                     SoHoaDon = hoadon.SoHoaDon,
                                                     NgayLapHoaDon = hoadon.NgayHoaDon,
                                                     LoaiSaiSotDeTimKiem = 0, //thông báo sai sót thông tin dựa trên giao diện
                                                     LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin, "emailThongBaoSaiThongTin"),
                                                     IdsChungTuLienQuan = GetIdChungTuLienQuan(hoadon.HoaDonDienTuId, queryHoaDonDienTu, true),
                                                     LoaiDieuChinh = hoadon.LoaiDieuChinh
                                                 };

                    var lstHoaDonSaiThongTin = queryHoaDonSaiThongTin.ToList();
                    var queryThamChieuHoaDonBiDieuChinh = from hoadon in queryHoaDonDienTu
                                                          join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                          where !string.IsNullOrWhiteSpace(hoadon.DieuChinhChoHoaDonId)
                                                          && ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa && hoadon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                          || (bkhhd.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && hoadon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe))
                                                          select hoadon.DieuChinhChoHoaDonId;

                    var queryHoaDonBiDieuChinh = from hoadon in queryHoaDonDienTu
                                                 join dc in queryHoaDonDienTu on hoadon.HoaDonDienTuId equals dc.DieuChinhChoHoaDonId
                                                 join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                 join bkhhd_dc in queryBoKyHieuHoaDon on dc.BoKyHieuHoaDonId equals bkhhd_dc.BoKyHieuHoaDonId
                                                 where
                                                 ((hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon
                                                     && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe
                                                     && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GuiKhongLoi
                                                     && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChoPhanHoi
                                                     && hoadon.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChuaGui
                                                  )
                                                 || (dc.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.CQTTiepNhanTatCaHoaDon
                                                     && dc.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe
                                                     && dc.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.GuiKhongLoi
                                                     && dc.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChoPhanHoi
                                                     && dc.TrangThaiGui04 != (int)TrangThaiGuiThongDiep.ChuaGui
                                                  )
                                                 || (_db.HoaDonDienTus.Where(x => x.DieuChinhChoHoaDonId == hoadon.DieuChinhChoHoaDonId).Count() > 1
                                                 && _db.ThongDiepChiTietGuiCQTs.Count(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId && x.PhanLoaiHDSaiSot == 2) <= _db.HoaDonDienTus.Where(x => x.DieuChinhChoHoaDonId == hoadon.HoaDonDienTuId).Count())
                                                 )
                                                 && dc.IsDaLapThongBao04 != true
                                                 && listEmail.Contains(hoadon.HoaDonDienTuId)
                                                 && bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon
                                                 && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                                 && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null) && queryThamChieuHoaDonBiDieuChinh.Contains(hoadon.HoaDonDienTuId)
                                                 && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))

                                                 //nếu là hóa đơn gốc bị điều chỉnh thì bắt buộc hóa đơn điều chỉnh phải được cấp mã
                                                 && (queryHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoadon.HoaDonDienTuId && x.SoHoaDon.HasValue && ((x.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa && x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) || (x.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe))).OrderByDescending(y => y.CreatedDate).Take(1).FirstOrDefault() != null)
                                                 select new HoaDonSaiSotViewModel
                                                 {
                                                     SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                                     HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                     ChungTuLienQuan = XacDinhSoChungTuLienQuan("dieuchinh", null, hoadon, queryHoaDonDienTu, bkhhd, dc, bkhhd_dc),
                                                     TrangThaiHoaDon = 1, //chỉ là hóa đơn gốc
                                                     DienGiaiTrangThai = "&nbsp;|&nbsp;Bị điều chỉnh",
                                                     PhanLoaiHDSaiSot = 2,
                                                     PhanLoaiHDSaiSotMacDinh = 2,
                                                     LoaiApDungHDDT = 1,
                                                     TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                                     MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                                     MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                                     KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                                     SoHoaDon = hoadon.SoHoaDon,
                                                     NgayLapHoaDon = hoadon.NgayHoaDon,
                                                     LoaiSaiSotDeTimKiem = 4, //hóa đơn bị điều chỉnh dựa trên giao diện
                                                     LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin, "hoaDonBiDieuChinh", dc, bkhhd_dc),
                                                     IdsChungTuLienQuan = GetIdChungTuLienQuan(hoadon.HoaDonDienTuId, queryHoaDonDienTu, false)
                                                 };

                    var bdc = queryHoaDonBiDieuChinh.ToList();
                    queryHoaDonBiDieuChinh = queryHoaDonBiDieuChinh.Where(x => _db.ThongDiepChiTietGuiCQTs.Count(o => o.ChungTuLienQuan == x.ChungTuLienQuan) < 1 || (x.ChungTuLienQuan.Split(";").Length > 1 && hoaDonDienTus.FirstOrDefault(y => y.HoaDonDienTuId == x.ChungTuLienQuan.Split(";")[1]).IsDaLapThongBao04 == false));

                    query = (queryHoaDonHuy.Union(queryHoaDonSaiThongTin).Union(queryHoaDonBiDieuChinh).Union(queryHoaDonHuyChuaGui)).ToList();
                }

                if (string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && @params.LoaiSaiSot != -1)
                {
                    query = query.Where(x => x.LoaiSaiSotDeTimKiem == @params.LoaiSaiSot).ToList();
                }

                foreach (var item in query)
                {
                    item.SoLanGuiCQT = await (from td in _db.ThongDiepGuiCQTs
                                              join ct in _db.ThongDiepChiTietGuiCQTs on td.Id equals ct.ThongDiepGuiCQTId
                                              where td.DaKyGuiCQT == true && ct.HoaDonDienTuId == item.HoaDonDienTuId && ct.ChungTuLienQuan == item.ChungTuLienQuan && ct.PhanLoaiHDSaiSot == item.PhanLoaiHDSaiSot
                                              select ct).CountAsync();
                }

                //nếu người dùng bấm nút lập hóa đơn từ cột thông tin sai sót thì lọc đúng vào dòng họ đã click
                if (!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && !string.IsNullOrWhiteSpace(@params.HoaDonDienTuIdLienQuan))
                {
                    query = query.Where(x => GetListIdChungTuLienQuan(x.IdsChungTuLienQuan).Contains(@params.HoaDonDienTuIdLienQuan)).ToList();
                }

                //lọc theo tìm kiếm theo
                if (@params.TimKiemTheo != null)
                {
                    var timKiemTheo = @params.TimKiemTheo;
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.MauHoaDon))
                    {
                        var keyword = timKiemTheo.MauHoaDon.ToUpper().ToTrim();
                        query = query.Where(x => x.MauHoaDon != null && x.MauHoaDon.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.KyHieuHoaDon))
                    {
                        var keyword = timKiemTheo.KyHieuHoaDon.ToUpper().ToTrim();
                        query = query.Where(x => x.KyHieuHoaDon != null && x.KyHieuHoaDon.ToUpper().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.SoHoaDon))
                    {
                        var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                        query = query.Where(x => x.SoHoaDon.HasValue && x.SoHoaDon.ToString().ToTrim().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.NgayLapHoaDon))
                    {
                        var keyword = timKiemTheo.NgayLapHoaDon.ToTrim();
                        query = query.Where(x => x.NgayLapHoaDon != null && x.NgayLapHoaDon.Value.ToString("dd/MM/yyyy").ToTrim().Contains(keyword)).ToList();
                    }
                }
                else
                {
                    //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                    if (string.IsNullOrWhiteSpace(@params.TimKiemBatKy) == false)
                    {
                        @params.TimKiemBatKy = @params.TimKiemBatKy.ToUpper().ToTrim();
                        query = query.Where(x =>
                            (x.MauHoaDon != null && x.MauHoaDon.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                            (x.KyHieuHoaDon != null && x.KyHieuHoaDon.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                            (x.SoHoaDon.HasValue && x.SoHoaDon.ToString().ToTrim().Contains(@params.TimKiemBatKy)) ||
                            (x.NgayLapHoaDon != null && x.NgayLapHoaDon.Value.ToString("dd/MM/yyyy").ToTrim().Contains(@params.TimKiemBatKy))
                        ).ToList();
                    }
                }

                //order by kết quả
                //query = query.OrderBy(x => x.MaCQTCap).ThenByDescending(x => x.MauHoaDon).ThenByDescending(x => x.KyHieuHoaDon).ThenByDescending(x => x.SoHoaDon).ToList();
                query = query.OrderBy(x => x.NgayLapHoaDon).ThenBy(x => x.MauHoaDon).ThenBy(x => x.SoHoaDon).ToList();
                //lọc trên cột
                if (@params.FilterColumns != null)
                {
                    @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                    for (int i = 0; i < @params.FilterColumns.Count; i++)
                    {
                        var item = @params.FilterColumns[i];
                        if (item.ColKey == "chungTu")
                        {
                            query = GenericFilterColumn<HoaDonSaiSotViewModel>.Query(query, x => x.ChungTuLienQuan, item, FilterValueType.String).ToList();
                        }
                        if (item.ColKey == "maCQTCap")
                        {
                            query = GenericFilterColumn<HoaDonSaiSotViewModel>.Query(query, x => x.MaCQTCap, item, FilterValueType.String).ToList();
                        }
                        if (item.ColKey == "soHoaDon")
                        {
                            query = GenericFilterColumn<HoaDonSaiSotViewModel>.Query(query, x => x.SoHoaDon, item, FilterValueType.String).ToList();
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(@params.SortKey))
                {
                    if (@params.SortKey == "ChungTuLienQuan" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.ChungTuLienQuan).ToList();
                    }
                    if (@params.SortKey == "ChungTuLienQuan" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.ChungTuLienQuan).ToList();
                    }

                    if (@params.SortKey == "MaCQTCap" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaCQTCap).ToList();
                    }
                    if (@params.SortKey == "MaCQTCap" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaCQTCap).ToList();
                    }

                    if (@params.SortKey == "MauHoaDon" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MauHoaDon + x.KyHieuHoaDon).ToList();
                    }
                    if (@params.SortKey == "MauHoaDon" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MauHoaDon + x.KyHieuHoaDon).ToList();
                    }

                    if (@params.SortKey == "SoHoaDon" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoHoaDon).ToList();
                    }
                    if (@params.SortKey == "SoHoaDon" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoHoaDon).ToList();
                    }

                    if (@params.SortKey == "NgayLapHoaDon" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayLapHoaDon).ToList();
                    }
                    if (@params.SortKey == "NgayLapHoaDon" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayLapHoaDon).ToList();
                    }
                }

                return query;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GetDanhSachDiaDanhAsync trả về danh sách các địa danh theo Thông tư số 78/2021/TT-BTC 
        /// </summary>
        /// <returns></returns>
        public async Task<List<DiaDanhParam>> GetDanhSachDiaDanhAsync()
        {
            var query = (from diaDanh in _db.DiaDanhs
                         orderby ConvertToNumber(diaDanh.Ma)
                         select new DiaDanhParam
                         {
                             code = diaDanh.Ma,
                             name = diaDanh.Ten
                         }).ToListAsync();
            /*
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\dia-danh.json";
            var list = new List<DiaDanhParam>().Deserialize(path).ToList();
            */

            return await query;
        }

        /// <summary>
        /// InsertThongBaoGuiHoaDonSaiSotAsync thêm bản ghi thông điệp gửi cơ quan thuế
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ThongDiepGuiCQTViewModel</returns>
        public async Task<KetQuaLuuThongDiep> InsertThongBaoGuiHoaDonSaiSotAsync(ThongDiepGuiCQTViewModel model)
        {
            ThongDiepChung thongDiepChung = null;
            if (string.IsNullOrWhiteSpace(model.Id) == false)
            {
                //nếu đã có bản ghi thì xóa trước khi lưu (đây là trường hợp sửa và lưu)
                var thongDiepChiTietGuiCQTs = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == model.Id).ToListAsync();
                _db.ThongDiepChiTietGuiCQTs.RemoveRange(thongDiepChiTietGuiCQTs);
                var ketQuaXoa = await _db.SaveChangesAsync();
                if (ketQuaXoa > 0)
                {
                    var thongDiepGuiCQT = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == model.Id);
                    _db.ThongDiepGuiCQTs.Remove(thongDiepGuiCQT);
                    await _db.SaveChangesAsync();

                    //xóa bản ghi ở bảng thông điệp chung
                    thongDiepChung = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == model.Id);
                    _db.ThongDiepChungs.Remove(thongDiepChung);
                    await _db.SaveChangesAsync();

                    //xóa file ở bảng filedata
                    if (thongDiepChung != null)
                    {
                        var fileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == thongDiepChung.ThongDiepChungId);
                        _db.FileDatas.Remove(fileData);
                        await _db.SaveChangesAsync();
                    }
                }
            }
            else
            {
                model.CreatedDate = DateTime.Now;
                model.Id = Guid.NewGuid().ToString();
                model.MaThongDiep = "0200784873" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper();
            }

            //thêm thông điệp gửi hóa đơn sai sót (đây là trường hợp thêm mới)
            model.ModifyDate = model.NgayGui = DateTime.Now;
            model.DaKyGuiCQT = false;
            model.SoThongBaoSaiSot = string.Format("{0} {1}", "TBSS", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            ThongDiepGuiCQT entity = _mp.Map<ThongDiepGuiCQT>(model);
            await _db.ThongDiepGuiCQTs.AddAsync(entity);
            model.Id = entity.Id;
            var ketQua = await _db.SaveChangesAsync();
            if (ketQua > 0)
            {
                //thêm thông điệp gửi hóa đơn chi tiết bị sai sót
                var STT = 1;
                foreach (var item in model.ThongDiepChiTietGuiCQTs)
                {
                    item.STT = STT;
                    item.Id = Guid.NewGuid().ToString();
                    item.ThongDiepGuiCQTId = model.Id;
                    item.ThongBaoChiTietHDRaSoatId = item.ThongBaoChiTietHDRaSoatId;
                    item.CreatedDate = item.ModifyDate = DateTime.Now;
                    STT += 1;
                }

                List<ThongDiepChiTietGuiCQT> children = _mp.Map<List<ThongDiepChiTietGuiCQT>>(model.ThongDiepChiTietGuiCQTs);
                await _db.ThongDiepChiTietGuiCQTs.AddRangeAsync(children);
                await _db.SaveChangesAsync();

                //đánh dấu hóa đơn đã lập thông báo 04
                var listIdHoaDonCanDanhDau = model.ThongDiepChiTietGuiCQTs.Select(x => x.HoaDonDienTuId).ToList();
                if (model.IsTBaoHuyGiaiTrinhKhacCuaNNT.GetValueOrDefault())
                {
                    //nếu là hóa đơn nhập từ phần mềm khác
                    var listHoaDonCanDanhDau = await _db.ThongTinHoaDons.Where(x => listIdHoaDonCanDanhDau.Contains(x.Id)).ToListAsync();
                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            item.ThongDiepGuiCQTId = model.Id;
                            item.IsDaLapThongBao04 = true;
                            item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.ChuaGui;
                        }
                        _db.ThongTinHoaDons.UpdateRange(listHoaDonCanDanhDau);
                        await _db.SaveChangesAsync();
                    }
                }
                else
                {
                    //nếu là hóa đơn nhập trong phần mềm hdbk
                    var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            item.ThongDiepGuiCQTId = model.Id;
                            item.IsDaLapThongBao04 = true;
                            item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.ChuaGui;
                        }
                        _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                        await _db.SaveChangesAsync();
                    }
                }

                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_UNSIGN}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                }
                catch (Exception) { }

                //ghi ra các file XML, Word, PDF sau khi lưu thành công
                var tenFile = "TD-" + Guid.NewGuid().ToString();
                var tDiepXML = await CreateXMLThongDiepGuiCQT(fullFolder + "/" + tenFile + ".xml", model);
                var tenFileWordPdf = await CreateWordAndPdfFile(tenFile, model);
                string fileNames = tenFile + ".xml" + ";" + tenFileWordPdf;

                //cập nhật lại file xml vào trường file đính kèm
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (entityToUpdate != null)
                {
                    entityToUpdate.FileDinhKem = fileNames;
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //khai báo biến kết quả lưu dữ liệu
                var ketQuaLuuDuLieu = new KetQuaLuuThongDiep
                {
                    Id = model.Id,
                    FileNames = fileNames,
                    FileContainerPath = $"FilesUpload/{databaseName}",
                    MaThongDiep = tDiepXML.TTChung.MTDiep,
                    CreatedDate = model.CreatedDate,
                    SoThongBaoSaiSot = model.SoThongBaoSaiSot
                };

                //thêm bản ghi vào bảng thông điệp chung để hiển thị ra bảng kê
                string thongDiepChungId = await ThemDuLieuVaoBangThongDiepChung(tDiepXML, ketQuaLuuDuLieu, thongDiepChung);

                //gán lại thongDiepChungId cho trường hợp cần
                ketQuaLuuDuLieu.ThongDiepChungId = thongDiepChungId;

                return ketQuaLuuDuLieu;
            }

            return null;
        }

        /// <summary>
        /// Xóa bản ghi thông báo hóa đơn sai sót và xóa thông điệp 300 ở tab thông điệp gửi.
        /// </summary>
        /// <param name="id">Là id của thông báo hóa đơn sai sót</param>
        /// <returns>
        /// Một task thể hiện tác vụ bất đồng bộ. Task này trả về giá trị có kiểu boolean; 
        /// giá trị này = true là xóa thành công; còn giá trị này = false là xóa không thành công.
        /// </returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var thongDiepGuiCQT = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == id);
            var thongDiepChiTietGuiCQTs = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == id).ToListAsync();

            _db.ThongDiepChiTietGuiCQTs.RemoveRange(thongDiepChiTietGuiCQTs);
            var ketQuaXoa = await _db.SaveChangesAsync();
            if (ketQuaXoa > 0)
            {
                //đánh dấu các hóa đơn ko lập thông báo 04
                var listIdHoaDonCanDanhDau = thongDiepChiTietGuiCQTs.Select(x => x.HoaDonDienTuId).ToList();
                var listIdHoaDonDaLap04 = await _db.ThongDiepChiTietGuiCQTs.Select(x => x.HoaDonDienTuId).ToListAsync();
                //lọc ra các id hóa đơn cần bỏ đánh dấu 04
                listIdHoaDonCanDanhDau = listIdHoaDonCanDanhDau.Where(x => listIdHoaDonDaLap04.Count(y => y == x) == 0).ToList();

                //đánh dấu các hóa đơn ko lập thông báo 04
                if (listIdHoaDonCanDanhDau.Count > 0)
                {
                    if (thongDiepGuiCQT.IsTBaoHuyGiaiTrinhKhacCuaNNT.GetValueOrDefault())
                    {
                        //nếu thông báo 04 được lập cho hóa đơn từ phần mềm khác
                        var listHoaDonCanDanhDau = await _db.ThongTinHoaDons.Where(x => listIdHoaDonCanDanhDau.Contains(x.Id)).ToListAsync();
                        if (listHoaDonCanDanhDau.Count > 0)
                        {
                            foreach (var item in listHoaDonCanDanhDau)
                            {
                                //reset các trạng thái của thông điệp 300 của hóa đơn
                                item.IsDaLapThongBao04 = false;
                                item.TrangThaiGui04 = null;
                                item.ThongDiepGuiCQTId = null;
                            }
                            _db.ThongTinHoaDons.UpdateRange(listHoaDonCanDanhDau);
                            await _db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        //nếu thông báo 04 được lập cho hóa đơn được nhập trong phần mềm
                        var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                        if (listHoaDonCanDanhDau.Count > 0)
                        {
                            foreach (var item in listHoaDonCanDanhDau)
                            {
                                //reset các trạng thái của thông điệp 300 của hóa đơn
                                item.IsDaLapThongBao04 = false;
                                item.TrangThaiGui04 = null;
                                item.ThongDiepGuiCQTId = null;
                            }
                            _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                            await _db.SaveChangesAsync();
                        }
                    }
                }

                //xóa bản ghi ở bảng ThongDiepGuiCQTs
                _db.ThongDiepGuiCQTs.Remove(thongDiepGuiCQT);
                var ketQuaXoa2 = await _db.SaveChangesAsync() > 0;

                if (ketQuaXoa2)
                {
                    //xóa bản ghi ở bảng thông điệp chung
                    var thongDiepChung = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == id);
                    if (thongDiepChung != null)
                    {
                        _db.ThongDiepChungs.Remove(thongDiepChung);
                        var ketQuaXoa3 = await _db.SaveChangesAsync() > 0;
                        if (ketQuaXoa3)
                        {
                            //xóa các file word, pdf, xml chưa ký đi
                            await XoaThuMucChuaFileTheoId(id);
                        }

                        return ketQuaXoa3;
                    }
                    else
                    {
                        //xóa các file word, pdf, xml chưa ký đi
                        await XoaThuMucChuaFileTheoId(id);

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// CreateXMLThongDiepGuiCQT tạo file XML chưa được ký để gửi lên cục thuế
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<TDiep> CreateXMLThongDiepGuiCQT(string xmlFilePath, ThongDiepGuiCQTViewModel model)
        {
            try
            {
                TTChung ttChung = new TTChung
                {
                    PBan = "2.0.0",
                    MNGui = "0200784873",       // "V0200784873", // "V0202029650",
                    MNNhan = "V0200784873",     //"TVAN",
                    MLTDiep = MaLoaiThongDiep,
                    MTDiep = model.MaThongDiep ?? "",
                    MTDTChieu = model.LoaiThongBao == 2 ? (model.MaTDiepThamChieu ?? "") : "", //đọc từ thông điệp nhận
                    MST = model.MaSoThue ?? "",
                    SLuong = 1
                };

                List<HDon> listHDon = new List<HDon>();
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count; i++)
                {
                    var item = model.ThongDiepChiTietGuiCQTs[i];
                    HDon hoaDon = new HDon
                    {
                        STT = i + 1,
                        MCQTCap = item.MaCQTCap ?? "", //giá trị này ở bên hóa đơn điện tử
                        KHMSHDon = item.MauHoaDon ?? "",
                        KHHDon = item.KyHieuHoaDon ?? "",
                        SHDon = item.SoHoaDon ?? "",
                        Ngay = item.NgayLapHoaDon.Value.ToString("yyyy-MM-dd"),
                        LADHDDT = item.LoaiApDungHoaDon,
                        TCTBao = item.PhanLoaiHDSaiSot,
                        LDo = item.LyDo ?? ""
                    };
                    listHDon.Add(hoaDon);
                }

                DLTBao dLTBao = new DLTBao
                {
                    PBan = "2.0.0",
                    MSo = "04/SS-HĐĐT",
                    Ten = "Thông báo hóa đơn điện tử có sai sót",
                    Loai = (model.LoaiThongBao == 3) ? (byte)1 : model.LoaiThongBao,
                    So = model.LoaiThongBao == 2 ? (model.SoTBCCQT ?? "") : "", //đọc từ thông điệp nhận
                    NTBCCQT = model.LoaiThongBao == 2 ? model.NTBCCQT.Value.ToString("yyyy-MM-dd") : "",
                    MCQT = model.MaCoQuanThue,
                    TCQT = model.TenCoQuanThue ?? "",
                    TNNT = model.NguoiNopThue ?? "",
                    MST = model.MaSoThue ?? "",
                    MDVQHNSach = "", //đọc từ thông điệp nhận sau
                    DDanh = model.DiaDanh ?? "",
                    NTBao = model.NgayLap.ToString("yyyy-MM-dd"),
                    DSHDon = listHDon
                };

                DSCKS dSCKS = new DSCKS
                {
                    NNT = ""
                };

                TBao tBao = new TBao
                {
                    DLTBao = dLTBao,
                    DSCKS = dSCKS
                };

                DLieu DLieu = new DLieu
                {
                    TBao = tBao
                };

                TDiep tDiep = new TDiep
                {
                    TTChung = ttChung,
                    DLieu = DLieu
                };

                //sau khi có các dữ liệu trên, thì lưu dữ liệu đó vào file XML
                XmlSerializerNamespaces xmlSerializingNameSpace = new XmlSerializerNamespaces();
                xmlSerializingNameSpace.Add("", "");

                XmlSerializer serialiser = new XmlSerializer(typeof(TDiep));

                using (TextWriter fileStream = new StreamWriter(xmlFilePath))
                {
                    serialiser.Serialize(fileStream, tDiep, xmlSerializingNameSpace);
                }

                string content = File.ReadAllText(xmlFilePath);
                await ThemDuLieuVaoBangFileData(model.Id, content, Path.GetFileName(xmlFilePath));

                return tDiep;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GateForWebSocket sẽ lưu file XML đã ký, và trả về đường dẫn file XML đó
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<string> GateForWebSocket(FileXMLThongDiepGuiParams @params)
        {
            try
            {
                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                try
                {
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                }
                catch (Exception) { }

                var tenFile = "TD-" + Guid.NewGuid().ToString();
                string xmlDeCode = DataHelper.Base64Decode(@params.DataXML);
                var fullDuongDanXML = fullFolder + "/" + tenFile + ".xml";
                File.WriteAllText(fullDuongDanXML, xmlDeCode);

                //lưu tên file vào database
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == @params.ThongDiepGuiCQTId);
                if (entityToUpdate != null)
                {
                    //nếu AutoCapNhatNgayLap = true; tự động cập nhật ngày lập là ngày hiện tại
                    if (@params.AutoCapNhatNgayLap.Value)
                    {
                        entityToUpdate.NgayLap = DateTime.Now;
                    }
                    entityToUpdate.NgayGui = DateTime.Now; //NgayGui dùng làm ngày ký và ngày gửi CQT, vì ký và gửi là cùng nhau
                    entityToUpdate.FileXMLDaKy = tenFile + ".xml";
                    entityToUpdate.ModifyDate = DateTime.Now;
                    entityToUpdate.SoThongBaoSaiSot = string.Format("{0} {1}", "TBSS", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //update bảng thông điệp chung
                var entityBangThongDiepChungToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == @params.ThongDiepGuiCQTId && x.MaLoaiThongDiep == MaLoaiThongDiep && x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChuaGui);
                if (entityBangThongDiepChungToUpdate != null)
                {
                    entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
                    entityBangThongDiepChungToUpdate.ModifyDate = DateTime.Now;
                    _db.ThongDiepChungs.Update(entityBangThongDiepChungToUpdate);
                    await _db.SaveChangesAsync();
                }

                //đánh dấu trạng thái gửi hóa đơn đã lập thông báo 04
                var listIdHoaDonCanDanhDau = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == @params.ThongDiepGuiCQTId).Select(x => x.HoaDonDienTuId).ToListAsync();
                if (listIdHoaDonCanDanhDau.Count > 0)
                {
                    var isTBaoHuyGiaiTrinhKhacCuaNNT = false;
                    if (entityToUpdate != null)
                    {
                        isTBaoHuyGiaiTrinhKhacCuaNNT = entityToUpdate.IsTBaoHuyGiaiTrinhKhacCuaNNT.GetValueOrDefault();
                    }

                    if (isTBaoHuyGiaiTrinhKhacCuaNNT)
                    {
                        //nếu lập 04 cho hóa đơn từ phần mềm khác
                        var listHoaDonCanDanhDau = await _db.ThongTinHoaDons.Where(x => listIdHoaDonCanDanhDau.Contains(x.Id)).ToListAsync();
                        if (listHoaDonCanDanhDau.Count > 0)
                        {
                            foreach (var item in listHoaDonCanDanhDau)
                            {
                                item.LanGui04 = (item.LanGui04 ?? 0) + 1;
                                item.IsDaLapThongBao04 = true;
                                item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
                            }
                            _db.ThongTinHoaDons.UpdateRange(listHoaDonCanDanhDau);
                            await _db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        //nếu lập 04 cho hóa đơn trong phần mềm hdbk
                        var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                        if (listHoaDonCanDanhDau.Count > 0)
                        {
                            foreach (var item in listHoaDonCanDanhDau)
                            {
                                item.IsDaLapThongBao04 = true;
                                item.LanGui04 = (item.LanGui04 ?? 0) + 1;
                                item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
                            }
                            _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                            await _db.SaveChangesAsync();
                        }

                        //Pdf, word
                        await CreateWordAndPdfFile(tenFile, _mp.Map<ThongDiepGuiCQTViewModel>(entityToUpdate), false);
                    }
                }

                return entityToUpdate.FileXMLDaKy;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// GuiThongDiepToiCQTAsync gửi dữ liệu tới cơ quan thuế
        /// </summary>
        /// <param name="DuLieuXMLGuiCQTParams"></param>
        /// <returns></returns>
        public async Task<bool> GuiThongDiepToiCQTAsync(DuLieuXMLGuiCQTParams @params)
        {
            try
            {
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

                //đường dẫn đến file xml đã ký
                var signedXmlFileFolder = fullFolder + "/" + @params.XMLFileName;

                ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhan999.TDiep thongDiep999 = null;
                bool ketQua = false;
                bool guiTCTNLoi = false; //biến này lưu kết quả có gửi được dữ liệu qua API của TVAN hay ko?

                // Gửi dữ liệu tới TVan
                var xmlContent = File.ReadAllText(signedXmlFileFolder);
                var responce999 = await _ITVanService.TVANSendData("api/error-invoice/send", xmlContent);

                if (string.IsNullOrWhiteSpace(responce999))
                {
                    guiTCTNLoi = true; //nếu ko gửi được thì có lỗi
                }
                else
                {
                    thongDiep999 = ConvertXMLDataToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhan999.TDiep>(responce999);
                    ketQua = (thongDiep999.DLieu.TBao.TTTNhan == 0);
                }

                //lưu trạng thái đã ký gửi tới cơ quan thuế hay chưa
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == @params.ThongDiepGuiCQTId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.NgayGui = DateTime.Now;
                    entityToUpdate.DaKyGuiCQT = (thongDiep999 != null);
                    entityToUpdate.ModifyDate = DateTime.Now;
                    entityToUpdate.SoThongBaoSaiSot = string.Format("{0} {1}", "TBSS", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //lưu thông tin ký gửi vào bảng thông điệp chung
                var entityBangThongDiepChungToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == @params.ThongDiepGuiCQTId && x.MaLoaiThongDiep == MaLoaiThongDiep && x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChoPhanHoi);
                if (entityBangThongDiepChungToUpdate != null)
                {
                    //cập nhật dữ liệu xml vào đây
                    var tDiep300 = ConvertXMLFileToObject<TDiep>(signedXmlFileFolder);
                    if (tDiep300 != null)
                    {
                        entityBangThongDiepChungToUpdate.MaThongDiep = tDiep300.TTChung.MTDiep;
                        entityBangThongDiepChungToUpdate.MaThongDiepThamChieu = tDiep300.TTChung.MTDTChieu;
                        entityBangThongDiepChungToUpdate.PhienBan = tDiep300.TTChung.PBan;
                        entityBangThongDiepChungToUpdate.MaNoiGui = tDiep300.TTChung.MNGui;
                        entityBangThongDiepChungToUpdate.MaNoiNhan = tDiep300.TTChung.MNNhan;
                        entityBangThongDiepChungToUpdate.MaSoThue = tDiep300.TTChung.MST;
                    }

                    if (guiTCTNLoi)
                    {
                        entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiTCTNLoi;
                    }
                    else
                    {
                        if (ketQua)
                        {
                            entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                        }
                        else
                        {
                            entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                        }
                        entityBangThongDiepChungToUpdate.MaThongDiepPhanHoi = thongDiep999.TTChung.MTDiep;
                    }

                    entityBangThongDiepChungToUpdate.NgayGui = DateTime.Now;
                    entityBangThongDiepChungToUpdate.ModifyDate = DateTime.Now;
                    entityBangThongDiepChungToUpdate.FileXML = @params.XMLFileName;
                    entityBangThongDiepChungToUpdate.NgayThongBao = DateTime.Now;

                    _db.ThongDiepChungs.Update(entityBangThongDiepChungToUpdate);
                    await _db.SaveChangesAsync();

                    // Cập nhật lại dữ liệu xml đã ký vào bảng filedatas
                    await ThemDuLieuVaoBangFileData(entityBangThongDiepChungToUpdate.ThongDiepChungId, xmlContent, @params.XMLFileName, 1, true);
                }

                //lưu thông điệp nhận 999 từ TVAN
                if (thongDiep999 != null)
                {
                    ThongDiepChung tdc999 = new ThongDiepChung
                    {
                        ThongDiepChungId = Guid.NewGuid().ToString(),
                        PhienBan = thongDiep999.TTChung.PBan,
                        MaNoiGui = thongDiep999.TTChung.MNGui,
                        MaNoiNhan = thongDiep999.TTChung.MNNhan,
                        MaLoaiThongDiep = 999,
                        MaThongDiep = thongDiep999.TTChung.MTDiep,
                        MaThongDiepThamChieu = thongDiep999.TTChung.MTDTChieu,
                        MaSoThue = thongDiep999.TTChung.MST,
                        SoLuong = 0,
                        ThongDiepGuiDi = false,
                        TrangThaiGui = (ketQua) ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = $"TD-{Guid.NewGuid()}.xml"
                    };
                    await _db.ThongDiepChungs.AddAsync(tdc999);
                    await _db.SaveChangesAsync();

                    //thêm nội dung file xml 999 vào bảng file data
                    await ThemDuLieuVaoBangFileData(tdc999.ThongDiepChungId, responce999, tdc999.FileXML, 1, true, 1);
                }

                //đánh dấu trạng thái gửi hóa đơn đã lập thông báo 04
                var listIdHoaDonCanDanhDau = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == @params.ThongDiepGuiCQTId).Select(x => x.HoaDonDienTuId).ToListAsync();
                if (listIdHoaDonCanDanhDau.Count > 0)
                {
                    var isTBaoHuyGiaiTrinhKhacCuaNNT = false;
                    if (entityToUpdate != null)
                    {
                        isTBaoHuyGiaiTrinhKhacCuaNNT = entityToUpdate.IsTBaoHuyGiaiTrinhKhacCuaNNT.GetValueOrDefault();
                    }

                    if (isTBaoHuyGiaiTrinhKhacCuaNNT)
                    {
                        //nếu lập 04 cho hóa đơn từ phần mềm khác
                        var listHoaDonCanDanhDau = await _db.ThongTinHoaDons.Where(x => listIdHoaDonCanDanhDau.Contains(x.Id)).ToListAsync();
                        if (listHoaDonCanDanhDau.Count > 0)
                        {
                            foreach (var item in listHoaDonCanDanhDau)
                            {
                                if (guiTCTNLoi)
                                {
                                    item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.GuiTCTNLoi;
                                }
                                else
                                {
                                    item.TrangThaiGui04 = (ketQua) ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi;
                                }
                            }
                            _db.ThongTinHoaDons.UpdateRange(listHoaDonCanDanhDau);
                            await _db.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        //nếu lập 04 cho hóa đơn trong phần mềm hdbk
                        var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                        if (listHoaDonCanDanhDau.Count > 0)
                        {
                            foreach (var item in listHoaDonCanDanhDau)
                            {
                                if (guiTCTNLoi)
                                {
                                    item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.GuiTCTNLoi;
                                }
                                else
                                {
                                    item.TrangThaiGui04 = (ketQua) ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi;
                                }
                            }
                            _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                            await _db.SaveChangesAsync();
                        }
                    }
                }

                return ketQua;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// CreateWordAndPdfFile tạo file Word và file PDF
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<string> CreateWordAndPdfFile(string fileName, ThongDiepGuiCQTViewModel model, bool saveToDatabase = false)
        {
            try
            {
                if(model.ThongDiepChiTietGuiCQTs == null || model.ThongDiepChiTietGuiCQTs.Count == 0)
                {
                    var param = new DataByIdParams
                    {
                        ThongDiepGuiCQTId = model.Id,
                        IsTraVeThongDiepChung = false
                    };
                    model = await GetThongDiepGuiCQTByIdAsync(param);
                }
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/QuanLy/thong-bao-hoa-don-dien-tu-co-sai-sot.docx");
                doc.LoadFromFile(docFolder);

                doc.Replace("<CoQuanThue>", model.TenCoQuanThue, true, true);
                doc.Replace("<TenNguoiNopThue>", model.NguoiNopThue, true, true);
                doc.Replace("<MaSoThue>", model.MaSoThue, true, true);
                doc.Replace("<DiaDanh>", model.DiaDanh ?? "", true, true);
                var ngayThangNam = model.NgayLap;
                doc.Replace("<NgayThangNam>", string.Format("ngày {0} tháng {1} năm {2}", ngayThangNam.Day, ngayThangNam.Month, ngayThangNam.Year), true, true);
                if(model.DaKyGuiCQT == false)
                    doc.Replace("<DaiDienNguoiNopThue>", model.NguoiNopThue, true, true);
                else
                {
                    ImageHelper.CreateSignatureBox(doc, model.NguoiNopThue, model.NgayGui, "<DaiDienNguoiNopThue>");
                }

                //thao tác với bảng dữ liệu đầu tiên
                var bangDuLieu = doc.Sections[0].Tables[0];
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count - 1; i++) //-1 vì đã có sẵn 1 dòng rồi
                {
                    // Clone row
                    TableRow newRow = bangDuLieu.Rows[2].Clone();
                    bangDuLieu.Rows.Insert(2, newRow);
                }

                //điền dữ liệu vào bảng
                TableRow row = null;
                Paragraph paragraph = null;
                ThongDiepChiTietGuiCQTViewModel item = null;
                for (int i = 0; i < model.ThongDiepChiTietGuiCQTs.Count; i++)
                {
                    item = model.ThongDiepChiTietGuiCQTs[i];

                    row = bangDuLieu.Rows[i + 2]; // +2 vì ko tính 2 dòng đầu
                    paragraph = row.Cells[0].Paragraphs[0];
                    paragraph.Text = (i + 1).ToString();

                    paragraph = row.Cells[1].Paragraphs[0];
                    paragraph.Text = item.MaCQTCap;

                    var mauHoaDon = "";
                    if (item.LoaiApDungHoaDon == 1)
                    {
                        mauHoaDon = (item.MauHoaDon ?? "") + (item.KyHieuHoaDon ?? "");
                    }
                    else
                    {
                        mauHoaDon = (item.MauHoaDon ?? "") + "-" + (item.KyHieuHoaDon ?? "");
                        mauHoaDon = mauHoaDon.Trim('-');
                    }

                    paragraph = row.Cells[2].Paragraphs[0];
                    paragraph.Text = mauHoaDon;

                    paragraph = row.Cells[3].Paragraphs[0];
                    paragraph.Text = item.SoHoaDon;

                    paragraph = row.Cells[4].Paragraphs[0];
                    paragraph.Text = item.NgayLapHoaDon.Value.ToString("dd/MM/yyyy");

                    /*
                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LoaiApDungHoaDon.GetDescription();

                    paragraph = row.Cells[6].Paragraphs[0];
                    paragraph.Text = item.PhanLoaiHDSaiSot.GetDescription();
                    */

                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LoaiApDungHoaDon.ToString();

                    paragraph = row.Cells[6].Paragraphs[0];
                    paragraph.Text = HienThiPhanLoaiHoaDonSaiSot(item.PhanLoaiHDSaiSot);

                    paragraph = row.Cells[7].Paragraphs[0];
                    paragraph.Text = item.LyDo;
                }

                //nếu lưu nội dung vào các file vật lý (.docx, .xml)
                if (saveToDatabase == false)
                {
                    //tạo thư mục để lưu các file dữ liệu
                    var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    string assetsFolder = $"FilesUpload/{databaseName}";
                    var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.FILE_ATTACH);
                    try
                    {
                        if (!Directory.Exists(fullFolder))
                        {
                            Directory.CreateDirectory(fullFolder);
                        }
                    }
                    catch (Exception) { }

                    fileName = fileName.Replace("TD", "TB");
                    //lưu file word
                    var duongDanFileWord = fullFolder + "/" + fileName + ".docx";
                    doc.SaveToFile(duongDanFileWord, FileFormat.Docx);

                    //lưu file pdf
                    var duongDanFilePdf = fullFolder + "/" + fileName + ".pdf";
                    doc.SaveToFile(duongDanFilePdf, FileFormat.PDF);

                    doc.Close();

                    await ThemAttachVaoBangFileData(model.Id, duongDanFileWord, model.DaKyGuiCQT ?? false);
                    await ThemAttachVaoBangFileData(model.Id, duongDanFilePdf, model.DaKyGuiCQT ?? false);

                    await ThemAttachVaoBangTaiLieuDinhKem(model.Id, duongDanFileWord);
                    await ThemAttachVaoBangTaiLieuDinhKem(model.Id, duongDanFilePdf);

                    var entity = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == model.Id);

                    return fileName + ".docx" + ";" + fileName + ".pdf";
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// HienThiPhanLoaiHoaDonSaiSot sẽ hiển thị chữ phân loại hóa đơn sai sót
        /// </summary>
        /// <param name="GiaTri"></param>
        /// <returns></returns>
        private string HienThiPhanLoaiHoaDonSaiSot(byte? GiaTri)
        {
            string ketQua;
            switch (GiaTri)
            {
                case 1:
                    ketQua = "Hủy";
                    break;
                case 2:
                    ketQua = "Điều chỉnh";
                    break;
                case 3:
                    ketQua = "Thay thế";
                    break;
                case 4:
                    ketQua = "Giải trình";
                    break;
                default:
                    ketQua = "";
                    break;
            }

            return ketQua;
        }

        /// <summary>
        /// XoaThuMucChuaFileTheoId sẽ xóa thư mục chứa các file word, pdf, xml chưa ký theo id bản ghi
        /// </summary>
        /// <param name="id"></param>
        private async Task XoaThuMucChuaFileTheoId(string id)
        {
            //var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
            //string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}";
            //var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            //var fullFolderWordPdf = fullFolder + $"/word_pdf/{id}"; //đường dẫn chứa các file word/pdf
            //var fullFolderUnsignedXML = fullFolder + $"/xml/unsigned/{id}"; //đường dẫn chứa file xml chưa ký
            //Directory.Delete(fullFolderWordPdf, true);
            //Directory.Delete(fullFolderUnsignedXML, true);

            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _IHttpContextAccessor);
            await uploadFile.DeleteInFileDataByRefIdAsync(id, _db);
        }

        #region Phần thêm dữ liệu vào bảng thông điệp chung để hiển thị ra bảng kê thông điệp
        /// <summary>
        /// ThemDuLieuVaoBangThongDiepChung sẽ thêm bản ghi vào bảng thông điệp chung
        /// </summary>
        /// <param name="tDiep"></param>
        /// <param name="ketQuaLuuThongDiep"></param>
        /// <returns></returns>
        private async Task<string> ThemDuLieuVaoBangThongDiepChung(TDiep tDiep, KetQuaLuuThongDiep ketQuaLuuThongDiep, ThongDiepChung thongDiepChung)
        {
            var createdDate = (thongDiepChung != null) ? thongDiepChung.CreatedDate.Value : DateTime.Now;

            ThongDiepChungViewModel model = new ThongDiepChungViewModel
            {
                ThongDiepChungId = thongDiepChung != null ? thongDiepChung.ThongDiepChungId : Guid.NewGuid().ToString(),
                MaThongDiepThamChieu = thongDiepChung != null ? thongDiepChung.MaThongDiepThamChieu : DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                ThongDiepGuiDi = true,
                MaLoaiThongDiep = tDiep.TTChung.MLTDiep,
                HinhThuc = (int)HThuc.ChinhThuc,
                TrangThaiGui = TrangThaiGuiThongDiep.ChuaGui,
                SoLuong = tDiep.TTChung.SLuong,
                NgayGui = null,
                CreatedDate = createdDate,
                ModifyDate = DateTime.Now,
                IdThamChieu = ketQuaLuuThongDiep.Id

                //lúc lưu thì ko cần lưu các trường này, chỉ có lúc ký gửi mới lưu
                //PhienBan = tDiep.TTChung.PBan,
                //MaThongDiep = tDiep.TTChung.MTDiep,
                //MaNoiGui = tDiep.TTChung.MNGui,
                //MaNoiNhan = tDiep.TTChung.MNNhan,
                //MaSoThue = tDiep.TTChung.MST,
            };

            var entity = _mp.Map<ThongDiepChung>(model);
            await _db.ThongDiepChungs.AddAsync(entity);
            var ketQua = await _db.SaveChangesAsync() > 0;

            if (ketQua)
            {
                //update lại ngày tạo CreatedDate
                var entityToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == model.ThongDiepChungId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.CreatedDate = createdDate;
                    _db.ThongDiepChungs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //thêm dữ liệu xml vào bảng filedatas
                XmlSerializer serialiser = new XmlSerializer(typeof(TDiep));
                using (var stringWriter = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(stringWriter))
                    {
                        serialiser.Serialize(writer, tDiep);
                        await ThemDuLieuVaoBangFileData(model.ThongDiepChungId, stringWriter.ToString(), null);
                    }
                }

                return model.ThongDiepChungId;
            }
            else
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// GetDSMauKyHieuHoaDon trả về danh sách mẫu ký hiệu hóa đơn
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<string>> GetDSMauKyHieuHoaDon(MauKyHieuHoaDonParams @params)
        {
            string[] loaiHoaDons = null;
            if (!string.IsNullOrWhiteSpace(@params.LoaiHoaDon))
            {
                //ko tính đến giá trị tất cả
                loaiHoaDons = @params.LoaiHoaDon.Split(';').Where(x => x != "0").ToArray();
            }

            var query = _db.HoaDonDienTus.Where(y => (string.IsNullOrWhiteSpace(y.MauSo) == false || string.IsNullOrWhiteSpace(y.KyHieu) == false)
             && (loaiHoaDons == null || (loaiHoaDons != null && loaiHoaDons.Contains(TachKyTuDauTien(y.MauSo))))
             && (string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) || (!string.IsNullOrWhiteSpace(@params.HinhThucHoaDon) && @params.HinhThucHoaDon.ToUpper() == TachKyTuDauTien(y.KyHieu).ToUpper()))
             ).Select(x => string.Format("{0}{1}", x.MauSo ?? "", x.KyHieu ?? "")).Distinct().OrderBy(z => z);

            return await query.ToListAsync();
        }

        //Method này để tách ra ký tự đầu tiên trong chuỗi
        private string TachKyTuDauTien(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.ToCharArray()[0].ToString();
        }

        #region Phần code cho trường hợp thông báo hóa đơn sai sót theo mẫu của CQT
        /// <summary>
        /// GetListHoaDonRaSoatAsync trả về danh sách các bản ghi thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<List<ThongBaoHoaDonRaSoatViewModel>> GetListHoaDonRaSoatAsync(HoaDonRaSoatParams @params)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}";

            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;

            if (string.IsNullOrWhiteSpace(@params.ThongBaoHoaDonRaSoatId) == true)
            {
                fromDate = DateTime.Parse(@params.FromDate);
                toDate = DateTime.Parse(@params.ToDate);
            }

            var query = from hoaDon in _db.ThongBaoHoaDonRaSoats
                        where
                        (
                            string.IsNullOrWhiteSpace(@params.ThongBaoHoaDonRaSoatId) == false
                            && hoaDon.Id == @params.ThongBaoHoaDonRaSoatId
                        )
                        ||
                        (
                            string.IsNullOrWhiteSpace(@params.ThongBaoHoaDonRaSoatId) == true
                            && DateTime.Parse(hoaDon.NgayThongBao.ToString("yyyy-MM-dd")) >= fromDate
                            && DateTime.Parse(hoaDon.NgayThongBao.ToString("yyyy-MM-dd")) <= toDate
                        )
                        select new ThongBaoHoaDonRaSoatViewModel
                        {
                            Id = hoaDon.Id,
                            SoThongBaoCuaCQT = hoaDon.SoThongBaoCuaCQT,
                            NgayThongBao = hoaDon.NgayThongBao,
                            TenCQTCapTren = hoaDon.TenCQTCapTren,
                            TenCQTRaThongBao = hoaDon.TenCQTRaThongBao,
                            TenNguoiNopThue = hoaDon.TenNguoiNopThue,
                            MaSoThue = hoaDon.MaSoThue,
                            NgayThoiHan = hoaDon.NgayThongBao.AddHours(hoaDon.ThoiHan),
                            Lan = hoaDon.Lan,
                            TinhTrang = hoaDon.NgayThongBao.AddDays(hoaDon.ThoiHan) > DateTime.Now,
                            //nếu tình trạng = true thì là trong hạn, ngược lại là quá hạn
                            FileDinhKem = hoaDon.FileDinhKem,
                            FileUploadPath = assetsFolder
                        };

            //lọc theo tìm kiếm theo
            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrWhiteSpace(timKiemTheo.SoThongBao))
                {
                    var keyword = timKiemTheo.SoThongBao.ToUpper().ToTrim();
                    query = query.Where(x => x.SoThongBaoCuaCQT != null && x.SoThongBaoCuaCQT.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.NgayThongBao))
                {
                    var keyword = timKiemTheo.NgayThongBao.ToTrim();
                    query = query.Where(x => x.NgayThongBao.ToString("dd/MM/yyyy").ToTrim().Contains(keyword));
                }
            }
            else
            {
                //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                if (string.IsNullOrWhiteSpace(@params.TimKiemBatKy) == false)
                {
                    @params.TimKiemBatKy = @params.TimKiemBatKy.ToUpper().ToTrim();
                    query = query.Where(x =>
                        (x.SoThongBaoCuaCQT != null && x.SoThongBaoCuaCQT.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        x.NgayThongBao.ToString("dd/MM/yyyy").ToTrim().Contains(@params.TimKiemBatKy));
                }
            }

            if (@params.FilterColumns != null)
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                for (int i = 0; i < @params.FilterColumns.Count; i++)
                {
                    var item = @params.FilterColumns[i];
                    if (item.ColKey == "soThongBaoCuaCQT")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.SoThongBaoCuaCQT, item, FilterValueType.String);
                    }
                    if (item.ColKey == "tenCQTCapTren")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.TenCQTCapTren, item, FilterValueType.String);
                    }
                    if (item.ColKey == "tenCQTRaThongBao")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.TenCQTRaThongBao, item, FilterValueType.String);
                    }
                    if (item.ColKey == "tenNguoiNopThue")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.TenNguoiNopThue, item, FilterValueType.String);
                    }
                    if (item.ColKey == "maSoThue")
                    {
                        query = GenericFilterColumn<ThongBaoHoaDonRaSoatViewModel>.Query(query, x => x.MaSoThue, item, FilterValueType.String);
                    }
                }
            }

            query = query.OrderBy(x => x.NgayThongBao).ThenBy(y => y.SoThongBaoCuaCQT);

            if (!string.IsNullOrWhiteSpace(@params.SortKey))
            {
                if (@params.SortKey == "SoThongBaoCuaCQT" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoThongBaoCuaCQT);
                }
                if (@params.SortKey == "SoThongBaoCuaCQT" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoThongBaoCuaCQT);
                }

                if (@params.SortKey == "TenCQTCapTren" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenCQTCapTren);
                }
                if (@params.SortKey == "TenCQTCapTren" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenCQTCapTren);
                }

                if (@params.SortKey == "TenCQTRaThongBao" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenCQTRaThongBao);
                }
                if (@params.SortKey == "TenCQTRaThongBao" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenCQTRaThongBao);
                }

                if (@params.SortKey == "NgayThongBao" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayThongBao);
                }
                if (@params.SortKey == "NgayThongBao" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayThongBao);
                }

                if (@params.SortKey == "TenNguoiNopThue" && @params.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenNguoiNopThue);
                }
                if (@params.SortKey == "TenNguoiNopThue" && @params.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenNguoiNopThue);
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

            return await query.ToListAsync();
        }

        /// <summary>
        /// GetListChiTietHoaDonRaSoatAsync sẽ đọc ra danh sách chi tiết thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="thongBaoHoaDonRaSoatId"></param>
        /// <returns></returns>
        public async Task<List<ThongBaoChiTietHoaDonRaSoatViewModel>> GetListChiTietHoaDonRaSoatAsync(string thongBaoHoaDonRaSoatId)
        {
            var query = from hoaDon in _db.ThongBaoChiTietHoaDonRaSoats
                        where hoaDon.ThongBaoHoaDonRaSoatId == thongBaoHoaDonRaSoatId
                        orderby hoaDon.CreatedDate
                        select new ThongBaoChiTietHoaDonRaSoatViewModel
                        {
                            Id = hoaDon.Id,
                            ThongBaoHoaDonRaSoatId = hoaDon.ThongBaoHoaDonRaSoatId,
                            MauHoaDon = hoaDon.MauHoaDon,
                            KyHieuHoaDon = hoaDon.KyHieuHoaDon,
                            SoHoaDon = hoaDon.SoHoaDon,
                            NgayLapHoaDon = hoaDon.NgayLapHoaDon,
                            LoaiApDungHD = hoaDon.LoaiApDungHD,
                            LyDoRaSoat = hoaDon.LyDoRaSoat
                        };

            return await query.ToListAsync();
        }

        /// <summary>
        /// ThemThongBaoHoaDonRaSoat sẽ thêm bản ghi thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="tDiep"></param>
        /// <returns></returns>
        public async Task<string> ThemThongBaoHoaDonRaSoat(ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep tDiep)
        {
            //Id bản ghi thông báo hóa đơn rà soát
            var thongBaoHoaDonRaSoatId = Guid.NewGuid().ToString();

            //Lưu ra file xml nội dung file đã nhận
            var fileNameGuid = "TD-" + Guid.NewGuid().ToString();
            var xmlFileName = fileNameGuid + ".xml";
            var pdfFileName = fileNameGuid + ".pdf";
            string pdfFullFolder = "";
            try
            {
                //tạo thư mục để lưu các file dữ liệu
                var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}";
                string xmlFullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.XML_SIGNED);
                if (!Directory.Exists(xmlFullFolder))
                {
                    Directory.CreateDirectory(xmlFullFolder);
                }
                pdfFullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.PDF_UNSIGN);
                if (!Directory.Exists(pdfFullFolder))
                {
                    Directory.CreateDirectory(pdfFullFolder);
                }

                //lưu file xml
                XmlSerializerNamespaces xmlSerializingNameSpace = new XmlSerializerNamespaces();
                xmlSerializingNameSpace.Add("", "");

                XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep));

                using (TextWriter fileStream = new StreamWriter(xmlFullFolder + "/" + xmlFileName))
                {
                    serialiser.Serialize(fileStream, tDiep, xmlSerializingNameSpace);
                }

                //thêm dữ liệu xml vào bảng filedatas, không cần lưu ở đây nữa
                //vì đã thêm xml vào filedatas ở API InsertThongDiepNhanAsync nên bỏ qua ở đây
                /*
                XmlSerializer serialiserRaSoat = new XmlSerializer(typeof(TDiep));
                using (var stringWriter = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(stringWriter))
                    {
                        serialiserRaSoat.Serialize(writer, tDiep);
                        await ThemDuLieuVaoBangFileData(thongBaoHoaDonRaSoatId, stringWriter.ToString(), xmlFileName);
                    }
                }
                */
            }
            catch (Exception)
            {
                xmlFileName = "";
            }

            //Lưu ra file PDF
            CreatePdfFileThongBaoRaSoat(pdfFullFolder + "/" + pdfFileName, tDiep);

            //Lưu dữ liệu vào database
            ThongBaoHoaDonRaSoatViewModel model = new ThongBaoHoaDonRaSoatViewModel
            {
                Id = thongBaoHoaDonRaSoatId,
                SoThongBaoCuaCQT = tDiep.DLieu.TBao.STBao.So,
                NgayThongBao = DateTime.Parse(tDiep.DLieu.TBao.STBao.NTBao),
                TenCQTCapTren = tDiep.DLieu.TBao.DLTBao.TCQTCTren,
                TenCQTRaThongBao = tDiep.DLieu.TBao.DLTBao.TCQT,
                TenNguoiNopThue = tDiep.DLieu.TBao.DLTBao.TNNT,
                MaSoThue = tDiep.DLieu.TBao.DLTBao.MST,
                ThoiHan = tDiep.DLieu.TBao.DLTBao.THan,
                Lan = tDiep.DLieu.TBao.DLTBao.Lan,
                HinhThuc = tDiep.DLieu.TBao.DLTBao.TCQTCTren,// tDiep.DLieu.TBao.DLTBao.HThuc,
                ChucDanh = tDiep.DLieu.TBao.DLTBao.TCQT,//tDiep.DLieu.TBao.DLTBao.CDanh,
                FileDinhKem = xmlFileName + ";" + pdfFileName,
                CreatedDate = DateTime.Now,
                CreatedBy = "",
                ModifyDate = DateTime.Now,
                ModifyBy = "",
                MaThongDiep = tDiep.TTChung.MTDiep
            };

            var entity = _mp.Map<ThongBaoHoaDonRaSoat>(model);
            await _db.ThongBaoHoaDonRaSoats.AddAsync(entity);

            var ketQua = await _db.SaveChangesAsync() > 0;

            if (ketQua)
            {
                List<ThongBaoChiTietHoaDonRaSoat> children = new List<ThongBaoChiTietHoaDonRaSoat>();
                //lưu chi tiết thông báo hóa đơn rà soát
                foreach (var item in tDiep.DLieu.TBao.DLTBao.DSHDon)
                {
                    children.Add(
                        new ThongBaoChiTietHoaDonRaSoat
                        {
                            Id = Guid.NewGuid().ToString(),
                            ThongBaoHoaDonRaSoatId = model.Id,
                            MauHoaDon = item.KHMSHDon,
                            KyHieuHoaDon = item.KHHDon,
                            SoHoaDon = item.SHDon,
                            NgayLapHoaDon = DateTime.Parse(item.Ngay),
                            LoaiApDungHD = item.LADHDDT,
                            LyDoRaSoat = item.LDo,
                            DaGuiThongBao = false,
                            CreatedDate = DateTime.Now,
                            CreatedBy = "",
                            ModifyDate = DateTime.Now,
                            ModifyBy = ""
                        });
                }
                await _db.ThongBaoChiTietHoaDonRaSoats.AddRangeAsync(children);
                await _db.SaveChangesAsync();

                return model.Id;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// CreatePdfFileThongBaoRaSoat sẽ lưu file pdf của thông báo hóa đơn rà soát
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <param name="tDiep"></param>
        /// <returns></returns>
        private string CreatePdfFileThongBaoRaSoat(string pdfFilePath, ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep tDiep)
        {
            try
            {
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/QuanLy/thong-bao-hoa-don-dien-tu-can-ra-soat.docx");
                doc.LoadFromFile(docFolder);

                doc.Replace("<TenCQTCapTren>", tDiep.DLieu.TBao.DLTBao.TCQTCTren, true, true);
                doc.Replace("<TenCQT>", tDiep.DLieu.TBao.DLTBao.TCQT, true, true);
                doc.Replace("<TenNguoiNopThue>", tDiep.DLieu.TBao.DLTBao.TNNT ?? "", true, true);
                doc.Replace("<MaSoThue>", tDiep.DLieu.TBao.DLTBao.MST ?? "", true, true);

                doc.Replace("<DiaChiLienHe>", tDiep.DLieu.TBao.DLTBao.DCNNT ?? "", true, true);
                doc.Replace("<DiaChiThuDienTu>", tDiep.DLieu.TBao.DLTBao.DCTDTu ?? "", true, true);

                var ngayThangNam = DateTime.Parse(tDiep.DLieu.TBao.STBao.NTBao);
                doc.Replace("<NgayThangNam>", string.Format("Ngày {0} tháng {1} năm {2}", ngayThangNam.Day, ngayThangNam.Month, ngayThangNam.Year), true, true);

                //thao tác với bảng dữ liệu thứ 2 (bảng chi tiết)
                var bangDuLieu = doc.Sections[0].Tables[2];
                for (int i = 0; i < tDiep.DLieu.TBao.DLTBao.DSHDon.Count - 1; i++) //-1 vì đã có sẵn 1 dòng rồi
                {
                    // Clone row
                    TableRow newRow = bangDuLieu.Rows[2].Clone();
                    bangDuLieu.Rows.Insert(2, newRow);
                }

                //điền dữ liệu vào bảng
                TableRow row = null;
                Paragraph paragraph = null;
                ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.HDon item = null;
                for (int i = 0; i < tDiep.DLieu.TBao.DLTBao.DSHDon.Count; i++)
                {
                    item = tDiep.DLieu.TBao.DLTBao.DSHDon[i];

                    row = bangDuLieu.Rows[i + 2]; // +2 vì ko tính 2 dòng đầu
                    paragraph = row.Cells[0].Paragraphs[0];
                    paragraph.Text = (i + 1).ToString();

                    var mauHoaDon = "";
                    if (item.LADHDDT == 1)
                    {
                        mauHoaDon = (item.KHMSHDon ?? "") + (item.KHHDon ?? "");
                    }
                    else
                    {
                        mauHoaDon = (item.KHMSHDon ?? "") + "-" + (item.KHHDon ?? "");
                        mauHoaDon = mauHoaDon.Trim('-');
                    }

                    paragraph = row.Cells[1].Paragraphs[0];
                    paragraph.Text = mauHoaDon;

                    paragraph = row.Cells[2].Paragraphs[0];
                    paragraph.Text = item.SHDon ?? "";

                    paragraph = row.Cells[3].Paragraphs[0];
                    paragraph.Text = DateTime.Parse(item.Ngay).ToString("dd/MM/yyyy");

                    paragraph = row.Cells[4].Paragraphs[0];
                    paragraph.Text = item.LADHDDT.ToString();

                    paragraph = row.Cells[5].Paragraphs[0];
                    paragraph.Text = item.LDo;
                }

                //lưu file pdf
                doc.SaveToFile(pdfFilePath, FileFormat.PDF);

                return pdfFilePath;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion

        /// <summary>
        /// XuLyDuLieuNhanVeTuCQT xử lý dữ liệu nhận về từ cơ quan thuế
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public async Task<bool> XuLyDuLieuNhanVeTuCQT(ThongDiepPhanHoiParams @params)
        {
            var ketQua = true;

            switch (@params.MLTDiep)
            {
                case (int)ViewModels.XML.MLTDiep.TDTBHDDTCRSoat: // 302
                    var tDiep302 = DataHelper.ConvertBase64ToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep>(@params.DataXML);

                    ketQua = await ThemThongBaoHoaDonRaSoat(tDiep302) != null;
                    break;
                default:
                    break;
            }

            return ketQua;
        }

        /// <summary>
        /// GetListChungThuSoAsync trả về danh sách các chứng thư số liên quan đến hóa đơn
        /// </summary>
        /// <param name="ThongDiepGuiCQTId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetListChungThuSoAsync(string thongDiepGuiCQTId)
        {
            List<string> listSerial = new List<string>();

            var querySerialBoKyHieu = from thongDiep in _db.ThongDiepGuiCQTs
                                      join thongDiepChiTiet in _db.ThongDiepChiTietGuiCQTs on thongDiep.Id equals thongDiepChiTiet.ThongDiepGuiCQTId
                                      join hoaDon in _db.HoaDonDienTus on thongDiepChiTiet.HoaDonDienTuId equals hoaDon.HoaDonDienTuId
                                      join bkhhd in _db.NhatKyXacThucBoKyHieus on hoaDon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                      where thongDiep.Id == thongDiepGuiCQTId && !string.IsNullOrWhiteSpace(bkhhd.SoSeriChungThu)
                                      select bkhhd.SoSeriChungThu;
            listSerial = await querySerialBoKyHieu.Distinct().ToListAsync();

            IQueryable<ToKhaiDangKyThongTinViewModel> queryToKhai = from tdc in _db.ThongDiepChungs
                                                                    join tk in _db.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id
                                                                    join hs in _db.HoSoHDDTs on tdc.MaSoThue equals hs.MaSoThue
                                                                    where tdc.MaLoaiThongDiep == 100 && tdc.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
                                                                    orderby tdc.NgayThongBao descending
                                                                    select new ToKhaiDangKyThongTinViewModel
                                                                    {
                                                                        Id = tk.Id,
                                                                        NgayTao = tk.NgayTao,
                                                                        ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                                                        ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content)
                                                                    };
            var toKhai = await queryToKhai.FirstOrDefaultAsync();
            if (toKhai != null)
            {
                var listSerialTuToKhai = toKhai.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung
                    .Select(x => x.Seri).ToList();
                if (listSerialTuToKhai.Count > 0)
                {
                    listSerial = (listSerial.Union(listSerialTuToKhai)).Distinct().ToList();
                }
            }

            return listSerial;
        }

        /// <summary>
        /// GetBangKeHoaDonSaiSotAsync trả về bảng kê hóa đơn sai sót
        /// </summary>
        /// <param name="thongKeHoaDonSaiSotParams"></param>
        /// <returns></returns>
        public async Task<List<BangKeHoaDonSaiSot_ViewModel>> GetBangKeHoaDonSaiSotAsync(ThongKeHoaDonSaiSotParams thongKeHoaDonSaiSotParams)
        {
            var queryBangKe = await (from hoaDon in _db.ThongDiepChiTietGuiCQTs

                                     join hoaDonHeThong in _db.HoaDonDienTus on hoaDon.HoaDonDienTuId equals hoaDonHeThong.HoaDonDienTuId into tmpHoaDonHeThong
                                     from hoaDonHeThong in tmpHoaDonHeThong.DefaultIfEmpty()

                                     join loaiTienHoaDonHeThong in _db.LoaiTiens on hoaDonHeThong.LoaiTienId equals loaiTienHoaDonHeThong.LoaiTienId into tmpLoaiTienHoaDonHeThong
                                     from loaiTienHoaDonHeThong in tmpLoaiTienHoaDonHeThong.DefaultIfEmpty()

                                     join hoaDonKhac in _db.ThongTinHoaDons on hoaDon.HoaDonDienTuId equals hoaDonKhac.Id into tmpHoaDonKhac
                                     from hoaDonKhac in tmpHoaDonKhac.DefaultIfEmpty()

                                     join loaiTienHoaDonKhac in _db.LoaiTiens on hoaDonKhac.LoaiTienId equals loaiTienHoaDonKhac.LoaiTienId into tmpLoaiTienHoaDonKhac
                                     from loaiTienHoaDonKhac in tmpLoaiTienHoaDonKhac.DefaultIfEmpty()

                                     join thongBao302_HoaDonKhac in (from thongBaoRaSoat in _db.ThongBaoHoaDonRaSoats
                                                                     join thongBaoRaSoatChiTiet in _db.ThongBaoChiTietHoaDonRaSoats on thongBaoRaSoat.Id equals thongBaoRaSoatChiTiet.ThongBaoHoaDonRaSoatId
                                                                     select new ChiTietHoaDonRaSoat301_ViewModel
                                                                     { MauHoaDon = thongBaoRaSoatChiTiet.MauHoaDon, KyHieuHoaDon = thongBaoRaSoatChiTiet.KyHieuHoaDon, SoHoaDon = thongBaoRaSoatChiTiet.SoHoaDon, SoThongBaoCuaCQT = thongBaoRaSoat.SoThongBaoCuaCQT, NgayThongBao = thongBaoRaSoat.NgayThongBao }) on new
                                                                     { MauHoaDon = hoaDonKhac.MauSoHoaDon, hoaDonKhac.KyHieuHoaDon, hoaDonKhac.SoHoaDon } equals new { thongBao302_HoaDonKhac.MauHoaDon, thongBao302_HoaDonKhac.KyHieuHoaDon, thongBao302_HoaDonKhac.SoHoaDon } into tmpThongBao302_HoaDonKhac
                                     from thongBao302_HoaDonKhac in tmpThongBao302_HoaDonKhac.DefaultIfEmpty()

                                     join thongDiepGuiCQT in _db.ThongDiepGuiCQTs on hoaDon.ThongDiepGuiCQTId equals thongDiepGuiCQT.Id
                                     join thongDiepChung in _db.ThongDiepChungs on thongDiepGuiCQT.Id equals thongDiepChung.IdThamChieu into tmpThongDiepChung
                                     from thongDiepChung in tmpThongDiepChung.DefaultIfEmpty()

                                     join thongBao302 in _db.ThongBaoHoaDonRaSoats on thongDiepGuiCQT.ThongBaoHoaDonRaSoatId equals thongBao302.Id into tmpThongBao302
                                     from thongBao302 in tmpThongBao302.DefaultIfEmpty()

                                     where hoaDon.NgayLapHoaDon.Value.Date >= DateTime.Parse(thongKeHoaDonSaiSotParams.TuNgay)
                                     && hoaDon.NgayLapHoaDon.Value.Date <= DateTime.Parse(thongKeHoaDonSaiSotParams.DenNgay)

                                     && !string.IsNullOrWhiteSpace(thongDiepChung.ThongDiepChungId) //loại bỏ các bản ghi ko tồn tại trong thông điệp chung

                                     select new BangKeHoaDonSaiSot_ViewModel
                                     {
                                         HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                         MauHoaDon = hoaDon.MauHoaDon,
                                         KyHieuHoaDon = hoaDon.KyHieuHoaDon,
                                         SoHoaDon = hoaDon.SoHoaDon,
                                         NgayLapHoaDon = hoaDon.NgayLapHoaDon,
                                         MaCQTCap = hoaDon.MaCQTCap,
                                         TongTienThanhToan = hoaDonHeThong.TongTienThanhToan,
                                         MaLoaiTien = (hoaDonHeThong != null) ? (loaiTienHoaDonHeThong != null ? loaiTienHoaDonHeThong.Ma : "") : ((hoaDonKhac != null) ? (loaiTienHoaDonKhac != null ? loaiTienHoaDonKhac.Ma : "") : ""),

                                         LoaiApDungHoaDon = hoaDon.LoaiApDungHoaDon,
                                         LoaiSaiSot = GetLoaiSaiSot(hoaDon.PhanLoaiHDSaiSot),
                                         LyDo = hoaDon.LyDo,
                                         LoaiHoaDon = (hoaDonHeThong != null) ? ((LoaiHoaDon)hoaDonHeThong.LoaiHoaDon).GetDescription() : GetLoaiHoaDon(hoaDonKhac.MauSoHoaDon),
                                         TrangThaiHoaDon = new TrangThaiHoaDon_BangKeSaiSot_ViewModel
                                         {
                                             TrangThai = hoaDon.TrangThaiHoaDon,
                                             DienGiaiTrangThai = hoaDon.DienGiaiTrangThai,
                                             LoaiDieuChinh = hoaDonHeThong.LoaiDieuChinh
                                         },
                                         ChungTuLienQuan = hoaDon.ChungTuLienQuan,

                                         NgayThongBao = thongDiepGuiCQT.NgayGui,

                                         LoaiThongBaoSaiSot = GetLoaiThongBaoSaiSot(thongDiepGuiCQT.ThongBaoHoaDonRaSoatId, thongDiepGuiCQT.IsTBaoHuyGiaiTrinhKhacCuaNNT).LoaiThongBaoSaiSot,
                                         TenLoaiThongBaoSaiSot = GetLoaiThongBaoSaiSot(thongDiepGuiCQT.ThongBaoHoaDonRaSoatId, thongDiepGuiCQT.IsTBaoHuyGiaiTrinhKhacCuaNNT).TenLoaiThongBaoSaiSot,
                                         MauSoTBaoCuaCQT = (thongBao302 != null) ? "01/TB-RSĐT" : "",
                                         SoTBaoCuaCQT = (thongBao302 != null) ? thongBao302.SoThongBaoCuaCQT : thongBao302_HoaDonKhac.SoThongBaoCuaCQT,
                                         NgayTBaoCuaCQT = (thongBao302 != null) ? thongBao302.NgayThongBao : thongBao302_HoaDonKhac.NgayThongBao,

                                         MaThongDiepGui = thongDiepChung.MaThongDiep,
                                         MaThongDiepPhanHoi = thongDiepChung.MaThongDiepPhanHoi,
                                         MauSoTBaoPhanHoiTuCQT = thongDiepChung.MaLoaiThongDiep == 301 ? "01/TB-SSĐT" : "",
                                         SoTBaoPhanHoiTuCQT = thongDiepChung.SoTBaoPhanHoiCuaCQT,
                                         NgayTBaoPhanHoiTuCQT = thongDiepChung.NgayTBaoPhanHoiCuaCQT,

                                         TrangThaiGui = thongDiepChung.TrangThaiGui.HasValue ? thongDiepChung.TrangThaiGui : null,

                                         TenTrangThaiGui = thongDiepChung.TrangThaiGui.HasValue ? thongDiepChung.TrangThaiGui == (int)TrangThaiGuiThongDiep.GoiDuLieuHopLe ? "Hóa đơn hợp lệ" : ((TrangThaiGuiThongDiep)thongDiepChung.TrangThaiGui).GetDescription() : null,

                                         ThongDiepChungId = thongDiepChung.ThongDiepChungId,
                                         IdTDiepTBaoPhanHoiCuaCQT = thongDiepChung.IdTDiepTBaoPhanHoiCuaCQT,

                                         LaHoaDonNgoaiHeThong = thongDiepGuiCQT.IsTBaoHuyGiaiTrinhKhacCuaNNT,
                                         TrangThaiQuyTrinh = hoaDonHeThong.TrangThaiQuyTrinh
                                     }).ToListAsync();

            //lọc theo Loại thông báo sai sót
            if (thongKeHoaDonSaiSotParams.LoaiThongke == 2 && thongKeHoaDonSaiSotParams.LoaiThongBaoSaiSot != 0)
            {
                queryBangKe = queryBangKe.Where(x => x.LoaiThongBaoSaiSot == thongKeHoaDonSaiSotParams.LoaiThongBaoSaiSot).ToList();
            }

            //lọc theo tìm kiếm theo
            if (thongKeHoaDonSaiSotParams.LoaiThongke == 1)
            {
                var timKiemTheo = thongKeHoaDonSaiSotParams.TimKiemTheo;
                if (timKiemTheo != null)
                {
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.KyHieuHoaDon))
                    {
                        var keyword = timKiemTheo.KyHieuHoaDon.TrimToUpper();
                        queryBangKe = queryBangKe.Where(x => string.Format("{0}{1}", x.MauHoaDon, x.KyHieuHoaDon).TrimToUpper().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.SoHoaDon))
                    {
                        var keyword = timKiemTheo.SoHoaDon.TrimToUpper();
                        queryBangKe = queryBangKe.Where(x => x.SoHoaDon != null && x.SoHoaDon.TrimToUpper().Contains(keyword)).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(timKiemTheo.NgayHoaDon))
                    {
                        var keyword = timKiemTheo.NgayHoaDon.TrimToUpper();
                        queryBangKe = queryBangKe.Where(x => x.NgayLapHoaDon != null && x.NgayLapHoaDon.Value.ToString("dd/MM/yyyy").TrimToUpper().Contains(keyword)).ToList();
                    }
                }
                else
                {
                    //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                    if (string.IsNullOrWhiteSpace(thongKeHoaDonSaiSotParams.TimKiemBatKy) == false)
                    {
                        thongKeHoaDonSaiSotParams.TimKiemBatKy = thongKeHoaDonSaiSotParams.TimKiemBatKy.TrimToUpper();
                        queryBangKe = queryBangKe.Where(x =>
                            (string.Format("{0}{1}", x.MauHoaDon, x.KyHieuHoaDon).TrimToUpper().Contains(thongKeHoaDonSaiSotParams.TimKiemBatKy)) ||
                            (x.SoHoaDon != null && x.SoHoaDon.TrimToUpper().Contains(thongKeHoaDonSaiSotParams.TimKiemBatKy)) ||
                            (x.NgayLapHoaDon != null && x.NgayLapHoaDon.Value.ToString("dd/MM/yyyy").TrimToUpper().Contains(thongKeHoaDonSaiSotParams.TimKiemBatKy))
                        ).ToList();
                    }
                }
            }

            if (thongKeHoaDonSaiSotParams.LoaiThongke == 1) //thống kê theo hóa đơn
            {
                queryBangKe = queryBangKe.OrderBy(x => x.NgayLapHoaDon).ThenBy(x => x.KyHieuHoaDon).ThenBy(x => x.SoHoaDon).ThenBy(x => x.NgayThongBao).ToList();
            }
            else //thống kê theo thông điệp
            {
                queryBangKe = queryBangKe.OrderByDescending(x => x.NgayThongBao).ThenBy(x => x.LoaiThongBaoSaiSot).ThenBy(x => x.SoTBaoCuaCQT).ThenByDescending(x => x.NgayTBaoCuaCQT).ThenBy(x => x.MaThongDiepGui).ThenBy(x => x.SoTBaoPhanHoiTuCQT).ThenBy(x => x.NgayTBaoPhanHoiTuCQT).ToList();
            }
            #region Filter and Sort
            if (thongKeHoaDonSaiSotParams.FilterColumns != null && thongKeHoaDonSaiSotParams.FilterColumns.Any())
            {
                thongKeHoaDonSaiSotParams.FilterColumns = thongKeHoaDonSaiSotParams.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in thongKeHoaDonSaiSotParams.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        case nameof(thongKeHoaDonSaiSotParams.Filter.SoHoaDon):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.SoHoaDon, filterCol, FilterValueType.String).ToList();
                            break;
                        case nameof(thongKeHoaDonSaiSotParams.Filter.MauHoaDon):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.MauHoaDon, filterCol, FilterValueType.String).ToList();
                            break;
                        case nameof(thongKeHoaDonSaiSotParams.Filter.KyHieuHoaDon):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.KyHieuHoaDon, filterCol, FilterValueType.String).ToList();
                            break;
                        case nameof(thongKeHoaDonSaiSotParams.Filter.MaLoaiTien):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.MaLoaiTien, filterCol, FilterValueType.String).ToList();
                            break;
                        case nameof(thongKeHoaDonSaiSotParams.Filter.MaCQTCap):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.MaCQTCap, filterCol, FilterValueType.String).ToList();
                            break;
                        case nameof(thongKeHoaDonSaiSotParams.Filter.MaThongDiepGui):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.MaThongDiepGui, filterCol, FilterValueType.String).ToList();
                            break;
                        case nameof(thongKeHoaDonSaiSotParams.Filter.TongTienThanhToan):
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.TongTienThanhToan, filterCol, FilterValueType.Decimal).ToList();
                            break;
                        case "NgayHoaDon":
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.NgayLapHoaDon, filterCol, FilterValueType.DateTime).ToList();
                            break;
                        case "NgayThongBao":
                            queryBangKe = GenericFilterColumn<BangKeHoaDonSaiSot_ViewModel>.Query(queryBangKe, x => x.NgayThongBao, filterCol, FilterValueType.DateTime).ToList();
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(thongKeHoaDonSaiSotParams.SortKey))
            {
                if (thongKeHoaDonSaiSotParams.SortKey == "KyHieuHoaDon" && thongKeHoaDonSaiSotParams.SortValue == "ascend")
                {
                    queryBangKe = queryBangKe.OrderBy(x => x.KyHieuHoaDon).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "KyHieuHoaDon" && thongKeHoaDonSaiSotParams.SortValue == "descend")
                {
                    queryBangKe = queryBangKe.OrderByDescending(x => x.KyHieuHoaDon).ToList();
                }

                if (thongKeHoaDonSaiSotParams.SortKey == "SoNgay" && thongKeHoaDonSaiSotParams.SortValue == "ascend")
                {
                    queryBangKe = queryBangKe.OrderBy(x => x.NgayLapHoaDon).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "SoNgay" && thongKeHoaDonSaiSotParams.SortValue == "descend")
                {
                    queryBangKe = queryBangKe.OrderByDescending(x => x.NgayLapHoaDon).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "NgayThongBao" && thongKeHoaDonSaiSotParams.SortValue == "ascend")
                {
                    queryBangKe = queryBangKe.OrderBy(x => x.NgayThongBao).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "NgayThongBao" && thongKeHoaDonSaiSotParams.SortValue == "descend")
                {
                    queryBangKe = queryBangKe.OrderByDescending(x => x.NgayThongBao).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "MaCQTCap" && thongKeHoaDonSaiSotParams.SortValue == "ascend")
                {
                    queryBangKe = queryBangKe.OrderBy(x => x.MaCQTCap).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "MaCQTCap" && thongKeHoaDonSaiSotParams.SortValue == "descend")
                {
                    queryBangKe = queryBangKe.OrderByDescending(x => x.MaCQTCap).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "MaThongDiepGui" && thongKeHoaDonSaiSotParams.SortValue == "ascend")
                {
                    queryBangKe = queryBangKe.OrderBy(x => x.MaThongDiepGui).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "MaThongDiepGui" && thongKeHoaDonSaiSotParams.SortValue == "descend")
                {
                    queryBangKe = queryBangKe.OrderByDescending(x => x.MaThongDiepGui).ToList();
                }

                if (thongKeHoaDonSaiSotParams.SortKey == "TongTienThanhToan" && thongKeHoaDonSaiSotParams.SortValue == "ascend")
                {
                    queryBangKe = queryBangKe.OrderBy(x => x.TongTienThanhToan).ToList();
                }
                if (thongKeHoaDonSaiSotParams.SortKey == "TongTienThanhToan" && thongKeHoaDonSaiSotParams.SortValue == "descend")
                {
                    queryBangKe = queryBangKe.OrderByDescending(x => x.TongTienThanhToan).ToList();
                }

            }
            #endregion
            //bảng màu để tô các dòng khác nhau
            string[] colorHexes = { "FFC000", "FFFF00", "92D050", "00B0F0", "00FF7F", "40E0D0", "00FA9A", "EE82EE", "66CDAA", "8FBC8F", "7FFFD4", "7FFF00", "ADFF2F", "87CEFA", "FFD700", "F0E68C", "FFE4B5", "FFA500", "FFB6C1", "FF6347" };

            //sau khi sắp xếp các cột xong thì mới cài đặt KhongHienThiThongTinGiongNhau để ko bị sắp xếp nhầm
            if (thongKeHoaDonSaiSotParams.KhongHienThiThongTinGiongNhau)
            {
                string hoaDonHienTai = "";
                string hoaDonKeTiep = "";
                var colorIndex = 0;

                if (thongKeHoaDonSaiSotParams.LoaiThongke == 1) //thống kê theo hóa đơn
                {
                    for (var i = 0; i < queryBangKe.Count; i++)
                    {
                        if (hoaDonHienTai != queryBangKe[i].HoaDonDienTuId && !string.IsNullOrWhiteSpace(queryBangKe[i].HoaDonDienTuId))
                        {
                            hoaDonHienTai = queryBangKe[i].HoaDonDienTuId;
                            colorIndex = 0; //reset khi tới hóa đơn khác
                        }

                        if ((i + 1) < queryBangKe.Count)
                        {
                            hoaDonKeTiep = queryBangKe[i + 1].HoaDonDienTuId;
                            if (hoaDonHienTai == hoaDonKeTiep)
                            {
                                queryBangKe[i + 1].HoaDonDienTuId = null;
                                queryBangKe[i + 1].KyHieuHoaDon = null;
                                queryBangKe[i + 1].MauHoaDon = null;
                                queryBangKe[i + 1].SoHoaDon = null;
                                queryBangKe[i + 1].NgayLapHoaDon = null;

                                queryBangKe[i + 1].MaCQTCap = null;
                                queryBangKe[i + 1].TongTienThanhToan = null;
                                queryBangKe[i + 1].MaLoaiTien = null;
                                queryBangKe[i + 1].LoaiApDungHoaDon = 0;
                                //queryBangKe[i + 1].LoaiSaiSot = null;

                                //queryBangKe[i + 1].LyDo = null;
                                queryBangKe[i + 1].LoaiHoaDon = null;
                                queryBangKe[i + 1].TrangThaiHoaDon = null;
                                //queryBangKe[i + 1].ChungTuLienQuan = null;
                            }
                        }

                        //cài đặt màu cho dòng
                        //queryBangKe[i].ColorHex = string.Format("{0}{1}", "#", colorHexes[colorIndex]);

                        colorIndex++;
                        if (colorIndex >= colorHexes.Length)
                        {
                            colorIndex = 0;
                        }
                    }
                }
                else //thống kê theo thông báo
                {
                    string thongDiepHienTai = "";
                    string thongDiepKeTiep = "";

                    for (var i = 0; i < queryBangKe.Count; i++)
                    {
                        if (hoaDonHienTai != queryBangKe[i].HoaDonDienTuId && !string.IsNullOrWhiteSpace(queryBangKe[i].HoaDonDienTuId))
                        {
                            hoaDonHienTai = queryBangKe[i].HoaDonDienTuId;
                            colorIndex = 0; //reset khi tới hóa đơn khác
                        }

                        if (thongDiepHienTai != queryBangKe[i].ThongDiepChungId && !string.IsNullOrWhiteSpace(queryBangKe[i].ThongDiepChungId))
                        {
                            thongDiepHienTai = queryBangKe[i].ThongDiepChungId;
                        }

                        if ((i + 1) < queryBangKe.Count)
                        {
                            thongDiepKeTiep = queryBangKe[i + 1].ThongDiepChungId;
                            if (thongDiepHienTai == thongDiepKeTiep)
                            {
                                queryBangKe[i + 1].ThongDiepChungId = null;
                                queryBangKe[i + 1].NgayThongBao = null;
                                queryBangKe[i + 1].LoaiThongBaoSaiSot = null;
                                queryBangKe[i + 1].TenLoaiThongBaoSaiSot = null;
                                queryBangKe[i + 1].SoTBaoCuaCQT = null;
                                queryBangKe[i + 1].NgayTBaoCuaCQT = null;

                                queryBangKe[i + 1].MaThongDiepGui = null;
                                queryBangKe[i + 1].SoTBaoPhanHoiTuCQT = null;
                                queryBangKe[i + 1].NgayTBaoPhanHoiTuCQT = null;

                                queryBangKe[i + 1].TrangThaiGui = null;
                                queryBangKe[i + 1].TenTrangThaiGui = null;
                            }
                        }

                        //cài đặt màu cho dòng
                        //queryBangKe[i].ColorHex = string.Format("{0}{1}", "#", colorHexes[colorIndex]);

                        colorIndex++;
                        if (colorIndex >= colorHexes.Length)
                        {
                            colorIndex = 0;
                        }
                    }
                }
            }
            else
            {
                //cài đặt màu cho dòng
                string hoaDonHienTai = "";
                var colorIndex = 0;
                for (var i = 0; i < queryBangKe.Count; i++)
                {
                    if (hoaDonHienTai != queryBangKe[i].HoaDonDienTuId && !string.IsNullOrWhiteSpace(queryBangKe[i].HoaDonDienTuId))
                    {
                        hoaDonHienTai = queryBangKe[i].HoaDonDienTuId;
                        colorIndex = 0; //reset khi tới hóa đơn khác
                    }

                    //queryBangKe[i].ColorHex = string.Format("{0}{1}", "#", colorHexes[colorIndex]);

                    colorIndex++;
                    if (colorIndex >= colorHexes.Length)
                    {
                        colorIndex = 0;
                    }
                }
            }

            foreach (var item in queryBangKe)
            {
                var maThongDiepPhanHoi = item.MaThongDiepPhanHoi;
                if (!string.IsNullOrEmpty(maThongDiepPhanHoi))
                {
                    var thongDiepPhanHoi = _db.ThongDiepChungs.Where(x => x.MaThongDiep == maThongDiepPhanHoi && x.MauSoTBaoPhanHoiCuaCQT != null).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    if (thongDiepPhanHoi != null)
                    {
                        if (thongDiepPhanHoi.MaLoaiThongDiep == 302)
                        {
                            item.MauSoTBaoCuaCQT = thongDiepPhanHoi.MauSoTBaoPhanHoiCuaCQT;
                            item.SoTBaoCuaCQT = thongDiepPhanHoi.SoTBaoPhanHoiCuaCQT;
                            item.NgayTBaoCuaCQT = thongDiepPhanHoi.NgayTBaoPhanHoiCuaCQT;
                        }
                        else
                        {
                            item.MauSoTBaoPhanHoiTuCQT = thongDiepPhanHoi.MauSoTBaoPhanHoiCuaCQT;
                            item.SoTBaoPhanHoiTuCQT = thongDiepPhanHoi.SoTBaoPhanHoiCuaCQT;
                            item.NgayTBaoPhanHoiTuCQT = thongDiepPhanHoi.NgayTBaoPhanHoiCuaCQT;
                            item.IdTDiepTBaoPhanHoiCuaCQT = thongDiepPhanHoi.ThongDiepChungId;
                        }
                    }
                }
            }

            return queryBangKe;
        }

        /// <summary>
        /// ExportExcelBangKeSaiSotAsync xuất ra file excel bảng kê sai sót
        /// </summary>
        /// <param name="exportParams"></param>
        /// <returns></returns>
        public string ExportExcelBangKeSaiSotAsync(ExportExcelBangKeSaiSotParams exportParams)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string fileContainerPath = $"FilesUpload/{databaseName}/excels";
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, fileContainerPath);
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            else
            {
                FileHelper.ClearFolder(uploadFolder);
            }

            // Excel
            string excelFileName;
            string _sample;
            string moTaBaoCao;
            string ngayBaoCao = string.Format("Từ ngày {0} đến ngày {1}", DateTime.Parse(exportParams.Params.TuNgay).ToString("dd/MM/yyyy"), DateTime.Parse(exportParams.Params.DenNgay).ToString("dd/MM/yyyy"));

            if (exportParams.Params.LoaiThongke == 1) //theo hóa đơn
            {
                excelFileName = $"BangKeSaiSot_TheoHoaDon-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                _sample = $"docs/ThongDiep/BangKeSaiSot_TheoHoaDon.xlsx";
                moTaBaoCao = string.Format("{0}{1}", "Thống kê theo: Hóa đơn; ", ngayBaoCao);
            }
            else //theo thông báo
            {
                excelFileName = $"BangKeSaiSot_TheoThongBao-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                _sample = $"docs/ThongDiep/BangKeSaiSot_TheoThongBao.xlsx";
                moTaBaoCao = string.Format("{0}{1}", "Thống kê theo: Thông báo hóa đơn điện tử có sai sót; ", ngayBaoCao);
            }

            string excelPath = $"{uploadFolder}/{excelFileName}";
            string excelFullPath = Path.Combine(_hostingEnvironment.WebRootPath, excelPath);
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);
            FileInfo file = new FileInfo(_path_sample);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = exportParams.ListBangKeSaiSot.Count();
                int begin_row = 4;

                //dòng mô tả báo cáo
                worksheet.Cells[2, 1].Value = moTaBaoCao;

                // Add Row
                if (totalRows != 0)
                {
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                }
                // Fill data
                int idx = begin_row + (totalRows == 0 ? 1 : 0);
                if (exportParams.Params.LoaiThongke == 1) //theo hóa đơn
                {
                    foreach (var item in exportParams.ListBangKeSaiSot)
                    {
                        worksheet.Cells[idx, 1].Value = item.MauHoaDon;
                        worksheet.Cells[idx, 2].Value = item.KyHieuHoaDon;
                        worksheet.Cells[idx, 3].Value = item.SoHoaDon;
                        worksheet.Cells[idx, 4].Value = item.NgayLapHoaDon?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 5].Value = item.MaCQTCap;
                        worksheet.Cells[idx, 6].Value = item.TongTienThanhToan;
                        worksheet.Cells[idx, 7].Value = item.MaLoaiTien;
                        worksheet.Cells[idx, 8].Value = (item.LoaiApDungHoaDon == 0) ? "" : item.LoaiApDungHoaDon.ToString();
                        worksheet.Cells[idx, 9].Value = item.LoaiHoaDon;

                        //trạng thái hóa đơn
                        string tenTrangThai = "";
                        if (!item.LaHoaDonNgoaiHeThong.GetValueOrDefault())
                        {
                            if (item.TrangThaiHoaDon != null)
                            {
                                var trangThai = item.TrangThaiHoaDon.TrangThai;
                                switch (trangThai)
                                {
                                    case 1:
                                        tenTrangThai = "Hóa đơn gốc";
                                        break;
                                    case 2:
                                        tenTrangThai = "Hóa đơn hủy";
                                        break;
                                    case 3:
                                        tenTrangThai = "Hóa đơn thay thế";
                                        break;
                                    case 4:
                                        tenTrangThai = "Hóa đơn điều chỉnh";
                                        break;
                                    default:
                                        tenTrangThai = "";
                                        break;
                                }
                                //tenTrangThai += item.TrangThaiHoaDon.DienGiaiTrangThai?.Replace("&nbsp;", " ");
                            }
                        }
                        else
                        {
                            if (item.TrangThaiHoaDon != null)
                            {
                                var trangThai = item.TrangThaiHoaDon.TrangThai;
                                switch (trangThai)
                                {
                                    case 1:
                                        tenTrangThai = "Hóa đơn gốc";
                                        break;
                                    case 3:
                                        tenTrangThai = "Hóa đơn thay thế";
                                        break;
                                    case 4:
                                        tenTrangThai = "Hóa đơn điều chỉnh";
                                        break;
                                    default:
                                        tenTrangThai = "";
                                        break;
                                }
                                //tenTrangThai += item.TrangThaiHoaDon.DienGiaiTrangThai?.Replace("&nbsp;", " ");
                            }
                        }
                        var chungTuLienQuan = (!string.IsNullOrWhiteSpace(item.ChungTuLienQuan) && item.ChungTuLienQuan.IndexOf(";") > 0) ? item.ChungTuLienQuan.Split(';') : null;
                        worksheet.Cells[idx, 10].Value = tenTrangThai;
                        worksheet.Cells[idx, 11].Value = chungTuLienQuan == null ? "" : chungTuLienQuan[0];
                        worksheet.Cells[idx, 12].Value = item.LoaiSaiSot;
                        worksheet.Cells[idx, 13].Value = item.LyDo;
                        worksheet.Cells[idx, 14].Value = item.NgayThongBao?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 15].Value = item.LoaiThongBaoSaiSot;
                        worksheet.Cells[idx, 16].Value = item.MauSoTBaoCuaCQT;
                        worksheet.Cells[idx, 17].Value = item.SoTBaoCuaCQT;
                        worksheet.Cells[idx, 18].Value = item.NgayTBaoCuaCQT?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 19].Value = item.MaThongDiepGui;
                        worksheet.Cells[idx, 20].Value = item.MauSoTBaoPhanHoiTuCQT;
                        worksheet.Cells[idx, 21].Value = item.SoTBaoPhanHoiTuCQT;
                        worksheet.Cells[idx, 22].Value = item.NgayTBaoPhanHoiTuCQT?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 23].Value = item.TenTrangThaiGui;

                        idx += 1;
                    }
                }
                else //theo thông báo
                {
                    foreach (var item in exportParams.ListBangKeSaiSot)
                    {
                        worksheet.Cells[idx, 1].Value = item.NgayThongBao?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 2].Value = item.LoaiThongBaoSaiSot;
                        worksheet.Cells[idx, 3].Value = item.MauSoTBaoCuaCQT;
                        worksheet.Cells[idx, 4].Value = item.SoTBaoCuaCQT;
                        worksheet.Cells[idx, 5].Value = item.NgayTBaoCuaCQT?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 6].Value = item.MaThongDiepGui;
                        worksheet.Cells[idx, 7].Value = item.MauSoTBaoPhanHoiTuCQT;
                        worksheet.Cells[idx, 8].Value = item.SoTBaoPhanHoiTuCQT;
                        worksheet.Cells[idx, 9].Value = item.NgayTBaoPhanHoiTuCQT?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 10].Value = item.TenTrangThaiGui;

                        worksheet.Cells[idx, 11].Value = item.MauHoaDon;
                        worksheet.Cells[idx, 12].Value = item.KyHieuHoaDon;
                        worksheet.Cells[idx, 13].Value = item.SoHoaDon;
                        worksheet.Cells[idx, 14].Value = item.NgayLapHoaDon?.ToString("dd/MM/yyyy");
                        worksheet.Cells[idx, 15].Value = item.MaCQTCap;
                        worksheet.Cells[idx, 16].Value = item.TongTienThanhToan;
                        worksheet.Cells[idx, 17].Value = item.MaLoaiTien;
                        worksheet.Cells[idx, 18].Value = (item.LoaiApDungHoaDon == 0) ? "" : item.LoaiApDungHoaDon.ToString();
                        worksheet.Cells[idx, 19].Value = item.LoaiHoaDon;

                        //trạng thái hóa đơn
                        string tenTrangThai = "";
                        if (!item.LaHoaDonNgoaiHeThong.GetValueOrDefault())
                        {
                            if (item.TrangThaiHoaDon != null)
                            {
                                var trangThai = item.TrangThaiHoaDon.TrangThai;
                                switch (trangThai)
                                {
                                    case 1:
                                        tenTrangThai = "Hóa đơn gốc";
                                        break;
                                    case 2:
                                        tenTrangThai = "Hóa đơn hủy";
                                        break;
                                    case 3:
                                        tenTrangThai = "Hóa đơn thay thế";
                                        break;
                                    case 4:
                                        tenTrangThai = "Hóa đơn điều chỉnh";
                                        break;
                                    default:
                                        tenTrangThai = "";
                                        break;
                                }
                                //tenTrangThai += item.TrangThaiHoaDon.DienGiaiTrangThai?.Replace("&nbsp;", " ");
                            }
                        }
                        else
                        {
                            if (item.TrangThaiHoaDon != null)
                            {
                                var trangThai = item.TrangThaiHoaDon.TrangThai;
                                switch (trangThai)
                                {
                                    case 1:
                                        tenTrangThai = "Hóa đơn gốc";
                                        break;
                                    case 3:
                                        tenTrangThai = "Hóa đơn thay thế";
                                        break;
                                    case 4:
                                        tenTrangThai = "Hóa đơn điều chỉnh";
                                        break;
                                    default:
                                        tenTrangThai = "";
                                        break;
                                }
                                //tenTrangThai += item.TrangThaiHoaDon.DienGiaiTrangThai?.Replace("&nbsp;", " ");
                            }
                        }
                        worksheet.Cells[idx, 20].Value = tenTrangThai;
                        var chungTuLienQuan = (!string.IsNullOrWhiteSpace(item.ChungTuLienQuan) && item.ChungTuLienQuan.IndexOf(";") > 0) ? item.ChungTuLienQuan.Split(';') : null;
                        worksheet.Cells[idx, 21].Value = chungTuLienQuan == null ? "" : chungTuLienQuan[0];
                        worksheet.Cells[idx, 22].Value = item.LoaiSaiSot;
                        worksheet.Cells[idx, 23].Value = item.LyDo;
                        idx += 1;
                    }
                }

                package.SaveAs(new FileInfo(excelFullPath));
            }

            return string.Format("{0}/{1}", fileContainerPath, excelFileName);
        }

        /// <summary>
        /// GetXMLContentAsync trả về nội dung XML của thông điệp
        /// </summary>
        /// <param name="thongDiepChungId"></param>
        /// <returns></returns>
        public async Task<string> GetXMLContentAsync(string thongDiepChungId)
        {
            var query = await (from thongDiep in _db.ThongDiepChungs
                               join fileData in _db.FileDatas on thongDiep.ThongDiepChungId equals fileData.RefId
                               where thongDiep.ThongDiepChungId == thongDiepChungId
                               select fileData.Content).FirstOrDefaultAsync();
            return query;
        }

        //Các phương thức private ==============================================================

        //Method này sẽ thêm bản ghi vào bảng FileDatas
        private async Task ThemDuLieuVaoBangFileData(string refId, string data, string fileName, int type = 1, bool isSigned = false, byte bothCheckUpdateAndInsert = 3)
        {
            // Ghi chú: bothCheckUpdateAndInsert = 1 là thêm mới; 2 là update; 3 là vừa kiểm tra update và insert
            if (bothCheckUpdateAndInsert == 3)
            {
                var entityFileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == refId);
                if (entityFileData != null)
                {
                    //nếu đã có bản ghi thì cập nhật
                    entityFileData.Content = data;
                    entityFileData.FileName = fileName;
                    entityFileData.DateTime = DateTime.Now;
                    entityFileData.IsSigned = isSigned;
                    _db.FileDatas.Update(entityFileData);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //thêm bản ghi vào nếu chưa có
                    FileData fileData = new FileData
                    {
                        FileDataId = Guid.NewGuid().ToString(),
                        RefId = refId,
                        Type = type,
                        DateTime = DateTime.Now,
                        Content = data,
                        IsSigned = isSigned,
                        FileName = fileName
                    };
                    await _db.FileDatas.AddAsync(fileData);
                    await _db.SaveChangesAsync();
                }
            }
            else if (bothCheckUpdateAndInsert == 1)
            {
                //thêm bản ghi vào nếu chưa có
                FileData fileData = new FileData
                {
                    FileDataId = Guid.NewGuid().ToString(),
                    RefId = refId,
                    Type = type,
                    DateTime = DateTime.Now,
                    Content = data,
                    IsSigned = isSigned,
                    FileName = fileName
                };
                await _db.FileDatas.AddAsync(fileData);
                await _db.SaveChangesAsync();
            }
        }

        private async Task ThemAttachVaoBangTaiLieuDinhKem(string refId, string path)
        {
            TaiLieuDinhKem taiLieuDinhKem = new TaiLieuDinhKem
            {
                TaiLieuDinhKemId = Guid.NewGuid().ToString(),
                LoaiNghiepVu = RefType.ThongDiepGuiNhanCQT,
                NghiepVuId = refId,
                TenGoc = Path.GetFileName(path),
                TenGuid = Path.GetFileName(path),
                CreatedDate = DateTime.Now,
                Status = true
            };
            await _db.TaiLieuDinhKems.AddAsync(taiLieuDinhKem);
            await _db.SaveChangesAsync();
        }

        private async Task ThemAttachVaoBangFileData(string refId, string path, bool isSigned = false)
        {
            FileData fileData = new FileData
            {
                FileDataId = Guid.NewGuid().ToString(),
                RefId = refId,
                IsSigned = isSigned,
                Type = 4,
                DateTime = DateTime.Now,
                Binary = File.ReadAllBytes(path),
                FileName = Path.GetFileName(path),
            };
            await _db.FileDatas.AddAsync(fileData);
            await _db.SaveChangesAsync();
        }

        //Method này để chuyển nội dung file XML sang popco
        private T ConvertXMLFileToObject<T>(string xmlFilePath)
        {
            XDocument xd = XDocument.Load(xmlFilePath);
            if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT").Remove();
            }

            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            var model = (T)serialiser.Deserialize(xd.CreateReader());
            return model;
        }

        //Method này để chuyển nội dung chuỗi sang popco
        private T ConvertXMLDataToObject<T>(string xmlData)
        {
            XDocument xd = XDocument.Parse(xmlData);
            if (xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT") != null)
            {
                xd.XPathSelectElement("/TDiep/DLieu/TBao/DSCKS/NNT").Remove();
            }

            XmlSerializer serialiser = new XmlSerializer(typeof(T));
            var model = (T)serialiser.Deserialize(xd.CreateReader());
            return model;
        }

        //Method này để đọc thông tin người dùng đã đăng nhập
        //private string GetUserId()
        //{
        //    string nameIdentifier = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    return nameIdentifier;
        //}

        //Method này để convert chuỗi sang số
        private int ConvertToNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            var isNumeric = int.TryParse(value, out int giaTri);
            if (isNumeric)
            {
                return giaTri;
            }
            else
            {
                return 0;
            }
        }

        //Method này sẽ hiển thị diễn giải bên cạnh trạng thái hóa đơn
        private string GetDienGiaiTrangThai(HoaDonDienTu hoaDon, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon, List<HoaDonDienTu> listHoaDonDienTu)
        {
            var hinhThucXoaBo = hoaDon.HinhThucXoabo;
            var thayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId;

            //nếu là hóa đơn xóa bỏ mà hóa đơn thay thế chưa được cấp mã thì vẫn hiển thị là xóa bỏ
            var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
            if (hoaDonThayThe != null)
            {
                if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2 || hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc5)
                {
                    if (boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                    {
                        if (string.IsNullOrWhiteSpace(hoaDonThayThe.MaCuaCQT))
                        {
                            return "&nbsp;|&nbsp;Đã bị xóa bỏ";
                        }
                    }
                }
            }

            if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2)
            {
                return "&nbsp;|&nbsp;Bị thay thế";
            }
            else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
            {
                if (string.IsNullOrWhiteSpace(thayTheChoHoaDonId))
                {
                    return "&nbsp;|&nbsp;Hóa đơn gốc (LDO)";
                }
                else
                {
                    return "&nbsp;|&nbsp;Hóa đơn thay thế (LDO)";
                }
            }
            else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc5)
            {
                return "&nbsp;|&nbsp;Bị thay thế";
            }

            return "";
        }

        //Method này gợi ý phân loại hóa đơn là hủy/thay thế/điều chỉnh/giải trình
        private int GetGoiY(int? hinhThucXoaBo, string thayTheChoHoaDonId)
        {
            if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2)
            {
                return 3;
            }
            else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
            {
                return 1;
            }
            else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc5)
            {
                if (!string.IsNullOrWhiteSpace(thayTheChoHoaDonId))
                {
                    return 3;
                }
            }

            return 0;
        }

        //Method này xác định trạng thái hóa đơn hiện tại
        private int XacDinhTrangThaiHoaDon(HoaDonDienTu hoaDon, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon, List<HoaDonDienTu> listHoaDonDienTu)
        {
            var thayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId;
            var dieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId;
            var hinhThucXoaBo = hoaDon.HinhThucXoabo;
            var ngayGuiTBaoSaiSotKhongPhaiLapHD = hoaDon.NgayGuiTBaoSaiSotKhongPhaiLapHD;

            //nếu là hóa đơn xóa bỏ mà hóa đơn thay thế chưa được cấp mã thì vẫn hiển thị là xóa bỏ
            var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
            if (hoaDonThayThe != null)
            {
                if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2 || hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc5)
                {
                    if (boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                    {
                        if (string.IsNullOrWhiteSpace(hoaDonThayThe.MaCuaCQT))
                        {
                            if (string.IsNullOrWhiteSpace(thayTheChoHoaDonId) && string.IsNullOrWhiteSpace(dieuChinhChoHoaDonId))
                            {
                                return 1; //hóa đơn gốc
                            }
                            if (!string.IsNullOrWhiteSpace(thayTheChoHoaDonId))
                            {
                                return 3; //hóa đơn thay thế
                            }
                        }
                    }
                }
            }

            //nếu là hóa đơn gốc
            if (string.IsNullOrWhiteSpace(thayTheChoHoaDonId) && string.IsNullOrWhiteSpace(dieuChinhChoHoaDonId))
            {
                if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2)
                {
                    return 1; //hóa đơn gốc
                }
                else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
                {
                    return 2; //hóa đơn hủy
                }
                else
                {
                    //nếu có gửi thông báo sai sót ko phải lập lại hóa đơn
                    if (ngayGuiTBaoSaiSotKhongPhaiLapHD != null)
                    {
                        return 1; //hóa đơn gốc
                    }
                }
            }

            //nếu là hóa đơn thay thế
            if (!string.IsNullOrWhiteSpace(thayTheChoHoaDonId))
            {
                if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
                {
                    return 2; //hóa đơn hủy
                }
                else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc5)
                {
                    return 3; //hóa đơn thay thế
                }
                else
                {
                    //nếu có gửi thông báo sai sót ko phải lập lại hóa đơn
                    if (ngayGuiTBaoSaiSotKhongPhaiLapHD != null)
                    {
                        return 3; //hóa đơn thay thế
                    }
                }
            }

            //nếu là hóa đơn điều chỉnh
            if (!string.IsNullOrWhiteSpace(dieuChinhChoHoaDonId))
            {
                //nếu có gửi thông báo sai sót ko phải lập lại hóa đơn
                if (ngayGuiTBaoSaiSotKhongPhaiLapHD != null)
                {
                    return 4; //hóa đơn điều chỉnh
                }
            }

            return 0;
        }

        //Method này xác định loại sai sót dựa vào hiển thị ở giao diện
        private int XacDinhLoaiSaiSotDuaTrenGiaoDien(string thayTheChoHoaDonId, string dieuChinhChoHoaDonId, int? hinhThucXoaBo)
        {
            //nếu là hóa đơn gốc
            if (string.IsNullOrWhiteSpace(thayTheChoHoaDonId) && string.IsNullOrWhiteSpace(dieuChinhChoHoaDonId))
            {
                if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2)
                {
                    return 1; //xóa hóa đơn để lập thay thế
                }
                else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
                {
                    return 2; //hủy hóa đơn do hợp đồng mua bán bị hủy
                }
            }

            //nếu là hóa đơn thay thế
            if (!string.IsNullOrWhiteSpace(thayTheChoHoaDonId))
            {
                if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
                {
                    return 2; //hủy hóa đơn do hợp đồng mua bán bị hủy
                }
                else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc5)
                {
                    return 3; //xóa hóa đơn để lập hóa đơn thay thế mới
                }
            }

            return 0;
        }

        //Method này trả về chuỗi chứng từ liên quan
        private string XacDinhSoChungTuLienQuan(string phanLoai, int? trangThaiHoaDon, HoaDonDienTu hoaDon, List<HoaDonDienTu> listHoaDonDienTu, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon, HoaDonDienTu hoaDonDieuChinh = null, DLL.Entity.QuanLy.BoKyHieuHoaDon boKyHieuHoaDon_DieuChinh = null)
        {
            //ghi chú: chuỗi chứng từ liên quan gồm: tên hiển thị chứng từ liên quan và id của chứng từ liên quan
            //tên và id sẽ được phân cách nhau bởi dấu ;
            if (phanLoai == "huy_va_thaythe")
            {
                if (trangThaiHoaDon == 2) //nếu là các hóa đơn hủy thì trả về số chứng từ xóa bỏ
                {
                    return string.Format("{0};{1}", (hoaDon.SoCTXoaBo ?? ""), "XHD-" + hoaDon.HoaDonDienTuId);
                }
                else //nếu là hóa đơn bị thay thế thì trả về số hóa đơn thay thế
                {
                    var hoaDonThayThe = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
                    if (hoaDonThayThe != null)
                    {
                        if (boKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                        {
                            if (string.IsNullOrWhiteSpace(hoaDonThayThe.MaCuaCQT))
                            {
                                return "";
                            }
                            else
                            {
                                return string.Format("{0}-{1}-{2};{3}", (boKyHieuHoaDon.KyHieuMauSoHoaDon.ToString() + (boKyHieuHoaDon.KyHieuHoaDon ?? "")), hoaDonThayThe.SoHoaDon, hoaDonThayThe.NgayHoaDon?.ToString("dd/MM/yyyy"), hoaDonThayThe.HoaDonDienTuId);
                            }
                        }
                        else
                        {
                            return ""; //ko mã thì tạm để như vậy
                        }
                    }
                }
            }
            if (phanLoai == "dieuchinh")
            {
                //nếu là hóa đơn bị điều chỉnh thì trả về số hóa đơn điều chỉnh
                if (hoaDonDieuChinh == null)
                {
                    var hoaDonDC = listHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();
                    if (hoaDonDC != null)
                    {
                        return string.Format("{0}-{1}-{2};{3}", (boKyHieuHoaDon.KyHieuMauSoHoaDon.ToString() + (boKyHieuHoaDon.KyHieuHoaDon ?? "")), hoaDonDC.SoHoaDon, hoaDonDC.NgayHoaDon?.ToString("dd/MM/yyyy"), hoaDonDC.HoaDonDienTuId);
                    }
                }
                else return string.Format("{0}-{1}-{2};{3}", (boKyHieuHoaDon_DieuChinh.KyHieuMauSoHoaDon.ToString() + (boKyHieuHoaDon_DieuChinh.KyHieuHoaDon ?? "")), hoaDonDieuChinh.SoHoaDon, hoaDonDieuChinh.NgayHoaDon?.ToString("dd/MM/yyyy"), hoaDonDieuChinh.HoaDonDienTuId);
            }

            return "";
        }

        //Method này gợi ý lý do sai sót
        private string GetGoiYLyDoSaiSot(HoaDonDienTu hoaDon, List<HoaDonDienTu> listHoaDonDienTu, List<ThongBaoSaiThongTin> listThongBaoSaiThongTin, string phanLoaiBanGhi, HoaDonDienTu hddc = null, BoKyHieuHoaDon bkhhd = null)
        {
            if (hoaDon.HinhThucXoabo != null)
            {
                switch ((HinhThucXoabo)hoaDon.HinhThucXoabo)
                {
                    // Nếu là xóa để lập thay thế || xóa để lập thay thế mới
                    case HinhThucXoabo.HinhThuc2:
                    case HinhThucXoabo.HinhThuc4:
                        var hoaDonThayThe = listHoaDonDienTu.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId);
                        if (hoaDonThayThe != null)
                        {
                            string lyDoXoaBo = string.Empty;

                            if (!string.IsNullOrEmpty(hoaDonThayThe.LyDoXoaBo))
                            {
                                lyDoXoaBo = $"{hoaDonThayThe.LyDoXoaBo}. ";
                            }

                            return $"{lyDoXoaBo}Sai sót này đã được xử lý bằng hình thức lập hóa đơn thay thế có ký hiệu {hoaDonThayThe.BoKyHieuHoaDon.KyHieu} số hóa đơn {hoaDonThayThe.SoHoaDon} ngày {hoaDonThayThe.NgayHoaDon:dd/MM/yyyy}";
                        }
                        break;
                    default:
                        return hoaDon.LyDoXoaBo;
                }

            }
            else
            {
                if (phanLoaiBanGhi == "hoaDonBiDieuChinh")
                {
                    if (hddc != null)
                    {
                        var lyDoDieuChinhModel = string.IsNullOrWhiteSpace(hddc.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hddc.LyDoDieuChinh);
                        if (lyDoDieuChinhModel != null)
                        {
                            string lyDoDieuChinh = string.Empty;

                            if (!string.IsNullOrEmpty(lyDoDieuChinhModel.LyDo))
                            {
                                lyDoDieuChinh = $"{lyDoDieuChinhModel.LyDo}. ";
                            }

                            return $"{lyDoDieuChinh}Sai sót này đã được xử lý bằng hình thức lập hóa đơn điều chỉnh có ký hiệu {bkhhd.KyHieu} số hóa đơn {hddc.SoHoaDon} ngày {hddc.NgayHoaDon:dd/MM/yyyy}";
                        }
                    }
                    else
                    {
                        //nếu đã bị điều chỉnh thì ko cần kiểm tra có gửi thông báo sai thông tin hay ko
                        var hoaDonDieuChinh = listHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId)?.OrderByDescending(y => y.CreatedDate)?.Take(1)?.FirstOrDefault();

                        if (hoaDonDieuChinh != null)
                        {
                            var lyDoDieuChinhModel = string.IsNullOrWhiteSpace(hoaDonDieuChinh.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hoaDonDieuChinh.LyDoDieuChinh);
                            if (lyDoDieuChinhModel != null)
                            {
                                string lyDoDieuChinh = string.Empty;

                                if (!string.IsNullOrEmpty(lyDoDieuChinhModel.LyDo))
                                {
                                    lyDoDieuChinh = $"{lyDoDieuChinhModel.LyDo}. ";
                                }

                                return $"{lyDoDieuChinh}Sai sót này đã được xử lý bằng hình thức lập hóa đơn điều chỉnh có ký hiệu {hoaDonDieuChinh.BoKyHieuHoaDon.KyHieu} số hóa đơn {hoaDonDieuChinh.SoHoaDon} ngày {hoaDonDieuChinh.NgayHoaDon:dd/MM/yyyy}";
                            }
                        }
                    }
                }
                else if (phanLoaiBanGhi == "emailThongBaoSaiThongTin")
                {
                    //kiểm tra đã gửi thông báo sai thông tin hay chưa
                    var thongBaoSaiThongTin = listThongBaoSaiThongTin.Where(x => x.HoaDonDienTuId == hoaDon.HoaDonDienTuId).OrderByDescending(x => x.CreatedDate).Take(1).FirstOrDefault();
                    if (thongBaoSaiThongTin != null)
                    {
                        var thongBaoSaiSot = "";
                        if (!string.IsNullOrWhiteSpace(thongBaoSaiThongTin.HoTenNguoiMuaHang_Dung))
                        {
                            if (thongBaoSaiThongTin.HoTenNguoiMuaHang_Dung.TrimToUpper() != thongBaoSaiThongTin.HoTenNguoiMuaHang_Sai.TrimToUpper())
                            {
                                thongBaoSaiSot = "Họ và tên người mua hàng đúng là: " + thongBaoSaiThongTin.HoTenNguoiMuaHang_Dung;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(thongBaoSaiThongTin.TenDonVi_Dung))
                        {
                            if (thongBaoSaiThongTin.TenDonVi_Dung.TrimToUpper() != thongBaoSaiThongTin.TenDonVi_Sai.TrimToUpper())
                            {
                                if (string.IsNullOrWhiteSpace(thongBaoSaiSot))
                                {
                                    thongBaoSaiSot = "Tên đơn vị đúng là: " + thongBaoSaiThongTin.TenDonVi_Dung;
                                }
                                else
                                {
                                    thongBaoSaiSot += "; Tên đơn vị đúng là: " + thongBaoSaiThongTin.TenDonVi_Dung;
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(thongBaoSaiThongTin.DiaChi_Dung))
                        {
                            if (thongBaoSaiThongTin.DiaChi_Dung.TrimToUpper() != thongBaoSaiThongTin.DiaChi_Sai.TrimToUpper())
                            {
                                if (string.IsNullOrWhiteSpace(thongBaoSaiSot))
                                {
                                    thongBaoSaiSot = "Địa chỉ đúng là: " + thongBaoSaiThongTin.DiaChi_Dung;
                                }
                                else
                                {
                                    thongBaoSaiSot += "; Địa chỉ đúng là: " + thongBaoSaiThongTin.DiaChi_Dung;
                                }
                            }
                        }

                        return thongBaoSaiSot;
                    }
                }
            }

            return "";
        }

        //Method này get diễn giải trạng thái của hóa đơn nhập ngoài (ví dụ: hóa đơn 32,...)
        private string GetDienGiaiTrangThaiHoaDonKhacCuaNNT(string thayTheChoHoaDonId, string dieuChinhChoHoaDonId)
        {
            if (!string.IsNullOrWhiteSpace(thayTheChoHoaDonId)) return "Bị thay thế";
            if (!string.IsNullOrWhiteSpace(dieuChinhChoHoaDonId)) return "Bị điều chỉnh";

            return "";
        }

        //Method này xác định id của chứng từ liên quan
        private string GetIdChungTuLienQuan(string hoaDonDienTuId, List<HoaDonDienTu> listHoaDonDienTu, bool guiEmailKhongPhaiLapLaiHoaDon)
        {
            if (guiEmailKhongPhaiLapLaiHoaDon)
            {
                return "KhongLapLaiHoaDon";
            }
            else
            {
                var idsHoaDonLienQuan = "";
                var listHoaDonThayTheLienQuan = listHoaDonDienTu.Where(x => x.ThayTheChoHoaDonId == hoaDonDienTuId).Select(y => y.HoaDonDienTuId).ToList();
                var listHoaDonDieuChinhLienQuan = listHoaDonDienTu.Where(x => x.DieuChinhChoHoaDonId == hoaDonDienTuId).Select(y => y.HoaDonDienTuId).ToList();
                idsHoaDonLienQuan = string.Join(";", listHoaDonThayTheLienQuan) + ";" + string.Join(";", listHoaDonDieuChinhLienQuan);
                return idsHoaDonLienQuan;
            }
        }

        //Method này để convert chuỗi sang mảng string: convert chuỗi gồm nhiều id sang mảng id
        private string[] GetListIdChungTuLienQuan(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new string[] { };
            }
            else
            {
                return input.Split(";");
            }
        }

        //Method này trả về tên loại hóa đơn được nhập trong bảng thông tin hóa đơn, được dùng trong API bảng kê
        private string GetLoaiHoaDon(string mauSoHoaDon)
        {
            if (string.IsNullOrWhiteSpace(mauSoHoaDon)) return "";

            if (mauSoHoaDon.StartsWith("01GTKT"))
            {
                return "Hóa đơn GTGT";
            }
            else
            if (mauSoHoaDon.StartsWith("02GTTT"))
            {
                return "Hóa đơn bán hàng";
            }
            else
            if (mauSoHoaDon.StartsWith("06HDXK"))
            {
                return "Hóa đơn xuất khẩu";
            }
            else
            if (mauSoHoaDon.StartsWith("07KPTQ"))
            {
                return "Hóa đơn bán hàng (dành cho tổ chức, cá nhân trong khu phi thuế quan)";
            }
            else
            if (mauSoHoaDon.StartsWith("03XKNB"))
            {
                return "Phiếu xuất kho kiêm vận chuyển hàng hóa nội bộ";
            }
            else
            if (mauSoHoaDon.StartsWith("04HGDL"))
            {
                return "Phiếu xuất kho gửi bán hàng đại lý";
            }

            return "";
        }

        //Method này trả về tên loại thông báo sai sót, được dùng trong API bảng kê
        private LoaiThongBaoSaiSotViewModel GetLoaiThongBaoSaiSot(string ThongBaoHoaDonRaSoatId, bool? IsTBaoHuyGiaiTrinhKhacCuaNNT)
        {
            if (IsTBaoHuyGiaiTrinhKhacCuaNNT.GetValueOrDefault())
            {
                return new LoaiThongBaoSaiSotViewModel
                {
                    TenLoaiThongBaoSaiSot = "Thông báo hủy/giải trình của NNT (KHÁC)",
                    LoaiThongBaoSaiSot = 3
                };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ThongBaoHoaDonRaSoatId))
                {
                    return new LoaiThongBaoSaiSotViewModel
                    {
                        TenLoaiThongBaoSaiSot = "Thông báo hủy/giải trình của NNT theo thông báo của CQT",
                        LoaiThongBaoSaiSot = 2
                    };
                }
            }

            return new LoaiThongBaoSaiSotViewModel
            {
                TenLoaiThongBaoSaiSot = "Thông báo hủy/giải trình của NNT",
                LoaiThongBaoSaiSot = 1
            };
        }

        //Method này trả về thông tin loại sai sót, được dùng trong API bảng kê
        private string GetLoaiSaiSot(byte loaiSaiSot)
        {
            switch (loaiSaiSot)
            {
                case 1:
                    return "Hủy";
                case 2:
                    return "Điều chỉnh";
                case 3:
                    return "Thay thế";
                case 4:
                    return "Giải trình";
                default:
                    return "";
            }
        }

        //Method này đọc tên người ký và thời gian ký
        private ThongTinChuKySoViewModel GetThongTinNguoiKy(XmlDocument xmlDoc, string loaiChuKy)
        {
            //loaiChuKy: là các thẻ chữ ký số trong thẻ <DSCKS>. Ở trong file này: dùng 2 loại là TTCQT và CQT

            var signature = xmlDoc.SelectNodes("/TDiep/DLieu/TBao/DSCKS/" + loaiChuKy);
            if (signature != null)
            {
                if (signature.Count > 0)
                {
                    var tenNguoiKy = "";
                    var ngayKy = "";

                    //đọc ra tên người ký
                    var node_Signature = signature[0]["Signature"];
                    var node_KeyInfo = node_Signature["KeyInfo"];
                    var node_X509Data = node_KeyInfo["X509Data"];
                    var node_X509SubjectName = node_X509Data["X509SubjectName"];
                    var chuoiTenNguoiKy = node_X509SubjectName.InnerText;
                    var tachChuoiTenNguoiKy = chuoiTenNguoiKy.Split(",");
                    var tachTenNguoiKy = tachChuoiTenNguoiKy.FirstOrDefault(x => x.Trim().StartsWith("CN="));
                    var tachTenNguoiKy2 = tachTenNguoiKy.Split("=");
                    if (tachTenNguoiKy2.Length > 0)
                    {
                        tenNguoiKy = tachTenNguoiKy2[1]; //lấy index = 1 vì tên người ký đứng sau chữ CN=
                    }

                    //đọc ra thời gian ký
                    var node_Object = node_Signature["Object"];
                    var node_SignatureProperties = node_Object["SignatureProperties"];
                    var node_SignatureProperty = node_SignatureProperties["SignatureProperty"];
                    var node_SigningTime = node_SignatureProperty["SigningTime"];
                    ngayKy = node_SigningTime.InnerText;

                    return new ThongTinChuKySoViewModel
                    {
                        TenNguoiKy = tenNguoiKy,
                        NgayKy = ngayKy
                    };
                }
            }

            return null;
        }
        private string GetTCTBao(int tc)
        {
            string tchat = string.Empty;
            switch (tc)
            {
                case 0:
                    tchat = "Mới";
                    break;
                case 1:
                    tchat = "Hủy";
                    break;
                case 2:
                    tchat = "Điều chỉnh";
                    break;
                case 3:
                    tchat = "Thay thế";
                    break;
                case 4:
                    tchat = "Giải trình";
                    break;
                case 5:
                    tchat = "Sai sót do tổng hợp";
                    break;
                default:
                    break;
            }
            return tchat;
        }
        /// <summary>
        /// GetPdfFile301Async trả về đường dẫn file pdf của 301
        /// </summary>
        /// <param name="idThongDiepChung"></param>
        /// <returns></returns>
        public async Task<KetQuaConvertPDF> GetPdfFile301Async(string thongDiepChungId)
        {
            var xmlContent301 = await (from fileData in _db.FileDatas
                                       join thongDiepChung in _db.ThongDiepChungs on fileData.RefId equals thongDiepChung.ThongDiepChungId
                                       where thongDiepChung.ThongDiepChungId == thongDiepChungId
                                       select fileData.Content).FirstOrDefaultAsync();
            if (xmlContent301 != null)
            {
                string pdfFileName = string.Empty;
                var path = string.Empty;
                var pathXML = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent301);
                var chuKyCua_TTCQT = GetThongTinNguoiKy(xmlDoc, "TTCQT");
                var chuKyCua_CQT = GetThongTinNguoiKy(xmlDoc, "CQT");

                var tDiep301 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(xmlContent301);

                if (tDiep301 != null)
                {
                    //điền dữ liệu từ XML vào file word
                    Document doc = new Document();
                    string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/QuanLy/thong_bao_301.docx");
                    doc.LoadFromFile(docFolder);

                    var coQuanThueDaChapNhanTatCa = true; //nếu 301 cho biết CQT đã chấp nhận tất cả thì coQuanThueDaChapNhanTatCa = true
                    coQuanThueDaChapNhanTatCa = tDiep301.DLieu.TBao.DLTBao.DSHDon.Count(x => x.TTTNCCQT == 2) <= 0;

                    var sections = doc.Sections;
                    if (sections.Count > 0)
                    {
                        var paragraphs = doc.Sections[0].Paragraphs;
                        if (coQuanThueDaChapNhanTatCa)
                        {
                            //nếu CQT đã chấp nhận tất cả hóa đơn thì xóa paragraph số 9
                            paragraphs.RemoveAt(9);

                            doc.Replace("<tiepnhan>", string.Format("tiếp nhận"), true, true);

                            doc.Sections[0].Tables.RemoveAt(2);

                            paragraphs.RemoveAt(9);
                            paragraphs.RemoveAt(9);
                        }
                        else
                        {
                            //nếu CQT không chấp nhận tất cả hóa đơn thì xóa paragraph số 8
                            paragraphs.RemoveAt(8);
                            doc.Replace("<tiepnhan>", string.Format("không tiếp nhận"), true, true);

                            int line = tDiep301.DLieu.TBao.DLTBao.DSHDon.Count;
                            Table table = null;
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
                                    if (stt.ToUpper().Contains("STT"))
                                    {
                                        table = tb;
                                        break;
                                    }
                                }
                            }

                            int beginRow = 1;
                            for (int i = 0; i < line - 1; i++)
                            {
                                // Clone row
                                TableRow cl_row = table.Rows[beginRow].Clone();
                                table.Rows.Insert(beginRow, cl_row);
                            }

                            TableRow row = null;
                            for (int i = 0; i < line; i++)
                            {
                                // Get row for add data
                                row = table.Rows[i + beginRow];

                                _par = row.Cells[0].Paragraphs[0];
                                _par.Text = (i + 1).ToString();

                                _par = row.Cells[1].Paragraphs[0];
                                _par.Text = tDiep301.DLieu.TBao.DLTBao.DSHDon[i].MCQTCap;

                                _par = row.Cells[2].Paragraphs[0];
                                _par.Text = string.Format("{0}/{1}", tDiep301.DLieu.TBao.DLTBao.DSHDon[i].KHMSHDon, tDiep301.DLieu.TBao.DLTBao.DSHDon[i].KHHDon);

                                _par = row.Cells[3].Paragraphs[0];
                                _par.Text = tDiep301.DLieu.TBao.DLTBao.DSHDon[i].SHDon;

                                //thời gian nhận
                                DateTime nLap = DateTime.Now;
                                if (!string.IsNullOrWhiteSpace(tDiep301.DLieu.TBao.DLTBao.DSHDon[i].NLap))
                                {
                                    nLap = DateTime.Parse(tDiep301.DLieu.TBao.DLTBao.DSHDon[i].NLap);
                                }
                                _par = row.Cells[4].Paragraphs[0];
                                _par.Text = nLap.ToString("dd/MM/yyyy");

                                _par = row.Cells[5].Paragraphs[0];
                                _par.Text = GetTCTBao(tDiep301.DLieu.TBao.DLTBao.DSHDon[i].TCTBao);

                                _par = row.Cells[6].Paragraphs[0];
                                foreach (var item in tDiep301.DLieu.TBao.DLTBao.DSHDon[i].DSLDKTNhan)
                                {
                                    _par.Text += string.Format(item.MTa, Environment.NewLine);
                                }
                            }
                        }
                    }

                    doc.Replace("<TCQTCTren>", tDiep301.DLieu.TBao.DLTBao.TCQTCTren, true, true);
                    doc.Replace("<TCQT>", tDiep301.DLieu.TBao.DLTBao.TCQT, true, true);
                    doc.Replace("<SoTB>", tDiep301.DLieu.TBao.STBao.So, true, true);
                    doc.Replace("<DDanh>", (tDiep301.DLieu.TBao.DLTBao.DDanh ?? ""), true, true);

                    var ngayThangNam = tDiep301.DLieu.TBao.STBao.NTBao;
                    var mangNgayThangNam = ngayThangNam.Split("-"); //mangNgayThangNam có định dạng yyyy-mm-dd
                    doc.Replace("<NTBao>", string.Format("ngày {0} tháng {1} năm {2}", mangNgayThangNam[2], mangNgayThangNam[1], mangNgayThangNam[0]), true, true);

                    doc.Replace("<TNNT>", tDiep301.DLieu.TBao.DLTBao.TNNT, true, true);
                    doc.Replace("<MST>", tDiep301.DLieu.TBao.DLTBao.MST, true, true);

                    //thời gian nhận
                    DateTime thoiGianNhan = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(tDiep301.DLieu.TBao.DLTBao.TGNhan))
                    {
                        thoiGianNhan = DateTime.Parse(tDiep301.DLieu.TBao.DLTBao.TGNhan);
                    }
                    doc.Replace("<TGNhan>", thoiGianNhan.ToString("dd/MM/yyyy"), true, true);
                    doc.Replace("<chucvu>", tDiep301.DLieu.TBao.DLTBao.TCQT.ToUpper(), true, true);

                    //thêm chữ ký số
                    if (chuKyCua_TTCQT != null) //thêm chữ ký số của thủ trưởng CQT
                    {
                        //ImageHelper.AddSignatureImageToDocV2("<DIGITALSIGNATURETTCQT>", doc, chuKyCua_TTCQT.TenNguoiKy, LoaiNgonNgu.TiengViet, chuKyCua_TTCQT.NgayKy);

                        ImageHelper.CreateSignatureBox(doc, chuKyCua_TTCQT.TenNguoiKy, DateTime.Parse(chuKyCua_TTCQT.NgayKy), "<DIGITALSIGNATURETTCQT>");
                    }
                    else
                    {
                        doc.Replace("<DIGITALSIGNATURETTCQT>", "", true, true);
                    }
                    if (chuKyCua_CQT != null) //thêm chữ ký số của CQT
                    {
                        //ImageHelper.AddSignatureImageToDocV2("<DIGITALSIGNATURECQT>", doc, chuKyCua_CQT.TenNguoiKy, LoaiNgonNgu.TiengViet, chuKyCua_CQT.NgayKy);

                        ImageHelper.CreateSignatureBox(doc, chuKyCua_CQT.TenNguoiKy, DateTime.Parse(chuKyCua_CQT.NgayKy), "<DIGITALSIGNATURECQT>");
                    }
                    else
                    {
                        doc.Replace("<DIGITALSIGNATURECQT>", "", true, true);
                    }

                    //tạo thư mục để lưu các file dữ liệu
                    var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    string assetsFolder = $"FilesUpload/{databaseName}";
                    var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.FILE_ATTACH);
                    //try
                    //{
                    //    if (!Directory.Exists(fullFolder))
                    //    {
                    //        Directory.CreateDirectory(fullFolder);
                    //    }
                    //}
                    //catch (Exception) { }

                    //var tenFile = "TD-" + Guid.NewGuid().ToString();

                    ////lưu file pdf
                    //var duongDanLuuFilePdf = fullFolder + "/" + tenFile + ".pdf";
                    //doc.SaveToFile(duongDanLuuFilePdf, FileFormat.PDF);

                    //doc.Close();

                    //var duongDanTraVeFilePdf = Path.Combine(assetsFolder, ManageFolderPath.FILE_ATTACH) + "/" + tenFile + ".pdf";
                    //return duongDanTraVeFilePdf;
                    pdfFileName = string.Format("ThongBao-301-{0}-{1}{2}", tDiep301.DLieu.TBao.DLTBao.MST, tDiep301.TTChung.MTDiep, ".pdf");
                    #region create folder
                    if (!Directory.Exists(fullFolder))
                    {
                        Directory.CreateDirectory(fullFolder);
                    }
                    else
                    {

                        string oldFilePath = Path.Combine(fullFolder, pdfFileName);
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                    string fullPdfFilePath = Path.Combine(fullFolder, pdfFileName);
                    //doc.SaveToFile(fullPdfFilePath, Spire.Doc.FileFormat.PDF);
                    doc.SaveToPDF(fullPdfFilePath, _hostingEnvironment, LoaiNgonNgu.TiengViet);
                    path = $"FilesUpload/{databaseName}/{ManageFolderPath.FILE_ATTACH}/{pdfFileName}";
                    #endregion

                    return new KetQuaConvertPDF()
                    {
                        FilePDF = path,
                        FileXML = pathXML,
                        PdfName = pdfFileName,
                        PDFBase64 = fullPdfFilePath.EncodeFile()
                    };

                }
            }

            return null;
        }
        public string GetLinkFileXml(string fileName)
        {
            string assetsFolder = $"FilesUpload//ThongDiepGui";
            return $"{_IHttpContextAccessor.HttpContext.Request.PathBase}/{assetsFolder}/{fileName}";
        }
        /// <summary>
        /// Xóa file xml
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DeleteFileXML(string fileName)
        {
            string assetsFolder = $"FilesUpload/ThongDiepGui";
            string linkFile = $"{_hostingEnvironment.WebRootPath}/{assetsFolder}/{fileName}";

            try
            {
                if (File.Exists(linkFile))
                {
                    File.Delete(linkFile);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Create file xml to download trên view chi tiết thông điệp
        /// </summary>
        /// <param name="insertXMLSigned"></param>
        /// <returns></returns>
        public CreateFileXMLViewModel InsertFileXMLSigned(string XMLSignedEncode, string createdDate)
        {
            var valueBytes = TextHelper.Base64Decode(XMLSignedEncode);
            CreateFileXMLViewModel fileXMLParams = new CreateFileXMLViewModel
            {
                FileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.xml"
            };
            string assetsFolder = $"FilesUpload/ThongDiepGui/";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            // Save file
            if (!Directory.Exists(fullXmlFolder))
            {
                Directory.CreateDirectory(fullXmlFolder);
            }

            // Write to file
            // Đọc file xml lấy mã loại thông điệp
            XDocument doc = XDocument.Parse(valueBytes);
            int MLTDiep = int.Parse(doc.XPathSelectElement("/TDiep/TTChung/MLTDiep").Value);
            fileXMLParams.FileName = $"{MLTDiep}-{createdDate}.xml";
            string filePath = Path.Combine(fullXmlFolder, fileXMLParams.FileName);
            File.WriteAllText(filePath, valueBytes);
            return fileXMLParams;
            // Add To log
        }
        /// <summary>
        /// Lấy danh sách các thông  điệp liên qua đến thông điệp ID truyền vào
        /// </summary>
        /// <param name="maThongDiep"></param>
        /// <returns></returns>
        public async Task<List<ThongDiepChungViewModel>> GetAllThongDiepLienQuan(string maThongDiep)
        {
            var query = from tdc in _db.ThongDiepChungs
                        where tdc.MaThongDiep == maThongDiep || tdc.MaThongDiepThamChieu == maThongDiep || tdc.MaThongDiepPhanHoi == maThongDiep
                        orderby tdc.CreatedDate
                        select new ThongDiepChungViewModel
                        {
                            ThongDiepChungId = tdc.ThongDiepChungId,
                            CreatedDate = tdc.CreatedDate,
                            NgayThongBao = tdc.NgayThongBao,
                            MaNoiGui = tdc.MaNoiGui,
                            MaNoiNhan = tdc.MaNoiNhan,
                            MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                            MaThongDiep = tdc.MaThongDiep,
                            MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                            DataXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == tdc.MaThongDiep).XMLData
                        };
            return await query.ToListAsync();
        }

        /// <summary>
        /// generate files if not exists in server
        /// </summary>
        /// <param name="ThongDiepGuiCQTId"></param>
        /// <returns></returns>
        public async Task GenerateFileIfNotExistsAsync(string ThongDiepGuiCQTId)
        {
            var databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}", ManageFolderPath.FILE_ATTACH);
            if (!Directory.Exists(fullFolder))
            {
                Directory.CreateDirectory(fullFolder);
            }

            var entityModel = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == ThongDiepGuiCQTId);
            if(entityModel.DaKyGuiCQT == false)
            {
                var fileDinhKems = entityModel.FileDinhKem.Split(";");
                var fileDatas = await _db.FileDatas.Where(x => x.RefId == ThongDiepGuiCQTId && x.IsSigned == false && x.Binary != null && !string.IsNullOrEmpty(x.FileName)).AsNoTracking().ToListAsync();
                foreach (var item in fileDatas)
                {
                    string filePath = Path.Combine(fullFolder, item.FileName);
                    if (!File.Exists(filePath))
                    {
                        File.WriteAllBytes(filePath, item.Binary);
                    }

                    var file_ext = Path.GetExtension(item.FileName);
                    var fileDinhKem = fileDinhKems.FirstOrDefault(x => Path.GetExtension(x) == file_ext);
                    if (!string.IsNullOrEmpty(fileDinhKem))
                    {
                        filePath = Path.Combine(fullFolder, fileDinhKem);
                        if (!File.Exists(filePath))
                        {
                            File.WriteAllBytes(filePath, item.Binary);
                        }
                    }
                }
            }
            else
            {
                var fileDinhKems = entityModel.FileDinhKem.Split(";");
                var fileDatas = await _db.FileDatas.Where(x => x.RefId == ThongDiepGuiCQTId && x.IsSigned == true && x.Binary != null && !string.IsNullOrEmpty(x.FileName)).ToListAsync();
                if (fileDatas.Any())
                {
                    foreach (var item in fileDatas)
                    {
                        string filePath = Path.Combine(fullFolder, item.FileName);
                        if (!File.Exists(filePath))
                        {
                            File.WriteAllBytes(filePath, item.Binary);
                        }

                        var file_ext = Path.GetExtension(item.FileName);
                        var fileDinhKem = fileDinhKems.FirstOrDefault(x => Path.GetExtension(x) == file_ext);
                        if (!string.IsNullOrEmpty(fileDinhKem))
                        {
                            filePath = Path.Combine(fullFolder, fileDinhKem);
                            if (!File.Exists(filePath))
                            {
                                File.WriteAllBytes(filePath, item.Binary);
                            }
                        }
                    }
                }
                else
                {
                    var tenFile = "TD-" + Guid.NewGuid().ToString();
                    await CreateWordAndPdfFile(tenFile, _mp.Map<ThongDiepGuiCQTViewModel>(entityModel), false);
                    var fileDataSigneds = await _db.FileDatas.Where(x => x.RefId == ThongDiepGuiCQTId && x.IsSigned == true && x.Binary != null && !string.IsNullOrEmpty(x.FileName)).AsNoTracking().ToListAsync();
                    if (fileDataSigneds.Any())
                    {
                        foreach (var item in fileDataSigneds)
                        {
                            string filePath = Path.Combine(fullFolder, item.FileName);
                            if (!File.Exists(filePath))
                            {
                                File.WriteAllBytes(filePath, item.Binary);
                            }

                            var file_ext = Path.GetExtension(item.FileName);
                            var fileDinhKem = fileDinhKems.FirstOrDefault(x => Path.GetExtension(x) == file_ext);
                            if (!string.IsNullOrEmpty(fileDinhKem))
                            {
                                filePath = Path.Combine(fullFolder, fileDinhKem);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                File.WriteAllBytes(filePath, item.Binary);
                            }
                        }
                    }
                }
            }
        }

        public async Task<string> GetBase64XmlThongDiepChuaKyAsync(string ThongDiepGuiCQTId)
        {
            var fileData = await _db.FileDatas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RefId == ThongDiepGuiCQTId && x.Type == 1 && x.IsSigned != true);

            if (fileData != null)
            {
                var result = TextHelper.Base64Encode(fileData.Content);
                return result;
            }

            return null;
        }
    }
}
