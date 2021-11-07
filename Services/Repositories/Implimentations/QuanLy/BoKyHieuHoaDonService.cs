using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.QuanLy;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels.QuanLy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLy
{
    public class BoKyHieuHoaDonService : IBoKyHieuHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public BoKyHieuHoaDonService(
            Datacontext dataContext,
            IMapper mp)
        {
            _db = dataContext;
            _mp = mp;
        }

        public async Task<bool> CheckTrungKyHieuAsync(BoKyHieuHoaDonViewModel model)
        {
            bool result = await _db.BoKyHieuHoaDons.AnyAsync(x => x.KyHieu == model.KyHieu);
            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == id);
            _db.BoKyHieuHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<BoKyHieuHoaDonViewModel>> GetAllAsync()
        {
            var result = await _db.BoKyHieuHoaDons
                .Select(x => new BoKyHieuHoaDonViewModel
                {
                    BoKyHieuHoaDonId = x.BoKyHieuHoaDonId,
                    KyHieu = x.KyHieu,
                    KyHieuMauSoHoaDon = x.KyHieuMauSoHoaDon,
                    KyHieuHoaDon = x.KyHieuHoaDon
                })
                .OrderBy(x => x.KyHieu)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<BoKyHieuHoaDonViewModel>> GetAllPagingAsync(BoKyHieuHoaDonParams @params)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                        join dkun in _db.DangKyUyNhiems on bkhhd.DangKyUyNhiemId equals dkun.Id
                        join tdc in _db.ThongDiepChungs on dkun.IdToKhai equals tdc.ThongDiepChungId
                        orderby bkhhd.KyHieu
                        select new BoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            KyHieu = bkhhd.KyHieu,
                            UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                            MauHoaDon = new MauHoaDon
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                Ten = mhd.Ten
                            },
                            ThoiDiemChapNhan = tdc.NgayThongBao,
                            ModifyDate = bkhhd.ModifyDate,
                            SoBatDau = bkhhd.SoBatDau,
                            SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                            SoToiDa = bkhhd.SoToiDa,
                            TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                            TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription()
                        };

            if (@params.KyHieus.Any())
            {
                query = query.Where(x => @params.KyHieus.Contains(x.KyHieu));
            }

            if (@params.TrangThaiSuDungs.Any())
            {
                query = query.Where(x => @params.TrangThaiSuDungs.Contains(x.TrangThaiSuDung));
            }

            if (@params.UyNhiemLapHoaDon != 0)
            {
                query = query.Where(x => x.UyNhiemLapHoaDon == @params.UyNhiemLapHoaDon);
            }

            return await PagedList<BoKyHieuHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<BoKyHieuHoaDonViewModel> GetByIdAsync(string id)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        where bkhhd.BoKyHieuHoaDonId == id
                        select new BoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                            UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                            LoaiHoaDon = bkhhd.LoaiHoaDon,
                            KyHieu = bkhhd.KyHieu,
                            KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                            KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                            KyHieu1 = bkhhd.KyHieu1,
                            KyHieu23 = bkhhd.KyHieu23,
                            KyHieu4 = bkhhd.KyHieu4,
                            KyHieu56 = bkhhd.KyHieu56,
                            SoBatDau = bkhhd.SoBatDau,
                            SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                            SoToiDa = bkhhd.SoToiDa,
                            TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                            IsTuyChinh = bkhhd.IsTuyChinh,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            DangKyUyNhiemId = bkhhd.DangKyUyNhiemId,
                            CreatedBy = bkhhd.CreatedBy,
                            CreatedDate = bkhhd.CreatedDate,
                            Status = bkhhd.Status
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public async Task<BoKyHieuHoaDonViewModel> InsertAsync(BoKyHieuHoaDonViewModel model)
        {
            var entity = _mp.Map<BoKyHieuHoaDon>(model);
            await _db.BoKyHieuHoaDons.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<BoKyHieuHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(BoKyHieuHoaDonViewModel model)
        {
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
