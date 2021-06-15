using AutoMapper;
using DLL;
using DLL.Entity;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class PermissionRespositories : IPermissionRespositories
    {
        Datacontext db;
        IMapper mp;
        public PermissionRespositories(Datacontext datacontext, IMapper mapper)
        {
            this.db = datacontext;
            this.mp = mapper;
        }

        public async Task<List<PermissionViewModel>> GetAll(bool? IsActive)
        {
            var entity = await db.Permissions.Where(x=>x.Status == IsActive || IsActive == null).ToListAsync();
            var model = mp.Map<List<PermissionViewModel>>(entity);
            return model;
        }
    }
}
