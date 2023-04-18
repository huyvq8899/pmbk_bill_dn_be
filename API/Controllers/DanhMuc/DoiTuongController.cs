using API.Extentions;
using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.HeThong;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Implimentations.TienIch;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class DoiTuongController : BaseController
    {
        private readonly IDoiTuongService _doiTuongService;
        private readonly Datacontext _db;
        private readonly IDatabaseService _databaseService;
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;
        private readonly IMapper _mp;

        public DoiTuongController(IDoiTuongService doiTuongService, Datacontext db, IDatabaseService databaseService, IHoaDonDienTuService hoaDonDienTuService, IMapper mapper, INhatKyTruyCapService nhatKyTruyCapService)
        {
            _doiTuongService = doiTuongService;
            _databaseService = databaseService;
            _db = db;
            _hoaDonDienTuService = hoaDonDienTuService;
            _mp = mapper;
            _nhatKyTruyCapService = nhatKyTruyCapService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(DoiTuongParams @params)
        {
            var result = await _doiTuongService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(DoiTuongParams pagingParams)
        {
            var paged = await _doiTuongService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _doiTuongService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllKhachHang")]
        public async Task<IActionResult> GetAllKhachHang()
        {
            var result = await _doiTuongService.GetAllKhachHang();
            return Ok(result);
        }

        [HttpGet("GetKhachHangByMaSoThue/{MaSoThue}")]
        public async Task<IActionResult> GetKhachHangByMaSoThue(string MaSoThue)
        {
            var result = await _doiTuongService.GetKhachHangByMaSoThue(MaSoThue);
            return Ok(result);
        }

        [HttpGet("GetAllNhanVien")]
        public async Task<IActionResult> GetAllNhanVien()
        {
            var result = await _doiTuongService.GetAllNhanVien();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetAllNhanVien_TraCuu")]
        public async Task<IActionResult> GetAllNhanVien_TraCuu([FromQuery] string hoaDonDienTuId)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByHoaDonIdAsync(hoaDonDienTuId);

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

            var result = await _doiTuongService.GetAllNhanVien();
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("CheckTrungMaPos")]
        public async Task<IActionResult> CheckTrungMaPosAsync(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckTrungMaPosAsync(model);
            return Ok(result);
        }

        [HttpPost("CheckPhatSinh")]
        public async Task<IActionResult> CheckPhatSinh(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckPhatSinhAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.InsertAsync(model);
            return Ok(result);
        }
        /// <summary>
        /// PosSyncCustomers đồng bộ khách hàng từ POS gửi sang dạng json list object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("PosSyncCustomers")]
        public async Task<IActionResult> PosSyncCustomers(List<DoiTuongViewModel> model)
        {
            try
            {
                int success = 0;
                int stt_item = 0;
                var listOkRsError = new List<OkObjectResult>();
                var listInsertSuccess = new List<DoiTuongViewModel>();
                var listUpdateSuccess = new List<string>();

                foreach (var item in model)
                {
                    ++stt_item;
                    var result = false;
                    //Check data input
                    var itemExists = await _doiTuongService.CheckTrungMaPosAsync(item);
                    if (itemExists != null) item.DoiTuongId = itemExists.DoiTuongId;
                    var resultFilter = await _doiTuongService.FilterInsertKhachHangAsync(item);
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
                        if (!string.IsNullOrEmpty(item.DoiTuongId))
                        {
                            result = await _doiTuongService.UpdateAsync(item);
                            if (result)
                            {
                                success++;
                                //Isnsert nhật ký
                                await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Sua,
                                    RefType = RefType.KhachHang,
                                    ThamChieu = "Mã: " + item.Ma,
                                    RefId = item.DoiTuongId,
                                    DuLieuCu = JsonConvert.SerializeObject(itemExists),
                                    DuLieuMoi = JsonConvert.SerializeObject(item)
                                });
                                //Update lại vào hóa đơn với những hóa đơn nháp và KyDienTuLoi


                                await _hoaDonDienTuService.UpdateTruongMaKhiSuaTrongDanhMucAsync(new UpdateMa
                                {
                                    Loai = LoaiUpdateMa.KhachHang,
                                    Ma = item.Ma,
                                    RefId = item.DoiTuongId
                                });
                                listUpdateSuccess.Add(item.Ma);

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
                            item.DoiTuongId = Guid.NewGuid().ToString();
                            var kq = await _doiTuongService.InsertAsync(item);
                            if (kq != null)
                            {
                                //Isnsert nhật ký
                                var nk = new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Them,
                                    RefType = RefType.KhachHang,
                                    ThamChieu = "Mã: " + kq.Ma,
                                    RefId = kq.DoiTuongId
                                };

                                await _nhatKyTruyCapService.InsertAsync(nk);
                                result = true;
                                success++;
                                listInsertSuccess.Add(kq);
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

                return Ok(new
                {
                    status = listOkRsError.Count == 0,
                    numItem = model.Count,
                    numSuccess = success,
                    itemsError = listOkRsError,
                    itemsInsertSuccess = listInsertSuccess,
                    itemsUpdateSuccess_Ma = listUpdateSuccess
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }
        /// <summary>
        /// trả về khách hàng theo mã truyền vào
        /// </summary>
        /// <param name="Ma"></param>
        /// <returns></returns>
        [HttpGet("PosGetCustomerByAlias/{Ma}")]
        public async Task<IActionResult> PosGetCustomerByAlias(string Ma)
        {
            var result = await _doiTuongService.GetKhachHangByMa(Ma);
            return Ok(result);
        }
        /// <summary>
        /// trả về khách hàng theo tham số truyền vào
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        [HttpPost("PosGetAllCustomer")]
        public async Task<IActionResult> PosGetAllCustomer(DoiTuongParams pagingParams)
        {
            //chỉ lấy khách hàng
            pagingParams.LoaiDoiTuong = 1;//khách hàng
            pagingParams.LoaiKhachHang = 3;// cả tổ chức và cá nhân
            if (pagingParams.PageSize > 500) pagingParams.PageSize = 500;
            var paged = await _doiTuongService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _doiTuongService.DeleteAsync(id);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return Ok(new
                {
                    result = "DbUpdateException",
                    value = false
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(DoiTuongParams @params)
        {
            var result = await _doiTuongService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("ImportKhachHang")]
        public async Task<IActionResult> ImportKhachHang([FromForm] NhapKhauParams @params)
        {
            var result = await _doiTuongService.ImportKhachHang(@params.Files, @params.ModeValue);
            return Ok(result);
        }

        [HttpPost("InsertKhachHangImport")]
        public async Task<IActionResult> InsertKhachHangImport(List<DoiTuongViewModel> model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // convert
                    var listData = await _doiTuongService.ConvertImportKhachHang(model);
                    int success = 0;
                    foreach (var item in listData)
                    {
                        var result = false;
                        if (!string.IsNullOrEmpty(item.DoiTuongId))
                        {
                            result = await _doiTuongService.UpdateAsync(item);
                        }
                        else result = await _doiTuongService.InsertAsync(item) != null;
                        if (result == false) break;
                        success++;
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        status = true,
                        numDanhMuc = listData.Count,
                        numSuccess = success
                    });
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportKhachHangError")]
        public IActionResult CreateFileImportKhachHangError(List<DoiTuongViewModel> list)
        {
            try
            {
                var result = _doiTuongService.CreateFileImportKhachHangError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("ImportNhanVien")]
        public async Task<IActionResult> ImportNhanVien([FromForm] NhapKhauParams @params)
        {
            var result = await _doiTuongService.ImportNhanVien(@params.Files, @params.ModeValue);
            return Ok(result);
        }

        [HttpPost("InsertNhanVienImport")]
        public async Task<IActionResult> InsertNhanVienImport(List<DoiTuongViewModel> model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // convert
                    var listData = await _doiTuongService.ConvertImportNhanVien(model);
                    int success = 0;
                    foreach (var item in listData)
                    {
                        var result = false;
                        if (!string.IsNullOrEmpty(item.DoiTuongId))
                        {
                            result = await _doiTuongService.UpdateAsync(item);
                        }
                        else result = await _doiTuongService.InsertAsync(item) != null;
                        if (result == false) break;
                        success++;
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        status = true,
                        numDanhMuc = listData.Count,
                        numSuccess = success
                    });
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportNhanVienError")]
        public IActionResult CreateFileImportNhanVienError(List<DoiTuongViewModel> list)
        {
            try
            {
                var result = _doiTuongService.CreateFileImportNhanVienError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("GetAllPagingKhachHangXuatKhauAsync")]
        public async Task<IActionResult> GetAllPagingKhachHangXuatKhauAsync(DoiTuongParams pagingParams)
        {
            var paged = await _doiTuongService.GetAllPagingKhachHangXuatKhauAsync(pagingParams);
            return Ok(paged);
        }

        [HttpGet("GetKhachHangByMa{ma}")]
        public async Task<IActionResult> GetKhachHangByMa(string ma)
        {
            var result = await _doiTuongService.GetKhachHangByMa(ma);
            return Ok(result);
        }
    }
}
