using Services.ViewModels.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Config
{
    public interface ITuyChonService
    {
        Task<List<TuyChonViewModel>> GetAllAsync(string keyword = null);
        Task<TuyChonViewModel> GetDetailAsync(string ma);
        TuyChonViewModel GetDetail(string ma);
        Task<bool> UpdateAsync(TuyChonViewModel model);
        Task<List<ConfigNoiDungEmailViewModel>> GetAllNoiDungEmail();
        Task<bool> UpdateRangeNoiDungEmailAsync(List<ConfigNoiDungEmailViewModel> models);
    }
}
