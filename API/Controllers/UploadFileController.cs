using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ViewModels;

namespace API.Controllers
{
    public class UploadFileController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadFileController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("InsertFileAttaches")]
        public IActionResult InsertFileAttaches([FromForm] UploadFileViewModel model)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = uploadFile.InsertFileAttaches(model);
            return Ok(result);
        }

        [HttpPost("DeleteFileAttach")]
        public IActionResult DeleteFileAttach(UploadFileViewModel model)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = uploadFile.DeleteFileAttach(model);
            return Ok(result);
        }
    }
}
