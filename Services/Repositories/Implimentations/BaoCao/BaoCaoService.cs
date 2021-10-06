using AutoMapper;
using DLL;
using DLL.Entity.BaoCao;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.BaoCao;
using Services.Repositories.Interfaces.BaoCao;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels;
using Services.ViewModels.BaoCao;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
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
        private readonly IHoSoHDDTService _IHoSoHDDTService;


        public BaoCaoService(
            Datacontext db,
            IMapper mp,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor accessor,
            IHoSoHDDTService IHoSoHDDTService
        )
        {
            _db = db;
            _mp = mp;
            _hostingEnvironment = hostingEnvironment;
            _accessor = accessor;
            _IHoSoHDDTService = IHoSoHDDTService;
        }

        public async Task<string> ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var list = @params.ListSoLuongHoaDonDaPhatHanhs;
            string excelFileName = string.Empty;

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
                worksheet.Cells[3, 1].Value = string.Format("Từ ngày {0} đến ngày {1}", @params.TuNgay.Value.ToString("dd/MM/yyyy"), @params.DenNgay.Value.ToString("dd/MM/yyyy"));

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
                worksheet.Cells[idx, 1, idx, 4].Merge = true;
                worksheet.Cells[idx, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                decimal? total_tong_so = list.Sum(o => o.TongSo);
                decimal? total_da_su_dung = list.Sum(o => o.DaSuDung);
                decimal? total_da_xoa_bo = list.Sum(o => o.DaXoaBo);
                worksheet.Cells[idx, 1].Value = "Số dòng = " + totalRows;
                worksheet.Cells[idx, 5].Value = total_tong_so;
                worksheet.Cells[idx, 6].Value = total_da_su_dung;
                worksheet.Cells[idx, 7].Value = total_da_xoa_bo;


                package.SaveAs(new FileInfo(excelPath));
            }

            return GetLinkFile(excelFileName);
        }

        public async Task<string> PrintThongKeSoLuongHoaDonDaPhatHanh(BaoCaoParams @params)
        {
            try
            {
                string filePath = await ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(@params);
                string fileName = Path.GetFileName(filePath);

                string uploadFolder = $"FilesUpload/excels/{fileName}";
                string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder);
                string pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf");
                if (!Directory.Exists(pdfFolder))
                {
                    Directory.CreateDirectory(pdfFolder);
                }
                string pdfFileName = $"thongKeSoLuongHoaDonDaPhatHanh-{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/pdf/{pdfFileName}");

                // Convert excel to pdf
                FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, excelPath, pdfPath);
                return GetLinkFilePDF(pdfFileName);
            }
            catch (Exception ex)
            {
                return null;
            }
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

        private string GetLinkFilePDF(string link)
        {
            var filename = "FilesUpload/pdf/" + link;
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
                                LoaiTien = lt.Ma,
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
                                MaNhanVien = hdct.MaNhanVien,
                                TenNhanVien = hdct.TenNhanVien,
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

                for (int i = 0; i < result.Count; i++)
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
                worksheet.Cells[3, 1].Value = string.Format("Từ ngày {0} đến ngày {1}", @params.TuNgay.Value.ToString("dd/MM/yyyy"), @params.DenNgay.Value.ToString("dd/MM/yyyy"));

                // Get total all row
                int totalRows = list.Count;

                // Begin row
                int begin_row = 6;

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

            return GetLinkFile(excelFileName);
        }

        public async Task<string> PrintBangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            try
            {
                string filePath = await ExportExcelBangKeChiTietHoaDonAsync(@params);
                string fileName = Path.GetFileName(filePath);

                string uploadFolder = $"FilesUpload/excels/{fileName}";
                string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder);
                string pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf");
                if (!Directory.Exists(pdfFolder))
                {
                    Directory.CreateDirectory(pdfFolder);
                }
                string pdfFileName = $"bangKeChiTietHoaDon-{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/pdf/{pdfFileName}");

                // Convert excel to pdf
                FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, excelPath, pdfPath);
                return GetLinkFilePDF(pdfFileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<TongHopGiaTriHoaDonDaSuDung>> TongHopGiaTriHoaDonDaSuDungAsync(BaoCaoParams @params)
        {
            var queryHDDT = _db.HoaDonDienTus
               .Where(x => x.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh && !string.IsNullOrEmpty(x.SoHoaDon) &&
                           (string.IsNullOrEmpty(@params.LoaiTienId) || x.LoaiTienId == @params.LoaiTienId) &&
                           x.NgayHoaDon.Value.Date >= @params.TuNgay.Value && x.NgayHoaDon.Value.Date <= @params.DenNgay.Value);

            if (@params.IsKhongTinhGiaTriHoaDonGoc == true)
            {
                queryHDDT = queryHDDT.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonGoc);
            }

            if (@params.IsKhongTinhGiaTriHoaDonXoaBo == true)
            {
                queryHDDT = queryHDDT.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonXoaBo);
            }

            if (@params.IsKhongTinhGiaTriHoaDonThayThe == true)
            {
                queryHDDT = queryHDDT.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonThayThe);
            }

            if (@params.IsKhongTinhGiaTriHoaDonDieuChinh == true)
            {
                queryHDDT = queryHDDT.Where(x => (TrangThaiHoaDon)x.TrangThai != TrangThaiHoaDon.HoaDonDieuChinh);
            }

            var query = (from hddt in queryHDDT
                         join hddtct in _db.HoaDonDienTuChiTiets on hddt.HoaDonDienTuId equals hddtct.HoaDonDienTuId
                         select new TongHopGiaTriHoaDonDaSuDung
                         {
                             LoaiHoaDon = hddt.LoaiHoaDon,
                             TenLoaiHoaDon = ((LoaiHoaDon)hddt.LoaiHoaDon).GetDescription(),
                             MauSo = hddt.MauSo,
                             KyHieu = hddt.KyHieu,
                             TongTienHang = hddtct.ThanhTien,
                             TongTienHangQuyDoi = hddtct.ThanhTienQuyDoi,
                             DoanhThuBanChuaThue = hddtct.ThanhTien - hddtct.TienChietKhau,
                             DoanhThuBanChuaThueQuyDoi = hddtct.ThanhTienQuyDoi - hddtct.TienChietKhauQuyDoi,
                             TienThueGTGT = hddtct.TienThueGTGT,
                             TienThueGTGTQuyDoi = hddtct.TienThueGTGTQuyDoi,
                             TongTienThanhToan = hddtct.ThanhTien - hddtct.TienChietKhau + hddtct.TienThueGTGT,
                             TongTienThanhToanQuyDoi = hddtct.ThanhTienQuyDoi - hddtct.TienChietKhauQuyDoi + hddtct.TienThueGTGTQuyDoi,
                         })
                         .GroupBy(x => new { x.LoaiHoaDon, x.MauSo, x.KyHieu })
                         .Select(x => new TongHopGiaTriHoaDonDaSuDung
                         {
                             LoaiHoaDon = x.Key.LoaiHoaDon,
                             TenLoaiHoaDon = x.First().TenLoaiHoaDon,
                             MauSo = x.Key.MauSo,
                             KyHieu = x.Key.KyHieu,
                             TongTienHang = x.Sum(y => y.TongTienHang),
                             TongTienHangQuyDoi = x.Sum(y => y.TongTienHangQuyDoi),
                             DoanhThuBanChuaThue = x.Sum(y => y.DoanhThuBanChuaThue),
                             DoanhThuBanChuaThueQuyDoi = x.Sum(y => y.DoanhThuBanChuaThueQuyDoi),
                             TienThueGTGT = x.Sum(y => y.TienThueGTGT),
                             TienThueGTGTQuyDoi = x.Sum(y => y.TienThueGTGTQuyDoi),
                             TongTienThanhToan = x.Sum(y => y.TongTienThanhToan),
                             TongTienThanhToanQuyDoi = x.Sum(y => y.TongTienThanhToanQuyDoi),
                         });

            var result = await query.ToListAsync();

            result.Add(new TongHopGiaTriHoaDonDaSuDung
            {
                Count = result.Count(),
                IsTotal = true,
                TongTienHang = result.Sum(x => x.TongTienHang),
                TongTienHangQuyDoi = result.Sum(x => x.TongTienHangQuyDoi),
                DoanhThuBanChuaThue = result.Sum(x => x.DoanhThuBanChuaThue),
                DoanhThuBanChuaThueQuyDoi = result.Sum(x => x.DoanhThuBanChuaThueQuyDoi),
                TienThueGTGT = result.Sum(x => x.TienThueGTGT),
                TienThueGTGTQuyDoi = result.Sum(x => x.TienThueGTGTQuyDoi),
                TongTienThanhToan = result.Sum(x => x.TongTienThanhToan),
                TongTienThanhToanQuyDoi = result.Sum(x => x.TongTienThanhToanQuyDoi),
            });

            return result;
        }

        public async Task<bool> ThemBaoCaoTinhHinhSuDungHoaDon(ChonKyTinhThueParams @params)
        {
            var strId = Guid.NewGuid().ToString();
            var hsHDDT = await _IHoSoHDDTService.GetDetailAsync();
            var baoCao = new BaoCaoTinhHinhSuDungHoaDonViewModel()
            {
                BaoCaoTinhHinhSuDungHoaDonId = strId,
                Thang = @params.Thang,
                Quy = @params.Quy,
                Nam = @params.Nam,
                DienGiai = @params.Quy.HasValue ? "Quý " + @params.Quy.Value + " năm " + @params.Nam : "Tháng " + @params.Thang.Value + " năm " + @params.Nam,
                NgayLap = DateTime.Now,
                NguoiLapId = @params.ActionUser.UserId,
                TuNgay = @params.TuNgay.Value,
                DenNgay = @params.DenNgay.Value,
                TenNguoiLap = @params.ActionUser.FullName,
                TenNguoiDaiDienPhapLuat = hsHDDT.HoTenNguoiDaiDienPhapLuat
            };

            var entityBC = _mp.Map<BaoCaoTinhHinhSuDungHoaDon>(baoCao);
            await _db.BaoCaoTinhHinhSuDungHoaDons.AddAsync(entityBC);

            if (await _db.SaveChangesAsync() > 0)
            {
                var listChiTiets = (from mhd in _db.MauHoaDons
                                    join hd in _db.HoaDonDienTus on mhd.MauHoaDonId equals hd.MauHoaDonId into tmpHoaDons
                                    from hd in tmpHoaDons.DefaultIfEmpty()
                                    where hd.NgayHoaDon <= @params.DenNgay && hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh
                                    select new HoaDonDienTuViewModel()
                                    {
                                        HoaDonDienTuId = hd.HoaDonDienTuId,
                                        LoaiHoaDon = hd.LoaiHoaDon,
                                        MauHoaDonId = hd.MauHoaDonId,
                                        MauSo = hd.MauSo,
                                        KyHieu = hd.KyHieu,
                                        SoHoaDon = hd.SoHoaDon,
                                        TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                                        TrangThai = hd.TrangThai,
                                    }).ToList();
                var listChiTietsTrongKy = (from mhd in _db.MauHoaDons
                                           join hd in _db.HoaDonDienTus on mhd.MauHoaDonId equals hd.MauHoaDonId into tmpHoaDons
                                           from hd in tmpHoaDons.DefaultIfEmpty()
                                           where hd.NgayHoaDon >= @params.TuNgay && hd.NgayHoaDon <= @params.DenNgay && hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh
                                           select new HoaDonDienTuViewModel()
                                           {
                                               HoaDonDienTuId = hd.HoaDonDienTuId,
                                               LoaiHoaDon = hd.LoaiHoaDon,
                                               MauHoaDonId = hd.MauHoaDonId,
                                               MauSo = hd.MauSo,
                                               KyHieu = hd.KyHieu,
                                               SoHoaDon = hd.SoHoaDon,
                                               TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                                               TrangThai = hd.TrangThai,
                                           }).ToList();

                var listDauKy = (from mhd in _db.MauHoaDons
                                 join hd in _db.HoaDonDienTus on mhd.MauHoaDonId equals hd.MauHoaDonId into tmpHoaDons
                                 from hd in tmpHoaDons.DefaultIfEmpty()
                                 where hd.NgayHoaDon < @params.TuNgay && hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh
                                 select new HoaDonDienTuViewModel()
                                 {
                                     HoaDonDienTuId = hd.HoaDonDienTuId,
                                     LoaiHoaDon = hd.LoaiHoaDon,
                                     MauHoaDonId = hd.MauHoaDonId,
                                     SoHoaDon = hd.SoHoaDon,
                                     MauSo = hd.MauSo,
                                     KyHieu = hd.KyHieu,
                                     TrangThaiPhatHanh = hd.TrangThaiPhatHanh,
                                     TrangThai = hd.TrangThai,
                                 }).ToList();

                var listMauHoaDons = _mp.Map<List<MauHoaDonViewModel>>(await _db.MauHoaDons.Where(x => listDauKy.Any(o => o.MauHoaDonId == x.MauHoaDonId) || listChiTietsTrongKy.Any(o => o.MauHoaDonId == x.MauHoaDonId)).ToListAsync());
                var chiTiets = new List<BaoCaoTinhHinhSuDungHoaDonChiTietViewModel>();

                foreach (var item in listMauHoaDons)
                {
                    var thongBaoPhatHanhs = await _db.ThongBaoPhatHanhChiTiets.Include(x => x.ThongBaoPhatHanh)
                                                                        .Where(x => x.MauHoaDonId == item.MauHoaDonId && x.ThongBaoPhatHanh.Ngay <= @params.DenNgay)
                                                                        .ToListAsync();

                    var thongBaoPhatHanhDauKy = thongBaoPhatHanhs.Where(x => x.ThongBaoPhatHanh.Ngay < @params.TuNgay).ToList();
                    var thongBaoPhatHanhTrongKy = thongBaoPhatHanhs.Where(x => x.ThongBaoPhatHanh.Ngay >= @params.TuNgay).ToList();
                    var loaiHoaDons = listChiTiets.Where(x => x.MauHoaDonId == item.MauHoaDonId).Select(x => x.LoaiHoaDon).Distinct().ToList();

                    foreach (var loaiHD in loaiHoaDons)
                    {
                        var tongSo = thongBaoPhatHanhs.Where(x => x.MauHoaDonId == item.MauHoaDonId).Sum(x => (x.DenSo.Value - x.TuSo.Value)) -
                                    (listDauKy.Any(x => x.LoaiHoaDon == loaiHD && x.MauHoaDonId == item.MauHoaDonId) ? int.Parse(listDauKy.Where(x => x.MauHoaDonId == item.MauHoaDonId && x.LoaiHoaDon == loaiHD).Max(x => x.SoHoaDon)) : 0);
                        var tonDauKyTu = (listDauKy.Any() ? (thongBaoPhatHanhTrongKy.Min(x => x.TuSo) > int.Parse(listDauKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listDauKy.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon) : "0") ? (int.Parse(listDauKy.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon)) + 1).ToString("0000000") : string.Empty) : string.Empty);
                        var tonDauKyDen = (thongBaoPhatHanhTrongKy.Min(x => x.TuSo.Value) > int.Parse(listDauKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listDauKy.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon) : "0")) ? (thongBaoPhatHanhDauKy.Any() ? thongBaoPhatHanhDauKy.Max(x => x.DenSo.Value).ToString("0000000") : string.Empty) : string.Empty;
                        var trongKyTu = thongBaoPhatHanhTrongKy.Any() ? thongBaoPhatHanhTrongKy.Min(x => x.TuSo.Value).ToString("0000000") : string.Empty;
                        var trongKyDen = thongBaoPhatHanhTrongKy.Any() ? thongBaoPhatHanhTrongKy.Max(x => x.DenSo.Value).ToString("0000000") : string.Empty;
                        var tongSoSuDung = listChiTietsTrongKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listChiTietsTrongKy.Count(x => x.MauHoaDonId == item.MauHoaDonId) : 0;
                        var suDungTu = listChiTietsTrongKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listChiTietsTrongKy.Where(x => x.MauHoaDonId == item.MauHoaDonId).Min(x => x.SoHoaDon) : string.Empty;
                        var suDungDen = listChiTietsTrongKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listChiTietsTrongKy.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon) : string.Empty;
                        var daSuDung = listChiTietsTrongKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listChiTietsTrongKy.Count(x => x.MauHoaDonId == item.MauHoaDonId && x.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) : 0;
                        var daXoaBo = listChiTietsTrongKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? listChiTietsTrongKy.Count(x => x.MauHoaDonId == item.MauHoaDonId && x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo) : 0;
                        var soXoaBo = listChiTietsTrongKy.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? string.Join("-", listChiTietsTrongKy.Where(x => x.MauHoaDonId == item.MauHoaDonId && x.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo).OrderBy(x => x.SoHoaDon).Select(x => int.Parse(x.SoHoaDon).ToString()).ToArray()) : string.Empty;
                        var tonCuoiKyTu = listChiTiets.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? (thongBaoPhatHanhTrongKy.Max(x => x.DenSo) > int.Parse(listChiTiets.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon)) ? (int.Parse(listChiTiets.Max(x => x.SoHoaDon)) + 1).ToString("0000000") : string.Empty) : thongBaoPhatHanhDauKy.Min(x => x.TuSo.Value).ToString("0000000");
                        var tonCuoiKyDen = listChiTiets.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? (thongBaoPhatHanhTrongKy.Max(x => x.DenSo) > int.Parse(listChiTiets.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon)) ? thongBaoPhatHanhTrongKy.Max(x => x.DenSo.Value).ToString("0000000") : string.Empty) : thongBaoPhatHanhTrongKy.Max(x => x.DenSo.Value).ToString("0000000");
                        var soLuongTon = listChiTiets.Any(x => x.MauHoaDonId == item.MauHoaDonId) ? (thongBaoPhatHanhs.Max(x => x.DenSo.Value) - int.Parse(listChiTiets.Where(x => x.MauHoaDonId == item.MauHoaDonId).Max(x => x.SoHoaDon))) : (thongBaoPhatHanhs.Max(x => x.DenSo.Value) - thongBaoPhatHanhs.Min(x => x.TuSo.Value));

                        chiTiets.Add(new BaoCaoTinhHinhSuDungHoaDonChiTietViewModel()
                        {
                            BaoCaoTinhHinhSuDungHoaDonChiTietId = Guid.NewGuid().ToString(),
                            BaoCaoTinhHinhSuDungHoaDonId = strId,
                            LoaiHoaDon = loaiHD.Value,
                            MauSo = item.MauSo,
                            KyHieu = item.KyHieu,
                            TongSo = tongSo,
                            TonDauKyTu = tonDauKyTu,
                            TonDauKyDen = tonDauKyDen,
                            TrongKyTu = trongKyTu,
                            TrongKyDen = trongKyDen,
                            TongSoSuDung = tongSoSuDung,
                            SuDungTu = suDungTu,
                            SuDungDen = suDungDen,
                            DaSuDung = daSuDung,
                            DaXoaBo = daXoaBo,
                            SoXoaBo = soXoaBo,
                            DaHuy = 0,
                            SoHuy = string.Empty,
                            DaMat = 0,
                            SoMat = string.Empty,
                            TonCuoiKyTu = tonCuoiKyTu,
                            TonCuoiKyDen = tonCuoiKyDen,
                            SoLuongTon = soLuongTon
                        });
                    }

                }

                var entities = _mp.Map<List<BaoCaoTinhHinhSuDungHoaDonChiTiet>>(chiTiets);
                await _db.BaoCaoTinhHinhSuDungHoaDonChiTiets.AddRangeAsync(entities);
                return await _db.SaveChangesAsync() == entities.Count;
            }

            return false;
        }

        public async Task<bool> CapNhatChiTietBaoCaoTinhHinhSuDungHoaDon(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao)
        {
            var chiTiets = baoCao.ChiTiets;
            baoCao.ChiTiets = null;

            var entitiesCT = _db.BaoCaoTinhHinhSuDungHoaDonChiTiets.Where(x => x.BaoCaoTinhHinhSuDungHoaDonId == baoCao.BaoCaoTinhHinhSuDungHoaDonId).ToList();
            _db.BaoCaoTinhHinhSuDungHoaDonChiTiets.RemoveRange(entitiesCT);
            if (await _db.SaveChangesAsync() == entitiesCT.Count)
            {
                entitiesCT = _mp.Map<List<BaoCaoTinhHinhSuDungHoaDonChiTiet>>(chiTiets);
                await _db.BaoCaoTinhHinhSuDungHoaDonChiTiets.AddRangeAsync(entitiesCT);
                await _db.SaveChangesAsync();

                var entity = _mp.Map<BaoCaoTinhHinhSuDungHoaDon>(baoCao);
                _db.BaoCaoTinhHinhSuDungHoaDons.Update(entity);
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<List<BaoCaoTinhHinhSuDungHoaDonViewModel>> GetListTinhHinhSuDungHoaDon(PagingParams @params)
        {
            var result = new List<BaoCaoTinhHinhSuDungHoaDonViewModel>();

            DateTime _from = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime _to = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            result = _mp.Map<List<BaoCaoTinhHinhSuDungHoaDonViewModel>>(await _db.BaoCaoTinhHinhSuDungHoaDons.Where(x => x.NgayLap >= _from && x.NgayLap <= _to).ToListAsync());

            return result;
        }

        public async Task<BaoCaoTinhHinhSuDungHoaDonViewModel> GetById(string baoCaoId)
        {
            var query = from bc in _db.BaoCaoTinhHinhSuDungHoaDons
                        join nl in _db.Users on bc.NguoiLapId equals nl.UserId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        where bc.BaoCaoTinhHinhSuDungHoaDonId == baoCaoId
                        select new BaoCaoTinhHinhSuDungHoaDonViewModel
                        {
                            BaoCaoTinhHinhSuDungHoaDonId = bc.BaoCaoTinhHinhSuDungHoaDonId,
                            DienGiai = bc.DienGiai,
                            TuNgay = bc.TuNgay,
                            DenNgay = bc.DenNgay,
                            Nam = bc.Nam,
                            Thang = bc.Thang,
                            Quy = bc.Quy,
                            NgayLap = bc.NgayLap,
                            NguoiLapId = bc.NguoiLapId,
                            NguoiLap = _mp.Map<UserViewModel>(nl),
                            TenNguoiLap = nl.FullName,
                            TenNguoiDaiDienPhapLuat = bc.TenNguoiDaiDienPhapLuat,
                            ChiTiets = (from bcct in _db.BaoCaoTinhHinhSuDungHoaDonChiTiets
                                        where bcct.BaoCaoTinhHinhSuDungHoaDonId == baoCaoId
                                        select new BaoCaoTinhHinhSuDungHoaDonChiTietViewModel
                                        {
                                            BaoCaoTinhHinhSuDungHoaDonChiTietId = bcct.BaoCaoTinhHinhSuDungHoaDonChiTietId,
                                            BaoCaoTinhHinhSuDungHoaDonId = bc.BaoCaoTinhHinhSuDungHoaDonId,
                                            LoaiHoaDon = bcct.LoaiHoaDon,
                                            MauSo = bcct.MauSo,
                                            KyHieu = bcct.KyHieu,
                                            TongSo = bcct.TongSo,
                                            TonDauKyTu = bcct.TonDauKyTu,
                                            TonDauKyDen = bcct.TonDauKyDen,
                                            TrongKyTu = bcct.TrongKyTu,
                                            TrongKyDen = bcct.TrongKyDen,
                                            TongSoSuDung = bcct.TongSoSuDung,
                                            DaSuDung = bcct.DaSuDung,
                                            SuDungTu = bcct.SuDungTu,
                                            SuDungDen = bcct.SuDungDen,
                                            DaXoaBo = bcct.DaXoaBo,
                                            SoXoaBo = bcct.SoXoaBo,
                                            DaMat = bcct.DaMat,
                                            SoMat = bcct.SoMat,
                                            SoHuy = bcct.SoHuy,
                                            DaHuy = bcct.DaHuy,
                                            TonCuoiKyTu = bcct.TonCuoiKyTu,
                                            TonCuoiKyDen = bcct.TonCuoiKyDen,
                                            SoLuongTon = bcct.SoLuongTon
                                        }).ToList()
                        };

            var result = await query.FirstOrDefaultAsync();
            result.ExcelPath = await ExportExcelBaoCaoTinhHinhSuDungHoaDonAsync(result);
            return result;
        }

        public async Task<ChonKyTinhThueParams> CheckNgayThangBaoCaoTinhHinhSuDungHD(ChonKyTinhThueParams @params)
        {
            if (await _db.BaoCaoTinhHinhSuDungHoaDons.AnyAsync(x => (x.TuNgay <= @params.TuNgay && x.DenNgay >= @params.DenNgay)
                                                                 || (@params.TuNgay >= x.TuNgay && @params.DenNgay < x.DenNgay)
                                                                 || (@params.TuNgay < x.DenNgay)))
                return await _db.BaoCaoTinhHinhSuDungHoaDons.Where(x => (x.TuNgay <= @params.TuNgay && x.DenNgay >= @params.DenNgay)
                                                                    || (@params.TuNgay >= x.TuNgay && @params.DenNgay < x.DenNgay)
                                                                    || (@params.TuNgay < x.DenNgay))
                    .Select(x => new ChonKyTinhThueParams
                    {
                        TuNgay = x.TuNgay,
                        DenNgay = x.DenNgay
                    })
                    .FirstOrDefaultAsync();

            return null;
        }

        public async Task<bool> XoaBaoCaoTinhHinhSuDungHoaDon(string BaoCaoId)
        {
            var entitiesChiTiet = await _db.BaoCaoTinhHinhSuDungHoaDonChiTiets.Where(x => x.BaoCaoTinhHinhSuDungHoaDonId == BaoCaoId)
                                                                              .ToListAsync();

            if (entitiesChiTiet.Count > 0)
            {
                _db.BaoCaoTinhHinhSuDungHoaDonChiTiets.RemoveRange(entitiesChiTiet);
                if (await _db.SaveChangesAsync() > 0)
                {
                    var entity = await _db.BaoCaoTinhHinhSuDungHoaDons.FirstOrDefaultAsync(x => x.BaoCaoTinhHinhSuDungHoaDonId == BaoCaoId);
                    _db.BaoCaoTinhHinhSuDungHoaDons.Remove(entity);
                    return await _db.SaveChangesAsync() > 0;
                }
            }
            else
            {
                var entity = await _db.BaoCaoTinhHinhSuDungHoaDons.FirstOrDefaultAsync(x => x.BaoCaoTinhHinhSuDungHoaDonId == BaoCaoId);
                _db.BaoCaoTinhHinhSuDungHoaDons.Remove(entity);
                return await _db.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<BaoCaoBangKeChiTietHoaDonViewModel> GetBaoCaoByKyTinhThue(ChonKyTinhThueParams @params)
        {
            return _mp.Map<BaoCaoBangKeChiTietHoaDonViewModel>(await _db.BaoCaoTinhHinhSuDungHoaDons.FirstOrDefaultAsync(x => x.TuNgay == @params.TuNgay && x.DenNgay == @params.DenNgay));
        }

        public async Task<FileReturn> ExportExcelTongHopGiaTriHoaDonDaSuDungAsync(BaoCaoParams @params)
        {
            List<TongHopGiaTriHoaDonDaSuDung> list = @params.TongHopGiaTriHoaDonDaSuDungs;

            HoSoHDDTViewModel hoSoHDDTVM = await _IHoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/BaoCao/{(@params.LoaiMau == 1 ? "Tong_hop_gia_tri_hoa_don_da_su_dung.xlsx" : "Tong_hop_gia_tri_hoa_don_da_su_dung_maungoaite.xlsx")}";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                var total = list.FirstOrDefault(x => x.IsTotal == true);
                int totalRows = total.Count ?? 0;
                int begin_row = @params.LoaiMau == 1 ? 9 : 10;

                worksheet.Cells[1, 1].Value = hoSoHDDTVM.TenDonVi;
                worksheet.Cells[2, 1].Value = hoSoHDDTVM.DiaChi;

                worksheet.Cells[5, 1].Value = @params.KyBaoCao;
                worksheet.Cells[6, 1].Value = @params.ThongTinVeLoaiTienVaTrangThai;

                // Add Row
                if (totalRows != 0)
                {
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                }
                // Fill data
                int idx = begin_row + (totalRows == 0 ? 1 : 0);
                int stt = 1;
                foreach (var _it in list.Where(x => x.IsTotal != true))
                {
                    worksheet.Cells[idx, 1].Value = stt;
                    worksheet.Cells[idx, 2].Value = _it.TenLoaiHoaDon;
                    worksheet.Cells[idx, 3].Value = _it.MauSo;
                    worksheet.Cells[idx, 4].Value = _it.KyHieu;

                    if (@params.LoaiMau == 1)
                    {
                        worksheet.Cells[idx, 5].Value = _it.TongTienHangQuyDoi;
                        worksheet.Cells[idx, 6].Value = _it.TienChietKhauQuyDoi;
                        worksheet.Cells[idx, 7].Value = _it.DoanhThuBanChuaThueQuyDoi;
                        worksheet.Cells[idx, 8].Value = _it.TienThueGTGTQuyDoi;
                        worksheet.Cells[idx, 9].Value = _it.TongTienThanhToanQuyDoi;
                    }
                    else
                    {
                        worksheet.Cells[idx, 5].Value = _it.TongTienHang;
                        worksheet.Cells[idx, 6].Value = _it.TongTienHangQuyDoi;
                        worksheet.Cells[idx, 7].Value = _it.TienChietKhau;
                        worksheet.Cells[idx, 8].Value = _it.TienChietKhauQuyDoi;
                        worksheet.Cells[idx, 9].Value = _it.DoanhThuBanChuaThue;
                        worksheet.Cells[idx, 10].Value = _it.DoanhThuBanChuaThueQuyDoi;
                        worksheet.Cells[idx, 11].Value = _it.TienThueGTGT;
                        worksheet.Cells[idx, 12].Value = _it.TienThueGTGTQuyDoi;
                        worksheet.Cells[idx, 13].Value = _it.TongTienThanhToan;
                        worksheet.Cells[idx, 14].Value = _it.TongTienThanhToanQuyDoi;
                    }

                    idx += 1;
                    stt += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);
                if (@params.LoaiMau == 1)
                {
                    worksheet.Cells[idx, 5].Value = total.TongTienHangQuyDoi;
                    worksheet.Cells[idx, 6].Value = total.TienChietKhauQuyDoi;
                    worksheet.Cells[idx, 7].Value = total.DoanhThuBanChuaThueQuyDoi;
                    worksheet.Cells[idx, 8].Value = total.TienThueGTGTQuyDoi;
                    worksheet.Cells[idx, 9].Value = total.TongTienThanhToanQuyDoi;
                }
                else
                {
                    worksheet.Cells[idx, 5].Value = total.TongTienHang;
                    worksheet.Cells[idx, 6].Value = total.TongTienHangQuyDoi;
                    worksheet.Cells[idx, 7].Value = total.TienChietKhau;
                    worksheet.Cells[idx, 8].Value = total.TienChietKhauQuyDoi;
                    worksheet.Cells[idx, 9].Value = total.DoanhThuBanChuaThue;
                    worksheet.Cells[idx, 10].Value = total.DoanhThuBanChuaThueQuyDoi;
                    worksheet.Cells[idx, 11].Value = total.TienThueGTGT;
                    worksheet.Cells[idx, 12].Value = total.TienThueGTGTQuyDoi;
                    worksheet.Cells[idx, 13].Value = total.TongTienThanhToan;
                    worksheet.Cells[idx, 14].Value = total.TongTienThanhToanQuyDoi;
                }

                string fileName = $"Tong_hop_gia_tri_hoa_don_da_su_dung_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string filePath = Path.Combine(destPath, fileName);
                package.SaveAs(new FileInfo(filePath));

                byte[] fileByte;

                if (@params.IsPrint == true)
                {
                    string pdfPath = Path.Combine(destPath, $"print-{DateTime.Now:yyyyMMddHHmmss}.pdf");
                    FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, filePath, pdfPath);
                    File.Delete(filePath);
                    fileByte = File.ReadAllBytes(pdfPath);
                    filePath = pdfPath;
                    File.Delete(filePath);
                }
                else
                {
                    fileByte = File.ReadAllBytes(filePath);
                    File.Delete(filePath);
                }

                return new FileReturn
                {
                    Bytes = fileByte,
                    ContentType = MimeTypes.GetMimeType(filePath),
                    FileName = Path.GetFileName(filePath)
                };
            }
        }

        public async Task<string> ExportExcelBaoCaoTinhHinhSuDungHoaDonAsync(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao)
        {
            var list = baoCao.ChiTiets;
            string excelFileName = string.Empty;

            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            else
            {
                FileHelper.ClearFolder(uploadFolder);
            }

            excelFileName = $"BAO_CAO_TINH_HINH_SU_DUNG_HOA_DON_BC26-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
            string excelFolder = $"FilesUpload/excels/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            // Excel
            string _sample = $"docs/BaoCao/BAO_CAO_TINH_HINH_SU_DUNG_HOA_DON_BC26.xlsx";

            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // From to time
                worksheet.Cells[6, 1].Value = baoCao.DienGiai;
                worksheet.Cells[7, 1].Value = string.Format("(Từ ngày {0} đến ngày {1})", baoCao.TuNgay.ToString("dd/MM/yyyy"), baoCao.DenNgay.ToString("dd/MM/yyyy"));

                //Tên tổ chức, cá nhân
                var thongTinCty = await _IHoSoHDDTService.GetDetailAsync();
                worksheet.Cells[8, 3].Value = thongTinCty.TenDonVi;
                worksheet.Cells[9, 3].Value = thongTinCty.MaSoThue;
                worksheet.Cells[10, 3].Value = thongTinCty.DiaChi;

                // Get total all row
                int totalRows = list.Count;

                // Begin row
                int begin_row = 17;

                // Add Row
                if (totalRows > 0) worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);

                // Fill data
                int idx = begin_row;
                int count = 1;

                foreach (var _it in list)
                {
                    worksheet.Row(idx).Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[idx, 1].Value = count;
                    worksheet.Cells[idx, 2].Value = _it.LoaiHoaDon == 1 ? "Hóa đơn GTGT" : "Hóa đơn bán hàng";
                    worksheet.Cells[idx, 3].Value = _it.MauSo;
                    worksheet.Cells[idx, 4].Value = _it.KyHieu;
                    worksheet.Cells[idx, 5].Value = _it.TongSo;
                    worksheet.Cells[idx, 6].Value = _it.TonDauKyTu;
                    worksheet.Cells[idx, 7].Value = _it.TonDauKyDen;
                    worksheet.Cells[idx, 8].Value = _it.TrongKyTu;
                    worksheet.Cells[idx, 9].Value = _it.TrongKyDen;
                    worksheet.Cells[idx, 10].Value = _it.SuDungTu;
                    worksheet.Cells[idx, 11].Value = _it.SuDungDen;
                    worksheet.Cells[idx, 12].Value = _it.DaSuDung;
                    worksheet.Cells[idx, 13].Value = _it.TongSoSuDung;
                    worksheet.Cells[idx, 14].Value = _it.DaXoaBo;
                    worksheet.Cells[idx, 15].Value = _it.SoXoaBo;
                    worksheet.Cells[idx, 16].Value = _it.DaMat;
                    worksheet.Cells[idx, 17].Value = _it.SoMat;
                    worksheet.Cells[idx, 18].Value = _it.DaHuy;
                    worksheet.Cells[idx, 19].Value = _it.SoHuy;
                    worksheet.Cells[idx, 20].Value = _it.TonCuoiKyTu;
                    worksheet.Cells[idx, 21].Value = _it.TonCuoiKyDen;
                    worksheet.Cells[idx, 22].Value = _it.SoLuongTon;
                    idx += 1;
                    count++;
                }

                worksheet.Cells[idx + 12, 3].Value = baoCao.TenNguoiLap;
                worksheet.Cells[idx + 3, 17].Value = string.Format("Ngày {0} tháng {1} năm {2}", baoCao.NgayLap.Day, baoCao.NgayLap.Month, baoCao.NgayLap.Year);
                worksheet.Cells[idx + 12, 17].Value = baoCao.TenNguoiDaiDienPhapLuat;
                package.SaveAs(new FileInfo(excelPath));
            }

            return GetLinkFile(excelFileName);
        }

        public async Task<string> PrintChiTietBaoCaoTinhHinhSuDungHoaDonAsync(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao)
        {
            try
            {
                string filePath = await ExportExcelBaoCaoTinhHinhSuDungHoaDonAsync(baoCao);
                string fileName = Path.GetFileName(filePath);

                string uploadFolder = $"FilesUpload/excels/{fileName}";
                string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, uploadFolder);
                string pdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/pdf");
                if (!Directory.Exists(pdfFolder))
                {
                    Directory.CreateDirectory(pdfFolder);
                }
                string pdfFileName = $"BAO_CAO_TINH_HINH_SU_DUNG_HOA_DON-{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string pdfPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/pdf/{pdfFileName}");

                // Convert excel to pdf
                FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, excelPath, pdfPath);
                return GetLinkFilePDF(pdfFileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
