using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.HoaDonSaiSot;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IThongDiepGuiNhanCQTService
    {
        Task<List<HoaDonSaiSotViewModel>> GetListHoaDonSaiSotAsync(HoaDonSaiSotParams @params);
        Task<KetQuaLuuThongDiep> InsertThongBaoGuiHoaDonSaiSotAsync(ThongDiepGuiCQTViewModel model);
        Task<string> GateForWebSocket(FileXMLThongDiepGuiParams @params);
        Task<bool> DeleteAsync(string id);
        Task<List<DiaDanhParam>> GetDanhSachDiaDanhAsync();
        Task<bool> GuiThongDiepToiCQTAsync(DuLieuXMLGuiCQTParams @params);
        Task<List<string>> GetDSMauKyHieuHoaDon(MauKyHieuHoaDonParams @params);
        Task<List<ThongBaoHoaDonRaSoatViewModel>> GetListHoaDonRaSoatAsync(HoaDonRaSoatParams @params);
        Task<List<ThongBaoChiTietHoaDonRaSoatViewModel>> GetListChiTietHoaDonRaSoatAsync(string thongBaoHoaDonRaSoatId);
        Task<ThongDiepGuiCQTViewModel> GetThongDiepGuiCQTByIdAsync(DataByIdParams @params);
        Task<bool> XuLyDuLieuNhanVeTuCQT(ThongDiepPhanHoiParams @params);
        Task<string> ThemThongBaoHoaDonRaSoat(ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep tDiep);
        Task<List<string>> GetListChungThuSoAsync(string ThongDiepGuiCQTId);
        Task<List<ThongBaoSaiSotSearch>> KiemTraHoaDonDaLapThongBaoSaiSotAsync(List<ThongBaoSaiSotSearch> @params);
        Task<List<HoaDonHeThongViewModel>> KiemTraHoaDonDaNhapTrungVoiHoaDonHeThongAsync(List<ThongBaoSaiSotSearch> @params);
        Task<string> TaoSoThongBaoSaiSotAsync();
        Task<List<BangKeHoaDonSaiSot_ViewModel>> GetBangKeHoaDonSaiSotAsync(ThongKeHoaDonSaiSotParams thongKeHoaDonSaiSotParams);
        string ExportExcelBangKeSaiSotAsync(ExportExcelBangKeSaiSotParams exportParams);
        Task<string> GetXMLContentAsync(string thongDiepChungId);
        Task<string> GetNgayCoQuanThueCapMa202(string hoaDonDienTuId);

        Task<KetQuaConvertPDF> GetPdfFile301Async(string thongDiepChungId);
        Task<List<ThongDiepChungViewModel>> GetAllThongDiepLienQuan(string thongDiepChungId);
        CreateFileXMLViewModel InsertFileXMLSigned(string XMLSignedEncode, string createdDate);
        Task GenerateFileIfNotExistsAsync(string ThongDiepGuiCQTId);
        Task<string> GetBase64XmlThongDiepChuaKyAsync(string ThongDiepGuiCQTId);
        string GetLinkFileXml(string fileName);
        bool DeleteFileXML(string fileName);
        Task<KetQuaLuuThongDiep> InsertThongBaoGuiHoaDonSaiSot303Async(ThongDiepGuiCQTViewModel model);

    }
}
