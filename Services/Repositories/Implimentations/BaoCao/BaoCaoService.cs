using AutoMapper;
using DLL;
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
                                                .GroupBy(x => new { x.LoaiHoaDon,x.TenMauSo, x.MauSo})
                                                .Select(x=>new SoLuongHoaDonDaPhatHanhViewModel
                                                {
                                                    TenLoaiHoaDon = x.First().LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG ? "Hóa đơn giá trị gia tăng" : "Hóa đơn bán hàng",
                                                    MauSo = x.First().TenMauSo,
                                                    KyHieu = x.First().MauSo,
                                                    TongSo = x.Count(),
                                                    DaSuDung = x.Count(o=>o.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo),
                                                    DaXoaBo = x.Count(o=>o.TrangThai == (int)TrangThaiHoaDon.HoaDonXoaBo)
                                                })
                                                .ToListAsync();
                @params.ListSoLuongHoaDonDaPhatHanhs = result;
                @params.FilePath = await ExportExcelThongKeSoLuongHoaDonDaPhatHanhAsync(@params);
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return result;
        }
    }
}
