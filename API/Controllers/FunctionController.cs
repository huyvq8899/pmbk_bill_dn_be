using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.HeThong;
using Services.Repositories.Interfaces;
using Services.ViewModels;

namespace API.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionRespositories _IFunctionRespositories;
        private readonly Datacontext db;
        public FunctionController(IFunctionRespositories IFunctionRespositories, Datacontext Datacontext)
        {
            _IFunctionRespositories = IFunctionRespositories;
            db = Datacontext;
        }
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(FunctionViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IFunctionRespositories.Insert(model);
                    if (result == false)
                    {
                        transaction.Rollback();
                    }
                    else transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }
        [HttpDelete("Delete/{functionId}")]
        public async Task<IActionResult> Delete(string functionId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IFunctionRespositories.Delete(functionId);
                    if (result == false)
                    {
                        transaction.Rollback();
                    }
                    else transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("GetAllForTreeByRole")]
        public async Task<IActionResult> GetAllForTreeByRole(FunctionParams @params)
        {
            var result = await _IFunctionRespositories.GetAllForTreeByRole(@params.RoleId, @params.ToanQuyen);
            return Ok(result);
        }

        [HttpGet("GetThaoTacByFunction")]
        public async Task<IActionResult> GetThaoTacByFunction([FromQuery] string FunctionId, [FromQuery] string RoleId)
        {
            var result = await _IFunctionRespositories.GetThaoTacOfFunction(FunctionId, RoleId);
            return Ok(result);
        }

        [HttpPost("InsertUpdateThaoTacToFunction")]
        public async Task<IActionResult> InsertUpdateThaoTacToFunction(Function_ThaoTacViewModel model)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IFunctionRespositories.InsertUpdateThaoTacToFunction(model);
                    if (result == false)
                    {
                        transaction.Rollback();
                    }
                    else transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("InsertUpdateMultipleThaoTacToFunction")]
        public async Task<IActionResult> InsertUpdateMultipleThaoTacToFunction(List<Function_ThaoTacViewModel> models)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IFunctionRespositories.InsertUpdateMultipleThaoTacToFunction(models);
                    if (result == false)
                    {
                        transaction.Rollback();
                    }
                    else transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("GetAllForTreeByUser/{UserId}")]
        public async Task<IActionResult> GetAllForTreeByUser(string UserId)
        {
            var result = await _IFunctionRespositories.GetAllForTreeByUser(UserId);
            return Ok(result);
        }
    }
}