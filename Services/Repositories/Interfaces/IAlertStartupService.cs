using ManagementServices.Helper;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IAlertStartupService
    {
        Task<List<AlertStartupViewModel>> GetAll();
        Task<AlertStartupViewModel> GetByStatus();
        Task<AlertStartupViewModel> GetById(string Id);
        Task<AlertStartupViewModel> Insert(AlertStartupViewModel model);
        Task<int> Delete(string Id);
        Task<bool> Update(AlertStartupViewModel model);
    }
}
