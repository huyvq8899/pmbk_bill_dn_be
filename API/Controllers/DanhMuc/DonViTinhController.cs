using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class DonViTinhController : BaseController
    {
        private readonly IDonViTinhService _donViTinhService;
        public DonViTinhController(IDonViTinhService donViTinhService)
        {
            _donViTinhService = donViTinhService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(DonViTinhParams @params)
        {
            var result = await _donViTinhService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(DonViTinhParams pagingParams)
        {
            var paged = await _donViTinhService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _donViTinhService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(DonViTinhViewModel model)
        {
            var result = await _donViTinhService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(DonViTinhViewModel model)
        {
            var result = await _donViTinhService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _donViTinhService.DeleteAsync(id);
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
    }
}
