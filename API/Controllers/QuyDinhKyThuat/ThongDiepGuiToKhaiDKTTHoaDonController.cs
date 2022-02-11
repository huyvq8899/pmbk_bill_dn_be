using API.Extentions;
using DLL;
using DLL.Constants;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
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
        private readonly IDatabaseService _IDatabaseService;
        public ThongDiepGuiToKhaiDKTTHoaDonController(
            IXMLInvoiceService IXMLInvoiceService,
            IQuyDinhKyThuatService IQuyDinhKyThuatService,
            IDatabaseService IDatabaseService,
            Datacontext db
        )
        {
            _IXMLInvoiceService = IXMLInvoiceService;
            _IDatabaseService = IDatabaseService;
            _IQuyDinhKyThuatService = IQuyDinhKyThuatService;
            _db = db;
        }

        [HttpPost("GetXMLToKhaiDangKyKhongUyNhiem")]
        public IActionResult GetXMLToKhaiDangKyKhongUyNhiem(ToKhaiParams tKhai)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = string.Empty;
            if (!string.IsNullOrEmpty(tKhai.ToKhaiId))
            {
                result = _IXMLInvoiceService.CreateFileXML(tKhai.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, tKhai.ToKhaiId);
            }
            else result = _IXMLInvoiceService.CreateFileXML(tKhai.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            return Ok(new { result });
        }

        [HttpPost("GetXMLThongDiepKhongUyNhiem")]
        public IActionResult GetXMLThongDiepKhongUyNhiem(ThongDiepParams tDiep)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, tDiep.ThongDiepId);
            return Ok(new { result });
        }

        [HttpPost("GetXMLToKhaiDangKyUyNhiem")]
        public IActionResult GetXMLToKhaiDangKyUyNhiem(ToKhaiParams @params)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = string.Empty;
            if (!string.IsNullOrEmpty(@params.ToKhaiId))
            {
                result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, @params.ToKhaiId);
            }
            else result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            return Ok(new { result });
        }

        [HttpPost("GetXMLThongDiepUyNhiem")]
        public IActionResult GetXMLThongDiepUyNhiem(ThongDiepParams tDiep)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            var result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, tDiep.ThongDiepId);
            return Ok(new { result });
        }

        [HttpGet("GetNoiDungThongDiepXMLChuaKy/{thongDiepId}")]
        public async Task<IActionResult> GetNoiDungThongDiepXMLChuaKy(string thongDiepId)
        {
            var result = await _IQuyDinhKyThuatService.GetNoiDungThongDiepXMLChuaKy(thongDiepId);
            result = TextHelper.Base64Encode(result);
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

        [HttpPost("DeleteRangeChungThuSo")]
        public async Task<IActionResult> DeleteRangeChungThuSo(List<string> ids)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IQuyDinhKyThuatService.DeleteRangeChungThuSo(ids);
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
                foreach (var item in paged.Items)
                {
                    if (item.ThongDiepGuiDi == false && item.TrangThaiGui == (TrangThaiGuiThongDiep.ChoPhanHoi))
                    {
                        item.TrangThaiGui = (TrangThaiGuiThongDiep)_IQuyDinhKyThuatService.GetTrangThaiPhanHoiThongDiepNhan(item);
                        item.TenTrangThaiGui = item.TrangThaiGui.GetDescription();
                    }

                    if (item.TrangThaiGui == TrangThaiGuiThongDiep.DaTiepNhan)
                    {
                        if (item.MaLoaiThongDiep == (int)MLTDiep.TBTNToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhaiUN)
                        {
                            item.TenTrangThaiGui = "CQT đã tiếp nhận";
                        }
                    }

                    if (item.TrangThaiGui == TrangThaiGuiThongDiep.TuChoiTiepNhan)
                    {
                        if (item.MaLoaiThongDiep == (int)MLTDiep.TBTNToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhaiUN)
                        {
                            item.TenTrangThaiGui = "CQT không tiếp nhận";
                        }
                    }
                }

                if (pagingParams.TrangThaiGui != -99 && pagingParams.TrangThaiGui != null)
                {
                    paged.Items = paged.Items.Where(x => x.TrangThaiGui == (TrangThaiGuiThongDiep)pagingParams.TrangThaiGui).ToList();
                }
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

        [HttpGet("GetListDangKyUyNhiem/{IdToKhai}")]
        public async Task<IActionResult> GetListDangKyUyNhiem(string IdToKhai)
        {
            var result = await _IQuyDinhKyThuatService.GetListDangKyUyNhiem(IdToKhai);
            return Ok(result);
        }

        [HttpPost("GetListTrungKyHieuTrongHeThong")]
        public IActionResult GetListTrungKyHieuTrongHeThong(List<DangKyUyNhiemViewModel> data)
        {
            var result = _IQuyDinhKyThuatService.GetListTrungKyHieuTrongHeThong(data);
            return Ok(result);
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

        [HttpGet("GetThongDiepThemMoiToKhaiDuocChapNhan")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhaiDuocChapNhan()
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhaiDuocChapNhan();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetThongDiepThemMoiToKhaiDuocChapNhan_TraCuu1/{MaTraCuu}")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhaiDuocChapNhan(string MaTraCuu)
        {
            CompanyModel companyModel = await _IDatabaseService.GetDetailByLookupCodeAsync(MaTraCuu.Trim());

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhaiDuocChapNhan();
            var tk = await _IQuyDinhKyThuatService.GetToKhaiById(result.IdThamChieu);
            return Ok(tk);
        }

        [AllowAnonymous]
        [HttpPost("GetThongDiepThemMoiToKhaiDuocChapNhan_TraCuu2")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhaiDuocChapNhan(KetQuaTraCuuXML input)
        {
            CompanyModel companyModel = await _IDatabaseService.GetDetailBySoHoaDonAsync(input);

            if (companyModel != null)
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

                var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhaiDuocChapNhan();
                var tk = await _IQuyDinhKyThuatService.GetToKhaiById(result.IdThamChieu);
                return Ok(tk);
            }
            else return Ok(null);
        }

        [HttpGet("GetAllThongDiepTraVe/{Id}")]
        public async Task<IActionResult> GetAllThongDiepTraVe(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetAllThongDiepTraVe(Id);
            return Ok(result);
        }

        [HttpGet("GetAllThongDiepTraVeV2/{giaTriTimKiem}/{phanLoai}")]
        public async Task<IActionResult> GetAllThongDiepTraVeV2(string giaTriTimKiem, string phanLoai)
        {
            var result = await _IQuyDinhKyThuatService.GetAllThongDiepTraVeV2(giaTriTimKiem, phanLoai);
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

        [HttpGet("GetTrangThaiGuiPhanHoiTuCQT/{maLoaiThongDiep}")]
        public IActionResult GetTrangThaiGuiPhanHoiTuCQT(int maLoaiThongDiep)
        {
            var result = _IQuyDinhKyThuatService.GetTrangThaiGuiPhanHoiTuCQT(maLoaiThongDiep);
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

        [HttpGet("ThongKeSoLuongThongDiep/{TrangThaiGuiThongDiep}/{CoThongKeSoLuong}")]
        public async Task<IActionResult> ThongKeSoLuongThongDiep(int trangThaiGuiThongDiep, byte coThongKeSoLuong)
        {
            var result = await _IQuyDinhKyThuatService.ThongKeSoLuongThongDiepAsync(trangThaiGuiThongDiep, coThongKeSoLuong);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("UpdateNgayThongBaoToKhai")]
        public async Task<IActionResult> UpdateNgayThongBaoToKhai([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                string dbString = (param.KeyString).Base64Decode();

                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);

                using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var result = await _IQuyDinhKyThuatService.UpdateNgayThongBaoToKhaiAsync();
                        transaction.Commit();
                        return Ok(result);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }
                }
            }

            return Ok(false);
        }
    }
}
