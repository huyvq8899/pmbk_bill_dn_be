using AutoMapper;
using DLL;
using DLL.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AutoMapper.QueryableExtensions;

namespace Services.Repositories.Implimentations
{
    public class AlertStartupService : IAlertStartupService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AlertStartupService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task<List<AlertStartupViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<AlertStartupViewModel> GetByStatus()
        {
            try
            {
                AlertStartupViewModel main = await _db.AlertStartups
                .Where(x => x.Status == true).OrderByDescending(x => x.ModifyDate).ProjectTo<AlertStartupViewModel>(_mp.ConfigurationProvider)
                .FirstOrDefaultAsync();

                return main;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Task<AlertStartupViewModel> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<AlertStartupViewModel> Insert(AlertStartupViewModel model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = DateTime.Now;
            model.ModifyDate = model.CreatedDate;
            var entity = _mp.Map<AlertStartup>(model);
            await _db.AlertStartups.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<AlertStartupViewModel>(entity);
            return result;
        }

        public async Task<bool> Update(AlertStartupViewModel model)
        {
            var entity = await _db.AlertStartups.FirstOrDefaultAsync(x => x.Id == model.Id);
            _db.Entry(entity).CurrentValues.SetValues(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> Delete(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
