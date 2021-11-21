using DLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IDuLieuGuiHDDTService _thongDiepGuiHDDTKhongMaService;

        public ThongDiepGuiDuLieuHDDTController(
            Datacontext datacontext,
            IDuLieuGuiHDDTService thongDiepGuiHDDTKhongMaService)
        {
            _db = datacontext;
            _thongDiepGuiHDDTKhongMaService = thongDiepGuiHDDTKhongMaService;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetByIdAsync(Id);
            return Ok(result);
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

        [HttpPost("GetDuLieuBangTongHopGuiDenCQT")]
        public async Task<IActionResult> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetDuLieuBangTongHopGuiDenCQT(@params);
            return Ok(result);
        }

        [HttpPost("CreateXMLBangTongHopDuLieu")]
        public async Task<IActionResult> CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.CreateXMLBangTongHopDuLieu(@params);
            return Ok(new { result });
        }

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
