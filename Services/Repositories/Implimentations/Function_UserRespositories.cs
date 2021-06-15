using AutoMapper;
using DLL;
using DLL.Entity;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class Function_UserRespositories : IFunction_UserRespositories
    {
        Datacontext db;
        IMapper mp;
        public Function_UserRespositories(Datacontext datacontext, IMapper mapper)
        {
            this.db = datacontext;
            this.mp = mapper;
        }

        public async Task<bool> Delete(string FUID)
        {
            var entity = await db.Function_Users.FirstOrDefaultAsync(x => x.FUID == FUID);
            db.Function_Users.Remove(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<List<Function_UserViewModel>> GetAll()
        {
            var query = await db.Function_Users.ToListAsync();
            return mp.Map<List<Function_UserViewModel>>(query);
        }

        public async Task<bool> Insert(Function_UserViewModel model)
        {
            model.FUID = Guid.NewGuid().ToString();
            var entity = mp.Map<Function_User>(model);
            await db.Function_Users.AddAsync(entity);
            return await db.SaveChangesAsync() > 0;
        }

        public async Task<List<Function_UserViewModel>> GetFunctionByUserId(string UserId)
        {
            var entity = await db.Function_Users.Where(x => x.UserId.ToLower().Trim() == UserId.ToLower().Trim()).ToListAsync();
            var model = mp.Map<List<Function_UserViewModel>>(entity);
            return model;
        }
    }
}
