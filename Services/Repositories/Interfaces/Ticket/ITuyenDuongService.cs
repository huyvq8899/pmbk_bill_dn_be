using ManagementServices.Helper;
using Services.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Ticket
{
    public interface ITuyenDuongService
    {
        Task<bool> Insert(TuyenDuongViewModel model);
        Task<bool> Update(TuyenDuongViewModel model);
        Task<bool> Delete(string tuyenDuongId);
        Task<List<TuyenDuongViewModel>> GetAll();
        Task<PagedList<TuyenDuongViewModel>> GetAllPaging(PagingParams param);
        Task<TuyenDuongViewModel> GetById(string id);
        Task<List<TuyenDuongViewModel>> GetAllActiveAsync();
    }
}
