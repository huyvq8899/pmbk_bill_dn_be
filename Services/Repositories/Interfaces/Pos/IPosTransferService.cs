using Services.ViewModels.Pos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.Pos
{
    public interface IPosTransferService
    {
        Task<string> SendResponseTCTToPos(List<ResultHoaDonMTTViewModels> listResult);
    }
}
