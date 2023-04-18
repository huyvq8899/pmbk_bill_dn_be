using API.Extentions;
using DLL;
using DLL.Entity.Config;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper.Constants;
using Services.Repositories.Implimentations.QuanLyHoaDon;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.BaoCao;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Config
{
    public class TuyChonController : BaseController
    {
        private readonly ITuyChonService _tuyChonService;
        private readonly Datacontext _db;
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly IHoaDonDienTuChiTietService _hoaDonDienTuChiTietService;
        private readonly IUserRespositories _userRespositories;

        public TuyChonController(ITuyChonService tuyChonService, Datacontext datacontext,
            IHoaDonDienTuChiTietService hoaDonDienTuChiTietService,
            IUserRespositories userRespositories,
            IHoaDonDienTuService hoaDonDienTuService
            )
        {
            _tuyChonService = tuyChonService;
            _db = datacontext;
            _hoaDonDienTuService = hoaDonDienTuService;
            _hoaDonDienTuChiTietService = hoaDonDienTuChiTietService;
            _userRespositories = userRespositories;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string keyword)
        {
            var result = await _tuyChonService.GetAllAsync(keyword);
            return Ok(result);
        }

        [HttpGet("GetAllNoiDungEmail")]
        public async Task<IActionResult> GetAllNoiDungEmail()
        {
            var result = await _tuyChonService.GetAllNoiDungEmail();
            return Ok(result);
        }

        [HttpGet("GetDetail/{ma}")]
        public async Task<IActionResult> GetDetail(string ma)
        {
            if (string.IsNullOrEmpty(ma))
            {
                return BadRequest();
            }

            var result = await _tuyChonService.GetDetailAsync(ma);
            return Ok(result);
        }

        [HttpPost("LayLaiThietLapEmailMacDinh")]
        public async Task<IActionResult> LayLaiThietLapMacDinh(int LoaiEmail)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.LayLaiThietLapEmailMacDinh(LoaiEmail);
                    transaction.Commit();

                    return Ok(rs);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TuyChonViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateAsync(model);
                    #region Update lại hóa đơn điện tử ở trạng thái chưa ký điện tử khi cập nhập lại định dạng số
                    // kiểm tra xem định dạng số có bị thay đổi không
                    string jsonOldList = JsonConvert.SerializeObject(model.OldList);
                    string jsonNewList = JsonConvert.SerializeObject(model.NewList);
                    bool checkChange = false;
                    var listDinhDangSo_Changed = new List<TuyChonViewModel>();
                    if (jsonOldList != jsonNewList)
                    {
                        int length = model.OldList.Count;

                        for (int i = 0; i < length; i++)
                        {
                            var oldItem = model.OldList[i];
                            var newItem = model.NewList[i];
                            if (oldItem.GiaTri != newItem.GiaTri)
                            {
                                switch (oldItem.Ma)
                                {
                                    case LoaiDinhDangSo.DON_GIA_NGOAI_TE:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                    case LoaiDinhDangSo.DON_GIA_QUY_DOI:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                    case LoaiDinhDangSo.HESO_TYLE:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                    case LoaiDinhDangSo.SO_LUONG:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                    case LoaiDinhDangSo.TIEN_NGOAI_TE:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                    case LoaiDinhDangSo.TIEN_QUY_DOI:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                    case LoaiDinhDangSo.TY_GIA:
                                        listDinhDangSo_Changed.Add(newItem);
                                        checkChange = true;
                                        break;
                                }
                            }
                        }
                    }
                    // lấy hd chưa ký điện tử
                    if (checkChange)
                    {
                        var hoaDons = await _hoaDonDienTuService.GetHoaDonByTrangThaiQuyTrinhAsync((int)TrangThaiQuyTrinh.ChuaKyDienTu);
                        // gọi hàm update

                        var TIEN_QUY_DOI_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.TIEN_QUY_DOI.ToUpper().Trim()) == 0);
                        var TIEN_NGOAI_TE_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.TIEN_NGOAI_TE.ToUpper().Trim()) == 0);
                        var DON_GIA_QUY_DOI_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.DON_GIA_QUY_DOI.ToUpper().Trim()) == 0);
                        var DON_GIA_NGOAI_TE_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.DON_GIA_NGOAI_TE.ToUpper().Trim()) == 0);
                        var SO_LUONG_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.SO_LUONG.ToUpper().Trim()) == 0);
                        var tY_GIA_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.TY_GIA.ToUpper().Trim()) == 0);
                        var HESO_TYLE_Change = listDinhDangSo_Changed.Find(x => string.Compare(x.Ma.ToUpper().Trim(), LoaiDinhDangSo.HESO_TYLE.ToUpper().Trim()) == 0);

                        foreach (var item in hoaDons)
                        {
                            //var item = await _hoaDonDienTuService.GetByIdAsync(hd.HoaDonDienTuId);
                            #region Làm tròn số theo tùy chọn chung

                            if (item.TyGia.HasValue && tY_GIA_Change != null) item.TyGia = item.TyGia.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TY_GIA);                            

                            if (item.TongTienHang.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) item.TongTienHang = item.TongTienHang.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_QUY_DOI);

                            if (item.TongTienHang.HasValue && DON_GIA_NGOAI_TE_Change != null && item.IsVND != true) item.TongTienHang = item.TongTienHang.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                            if (item.TongTienChietKhau.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) item.TongTienChietKhau = item.TongTienChietKhau.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_QUY_DOI);

                            if (item.TongTienChietKhau.HasValue && DON_GIA_NGOAI_TE_Change != null && item.IsVND != true) item.TongTienChietKhau = item.TongTienChietKhau.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                            if (item.TongTienThanhToan.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) item.TongTienThanhToan = item.TongTienThanhToan.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_QUY_DOI);

                            if (item.TongTienThanhToan.HasValue && DON_GIA_NGOAI_TE_Change != null && item.IsVND != true) item.TongTienThanhToan = item.TongTienThanhToan.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                            if (item.TongTienThueGTGT.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) item.TongTienThueGTGT = item.TongTienThueGTGT.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_QUY_DOI);

                            if (item.TongTienThueGTGT.HasValue && DON_GIA_NGOAI_TE_Change != null && item.IsVND != true) item.TongTienThueGTGT = item.TongTienThueGTGT.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                            if (item.TongTienHangQuyDoi.HasValue && TIEN_QUY_DOI_Change != null ) item.TongTienHangQuyDoi = item.TongTienHangQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);
                            if (item.TongTienChietKhauQuyDoi.HasValue && TIEN_QUY_DOI_Change != null ) item.TongTienChietKhauQuyDoi = item.TongTienChietKhauQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);
                            if (item.TongTienThueGTGTQuyDoi.HasValue && TIEN_QUY_DOI_Change != null ) item.TongTienThueGTGTQuyDoi = item.TongTienThueGTGTQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);
                            if (item.TongTienThanhToanQuyDoi.HasValue && TIEN_QUY_DOI_Change != null ) item.TongTienThanhToanQuyDoi = item.TongTienThanhToanQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);

                            if (item.TyLeChietKhau.HasValue && HESO_TYLE_Change !=null) item.TyLeChietKhau = item.TyLeChietKhau.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.HESO_TYLE);
                            #endregion

                            #region Hàng hóa dịch vụ

                            foreach (var hdct in item.HoaDonChiTiets)
                            {
                                if (string.IsNullOrEmpty(hdct.HoaDonDienTuChiTietId)) hdct.HoaDonDienTuChiTietId = Guid.NewGuid().ToString();
                                #region Làm tròn số theo tùy chọn chung
                                if (hdct.SoLuong.HasValue && SO_LUONG_Change !=null) hdct.SoLuong = hdct.SoLuong.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.SO_LUONG);

                                if (hdct.TyLeChietKhau.HasValue && HESO_TYLE_Change != null) hdct.TyLeChietKhau = hdct.TyLeChietKhau.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.HESO_TYLE);

                                if (hdct.DonGia.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) hdct.DonGia = hdct.DonGia.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_QUY_DOI);

                                if (hdct.DonGia.HasValue && DON_GIA_NGOAI_TE_Change != null && item.IsVND != true) hdct.DonGia = hdct.DonGia.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                                if (hdct.ThanhTien.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) hdct.ThanhTien = hdct.ThanhTien.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_QUY_DOI);

                                if (hdct.ThanhTien.HasValue && DON_GIA_NGOAI_TE_Change != null && item.IsVND != true) hdct.ThanhTien = hdct.ThanhTien.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                                if (hdct.ThanhTienQuyDoi.HasValue && TIEN_QUY_DOI_Change != null) hdct.ThanhTienQuyDoi = hdct.ThanhTienQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);

                                if (hdct.TienChietKhau.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) hdct.TienChietKhau = hdct.TienChietKhau.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);
                                if (hdct.TienChietKhau.HasValue && TIEN_NGOAI_TE_Change != null && item.IsVND != true) hdct.TienChietKhau = hdct.TienChietKhau.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_NGOAI_TE);

                                if (hdct.TienChietKhauQuyDoi.HasValue && TIEN_QUY_DOI_Change != null) hdct.TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);

                                if (hdct.TienThueGTGT.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) hdct.TienThueGTGT = hdct.TienThueGTGT.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);
                                
                                if (hdct.TienThueGTGT.HasValue && TIEN_NGOAI_TE_Change != null && item.IsVND != true) hdct.TienThueGTGT = hdct.TienThueGTGT.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_NGOAI_TE);

                                if (hdct.TienThueGTGTQuyDoi.HasValue && TIEN_QUY_DOI_Change != null) hdct.TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);

                                if (hdct.TyLePhanTramDoanhThu.HasValue && HESO_TYLE_Change !=null) hdct.TyLePhanTramDoanhThu = hdct.TyLePhanTramDoanhThu.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.HESO_TYLE);

                                if (hdct.TienGiam.HasValue && DON_GIA_QUY_DOI_Change != null && item.IsVND == true) hdct.TienGiam = hdct.TienGiam.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed,LoaiDinhDangSo.DON_GIA_QUY_DOI);
                                if (hdct.TienGiam.HasValue && TIEN_NGOAI_TE_Change != null && item.IsVND != true) hdct.TienGiam = hdct.TienGiam.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_NGOAI_TE);

                                if (hdct.TienGiamQuyDoi.HasValue && TIEN_QUY_DOI_Change !=null) hdct.TienGiamQuyDoi = hdct.TienGiamQuyDoi.Value.MathRoundNumberByTuyChon(listDinhDangSo_Changed, LoaiDinhDangSo.TIEN_QUY_DOI);
                                #endregion
                            }
                            #endregion

                            await _hoaDonDienTuChiTietService.RemoveRangeAsync(item.HoaDonDienTuId);
                            await _hoaDonDienTuChiTietService.InsertRangeAsync(item, item.HoaDonChiTiets);

                            bool resultHD = await _hoaDonDienTuService.UpdateAsync(item);

                            if (resultHD)
                            {
                                var _currentUser = await _userRespositories.GetById(HttpContext.User.GetUserId());
                                var nk = new NhatKyThaoTacHoaDonViewModel
                                {
                                    HoaDonDienTuId = item.HoaDonDienTuId,
                                    LoaiThaoTac = (int)LoaiThaoTac.SuaHoaDon,
                                    MoTa = "Thao tác Sửa  tùy chọn và tự động sửa lại hóa đơn theo định dạng số mới lúc " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                                    NguoiThucHienId = _currentUser.UserId,
                                    NgayGio = DateTime.Now,
                                    HasError = !resultHD
                                };
                                await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(nk);
                            }
                        }
                    }

                    #endregion
                    transaction.Commit();

                    foreach (var item in model.NewList)
                    {
                        Response.Cookies.Append(item.Ma, item.GiaTri);
                    }

                    return Ok(rs);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("UpdateRangeNoiDungEmailAsync")]
        public async Task<IActionResult> UpdateRangeNoiDungEmailAsync(List<ConfigNoiDungEmailViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateRangeNoiDungEmailAsync(models);
                    transaction.Commit();

                    return Ok(rs);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("GetThongTinHienThiTruongDL/{tenChucNang}")]
        public async Task<IActionResult> GetThongTinHienThiTruongDL(string tenChucNang)
        {
            if (string.IsNullOrEmpty(tenChucNang))
            {
                return BadRequest();
            }

            var result = await _tuyChonService.GetThongTinHienThiTruongDL(tenChucNang);

            return Ok(result);
        }

        [HttpPost("UpdateHienThiTruongDuLieu")]
        public async Task<IActionResult> UpdateHienThiTruongDuLieu(List<TruongDuLieuViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateHienThiTruongDuLieu(models);
                    transaction.Commit();

                    return Ok(rs);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("CheckCoPhatSinhNgoaiTe")]
        public async Task<IActionResult> CheckCoPhatSinhNgoaiTe()
        {
            var result = await _tuyChonService.CheckCoPhatSinhNgoaiTeAsync();
            return Ok(result);
        }
        [HttpGet("GetTypeChuKi")]
        public async Task<IActionResult> GetTypeChuKi()
        {
            var result = await _tuyChonService.GetTypeLoaiChuKi();
            return Ok(result);
        }

        [HttpPost("UpdateLoaiChuKi")]
        public async Task<IActionResult> UpdateLoaiChuKi(TuyChonViewModel model)
        {
            try
            {
                var rs = await _tuyChonService.UpdateLoaiChuKi(model);

                return Ok(rs);
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpGet("GetListByHoaDonId/{hoaDonId}")]
        public async Task<IActionResult> GetListByHoaDonId(string hoaDonId)
        {
            var result = await _tuyChonService.GetListByHoaDonIdAsync(hoaDonId);
            return Ok(result);
        }
    }
}
