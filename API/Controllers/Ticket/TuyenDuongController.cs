using API.Extentions;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Ticket;
using Services.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Ticket
{
    public class TuyenDuongController : BaseController
    {
        private readonly ITuyenDuongService _ITuyenDuongService;

        public TuyenDuongController(ITuyenDuongService ITuyenDuongService)
        {
            _ITuyenDuongService = ITuyenDuongService;
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(TuyenDuongViewModel model)
        {
            var result = await _ITuyenDuongService.Insert(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(TuyenDuongViewModel model)
        {
            var result = await _ITuyenDuongService.Update(model);
            return Ok(result);
        }


        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _ITuyenDuongService.Delete(Id);
            return Ok(result);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _ITuyenDuongService.GetById(Id);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ITuyenDuongService.GetAll();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(PagingParams pagingParams)
        {
            var paged = await _ITuyenDuongService.GetAllPaging(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        /// <summary>
        /// Get tuyến đường đang hoạt động
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _ITuyenDuongService.GetAllActiveAsync();
            return Ok(result);
        }
    }
}
