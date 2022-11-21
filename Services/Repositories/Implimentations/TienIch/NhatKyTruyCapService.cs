using AutoMapper;
using DLL;
using DLL.Entity.TienIch;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.Enums;
using Services.Helper;
using Services.Helper.LogHelper;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.TienIch
{
    public class NhatKyTruyCapService : INhatKyTruyCapService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _accessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public NhatKyTruyCapService(
            Datacontext datacontext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _accessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _hoSoHDDTService = hoSoHDDTService;
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

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrWhiteSpace(timKiemTheo.HanhDong))
                {
                    var keyword = timKiemTheo.HanhDong.ToUpper().ToTrim();
                    query = query.Where(x => x.HanhDong != null && x.HanhDong.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.TenMayTinh))
                {
                    var keyword = timKiemTheo.TenMayTinh.ToUpper().ToTrim();
                    query = query.Where(x => x.TenMayTinh != null && x.TenMayTinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.DiaChiIP))
                {
                    var keyword = timKiemTheo.DiaChiIP.ToUpper().ToTrim();
                    query = query.Where(x => x.DiaChiIP != null && x.DiaChiIP.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.NguoiThucHien))
                {
                    var keyword = timKiemTheo.NguoiThucHien.ToUpper().ToTrim();
                    query = query.Where(x => x.CreatedByUserName != null && x.CreatedByUserName.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(timKiemTheo.ThamChieu))
                {
                    var keyword = timKiemTheo.ThamChieu.ToUpper().ToTrim();
                    query = query.Where(x => x.ThamChieu != null && x.ThamChieu.ToUpper().ToTrim().Contains(keyword));
                }
            }
            else
            {
                //nếu nhập vào giá trị bất kỳ mà ko tích chọn loại tìm kiếm
                if (string.IsNullOrWhiteSpace(@params.TimKiemBatKy) == false)
                {
                    @params.TimKiemBatKy = @params.TimKiemBatKy.ToUpper().ToTrim();
                    query = query.Where(x =>
                        (x.HanhDong != null && x.HanhDong.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.DiaChiIP != null && x.DiaChiIP.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.TenMayTinh != null && x.TenMayTinh.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.CreatedByUserName != null && x.CreatedByUserName.ToUpper().ToTrim().Contains(@params.TimKiemBatKy)) ||
                        (x.ThamChieu != null && x.ThamChieu.ToUpper().ToTrim().Contains(@params.TimKiemBatKy))
                    );
                }
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
                DoiTuongThaoTac = !string.IsNullOrEmpty(model.DoiTuongThaoTac) ? (model.DoiTuongThaoTac == "empty" ? string.Empty : model.DoiTuongThaoTac) : model.RefType.GetDescription(),
                HanhDong = !string.IsNullOrEmpty(model.HanhDong) ? model.HanhDong : model.LoaiHanhDong.GetDescription(),
                ThamChieu = model.ThamChieu,
                MoTaChiTiet = model.MoTaChiTiet,
                DiaChiIP = GetIpAddressOfClient(),
                TenMayTinh = "",
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
                    if (model.DuLieuCu == null || model.DuLieuMoi == null)
                    {
                        break;
                    }

                    object[] oldEntries = null;
                    object[] newEntries = null;
                    if (model.RefType == RefType.HoaDonDienTu)
                    {
                        oldEntries = model.DuLieuChiTietCu;
                        newEntries = model.DuLieuChiTietMoi;
                    }

                    entity.MoTaChiTiet = GetChanges(model.RefType, model.DuLieuCu, model.DuLieuMoi, oldEntries, newEntries);
                    if (string.IsNullOrEmpty(entity.MoTaChiTiet))
                    {
                        isAllowAdd = false;
                    }

                    if (model.RefType == RefType.ThongBaoKetQuaHuyHoaDon)
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
            bool isVND = true;
            bool isHoaDonDienTu = false;

            switch (refType)
            {
                case RefType.KhachHang:
                case RefType.NhanVien:
                    oldEntry = JsonConvert.DeserializeObject<DoiTuongViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<DoiTuongViewModel>(newEntry.ToString());
                    break;
                case RefType.DonViTinh:
                    oldEntry = JsonConvert.DeserializeObject<DonViTinhViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<DonViTinhViewModel>(newEntry.ToString());
                    break;
                case RefType.HangHoaDichVu:
                    oldEntry = JsonConvert.DeserializeObject<HangHoaDichVuViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<HangHoaDichVuViewModel>(newEntry.ToString());
                    break;
                case RefType.LoaiTien:
                    oldEntry = JsonConvert.DeserializeObject<LoaiTienViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<LoaiTienViewModel>(newEntry.ToString());
                    break;
                case RefType.HinhThucThanhToan:
                    oldEntry = JsonConvert.DeserializeObject<HinhThucThanhToanViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<HinhThucThanhToanViewModel>(newEntry.ToString());
                    break;
                case RefType.HoSoHoaDonDienTu:
                    oldEntry = JsonConvert.DeserializeObject<HoSoHDDTViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<HoSoHDDTViewModel>(newEntry.ToString());
                    break;
                case RefType.MauHoaDon:
                    oldEntry = JsonConvert.DeserializeObject<MauHoaDonViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<MauHoaDonViewModel>(newEntry.ToString());
                    break;
                case RefType.HoaDonDienTu:
                    hasDetail = true;
                    isHoaDonDienTu = true;
                    oldEntry = JsonConvert.DeserializeObject<HoaDonDienTuViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<HoaDonDienTuViewModel>(newEntry.ToString());
                    isVND = ((HoaDonDienTuViewModel)newEntry).IsVND.Value;
                    break;
                case RefType.HoaDonXoaBo:
                    break;
                case RefType.HoaDonThayThe:
                    break;
                case RefType.HoaDonDieuChinh:
                    break;
                case RefType.BienBanDieuChinh:
                    oldEntry = JsonConvert.DeserializeObject<BienBanDieuChinhViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<BienBanDieuChinhViewModel>(newEntry.ToString());
                    break;
                case RefType.BienBanXoaBo:
                    oldEntry = JsonConvert.DeserializeObject<BienBanXoaBoViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<BienBanXoaBoViewModel>(newEntry.ToString());
                    break;
                case RefType.NguoiDung:
                    oldEntry = JsonConvert.DeserializeObject<UserViewModel>(oldEntry.ToString());
                    newEntry = JsonConvert.DeserializeObject<UserViewModel>(newEntry.ToString());
                    break;
                default:
                    break;
            }

            if (oldEntries != null || newEntries != null)
            {
                hasDetail = true;

                switch (refType)
                {
                    case RefType.HoaDonDienTu:
                        oldEntries = ((HoaDonDienTuViewModel)oldEntry).HoaDonChiTiets.ToArray();
                        newEntries = ((HoaDonDienTuViewModel)newEntry).HoaDonChiTiets.ToArray();
                        break;
                    default:
                        break;
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

                if (Attribute.IsDefined(matchingProperty, typeof(SpecialAttribute)))
                {
                    if (refType == RefType.MauHoaDon)
                    {
                        var oldModel = (MauHoaDonViewModel)oldEntry;
                        var newModel = (MauHoaDonViewModel)newEntry;

                        string specialValue = GetChangesMauHoaDonChiTiet(matchingProperty, logs, oldModel, newModel);
                        if (!string.IsNullOrEmpty(specialValue))
                        {
                            logs.Add(new ChangeLogModel()
                            {
                                SpecialValue = specialValue
                            });
                        }

                        logs = logs.Where(x => !string.IsNullOrEmpty(x.SpecialValue)).ToList();
                    }
                }
                else
                {
                    if (matchingProperty != null && oldValue != newValue)
                    {
                        DisplayAttribute displayNameAttr = (DisplayAttribute)matchingProperty.GetCustomAttribute(typeof(DisplayAttribute));
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        if (isHoaDonDienTu)
                        {
                            LogHoaDonDienTu logHDDT = GetChangesHoaDonDienTu(matchingProperty, isVND);
                            if (logHDDT.IsAllowContinue)
                            {
                                continue;
                            }
                            else
                            {
                                displayName = logHDDT.DisplayName;
                            }
                        }

                        logs.Add(new ChangeLogModel()
                        {
                            PropertyName = displayName,
                            OldValue = oldValue,
                            NewValue = newValue
                        });
                    }
                }
            }

            string result = string.Empty;
            var logDetails = new List<ChangeLogModel>();
            if (hasDetail && oldEntries != null && newEntries != null)
            {
                logDetails = GetChangesArray(oldEntries, newEntries, isVND, isHoaDonDienTu);
            }

            if (logs.Any() || logDetails.Any())
            {
                if (hasDetail == true && logs.Any())
                {
                    result += "1. Thông tin chung:\n";
                }

                foreach (var item in logs)
                {
                    if (!string.IsNullOrEmpty(item.SpecialValue))
                    {
                        result += $"{item.SpecialValue}\n";
                    }
                    else
                    {
                        result += $"- {item.PropertyName}: Từ <{item.OldValue}> thành <{item.NewValue}>\n";
                    }
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

        private string GetChangesMauHoaDonChiTiet(PropertyInfo matchingProperty, List<ChangeLogModel> logs, MauHoaDonViewModel oldModel, MauHoaDonViewModel newModel)
        {
            List<string> listResult = new List<string>();

            if (matchingProperty.Name == nameof(oldModel.MauHoaDonThietLapMacDinhs))
            {
                List<string> listThayDoiThietLapChung = new List<string>();
                List<string> listThayDoiThietLapHinhNen = new List<string>();

                if (logs.Any())
                {
                    listThayDoiThietLapChung.Add(string.Join("\n", logs.Select(x => $"- {x.PropertyName}: từ <{x.OldValue}> thành <{x.NewValue}>")));
                }

                var oldListThietLapChung = oldModel.MauHoaDonThietLapMacDinhs.Where(x => x.Loai >= LoaiThietLapMacDinh.Logo && x.Loai <= LoaiThietLapMacDinh.SoDongTrang).OrderBy(x => x.Loai).ToList();
                var newListThietLapChung = newModel.MauHoaDonThietLapMacDinhs.Where(x => x.Loai >= LoaiThietLapMacDinh.Logo && x.Loai <= LoaiThietLapMacDinh.SoDongTrang).OrderBy(x => x.Loai).ToList();

                string jsonOldListThietLapChung = JsonConvert.SerializeObject(oldListThietLapChung);
                string jsonNewListThietLapChung = JsonConvert.SerializeObject(newListThietLapChung);

                if (jsonOldListThietLapChung != jsonNewListThietLapChung)
                {
                    int length = oldListThietLapChung.Count;
                    for (int i = 0; i < length; i++)
                    {
                        var oldItem = oldListThietLapChung[i];
                        var newItem = newListThietLapChung[i];
                        string resultThietLapChung = "";

                        if (oldItem.GiaTri != newItem.GiaTri || oldItem.GiaTriBoSung != newItem.GiaTriBoSung)
                        {
                            string tenLoai = oldItem.Loai.GetDescription();

                            switch (oldItem.Loai)
                            {
                                case LoaiThietLapMacDinh.Logo:
                                    if (string.IsNullOrEmpty(oldItem.GiaTri) && !string.IsNullOrEmpty(newItem.GiaTri))
                                    {
                                        resultThietLapChung = $"- {tenLoai}: thêm";
                                    }
                                    else if (!string.IsNullOrEmpty(oldItem.GiaTri) && string.IsNullOrEmpty(newItem.GiaTri))
                                    {
                                        resultThietLapChung = $"- {tenLoai}: xóa";
                                    }
                                    else
                                    {
                                        resultThietLapChung = $"- {tenLoai}: sửa";
                                    }
                                    break;
                                case LoaiThietLapMacDinh.KieuChu:
                                case LoaiThietLapMacDinh.MauChu:
                                case LoaiThietLapMacDinh.SoDongTrang:
                                    resultThietLapChung = $"- {tenLoai}: từ <{oldItem.GiaTri}> thành <{newItem.GiaTri}>";
                                    break;
                                case LoaiThietLapMacDinh.CoChu:
                                    resultThietLapChung = $"- {tenLoai}: {(int.Parse(oldItem.GiaTri) < int.Parse(newItem.GiaTri) ? "tăng" : "giảm")}";
                                    break;
                                case LoaiThietLapMacDinh.HienThiQRCode:
                                case LoaiThietLapMacDinh.LapLaiThongTinKhiHoaDonCoNhieuTrang:
                                case LoaiThietLapMacDinh.ThietLapDongKyHieuCot:
                                    var oldValue = bool.Parse(oldItem.GiaTri) ? "Có" : "Không";
                                    var newValue = bool.Parse(newItem.GiaTri) ? "Có" : "Không";

                                    resultThietLapChung = $"- {tenLoai}: từ <{oldValue}> thành <{newValue}>";
                                    break;
                                default:
                                    break;
                            }

                            listThayDoiThietLapChung.Add(resultThietLapChung);
                        }
                    }
                }
                if (listThayDoiThietLapChung.Any())
                {
                    var result = "2. Thiết lập chung:\n" + string.Join("\n", listThayDoiThietLapChung);
                    listResult.Add(result);
                }

                var oldListThietLapHinhNen = oldModel.MauHoaDonThietLapMacDinhs.Where(x => x.Loai > LoaiThietLapMacDinh.SoDongTrang).OrderBy(x => x.Loai).ToList();
                var newListThietLapNen = newModel.MauHoaDonThietLapMacDinhs.Where(x => x.Loai > LoaiThietLapMacDinh.SoDongTrang).OrderBy(x => x.Loai).ToList();

                string jsonOldListThietLapHinhNen = JsonConvert.SerializeObject(oldListThietLapHinhNen);
                string jsonNewListThietLapHinhNen = JsonConvert.SerializeObject(newListThietLapNen);

                if (jsonOldListThietLapHinhNen != jsonNewListThietLapHinhNen)
                {
                    int length = oldListThietLapHinhNen.Count;
                    for (int i = 0; i < length; i++)
                    {
                        var oldItem = oldListThietLapHinhNen[i];
                        var newItem = newListThietLapNen[i];
                        string resultThietLapHinhNen = "";

                        if (oldItem.GiaTri != newItem.GiaTri || oldItem.GiaTriBoSung != newItem.GiaTriBoSung)
                        {
                            string tenLoai = oldItem.Loai.GetDescription();

                            switch (oldItem.Loai)
                            {
                                case LoaiThietLapMacDinh.HinhNenMacDinh:
                                case LoaiThietLapMacDinh.HinhNenTaiLen:
                                case LoaiThietLapMacDinh.KhungVienMacDinh:
                                    if (string.IsNullOrEmpty(oldItem.GiaTri) && !string.IsNullOrEmpty(newItem.GiaTri))
                                    {
                                        resultThietLapHinhNen = $"- {tenLoai}: thêm";
                                    }
                                    else if (!string.IsNullOrEmpty(oldItem.GiaTri) && string.IsNullOrEmpty(newItem.GiaTri))
                                    {
                                        resultThietLapHinhNen = $"- {tenLoai}: xóa";
                                    }
                                    else
                                    {
                                        resultThietLapHinhNen = $"- {tenLoai}: sửa";
                                    }
                                    break;
                                default:
                                    break;
                            }

                            listThayDoiThietLapHinhNen.Add(resultThietLapHinhNen);
                        }
                    }
                }
                if (listThayDoiThietLapHinhNen.Any())
                {
                    var result = "3. Thiết lập hình nền:\n" + string.Join("\n", listThayDoiThietLapHinhNen);
                    listResult.Add(result);
                }
            }

            if (matchingProperty.Name == nameof(oldModel.MauHoaDonTuyChinhChiTiets))
            {
                var oldList = oldModel.MauHoaDonTuyChinhChiTiets.Select(x => new MauHoaDonTuyChinhChiTietViewModel
                {
                    GiaTri = x.GiaTri,
                    TuyChinhChiTiet = x.TuyChinhChiTiet,
                    TuyChonChiTiet = x.TuyChonChiTiet,
                    TenTiengAnh = x.TenTiengAnh,
                    GiaTriMacDinh = x.GiaTriMacDinh,
                    DoRong = x.DoRong,
                    KieuDuLieuThietLap = x.KieuDuLieuThietLap,
                    Loai = x.Loai,
                    LoaiChiTiet = x.LoaiChiTiet,
                    LoaiContainer = x.LoaiContainer,
                    IsParent = x.IsParent,
                    Checked = x.Checked,
                    Disabled = x.Disabled,
                    CustomKey = x.CustomKey
                }).OrderBy(x => x.LoaiChiTiet).ToList();

                var newList = newModel.MauHoaDonTuyChinhChiTiets.Select(x => new MauHoaDonTuyChinhChiTietViewModel
                {
                    GiaTri = x.GiaTri,
                    TuyChinhChiTiet = x.TuyChinhChiTiet,
                    TuyChonChiTiet = x.TuyChonChiTiet,
                    TenTiengAnh = x.TenTiengAnh,
                    GiaTriMacDinh = x.GiaTriMacDinh,
                    DoRong = x.DoRong,
                    KieuDuLieuThietLap = x.KieuDuLieuThietLap,
                    Loai = x.Loai,
                    LoaiChiTiet = x.LoaiChiTiet,
                    LoaiContainer = x.LoaiContainer,
                    IsParent = x.IsParent,
                    Checked = x.Checked,
                    Disabled = x.Disabled,
                    CustomKey = x.CustomKey
                }).OrderBy(x => x.LoaiChiTiet).ToList();

                string jsonOldList = JsonConvert.SerializeObject(oldList);
                string jsonNewList = JsonConvert.SerializeObject(newList);

                if (jsonOldList != jsonNewList)
                {
                    var result = "4. Tùy chỉnh chi tiết: thay đổi";
                    listResult.Add(result);
                }
            }

            return string.Join("\n", listResult.ToArray());
        }

        public List<ChangeLogModel> GetChangesArray(object[] oldEntries, object[] newEntries, bool isVND, bool isHoaDonDienTu)
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
                var displayDetailKey = (DisplayAttribute)propDetaiKey.GetCustomAttribute(typeof(DisplayAttribute));
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
                var displayDetailKey = (DisplayAttribute)propDetaiKey.GetCustomAttribute(typeof(DisplayAttribute));
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
                        DisplayAttribute displayNameAttr = (DisplayAttribute)matchingProperty.GetCustomAttribute(typeof(DisplayAttribute));
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        if (isHoaDonDienTu)
                        {
                            LogHoaDonDienTu logHDDT = GetChangesHoaDonDienTu(matchingProperty, isVND);
                            if (logHDDT.IsAllowContinue)
                            {
                                continue;
                            }
                            else
                            {
                                displayName = logHDDT.DisplayName;
                            }
                        }

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
                        DisplayAttribute displayNameAttr = (DisplayAttribute)matchingProperty.GetCustomAttribute(typeof(DisplayAttribute));
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        if (isHoaDonDienTu)
                        {
                            LogHoaDonDienTu logHDDT = GetChangesHoaDonDienTu(matchingProperty, isVND);
                            if (logHDDT.IsAllowContinue)
                            {
                                continue;
                            }
                            else
                            {
                                displayName = logHDDT.DisplayName;
                            }
                        }

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
                        DisplayAttribute displayNameAttr = (DisplayAttribute)matchingProperty.GetCustomAttribute(typeof(DisplayAttribute));
                        string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;

                        if (isHoaDonDienTu)
                        {
                            LogHoaDonDienTu logHDDT = GetChangesHoaDonDienTu(matchingProperty, isVND);
                            if (logHDDT.IsAllowContinue)
                            {
                                continue;
                            }
                            else
                            {
                                displayName = logHDDT.DisplayName;
                            }
                        }

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

        /// <summary>
        /// get change value of tuy chon
        /// </summary>
        /// <param name="oldEntries"></param>
        /// <param name="newEntries"></param>
        /// <returns></returns>
        //public List<ChangeLogModel> GetChangeTuyChons(object[] oldEntries, object[] newEntries)
        //{
        //    // list result
        //    List<ChangeLogModel> logs = new List<ChangeLogModel>();

        //    // get type of oldList and newList
        //    var oldType = oldEntries.GetType();
        //    var newType = newEntries.GetType();
        //    if (oldType != newType)
        //    {
        //        return null; //Types don't match, cannot log changes
        //    }

        //    List<ChangeLogDetails> oldList = new List<ChangeLogDetails>();
        //    List<ChangeLogDetails> newList = new List<ChangeLogDetails>();

        //    for (int i = 0; i < oldEntries.Length; i++)
        //    {
        //        var properties = oldEntries[i].GetType().GetProperties();
        //        var primaryKey = properties.Where(x => Attribute.IsDefined(x, typeof(LoggingPrimaryKeyAttribute))).First().GetValue(oldEntries[i]) ?? string.Empty;
        //        oldList.Add(new ChangeLogDetails
        //        {
        //            Id = primaryKey.ToString(),
        //            Entry = oldEntries[i],
        //            Properties = properties
        //        });
        //    }

        //    for (int i = 0; i < newEntries.Length; i++)
        //    {
        //        var properties = newEntries[i].GetType().GetProperties();
        //        var primaryKey = properties.Where(x => Attribute.IsDefined(x, typeof(LoggingPrimaryKeyAttribute))).First().GetValue(newEntries[i]) ?? string.Empty;
        //        newList.Add(new ChangeLogDetails
        //        {
        //            Id = primaryKey.ToString(),
        //            Entry = newEntries[i],
        //            Properties = properties
        //        });
        //    }

        //    int counOfList = oldList.Count;
        //    for (int i = 0; i < counOfList; i++)
        //    {
        //        var oldEntry = oldList[i];
        //        var newEntry = newList[i];

        //        var oldProperties = oldEntry.Properties;
        //        var newProperties = newEntry.Properties;

        //        var maTuyChon = string.Empty;
        //        var titleTuyChon = string.Empty;

        //        foreach (var oldProperty in oldProperties)
        //        {
        //            var matchingProperty = newProperties.Where(x => !Attribute.IsDefined(x, typeof(IgnoreLoggingAttribute))
        //                                                            && x.Name == oldProperty.Name
        //                                                            && x.PropertyType == oldProperty.PropertyType)
        //                                                .FirstOrDefault();

        //            if (matchingProperty == null)
        //            {
        //                continue;
        //            }

        //            if (Attribute.IsDefined(matchingProperty, typeof(LoggingPrimaryKeyAttribute)))
        //            {
        //                maTuyChon = (oldProperty.GetValue(oldEntry.Entry) ?? string.Empty).ToString();
        //            }
        //            else if (Attribute.IsDefined(matchingProperty, typeof(LabelAttribute)))
        //            {
        //                titleTuyChon = (oldProperty.GetValue(oldEntry.Entry) ?? string.Empty).ToString();
        //            }
        //            else
        //            {
        //                var oldValue = (oldProperty.GetValue(oldEntry.Entry) ?? string.Empty).ToString();
        //                var newValue = (matchingProperty.GetValue(newEntry.Entry) ?? string.Empty).ToString();

        //                // convert value of db to view nhat ky
        //                switch (oldValue)
        //                {
        //                    case "true":
        //                    case "false":
        //                        oldValue = oldValue == "true" ? "Có" : "Không";
        //                        newValue = newValue == "true" ? "Có" : "Không";
        //                        break;
        //                    case "CanhBaoKhiNhapMaSoThueKhongHopLe":
        //                        oldValue = oldValue == "KhongCB" ? "Không cảnh báo" : (oldValue == "CB" ? "Cảnh báo" : "Cảnh báo và không lưu");
        //                        newValue = newValue == "KhongCB" ? "Không cảnh báo" : (newValue == "CB" ? "Cảnh báo" : "Cảnh báo và không lưu");
        //                        break;
        //                    case "IntCanhBaoKhiKhongLapVBDTTT":
        //                        break;
        //                    default:
        //                        break;
        //                }

        //                if (matchingProperty != null && oldValue != newValue)
        //                {
        //                    logs.Add(new ChangeLogModel()
        //                    {
        //                        PropertyName = titleTuyChon,
        //                        OldValue = oldValue,
        //                        NewValue = newValue
        //                    });
        //                }
        //                break;
        //            }
        //        }
        //    }

        //    return logs;
        //}

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
                if (matchingProperty.PropertyType == typeof(KyKeKhaiThue))
                {
                    oldValue = ((KyKeKhaiThue)Enum.Parse(typeof(KyKeKhaiThue), oldValue)).GetDescription();
                    newValue = ((KyKeKhaiThue)Enum.Parse(typeof(KyKeKhaiThue), newValue)).GetDescription();
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
            if (Attribute.IsDefined(matchingProperty, typeof(DisplayStatusAttribute)))
            {
                if (string.IsNullOrEmpty(oldValue))
                {
                    oldValue = "false";
                }

                if (string.IsNullOrEmpty(newValue))
                {
                    newValue = "false";
                }

                oldValue = bool.Parse(oldValue) == true ? "hiện" : "ẩn";
                newValue = bool.Parse(newValue) == true ? "hiện" : "ẩn";
            }

            logValue.OldValue = oldValue;
            logValue.NewValue = newValue;
        }

        private LogHoaDonDienTu GetChangesHoaDonDienTu(PropertyInfo matchingProperty, bool isVND)
        {
            HoaDonDienTuViewModel model;
            HoaDonDienTuChiTietViewModel detail;
            LogHoaDonDienTu logHoaDonDienTu = new LogHoaDonDienTu();
            DisplayAttribute displayNameAttr = (DisplayAttribute)matchingProperty.GetCustomAttribute(typeof(DisplayAttribute));
            string displayName = displayNameAttr != null ? displayNameAttr.Name : matchingProperty.Name;
            string propertyName = matchingProperty.Name;

            if (isVND)
            {
                if (propertyName == nameof(model.TongTienHang) ||
                    propertyName == nameof(model.TongTienChietKhau) ||
                    propertyName == nameof(model.TongTienThueGTGT) ||
                    propertyName == nameof(model.TongTienThanhToan) ||
                    propertyName == nameof(detail.ThanhTien) ||
                    propertyName == nameof(detail.ThanhTienSauThue) ||
                    propertyName == nameof(detail.TienChietKhau) ||
                    propertyName == nameof(detail.TienThueGTGT))
                {
                    logHoaDonDienTu.IsAllowContinue = true;
                }
                else
                {
                    logHoaDonDienTu.DisplayName = displayName;
                }
            }
            else
            {
                if (propertyName.Contains("QuyDoi"))
                {
                    logHoaDonDienTu.DisplayName = displayName + " quy đổi";
                }
                else
                {
                    logHoaDonDienTu.DisplayName = displayName;
                }
            }

            return logHoaDonDienTu;
        }

        public async Task<FileReturn> ExportExcelAsync(NhatKyTruyCapParams @params)
        {
            @params.PageSize = -1;
            PagedList<NhatKyTruyCapViewModel> paged = await GetAllPagingAsync(@params);
            List<NhatKyTruyCapViewModel> list = paged.Items;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/NhatKyTruyCap/DANH_SACH_NHAT_KY_TRUY_CAP.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = list.Count;
                int begin_row = 7;

                worksheet.Cells[1, 1].Value = hoSoHDDTVM.TenDonVi;
                worksheet.Cells[2, 1].Value = hoSoHDDTVM.DiaChi;
                // Add Row
                if (totalRows != 0)
                {
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                }
                // Fill data
                int idx = begin_row + (totalRows == 0 ? 1 : 0);
                foreach (var _it in list)
                {
                    worksheet.Cells[idx, 1].Value = _it.CreatedByUserName;
                    worksheet.Cells[idx, 2].Value = _it.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells[idx, 3].Value = _it.DoiTuongThaoTac;
                    worksheet.Cells[idx, 4].Value = _it.HanhDong;
                    worksheet.Cells[idx, 5].Value = _it.ThamChieu;
                    worksheet.Cells[idx, 6].Value = _it.MoTaChiTiet;
                    worksheet.Cells[idx, 7].Value = _it.DiaChiIP;

                    idx += 1;
                }

                worksheet.Cells[idx, 1].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"DANH_SACH_NHAT_KY_TRUY_CAP_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string filePath = Path.Combine(destPath, fileName);
                package.SaveAs(new FileInfo(filePath));
                byte[] fileByte;

                if (@params.IsExportPDF == true)
                {
                    string pdfPath = Path.Combine(destPath, $"print-{DateTime.Now:yyyyMMddHHmmss}.pdf");
                    FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, filePath, pdfPath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    fileByte = File.ReadAllBytes(pdfPath);
                    filePath = pdfPath;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    fileByte = File.ReadAllBytes(filePath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                return new FileReturn
                {
                    Bytes = fileByte,
                    ContentType = MimeTypes.GetMimeType(filePath),
                    FileName = Path.GetFileName(filePath)
                };
            }
        }

        public string GetIpAddressOfClient()
        {
            var ipAddress = string.Empty;
            IPAddress ip = _accessor.HttpContext.Connection.RemoteIpAddress;

            if (ip != null)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ip = Dns.GetHostEntry(ip).AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
                }

                ipAddress = ip?.ToString();
            }

            return ipAddress;
        }

        public class LogValue
        {
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }

        public class LogHoaDonDienTu
        {
            public string DisplayName { get; set; }
            public bool IsAllowContinue { get; set; }
        }

        public async Task<List<NhatKyTruyCapViewModel>> GetByRefIdAsync(string id)
        {

            var bbHuyId = (from bbxb in _db.BienBanXoaBos where bbxb.HoaDonDienTuId == id select bbxb.Id).FirstOrDefault();
            var bbdcId = (from bbdc in _db.BienBanDieuChinhs where bbdc.HoaDonBiDieuChinhId == id select bbdc.BienBanDieuChinhId).FirstOrDefault();
            var query = from nktc in _db.NhatKyTruyCaps
                        join u in _db.Users on nktc.CreatedBy equals u.UserId
                        where nktc.RefId == id || (nktc.RefId == bbHuyId && bbHuyId != null) || (nktc.RefId == bbdcId && bbdcId != null)
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

            return await query.ToListAsync();
        }

        public async Task<bool> InsertRangeAsync(bool hasSaveChanges, List<NhatKyTruyCapViewModel> models)
        {
            bool isAllowAdd = true;
            List<NhatKyTruyCap> entities = new List<NhatKyTruyCap>();

            foreach (var model in models)
            {
                NhatKyTruyCap entity = new NhatKyTruyCap
                {
                    DoiTuongThaoTac = !string.IsNullOrEmpty(model.DoiTuongThaoTac) ? (model.DoiTuongThaoTac == "empty" ? string.Empty : model.DoiTuongThaoTac) : model.RefType.GetDescription(),
                    HanhDong = !string.IsNullOrEmpty(model.HanhDong) ? model.HanhDong : model.LoaiHanhDong.GetDescription(),
                    ThamChieu = model.ThamChieu,
                    MoTaChiTiet = model.MoTaChiTiet,
                    DiaChiIP = GetIpAddressOfClient(),
                    TenMayTinh = "",
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
                        if (model.DuLieuCu == null || model.DuLieuMoi == null)
                        {
                            break;
                        }

                        object[] oldEntries = null;
                        object[] newEntries = null;
                        if (model.RefType == RefType.HoaDonDienTu)
                        {
                            oldEntries = model.DuLieuChiTietCu;
                            newEntries = model.DuLieuChiTietMoi;
                        }

                        entity.MoTaChiTiet = GetChanges(model.RefType, model.DuLieuCu, model.DuLieuMoi, oldEntries, newEntries);
                        if (string.IsNullOrEmpty(entity.MoTaChiTiet))
                        {
                            isAllowAdd = false;
                        }

                        if (model.RefType == RefType.ThongBaoKetQuaHuyHoaDon)
                        {
                            entity.MoTaChiTiet = null;
                        }

                        break;
                    case LoaiHanhDong.Xoa:
                        break;
                    default:
                        break;
                }

                entities.Add(entity);
            }

            if (isAllowAdd == true)
            {
                await _db.NhatKyTruyCaps.AddRangeAsync(entities);

                if (hasSaveChanges)
                {
                    await _db.SaveChangesAsync();
                }
            }
            return true;
        }
    }
}
