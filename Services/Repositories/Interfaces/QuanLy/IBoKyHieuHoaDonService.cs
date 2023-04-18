using DLL.Entity.QuanLy;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuanLy;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLy
{
    public interface IBoKyHieuHoaDonService
    {
        Task<List<BoKyHieuHoaDonViewModel>> GetAllAsync();
        Task<PagedList<BoKyHieuHoaDonViewModel>> GetAllPagingAsync(BoKyHieuHoaDonParams @params);
        Task<BoKyHieuHoaDonViewModel> GetByIdAsync(string id);
        Task<bool> CheckTrungKyHieuAsync(BoKyHieuHoaDonViewModel model);
        Task<List<BoKyHieuHoaDonViewModel>> GetListByMauHoaDonIdAsync(string mauHoaDonId);
        Task<List<NhatKyXacThucBoKyHieuViewModel>> GetListNhatKyXacThucByIdAsync(string id);
        CtsModel CheckSoSeriChungThu(BoKyHieuHoaDonViewModel model);
        Task<bool> CheckThoiHanChungThuSoAsync(BoKyHieuHoaDonViewModel model);
        Task<List<BoKyHieuHoaDonViewModel>> GetListForHoaDonAsync(BoKyHieuHoaDonViewModel model);
        Task<List<BoKyHieuHoaDonViewModel>> GetListForPhatHanhDongLoatAsync(PagingParams param);
        Task<bool> CheckDaHetSoLuongHoaDonAsync(string boKyHieuHoaDonId, long? soHoaDon);
        Task<string> GetSoSeriChungThuByIdAsync(string id);
        BoKyHieuHoaDonViewModel CheckKyHieuOutObject(string kyHieu, List<BoKyHieuHoaDon> models);
        Task<List<string>> GetChungThuSoByIdAsync(string id);
        Task<List<string>> GetMultiChungThuSoByIdAsync(List<string> listId);
        Task<bool> CheckDaKySoBatDauAsync(string id);
        Task<string> GetNhatKyXacThucBoKyHieuIdForXemMauHoaDonAsync(string nhatKyXacThucBoKyHieuId);
        Task<string> CheckHasToKhaiMoiNhatAsync(BoKyHieuHoaDonViewModel model);
        Task CheckDaHetSoLuongHoaDonVaXacThucAsync(HoaDonDienTuViewModel hoaDon);

        Task<BoKyHieuHoaDonViewModel> InsertAsync(BoKyHieuHoaDonViewModel model);
        Task<bool> UpdateAsync(BoKyHieuHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> XacThucBoKyHieuHoaDonAsync(NhatKyXacThucBoKyHieuViewModel model);
        Task<ToKhaiForBoKyHieuHoaDonViewModel> CheckToKhaiPhuHopAsync(BoKyHieuHoaDonViewModel model);
        Task<BoKyHieuHoaDonViewModel> GetThongTinTuToKhaiMoiNhatAsync();
        Task<bool> HasChuyenTheoBangTongHopDuLieuHDDTAsync(string id);
        Task<NhatKyXacThucBoKyHieuViewModel> CheckTrangThaiSuDungTruocKhiSuaAsync(string id);
        Task<string> CheckBoKyHieuDangPhatHanhAsync(List<string> boKyHieuHoaDonIds);
        Task<bool> ClearBoKyHieuDaPhatHanhAsync();
        Task<List<BoKyHieuForMayTinhTienViewModels>> GetListBoKyHieuForHoaDonCMMTTienAsync(BoKyHieuHoaDonViewModel model);

        Task<List<BoKyHieuHoaDonViewModel>> PosGetAllBoKyHieu();
        Task<List<BoKyHieuHoaDonViewModel>> PosGetBoKyHieuMTT();

        bool CheckUsedBoKyHieuHoaDon(string Id);
    }
}
