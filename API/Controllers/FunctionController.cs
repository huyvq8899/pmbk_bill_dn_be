using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces;
using Services.ViewModels;

namespace API.Controllers
{
    public class FunctionController : BaseController
    {
        IFunctionRespositories _IFunctionRespositories;
        Datacontext db;
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
                        throw new Exception("");
                    }
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
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
                        throw new Exception("");
                    }
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("GetAllForTreeByRole/{RoleId}")]
        public async Task<IActionResult> GetAllForTreeByRole(string RoleId)
        {
            var result = await _IFunctionRespositories.GetAllForTreeByRole(RoleId);
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
                        throw new Exception("");
                    }
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
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
                        throw new Exception("");
                    }
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception ex)
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