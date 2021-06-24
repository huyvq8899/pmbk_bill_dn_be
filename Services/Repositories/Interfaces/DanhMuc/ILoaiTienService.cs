using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface ILoaiTienService
    {
        Task<bool> Insert(LoaiTienViewModel model);
        Task<bool> InsertRange(List<LoaiTienViewModel> model);
        Task<bool> Update(LoaiTienViewModel model);
        Task<bool> Delete(string id);
        Task<LoaiTienViewModel> GetById(string id);
        Task<LoaiTienViewModel> GetByMa(string ma);
        Task<LoaiTienViewModel> GetTienVietAsync();
        LoaiTienViewModel GetTienViet();
        Task<List<LoaiTienViewModel>> GetAll();
        Task<List<LoaiTienViewModel>> GetAllActive();
        Task<bool> CheckMa(string maVT_HH);
        Task<int> CheckMa(LoaiTienViewModel data);
        Task<string> ExportExcelLoaiTien();
    }
}
