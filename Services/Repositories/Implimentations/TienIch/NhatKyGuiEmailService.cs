using AutoMapper;
using DLL;
using ManagementServices.Helper;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using DLL.Entity.TienIch;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyGuiEmailService : INhatKyGuiEmailService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public NhatKyGuiEmailService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedList<NhatKyGuiEmailViewModel>> GetAllPagingAsync(NhatKyGuiEmailParams @params)
        {
            var query = from nk in _db.NhatKyGuiEmails
                        join u in _db.Users on nk.CreatedBy equals u.UserId
                        orderby nk.CreatedDate descending
                        select new NhatKyGuiEmailViewModel
                        {
                            NhatKyGuiEmailId = nk.NhatKyGuiEmailId,
                            MauSo = nk.MauSo,
                            KyHieu = nk.KyHieu,
                            So = nk.So,
                            Ngay = nk.Ngay,
                            TrangThaiGuiEmail = nk.TrangThaiGuiEmail,
                            TenTrangThaiGuiEmail = nk.TrangThaiGuiEmail.GetDescription(),
                            TenNguoiGui = nk.TenNguoiGui,
                            EmailGui = nk.EmailGui,
                            TenNguoiNhan = nk.TenNguoiNhan,
                            EmailNguoiNhan = nk.EmailNguoiNhan,
                            LoaiEmail = nk.LoaiEmail,
                            TenLoaiEmail = nk.LoaiEmail.GetDescription(),
                            TieuDeEmail = nk.TieuDeEmail,
                            RefId = nk.RefId,
                            RefType = nk.RefType,
                            CreatedDate = nk.CreatedDate,
                            CreatedBy = nk.CreatedBy,
                            Status = nk.Status
                        };

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                var fromDate = DateTime.Parse(@params.FromDate);
                var toDate = DateTime.Parse(@params.ToDate);
                query = query.Where(x => x.CreatedDate.Value.Date >= fromDate.Date && x.CreatedDate.Value.Date <= toDate.Date);
            }

            if (@params.Filter != null)
            {
                //if (!string.IsNullOrEmpty(@params.Filter.))
                //{
                //    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                //    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                //}
                //if (!string.IsNullOrEmpty(@params.Filter.MoTa))
                //{
                //    var keyword = @params.Filter.MoTa.ToUpper().ToTrim();
                //    query = query.Where(x => x.MoTa.ToUpper().ToTrim().Contains(keyword) || x.MoTa.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                //}
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                //if (@params.SortKey == nameof(@params.Filter.Ten))
                //{
                //    if (@params.SortValue == "ascend")
                //    {
                //        query = query.OrderBy(x => x.Ten);
                //    }
                //    if (@params.SortValue == "descend")
                //    {
                //        query = query.OrderByDescending(x => x.Ten);
                //    }
                //}
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<NhatKyGuiEmailViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public Task<NhatKyTruyCapViewModel> GetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> InsertAsync(NhatKyGuiEmailViewModel model)
        {
            var _configuration = await _db.TuyChons.AsNoTracking().ToListAsync();
            string fromMail = _configuration.Where(x => x.Ma == "TenDangNhapEmail").Select(x => x.GiaTri).FirstOrDefault();
            string fromName = _configuration.Where(x => x.Ma == "TenNguoiGui").Select(x => x.GiaTri).FirstOrDefault();

            model.EmailGui = fromMail;
            model.TenNguoiGui = fromName;
            model.Status = true;
            var entity = _mp.Map<NhatKyGuiEmail>(model);
            await _db.NhatKyGuiEmails.AddAsync(entity);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }
    }
}
