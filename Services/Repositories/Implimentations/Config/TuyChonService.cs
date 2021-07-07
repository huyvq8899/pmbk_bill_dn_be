﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.Config;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Config
{
    public class TuyChonService : ITuyChonService
    {
        Datacontext _db;
        IMapper _mp;

        public TuyChonService(
            Datacontext datacontext,
            IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<List<ConfigNoiDungEmailViewModel>> GetAllNoiDungEmail()
        {
            var result = new List<ConfigNoiDungEmailViewModel>();
            try
            {
                result = _mp.Map<List<ConfigNoiDungEmailViewModel>>(await _db.ConfigNoiDungEmails.ToListAsync());
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return result;
        }

        public async Task<bool> UpdateRangeNoiDungEmailAsync(List<ConfigNoiDungEmailViewModel> models)
        {
            List<ConfigNoiDungEmail> entities = _mp.Map<List<ConfigNoiDungEmail>>(models);
            var entities_notExist = entities.Where(x => !_db.ConfigNoiDungEmails.Select(o => o.LoaiEmail).ToList().Contains(x.LoaiEmail)).ToList();
            var entities_Exist = entities.Where(x => _db.ConfigNoiDungEmails.Select(o => o.LoaiEmail).ToList().Contains(x.LoaiEmail)).ToList();

            if (entities_Exist.Any())
            {
                _db.ConfigNoiDungEmails.UpdateRange(entities_Exist);
            }

            if (entities_notExist.Any())
            {
                _db.ConfigNoiDungEmails.AddRange(entities_notExist);
            }
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<TuyChonViewModel>> GetAllAsync(string keyword = null)
        {
            List<TuyChonViewModel> list = await _db.TuyChons
                .Where(x => !string.IsNullOrEmpty(keyword) != true || x.Ma.Contains(keyword))
                .ProjectTo<TuyChonViewModel>(_mp.ConfigurationProvider)
                .ToListAsync();

            return list;
        }

        public TuyChonViewModel GetDetail(string ma)
        {
            TuyChon entity = _db.TuyChons.FirstOrDefault(x => x.Ma == ma);
            TuyChonViewModel result = _mp.Map<TuyChonViewModel>(entity);
            return result;
        }

        public async Task<TuyChonViewModel> GetDetailAsync(string ma)
        {
            TuyChon entity = await _db.TuyChons.FirstOrDefaultAsync(x => x.Ma == ma);
            TuyChonViewModel result = _mp.Map<TuyChonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(TuyChonViewModel model)
        {
            List<TuyChon> entities = _mp.Map<List<TuyChon>>(model.NewList);
            var entities_notExist = entities.Where(x => !_db.TuyChons.Select(o => o.Ma).ToList().Contains(x.Ma)).ToList();
            var entities_Exist = entities.Where(x => _db.TuyChons.Select(o => o.Ma).ToList().Contains(x.Ma)).ToList();

            if (entities_Exist.Any())
            {
                _db.TuyChons.UpdateRange(entities_Exist);
            }

            if (entities_notExist.Any())
            {
                _db.TuyChons.AddRange(entities_notExist);
            }
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
