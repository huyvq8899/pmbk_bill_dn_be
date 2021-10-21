using API.Extentions;
using DLL;
using DLL.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.ViewModels.XML;
using System.Threading.Tasks;

using TDiep204 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepPhanHoiController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IDatabaseService _databaseService;

        public ThongDiepPhanHoiController(Datacontext datacontext, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _db = datacontext;
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

            // get thongdiep gui

            switch (int.Parse(ttChung.MLTDiep))
            {
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                    break;
                case (int)MLTDiep.TDTBKQKTDLHDon:
                    var tDiep204 = DataHelper.ConvertBase64ToObject<TDiep204>(model.DataXML);
                    ////// create thongdiepnhan
                    break;
                default:
                    break;
            }

            return Ok(true);
        }
    }
}
