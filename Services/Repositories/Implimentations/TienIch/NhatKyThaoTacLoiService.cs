using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyThaoTacLoiService : INhatKyThaoTacLoiService
    {
        public Task<List<NhatKyThaoTacLoiViewModel>> GetByRefIdAsync(string refId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAsync(NhatKyThaoTacLoiViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
