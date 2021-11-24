using API.Extentions;
using DLL;
using DLL.Constants;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepPhanHoiController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IDatabaseService _databaseService;
        private readonly IQuyDinhKyThuatService _quyDinhKyThuatService;
        private readonly IDuLieuGuiHDDTService _duLieuGuiHDDTService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ThongDiepPhanHoiController(
            Datacontext datacontext,
            IDatabaseService databaseService,
            IQuyDinhKyThuatService quyDinhKyThuatService,
            IDuLieuGuiHDDTService duLieuGuiHDDTService,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment)
        {
            _databaseService = databaseService;
            _db = datacontext;
            _quyDinhKyThuatService = quyDinhKyThuatService;
            _duLieuGuiHDDTService = duLieuGuiHDDTService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        [HttpPost("GetPhanHoiTuCQT")]
        public async Task<IActionResult> GetPhanHoiTuCQT(ThongDiepPhanHoiParams model)
        {
            // Decode xml
            model.DataXML = TextHelper.Base64Decode(model.DataXML);

            // Get information normal
            var ttChung = XmlHelper.GetTTChungFromStringXML(model.DataXML);

            // Switch database
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(ttChung.MST);
            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            
            model.MLTDiep = int.Parse(ttChung.MLTDiep);

            model.ThongDiepId = ttChung.MTDTChieu;

            // Handle message
            await _quyDinhKyThuatService.InsertThongDiepNhanAsync(model);

            return Ok(true);
        }

        //[AllowAnonymous]
        //[HttpPost("CreateThongDiepPhanHoi")]
        //public async Task<IActionResult> CreateThongDiepPhanHoi(ThongDiepPhanHoiParams model)
        //{
        //    // get ttchung
        //    var ttChung = XmlHelper.GetTTChungFromBase64(model.DataXML);

        //    // switch database
        //    CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(ttChung.MST);
        //    User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
        //    User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
        //    model.MLTDiep = int.Parse(ttChung.MLTDiep);

        //    var result = _duLieuGuiHDDTService.CreateThongDiepPhanHoi(model);
        //    if (result != null)
        //    {
        //        return File(result.Bytes, result.ContentType, result.FileName);
        //    }
        //    return Ok(result);
        //}

        [HttpPost("GetNoiDungThongDiepPhanHoi")]
        public async Task<IActionResult> GetNoiDungThongDiepPhanHoi(ThongDiepChungViewModel model)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }

            string filePath = Path.Combine(fullFolderPath, model.FileXML);
            string encoder = filePath.EncodeFile();

            switch (model.MaLoaiThongDiep)
            {
                case (int)MLTDiep.TBTNToKhai:
                    var td102 = _quyDinhKyThuatService.ConvertToThongDiepTiepNhan(encoder);
                    return Ok(td102);
                case (int)MLTDiep.TBCNToKhai:
                    var td103 = _quyDinhKyThuatService.ConvertToThongDiepKUNCQT(encoder);
                    return Ok(td103);
                case (int)MLTDiep.TBCNToKhaiUN:
                    var td104 = _quyDinhKyThuatService.ConvertToThongDiepUNCQT(encoder);
                    return Ok(td104);
                default:
                    return Ok(null);
            }
        }
    }
}
