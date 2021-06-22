using Services.ViewModels.DanhMuc;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IHoSoHDDTService
    {
        Task<HoSoHDDTViewModel> GetDetailAsync();

        Task<bool> UpdateAsync(HoSoHDDTViewModel model);
    }
}
