using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Ticket;
using Services.ViewModels.Ticket;
using System.Threading.Tasks;

namespace API.Controllers.Ticket
{
    public class XeController : BaseController
    {
        private readonly IXeService _xeService;

        public XeController(IXeService xeService)
        {
            _xeService = xeService;
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(XeViewModel model)
        {
            var result = await _xeService.Insert(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(XeViewModel model)
        {
            var result = await _xeService.Update(model);
            return Ok(result);
        }


        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _xeService.Delete(Id);
            return Ok(result);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _xeService.GetById(Id);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _xeService.GetAll();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(PagingParams pagingParams)
        {
            var paged = await _xeService.GetAllPaging(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        /// <summary>
        /// Get xe đang hoạt động
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _xeService.GetAllActiveAsync();
            return Ok(result);
        }
    }
}
