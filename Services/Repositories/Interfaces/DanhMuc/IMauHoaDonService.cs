using DLL.Entity.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IMauHoaDonService
    {
        Task<bool> Insert(MauHoaDon model);
        Task<bool> Update(MauHoaDon model);
        Task<bool> Delete(string id);
        Task<bool> DeleteByMauSo(MauHoaDonViewModel mauSo);
        Task<bool> CheckTrungMauSo(string mauSo);
        Task<MauHoaDonViewModel> GetById(string id);
        Task<List<MauHoaDonViewModel>> GetAll();
        Task<List<MauHoaDonViewModel>> GetAllActive();
        Task<List<MauHoaDonViewModel>> GetAllTuyChinh();
        Task<bool> CheckTrungMa(string ma);
    }
}
