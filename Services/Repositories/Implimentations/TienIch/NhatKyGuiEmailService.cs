using AutoMapper;
using DLL;
using ManagementServices.Helper;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using DLL.Entity.TienIch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Repositories.Interfaces.DanhMuc;
using System.Collections.Generic;
using Services.ViewModels.DanhMuc;
using System.IO;
using OfficeOpenXml;
using MimeKit;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyGuiEmailService : INhatKyGuiEmailService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public NhatKyGuiEmailService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<FileReturn> ExportExcelAsync(NhatKyGuiEmailParams @params)
        {
            @params.PageSize = -1;
            PagedList<NhatKyGuiEmailViewModel> paged = await GetAllPagingAsync(@params);
            List<NhatKyGuiEmailViewModel> list = paged.Items;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/NhatKyGuiEmail/DANH_SACH_NHAT_KY_GUI_EMAIL.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = list.Count;
                int begin_row = 7;

                worksheet.Cells[1, 1].Value = hoSoHDDTVM.TenDonVi;
                worksheet.Cells[2, 1].Value = hoSoHDDTVM.DiaChi;
                // Add Row
                if (totalRows != 0)
                {
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                }
                // Fill data
                int idx = begin_row + (totalRows == 0 ? 1 : 0);
                foreach (var _it in list)
                {
                    worksheet.Cells[idx, 1].Value = _it.MauSo;
                    worksheet.Cells[idx, 2].Value = _it.KyHieu;
                    worksheet.Cells[idx, 3].Value = _it.So;
                    worksheet.Cells[idx, 4].Value = _it.Ngay;
                    worksheet.Cells[idx, 5].Value = _it.TenTrangThaiGuiEmail;
                    worksheet.Cells[idx, 6].Value = _it.CreatedDate.Value.ToString("dd/MM/yyyy");
                    worksheet.Cells[idx, 7].Value = _it.TenNguoiGui;
                    worksheet.Cells[idx, 8].Value = _it.EmailGui;
                    worksheet.Cells[idx, 9].Value = _it.TenNguoiNhan;
                    worksheet.Cells[idx, 10].Value = _it.EmailNguoiNhan;
                    worksheet.Cells[idx, 11].Value = _it.TenLoaiEmail;
                    worksheet.Cells[idx, 12].Value = _it.TieuDeEmail;

                    idx += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"DANH_SACH_NHAT_KY_GUI_EMAIL_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string filePath = Path.Combine(destPath, fileName);
                package.SaveAs(new FileInfo(filePath));

                byte[] fileByte;

                if (@params.IsExportPDF == true)
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

        public async Task<PagedList<NhatKyGuiEmailViewModel>> GetAllPagingAsync(NhatKyGuiEmailParams @params)
        {
            var query = from nk in _db.NhatKyGuiEmails
                        join u in _db.Users on nk.CreatedBy equals u.UserId
                        orderby nk.CreatedDate descending
                        select new NhatKyGuiEmailViewModel
                        {
                            NhatKyGuiEmailId = nk.NhatKyGuiEmailId,
                            MauSo = nk.MauSo,
                            KyHieu = nk.KyHieu,
                            So = nk.So,
                            Ngay = nk.Ngay,
                            TrangThaiGuiEmail = nk.TrangThaiGuiEmail,
                            TenTrangThaiGuiEmail = (nk.LoaiEmail == DLL.Enums.LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon)? ((DLL.Enums.TrangThaiGuiEmailV2)nk.TrangThaiGuiEmail).GetDescription(): nk.TrangThaiGuiEmail.GetDescription(),
                            TenNguoiGui = nk.TenNguoiGui,
                            EmailGui = nk.EmailGui,
                            TenNguoiNhan = nk.TenNguoiNhan,
                            EmailNguoiNhan = nk.EmailNguoiNhan,
                            LoaiEmail = nk.LoaiEmail,
                            TenLoaiEmail = nk.LoaiEmail.GetDescription(),
                            TieuDeEmail = nk.TieuDeEmail,
                            RefId = nk.RefId,
                            RefType = nk.RefType,
                            CreatedDate = nk.CreatedDate,
                            CreatedBy = nk.CreatedBy,
                            Status = nk.Status,
                            NguoiThucHien = u.UserName
                        };

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                var fromDate = DateTime.Parse(@params.FromDate);
                var toDate = DateTime.Parse(@params.ToDate);
                query = query.Where(x => x.CreatedDate.Value.Date >= fromDate.Date && x.CreatedDate.Value.Date <= toDate.Date);
            }

            if (@params.LoaiEmail != -1)
            {
                query = query.Where(x => x.LoaiEmail == (DLL.Enums.LoaiEmail)@params.LoaiEmail);
            }

            if (@params.TrangThaiGuiEmail != -1)
            {
                query = query.Where(x => x.TrangThaiGuiEmail == (DLL.Enums.TrangThaiGuiEmail)@params.TrangThaiGuiEmail);
            }

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.MauHoaDon))
                {
                    var keyword = timKiemTheo.MauHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieuHoaDon))
                {
                    var keyword = timKiemTheo.KyHieuHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.So != null && x.So.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NguoiThucHien))
                {
                    var keyword = timKiemTheo.NguoiThucHien.ToUpper().ToTrim();
                    query = query.Where(x => x.NguoiThucHien != null && x.NguoiThucHien.ToUpper().ToTrim().Contains(keyword));
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
                        (x.So != null && x.So.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) || 
                        (x.NguoiThucHien != null && x.NguoiThucHien.ToUpper().ToTrim().Contains(@params.TimKiemBatKy))
                    );
                }
            }

            if (@params.Filter != null)
            {
                //if (!string.IsNullOrEmpty(@params.Filter.))
                //{
                //    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                //    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                //}
                //if (!string.IsNullOrEmpty(@params.Filter.MoTa))
                //{
                //    var keyword = @params.Filter.MoTa.ToUpper().ToTrim();
                //    query = query.Where(x => x.MoTa.ToUpper().ToTrim().Contains(keyword) || x.MoTa.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                //}
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                //if (@params.SortKey == nameof(@params.Filter.Ten))
                //{
                //    if (@params.SortValue == "ascend")
                //    {
                //        query = query.OrderBy(x => x.Ten);
                //    }
                //    if (@params.SortValue == "descend")
                //    {
                //        query = query.OrderByDescending(x => x.Ten);
                //    }
                //}
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<NhatKyGuiEmailViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<bool> InsertAsync(NhatKyGuiEmailViewModel model)
        {
            var _configuration = await _db.TuyChons.AsNoTracking().ToListAsync();
            string fromMail = _configuration.Where(x => x.Ma == "TenDangNhapEmail").Select(x => x.GiaTri).FirstOrDefault();
            string fromName = _configuration.Where(x => x.Ma == "TenNguoiGui").Select(x => x.GiaTri).FirstOrDefault();

            model.EmailGui = fromMail;
            model.TenNguoiGui = fromName;
            model.Status = true;
            var entity = _mp.Map<NhatKyGuiEmail>(model);
            await _db.NhatKyGuiEmails.AddAsync(entity);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> KiemTraDaGuiEmailChoKhachHangAsync(string hoaDonDienTuId)
        {
            var query = await _db.NhatKyGuiEmails.CountAsync(x => x.RefId == hoaDonDienTuId && x.TrangThaiGuiEmail == DLL.Enums.TrangThaiGuiEmail.DaGui);

            return query > 0;
        }
    }
}
