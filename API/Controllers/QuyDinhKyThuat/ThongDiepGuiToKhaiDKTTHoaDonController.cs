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
                    if (result != null) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(null);
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

        [HttpGet("GetXMLToKhaiDaKy/{Id}")]
        public async Task<IActionResult> GetXMLToKhaiDaKy(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetXMLDaKy(Id);
            return Ok(new { result });
        }

        [HttpPost("GetAllPagingThongDiepChung")]
        public async Task<IActionResult> GetAllPagingThongDiepChung(ThongDiepChungParams pagingParams)
        {
            var paged = await _IQuyDinhKyThuatService.GetPagingThongDiepChungAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("InsertThongDiepChung")]
        public async Task<IActionResult> InsertThongDiepChung(ThongDiepChungViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.InsertThongDiepChung(model);
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

        [HttpPost("UpdateThongDiepChung")]
        public async Task<IActionResult> UpdateThongDiepChung(ThongDiepChungViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.UpdateThongDiepChung(model);
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

        [HttpDelete("DeleteThongDiepChung/{Id}")]
        public async Task<IActionResult> DeleteThongDiepChung(string Id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.DeleteThongDiepChung(Id);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("GetThongDiepChungById/{Id}")]
        public async Task<IActionResult> GetThongDiepChungById(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepChungById(Id);
            return Ok(result);
        }

        [HttpGet("GetAllThongDiepTraVe/{Id}")]
        public async Task<IActionResult> GetAllThongDiepTraVe(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetAllThongDiepTraVe(Id);
            return Ok(result);
        }

        [HttpGet("GetThongDiepByThamChieu/{Id}")]
        public async Task<IActionResult> GetThongDiepByThamChieu(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepByThamChieu(Id);
            return Ok(result);
        }

        [HttpGet("GetToKhaiById/{Id}")]
        public async Task<IActionResult> GetToKhaiById(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetToKhaiById(Id);
            return Ok(result);
        }

        [HttpGet("GetLanThuMax")]
        public async Task<IActionResult> GetLanThuMax(int MaLoaiThongDiep)
        {
            var result = await _IQuyDinhKyThuatService.GetLanThuMax(MaLoaiThongDiep);
            return Ok(result);
        }

        [HttpPost("GetLanGuiMax")]
        public async Task<IActionResult> GetLanGuiMax(ThongDiepChungViewModel td)
        {
            var result = await _IQuyDinhKyThuatService.GetLanGuiMax(td);
            return Ok(result);
        }

        [HttpPost("GuiToKhai")]
        public async Task<IActionResult> GuiToKhai(GuiNhanToKhaiParams @params)
        {
            var result = await _IQuyDinhKyThuatService.GuiToKhai(@params.FileXml, @params.Id, @params.MaThongDiep, @params.MST);
            return Ok(result);
        }

        [HttpPost("NhanToKhai")]
        public async Task<IActionResult> NhanPhanHoiCQT(GuiNhanToKhaiParams @params)
        {
            var result = await _IQuyDinhKyThuatService.NhanPhanHoiCQT(@params.FileXml, @params.Id);
            return Ok(result);
        }

        [HttpPost("ConvertToThongDiepTiepNhan")]
        public IActionResult ConvertToThongDiepTiepNhan(string encodedContent)
        {
            var result = _IQuyDinhKyThuatService.ConvertToThongDiepTiepNhan(encodedContent);
            return Ok(result);
        }

        [HttpPost("ThongDiepDaGui")]
        public async Task<IActionResult> ThongDiepDaGui(ThongDiepChungViewModel td)
        {
            var result = await _IQuyDinhKyThuatService.ThongDiepDaGui(td);
            return Ok(result);
        }

        [HttpGet("GetListLoaiThongDiepNhan")]
        public IActionResult GetListLoaiThongDiepNhan()
        {
            var result = _IQuyDinhKyThuatService.GetListLoaiThongDiepNhan();
            return Ok(result);
        }

        [HttpPost("ConvertToThongDiepKUNCQT")]
        public IActionResult ConvertToThongDiepKUNCQT(string encodedContent)
        {
            var result = _IQuyDinhKyThuatService.ConvertToThongDiepKUNCQT(encodedContent);
            return Ok(result);
        }

        [HttpPost("ConvertToThongDiepUNCQT")]
        public IActionResult ConvertToThongDiepUNCQT(string encodedContent)
        {
            var result = _IQuyDinhKyThuatService.ConvertToThongDiepUNCQT(encodedContent);
            return Ok(result);
        }

        [HttpGet("ShowThongDiepFromFileById/{id}")]
        public async Task<IActionResult> ShowThongDiepFromFileById(string id)
        {
            var result = await _IQuyDinhKyThuatService.ShowThongDiepFromFileByIdAsync(id);
            return Ok(result);
        }
    }
}
