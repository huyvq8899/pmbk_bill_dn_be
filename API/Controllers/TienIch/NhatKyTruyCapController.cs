using Services.Repositories.Interfaces.TienIch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.TienIch
{
    public class NhatKyTruyCapController : BaseController
    {
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;

        public NhatKyTruyCapController(INhatKyTruyCapService nhatKyTruyCapService)
        {
            _nhatKyTruyCapService = nhatKyTruyCapService;
        }
    }
}
