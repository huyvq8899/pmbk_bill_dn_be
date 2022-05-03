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

        [HttpPost("InsertFileMauHoaDon")]
        public async Task<IActionResult> InsertFileMauHoaDon([FromForm] MauHoaDonUploadImage model)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = await uploadFile.InsertFileMauHoaDon(model, _datacontext);
            return Ok(result);
        }

        [HttpPost("DeleteFileAttach")]
        public async Task<IActionResult> DeleteFileAttach(TaiLieuDinhKemViewModel model)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = await uploadFile.DeleteFileAttach(model, _datacontext);
            return Ok(result);
        }

        [HttpGet("GetFilesById/{Id}")]
        public async Task<IActionResult> GetFilesById(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = await uploadFile.GetFilesById(id, _datacontext);
            return Ok(result);
        }
        /// <summary>
        /// Kiểm tra xem file đã tồn tại hây chưa, nếu chưa tồn tại thì generate file từ byte
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("CheckExistsFilesById/{Id}")]
        public async Task<IActionResult> CheckExistsFilesById(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            var result = await uploadFile.CheckExistsFilesById(id, _datacontext);
            return Ok(result);
        }
    }
}
