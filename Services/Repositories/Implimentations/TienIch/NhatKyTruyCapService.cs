using AutoMapper;
using DLL;
using DLL.Entity.TienIch;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper;
using Services.Helper.LogHelper;
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
                            MoTaChiTiet = nktc.MoTaChiTiet.LimitLine(2),
                            DiaChiIP = nktc.DiaChiIP,
                            TenMayTinh = nktc.TenMayTinh,
                            RefFile = nktc.RefFile,
                            RefId = nktc.RefId,
                            RefType = nktc.RefType,
                            CreatedDate = nktc.CreatedDate,
                            CreatedBy = nktc.CreatedBy,
                            CreatedByUserName = u.UserName
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
            bool isAllowAdd = true;
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
                    break;
                case LoaiHanhDong.Sua:
                    entity.MoTaChiTiet = GetChanges(model.RefType, model.DuLieuCu, model.DuLieuMoi);
                    if (string.IsNullOrEmpty(entity.MoTaChiTiet))
                    {
                        isAllowAdd = false;
                    }
                    break;
                case LoaiHanhDong.Xoa:
                    break;
                default:
                    break;
            }

            if (isAllowAdd == true)
            {
                await _db.NhatKyTruyCaps.AddAsync(entity);
                await _db.SaveChangesAsync();
            }
            return true;
        }

        private string GetChanges(RefType refType, object oldEntry, object newEntry, object[] oldEntries = null, object[] newEntries = null)
        {
            List<ChangeLogModel> logs = new List<ChangeLogModel>();
            List<ChangeLogModel> logDetail = new List<ChangeLogModel>();
            bool hasDetail = false;

            if (refType == RefType.KhachHang || refType == RefType.NhanVien)
            {
                oldEntry = JsonConvert.DeserializeObject<DoiTuongViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<DoiTuongViewModel>(newEntry.ToString());
            }
            if (refType == RefType.DonViTinh)
            {
                oldEntry = JsonConvert.DeserializeObject<DonViTinhViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<DonViTinhViewModel>(newEntry.ToString());
            }
            if (refType == RefType.HangHoaDichVu)
            {
                oldEntry = JsonConvert.DeserializeObject<HangHoaDichVuViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<HangHoaDichVuViewModel>(newEntry.ToString());
            }
            if (refType == RefType.LoaiTien)
            {
                oldEntry = JsonConvert.DeserializeObject<LoaiTienViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<LoaiTienViewModel>(newEntry.ToString());
            }
            if (refType == RefType.HinhThucThanhToan)
            {
                oldEntry = JsonConvert.DeserializeObject<HinhThucThanhToanViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<HinhThucThanhToanViewModel>(newEntry.ToString());
            }
            if (refType == RefType.HoSoHoaDonDienTu)
            {
                oldEntry = JsonConvert.DeserializeObject<HoSoHDDTViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<HoSoHDDTViewModel>(newEntry.ToString());
            }
            if (refType == RefType.MauHoaDon)
            {
                oldEntry = JsonConvert.DeserializeObject<MauHoaDonViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<MauHoaDonViewModel>(newEntry.ToString());
            }

            if (oldEntries != null || newEntries != null)
            {
                hasDetail = true;
            }

            var oldType = oldEntry.GetType();
            var newType = newEntry.GetType();
            if (oldType != newType)
            {
                return null; //Types don't match, cannot log changes
            }

            var oldProperties = oldType.GetProperties();
            var newProperties = newType.GetProperties();

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

                if (matchingProperty.PropertyType.IsEnum)
                {
                    if (matchingProperty.PropertyType == typeof(ThueGTGT))
                    {
                        oldValue = ((ThueGTGT)Enum.Parse(typeof(ThueGTGT), oldValue)).GetDescription();
                        newValue = ((ThueGTGT)Enum.Parse(typeof(ThueGTGT), newValue)).GetDescription();
                    }
                }
                if (Attribute.IsDefined(matchingProperty, typeof(CurrencyAttribute)))
                {
                    if (string.IsNullOrEmpty(oldValue))
                    {
                        oldValue = "0";
                    }
                    if (string.IsNullOrEmpty(newValue))
                    {
                        newValue = "0";
                    }

                    oldValue = decimal.Parse(oldValue).ToString("N0");
                    newValue = decimal.Parse(newValue).ToString("N0");
                }
                if (Attribute.IsDefined(matchingProperty, typeof(PercentAttribute)))
                {
                    oldValue = $"{oldValue}%";
                    newValue = $"{newValue}%";
                }
                if (Attribute.IsDefined(matchingProperty, typeof(CheckBoxAttribute)))
                {
                    oldValue = bool.Parse(oldValue) == true ? "Có" : "Không";
                    newValue = bool.Parse(newValue) == true ? "Có" : "Không";
                }

                if (matchingProperty != null && oldValue != newValue)
                {
                    DisplayAttribute displayNameAttr = matchingProperty.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

                    logs.Add(new ChangeLogModel()
                    {
                        PropertyName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name,
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                }
            }

            string result = string.Empty;
            if (logs.Any())
            {
                if (hasDetail == true)
                {
                    result += "1. Thông tin chung:";
                }

                foreach (var item in logs)
                {
                    result += $"- {item.PropertyName}: Từ <{item.OldValue}> thành <{item.NewValue}>\n";
                }

                result = result.Trim();
            }

            return result;
        }
    }
}
