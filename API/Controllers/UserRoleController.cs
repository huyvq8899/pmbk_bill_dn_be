using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels;

namespace API.Controllers
{
    public class UserRoleController : BaseController
    {
        IUser_RoleRespositories _IUser_RoleRespositories;
        Datacontext db;
        public UserRoleController(IUser_RoleRespositories IUser_RoleRespositories, Datacontext Datacontext)
        {
            _IUser_RoleRespositories = IUser_RoleRespositories;
            db = Datacontext;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _IUser_RoleRespositories.GetAll();
            return Ok(result);
        }

        [HttpGet("GetAllByUserId/{UserId}")]
        public async Task<IActionResult> GetAllByUserId(string UserId)
        {
            var result = await _IUser_RoleRespositories.GetAllByUserId(UserId);
            return Ok(result);
        }
    }
}