using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IBangTongHopService
    {
        string CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params);
        Task<List<BangTongHopDuLieuHoaDonChiTietViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params);
        Task<dynamic> CheckLanDau(BangTongHopParams3 @params);
        Task<dynamic> CheckSuaDoi(BangTongHopParams3 @params);
        Task<dynamic> CheckBoSung(BangTongHopParams3 @params);
        Task<int> GetSoBangTongHopDuLieu(BangTongHopParams2 @params);
        Task<bool> GuiBangDuLieu(string thongDiepChungId, string maThongDiep, string mst);
        Task<bool> LuuDuLieuKy(string encodedContent, string thongDiepId);
        Task<int> GetLanBoSung(BangTongHopParams3 @params);
        Task<int> GetLanSuaDoi(BangTongHopParams3 @params);
        Task<bool> CheckSuaDoiChuaGui(BangTongHopParams3 @params);
        Task<bool> InsertBangTongHopDuLieuHoaDonAsync(BangTongHopDuLieuHoaDonViewModel model);
        Task<bool> UpdateBangTongHopDuLieuHoaDonAsync(BangTongHopDuLieuHoaDonViewModel model);
        Task<bool> DeleteBangTongHopDuLieuHoaDonAsync(string BangTongHopId);
        List<EnumModel> GetListTimKiemTheoBangTongHop();
        Task<BangTongHopDuLieuHoaDonViewModel> GetById(string Id);
        Task<PagedList<BangTongHopDuLieuHoaDonViewModel>> GetAllPagingBangTongHopAsync(BangTongHopDuLieuHoaDonParams @params);    }
}
