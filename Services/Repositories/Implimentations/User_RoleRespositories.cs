using AutoMapper;
using DLL;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class User_RoleRespositories : IUser_RoleRespositories
    {
        private readonly Datacontext db;
        private readonly IMapper mp;
        public User_RoleRespositories(Datacontext datacontext, IMapper mapper)
        {
            this.db = datacontext;
            this.mp = mapper;
        }

        public async Task<List<User_RoleViewModel>> GetAll()
        {
            var entity = await db.User_Roles.ToListAsync();
            var model = mp.Map<List<User_RoleViewModel>>(entity);
            return model;
        }

        public async Task<List<User_Role_ByUserViewModel>> GetAllByUserId(string UserId)
        {
            var query = await (from table1 in db.Roles
                               join table2 in db.User_Roles.Where(x => x.UserId.ToLower() == UserId.ToLower()) on table1.RoleId equals table2.RoleId into temp
                               from table2 in temp.DefaultIfEmpty()
                               select new User_Role_ByUserViewModel
                               {
                                   RoleId = table1.RoleId,
                                   RoleName = table1.RoleName,
                                   UserId = table2.UserId,
                               }).ToListAsync();
            return query;
        }
    }
}
