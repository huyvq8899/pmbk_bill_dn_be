﻿using DLL.Enums;
using ManagementServices.Helper;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
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
        Task<List<MauHoaDonViewModel>> GetListMauDaDuocChapNhanAsync();
        List<EnumModel> GetListQuyDinhApDung();
        List<EnumModel> GetListLoaiHoaDon();
        List<EnumModel> GetListLoaiMau();
        List<EnumModel> GetListLoaiThueGTGT();
        List<EnumModel> GetListLoaiNgonNgu();
        List<EnumModel> GetListLoaiKhoGiay();

        Task<List<string>> GetAllMauSoHoaDon();
        Task<List<string>> GetAllKyHieuHoaDon(string ms="");
        Task<MauHoaDonViewModel> InsertAsync(MauHoaDonViewModel model);
        Task<bool> UpdateAsync(MauHoaDonViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<ChiTietMauHoaDon> GetChiTietByMauHoaDon(string mauHoaDonId);
    }
}
