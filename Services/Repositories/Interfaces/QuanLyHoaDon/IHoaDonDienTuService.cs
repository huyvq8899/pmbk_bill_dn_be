﻿using DLL.Enums;
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
        ThamChieuModel DeleteRangeHoaDonDienTuAsync(List<HoaDonDienTuViewModel> list);
        Task<HoaDonDienTuViewModel> GetByIdAsync(string id);
        Task<List<HoaDonDienTuViewModel>> GetAllAsync();
        Task<PagedList<HoaDonDienTuViewModel>> GetAllPagingAsync(HoaDonParams pagingParams);
        //Task<string> CreateSoChungTuAsync();
        Task<bool> CheckSoHoaDonAsync(string SoHoaDon); // 1: nvk, 2: qttu        //Task<string> PreviewMultiplePDFQuyetToanTamUng(PreviewMultipleViewModel previewMultipleVM);
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
        Task<ResultParams> CapPhatSoHoaDon(HoaDonDienTuViewModel hd, string soHoaDon);
        Task<ResultParams> CapPhatSoHoaDonHangLoat(List<HoaDonDienTuViewModel> hd, List<string> soHoaDon);
        Task<List<ChiTietMauHoaDon>> GetListChiTietByMauHoaDon(string mauHoaDonId);
        Task<KetQuaConvertPDF> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd);
        Task<FileReturn> ConvertHoaDonToHoaDonGiay(ParamsChuyenDoiThanhHDGiay @params);
        Task<bool> GateForWebSocket(ParamPhatHanhHD @param);
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
        List<EnumModel> GetListHinhThucHoaDonCanThayThe();
        Task<LuuTruTrangThaiBBXBViewModel> GetTrangThaiLuuTruBBXB(string BienBanXoaBoId);
        Task<bool> UpdateTrangThaiLuuFileBBXB(LuuTruTrangThaiBBXBViewModel model);
        Task<bool> GateForWebSocket(ParamKyBienBanHuyHoaDon @param);
        Task<bool> XoaBoHoaDon(ParamXoaBoHoaDon @params);
        Task<bool> GetStatusDaThayTheHoaDon(string HoaDonId);
        Task<bool> CheckMaTraCuuAsync(string maTraCuu);
        FileReturn XemHoaDonDongLoat(List<string> fileArray);
        FileReturn XemHoaDonDongLoat2(List<string> fileArray);
        KetQuaConvertPDF TaiHoaDon(HoaDonDienTuViewModel hoaDonDienTuViewModel);
        Task<List<ViewModels.QuanLy.DanhSachRutGonBoKyHieuHoaDonViewModel>> GetDSRutGonBoKyHieuHoaDonAsync();
        Task<PagedList<HoaDonDienTuViewModel>> GetDSHoaDonDeXoaBo(HoaDonParams pagingParams);
        Task UpdateTrangThaiQuyTrinhAsync(string id, TrangThaiQuyTrinh status);
        Task<bool> RemoveDigitalSignatureAsync(string id);
    }
}
