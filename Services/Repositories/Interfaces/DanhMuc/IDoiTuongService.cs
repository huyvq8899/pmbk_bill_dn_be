﻿using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IDoiTuongService
    {
        Task<List<DoiTuongViewModel>> GetAllAsync(DoiTuongParams @params = null);
        Task<PagedList<DoiTuongViewModel>> GetAllPagingAsync(DoiTuongParams @params);
        Task<DoiTuongViewModel> GetByIdAsync(string id);
        Task<FileReturn> ExportExcelAsync(DoiTuongParams @params);

        Task<DoiTuongViewModel> InsertAsync(DoiTuongViewModel model);
        Task<bool> UpdateAsync(DoiTuongViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(DoiTuongViewModel model);
        Task<List<DoiTuongViewModel>> GetAllKhachHang();
        Task<List<DoiTuongViewModel>> GetAllNhanVien();
        Task<List<DoiTuongViewModel>> ImportKhachHang(IList<IFormFile> files);
        Task<List<DoiTuongViewModel>> ConvertImportKhachHang(List<DoiTuongViewModel> model);
        Task<string> CreateFileImportKhachHangError(List<DoiTuongViewModel> list);
        Task<List<DoiTuongViewModel>> ImportNhanVien(IList<IFormFile> files);
        Task<List<DoiTuongViewModel>> ConvertImportNhanVien(List<DoiTuongViewModel> model);
        Task<string> CreateFileImportNhanVienError(List<DoiTuongViewModel> list);
        string GetLinkFileExcel(string link);
    }
}
