using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Services.Helper;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class LoaiTienService : ILoaiTienService
    {
        Datacontext _db;
        IMapper _mp;
        IHostingEnvironment _hostingEnvironment;
        IHttpContextAccessor _accessor;

        public LoaiTienService(Datacontext datacontext, IMapper mapper, IHostingEnvironment IHostingEnvironment,
            IHttpContextAccessor IHttpContextAccessor)
        {
            this._db = datacontext;
            this._mp = mapper;
            _hostingEnvironment = IHostingEnvironment;
            _accessor = IHttpContextAccessor;
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == id);
            _db.LoaiTiens.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<LoaiTienViewModel>> GetAll()
        {
            var entity = await _db.LoaiTiens.OrderBy(x => x.SapXep).ToListAsync();
            return _mp.Map<List<LoaiTienViewModel>>(entity);
        }

        public async Task<List<LoaiTienViewModel>> GetAllActive()
        {
            var entity = await _db.LoaiTiens.Where(x => x.Status == true).OrderBy(x => x.SapXep).ToListAsync();
            var _result = _mp.Map<List<LoaiTienViewModel>>(entity);
            // Không thêm dòng này vì ảnh hưởng đến các phiếu dùng ngoại tệ, thêm cái này vào trong front-end modal lọc báo cáo thôi, xem thao khảo file loc-bao-cao-tien-modal dòng: 68
            //_result.Insert(0, new LoaiTienViewModel() { LoaiTienId = "", Ma = "TH", Ten = "Tổng hợp" }); // 
            return _result;
        }

        public async Task<LoaiTienViewModel> GetById(string id)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == id);
            return _mp.Map<LoaiTienViewModel>(entity);
        }

        public async Task<LoaiTienViewModel> GetByMa(string ma)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.Ma == ma);
            return _mp.Map<LoaiTienViewModel>(entity);
        }

        public async Task<bool> CheckMa(string maVT_HH)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.Ma.ToUpper().Trim() == maVT_HH.ToUpper().Trim());
            if (entity != null) return true;
            else return false;
        }


        public async Task<int> CheckMa(LoaiTienViewModel data)
        {
            return await _db.LoaiTiens.CountAsync(x => x.Ma.ToUpper().Trim() == data.Ma.ToUpper().Trim());
        }

        public async Task<bool> Insert(LoaiTienViewModel model)
        {
            var entity = _mp.Map<LoaiTien>(model);
            entity.LoaiTienId = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.ModifyDate = entity.CreatedDate;
            entity.CreatedBy = model.ActionUser.UserId;
            entity.ModifyBy = entity.CreatedBy;
            await _db.LoaiTiens.AddAsync(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> InsertRange(List<LoaiTienViewModel> model)
        {
            foreach (var item in model)
            {
                var entity = _mp.Map<LoaiTien>(item);
                entity.LoaiTienId = Guid.NewGuid().ToString();
                entity.CreatedDate = DateTime.Now;
                entity.ModifyDate = entity.CreatedDate;
                //entity.CreatedBy = item.ActionUser.UserId;
                entity.ModifyBy = entity.CreatedBy;
                await _db.LoaiTiens.AddAsync(entity);
            }
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(LoaiTienViewModel model)
        {
            var entity = _mp.Map<LoaiTien>(model);
            entity.ModifyDate = DateTime.Now;
            entity.ModifyBy = model.ActionUser.UserId;
            _db.LoaiTiens.Update(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<LoaiTienViewModel> GetTienVietAsync()
        {
            LoaiTien model = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.Ma == "VND");
            LoaiTienViewModel result = _mp.Map<LoaiTienViewModel>(model);
            return result;
        }

        public LoaiTienViewModel GetTienViet()
        {
            LoaiTien model = _db.LoaiTiens.FirstOrDefault(x => x.Ma == "VND");
            LoaiTienViewModel result = _mp.Map<LoaiTienViewModel>(model);
            return result;
        }

        public async Task<string> ExportExcelLoaiTien()
        {
            var query = await GetAll();
            var fileName = await this.ExportLTsync(query);
            return this.GetLinkFile(fileName);
        }

        public string GetLinkFile(string link)
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

        private async Task<string> ExportLTsync(List<LoaiTienViewModel> query)
        {
            string excelFileName = string.Empty;

            try
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

                excelFileName = $"BANG_KE_LOAI_TIEN_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                string excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"docs/samples/BangKeDanhMuc/BANG_KE_LOAI_TIEN.xlsx";
                string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

                FileInfo file = new FileInfo(_path_sample);
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // Open sheet1
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    // From to time
                    //worksheet.Cells[5, 1].Value = string.Format("Từ ngày {0} đến ngày {1}", _from.ToString("dd/MM/yyyy"), _to.ToString("dd/MM/yyyy"));
                    // Get total all row
                    int totalRows = query.Count;
                    // Begin row
                    int begin_row = 7;
                    // get co cau to chuc 
                    //var entity = await _db.CoCauToChucs.FirstOrDefaultAsync(x => x.CapToChuc == "0");
                    //CoCauToChucViewModel coCauToChuc = _mp.Map<CoCauToChucViewModel>(entity);
                    //var _thongTinIn = _db.CoCauToChuc_ThongTinInBaoCaos
                    //            .AsNoTracking()
                    //            .FirstOrDefault();
                    //worksheet.Cells[1, 1].Value = _thongTinIn.TenDonVi;
                    //worksheet.Cells[2, 1].Value = _thongTinIn.DiaChi;
                    // Add Row
                    if (totalRows != 0)
                    {
                        worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                    }
                    // Fill data
                    int idx = begin_row + (totalRows == 0 ? 1 : 0);
                    foreach (var _it in query)
                    {
                        worksheet.Row(idx).Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[idx, 1].Value = _it.Ma;
                        worksheet.Cells[idx, 2].Value = _it.Ten;
                        worksheet.Cells[idx, 3].Value = _it.TyGiaQuyDoi;
                        worksheet.Cells[idx, 4].Value = _it.Status == true ? "x" : string.Empty;
                        idx += 1;
                    }
                    worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);
                    worksheet.Cells[idx, 2].Value = string.Empty;
                    worksheet.Cells[idx, 3].Value = string.Empty;
                    worksheet.Cells[idx, 4].Value = string.Empty;
                    package.SaveAs(new FileInfo(excelPath));
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
            return excelFileName;
        }

    }
}
