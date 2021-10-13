using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces;
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
        private readonly IXMLInvoiceService _IXMLInvoiceService;
        public QuyDinhKyThuat_PhanII_I_1Controller(
            IXMLInvoiceService IXMLInvoiceService
        ) 
        {
            _IXMLInvoiceService = IXMLInvoiceService;
        }

        [HttpPost("GetXMLToKhaiDangKyKhongUyNhiem")]
        public async Task<IActionResult> GetXMLToKhaiDangKyKhongUyNhiem(TKhai tKhai)
        {
            var result = _IXMLInvoiceService.CreateFileXML(tKhai, "QuyDinhKyThuatHDDT_PhanII_I_1");
            return Ok(new { result });
        }

    }
}
