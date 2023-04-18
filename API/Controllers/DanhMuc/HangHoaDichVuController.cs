using DLL;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper.Constants;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.HeThong;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Implimentations.DanhMuc;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class HangHoaDichVuController : BaseController
    {
        private readonly IHangHoaDichVuService _hangHoaDichVuService;
        private readonly Datacontext _db;
        private readonly IDonViTinhService _donViTinhService;
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;
        private readonly ITuyChonService _TuyChonService;
            
        public HangHoaDichVuController(IHangHoaDichVuService hangHoaDichVuService, Datacontext db, IDonViTinhService donViTinhService, IHoaDonDienTuService hoaDonDienTuService, INhatKyTruyCapService nhatKyTruyCapService, ITuyChonService TuyChonService)
        {
            _hangHoaDichVuService = hangHoaDichVuService;
            _db = db;
            _donViTinhService = donViTinhService;
            _hoaDonDienTuService = hoaDonDienTuService;
            _nhatKyTruyCapService = nhatKyTruyCapService;
            _TuyChonService = TuyChonService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(HangHoaDichVuParams @params)
        {
            var result = await _hangHoaDichVuService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(HangHoaDichVuParams pagingParams)
        {
            var paged = await _hangHoaDichVuService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _hangHoaDichVuService.GetByIdAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Kiểm tra xem hàng hóa dịch vụ đã được tạo trong hóa đơn chưa. nếu có trong tạo hóa đơn rồi thì không được xóa hhdv.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("CheckPhatSinh/{Id}")]
        public async Task<IActionResult> CheckPhatSinh(string id)
        {
            var result = await _hangHoaDichVuService.CheckPhatSinhAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(HangHoaDichVuViewModel model)
        {
            var result = await _hangHoaDichVuService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HangHoaDichVuViewModel model)
        {
            var result = await _hangHoaDichVuService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HangHoaDichVuViewModel model)
        {
            var result = await _hangHoaDichVuService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _hangHoaDichVuService.DeleteAsync(id);
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

        /// <summary>
        /// PosSyncGoods đồng bộ hàng hóa dịch vụ từ POS gửi sang dạng json list object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("PosSyncGoods")]
        public async Task<IActionResult> PosSyncGoods(List<HangHoaDichVuViewModel> model)
        {
            try
            {
                int stt_item = 0;
                var listOkRsError = new List<OkObjectResult>();
                var listDVT = await _db.DonViTinhs.ToListAsync();
                int success = 0;
                var _tuyChons = await _TuyChonService.GetAllAsync();
                var listInsertSuccess = new List<HangHoaDichVuViewModel>();
                var listUpdateSuccess = new List<string>();
                foreach (var item in model)
                {
                    
                    ++stt_item;
                    var result = false;
                    //Check data input
                    var itemExists = await _hangHoaDichVuService.GetHangHoaDichVuByMa(item.Ma);
                    if (itemExists != null) item.HangHoaDichVuId = itemExists.HangHoaDichVuId;
                    var resultFilter = await _hangHoaDichVuService.FilterPosSyncGoods(item);
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
                        // Đơn vị tính
                        var donViTinh = new DonViTinh();
                        if (!string.IsNullOrEmpty(item.TenDonViTinh))donViTinh = listDVT.Find(x => string.Compare(x.Ten.ToUpper().Trim(), item.TenDonViTinh.ToUpper().Trim()) == 0);
                        if (!string.IsNullOrEmpty(item.TenDonViTinh) && donViTinh != null)
                        {
                            item.DonViTinhId = donViTinh.DonViTinhId;
                            if(itemExists != null) itemExists.TenDonViTinh = donViTinh.Ten;
                        }
                        else if (itemExists != null)
                        {
                            item.DonViTinhId = itemExists.DonViTinhId;
                            item.TenDonViTinh = itemExists.TenDonViTinh;
                        }

                        if (!string.IsNullOrEmpty(item.TenDonViTinh) && item.TenDonViTinh.Length <= 50 && donViTinh == null)
                        {
                            //msg += "<Đơn vị tính> chưa tồn tại trong hệ thống";
                            //Insert 
                            var dvt = await _donViTinhService.InsertAsync(new DonViTinhViewModel
                            {
                                Ten = item.TenDonViTinh,
                                MoTa = "pos gửi sang gp",
                                Status = true
                            });
                            if (dvt != null) { 
                                item.DonViTinhId = dvt.DonViTinhId;
                            }
                        }
                        item.Status = true; //vì code cũ trên giao diện đang dùng phủ định nên phải để item.Status = true
                        //LÀM TRÒN SỐ THEO TÙY CHỌN CHUNG
                        //donGiaBan
                        if (item.DonGiaBan.HasValue)
                        {
                            item.DonGiaBan = item.DonGiaBan.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.DON_GIA_QUY_DOI);
                        }
                        //tyLeChietKhau
                        if (item.TyLeChietKhau.HasValue)
                        {
                            item.TyLeChietKhau = item.TyLeChietKhau.Value.MathRoundNumberByTuyChon(_tuyChons, LoaiDinhDangSo.HESO_TYLE);
                        }
                        
                        if (!string.IsNullOrEmpty(item.HangHoaDichVuId))
                        {
                            if(itemExists == null) itemExists = await _hangHoaDichVuService.GetByIdAsync(item.HangHoaDichVuId);

                            result = await _hangHoaDichVuService.UpdateAsync(item);
                            if (result)
                            {
                                success++;
                                //Isnsert nhật ký
                                await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Sua,
                                    RefType = RefType.HangHoaDichVu,
                                    ThamChieu = "POS gửi Mã: " + item.Ma,
                                    RefId = item.HangHoaDichVuId,
                                    DuLieuCu = JsonConvert.SerializeObject(itemExists),
                                    DuLieuMoi = JsonConvert.SerializeObject(item)
                                });

                                await _hoaDonDienTuService.UpdateTruongMaKhiSuaTrongDanhMucAsync(new UpdateMa
                                {
                                    Loai = LoaiUpdateMa.HangHoaDichVu,
                                    RefId = item.HangHoaDichVuId,
                                    Ma = item.Ma,
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
                            item.CreatedDate= DateTime.Now;

                            var kq = await _hangHoaDichVuService.InsertAsync(item);
                            if (kq != null)
                            {
                                //Isnsert nhật ký
                                var nk = new NhatKyTruyCapViewModel
                                {
                                    LoaiHanhDong = LoaiHanhDong.Them,
                                    RefType = RefType.HangHoaDichVu,
                                    ThamChieu = "POS gửi Mã: " + kq.Ma,
                                    RefId = kq.HangHoaDichVuId
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
        /// Lấy hàng hóa dịch vụ theo mã truyền vào
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        [HttpGet("PosGetGoodsByAlias/{ma}")]
        public async Task<IActionResult> PosGetGoodsByAlias(string ma)
        {
            var result = await _hangHoaDichVuService.GetHangHoaDichVuByMa(ma);
            return Ok(result);
        }
        /// <summary>
        /// Lấy tất cả hàng hóa theo tham số truyền vào
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        [HttpPost("PosGetAllGoods")]
        public async Task<IActionResult> PosGetAllGoods(HangHoaDichVuParams pagingParams)
        {
            //check PageSize giới hạn 150/page
            if (pagingParams.PageSize > 150) pagingParams.PageSize = 150;
            var paged = await _hangHoaDichVuService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("InsertVTHHImport")]
        public async Task<IActionResult> InsertVTHHImport(List<HangHoaDichVuViewModel> model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // convert
                    var listData = await _hangHoaDichVuService.ConvertImport(model);
                    int success = 0;
                    foreach (var item in listData)
                    {
                        //khi import thì chuyển sang ngừng theo dõi
                        item.Status = true; //vì code cũ trên giao diện đang dùng phủ định nên phải để item.Status = true
                        if (!string.IsNullOrEmpty(item.HangHoaDichVuId))
                        {
                            var res = await _hangHoaDichVuService.UpdateAsync(item);
                            if (res) success++;
                        }
                        else
                        {
                            HangHoaDichVuViewModel result = await _hangHoaDichVuService.InsertAsync(item);
                            if (result != null) success++;
                        }
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

        [HttpPost("CreateFileImportVTHHError")]
        public IActionResult CreateFileImportVTHHError(List<HangHoaDichVuViewModel> list)
        {
            var result = _hangHoaDichVuService.CreateFileImportVTHHError(list);
            return Ok(new
            {
                status = true,
                link = result
            });
        }

        [HttpPost("ImportVTHH")]
        public async Task<IActionResult> ImportVT([FromForm] NhapKhauParams @params)
        {
            var result = await _hangHoaDichVuService.ImportVTHH(@params.Files, @params.ModeValue);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(HangHoaDichVuParams @params)
        {
            var result = await _hangHoaDichVuService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        
        [HttpGet("GetHangHoaDichVuByMa/{ma}")]
        public async Task<IActionResult> GetHangHoaDichVuByMa(string ma)
        {
            var result = await _hangHoaDichVuService.GetHangHoaDichVuByMa(ma);
            return Ok(result);
        }

    }
}
