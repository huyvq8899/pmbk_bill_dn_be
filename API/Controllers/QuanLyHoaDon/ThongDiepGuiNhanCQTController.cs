using DLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.HoaDonSaiSot;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML.ThongDiepGuiNhanCQT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class ThongDiepGuiNhanCQTController : BaseController
    {
        private readonly IThongDiepGuiNhanCQTService _IThongDiepGuiNhanCQTService;
        private readonly Datacontext _db;

        public ThongDiepGuiNhanCQTController(IThongDiepGuiNhanCQTService iThongDiepGuiNhanCQTService, Datacontext datacontext)
        {
            _IThongDiepGuiNhanCQTService = iThongDiepGuiNhanCQTService;
            _db = datacontext;
        }

        [HttpPost("GetListHoaDonSaiSot")]
        public async Task<IActionResult> GetListHoaDonSaiSot(HoaDonSaiSotParams @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetListHoaDonSaiSotAsync(@params);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    bool result = await _IThongDiepGuiNhanCQTService.DeleteAsync(id);

                    if (result)
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(false);
                }
            }
        }

        [HttpPost("InsertThongBaoGuiHoaDonSaiSot")]
        public async Task<IActionResult> InsertThongBaoGuiHoaDonSaiSot(ThongDiepGuiCQTViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    KetQuaLuuThongDiep result = await _IThongDiepGuiNhanCQTService.InsertThongBaoGuiHoaDonSaiSotAsync(model);
                    if (result == null)
                    {
                        transaction.Rollback();
                        return Ok(null);
                    }
                    else
                    {
                        transaction.Commit();
                        return Ok(new { ketQuaLuuThongDiep = result });
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPost("GateForWebSocket")]
        public async Task<IActionResult> GateForWebSocket(FileXMLThongDiepGuiParams @params)
        {
            if (string.IsNullOrWhiteSpace(@params.DataXML))
            {
                return BadRequest();
            }

            var ketQua = await _IThongDiepGuiNhanCQTService.GateForWebSocket(@params);
            return Ok(new { xmlFileName = ketQua });
        }

        [HttpPost("GuiThongDiepToiCQT")]
        public async Task<IActionResult> GuiThongDiepToiCQT(DuLieuXMLGuiCQTParams @params)
        {
            var ketQua = await _IThongDiepGuiNhanCQTService.GuiThongDiepToiCQTAsync(@params);
            return Ok(ketQua);
        }

        [HttpGet("GetDanhSachDiaDanh")]
        public async Task<IActionResult> GetDanhSachDiaDanh()
        {
            var result = await _IThongDiepGuiNhanCQTService.GetDanhSachDiaDanhAsync();
            return Ok(result);
        }

        [HttpPost("GetDSMauKyHieuHoaDon")]
        public async Task<IActionResult> GetDSMauKyHieuHoaDon(MauKyHieuHoaDonParams @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetDSMauKyHieuHoaDon(@params);
            return Ok(result);
        }

        #region Phần code cho trường hợp thông báo hóa đơn sai sót theo mẫu của CQT
        [HttpPost("GetListHoaDonRaSoat")]
        public async Task<IActionResult> GetListHoaDonRaSoat(HoaDonRaSoatParams @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetListHoaDonRaSoatAsync(@params);
            return Ok(result);
        }

        [HttpGet("GetListChiTietHoaDonRaSoat/{ThongBaoHoaDonRaSoatId}")]
        public IActionResult GetListChiTietHoaDonRaSoat(string thongBaoHoaDonRaSoatId)
        {
            var result = _IThongDiepGuiNhanCQTService.GetListChiTietHoaDonRaSoatAsync(thongBaoHoaDonRaSoatId);
            return Ok(result);
        }

        [HttpPost("GetThongDiepGuiCQTById")]
        public async Task<IActionResult> GetThongDiepGuiCQTById(DataByIdParams @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetThongDiepGuiCQTByIdAsync(@params);
            return Ok(result);
        }

        [HttpGet("GetListChungThuSo/{ThongDiepGuiCQTId}")]
        public async Task<IActionResult> GetListChungThuSo(string thongDiepGuiCQTId)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetListChungThuSoAsync(thongDiepGuiCQTId);
            return Ok(result);
        }

        [HttpPost("KiemTraHoaDonDaLapThongBaoSaiSot")]
        public async Task<IActionResult> KiemTraHoaDonDaLapThongBaoSaiSot(List<ThongBaoSaiSotSearch> @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.KiemTraHoaDonDaLapThongBaoSaiSotAsync(@params);
            return Ok(result);
        }

        [HttpPost("KiemTraHoaDonDaNhapTrungVoiHoaDonHeThong")]
        public async Task<IActionResult> KiemTraHoaDonDaNhapTrungVoiHoaDonHeThong(List<ThongBaoSaiSotSearch> @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.KiemTraHoaDonDaNhapTrungVoiHoaDonHeThongAsync(@params);
            return Ok(result);
        }

        [HttpGet("TaoSoThongBaoSaiSot")]
        public async Task<IActionResult> TaoSoThongBaoSaiSot()
        {
            var result = await _IThongDiepGuiNhanCQTService.TaoSoThongBaoSaiSotAsync();
            return Ok(new { result });
        }

        [HttpPost("GetBangKeHoaDonSaiSot")]
        public async Task<IActionResult> GetBangKeHoaDonSaiSot(ThongKeHoaDonSaiSotParams @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetBangKeHoaDonSaiSotAsync(@params);
            return Ok(result);
        }

        [HttpPost("ExportExcelBangKeSaiSot")]
        public IActionResult ExportExcelBangKeSaiSot(ExportExcelBangKeSaiSotParams @params)
        {
            var result = _IThongDiepGuiNhanCQTService.ExportExcelBangKeSaiSotAsync(@params);
            return Ok(new { result });
        }

        [HttpGet("GetXMLContent/{thongDiepChungId}")]
        public async Task<IActionResult> GetXMLContent(string thongDiepChungId)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetXMLContentAsync(thongDiepChungId);
            return Ok(new { result });
        }
        #endregion
        /// <summary>
        /// GetPdfFile301Async trả về đường dẫn file pdf của 301
        /// </summary>
        /// <param name="thongDiepChungId"></param>
        /// <returns></returns>
        [HttpGet("GetPdfFile301/{ThongDiepChungId}")]
        public async Task<IActionResult> GetPdfFile301(string thongDiepChungId)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetPdfFile301Async(thongDiepChungId);
            return Ok(result);
        }
        /// <summary>
        /// Lấy danh sách các thông  điệp liên qua đến thông điệp ID truyền vào
        /// </summary>
        /// <param name="thongDiepId"></param>
        /// <returns></returns>
        [HttpGet("GetAllThongDiepLienQuan/{thongDiepId}")]
        public async Task<IActionResult> GetAllThongDiepLienQuan(string thongDiepId)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetAllThongDiepLienQuan(thongDiepId);
            return Ok(result);
        }
        /// <summary>
        /// Create file xml to download trên view chi tiết thông điệp
        /// </summary>
        /// <param name="insertXMLSigned"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("InsertFileXMLSigned")]
        public IActionResult InsertFileXMLSigned(InsertXMLSigned insertXMLSigned)
        {
            var result = _IThongDiepGuiNhanCQTService.InsertFileXMLSigned(insertXMLSigned.DataXMLSigned);
            return Ok(new { result });
        }
        /// <summary>
        /// GetLinkFileXml lấy đường dẫn file xml
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetLinkFileXml/{fileName}")]
        public IActionResult GetLinkFileXml(string fileName)
        {
            var result = _IThongDiepGuiNhanCQTService.GetLinkFileXml(fileName);
            return Ok(new { result });
        }
        /// <summary>
        /// Xóa file xml
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("DeleteFileXML/{fileName}")]
        public IActionResult DeleteFileXML(string fileName)
        {
            var result = _IThongDiepGuiNhanCQTService.DeleteFileXML(fileName);
            return Ok(new { result });
        }
        /// <summary>
        /// Trường dữ liệu chưa xml
        /// </summary>
        public class InsertXMLSigned
        {
            public string DataXMLSigned { get; set; }
        }
    }
}
