using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TBao103 = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.TBao;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IToKhaiService
    {
        #region Check
        Task<bool> IsHasToKhaiDuocChapNhan();
        Task<int> CheckThongDiep103(IFormFile file);
        #endregion

        #region CRUD
        Task<ToKhaiDangKyThongTinViewModel> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> XoaToKhai(string Id);
        #endregion

        #region Lấy dữ liệu
        Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id);
        #endregion

        #region Ký và gửi
        Task<string> CheckToKhaiThayDoiThongTinTruocKhiKyVaGuiAsync(string toKhaiId);
        Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai);
        Task<bool> GuiToKhai(string idThongDiep, string maThongDiep, string mst);
        Task<TBao103> GetThongDiep103FromFile(IFormFile file);
        Task<bool> XacNhanToKhai01(string idThongDiep, string mst, IFormFile file);
        bool VerifyFile103(IFormFile file);
        #endregion

        #region Đăng ký ủy nhiệm
        Task<bool> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> listDangKyUyNhiems);
        Task<List<DangKyUyNhiemViewModel>> GetListDangKyUyNhiem(string idToKhai);
        #endregion

        #region Chứng thư số
        Task<bool> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models);
        Task<bool> DeleteRangeChungThuSo(List<string> Ids);
        #endregion

        Task<string> UpdateFile103AndMCQTToHoSo(IList<IFormFile> files, string MaCQT);
        Task<string> GetFile103Imported(string fileId);
    }
}
