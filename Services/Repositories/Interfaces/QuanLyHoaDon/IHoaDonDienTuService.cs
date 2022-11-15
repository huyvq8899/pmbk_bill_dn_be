﻿using DLL.Enums;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.HeThong;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using Services.ViewModels.Import;
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
        ThamChieuModel DeleteRangeHoaDonDienTuAsync(List<HoaDonDienTuViewModel> list);
        Task<HoaDonDienTuViewModel> GetByIdAsync(string id);
        Task<List<HoaDonDienTuViewModel>> GetMultiByIdAsync(List<string> ids);
        Task<HoaDonDienTuViewModel> GetByIdAsync(long SoHoaDon, string KyHieuHoaDon, string KyHieuMauSoHoaDon);
        Task<List<HoaDonDienTuViewModel>> GetAllAsync();
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingAsync(HoaDonParams pagingParams);
        //Task<string> CreateSoChungTuAsync();
        Task<bool> CheckSoHoaDonAsync(long? SoHoaDon); // 1: nvk, 2: qttu        //Task<string> PreviewMultiplePDFQuyetToanTamUng(PreviewMultipleViewModel previewMultipleVM);
        //Task<string> PreviewMultiplePDFChungTuKeToan(PreviewMultipleViewModel previewMultipleVM);
        //Task<string> ExportExcelBangKe(PagingParams pagingParams);
        Task<List<TrangThai>> GetTrangThaiHoaDon(int? idCha = null);
        Task<List<TrangThai>> GetTreeTrangThai(int LoaiHD, DateTime fromDate, DateTime toDate, int? idCha = null);
        Task<List<TrangThai>> GetTrangThaiGuiHoaDon(int? idCha = null);
        Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model);
        Task<List<string>> GetError(string hoaDonDienTuId, int loaiLoi);
        string ExportErrorFile(List<HoaDonDienTuViewModel> listError, int action);
        Task<string> ExportExcelBangKe(HoaDonParams pagingParams);
        Task<KetQuaCapSoHoaDon> CreateSoHoaDon(HoaDonDienTuViewModel hd);
        Task<string> CreateSoCTXoaBoHoaDon();
        Task<string> CreateSoBienBanXoaBoHoaDon();
        Task<List<ChiTietMauHoaDon>> GetListChiTietByMauHoaDon(string mauHoaDonId);
        Task<KetQuaConvertPDF> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd);
        KetQuaConvertPDF ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd, string dataBaseName);
        Task<FileReturn> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay @params);
        Task<bool> GateForWebSocket(ParamPhatHanhHD @param);
        Task<bool> WaitForTCTResonseAsync(string id);
        Task<bool> WaitMultiForTCTResonseAsync(List<string> ids);
        Task<LuuTruTrangThaiFileHDDTViewModel> GetTrangThaiLuuTru(string HoaDonDienTuId);
        Task<bool> UpdateTrangThaiLuuFileHDDT(LuuTruTrangThaiFileHDDTViewModel model);
        Task<bool> ThemNhatKyThaoTacHoaDonAsync(NhatKyThaoTacHoaDonViewModel model);
        Task<bool> SendEmail(HoaDonDienTuViewModel hddt, string TenNguoiNhan = "", string ToMail = "");
        Task<bool> SendEmailAsync(ParamsSendMail @params);
        Task<bool> SendEmailThongTinHoaDonAsync(ParamsSendMailThongTinHoaDon @params);
        Task<string> ExportExcelBangKeChiTiet(ParamsXuatKhauChiTietHoaDon @params);
        Task<List<NhatKyThaoTacHoaDonViewModel>> XemLichSuHoaDon(string HoaDonDienTuId);
        Task<BienBanXoaBoViewModel> GetBienBanXoaBoHoaDon(string HoaDonDienTuId);
        Task<bool> CapNhatBienBanXoaBoHoaDon(BienBanXoaBoViewModel bb);
        Task<BienBanXoaBoViewModel> SaveBienBanXoaHoaDon(ParamLapBienBanHuyHoaDon @params);
        Task<bool> DeleteBienBanXoaHoaDon(string Id);
        Task<KetQuaConvertPDF> ConvertBienBanXoaHoaDon(BienBanXoaBoViewModel bb);
        Task<BienBanXoaBoViewModel> GetBienBanXoaBoById(string Id);
        Task<List<HoaDonDienTuViewModel>> GetAllListHoaDonLienQuan(string Id, DateTime ngayTao);
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonThayTheAsync(HoaDonThayTheParams @params);
        Task<PagedList<BangKeHoaDonDieuChinh>> GetAllPagingHoaDonDieuChinhAsync(HoaDonDieuChinhParams @params);
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingHoaDonDieuChinhAsync_New(HoaDonDieuChinhParams @params);
        Task<List<HoaDonDienTuViewModel>> GetListHoaDonXoaBoCanThayTheAsync(HoaDonThayTheParams @params);
        Task<List<HoaDonDienTuViewModel>> GetListHoaDonCanDieuChinhAsync(HoaDonDieuChinhParams @params);
        Task<List<HoaDonDienTuViewModel>> GetListHoaDonKhongMaAsync(HoaDonParams @params);
        Task<List<HoaDonDienTuViewModel>> GetListHoaDonCanCapMaAsync(HoaDonParams @params);
        List<TrangThaiHoaDonDieuChinh> GetTrangThaiHoaDonDieuChinhs();
        List<EnumModel> GetLoaiTrangThaiBienBanDieuChinhHoaDons();
        List<EnumModel> GetLoaiTrangThaiPhatHanhs();
        List<EnumModel> GetLoaiTrangThaiGuiHoaDons();
        List<EnumModel> GetListTimKiemTheoHoaDonThayThe();
        List<EnumModel> GetListTimKiemTheoHoaDon();
        List<EnumModel> GetListHinhThucHoaDonCanThayThe();
        Task<LuuTruTrangThaiBBXBViewModel> GetTrangThaiLuuTruBBXB(string BienBanXoaBoId);
        Task<bool> UpdateTrangThaiLuuFileBBXB(LuuTruTrangThaiBBXBViewModel model);
        Task<bool> GateForWebSocket(ParamKyBienBanHuyHoaDon @param);
        Task<bool> XoaBoHoaDon(ParamXoaBoHoaDon @params);
        Task<bool> GetStatusDaThayTheHoaDon(string HoaDonId);
        Task<bool> CheckMaTraCuuAsync(string maTraCuu);
        FileReturn XemHoaDonDongLoat(List<string> fileArray);
        FileReturn XemHoaDonDongLoat2(List<string> fileArray);
        Task<KetQuaConvertPDF> TaiHoaDon(HoaDonDienTuViewModel hoaDonDienTuViewModel);
        Task<List<ViewModels.QuanLy.DanhSachRutGonBoKyHieuHoaDonViewModel>> GetDSRutGonBoKyHieuHoaDonAsync();
        Task<List<HoaDonDienTuViewModel>> GetDSXoaBoChuaLapThayTheAsync(int? loaiNghiepVu);
        Task<List<HoaDonDienTuViewModel>> GetHoaDonDaLapBbChuaXoaBoAsync(int? loaiNghiepVu);
        Task<List<HoaDonDienTuViewModel>> GetDSHdDaXoaBo(HoaDonParams pagingParams);
        Task<List<HoaDonDienTuViewModel>> GetDSHoaDonDeXoaBo(HoaDonParams pagingParams);
        Task UpdateTrangThaiQuyTrinhAsync(string id, TrangThaiQuyTrinh status);
        Task<bool> RemoveDigitalSignatureAsync(string id);
        Task<ReloadPDFResult> ReloadPDFAsync(ReloadPDFParams @params);
        Task<FileReturn> DowloadXMLAsync(string id);
        Task<NhapKhauResult> ImportHoaDonAsync(NhapKhauParams @params);
        Task<NhapKhauResult> ImportPhieuXuatKhoAsync(NhapKhauParams @params);
        Task<bool> InsertImportHoaDonAsync(List<HoaDonDienTuImport> data);
        FileReturn CreateFileImportHoaDonError(NhapKhauResult result);
        Task<bool> SendEmailThongBaoSaiThongTinAsync(ParamsSendMailThongBaoSaiThongTin @params);
        string GetNgayHienTai();
        Task<ReloadXmlResult> ReloadXMLAsync(ReloadXmlParams @params);
        Task<ReloadXmlResult> InsertThongDiepChungAsync(ReloadXmlParams @params);
        Task<KetQuaKiemTraLapTBao04ViewModel> KiemTraHoaDonDaLapTBaoCoSaiSotAsync(string hoaDonDienTuId);
        Task<KetQuaCapSoHoaDon> CheckHoaDonPhatHanhAsync(ParamPhatHanhHD @param);
        Task<List<KetQuaCapSoHoaDon>> CheckMultiHoaDonPhatHanhAsync(List<ParamPhatHanhHD> @params);
        Task<(bool, List<HoaDonDienTuViewModel>)> UpdateNgayHoaDonBangNgayHoaDonPhatHanhAsync(HoaDonDienTuViewModel model);
        Task UpdateRangeNgayHoaDonVeNgayHienTaiAsync(List<string> ids);
        Task<List<HoaDonDienTuViewModel>> GetListHoaDonSaiSotCanThayTheAsync(HoaDonThayTheParams @params);
        Task<ThongKeSoLuongHoaDonCoSaiSotViewModel> ThongKeSoLuongHoaDonSaiSotChuaLapThongBaoAsync(byte coThongKeSoLuong, int? loaiNghiepVu);
        Task<int> KiemTraSoLanGuiEmailSaiSotAsync(string hoaDonDienTuId, byte loaiSaiSot);
        Task<string> KiemTraHoaDonThayTheDaDuocCapMaAsync(string hoaDonDienTuId);
        Task<bool> CheckDaPhatSinhThongDiepTruyenNhanVoiCQTAsync(string id);
        Task<bool> CheckLaHoaDonGuiTCTNLoiAsync(string id);
        Task<int> GetTrangThaiQuyTrinhByIdAsync(string id);
        Task<List<int>> GetMultiTrangThaiQuyTrinhByIdAsync(List<string> ids);
        IEnumerable<HoaDonDienTuViewModel> SortListSelected(HoaDonParams pagingParams);
        Task<string> GetMaThongDiepInXMLSignedByIdAsync(string id);
        Task<List<TaiLieuDinhKemViewModel>> GetTaiLieuDinhKemsByIdAsync(string id);
        Task<HoaDonDienTuViewModel> GetHoaDonByThayTheChoHoaDonIdAsync(string id);
        Task<bool> IsDaGuiEmailChoKhachHangAsync(string id);
        Task<List<HoaDonChoKeToanBachKhoaViewModel>> GetHoaDonChoKeToanBachKhoaAsync(ThamSoLayDuLieuHoaDon thamSoLayDuLieu);
        Task<bool> UpdateTruongMaKhiSuaTrongDanhMucAsync(UpdateMa param);
        Task<FileReturn> CreateXMLToSignAsync(HoaDonDienTuViewModel hd);
        Task<PagedList<HoaDonDienTuViewModel>> GetListHoaDonDePhatHanhDongLoatAsync(HoaDonParams pagingParams);
        Task<PagedList<HoaDonDienTuViewModel>> GetListHoaDonDeGuiEmailDongLoatAsync(HoaDonParams pagingParams);
        Task<List<HoaDonDienTuViewModel>> GroupListDeXemDuLieuPhatHanhDongLoatAsync(List<HoaDonDienTuViewModel> list);
        Task<List<HoaDonDienTuViewModel>> PhatHanhHoaDonDongLoatAsync(List<ParamPhatHanhHD> @params);
        Task<bool> PhatHanhHoaDonAsync(ParamPhatHanhHD param);
        Task<FileReturn> TaiTepPhatHanhHoaDonLoiAsync(List<HoaDonDienTuViewModel> list);
        Task<FileReturn> TaiTepGuiHoaDonLoiAsync(List<HoaDonDienTuViewModel> list);
        Task<List<KetQuaThucHienPhatHanhDongLoat>> GetKetQuaThucHienPhatHanhDongLoatAsync(List<string> ids);
        Task<List<ListCheckHoaDonSaiSotViewModel>> CheckExistInvoidAsync(List<ListCheckHoaDonSaiSotViewModel> list);
        // Vé điện tử
        Task<List<HoaDonDienTuViewModel>> GetListMauVeCanXuatAsync(PagingParams param);
        Task<List<HoaDonDienTuViewModel>> GetListVeTrongNgayAsync(int loaiNghiepVu);
        Task<List<HoaDonDienTuViewModel>> GetListByTuyenDuongAsync(string tuyenDuongId);
        Task<bool> InsertVeAsync(HoaDonDienTuViewModel model);
        Task<bool> XuatMultiVeAsync(List<ParamPhatHanhHD> list);
        Task<FileReturn> PreviewPDFXuatVeAsync(HoaDonDienTuViewModel model);
        Task<bool> SaveAllVeNhapAsync(List<HoaDonDienTuViewModel> list);
        Task<bool> DeleteAllByLoaiAsync(int loaiNghiepVu);
        Task<HoaDonDienTuViewModel> GetVeByIdAsync(string Id);
        Task<List<string>> PreviewMultiTicketByIdsAsync(List<string> Ids);
        Task<bool> UpdateVeAsync(HoaDonDienTuViewModel model);
        Task<List<HoaDonDienTuViewModel>> GetListDeXuatVeDongLoatAsync(List<HoaDonDienTuViewModel> list);
        Task<int> GetSoChuyenByTuyenDuongAsync(string tuyenDuongId);
        Task<bool> InsertVeTrongNgayFromMobileAsync(HoaDonDienTuViewModel model);
        Task<FileReturn> PreviewHTMLTicketAsync(HoaDonDienTuViewModel model);
        FileReturn PrintToPDF(List<string> base64s);
        Task<(string Message, int Type)> WaitForTCTResonseTicketAsync(List<string> ids);
        Task<HoaDonDienTuViewModel> ThongKeXuatVeTrongNgayAsync();

        /// <summary>
        /// Get List tuyến đường có vé trong ngày
        /// </summary>
        /// <param name="SoChuyen"></param>
        /// <returns></returns>
        Task<List<MobileResult>> GetVeToMobileAsync(int SoChuyen);

        /// <summary>
        /// Lấy các vé theo tuyến đường trong ngày
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<List<HoaDonDienTuViewModel>> GetListVeBySoChuyenToMobileAsync(MobileParamsExport @params);

        /// <summary>
        /// Get các bến mà xe đi qua trong tuyến
        /// </summary>
        /// <param name="SoChuyen"></param>
        /// <returns></returns>
        Task<List<string>> GetBenToMobileAsync(int SoChuyen);

        /// <summary>
        /// Xuất báo cáo theo vé đã xuất theo tuyến đường
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<List<BaoCaoMobileResult>> ExportBaoCaoMobileAsync(QuanLyVeParams @params);

        /// <summary>
        /// Xuất ra số lượng vé theo bến
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<List<QuanLyVeResult>> ExportQuanLyVeMobileAsync(QuanLyVeParams @params);

        /// <summary>
        /// Xem đồng loạt vé
        /// </summary>
        /// <param name="listPdfFiles"></param>
        /// <returns></returns>
        FileReturn XemVeDongLoat(List<string> listPdfFiles);

        /// <summary>
        /// Lấy danh sách vé đã xuất trên mobile
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        Task<List<HoaDonDienTuViewModel>> GetListVeMobileAsync(QuanLyVeParams @params);
    }
}
