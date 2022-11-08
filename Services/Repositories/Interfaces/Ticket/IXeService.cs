using ManagementServices.Helper;
using Services.ViewModels.Ticket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Ticket
{
    public interface IXeService
    {
        Task<bool> Insert(XeViewModel model);
        Task<bool> Update(XeViewModel model);
        Task<bool> Delete(string id);
        Task<List<XeViewModel>> GetAll();
        Task<PagedList<XeViewModel>> GetAllPaging(PagingParams param);
        Task<XeViewModel> GetById(string id);
        Task<List<XeViewModel>> GetAllActiveAsync();
    }
}
