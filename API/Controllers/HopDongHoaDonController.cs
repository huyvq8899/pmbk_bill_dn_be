using API.Extentions;
using DLL.Constants;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Repositories.Implimentations;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using Services.ViewModels.Params;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopDongHoaDonController : BaseController
    {
        private readonly IDatabaseService _databaseService;
        private readonly IHopDongHoaDonService _hopDongHoaDonService;
        public HopDongHoaDonController(IHopDongHoaDonService hopDongHoaDonService,
            IDatabaseService databaseService)
        {
            _hopDongHoaDonService = hopDongHoaDonService;
            _databaseService = databaseService;
        }

        /// <summary>
        ///API lấy toàn bộ hợp đồng không phân trang
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hopDongHoaDonService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        ///API lấy toàn bộ hợp đồng theo mst
        /// </summary>
        /// <param name="MaSoThue">Mst Khách hàng</param>
        /// <returns></returns>
        [HttpGet("GetHopDongByTaxcode/{maSoThue}")]
        public async Task<IActionResult> GetHopDongByTaxcode(string maSoThue)
        {
            var result = await _hopDongHoaDonService.GetHopDongByTaxcodeAsync(maSoThue);
            return Ok(result);
        }

        /// <summary>
        /// API tính tổng giá trị hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpGet("SumGiaTriHopDong")]
        public async Task<IActionResult> SumGiaTriHopDong()
        {
            var result = await _hopDongHoaDonService.SumGiaTriHopDongAsync();
            return Ok(new { tongTien = result });
        }

        /// <summary>
        /// API lấy thông tin hợp đồng theo Id
        /// </summary>
        /// <param name="id">Hợp đồng hóa đơn Id</param>
        /// <returns></returns>
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _hopDongHoaDonService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// API lấy toàn bộ hợp đồng theo phân trang
        /// </summary>
        /// <param name="pagingParams">Thông tin trong 1 page</param>
        /// <returns></returns>
        [HttpGet("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] PagingParams pagingParams)
        {
            var paged = await _hopDongHoaDonService.GetAllPagingAsync(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        /// <summary>
        /// API insert hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// [AllowAnonymous]
        [AllowAnonymous]
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(List<HopDongHoaDonViewModel> listHDong)
        {
            try
            {
                var result = new List<bool>();

                foreach (var model in listHDong)
                {
                    CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(model.MaSoThue.Trim());
                    if (companyModel != null)
                    {
                        User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                        User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
                        User.AddClaim(ClaimTypeConstants.TAX_CODE, companyModel.TaxCode);

                        result.Add(await _hopDongHoaDonService.CreateAsync(model));
                    }
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// API update thông tin hợp đồng hóa đơn
        /// </summary>
        /// <param name="model">Thông tin cần update</param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(HopDongHoaDonViewModel model)
        {
            try
            {
                var result = await _hopDongHoaDonService.UpdateAsync(model);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}