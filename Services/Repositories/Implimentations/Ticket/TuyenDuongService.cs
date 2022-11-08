using AutoMapper;
using DLL;
using DLL.Entity.Ticket;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Repositories.Interfaces.Ticket;
using Services.ViewModels.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Ticket
{
    public class TuyenDuongService : ITuyenDuongService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public TuyenDuongService(Datacontext db, IMapper mp)
        {
            _db = db;
            _mp = mp;
        }

        public async Task<bool> Insert(TuyenDuongViewModel model)
        {
            model.TuyenDuongId = Guid.NewGuid();
            var entity = _mp.Map<TuyenDuong>(model);
            await _db.TuyenDuongs.AddAsync(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(TuyenDuongViewModel model)
        {
            var entity = _mp.Map<TuyenDuong>(model);
            _db.TuyenDuongs.Update(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(string tuyenDuongId)
        {
            var entity = _db.TuyenDuongs.FirstOrDefault(x => x.TuyenDuongId == Guid.Parse(tuyenDuongId));
            _db.TuyenDuongs.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<TuyenDuongViewModel>> GetAllPaging(PagingParams @params)
        {
            var query = _db.TuyenDuongs.OrderBy(x => x.CreatedDate)
                .Select(x => new TuyenDuongViewModel
                {
                    TuyenDuongId = x.TuyenDuongId,
                    TenTuyenDuong = x.TenTuyenDuong,
                    BenDi = x.BenDi,
                    BenDen = x.BenDen,
                    ThoiGianKhoiHanh = x.ThoiGianKhoiHanh,
                    SoXe = string.Join(";", (from xe in _db.Xes
                                             where x.SoXe.Contains(xe.XeId)
                                             select xe.SoXe).ToList()),
                    Status = x.Status,
                    STT = x.STT,
                });
            if (@params.IsActive.HasValue)
            {
                query = query.Where(x => x.Status == @params.IsActive);
            }
            query = query.OrderBy(x => x.STT);

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<TuyenDuongViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        /// <summary>
        /// Get tuyến đường đang hoạt động
        /// </summary>
        /// <returns></returns>
        public async Task<List<TuyenDuongViewModel>> GetAllActiveAsync()
        {
            var query = _db.TuyenDuongs.Where(x => x.Status == true).OrderBy(x => x.CreatedDate)
                .Select(x => new TuyenDuongViewModel
                {
                    TuyenDuongId = x.TuyenDuongId,
                    TenTuyenDuong = x.TenTuyenDuong,
                    BenDi = x.BenDi,
                    BenDen = x.BenDen,
                    ThoiGianKhoiHanh = x.ThoiGianKhoiHanh,
                    SoXe = x.SoXe,
                    Status = x.Status,
                    STT = x.STT,
                    Xes = (from xe in _db.Xes
                           where x.SoXe.Contains(xe.XeId)
                           select new XeViewModel
                           {
                               XeId = xe.XeId,
                               MaXe = xe.MaXe,
                               SoXe = xe.SoXe
                           })
                           .ToList()
                });

            return await query.OrderBy(x => x.STT).ToListAsync();
        }


        public async Task<TuyenDuongViewModel> GetById(string id)
        {
            var query = _db.TuyenDuongs.Where(x => x.TuyenDuongId == Guid.Parse(id))
                .Select(x => new TuyenDuongViewModel
                {
                    TuyenDuongId = x.TuyenDuongId,
                    TenTuyenDuong = x.TenTuyenDuong,
                    BenDi = x.BenDi,
                    BenDen = x.BenDen,
                    ThoiGianKhoiHanh = x.ThoiGianKhoiHanh,
                    SoXe = x.SoXe,
                    Status = x.Status,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    STT = x.STT,
                    Xes = (from xe in _db.Xes
                           where x.SoXe.Contains(xe.XeId)
                           select new XeViewModel
                           {
                               XeId = xe.XeId,
                               MaXe = xe.MaXe,
                               SoXe = xe.SoXe
                           })
                           .ToList()

                });
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TuyenDuongViewModel>> GetAll()
        {
            var result = await _db.TuyenDuongs
                .OrderBy(x => x.CreatedDate)
                .Select(x => new TuyenDuongViewModel
                {
                    TuyenDuongId = x.TuyenDuongId,
                    TenTuyenDuong = x.TenTuyenDuong,
                    BenDi = x.BenDi,
                    BenDen = x.BenDen,
                    ThoiGianKhoiHanh = x.ThoiGianKhoiHanh,
                    SoXe = x.SoXe,
                    Status = x.Status,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    STT = x.STT,
                }).OrderBy(x => x.STT).ToListAsync();

            return result;
        }
    }
}
