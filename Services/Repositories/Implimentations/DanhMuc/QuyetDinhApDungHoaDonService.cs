using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class QuyetDinhApDungHoaDonService : IQuyetDinhApDungHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuyetDinhApDungHoaDonService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckTrungMaAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            bool result = await _db.QuyetDinhApDungHoaDons
               .AnyAsync(x => x.SoQuyetDinh.ToUpper() == model.SoQuyetDinh.ToUpper());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.QuyetDinhApDungHoaDon, _db);

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
                            QuyetDinhApDungHoaDonId = qd.QuyetDinhApDungHoaDonId,
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
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyetDinhApDungHoaDon);
            string folder = $@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{id}\FileAttach";

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
                            HasChungThuSo = qd.HasChungThuSo,
                            Dieu1 = qd.Dieu1,
                            Dieu2 = qd.Dieu2,
                            Dieu3 = qd.Dieu3,
                            NoiDungDieu3 = qd.NoiDungDieu3,
                            Dieu4 = qd.Dieu4,
                            NoiDungDieu4 = qd.NoiDungDieu4,
                            Dieu5 = qd.Dieu5,
                            NgayHieuLuc = qd.NgayHieuLuc,
                            CoQuanThue = qd.CoQuanThue,
                            CreatedBy = qd.CreatedBy,
                            CreatedDate = qd.CreatedDate,
                            Status = qd.Status,
                            QuyetDinhApDungHoaDonDieu1s = (from qdd1 in _db.QuyetDinhApDungHoaDonDieu1s
                                                           where qdd1.QuyetDinhApDungHoaDonId == qd.QuyetDinhApDungHoaDonId
                                                           orderby qdd1.CreatedDate
                                                           select new QuyetDinhApDungHoaDonDieu1ViewModel
                                                           {
                                                               QuyetDinhApDungHoaDonDieu1Id = qdd1.QuyetDinhApDungHoaDonDieu1Id,
                                                               QuyetDinhApDungHoaDonId = qdd1.QuyetDinhApDungHoaDonId,
                                                               Ten = qdd1.Ten,
                                                               GiaTri = qdd1.GiaTri,
                                                               Checked = qdd1.Checked,
                                                               LoaiDieu1 = qdd1.LoaiDieu1,
                                                               Disabled = qdd1.Disabled,
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
                                                               MucDichSuDung = qdd2.MucDichSuDung,
                                                           })
                                                           .ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == qd.QuyetDinhApDungHoaDonId
                                               orderby tldk.CreatedDate
                                               select new TaiLieuDinhKemViewModel
                                               {
                                                   TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                   NghiepVuId = tldk.NghiepVuId,
                                                   LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                   TenGoc = tldk.TenGoc,
                                                   TenGuid = tldk.TenGuid,
                                                   CreatedDate = tldk.CreatedDate,
                                                   Link = _httpContextAccessor.GetDomain() + Path.Combine(folder, tldk.TenGuid),
                                                   Status = tldk.Status
                                               })
                                               .ToList(),
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<QuyetDinhApDungHoaDonDieu2ViewModel>> GetListMauHoaDonByIdAsync(string id)
        {
            var query = from d2 in _db.QuyetDinhApDungHoaDonDieu2s
                        join mhd in _db.MauHoaDons on d2.MauHoaDonId equals mhd.MauHoaDonId
                        where d2.QuyetDinhApDungHoaDonId == id
                        orderby d2.CreatedDate
                        select new QuyetDinhApDungHoaDonDieu2ViewModel
                        {
                            QuyetDinhApDungHoaDonDieu2Id = d2.QuyetDinhApDungHoaDonDieu2Id,
                            QuyetDinhApDungHoaDonId = d2.QuyetDinhApDungHoaDonId,
                            MauHoaDonId = d2.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            MucDichSuDung = mhd.KyHieu
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<QuyetDinhApDungHoaDonDieu2ViewModel>> GetMauCacLoaiHoaDonAsync(string id)
        {
            var query = from mhd in _db.MauHoaDons
                        join d2 in _db.QuyetDinhApDungHoaDonDieu2s on mhd.MauHoaDonId equals d2.MauHoaDonId into tmpDieu2
                        from d2 in tmpDieu2.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(id) && d2 == null) || (!string.IsNullOrEmpty(id) && (d2 == null || d2.QuyetDinhApDungHoaDonId == id))
                        select new QuyetDinhApDungHoaDonDieu2ViewModel
                        {
                            QuyetDinhApDungHoaDonDieu2Id = d2 != null ? d2.QuyetDinhApDungHoaDonDieu2Id : null,
                            QuyetDinhApDungHoaDonId = d2 != null ? d2.QuyetDinhApDungHoaDonId : null,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            MauHoaDonId = mhd.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            MucDichSuDung = d2 != null ? d2.MucDichSuDung : "Bán hàng",
                            Status = true,
                            Checked = d2 != null
                        };

            var result = await query.OrderBy(x => x.MauSo).ToListAsync();
            return result;
        }

        public async Task<QuyetDinhApDungHoaDonViewModel> InsertAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            List<QuyetDinhApDungHoaDonDieu1ViewModel> quyetDinhApDungHoaDonDieu1s = model.QuyetDinhApDungHoaDonDieu1s;
            List<QuyetDinhApDungHoaDonDieu2ViewModel> quyetDinhApDungHoaDonDieu2s = model.QuyetDinhApDungHoaDonDieu2s;

            model.QuyetDinhApDungHoaDonDieu1s = null;
            model.QuyetDinhApDungHoaDonDieu2s = null;
            var entity = _mp.Map<QuyetDinhApDungHoaDon>(model);
            await _db.QuyetDinhApDungHoaDons.AddAsync(entity);

            foreach (var item in quyetDinhApDungHoaDonDieu1s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu1>(item);
                await _db.QuyetDinhApDungHoaDonDieu1s.AddAsync(detail);
            }

            foreach (var item in quyetDinhApDungHoaDonDieu2s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu2>(item);
                await _db.QuyetDinhApDungHoaDonDieu2s.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            var result = _mp.Map<QuyetDinhApDungHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            List<QuyetDinhApDungHoaDonDieu1ViewModel> quyetDinhApDungHoaDonDieu1s = model.QuyetDinhApDungHoaDonDieu1s;
            List<QuyetDinhApDungHoaDonDieu2ViewModel> quyetDinhApDungHoaDonDieu2s = model.QuyetDinhApDungHoaDonDieu2s;

            model.QuyetDinhApDungHoaDonDieu1s = null;
            model.QuyetDinhApDungHoaDonDieu2s = null;
            var entity = await _db.QuyetDinhApDungHoaDons.FirstOrDefaultAsync(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<QuyetDinhApDungHoaDonDieu1> removedDieu1s = await _db.QuyetDinhApDungHoaDonDieu1s.Where(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId).ToListAsync();
            List<QuyetDinhApDungHoaDonDieu2> removedDieu2s = await _db.QuyetDinhApDungHoaDonDieu2s.Where(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId).ToListAsync();
            _db.QuyetDinhApDungHoaDonDieu1s.RemoveRange(removedDieu1s);
            _db.QuyetDinhApDungHoaDonDieu2s.RemoveRange(removedDieu2s);

            foreach (var item in quyetDinhApDungHoaDonDieu1s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu1>(item);
                await _db.QuyetDinhApDungHoaDonDieu1s.AddAsync(detail);
            }

            foreach (var item in quyetDinhApDungHoaDonDieu2s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu2>(item);
                await _db.QuyetDinhApDungHoaDonDieu2s.AddAsync(detail);
            }

            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
