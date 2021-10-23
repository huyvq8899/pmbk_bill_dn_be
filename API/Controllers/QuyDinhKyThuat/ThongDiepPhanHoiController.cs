using API.Extentions;
using DLL;
using DLL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepPhanHoiController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IDatabaseService _databaseService;
        private readonly IQuyDinhKyThuatService _quyDinhKyThuatService;

        public ThongDiepPhanHoiController(Datacontext datacontext, IDatabaseService databaseService, IQuyDinhKyThuatService quyDinhKyThuatService)
        {
            _databaseService = databaseService;
            _db = datacontext;
            _quyDinhKyThuatService = quyDinhKyThuatService;
        }

        [AllowAnonymous]
        [HttpPost("GetPhanHoiTuCQT")]
        public async Task<IActionResult> GetPhanHoiTuCQT(ThongDiepPhanHoiParams model)
        {
            // get ttchung
            var ttChung = XmlHelper.GetTTChungFromBase64(model.DataXML);

            // switch database
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(ttChung.MST);
            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            model.MLTDiep = int.Parse(ttChung.MLTDiep);

            await _quyDinhKyThuatService.InsertThongDiepNhanAsync(model);

            return Ok(true);
        }
    }
}
