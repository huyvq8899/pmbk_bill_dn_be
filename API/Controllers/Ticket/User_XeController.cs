using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Ticket;
using Services.ViewModels.Ticket;
using System.Threading.Tasks;

namespace API.Controllers.Ticket
{
    public class User_XeController : BaseController
    {
        private readonly IUser_XeService _user_XeService;
        private readonly Datacontext _db;

        public User_XeController(IUser_XeService user_XeService,
            Datacontext db)
        {
            _user_XeService = user_XeService;
            _db = db;
        }

        [HttpGet("GetListPermission")]
        public async Task<IActionResult> GetListPermission()
        {
            var result = await _user_XeService.GetListPermissionAsync();
            return Ok(result);
        }

        [HttpPost("SavePermission")]
        public async Task<IActionResult> SavePermission(User_XeViewModel model)
        {
            var result = await _user_XeService.SavePermissionAsync(model);
            return Ok(result);
        }
    }
}
