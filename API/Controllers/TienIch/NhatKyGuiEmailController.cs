using Services.Repositories.Interfaces.TienIch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.TienIch
{
    public class NhatKyGuiEmailController : BaseController
    {
        private readonly INhatKyGuiEmailService _nhatKyGuiEmailService;

        public NhatKyGuiEmailController(INhatKyGuiEmailService nhatKyGuiEmailService)
        {
            _nhatKyGuiEmailService = nhatKyGuiEmailService;
        }
    }
}
