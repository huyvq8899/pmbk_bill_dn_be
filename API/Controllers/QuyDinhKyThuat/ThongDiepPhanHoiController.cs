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
using TDiep102 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep;
using TDiep103_1 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep;
using TDiep103_2 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep;

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

            switch (int.Parse(ttChung.MLTDiep))
            {
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                    break;
                case (int)MLTDiep.TDTBKQKTDLHDon:
                    var tDiep204 = DataHelper.ConvertBase64ToObject<TDiep204>(model.DataXML);
                    ////// create thongdiepnhan
                    break;
                case (int)MLTDiep.TBTNToKhai:
                    var tDiep102 = DataHelper.ConvertBase64ToObject<TDiep102>(model.DataXML);
                    return Ok(tDiep102);
                    break;
                case (int)MLTDiep.TBCNToKhai:
                    var tDiep103 = DataHelper.ConvertBase64ToObject<TDiep103_1>(model.DataXML);
                    return Ok(tDiep103);
                    break;
                case (int)MLTDiep.TBCNToKhaiUN:
                    var tDiep104 = DataHelper.ConvertBase64ToObject<TDiep103_2>(model.DataXML);
                    return Ok(tDiep104);
                    break;
                default:
                    break;
            }

            return Ok(true);
        }
    }
}
