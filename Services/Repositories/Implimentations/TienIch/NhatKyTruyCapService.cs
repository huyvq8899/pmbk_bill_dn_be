using ManagementServices.Helper;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyTruyCapService : INhatKyTruyCapService
    {
        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<NhatKyTruyCapViewModel>> GetAllPagingAsync(NhatKyTruyCapParams @params)
        {
            throw new System.NotImplementedException();
        }

        public Task<NhatKyTruyCapViewModel> GetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
