using Services.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IDatabaseService
    {
        Task<CompanyModel> GetDetailByKeyAsync(string key);
    }
}
