using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IToKhaiService
    {
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
        #endregion

        #region Đăng ký ủy nhiệm
        Task<bool> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> listDangKyUyNhiems);
        Task<List<DangKyUyNhiemViewModel>> GetListDangKyUyNhiem(string idToKhai);
        #endregion

        #region Chứng thư số
        Task<bool> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models);
        Task<bool> DeleteRangeChungThuSo(List<string> Ids);
        #endregion
    }
}
