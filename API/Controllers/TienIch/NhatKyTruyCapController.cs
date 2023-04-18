using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;

namespace API.Controllers.TienIch
{
    public class NhatKyTruyCapController : BaseController
    {
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;

        public NhatKyTruyCapController(INhatKyTruyCapService nhatKyTruyCapService)
        {
            _nhatKyTruyCapService = nhatKyTruyCapService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(NhatKyTruyCapParams pagingParams)
        {
            var paged = await _nhatKyTruyCapService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _nhatKyTruyCapService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(NhatKyTruyCapViewModel model)
        {
            var result = await _nhatKyTruyCapService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(NhatKyTruyCapParams @params)
        {
            var result = await _nhatKyTruyCapService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }
        /// <summary>
        /// Lấy nhật ký truy cập theo RefId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByRefId/{Id}")]
        public async Task<IActionResult> GetByRefId(string id)
        {
            var result = await _nhatKyTruyCapService.GetByRefIdAsync(id);
            return Ok(result);
        }
    }
}
