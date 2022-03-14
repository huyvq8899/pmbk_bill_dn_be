using API.Extentions;
using DLL;
using DLL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class BienBanDieuChinhController : BaseController
    {
        private readonly IBienBanDieuChinhService _bienBanDieuChinhService;
        private readonly IDatabaseService _databaseService;
        private readonly Datacontext _db;

        public BienBanDieuChinhController(IBienBanDieuChinhService bienBanDieuChinhService, Datacontext datacontext, IDatabaseService databaseService)
        {
            _bienBanDieuChinhService = bienBanDieuChinhService;
            _db = datacontext;
            _databaseService = databaseService;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _bienBanDieuChinhService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Xem biên bản điều chỉnh dưới dạng pdf (phía người bán)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("PreviewBienBan/{id}")]
        public async Task<IActionResult> PreviewBienBan(string id)
        {
            var result = await _bienBanDieuChinhService.PreviewBienBanAsync(id);
            return Ok(new { filePath = result });
        }

        /// <summary>
        /// Xem biên bản điều chỉnh dưới dạng pdf (phía người mua)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("PreviewBienBan_NM/{id}")]
        public async Task<IActionResult> PreviewBienBan_NM(string id)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByBienBanDieuChinhIdAsync(id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _bienBanDieuChinhService.PreviewBienBanAsync(id);
            return Ok(new { filePath = result });
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(BienBanDieuChinhViewModel model)
        {
            var result = await _bienBanDieuChinhService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(BienBanDieuChinhViewModel model)
        {
            var result = await _bienBanDieuChinhService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _bienBanDieuChinhService.DeleteAsync(id);
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

        /// <summary>
        /// Ký biên bản điều chỉnh (phía người mua)
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GateForWebSocket_NM")]
        public async Task<IActionResult> GateForWebSocket_NM(ParamPhatHanhBBDC @params)
        {
            if (string.IsNullOrEmpty(@params.BienBanDieuChinhId))
            {
                return BadRequest();
            }

            CompanyModel companyModel = await _databaseService.GetDetailByBienBanDieuChinhIdAsync(@params.BienBanDieuChinhId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _bienBanDieuChinhService.GateForWebSocket(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        /// <summary>
        /// Ký biên bản điều chỉnh (phía người bán)
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GateForWebSocket")]
        public async Task<IActionResult> GateForWebSocket(ParamPhatHanhBBDC @params)
        {
            if (string.IsNullOrEmpty(@params.BienBanDieuChinhId))
            {
                return BadRequest();
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _bienBanDieuChinhService.GateForWebSocket(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }
    }
}
