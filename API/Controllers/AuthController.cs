using API.Extentions;
using DLL;
using DLL.Constants;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IConfiguration _config;

        IUserRespositories _IUserRespositories;

        Datacontext db;

        IDatabaseService _databaseService;

        ITuyChonService _ITuyChonService;

        public AuthController(IUserRespositories IUserRespositories,
            Datacontext Datacontext,
            IConfiguration IConfiguration,
            IDatabaseService IDatabaseService,
            ITuyChonService ITuyChonService)
        {
            _IUserRespositories = IUserRespositories;
            db = Datacontext;
            _config = IConfiguration;
            _databaseService = IDatabaseService;
            _ITuyChonService = ITuyChonService;
        }

        [AllowAnonymous]
        [HttpGet("UpdateDatabaseMultilDB/{dbString}")]
        public async Task<IActionResult> UpdateDatabaseMultilDB(string dbString)
        {
            try
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);
                db.Database.Migrate();

                return Ok(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            int result = 0;

            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(login.TaxCode);
            if (companyModel != null)
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);

                // Login with db name.
                result = await _IUserRespositories.Login(login.username, login.password);
            }
            else
            {
                result = -2;
            }

            // Login success
            if (result == 1)
            {
                var userModel = await _IUserRespositories.GetByUserName(login.username);
                if (userModel != null)
                {
                    var tuyChon = await _ITuyChonService.GetAllAsync();
                    return Ok(new
                    {
                        result,
                        userName = userModel.UserName,
                        tokenKey = await GenerateJwtAsync(userModel, companyModel, tuyChon),
                        userId = userModel.UserId,
                        model = userModel,
                        typeDetail = companyModel.TypeDetail,
                        setting = tuyChon,
                        urlInvoice = companyModel.UrlInvoice
                    });
                }
            }
            return Ok(new { result, userName = "", tokenKey = "" });
        }

        private async Task<string> GenerateJwtAsync(UserViewModel user, CompanyModel company, List<TuyChonViewModel> tuyChons)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypeConstants.FULL_NAME, user.FullName),
                new Claim(ClaimTypes.Role, user.RoleName),
                new Claim(ClaimTypeConstants.CONNECTION_STRING, company.ConnectionString),
                new Claim(ClaimTypeConstants.DATABASE_NAME, company.DataBaseName),
            };

            //foreach (var item in tuyChons)
            //{
            //    Response.Cookies.Append(item.Ma, item.GiaTri);
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                //Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token).EncodeToken();
            //return tokenHandler.WriteToken(token);
        }
    }
}