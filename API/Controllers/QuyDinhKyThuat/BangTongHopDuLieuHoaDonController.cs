﻿using API.Extentions;
using DLL;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    [Route("api/[controller]")]
    public class BangTongHopDuLieuHoaDonController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IBangTongHopService _IBangTongHopService;

        public BangTongHopDuLieuHoaDonController(
            Datacontext datacontext,
            IBangTongHopService IBangTongHopService)
        {
            _db = datacontext;
            _IBangTongHopService = IBangTongHopService;
        }

        #region Ký và gửi thông điệp bảng tổng hợp dữ liệu
        /// <summary>
        /// Tạo xml thông điệp 400
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("CreateXMLBangTongHopDuLieu")]
        public IActionResult CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params)
        {
            var result = _IBangTongHopService.CreateXMLBangTongHopDuLieu(@params);
            return Ok(new { result });
        }

        /// <summary>
        /// Lưu dữ liệu bảng tổng hợp đã ký vào file datas
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("LuuDuLieuKy")]
        public IActionResult LuuDuLieuKy(GuiNhanToKhaiParams @params)
        {
            var result = _IBangTongHopService.LuuDuLieuKy(@params.EncodedContent, @params.Id);
            return Ok(new { result });
        }


        /// <summary>
        /// Gửi bảng tổng hợp dữ liệu cho TVAN
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GuiBangDuLieu")]
        public async Task<IActionResult> GuiBangDuLieu(GuiNhanToKhaiParams @params)
        {
            var result = await _IBangTongHopService.GuiBangDuLieu(@params.Id, @params.MaThongDiep, @params.MST);
            return Ok(result);
        }
        #endregion

        #region AutoData
        /// <summary>
        /// Tự động sinh số lần bổ sung với trường hợp bố sung. 
        /// Số lần bổ sung >= 1 và <=999
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GetLanBoSung")]
        public async Task<IActionResult> GetLanBoSung(BangTongHopParams3 @params)
        {
            var result = await _IBangTongHopService.GetLanBoSung(@params);
            return Ok(result);
        }

        /// <summary>
        /// Generate tự động số bảng tổng hợp dựa trên các bảng tổng hợp đã gửi
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GetSoBangTongHopDuLieu")]
        public async Task<IActionResult> GetSoBangTongHopDuLieu(BangTongHopParams2 @params)
        {
            var result = await _IBangTongHopService.GetSoBangTongHopDuLieu(@params);
            return Ok(result);
        }
        #endregion

        #region Check
        /// <summary>
        /// Check xem bảng tổng hợp có phải là lần đầu và đã gửi thành công cho CQT không
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("CheckLanDau")]
        public async Task<IActionResult> CheckLanDau(BangTongHopParams3 @params)
        {
            var result = await _IBangTongHopService.CheckLanDau(@params);
            return Ok(result);
        }
        #endregion

        #region Get Data
        /// <summary>
        /// Lấy các tiêu chí tìm kiếm ở tab bảng tổng hợp dữ liệu
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListTimKiemTheoBangTongHop")]
        public IActionResult GetListTimKiemTheoBangTongHop()
        {
            var result = _IBangTongHopService.GetListTimKiemTheoBangTongHop();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(BangTongHopDuLieuHoaDonParams pagingParams)
        {
            var paged = await _IBangTongHopService.GetAllPagingBangTongHopAsync(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            foreach(var item in paged.Items)
            {
                item.IsQuaHan = IsQuaHan(item);
            }
            return Ok(new { paged.Items, paged.AllItemIds, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _IBangTongHopService.GetById(id);
            return Ok(result);
        }
        #endregion

        #region CRUD
        /// <summary>
        /// Lấy dữ liệu bảng tổng hợp không mã gửi CQT
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GetDuLieuBangTongHopGuiDenCQT")]
        public async Task<IActionResult> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        {
            var result = await _IBangTongHopService.GetDuLieuBangTongHopGuiDenCQT(@params);
            return Ok(result);
        }

        /// <summary>
        /// Thêm mới bảng tổng hợp dữ liệu hóa đơn
        /// </summary>
        /// <param name="model"></param>
        /// <returns>boolean: true nếu lưu thành công, false nếu lưu lỗi</returns>
        [HttpPost("InsertBangTongHopDuLieuHoaDonAsync")]
        public async Task<IActionResult> InsertBangTongHopDuLieuHoaDonAsync(BangTongHopDuLieuHoaDonViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IBangTongHopService.InsertBangTongHopDuLieuHoaDonAsync(model);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Cập nhật bảng tổng hợp dữ liệu hóa đơn
        /// </summary>
        /// <param name="model"></param>
        /// <returns>boolean: true nếu lưu thành công, false nếu lưu lỗi</returns>
        [HttpPost("UpdateBangTongHopDuLieuHoaDonAsync")]
        public async Task<IActionResult> UpdateBangTongHopDuLieuHoaDonAsync(BangTongHopDuLieuHoaDonViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IBangTongHopService.UpdateBangTongHopDuLieuHoaDonAsync(model);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Xóa bảng tổng hợp dữ liệu hóa đơn
        /// </summary>
        /// <param name="model"></param>
        /// <returns>boolean: true nếu xóa thành công, false nếu xóa lỗi</returns>
        [HttpDelete("DeleteBangTongHopDuLieuHoaDonAsync/{Id}")]
        public async Task<IActionResult> DeleteBangTongHopDuLieuHoaDonAsync(string Id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IBangTongHopService.DeleteBangTongHopDuLieuHoaDonAsync(Id);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }
        #endregion

        #region private function
        private bool IsQuaHan(BangTongHopDuLieuHoaDonViewModel bth)
        {
            if (bth.TrangThaiGui == TrangThaiGuiThongDiep.ChuaGui)
            {
                if (bth.ThoiHanGui >= DateTime.Now) return false;
                else return true;
            }
            else
            {
                if (bth.ThoiHanGui >= bth.ThoiGianGui) return false;
                else return true;
            }
        }
        #endregion
    }
}