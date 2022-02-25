using Services.Helper.Params.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IBangTongHopService
    {
        string CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params);
        Task<List<TongHopDuLieuHoaDonGuiCQTViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params);
        Task<int> CheckLanDau(BangTongHopParams3 @params);
        Task<int> GetSoBangTongHopDuLieu(BangTongHopParams2 @params);
        Task<bool> GuiBangDuLieu(string XMLUrl, string thongDiepChungId, string maThongDiep, string mst);
        string LuuDuLieuKy(string encodedContent, string thongDiepId);
    }
}
