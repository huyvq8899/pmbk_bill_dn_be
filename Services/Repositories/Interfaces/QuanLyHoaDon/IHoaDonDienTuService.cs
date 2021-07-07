﻿using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
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
        Task<string> CreateSoHoaDon(MauHoaDonViewModel ms);
        Task<ResultParams> CapPhatSoHoaDon(HoaDonDienTuViewModel hd, string soHoaDon);
        Task<ResultParams> CapPhatSoHoaDonHangLoat(List<HoaDonDienTuViewModel> hd, List<string> soHoaDon);
        Task<List<ChiTietMauHoaDon>> GetListChiTietByMauHoaDon(string mauHoaDonId);
        Task<string> ConvertHoaDonToFilePDF(HoaDonDienTuViewModel hd, int LoaiMau = 1, int LoaiThueSuat = 1, int LoaiKhoGiay = 1);
    }
}
