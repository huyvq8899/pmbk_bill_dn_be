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
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels;
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
        private readonly IUserRespositories _IUserRespositories;
        private readonly Datacontext db;
        private readonly IDatabaseService _databaseService;
        private readonly ITuyChonService _ITuyChonService;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public AuthController(IUserRespositories IUserRespositories,
            Datacontext Datacontext,
            IConfiguration IConfiguration,
            IDatabaseService IDatabaseService,
            ITuyChonService ITuyChonService,
            IHoSoHDDTService hoSoHDDTService)
        {
            _IUserRespositories = IUserRespositories;
            db = Datacontext;
            _config = IConfiguration;
            _databaseService = IDatabaseService;
            _ITuyChonService = ITuyChonService;
            _hoSoHDDTService = hoSoHDDTService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(login.TaxCode);
            if (companyModel != null)
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
                User.AddClaim(ClaimTypeConstants.TAX_CODE, companyModel.TaxCode);

                // Login with db name.
                var result = await _IUserRespositories.Login(login.Username, login.Password);
                // Login success
                if (result == 1)
                {
                    var userModel = await _IUserRespositories.GetByUserName(login.Username);
                    var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

                    if (userModel != null)
                    {
                        var tuyChon = await _ITuyChonService.GetAllAsync();
                        return Ok(new
                        {
                            result,
                            userName = userModel.UserName,
                            tokenKey = GenerateJwtAsync(userModel, companyModel),
                            userId = userModel.UserId,
                            model = userModel,
                            typeDetail = companyModel.TypeDetail,
                            setting = tuyChon,
                            urlInvoice = companyModel.UrlInvoice,
                            kyKeKhaiThue = hoSoHDDT.KyKeKhaiThue
                        });
                    }
                    else return Ok(new { result, userName = "", tokenKey = "" });
                }
                else return Ok(new { result, userName = "", tokenKey = "" });
            }

            return Ok(new { result = -2, userName = "", tokenKey = "" });
        }

        [AllowAnonymous]
        [HttpPost("UpdateDatabase")]
        public IActionResult UpdateDatabase([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                string dbString = (param.KeyString).Base64Decode();

                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);

                db.Database.Migrate();

                return Ok(true);
            }

            return Ok(false);
        }


        private string GenerateJwtAsync(UserViewModel user, CompanyModel company)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypeConstants.FULL_NAME, user.FullName),
                new Claim(ClaimTypes.Role, user.RoleName),
                new Claim(ClaimTypeConstants.CONNECTION_STRING, company.ConnectionString),
                new Claim(ClaimTypeConstants.DATABASE_NAME, company.DataBaseName),
                new Claim(ClaimTypeConstants.TAX_CODE, company.TaxCode),
            };

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