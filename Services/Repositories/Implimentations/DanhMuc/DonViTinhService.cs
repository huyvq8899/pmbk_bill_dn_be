using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class DonViTinhService : IDonViTinhService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public DonViTinhService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> CheckTrungMaAsync(DonViTinhViewModel model)
        {
            bool result = await _db.DonViTinhs
                .AnyAsync(x => x.Ten.ToUpper().Trim() == model.Ten.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.DonViTinhs.FirstOrDefaultAsync(x => x.DonViTinhId == id);
            _db.DonViTinhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportExcelAsync(DonViTinhParams @params)
        {
            @params.PageSize = -1;
            PagedList<DonViTinhViewModel> paged = await GetAllPagingAsync(@params);
            List<DonViTinhViewModel> list = paged.Items;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/DonViTinh/BANG_KE_DON_VI_TINH.xlsx";
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
                    worksheet.Cells[idx, 1].Value = _it.Ten;
                    worksheet.Cells[idx, 2].Value = _it.MoTa;
                    worksheet.Cells[idx, 3].Value = _it.Status == true ? string.Empty : "ü";

                    idx += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"BANG_KE_DON_VI_TINH_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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

        public async Task<List<DonViTinhViewModel>> GetAllAsync(DonViTinhParams @params = null)
        {
            var result = new List<DonViTinhViewModel>();
            try
            {
                var query = _db.DonViTinhs.AsQueryable();

                if (@params != null)
                {
                    if (!string.IsNullOrEmpty(@params.Keyword))
                    {
                        string keyword = @params.Keyword.ToUpper().ToTrim();
                        query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()) ||
                                                x.MoTa.ToUpper().ToTrim().Contains(keyword) || x.MoTa.ToUpper().ToTrim().ToUpper().Contains(keyword.ToUpper()));
                    }

                    if (@params.IsActive.HasValue)
                    {
                        query = query.Where(x => x.Status == @params.IsActive);
                    }
                }

                result = await query
                    .ProjectTo<DonViTinhViewModel>(_mp.ConfigurationProvider)
                    .AsNoTracking()
                    .OrderBy(x => x.Ten)
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return result;
        }

        public async Task<PagedList<DonViTinhViewModel>> GetAllPagingAsync(DonViTinhParams @params)
        {
            var query = _db.DonViTinhs
                .OrderBy(x => x.Ten)
                .Select(x => new DonViTinhViewModel
                {
                    DonViTinhId = x.DonViTinhId,
                    Ten = x.Ten ?? string.Empty,
                    MoTa = x.MoTa ?? string.Empty,
                    Status = true
                });

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.MoTa))
                {
                    var keyword = @params.Filter.MoTa.ToUpper().ToTrim();
                    query = query.Where(x => x.MoTa.ToUpper().ToTrim().Contains(keyword) || x.MoTa.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
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

                if (@params.SortKey == nameof(@params.Filter.MoTa))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MoTa);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MoTa);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<DonViTinhViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<DonViTinhViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.DonViTinhs.AsNoTracking().FirstOrDefaultAsync(x => x.DonViTinhId == id);
            var result = _mp.Map<DonViTinhViewModel>(entity);
            return result;
        }

        public async Task<DonViTinhViewModel> InsertAsync(DonViTinhViewModel model)
        {
            var entity = _mp.Map<DonViTinh>(model);
            await _db.DonViTinhs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<DonViTinhViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(DonViTinhViewModel model)
        {
            var entity = await _db.DonViTinhs.FirstOrDefaultAsync(x => x.DonViTinhId == model.DonViTinhId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
