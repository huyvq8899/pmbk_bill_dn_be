using System;
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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ManagementServices.Helper;
using System.IO;
using DLL.Constants;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Hosting;
using Services.Helper.Constants;
using System.Security.Cryptography.X509Certificates;
using Services.Helper.Params;
using System.Security.Cryptography;
using DLL.Entity.QuanLyHoaDon;
using System.Collections.Generic;
using Services.ViewModels.QuanLy;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Text;
using Services.ViewModels.XML;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class TraCuuService : ITraCuuService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public TraCuuService(Datacontext db,
            IMapper mp,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment hostingEnvironment
        )
        {
            _db = db;
            _mp = mp;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<KetQuaTraCuuXML> GetMaTraCuuInXml(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return null;
                }

                var data = new KetQuaTraCuuXML
                {
                    SoHoaDon = 0,
                    KyHieuHoaDon = string.Empty,
                    KyHieuMauSoHoaDon = 0
                };
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
                XmlNode node1 = xmlDoc.SelectSingleNode("TDiep/DLieu/HDon/DLHDon/TTChung/KHMSHDon");
                if(node1 == null) node1 = xmlDoc.SelectSingleNode("HDon/DLHDon/TTChung/KHMSHDon");
                
                XmlNode node2 = xmlDoc.SelectSingleNode("TDiep/DLieu/HDon/DLHDon/TTChung/KHHDon");
                if (node2 == null) node2 = xmlDoc.SelectSingleNode("HDon/DLHDon/TTChung/KHHDon");

                XmlNode node3 = xmlDoc.SelectSingleNode("TDiep/DLieu/HDon/DLHDon/TTChung/SHDon");
                if (node3 == null) node3 = xmlDoc.SelectSingleNode("HDon/DLHDon/TTChung/SHDon");

                bool result;
                if (node1 != null && node2 != null && node3 != null)
                {
                    result = CheckCorrectLookupFile3(checkXmlPath);


                    if (!string.IsNullOrEmpty(checkXmlPath))
                    {
                        if (File.Exists(checkXmlPath))
                        {
                            File.Delete(checkXmlPath);
                        }
                    }

                    if (result)
                    {
                        string kHHDon = node2.InnerText.ToString();
                        int soHoaDon = !string.IsNullOrEmpty(node3.InnerText.ToString()) ? int.Parse(node3.InnerText.ToString()) : 0;
                        int khmshDon = !string.IsNullOrEmpty(node1.InnerText.ToString()) ? int.Parse(node1.InnerText.ToString()) : 0;
                        if (!string.IsNullOrEmpty(kHHDon) && soHoaDon != 0 && khmshDon != 0)
                        {
                            return new KetQuaTraCuuXML
                            {
                                SoHoaDon = soHoaDon,
                                KyHieuHoaDon = kHHDon,
                                KyHieuMauSoHoaDon = khmshDon
                            };
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }

            return null;
        }

        public async Task<HoaDonDienTuViewModel> TraCuuBySoHoaDon(KetQuaTraCuuXML input)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                         where hoaDon.HinhThucXoabo != null || hoaDon.ThayTheChoHoaDonId != null || hoaDon.DieuChinhChoHoaDonId != null
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


            var query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
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
                        join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        join bbdc_dc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc_dc.HoaDonDieuChinhId into tmpBienBanDieuChinh_DCs
                        from bbdc_dc in tmpBienBanDieuChinh_DCs.DefaultIfEmpty()
                        where hd.SoHoaDon == input.SoHoaDon && bkhhd.KyHieuHoaDon == input.KyHieuHoaDon && (hd.TrangThaiQuyTrinh >= (int)TrangThaiQuyTrinh.DaKyDienTu && hd.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.ChuaPhatHanh) && bkhhd.KyHieuMauSoHoaDon == input.KyHieuMauSoHoaDon
                        select new HoaDonDienTuViewModel
                        {
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
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
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
                            GhiChuThayTheSaiSot = hd.GhiChuThayTheSaiSot,
                            HoaDonChiTiets = (
                                               from hdct in _db.HoaDonDienTuChiTiets
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
            return result;
        }

        public async Task<HoaDonDienTuViewModel> TraCuuByMa(string strMaTraCuu)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
                                                         where hoaDon.HinhThucXoabo != null || hoaDon.ThayTheChoHoaDonId != null || hoaDon.DieuChinhChoHoaDonId != null
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

            var query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
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
                        join bbdc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        join bbdc_dc in _db.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc_dc.HoaDonDieuChinhId into tmpBienBanDieuChinh_DCs
                        from bbdc_dc in tmpBienBanDieuChinh_DCs.DefaultIfEmpty()
                        where hd.MaTraCuu == strMaTraCuu
                        select new HoaDonDienTuViewModel
                        {
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
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
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
                            GhiChuThayTheSaiSot = hd.GhiChuThayTheSaiSot,
                            HoaDonChiTiets = (
                                               from hdct in _db.HoaDonDienTuChiTiets
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
            if (result != null)
            {
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

                //result.TongTienThanhToan = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToan ?? 0);
                //result.TongTienThanhToanQuyDoi = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToanQuyDoi ?? 0);
                return result;
            }
            else return null;
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
        //private bool CheckCorrectLookupFile(string filePath)
        //{
        //    try
        //    {
        //        // Check the arguments.  
        //        if (filePath == null)
        //            throw new ArgumentNullException("Name");

        //        // Create a new XML document.
        //        XmlDocument xmlDocument = new XmlDocument();
        //        xmlDocument.PreserveWhitespace = true;

        //        // Format using white spaces.
        //        //xmlDocument.PreserveWhitespace = true;

        //        // Load the passed XML file into the document. 
        //        xmlDocument.Load(filePath);

        //        // Create a new SignedXml object and pass it
        //        // the XML document class.
        //        SignedXml signedXml = new SignedXml(xmlDocument);

        //        // Find the "Signature" node and create a new
        //        // XmlNodeList object.
        //        XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

        //        // Load the signature node.
        //        signedXml.LoadXml((XmlElement)nodeList[0]);

        //        // Check the signature and return the result.
        //        return signedXml.CheckSignature();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //private bool CheckCorrectLookupFile2(string filePath)
        //{
        //    try
        //    {
        //        // Check the arguments.  
        //        if (filePath == null)
        //            throw new ArgumentNullException("Name");

        //        // Create a new XML document.
        //        XmlDocument xmlDocument = new XmlDocument();
        //        xmlDocument.PreserveWhitespace = true;
        //        // Format using white spaces.
        //        //xmlDocument.PreserveWhitespace = true;

        //        // Load the passed XML file into the document. 
        //        xmlDocument.Load(filePath);

        //        XmlNode signatureNode = FindSignatureElement(xmlDocument);
        //        if (signatureNode != null) return true;

        //        return false;

        //        //SignedXml signedXml = new SignedXml(xmlDocument);
        //        //signedXml.LoadXml((XmlElement)signatureNode);

        //        //var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
        //        //var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

        //        //if (certificate == null) throw new InvalidOperationException("Signature does not contain a X509 certificate public key to verify the signature");
        //        //return signedXml.CheckSignature(certificate, true);

        //        //return signedXml.CheckSignature();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        private XmlNode FindSignatureElement(XmlDocument doc)
        {
            var signatureElements = doc.DocumentElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
            if (signatureElements.Count >= 1)
                return signatureElements[0];
            else if (signatureElements.Count == 0)
                return null;
            else
                throw new InvalidOperationException("Document has multiple xmldsig Signature elements");
        }

        public byte[] FindSignatureElement(string hoaDonDienTuId, int type)
        {
            try
            {
                var dataXML = string.Empty;
                var dataXMLRes = string.Empty;
                var hoaDon = _db.HoaDonDienTus.FirstOrDefault(x => x.HoaDonDienTuId == hoaDonDienTuId);
                var phuongThucChuyenDL = _db.BoKyHieuHoaDons.FirstOrDefault(x => x.BoKyHieuHoaDonId == hoaDon.BoKyHieuHoaDonId).PhuongThucChuyenDL;
                var hinhThucHoaDon = _db.BoKyHieuHoaDons.FirstOrDefault(x => x.BoKyHieuHoaDonId == hoaDon.BoKyHieuHoaDonId).HinhThucHoaDon;
                if(hinhThucHoaDon != HinhThucHoaDon.CoMaTuMayTinhTien)
                {
                    if (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu)
                    {
                        dataXML = _db.FileDatas.Where(x => x.RefId == hoaDonDienTuId && x.IsSigned == true && x.Type == 1).Select(x => x.Content).FirstOrDefault();
                    }
                    else
                    {
                        if (phuongThucChuyenDL == PhuongThucChuyenDL.CDDu) {
                            var thongDiep200 = (from dlghddt in _db.DuLieuGuiHDDTs
                                                join td in _db.ThongDiepChungs on dlghddt.DuLieuGuiHDDTId equals td.IdThamChieu
                                                where td.MaLoaiThongDiep == (int)MLTDiep.TDGHDDTTCQTCapMa || td.MaLoaiThongDiep == (int)MLTDiep.TDCDLHDKMDCQThue
                                                select td).FirstOrDefault();

                            if(hoaDon.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.CQTDaCapMa && hoaDon.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.HoaDonHopLe && hoaDon.TrangThaiQuyTrinh != (int)TrangThaiQuyTrinh.HoaDonKhongHopLe)
                            {
                                dataXML = _db.FileDatas.Where(x => x.RefId == thongDiep200.ThongDiepChungId && x.Type == 1).FirstOrDefault().Content;
                                if (string.IsNullOrEmpty(dataXML))
                                {
                                    var bin = _db.FileDatas.Where(x => x.RefId == thongDiep200.ThongDiepChungId && x.Type == 1).FirstOrDefault().Binary;
                                    dataXML = Encoding.UTF8.GetString(bin);
                                }
                            }
                            else
                            {
                                if(hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa)
                                {
                                    var thongDiep202 = (from td in _db.ThongDiepChungs
                                                        where td.MaThongDiepThamChieu == thongDiep200.MaThongDiep && td.MaLoaiThongDiep == (int)MLTDiep.TBKQCMHDon
                                                        select td).FirstOrDefault();
                                    dataXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == thongDiep202.MaThongDiep).XMLData;
                                }
                                else
                                {
                                    var thongDiep204 = (from td in _db.ThongDiepChungs
                                                        where td.MaThongDiepThamChieu == thongDiep200.MaThongDiep && td.MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon
                                                        select td).FirstOrDefault();
                                    dataXMLRes = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == thongDiep204.MaThongDiep).XMLData;
                                }
                            }
                        }
                        else
                        {
                            var thongDiep400 = (from bth in _db.BangTongHopDuLieuHoaDons
                                                join bthct in _db.BangTongHopDuLieuHoaDonChiTiets on bth.Id equals bthct.BangTongHopDuLieuHoaDonId
                                                join td in _db.ThongDiepChungs on bth.ThongDiepChungId equals td.ThongDiepChungId
                                                where td.MaLoaiThongDiep == (int)MLTDiep.TDCBTHDLHDDDTDCQThue && bthct.RefHoaDonDienTuId == hoaDonDienTuId
                                                select td).FirstOrDefault();

                            dataXML = _db.FileDatas.Where(x => x.RefId == thongDiep400.ThongDiepChungId && x.Type == 1).FirstOrDefault().Content;
                            if (string.IsNullOrEmpty(dataXML))
                            {
                                var bin = _db.FileDatas.Where(x => x.RefId == thongDiep400.ThongDiepChungId && x.Type == 1).FirstOrDefault().Binary;
                                dataXML = Encoding.UTF8.GetString(bin);
                            }

                            if (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe || hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonKhongHopLe)
                            {
                                var thongDiep204 = (from td in _db.ThongDiepChungs
                                                    where td.MaThongDiepThamChieu == thongDiep400.MaThongDiep && td.MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon
                                                    select td).FirstOrDefault();
                                dataXMLRes = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == thongDiep204.MaThongDiep).XMLData;
                            }

                        }
                    }
                }
                else
                {
                    var thongDiep206 = (from dlghddt in _db.DuLieuGuiHDDTs
                                        join td in _db.ThongDiepChungs on dlghddt.ThongDiepChungId equals td.ThongDiepChungId
                                        where dlghddt.HoaDonDienTuId == hoaDonDienTuId
                                        select td).FirstOrDefault();

                    dataXML = _db.FileDatas.Where(x => x.RefId == thongDiep206.ThongDiepChungId && x.Type == 1).FirstOrDefault().Content;
                    if (string.IsNullOrEmpty(dataXML))
                    {
                        var bin = _db.FileDatas.Where(x => x.RefId == thongDiep206.ThongDiepChungId && x.Type == 1).FirstOrDefault().Binary;
                        dataXML = Encoding.UTF8.GetString(bin);
                    }

                    if (hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonHopLe || hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.HoaDonKhongHopLe)
                    {
                        var thongDiep204 = (from td in _db.ThongDiepChungs
                                            where td.MaThongDiepThamChieu == thongDiep206.MaThongDiep && td.MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon
                                            select td).FirstOrDefault();
                        dataXMLRes = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == thongDiep204.MaThongDiep).XMLData;
                    }

                }



                XmlDocument xmlDocument = new XmlDocument
                {
                    PreserveWhitespace = true
                };

                XmlDocument xmlDocumentRes = new XmlDocument
                {
                    PreserveWhitespace = true
                };

                // Format using white spaces.
                //xmlDocument.PreserveWhitespace = true;

                // Load the passed XML file into the document. 
                xmlDocument.LoadXml(dataXML);
                if (!string.IsNullOrEmpty(dataXMLRes) && dataXMLRes != "")
                {
                    xmlDocumentRes.LoadXml(dataXMLRes);
                }

                string query = string.Format("//*[@Id='{0}']", "SigningData"); // or "//book[@id='{0}']"
                XmlElement el = (XmlElement)xmlDocument.SelectSingleNode(query);
                string queryRes = string.Format("//*[@Id='{0}']", "Id-ID"); // or "//book[@id='{0}']"
                XmlNodeList elRes = xmlDocumentRes.SelectNodes("//*[contains(@Id, 'Id-ID')]");

                var signatureElements = xmlDocument.DocumentElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
                var signatureElementRes = xmlDocumentRes.DocumentElement != null ? xmlDocumentRes.DocumentElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#") : null;

                if (signatureElements.Count >= 1)
                {
                    SignedXml signedXml = new SignedXml(el);

                    if (type == 1)
                    {
                        signedXml.LoadXml((XmlElement)signatureElements[0]);

                        var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                        var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                        //thông tin người bán
                        return certificate.GetRawCertData();
                    }
                    else if (type == 2)
                    {
                        if(signatureElements.Count >= 2)
                        {
                            signedXml.LoadXml((XmlElement)signatureElements[1]);

                            var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                            var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                            //thông tin CQT
                            return certificate.GetRawCertData();
                        }
                        else
                        {
                            if (signatureElementRes != null && signatureElementRes.Count >= 1)
                            {
                                SignedXml signedXmlRes = new SignedXml((XmlElement)elRes[0]);
                                signedXml.LoadXml((XmlElement)signatureElementRes[0]);

                                var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                                var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                                //thông tin CQT
                                return certificate.GetRawCertData();

                            }
                            else return null;
                        }
                    }
                    else if (type == 3)
                    {
                        if (signatureElements.Count >= 3)
                        {
                            signedXml.LoadXml((XmlElement)signatureElements[2]);

                            var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                            var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                            //thông tin CQT
                            return certificate.GetRawCertData();
                        }
                        else if (string.IsNullOrEmpty(dataXMLRes) && signatureElements.Count >= 2)
                        {
                            signedXml.LoadXml((XmlElement)signatureElements[1]);

                            var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                            var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                            //thông tin CQT
                            return certificate.GetRawCertData();
                        }
                        else return null;
                    }
                    else return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.Write("Error FindSignatureElement:" + ex);
                return null;
            }
        }



        public bool CheckCorrectLookupFile3(string filePath)
        {
            try
            {
                // Check the arguments.  
                if (filePath == null)
                    throw new ArgumentNullException("Name");

                // Create a new XML document.
                XmlDocument xmlDocument = new XmlDocument
                {
                    PreserveWhitespace = false
                };
                // Format using white spaces.
                //xmlDocument.PreserveWhitespace = true;

                // Load the passed XML file into the document. 
                xmlDocument.Load(filePath);

                string query = string.Format("//*[@Id='{0}']", "SigningData"); // or "//book[@id='{0}']"
                XmlElement el = (XmlElement)xmlDocument.SelectSingleNode(query);
                XmlNode signatureNode = FindSignatureElement(xmlDocument);
                if (signatureNode == null) return true;

                SignedXml signedXml = new SignedXml(el);
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
    }
}
