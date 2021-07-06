using Services.Repositories.Interfaces.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoPhatHanhController : BaseController
    {
        private readonly IThongBaoPhatHanhService _thongBaoPhatHanhService;
        public ThongBaoPhatHanhController(IThongBaoPhatHanhService thongBaoPhatHanhService)
        {
            _thongBaoPhatHanhService = thongBaoPhatHanhService;
        }
    }
}
