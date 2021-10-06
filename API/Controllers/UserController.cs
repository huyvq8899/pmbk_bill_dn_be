using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using API.Extentions;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRespositories _IUserRespositories;
        private readonly Datacontext db;
        public UserController(IUserRespositories IUserRespositories, Datacontext Datacontext)
        {
            _IUserRespositories = IUserRespositories;
            db = Datacontext;
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _IUserRespositories.Delete(Id);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _IUserRespositories.GetAll();
            return Ok(result);
        }
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _IUserRespositories.GetAllActive();
            return Ok(result);
        }
        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _IUserRespositories.GetById(Id);
            return Ok(result);
        }
        [HttpGet("GetDataLogin/{userName}")]
        public async Task<IActionResult> GetDataLogin(string userName)
        {
            var userModel = await _IUserRespositories.GetByUserName(userName);
            return Ok(new
            {
                userName = userModel.UserName,
                userId = userModel.UserId,
                model = userModel,
            });
        }
        [HttpGet("GetByUserName/{UserName}")]
        public async Task<IActionResult> GetByUserName(string UserName)
        {
            var result = await _IUserRespositories.GetByUserName(UserName);
            return Ok(result);
        }
        [HttpGet("CheckUserName/{userName}")]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            var result = await _IUserRespositories.CheckUserName(userName);
            return Ok(result);
        }
        [HttpGet("CheckEmail/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var result = await _IUserRespositories.CheckEmail(email);
            return Ok(result);
        }
        [HttpGet("CheckPass/{pass}")]
        public async Task<IActionResult> CheckPass(string username, string pass)
        {
            var result = await _IUserRespositories.CheckPass(username, pass);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(UserViewModel model)
        {
            var result = await _IUserRespositories.Insert(model);
            return Ok(result);
        }
        [HttpGet("ChangeStatus/{userId}")]
        public async Task<IActionResult> ChangeStatus(string userId)
        {
            var result = await _IUserRespositories.ChangeStatus(userId);
            return Ok(result);
        }
        [HttpGet("SetOnline/{userId}/{isOnline}")]
        public async Task<IActionResult> SetOnline(string userId, bool isOnline)
        {
            var result = await _IUserRespositories.SetOnline(userId, isOnline);
            return Ok(result);
        }
        [HttpPost("SetRole")]
        public async Task<IActionResult> SetRole(SetRoleParam param)
        {
            var result = false;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    result = await _IUserRespositories.SetRole(param);
                    if (!result)
                    {
                        transaction.Rollback();
                    }
                    else transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(result);
                }
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            var result = await _IUserRespositories.Update(model);
            return Ok(result);
        }
        [HttpPut("UpdatePassWord")]
        public async Task<IActionResult> UpdatePassWord(UserViewModel model)
        {
            var result = await _IUserRespositories.UpdatePassWord(model);
            return Ok(result);
        }
        [HttpPost("DeleteAll")]
        public async Task<IActionResult> DeleteAll(List<string> Ids)
        {
            var result = await _IUserRespositories.DeleteAll(Ids);
            return Ok(result);
        }
        [HttpGet("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging([FromQuery]PagingParams pagingParams)
        {
            var paged = await _IUserRespositories.GetAllPagingAsync(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });

        }
        [HttpPost("UpdateAvatar")]
        public async Task<IActionResult> UpdateAvatar(IList<IFormFile> files)
        {
            string fileName = Request.Form["fileName"];
            string fileSize = Request.Form["fileSize"];
            string userId = Request.Form["userId"];
            var result = await _IUserRespositories.UpdateAvatar(files,fileName,fileSize,userId);
            if (result.Result != true) throw new Exception("");
            return Ok(new {
                result.Result,
                result.User
            });
        }

        [HttpPost("GetPermissionByUserName")]
        public async Task<IActionResult> GetPermissionByUserName(InputUser inpUser)
        {
            var user = await _IUserRespositories.GetByUserName(inpUser.UserName);
            if (user.IsAdmin == null) user.IsAdmin = false;
            if (user.IsNodeAdmin == null) user.IsNodeAdmin = false;
            if (user.IsAdmin.Value || user.IsNodeAdmin.Value)
            {
                return Ok(true);
            }
            var result = await _IUserRespositories.GetPermissionByUserName_new(inpUser.UserName);
            return Ok(result);
        }

        [HttpPost("PhanQuyenUser")]
        public async Task<IActionResult> PhanQuyenUser(UserRoleParams param)
        {
            var result = await _IUserRespositories.PhanQuyenUser(param);
            return Ok(result);
        }

        [HttpGet("GetUserOnline")]
        public async Task<IActionResult> GetUserOnline()
        {
            var result = await _IUserRespositories.GetUserOnline();
            return Ok(result);
        }

        [HttpPost("CheckTrungTenDangNhap")]
        public async Task<IActionResult> CheckTrungTenDangNhap(UserViewModel user)
        {
            var result = await _IUserRespositories.CheckTrungTenDangNhap(user);
            return Ok(result);
        }

        [HttpGet("GetThongTinGanNhat")]
        public async Task<IActionResult> GetThongTinGanNhat()
        {
            var result = await _IUserRespositories.GetThongTinGanNhat();
            return Ok(result);
        }
    }

    public class InputUser
    {
        public string UserName { get; set; }
    }
}
