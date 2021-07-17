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
                            TenNguoiNhan = nk.TenNguoiNhan,
                            EmailNguoiNhan = nk.EmailNguoiNhan,
                            RefId = nk.RefId,
                            RefType = nk.RefType,
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

        public Task<bool> InsertAsync(NhatKyTruyCapViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
