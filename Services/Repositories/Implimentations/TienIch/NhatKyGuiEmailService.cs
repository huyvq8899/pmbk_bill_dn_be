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
using DLL.Enums;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Newtonsoft.Json;
using Services.ViewModels.QuanLy;
using DLL.Entity.QuanLyHoaDon;
using Services.Helper.Params.Filter;

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
                    worksheet.Cells[idx, 3].Value = !string.IsNullOrEmpty(_it.So) ? _it.So : "<Chưa cấp số>";
                    worksheet.Cells[idx, 4].Value = _it.Ngay.Value.ToString("dd/MM/yyyy");
                    worksheet.Cells[idx, 5].Value = _it.TenTrangThaiGuiEmail;
                    worksheet.Cells[idx, 6].Value = _it.NguoiThucHien;
                    worksheet.Cells[idx, 7].Value = _it.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells[idx, 8].Value = _it.TenNguoiGui;
                    worksheet.Cells[idx, 9].Value = _it.EmailGui;
                    worksheet.Cells[idx, 10].Value = _it.TenNguoiNhan;
                    worksheet.Cells[idx, 11].Value = _it.EmailNguoiNhan;
                    worksheet.Cells[idx, 12].Value = _it.TenLoaiEmail;
                    worksheet.Cells[idx, 13].Value = _it.TieuDeEmail;
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
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    fileByte = File.ReadAllBytes(pdfPath);
                    filePath = pdfPath;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    fileByte = File.ReadAllBytes(filePath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
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
                        join hddtInSys in _db.HoaDonDienTus on nk.RefId equals hddtInSys.HoaDonDienTuId into tmpHDDTInSys
                        from hddtInSys in tmpHDDTInSys.DefaultIfEmpty()
                        join hddtOutSys in _db.ThongTinHoaDons on nk.RefId equals hddtOutSys.Id into tmpHDDTOutSys
                        from hddtOutSys in tmpHDDTOutSys.DefaultIfEmpty()
                        let soHoaDon = string.IsNullOrEmpty(nk.So) ? (hddtInSys != null ? (hddtInSys.SoHoaDon + "") : hddtOutSys.SoHoaDon) : nk.So
                        orderby nk.CreatedDate descending
                        select new NhatKyGuiEmailViewModel
                        {
                            NhatKyGuiEmailId = nk.NhatKyGuiEmailId,
                            MauSo = nk.MauSo,
                            KyHieu = nk.KyHieu,
                            StrKyHieu = nk.MauSo.CheckIsInteger() ? nk.MauSo + nk.KyHieu : nk.MauSo + " - " + nk.KyHieu,
                            So = string.IsNullOrEmpty(soHoaDon) ? "<Chưa cấp số>" : soHoaDon,
                            Ngay = nk.Ngay,
                            TrangThaiGuiEmail = nk.TrangThaiGuiEmail,
                            TenTrangThaiGuiEmail = (nk.LoaiEmail == LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon) ? nk.TrangThaiGuiEmail.GetDescription() : nk.TrangThaiGuiEmail.GetDescription(),
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
                            NguoiThucHien = u.UserName,
                            thongBaoSaiThongTin = _db.ThongBaoSaiThongTins.Where(t => t.NhatKyGuiEmailId == nk.NhatKyGuiEmailId).Select(tt => new ThongBaoSaiThongTin
                            {
                                Id = Guid.NewGuid().ToString(),
                                DoiTuongId = tt.DoiTuongId,
                                HoaDonDienTuId = tt.HoaDonDienTuId,
                                HoTenNguoiMuaHang_Sai = tt.HoTenNguoiMuaHang_Sai,
                                HoTenNguoiMuaHang_Dung = tt.HoTenNguoiMuaHang_Dung,
                                TenDonVi_Dung = tt.TenDonVi_Dung,
                                TenDonVi_Sai = tt.TenDonVi_Sai,
                                DiaChi_Dung = tt.DiaChi_Dung,
                                DiaChi_Sai = tt.DiaChi_Sai,
                                EmailCuaNguoiNhan = tt.EmailCuaNguoiNhan,
                                EmailBCCCuaNguoiNhan = tt.EmailBCCCuaNguoiNhan,
                                EmailCCCuaNguoiNhan = tt.EmailCCCuaNguoiNhan,
                                TenNguoiNhan = tt.TenNguoiNhan,
                                SDTCuaNguoiNhan = tt.SDTCuaNguoiNhan,
                                CreatedDate = tt.CreatedDate,
                                ModifyDate = tt.ModifyDate,
                            }).FirstOrDefault()
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
                if (!string.IsNullOrWhiteSpace(timKiemTheo.MauHoaDon))
                {
                    var keyword = timKiemTheo.MauHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.MauSo != null && x.MauSo.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.KyHieuHoaDon))
                {
                    var keyword = timKiemTheo.KyHieuHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu != null && x.KyHieu.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.SoHoaDon))
                {
                    var keyword = timKiemTheo.SoHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.So != null && x.So.ToUpper().ToString().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.NguoiThucHien))
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
                        (x.So != null && x.So.ToString().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.NguoiThucHien != null && x.NguoiThucHien.ToUpper().ToTrim().Contains(@params.TimKiemBatKy))
                    );
                }
            }

            if (@params.FilterColumns != null && @params.FilterColumns.Any())
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in @params.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        case nameof(@params.Filter.StrKyHieu):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.StrKyHieu, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.So):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.So, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.Ngay):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.Ngay, filterCol, FilterValueType.DateTime);
                            break;
                        case nameof(@params.Filter.TenTrangThaiGuiEmail):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.TenTrangThaiGuiEmail, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenLoaiEmail):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.TenLoaiEmail, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenNguoiGui):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.TenNguoiGui, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.CreatedDate):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.CreatedDate, filterCol, FilterValueType.DateTime);
                            break;
                        case nameof(@params.Filter.TenNguoiNhan):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.TenNguoiNhan, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.NguoiThucHien):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.NguoiThucHien, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.EmailGui):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.EmailGui, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.EmailNguoiNhan):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.EmailNguoiNhan, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TieuDeEmail):
                            query = GenericFilterColumn<NhatKyGuiEmailViewModel>.Query(query, x => x.TieuDeEmail, filterCol, FilterValueType.String);
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.StrKyHieu))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.StrKyHieu);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.StrKyHieu);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.So))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => long.Parse(x.So));
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => long.Parse(x.So));
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.StrKyHieu))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.StrKyHieu);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.StrKyHieu);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.So))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => long.Parse(x.So));
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => long.Parse(x.So));
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.Ngay))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ngay);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ngay);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.TenTrangThaiGuiEmail))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TrangThaiGuiEmail);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TrangThaiGuiEmail);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.NguoiThucHien))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NguoiThucHien);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NguoiThucHien);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.CreatedDate))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.CreatedDate);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.CreatedDate);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.TenNguoiGui))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenNguoiGui);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenNguoiGui);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.EmailGui))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.EmailGui);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.EmailGui);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.TenNguoiNhan))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenNguoiNhan);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenNguoiNhan);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.EmailNguoiNhan))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.EmailNguoiNhan);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.EmailNguoiNhan);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.TenLoaiEmail))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.LoaiEmail);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.LoaiEmail);
                    }
                }
                if (@params.SortKey == nameof(@params.Filter.TieuDeEmail))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TieuDeEmail);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TieuDeEmail);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            var paged = await PagedList<NhatKyGuiEmailViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);

            return paged;
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

        public async Task<bool> KiemTraDaGuiEmailChoKhachHangAsync(string hoaDonDienTuId, int type)
        {
            var query = await _db.NhatKyGuiEmails.CountAsync(x => x.RefId == hoaDonDienTuId && (x.LoaiEmail == (LoaiEmail)type || type == -100) && (x.TrangThaiGuiEmail == TrangThaiGuiEmail.DaGui || x.TrangThaiGuiEmail == TrangThaiGuiEmail.KhachHangDaNhan));

            return query > 0;
        }
        public async Task<NhatKyGuiEmailViewModel> GetNhatKyGuiEmailByHoaDonDienTuIdAsync(string hoaDonDienTuId, int type)
        {
            var query = _db.NhatKyGuiEmails.Where(x => x.NhatKyGuiEmailId == hoaDonDienTuId && (x.LoaiEmail == (LoaiEmail)type || type == -100) && (x.TrangThaiGuiEmail == TrangThaiGuiEmail.DaGui || x.TrangThaiGuiEmail == TrangThaiGuiEmail.KhachHangDaNhan)).OrderByDescending(x => x.ModifyDate)
                .Select(nk => new NhatKyGuiEmailViewModel
                {
                    NhatKyGuiEmailId = nk.NhatKyGuiEmailId,
                    MauSo = nk.MauSo,
                    KyHieu = nk.KyHieu,
                    StrKyHieu = nk.MauSo.CheckIsInteger() ? nk.MauSo + nk.KyHieu : nk.MauSo + " - " + nk.KyHieu,
                    So = nk.So,
                    Ngay = nk.Ngay,
                    TrangThaiGuiEmail = nk.TrangThaiGuiEmail,
                    TenTrangThaiGuiEmail = (nk.LoaiEmail == DLL.Enums.LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon) ? nk.TrangThaiGuiEmail.GetDescription() : nk.TrangThaiGuiEmail.GetDescription(),
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
                    thongBaoSaiThongTin = _db.ThongBaoSaiThongTins.Where(t => t.NhatKyGuiEmailId == nk.NhatKyGuiEmailId).Select(tt => new ThongBaoSaiThongTin
                    {
                        Id = Guid.NewGuid().ToString(),
                        DoiTuongId = tt.DoiTuongId,
                        HoaDonDienTuId = tt.HoaDonDienTuId,
                        HoTenNguoiMuaHang_Sai = tt.HoTenNguoiMuaHang_Sai,
                        HoTenNguoiMuaHang_Dung = tt.HoTenNguoiMuaHang_Dung,
                        TenDonVi_Dung = tt.TenDonVi_Dung,
                        TenDonVi_Sai = tt.TenDonVi_Sai,
                        DiaChi_Dung = tt.DiaChi_Dung,
                        DiaChi_Sai = tt.DiaChi_Sai,
                        EmailCuaNguoiNhan = tt.EmailCuaNguoiNhan,
                        EmailBCCCuaNguoiNhan = tt.EmailBCCCuaNguoiNhan,
                        EmailCCCuaNguoiNhan = tt.EmailCCCuaNguoiNhan,
                        TenNguoiNhan = tt.TenNguoiNhan,
                        SDTCuaNguoiNhan = tt.SDTCuaNguoiNhan,
                        CreatedDate = tt.CreatedDate,
                        ModifyDate = tt.ModifyDate,
                    }).FirstOrDefault()
                }).FirstOrDefault();

            return query;
        }

        public async Task<HoaDonDienTuViewModel> GetThongTinById(string Id)
        {
            var bbdc = _db.BienBanDieuChinhs.Where(o => o.HoaDonBiDieuChinhId == Id).FirstOrDefault();
            return await _db.ThongTinHoaDons.Where(x => x.Id == Id)
                                            .Select(x => new HoaDonDienTuViewModel
                                            {
                                                HoaDonDienTuId = x.Id,
                                                LoaiHoaDon = x.LoaiHoaDon,
                                                MauSo = x.MauSoHoaDon,
                                                KyHieu = x.KyHieuHoaDon,
                                                MaCuaCQT = x.MaCQTCap,
                                                MaTraCuu = x.MaTraCuu,
                                                NgayHoaDon = x.NgayHoaDon,
                                                StrSoHoaDon = x.SoHoaDon,
                                                TrangThai = x.TrangThaiHoaDon,
                                                TrangThaiBienBanXoaBo = x.TrangThaiBienBanXoaBo,
                                                LoaiApDungHoaDonDieuChinh = x.HinhThucApDung,
                                                LoaiApDungHoaDonCanThayThe = x.HinhThucApDung,
                                                BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : null,
                                                TrangThaiBienBanDieuChinh = bbdc != null ? bbdc.TrangThaiBienBan : (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan,
                                                LoaiTienId = x.LoaiTienId,
                                                LoaiTien = _mp.Map<LoaiTienViewModel>(_db.LoaiTiens.FirstOrDefault(y => y.LoaiTienId == x.LoaiTienId))
                                            })
                                            .FirstOrDefaultAsync();
        }

        public async Task<HoaDonDienTuViewModel> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || id == "null" || id == "undefined") return null;

            var tuyChonKyKeKhai = (await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _db.HoaDonDienTus
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

            //đọc ra thông tin hóa đơn được nhập từ phần mềm khác, (được dùng để hiển thị cột thông tin sai sót ở hóa đơn điều chỉnh); việc đọc ra bảng này vì phải truy vấn thông tin với các hóa đơn được nhập từ phần mềm khác
            List<ThongTinHoaDon> listThongTinHoaDon = await (from hoaDon in _db.ThongTinHoaDons
                                                             where listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.Id) > 0
                                                             select new ThongTinHoaDon
                                                             {
                                                                 Id = hoaDon.Id,
                                                                 TrangThaiHoaDon = hoaDon.TrangThaiHoaDon,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04
                                                             }).ToListAsync();


            var query = from hd in _db.HoaDonDienTus
                        join bkhhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
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
                        where hd.HoaDonDienTuId == id
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
                                TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL
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
                            SoLanChuyenDoi = hd.SoLanChuyenDoi ?? 0,
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
                            LyDoBiDieuChinh = bbdc != null ? bbdc.LyDoDieuChinh : null,
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
                            //LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
                            GhiChuThayTheSaiSot = hd.GhiChuThayTheSaiSot,
                            HoaDonChiTiets = (
                                               from hdct in _db.HoaDonDienTuChiTiets
                                               join hd in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                               from hd in tmpHoaDons.DefaultIfEmpty()
                                               join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                               from vt in tmpHangHoas.DefaultIfEmpty()
                                               join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                               from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                               where hdct.HoaDonDienTuId == id
                                               orderby hdct.CreatedDate
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   STT = hdct.STT,
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   HangHoaDichVu = new HangHoaDichVuViewModel
                                                   {
                                                       HangHoaDichVuId = vt.HangHoaDichVuId,
                                                       Ma = vt.Ma,
                                                       Ten = vt.Ten,
                                                       DonGiaBan = vt.DonGiaBan,
                                                       ThueGTGT = vt.ThueGTGT,
                                                       IsGiaBanLaDonGiaSauThue = vt.IsGiaBanLaDonGiaSauThue,
                                                       TyLeChietKhau = vt.TyLeChietKhau
                                                   },
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
                            IsThongTinNguoiBanHoacNguoiMua = hd.IsThongTinNguoiBanHoacNguoiMua,
                            IsTheHienLyDoTrenHoaDon = hd.IsTheHienLyDoTrenHoaDon,
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
            if (result == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(result.LyDoDieuChinh) && result.LyDoDieuChinh.StartsWith("{"))
            {
                result.LyDoDieuChinhModel = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(result.LyDoDieuChinh);
            }
            else
            {
                if (string.IsNullOrEmpty(result.LyDoDieuChinh))
                {
                    if (!string.IsNullOrEmpty(result.BienBanDieuChinhId))
                    {
                        var bbdc = _db.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == result.BienBanDieuChinhId);
                        result.LyDoDieuChinhModel = new LyDoDieuChinhModel { LyDo = bbdc.LyDoDieuChinh };
                    }
                    else result.LyDoDieuChinhModel = null;
                }
                else result.LyDoDieuChinhModel = new LyDoDieuChinhModel { LyDo = result.LyDoDieuChinh };
            }

            if (result.LyDoDieuChinhModel != null)
            {
                result.LyDoDieuChinhModel.DieuChinhChoHoaDonId = result.DieuChinhChoHoaDonId;
            }
            result.TenTrangThaiLanDieuChinhGanNhat = result.TrangThaiLanDieuChinhGanNhat.HasValue ? ((TrangThaiQuyTrinh)result.TrangThaiLanDieuChinhGanNhat.Value).GetDescription() : string.Empty;
            return result;
        }

        public async Task<NhatKyGuiEmailViewModel> GetThongTinEmailDaGuiChoKHGanNhatAsync()
        {
            var entity = await _db.NhatKyGuiEmails
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync(x => x.TrangThaiGuiEmail == TrangThaiGuiEmail.DaGui);

            var result = _mp.Map<NhatKyGuiEmailViewModel>(entity);
            return result;
        }
    }
}
