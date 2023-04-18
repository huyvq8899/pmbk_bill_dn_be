using API.Extentions;
using DLL;
using DLL.Constants;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using ImageMagick;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X500;
using Services.Enums;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.HeThong;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Implimentations.DanhMuc;
using Services.Repositories.Implimentations.ESignCloud;
using Services.Repositories.Implimentations.QuanLyHoaDon;
using Services.Repositories.Implimentations.TienIch;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.ESignCloud;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using Services.ViewModels.Import;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.TienIch;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers.QuanLyHoaDon
{
    public class HoaDonDienTuController : BaseController
    {
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly IThongTinHoaDonService _thongTinHoaDonService;
        private readonly IHoaDonDienTuChiTietService _hoaDonDienTuChiTietService;
        private readonly IUserRespositories _userRespositories;
        private readonly ITraCuuService _traCuuService;
        private readonly IDatabaseService _databaseService;
        private readonly INhatKyThaoTacLoiService _nhatKyThaoTacLoiService;
        private readonly Datacontext _db;
        private readonly IThongDiepGuiNhanCQTService _IThongDiepGuiNhanCQTService;
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;
        private readonly IDoiTuongService _doiTuongService;
        private readonly IDonViTinhService _donViTinhService;
        private readonly IHangHoaDichVuService _hangHoaDichVuService;
        private readonly ITuyChonService _TuyChonService;
        private readonly ILoaiTienService _LoaiTienService;
        private readonly IESignCloudService _SignCloudService;
        private readonly IHoSoHDDTService _HoSoHDDTService;
        private readonly IDuLieuGuiHDDTService _DuLieuGuiHDDTService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HoaDonDienTuController(
            IHoaDonDienTuService hoaDonDienTuService,
            IThongTinHoaDonService thongTinHoaDonService,
            IHoaDonDienTuChiTietService hoaDonDienTuChiTietService,
            IUserRespositories userRespositories,
            ITraCuuService traCuuService,
            IDatabaseService databaseService,
            INhatKyThaoTacLoiService nhatKyThaoTacLoiService,
            IThongDiepGuiNhanCQTService iThongDiepGuiNhanCQTService,
            Datacontext db, INhatKyTruyCapService nhatKyTruyCapService,
            IHttpContextAccessor IHttpContextAccessor,
            IHostingEnvironment IHostingEnvironment,
            IDoiTuongService doiTuongService
            , IDonViTinhService donViTinhService
            , IHangHoaDichVuService hangHoaDichVuService
            , ITuyChonService TuyChonService
            , ILoaiTienService loaiTienService
            , IESignCloudService signCloudService
            , IHoSoHDDTService hoSoHDDTService
            , IConfiguration configuration
            , IDuLieuGuiHDDTService duLieuGuiHDDTService
        )
        {
            _hoaDonDienTuService = hoaDonDienTuService;
            _hoaDonDienTuChiTietService = hoaDonDienTuChiTietService;
            _userRespositories = userRespositories;
            _traCuuService = traCuuService;
            _databaseService = databaseService;
            _thongTinHoaDonService = thongTinHoaDonService;
            _nhatKyThaoTacLoiService = nhatKyThaoTacLoiService;
            _db = db;
            _IThongDiepGuiNhanCQTService = iThongDiepGuiNhanCQTService;
            _nhatKyTruyCapService = nhatKyTruyCapService;
            _doiTuongService = doiTuongService;
            _donViTinhService = donViTinhService;
            _hangHoaDichVuService = hangHoaDichVuService;
            _TuyChonService = TuyChonService;
            _LoaiTienService = loaiTienService;
            _SignCloudService = signCloudService;
            _HoSoHDDTService = hoSoHDDTService;
            _DuLieuGuiHDDTService = duLieuGuiHDDTService;
            _configuration = configuration;
            _IHttpContextAccessor = IHttpContextAccessor;
            _hostingEnvironment = IHostingEnvironment;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hoaDonDienTuService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(HoaDonParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.AllItemIds, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages, paged.TongTienThanhToan });
        }

        [HttpPost("GetAllPagingHoaDonThayThe")]
        public async Task<IActionResult> GetAllPagingHoaDonThayThe(HoaDonThayTheParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetAllPagingHoaDonThayTheAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("GetListHoaDonXoaBoCanThayThe")]
        public async Task<IActionResult> GetListHoaDonXoaBoCanThayThe(HoaDonThayTheParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonXoaBoCanThayTheAsync(pagingParams);
            return Ok(result);
        }

        [HttpPost("GetListHoaDonCanDieuChinh")]
        public async Task<IActionResult> GetListHoaDonCanDieuChinh(HoaDonDieuChinhParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonCanDieuChinhAsync(pagingParams);
            return Ok(result);
        }

        [HttpPost("GetAllPagingHoaDonDieuChinh")]
        public async Task<IActionResult> GetAllPagingHoaDonDieuChinh(HoaDonDieuChinhParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetAllPagingHoaDonDieuChinhAsync_New(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("GetListHoaDonKhongMa")]
        public async Task<IActionResult> GetListHoaDonKhongMa(HoaDonParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonKhongMaAsync(pagingParams);
            return Ok(result);
        }

        [HttpPost("GetListHoaDonCanCapMa")]
        public async Task<IActionResult> GetListHoaDonCanCapMa(HoaDonParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonCanCapMaAsync(pagingParams);
            return Ok(result);
        }

        [HttpGet("GetChiTietHoaDon/{id}")]
        public async Task<IActionResult> GetChiTietHoaDon(string id)
        {
            var result = await _hoaDonDienTuChiTietService.GetChiTietHoaDonAsync(id, false);
            return Ok(result);
        }

        [HttpGet("GetTrangThaiHoaDonDieuChinhs")]
        public IActionResult GetTrangThaiHoaDonDieuChinhs()
        {
            var result = _hoaDonDienTuService.GetTrangThaiHoaDonDieuChinhs();
            return Ok(result);
        }

        [HttpGet("GetLoaiTrangThaiBienBanDieuChinhHoaDons")]
        public IActionResult GetLoaiTrangThaiBienBanDieuChinhHoaDons()
        {
            var result = _hoaDonDienTuService.GetLoaiTrangThaiBienBanDieuChinhHoaDons();
            return Ok(result);
        }

        [HttpGet("GetLoaiTrangThaiPhatHanhs")]
        public IActionResult GetLoaiTrangThaiPhatHanhs()
        {
            var result = _hoaDonDienTuService.GetLoaiTrangThaiPhatHanhs();
            return Ok(result);
        }

        [HttpGet("GetLoaiTrangThaiGuiHoaDons")]
        public IActionResult GetLoaiTrangThaiGuiHoaDons()
        {
            var result = _hoaDonDienTuService.GetLoaiTrangThaiGuiHoaDons();
            return Ok(result);
        }

        [HttpGet("GetAllListHoaDonLienQuan")]
        public async Task<IActionResult> GetAllListHoaDonLienQuan([FromQuery] string id, [FromQuery] DateTime ngayTao)
        {
            var result = await _hoaDonDienTuService.GetAllListHoaDonLienQuan(id, ngayTao);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllListHoaDonLienQuan_TraCuu")]
        public async Task<IActionResult> GetAllListHoaDonLienQuan_TraCuu([FromQuery] string id, [FromQuery] DateTime ngayTao)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.GetAllListHoaDonLienQuan(id, ngayTao);
            return Ok(result);
        }

        [HttpGet("GetListHinhThucHoaDonCanThayThe")]
        public IActionResult GetListHinhThucHoaDonCanThayThe()
        {
            var result = _hoaDonDienTuService.GetListHinhThucHoaDonCanThayThe();
            return Ok(result);
        }

        [HttpGet("GetListTimKiemTheoHoaDonThayThe")]
        public IActionResult GetListTimKiemTheoHoaDonThayThe()
        {
            var result = _hoaDonDienTuService.GetListTimKiemTheoHoaDonThayThe();
            return Ok(result);
        }

        [HttpGet("GetHoaDonByTrangThaiQuyTrinh/{trangthaiQuyTrinh}")]
        public async Task<IActionResult> GetHoaDonByTrangThaiQuyTrinh(int trangthaiQuyTrinh)
        {
            var result = await _hoaDonDienTuService.GetHoaDonByTrangThaiQuyTrinhAsync(trangthaiQuyTrinh);
            return Ok(result);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _hoaDonDienTuService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetBySoHoaDon")]
        public async Task<IActionResult> GetBySoHoaDon([FromQuery] long SoHoaDon, [FromQuery] string KyHieu, [FromQuery] string KyHieuMauSo)
        {
            var result = await _hoaDonDienTuService.GetByIdAsync(SoHoaDon, KyHieu, KyHieuMauSo);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetById_TraCuu/{Id}")]
        public async Task<IActionResult> GetById_TraCuu(string id)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(id);

            if (companyModel != null)
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
                var result = await _hoaDonDienTuService.GetByIdAsync(id);
                if (result == null) result = await _thongTinHoaDonService.GetById(id);
                return Ok(result);
            }
            else return Ok();
        }

        [AllowAnonymous]
        [HttpPost("FindSignatureElement")]
        public async Task<IActionResult> FindSignatureElement(CTSParams @params)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(@params.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = _traCuuService.FindSignatureElement(@params.HoaDonDienTuId, @params.Type);
            return Ok(result);
        }

        [HttpGet("CheckSoHoaDon")]
        public async Task<IActionResult> CheckSoHoaDon(long? soHoaDon)
        {
            var result = await _hoaDonDienTuService.CheckSoHoaDonAsync(soHoaDon);
            return Ok(result);
        }

        [HttpPost("ExportExcelBangKe")]
        public async Task<IActionResult> ExportExcelBangKe(HoaDonParams @params)
        {
            var result = await _hoaDonDienTuService.ExportExcelBangKe(@params);
            return Ok(new { Path = result });
        }

        [HttpPost("ExportExcelBangKeChiTiet")]
        public async Task<IActionResult> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params)
        {
            var result = await _hoaDonDienTuService.ExportExcelBangKeChiTiet(@params);
            return Ok(new { path = result });
        }

        [HttpPost("ExportExcelError")]
        public IActionResult ExportExcelError(TaiHoaDonLoiParams @params)
        {
            var result = _hoaDonDienTuService.ExportErrorFile(@params.ListError, @params.Action);
            return Ok(new { path = result });
        }


        [HttpGet("GetError")]
        public IActionResult GetError([FromQuery] int LoaiLoi, [FromQuery] string HoaDonDienTuId)
        {
            var result = _hoaDonDienTuService.GetError(HoaDonDienTuId, LoaiLoi);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    model.HoaDonDienTuId = Guid.NewGuid().ToString();
                    List<HoaDonDienTuChiTietViewModel> hoaDonDienTuChiTiets = model.HoaDonChiTiets;

                    foreach (var item in hoaDonDienTuChiTiets)
                    {
                        item.HoaDonDienTuChiTietId = Guid.NewGuid().ToString();
                    }

                    HoaDonDienTuViewModel result = await _hoaDonDienTuService.InsertAsync(model);
                    if (result == null)
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }

                    var models = await _hoaDonDienTuChiTietService.InsertRangeAsync(result, hoaDonDienTuChiTiets);
                    result.HoaDonChiTiets = models;
                    if (models.Count != hoaDonDienTuChiTiets.Count)
                    {
                        transaction.Rollback();
                        return Ok(false);
                    }

                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("PosGetAllPaging")]
        public async Task<IActionResult> PosGetAllPaging(HoaDonParams pagingParams)
        {
            var listOkRsError = new List<OkObjectResult>();
            if (string.IsNullOrEmpty(pagingParams.FromDate)) listOkRsError.Add(Ok(new { status = false, errorMessage = "<FromDate> Từ ngày không được để trống." }));
            if (string.IsNullOrEmpty(pagingParams.ToDate)) listOkRsError.Add(Ok(new { status = false, errorMessage = "<ToDate> Đến ngày không được để trống." }));
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!TextHelper.IsValidDate(pagingParams.FromDate))
            {
                listOkRsError.Add(Ok(new { status = false, errorMessage = "<FromDate> Sai định dạng ngày tháng." }));
            }
            if (!TextHelper.IsValidDate(pagingParams.ToDate))
            {
                listOkRsError.Add(Ok(new { status = false, errorMessage = "<ToDate> Sai định dạng ngày tháng." }));
            }
            if (listOkRsError != null)
            {
                return Ok(new
                {
                    status = false,
                    itemsError = listOkRsError,
                });
            }
            var paged = await _hoaDonDienTuService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.AllItemIds, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages, paged.TongTienThanhToan });
        }

        [HttpGet("PosGetById/{Id}")]
        public async Task<IActionResult> PosGetById(string id)
        {
            var result = await _hoaDonDienTuService.PosGetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("PosInsertInvoices")]
        public async Task<IActionResult> PosInsertInvoices(List<HoaDonDienTuViewModel> model)
        {
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            Tracert.WriteLog($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} " + databaseName + " PosInsertInvoices received count: " + model.Count());
            // Check folder                
            string folder = $"{_hostingEnvironment.WebRootPath}/FilesUpload/{databaseName}/PosInsertInvoices/{DateTime.Now:ddMMyyyy}";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var fileName = string.Format("{0}_{1}_Add.json", model.Count(), $"{DateTime.Now:ddMMyyyyHHmmss}");
            var pullPath = $"{_hostingEnvironment.WebRootPath}/FilesUpload/{databaseName}/PosInsertInvoices/{DateTime.Now:ddMMyyyy}/{fileName}";
            System.IO.File.WriteAllText(pullPath, JsonConvert.SerializeObject(model));
            try
            {
                int stt_item = 0;
                var listOkRsError = new List<OkObjectResult>();
                var listInsertSuccess = new List<HoaDonDienTuViewModel>();
                var listUpdateSuccess = new List<string>();
                int success = 0;
                var donViTinhParams = new DonViTinhParams();
                donViTinhParams.IsActive = true;
                var listDVT = await _donViTinhService.GetAllAsync(donViTinhParams);
                var _tuyChons = await _TuyChonService.GetAllAsync();

                foreach (var item in model)
                {
                    var itemPosGoc = item;
                    ++stt_item;
                    var result = false;
                    //Check data input
                    item.TimeUpdateByNSD = DateTime.Now;

                    if (item.IsTicket == true && !string.IsNullOrEmpty(item.MaCuaCQT))
                    {
                        var createSo = await _hoaDonDienTuService.CreateSoHoaDon(item);
                        item.SoHoaDon = createSo.SoHoaDon;
                        item.IsTicket = false;//gán = false để k tạo số hóa đơn trong hàm insert nữa
                        item.FromGP = null;//k gen mã cơ quan thuế
                        item.LoaiChungTu = 6;//Từ các bên sử dụng API
                    }
                    else
                    {
                        item.FromGP = false;
                        item.LoaiChungTu = 4;//Từ các bên sử dụng API
                    }
                    var resultFilter = await _hoaDonDienTuService.FilterPosInsertInvoices(item);
                    if (resultFilter.Success == false)
                    {
                        var okRsError = Ok(new
                        {
                            status = resultFilter.Success,
                            index_item_error = stt_item,
                            errorMessage = resultFilter.ErrorMessage
                        });
                        listOkRsError.Add(okRsError);
                    }
                    else
                    {
                        #region Làm tròn số theo tùy chọn chung
                        if (!item.TyGia.HasValue) item.TyGia = 1;
                        if (item.TyGia.HasValue) item.TyGia = item.TyGia.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TY_GIA);
                        if (!string.IsNullOrEmpty(item.MaLoaiTien) && item.MaLoaiTien.Length <= 3)
                        {
                            var loaiTien = await _LoaiTienService.GetByMaAsync(item.MaLoaiTien);

                            if (loaiTien != null)
                            {
                                item.LoaiTienId = loaiTien.LoaiTienId;
                                item.IsVND = loaiTien.Ma == "VND";
                                item.MaLoaiTien = loaiTien.Ma;
                            }
                        }
                        #endregion
                        #region kiểm tra nhân viên xem đã tồn tại chưa
                        if (!string.IsNullOrEmpty(item.MaNhanVienBanHang))
                        {

                            var nhanVien = await _doiTuongService.GetNhanVienByMa(item.MaNhanVienBanHang);
                            if (nhanVien != null)
                            {
                                item.NhanVienBanHangId = nhanVien.DoiTuongId;
                            }
                            else
                            {
                                // kq == true thì mới insert
                                var dtInserted = await _doiTuongService.InsertAsync(new DoiTuongViewModel
                                {
                                    Ma = item.MaNhanVienBanHang,
                                    Ten = item.TenNhanVienBanHang ?? "SYSTEM",
                                    IsNhanVien = true,
                                    Status = true,
                                    CreatedDate = DateTime.Now,
                                    ModifyDate = DateTime.Now,
                                    LoaiKhachHang = 1
                                });
                                //thêm nhật ký khi insert
                                //Isnsert nhật ký
                                var nk = new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Them,
                                    RefType = RefType.NhanVien,
                                    ThamChieu = "Mã: " + dtInserted.Ma,
                                    RefId = dtInserted.DoiTuongId
                                };

                                await _nhatKyTruyCapService.InsertAsync(nk);
                            }
                        }

                        #endregion
                        #region kiểm tra khách hàng xem đã tồn tại chưa
                        if (!string.IsNullOrEmpty(item.MaKhachHang))
                        {

                            var khachHang = await _doiTuongService.GetKhachHangByMa(item.MaKhachHang);
                            if (khachHang != null)
                            {
                                item.KhachHangId = khachHang.DoiTuongId;
                            }
                            else
                            {
                                // kq == true thì mới insert
                                var dtInserted = await _doiTuongService.InsertAsync(new DoiTuongViewModel
                                {
                                    Ma = item.MaKhachHang,
                                    Ten = item.TenKhachHang ?? "",
                                    IsKhachHang = true,
                                    Status = true,
                                    CreatedDate = DateTime.Now,
                                    ModifyDate = DateTime.Now,
                                    LoaiKhachHang = 1
                                });
                                //thêm nhật ký khi insert
                                //Isnsert nhật ký
                                var nk = new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Them,
                                    RefType = RefType.KhachHang,
                                    ThamChieu = "Mã: " + dtInserted.Ma,
                                    RefId = dtInserted.DoiTuongId
                                };

                                await _nhatKyTruyCapService.InsertAsync(nk);
                            }
                        }
                        #endregion
                        #region Hàng hóa dịch vụ

                        foreach (var hdct in item.HoaDonChiTiets)
                        {
                            if (string.IsNullOrEmpty(hdct.HoaDonDienTuChiTietId)) hdct.HoaDonDienTuChiTietId = Guid.NewGuid().ToString();
                            #region Làm tròn số theo tùy chọn chung
                            if (hdct.SoLuong.HasValue) hdct.SoLuong = hdct.SoLuong.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.SO_LUONG);

                            if (hdct.TyLeChietKhau.HasValue) hdct.TyLeChietKhau = hdct.TyLeChietKhau.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE);

                            if (hdct.DonGia.HasValue) hdct.DonGia = hdct.DonGia.Value.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.DON_GIA_QUY_DOI : LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                            if (hdct.ThanhTien.HasValue) hdct.ThanhTien = hdct.ThanhTien.Value.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.DON_GIA_QUY_DOI : LoaiDinhDangSo.DON_GIA_NGOAI_TE);

                            if (hdct.ThanhTienQuyDoi.HasValue) hdct.ThanhTienQuyDoi = hdct.ThanhTienQuyDoi.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);

                            if (hdct.TienChietKhau.HasValue) hdct.TienChietKhau = hdct.TienChietKhau.Value.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);

                            if (hdct.TienChietKhauQuyDoi.HasValue) hdct.TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);

                            if (hdct.TienThueGTGT.HasValue) hdct.TienThueGTGT = hdct.TienThueGTGT.Value.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);

                            if (hdct.TienThueGTGTQuyDoi.HasValue) hdct.TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);

                            if (hdct.TyLePhanTramDoanhThu.HasValue) hdct.TyLePhanTramDoanhThu = hdct.TyLePhanTramDoanhThu.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE);

                            if (hdct.TienGiam.HasValue) hdct.TienGiam = hdct.TienGiam.Value.MathRoundNumberByTuyChon(_tuyChons, item.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE);

                            if (hdct.TienGiamQuyDoi.HasValue) hdct.TienGiamQuyDoi = hdct.TienGiamQuyDoi.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.TIEN_QUY_DOI);
                            #endregion

                            #region  Đơn vị tính
                            var donViTinh = listDVT.Find(x => string.Compare(x.Ten.ToUpper().Trim(), hdct.TenDonViTinh.ToUpper().Trim()) == 0);
                            if (!string.IsNullOrEmpty(hdct.TenDonViTinh) && donViTinh != null)
                            {
                                hdct.DonViTinhId = donViTinh.DonViTinhId;
                                hdct.TenDonViTinh = donViTinh.Ten;
                            }

                            if (!string.IsNullOrEmpty(hdct.TenDonViTinh) && hdct.TenDonViTinh.Length <= 50 && donViTinh == null)
                            {
                                //msg += "<Đơn vị tính> chưa tồn tại trong hệ thống";
                                //Insert 
                                var dvt = await _donViTinhService.InsertAsync(new DonViTinhViewModel
                                {
                                    Ten = hdct.TenDonViTinh,
                                    MoTa = item.IsPos != null ? "MTT gửi sang GP" : string.Empty,
                                    Status = true
                                });
                                if (dvt != null)
                                {
                                    hdct.DonViTinhId = dvt.DonViTinhId;
                                }
                            }
                            #endregion

                            #region hàng hóa dịch vụ
                            if (!string.IsNullOrEmpty(hdct.MaHang))
                            {
                                var hhdvExists = await _hangHoaDichVuService.GetHangHoaDichVuByMa(hdct.MaHang);
                                if (hhdvExists != null)
                                {
                                    hdct.HangHoaDichVuId = hhdvExists.HangHoaDichVuId;
                                }
                                else
                                {
                                    var kqHHDV = await _hangHoaDichVuService.InsertAsync(new HangHoaDichVuViewModel
                                    {
                                        Status = true,
                                        Ma = hdct.MaHang,
                                        Ten = hdct.TenHang,
                                        TenDonViTinh = hdct.TenDonViTinh,
                                        DonViTinhId = hdct.DonViTinhId
                                    });
                                    if (kqHHDV != null)
                                    {
                                        hdct.HangHoaDichVuId = kqHHDV.HangHoaDichVuId;
                                        //Isnsert nhật ký
                                        var nk = new NhatKyTruyCapViewModel
                                        {
                                            LoaiHanhDong = LoaiHanhDong.Them,
                                            RefType = RefType.HangHoaDichVu,
                                            ThamChieu = "Mã: " + kqHHDV.Ma,
                                            RefId = kqHHDV.HangHoaDichVuId
                                        };

                                        await _nhatKyTruyCapService.InsertAsync(nk);
                                        result = true;
                                        success++;
                                    }
                                    else
                                    {
                                        var okRsError = Ok(new
                                        {
                                            status = false,
                                            index_item_error = stt_item,
                                            errorMessage = "Lỗi InsertAsync HHDV"
                                        });
                                        listOkRsError.Add(okRsError);
                                    }
                                }
                            }

                            #endregion

                            if (!string.IsNullOrEmpty(hdct.ThueGTGT) && hdct.ThueGTGT.CheckValidThueGTGT() && hdct.ThueGTGT.Contains("KHAC") == true)
                            {
                                //set lại thuế khác
                                hdct.ThueGTGT = TextHelper.ConvertThueExcetToDB(hdct.ThueGTGT);
                            }
                        }
                        #endregion

                        if (!item.LoaiChungTu.HasValue) item.LoaiChungTu = 4;//SET Mặc định Từ các bên sử dụng API
                        if (!item.TyLeChietKhau.HasValue) item.TyLeChietKhau = 0;//SET Mặc định Từ các bên sử dụng API

                        if (!string.IsNullOrEmpty(item.HoaDonDienTuId))
                        {
                            var itemExists = await _hoaDonDienTuService.PosGetByIdAsync(item.HoaDonDienTuId);
                            if ((itemExists.BoKyHieuHoaDon.HinhThucHoaDon != HinhThucHoaDon.CoMaTuMayTinhTien && (itemExists.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaKyDienTu) || (itemExists.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.KyDienTuLoi) || (itemExists.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiTCTNLoi)) || (itemExists.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien && (itemExists.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaPhatHanh || itemExists.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaGuiCQT)))
                            {
                                await _hoaDonDienTuChiTietService.RemoveRangeAsync(item.HoaDonDienTuId);
                                await _hoaDonDienTuChiTietService.InsertRangeAsync(item, item.HoaDonChiTiets);
                                item.ModifyDate = DateTime.Now;
                                if (!item.TrangThai.HasValue) item.TrangThai = itemExists.TrangThai;
                                if (!item.Status.HasValue) item.Status = itemExists.Status;
                                if (!item.TrangThaiQuyTrinh.HasValue) item.TrangThaiQuyTrinh = itemExists.TrangThaiQuyTrinh;
                                if (string.IsNullOrEmpty(item.MauSo)) item.MauSo = itemExists.MauSo;
                                if (string.IsNullOrEmpty(item.MaTraCuu)) item.MaTraCuu = itemExists.MaTraCuu;
                                if (string.IsNullOrEmpty(item.FileChuaKy)) item.FileChuaKy = itemExists.FileChuaKy;
                                if (string.IsNullOrEmpty(item.XMLChuaKy)) item.XMLChuaKy = itemExists.XMLChuaKy;
                                if (string.IsNullOrEmpty(item.MaCuaCQT)) item.MaCuaCQT = itemExists.MaCuaCQT;
                                if (string.IsNullOrEmpty(item.MCCQT)) item.MCCQT = itemExists.MCCQT;
                                if (string.IsNullOrEmpty(item.CreatedBy)) item.CreatedBy = itemExists.CreatedBy;
                                if (item.CreatedDate == null) item.CreatedDate = itemExists.CreatedDate;
                                if (item.NgayLap == null) item.NgayLap = itemExists.NgayLap;
                                if (!item.LoaiApDungHoaDonDieuChinh.HasValue) item.LoaiApDungHoaDonDieuChinh = itemExists.LoaiApDungHoaDonDieuChinh;
                                if (!item.IsGiamTheoNghiQuyet.HasValue) item.IsGiamTheoNghiQuyet = itemExists.IsGiamTheoNghiQuyet;
                                if (!item.TyLePhanTramDoanhThu.HasValue) item.TyLePhanTramDoanhThu = itemExists.TyLePhanTramDoanhThu;
                                if (!item.IsNopThueTheoThongTu1032014BTC.HasValue) item.IsNopThueTheoThongTu1032014BTC = itemExists.IsNopThueTheoThongTu1032014BTC;
                                if (!item.SoHoaDon.HasValue) item.SoHoaDon = itemExists.SoHoaDon;
                                if (!item.TyLeChietKhau.HasValue) item.TyLeChietKhau = itemExists.TyLeChietKhau;

                                result = await _hoaDonDienTuService.UpdateAsync(item);

                                if (result)
                                {
                                    var shdon = itemExists.SoHoaDon != 0 ? itemExists.SoHoaDon.ToString() : "<Chưa cấp số>";
                                    success++;
                                    //Isnsert nhật ký
                                    var nk = new NhatKyThaoTacHoaDonViewModel
                                    {
                                        HoaDonDienTuId = item.HoaDonDienTuId,
                                        LoaiThaoTac = (int)LoaiThaoTac.SuaHoaDon,
                                        MoTa = "Sửa hóa đơn lúc " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                                        NguoiThucHienId = User.GetUserId(),
                                        NgayGio = DateTime.Now,
                                        HasError = !result
                                    };
                                    await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(nk);
                                    await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                                    {
                                        LoaiHanhDong = LoaiHanhDong.SuaBoiBKPOS,
                                        RefType = RefType.HoaDonDienTu,
                                        DoiTuongThaoTac = "Tên loại hóa đơn: " + itemExists.BoKyHieuHoaDon.LoaiHoaDon.GetTenLoaiHoaDonFull(),
                                        ThamChieu = "Số hóa đơn " + shdon + " \nNgày hóa đơn " + item.NgayHoaDon.Value.ToString("dd/MM/yyyy"),
                                        RefId = item.HoaDonDienTuId,
                                        DuLieuCu = JsonConvert.SerializeObject(itemExists),
                                        DuLieuMoi = JsonConvert.SerializeObject(itemPosGoc)
                                    });
                                    listUpdateSuccess.Add(item.HoaDonDienTuId);
                                }
                                else
                                {
                                    var okRsError = Ok(new
                                    {
                                        status = false,
                                        index_item_error = stt_item,
                                        errorMessage = "Lỗi UpdateAsync"
                                    });
                                    listOkRsError.Add(okRsError);
                                }
                            }
                            else
                            {
                                var okRsError = Ok(new
                                {
                                    status = false,
                                    index_item_error = stt_item,
                                    errorMessage = "Hệ thống chỉ cho phép thực hiện <Sửa> hóa đơn có trạng thái quy trình là <Chưa ký điện tử>(giá trị = 0), <Ký điện tử lỗi>(giá trị = 2), <Gửi TCTN lỗi>(giá trị = 4), hoặc hóa đơn từ máy tính tiền có trạng thái quy trình <Chưa phát hành>(giá trị = 13), <Chưa gửi CQT> (giá trị = 12). Vui lòng kiểm tra lại!"
                                });
                                listOkRsError.Add(okRsError);
                            }
                        }
                        else
                        {
                            item.HoaDonDienTuId = Guid.NewGuid().ToString();
                            if (item.NgayLap.HasValue == false) item.NgayLap = DateTime.Now;
                            item.TrangThai = 1;//hóa đơn gốc

                            List<HoaDonDienTuChiTietViewModel> hoaDonDienTuChiTiets = item.HoaDonChiTiets;

                            var kq = await _hoaDonDienTuService.InsertAsync(item);
                            if (kq != null)
                            {
                                result = true;
                                success++;
                                var models = await _hoaDonDienTuChiTietService.InsertRangeAsync(kq, hoaDonDienTuChiTiets);
                                kq.HoaDonChiTiets = models;
                                if (models.Count != hoaDonDienTuChiTiets.Count)
                                {
                                    var okRsError = Ok(new
                                    {
                                        status = false,
                                        index_item_error = stt_item,
                                        errorMessage = "Lỗi InsertAsync"
                                    });
                                    listOkRsError.Add(okRsError);
                                }
                                listInsertSuccess.Add(kq);
                                await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Them,
                                    RefType = RefType.HoaDonDienTu,
                                    DoiTuongThaoTac = "Hệ thống đã thêm hóa đơn",
                                    MoTaChiTiet = "Hóa đơn được thêm bởi phần mềm đã kết nối " + item.PosCustomerURL,
                                    RefId = kq.HoaDonDienTuId
                                });
                                //Check Auto ký hd
                                var tuychonKy = _tuyChons.FirstOrDefault(x => x.Ma == "IsSelectChuKiCung");
                                var tuychonAutoSend = _tuyChons.FirstOrDefault(x => x.Ma == "BoolAutoSendHd");
                                if (kq.PhatHanhNgayPos == 1 && tuychonKy != null && tuychonKy.GiaTri != "KiCung" && tuychonAutoSend != null && tuychonAutoSend.GiaTri == "true")
                                {
                                    //nếu hình thức hóa đơn không phải là hd khơi tạo từ máy tính tiền
                                    if (kq.BoKyHieuHoaDon.HinhThucHoaDon != HinhThucHoaDon.CoMaTuMayTinhTien)
                                    {
                                        if (kq.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaKyDienTu
                                            || kq.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DangKyDienTu
                                            || kq.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.KyDienTuLoi
                                            || kq.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiTCTNLoi)
                                        {
                                            var kshDon = await _hoaDonDienTuService.CreateSoHoaDon(kq);
                                            var hdCreateXml = await _hoaDonDienTuService.GetByIdAsync(kq.HoaDonDienTuId);
                                            hdCreateXml.SoHoaDon = kshDon.SoHoaDon;
                                            var parmCheckPhatHanh = new ParamPhatHanhHD();
                                            parmCheckPhatHanh.HoaDonDienTuId = hdCreateXml.HoaDonDienTuId;
                                            parmCheckPhatHanh.SkipCheckHetHieuLucTrongKhoang = false;
                                            parmCheckPhatHanh.SkipChecNgayKyLonHonNgayHoaDon = false;
                                            parmCheckPhatHanh.SkipCheckChenhLechThanhTien = false;
                                            parmCheckPhatHanh.SkipCheckChenhLechTienChietKhau = false;
                                            parmCheckPhatHanh.SkipCheckChenhLechTienThueGTGT = false;
                                            parmCheckPhatHanh.IsPhatHanh = true;
                                            parmCheckPhatHanh.HoaDon = hdCreateXml;
                                            var checkPhatHanh = await _hoaDonDienTuService.CheckHoaDonPhatHanhAsync(parmCheckPhatHanh);
                                            if (checkPhatHanh == null)
                                            {
                                                var hoSoHDDT = await _HoSoHDDTService.GetDetailAsync();
                                                var tTChungThongDiep = new TTChungThongDiep();
                                                tTChungThongDiep.PBan = "2.0.0";
                                                tTChungThongDiep.MNGui = _configuration["TTChung:MNGui"];
                                                tTChungThongDiep.MNNhan = _configuration["TTChung:MNNhan"];
                                                tTChungThongDiep.MLTDiep = hdCreateXml.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa ? "200" : "203";
                                                tTChungThongDiep.MTDiep = string.Format("{0}{1}", tTChungThongDiep.MNGui, Guid.NewGuid().ToString().Replace("-", "")).ToUpper();
                                                tTChungThongDiep.MST = hoSoHDDT.MaSoThue;
                                                tTChungThongDiep.SLuong = 1;
                                                tTChungThongDiep.MTDTChieu = "";
                                                hdCreateXml.TTChungThongDiep = tTChungThongDiep;
                                                hdCreateXml.IsPhatHanh = true;
                                                var rawXml = await _hoaDonDienTuService.CreateXMLToSignAsync(hdCreateXml);
                                                var userkeySign = tuychonKy.GiaTri.Split('|')[1];
                                                var passCode = tuychonKy.GiaTri.Split('|')[2];
                                                var dataKy = new MessageObj();
                                                dataKy.DataXML = rawXml.Base64;
                                                dataKy.UserkeySign = userkeySign;
                                                dataKy.PassCode = passCode;
                                                dataKy.MLTDiep = MLTDiep.TDCDLHDKMDCQThue;
                                                var xmlSigned = await _SignCloudService.SignCloudFile(dataKy);
                                                parmCheckPhatHanh.DataXML = xmlSigned.XMLSigned;
                                                var updateDataXml = await _hoaDonDienTuService.GateForWebSocket(parmCheckPhatHanh);
                                                if (updateDataXml)
                                                {
                                                    var ttChungModel = new ThongDiepChungViewModel();
                                                    ttChungModel.PhienBan = "2.0.0";
                                                    ttChungModel.MaNoiGui = _configuration["TTChung:MNGui"];
                                                    ttChungModel.MaNoiNhan = _configuration["TTChung:MNNhan"];
                                                    ttChungModel.MaLoaiThongDiep = hdCreateXml.BoKyHieuHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa ? 200 : 203;
                                                    ttChungModel.MaThongDiep = string.Format("{0}{1}", ttChungModel.MaNoiGui, Guid.NewGuid().ToString().Replace("-", "")).ToUpper();
                                                    ttChungModel.MaSoThue = hoSoHDDT.MaSoThue;
                                                    ttChungModel.MaThongDiepThamChieu = "";
                                                    ttChungModel.SoLuong = 1;
                                                    var dlGui = new DuLieuGuiHDDTViewModel();
                                                    dlGui.HoaDonDienTuId = hdCreateXml.HoaDonDienTuId;
                                                    dlGui.HoaDonDienTu = hdCreateXml;
                                                    ttChungModel.DuLieuGuiHDDT = dlGui;

                                                    var ttChung = await _DuLieuGuiHDDTService.InsertAsync(ttChungModel);
                                                    if (ttChung != null)
                                                    {
                                                        var sendCqt = await _DuLieuGuiHDDTService.GuiThongDiepDuLieuHDDTAsync(ttChung.ThongDiepChungId);
                                                        var nhatkytruycap = new NhatKyTruyCapViewModel
                                                        {
                                                            LoaiHanhDong = LoaiHanhDong.PhatHanhHoaDonThanhCong,
                                                            RefType = RefType.HoaDonDienTu,
                                                            DoiTuongThaoTac = "Hệ thống đã phát hóa đơn",
                                                            ThamChieu = "Số hóa đơn " + kshDon.SoHoaDon,
                                                            MoTaChiTiet = "Tự động phát hành hóa đơn.",
                                                            RefId = kq.HoaDonDienTuId
                                                        };
                                                        await _nhatKyTruyCapService.InsertAsync(nhatkytruycap);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var okRsError = Ok(new
                                                {
                                                    index_item_error = stt_item,
                                                    errorMessage = Ok(new
                                                    {
                                                        titleMessage = checkPhatHanh.TitleMessage,
                                                        errorMessage = checkPhatHanh.ErrorMessage
                                                    })
                                                });
                                                listOkRsError.Add(okRsError);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //nếu hình thức là hd máy tính tiền chưa gen mcqt

                                    }
                                }
                            }
                            else
                            {
                                var okRsError = Ok(new
                                {
                                    status = false,
                                    index_item_error = stt_item,
                                    errorMessage = "Lỗi InsertAsync"
                                });
                                listOkRsError.Add(okRsError);
                            }
                        }
                    }
                }
                Tracert.WriteLog($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}" + " PosInsertInvoices Insert Success:: " + listInsertSuccess.Count() + "|| PosInsertInvoices Update Success:: " + listUpdateSuccess.Count() + "|| PosInsertInvoices Error:: " + JsonConvert.SerializeObject(listOkRsError));

                return Ok(new
                {
                    status = listOkRsError.Count == 0,
                    num = model.Count,
                    numSuccess = success,
                    itemsError = listOkRsError,
                    itemsInsertSuccess = listInsertSuccess,
                    itemsUpdateSuccess_HoaDonDienTuId = listUpdateSuccess
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    num = model.Count,
                    itemsError = ex,
                });
            }
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(model.HoaDonDienTuId);
                    await _hoaDonDienTuChiTietService.InsertRangeAsync(model, model.HoaDonChiTiets);

                    bool result = await _hoaDonDienTuService.UpdateAsync(model);

                    if (result)
                    {
                        var _currentUser = await _userRespositories.GetById(HttpContext.User.GetUserId());
                        var nk = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = model.HoaDonDienTuId,
                            LoaiThaoTac = (int)LoaiThaoTac.SuaHoaDon,
                            MoTa = "Sửa hóa đơn lúc " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            NguoiThucHienId = _currentUser.UserId,
                            NgayGio = DateTime.Now,
                            HasError = !result
                        };
                        await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(nk);
                    }
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Ok(false);
                }
            }
        }

        [HttpPut("Update_TraCuu")]
        public async Task<IActionResult> Update_TraCuu(HoaDonDienTuViewModel model)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(model.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(model.HoaDonDienTuId);
                    await _hoaDonDienTuChiTietService.InsertRangeAsync(model, model.HoaDonChiTiets);

                    bool result = await _hoaDonDienTuService.UpdateAsync(model);

                    if (result)
                    {
                        var _currentUser = await _userRespositories.GetById(HttpContext.User.GetUserId());
                        var nk = new NhatKyThaoTacHoaDonViewModel
                        {
                            HoaDonDienTuId = model.HoaDonDienTuId,
                            LoaiThaoTac = (int)LoaiThaoTac.SuaHoaDon,
                            MoTa = "Sửa hóa đơn lúc " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                            NguoiThucHienId = _currentUser.UserId,
                            NgayGio = DateTime.Now,
                            HasError = !result
                        };
                        await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(nk);
                    }
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

        [HttpPost("XemHoaDonHangLoat")]
        public IActionResult XemHoaDonHangLoat(List<string> fileArray)
        {
            var result = _hoaDonDienTuService.XemHoaDonDongLoat(fileArray);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("XemHoaDonHangLoat2")]
        public IActionResult XemHoaDonDongLoat2(List<string> fileArray)
        {
            var result = _hoaDonDienTuService.XemHoaDonDongLoat2(fileArray);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    bool result = await _hoaDonDienTuService.DeleteAsync(id);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateSoHoaDon")]
        public async Task<IActionResult> CreateSoHoaDon(HoaDonDienTuViewModel mhd)
        {
            var result = await _hoaDonDienTuService.CreateSoHoaDon(mhd);
            return Ok(result);
        }

        [HttpGet("CreateSoCTXoaBoHoaDon")]
        public async Task<IActionResult> CreateSoCTXoaBoHoaDon()
        {
            var result = await _hoaDonDienTuService.CreateSoCTXoaBoHoaDon();
            return Ok(new { Data = result });
        }

        [HttpGet("CreateSoBienBanXoaBoHoaDon")]
        public async Task<IActionResult> CreateSoBienBanXoaBoHoaDon()
        {
            var result = await _hoaDonDienTuService.CreateSoBienBanXoaBoHoaDon();
            return Ok(new { Data = result });
        }

        [HttpPost("TaiHoaDon")]
        public async Task<IActionResult> TaiHoaDon(HoaDonDienTuViewModel hoaDonDienTu)
        {
            var result = await _hoaDonDienTuService.TaiHoaDon(hoaDonDienTu);
            return Ok(result);
        }

        [HttpPost("TaiHoaDon_Multiple")]
        public async Task<IActionResult> TaiHoaDon_Multiple(List<HoaDonDienTuViewModel> listHD)
        {
            var result = new List<KetQuaConvertPDF>();
            foreach (var hd in listHD)
            {
                var hd123 = await _hoaDonDienTuService.GetByIdAsync(hd.HoaDonDienTuId);
                result.Add(await _hoaDonDienTuService.TaiHoaDon(hd123));
            }

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("TaiHoaDon_TraCuu")]
        public async Task<IActionResult> TaiHoaDon_TraCuu(HoaDonDienTuViewModel hoaDonDienTu)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hoaDonDienTu.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.TaiHoaDon(hoaDonDienTu);
            return Ok(result);
        }

        [HttpPost("ThemNhatKyThaoTacHoaDon")]
        public async Task<IActionResult> ThemNhatKyThaoTacHoaDon(NhatKyThaoTacHoaDonViewModel model)
        {
            var result = await _hoaDonDienTuService.ThemNhatKyThaoTacHoaDonAsync(model);
            return Ok(result);
        }
        [HttpPost("ConvertHoaDonToFilePDF")]
        public async Task<IActionResult> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd)
        {
            try
            {
                var hd123 = await _hoaDonDienTuService.GetByIdAsync(hd.HoaDonDienTuId);
                var result = await _hoaDonDienTuService.ConvertHoaDonToFilePDF(hd123);

                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(null);
            }
        }

        [HttpPost("ConvertHoaDonToFilePDF_Multiple")]
        public async Task<IActionResult> ConvertHoaDonToFilePDF_Multiple(List<HoaDonDienTuViewModel> listHD)
        {
            try
            {
                var result = new List<KetQuaConvertPDF>();
                foreach (var hd in listHD)
                {
                    var hd123 = await _hoaDonDienTuService.GetByIdAsync(hd.HoaDonDienTuId);
                    result.Add(await _hoaDonDienTuService.ConvertHoaDonToFilePDF(hd123));
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(null);
            }
        }

        [AllowAnonymous]
        [HttpPost("ConvertHoaDonToFilePDF_TraCuu")]
        public async Task<IActionResult> ConvertHoaDonToFilePDF_TraCuu(HoaDonDienTuViewModel hd)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hd.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.ConvertHoaDonToFilePDF(hd);
            return Ok(result);
        }

        [HttpGet("GetTrangThaiHoaDon")]
        public async Task<IActionResult> GetTrangThaiHoaDon()
        {
            var result = await _hoaDonDienTuService.GetTrangThaiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetTrangThaiGuiHoaDon")]
        public async Task<IActionResult> GetTrangThaiGuiHoaDon()
        {
            var result = await _hoaDonDienTuService.GetTrangThaiGuiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetTreeTrangThai")]
        public async Task<IActionResult> GetTreeTrangThai(int LoaiHoaDon, DateTime fromDate, DateTime toDate)
        {
            var result = await _hoaDonDienTuService.GetTreeTrangThai(LoaiHoaDon, fromDate, toDate);
            return Ok(result);
        }

        [HttpPost("DeleteRangeHoaDonDienTu")]
        public IActionResult DeleteRangeHoaDonDienTu(DeleteRangeHDDTParams @params)
        {
            var result = _hoaDonDienTuService.DeleteRangeHoaDonDienTuAsync(@params.ListHoaDon);
            return Ok(result);
        }

        [HttpPost("InsertThongBaoGuiHoaDonSaiSot303")]
        public async Task<IActionResult> InsertThongBaoGuiHoaDonSaiSot303(ThongDiepGuiCQTViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    KetQuaLuuThongDiep result = await _IThongDiepGuiNhanCQTService.InsertThongBaoGuiHoaDonSaiSot303Async(model);
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
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }
        [HttpPost("GateForWebSocket")]
        public async Task<IActionResult> GateForWebSocket(ParamPhatHanhHD @params)
        {
            if (@params.HoaDon == null || string.IsNullOrEmpty(@params.HoaDonDienTuId))
            {
                return BadRequest();
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.GateForWebSocket(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    Tracert.WriteLog("testEX: ", e);
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPost("PhatHanhHoaDonCoMaTuMTT")]
        public async Task<IActionResult> PhatHanhHoaDonCoMaTuMTT(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.PhatHanhHoaDonCoMaTuMTT(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    Tracert.WriteLog("testEX: ", e);
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPost("WaitForTCTResonse")]
        public async Task<IActionResult> WaitForTCTResonse(ParamPhatHanhHD @params)
        {
            if (string.IsNullOrEmpty(@params.HoaDonDienTuId))
            {
                return BadRequest();
            }

            var result = await _hoaDonDienTuService.WaitForTCTResonseAsync(@params.HoaDonDienTuId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("GateForWebSocket_TraCuu")]
        public async Task<IActionResult> GateForWebSocket_TraCuu(ParamPhatHanhHD @params)
        {
            if (@params.HoaDon == null || string.IsNullOrEmpty(@params.HoaDonDienTuId))
            {
                return BadRequest();
            }

            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(@params.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _hoaDonDienTuService.GateForWebSocket(@params))
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else transaction.Rollback();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("SendMailAsync")]
        public async Task<IActionResult> SendMailAsync(ParamsSendMail hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.SendEmailAsync(hd);
                    if (result == true)
                        transaction.Commit();
                    else transaction.Rollback();

                    if (hd.ErrorActionModel != null)
                    {
                        hd.ErrorActionModel.RefId = hd.HoaDon.HoaDonDienTuId;
                        await _nhatKyThaoTacLoiService.InsertAsync(hd.ErrorActionModel);
                    }

                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("SendEmailThongTinHoaDon")]
        public async Task<IActionResult> SendEmailThongTinHoaDon(ParamsSendMailThongTinHoaDon hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.SendEmailThongTinHoaDonAsync(hd);
                    if (result == true)
                        transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("SendEmailThongBaoSaiThongTin")]
        public async Task<IActionResult> SendEmailThongBaoSaiThongTin(ParamsSendMailThongBaoSaiThongTin hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.SendEmailThongBaoSaiThongTinAsync(hd);
                    if (result == true)
                        transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("ConvertHoaDonToHoaDonGiay")]
        public async Task<IActionResult> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.ConvertHoaDonToHoaDonGiay(hd);
                if (result != null)
                {
                    transaction.Commit();
                    return File(result.Bytes, result.ContentType, result.FileName);
                }
                else transaction.Rollback();

                return Ok(result);
            }
        }

        [HttpPost("ConvertHoaDonToHoaDonGiay2")]
        public async Task<IActionResult> ConvertHoaDonToHoaDonGiay2(ParamsChuyenDoiThanhHDGiay hd)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.ConvertHoaDonToHoaDonGiay(hd);
                if (result != null)
                {
                    transaction.Commit();
                    return Ok(new { result = hd.FilePath });
                }
                else transaction.Rollback();

                return Ok(result);
            }
        }

        [AllowAnonymous]
        [HttpPost("ConvertHoaDonToHoaDonGiay_TraCuu")]
        public async Task<IActionResult> ConvertHoaDonToHoaDonGiay_TraCuu(ParamsChuyenDoiThanhHDGiay hd)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hd.HoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.ConvertHoaDonToHoaDonGiay(hd);
                if (result != null)
                {
                    transaction.Commit();
                    return File(result.Bytes, result.ContentType, result.FileName);
                }
                else transaction.Rollback();

                return Ok(result);
            }
        }

        [HttpGet("XemLichSuHoaDon/{id}")]
        public async Task<IActionResult> XemLichSuHoaDon(string id)
        {
            var result = await _hoaDonDienTuService.XemLichSuHoaDon(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("TraCuuByMa/{MaTraCuu}")]
        public async Task<IActionResult> TraCuuByMa(string MaTraCuu)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByLookupCodeAsync(MaTraCuu.Trim());

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = await _traCuuService.TraCuuByMa(MaTraCuu.Trim());
            var res = await _hoaDonDienTuService.ConvertHoaDonToFilePDF(result);
            return Ok(new { data = result, path = res.FilePDF });
        }

        [AllowAnonymous]
        [HttpPost("TraCuuBySoHoaDon")]
        public async Task<IActionResult> TraCuuBySoHoaDon(KetQuaTraCuuXML input)
        {
            CompanyModel companyModel = await _databaseService.GetDetailBySoHoaDonAsync(input);

            if (companyModel != null)
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

                var result = await _traCuuService.TraCuuBySoHoaDon(input);
                var res = await _hoaDonDienTuService.ConvertHoaDonToFilePDF(result);
                return Ok(new { data = result, path = res.FilePDF });
            }
            else return Ok(null);
        }

        [AllowAnonymous]
        [HttpPost("GetMaTraCuuInXml")]
        public async Task<IActionResult> GetMaTraCuuInXml([FromForm] IFormFile file)
        {
            var result = await _traCuuService.GetMaTraCuuInXml(file);
            return Ok(result);
        }

        [HttpPost("TienLuiChungTu")]
        public async Task<IActionResult> TienLuiChungTu(TienLuiViewModel model)
        {
            var result = await _hoaDonDienTuService.TienLuiChungTuAsync(model);
            return Ok(result);
        }

        [HttpGet("GetBienBanXoaBoHoaDon/{id}")]
        public async Task<IActionResult> GetBienBanXoaBoHoaDon(string id)
        {
            var result = await _hoaDonDienTuService.GetBienBanXoaBoHoaDon(id);
            return Ok(result);
        }
        [HttpGet("GetHoaDonGocCuaHDBI/{id}/{loai}")]
        public async Task<IActionResult> GetHoaDonGocCuaHDBI(string id, string loai)
        {
            var result = await _hoaDonDienTuService.GetHoaDonGocCuaHDBI(id, loai);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetBienBanXoaBoHoaDonById/{id}")]
        public async Task<IActionResult> GetBienBanXoaBoHoaDonById(string id)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByBienBanXoaBoIdAsync(id);

            if (companyModel == null) return Ok(null);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.GetBienBanXoaBoById(id);
            return Ok(result);
        }

        [HttpPost("SaveBienBanXoaHoaDon")]
        public async Task<IActionResult> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.SaveBienBanXoaHoaDon(model);
                transaction.Commit();
                return Ok(result);
            }
        }

        [HttpPost("CapNhatBienBanXoaBoHoaDon")]
        public async Task<IActionResult> CapNhatBienBanXoaBoHoaDon(BienBanXoaBoViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _hoaDonDienTuService.CapNhatBienBanXoaBoHoaDon(model);
                if (result)
                {
                    transaction.Commit();
                }
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [HttpDelete("DeleteBienBanXoaHoaDon/{Id}")]
        public async Task<IActionResult> DeleteBienBanXoaHoaDon(string Id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var entity = await _hoaDonDienTuService.GetBienBanXoaBoById(Id);
                HoaDonDienTuViewModel entityHD = null;
                if (entity.HoaDonDienTuId != null && entity.ThongTinHoaDonId == null)
                {
                    entityHD = await _hoaDonDienTuService.GetByIdAsync(entity.HoaDonDienTuId);
                }

                var result = await _hoaDonDienTuService.DeleteBienBanXoaHoaDon(Id);
                if (result)
                {
                    if (entityHD != null)
                    {
                        entityHD.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaLap;
                        if (await _hoaDonDienTuService.UpdateAsync(entityHD))
                            transaction.Commit();
                        else
                        {
                            result = false;
                            transaction.Rollback();
                        }
                    }
                    else if (entity.ThongTinHoaDonId != null)
                    {
                        var ttUpdated = await _db.ThongTinHoaDons.FirstOrDefaultAsync(x => x.Id == entity.ThongTinHoaDonId);
                        ttUpdated.TrangThaiBienBanXoaBo = (int)TrangThaiBienBanXoaBo.ChuaLap;
                        ttUpdated.ModifyDate = DateTime.Now;
                        _db.Entry(ttUpdated).CurrentValues.SetValues(ttUpdated);

                        if (await _db.SaveChangesAsync() > 0) transaction.Commit();
                    }
                    else
                    {
                        //trường hợp xóa biên bản hủy hóa đơn ở hóa đơn bên ngoài
                        transaction.Commit();
                    }
                }
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [HttpPost("KyBienBanXoaBo_NB")]
        public async Task<IActionResult> KyBienBanXoaBo_NB(ParamKyBienBanHuyHoaDon @params)
        {
            if (@params.BienBan == null)
            {
                return BadRequest();
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _hoaDonDienTuService.GateForWebSocket(@params))
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else transaction.Rollback();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [AllowAnonymous]
        [HttpPost("KyBienBanXoaBo")]
        public async Task<IActionResult> KyBienBanXoaBo(ParamKyBienBanHuyHoaDon @params)
        {
            if (@params.BienBan == null)
            {
                return BadRequest();
            }

            CompanyModel companyModel = await _databaseService.GetDetailByBienBanXoaBoIdAsync(@params.BienBan.Id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (await _hoaDonDienTuService.GateForWebSocket(@params))
                    {
                        transaction.Commit();
                        return Ok(true);
                    }
                    else transaction.Rollback();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("ConvertBienBanXoaBoToFilePDF_NB")]
        public async Task<IActionResult> ConvertBienBanXoaBoToFilePDF_NB(BienBanXoaBoViewModel bb)
        {
            var result = await _hoaDonDienTuService.ConvertBienBanXoaHoaDon(bb);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("ConvertBienBanXoaBoToFilePDF")]
        public async Task<IActionResult> ConvertBienBanXoaBoToFilePDF(BienBanXoaBoViewModel bb)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByBienBanXoaBoIdAsync(bb.Id);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _hoaDonDienTuService.ConvertBienBanXoaHoaDon(bb);
            return Ok(result);
        }

        [HttpPost("XoaBoHoaDon")]
        public async Task<IActionResult> XoaBoHoaDon(ParamXoaBoHoaDon @params)
        {
            if (@params.HoaDon == null)
            {
                return Ok(null);
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.XoaBoHoaDon(@params);
                    if (!string.IsNullOrEmpty(result))
                    {
                        transaction.Commit();
                        return Ok(new { result });
                    }
                    else
                    {
                        transaction.Rollback();
                        return Ok(null);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpGet("GetStatusDaThayTheHoaDon/{HoaDonId}")]
        public async Task<IActionResult> GetStatusDaThayTheHoaDon(string HoaDonId)
        {
            var result = await _hoaDonDienTuService.GetStatusDaThayTheHoaDon(HoaDonId);
            return Ok(result);
        }

        [HttpGet("UpdateIsTBaoHuyKhongDuocChapNhan")]
        public async Task<IActionResult> UpdateIsTBaoHuyKhongDuocChapNhan()
        {
            var result = await _hoaDonDienTuService.UpdateIsTBaoHuyKhongDuocChapNhan();
            return Ok(result);
        }

        [HttpGet("GetDSRutGonBoKyHieuHoaDon")]
        public async Task<IActionResult> GetDSRutGonBoKyHieuHoaDon()
        {
            var result = await _hoaDonDienTuService.GetDSRutGonBoKyHieuHoaDonAsync();
            return Ok(result);
        }

        [HttpGet("GetDSXoaBoChuaLapThayThe/{loaiNghiepVu}")]
        public async Task<IActionResult> GetDSXoaBoChuaLapThayThe(int? loaiNghiepVu)
        {
            var result = await _hoaDonDienTuService.GetDSXoaBoChuaLapThayTheAsync(loaiNghiepVu);
            return Ok(result);
        }
        [HttpGet("GetHoaDonDaLapBbChuaXoaBo/{loaiNghiepVu}")]
        public async Task<IActionResult> GetHoaDonDaLapBbChuaXoaBo(int? loaiNghiepVu)
        {
            var result = await _hoaDonDienTuService.GetHoaDonDaLapBbChuaXoaBoAsync(loaiNghiepVu);
            return Ok(result);
        }

        [HttpPost("GetDSHdDaXoaBo")]
        public async Task<IActionResult> GetDSHdDaXoaBo(HoaDonParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetDSHdDaXoaBo(pagingParams);
            return Ok(result);
        }
        [HttpPost("GetDSHoaDonDeXoaBo")]
        public async Task<IActionResult> GetDSHoaDonDeXoaBo(HoaDonParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetDSHoaDonDeXoaBo(pagingParams);
            return Ok(result);
        }

        [HttpPut("UpdateTrangThaiQuyTrinh")]
        public async Task<IActionResult> UpdateTrangThaiQuyTrinh(HoaDonDienTuViewModel model)
        {
            await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(model.HoaDonDienTuId, (TrangThaiQuyTrinh)model.TrangThaiQuyTrinh);
            return Ok(true);
        }

        [HttpPut("UpdateMultiTrangThaiQuyTrinh")]
        public async Task<IActionResult> UpdateMultiTrangThaiQuyTrinh(List<HoaDonDienTuViewModel> models)
        {
            foreach (var model in models)
            {
                await _hoaDonDienTuService.UpdateTrangThaiQuyTrinhAsync(model.HoaDonDienTuId, (TrangThaiQuyTrinh)model.TrangThaiQuyTrinh);
            }
            return Ok(true);
        }

        [HttpDelete("RemoveDigitalSignature/{id}")]
        public async Task<IActionResult> RemoveDigitalSignature(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.RemoveDigitalSignatureAsync(id);
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

        [AllowAnonymous]
        [HttpPost("ReloadPDF")]
        public async Task<IActionResult> ReloadPDF(ReloadPDFParams @params)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(@params.MaSoThue);
            if (companyModel == null)
            {
                return Ok(new ReloadPDFResult
                {
                    Status = false,
                    Message = "Mã số thuế không tồn tại"
                });
            }

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.ReloadPDFAsync(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Ok(new ReloadPDFResult
                    {
                        Status = false,
                        Message = "Exception: " + e.Message
                    });
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("DownloadXML")]
        public async Task<IActionResult> DowloadXML(string id, string maSoThue)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(maSoThue);
            if (companyModel == null)
            {
                return NotFound();
            }

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = await _hoaDonDienTuService.DowloadXMLAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("ImportHoaDon")]
        public async Task<IActionResult> ImportHoaDon([FromForm] NhapKhauParams @params)
        {
            var result = await _hoaDonDienTuService.ImportHoaDonAsync(@params);
            return Ok(result);
        }

        [HttpPost("ImportPhieuXuatKho")]
        public async Task<IActionResult> ImportPhieuXuatKho([FromForm] NhapKhauParams @params)
        {
            var result = await _hoaDonDienTuService.ImportPhieuXuatKhoAsync(@params);
            return Ok(result);
        }

        [HttpPost("InsertImportHoaDon")]
        public async Task<IActionResult> InsertImportHoaDon(List<HoaDonDienTuImport> data)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.InsertImportHoaDonAsync(data);
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

        [HttpPost("CreateFileImportHoaDonError")]
        public IActionResult CreateFileImportHoaDonError(NhapKhauResult data)
        {
            var result = _hoaDonDienTuService.CreateFileImportHoaDonError(data);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpGet("GetNgayHienTai")]
        public IActionResult GetNgayHienTai()
        {
            var result = _hoaDonDienTuService.GetNgayHienTai();
            return Ok(new { result });
        }

        [AllowAnonymous]
        [HttpPost("ReloadXML")]
        public async Task<IActionResult> ReloadXML(IFormFile formFile, string maSoThue)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(maSoThue);
            if (companyModel == null)
            {
                return NotFound();
            }

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.ReloadXMLAsync(new ReloadXmlParams
                    {
                        File = formFile,
                        MaSoThue = maSoThue
                    });
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

        [AllowAnonymous]
        [HttpPost("InsertThongDiepChung")]
        public async Task<IActionResult> InsertThongDiepChung(IFormFile formFile, string maSoThue)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(maSoThue);
            if (companyModel == null)
            {
                return NotFound();
            }

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.InsertThongDiepChungAsync(new ReloadXmlParams
                    {
                        File = formFile,
                        MaSoThue = maSoThue
                    });
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

        [HttpGet("KiemTraHoaDonDaLapTBaoCoSaiSot/{HoaDonDienTuId}")]
        public async Task<IActionResult> KiemTraHoaDonDaLapTBaoCoSaiSot(string hoaDonDienTuId)
        {
            var result = await _hoaDonDienTuService.KiemTraHoaDonDaLapTBaoCoSaiSotAsync(hoaDonDienTuId);
            return Ok(result);
        }

        [HttpPost("CheckHoaDonPhatHanh")]
        public async Task<IActionResult> CheckHoaDonPhatHanh(ParamPhatHanhHD @param)
        {
            var result = await _hoaDonDienTuService.CheckHoaDonPhatHanhAsync(@param);
            return Ok(result);
        }

        [HttpPost("UpdateNgayHoaDonBangNgayHoaDonPhatHanh")]
        public async Task<IActionResult> UpdateNgayHoaDonBangNgayHoaDonPhatHanh(HoaDonDienTuViewModel model)
        {
            var result = await _hoaDonDienTuService.UpdateNgayHoaDonBangNgayHoaDonPhatHanhAsync(model);
            return Ok(new { Status = result.Item1, Data = result.Item2 });
        }

        [HttpPost("GetListHoaDonSaiSotCanThayThe")]
        public async Task<IActionResult> GetListHoaDonSaiSotCanThayThe(HoaDonThayTheParams pagingParams)
        {
            var result = await _hoaDonDienTuService.GetListHoaDonSaiSotCanThayTheAsync(pagingParams);
            return Ok(result);
        }

        [HttpGet("ThongKeSoLuongHoaDonSaiSotChuaLapThongBao/{coThongKeSoLuong}/{loaiNghiepVu}")]
        public async Task<IActionResult> ThongKeSoLuongHoaDonSaiSotChuaLapThongBao(byte coThongKeSoLuong, int? loaiNghiepVu)
        {
            var result = await _hoaDonDienTuService.ThongKeSoLuongHoaDonSaiSotChuaLapThongBaoAsync(coThongKeSoLuong, loaiNghiepVu);
            return Ok(result);
        }

        [HttpGet("KiemTraSoLanGuiEmailSaiSot/{hoaDonDienTuId}/{loaiSaiSot}")]
        public async Task<IActionResult> KiemTraSoLanGuiEmailSaiSot(string hoaDonDienTuId, byte loaiSaiSot)
        {
            var result = await _hoaDonDienTuService.KiemTraSoLanGuiEmailSaiSotAsync(hoaDonDienTuId, loaiSaiSot);
            return Ok(result);
        }

        [HttpGet("KiemTraHoaDonThayTheDaDuocCapMa/{hoaDonDienTuId}")]
        public async Task<IActionResult> KiemTraHoaDonThayTheDaDuocCapMa(string hoaDonDienTuId)
        {
            var result = await _hoaDonDienTuService.KiemTraHoaDonThayTheDaDuocCapMaAsync(hoaDonDienTuId);
            return Ok(new { result });
        }

        [HttpGet("CheckDaPhatSinhThongDiepTruyenNhanVoiCQT/{hoaDonDienTuId}")]
        public async Task<IActionResult> CheckDaPhatSinhThongDiepTruyenNhanVoiCQT(string hoaDonDienTuId)
        {
            var result = await _hoaDonDienTuService.CheckDaPhatSinhThongDiepTruyenNhanVoiCQTAsync(hoaDonDienTuId);
            return Ok(result);
        }

        [HttpGet("CheckLaHoaDonGuiTCTNLoi/{hoaDonDienTuId}")]
        public async Task<IActionResult> CheckLaHoaDonGuiTCTNLoi(string hoaDonDienTuId)
        {
            var result = await _hoaDonDienTuService.CheckLaHoaDonGuiTCTNLoiAsync(hoaDonDienTuId);
            return Ok(result);
        }

        [HttpGet("GetTrangThaiQuyTrinhById/{hoaDonDienTuId}")]
        public async Task<IActionResult> GetTrangThaiQuyTrinhById(string hoaDonDienTuId)
        {
            var result = await _hoaDonDienTuService.GetTrangThaiQuyTrinhByIdAsync(hoaDonDienTuId);
            return Ok(result);
        }

        [HttpPost("SortListSelected")]
        public IActionResult SortListSelected(HoaDonParams param)
        {
            var result = _hoaDonDienTuService.SortListSelected(param);
            return Ok(result);
        }

        [HttpGet("GetMaThongDiepInXMLSignedById/{id}")]
        public async Task<IActionResult> GetMaThongDiepInXMLSignedById(string id)
        {
            var result = await _hoaDonDienTuService.GetMaThongDiepInXMLSignedByIdAsync(id);
            return Ok(new { result });
        }

        [HttpGet("GetTaiLieuDinhKemsById/{id}")]
        public async Task<IActionResult> GetTaiLieuDinhKemsById(string id)
        {
            var result = await _hoaDonDienTuService.GetTaiLieuDinhKemsByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Lấy hóa đơn chi tiết qua thaythechohoadonId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetHoaDonByThayTheChoHoaDonId/{Id}")]
        public async Task<IActionResult> GetHoaDonByThayTheChoHoaDonId(string id)
        {
            var result = await _hoaDonDienTuService.GetHoaDonByThayTheChoHoaDonIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Check là hóa đơn đã gửi email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("IsDaGuiEmailChoKhachHang/{id}")]
        public async Task<IActionResult> IsDaGuiEmailChoKhachHang(string id)
        {
            var result = await _hoaDonDienTuService.IsDaGuiEmailChoKhachHangAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Đọc dữ liệu hóa đơn để import vào chức năng đề nghị ghi nhận doanh thu của phần mềm kế toán bách khoa.
        /// </summary>
        /// <param name="thamSoLayDuLieu">Điều kiện đọc dữ liệu.</param>
        /// <returns>Danh sách các hóa đơn cần để import.</returns>
        [HttpPost("GetHoaDonChoKeToanBachKhoa")]
        public async Task<IActionResult> GetHoaDonChoKeToanBachKhoa(ThamSoLayDuLieuHoaDon thamSoLayDuLieu)
        {
            var result = await _hoaDonDienTuService.GetHoaDonChoKeToanBachKhoaAsync(thamSoLayDuLieu);
            return Ok(result);
        }

        [HttpPost("UpdateTruongMaKhiSuaTrongDanhMuc")]
        public async Task<IActionResult> UpdateTruongMaKhiSuaTrongDanhMuc(UpdateMa param)
        {
            var result = await _hoaDonDienTuService.UpdateTruongMaKhiSuaTrongDanhMucAsync(param);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("CreateXMLToSign")]
        public async Task<IActionResult> CreateXMLToSign(HoaDonDienTuViewModel hd)
        {
            try
            {
                var result = await _hoaDonDienTuService.CreateXMLToSignAsync(hd);
                return Ok(result);
            }
            catch (Exception e)
            {
                Tracert.WriteLog("CreateXMLToSign", e);
                return Ok(null);
            }
        }

        [AllowAnonymous]
        [HttpPost("MultiCreateXMLToSign")]
        public async Task<IActionResult> MultiCreateXMLToSign(List<HoaDonDienTuViewModel> list)
        {
            try
            {
                var result = await _hoaDonDienTuService.MultiCreateXMLToSignAsync(list);
                return Ok(result);
            }
            catch (Exception e)
            {
                Tracert.WriteLog("MultiCreateXMLToSignAsync", e);
                return Ok(null);
            }
        }

        [HttpPost("GetListHoaDonDePhatHanhDongLoat")]
        public async Task<IActionResult> GetListHoaDonDePhatHanhDongLoat(HoaDonParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetListHoaDonDePhatHanhDongLoatAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("GroupListDeXemDuLieuPhatHanhDongLoat")]
        public async Task<IActionResult> GroupListDeXemDuLieuPhatHanhDongLoat(HoaDonParams @params)
        {
            var result = await _hoaDonDienTuService.GroupListDeXemDuLieuPhatHanhDongLoatAsync(@params.HoaDonDienTus);
            return Ok(result);
        }

        [HttpPost("PhatHanhHoaDonDongLoat")]
        public async Task<IActionResult> PhatHanhHoaDonDongLoat(List<ParamPhatHanhHD> @params)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.PhatHanhHoaDonDongLoatAsync(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    Tracert.WriteLog("PhatHanhHoaDonDongLoat: ", e);
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPost("PhatHanhHoaDonCoMaTuMTTDongLoat")]
        public async Task<IActionResult> PhatHanhHoaDonCoMaTuMTTDongLoat(List<string> hoaDonIds)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _hoaDonDienTuService.PhatHanhHoaDonCoMaTuMTTDongLoat(hoaDonIds);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    Tracert.WriteLog("PhatHanhHoaDonDongLoat: ", e);
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPost("TaiTepPhatHanhHoaDonLoi")]
        public async Task<IActionResult> TaiTepPhatHanhHoaDonLoi(List<HoaDonDienTuViewModel> list)
        {
            var result = await _hoaDonDienTuService.TaiTepPhatHanhHoaDonLoiAsync(list);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("GetListHoaDonDeGuiEmailDongLoat")]
        public async Task<IActionResult> GetListHoaDonDeGuiEmailDongLoat(HoaDonParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetListHoaDonDeGuiEmailDongLoatAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("TaiTepGuiHoaDonLoi")]
        public async Task<IActionResult> TaiTepGuiHoaDonLoi(List<HoaDonDienTuViewModel> list)
        {
            var result = await _hoaDonDienTuService.TaiTepGuiHoaDonLoiAsync(list);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("CheckMultiHoaDonPhatHanh")]
        public async Task<IActionResult> CheckMultiHoaDonPhatHanh(List<ParamPhatHanhHD> @params)
        {
            var result = await _hoaDonDienTuService.CheckMultiHoaDonPhatHanhAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetMultiTrangThaiQuyTrinhById")]
        public async Task<IActionResult> GetMultiTrangThaiQuyTrinhById(List<string> ids)
        {
            var result = await _hoaDonDienTuService.GetMultiTrangThaiQuyTrinhByIdAsync(ids);
            return Ok(result);
        }

        [HttpPost("GetMultiById")]
        public async Task<IActionResult> GetMultiById(List<string> ids)
        {
            var result = await _hoaDonDienTuService.GetMultiByIdAsync(ids);
            return Ok(result);
        }

        [HttpPost("WaitMultiForTCTResonse")]
        public async Task<IActionResult> WaitMultiForTCTResonse(List<string> ids)
        {
            var result = await _hoaDonDienTuService.WaitMultiForTCTResonseAsync(ids);
            return Ok(result);
        }

        [HttpPost("GetKetQuaThucHienPhatHanhDongLoat")]
        public async Task<IActionResult> GetKetQuaThucHienPhatHanhDongLoat(List<string> ids)
        {
            var result = await _hoaDonDienTuService.GetKetQuaThucHienPhatHanhDongLoatAsync(ids);
            return Ok(result);
        }

        [HttpPost("UpdateRangeNgayHoaDonVeNgayHienTai")]
        public async Task<IActionResult> UpdateRangeNgayHoaDonVeNgayHienTai(List<string> ids)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuService.UpdateRangeNgayHoaDonVeNgayHienTaiAsync(ids);
                    transaction.Commit();
                    return Ok(true);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(false);
                }
            }
        }
        /// <summary>
        /// Kiểm tra xem danh sách hóa đơn truyền vào đã tồn tại chưa
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("CheckExistInvoid")]
        public async Task<IActionResult> CheckExistInvoid(List<ListCheckHoaDonSaiSotViewModel> list)
        {
            var result = await _hoaDonDienTuService.CheckExistInvoidAsync(list);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("InsertTuyChonHoaDonTroVeTruoc")]
        public async Task<IActionResult> InsertTuyChonHoaDonTroVeTruoc([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                string dbString = param.KeyString.Base64Decode();

                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, param.DatabaseName);

                try
                {
                    await _hoaDonDienTuService.InsertTuyChonHoaDonTroVeTruocAsync();
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }

            return Ok(false);
        }

        /// <summary>
        /// Xác nhận trạng thái hóa đơn đã gửi cho khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("XacNhanHoaDonDaGuiChoKhachHang/{id}")]
        public async Task<IActionResult> XacNhanHoaDonDaGuiChoKhachHang(string id)
        {
            var result = await _hoaDonDienTuService.XacNhanHoaDonDaGuiChoKhachHang(id);
            return Ok(result);
        }
        /// <summary>
        /// lấy số lượng hóa đơn MTT yêu cầu hủy IsPos == 1200
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHoaDonMttYeuCauHuy")]
        public async Task<IActionResult> GetHoaDonMttYeuCauHuy()
        {
            var result = await _hoaDonDienTuService.GetHoaDonMttYeuCauHuy();
            return Ok(result);
        }
        /// <summary>
        /// process GuiThongDiep206Background
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GuiThongDiep206Background")]
        public async Task<IActionResult> GuiThongDiep206Background([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                try
                {
                    string dbString = (param.KeyString).Base64Decode();

                    // Switch database
                    User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);
                    User.AddClaim(ClaimTypeConstants.DATABASE_NAME, param.DatabaseName);

                    var result = await _hoaDonDienTuService.GuiThongDiep206BackgroundAsync(param.DateTimeStart);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return Ok(e.ToString());
                }
            }

            return Ok(false);
        }
    }
}
