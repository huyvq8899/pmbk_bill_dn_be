using ManagementServices.Helper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IPermissionRespositories
    {
        Task<List<PermissionViewModel>> GetAll(bool? IsActive);
    }
}
