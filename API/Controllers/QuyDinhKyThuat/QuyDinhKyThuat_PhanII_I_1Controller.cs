using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuyDinhKyThuat_PhanII_I_1Controller : BaseController
    {
        private readonly Datacontext _db;
        private readonly IQuyDinhKyThuatService _IQuyDinhKyThuatService;
        private readonly IXMLInvoiceService _IXMLInvoiceService;
        public QuyDinhKyThuat_PhanII_I_1Controller(
            IXMLInvoiceService IXMLInvoiceService,
            IQuyDinhKyThuatService IQuyDinhKyThuatService,
            Datacontext db
        ) 
        {
            _IXMLInvoiceService = IXMLInvoiceService;
            _IQuyDinhKyThuatService = IQuyDinhKyThuatService;
            _db = db;
        }

        [HttpPost("GetXMLToKhaiDangKyKhongUyNhiem")]
        public async Task<IActionResult> GetXMLToKhaiDangKyKhongUyNhiem(TKhai tKhai)
        {
            var result = _IXMLInvoiceService.CreateFileXML(tKhai, "QuyDinhKyThuatHDDT_PhanII_I_1");
            return Ok(new { result });
        }

        [HttpPost("LuuToKhaiDangKyThongTin")]
        public async Task<IActionResult> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.LuuToKhaiDangKyThongTin(model);
                    if (result == true) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(PagingParams pagingParams)
        {
            var paged = await _IQuyDinhKyThuatService.GetPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }
    }
}
