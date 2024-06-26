﻿using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IHangHoaDichVuService
    {
        Task<List<HangHoaDichVuViewModel>> GetAllAsync(HangHoaDichVuParams @params = null);
        Task<PagedList<HangHoaDichVuViewModel>> GetAllPagingAsync(HangHoaDichVuParams @params);
        Task<HangHoaDichVuViewModel> GetByIdAsync(string id);
        Task<bool> CheckPhatSinhAsync(string id);
        Task<FileReturn> ExportExcelAsync(HangHoaDichVuParams @params);
        HangHoaDichVuViewModel CheckMaOutObject(string ma, List<HangHoaDichVu> models);

        Task<HangHoaDichVuViewModel> InsertAsync(HangHoaDichVuViewModel model);

        Task<HangHoaDichVuViewModel> GetHangHoaDichVuByMa(string ma);
        Task<ResultParams> FilterPosSyncGoods(HangHoaDichVuViewModel model);


        Task<bool> UpdateAsync(HangHoaDichVuViewModel model);
        Task<bool> DeleteAsync(string id);
        Task<bool> CheckTrungMaAsync(HangHoaDichVuViewModel model);
        Task<List<HangHoaDichVuViewModel>> ImportVTHH(IList<IFormFile> files, int modeValue);
        Task<List<HangHoaDichVuViewModel>> ConvertImport(List<HangHoaDichVuViewModel> model);
        string CreateFileImportVTHHError(List<HangHoaDichVuViewModel> list);
        string GetLinkFileExcel(string link);
    }
}
