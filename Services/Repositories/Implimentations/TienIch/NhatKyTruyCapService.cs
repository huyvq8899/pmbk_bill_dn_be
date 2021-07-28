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
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
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
                            MoTaChiTietLimit = nktc.MoTaChiTiet.LimitLine(2),
                            IsOverLimitContent = nktc.MoTaChiTiet.IsOverLimit(2),
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
                DoiTuongThaoTac = !string.IsNullOrEmpty(model.DoiTuongThaoTac) ? model.DoiTuongThaoTac : model.RefType.GetDescription(),
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
                    object[] oldEntries = null;
                    object[] newEntries = null;
                    if (model.RefType == RefType.ThongBaoPhatHanhHoaDon || model.RefType == RefType.ThongBaoKetQuaHuyHoaDon || model.RefType == RefType.ThongBaoDieuChinhThongTinHoaDon)
                    {
                        oldEntries = model.DuLieuChiTietCu;
                        newEntries = model.DuLieuChiTietMoi;
                    }

                    entity.MoTaChiTiet = GetChanges(model.RefType, model.DuLieuCu, model.DuLieuMoi, oldEntries, newEntries);
                    if (string.IsNullOrEmpty(entity.MoTaChiTiet))
                    {
                        isAllowAdd = false;
                    }

                    if (model.RefType == RefType.MauHoaDon || model.RefType == RefType.ThongBaoKetQuaHuyHoaDon)
                    {
                        entity.MoTaChiTiet = null;
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
            if (refType == RefType.ThongBaoPhatHanhHoaDon)
            {
                oldEntry = JsonConvert.DeserializeObject<ThongBaoPhatHanhViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<ThongBaoPhatHanhViewModel>(newEntry.ToString());
            }
            if (refType == RefType.ThongBaoKetQuaHuyHoaDon)
            {
                oldEntry = JsonConvert.DeserializeObject<ThongBaoKetQuaHuyHoaDonViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<ThongBaoKetQuaHuyHoaDonViewModel>(newEntry.ToString());
            }
            if (refType == RefType.ThongBaoDieuChinhThongTinHoaDon)
            {
                oldEntry = JsonConvert.DeserializeObject<ThongBaoDieuChinhThongTinHoaDonViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<ThongBaoDieuChinhThongTinHoaDonViewModel>(newEntry.ToString());
            }
            if (refType == RefType.NguoiDung)
            {
                oldEntry = JsonConvert.DeserializeObject<UserViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<UserViewModel>(newEntry.ToString());
            }
            if (refType == RefType.HoaDonDienTu)
            {
                oldEntry = JsonConvert.DeserializeObject<HoaDonDienTuViewModel>(oldEntry.ToString());
                newEntry = JsonConvert.DeserializeObject<HoaDonDienTuViewModel>(newEntry.ToString());
            }

            if (oldEntries != null || newEntries != null)
            {
                hasDetail = true;

                if (refType == RefType.ThongBaoPhatHanhHoaDon)
                {
                    oldEntries = oldEntries?.ToList().Select(x => JsonConvert.DeserializeObject<ThongBaoPhatHanhChiTietViewModel>(x.ToString())).ToArray();
                    newEntries = newEntries?.ToList().Select(x => JsonConvert.DeserializeObject<ThongBaoPhatHanhChiTietViewModel>(x.ToString())).ToArray();
                }
                if (refType == RefType.ThongBaoKetQuaHuyHoaDon)
                {
                    oldEntries = oldEntries?.ToList().Select(x => JsonConvert.DeserializeObject<ThongBaoKetQuaHuyHoaDonChiTietViewModel>(x.ToString())).ToArray();
                    newEntries = newEntries?.ToList().Select(x => JsonConvert.DeserializeObject<ThongBaoKetQuaHuyHoaDonChiTietViewModel>(x.ToString())).ToArray();
                }
                if (refType == RefType.ThongBaoDieuChinhThongTinHoaDon)
                {
                    oldEntries = oldEntries?.ToList().Select(x => JsonConvert.DeserializeObject<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>(x.ToString())).ToArray();
                    newEntries = newEntries?.ToList().Select(x => JsonConvert.DeserializeObject<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>(x.ToString())).ToArray();
                }
                if (refType == RefType.HoaDonDienTu)
                {
                    oldEntries = oldEntries?.ToList().Select(x => JsonConvert.DeserializeObject<HoaDonDienTuChiTietViewModel>(x.ToString())).ToArray();
                    newEntries = newEntries?.ToList().Select(x => JsonConvert.DeserializeObject<HoaDonDienTuChiTietViewModel>(x.ToString())).ToArray();
                }
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

                LogValue logValue = new LogValue
                {
                    OldValue = oldValue,
                    NewValue = newValue
                };
                FormatInputValue(matchingProperty, logValue);
                oldValue = logValue.OldValue;
                newValue = logValue.NewValue;

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
            var logDetails = new List<ChangeLogModel>();
            if (hasDetail)
            {
                logDetails = GetChangesArray(oldEntries, newEntries);
            }
            if (logs.Any() || logDetails.Any())
            {
                if (hasDetail == true && logs.Any())
                {
                    result += "1. Thông tin chung:\n";
                }

                foreach (var item in logs)
                {
                    result += $"- {item.PropertyName}: Từ <{item.OldValue}> thành <{item.NewValue}>\n";
                }

                if (logDetails.Any())
                {
                    int idxDetail = logs.Any() ? 2 : 1;
                    logDetails = logDetails.GroupBy(x => x.DetailType)
                        .Select(x => new ChangeLogModel
                        {
                            DetailType = x.Key,
                            NewValue = string.Join("\n", x.Select(y => y.NewValue).ToList())
                        })
                        .OrderBy(x => x.DetailType)
                        .ToList();

                    foreach (var item in logDetails)
                    {
                        result += $"{idxDetail}. {item.DetailType.GetDescription()}\n" + item.NewValue + "\n";
                        idxDetail += 1;
                    }
                }

                result = result.Trim();
            }

            return result;
        }

        public List<ChangeLogModel> GetChangesArray(object[] oldEntries, object[] newEntries)
        {
            List<ChangeLogModel> logs = new List<ChangeLogModel>();

            var oldType = oldEntries.GetType();
            var newType = newEntries.GetType();
            if (oldType != newType)
            {
                return null; //Types don't match, cannot log changes
            }

            List<ChangeLogDetails> oldList = new List<ChangeLogDetails>();
            List<ChangeLogDetails> newList = new List<ChangeLogDetails>();

            for (int i = 0; i < oldEntries.Length; i++)
            {
                var properties = oldEntries[i].GetType().GetProperties();
                var primaryKey = properties.Where(x => Attribute.IsDefined(x, typeof(LoggingPrimaryKeyAttribute))).First().GetValue(oldEntries[i]) ?? string.Empty;
                var propDetaiKey = properties.Where(x => Attribute.IsDefined(x, typeof(DetailKeyAttribute))).First();
                var detailKey = propDetaiKey.GetValue(oldEntries[i]) ?? string.Empty;
                var displayDetailKey = propDetaiKey.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                oldList.Add(new ChangeLogDetails
                {
                    Id = primaryKey.ToString(),
                    DetailKey = (displayDetailKey != null ? displayDetailKey.Name : propDetaiKey.Name) + " " + detailKey.ToString(),
                    Entry = oldEntries[i],
                    Properties = properties
                });
            }

            for (int i = 0; i < newEntries.Length; i++)
            {
                var properties = newEntries[i].GetType().GetProperties();
                var primaryKey = properties.Where(x => Attribute.IsDefined(x, typeof(LoggingPrimaryKeyAttribute))).First().GetValue(newEntries[i]) ?? string.Empty;
                var propDetaiKey = properties.Where(x => Attribute.IsDefined(x, typeof(DetailKeyAttribute))).First();
                var detailKey = propDetaiKey.GetValue(newEntries[i]) ?? string.Empty;
                var displayDetailKey = propDetaiKey.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                newList.Add(new ChangeLogDetails
                {
                    Id = primaryKey.ToString(),
                    DetailKey = (displayDetailKey != null ? displayDetailKey.Name : propDetaiKey.Name) + " " + detailKey.ToString(),
                    Entry = newEntries[i],
                    Properties = properties
                });
            }

            List<ChangeLogDetails> removedList = oldList.Where(x => !newList.Any(y => y.Id == x.Id)).ToList();
            List<ChangeLogDetails> addedList = newList.Where(x => !oldList.Any(y => y.Id == x.Id)).ToList();
            List<ChangeLogDetails> updatedList = newList.Where(x => oldList.Any(y => y.Id == x.Id)).ToList();

            // Thêm dòng
            foreach (var added in addedList)
            {
                var newModel = added;

                List<string> listNewRow = new List<string>();
                foreach (var newProperty in newModel.Properties)
                {
                    var matchingProperty = newModel.Properties.Where(x => !Attribute.IsDefined(x, typeof(IgnoreLoggingAttribute))
                                                                    && x.Name == newProperty.Name
                                                                    && x.PropertyType == newProperty.PropertyType)
                                                        .FirstOrDefault();

                    if (matchingProperty == null)
                    {
                        continue;
                    }

                    var newValue = (matchingProperty.GetValue(newModel.Entry) ?? string.Empty).ToString();

                    LogValue logValue = new LogValue
                    {
                        OldValue = string.Empty,
                        NewValue = newValue
                    };
                    FormatInputValue(matchingProperty, logValue);
                    newValue = logValue.NewValue;

                    if (matchingProperty != null && matchingProperty.Name != "Status")
                    {
                        DisplayAttribute displayNameAttr = matchingProperty.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        listNewRow.Add($"{displayName}: {newValue}");
                    }
                }

                logs.Add(new ChangeLogModel()
                {
                    NewValue = string.Join("; ", listNewRow.ToArray()),
                    DetailType = DetailType.Create
                });
            }

            // Sửa dòng
            foreach (var updated in updatedList)
            {
                var oldModel = oldList.FirstOrDefault(x => x.Id == updated.Id);
                var newModel = updated;

                List<string> listNewRow = new List<string>();
                foreach (var oldProperty in oldModel.Properties)
                {
                    var matchingProperty = newModel.Properties.Where(x => !Attribute.IsDefined(x, typeof(IgnoreLoggingAttribute))
                                                                    && x.Name == oldProperty.Name
                                                                    && x.PropertyType == oldProperty.PropertyType)
                                                        .FirstOrDefault();

                    if (matchingProperty == null)
                    {
                        continue;
                    }

                    var oldValue = (oldProperty.GetValue(oldModel.Entry) ?? string.Empty).ToString();
                    var newValue = (matchingProperty.GetValue(newModel.Entry) ?? string.Empty).ToString();

                    LogValue logValue = new LogValue
                    {
                        OldValue = oldValue,
                        NewValue = newValue
                    };
                    FormatInputValue(matchingProperty, logValue);
                    oldValue = logValue.OldValue;
                    newValue = logValue.NewValue;

                    if (matchingProperty != null && oldValue != newValue)
                    {
                        DisplayAttribute displayNameAttr = matchingProperty.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        listNewRow.Add($"{displayName}: Từ <{oldValue}> thành <{newValue}>");
                    }
                }

                if (listNewRow.Any())
                {
                    logs.Add(new ChangeLogModel()
                    {
                        NewValue = updated.DetailKey + ": " + string.Join("; ", listNewRow.ToArray()),
                        DetailType = DetailType.Update
                    });
                }
            }

            // Xóa dòng
            foreach (var removed in removedList)
            {
                var oldModel = removed;

                List<string> listNewRow = new List<string>();
                foreach (var oldProperty in oldModel.Properties)
                {
                    var matchingProperty = oldModel.Properties.Where(x => !Attribute.IsDefined(x, typeof(IgnoreLoggingAttribute))
                                                                    && x.Name == oldProperty.Name
                                                                    && x.PropertyType == oldProperty.PropertyType)
                                                        .FirstOrDefault();

                    if (matchingProperty == null)
                    {
                        continue;
                    }

                    var newValue = (matchingProperty.GetValue(oldModel.Entry) ?? string.Empty).ToString();

                    LogValue logValue = new LogValue
                    {
                        OldValue = string.Empty,
                        NewValue = newValue
                    };
                    FormatInputValue(matchingProperty, logValue);
                    newValue = logValue.NewValue;

                    if (matchingProperty != null && matchingProperty.Name != "Status")
                    {
                        DisplayAttribute displayNameAttr = matchingProperty.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        listNewRow.Add($"{displayName}: {newValue}");
                    }
                }

                logs.Add(new ChangeLogModel()
                {
                    NewValue = string.Join("; ", listNewRow.ToArray()),
                    DetailType = DetailType.Delete
                });
            }

            return logs;
        }

        private void FormatInputValue(PropertyInfo matchingProperty, LogValue logValue)
        {
            string oldValue = logValue.OldValue;
            string newValue = logValue.NewValue;

            if (matchingProperty.PropertyType.IsEnum)
            {
                if (matchingProperty.PropertyType == typeof(ThueGTGT))
                {
                    oldValue = ((ThueGTGT)Enum.Parse(typeof(ThueGTGT), oldValue)).GetDescription();
                    newValue = ((ThueGTGT)Enum.Parse(typeof(ThueGTGT), newValue)).GetDescription();
                }
                if (matchingProperty.PropertyType == typeof(TrangThaiNop))
                {
                    oldValue = ((TrangThaiNop)Enum.Parse(typeof(TrangThaiNop), oldValue)).GetDescription();
                    newValue = ((TrangThaiNop)Enum.Parse(typeof(TrangThaiNop), newValue)).GetDescription();
                }
            }
            if (matchingProperty.PropertyType == typeof(DateTime?) || matchingProperty.PropertyType == typeof(DateTime))
            {
                oldValue = string.IsNullOrEmpty(oldValue) ? null : DateTime.Parse(oldValue).ToString("dd/MM/yyyy");
                newValue = string.IsNullOrEmpty(newValue) ? null : DateTime.Parse(newValue).ToString("dd/MM/yyyy");
            }
            if (Attribute.IsDefined(matchingProperty, typeof(OrderNumberAttribute)))
            {
                oldValue = string.IsNullOrEmpty(oldValue) ? null : int.Parse(oldValue).PadZerro();
                newValue = string.IsNullOrEmpty(newValue) ? null : int.Parse(newValue).PadZerro();
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
                oldValue = string.IsNullOrEmpty(oldValue) ? null : $"{oldValue}%";
                newValue = string.IsNullOrEmpty(newValue) ? null : $"{newValue}%";
            }
            if (Attribute.IsDefined(matchingProperty, typeof(CheckBoxAttribute)))
            {
                if (string.IsNullOrEmpty(oldValue))
                {
                    oldValue = "false";
                }

                if (string.IsNullOrEmpty(newValue))
                {
                    newValue = "false";
                }

                oldValue = bool.Parse(oldValue) == true ? "Có" : "Không";
                newValue = bool.Parse(newValue) == true ? "Có" : "Không";
            }

            logValue.OldValue = oldValue;
            logValue.NewValue = newValue;
        }

        public class LogValue
        {
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }
    }
}
