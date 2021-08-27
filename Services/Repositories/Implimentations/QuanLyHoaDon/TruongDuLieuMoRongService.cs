using AutoMapper;
using DLL;
using DLL.Entity.Config;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class TruongDuLieuMoRongService : ITruongDuLieuMoRongService
    {
        Datacontext _db;
        IMapper _mp;
        public TruongDuLieuMoRongService(Datacontext db, IMapper mp)
        {
            _db = db;
            _mp = mp;
        }

        public async Task<bool> InsertRangeAsync(List<TruongDuLieuMoRongViewModel> range)
        {
            var entities = _mp.Map<List<TruongDuLieuMoRong>>(range);
            await _db.TruongDuLieuMoRongs.AddRangeAsync(entities);
            return await _db.SaveChangesAsync() == range.Count;
        }

        public async Task<bool> UpdateRangeAsync(List<TruongDuLieuMoRongViewModel> range)
        {
            try
            {
                //var entities = _mp.Map<List<TruongDuLieuMoRong>>(range);
                //_db.TruongDuLieuMoRongs.UpdateRange(entities);
                foreach(var item in range)
                {
                    var entity = await _db.TruongDuLieuMoRongs.FirstOrDefaultAsync(x => x.Id == item.Id);

                    if(entity != null) _db.Entry<TruongDuLieuMoRong>(entity).CurrentValues.SetValues(item);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return false;
        }
    }
}
