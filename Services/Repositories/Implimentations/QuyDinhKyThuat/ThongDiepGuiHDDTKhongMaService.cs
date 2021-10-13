using AutoMapper;
using AutoMapper.Configuration;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Helper;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class ThongDiepGuiHDDTKhongMaService : IThongDiepGuiHDDTKhongMaService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mp;

        public ThongDiepGuiHDDTKhongMaService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            IMapper mp)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _mp = mp;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<FileReturn> ExportXMLAsync(ThongDiepGuiHDDTKhongMaViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<FileReturn> ExportXMLAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ThongDiepGuiHDDTKhongMaViewModel>> GetAllPagingAsync(PagingParams @params)
        {
            throw new NotImplementedException();
        }

        public async Task<ThongDiepGuiHDDTKhongMaViewModel> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ThongDiepGuiHDDTKhongMaViewModel> InsertAsync(ThongDiepGuiHDDTKhongMaViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(ThongDiepGuiHDDTKhongMaViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
