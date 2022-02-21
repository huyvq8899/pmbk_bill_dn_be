using API.Extentions;
using DLL;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using System;
using System.Threading.Tasks;
using Services.Helper;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IDuLieuGuiHDDTService _thongDiepGuiHDDTKhongMaService;
        private readonly IQuyDinhKyThuatService _IQuyDinhKyThuatService;

        public ThongDiepGuiDuLieuHDDTController(
            Datacontext datacontext,
            IQuyDinhKyThuatService IQuyDinhKyThuatService,
            IDuLieuGuiHDDTService thongDiepGuiHDDTKhongMaService)
        {
            _db = datacontext;
            _thongDiepGuiHDDTKhongMaService = thongDiepGuiHDDTKhongMaService;
            _IQuyDinhKyThuatService = IQuyDinhKyThuatService;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetByIdAsync(Id);
            return Ok(result);
        }

        [HttpPost("GetByHoaDonDienTuId")]
        public async Task<IActionResult> GetByHoaDonDienTuId(ThongDiepChungParams pagingParams)
        {
            var paged = await _thongDiepGuiHDDTKhongMaService.GetByHoaDonDienTuIdAsync(pagingParams);
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
                return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
            }
            else return Ok(null);
        }

        [HttpGet("GetAllThongDiepTraVeInTransLogs/{id}")]
        public async Task<IActionResult> GetAllThongDiepTraVeInTransLogs(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetAllThongDiepTraVeInTransLogsAsync(id);
            return Ok(new { result });
        }
        [HttpGet("GetThongDiepTraVeInTransLogs/{id}")]
        public async Task<IActionResult> GetThongDiepTraVeInTransLogs(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetThongDiepTraVeInTransLogsAsync(id);
            if (result != null)
            {
                return Ok(new { result });
            }
            else return Ok(null);
        }
        [AllowAnonymous]
        [HttpPost("GuiThongDiepKiemTraDuLieuHoaDon")]
        public IActionResult GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.GuiThongDiepKiemTraDuLieuHoaDon(@params);
            return Ok(new { result });
        }

        [AllowAnonymous]
        [HttpPost("GuiThongDiepKiemTraKyThuat")]
        public IActionResult GuiThongDiepKiemTraKyThuat(ThongDiepParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.GuiThongDiepKiemTraKyThuat(@params);
            return Ok(new { result });
        }

        [HttpPut("UpdateTrangThaiGui")]
        public async Task<IActionResult> UpdateTrangThaiGui(DuLieuGuiHDDTViewModel model)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.UpdateTrangThaiGuiAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongDiepChungViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongDiepGuiHDDTKhongMaService.InsertAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(DuLieuGuiHDDTViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongDiepGuiHDDTKhongMaService.UpdateAsync(model);
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

        [HttpGet("GuiThongDiepDuLieuHDDT/{id}")]
        public async Task<IActionResult> GuiThongDiepDuLieuHDDT(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GuiThongDiepDuLieuHDDTAsync(id);
            return Ok(new { result });
        }

        /// <summary>
        /// Lấy dữ liệu bảng tổng hợp không mã gửi CQT
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GetDuLieuBangTongHopGuiDenCQT")]
        public async Task<IActionResult> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetDuLieuBangTongHopGuiDenCQT(@params);
            return Ok(result);
        }

        /// <summary>
        /// Tạo xml thông điệp 400
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("CreateXMLBangTongHopDuLieu")]
        public async Task<IActionResult> CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.CreateXMLBangTongHopDuLieu(@params);
            return Ok(new { result });
        }

        /// <summary>
        /// Gửi bảng tổng hợp dữ liệu cho CQT
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GuiBangDuLieu")]
        public async Task<IActionResult> GuiBangDuLieu(GuiNhanToKhaiParams @params)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GuiBangDuLieu(@params.FileXml, @params.Id, @params.MaThongDiep, @params.MST);
            return Ok(result);
        }

        [HttpPost("LuuDuLieuKy")]
        public async Task<IActionResult> LuuDuLieuKy(GuiNhanToKhaiParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.LuuDuLieuKy(@params.EncodedContent, @params.Id);
            return Ok(new { result });
        }
    }
}
