﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class HangHoaDichVuService : IHangHoaDichVuService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public HangHoaDichVuService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _IHttpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> CheckTrungMaAsync(HangHoaDichVuViewModel model)
        {
            bool result = await _db.HangHoaDichVus
                .AnyAsync(x => x.Ma.ToUpper().Trim() == model.Ma.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.HangHoaDichVus.FirstOrDefaultAsync(x => x.HangHoaDichVuId == id);
            _db.HangHoaDichVus.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportExcelAsync(HangHoaDichVuParams @params)
        {
            @params.PageSize = -1;
            PagedList<HangHoaDichVuViewModel> paged = await GetAllPagingAsync(@params);
            List<HangHoaDichVuViewModel> list = paged.Items;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/HangHoaDichVu/BANG_KE_HANG_HOA_DICH_VU.xlsx";
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
                    worksheet.Cells[idx, 1].Value = _it.Ma;
                    worksheet.Cells[idx, 2].Value = _it.Ten;
                    worksheet.Cells[idx, 3].Value = _it.TenDonViTinh;
                    worksheet.Cells[idx, 4].Value = _it.DonGiaBan;
                    worksheet.Cells[idx, 5].Value = _it.TenThueGTGT;
                    worksheet.Cells[idx, 6].Value = _it.TyLeChietKhau;
                    worksheet.Cells[idx, 7].Value = _it.Status == true ? string.Empty : "ü";

                    idx += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"BANG_KE_HANG_HOA_DICH_VU_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string filePath = Path.Combine(destPath, fileName);
                package.SaveAs(new FileInfo(filePath));
                byte[] fileByte = File.ReadAllBytes(filePath);
                File.Delete(filePath);

                return new FileReturn
                {
                    Bytes = fileByte,
                    ContentType = MimeTypes.GetMimeType(filePath),
                    FileName = Path.GetFileName(filePath)
                };
            }
        }

        public async Task<List<HangHoaDichVuViewModel>> GetAllAsync(HangHoaDichVuParams @params = null)
        {
            var result = new List<HangHoaDichVuViewModel>();

            var query = _db.HangHoaDichVus.AsQueryable();

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    string keyword = @params.Keyword.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) || x.Ma.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()) ||
                                            x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUpper().Contains(keyword.ToUpper()));
                }
            }

            result = await query
                .ProjectTo<HangHoaDichVuViewModel>(_mp.ConfigurationProvider)
                .AsNoTracking()
                .OrderBy(x => x.Ma)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<HangHoaDichVuViewModel>> GetAllPagingAsync(HangHoaDichVuParams @params)
        {
            var query = from hhdv in _db.HangHoaDichVus
                        join dvt in _db.DonViTinhs on hhdv.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        orderby hhdv.Ma
                        select new HangHoaDichVuViewModel
                        {
                            HangHoaDichVuId = hhdv.HangHoaDichVuId,
                            Ma = hhdv.Ma ?? string.Empty,
                            Ten = hhdv.Ten ?? string.Empty,
                            DonGiaBan = hhdv.DonGiaBan,
                            IsGiaBanLaDonGiaSauThue = hhdv.IsGiaBanLaDonGiaSauThue,
                            ThueGTGT = hhdv.ThueGTGT,
                            TenThueGTGT = hhdv.ThueGTGT,
                            ThueGTGTDisplay = hhdv.ThueGTGT.GetThueHasPer(),
                            TyLeChietKhau = hhdv.TyLeChietKhau,
                            MoTa = hhdv.MoTa,
                            TenDonViTinh = dvt != null ? dvt.Ten : string.Empty,
                            Status = hhdv.Status
                        };

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.Ma))
                {
                    var keyword = @params.Filter.Ma.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.TenDonViTinh))
                {
                    var keyword = @params.Filter.TenDonViTinh.ToUpper().ToTrim();
                    query = query.Where(x => x.TenDonViTinh.ToUpper().ToTrim().Contains(keyword) || x.TenDonViTinh.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (@params.Filter.DonGiaBan.HasValue)
                {
                    var keyword = @params.Filter.DonGiaBan.ToString().ToTrim();
                    query = query.Where(x => x.DonGiaBan.ToString().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.TenThueGTGT))
                {
                    var keyword = @params.Filter.TenThueGTGT.ToUpper().ToTrim();
                    query = query.Where(x => x.TenThueGTGT.ToUpper().ToTrim().Contains(keyword));
                }
                if (@params.Filter.TyLeChietKhau.HasValue)
                {
                    var keyword = @params.Filter.TyLeChietKhau.ToString().ToTrim();
                    query = query.Where(x => x.TyLeChietKhau.ToString().ToTrim().Contains(keyword));
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.Ma))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ma);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ma);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.Ten))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ten);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ten);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TenDonViTinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenDonViTinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenDonViTinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.DonGiaBan))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.DonGiaBan);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.DonGiaBan);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TenThueGTGT))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenThueGTGT);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenThueGTGT);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TyLeChietKhau))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TyLeChietKhau);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TyLeChietKhau);
                    }
                }
            }

            if (!string.IsNullOrEmpty(@params.Keyword)) //trường hợp tìm kiếm mã và tên trên combobox
            {
                query = query.Where(x => x.Ma.Trim().ToUpper().Contains(@params.Keyword.Trim().ToUpper()) ||
                                       x.Ten.Trim().ToUpper().Contains(@params.Keyword.Trim().ToUpper()));
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<HangHoaDichVuViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<HangHoaDichVuViewModel> GetByIdAsync(string id)
        {
            var query = from hhdv in _db.HangHoaDichVus
                        join dvt in _db.DonViTinhs on hhdv.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        where hhdv.HangHoaDichVuId == id
                        select new HangHoaDichVuViewModel
                        {
                            HangHoaDichVuId = hhdv.HangHoaDichVuId,
                            Ma = hhdv.Ma,
                            Ten = hhdv.Ten,
                            DonGiaBan = hhdv.DonGiaBan,
                            IsGiaBanLaDonGiaSauThue = hhdv.IsGiaBanLaDonGiaSauThue,
                            ThueGTGT = hhdv.ThueGTGT,
                            TenThueGTGT = hhdv.ThueGTGT.GetDescription(),
                            ThueGTGTDisplay = hhdv.ThueGTGT.GetThueHasPer(),
                            TyLeChietKhau = hhdv.TyLeChietKhau,
                            MoTa = hhdv.MoTa,
                            DonViTinhId = hhdv.DonViTinhId,
                            TenDonViTinh = dvt != null ? dvt.Ten : string.Empty,
                            CreatedBy = hhdv.CreatedBy,
                            CreatedDate = hhdv.CreatedDate,
                            Status = hhdv.Status
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<HangHoaDichVuViewModel> InsertAsync(HangHoaDichVuViewModel model)
        {
            var entity = _mp.Map<HangHoaDichVu>(model);
            await _db.HangHoaDichVus.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<HangHoaDichVuViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(HangHoaDichVuViewModel model)
        {
            var entity = await _db.HangHoaDichVus.FirstOrDefaultAsync(x => x.HangHoaDichVuId == model.HangHoaDichVuId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<HangHoaDichVuViewModel>> ImportVTHH(IList<IFormFile> files, int modeValue)
        {
            var formFile = files[0];
            var list = new List<HangHoaDichVuViewModel>();
            var listVTHH = await _db.HangHoaDichVus.ToListAsync();
            var listDVT = await _db.DonViTinhs.ToListAsync();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        HangHoaDichVuViewModel item = new HangHoaDichVuViewModel();
                        item.Row = row - 1;

                        #region Thông tin chung
                        // Mã vật tư, hàng hóa
                        item.Ma = worksheet.Cells[row, 1].Value == null ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
                        if (item.Ma != "")
                        {
                            var existed = list.FirstOrDefault(x => x.Ma == item.Ma);
                            if (existed != null && !existed.HasError)
                            {
                                item.ErrorMessage = $"<Mã hàng> trùng với (dòng số <{existed.Row}>)";
                                item.HasError = true;
                            }
                        }
                        if (item.ErrorMessage == null)
                        {
                            if (item.Ma == "")
                            {
                                item.ErrorMessage = "<Mã hàng> không được để trống";
                                item.HasError = true;
                            }
                            else if (await CheckTrungMaAsync(item))
                            {
                                if (modeValue == 1)
                                {
                                    item.ErrorMessage = "<Mã hàng> đã tồn tại";
                                    item.HasError = true;
                                }
                                else
                                {
                                    item.Existed = true;
                                }
                            }
                        }

                        // Tên vật tư, hàng hóa
                        item.Ten = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.Ten == "")
                            {
                                item.ErrorMessage = "<Tên hàng> không được để trống";
                                item.HasError = true;
                            }
                        }

                        // Đơn vị tính
                        item.TenDonViTinh = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
                        var donViTinh = listDVT.FirstOrDefault(x => string.Compare(x.Ten.ToUpper().Trim(), item.TenDonViTinh.ToUpper().Trim()) == 0);
                        if (item.TenDonViTinh != "" && donViTinh != null)
                        {
                            item.DonViTinhId = donViTinh.DonViTinhId;
                        }
                        if (item.ErrorMessage == null)
                        {
                            if (item.TenDonViTinh != "" && item.DonViTinhId == null)
                            {
                                item.ErrorMessage = "<Đơn vị tính> chưa tồn tại trong hệ thống";
                                item.HasError = true;
                            }
                        }
                        // Mô tả
                        item.MoTa = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim();
                        // Hạn bảo hành

                        #endregion
                        #region Ngầm định
                        // Kho ngầm định
                        // Đơn giá mua cố định
                        item.DonGiaBanText = worksheet.Cells[row, 4].Value == null ? "0" : worksheet.Cells[row, 4].Value.ToString().Trim();
                        if (item.DonGiaBanText.IsValidCurrency() == false)
                        {
                            if (item.ErrorMessage == null)
                            {
                                item.ErrorMessage = "<Giá bán> sai định dạng";
                                item.HasError = true;
                            }
                        }
                        else
                        {
                            item.DonGiaBan = decimal.Parse(item.DonGiaBanText);
                        }

                        // Đơn giá mua gần nhất
                        var laDonGiaSauThue = worksheet.Cells[row, 5].Value;
                        if (laDonGiaSauThue == null) laDonGiaSauThue = "";
                        item.IsGiaBanLaDonGiaSauThueText = laDonGiaSauThue.ToString();
                        if (string.IsNullOrWhiteSpace(laDonGiaSauThue.ToString()))
                        {
                            item.IsGiaBanLaDonGiaSauThue = false;
                        }
                        else
                        {
                            if (laDonGiaSauThue.ToString() != "0" && laDonGiaSauThue.ToString() != "1")
                            {
                                if (item.ErrorMessage == null)
                                {
                                    item.ErrorMessage = "<Giá bán là đơn giá sau thuế> không hợp lệ";
                                    item.HasError = true;
                                }
                            }
                            else
                            {
                                item.IsGiaBanLaDonGiaSauThue = laDonGiaSauThue.ToString() != "0";
                            }
                        }


                        // Thuế suất GTGT(%)
                        item.ThueGTGTText = worksheet.Cells[row, 6].Value == null ? "0" : worksheet.Cells[row, 6].Value.ToString().Trim();
                        if (item.ThueGTGTText != "0" && item.ThueGTGTText != "5" && item.ThueGTGTText != "10" &&
                            item.ThueGTGTText != "KCT" && item.ThueGTGTText != "KKKNT" && item.ThueGTGTText != "KHAC")
                        {
                            if (item.ErrorMessage == null)
                            {
                                item.ErrorMessage = "<Thuế GTGT (%)> không hợp lệ";
                                item.HasError = true;
                            }
                        }
                        else
                        {
                            item.ThueGTGT = item.ThueGTGTText;
                        }
                        #endregion

                        #region Chiết khấu
                        // Chiết khấu theo phần trăm
                        item.TyLeChietKhauText = worksheet.Cells[row, 7].Value == null ? "0" : worksheet.Cells[row, 7].Value.ToString().Trim();
                        if (item.TyLeChietKhauText.IsValidCurrency() == false)
                        {
                            if (item.ErrorMessage == null)
                            {
                                item.ErrorMessage = "<Tỷ lệ chiết khấu (%)> sai định dạng";
                                item.HasError = true;
                            }
                        }
                        else
                        {
                            item.TyLeChietKhau = decimal.Parse(item.TyLeChietKhauText);
                        }

                        #endregion

                        // success
                        if (item.HasError == false)
                        {
                            item.ErrorMessage = "<Hợp lệ>";
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public async Task<List<HangHoaDichVuViewModel>> ConvertImport(List<HangHoaDichVuViewModel> model)
        {
            List<HangHoaDichVuViewModel> listData = new List<HangHoaDichVuViewModel>();
            foreach (var item in model)
            {
                if (!item.Existed)
                {
                    var vthh = new HangHoaDichVuViewModel
                    {
                        Ma = item.Ma,
                        Ten = item.Ten,
                        DonViTinhId = item.DonViTinhId,
                        MoTa = item.MoTa,
                        DonGiaBan = item.DonGiaBan,
                        ThueGTGT = item.ThueGTGT,
                        IsGiaBanLaDonGiaSauThue = item.IsGiaBanLaDonGiaSauThue,
                        // Chiết khấu
                        TyLeChietKhau = item.TyLeChietKhau
                    };

                    listData.Add(vthh);
                }
                else
                {
                    var vthh = _mp.Map<HangHoaDichVuViewModel>(await _db.HangHoaDichVus.FirstOrDefaultAsync(x => x.Ma == item.Ma));
                    vthh.Ma = item.Ma;
                    vthh.Ten = item.Ten;
                    vthh.DonViTinhId = item.DonViTinhId;
                    vthh.MoTa = item.MoTa;
                    vthh.DonGiaBan = item.DonGiaBan;
                    vthh.ThueGTGT = item.ThueGTGT;
                    vthh.IsGiaBanLaDonGiaSauThue = item.IsGiaBanLaDonGiaSauThue;
                    // Chiết khấu
                    vthh.TyLeChietKhau = item.TyLeChietKhau;

                    listData.Add(vthh);
                }
            }
            return listData;
        }

        public string CreateFileImportVTHHError(List<HangHoaDichVuViewModel> list)
        {
            // Export excel
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "FilesUpload/excels");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            else
            {
                FileHelper.ClearFolder(uploadFolder);
            }

            string excelFileName = $"vat-tu-hang-hoa-error-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            string excelFolder = $"FilesUpload/excels/{excelFileName}";
            string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

            // Excel
            string _sample = $"Template/Danh_Muc_Hang_Hoa_Dich_Vu_Import.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int begin_row = 2;
                int i = begin_row;
                foreach (var item in list)
                {
                    worksheet.Cells[i, 1].Value = item.Ma;
                    worksheet.Cells[i, 2].Value = item.Ten;
                    worksheet.Cells[i, 3].Value = item.TenDonViTinh;
                    worksheet.Cells[i, 4].Value = item.DonGiaBan;
                    worksheet.Cells[i, 5].Value = item.IsGiaBanLaDonGiaSauThue;
                    worksheet.Cells[i, 6].Value = item.ThueGTGT.GetDescription();
                    worksheet.Cells[i, 7].Value = item.TyLeChietKhau;
                    worksheet.Cells[i, 8].Value = item.MoTa;
                    worksheet.Cells[i, 9].Value = item.ErrorMessage;
                    worksheet.Cells[i, 9].Style.Font.Color.SetColor(Color.Red);
                    i += 1;
                }
                package.SaveAs(new FileInfo(excelPath));
            }

            return this.GetLinkFileExcel(excelFileName);
        }

        public string GetLinkFileExcel(string link)
        {
            var filename = "FilesUpload/excels/" + link;
            string url;
            if (_IHttpContextAccessor.HttpContext.Request.IsHttps)
            {
                url = "https://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            else
            {
                url = "http://" + _IHttpContextAccessor.HttpContext.Request.Host;
            }
            url = url + "/" + filename;
            return url;
        }

        public HangHoaDichVuViewModel CheckMaOutObject(string ma, List<HangHoaDichVu> models)
        {
            if (string.IsNullOrEmpty(ma))
            {
                return null;
            }

            var model = models.FirstOrDefault(x => x.Ma.ToUpper() == ma.ToUpper());
            var result = _mp.Map<HangHoaDichVuViewModel>(model);
            return result;
        }
    }
}
