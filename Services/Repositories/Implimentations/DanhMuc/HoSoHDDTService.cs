using AutoMapper;
using DLL;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class HoSoHDDTService : IHoSoHDDTService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public HoSoHDDTService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<HoSoHDDTViewModel> GetDetailAsync()
        {
            var entity = await _db.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
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
    }
}
