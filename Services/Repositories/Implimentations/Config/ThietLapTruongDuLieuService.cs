using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.Config;
using DLL.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Config
{
    public class ThietLapTruongDuLieuService : IThietLapTruongDuLieuService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public ThietLapTruongDuLieuService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public List<ThietLapTruongDuLieuViewModel> GetListThietLapMacDinh(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon)
        {
            ThietLapTruongDuLieu entity = new ThietLapTruongDuLieu();
            var result = _mp.Map<List<ThietLapTruongDuLieuViewModel>>(entity.InitData());
            result = result.Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon).ToList();
            return result;
        }

        public async Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongDuLieuByLoaiTruongAsync(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon)
        {
            var result = await _db.ThietLapTruongDuLieus
                .Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon)
                .OrderBy(x => x.STT)
                .ProjectTo<ThietLapTruongDuLieuViewModel>(_mp.ConfigurationProvider)
                .ToListAsync();

            //ThietLapTruongDuLieu entity = new ThietLapTruongDuLieu();
            //var result = _mp.Map<List<ThietLapTruongDuLieuViewModel>>(entity.InitData());
            //result = result.Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon).ToList();

            return result;
        }

        public async Task UpdateTruongDuLieuAsync(List<ThietLapTruongDuLieuViewModel> models)
        {
            var entities = await _db.ThietLapTruongDuLieus
                .Where(x => models.Select(y => y.ThietLapTruongDuLieuId).Contains(x.ThietLapTruongDuLieuId))
                .ToListAsync();

            foreach (var item in entities)
            {
                var model = models.FirstOrDefault(x => x.ThietLapTruongDuLieuId == item.ThietLapTruongDuLieuId);
                item.TenTruongHienThi = model.TenTruongHienThi;
                item.GhiChu = model.GhiChu;
                item.DoRong = model.DoRong;
                item.STT = model.STT;
                item.HienThi = model.HienThi;
            }

            await _db.SaveChangesAsync();
        }
    }
}
