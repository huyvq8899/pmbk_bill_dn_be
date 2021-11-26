using API.Extentions;
using DLL;
using DLL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.HeThong;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class DoiTuongController : BaseController
    {
        private readonly IDoiTuongService _doiTuongService;
        private readonly Datacontext _db;
        private readonly IDatabaseService _databaseService;
        public DoiTuongController(IDoiTuongService doiTuongService, Datacontext db, IDatabaseService databaseService)
        {
            _doiTuongService = doiTuongService;
            _databaseService = databaseService;
            _db = db;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(DoiTuongParams @params)
        {
            var result = await _doiTuongService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(DoiTuongParams pagingParams)
        {
            var paged = await _doiTuongService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _doiTuongService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllKhachHang")]
        public async Task<IActionResult> GetAllKhachHang()
        {
            var result = await _doiTuongService.GetAllKhachHang();
            return Ok(result);
        }

        [HttpGet("GetAllNhanVien")]
        public async Task<IActionResult> GetAllNhanVien()
        {
            var result = await _doiTuongService.GetAllNhanVien();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllNhanVien_TraCuu")]
        public async Task<IActionResult> GetAllNhanVien_TraCuu([FromQuery]string hoaDonDienTuId)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = await _doiTuongService.GetAllNhanVien();
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("CheckPhatSinh")]
        public async Task<IActionResult> CheckPhatSinh(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckPhatSinhAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _doiTuongService.DeleteAsync(id);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return Ok(new
                {
                    result = "DbUpdateException",
                    value = false
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(DoiTuongParams @params)
        {
            var result = await _doiTuongService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("ImportKhachHang")]
        public async Task<IActionResult> ImportKhachHang([FromForm] NhapKhauParams @params)
        {
            var result = await _doiTuongService.ImportKhachHang(@params.Files, @params.ModeValue);
            return Ok(result);
        }

        [HttpPost("InsertKhachHangImport")]
        public async Task<IActionResult> InsertKhachHangImport(List<DoiTuongViewModel> model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // convert
                    var listData = await _doiTuongService.ConvertImportKhachHang(model);
                    int success = 0;
                    foreach (var item in listData)
                    {
                        var result = false;
                        if (!string.IsNullOrEmpty(item.DoiTuongId))
                        {
                            result = await _doiTuongService.UpdateAsync(item);
                        }
                        else result = await _doiTuongService.InsertAsync(item) != null;
                        if (result == false) break;
                        success++;
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        status = true,
                        numDanhMuc = listData.Count,
                        numSuccess = success
                    });
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportKhachHangError")]
        public IActionResult CreateFileImportKhachHangError(List<DoiTuongViewModel> list)
        {
            try
            {
                var result = _doiTuongService.CreateFileImportKhachHangError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("ImportNhanVien")]
        public async Task<IActionResult> ImportNhanVien([FromForm] NhapKhauParams @params)
        {
            var result = await _doiTuongService.ImportNhanVien(@params.Files, @params.ModeValue);
            return Ok(result);
        }

        [HttpPost("InsertNhanVienImport")]
        public async Task<IActionResult> InsertNhanVienImport(List<DoiTuongViewModel> model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // convert
                    var listData = await _doiTuongService.ConvertImportNhanVien(model);
                    int success = 0;
                    foreach (var item in listData)
                    {
                        var result = false;
                        if (!string.IsNullOrEmpty(item.DoiTuongId))
                        {
                            result = await _doiTuongService.UpdateAsync(item);
                        }
                        else result = await _doiTuongService.InsertAsync(item) != null;
                        if (result == false) break;
                        success++;
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        status = true,
                        numDanhMuc = listData.Count,
                        numSuccess = success
                    });
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportNhanVienError")]
        public IActionResult CreateFileImportNhanVienError(List<DoiTuongViewModel> list)
        {
            try
            {
                var result = _doiTuongService.CreateFileImportNhanVienError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

    }
}
