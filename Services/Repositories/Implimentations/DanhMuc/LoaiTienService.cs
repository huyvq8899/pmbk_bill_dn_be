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
    public class LoaiTienService : ILoaiTienService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public LoaiTienService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public LoaiTienViewModel CheckMaOutObject(string ma, List<LoaiTien> models)
        {
            var model = models.FirstOrDefault(x => x.Ma.ToUpper() == ma.ToUpper());
            var result = _mp.Map<LoaiTienViewModel>(model);
            return result;
        }

        public async Task<bool> CheckTrungMaAsync(LoaiTienViewModel model)
        {
            bool result = await _db.LoaiTiens
                .AnyAsync(x => x.Ma.ToUpper().Trim() == model.Ma.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == id);
            _db.LoaiTiens.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportExcelAsync(LoaiTienParams @params)
        {
            @params.PageSize = -1;
            PagedList<LoaiTienViewModel> paged = await GetAllPagingAsync(@params);
            List<LoaiTienViewModel> list = paged.Items;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/LoaiTien/BANG_KE_LOAI_TIEN.xlsx";
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
                    worksheet.Cells[idx, 3].Value = _it.TyGiaQuyDoi;
                    worksheet.Cells[idx, 4].Value = _it.SapXep;
                    worksheet.Cells[idx, 5].Value = _it.Status == true ? string.Empty : "ü";

                    idx += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"BANG_KE_LOAI_TIEN_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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

        public async Task<List<LoaiTienViewModel>> GetAllAsync(LoaiTienParams @params = null)
        {
            var result = new List<LoaiTienViewModel>();

            var query = _db.LoaiTiens.AsQueryable();

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    string keyword = @params.Keyword.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) || x.Ma.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()) ||
                                            x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUpper().Contains(keyword.ToUpper()));
                }

                if (@params.IsActive.HasValue)
                {
                    query = query.Where(x => x.Status == @params.IsActive);
                }
            }

            result = await query
                .ProjectTo<LoaiTienViewModel>(_mp.ConfigurationProvider)
                .AsNoTracking()
                .OrderBy(x => x.SapXep).ThenBy(x => x.Ma)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<LoaiTienViewModel>> GetAllPagingAsync(LoaiTienParams @params)
        {
            var query = _db.LoaiTiens
                .OrderBy(x => x.SapXep).ThenBy(x => x.Ma)
                .Select(x => new LoaiTienViewModel
                {
                    LoaiTienId = x.LoaiTienId,
                    Ma = x.Ma ?? string.Empty,
                    Ten = x.Ten ?? string.Empty,
                    TyGiaQuyDoi = x.TyGiaQuyDoi,
                    SapXep = x.SapXep,
                    Status = x.Status
                });

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.Ma))
                {
                    var keyword = @params.Filter.Ma.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) || x.Ma.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<LoaiTienViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<LoaiTienViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == id);
            var result = _mp.Map<LoaiTienViewModel>(entity);
            return result;
        }

        public async Task<LoaiTienViewModel> InsertAsync(LoaiTienViewModel model)
        {
            var entity = _mp.Map<LoaiTien>(model);
            await _db.LoaiTiens.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<LoaiTienViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(LoaiTienViewModel model)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == model.LoaiTienId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
        public async Task<bool> UpdateRangeAsync(List<LoaiTienViewModel> models)
        {
            var entities = await _db.LoaiTiens
                .Where(x => models.Select(y => y.LoaiTienId).Contains(x.LoaiTienId))
                .ToListAsync();

            foreach (var item in entities)
            {
                var model = models.FirstOrDefault(x => x.LoaiTienId == item.LoaiTienId);
                item.SapXep = model.SapXep;
            }

            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

    }
}
