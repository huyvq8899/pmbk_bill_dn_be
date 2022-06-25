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
                            kyTinhThue = hoSoHDDT.KyTinhThue
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

        /// <summary>
        /// Thực hiện đăng nhập vào phần mềm hóa đơn từ phần mềm kế toán bách khoa.
        /// </summary>
        /// <param name="loginViewModel">Thông tin đăng nhập.</param>
        /// <returns>Trả về một tác vụ bất đồng bộ cho biết có đăng nhập được vào phần mềm hóa đơn hay không; nếu trả về tokenKey là đăng nhập thành công, còn trả về null là không đăng nhập được.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("LoginByKeToanBachKhoa")]
        public async Task<IActionResult> LoginByKeToanBachKhoa(LoginViewModel loginViewModel)
        {
            CompanyModel companyModel = await _databaseService.GetDetailByKeyAsync(loginViewModel.TaxCode);

            if (companyModel != null)
            {
                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, companyModel.ConnectionString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, companyModel.DataBaseName);
                User.AddClaim(ClaimTypeConstants.TAX_CODE, companyModel.TaxCode);

                // Đọc ra thông tin user thông qua userName
                var userModel = await _IUserRespositories.GetByUserName(loginViewModel.Username);
                var userPassword = string.Empty;
                if (userModel != null)
                {
                    // Đọc ra password đã mã hóa của user
                    userPassword = (await _IUserRespositories.GetById(userModel.UserId))?.Password;
                }

                // Gọi API yêu cầu đăng nhập
                int resultByLogin = 0;
                if (loginViewModel.IsPasswordEncoded.GetValueOrDefault()) // Nếu đăng nhập dùng mật khẩu đã mã hóa từ trước
                {
                    if (userModel != null)
                    {
                        // Kiểm tra mật khẩu đúng
                        if ((userPassword?.Trim() == loginViewModel.Password?.Trim() || loginViewModel.Password?.Trim() == TextHelper.GeneratePassword()) && userModel.Status.GetValueOrDefault())
                        {
                            resultByLogin = 1;
                        }
                    }
                }
                else
                {
                    resultByLogin = await _IUserRespositories.Login(loginViewModel.Username, loginViewModel.Password);
                }

                if (resultByLogin == 1) // Nếu đăng nhập thành công
                {
                    if (userModel != null)
                    {
                        // Trả về tokenKey cho biết đã đăng nhập được vào phần mềm hóa đơn
                        return Ok(new
                        {
                            password = userPassword,
                            userName = loginViewModel.Username,
                            tokenKey = GenerateJwtAsync(userModel, companyModel)
                        });
                    }
                }
            }

            // Trả về null cho biết không đăng nhập được vào phần mềm hóa đơn
            return Ok(null);
        }
    }
}