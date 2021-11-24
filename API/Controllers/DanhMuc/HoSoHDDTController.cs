using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class HoSoHDDTController : BaseController
    {
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public HoSoHDDTController(IHoSoHDDTService hoSoHDDTService)
        {
            _hoSoHDDTService = hoSoHDDTService;
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail()
        {
            var result = await _hoSoHDDTService.GetDetailAsync();
            return Ok(result);
        }

        [HttpGet("GetListCoQuanThueCapCuc")]
        public IActionResult GetListCoQuanThueCapCuc()
        {
            var result = _hoSoHDDTService.GetListCoQuanThueCapCuc();
            return Ok(result);
        }

        [HttpGet("GetListCoQuanThueQuanLy")]
        public IActionResult GetListCoQuanThueQuanLy()
        {
            var result = _hoSoHDDTService.GetListCoQuanThueQuanLy();
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HoSoHDDTViewModel model)
        {
            var result = await _hoSoHDDTService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HoSoHDDTViewModel model)
        {
            var result = await _hoSoHDDTService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpGet("GetListCity")]
        public IActionResult GetListCity()
        {
            var result = _hoSoHDDTService.GetListCity();
            return Ok(result);
        }
    }
}
