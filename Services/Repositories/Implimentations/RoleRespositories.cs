using AutoMapper;
using DLL;
using DLL.Entity;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations
{
    public class RoleRespositories : IRoleRespositories
    {
        private readonly Datacontext db;
        private readonly IMapper mp;
        public RoleRespositories(Datacontext datacontext, IMapper mapper)
        {
            this.db = datacontext;
            this.mp = mapper;
        }

        public async Task<bool> CheckPhatSinh(string roleID)
        {
            return await db.User_Roles.AnyAsync(x => x.RoleId == roleID);
        }

        public async Task<int> CheckTrungMaWithObjectInput(RoleViewModel role)
        {
            return await db.Roles.CountAsync(x => x.RoleName.ToUpper() == role.RoleName.ToUpper());
        }

        public async Task<int> Delete(string Id)
        {
            try
            {
                var entity = await db.Roles.FindAsync(Id);
                db.Roles.Remove(entity);
                var rs = await db.SaveChangesAsync();
                return rs;
            }
            catch (DbUpdateException)
            {
                return -1; // khong xoa duoc khoa ngoai
            }
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var entity = await db.Roles.OrderBy(x => x.RoleName).ToListAsync();
            var model = mp.Map<List<RoleViewModel>>(entity);
            return model;
        }

        public async Task<PagedList<RoleViewModel>> GetAllPaging(PagingParams pagingParams)
        {
            var query = from r in db.Roles
                        select new RoleViewModel
                        {
                            RoleId = r.RoleId,
                            RoleName = r.RoleName ?? string.Empty,
                            CreatedDate = r.CreatedDate,
                            CreatedBy = r.CreatedBy ?? string.Empty,
                            ModifyDate = r.ModifyDate,
                            Status = r.Status
                        };

            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                var keyword = pagingParams.Keyword.ToUpper().ToTrim();
                query = query.Where(x => x.RoleName.ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        x.RoleName.ToUpper().Contains(keyword)
                                        );
            }
            if (!string.IsNullOrEmpty(pagingParams.SortValue) && !pagingParams.SortValue.Equals("null") && !pagingParams.SortValue.Equals("undefined"))
            {
                switch (pagingParams.SortKey)
                {
                    case "roleName":
                        if (pagingParams.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.RoleName);
                        }
                        else
                        {
                            query = query.OrderBy(x => x.RoleName);
                        }
                        break;
                    default:
                        break;
                }
            }
            return await PagedList<RoleViewModel>.CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<RoleViewModel> GetById(string Id)
        {
            var entity = await db.Roles.FindAsync(Id);
            return mp.Map<RoleViewModel>(entity);
        }

        public async Task<List<MauHoaDonViewModel>> GetListHoaDonDaPhanQuyen(string RoleId)
        {
            var query = from mhd in db.MauHoaDons
                        join tbphct in db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                        join tbph in db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                        where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                        group mhd by new { mhd.LoaiHoaDon, mhd.MauSo, mhd.KyHieu } into g
                        select new MauHoaDonViewModel
                        {
                            LoaiHoaDon = g.Key.LoaiHoaDon,
                            MauHoaDonId = g.First().MauHoaDonId,
                            TenLoaiHoaDon = g.Key.LoaiHoaDon.GetDescription(),
                            MauSo = g.Key.MauSo,
                            KyHieu = g.Key.KyHieu,
                        };

            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                item.Active = await db.PhanQuyenMauHoaDons.AnyAsync(x => x.BoKyHieuHoaDonId == item.MauHoaDonId && x.RoleId == RoleId);
            }

            return result;
        }

        public async Task<List<BoKyHieuHoaDonViewModel>> GetListBoKyHieuHoaDonDaPhanQuyen(string RoleId)
        {
            try
            {
                var query = from bkh in db.BoKyHieuHoaDons
                            select new BoKyHieuHoaDonViewModel
                            {
                                TrangThaiSuDung = bkh.TrangThaiSuDung,
                                TenTrangThaiSuDung = bkh.TrangThaiSuDung.GetDescription(),
                                KyHieuMauSoHoaDon = bkh.KyHieuMauSoHoaDon,
                                KyHieuHoaDon = bkh.KyHieuHoaDon,
                                KyHieu23 = bkh.KyHieu23,
                                UyNhiemLapHoaDon = bkh.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkh.UyNhiemLapHoaDon.GetDescription(),
                                TenMauHoaDon = db.MauHoaDons.FirstOrDefault(x => x.MauHoaDonId == bkh.MauHoaDonId).Ten,
                                BoKyHieuHoaDonId = bkh.BoKyHieuHoaDonId
                            };

                var result = await query.OrderByDescending(x => x.KyHieu23).ToListAsync();
                foreach (var item in result)
                {
                    item.Actived = await db.PhanQuyenMauHoaDons.AnyAsync(x => x.BoKyHieuHoaDonId == item.BoKyHieuHoaDonId && x.RoleId == RoleId);
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<RoleViewModel> Insert(RoleViewModel model)
        {
            model.RoleId = Guid.NewGuid().ToString();
            model.CreatedDate = DateTime.Now;
            model.ModifyDate = model.CreatedDate;
            var entity = mp.Map<Role>(model);
            await db.Roles.AddAsync(entity);
            var rs = await db.SaveChangesAsync();
            if (rs > 0)
            {
                var role = new SqlParameter("@RoleId", model.RoleId);
                this.db.Database.ExecuteSqlCommand("exec exec_afterThemVaiTro @RoleId", role);
                return mp.Map<RoleViewModel>(entity);
            }
            return null;
        }

        public async Task<bool> PhanQuyenMauHoaDon(List<PhanQuyenMauHoaDonViewModel> listPQ, string RoleId)
        {
            var lstPQ = await db.PhanQuyenMauHoaDons.Where(x => x.RoleId == RoleId).ToListAsync();
            db.RemoveRange(lstPQ);
            if (await db.SaveChangesAsync() == lstPQ.Count)
            {
                var entities = mp.Map<List<PhanQuyenMauHoaDon>>(listPQ);
                db.PhanQuyenMauHoaDons.AddRange(entities);
                return await db.SaveChangesAsync() == listPQ.Count;
            }

            return false;
        }

        public async Task<int> Update(RoleViewModel model)
        {
            var entity = mp.Map<Role>(model);
            entity.ModifyDate = DateTime.Now;
            db.Roles.Update(entity);
            var rs = await db.SaveChangesAsync();
            return rs;
        }
    }
}
