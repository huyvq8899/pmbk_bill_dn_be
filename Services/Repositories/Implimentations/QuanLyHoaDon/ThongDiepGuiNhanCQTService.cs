﻿using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.HoaDonSaiSot;
using Services.Helper.Params.Filter;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
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
                            LoaiThongBao = (thongDiep.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)? ((byte)3): (string.IsNullOrWhiteSpace(thongDiep.ThongBaoHoaDonRaSoatId) ? (byte)1 : (byte)2),
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
                                                           SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == chiTiet.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
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
                                                           TrangThaiHoaDon = chiTiet.TrangThaiHoaDon
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
                             SoHoaDon = hoadon.SoHoaDon ?? "",
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
                          x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.TrimToUpper() &&
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
                          x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.TrimToUpper() &&
                          x.NgayLapHoaDon == hoadon.NgayLapHoaDon) > 0
                                          select new HoaDonHeThongViewModel
                                          {
                                              HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                              MauHoaDon = hoadon.MauHoaDon,
                                              KyHieuHoaDon = hoadon.KyHieuHoaDon,
                                              SoHoaDon = hoadon.SoHoaDon,
                                              NgayLapHoaDon = hoadon.NgayLapHoaDon,
                                              PhanLoaiTrungHoaDon = 2,

                                              MauHoaDonThayThe = query.FirstOrDefault(x=>x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.MauHoaDon,
                                              KyHieuHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.KyHieuHoaDon,
                                              SoHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.SoHoaDon,
                                              NgayLapHoaDonThayThe = query.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoadon.HoaDonDienTuId)?.NgayLapHoaDon
                                          }).ToList();

            //kiểm tra trùng với hóa đơn bị điều chỉnh thì hiển thị ra hóa đơn điều chỉnh liên quan
            var listTrungHoaDonBiDieuChinh = (from hoadon in listHoaDonBiDieuChinh
                                              where
                          @params.Count(x => x.MauHoaDon.TrimToUpper() == hoadon.MauHoaDon.TrimToUpper() &&
                          x.KyHieuHoaDon.TrimToUpper() == hoadon.KyHieuHoaDon.TrimToUpper() &&
                          x.SoHoaDon.TrimToUpper() == hoadon.SoHoaDon.TrimToUpper() &&
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
            List<HoaDonDienTu> queryHoaDonDienTu = new List<HoaDonDienTu>();

            var queryBoKyHieuHoaDon = await (from boKyHieuHoaDon in _db.BoKyHieuHoaDons
                                      select new DLL.Entity.QuanLy.BoKyHieuHoaDon
                                      {
                                        BoKyHieuHoaDonId = boKyHieuHoaDon.BoKyHieuHoaDonId,
                                        HinhThucHoaDon = boKyHieuHoaDon.HinhThucHoaDon,
                                        KyHieuMauSoHoaDon = boKyHieuHoaDon.KyHieuMauSoHoaDon,
                                        KyHieuHoaDon = boKyHieuHoaDon.KyHieuHoaDon
                                      }).ToListAsync();

            var querySoLanGuiCQT = await _db.ThongDiepChiTietGuiCQTs.ToListAsync();
            List<ThongBaoSaiThongTin> queryThongBaoSaiThongTin = new List<ThongBaoSaiThongTin>();

            DateTime ? fromDate = null;
            DateTime? toDate = null;
            if (!string.IsNullOrWhiteSpace(@params.FromDate))
            {
                fromDate = DateTime.Parse(@params.FromDate);
            }
            if (!string.IsNullOrWhiteSpace(@params.ToDate))
            {
                toDate = DateTime.Parse(@params.ToDate);
            }

            if (string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId))
            {
                queryHoaDonDienTu = await _db.HoaDonDienTus.ToListAsync();
                queryThongBaoSaiThongTin = await _db.ThongBaoSaiThongTins.ToListAsync();
            }
            else
            {
                queryHoaDonDienTu = await (from hoadon in _db.HoaDonDienTus
                                           where hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId 
                                           || hoadon.ThayTheChoHoaDonId == @params.LapTuHoaDonDienTuId 
                                           || hoadon.DieuChinhChoHoaDonId == @params.LapTuHoaDonDienTuId 
                                           select hoadon).ToListAsync();
                if (queryHoaDonDienTu.Count > 0)
                {
                    @params.TrangThaiGuiHoaDon = (int)(queryHoaDonDienTu.FirstOrDefault(x=>x.HoaDonDienTuId == @params.LapTuHoaDonDienTuId)?.TrangThaiGuiHoaDon.GetValueOrDefault());
                }

                queryThongBaoSaiThongTin = await _db.ThongBaoSaiThongTins.Where(x=>x.HoaDonDienTuId == @params.LapTuHoaDonDienTuId).ToListAsync();
            }

            List<string> listEmail = new List<string>();
            if (@params.TrangThaiGuiHoaDon == (int)TrangThaiGuiHoaDon.ChuaGui)
            {
                listEmail = await (from email in _db.NhatKyGuiEmails 
                             where email.TrangThaiGuiEmail == TrangThaiGuiEmail.GuiLoi
                                   select email.RefId).ToListAsync();
            }
            else
            {
                listEmail = await (from email in _db.NhatKyGuiEmails
                                   where email.TrangThaiGuiEmail != 0 
                                   select email.RefId).ToListAsync();
            }

            List < HoaDonSaiSotViewModel > query = new List<HoaDonSaiSotViewModel>();

            if (@params.IsTBaoHuyGiaiTrinhKhacCuaNNT != true)
            {
                if (@params.TrangThaiGuiHoaDon == (int)TrangThaiGuiHoaDon.ChuaGui)
                {
                    var queryHoaDonHuy = from hoadon in queryHoaDonDienTu 
                                         join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                         where
                                         ((listEmail.Contains(hoadon.HoaDonDienTuId) || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)) && 
                                         (hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc1
                                         || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc4
                                         || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc6) 
                                         && 
                                         ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId ) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                         select new HoaDonSaiSotViewModel
                                         {
                                             SoLanGuiCQT = querySoLanGuiCQT.Where(x=>x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y=>y.ThongDiepGuiCQTId).Distinct().Count(),
                                             HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                             ChungTuLienQuan = hoadon.SoCTXoaBo,
                                             TrangThaiHoaDon = 2,
                                             DienGiaiTrangThai = "&nbsp;|&nbsp;Hóa đơn gốc (SS)",
                                             PhanLoaiHDSaiSot = 1,
                                             PhanLoaiHDSaiSotMacDinh = 1,
                                             LoaiApDungHDDT = 1,
                                             TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                             MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                             MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                             KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                             SoHoaDon = hoadon.SoHoaDon ?? "",
                                             NgayLapHoaDon = hoadon.NgayHoaDon,
                                             LoaiSaiSotDeTimKiem = 0, //hủy hóa đơn do sai sót dựa trên giao diện
                                             LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin)
                                         };
                    query = queryHoaDonHuy.ToList();
                }

                if (@params.TrangThaiGuiHoaDon == (int)TrangThaiGuiHoaDon.DaGui)
                {
                    var queryHoaDonHuy = from hoadon in queryHoaDonDienTu
                                         join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                         where
                                         ((listEmail.Contains(hoadon.HoaDonDienTuId) || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)) && 
                                         (hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2
                                         || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3
                                         || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5) 
                                         && 
                                         ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                         select new HoaDonSaiSotViewModel
                                         {
                                             SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                             HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                             ChungTuLienQuan = XacDinhSoChungTuLienQuan("huy_va_thaythe", XacDinhTrangThaiHoaDon(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo, hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD), hoadon, queryHoaDonDienTu, (bkhhd.KyHieuMauSoHoaDon.ToString() +  (bkhhd.KyHieuHoaDon ?? ""))),
                                             TrangThaiHoaDon = XacDinhTrangThaiHoaDon(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo, hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD),
                                             DienGiaiTrangThai = GetDienGiaiTrangThai(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                             PhanLoaiHDSaiSot = (byte)GetGoiY(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                             PhanLoaiHDSaiSotMacDinh = (byte)GetGoiY(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                             LoaiApDungHDDT = 1,
                                             TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                             MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                             MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                             KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                             SoHoaDon = hoadon.SoHoaDon ?? "",
                                             NgayLapHoaDon = hoadon.NgayHoaDon,
                                             LoaiSaiSotDeTimKiem = XacDinhLoaiSaiSotDuaTrenGiaoDien(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo),
                                             LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin)
                                         };

                    var queryThamChieuHoaDonSaiThongTin = await (from hoadon in _db.NhatKyGuiEmails
                                                                 where hoadon.LoaiEmail == LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon
                                                                 && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.RefId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                                                 select hoadon.RefId).ToListAsync();
                    var queryHoaDonSaiThongTin = from hoadon in queryHoaDonDienTu
                                                 join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                 where
                                                 ((listEmail.Contains(hoadon.HoaDonDienTuId) || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)) && queryThamChieuHoaDonSaiThongTin.Contains(hoadon.HoaDonDienTuId) 
                                    && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                                 select new HoaDonSaiSotViewModel
                                                 {
                                                     SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                                     HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                     ChungTuLienQuan = "<Thông báo sai sót thông tin>",
                                                     TrangThaiHoaDon = XacDinhTrangThaiHoaDon(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo, hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD),
                                                     DienGiaiTrangThai = "",
                                                     PhanLoaiHDSaiSot = 4,
                                                     PhanLoaiHDSaiSotMacDinh = 4,
                                                     LoaiApDungHDDT = 1,
                                                     TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                                     LaThongTinSaiSot = true,
                                                     MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                                     MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                                     KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                                     SoHoaDon = hoadon.SoHoaDon ?? "",
                                                     NgayLapHoaDon = hoadon.NgayHoaDon,
                                                     LoaiSaiSotDeTimKiem = 0, //thông báo sai sót thông tin dựa trên giao diện
                                                     LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin)
                                                 };

                    var queryThamChieuHoaDonBiDieuChinh = from hoadon in queryHoaDonDienTu
                                                          join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                          where !string.IsNullOrWhiteSpace(hoadon.DieuChinhChoHoaDonId) 
                                                          && ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa && hoadon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                          || (bkhhd.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && hoadon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                          select hoadon.DieuChinhChoHoaDonId;

                    var queryHoaDonBiDieuChinh = from hoadon in queryHoaDonDienTu
                                                 join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                 where
                                                 ((listEmail.Contains(hoadon.HoaDonDienTuId) || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)) && queryThamChieuHoaDonBiDieuChinh.Contains(hoadon.HoaDonDienTuId) 
                                    && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                                 select new HoaDonSaiSotViewModel
                                                 {
                                                     SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                                     HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                     ChungTuLienQuan = XacDinhSoChungTuLienQuan("dieuchinh", null, hoadon, queryHoaDonDienTu, (bkhhd.KyHieuMauSoHoaDon.ToString() + (bkhhd.KyHieuHoaDon ?? ""))),
                                                     TrangThaiHoaDon = 1, //chỉ là hóa đơn gốc
                                                     DienGiaiTrangThai = "&nbsp;|&nbsp;Bị điều chỉnh",
                                                     PhanLoaiHDSaiSot = 2,
                                                     PhanLoaiHDSaiSotMacDinh = 2,
                                                     LoaiApDungHDDT = 1,
                                                     TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                                     MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                                     MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                                     KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                                     SoHoaDon = hoadon.SoHoaDon ?? "",
                                                     NgayLapHoaDon = hoadon.NgayHoaDon,
                                                     LoaiSaiSotDeTimKiem = 4, //hóa đơn bị điều chỉnh dựa trên giao diện
                                                     LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin)
                                                 };
                    query = (queryHoaDonHuy.Union(queryHoaDonSaiThongTin).Union(queryHoaDonBiDieuChinh)).ToList();
                }
            }
            else
            {
                var queryHoaDonHuy = from hoadon in queryHoaDonDienTu
                                     join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                     where
                                     ((listEmail.Contains(hoadon.HoaDonDienTuId) || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)) && 
                                     (hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc2
                                     || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc3
                                     || hoadon.HinhThucXoabo == (int)HinhThucXoabo.HinhThuc5) 
                                     && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                     select new HoaDonSaiSotViewModel
                                     {
                                         SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                         HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                         ChungTuLienQuan = XacDinhSoChungTuLienQuan("huy_va_thaythe", XacDinhTrangThaiHoaDon(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo, hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD), hoadon, queryHoaDonDienTu, (bkhhd.KyHieuMauSoHoaDon.ToString() + (bkhhd.KyHieuHoaDon ?? ""))),
                                         TrangThaiHoaDon = XacDinhTrangThaiHoaDon(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo, hoadon.NgayGuiTBaoSaiSotKhongPhaiLapHD),
                                         DienGiaiTrangThai = GetDienGiaiTrangThai(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                         PhanLoaiHDSaiSot = (byte)GetGoiY(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                         PhanLoaiHDSaiSotMacDinh = (byte)GetGoiY(hoadon.HinhThucXoabo, hoadon.ThayTheChoHoaDonId),
                                         LoaiApDungHDDT = 1,
                                         TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                         MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                         MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                         KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                         SoHoaDon = hoadon.SoHoaDon ?? "",
                                         NgayLapHoaDon = hoadon.NgayHoaDon,
                                         LoaiSaiSotDeTimKiem = XacDinhLoaiSaiSotDuaTrenGiaoDien(hoadon.ThayTheChoHoaDonId, hoadon.DieuChinhChoHoaDonId, hoadon.HinhThucXoabo),
                                         LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin)
                                     };
                queryHoaDonHuy = queryHoaDonHuy.Where(x => x.TrangThaiHoaDon != 2); //lọc ko lấy hóa đơn hủy

                var queryThamChieuHoaDonBiDieuChinh = from hoadon in queryHoaDonDienTu
                                                      join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                      where !string.IsNullOrWhiteSpace(hoadon.DieuChinhChoHoaDonId) 
                                                      && ((bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa && hoadon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                                      || (bkhhd.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa && hoadon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiKhongLoi))
                                                      select hoadon.DieuChinhChoHoaDonId;

                var queryHoaDonBiDieuChinh = from hoadon in queryHoaDonDienTu
                                             join bkhhd in queryBoKyHieuHoaDon on hoadon.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                             where
                                             ((listEmail.Contains(hoadon.HoaDonDienTuId) || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (bkhhd.HinhThucHoaDon == (HinhThucHoaDon)@params.HinhThucHoaDon || @params.IsTBaoHuyGiaiTrinhKhacCuaNNT == true)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) >= fromDate || fromDate == null)
                                    && (DateTime.Parse(hoadon.NgayHoaDon.Value.ToString("yyyy-MM-dd")) <= toDate || toDate == null)) && queryThamChieuHoaDonBiDieuChinh.Contains(hoadon.HoaDonDienTuId) 
                                    && ((!string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && hoadon.HoaDonDienTuId == @params.LapTuHoaDonDienTuId) || string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId)) 
                                             select new HoaDonSaiSotViewModel
                                             {
                                                 SoLanGuiCQT = querySoLanGuiCQT.Where(x => x.HoaDonDienTuId == hoadon.HoaDonDienTuId).Select(y => y.ThongDiepGuiCQTId).Distinct().Count(),
                                                 HoaDonDienTuId = hoadon.HoaDonDienTuId,
                                                 ChungTuLienQuan = XacDinhSoChungTuLienQuan("dieuchinh", null, hoadon, queryHoaDonDienTu, (bkhhd.KyHieuMauSoHoaDon.ToString() + (bkhhd.KyHieuHoaDon ?? ""))),
                                                 TrangThaiHoaDon = 1, //chỉ là hóa đơn gốc
                                                 DienGiaiTrangThai = "&nbsp;|&nbsp;Bị điều chỉnh",
                                                 PhanLoaiHDSaiSot = 2,
                                                 PhanLoaiHDSaiSotMacDinh = 2,
                                                 LoaiApDungHDDT = 1,
                                                 TenLoaiApDungHDDT = ((HinhThucHoaDonCanThayThe)1).GetDescription(),
                                                 MaCQTCap = (bkhhd.HinhThucHoaDon == HinhThucHoaDon.CoMa) ? (hoadon.MaCuaCQT ?? "<Chưa cấp mã>") : "",
                                                 MauHoaDon = bkhhd.KyHieuMauSoHoaDon.ToString(),
                                                 KyHieuHoaDon = bkhhd.KyHieuHoaDon ?? "",
                                                 SoHoaDon = hoadon.SoHoaDon ?? "",
                                                 NgayLapHoaDon = hoadon.NgayHoaDon,
                                                 LoaiSaiSotDeTimKiem = 4, //hóa đơn bị điều chỉnh dựa trên giao diện
                                                 LyDo = GetGoiYLyDoSaiSot(hoadon, queryHoaDonDienTu, queryThongBaoSaiThongTin)
                                             };
                query = (queryHoaDonHuy.Union(queryHoaDonBiDieuChinh)).ToList();
            }

            //lọc loại sai sót
            //nếu IsTBaoHuyGiaiTrinhKhacCuaNNT = true thì ko cần lọc loại sai sót
            if (@params.IsTBaoHuyGiaiTrinhKhacCuaNNT != true)
            {
                if (string.IsNullOrWhiteSpace(@params.LapTuHoaDonDienTuId) && @params.LoaiSaiSot != -1)
                {
                    query = query.Where(x => x.LoaiSaiSotDeTimKiem == @params.LoaiSaiSot).ToList();
                }
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
                    query = query.Where(x => x.SoHoaDon != null && x.SoHoaDon.ToUpper().ToTrim().Contains(keyword)).ToList();
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
                        (x.SoHoaDon != null && x.SoHoaDon.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.NgayLapHoaDon != null && x.NgayLapHoaDon.Value.ToString("dd/MM/yyyy").ToTrim().Contains(@params.TimKiemBatKy))
                    ).ToList();
                }
            }

            //order by kết quả
            query = query.OrderBy(x => x.MaCQTCap).ThenByDescending(x => x.MauHoaDon).ThenByDescending(x => x.KyHieuHoaDon).ThenByDescending(x => x.SoHoaDon).ToList();

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
                //model.MaThongDiep = "V0200784873" + string.Join("", Guid.NewGuid().ToString().Split("-")).ToUpper();
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
        /// DeleteAsync xóa bản ghi thông báo hóa đơn sai sót
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
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

                if (listIdHoaDonCanDanhDau.Count > 0)
                {
                    //đánh dấu các hóa đơn ko lập thông báo 04
                    var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            item.IsDaLapThongBao04 = false;
                        }
                        _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                        await _db.SaveChangesAsync();
                    }
                }    
                
                //xóa bản ghi ở bảng ThongDiepGuiCQTs
                var thongDiepGuiCQT = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == id);
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
                    MNGui = "0200784873", // "V0200784873", // "V0202029650",
                    MNNhan = "0105987432", //"TCT",
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
                    Loai = model.LoaiThongBao,
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
                    entityToUpdate.NgayGui = DateTime.Now; //NgayGui dùng làm ngày ký và ngày gửi CQT, vì ký và gửi là cùng nhau
                    entityToUpdate.FileXMLDaKy = tenFile + ".xml";
                    entityToUpdate.SoThongBaoSaiSot = string.Format("{0} {1}", "TBSS", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    _db.ThongDiepGuiCQTs.Update(entityToUpdate);
                    await _db.SaveChangesAsync();
                }

                //update bảng thông điệp chung
                var entityBangThongDiepChungToUpdate = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == @params.ThongDiepGuiCQTId && x.MaLoaiThongDiep == MaLoaiThongDiep && x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChuaGui);
                if (entityBangThongDiepChungToUpdate != null)
                {
                    entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
                    _db.ThongDiepChungs.Update(entityBangThongDiepChungToUpdate);
                    await _db.SaveChangesAsync();
                }

                //đánh dấu trạng thái gửi hóa đơn đã lập thông báo 04
                var listIdHoaDonCanDanhDau = await _db.ThongDiepChiTietGuiCQTs.Where(x=>x.ThongDiepGuiCQTId == @params.ThongDiepGuiCQTId).Select(x => x.HoaDonDienTuId).ToListAsync();
                if (listIdHoaDonCanDanhDau.Count > 0)
                {
                    var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            item.LanGui04 = (item.LanGui04 ?? 0) + 1;
                            item.TrangThaiGui04 = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
                        }
                        _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                        await _db.SaveChangesAsync();
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

                bool ketQua = false;

                // Gửi dữ liệu tới TVan
                var xmlContent = File.ReadAllText(signedXmlFileFolder);
                var responce999 = await _ITVanService.TVANSendData("api/error-invoice/send", xmlContent);
                var thongDiep999 = ConvertXMLDataToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhan999.TDiep>(responce999);
                ketQua = (thongDiep999.DLieu.TBao.TTTNhan == 0);

                //lưu trạng thái đã ký gửi thành công tới cơ quan thuế hay chưa
                var entityToUpdate = await _db.ThongDiepGuiCQTs.FirstOrDefaultAsync(x => x.Id == @params.ThongDiepGuiCQTId);
                if (entityToUpdate != null)
                {
                    entityToUpdate.NgayGui = DateTime.Now;
                    entityToUpdate.DaKyGuiCQT = (thongDiep999 != null)? true: false;
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

                    if (ketQua)
                    {
                        entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiKhongLoi;
                    }
                    else
                    {
                        entityBangThongDiepChungToUpdate.TrangThaiGui = (int)TrangThaiGuiThongDiep.GuiLoi;
                    }

                    entityBangThongDiepChungToUpdate.NgayGui = DateTime.Now;
                    entityBangThongDiepChungToUpdate.FileXML = @params.XMLFileName;
                    entityBangThongDiepChungToUpdate.NgayThongBao = DateTime.Now;
                    entityBangThongDiepChungToUpdate.MaThongDiepPhanHoi = thongDiep999.TTChung.MTDiep;

                    _db.ThongDiepChungs.Update(entityBangThongDiepChungToUpdate);
                    await _db.SaveChangesAsync();

                    // Cập nhật lại dữ liệu xml đã ký vào bảng filedatas
                    await ThemDuLieuVaoBangFileData(entityBangThongDiepChungToUpdate.ThongDiepChungId, xmlContent, @params.XMLFileName, 1, ketQua);
                }

                //lưu thông điệp nhận 999 từ TVAN
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
                    TrangThaiGui = (ketQua)? (int)TrangThaiGuiThongDiep.GuiKhongLoi: (int)TrangThaiGuiThongDiep.GuiLoi,
                    HinhThuc = 0,
                    NgayThongBao = DateTime.Now,
                    FileXML = $"TD-{Guid.NewGuid()}.xml"
                };
                await _db.ThongDiepChungs.AddAsync(tdc999);
                await _db.SaveChangesAsync();

                //đánh dấu trạng thái gửi hóa đơn đã lập thông báo 04
                var listIdHoaDonCanDanhDau = await _db.ThongDiepChiTietGuiCQTs.Where(x => x.ThongDiepGuiCQTId == @params.ThongDiepGuiCQTId).Select(x => x.HoaDonDienTuId).ToListAsync();
                if (listIdHoaDonCanDanhDau.Count > 0)
                {
                    var listHoaDonCanDanhDau = await _db.HoaDonDienTus.Where(x => listIdHoaDonCanDanhDau.Contains(x.HoaDonDienTuId)).ToListAsync();
                    if (listHoaDonCanDanhDau.Count > 0)
                    {
                        foreach (var item in listHoaDonCanDanhDau)
                        {
                            item.TrangThaiGui04 = (ketQua) ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi;
                        }
                        _db.HoaDonDienTus.UpdateRange(listHoaDonCanDanhDau);
                        await _db.SaveChangesAsync();
                    }
                }

                //thêm nội dung file xml 999 vào bảng file data
                await ThemDuLieuVaoBangFileData(tdc999.ThongDiepChungId, responce999, tdc999.FileXML, 1, true, 1);

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
                Document doc = new Document();
                string docFolder = Path.Combine(_hostingEnvironment.WebRootPath, "docs/QuanLy/thong-bao-hoa-don-dien-tu-co-sai-sot.docx");
                doc.LoadFromFile(docFolder);

                doc.Replace("<CoQuanThue>", model.TenCoQuanThue, true, true);
                doc.Replace("<TenNguoiNopThue>", model.NguoiNopThue, true, true);
                doc.Replace("<MaSoThue>", model.MaSoThue, true, true);
                doc.Replace("<DiaDanh>", model.DiaDanh ?? "", true, true);
                var ngayThangNam = model.NgayLap;
                doc.Replace("<NgayThangNam>", string.Format("ngày {0} tháng {1} năm {2}", ngayThangNam.Day, ngayThangNam.Month, ngayThangNam.Year), true, true);
                doc.Replace("<DaiDienNguoiNopThue>", model.DaiDienNguoiNopThue, true, true);

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

                    await ThemAttachVaoBangFileData(model.Id, duongDanFileWord);
                    await ThemAttachVaoBangFileData(model.Id, duongDanFilePdf);

                    await ThemAttachVaoBangTaiLieuDinhKem(model.Id, duongDanFileWord);
                    await ThemAttachVaoBangTaiLieuDinhKem(model.Id, duongDanFilePdf);

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
                            NgayThoiHan = hoaDon.NgayThongBao.AddDays(hoaDon.ThoiHan),
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

            query = query.OrderBy(x=>x.NgayThongBao).ThenBy(y=>y.SoThongBaoCuaCQT);

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
                HinhThuc = tDiep.DLieu.TBao.DLTBao.HThuc,
                ChucDanh = tDiep.DLieu.TBao.DLTBao.CDanh,
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
                            NgayLapHoaDon = DateTime.Parse(item.NLap),
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
                    paragraph.Text = DateTime.Parse(item.NLap).ToString("dd/MM/yyyy");

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
                                                                    where tdc.MaLoaiThongDiep == 100 && tdc.HinhThuc == (int)HThuc.DangKyMoi && tdc.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
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

        private async Task ThemAttachVaoBangFileData(string refId, string path)
        {
            FileData fileData = new FileData
            {
                FileDataId = Guid.NewGuid().ToString(),
                RefId = refId,
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
        private string GetUserId()
        {
            string nameIdentifier = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return nameIdentifier;
        }

        //Method này để convert chuỗi sang số
        private int ConvertToNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;

            var giaTri = 0;
            var isNumeric = int.TryParse(value, out giaTri);
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
        private string GetDienGiaiTrangThai(int? hinhThucXoaBo, string thayTheChoHoaDonId)
        {
            if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc2)
            {
                return "&nbsp;|&nbsp;Bị thay thế";
            }
            else if (hinhThucXoaBo == (int)HinhThucXoabo.HinhThuc3)
            {
                if (string.IsNullOrWhiteSpace(thayTheChoHoaDonId))
                {
                    return "&nbsp;|&nbsp;Hóa đơn gốc (HĐH)";
                }
                else
                {
                    return "&nbsp;|&nbsp;Hóa đơn thay thế (HĐH)";
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
        private int XacDinhTrangThaiHoaDon(string thayTheChoHoaDonId, string dieuChinhChoHoaDonId, int? hinhThucXoaBo, DateTime? ngayGuiTBaoSaiSotKhongPhaiLapHD)
        {
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

        //Method này để xác định số chứng từ liên quan
        private string XacDinhSoChungTuLienQuan(string phanLoai, int? trangThaiHoaDon, HoaDonDienTu hoaDon, List<HoaDonDienTu> listHoaDonDienTu, string kyHieuMauHoaDon)
        {
            if (phanLoai == "huy_va_thaythe")
            {
                if (trangThaiHoaDon == 2) //nếu là các hóa đơn hủy thì trả về số chứng từ xóa bỏ
                {
                    return hoaDon.SoCTXoaBo;
                }
                else //nếu là hóa đơn bị thay thế thì trả về số hóa đơn thay thế
                {
                    var hoaDonThayThe = listHoaDonDienTu.FirstOrDefault(x => x.ThayTheChoHoaDonId == hoaDon.HoaDonDienTuId);
                    if (hoaDonThayThe != null)
                    {
                        return string.Format("{0}-{1}-{2}", kyHieuMauHoaDon, hoaDonThayThe.SoHoaDon, hoaDonThayThe.NgayHoaDon?.ToString("dd/MM/yyyy"));
                    }
                }
            }
            if (phanLoai == "dieuchinh")
            {
                //nếu là hóa đơn bị điều chỉnh thì trả về số hóa đơn điều chỉnh
                var hoaDonDieuChinh = listHoaDonDienTu.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId);
                if (hoaDonDieuChinh != null)
                {
                    return string.Format("{0}-{1}-{2}", kyHieuMauHoaDon, hoaDonDieuChinh.SoHoaDon, hoaDonDieuChinh.NgayHoaDon?.ToString("dd/MM/yyyy"));
                }
            }

            return "";
        }

        //Method này gợi ý lý do sai sót
        private string GetGoiYLyDoSaiSot(HoaDonDienTu hoaDon, List<HoaDonDienTu> listHoaDonDienTu, List<ThongBaoSaiThongTin> listThongBaoSaiThongTin)
        {
            if (hoaDon.HinhThucXoabo != null)
            {
                return hoaDon.LyDoXoaBo;
            }
            else
            {
                //nếu đã bị điều chỉnh thì ko cần kiểm tra có gửi thông báo sai thông tin hay ko
                var hoaDonDieuChinh = listHoaDonDienTu.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hoaDon.HoaDonDienTuId);
                if (hoaDonDieuChinh != null)
                {
                    var lyDoDieuChinhModel = string.IsNullOrWhiteSpace(hoaDonDieuChinh.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hoaDonDieuChinh.LyDoDieuChinh);
                    if (lyDoDieuChinhModel != null)
                    {
                        return lyDoDieuChinhModel.LyDo;
                    }
                }
                else
                {
                    //kiểm tra đã gửi thông báo sai thông tin hay chưa
                    var thongBaoSaiThongTin = listThongBaoSaiThongTin.Where(x => x.HoaDonDienTuId == hoaDon.HoaDonDienTuId).OrderByDescending(x => x.CreatedDate).Take(1).FirstOrDefault();
                    if (thongBaoSaiThongTin != null)
                    {
                        var thongBaoSaiSot = "";
                        if (!string.IsNullOrWhiteSpace(thongBaoSaiThongTin.HoTenNguoiMuaHang_Dung))
                        {
                            thongBaoSaiSot = "Họ và tên người mua hàng đúng là: " + thongBaoSaiThongTin.HoTenNguoiMuaHang_Dung;
                        }

                        if (!string.IsNullOrWhiteSpace(thongBaoSaiThongTin.TenDonVi_Dung))
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

                        if (!string.IsNullOrWhiteSpace(thongBaoSaiThongTin.DiaChi_Dung))
                        {
                            if (string.IsNullOrWhiteSpace(thongBaoSaiSot))
                            {
                                thongBaoSaiSot = "Địa chỉ đúng là: " + thongBaoSaiThongTin.TenDonVi_Dung;
                            }
                            else
                            {
                                thongBaoSaiSot += "; Địa chỉ đúng là: " + thongBaoSaiThongTin.TenDonVi_Dung;
                            }
                        }

                        return thongBaoSaiSot;
                    }
                }
            }

            return "";
        }
    }
}
