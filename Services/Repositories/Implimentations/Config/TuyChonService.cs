﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.BaoCao;
using DLL.Entity.Config;
using DLL.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.BaoCao;
using Services.ViewModels.Config;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Config
{
    public class TuyChonService : ITuyChonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public TuyChonService(
            Datacontext datacontext,
            IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<List<ConfigNoiDungEmailViewModel>> GetAllNoiDungEmail()
        {
            var result = await _db.ConfigNoiDungEmails
                .ProjectTo<ConfigNoiDungEmailViewModel>(_mp.ConfigurationProvider)
                .ToListAsync();

             return result;
        }

        public async Task<bool> LayLaiThietLapEmailMacDinh(int LoaiEmail)
        {
            var noiDungEmailCurrent = await _db.ConfigNoiDungEmails.FirstOrDefaultAsync(x => x.LoaiEmail == LoaiEmail && x.IsDefault == false);
            var noiDungEmailDefault = await _db.ConfigNoiDungEmails.FirstOrDefaultAsync(x => x.LoaiEmail == LoaiEmail && x.IsDefault == true);

            noiDungEmailCurrent.NoiDungEmail = noiDungEmailDefault.NoiDungEmail;

            _db.ConfigNoiDungEmails.Update(noiDungEmailCurrent);
            return await _db.SaveChangesAsync() > 0;
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
            var entities_notExist = entities.Where(x => !_db.TuyChons.AsNoTracking().Select(o => o.Ma).ToList().Contains(x.Ma)).ToList();
            var entities_Exist = entities.Where(x => _db.TuyChons.AsNoTracking().Select(o => o.Ma).ToList().Contains(x.Ma)).ToList();

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

        public async Task<List<TruongDuLieuViewModel>> GetThongTinHienThiTruongDL(string tenChucNang)
        {
            var result = _mp.Map<List<TruongDuLieuViewModel>>(await _db.TruongDuLieus
                                                            .Include(x => x.NghiepVu)
                                                            .Where(x => x.NghiepVu.TenNghiepVu == tenChucNang)
                                                            .OrderBy(x => x.STT)
                                                            .ToListAsync()
                                                            );
            return result;
        }

        public async Task<bool> UpdateHienThiTruongDuLieu(List<TruongDuLieuViewModel> datas)
        {
            var entities = _mp.Map<List<TruongDuLieu>>(datas);
            _db.TruongDuLieus.UpdateRange(entities);
            return await _db.SaveChangesAsync() > 0;
        }

        //public async Task<List<TruongDuLieuHoaDonViewModel>> GetThongTinHienThiTruongDLHoaDon(bool isChiTiet, int LoaiHoaDon)
        //{
        //    var result = new List<TruongDuLieuHoaDonViewModel>();
        //    if (isChiTiet == true)
        //    {
        //        result = _mp.Map<List<TruongDuLieuHoaDonViewModel>>(await _db.TruongDuLieuHoaDons
        //                                                        .Where(x => x.IsChiTiet == isChiTiet && x.LoaiHoaDon == LoaiHoaDon)
        //                                                        .OrderBy(x => x.STT)
        //                                                        .ToListAsync()
        //                                                    );
        //    }
        //    else
        //    {
        //        result = _mp.Map<List<TruongDuLieuHoaDonViewModel>>(await _db.TruongDuLieuHoaDons
        //                                                        .Where(x => x.IsChiTiet == isChiTiet && (x.LoaiHoaDon == LoaiHoaDon || x.LoaiHoaDon == 0))
        //                                                        .OrderBy(x => x.STT)
        //                                                        .ToListAsync()
        //                                                    );
        //    }

        //    foreach (var item in result)
        //    {
        //        if (item.IsLeft && item.Status)
        //        {
        //            item.Left = 50 + result.Where(x => x.Status && x.STT < item.STT && x.STT == x.DefaultSTT)
        //                              .Sum(x => x.Size);
        //        }
        //    }
        //    return result;
        //}

        //public async Task<List<TruongDuLieuHoaDonViewModel>> GetThongTinHienThiTruongDLHoaDon(bool isChiTiet)
        //{

        //    var result = _mp.Map<List<TruongDuLieuHoaDonViewModel>>(await _db.TruongDuLieuHoaDons
        //                                                        .Where(x => x.IsChiTiet == isChiTiet)
        //                                                        .OrderBy(x => x.STT)
        //                                                        .ToListAsync()
        //                                                        );
        //    foreach (var item in result)
        //    {
        //        if (item.IsLeft && item.Status)
        //        {
        //            item.Left = 50 + result.Where(x => x.Status && x.STT < item.STT && x.STT == x.DefaultSTT && x.LoaiHoaDon == 0)
        //                              .Sum(x => x.Size);
        //        }
        //    }
        //    return result;
        //    return null;
        //}

        //public async Task<bool> UpdateHienThiTruongDuLieuHoaDon(List<TruongDuLieuHoaDonViewModel> datas)
        //{
        //    try
        //    {
        //        var entities = _mp.Map<List<TruongDuLieuHoaDon>>(datas);
        //        _db.TruongDuLieuHoaDons.UpdateRange(entities);
        //        return await _db.SaveChangesAsync() > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(ex.Message);
        //    }

        //    return false;
        //}

        public async Task<List<ThietLapTruongDuLieuViewModel>> GetThongTinHienThiTruongDLMoRong(int loaiHoaDon)
        {
            var result = _mp.Map<List<ThietLapTruongDuLieuViewModel>>(await _db.ThietLapTruongDuLieus
                                                            .Where(x => x.LoaiHoaDon == (LoaiHoaDon)loaiHoaDon)
                                                            .OrderBy(x => x.STT)
                                                            .ToListAsync()
                                                            );
            return result;
        }

        public async Task<bool> UpdateThietLapTruongDuLieuMoRong(List<ThietLapTruongDuLieuViewModel> datas)
        {
            var entities = _mp.Map<List<ThietLapTruongDuLieu>>(datas);
            _db.ThietLapTruongDuLieus.UpdateRange(entities);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckCoPhatSinhNgoaiTeAsync()
        {
            var result = await (from hddt in _db.HoaDonDienTus
                                join lt in _db.LoaiTiens on hddt.LoaiTienId equals lt.LoaiTienId
                                where lt.Ma != "VND"
                                select hddt.HoaDonDienTuId).AnyAsync();

            return result;
        }

        public async Task<bool> UpdateLoaiChuKi(TuyChonViewModel model)
        {
            var result = await _db.TuyChons.Where(x => x.Ma == "IsSelectChuKiCung").FirstOrDefaultAsync();
            result.GiaTri = model.GiaTri;
            _db.TuyChons.Update(result);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> GetTypeLoaiChuKi()
        {
            var result = await _db.TuyChons.Where(x => x.Ma == "IsSelectChuKiCung").FirstOrDefaultAsync();
            return result != null ? result.GiaTri == "KiCung" : true;
        }

        /// <summary>
        /// Get list tùy chọn theo hóa đơn
        /// </summary>
        /// <param name="hoaDonId"></param>
        /// <returns></returns>
        public async Task<List<TuyChonViewModel>> GetListByHoaDonIdAsync(string hoaDonId)
        {
            var result = await (from tc in _db.TuyChons
                                join tchd in _db.TuyChonHoaDons.Where(x => x.RefId == hoaDonId)
                                on tc.Ma equals tchd.Ma into tmpTuyChonHoaDons
                                from tchd in tmpTuyChonHoaDons.DefaultIfEmpty()
                                select new TuyChonViewModel
                                {
                                    Ma = tchd != null ? tchd.Ma : tc.Ma,
                                    GiaTri = tchd != null ? tchd.GiaTri : tc.GiaTri,
                                })
                                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get list tùy chọn theo danh sách hóa đơn id
        /// </summary>
        /// <param name="hoaDonIds"></param>
        /// <returns></returns>
        public async Task<List<TuyChonViewModel>> GetListByHoaDonIdsAsync(List<string> hoaDonIds)
        {
            var tuyChonHoaDons = await _db.TuyChonHoaDons
                .Where(x => hoaDonIds.Contains(x.RefId))
                .AsNoTracking()
                .ToListAsync();

            var tuyChons = await GetAllAsync();

            var result = new List<TuyChonViewModel>();

            foreach (var id in hoaDonIds)
            {
                foreach (var tuyChon in tuyChons)
                {
                    var tuyChonHoaDon = tuyChonHoaDons.FirstOrDefault(x => x.RefId == id && x.Ma == tuyChon.Ma);

                    result.Add(new TuyChonViewModel
                    {
                        Ma = tuyChon.Ma,
                        GiaTri = tuyChonHoaDon != null ? tuyChonHoaDon.GiaTri : tuyChon.GiaTri,
                        RefId = id
                    });
                }
            }

            return result;
        }
    }
}
