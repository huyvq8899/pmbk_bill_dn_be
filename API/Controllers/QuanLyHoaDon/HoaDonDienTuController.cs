using API.Extentions;
using DLL;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.FormActions;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class HoaDonDienTuController : BaseController
    {
        IHoaDonDienTuService _hoaDonDienTuService;
        IHoaDonDienTuChiTietService _hoaDonDienTuChiTietService;
        IUserRespositories _userRespositories;
        //IThamChieuService _thamChieuService;
        Datacontext _db;

        public HoaDonDienTuController(
            IHoaDonDienTuService hoaDonDienTuService,
            IHoaDonDienTuChiTietService hoaDonDienTuChiTietService,
            IUserRespositories userRespositories,
            //IThamChieuService thamChieuService,
            Datacontext db
        )
        {
            _hoaDonDienTuService = hoaDonDienTuService;
            _hoaDonDienTuChiTietService = hoaDonDienTuChiTietService;
            _userRespositories = userRespositories;
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
            var paged = await _hoaDonDienTuService.GetAllPagingAsync(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
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
            var paged = await _hoaDonDienTuService.GetAllPagingHoaDonDieuChinhAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetChiTietHoaDon/{id}")]
        public async Task<IActionResult> GetChiTietHoaDon(string id)
        {
            var result = await _hoaDonDienTuChiTietService.GetChiTietHoaDonAsync(id);
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
            return Ok(result);
        }

        [HttpPost("ExportExcelBangKeChiTiet")]
        public async Task<IActionResult> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params)
        {
            var result = await _hoaDonDienTuService.ExportExcelBangKeChiTiet(@params);
            return Ok(new { path = result });
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    List<HoaDonDienTuChiTietViewModel> hoaDonDienTuChiTiets = model.HoaDonChiTiets;

                    HoaDonDienTuViewModel result = await _hoaDonDienTuService.InsertAsync(model);
                    if (result != null)
                    {
                        var models = await _hoaDonDienTuChiTietService.InsertRangeAsync(result, hoaDonDienTuChiTiets);
                        if (models.Count != hoaDonDienTuChiTiets.Count)
                        {
                            transaction.Rollback();
                            return Ok(false);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }
                    //tham chiếu
                    //if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                    //    await _thamChieuService.UpdateRangeAsync(result.HoaDonDienTuId, result.SoHoaDon, BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG, model.ThamChieus);
                    //else await _thamChieuService.UpdateRangeAsync(result.HoaDonDienTuId, result.SoHoaDon, BusinessOfType.HOA_DON_BAN_HANG, model.ThamChieus);

                    //
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
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


                    //tham chiếu
                    //if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                    //    await _thamChieuService.UpdateRangeAsync(model.HoaDonDienTuId, model.SoHoaDon, BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG, model.ThamChieus);
                    //else await _thamChieuService.UpdateRangeAsync(model.HoaDonDienTuId, model.SoHoaDon, BusinessOfType.HOA_DON_BAN_HANG, model.ThamChieus);

                    //

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
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(id);
                    //await _thamChieuService.DeleteRangeAsync(id);

                    bool result = await _hoaDonDienTuService.DeleteAsync(id);
                    var _currentUser = await _userRespositories.GetById(HttpContext.User.GetUserId());
                    var nk = new NhatKyThaoTacHoaDonViewModel
                    {
                        HoaDonDienTuId = id,
                        LoaiThaoTac = (int)LoaiThaoTac.XoaHoaDon,
                        MoTa = "Xóa hóa đơn lúc " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                        NguoiThucHienId = _currentUser.UserId,
                        NgayGio = DateTime.Now,
                        HasError = !result
                    };
                    await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(nk);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    throw;
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

        [HttpPost("ConvertHoaDonToFilePDF")]
        public async Task<IActionResult> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd)
        {
            var result = await _hoaDonDienTuService.ConvertHoaDonToFilePDF(hd);
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
        public async Task<IActionResult> DeleteRangeHoaDonDienTu(List<HoaDonDienTuViewModel> list)
        {
            var result = await _hoaDonDienTuService.DeleteRangeHoaDonDienTuAsync(list);
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
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
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
                    await _hoaDonDienTuService.SendEmailAsync(hd);
                    transaction.Commit();
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
                    transaction.Rollback();
                    throw;
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
                if (result.ThanhCong)
                {
                    transaction.Commit();
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

        [HttpPost("SaveBienBanXoaHoaDon")]
        public async Task<IActionResult> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.SaveBienBanXoaHoaDon(model);
                if (result)
                {
                    transaction.Commit();
                }
                else transaction.Rollback();
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
                var result = await _hoaDonDienTuService.DeleteBienBanXoaHoaDon(Id);
                if (result)
                {
                    transaction.Commit();
                }
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [HttpPost("KyBienBanXoaBo")]
        public async Task<IActionResult> KyBienBanXoaBo(ParamKyBienBanHuyHoaDon @params)
        {
            if (@params.BienBan == null)
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
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }


        [HttpPost("ConvertBienBanXoaBoToFilePDF")]
        public async Task<IActionResult> ConvertBienBanXoaBoToFilePDF(BienBanXoaBoViewModel bb)
        {
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
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

    }
}
