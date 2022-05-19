using API.Extentions;
using DLL;
using DLL.Constants;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class MauHoaDonController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IMauHoaDonService _mauHoaDonService;
        private readonly IDatabaseService _databaseService;

        public MauHoaDonController(IMauHoaDonService mauHoaDonService, Datacontext datacontext, IDatabaseService databaseService)
        {
            _mauHoaDonService = mauHoaDonService;
            _db = datacontext;
            _databaseService = databaseService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(MauHoaDonParams @params)
        {
            var result = await _mauHoaDonService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(MauHoaDonParams pagingParams)
        {
            var paged = await _mauHoaDonService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mauHoaDonService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetNgayKyById/{Id}")]
        public async Task<IActionResult> GetNgayKyById(string id)
        {
            var result = await _mauHoaDonService.GetNgayKyByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("GetListFromBoKyHieuHoaDon")]
        public async Task<IActionResult> GetListFromBoKyHieuHoaDon(MauHoaDonParams @params)
        {
            var result = await _mauHoaDonService.GetListFromBoKyHieuHoaDonAsync(@params);
            return Ok(result);
        }

        [HttpPost("CheckAllowUpdate")]
        public async Task<IActionResult> CheckAllowUpdate(MauHoaDonViewModel model)
        {
            var result = await _mauHoaDonService.CheckAllowUpdateAsync(model);
            return Ok(new { result });
        }

        [HttpGet("GetMauHoaDonBackgrounds")]
        public IActionResult GetMauHoaDonBackgrounds()
        {
            var result = _mauHoaDonService.GetMauHoaDonBackgrounds();
            return Ok(result);
        }

        [HttpGet("GetBackgrounds")]
        public IActionResult GetBackgrounds()
        {
            var result = _mauHoaDonService.GetBackgrounds();
            return Ok(result);
        }

        [HttpGet("GetBorders")]
        public IActionResult GetBorders()
        {
            var result = _mauHoaDonService.GetBorders();
            return Ok(result);
        }

        [HttpPost("GetListMauHoaDon")]
        public IActionResult GetListMauHoaDon(MauHoaDonParams pagingParams)
        {
            var result = _mauHoaDonService.GetListMauHoaDon(pagingParams);
            return Ok(result);
        }

        [HttpGet("GetListMauDaDuocChapNhan")]
        public async Task<IActionResult> GetListMauDaDuocChapNhan()
        {
            var result = await _mauHoaDonService.GetListMauDaDuocChapNhanAsync();
            return Ok(result);
        }

        [HttpGet("GetListQuyDinhApDung")]
        public IActionResult GetListQuyDinhApDung()
        {
            var result = _mauHoaDonService.GetListQuyDinhApDung();
            return Ok(result);
        }

        [HttpGet("GetListLoaiHoaDon")]
        public IActionResult GetListLoaiHoaDon()
        {
            var result = _mauHoaDonService.GetListLoaiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetListLoaiMau")]
        public IActionResult GetListLoaiMau()
        {
            var result = _mauHoaDonService.GetListLoaiMau();
            return Ok(result);
        }

        [HttpGet("GetListLoaiThueGTGT")]
        public IActionResult GetListLoaiThueGTGT()
        {
            var result = _mauHoaDonService.GetListLoaiThueGTGT();
            return Ok(result);
        }

        [HttpGet("GetListLoaiNgonNgu")]
        public IActionResult GetListLoaiNgonNgu()
        {
            var result = _mauHoaDonService.GetListLoaiNgonNgu();
            return Ok(result);
        }

        [HttpGet("GetListLoaiKhoGiay")]
        public IActionResult GetListLoaiKhoGiay()
        {
            var result = _mauHoaDonService.GetListLoaiKhoGiay();
            return Ok(result);
        }

        [HttpPost("CheckTrungTenMauHoaDon")]
        public async Task<IActionResult> CheckTrungTenMauHoaDon(MauHoaDonViewModel model)
        {
            var result = await _mauHoaDonService.CheckTrungTenMauHoaDonAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(MauHoaDonViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _mauHoaDonService.InsertAsync(model);
                    if (result != null) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(MauHoaDonViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _mauHoaDonService.UpdateAsync(model);
                    if (result) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPut("UpdateNgayKy")]
        public async Task<IActionResult> UpdateNgayKy(MauHoaDonViewModel model)
        {
            var result = await _mauHoaDonService.UpdateNgayKyAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _mauHoaDonService.DeleteAsync(id);
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

        [HttpGet("GetListNhatKyHoaDon/{mauHoaDonId}")]
        public async Task<IActionResult> GetListNhatKyHoaDon(string mauHoaDonId)
        {
            var result = await _mauHoaDonService.GetListNhatKyHoaDonAsync(mauHoaDonId);
            return Ok(result);
        }

        [HttpGet("GetChiTietByMauHoaDon/{mauHoaDonId}")]
        public async Task<IActionResult> GetChiTietByMauHoaDon(string mauHoaDonId)
        {
            var result = await _mauHoaDonService.GetChiTietByMauHoaDon(mauHoaDonId);
            return Ok(result);
        }

        [HttpGet("GetAllMauHoaDon")]
        public async Task<IActionResult> GetAllMauHoaDon()
        {
            var result = await _mauHoaDonService.GetAllMauSoHoaDon();
            return Ok(result);
        }

        [HttpPost("GetAllKyHieuHoaDon")]
        public async Task<IActionResult> GetAllKyHieuHoaDon(GetKyHieuHoaDonParams @params)
        {
            var result = await _mauHoaDonService.GetAllKyHieuHoaDon(@params.MauSo);
            return Ok(result);
        }

        [HttpPost("PreviewPdf")]
        public async Task<IActionResult> PreviewPdf(MauHoaDonFileParams @params)
        {
            var result = await _mauHoaDonService.PreviewPdfAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(MauHoaDonFileParams @params)
        {
            var result = await _mauHoaDonService.DownloadFileAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("ExportMauHoaDon")]
        public async Task<IActionResult> ExportMauHoaDon(ExportMauHoaDonParams @params)
        {
            var result = await _mauHoaDonService.ExportMauHoaDonAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpGet("GetFileToSign")]
        public IActionResult GetFileToSign()
        {
            var result = _mauHoaDonService.GetFileToSign();
            return Ok(new { result });
        }

        [AllowAnonymous]
        [HttpGet("UpdateMauTuyChonChiTietBanHang")]
        public async Task<IActionResult> UpdateMauTuyChonChiTietBanHang(string maSoThue)
        {
            if (string.IsNullOrEmpty(maSoThue))
            {
                return Ok(new
                {
                    Status = false,
                    Message = "Vui lòng nhập mã số thuế"
                });
            }

            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(maSoThue);
            if (companyModel == null)
            {
                return Ok(new
                {
                    Status = false,
                    Message = "Mã số thuế không tồn tại"
                });
            }

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _mauHoaDonService.UpdateMauTuyChonChiTietBanHangAsync();
                    transaction.Commit();
                    return Ok(new
                    {
                        Status = true,
                        Message = result
                    });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Ok(new
                    {
                        Status = false,
                        Message = "Exception: " + e.Message
                    });
                }
            }
        }

        [HttpGet("CheckXoaKyDienTu/{mauHoaDonId}")]
        public async Task<IActionResult> CheckXoaKyDienTu(string mauHoaDonId)
        {
            var result = await _mauHoaDonService.CheckXoaKyDienTuAsync(mauHoaDonId);
            return Ok(result);
        }

        [HttpGet("GetByIdBasic/{mauHoaDonId}")]
        public async Task<IActionResult> GetByIdBasic(string mauHoaDonId)
        {
            var result = await _mauHoaDonService.GetByIdBasicAsync(mauHoaDonId);
            return Ok(result);
        }

        [HttpPost("PreviewPdfOfXacThuc")]
        public async Task<IActionResult> PreviewPdfOfXacThuc(MauHoaDonFileParams @params)
        {
            var result = await _mauHoaDonService.PreviewPdfOfXacThucAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpGet("AddDocFiles/{mauHoaDonId}")]
        public async Task<IActionResult> AddDocFiles(string mauHoaDonId)
        {
            if (string.IsNullOrEmpty(mauHoaDonId))
            {
                return Ok(false);
            }

            var model = await _mauHoaDonService.GetByIdAsync(mauHoaDonId);
            if (model == null)
            {
                return Ok(false);
            }

            var result = await _mauHoaDonService.AddDocFilesAsync(model);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("AddDongTienThanhToanVaTyGiaChiTiet")]
        public async Task<IActionResult> AddDongTienThanhToanVaTyGiaChiTiet([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                string dbString = (param.KeyString).Base64Decode();

                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);

                using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var result = await _mauHoaDonService.AddDongTienThanhToanVaTyGiaChiTietAsync();
                        transaction.Commit();
                        return Ok(result);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }
                }
            }

            return Ok(false);
        }
    }
}
