﻿using DLL.Enums;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IMauHoaDonService
    {
        List<ImageParam> GetMauHoaDonBackgrounds();
        Task<List<MauHoaDonViewModel>> GetAllAsync(MauHoaDonParams @params = null);
        Task<PagedList<MauHoaDonViewModel>> GetAllPagingAsync(MauHoaDonParams @params);
        Task<MauHoaDonViewModel> GetByIdAsync(string id);
        List<MauParam> GetListMauHoaDon(MauHoaDonParams @params);
        Task<List<string>> GetAllMauSoHoaDon();
        Task<List<string>> GetAllKyHieuHoaDon(string ms = "");
        Task<List<MauHoaDonViewModel>> GetListMauDaDuocChapNhanAsync();
        List<EnumModel> GetListQuyDinhApDung();
        List<EnumModel> GetListLoaiHoaDon();
        List<EnumModel> GetListLoaiMau();
        List<EnumModel> GetListLoaiThueGTGT();
        List<EnumModel> GetListLoaiNgonNgu();
        List<EnumModel> GetListLoaiKhoGiay();
        Task<FileReturn> PreviewPdfAsync(string id, HinhThucMauHoaDon loai);
        Task<FileReturn> DownloadFileAsync(string id, HinhThucMauHoaDon loai, DinhDangTepMau loaiFile);
        Task<string> CheckAllowUpdateAsync(MauHoaDonViewModel model); // 0 allow
        Task<FileReturn> ExportMauHoaDonAsync(ExportMauHoaDonParams @params); // 0 allow

        Task<MauHoaDonViewModel> InsertAsync(MauHoaDonViewModel model);
        Task<bool> UpdateAsync(MauHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMauSoAsync(MauHoaDonViewModel model);
        Task<ChiTietMauHoaDon> GetChiTietByMauHoaDon(string mauHoaDonId);
    }
}
