using API.Extentions;
using DLL.Constants;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepPhanHoiController : BaseController
    {
        private readonly IDatabaseService _databaseService;

        public ThongDiepPhanHoiController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("GetPhanHoiTuCQT")]
        public async Task<IActionResult> GetPhanHoiTuCQT(ThongDiepPhanHoiParams model)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(model.MST);
            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            switch (model.MTDTChieu)
            {
                ////////////////////////////////////////////////////////////
                default:
                    break;
            }

            return Ok(true);
        }
    }
}
