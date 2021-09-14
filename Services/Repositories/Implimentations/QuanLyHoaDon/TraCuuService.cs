using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DLL;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Linq;
using Services.ViewModels.DanhMuc;
using DLL.Enums;
using Services.Enums;
using Services.Helper;
using Services.ViewModels.Config;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ManagementServices.Helper;
using System.IO;
using DLL.Constants;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Hosting;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class TraCuuService : ITraCuuService
    {
        Datacontext _db;
        IMapper _mp;
        IHttpContextAccessor _IHttpContextAccessor;
        IHostingEnvironment _hostingEnvironment;
        IHoaDonDienTuService _hoaDonDienTuService;

        public TraCuuService(Datacontext db,
            IMapper mp,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IHoaDonDienTuService hoaDonDienTuService
        )
        {
            _db = db;
            _mp = mp;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> GetMaTraCuuInXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            string data = string.Empty;
            string checkXmlFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "Assets/uploaded/xml/temp");
            if (!Directory.Exists(checkXmlFolder))
            {
                Directory.CreateDirectory(checkXmlFolder);
            }

            string checkXmlPath = Path.Combine(checkXmlFolder, "temp.xml");
            using (FileStream fileStream = new FileStream(checkXmlPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(checkXmlPath);
            XmlNode node = xmlDoc.SelectSingleNode("HDon/DLHDon/TTChung/MTCuu");
            bool result = false;
            if (node != null)
            {
                result = CheckCorrectLookupFile2(checkXmlPath);
            }
            else
            {
                result = CheckCorrectLookupFile(checkXmlPath);
            }

            if (result)
            {
                if (node != null)
                {
                    string lookupCode = node.InnerText.ToString();
                    if (!string.IsNullOrEmpty(lookupCode))
                    {
                        var hddt = await TraCuuByMa(lookupCode);

                        if (hddt == null)
                        {
                            data = string.Empty;
                        }
                        else
                        {
                            result = await _hoaDonDienTuService.CheckMaTraCuuAsync(hddt.MaTraCuu);

                            if (result)
                                data = hddt.MaTraCuu;
                        }
                    }
                    else
                    {
                        data = string.Empty;
                    }
                }
                else
                {
                    data = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(checkXmlPath))
            {
                if (System.IO.File.Exists(checkXmlPath))
                {
                    System.IO.File.Delete(checkXmlPath);
                }
            }

            return data;
        }

        public async Task<HoaDonDienTuViewModel> TraCuuByMa(string strMaTraCuu)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);

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
                            //join tbs1 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung1 equals tbs1.Id into tmpTBS1
                            //from tbs1 in tmpTBS1.DefaultIfEmpty()
                            //join tbs2 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung2 equals tbs2.Id into tmpTBS2
                            //from tbs2 in tmpTBS2.DefaultIfEmpty()
                            //join tbs3 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung3 equals tbs3.Id into tmpTBS3
                            //from tbs3 in tmpTBS3.DefaultIfEmpty()
                            //join tbs4 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung4 equals tbs4.Id into tmpTBS4
                            //from tbs4 in tmpTBS4.DefaultIfEmpty()
                            //join tbs5 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung5 equals tbs5.Id into tmpTBS5
                            //from tbs5 in tmpTBS5.DefaultIfEmpty()
                            //join tbs6 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung6 equals tbs6.Id into tmpTBS6
                            //from tbs6 in tmpTBS6.DefaultIfEmpty()
                            //join tbs7 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung7 equals tbs7.Id into tmpTBS7
                            //from tbs7 in tmpTBS7.DefaultIfEmpty()
                            //join tbs8 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung8 equals tbs8.Id into tmpTBS8
                            //from tbs8 in tmpTBS8.DefaultIfEmpty()
                            //join tbs9 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung9 equals tbs9.Id into tmpTBS9
                            //from tbs9 in tmpTBS9.DefaultIfEmpty()
                            //join tbs10 in _db.TruongDuLieuMoRongs on hd.TruongThongTinBoSung10 equals tbs10.Id into tmpTBS10
                            //from tbs10 in tmpTBS9.DefaultIfEmpty()
                        where hd.MaTraCuu == strMaTraCuu && hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh
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
                            //TruongThongTinBoSung1Id = hd.TruongThongTinBoSung1Id,
                            //TruongThongTinBoSung1 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs1.Id,
                            //    DataId = tbs1.DataId,
                            //    TenTruong = tbs1.TenTruong,
                            //    TenTruongHienThi = tbs1.TenTruongHienThi,
                            //    DuLieu = tbs1.DuLieu,
                            //    HienThi = tbs1.HienThi
                            //},
                            //TruongThongTinBoSung2Id = hd.TruongThongTinBoSung2Id,
                            //TruongThongTinBoSung2 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs2.Id,
                            //    DataId = tbs2.DataId,
                            //    TenTruong = tbs2.TenTruong,
                            //    TenTruongHienThi = tbs2.TenTruongHienThi,
                            //    DuLieu = tbs2.DuLieu,
                            //    HienThi = tbs2.HienThi
                            //},
                            //TruongThongTinBoSung3Id = hd.TruongThongTinBoSung3Id,
                            //TruongThongTinBoSung3 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs3.Id,
                            //    DataId = tbs3.DataId,
                            //    TenTruong = tbs3.TenTruong,
                            //    TenTruongHienThi = tbs3.TenTruongHienThi,
                            //    DuLieu = tbs3.DuLieu,
                            //    HienThi = tbs3.HienThi
                            //},
                            //TruongThongTinBoSung4Id = hd.TruongThongTinBoSung4Id,
                            //TruongThongTinBoSung4 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs4.Id,
                            //    DataId = tbs4.DataId,
                            //    TenTruong = tbs4.TenTruong,
                            //    TenTruongHienThi = tbs4.TenTruongHienThi,
                            //    DuLieu = tbs4.DuLieu,
                            //    HienThi = tbs4.HienThi
                            //},
                            //TruongThongTinBoSung5Id = hd.TruongThongTinBoSung5Id,
                            //TruongThongTinBoSung5 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs5.Id,
                            //    DataId = tbs5.DataId,
                            //    TenTruong = tbs5.TenTruong,
                            //    TenTruongHienThi = tbs5.TenTruongHienThi,
                            //    DuLieu = tbs5.DuLieu,
                            //    HienThi = tbs5.HienThi,
                            //},
                            //TruongThongTinBoSung6Id = hd.TruongThongTinBoSung6Id,
                            //TruongThongTinBoSung6 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs6.Id,
                            //    DataId = tbs6.DataId,
                            //    TenTruong = tbs6.TenTruong,
                            //    TenTruongHienThi = tbs6.TenTruongHienThi,
                            //    DuLieu = tbs6.DuLieu,
                            //    HienThi = tbs6.HienThi
                            //},
                            //TruongThongTinBoSung7Id = hd.TruongThongTinBoSung7Id,
                            //TruongThongTinBoSung7 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs7.Id,
                            //    DataId = tbs7.DataId,
                            //    TenTruong = tbs7.TenTruong,
                            //    TenTruongHienThi = tbs7.TenTruongHienThi,
                            //    DuLieu = tbs7.DuLieu,
                            //    HienThi = tbs7.HienThi
                            //},
                            //TruongThongTinBoSung8Id = hd.TruongThongTinBoSung8Id,
                            //TruongThongTinBoSung8 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs8.Id,
                            //    DataId = tbs8.DataId,
                            //    TenTruong = tbs8.TenTruong,
                            //    TenTruongHienThi = tbs8.TenTruongHienThi,
                            //    DuLieu = tbs8.DuLieu,
                            //    HienThi = tbs8.HienThi
                            //},
                            //TruongThongTinBoSung9Id = hd.TruongThongTinBoSung9Id,
                            //TruongThongTinBoSung9 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs9.Id,
                            //    DataId = tbs9.DataId,
                            //    TenTruong = tbs9.TenTruong,
                            //    TenTruongHienThi = tbs9.TenTruongHienThi,
                            //    DuLieu = tbs9.DuLieu,
                            //    HienThi = tbs9.HienThi
                            //},
                            //TruongThongTinBoSung10Id = hd.TruongThongTinBoSung10Id,
                            //TruongThongTinBoSung10 = new TruongDuLieuMoRongViewModel
                            //{
                            //    Id = tbs10.Id,
                            //    DataId = tbs10.DataId,
                            //    TenTruong = tbs10.TenTruong,
                            //    TenTruongHienThi = tbs10.TenTruongHienThi,
                            //    DuLieu = tbs10.DuLieu,
                            //    HienThi = tbs10.HienThi
                            //},
                            BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : null,
                            LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
                            HoaDonChiTiets = (
                                               from hdct in _db.HoaDonDienTuChiTiets
                                               join hd1 in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                               from hd1 in tmpHoaDons.DefaultIfEmpty()
                                               join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                               from vt in tmpHangHoas.DefaultIfEmpty()
                                               join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                               from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                                   //join tmr1 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet1 equals tmr1.Id into tmpTMR1
                                                   //from tmr1 in tmpTMR1.DefaultIfEmpty()
                                                   //join tmr2 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet2 equals tmr2.Id into tmpTMR2
                                                   //from tmr2 in tmpTMR2.DefaultIfEmpty()
                                                   //join tmr3 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet3 equals tmr3.Id into tmpTMR3
                                                   //from tmr3 in tmpTMR3.DefaultIfEmpty()
                                                   //join tmr4 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet4 equals tmr4.Id into tmpTMR4
                                                   //from tmr4 in tmpTMR4.DefaultIfEmpty()
                                                   //join tmr5 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet5 equals tmr5.Id into tmpTMR5
                                                   //from tmr5 in tmpTMR5.DefaultIfEmpty()
                                                   //join tmr6 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet6 equals tmr6.Id into tmpTMR6
                                                   //from tmr6 in tmpTMR6.DefaultIfEmpty()
                                                   //join tmr7 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet7 equals tmr7.Id into tmpTMR7
                                                   //from tmr7 in tmpTMR7.DefaultIfEmpty()
                                                   //join tmr8 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet8 equals tmr8.Id into tmpTMR8
                                                   //from tmr8 in tmpTMR8.DefaultIfEmpty()
                                                   //join tmr9 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet9 equals tmr9.Id into tmpTMR9
                                                   //from tmr9 in tmpTMR9.DefaultIfEmpty()
                                                   //join tmr10 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet10 equals tmr10.Id into tmpTMR10
                                                   //from tmr10 in tmpTMR9.DefaultIfEmpty()
                                               where hdct.HoaDonDienTuId == hd.HoaDonDienTuId
                                               orderby vt.Ma descending
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd1.HoaDonDienTuId,
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
                                                   //TruongMoRongChiTiet1 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr1.Id,
                                                   //    DataId = tmr1.DataId,
                                                   //    TenTruong = tmr1.TenTruong,
                                                   //    TenTruongHienThi = tmr1.TenTruongHienThi,
                                                   //    DuLieu = tmr1.DuLieu,
                                                   //    HienThi = tmr1.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet2 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr2.Id,
                                                   //    DataId = tmr2.DataId,
                                                   //    TenTruong = tmr2.TenTruong,
                                                   //    TenTruongHienThi = tmr2.TenTruongHienThi,
                                                   //    DuLieu = tmr2.DuLieu,
                                                   //    HienThi = tmr2.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet3 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr3.Id,
                                                   //    DataId = tmr3.DataId,
                                                   //    TenTruong = tmr3.TenTruong,
                                                   //    TenTruongHienThi = tmr3.TenTruongHienThi,
                                                   //    DuLieu = tmr3.DuLieu,
                                                   //    HienThi = tmr3.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet4 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr4.Id,
                                                   //    DataId = tmr4.DataId,
                                                   //    TenTruong = tmr4.TenTruong,
                                                   //    TenTruongHienThi = tmr4.TenTruongHienThi,
                                                   //    DuLieu = tmr4.DuLieu,
                                                   //    HienThi = tmr4.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet5 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr5.Id,
                                                   //    DataId = tmr5.DataId,
                                                   //    TenTruong = tmr5.TenTruong,
                                                   //    TenTruongHienThi = tmr5.TenTruongHienThi,
                                                   //    DuLieu = tmr5.DuLieu,
                                                   //    HienThi = tmr5.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet6 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr6.Id,
                                                   //    DataId = tmr6.DataId,
                                                   //    TenTruong = tmr6.TenTruong,
                                                   //    TenTruongHienThi = tmr6.TenTruongHienThi,
                                                   //    DuLieu = tmr6.DuLieu,
                                                   //    HienThi = tmr6.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet7 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr7.Id,
                                                   //    DataId = tmr7.DataId,
                                                   //    TenTruong = tmr7.TenTruong,
                                                   //    TenTruongHienThi = tmr7.TenTruongHienThi,
                                                   //    DuLieu = tmr7.DuLieu,
                                                   //    HienThi = tmr7.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet8 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr8.Id,
                                                   //    DataId = tmr8.DataId,
                                                   //    TenTruong = tmr8.TenTruong,
                                                   //    TenTruongHienThi = tmr8.TenTruongHienThi,
                                                   //    DuLieu = tmr8.DuLieu,
                                                   //    HienThi = tmr8.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet9 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr9.Id,
                                                   //    DataId = tmr9.DataId,
                                                   //    TenTruong = tmr9.TenTruong,
                                                   //    TenTruongHienThi = tmr9.TenTruongHienThi,
                                                   //    DuLieu = tmr9.DuLieu,
                                                   //    HienThi = tmr9.HienThi
                                                   //},
                                                   //TruongMoRongChiTiet10 = new TruongDuLieuMoRongViewModel
                                                   //{
                                                   //    Id = tmr10.Id,
                                                   //    DataId = tmr10.DataId,
                                                   //    TenTruong = tmr10.TenTruong,
                                                   //    TenTruongHienThi = tmr10.TenTruongHienThi,
                                                   //    DuLieu = tmr10.DuLieu,
                                                   //    HienThi = tmr10.HienThi
                                                   //},
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
                                                   Link = _IHttpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{hd.HoaDonDienTuId}\FileAttach", tldk.TenGuid),
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
            if (result != null)
            {
                result.TongTienThanhToan = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToan ?? 0);
                result.TongTienThanhToanQuyDoi = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToanQuyDoi ?? 0);
                return result;
            }
            else return null;
        }


        private bool CheckCorrectLookupFile(string filePath)
        {
            try
            {
                // Check the arguments.  
                if (filePath == null)
                    throw new ArgumentNullException("Name");

                // Create a new XML document.
                XmlDocument xmlDocument = new XmlDocument();

                // Format using white spaces.
                //xmlDocument.PreserveWhitespace = true;

                // Load the passed XML file into the document. 
                xmlDocument.Load(filePath);

                // Create a new SignedXml object and pass it
                // the XML document class.
                SignedXml signedXml = new SignedXml(xmlDocument);

                // Find the "Signature" node and create a new
                // XmlNodeList object.
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

                // Load the signature node.
                signedXml.LoadXml((XmlElement)nodeList[0]);

                // Check the signature and return the result.
                return signedXml.CheckSignature();
            }
            catch (Exception exc)
            {
                Console.Write("Error:" + exc);
                return false;
            }
        }

        private bool CheckCorrectLookupFile2(string filePath)
        {
            try
            {
                // Check the arguments.  
                if (filePath == null)
                    throw new ArgumentNullException("Name");

                // Create a new XML document.
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                // Format using white spaces.
                //xmlDocument.PreserveWhitespace = true;

                // Load the passed XML file into the document. 
                xmlDocument.Load(filePath);

                XmlNode signatureNode = findSignatureElement(xmlDocument);
                if (signatureNode == null) return true;

                SignedXml signedXml = new SignedXml(xmlDocument);
                signedXml.LoadXml((XmlElement)signatureNode);

                //var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                //var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                //if (certificate == null) throw new InvalidOperationException("Signature does not contain a X509 certificate public key to verify the signature");
                //return signedXml.CheckSignature(certificate, true);

                return signedXml.CheckSignature();
            }
            catch (Exception exc)
            {
                Console.Write("Error:" + exc);
                return false;
            }
        }

        private XmlNode findSignatureElement(XmlDocument doc)
        {
            var signatureElements = doc.DocumentElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
            if (signatureElements.Count == 1)
                return signatureElements[0];
            else if (signatureElements.Count == 0)
                return null;
            else
                throw new InvalidOperationException("Document has multiple xmldsig Signature elements");
        }
    }
}
