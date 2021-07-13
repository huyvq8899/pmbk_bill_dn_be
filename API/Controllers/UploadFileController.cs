using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels.DanhMuc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UploadFileController : BaseController
    {
        private readonly Datacontext _datacontext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadFileController(Datacontext datacontext, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _datacontext = datacontext;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("InsertFileAttaches")]
        public async Task<IActionResult> InsertFileAttaches([FromForm] TaiLieuDinhKemViewModel model)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = await uploadFile.InsertFileAttaches(model, _datacontext);
            return Ok(result);
        }

        [HttpPost("DeleteFileAttach")]
        public IActionResult DeleteFileAttach(TaiLieuDinhKemViewModel model)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = uploadFile.DeleteFileAttach(model);
            return Ok(result);
        }
    }
}
