using DLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
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
        public DoiTuongController(IDoiTuongService doiTuongService, Datacontext db)
        {
            _doiTuongService = doiTuongService;
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

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckTrungMaAsync(model);
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
            catch (DbUpdateException ex)
            {
                return Ok(new
                {
                    result = "DbUpdateException",
                    value = false
                });
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

        public async Task<IActionResult> ImportKhachHang(IList<IFormFile> files)
        {
            var result = await _doiTuongService.ImportKhachHang(files);
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
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportKhachHangError")]
        public async Task<IActionResult> CreateFileImportKhachHangError(List<DoiTuongViewModel> list)
        {
            try
            {
                var result = await _doiTuongService.CreateFileImportKhachHangError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

        [HttpPost("ImportNhanVien")]
        public async Task<IActionResult> ImportNhanVien(IList<IFormFile> files)
        {
            var result = await _doiTuongService.ImportNhanVien(files);
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
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportNhanVienError")]
        public async Task<IActionResult> CreateFileImportNhanVienError(List<DoiTuongViewModel> list)
        {
            try
            {
                var result = await _doiTuongService.CreateFileImportNhanVienError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

    }
}
