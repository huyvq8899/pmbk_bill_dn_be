using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class QuyetDinhApDungHoaDonService : IQuyetDinhApDungHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public QuyetDinhApDungHoaDonService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            bool result = await _db.QuyetDinhApDungHoaDons
               .AnyAsync(x => x.SoQuyetDinh.ToUpper() == model.SoQuyetDinh.ToUpper());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.QuyetDinhApDungHoaDons.FirstOrDefaultAsync(x => x.QuyetDinhApDungHoaDonId == id);
            _db.QuyetDinhApDungHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<PagedList<QuyetDinhApDungHoaDonViewModel>> GetAllPagingAsync(QuyetDinhApDungHoaDonParams @params)
        {
            var query = from qd in _db.QuyetDinhApDungHoaDons
                        join u in _db.Users on qd.CreatedBy equals u.UserId
                        orderby qd.NgayQuyetDinh descending
                        select new QuyetDinhApDungHoaDonViewModel
                        {
                            NgayQuyetDinh = qd.NgayQuyetDinh,
                            SoQuyetDinh = qd.SoQuyetDinh,
                            NgayHieuLuc = qd.NgayHieuLuc,
                            NguoiTao = u.UserName
                        };

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.NgayQuyetDinhFilter))
                {
                    var keyword = @params.Filter.NgayQuyetDinhFilter.ToUpper().ToTrim();
                    query = query.Where(x => x.NgayQuyetDinhFilter.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.SoQuyetDinh))
                {
                    var keyword = @params.Filter.SoQuyetDinh.ToUpper().ToTrim();
                    query = query.Where(x => x.SoQuyetDinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.NgayHieuLucFilter))
                {
                    var keyword = @params.Filter.NgayHieuLucFilter.ToUpper().ToTrim();
                    query = query.Where(x => x.NgayHieuLucFilter.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.NguoiTao))
                {
                    var keyword = @params.Filter.NguoiTao.ToUpper().ToTrim();
                    query = query.Where(x => x.NguoiTao.ToUpper().ToTrim().Contains(keyword));
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.NgayQuyetDinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayQuyetDinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayQuyetDinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.SoQuyetDinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoQuyetDinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoQuyetDinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.NgayHieuLuc))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayHieuLuc);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayHieuLuc);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.NguoiTao))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NguoiTao);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NguoiTao);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<QuyetDinhApDungHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<QuyetDinhApDungHoaDonViewModel> GetByIdAsync(string id)
        {
            var query = from qd in _db.QuyetDinhApDungHoaDons
                        where qd.QuyetDinhApDungHoaDonId == id
                        select new QuyetDinhApDungHoaDonViewModel
                        {
                            QuyetDinhApDungHoaDonId = qd.QuyetDinhApDungHoaDonId,
                            NguoiDaiDienPhapLuat = qd.NguoiDaiDienPhapLuat,
                            ChucDanh = qd.ChucDanh,
                            SoQuyetDinh = qd.SoQuyetDinh,
                            NgayQuyetDinh = qd.NgayQuyetDinh,
                            CanCuDeBanHanhQuyetDinh = qd.CanCuDeBanHanhQuyetDinh,
                            HasMayTinh = qd.HasMayTinh,
                            HasMayIn = qd.HasMayIn,
                            HasChungTuSo = qd.HasChungTuSo,
                            Dieu3 = qd.Dieu3,
                            Dieu4 = qd.Dieu4,
                            Dieu5 = qd.Dieu5,
                            NgayHieuLuc = qd.NgayHieuLuc,
                            CoQuanThue = qd.CoQuanThue,
                            QuyetDinhApDungHoaDonDieu1s = (from qdd1 in _db.QuyetDinhApDungHoaDonDieu1s
                                                           where qdd1.QuyetDinhApDungHoaDonId == qd.QuyetDinhApDungHoaDonId
                                                           orderby qdd1.CreatedDate
                                                           select new QuyetDinhApDungHoaDonDieu1ViewModel
                                                           {
                                                               QuyetDinhApDungHoaDonDieu1Id = qdd1.QuyetDinhApDungHoaDonDieu1Id,
                                                               QuyetDinhApDungHoaDonId = qdd1.QuyetDinhApDungHoaDonId,
                                                               Ten = qdd1.Ten,
                                                               GiaTri = qdd1.GiaTri,
                                                               Checked = qdd1.Checked
                                                           })
                                                           .ToList(),
                            QuyetDinhApDungHoaDonDieu2s = (from qdd2 in _db.QuyetDinhApDungHoaDonDieu2s
                                                           where qdd2.QuyetDinhApDungHoaDonId == qd.QuyetDinhApDungHoaDonId
                                                           orderby qdd2.CreatedDate
                                                           select new QuyetDinhApDungHoaDonDieu2ViewModel
                                                           {
                                                               QuyetDinhApDungHoaDonDieu2Id = qdd2.QuyetDinhApDungHoaDonDieu2Id,
                                                               QuyetDinhApDungHoaDonId = qdd2.QuyetDinhApDungHoaDonId,
                                                               MauHoaDonId = qdd2.MauHoaDonId,
                                                               MucDichSuDung = qdd2.MucDichSuDung
                                                           })
                                                           .ToList()
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<QuyetDinhApDungHoaDonViewModel> InsertAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            var entity = _mp.Map<QuyetDinhApDungHoaDon>(model);
            await _db.QuyetDinhApDungHoaDons.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<QuyetDinhApDungHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            var entity = await _db.QuyetDinhApDungHoaDons.FirstOrDefaultAsync(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
