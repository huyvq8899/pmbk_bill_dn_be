using AutoMapper;
using DLL;
using DLL.Entity.Ticket;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.Ticket;
using Services.ViewModels.Ticket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Ticket
{
    public class XeService : IXeService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public XeService(Datacontext db, IMapper mp)
        {
            _db = db;
            _mp = mp;
        }

        public async Task<bool> Delete(string id)
        {
            var entity = _db.Xes.FirstOrDefault(x => x.XeId == id);
            _db.Xes.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<XeViewModel>> GetAll()
        {
            var result = await _db.Xes
                .OrderBy(x => x.MaXe)
                .Select(x => new XeViewModel
                {
                    XeId = x.XeId,
                    MaXe = x.MaXe,
                    SoXe = x.SoXe,
                    LoaiXe = x.LoaiXe,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifyBy = x.ModifyBy,
                    ModifyDate = x.ModifyDate,
                    Status = x.Status,
                    STT = x.STT
                }).OrderBy(x => x.CreatedDate).ToListAsync();

            return result;
        }

        public async Task<List<XeViewModel>> GetAllActiveAsync()
        {
            var query = _db.Xes.Where(x => x.Status == true)
                .OrderBy(x => x.MaXe)
                .Select(x => new XeViewModel
                {
                    XeId = x.XeId,
                    MaXe = x.MaXe,
                    SoXe = x.SoXe,
                    LoaiXe = x.LoaiXe,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifyBy = x.ModifyBy,
                    ModifyDate = x.ModifyDate,
                    Status = x.Status,
                    STT = x.STT
                });

            return await query.OrderBy(x => x.STT).ToListAsync();
        }

        public async Task<PagedList<XeViewModel>> GetAllPaging(PagingParams param)
        {
            var query = _db.Xes.OrderBy(x => x.MaXe)
                .Select(x => new XeViewModel
                {
                    XeId = x.XeId,
                    MaXe = x.MaXe,
                    SoXe = x.SoXe,
                    LoaiXe = x.LoaiXe,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifyBy = x.ModifyBy,
                    ModifyDate = x.ModifyDate,
                    Status = x.Status
                });

            if (param.IsActive.HasValue)
            {
                query = query.Where(x => x.Status == param.IsActive);
            }

            if (param.PageSize == -1)
            {
                param.PageSize = await query.CountAsync();
            }

            return await PagedList<XeViewModel>.CreateAsync(query, param.PageNumber, param.PageSize);
        }

        public async Task<XeViewModel> GetById(string id)
        {
            var query = _db.Xes.Where(x => x.XeId == id)
                .Select(x => new XeViewModel
                {
                    XeId = x.XeId,
                    MaXe = x.MaXe,
                    SoXe = x.SoXe,
                    LoaiXe = x.LoaiXe,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    ModifyBy = x.ModifyBy,
                    ModifyDate = x.ModifyDate,
                    Status = x.Status,
                    STT = x.STT
                });
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> Insert(XeViewModel model)
        {
            var entity = _mp.Map<Xe>(model);
            await _db.Xes.AddAsync(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(XeViewModel model)
        {
            var entity = _mp.Map<Xe>(model);
            _db.Xes.Update(entity);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
