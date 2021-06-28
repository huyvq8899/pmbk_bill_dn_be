using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class MauHoaDonService : IMauHoaDonService
    {
        Datacontext _db;
        IMapper _mp;
        List<MauHoaDonViewModel> listMauHoaDon = new List<MauHoaDonViewModel>();
        public MauHoaDonService(Datacontext datacontext, IMapper mapper)
        {
            this._db = datacontext;
            this._mp = mapper;

            listMauHoaDon = new List<MauHoaDonViewModel>
            {
               new MauHoaDonViewModel { MauSo = "01GTKT0/001", TenMauSo = "Hóa đơn giá trị gia tăng (HĐ điện tử)", Status = true, TuNhap = false },
               new MauHoaDonViewModel { MauSo = "01GTKT3/001", TenMauSo = "Hóa đơn giá trị gia tăng", Status = true, TuNhap = false },
               new MauHoaDonViewModel { MauSo = "02GTTT0/001", TenMauSo = "Hóa đơn bán hàng (HĐ điện tử)", Status = true, TuNhap = false },
               new MauHoaDonViewModel { MauSo = "02GTTT3/001", TenMauSo = "Hóa đơn bán hàng", Status = true, TuNhap = false },
               new MauHoaDonViewModel { MauSo = "03XKNB3/001", TenMauSo = "Phiếu xuất kho kiêm vận chuyển hàng hóa nội bộ", Status = true, TuNhap = false },
               new MauHoaDonViewModel { MauSo = "04HGDL3/001", TenMauSo = "Phiếu xuất kho gửi bán hàng đại lý", Status = true, TuNhap = false },
               new MauHoaDonViewModel { MauSo = "07KPTQ3/001", TenMauSo = "Hóa đơn bán hàng (dành cho tổ chức, cá nhân trong khu phi thuế quan)", Status = true, TuNhap = false }
            };
        }

        public async Task<bool> CheckTrungMauSo(string mauSo)
        {
            var count = await _db.MauHoaDons.Where(x => x.MauSo == mauSo).CountAsync();
            return count > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == id);
            _db.MauHoaDons.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByMauSo(MauHoaDonViewModel MauSo)
        {
            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauSo.ToLower().Trim() == MauSo.MauSo.ToLower().Trim());
            _db.MauHoaDons.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<List<MauHoaDonViewModel>> GetAll()
        {
            var list = _mp.Map<List<MauHoaDonViewModel>>(await _db.MauHoaDons.OrderBy(x=>x.STT).ToListAsync());
            return list;
        }

        public async Task<List<MauHoaDonViewModel>> GetAllActive()
        {
            var listHoaDon = _mp.Map<List<MauHoaDonViewModel>>(await _db.MauHoaDons.Where(x => x.Status == true).OrderBy(x=>x.STT).ToListAsync());
            return listHoaDon;
        }

        public async Task<List<MauHoaDonViewModel>> GetAllTuyChinh()
        {
            var listHoaDon = await _db.MauHoaDons.OrderBy(x => x.STT).ToListAsync();
            foreach (var item in listHoaDon)
            {
                listMauHoaDon.Add(
                    new MauHoaDonViewModel
                    {
                        MauSo = item.MauSo,
                        TenMauSo = item.TenMauSo,
                        Status = true,
                        TuNhap = true
                    });
            }
            return listMauHoaDon;
        }

        public async Task<MauHoaDonViewModel> GetById(string id)
        {
            var entity = _mp.Map<MauHoaDonViewModel>(await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == id));
            return entity;
        }

        public async Task<bool> Insert(MauHoaDon model)
        {
            if (_db.MauHoaDons.Where(x => x.MauSo.ToLower().Trim() == model.MauSo.ToLower().Trim()).Count() > 0 ||
                listMauHoaDon.Where(x => x.MauSo.ToLower().Trim() == model.MauSo.ToLower().Trim()).Count() > 0)
            {
                return true;
            }

            if (!model.STT.HasValue)
            {
                bool any = await _db.MauHoaDons.AnyAsync();
                if (any)
                {
                    int stt = await _db.MauHoaDons.OrderBy(x=>x.STT).MaxAsync(x => x.STT ?? 0);
                    model.STT = stt + 1;
                }
                else
                {
                    model.STT = 1;
                }
            }

            model.TuNhap = true;
            await _db.MauHoaDons.AddAsync(model);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(MauHoaDon model)
        {
            _db.MauHoaDons.Update(model);
            return await _db.SaveChangesAsync() > 0;
        }
        
        public async Task<bool> CheckTrungMa(string Ma)
        {
            bool rs = await _db.MauHoaDons.AnyAsync(x => x.MauSo.ToUpper().Trim() == Ma.ToUpper().Trim());
            return rs;
        }

    }
}
