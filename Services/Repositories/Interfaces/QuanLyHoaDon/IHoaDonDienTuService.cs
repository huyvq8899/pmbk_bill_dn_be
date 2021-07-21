using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.FormActions;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuanLyHoaDon
{
    public interface IHoaDonDienTuService
    {
        Task<HoaDonDienTuViewModel> InsertAsync(HoaDonDienTuViewModel model);
        Task<bool> UpdateAsync(HoaDonDienTuViewModel model);
        Task<bool> UpdateRangeAsync(List<HoaDonDienTuViewModel> model);
        Task<bool> DeleteAsync(string id);
        Task<ThamChieuModel> DeleteRangeHoaDonDienTuAsync(List<HoaDonDienTuViewModel> list);
        Task<HoaDonDienTuViewModel> GetByIdAsync(string id);
        Task<List<HoaDonDienTuViewModel>> GetAllAsync();
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingAsync(HoaDonParams pagingParams);
        //Task<string> CreateSoChungTuAsync();
        Task<bool> CheckSoHoaDonAsync(string SoHoaDon); // 1: nvk, 2: qttu        //Task<string> PreviewMultiplePDFQuyetToanTamUng(PreviewMultipleViewModel previewMultipleVM);
        //Task<string> PreviewMultiplePDFChungTuKeToan(PreviewMultipleViewModel previewMultipleVM);
        Task<bool> DeleteFilePDF(string fileName);
        //Task<string> ExportExcelBangKe(PagingParams pagingParams);
        Task<List<TrangThai>> GetTrangThaiHoaDon(int? idCha = null);
        Task<List<TrangThai>> GetTreeTrangThai(int LoaiHD, DateTime fromDate, DateTime toDate, int? idCha = null);
        Task<List<TrangThai>> GetTrangThaiGuiHoaDon(int? idCha = null);
        Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model);
        Task<string> ExportExcelBangKe(HoaDonParams pagingParams);
        Task<KetQuaCapSoHoaDon> CreateSoHoaDon(HoaDonDienTuViewModel hd);
        Task<string> CreateSoCTXoaBoHoaDon();
        Task<ResultParams> CapPhatSoHoaDon(HoaDonDienTuViewModel hd, string soHoaDon);
        Task<ResultParams> CapPhatSoHoaDonHangLoat(List<HoaDonDienTuViewModel> hd, List<string> soHoaDon);
        Task<List<ChiTietMauHoaDon>> GetListChiTietByMauHoaDon(string mauHoaDonId);
        Task<string> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd);
        Task<KetQuaChuyenDoi> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay @params);
        Task<bool> GateForWebSocket(ParamPhatHanhHD @param);
        Task<LuuTruTrangThaiFileHDDTViewModel> GetTrangThaiLuuTru(string HoaDonDienTuId);
        Task<bool> UpdateTrangThaiLuuFileHDDT(LuuTruTrangThaiFileHDDTViewModel model);
        Task<bool> ThemNhatKyThaoTacHoaDonAsync(NhatKyThaoTacHoaDonViewModel model);
        Task<bool> SendEmail(HoaDonDienTuViewModel hddt, string TenNguoiNhan = "", string ToMail = "");
        Task<bool> SendEmailAsync(ParamsSendMail @params);
        Task<string> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params);
        Task<List<NhatKyThaoTacHoaDonViewModel>> XemLichSuHoaDon(string HoaDonDienTuId);
        Task<BienBanXoaBoViewModel> GetBienBanXoaBoHoaDon(string HoaDonDienTuId);
        Task<bool> CapNhatBienBanXoaBoHoaDon(BienBanXoaBoViewModel bb);
        Task<bool> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon @params);
        Task<bool> DeleteBienBanXoaHoaDon(string Id);
        Task<string> ConvertBienBanXoaHoaDon(BienBanXoaBoViewModel bb);

        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonThayTheAsync(HoaDonThayTheParams @params);
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonDieuChinhAsync(HoaDonDieuChinhParams @params);
        Task<List<HoaDonDienTuViewModel>> GetListHoaDonXoaBoCanThayTheAsync(HoaDonThayTheParams @params);
        List<TrangThaiHoaDonDieuChinh> GetTrangThaiHoaDonDieuChinhs();
        List<EnumModel> GetLoaiTrangThaiBienBanDieuChinhHoaDons();
        List<EnumModel> GetLoaiTrangThaiPhatHanhs();
        List<EnumModel> GetLoaiTrangThaiGuiHoaDons();
        List<EnumModel> GetListTimKiemTheoHoaDonThayThe();
        List<EnumModel> GetListHinhThucHoaDonCanThayThe();
        Task<LuuTruTrangThaiBBXBViewModel> GetTrangThaiLuuTruBBXB(string BienBanXoaBoId);
        Task<bool> UpdateTrangThaiLuuFileBBXB(LuuTruTrangThaiBBXBViewModel model);
        Task GateForWebSocket(ParamKyBienBanHuyHoaDon @param);
        Task<bool> XoaBoHoaDon(ParamXoaBoHoaDon @params);
    }
}
