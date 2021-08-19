using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class DoiTuongService : IDoiTuongService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public DoiTuongService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _IHttpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> CheckTrungMaAsync(DoiTuongViewModel model)
        {
            bool result = await _db.DoiTuongs
                .AnyAsync(x => x.Ma.ToUpper().Trim() == model.Ma.ToUpper().Trim() && x.IsKhachHang == model.IsKhachHang && x.IsNhanVien == model.IsNhanVien);

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == id);
            _db.DoiTuongs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportExcelAsync(DoiTuongParams @params)
        {
            @params.PageSize = -1;
            PagedList<DoiTuongViewModel> paged = await GetAllPagingAsync(@params);
            List<DoiTuongViewModel> list = paged.Items;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/DoiTuong/BANG_KE_{(@params.LoaiDoiTuong == 1 ? "KHACH_HANG" : "NHAN_VIEN")}.xlsx";
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

                    if (@params.LoaiDoiTuong == 1)
                    {
                        worksheet.Cells[idx, 3].Value = _it.DiaChi;
                        worksheet.Cells[idx, 4].Value = _it.MaSoThue;
                        worksheet.Cells[idx, 5].Value = _it.SoDienThoaiNguoiNhanHD;
                        worksheet.Cells[idx, 6].Value = _it.HoTenNguoiNhanHD;
                        worksheet.Cells[idx, 7].Value = _it.EmailNguoiNhanHD;
                    }
                    else
                    {
                        worksheet.Cells[idx, 3].Value = _it.MaSoThue;
                        worksheet.Cells[idx, 4].Value = _it.ChucDanh;
                        worksheet.Cells[idx, 5].Value = _it.TenDonVi;
                        worksheet.Cells[idx, 6].Value = _it.EmailNguoiNhanHD;
                        worksheet.Cells[idx, 7].Value = _it.SoDienThoaiNguoiNhanHD;
                    }

                    worksheet.Cells[idx, 8].Value = _it.SoTaiKhoanNganHang;
                    worksheet.Cells[idx, 9].Value = _it.TenNganHang;
                    worksheet.Cells[idx, 10].Value = _it.ChiNhanh;
                    worksheet.Cells[idx, 11].Value = _it.Status == true ? string.Empty : "ü";

                    idx += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"BANG_KE_{(@params.LoaiDoiTuong == 1 ? "KHACH_HANG" : "NHAN_VIEN")}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
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

        public async Task<List<DoiTuongViewModel>> GetAllAsync(DoiTuongParams @params = null)
        {
            var query = _db.DoiTuongs.AsQueryable();

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    string keyword = @params.Keyword.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) ||
                                            x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()));
                }

                if (@params.LoaiDoiTuong.HasValue == true)
                {
                    query = query.Where(x => @params.LoaiDoiTuong == 1 ? (x.IsKhachHang == true) : (x.IsNhanVien == true));
                }
            }

            var result = await query
                .ProjectTo<DoiTuongViewModel>(_mp.ConfigurationProvider)
                .AsNoTracking()
                .OrderBy(x => x.Ma)
                .ToListAsync();

            return result;
        }

        public async Task<List<DoiTuongViewModel>> GetAllKhachHang()
        {
            var query = new List<DoiTuongViewModel>();
            try
            {
                query = _mp.Map<List<DoiTuongViewModel>>(await _db.DoiTuongs.Where(x => x.IsKhachHang == true).ToListAsync());
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return query;
        }

        public async Task<List<DoiTuongViewModel>> GetAllNhanVien()
        {
            var query = new List<DoiTuongViewModel>();
            try
            {
                query = _mp.Map<List<DoiTuongViewModel>>(await _db.DoiTuongs.Where(x => x.IsNhanVien == true).ToListAsync());
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return query;
        }

        public async Task<PagedList<DoiTuongViewModel>> GetAllPagingAsync(DoiTuongParams @params)
        {
            var query = _db.DoiTuongs
                .OrderBy(x => x.Ma)
                .Select(x => new DoiTuongViewModel
                {
                    DoiTuongId = x.DoiTuongId,
                    LoaiKhachHang = x.LoaiKhachHang,
                    MaSoThue = x.MaSoThue ?? string.Empty,
                    Ma = x.Ma ?? string.Empty,
                    Ten = x.Ten ?? string.Empty,
                    DiaChi = x.DiaChi ?? string.Empty,
                    SoTaiKhoanNganHang = x.SoTaiKhoanNganHang ?? string.Empty,
                    TenNganHang = x.TenNganHang ?? string.Empty,
                    ChiNhanh = x.ChiNhanh ?? string.Empty,
                    HoTenNguoiMuaHang = x.HoTenNguoiMuaHang ?? string.Empty,
                    EmailNguoiMuaHang = x.EmailNguoiMuaHang ?? string.Empty,
                    SoDienThoaiNguoiMuaHang = x.SoDienThoaiNguoiMuaHang ?? string.Empty,
                    HoTenNguoiNhanHD = x.HoTenNguoiNhanHD ?? string.Empty,
                    EmailNguoiNhanHD = x.EmailNguoiNhanHD ?? string.Empty,
                    SoDienThoaiNguoiNhanHD = x.SoDienThoaiNguoiNhanHD ?? string.Empty,
                    ChucDanh = x.ChucDanh ?? string.Empty,
                    TenDonVi = x.TenDonVi ?? string.Empty,
                    IsKhachHang = x.IsKhachHang,
                    IsNhanVien = x.IsNhanVien,
                    Status = true
                });

            if (@params.LoaiKhachHang.HasValue == true && (@params.LoaiKhachHang == 1 || @params.LoaiKhachHang == 2))
            {
                query = query.Where(x => x.LoaiKhachHang == @params.LoaiKhachHang);
            }

            if (@params.LoaiDoiTuong.HasValue == true)
            {
                query = query.Where(x => @params.LoaiDoiTuong == 1 ? (x.IsKhachHang == true) : (x.IsNhanVien == true));
            }

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
                if (!string.IsNullOrEmpty(@params.Filter.MaSoThue))
                {
                    var keyword = @params.Filter.MaSoThue.ToUpper().ToTrim();
                    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.DiaChi))
                {
                    var keyword = @params.Filter.DiaChi.ToUpper().ToTrim();
                    query = query.Where(x => x.DiaChi.ToUpper().ToTrim().Contains(keyword) || x.DiaChi.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.SoTaiKhoanNganHang))
                {
                    var keyword = @params.Filter.SoTaiKhoanNganHang.ToUpper().ToTrim();
                    query = query.Where(x => x.SoTaiKhoanNganHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.TenNganHang))
                {
                    var keyword = @params.Filter.TenNganHang.ToUpper().ToTrim();
                    query = query.Where(x => x.TenNganHang.ToUpper().ToTrim().Contains(keyword) || x.TenNganHang.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.ChiNhanh))
                {
                    var keyword = @params.Filter.ChiNhanh.ToUpper().ToTrim();
                    query = query.Where(x => x.ChiNhanh.ToUpper().ToTrim().Contains(keyword) || x.ChiNhanh.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.HoTenNguoiMuaHang))
                {
                    var keyword = @params.Filter.HoTenNguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiMuaHang.ToUpper().ToTrim().Contains(keyword) || x.HoTenNguoiMuaHang.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.EmailNguoiMuaHang))
                {
                    var keyword = @params.Filter.EmailNguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.EmailNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.SoDienThoaiNguoiMuaHang))
                {
                    var keyword = @params.Filter.SoDienThoaiNguoiMuaHang.ToUpper().ToTrim();
                    query = query.Where(x => x.SoDienThoaiNguoiMuaHang.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.HoTenNguoiNhanHD))
                {
                    var keyword = @params.Filter.HoTenNguoiNhanHD.ToUpper().ToTrim();
                    query = query.Where(x => x.HoTenNguoiNhanHD.ToUpper().ToTrim().Contains(keyword) || x.HoTenNguoiNhanHD.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.EmailNguoiNhanHD))
                {
                    var keyword = @params.Filter.EmailNguoiNhanHD.ToUpper().ToTrim();
                    query = query.Where(x => x.EmailNguoiNhanHD.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.SoDienThoaiNguoiNhanHD))
                {
                    var keyword = @params.Filter.SoDienThoaiNguoiNhanHD.ToUpper().ToTrim();
                    query = query.Where(x => x.SoDienThoaiNguoiNhanHD.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.ChucDanh))
                {
                    var keyword = @params.Filter.ChucDanh.ToUpper().ToTrim();
                    query = query.Where(x => x.ChucDanh.ToUpper().ToTrim().Contains(keyword) || x.ChucDanh.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.TenDonVi))
                {
                    var keyword = @params.Filter.TenDonVi.ToUpper().ToTrim();
                    query = query.Where(x => x.TenDonVi.ToUpper().ToTrim().Contains(keyword) || x.TenDonVi.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
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

                if (@params.SortKey == nameof(@params.Filter.MaSoThue))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaSoThue);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaSoThue);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.DiaChi))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.DiaChi);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.DiaChi);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.SoTaiKhoanNganHang))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoTaiKhoanNganHang);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoTaiKhoanNganHang);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TenNganHang))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenNganHang);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenNganHang);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.ChiNhanh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.ChiNhanh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.ChiNhanh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.HoTenNguoiMuaHang))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.HoTenNguoiMuaHang);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.HoTenNguoiMuaHang);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.EmailNguoiMuaHang))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.EmailNguoiMuaHang);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.EmailNguoiMuaHang);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.SoDienThoaiNguoiMuaHang))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoDienThoaiNguoiMuaHang);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoDienThoaiNguoiMuaHang);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.HoTenNguoiNhanHD))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.HoTenNguoiNhanHD);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.HoTenNguoiNhanHD);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.EmailNguoiNhanHD))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.EmailNguoiNhanHD);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.EmailNguoiNhanHD);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.SoDienThoaiNguoiNhanHD))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoDienThoaiNguoiNhanHD);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoDienThoaiNguoiNhanHD);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.ChucDanh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.ChucDanh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.ChucDanh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TenDonVi))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenDonVi);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenDonVi);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.Status))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Status);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Status);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<DoiTuongViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<DoiTuongViewModel> GetByIdAsync(string id)
        {
            var query = from dt in _db.DoiTuongs
                        where dt.DoiTuongId == id
                        select new DoiTuongViewModel
                        {
                            DoiTuongId = dt.DoiTuongId,
                            LoaiKhachHang = dt.LoaiKhachHang,
                            TenLoaiKhachHang = dt.LoaiKhachHang == 1 ? "Cá nhân" : "Tổ chức",
                            MaSoThue = dt.MaSoThue,
                            Ma = dt.Ma,
                            Ten = dt.Ten,
                            DiaChi = dt.DiaChi,
                            SoTaiKhoanNganHang = dt.SoTaiKhoanNganHang,
                            TenNganHang = dt.TenNganHang,
                            ChiNhanh = dt.ChiNhanh,
                            HoTenNguoiMuaHang = dt.HoTenNguoiMuaHang,
                            EmailNguoiMuaHang = dt.EmailNguoiMuaHang,
                            SoDienThoaiNguoiMuaHang = dt.SoDienThoaiNguoiMuaHang,
                            HoTenNguoiNhanHD = dt.HoTenNguoiNhanHD,
                            EmailNguoiNhanHD = dt.EmailNguoiNhanHD,
                            SoDienThoaiNguoiNhanHD = dt.SoDienThoaiNguoiNhanHD,
                            ChucDanh = dt.ChucDanh,
                            TenDonVi = dt.TenDonVi,
                            IsKhachHang = dt.IsKhachHang,
                            IsNhanVien = dt.IsNhanVien,
                            CreatedBy = dt.CreatedBy,
                            CreatedDate = dt.CreatedDate,
                            Status = dt.Status
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public async Task<DoiTuongViewModel> InsertAsync(DoiTuongViewModel model)
        {
            var entity = _mp.Map<DoiTuong>(model);
            await _db.DoiTuongs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<DoiTuongViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(DoiTuongViewModel model)
        {
            var entity = await _db.DoiTuongs.FirstOrDefaultAsync(x => x.DoiTuongId == model.DoiTuongId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<DoiTuongViewModel>> ImportKhachHang(IList<IFormFile> files)
        {
            var formFile = files[0];
            var list = new List<DoiTuongViewModel>();
            var listNV = await _db.DoiTuongs.Where(x => x.IsNhanVien == true).ToListAsync();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DoiTuongViewModel item = new DoiTuongViewModel();
                        item.Row = row - 1;
                        // Là tổ chức/cá nhân
                        item.LoaiKhachHang = worksheet.Cells[row, 1].Value == null ? 1 : int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
                        if (item.ErrorMessage == null)
                        {
                            if (item.LoaiKhachHang != 1 && item.LoaiKhachHang != 2)
                            {
                                item.ErrorMessage = "<Là tổ chức/cá nhân> sai định dạng";
                                item.HasError = true;
                            }
                        }

                        // Mã số thuế
                        item.MaSoThue = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim();


                        // Số tài khoản NH
                        item.SoTaiKhoanNganHang = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim();

                        // Tên ngân hàng
                        item.TenNganHang = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.SoTaiKhoanNganHang != "" && item.TenNganHang == "")
                            {
                                item.ErrorMessage = "<Tên ngân hàng> không được để trống vì đã nhập <Số tài khoản NH>";
                                item.HasError = true;
                            }
                            if (item.SoTaiKhoanNganHang == "" && item.TenNganHang != "")
                            {
                                item.ErrorMessage = "<Số tài khoản NH> không được để trống vì đã nhập <Tên ngân hàng>";
                                item.HasError = true;
                            }
                        }

                        // Chi nhánh
                        item.ChiNhanh = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.ChiNhanh != "" && item.SoTaiKhoanNganHang == "" && item.TenNganHang == "")
                            {
                                item.ErrorMessage = "<Chi nhánh> không được nhập vì <Số tài khoản NH> và <Tên ngân hàng> đang để trống";
                                item.HasError = true;
                            }
                        }

                        item.Ma = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.Ma == "")
                            {
                                item.ErrorMessage = "<Mã khách hàng> không được để trống";
                                item.HasError = true;
                            }
                            else
                            {
                                var existed = list.FirstOrDefault(x => x.Ma.ToUpper() == item.Ma.ToUpper());
                                if (existed != null)
                                {
                                    item.Duplicate = true;
                                    if (!existed.HasError && list.Count(x => x.Ma.ToUpper() == item.Ma.ToUpper()) > 0)
                                    {
                                        item.ErrorMessage = $"<Mã khách hàng> trùng với (dòng số <{existed.Row}>)";
                                        item.HasError = true;
                                    }
                                    else if (await CheckTrungMaAsync(item))
                                    {
                                        item.ErrorMessage = "<Mã khách hàng> đã tồn tại trong hệ thống";
                                        item.HasError = true;
                                    }
                                }
                                else if (await CheckTrungMaAsync(item))
                                {
                                    item.ErrorMessage = "<Mã khách hàng> đã tồn tại trong hệ thống";
                                    item.HasError = true;
                                }
                            }
                        }

                        // Tên khách hàng
                        item.Ten = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.Ten == "")
                            {
                                item.ErrorMessage = "<Tên khách hàng> không được để trống";
                                item.HasError = true;
                            }
                        }

                        // Địa chỉ
                        item.DiaChi = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim();

                        // Điện thoại
                        item.HoTenNguoiMuaHang = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim();

                        // Website
                        item.EmailNguoiMuaHang = worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim();

                        // Fax
                        item.SoDienThoaiNguoiMuaHang = worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim();

                        // Email
                        item.HoTenNguoiNhanHD = worksheet.Cells[row, 12].Value == null ? "" : worksheet.Cells[row, 12].Value.ToString().Trim();

                        // Số CMND
                        item.EmailNguoiNhanHD = worksheet.Cells[row, 13].Value == null ? "" : worksheet.Cells[row, 13].Value.ToString().Trim();

                        // Ngày cấp CMND
                        item.SoDienThoaiNguoiNhanHD = worksheet.Cells[row, 14].Value == null ? "" : worksheet.Cells[row, 14].Value.ToString().Trim();

                        // success
                        if (item.HasError == false)
                        {
                            item.ErrorMessage = "<hợp lệ>";
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public async Task<List<DoiTuongViewModel>> ConvertImportKhachHang(List<DoiTuongViewModel> model)
        {
            List<DoiTuongViewModel> listData = new List<DoiTuongViewModel>();
            List<string> existCode = new List<string>();
            foreach (var item in model)
            {
                DoiTuongViewModel dt = new DoiTuongViewModel();
                dt.IsKhachHang = true;
                dt.IsNhanVien = false;
                dt.LoaiKhachHang = item.LoaiKhachHang;
                dt.MaSoThue = item.MaSoThue;
                dt.Ma = item.Ma;
                dt.Ten = item.Ten;
                dt.DiaChi = item.DiaChi;
                dt.SoTaiKhoanNganHang = item.SoTaiKhoanNganHang;
                dt.TenNganHang = item.TenNganHang;
                dt.ChiNhanh = item.ChiNhanh;
                dt.HoTenNguoiMuaHang = item.HoTenNguoiMuaHang;
                dt.EmailNguoiMuaHang = item.EmailNguoiMuaHang;
                dt.SoDienThoaiNguoiMuaHang = item.SoDienThoaiNguoiMuaHang;
                dt.HoTenNguoiNhanHD = item.HoTenNguoiNhanHD;
                dt.EmailNguoiNhanHD = item.EmailNguoiNhanHD;
                dt.SoDienThoaiNguoiNhanHD = item.SoDienThoaiNguoiNhanHD;
            }

            return listData;
        }
        public async Task<string> CreateFileImportKhachHangError(List<DoiTuongViewModel> list)
        {
            string excelFileName = string.Empty;
            string excelPath = string.Empty;
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

                excelFileName = $"khach-hang-error-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"Template/ImportDanhMuc/Danh_Muc_Khach_Hang_Import.xlsx";
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
                        worksheet.Cells[i, 1].Value = item.LoaiKhachHang;
                        worksheet.Cells[i, 2].Value = item.Ma;
                        worksheet.Cells[i, 3].Value = item.Ten;
                        worksheet.Cells[i, 4].Value = item.MaSoThue;
                        worksheet.Cells[i, 5].Value = item.DiaChi;
                        worksheet.Cells[i, 6].Value = item.SoTaiKhoanNganHang;
                        worksheet.Cells[i, 7].Value = item.TenNganHang;
                        worksheet.Cells[i, 8].Value = item.ChiNhanh;
                        worksheet.Cells[i, 9].Value = item.HoTenNguoiMuaHang;
                        worksheet.Cells[i, 10].Value = item.EmailNguoiMuaHang;
                        worksheet.Cells[i, 11].Value = item.SoDienThoaiNguoiMuaHang;
                        worksheet.Cells[i, 12].Value = item.HoTenNguoiNhanHD;
                        worksheet.Cells[i, 13].Value = item.EmailNguoiNhanHD;
                        worksheet.Cells[i, 14].Value = item.SoDienThoaiNguoiNhanHD;
                        worksheet.Cells[i, 15].Value = item.ErrorMessage;
                        worksheet.Cells[i, 15].Style.Font.Color.SetColor(Color.Red);
                        i += 1;
                    }
                    package.SaveAs(new FileInfo(excelPath));
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
            return this.GetLinkFileExcel(excelFileName);
        }

        public async Task<List<DoiTuongViewModel>> ImportNhanVien(IList<IFormFile> files)
        {
            var formFile = files[0];
            var list = new List<DoiTuongViewModel>();
            var listNV = await _db.DoiTuongs.Where(x => x.IsNhanVien == true).ToListAsync();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DoiTuongViewModel item = new DoiTuongViewModel();
                        item.Row = row;
                        // Mã số thuế
                        item.MaSoThue = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString().Trim();

                        item.IsKhachHang = worksheet.Cells[row, 1].Value == null ? false : bool.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
                        // Số tài khoản NH
                        item.SoTaiKhoanNganHang = worksheet.Cells[row, 7].Value == null ? "" : worksheet.Cells[row, 7].Value.ToString().Trim();

                        // Tên ngân hàng
                        item.TenNganHang = worksheet.Cells[row, 8].Value == null ? "" : worksheet.Cells[row, 8].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.SoTaiKhoanNganHang != "" && item.TenNganHang == "")
                            {
                                item.ErrorMessage = "<Tên ngân hàng> không được để trống vì đã nhập <Số tài khoản NH>";
                                item.HasError = true;
                            }
                            if (item.SoTaiKhoanNganHang == "" && item.TenNganHang != "")
                            {
                                item.ErrorMessage = "<Số tài khoản NH> không được để trống vì đã nhập <Tên ngân hàng>";
                                item.HasError = true;
                            }
                        }

                        // Chi nhánh
                        item.ChiNhanh = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.ChiNhanh != "" && item.SoTaiKhoanNganHang == "" && item.TenNganHang == "")
                            {
                                item.ErrorMessage = "<Chi nhánh> không được nhập vì <Số tài khoản NH> và <Tên ngân hàng> đang để trống";
                                item.HasError = true;
                            }
                        }

                        item.Ma = worksheet.Cells[row, 3].Value == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.Ma == "")
                            {
                                item.ErrorMessage = "<Mã nhân viên> không được để trống";
                                item.HasError = true;
                            }
                            else
                            {
                                var existed = list.FirstOrDefault(x => x.Ma.ToUpper() == item.Ma.ToUpper());
                                if (existed != null)
                                {
                                    item.Duplicate = true;
                                    if (!existed.HasError && list.Count(x => x.Ma.ToUpper() == item.Ma.ToUpper()) > 0)
                                    {
                                        item.ErrorMessage = $"<Mã nhân viên> trùng với (dòng số <{existed.Row}>)";
                                        item.HasError = true;
                                    }
                                    else if (await CheckTrungMaAsync(item))
                                    {
                                        item.ErrorMessage = "<Mã nhân viên> đã tồn tại trong hệ thống";
                                        item.HasError = true;
                                    }
                                }
                                else if (await CheckTrungMaAsync(item))
                                {
                                    item.ErrorMessage = "<Mã nhân viên> đã tồn tại trong hệ thống";
                                    item.HasError = true;
                                }
                            }
                        }

                        // Tên khách hàng
                        item.Ten = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim();
                        if (item.ErrorMessage == null)
                        {
                            if (item.Ten == "")
                            {
                                item.ErrorMessage = "<Tên nhân viên> không được để trống";
                                item.HasError = true;
                            }
                        }

                        //Chức danh
                        item.ChucDanh = worksheet.Cells[row, 5].Value == null ? "" : worksheet.Cells[row, 5].Value.ToString().Trim();

                        // Địa chỉ
                        item.TenDonVi = worksheet.Cells[row, 6].Value == null ? "" : worksheet.Cells[row, 6].Value.ToString().Trim();

                        // Điện thoại
                        item.EmailNguoiNhanHD = worksheet.Cells[row, 10].Value == null ? "" : worksheet.Cells[row, 10].Value.ToString().Trim();

                        // Website
                        item.SoDienThoaiNguoiNhanHD = worksheet.Cells[row, 11].Value == null ? "" : worksheet.Cells[row, 11].Value.ToString().Trim();

                        // success
                        if (item.HasError == false)
                        {
                            item.ErrorMessage = "<hợp lệ>";
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public async Task<List<DoiTuongViewModel>> ConvertImportNhanVien(List<DoiTuongViewModel> model)
        {
            List<DoiTuongViewModel> listData = new List<DoiTuongViewModel>();
            List<string> existCode = new List<string>();
            foreach (var item in model)
            {
                DoiTuongViewModel dt = new DoiTuongViewModel();
                dt.IsKhachHang = item.IsKhachHang;
                dt.IsNhanVien = false;
                dt.MaSoThue = item.MaSoThue;
                dt.Ma = item.Ma;
                dt.Ten = item.Ten;
                dt.ChucDanh = item.ChucDanh;
                dt.TenDonVi = item.TenDonVi;
                dt.SoTaiKhoanNganHang = item.SoTaiKhoanNganHang;
                dt.TenNganHang = item.TenNganHang;
                dt.ChiNhanh = item.ChiNhanh;
                dt.EmailNguoiNhanHD = item.EmailNguoiNhanHD;
                dt.SoDienThoaiNguoiNhanHD = item.SoDienThoaiNguoiNhanHD;
            }

            return listData;
        }
        public async Task<string> CreateFileImportNhanVienError(List<DoiTuongViewModel> list)
        {
            string excelFileName = string.Empty;
            string excelPath = string.Empty;
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

                excelFileName = $"nhan-vien-error-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                string excelFolder = $"FilesUpload/excels/{excelFileName}";
                excelPath = Path.Combine(_hostingEnvironment.WebRootPath, excelFolder);

                // Excel
                string _sample = $"Template/ImportDanhMuc/Danh_Muc_Nhan_Vien_Import.xlsx";
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
                        worksheet.Cells[i, 1].Value = item.IsKhachHang;
                        worksheet.Cells[i, 2].Value = item.MaSoThue;
                        worksheet.Cells[i, 3].Value = item.Ma;
                        worksheet.Cells[i, 4].Value = item.Ten;
                        worksheet.Cells[i, 5].Value = item.ChucDanh;
                        worksheet.Cells[i, 6].Value = item.TenDonVi;
                        worksheet.Cells[i, 7].Value = item.SoTaiKhoanNganHang;
                        worksheet.Cells[i, 8].Value = item.TenNganHang;
                        worksheet.Cells[i, 9].Value = item.ChiNhanh;
                        worksheet.Cells[i, 10].Value = item.EmailNguoiNhanHD;
                        worksheet.Cells[i, 11].Value = item.SoDienThoaiNguoiNhanHD;
                        worksheet.Cells[i, 12].Value = item.ErrorMessage;
                        worksheet.Cells[i, 12].Style.Font.Color.SetColor(Color.Red);
                        i += 1;
                    }
                    package.SaveAs(new FileInfo(excelPath));
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
            return this.GetLinkFileExcel(excelFileName);
        }

        public string GetLinkFileExcel(string link)
        {
            var filename = "FilesUpload/excels/" + link;
            string url = "";
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
    }
}
