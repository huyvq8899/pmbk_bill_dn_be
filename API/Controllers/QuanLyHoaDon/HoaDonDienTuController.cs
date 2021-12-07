using API.Extentions;
using DLL;
using DLL.Constants;
using DLL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.FormActions;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class HoaDonDienTuController : BaseController
    {
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly IHoaDonDienTuChiTietService _hoaDonDienTuChiTietService;
        private readonly IUserRespositories _userRespositories;
        private readonly ITraCuuService _traCuuService;
        private readonly IDatabaseService _databaseService;

        //IThamChieuService _thamChieuService;
        private readonly Datacontext _db;

        public HoaDonDienTuController(
            IHoaDonDienTuService hoaDonDienTuService,
            IHoaDonDienTuChiTietService hoaDonDienTuChiTietService,
            IUserRespositories userRespositories,
            ITraCuuService traCuuService,
            IDatabaseService databaseService,
            //IThamChieuService thamChieuService,
            Datacontext db
        )
        {
            _hoaDonDienTuService = hoaDonDienTuService;
            _hoaDonDienTuChiTietService = hoaDonDienTuChiTietService;
            _userRespositories = userRespositories;
            _traCuuService = traCuuService;
            _databaseService = databaseService;
            //_thamChieuService = thamChieuService;
            _db = db;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hoaDonDienTuService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(HoaDonParams pagingParams)
        {
            try
            {
                var paged = await _hoaDonDienTuService.GetAllPagingAsync(pagingParams);
                Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
                return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPost("GetAllPagingHoaDonThayThe")]
        public async Task<IActionResult> GetAllPagingHoaDonThayThe(HoaDonThayTheParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetAllPagingHoaDonThayTheAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("GetListHoaDonXoaBoCanThayThe")]
        public async Task<IActionResult> GetListHoaDonXoaBoCanThayThe(HoaDonThayTheParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonXoaBoCanThayTheAsync(pagingParams);
            return Ok(result);
        }

        [HttpPost("GetListHoaDonCanDieuChinh")]
        public async Task<IActionResult> GetListHoaDonCanDieuChinh(HoaDonDieuChinhParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonCanDieuChinhAsync(pagingParams);
            return Ok(result);
        }

        [HttpPost("GetAllPagingHoaDonDieuChinh")]
        public async Task<IActionResult> GetAllPagingHoaDonDieuChinh(HoaDonDieuChinhParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetAllPagingHoaDonDieuChinhAsync_New(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("GetListHoaDonKhongMa")]
        public async Task<IActionResult> GetListHoaDonKhongMa(HoaDonParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonKhongMaAsync(pagingParams);
            return Ok(result);
        }

        [HttpPost("GetListHoaDonCanCapMa")]
        public async Task<IActionResult> GetListHoaDonCanCapMa(HoaDonParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonCanCapMaAsync(pagingParams);
            return Ok(result);
        }

        [HttpGet("GetChiTietHoaDon/{id}")]
        public async Task<IActionResult> GetChiTietHoaDon(string id)
        {
            var result = await _hoaDonDienTuChiTietService.GetChiTietHoaDonAsync(id, false);
            return Ok(result);
        }

        [HttpGet("GetTrangThaiHoaDonDieuChinhs")]
        public IActionResult GetTrangThaiHoaDonDieuChinhs()
        {
            var result = _hoaDonDienTuService.GetTrangThaiHoaDonDieuChinhs();
            return Ok(result);
        }

        [HttpGet("GetLoaiTrangThaiBienBanDieuChinhHoaDons")]
        public IActionResult GetLoaiTrangThaiBienBanDieuChinhHoaDons()
        {
            var result = _hoaDonDienTuService.GetLoaiTrangThaiBienBanDieuChinhHoaDons();
            return Ok(result);
        }

        [HttpGet("GetLoaiTrangThaiPhatHanhs")]
        public IActionResult GetLoaiTrangThaiPhatHanhs()
        {
            var result = _hoaDonDienTuService.GetLoaiTrangThaiPhatHanhs();
            return Ok(result);
        }

        [HttpGet("GetLoaiTrangThaiGuiHoaDons")]
        public IActionResult GetLoaiTrangThaiGuiHoaDons()
        {
            var result = _hoaDonDienTuService.GetLoaiTrangThaiGuiHoaDons();
            return Ok(result);
        }

        [HttpGet("GetListHinhThucHoaDonCanThayThe")]
        public IActionResult GetListHinhThucHoaDonCanThayThe()
        {
            var result = _hoaDonDienTuService.GetListHinhThucHoaDonCanThayThe();
            return Ok(result);
        }

        [HttpGet("GetListTimKiemTheoHoaDonThayThe")]
        public IActionResult GetListTimKiemTheoHoaDonThayThe()
        {
            var result = _hoaDonDienTuService.GetListTimKiemTheoHoaDonThayThe();
            return Ok(result);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _hoaDonDienTuService.GetByIdAsync(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetById_TraCuu/{Id}")]
        public async Task<IActionResult> GetById_TraCuu(string id)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("CheckSoHoaDon")]
        public async Task<IActionResult> CheckSoHoaDon(string soHoaDon)
        {
            var result = await _hoaDonDienTuService.CheckSoHoaDonAsync(soHoaDon);
            return Ok(result);
        }

        [HttpPost("ExportExcelBangKe")]
        public async Task<IActionResult> ExportExcelBangKe(HoaDonParams @params)
        {
            var result = await _hoaDonDienTuService.ExportExcelBangKe(@params);
            return Ok(new { Path = result });
        }

        [HttpPost("ExportExcelBangKeChiTiet")]
        public async Task<IActionResult> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params)
        {
            var result = await _hoaDonDienTuService.ExportExcelBangKeChiTiet(@params);
            return Ok(new { path = result });
        }

        [HttpPost("ExportExcelError")]
        public IActionResult ExportExcelError(TaiHoaDonLoiParams @params)
        {
            var result = _hoaDonDienTuService.ExportErrorFile(@params.ListError, @params.Action);
            return Ok(new { path = result });
        }


        [HttpGet("GetError")]
        public IActionResult GetError([FromQuery] int LoaiLoi, [FromQuery] string HoaDonDienTuId)
        {
            var result = _hoaDonDienTuService.GetError(HoaDonDienTuId, LoaiLoi);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    model.HoaDonDienTuId = Guid.NewGuid().ToString();
                    List<HoaDonDienTuChiTietViewModel> hoaDonDienTuChiTiets = model.HoaDonChiTiets;

                    HoaDonDienTuViewModel result = await _hoaDonDienTuService.InsertAsync(model);
                    if (result == null)
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }

                    var models = await _hoaDonDienTuChiTietService.InsertRangeAsync(result, hoaDonDienTuChiTiets);
                    if (models.Count != hoaDonDienTuChiTiets.Count)
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }

                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(model.HoaDonDienTuId);
                    await _hoaDonDienTuChiTietService.InsertRangeAsync(model, model.HoaDonChiTiets);

                    bool result = await _hoaDonDienTuService.UpdateAsync(model);

                    if (result)
                    {
                        var _currentUser = await _userRespositories.GetById(HttpContext.User.GetUserId());
                        var nk = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = model.HoaDonDienTuId,
                            LoaiThaoTac = (int)LoaiThaoTac.SuaHoaDon,
                            MoTa = "Sửa hóa đơn lúc " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            NguoiThucHienId = _currentUser.UserId,
                            NgayGio = DateTime.Now,
                            HasError = !result
                        };
                        await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(nk);
                    }
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

        [HttpPost("XemHoaDonHangLoat")]
        public IActionResult XemHoaDonHangLoat(List<string> fileArray)
        {
            var result = _hoaDonDienTuService.XemHoaDonDongLoat(fileArray);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("XemHoaDonHangLoat2")]
        public IActionResult XemHoaDonDongLoat2(List<string> fileArray)
        {
            var result = _hoaDonDienTuService.XemHoaDonDongLoat2(fileArray);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(id);
                    bool result = await _hoaDonDienTuService.DeleteAsync(id);

                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateSoHoaDon")]
        public async Task<IActionResult> CreateSoHoaDon(HoaDonDienTuViewModel mhd)
        {
            var result = await _hoaDonDienTuService.CreateSoHoaDon(mhd);
            return Ok(result);
        }

        [HttpGet("CreateSoCTXoaBoHoaDon")]
        public async Task<IActionResult> CreateSoCTXoaBoHoaDon()
        {
            var result = await _hoaDonDienTuService.CreateSoCTXoaBoHoaDon();
            return Ok(new { Data = result });
        }

        [HttpGet("CreateSoBienBanXoaBoHoaDon")]
        public async Task<IActionResult> CreateSoBienBanXoaBoHoaDon()
        {
            var result = await _hoaDonDienTuService.CreateSoBienBanXoaBoHoaDon();
            return Ok(new { Data = result });
        }

        [HttpPost("CapPhatSoHoaDon")]
        public async Task<IActionResult> CapPhatSoHoaDon(CapPhatSoHoaDonParam @params)
        {
            var result = await _hoaDonDienTuService.CapPhatSoHoaDon(@params.Model, @params.SoHoaDon);
            return Ok(result);
        }

        [HttpPost("CapPhatSoHoaDonHangLoat")]
        public async Task<IActionResult> CapPhatSoHoaDonHangLoat(CapPhatSoHoaDonHangLoatParam @params)
        {
            var result = await _hoaDonDienTuService.CapPhatSoHoaDonHangLoat(@params.Models, @params.SoHoaDons);
            return Ok(result);
        }

        [HttpPost("TaiHoaDon")]
        public IActionResult TaiHoaDon(HoaDonDienTuViewModel hoaDonDienTu)
        {
            var result = _hoaDonDienTuService.TaiHoaDon(hoaDonDienTu);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("TaiHoaDon_TraCuu")]
        public async Task<IActionResult> TaiHoaDon_TraCuu(HoaDonDienTuViewModel hoaDonDienTu)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hoaDonDienTu.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = _hoaDonDienTuService.TaiHoaDon(hoaDonDienTu);
            return Ok(result);
        }

        [HttpPost("ThemNhatKyThaoTacHoaDon")]
        public async Task<IActionResult> ThemNhatKyThaoTacHoaDon(NhatKyThaoTacHoaDonViewModel model)
        {
            var result = await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(model);
            return Ok(result);
        }
        [HttpPost("ConvertHoaDonToFilePDF")]
        public async Task<IActionResult> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.ConvertHoaDonToFilePDF(hd);

                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return Ok(null);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("ConvertHoaDonToFilePDF_TraCuu")]
        public async Task<IActionResult> ConvertHoaDonToFilePDF_TraCuu(HoaDonDienTuViewModel hd)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hd.HoaDonDienTuId);

            //User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            //User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = _hoaDonDienTuService.ConvertHoaDonToFilePDF_TraCuu(hd, companyModel.DataBaseName);
            return Ok(result);
        }

        [HttpGet("GetTrangThaiHoaDon")]
        public async Task<IActionResult> GetTrangThaiHoaDon()
        {
            var result = await _hoaDonDienTuService.GetTrangThaiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetTrangThaiGuiHoaDon")]
        public async Task<IActionResult> GetTrangThaiGuiHoaDon()
        {
            var result = await _hoaDonDienTuService.GetTrangThaiGuiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetTreeTrangThai")]
        public async Task<IActionResult> GetTreeTrangThai(int LoaiHoaDon, DateTime fromDate, DateTime toDate)
        {
            var result = await _hoaDonDienTuService.GetTreeTrangThai(LoaiHoaDon, fromDate, toDate);
            return Ok(result);
        }

        [HttpPost("DeleteRangeHoaDonDienTu")]
        public IActionResult DeleteRangeHoaDonDienTu(DeleteRangeHDDTParams @params)
        {
            var result = _hoaDonDienTuService.DeleteRangeHoaDonDienTuAsync(@params.ListHoaDon);
            return Ok(result);
        }

        [HttpPost("GateForWebSocket")]
        public async Task<IActionResult> GateForWebSocket(ParamPhatHanhHD @params)
        {
            if (@params.HoaDon == null || string.IsNullOrEmpty(@params.HoaDonDienTuId))
            {
                return BadRequest();
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _hoaDonDienTuService.GateForWebSocket(@params))
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else transaction.Rollback();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("SendMailAsync")]
        public async Task<IActionResult> SendMailAsync(ParamsSendMail hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.SendEmailAsync(hd);
                    if (result == true)
                        transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [AllowAnonymous]
        [HttpPost("SendEmailThongTinHoaDon")]
        public async Task<IActionResult> SendEmailThongTinHoaDon(ParamsSendMailThongTinHoaDon hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.SendEmailThongTinHoaDonAsync(hd);
                    if (result == true)
                        transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("ConvertHoaDonToHoaDonGiay")]
        public async Task<IActionResult> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.ConvertHoaDonToHoaDonGiay(hd);
                if (result != null)
                {
                    transaction.Commit();
                    return File(result.Bytes, result.ContentType, result.FileName);
                }
                else transaction.Rollback();

                return Ok(result);
            }
        }

        [HttpPost("ConvertHoaDonToHoaDonGiay2")]
        public async Task<IActionResult> ConvertHoaDonToHoaDonGiay2(ParamsChuyenDoiThanhHDGiay hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.ConvertHoaDonToHoaDonGiay(hd);
                if (result != null)
                {
                    transaction.Commit();
                    return Ok(new { result = hd.FilePath });
                }
                else transaction.Rollback();

                return Ok(result);
            }
        }

        [AllowAnonymous]
        [HttpPost("ConvertHoaDonToHoaDonGiay_TraCuu")]
        public async Task<IActionResult> ConvertHoaDonToHoaDonGiay_TraCuu(ParamsChuyenDoiThanhHDGiay hd)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hd.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.ConvertHoaDonToHoaDonGiay(hd);
                if (result != null)
                {
                    transaction.Commit();
                    return File(result.Bytes, result.ContentType, result.FileName);
                }
                else transaction.Rollback();

                return Ok(result);
            }
        }

        [HttpGet("XemLichSuHoaDon/{id}")]
        public async Task<IActionResult> XemLichSuHoaDon(string id)
        {
            var result = await _hoaDonDienTuService.XemLichSuHoaDon(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("TraCuuByMa/{MaTraCuu}")]
        public async Task<IActionResult> TraCuuByMa(string MaTraCuu)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByLookupCodeAsync(MaTraCuu);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = await _traCuuService.TraCuuByMa(MaTraCuu);
            var res = _hoaDonDienTuService.ConvertHoaDonToFilePDF_TraCuu(result, companyModel.DataBaseName);
            return Ok(new { data = result, path = res.FilePDF });
        }

        [AllowAnonymous]
        [HttpPost("GetMaTraCuuInXml")]
        public async Task<IActionResult> GetMaTraCuuInXml([FromForm] IFormFile file)
        {
            var result = await _traCuuService.GetMaTraCuuInXml(file);
            return Ok(result);
        }

        [HttpPost("TienLuiChungTu")]
        public async Task<IActionResult> TienLuiChungTu(TienLuiViewModel model)
        {
            var result = await _hoaDonDienTuService.TienLuiChungTuAsync(model);
            return Ok(result);
        }

        [HttpGet("GetBienBanXoaBoHoaDon/{id}")]
        public async Task<IActionResult> GetBienBanXoaBoHoaDon(string id)
        {
            var result = await _hoaDonDienTuService.GetBienBanXoaBoHoaDon(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetBienBanXoaBoHoaDonById/{id}")]
        public async Task<IActionResult> GetBienBanXoaBoHoaDonById(string id)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByBienBanXoaBoIdAsync(id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.GetBienBanXoaBoById(id);
            return Ok(result);
        }

        [HttpPost("SaveBienBanXoaHoaDon")]
        public async Task<IActionResult> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.SaveBienBanXoaHoaDon(model);
                transaction.Commit();
                return Ok(result);
            }
        }

        [HttpPost("CapNhatBienBanXoaBoHoaDon")]
        public async Task<IActionResult> CapNhatBienBanXoaBoHoaDon(BienBanXoaBoViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.CapNhatBienBanXoaBoHoaDon(model);
                if (result)
                {
                    transaction.Commit();
                }
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [HttpDelete("DeleteBienBanXoaHoaDon/{Id}")]
        public async Task<IActionResult> DeleteBienBanXoaHoaDon(string Id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var entity = await _db.BienBanXoaBos.FirstOrDefaultAsync(x => x.Id == Id);
                HoaDonDienTuViewModel entityHD = null;
                if (entity.HoaDonDienTuId != null && entity.ThongTinHoaDonId == null)
                {
                    entityHD = await _hoaDonDienTuService.GetByIdAsync(entity.HoaDonDienTuId);
                }

                var result = await _hoaDonDienTuService.DeleteBienBanXoaHoaDon(Id);
                if (result)
                {
                    if (entityHD != null)
                    {
                        entityHD.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaLap;
                        if (await _hoaDonDienTuService.UpdateAsync(entityHD))
                            transaction.Commit();
                        else
                        {
                            result = false;
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        //trường hợp xóa biên bản hủy hóa đơn ở hóa đơn bên ngoài
                        transaction.Commit();
                    }
                }
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [AllowAnonymous]
        [HttpPost("KyBienBanXoaBo")]
        public async Task<IActionResult> KyBienBanXoaBo(ParamKyBienBanHuyHoaDon @params)
        {
            if (@params.BienBan == null)
            {
                return BadRequest();
            }

            CompanyModel companyModel = await _databaseService.GetDetailByBienBanXoaBoIdAsync(@params.BienBan.Id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _hoaDonDienTuService.GateForWebSocket(@params))
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else transaction.Rollback();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [AllowAnonymous]
        [HttpPost("ConvertBienBanXoaBoToFilePDF")]
        public async Task<IActionResult> ConvertBienBanXoaBoToFilePDF(BienBanXoaBoViewModel bb)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByBienBanXoaBoIdAsync(bb.Id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.ConvertBienBanXoaHoaDon(bb);
            return Ok(result);
        }

        [HttpPost("XoaBoHoaDon")]
        public async Task<IActionResult> XoaBoHoaDon(ParamXoaBoHoaDon @params)
        {
            if (@params.HoaDon == null)
            {
                return Ok(false);
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _hoaDonDienTuService.XoaBoHoaDon(@params))
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpGet("GetStatusDaThayTheHoaDon/{HoaDonId}")]
        public async Task<IActionResult> GetStatusDaThayTheHoaDon(string HoaDonId)
        {
            var result = await _hoaDonDienTuService.GetStatusDaThayTheHoaDon(HoaDonId);
            return Ok(result);
        }

        [HttpGet("GetDSRutGonBoKyHieuHoaDon")]
        public async Task<IActionResult> GetDSRutGonBoKyHieuHoaDon()
        {
            var result = await _hoaDonDienTuService.GetDSRutGonBoKyHieuHoaDonAsync();
            return Ok(result);
        }

        [HttpPost("GetDSHoaDonDeXoaBo")]
        public async Task<IActionResult> GetDSHoaDonDeXoaBo(HoaDonParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetDSHoaDonDeXoaBo(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPut("UpdateTrangThaiQuyTrinh")]
        public async Task<IActionResult> UpdateTrangThaiQuyTrinh(HoaDonDienTuViewModel model)
        {
            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(model.HoaDonDienTuId, (TrangThaiQuyTrinh)model.TrangThaiQuyTrinh);
            return Ok(true);
        }

        [HttpDelete("RemoveDigitalSignature/{id}")]
        public async Task<IActionResult> RemoveDigitalSignature(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.RemoveDigitalSignatureAsync(id);
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

        [AllowAnonymous]
        [HttpPost("ReloadPDF")]
        public async Task<IActionResult> ReloadPDF(ReloadPDFParams @params)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(@params.MaSoThue);
            if (companyModel == null)
            {
                return Ok(new ReloadPDFResult
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
                    var result = await _hoaDonDienTuService.ReloadPDFAsync(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Ok(new ReloadPDFResult
                    {
                        Status = false,
                        Message = "Exception: " + e.Message
                    });
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("DownloadXML")]
        public async Task<IActionResult> DowloadXML(string id, string maSoThue)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(maSoThue);
            if (companyModel == null)
            {
                return NotFound();
            }

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = await _hoaDonDienTuService.DowloadXMLAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return File(result.Bytes, result.ContentType, result.FileName);
        }
    }
}
