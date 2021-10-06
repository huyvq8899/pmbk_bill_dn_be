using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.HeThong;
using Services.Repositories.Interfaces;
using Services.ViewModels;

namespace API.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleRespositories _IRoleRespositories;
        private readonly Datacontext _db;

        public RoleController(IRoleRespositories IRoleRespositories, Datacontext db)
        {
            _IRoleRespositories = IRoleRespositories;
            _db = db;
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IRoleRespositories.Delete(Id);
                    if (result <= 0)
                    {
                        transaction.Rollback();
                        return Ok(result);
                    }
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("CheckPhatSinh/{roleId}")]
        public async Task<IActionResult> CheckPhatSinh(string roleId)
        {
            var result = await _IRoleRespositories.CheckPhatSinh(roleId);
            return Ok(result);
        }

        [HttpGet("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            var result = await _IRoleRespositories.GetAll();
            return Ok(result);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _IRoleRespositories.GetById(Id);
            return Ok(result);
        }

        [HttpGet("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging([FromQuery]PagingParams pagingParams)
        {
            var paged = await _IRoleRespositories.GetAllPaging(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(RoleViewModel model)
        {
            var result = await _IRoleRespositories.Insert(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(RoleViewModel model)
        {
            var result = await _IRoleRespositories.Update(model);
            return Ok(result);
        }

        [HttpPost("CheckTrungMaWithObjectInput")]
        public async Task<IActionResult> CheckTrungMaWithObjectInput(RoleViewModel model)
        {
            var result = await _IRoleRespositories.CheckTrungMaWithObjectInput(model);
            return Ok(result);
        }

        [HttpGet("GetListHoaDonDaPhanQuyen/{RoleId}")]
        public async Task<IActionResult> GetListHoaDonDaPhanQuyen(string RoleId)
        {
            var result = await _IRoleRespositories.GetListHoaDonDaPhanQuyen(RoleId);
            return Ok(result);
        }

        [HttpPost("PhanQuyenMauHoaDon")]
        public async Task<IActionResult> PhanQuyenMauHoaDon(PhanQuyenMHDParams @params)
        {
            var result = await _IRoleRespositories.PhanQuyenMauHoaDon(@params.ListPQ, @params.RoleId);
            return Ok(result);
        }
    }
}