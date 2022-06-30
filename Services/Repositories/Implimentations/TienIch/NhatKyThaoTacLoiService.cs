using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.TienIch;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyThaoTacLoiService : INhatKyThaoTacLoiService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public NhatKyThaoTacLoiService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<List<NhatKyThaoTacLoiViewModel>> GetByRefIdAsync(string refId)
        {
            var result = await _db.NhatKyThaoTacLois
                .Where(x => x.RefId == refId)
                .OrderBy(x => x.CreatedDate)
                .ProjectTo<NhatKyThaoTacLoiViewModel>(_mp.ConfigurationProvider)
                .ToListAsync();

            return result;
        }

        public async Task<bool> InsertAsync(NhatKyThaoTacLoiViewModel model)
        {
            // remove old list nhat ky
            var oldNhatKys = await _db.NhatKyThaoTacLois.Where(x => x.RefId == model.RefId).ToListAsync();
            _db.NhatKyThaoTacLois.RemoveRange(oldNhatKys);

            // insert
            var entity = _mp.Map<NhatKyThaoTacLoi>(model);
            await _db.NhatKyThaoTacLois.AddAsync(entity);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }
    }
}
