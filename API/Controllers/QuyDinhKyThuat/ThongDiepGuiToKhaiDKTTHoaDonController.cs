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
        private readonly IToKhaiService _IToKhaiService;
        public ThongDiepGuiToKhaiDKTTHoaDonController(
            IXMLInvoiceService IXMLInvoiceService,
            IQuyDinhKyThuatService IQuyDinhKyThuatService,
            IToKhaiService IToKhaiService,
            IDatabaseService IDatabaseService,
            Datacontext db
        )
        {
            _IXMLInvoiceService = IXMLInvoiceService;
            _IDatabaseService = IDatabaseService;
            _IQuyDinhKyThuatService = IQuyDinhKyThuatService;
            _IToKhaiService = IToKhaiService;
            _db = db;
        }

        /// <summary>
        /// Tạo xml cho tờ khai
        /// </summary>
        /// <param name="tKhai"></param>
        /// <returns>string: tên file tờ khai</returns>
        [HttpPost("GetXMLToKhai")]
        public IActionResult GetXMLToKhai(ToKhaiParams @params)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            string result;
            if (@params.ToKhaiKhongUyNhiem != null)
            {
                if (!string.IsNullOrEmpty(@params.ToKhaiId))
                {
                    result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, @params.ToKhaiId);
                }
                else result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            }
            else
            {
                if (!string.IsNullOrEmpty(@params.ToKhaiId))
                {
                    result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, @params.ToKhaiId);
                }
                else result = _IXMLInvoiceService.CreateFileXML(@params.ToKhaiUyNhiem, ManageFolderPath.XML_UNSIGN, fileName);
            }
            return Ok(new { result });
        }

        /// <summary>
        /// Tạo xml cho thông điệp 100, 101
        /// </summary>
        /// <param name="tDiep"></param>
        /// <returns>string: tên file xml thông điệp 100</returns>
        [HttpPost("GetXMLThongDiepToKhai")]
        public IActionResult GetXMLThongDiepToKhai(ThongDiepParams tDiep)
        {
            string fileName = $"TK-{Guid.NewGuid()}.xml";
            string result;
            if (tDiep.ThongDiepKhongUyNhiem != null)
            {
                result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepKhongUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, tDiep.ThongDiepId);
            }
            else
            {
                result = _IXMLInvoiceService.CreateFileXML(tDiep.ThongDiepUyNhiem, ManageFolderPath.XML_UNSIGN, fileName, tDiep.ThongDiepId);
            }

            return Ok(new { result });
        }

        /// <summary>
        /// Lấy nội dung thông điệp chưa ký từ file xml và convert ra dạng viewmodel
        /// </summary>
        /// <param name="thongDiepId"></param>
        /// <returns>viewModel: thông điệp dưới dạng viewmodel</returns>
        [HttpGet("GetNoiDungThongDiepXMLChuaKy/{thongDiepId}")]
        public async Task<IActionResult> GetNoiDungThongDiepXMLChuaKy(string thongDiepId)
        {
            var result = await _IQuyDinhKyThuatService.GetNoiDungThongDiepXMLChuaKy(thongDiepId);
            result = TextHelper.Base64Encode(result);
            return Ok(new { result });
        }

        /// <summary>
        /// Lưu tờ khai đăng ký / thay đổi thông tin
        /// </summary>
        /// <param name="model"></param>
        /// <returns>boolean: true nếu lưu thành công, false nếu lưu lỗi</returns>
        [HttpPost("LuuToKhaiDangKyThongTin")]
        public async Task<IActionResult> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.LuuToKhaiDangKyThongTin(model);
                if (result != null) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Sửa tờ khai đăng ký/thay đổi thông tin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("SuaToKhaiDangKyThongTin")]
        public async Task<IActionResult> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.SuaToKhaiDangKyThongTin(model);
                if (result == true) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Xóa tờ khai
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>boolean: true: xóa thành công, false: lỗi xóa</returns>
        [HttpDelete("XoaToKhai/{Id}")]
        public async Task<IActionResult> XoaToKhai(string Id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.XoaToKhai(Id);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Lưu dữ liệu ký tờ khai
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LuuDuLieuKy")]
        public async Task<IActionResult> LuuDuLieuKy(DuLieuKyToKhaiViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.LuuDuLieuKy(model);
                if (result == true) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GetLinkFileXml")]
        public async Task<IActionResult> GetLinkFileXml(ExportParams @params)
        {
            var result = await _IQuyDinhKyThuatService.GetLinkFileXml(@params.ThongDiep, @params.Signed);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("AddRangeChungThuSo")]
        public async Task<IActionResult> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.AddRangeChungThuSo(models);
                if (result == true) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost("DeleteRangeChungThuSo")]
        public async Task<IActionResult> DeleteRangeChungThuSo(List<string> ids)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.DeleteRangeChungThuSo(ids);
                if (result == true) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Lấy thông điệp chung theo điều kiện lọc
        /// Trả về dạng page list
        /// </summary>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Thêm thông điệp gửi vào hệ thống
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("InsertThongDiepChung")]
        public async Task<IActionResult> InsertThongDiepChung(ThongDiepChungViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IQuyDinhKyThuatService.InsertThongDiepChung(model);
                if (result != null) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Cập nhật thông điệp gửi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateThongDiepChung")]
        public async Task<IActionResult> UpdateThongDiepChung(ThongDiepChungViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IQuyDinhKyThuatService.UpdateThongDiepChung(model);
                if (result == true) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Xóa thông điệp gửi với id nhất định
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteThongDiepChung/{Id}")]
        public async Task<IActionResult> DeleteThongDiepChung(string Id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IQuyDinhKyThuatService.DeleteThongDiepChung(Id);
                return Ok(result);
            }
        }

        /// <summary>
        /// Thêm list đăng ký ủy nhiệm (đồng thời với việc tạo/cập nhật tờ khai ủy nhiệm)
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("AddRangeDangKyUyNhiem")]
        public async Task<IActionResult> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                var result = await _IToKhaiService.AddRangeDangKyUyNhiem(models);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        /// <summary>
        /// Lấy list đăng ký ủy nhiệm của một tờ khai
        /// </summary>
        /// <param name="IdToKhai"></param>
        /// <returns></returns>
        [HttpGet("GetListDangKyUyNhiem/{IdToKhai}")]
        public async Task<IActionResult> GetListDangKyUyNhiem(string IdToKhai)
        {
            var result = await _IToKhaiService.GetListDangKyUyNhiem(IdToKhai);
            return Ok(result);
        }

        /// <summary>
        /// Lấy những đăng ký ủy nhiệm bị trùng ký hiệu hóa đơn
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("GetListTrungKyHieuTrongHeThong")]
        public IActionResult GetListTrungKyHieuTrongHeThong(List<DangKyUyNhiemViewModel> data)
        {
            var result = _IQuyDinhKyThuatService.GetListTrungKyHieuTrongHeThong(data);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin thông điệp biết id thông điệp
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetThongDiepChungById/{Id}")]
        public async Task<IActionResult> GetThongDiepChungById(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepChungById(Id);
            return Ok(result);
        }

        /// <summary>
        /// Lấy các tiêu chí tìm kiếm ở bảng thông điệp gửi và nhận
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListTimKiemTheoThongDiep")]
        public IActionResult GetListTimKiemTheoThongDiep()
        {
            var result = _IQuyDinhKyThuatService.GetListTimKiemTheoThongDiep();
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các thông điệp 100 với hình thức đăng ký trong hệ thống
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetThongDiepThemMoiToKhai")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhai()
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhai();
            return Ok(result);
        }

        /// <summary>
        /// Lấy các thông điệp 100 hình thức đăng ký được chấp nhận
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetThongDiepThemMoiToKhaiDuocChapNhan")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhaiDuocChapNhan()
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhaiDuocChapNhan();
            return Ok(result);
        }

        /// <summary>
        /// Lấy các thông điệp 100 hình thức đăng ký được chấp nhận (dùng khi tra cứu với đầu vào là mã)
        /// </summary>
        /// <param name="MaTraCuu"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetThongDiepThemMoiToKhaiDuocChapNhan_TraCuu1/{MaTraCuu}")]
        public async Task<IActionResult> GetThongDiepThemMoiToKhaiDuocChapNhan(string MaTraCuu)
        {
            CompanyModel companyModel = await _IDatabaseService.GetDetailByLookupCodeAsync(MaTraCuu.Trim());

            User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
            User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
            var result = await _IQuyDinhKyThuatService.GetThongDiepThemMoiToKhaiDuocChapNhan();
            var tk = await _IToKhaiService.GetToKhaiById(result.IdThamChieu);
            return Ok(tk);
        }

        /// <summary>
        /// Lấy các thông điệp 100 hình thức đăng ký được chấp nhận (dùng khi tra cứu với đầu vào là xml)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
                var tk = await _IToKhaiService.GetToKhaiById(result.IdThamChieu);
                return Ok(tk);
            }
            else return Ok(null);
        }

        /// <summary>
        /// Lấy tất cả các thông điệp phản hồi của 1 thông điệp gửi đi
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetAllThongDiepTraVe/{Id}")]
        public async Task<IActionResult> GetAllThongDiepTraVe(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetAllThongDiepTraVe(Id);
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các thông điệp phản hồi của 1 thông điệp gửi đi với các tiêu chí (loại thông điệp, mã thông điệp, etc...)
        /// </summary>
        /// <param name="giaTriTimKiem"></param>
        /// <param name="phanLoai"></param>
        /// <returns></returns>
        [HttpGet("GetAllThongDiepTraVeV2/{giaTriTimKiem}/{phanLoai}")]
        public async Task<IActionResult> GetAllThongDiepTraVeV2(string giaTriTimKiem, string phanLoai)
        {
            var result = await _IQuyDinhKyThuatService.GetAllThongDiepTraVeV2(giaTriTimKiem, phanLoai);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông điệp gửi khi biết mã thông điệp tham chiếu trên thông điệp nhận
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetThongDiepByThamChieu/{Id}")]
        public async Task<IActionResult> GetThongDiepByThamChieu(string Id)
        {
            var result = await _IQuyDinhKyThuatService.GetThongDiepByThamChieu(Id);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin nội dung tờ khai khi biết id tờ khai
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetToKhaiById/{Id}")]
        public async Task<IActionResult> GetToKhaiById(string Id)
        {
            var result = await _IToKhaiService.GetToKhaiById(Id);
            return Ok(result);
        }

        /// <summary>
        /// Gửi tờ khai (thông điệp 100, 101)
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("GuiToKhai")]
        public async Task<IActionResult> GuiToKhai(GuiNhanToKhaiParams @params)
        {
            var result = await _IToKhaiService.GuiToKhai(@params.Id, @params.MaThongDiep, @params.MST);
            return Ok(result);
        }

        /// <summary>
        /// Kiểm tra tờ khai thay đổi thông tin trước khi ký và gửi
        /// </summary>
        /// <param name="toKhaiId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("CheckToKhaiThayDoiThongTinTruocKhiKyVaGui/{toKhaiId}")]
        public async Task<IActionResult> CheckToKhaiThayDoiThongTinTruocKhiKyVaGui(string toKhaiId)
        {
            var result = await _IToKhaiService.CheckToKhaiThayDoiThongTinTruocKhiKyVaGuiAsync(toKhaiId);
            return Ok(new { result });
        }

        /// <summary>
        /// Chuyển mã byte trả về khi ký/gửi thành xml
        /// </summary>
        /// <param name="encodedContent"></param>
        /// <returns></returns>
        [HttpPost("ConvertToThongDiepTiepNhan")]
        public IActionResult ConvertToThongDiepTiepNhan(string encodedContent)
        {
            var result = _IQuyDinhKyThuatService.ConvertToThongDiepTiepNhan(encodedContent);
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các mã loại thông điệp TVAN và CQT trả về
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListLoaiThongDiepNhan")]
        public IActionResult GetListLoaiThongDiepNhan()
        {
            var result = _IQuyDinhKyThuatService.GetListLoaiThongDiepNhan();
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các mã loại thông điệp hệ thống có thể gửi đi
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetListLoaiThongDiepGui")]
        public IActionResult GetListLoaiThongDiepGui()
        {
            var result = _IQuyDinhKyThuatService.GetListLoaiThongDiepGui();
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các trạng thái phản hồi từ TVAN và CQT của 1 loại thông điệp bất kỳ
        /// </summary>
        /// <param name="maLoaiThongDiep"></param>
        /// <returns></returns>
        [HttpGet("GetTrangThaiGuiPhanHoiTuCQT/{maLoaiThongDiep}")]
        public IActionResult GetTrangThaiGuiPhanHoiTuCQT(int maLoaiThongDiep)
        {
            var result = _IQuyDinhKyThuatService.GetTrangThaiGuiPhanHoiTuCQT(maLoaiThongDiep);
            return Ok(result);
        }

        /// <summary>
        /// Lấy các tờ khai được liên kết với 1 bộ ký hiệu hóa đơn
        /// </summary>
        /// <param name="toKhaiParams"></param>
        /// <returns></returns>
        [HttpPost("GetListToKhaiFromBoKyHieuHoaDon")]
        public async Task<IActionResult> GetListToKhaiFromBoKyHieuHoaDon(ToKhaiParams toKhaiParams)
        {
            var result = await _IQuyDinhKyThuatService.GetListToKhaiFromBoKyHieuHoaDonAsync(toKhaiParams);
            return Ok(result);
        }

        /// <summary>
        /// Convert nội dung được mã hóa thành thông điệp 103 (Phản hồi từ CQT đối với thông điệp 100)
        /// </summary>
        /// <param name="encodedContent"></param>
        /// <returns></returns>
        [HttpPost("ConvertToThongDiepKUNCQT")]
        public IActionResult ConvertToThongDiepKUNCQT(string encodedContent)
        {
            var result = _IQuyDinhKyThuatService.ConvertToThongDiepKUNCQT(encodedContent);
            return Ok(result);
        }

        /// <summary>
        /// Convert nội dung được mã hóa thành thông điệp 104 (Phản hồi từ CQT đối với thông điệp 101)
        /// </summary>
        /// <param name="encodedContent"></param>
        /// <returns></returns>
        [HttpPost("ConvertToThongDiepUNCQT")]
        public IActionResult ConvertToThongDiepUNCQT(string encodedContent)
        {
            var result = _IQuyDinhKyThuatService.ConvertToThongDiepUNCQT(encodedContent);
            return Ok(result);
        }

        /// <summary>
        /// Lấy nội dung thông điệp từ file xml hoặc nội dung lưu trong bảng filedatas của 1 thông điệp bất kỳ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ShowThongDiepFromFileById/{id}")]
        public async Task<IActionResult> ShowThongDiepFromFileById(string id)
        {
            var result = await _IQuyDinhKyThuatService.ShowThongDiepFromFileByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Xuất khẩu bảng kê (list thông điệp nhận) dưới dạng file xml
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        [HttpPost("ExportBangKe")]
        public async Task<IActionResult> ExportBangKe(ThongDiepChungParams @params)
        {
            var result = await _IQuyDinhKyThuatService.ExportBangKeAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        /// <summary>
        /// Lấy tất cả thông tin cts lưu trữ trên hệ thống (từ tờ khai & ttnnt)
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllListCTS")]
        public async Task<IActionResult> GetAllListCTS()
        {
            var result = await _IQuyDinhKyThuatService.GetAllListCTS();
            return Ok(result);
        }

        /// <summary>
        /// Lấy nội dung file xml tương ứng với 1 thông điệp bất kỳ
        /// </summary>
        /// <param name="maThongDiep"></param>
        /// <returns></returns>
        [HttpGet("GetXmlContentThongDiep/{maThongDiep}")]
        public async Task<IActionResult> GetXmlContentThongDiep(string maThongDiep)
        {
            var result = await _IQuyDinhKyThuatService.GetXmlContentThongDiepAsync(maThongDiep);
            return Ok(new { result });
        }

        /// <summary>
        /// Thống kê số lượng thông điệp theo trạng thái gửi
        /// </summary>
        /// <param name="trangThaiGuiThongDiep"></param>
        /// <param name="coThongKeSoLuong"></param>
        /// <returns></returns>
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
