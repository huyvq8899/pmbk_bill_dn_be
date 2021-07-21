using AutoMapper;
using DLL;
using DLL.Entity.QuanLyHoaDon;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class BienBanDieuChinhService : IBienBanDieuChinhService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public BienBanDieuChinhService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == id);
            _db.BienBanDieuChinhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<BienBanDieuChinhViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.BienBanDieuChinhs.AsNoTracking().FirstOrDefaultAsync(x => x.BienBanDieuChinhId == id);
            var result = _mp.Map<BienBanDieuChinhViewModel>(entity);
            return result;
        }

        public async Task<BienBanDieuChinhViewModel> InsertAsync(BienBanDieuChinhViewModel model)
        {
            var entity = _mp.Map<BienBanDieuChinh>(model);
            await _db.BienBanDieuChinhs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<BienBanDieuChinhViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(BienBanDieuChinhViewModel model)
        {
            var entity = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == model.BienBanDieuChinhId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
