using AutoMapper;
using DLL;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.BaoCao;
using Services.Repositories.Interfaces.BaoCao;
using Services.ViewModels.BaoCao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.BaoCao
{
    public class BaoCaoService : IBaoCaoService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;


        public BaoCaoService(
            Datacontext db,
            IMapper mp,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor accessor
        )
        {
            _db = db;
            _mp = mp;
            _hostingEnvironment = hostingEnvironment;
            _accessor = accessor;
        }

        public async Task<string> ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var list = @params.ListSoLuongHoaDonDaPhatHanhs;
            string excelFileName = string.Empty;

            try
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                else
                {
                    FileHelper.ClearFolder(uploadFolder);
                }

                excelFileName = $"Thong_Ke_So_Luong_Hoa_Don_Da_Phat_Hanh-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"docs/BaoCao/Thong_Ke_So_Luong_Hoa_Don_Da_Phat_Hanh.xlsx";
                string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

                FileInfo file = new FileInfo(_path_sample);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // Open sheet1
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // From to time
                    worksheet.Cells[5, 1].Value = string.Format("Từ ngày {0} đến ngày {1}", @params.TuNgay.Value.ToString("dd/MM/yyyy"), @params.DenNgay.Value.ToString("dd/MM/yyyy"));

                    // Get total all row
                    int totalRows = list.Count;

                    // Begin row
                    int begin_row = 9;

                    // Add Row
                    if (totalRows > 0) worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);

                    // Fill data
                    int idx = begin_row;
                    int count = 1;
                    foreach (var _it in list)
                    {
                        worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[idx, 1].Value = count;
                        worksheet.Cells[idx, 2].Value = _it.TenLoaiHoaDon;
                        worksheet.Cells[idx, 3].Value = _it.MauSo;
                        worksheet.Cells[idx, 4].Value = _it.KyHieu;
                        worksheet.Cells[idx, 5].Value = _it.TongSo;
                        worksheet.Cells[idx, 6].Value = _it.DaSuDung;
                        worksheet.Cells[idx, 7].Value = _it.DaXoaBo;

                        idx += 1;
                        count++;
                    }

                    if (idx == begin_row) idx++;
                    worksheet.Row(idx).Style.Font.Bold = true;
                    worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                    decimal? total_tong_so = list.Sum(o => o.TongSo);
                    decimal? total_da_su_dung = list.Sum(o => o.DaSuDung);
                    decimal? total_da_xoa_bo = list.Sum(o => o.DaXoaBo);
                    worksheet.Cells[idx, 1].Value = "Số dòng = " + totalRows;
                    worksheet.Cells[idx, 5].Value = total_tong_so;
                    worksheet.Cells[idx, 6].Value = total_da_su_dung;
                    worksheet.Cells[idx, 7].Value = total_da_xoa_bo;


                    package.SaveAs(new FileInfo(excelPath));
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return GetLinkFile(excelFileName);
        }

        private string GetLinkFile(string link)
        {
            var filename = "FilesUpload/excels/" + link;
            string url = "";
            if (_accessor.HttpContext.Request.IsHttps)
            {
                url = "https://" + _accessor.HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + _accessor.HttpContext.Request.Host;
            }
            url = url + "/" + filename;
            return url;
        }

        public async Task<List<SoLuongHoaDonDaPhatHanhViewModel>> ThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var result = new List<SoLuongHoaDonDaPhatHanhViewModel>();
            try
            {
                result = await _db.HoaDonDienTus.Where(x => x.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh && !string.IsNullOrEmpty(x.SoHoaDon)
                                                      && x.NgayHoaDon >= @params.TuNgay.Value && x.NgayHoaDon <= @params.DenNgay.Value
                                                 )
                                                .GroupBy(x => new { x.LoaiHoaDon, x.MauSo, x.KyHieu })
                                                .Select(x => new SoLuongHoaDonDaPhatHanhViewModel
                                                {
                                                    TenLoaiHoaDon = x.First().LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG ? "Hóa đơn giá trị gia tăng" : "Hóa đơn bán hàng",
                                                    MauSo = x.First().MauSo,
                                                    KyHieu = x.First().KyHieu,
                                                    TongSo = x.Count(),
                                                    DaSuDung = x.Count(o => o.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo),
                                                    DaXoaBo = x.Count(o => o.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo)
                                                })
                                                .ToListAsync();
                @params.ListSoLuongHoaDonDaPhatHanhs = result;
                @params.FilePath = await ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(@params);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return result;
        }

        public async Task<List<BaoCaoBangKeChiTietHoaDonViewModel>> BangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var result = new List<BaoCaoBangKeChiTietHoaDonViewModel>();
            try
            {
                var query = from hd in _db.HoaDonDienTus
                            join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauhoaDons
                            from mhd in tmpMauhoaDons.DefaultIfEmpty()
                            join kh in _db.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                            from kh in tmpKhachHangs.DefaultIfEmpty()
                            join httt in _db.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThuc
                            from httt in tmpHinhThuc.DefaultIfEmpty()
                            join hdct in _db.HoaDonDienTuChiTiets on hd.HoaDonDienTuId equals hdct.HoaDonDienTuId into tmpChiTiets
                            from hdct in tmpChiTiets.DefaultIfEmpty()
                            join hh in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals hh.HangHoaDichVuId into tmpHangHoas
                            from hh in tmpHangHoas.DefaultIfEmpty()
                            join nl in _db.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                            from nl in tmpNguoiLaps.DefaultIfEmpty()
                            join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonVis
                            from dvt in tmpDonVis.DefaultIfEmpty()
                            join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                            from lt in tmpLoaiTiens.DefaultIfEmpty()
                            where hd.NgayHoaDon <= @params.DenNgay && hd.NgayHoaDon >= @params.TuNgay && hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh
                            select new BaoCaoBangKeChiTietHoaDonViewModel
                            {
                                HoaDonDienTuId = hd.HoaDonDienTuId,
                                NgayHoaDon = hd.NgayHoaDon,
                                SoHoaDon = hd.SoHoaDon ?? "<Chưa cấp số>",
                                MauSoHoaDon = hd.MauSo ?? mhd.MauSo,
                                KyHieuHoaDon = hd.KyHieu ?? mhd.KyHieu,
                                MaKhachHang = hd.MaKhachHang ?? kh.Ma,
                                TenKhachHang = hd.TenKhachHang ?? kh.Ten,
                                DiaChi = hd.DiaChi ?? kh.DiaChi,
                                MaSoThue = hd.MaSoThue ?? kh.MaSoThue,
                                HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang ?? kh.HoTenNguoiMuaHang,
                                HinhThucThanhToan = httt.Ten,
                                LoaiTien = lt.Ten,
                                TyGia = hd.TyGia ?? 1,
                                MaHang = hdct.MaHang ?? hh.Ma,
                                TenHang = hdct.TenHang ?? hh.Ten,
                                DonViTinh = dvt.Ten,
                                SoLuong = hdct.SoLuong ?? 0,
                                DonGiaSauThue = hdct.DonGiaSauThue ?? (hdct.DonGia ?? 0),
                                DonGia = hdct.DonGia ?? 0,
                                ThanhTienSauThue = hdct.ThanhTienSauThue ?? (hdct.ThanhTien ?? 0),
                                ThanhTien = hdct.ThanhTien ?? 0,
                                ThanhTienQuyDoi = hdct.ThanhTienQuyDoi ?? 0,
                                TyLeChietKhau = hdct.TyLeChietKhau ?? 0,
                                TienChietKhau = hdct.TienChietKhau ?? 0,
                                TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi ?? 0,
                                DoanhThuChuaThue = (hdct.ThanhTien ?? 0) - (hdct.TienChietKhau ?? 0),
                                DoanhThuChuaThueQuyDoi = (hdct.ThanhTienQuyDoi ?? 0) - (hdct.TienChietKhauQuyDoi ?? 0),
                                ThueGTGT = hdct.ThueGTGT,
                                TienThueGTGT = hdct.TienThueGTGT ?? 0,
                                TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi ?? 0,
                                TongTienThanhToan = (hdct.ThanhTien ?? 0) - (hdct.TienChietKhau ?? 0) + (hdct.TienThueGTGT ?? 0),
                                TongTienThanhToanQuyDoi = (hdct.ThanhTienQuyDoi ?? 0) - (hdct.TienChietKhauQuyDoi ?? 0) + (hdct.TienThueGTGTQuyDoi ?? 0),
                                HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                HanSuDung = hdct.HanSuDung,
                                SoKhung = hdct.SoKhung,
                                SoMay = hdct.SoMay,
                                XuatBanPhi = 0,
                                GhiChu = hdct.GhiChu,
                                MaNhanVien = hd.MaNhanVienBanHang,
                                TenNhanVien = hd.TenNhanVienBanHang,
                                LoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                                TrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
                                TrangThaiPhatHanh = ((TrangThaiPhatHanh)hd.TrangThaiPhatHanh).GetDescription(),
                                MaTraCuu = hd.MaTraCuu,
                                LyDoXoaBo = hd.LyDoXoaBo,
                                NgayLap = hd.CreatedDate,
                                NguoiLap = nl != null ? nl.Ten : string.Empty
                            };

                if (@params.TuNgay.HasValue && @params.DenNgay.HasValue)
                {
                    query = query.Where(x => x.NgayHoaDon <= @params.DenNgay && x.NgayHoaDon >= @params.TuNgay);
                }

                if (!string.IsNullOrEmpty(@params.Key) && !string.IsNullOrEmpty(@params.Keyword))
                {
                    if (@params.Key == "soHoaDon")
                    {
                        query = query.Where(x => x.SoHoaDon.Contains(@params.Keyword));
                    }

                    if (@params.Key == "maSoThue")
                    {
                        query = query.Where(x => x.MaSoThue.Contains(@params.Keyword));
                    }

                    if (@params.Key == "tenKhachHang")
                    {
                        query = query.Where(x => x.TenKhachHang.Contains(@params.Keyword));
                    }

                    if (@params.Key == "hoTenNguoiMuaHang")
                    {
                        query = query.Where(x => x.HoTenNguoiMuaHang.Contains(@params.Keyword));
                    }
                }

                result = await query.ToListAsync();

                if (@params.CongGopTheoHoaDon == true)
                {
                    result = result.GroupBy(x => new { x.HoaDonDienTuId })
                                    .Select(x => new BaoCaoBangKeChiTietHoaDonViewModel
                                    {
                                        HoaDonDienTuId = x.Key.HoaDonDienTuId,
                                        NgayHoaDon = x.First().NgayHoaDon,
                                        SoHoaDon = x.First().SoHoaDon ?? "<Chưa cấp số>",
                                        MauSoHoaDon = x.First().MauSoHoaDon,
                                        KyHieuHoaDon = x.First().KyHieuHoaDon,
                                        MaKhachHang = x.First().MaKhachHang,
                                        TenKhachHang = x.First().TenKhachHang,
                                        DiaChi = x.First().DiaChi,
                                        MaSoThue = x.First().MaSoThue,
                                        HoTenNguoiMuaHang = x.First().HoTenNguoiMuaHang,
                                        HinhThucThanhToan = x.First().HinhThucThanhToan,
                                        LoaiTien = x.First().LoaiTien,
                                        TyGia = x.First().TyGia,
                                        ThanhTien = x.Sum(o => (o.ThanhTien)),
                                        ThanhTienQuyDoi = x.Sum(o => o.ThanhTienQuyDoi),
                                        TienChietKhau = x.Sum(o => o.TienChietKhau),
                                        TienChietKhauQuyDoi = x.Sum(o => o.TienChietKhauQuyDoi),
                                        DoanhThuChuaThue = x.Sum(o => o.DoanhThuChuaThue),
                                        DoanhThuChuaThueQuyDoi = x.Sum(o => o.DoanhThuChuaThueQuyDoi),
                                        LoaiHoaDon = x.First().LoaiHoaDon,
                                        TrangThaiHoaDon = x.First().TrangThaiHoaDon,
                                        TrangThaiPhatHanh = x.First().TrangThaiPhatHanh,
                                        MaTraCuu = x.First().MaTraCuu,
                                        LyDoXoaBo = x.First().LyDoXoaBo,
                                        NgayLap = x.First().NgayLap,
                                        NguoiLap = x.First().NguoiLap
                                    }).ToList();
                }

                for(int i=0; i<result.Count; i++)
                {
                    result[i].STT = i + 1;
                }
                @params.BangKeChiTietHoaDons = result;
                @params.FilePath = await ExportExcelBangKeChiTietHoaDonAsync(@params);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<string> ExportExcelBangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var list = @params.BangKeChiTietHoaDons;
            string excelFileName = string.Empty;

            try
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                else
                {
                    FileHelper.ClearFolder(uploadFolder);
                }

                excelFileName = $"BANG_KE_CHI_TIET_HOA_DON_DA_SU_DUNG-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"docs/BaoCao/BANG_KE_CHI_TIET_HOA_DON_DA_SU_DUNG.xlsx";

                string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

                FileInfo file = new FileInfo(_path_sample);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // Open sheet1
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // From to time
                    worksheet.Cells[5, 1].Value = string.Format("Từ ngày {0} đến ngày {1}", @params.TuNgay.Value.ToString("dd/MM/yyyy"), @params.DenNgay.Value.ToString("dd/MM/yyyy"));

                    // Get total all row
                    int totalRows = list.Count;

                    // Begin row
                    int begin_row = 8;

                    // Add Row
                    if (totalRows > 0) worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);

                    // Fill data
                    int idx = begin_row;
                    int count = 1;

                    foreach (var _it in list)
                    {
                        if (@params.CongGopTheoHoaDon == false)
                        {
                            worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[idx, 1].Value = count;
                            worksheet.Cells[idx, 2].Value = _it.NgayHoaDon.Value.ToString("dd/MM/yyyy");
                            worksheet.Cells[idx, 3].Value = _it.SoHoaDon;
                            worksheet.Cells[idx, 4].Value = _it.MauSoHoaDon;
                            worksheet.Cells[idx, 5].Value = _it.KyHieuHoaDon;
                            worksheet.Cells[idx, 6].Value = _it.MaKhachHang;
                            worksheet.Cells[idx, 7].Value = _it.TenKhachHang;
                            worksheet.Cells[idx, 8].Value = _it.DiaChi;
                            worksheet.Cells[idx, 9].Value = _it.MaSoThue;
                            worksheet.Cells[idx, 10].Value = _it.HoTenNguoiMuaHang;
                            worksheet.Cells[idx, 11].Value = _it.HinhThucThanhToan;
                            worksheet.Cells[idx, 12].Value = _it.LoaiTien;
                            worksheet.Cells[idx, 13].Value = _it.TyGia;
                            worksheet.Cells[idx, 14].Value = _it.MaHang;
                            worksheet.Cells[idx, 15].Value = _it.TenHang;
                            worksheet.Cells[idx, 16].Value = _it.DonViTinh;
                            worksheet.Cells[idx, 17].Value = _it.SoLuong;
                            worksheet.Cells[idx, 18].Value = _it.DonGia;
                            worksheet.Cells[idx, 19].Value = _it.ThanhTien;
                            worksheet.Cells[idx, 20].Value = _it.TyLeChietKhau;
                            worksheet.Cells[idx, 21].Value = _it.TienChietKhau;
                            worksheet.Cells[idx, 22].Value = _it.DoanhThuChuaThue;
                            worksheet.Cells[idx, 23].Value = _it.ThueGTGT;
                            worksheet.Cells[idx, 24].Value = _it.TienThueGTGT;
                            worksheet.Cells[idx, 25].Value = _it.TongTienThanhToan;
                            worksheet.Cells[idx, 26].Value = _it.HangKhuyenMai;
                            worksheet.Cells[idx, 27].Value = _it.MaNhanVien;
                            worksheet.Cells[idx, 28].Value = _it.TenNhanVien;
                            worksheet.Cells[idx, 29].Value = _it.LoaiHoaDon;
                            worksheet.Cells[idx, 30].Value = _it.TrangThaiHoaDon;
                            worksheet.Cells[idx, 31].Value = _it.TrangThaiPhatHanh;
                            worksheet.Cells[idx, 32].Value = _it.MaTraCuu;
                            worksheet.Cells[idx, 33].Value = _it.LyDoXoaBo;
                            worksheet.Cells[idx, 34].Value = _it.NgayLap.Value.ToString("dd/MM/yyyy");
                            worksheet.Cells[idx, 35].Value = _it.NguoiLap;
                        }

                        idx += 1;
                        count++;
                    }

                    if (idx == begin_row) idx++;
                    worksheet.Row(idx).Style.Font.Bold = true;
                    worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                    decimal? total_tong_thanh_tien = list.Sum(o => o.ThanhTien);
                    decimal? total_tien_chiet_khau = list.Sum(o => o.TienChietKhau);
                    decimal? total_doanh_so = list.Sum(o => o.DoanhThuChuaThue);
                    decimal? total_tien_thue_gtgt = list.Sum(o => o.TienThueGTGT);
                    decimal? total_tong_tien_thanh_toan = list.Sum(o => o.TongTienThanhToan);

                    worksheet.Cells[idx, 1].Value = "Số dòng = " + totalRows;
                    worksheet.Cells[idx, 19].Value = total_tong_thanh_tien;
                    worksheet.Cells[idx, 21].Value = total_tien_chiet_khau;
                    worksheet.Cells[idx, 22].Value = total_doanh_so;
                    worksheet.Cells[idx, 24].Value = total_tien_thue_gtgt;
                    worksheet.Cells[idx, 25].Value = total_tong_tien_thanh_toan;

                    package.SaveAs(new FileInfo(excelPath));
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return GetLinkFile(excelFileName);
        }

        public async Task<List<TongHopGiaTriHoaDonDaSuDung>> TongHopGiaTriHoaDonDaSuDungAsync(BaoCaoParams @params)
        {
            var query = _db.HoaDonDienTus
                .Where(x => x.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh && !string.IsNullOrEmpty(x.SoHoaDon) &&
                            (string.IsNullOrEmpty(@params.LoaiTienId) || x.LoaiTienId == @params.LoaiTienId) &&
                            x.NgayHoaDon.Value.Date >= @params.TuNgay.Value && x.NgayHoaDon.Value.Date <= @params.DenNgay.Value);

            if (@params.IsKhongTinhGiaTriHoaDonGoc == true)
            {
                query = query.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonGoc);
            }

            if (@params.IsKhongTinhGiaTriHoaDonXoaBo == true)
            {
                query = query.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonXoaBo);
            }

            if (@params.IsKhongTinhGiaTriHoaDonThayThe == true)
            {
                query = query.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonThayThe);
            }

            if (@params.IsKhongTinhGiaTriHoaDonDieuChinh == true)
            {
                query = query.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonDieuChinh);
            }

            var result = await query.GroupBy(x => new { x.LoaiHoaDon, x.MauSo, x.KyHieu })
                 .Select(x => new TongHopGiaTriHoaDonDaSuDung
                 {
                     TenLoaiHoaDon = x.Key.LoaiHoaDon.GetDescription(),
                     MauSo = x.Key.MauSo,
                     KyHieu = x.Key.KyHieu,
                     TongTienHang = x.Sum(y => y.TongTienHang),
                     TienChietKhau = x.Sum(y => y.TongTienChietKhau),
                     DoanhThuBanChuaThue = x.Sum(y => y.TongTienHang - y.TongTienChietKhau),
                     TienThueGTGT = x.Sum(y => y.TongTienThueGTGT),
                     TongTienThanhToan = x.Sum(y => y.TongTienThanhToan),
                     TongTienHangQuyDoi = x.Sum(y => y.TongTienHangQuyDoi),
                     TienChietKhauQuyDoi = x.Sum(y => y.TongTienChietKhauQuyDoi),
                     DoanhThuBanChuaThueQuyDoi = x.Sum(y => y.TongTienHangQuyDoi - y.TongTienChietKhauQuyDoi),
                     TienThueGTGTQuyDoi = x.Sum(y => y.TongTienThueGTGTQuyDoi),
                     TongTienThanhToanQuyDoi = x.Sum(y => y.TongTienThanhToanQuyDoi)
                 })
                 .ToListAsync();

            return result;
        }
    }
}
