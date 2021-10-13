using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class HoSoHDDTService : IHoSoHDDTService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HoSoHDDTService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HoSoHDDTViewModel> GetDetailAsync()
        {
            var entity = await _db.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
            if (entity == null)
            {
                var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
                entity = new HoSoHDDT { MaSoThue = taxCode };
            }

            var result = _mp.Map<HoSoHDDTViewModel>(entity);

            if (!string.IsNullOrEmpty(result.CoQuanThueQuanLy))
            {
                result.TenCoQuanThueQuanLy = GetListCoQuanThueQuanLy().FirstOrDefault(x => x.Code == result.CoQuanThueQuanLy).Name;
            }

            return result;
        }

        public List<CityParam> GetListCoQuanThueCapCuc()
        {
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\co-quan-thue-cap-cuc.json";
            var list = new List<CityParam>().Deserialize(path).ToList();
            return list;
        }

        public List<DistrictsParam> GetListCoQuanThueQuanLy()
        {
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\co-quan-thue-quan-ly.json";
            var list = new List<DistrictsParam>().Deserialize(path).ToList();
            return list;
        }

        public async Task<HoSoHDDTViewModel> InsertAsync(HoSoHDDTViewModel model)
        {
            var entity = _mp.Map<HoSoHDDT>(model);
            await _db.HoSoHDDTs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<HoSoHDDTViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(HoSoHDDTViewModel model)
        {
            var entity = await _db.HoSoHDDTs.FirstOrDefaultAsync(x => x.HoSoHDDTId == model.HoSoHDDTId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public List<CityParam> GetListCity()
        {
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\city.json";
            var list = new List<CityParam>().Deserialize(path).ToList();
            return list;
        }

    }
}
