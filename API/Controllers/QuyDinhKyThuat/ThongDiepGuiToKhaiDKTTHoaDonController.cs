using API.Extentions;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Services.Helper.Constants;
using Services.Helper.Params;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
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
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = _IXMLInvoiceService.CreateFileXML(tKhai.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            return Ok(new { result });
        }

        [HttpPost("GetXMLThongDiepKhongUyNhiem")]
        public IActionResult GetXMLThongDiepKhongUyNhiem(ThongDiepParams tDiep)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            return Ok(new { result });
        }

        [HttpPost("GetXMLToKhaiDangKyUyNhiem")]
        public IActionResult GetXMLToKhaiDangKyUyNhiem(ToKhaiParams @params)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            return Ok(new { result });
        }

        [HttpPost("GetXMLThongDiepUyNhiem")]
        public IActionResult GetXMLThongDiepUyNhiem(ThongDiepParams tDiep)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
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

        [HttpPost("GetLinkFileXml")]
        public async Task<IActionResult> GetLinkFileXml(ExportParams @params)
        {
            var result = await _IQuyDinhKyThuatService.GetLinkFileXml(@params.ThongDiep, @params.Signed);
            return Ok(result);
        }

        [HttpPost("AddRangeChungThuSo")]
        public async Task<IActionResult> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.AddRangeChungThuSo(models);
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

        [HttpPost("GetAllPagingThongDiepChung")]
        public async Task<IActionResult> GetAllPagingThongDiepChung(ThongDiepChungParams pagingParams)
        {
            var paged = await _IQuyDinhKyThuatService.GetPagingThongDiepChungAsync(pagingParams);
            if (paged != null)
            {
                Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
                return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
            }
            else return Ok(null);
        }

        [HttpPost("InsertThongDiepChung")]
        public async Task<IActionResult> InsertThongDiepChung(ThongDiepChungViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.InsertThongDiepChung(model);
                    if (result != null) transaction.Commit();
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

        [HttpPost("AddRangeDangKyUyNhiem")]
        public async Task<IActionResult> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.AddRangeDangKyUyNhiem(models);
                    if (result) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
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

        [HttpGet("GetListTimKiemTheoThongDiep")]
        public IActionResult GetListTimKiemTheoThongDiep()
        {
            var result = _IQuyDinhKyThuatService.GetListTimKiemTheoThongDiep();
            return Ok(result);
        }

        [HttpGet("GetThongDiepThemMoiToKhai")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhai()
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhai();
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

        [HttpGet("GetListLoaiThongDiepGui")]
        public IActionResult GetListLoaiThongDiepGui()
        {
            var result = _IQuyDinhKyThuatService.GetListLoaiThongDiepGui();
            return Ok(result);
        }

        [HttpPost("GetListToKhaiFromBoKyHieuHoaDon")]
        public async Task<IActionResult> GetListToKhaiFromBoKyHieuHoaDon(ToKhaiParams toKhaiParams)
        {
            var result = await _IQuyDinhKyThuatService.GetListToKhaiFromBoKyHieuHoaDonAsync(toKhaiParams);
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

        [HttpPost("ExportBangKe")]
        public async Task<IActionResult> ExportBangKe(ThongDiepChungParams @params)
        {
            var result = await _IQuyDinhKyThuatService.ExportBangKeAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpGet("GetAllListCTS")]
        public async Task<IActionResult> GetAllListCTS()
        {
            var result = await _IQuyDinhKyThuatService.GetAllListCTS();
            return Ok(result);
        }
    }
}
