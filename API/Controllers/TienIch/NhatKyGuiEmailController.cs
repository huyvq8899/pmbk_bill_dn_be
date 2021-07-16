using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using System.Threading.Tasks;

namespace API.Controllers.TienIch
{
    public class NhatKyGuiEmailController : BaseController
    {
        private readonly INhatKyGuiEmailService _nhatKyGuiEmailService;

        public NhatKyGuiEmailController(INhatKyGuiEmailService nhatKyGuiEmailService)
        {
            _nhatKyGuiEmailService = nhatKyGuiEmailService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(NhatKyGuiEmailParams pagingParams)
        {
            var paged = await _nhatKyGuiEmailService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }
    }
}
