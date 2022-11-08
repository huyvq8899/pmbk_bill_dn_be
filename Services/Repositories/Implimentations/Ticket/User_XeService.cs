using DLL;
using DLL.Entity.Ticket;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.Ticket;
using Services.ViewModels;
using Services.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Ticket
{
    public class User_XeService : IUser_XeService
    {
        private readonly Datacontext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public User_XeService(Datacontext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get danh sách quyền xe cho người dùng
        /// </summary>
        /// <returns></returns>
        public async Task<List<User_XeViewModel>> GetListPermissionAsync()
        {
            var query = from x in _db.Xes
                        join ux in _db.User_Xes on x.XeId equals ux.XeId into tmpUserXes
                        from ux in tmpUserXes.DefaultIfEmpty()
                        join u in _db.Users on ux.UserId equals u.UserId into tmpUsers
                        from u in tmpUsers.DefaultIfEmpty()
                        orderby x.MaXe
                        select new User_XeViewModel
                        {
                            Xe = new XeViewModel
                            {
                                XeId = x.XeId,
                                MaXe = x.MaXe,
                                SoXe = x.SoXe,
                                LoaiXe = x.LoaiXe
                            },
                            User = u != null ? new UserViewModel
                            {
                                UserId = u.UserId,
                                CreatedDate = ux.CreatedDate
                            } : null
                        };

            var result = await query
                .GroupBy(x => x.Xe.XeId)
                .Select(x => new User_XeViewModel
                {
                    Xe = new XeViewModel
                    {
                        XeId = x.Key,
                        MaXe = x.First().Xe.MaXe,
                        SoXe = x.First().Xe.SoXe,
                        LoaiXe = x.First().Xe.LoaiXe
                    },
                    UserIds = x.Where(y => y.User != null).OrderBy(y => y.User.CreatedDate).Select(y => y.User.UserId).ToList()
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<string>> GetListXeIdByClaimUserIdAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isAdmin = await _db.Users.Where(x => x.UserId == userId).AnyAsync(x => x.IsAdmin == true || x.IsNodeAdmin == true);

            var query = from x in _db.Xes
                        join ux in _db.User_Xes on x.XeId equals ux.XeId into tmpUserXes
                        from ux in tmpUserXes.DefaultIfEmpty()
                        join u in _db.Users on ux.UserId equals u.UserId into tmpUsers
                        from u in tmpUsers.DefaultIfEmpty()
                        where isAdmin || (u != null && u.UserId == userId)
                        select x.XeId;

            var result = await query.Distinct().ToListAsync();

            return result;
        }

        /// <summary>
        /// Lưu quyền người dùng theo xe
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> SavePermissionAsync(User_XeViewModel model)
        {
            var removedList = await _db.User_Xes.Where(x => x.XeId == model.Xe.XeId).ToListAsync();
            _db.User_Xes.RemoveRange(removedList);

            if (model.UserIds.Any())
            {
                var listPermission = new List<User_Xe>();

                foreach (var userId in model.UserIds)
                {
                    var permission = new User_Xe
                    {
                        UserId = userId,
                        XeId = model.Xe.XeId,
                        CreatedDate = DateTime.Now
                    };

                    listPermission.Add(permission);
                }

                await _db.User_Xes.AddRangeAsync(listPermission);
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
