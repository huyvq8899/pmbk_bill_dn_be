using Services.ViewModels.DanhMuc;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IHoSoHDDTService
    {
        Task<HoSoHDDTViewModel> GetDetailAsync();

        Task<HoSoHDDTViewModel> InsertAsync(HoSoHDDTViewModel model);
        Task<bool> UpdateAsync(HoSoHDDTViewModel model);
    }
}
