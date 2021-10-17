using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.QuyDinhKyThuat;
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
    public class ThongDiepGuiToKhaiDKTTHoaDonController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IQuyDinhKyThuatService _IQuyDinhKyThuatService;
        private readonly IXMLInvoiceService _IXMLInvoiceService;
        public ThongDiepGuiToKhaiDKTTHoaDonController(
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
        public IActionResult GetXMLToKhaiDangKyKhongUyNhiem(ToKhaiParams tKhai)
        {
            var result = _IXMLInvoiceService.CreateFileXML(tKhai.ToKhaiKhongUyNhiem, "QuyDinhKyThuatHDDT_PhanII_I_1");
            return Ok(new { result });
        }

        [HttpPost("GetXMLThongDiepKhongUyNhiem")]
        public IActionResult GetXMLThongDiepKhongUyNhiem(ThongDiepParams tDiep)
        {
            var result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepKhongUyNhiem, "QuyDinhKyThuatHDDT_PhanII_I_8");
            return Ok(new { result });
        }

        [HttpPost("GetXMLToKhaiDangKyUyNhiem")]
        public async Task<IActionResult> GetXMLToKhaiDangKyUyNhiem(ToKhaiParams @params)
        {
            var result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiUyNhiem, "QuyDinhKyThuatHDDT_PhanII_I_2");
            return Ok(new { result });
        }

        [HttpPost("GetXMLThongDiepUyNhiem")]
        public IActionResult GetXMLThongDiepUyNhiem(ThongDiepParams tDiep)
        {
            var result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepUyNhiem, "QuyDinhKyThuatHDDT_PhanII_I_9");
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
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("SuaToKhaiDangKyThongTin")]
        public async Task<IActionResult> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.SuaToKhaiDangKyThongTin(model);
                    if (result == true) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpDelete("XoaToKhai/{Id}")]
        public async Task<IActionResult> XoaToKhai(string Id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.XoaToKhai(Id);
                    if (result == true) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("LuuDuLieuKy")]
        public async Task<IActionResult> LuuDuLieuKy(DuLieuKyToKhaiViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.LuuDuLieuKy(model);
                    if (result == true) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("LuuDuLieuGuiToKhai")]
        public async Task<IActionResult> LuuDuLieuGuiToKhai(TrangThaiGuiToKhaiViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.LuuTrangThaiGuiToKhai(model);
                    if (result == true) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
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


        [HttpGet("GetToKhaiById/{Id}")]
        public async Task<IActionResult> GetToKhaiById(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetToKhaiById(Id);
            return Ok(result);
        }

        [HttpPost("GuiToKhai")]
        public async Task<IActionResult> GetToKhaiById(GuiNhanToKhaiParams @params)
        {
            var result = await _IQuyDinhKyThuatService.GuiToKhai(@params.FileXml, @params.Id);
            return Ok(result);
        }

        [HttpPost("NhanToKhai")]
        public async Task<IActionResult> NhanPhanHoiCQT(GuiNhanToKhaiParams @params)
        {
            var result = await _IQuyDinhKyThuatService.NhanPhanHoiCQT(@params.FileXml, @params.Id);
            return Ok(result);
        }
    }
}
