using AutoMapper;
using DLL;
using DLL.Entity.TienIch;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyTruyCapService : INhatKyTruyCapService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _accessor;

        public NhatKyTruyCapService(Datacontext datacontext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _accessor = httpContextAccessor;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedList<NhatKyTruyCapViewModel>> GetAllPagingAsync(NhatKyTruyCapParams @params)
        {
            var query = from nktc in _db.NhatKyTruyCaps
                        join u in _db.Users on nktc.CreatedBy equals u.UserId
                        orderby nktc.CreatedDate descending
                        select new NhatKyTruyCapViewModel
                        {
                            NhatKyTruyCapId = nktc.NhatKyTruyCapId,
                            DoiTuongThaoTac = nktc.DoiTuongThaoTac,
                            HanhDong = nktc.HanhDong,
                            ThamChieu = nktc.ThamChieu,
                            MoTaChiTiet = nktc.MoTaChiTiet,
                            DiaChiIP = nktc.DiaChiIP,
                            TenMayTinh = nktc.TenMayTinh,
                            RefFile = nktc.RefFile,
                            RefId = nktc.RefId,
                            RefType = nktc.RefType,
                            CreatedDate = nktc.CreatedDate,
                            CreatedBy = nktc.CreatedBy,
                            CreatedByUserName = u.UserName
                        };

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

            return await PagedList<NhatKyTruyCapViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<NhatKyTruyCapViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.NhatKyTruyCaps.AsNoTracking().FirstOrDefaultAsync(x => x.NhatKyTruyCapId == id);
            var result = _mp.Map<NhatKyTruyCapViewModel>(entity);
            return result;
        }

        public async Task<bool> InsertAsync(NhatKyTruyCapViewModel model)
        {
            NhatKyTruyCap entity = new NhatKyTruyCap
            {
                DoiTuongThaoTac = model.RefType.GetDescription(),
                HanhDong = model.LoaiHanhDong.GetDescription(),
                ThamChieu = model.ThamChieu,
                MoTaChiTiet = model.MoTaChiTiet,
                DiaChiIP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                TenMayTinh = Dns.GetHostEntry(_accessor.HttpContext.Connection.RemoteIpAddress).HostName,
                RefFile = model.RefFile,
                RefId = model.RefId,
                RefType = model.RefType,
                Status = true
            };

            switch (model.LoaiHanhDong)
            {
                case LoaiHanhDong.Them:
                    /////////////////////////////////////////
                    break;
                case LoaiHanhDong.Sua:
                    entity.MoTaChiTiet = GetChanges(model.DuLieuCu, model.DuLieuMoi, model.ClassName);
                    break;
                case LoaiHanhDong.Xoa:
                    break;
                default:
                    break;
            }

            await _db.NhatKyTruyCaps.AddAsync(entity);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        private string GetChanges(object oldEntry, object newEntry, string classname)
        {
            List<ChangeLogModel> logs = new List<ChangeLogModel>();

            if (classname == nameof(DoiTuongViewModel))
            {
                oldEntry = JsonConvert.DeserializeObject<DoiTuongViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<DoiTuongViewModel>(newEntry.ToString());
            }

            var oldType = oldEntry.GetType();
            var newType = newEntry.GetType();
            if (oldType != newType)
            {
                return null; //Types don't match, cannot log changes
            }

            var oldProperties = oldType.GetProperties();
            var newProperties = newType.GetProperties();

            var className = oldEntry.GetType().Name;

            foreach (var oldProperty in oldProperties)
            {
                var matchingProperty = newProperties.Where(x => !Attribute.IsDefined(x, typeof(IgnoreLoggingAttribute))
                                                                && x.Name == oldProperty.Name
                                                                && x.PropertyType == oldProperty.PropertyType)
                                                    .FirstOrDefault();

                if (matchingProperty == null)
                {
                    continue;
                }

                var oldValue = (oldProperty.GetValue(oldEntry) ?? string.Empty).ToString();
                var newValue = (matchingProperty.GetValue(newEntry) ?? string.Empty).ToString();
                if (matchingProperty != null && oldValue != newValue)
                {
                    DisplayAttribute displayNameAttr = matchingProperty.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

                    logs.Add(new ChangeLogModel()
                    {
                        ClassName = className,
                        PropertyName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name,
                        OldValue = oldProperty.GetValue(oldEntry).ToString(),
                        NewValue = matchingProperty.GetValue(newEntry).ToString()
                    });
                }
            }

            string result = string.Empty;
            foreach (var item in logs)
            {
                result += $"\n- {item.PropertyName}: Từ <{item.OldValue}> thành <{item.NewValue}>";
            }

            return result;
        }
    }
}
