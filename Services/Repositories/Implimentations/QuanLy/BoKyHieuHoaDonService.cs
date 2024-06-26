﻿using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using Org.BouncyCastle.Asn1.X500;
using Services.Helper;
using Services.Helper.Params.Filter;
using Services.Helper.Params.QuanLy;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLy
{
    public class BoKyHieuHoaDonService : IBoKyHieuHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITuyChonService _tuyChonService;
        private readonly IMauHoaDonService _mauHoaDonService;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public BoKyHieuHoaDonService(
            Datacontext dataContext,
            IMapper mp,
            IHttpContextAccessor httpContextAccessor,
            ITuyChonService tuyChonService,
            IMauHoaDonService mauHoaDonService,
            IHoSoHDDTService hoSoHDDTService)
        {
            _db = dataContext;
            _mp = mp;
            _httpContextAccessor = httpContextAccessor;
            _tuyChonService = tuyChonService;
            _mauHoaDonService = mauHoaDonService;
            _hoSoHDDTService = hoSoHDDTService;
        }

        /// <summary>
        /// Kiểm tra xem số lượng hóa đơn đã dùng hết chưa
        /// </summary>
        /// <param name="boKyHieuHoaDonId"></param>
        /// <param name="soHoaDon"></param>
        /// <returns></returns>
        public async Task<bool> CheckDaHetSoLuongHoaDonAsync(string boKyHieuHoaDonId, long? soHoaDon)
        {
            var result = false;
            var entity = await _db.BoKyHieuHoaDons
                .FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == boKyHieuHoaDonId);

            if (entity != null)
            {
                // nếu số lượng hiện tại ở bộ ký hiệu khác số hiện tại trên hóa đơn thì update
                if (entity.SoLonNhatDaLapDenHienTai != soHoaDon && soHoaDon > (entity.SoLonNhatDaLapDenHienTai ?? 0))
                {
                    if (soHoaDon > (entity.SoLonNhatDaLapDenHienTai ?? 0))
                    {
                        entity.SoLonNhatDaLapDenHienTai = soHoaDon;
                    }

                    if (entity.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc)
                    {
                        entity.TrangThaiSuDung = TrangThaiSuDung.DangSuDung;
                    }

                    await _db.SaveChangesAsync();
                }

                // nếu số hiện tại trên hóa đơn = số tối đa trong bộ ký hiệu thì báo đã dùng hết
                if (soHoaDon >= entity.SoToiDa)
                {
                    result = true;
                }
            }

            return result;
        }

        public BoKyHieuHoaDonViewModel CheckKyHieuOutObject(string kyHieu, List<BoKyHieuHoaDon> models)
        {
            var model = models.FirstOrDefault(x => x.KyHieu.ToUpper() == kyHieu.ToUpper());
            var result = _mp.Map<BoKyHieuHoaDonViewModel>(model);
            return result;
        }

        public CtsModel CheckSoSeriChungThu(BoKyHieuHoaDonViewModel model)
        {
            CtsModel result = new CtsModel();
            var maThongDiepGui = model.ToKhaiForBoKyHieuHoaDon.MaThongDiepGui;

            if (model.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem != null)
            {
                var cts = model.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung;
                if (!cts.Any(x => x.Seri.ToUpper() == model.SerialNumber.ToUpper()))
                {
                    result.Message = $"Chứng thư số &lt;{model.SerialNumber}&gt; không thuộc danh sách chứng thư số sử dụng tại mục 5" +
                                   $" của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                                   $"Vui lòng kiểm tra lại.";
                }
                else
                {
                    var ctsItem = cts.FirstOrDefault(x => x.Seri.ToUpper() == model.SerialNumber.ToUpper());
                    if (ctsItem.HThuc == 3)
                    {
                        result.Message = $"Chứng thư số &lt;{model.SerialNumber}&gt; có hình thức đăng ký là &lt;Ngừng sử dụng&gt; trên danh sách chứng thư số" +
                                       $" sử dụng tại mục 5 của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                                       $"Vui lòng kiểm tra lại.";
                    }
                    else
                    {
                        result = new CtsModel
                        {
                            TTChuc = ctsItem.TTChuc,
                            ThoiGianSuDungTu = DateTime.Parse(ctsItem.TNgay),
                            ThoiGianSuDungDen = DateTime.Parse(ctsItem.DNgay)
                        };
                    }
                }
            }

            if (model.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem != null)
            {
                var cts = model.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung;
                if (!cts.Any(x => x.Seri == model.SerialNumber))
                {
                    result.Message = $"Chứng thư số &lt;{model.SerialNumber}&gt; không thuộc danh sách chứng thư số sử dụng tại mục 5" +
                                   $" của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                                   $"Vui lòng kiểm tra lại.";
                }
                else
                {
                    var ctsItem = cts.FirstOrDefault(x => x.Seri == model.SerialNumber);
                    if (ctsItem.HThuc == 3)
                    {
                        result.Message = $"Chứng thư số &lt;{model.SerialNumber}&gt; có hình thức đăng ký là &lt;Ngừng sử dụng&gt; trên danh sách chứng thư số" +
                                       $" sử dụng tại mục 5 của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                                       $"Vui lòng kiểm tra lại.";
                    }
                    else
                    {
                        result = new CtsModel
                        {
                            TTChuc = ctsItem.TTChuc,
                            ThoiGianSuDungTu = DateTime.Parse(ctsItem.TNgay),
                            ThoiGianSuDungDen = DateTime.Parse(ctsItem.DNgay)
                        };
                    }
                }
            }

            return result;
        }

        public async Task<bool> CheckTrungKyHieuAsync(BoKyHieuHoaDonViewModel model)
        {
            bool result = await _db.BoKyHieuHoaDons.AnyAsync(x => x.KyHieu == model.KyHieu);
            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == id);
            _db.BoKyHieuHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<BoKyHieuHoaDonViewModel>> GetAllAsync()
        {
            var result = await _db.BoKyHieuHoaDons
                .Select(x => new BoKyHieuHoaDonViewModel
                {
                    BoKyHieuHoaDonId = x.BoKyHieuHoaDonId,
                    KyHieu = x.KyHieu,
                    KyHieuMauSoHoaDon = x.KyHieuMauSoHoaDon,
                    KyHieuHoaDon = x.KyHieuHoaDon
                })
                .OrderBy(x => x.KyHieu)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<BoKyHieuHoaDonViewModel>> GetAllPagingAsync(BoKyHieuHoaDonParams @params)
        {
            var query = (from bkhhd in _db.BoKyHieuHoaDons
                         join tdc in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdc.ThongDiepChungId
                         where @params.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)
                         select new BoKyHieuHoaDonViewModel
                         {
                             BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                             KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                             KyHieu = bkhhd.KyHieu,
                             UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                             TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                             HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                             LoaiHoaDon = bkhhd.LoaiHoaDon,
                             MauHoaDons = (from mhd in _db.MauHoaDons
                                           where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                           select new MauHoaDonViewModel
                                           {
                                               MauHoaDonId = mhd.MauHoaDonId,
                                               Ten = mhd.Ten,
                                               NgayKy = mhd.NgayKy,
                                               TenLoaiThueGTGT = mhd.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT ? mhd.LoaiThueGTGT.GetDescription() : string.Empty
                                           })
                                           .ToList(),
                             ThongDiepChung = new ThongDiepChungViewModel
                             {
                                 ThongDiepChungId = tdc.ThongDiepChungId,
                                 MaThongDiep = tdc.MaThongDiep,
                                 TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                                 TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                                 NgayThongBao = tdc.NgayThongBao,
                             },
                             ThoiDiemChapNhan = (from nk in _db.NhatKyXacThucBoKyHieus
                                                 where nk.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId && nk.ThoiDiemChapNhan.HasValue
                                                 orderby nk.CreatedDate
                                                 select nk.ThoiDiemChapNhan).FirstOrDefault() ?? tdc.NgayThongBao,
                             ModifyDate = bkhhd.ModifyDate,
                             SoBatDau = bkhhd.SoBatDau,
                             SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                             SoToiDa = bkhhd.SoToiDa,
                             TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                             TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription(),
                             PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL,
                             TenPhuongThucTruyenDuLieu = bkhhd.PhuongThucChuyenDL.GetDescription(),
                             MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan,
                             SoBatDauCMCQT = bkhhd.SoBatDauCMCQT,
                         })
                         .OrderByDescending(x => x.KyHieu23Int)
                         .ThenByDescending(x => x.ThoiDiemChapNhan)
                         .ThenByDescending(x => x.ModifyDate.Value.Date)
                         .AsQueryable();

            if (@params.KyHieus.Any() && !@params.KyHieus.Any(x => x == null))
            {
                query = query.Where(x => @params.KyHieus.Contains(x.BoKyHieuHoaDonId));
            }

            if (@params.TrangThaiSuDungs.Any() && !@params.TrangThaiSuDungs.Any(x => x == TrangThaiSuDung.TatCa))
            {
                query = query.Where(x => @params.TrangThaiSuDungs.Contains(x.TrangThaiSuDung));
            }

            if (@params.UyNhiemLapHoaDon != UyNhiemLapHoaDon.TatCa)
            {
                query = query.Where(x => x.UyNhiemLapHoaDon == @params.UyNhiemLapHoaDon);
            }
            if (@params.LoaiHoaDons.Any() && !@params.LoaiHoaDons.Any(x => x == LoaiHoaDon.TatCa))
            {
                query = query.Where(x => @params.LoaiHoaDons.Contains(x.LoaiHoaDon));
            }
            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenTrangThaiSuDung))
                {
                    var keyword = timKiemTheo.TenTrangThaiSuDung.ToUpper().ToTrim();
                    query = query.Where(x => x.TenTrangThaiSuDung.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenUyNhiemLapHoaDon))
                {
                    var keyword = timKiemTheo.TenUyNhiemLapHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenUyNhiemLapHoaDon.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenMauHoaDon))
                {
                    var keyword = timKiemTheo.TenMauHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.MauHoaDons.Select(y => y.Ten.ToUpper()).Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NgayCapNhatFilter))
                {
                    var keyword = timKiemTheo.NgayCapNhatFilter.ToTrim();
                    query = query.Where(x => x.ModifyDate.HasValue && x.ModifyDate.Value.ToString("dd/MM/yyyy").Contains(keyword));
                }
            }

            // filter each col in table
            if (@params.FilterColumns != null && @params.FilterColumns.Any())
            {
                @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                foreach (var filterCol in @params.FilterColumns)
                {
                    switch (filterCol.ColKey)
                    {
                        case nameof(@params.Filter.TrangThaiSuDung):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.TenTrangThaiSuDung, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.KyHieu):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.KyHieu, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.UyNhiemLapHoaDon):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.TenUyNhiemLapHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.PhuongThucChuyenDL):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.TenPhuongThucTruyenDuLieu, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.TenMauHoaDon):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.TenMauHoaDon, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.MaThongDiep):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.ThongDiepChung.MaThongDiep, filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.ModifyDate):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.ModifyDate.Value.ToString("dd/MM/yyyy"), filterCol, FilterValueType.String);
                            break;
                        case nameof(@params.Filter.SoBatDau):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.SoBatDau, filterCol, FilterValueType.Decimal);
                            break;
                        case nameof(@params.Filter.SoLonNhatDaLapDenHienTai):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.SoLonNhatDaLapDenHienTai, filterCol, FilterValueType.Decimal);
                            break;
                        case nameof(@params.Filter.SoToiDa):
                            query = GenericFilterColumn<BoKyHieuHoaDonViewModel>.Query(query, x => x.SoToiDa, filterCol, FilterValueType.Decimal);
                            break;
                    }
                }
            }

            // sort each col in table
            if (!string.IsNullOrEmpty(@params.SortKey) && !string.IsNullOrEmpty(@params.SortValue))
            {
                switch (@params.SortKey)
                {
                    case nameof(@params.Filter.TrangThaiSuDung):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenTrangThaiSuDung);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenTrangThaiSuDung);
                        }
                        break;
                    case nameof(@params.Filter.KyHieu):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.KyHieu);
                        }
                        if (@params.SortValue == "descend")
                        {
                            query = query.OrderByDescending(x => x.KyHieu);
                        }
                        break;
                    case nameof(@params.Filter.UyNhiemLapHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenUyNhiemLapHoaDon);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenUyNhiemLapHoaDon);
                        }
                        break;
                    case nameof(@params.Filter.PhuongThucChuyenDL):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.TenPhuongThucTruyenDuLieu);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.TenPhuongThucTruyenDuLieu);
                        }
                        break;
                    case nameof(@params.Filter.TenMauHoaDon):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.MauHoaDon.Ten);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.MauHoaDon.Ten);
                        }
                        break;
                    case nameof(@params.Filter.MaThongDiep):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.ThongDiepChung.MaThongDiep);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ThongDiepChung.MaThongDiep);
                        }
                        break;
                    case nameof(@params.Filter.ModifyDate):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.ModifyDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.ModifyDate);
                        }
                        break;
                    case nameof(@params.Filter.SoBatDau):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.SoBatDau);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SoBatDau);
                        }
                        break;
                    case nameof(@params.Filter.SoLonNhatDaLapDenHienTai):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.SoLonNhatDaLapDenHienTai);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SoLonNhatDaLapDenHienTai);
                        }
                        break;
                    case nameof(@params.Filter.SoToiDa):
                        if (@params.SortValue == "ascend")
                        {
                            query = query.OrderBy(x => x.SoToiDa);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.SoToiDa);
                        }
                        break;
                    default:
                        break;
                }
            }

            return await PagedList<BoKyHieuHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<BoKyHieuHoaDonViewModel> GetByIdAsync(string id)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                        join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                        join tdn in _db.ThongDiepChungs on tdg.MaThongDiep equals tdn.MaThongDiepThamChieu into tmpThongDiepNhans
                        from tdn in tmpThongDiepNhans.DefaultIfEmpty()
                        join tdgmt in _db.ThongDiepChungs on bkhhd.ThongDiepMoiNhatId equals tdgmt.ThongDiepChungId
                        join tkmt in _db.ToKhaiDangKyThongTins on tdgmt.IdThamChieu equals tkmt.Id
                        where bkhhd.BoKyHieuHoaDonId == id
                        select new BoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                            UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                            LoaiHoaDon = bkhhd.LoaiHoaDon,
                            KyHieu = bkhhd.KyHieu,
                            KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                            KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                            KyHieu1 = bkhhd.KyHieu1,
                            KyHieu23 = bkhhd.KyHieu23,
                            KyHieu4 = bkhhd.KyHieu4,
                            KyHieu56 = bkhhd.KyHieu56,
                            SoBatDau = bkhhd.SoBatDau,
                            SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                            SoToiDa = bkhhd.SoToiDa,
                            TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                            TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription(),
                            MaSoThueBenUyNhiem = bkhhd.MaSoThueBenUyNhiem,
                            IsTuyChinh = bkhhd.IsTuyChinh,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            ThongDiepId = bkhhd.ThongDiepId,
                            MauHoaDons = (from mhd in _db.MauHoaDons
                                          where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                          select new MauHoaDonViewModel
                                          {
                                              MauHoaDonId = mhd.MauHoaDonId,
                                              Ten = mhd.Ten,
                                              NgayKy = mhd.NgayKy,
                                              LoaiThueGTGT = mhd.LoaiThueGTGT,
                                              TenLoaiThueGTGT = mhd.LoaiThueGTGT.GetDescription()
                                          })
                                          .ToList(),
                            ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tk.Id,
                                ThongDiepId = bkhhd.ThongDiepId,
                                MaLoaiThongDiep = tdg.MaLoaiThongDiep,
                                MaThongDiepGui = tdg.MaThongDiep,
                                ThoiGianGui = tdg.NgayGui,
                                MaThongDiepNhan = tdn != null ? tdn.MaThongDiep : string.Empty,
                                TrangThaiGui = tdg.TrangThaiGui,
                                TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdg.TrangThaiGui).GetDescription(),
                                IsNhanUyNhiem = tk.NhanUyNhiem,
                                ThoiDiemChapNhan = tdg.NgayThongBao,
                            },
                            ToKhaiMoiNhatForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tkmt.Id,
                                ThongDiepId = bkhhd.ThongDiepMoiNhatId,
                                MaLoaiThongDiep = tdgmt.MaLoaiThongDiep,
                                MaThongDiepGui = tdgmt.MaThongDiep,
                                ThoiDiemChapNhan = tdgmt.NgayThongBao,
                            },
                            CreatedBy = bkhhd.CreatedBy,
                            CreatedDate = bkhhd.CreatedDate,
                            PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL,
                            Status = bkhhd.Status,
                            MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan,
                            SoBatDauCMCQT = bkhhd.SoBatDauCMCQT,

                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            if (result == null)
            {
                return result;
            }

            // get thời điểm chấp nhận ban đầu
            result.ToKhaiForBoKyHieuHoaDon.ThoiDiemChapNhan = await _db.NhatKyXacThucBoKyHieus
                .Where(x => x.BoKyHieuHoaDonId == result.BoKyHieuHoaDonId && x.ThoiDiemChapNhan.HasValue)
                .OrderBy(x => x.CreatedDate)
                .Select(x => x.ThoiDiemChapNhan)
                .FirstOrDefaultAsync() ?? result.ToKhaiForBoKyHieuHoaDon.ThoiDiemChapNhan;

            var fileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == result.ToKhaiForBoKyHieuHoaDon.ToKhaiId);
            if (fileData != null)
            {
                if (result.ToKhaiForBoKyHieuHoaDon.IsNhanUyNhiem == true)
                {
                    result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(fileData.Content);
                }
                else
                {
                    result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileData.Content);
                }
            }

            var fileDataMoiNhat = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == result.ToKhaiMoiNhatForBoKyHieuHoaDon.ToKhaiId);
            if (fileData != null)
            {
                if (result.ToKhaiMoiNhatForBoKyHieuHoaDon.IsNhanUyNhiem == true)
                {
                    result.ToKhaiMoiNhatForBoKyHieuHoaDon.ToKhaiUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(fileDataMoiNhat.Content);
                }
                else
                {
                    result.ToKhaiMoiNhatForBoKyHieuHoaDon.ToKhaiKhongUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileDataMoiNhat.Content);
                }
            }

            //if (result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem != null)
            //{
            //    var dkun = result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSDKUNhiem.FirstOrDefault(x => x.KHMSHDon == result.KyHieuMauSoHoaDon && x.KHHDon == result.KyHieuHoaDon);
            //    if (dkun != null)
            //    {
            //        result.ToKhaiForBoKyHieuHoaDon.STT = dkun.STT;
            //        result.ToKhaiForBoKyHieuHoaDon.TenLoaiHoaDonUyNhiem = dkun.TLHDon;
            //        result.ToKhaiForBoKyHieuHoaDon.KyHieuMauHoaDon = dkun.KHMSHDon;
            //        result.ToKhaiForBoKyHieuHoaDon.KyHieuHoaDonUyNhiem = dkun.KHHDon;
            //        result.ToKhaiForBoKyHieuHoaDon.TenToChucDuocUyNhiem = dkun.TTChuc;
            //        result.ToKhaiForBoKyHieuHoaDon.MaSoThueDuocUyNhiem = dkun.MST;
            //        result.ToKhaiForBoKyHieuHoaDon.MucDichUyNhiem = dkun.MDich;
            //        result.ToKhaiForBoKyHieuHoaDon.ThoiGianUyNhiem = DateTime.Parse(dkun.DNgay);
            //        result.ToKhaiForBoKyHieuHoaDon.PhuongThucThanhToan = (HTTToan)dkun.PThuc;
            //        result.ToKhaiForBoKyHieuHoaDon.TenPhuongThucThanhToan = (((HTTToan)dkun.PThuc).GetDescription());
            //    }
            //}

            return result;
        }

        public async Task<List<string>> GetChungThuSoByIdAsync(string id)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                        join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                        where bkhhd.BoKyHieuHoaDonId == id
                        select new BoKyHieuHoaDonViewModel
                        {
                            ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tk.Id,
                                IsNhanUyNhiem = tk.NhanUyNhiem
                            }
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            var fileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == result.ToKhaiForBoKyHieuHoaDon.ToKhaiId);
            if (fileData != null)
            {
                if (result.ToKhaiForBoKyHieuHoaDon.IsNhanUyNhiem == true)
                {
                    result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(fileData.Content);
                    return result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung.Select(x => x.Seri).ToList();
                }
                else
                {
                    result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileData.Content);
                    return result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung.Select(x => x.Seri).ToList();
                }

            }

            return null;
        }
        public async Task<List<string>> GetMultiChungThuSoByIdAsync(List<string> lisrId)
        {
            var listSerial = new List<string>();
            foreach (var id in lisrId)
            {
                var query = from bkhhd in _db.BoKyHieuHoaDons
                            join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                            join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                            where bkhhd.BoKyHieuHoaDonId == id
                            select new BoKyHieuHoaDonViewModel
                            {
                                ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                                {
                                    ToKhaiId = tk.Id,
                                    IsNhanUyNhiem = tk.NhanUyNhiem
                                }
                            };

                var result = await query.AsNoTracking().FirstOrDefaultAsync();

                if (result == null)
                {
                    return null;
                }

                var fileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == result.ToKhaiForBoKyHieuHoaDon.ToKhaiId);
                if (fileData != null)
                {
                    if (result.ToKhaiForBoKyHieuHoaDon.IsNhanUyNhiem == true)
                    {
                        result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(fileData.Content);
                        listSerial.AddRange(result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung.Select(x => x.Seri).ToList());
                    }
                    else
                    {
                        result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileData.Content);
                        listSerial.AddRange(result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung.Select(x => x.Seri).ToList());
                    }

                }
            }
            return listSerial;
        }

        public async Task<List<BoKyHieuHoaDonViewModel>> GetListByMauHoaDonIdAsync(string mauHoaDonId)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        let nhatKyXacThucCuoiCung = _db.NhatKyXacThucBoKyHieus
                            .Where(x => x.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId)
                            .OrderByDescending(x => x.ThoiGianXacThuc).FirstOrDefault()
                        where bkhhd.MauHoaDonId.Contains(mauHoaDonId)
                        select new BoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            KyHieu = bkhhd.KyHieu,
                            TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                            TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription(),
                            SoBatDau = bkhhd.SoBatDau,
                            SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                            SoToiDa = bkhhd.SoToiDa,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            MaSoThueBenUyNhiem = bkhhd.MaSoThueBenUyNhiem,
                            KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                            ThoiGianXacThuc = nhatKyXacThucCuoiCung != null ? nhatKyXacThucCuoiCung.ThoiGianXacThuc : null,
                            PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL
                        }
                        into bkhhds
                        orderby bkhhds.KyHieu23Int descending, bkhhds.ThoiGianXacThuc descending
                        select bkhhds;

            var result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Lấy danh sách bộ ký hiệu hóa đơn cho hóa đơn lựa chọn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<BoKyHieuHoaDonViewModel>> GetListForHoaDonAsync(BoKyHieuHoaDonViewModel model)
        {
            List<BoKyHieuHoaDonViewModel> result = new List<BoKyHieuHoaDonViewModel>();
            try
            {

                var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

                if (!model.NgayHoaDon.HasValue)
                {
                    return new List<BoKyHieuHoaDonViewModel>();
                }

                var yyOfNgayHoaDon = int.Parse(model.NgayHoaDon.Value.ToString("yy"));

                result = await (from bkhhd in _db.BoKyHieuHoaDons
                                where (bkhhd.LoaiHoaDon == model.LoaiHoaDon
                                || model.LoaiHoaDon == LoaiHoaDon.TatCa) &&
                                (bkhhd.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || bkhhd.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                                && (model.LoaiNghiepVu != null ? (model.LoaiNghiepVu == 1 ? (bkhhd.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || bkhhd.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang || bkhhd.LoaiHoaDon == LoaiHoaDon.CacLoaiHoaDonKhac) : model.LoaiNghiepVu == 3 ? 
                                (bkhhd.LoaiHoaDon == LoaiHoaDon.HoaDonGTGTCMTMTT || bkhhd.LoaiHoaDon == LoaiHoaDon.HoaDonBanHangCMTMTT || bkhhd.LoaiHoaDon == LoaiHoaDon.HoaDonKhacCMTMTT || bkhhd.LoaiHoaDon == LoaiHoaDon.TemVeGTGT) : (bkhhd.LoaiHoaDon  == LoaiHoaDon.PXKKiemVanChuyenNoiBo || bkhhd.LoaiHoaDon == LoaiHoaDon.PXKHangGuiBanDaiLy)) : true)
                                orderby bkhhd.KyHieu
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    LoaiHoaDon = bkhhd.LoaiHoaDon,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                                    KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                    KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                    MauHoaDons = (from mhd in _db.MauHoaDons
                                                  where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                                  && (string.IsNullOrEmpty(model.RoleId) || (!string.IsNullOrEmpty(model.RoleId) && _db.PhanQuyenMauHoaDons.Any(x=>x.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId && x.RoleId == model.RoleId && x.MauHoaDonIds.Contains(mhd.MauHoaDonId))))
                                                  orderby mhd.LoaiThueGTGT
                                                  select new MauHoaDonViewModel
                                                  {
                                                      MauHoaDonId = mhd.MauHoaDonId,
                                                      Ten = mhd.Ten,
                                                      NgayKy = mhd.NgayKy,
                                                      LoaiHoaDon = mhd.LoaiHoaDon,
                                                      LoaiThueGTGT = mhd.LoaiThueGTGT,
                                                      TenLoaiThueGTGT = mhd.LoaiThueGTGT.GetDescription(),
                                                      TenLoaiHoaDonFull = mhd.LoaiHoaDon.GetTenLoaiHoaDonFull()
                                                  })
                                                .ToList(),
                                    NhatKyXacThucBoKyHieus = (from nk in _db.NhatKyXacThucBoKyHieus
                                                              where nk.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId
                                                              orderby nk.CreatedDate
                                                              select new NhatKyXacThucBoKyHieuViewModel
                                                              {
                                                                  TrangThaiSuDung = nk.TrangThaiSuDung,
                                                                  LoaiHetHieuLuc = nk.LoaiHetHieuLuc,
                                                                  ThoiGianXacThuc = nk.ThoiGianXacThuc
                                                              })
                                                              .ToList(),
                                    MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan,
                                    SoBatDauCMCQT = bkhhd.SoBatDauCMCQT,
                                })
                                    .Where(x => x.KyHieu23Int == yyOfNgayHoaDon)
                                    .OrderByDescending(x => x.KyHieu23Int)
                                    .ThenBy(x => x.KyHieu)
                                    .ToListAsync();

                var yyOfCurrent = int.Parse(DateTime.Now.ToString("yy"));

                foreach (var item in result)
                {
                    if (item.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc) // trường hợp trạng thái là hết hiệu lực
                    {
                        // get last day of pre year
                        var lastDayOfPreYear = DateTime.Parse($"{DateTime.Now.Year - 1}-12-31 23:59:59");
                        if (item.NhatKyXacThucBoKyHieus.Count() > 1)
                        {
                            if (item.NhatKyXacThucBoKyHieus.LastOrDefault(x => x.LoaiHetHieuLuc == LoaiHetHieuLuc.ThoiDiemCuoiNam) != null && // trường hợp hết hiệu lực do ngày giờ thưc tế đã qua thời điểm 31/12/XXXX
                               (item.KyHieu23Int + 1) == yyOfCurrent &&  // nếu năm trong bộ ký hiệu là năm trước của năm hiện tại
                                item.NhatKyXacThucBoKyHieus[item.NhatKyXacThucBoKyHieus.Count - 2].TrangThaiSuDung != TrangThaiSuDung.NgungSuDung && // trường hợp trạng không phải là ngừng sử dụng -> hết hiệu lực
!item.NhatKyXacThucBoKyHieus.Any(x => x.TrangThaiSuDung == TrangThaiSuDung.NgungSuDung && x.ThoiGianXacThuc > lastDayOfPreYear)) // không có ngừng sử dụng sau thời gian 31/12/XXXX
                            {
                                if (hoSoHDDT.KyTinhThue == KyKeKhaiThue.Thang)
                                {
                                    var thoiDiem = DateTime.Parse($"{DateTime.Now.Year}-01-20");

                                    if (DateTime.Now.Date <= thoiDiem)
                                    {
                                        item.Checked = true;
                                    }
                                }
                                else
                                {
                                    var thoiDiem = DateTime.Parse($"{DateTime.Now.Year}-01-31");

                                    if (DateTime.Now.Date <= thoiDiem)
                                    {
                                        item.Checked = true;
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        item.Checked = true;
                    }
                }

                result = result.Where(x => x.Checked == true || (!string.IsNullOrEmpty(model.BoKyHieuHoaDonId) && x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId)).ToList();

            }
            catch (Exception ex)
            {
                Tracert.WriteLog("Bug-Get-Bo-Ky-Hieu " + ex.Message);
            }
            return result;

        }

        public async Task<List<NhatKyXacThucBoKyHieuViewModel>> GetListNhatKyXacThucByIdAsync(string id)
        {
            var result = await _db.NhatKyXacThucBoKyHieus
                .Where(x => x.BoKyHieuHoaDonId == id)
                .Select(x => new NhatKyXacThucBoKyHieuViewModel
                {
                    NhatKyXacThucBoKyHieuId = x.NhatKyXacThucBoKyHieuId,
                    TrangThaiSuDung = x.TrangThaiSuDung,
                    TenTrangThaiSuDung = x.TrangThaiSuDung.GetDescription(),
                    BoKyHieuHoaDonId = x.BoKyHieuHoaDonId,
                    CreatedDate = x.CreatedDate,
                    MauHoaDonId = x.MauHoaDonId,
                    NoiDung = x.NoiDung,
                    TenToChucChungThuc = x.TenToChucChungThuc,
                    SoSeriChungThu = x.SoSeriChungThu,
                    ThoiGianSuDungTu = x.ThoiGianSuDungTu,
                    ThoiGianSuDungDen = x.ThoiGianSuDungDen,
                    TenNguoiXacThuc = x.TenNguoiXacThuc,
                    TenMauHoaDon = x.TenMauHoaDon,
                    ThoiDiemChapNhan = x.ThoiDiemChapNhan,
                    ThoiGianXacThuc = x.ThoiGianXacThuc,
                    MaThongDiepGui = x.MaThongDiepGui,
                    ThongDiepId = x.ThongDiepId,
                    LoaiHetHieuLuc = x.LoaiHetHieuLuc,
                    SoLuongHoaDon = x.SoLuongHoaDon,
                    NgayHoaDon = x.NgayHoaDon,
                    CreatedBy = x.CreatedBy,
                    PhuongThucChuyenDL = x.PhuongThucChuyenDL,
                    TenPhuongThucChuyenDL = x.PhuongThucChuyenDL == PhuongThucChuyenDL.CDDu ? "Chuyển đầy đủ nội dung từng hóa đơn" : "Chuyển theo bảng tổng hợp dữ liệu hóa đơn điện tử"
                })
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();

            if (result.Count > 1 && result[0].TrangThaiSuDung == TrangThaiSuDung.ChuaXacThuc)
            {
                result = result.Skip(1).ToList();
            }
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    item.MauHoaDons = new List<MauHoaDonViewModel>();
                    if(!string.IsNullOrEmpty(item.MauHoaDonId))
                    {
                        var mauHoaDonIds = item.MauHoaDonId.Split(";");
                        if (mauHoaDonIds.Count() > 1)
                        {
                            foreach (var element in mauHoaDonIds)
                            {
                                MauHoaDonViewModel resultMauHoaDon = await _mauHoaDonService.GetByIdAsync(element);
                                item.MauHoaDons.Add(resultMauHoaDon);

                            }
                        }
                    }

                }
            }

                return result;
        }

        public async Task<string> GetSoSeriChungThuByIdAsync(string id)
        {
            var entity = await _db.NhatKyXacThucBoKyHieus
                .Where(x => x.BoKyHieuHoaDonId == id && !string.IsNullOrEmpty(x.SoSeriChungThu))
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            return entity?.SoSeriChungThu;
        }

        public async Task<BoKyHieuHoaDonViewModel> InsertAsync(BoKyHieuHoaDonViewModel model)
        {
            model.BoKyHieuHoaDonId = Guid.NewGuid().ToString();
            var entity = _mp.Map<BoKyHieuHoaDon>(model);
            entity.MaCuaCQTToKhaiChapNhan = model.MaCuaCQTToKhaiChapNhan;
            entity.SoBatDauCMCQT = 0;
            entity.CreatedDate = DateTime.Now;
            await _db.BoKyHieuHoaDons.AddAsync(entity);

            var nhatKyXacThuc = new NhatKyXacThucBoKyHieu
            {
                BoKyHieuHoaDonId = entity.BoKyHieuHoaDonId,
                TrangThaiSuDung = TrangThaiSuDung.ChuaXacThuc
            };
            await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyXacThuc);

            //cập nhật phân quyền mẫu hóa đơn
            if (string.IsNullOrEmpty(model.RoleId))
            {
                //trường hợp tk admin
               //ko làm gì cả, vì admin có quyền sử dụng mặc định đối với tất cả bkh
            }
            else
            {
                //trường hợp ko phải admin
                //thêm quyền sử dụng bkh được thêm đối vs nsd
                var pqModel = new PhanQuyenMauHoaDon
                {
                    RoleId = model.RoleId,
                    BoKyHieuHoaDonId = model.BoKyHieuHoaDonId,
                    MauHoaDonIds = model.MauHoaDonId
                };

                await _db.PhanQuyenMauHoaDons.AddAsync(pqModel);
            }

            await _db.SaveChangesAsync();
            var result = _mp.Map<BoKyHieuHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(BoKyHieuHoaDonViewModel model)
        {
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId);

            var mauHoaDonIds = model.MauHoaDonId.Split(";");

            var mauHoaDonIdOlds = entity.MauHoaDonId.Split(";");
            List<MauHoaDon> MauHoaDonOlds = new List<MauHoaDon>();
            foreach (var itemMauOld in mauHoaDonIdOlds)
            {
                MauHoaDon mauHoaDonOld = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == itemMauOld);
                if (mauHoaDonOld != null)
                {
                    MauHoaDonOlds.Add(mauHoaDonOld);
                }
            }

            foreach (var mauHoaDonId in mauHoaDonIds)
            {
                var mauHoaDon = await _mauHoaDonService.GetByIdAsync(mauHoaDonId);
                string MauHoaDonIdOldByThueSuat = "";
                switch (mauHoaDon.LoaiThueGTGT)
                {

                    case LoaiThueGTGT.MauMotThueSuat:
                        if (MauHoaDonOlds.Count() > 0)
                        {

                            MauHoaDon MauHoaDonMotThue = MauHoaDonOlds.FirstOrDefault(x => x.LoaiThueGTGT == LoaiThueGTGT.MauMotThueSuat);
                            if(MauHoaDonMotThue != null)
                            {
                                MauHoaDonIdOldByThueSuat = MauHoaDonMotThue.MauHoaDonId;
                                if (!string.IsNullOrEmpty(MauHoaDonIdOldByThueSuat) && MauHoaDonIdOldByThueSuat != mauHoaDonId)
                                {
                                    await _mauHoaDonService.UpdateAllHoaDonChuaKyWhenXacThuBytime(mauHoaDonId, MauHoaDonIdOldByThueSuat);
                                }
                            }
                        }

                        break;
                    case LoaiThueGTGT.MauNhieuThueSuat:
                        if (MauHoaDonOlds.Count() > 0)
                        {
                             MauHoaDon MauHoaDonNhieuThue = MauHoaDonOlds.FirstOrDefault(x => x.LoaiThueGTGT == LoaiThueGTGT.MauNhieuThueSuat);
                            if(MauHoaDonNhieuThue != null)
                            {
                                MauHoaDonIdOldByThueSuat = MauHoaDonNhieuThue.MauHoaDonId;
                                if (!string.IsNullOrEmpty(MauHoaDonIdOldByThueSuat) && MauHoaDonIdOldByThueSuat != mauHoaDonId)
                                {
                                    await _mauHoaDonService.UpdateAllHoaDonChuaKyWhenXacThuBytime(mauHoaDonId, MauHoaDonIdOldByThueSuat);
                                }
                            }

                        }
                        break;
                    default:
                        break;
                }
            }
            model.SoBatDauCMCQT = 0;
            _db.Entry(entity).CurrentValues.SetValues(model);

            //cập nhật phân quyền mẫu hóa đơn
            if (string.IsNullOrEmpty(model.RoleId))
            {
                //trường hợp tk admin
                //ko làm gì cả
            }
            else
            {
                //trường hợp ko phải admin
                var pqModels = await _db.PhanQuyenMauHoaDons.Where(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId && x.RoleId == model.RoleId).ToListAsync();
                foreach (var item in pqModels)
                {
                    item.MauHoaDonIds = model.MauHoaDonId;
                }

                _db.UpdateRange(pqModels);
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> XacThucBoKyHieuHoaDonAsync(NhatKyXacThucBoKyHieuViewModel model)
        {
            var fullName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId);
            entity.TrangThaiSuDung = model.TrangThaiSuDung;

            if (entity.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc)
            {
                // Nếu trạng thái là Đã xác thực và có số hóa đơn thì set Đang sử dụng
                if (entity.SoLonNhatDaLapDenHienTai.HasValue)
                {
                    entity.TrangThaiSuDung = TrangThaiSuDung.DangSuDung;
                }
            }

            switch (model.TrangThaiSuDung)
            {
                case TrangThaiSuDung.DaXacThuc:
                case TrangThaiSuDung.NgungSuDung:
                    var nhatKyDaXacThuc = new NhatKyXacThucBoKyHieu
                    {
                        TrangThaiSuDung = model.TrangThaiSuDung,
                        BoKyHieuHoaDonId = model.BoKyHieuHoaDonId,
                        MauHoaDonId = model.MauHoaDonId,
                        TenNguoiXacThuc = fullName,
                        ThongDiepId = model.ThongDiepId,
                        ThoiGianXacThuc = DateTime.Now,
                        TenToChucChungThuc = model.TenToChucChungThuc,
                        SoSeriChungThu = model.SoSeriChungThu,
                        ThoiGianSuDungTu = model.ThoiGianSuDungTu,
                        ThoiGianSuDungDen = model.ThoiGianSuDungDen,
                        ThoiDiemChapNhan = model.ThoiDiemChapNhan,
                        TenMauHoaDon = model.TenMauHoaDon,
                        MaThongDiepGui = model.MaThongDiepGui,
                        PhuongThucChuyenDL = model.PhuongThucChuyenDL
                    };

                    if (model.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc)
                    {
                        List<MauHoaDonXacThuc> mauHoaDonXacThucs = new List<MauHoaDonXacThuc>();

                        var listMauHoaDon = await _mauHoaDonService.GetListMauHoaDonXacThucAsync(model.MauHoaDonId);
                        foreach (var item in listMauHoaDon)
                        {
                            mauHoaDonXacThucs.Add(new MauHoaDonXacThuc
                            {
                                MauHoaDonId = item.MauHoaDonId,
                                FileByte = item.FileByte,
                                FileType = item.FileType
                            });
                        }

                        nhatKyDaXacThuc.MauHoaDonXacThucs = mauHoaDonXacThucs;
                    }

                    await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyDaXacThuc);
                    break;
                case TrangThaiSuDung.DangSuDung:
                    break;
                case TrangThaiSuDung.HetHieuLuc:
                    var nhatKyHetHieuLuc = new NhatKyXacThucBoKyHieu();

                    // Nếu là xác thực hết hiệu lực trên form
                    if (model.IsXacThucTrenForm == true)
                    {
                        entity.PhuongThucChuyenDL = model.PhuongThucChuyenDL;
                        entity.ThongDiepMoiNhatId = model.ThongDiepMoiNhatId;

                        nhatKyHetHieuLuc = new NhatKyXacThucBoKyHieu
                        {
                            TrangThaiSuDung = TrangThaiSuDung.DaXacThuc,
                            BoKyHieuHoaDonId = model.BoKyHieuHoaDonId,
                            MauHoaDonId = model.MauHoaDonId,
                            TenNguoiXacThuc = fullName,
                            ThongDiepId = model.ThongDiepMoiNhatId,
                            ThoiGianXacThuc = DateTime.Now,
                            TenToChucChungThuc = model.TenToChucChungThuc,
                            SoSeriChungThu = model.SoSeriChungThu,
                            ThoiGianSuDungTu = model.ThoiGianSuDungTu,
                            ThoiGianSuDungDen = model.ThoiGianSuDungDen,
                            ThoiDiemChapNhan = model.ThoiDiemChapNhan,
                            TenMauHoaDon = model.TenMauHoaDon,
                            MaThongDiepGui = model.MaThongDiepGui,
                            PhuongThucChuyenDL = model.PhuongThucChuyenDL
                        };
                    }
                    else
                    {
                        nhatKyHetHieuLuc = new NhatKyXacThucBoKyHieu
                        {
                            TrangThaiSuDung = model.TrangThaiSuDung,
                            BoKyHieuHoaDonId = model.BoKyHieuHoaDonId,
                            ThoiGianXacThuc = DateTime.Now,
                            LoaiHetHieuLuc = model.LoaiHetHieuLuc,
                            SoLuongHoaDon = model.LoaiHetHieuLuc == LoaiHetHieuLuc.XuatHetSoHoaDon ? model.SoLuongHoaDon : entity.SoLonNhatDaLapDenHienTai,
                            NgayHoaDon = model.NgayHoaDon
                        };
                    }

                    await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyHetHieuLuc);
                    break;
                default:
                    break;
            }

            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        //private async Task InsertThietLapTruongDuLieus(BoKyHieuHoaDon entity)
        //{
        //    var tltdlByMauHoaDonIds = await _thietLapTruongDuLieuService.GetListTruongMoRongByMauHoaDonIdAsync(entity.MauHoaDonId);

        //    var initData = new ThietLapTruongDuLieu().InitData();

        //    switch (entity.LoaiHoaDon)
        //    {
        //        case LoaiHoaDon.HoaDonGTGT:
        //            if (true)
        //            {

        //            }
        //            break;
        //        case LoaiHoaDon.HoaDonBanHang:
        //            break;
        //        case LoaiHoaDon.HoaDonBanTaiSanCong:
        //            break;
        //        case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
        //            break;
        //        case LoaiHoaDon.CacLoaiHoaDonKhac:
        //            break;
        //        case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
        //            break;
        //        default:
        //            break;
        //    }

        //    return;
        //}

        public async Task<bool> CheckThoiHanChungThuSoAsync(BoKyHieuHoaDonViewModel model)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                        join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                        where bkhhd.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId
                        select new BoKyHieuHoaDonViewModel
                        {
                            ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tk.Id,
                                IsNhanUyNhiem = tk.NhanUyNhiem
                            }
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            if (result == null)
            {
                return false;
            }

            DateTime signDate = DateTime.Now.Date;
            var fileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == result.ToKhaiForBoKyHieuHoaDon.ToKhaiId);
            if (fileData != null)
            {
                if (result.ToKhaiForBoKyHieuHoaDon.IsNhanUyNhiem == true)
                {
                    result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(fileData.Content);
                    var serial = result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung.FirstOrDefault(x => x.Seri == model.SerialNumber);
                    if (serial != null)
                    {
                        if (signDate >= DateTime.Parse(serial.TNgay) && signDate <= DateTime.Parse(serial.DNgay))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileData.Content);
                    var serial = result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung.FirstOrDefault(x => x.Seri == model.SerialNumber);
                    if (serial != null)
                    {
                        if (signDate >= DateTime.Parse(serial.TNgay) && signDate <= DateTime.Parse(serial.DNgay))
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }

        public async Task<bool> CheckDaKySoBatDauAsync(string id)
        {
            var result = await (from bkhdh in _db.BoKyHieuHoaDons
                                join hddt in _db.HoaDonDienTus on bkhdh.BoKyHieuHoaDonId equals hddt.BoKyHieuHoaDonId
                                where bkhdh.BoKyHieuHoaDonId == id && hddt.SoHoaDon == bkhdh.SoBatDau
                                select bkhdh).AnyAsync();

            return result;
        }

        /// <summary>
        /// get nhật ký xác thực bộ ký hiệu id cho việc xem mẫu hóa đơn
        /// </summary>
        /// <param name="nhatKyXacThucBoKyHieuId"></param>
        /// <returns></returns>
        public async Task<string> GetNhatKyXacThucBoKyHieuIdForXemMauHoaDonAsync(string nhatKyXacThucBoKyHieuId)
        {
            var result = await _db.MauHoaDonXacThucs.AnyAsync(x => x.NhatKyXacThucBoKyHieuId == nhatKyXacThucBoKyHieuId);

            // nếu không tồn tại dữ liệu mẫu hóa đơn thì lấy của xác thực trước đó
            if (!result)
            {
                var boKyHieuHoaDonId = await _db.NhatKyXacThucBoKyHieus
                    .Where(x => x.NhatKyXacThucBoKyHieuId == nhatKyXacThucBoKyHieuId)
                    .Select(x => x.BoKyHieuHoaDonId)
                    .FirstOrDefaultAsync();

                nhatKyXacThucBoKyHieuId = await (from nkxtbkh in _db.NhatKyXacThucBoKyHieus
                                                 join mhdxt in _db.MauHoaDonXacThucs on nkxtbkh.NhatKyXacThucBoKyHieuId equals mhdxt.NhatKyXacThucBoKyHieuId
                                                 orderby nkxtbkh.CreatedDate descending
                                                 select nkxtbkh.NhatKyXacThucBoKyHieuId).FirstOrDefaultAsync();
            }

            return nhatKyXacThucBoKyHieuId;
        }

        public async Task<string> CheckHasToKhaiMoiNhatAsync(BoKyHieuHoaDonViewModel model)
        {
            var thongDiepMoiNhat = await _db.ThongDiepChungs
                .Where(x => x.MaLoaiThongDiep == model.ToKhaiForBoKyHieuHoaDon.MaLoaiThongDiep &&
                            x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan &&
                            x.NgayGui > model.ToKhaiForBoKyHieuHoaDon.ThoiGianGui)
                .OrderByDescending(x => x.NgayGui)
                .FirstOrDefaultAsync();

            if (thongDiepMoiNhat == null)
            {
                return null;
            }

            string result = $"Ký hiệu {model.KyHieu} đang liên kết với tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn có mã thông điệp gửi " +
                $"&lt;{model.ToKhaiForBoKyHieuHoaDon.MaThongDiepGui}&gt; có thời gian gửi {model.ToKhaiForBoKyHieuHoaDon.ThoiGianGui.Value:dd/MM/yyyy HH:mm:ss}." +
                $"<p>Hệ thống tìm thấy tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử có mã thông điệp gửi " +
                $"&lt;{thongDiepMoiNhat.MaThongDiep}&gt; có thời gian gửi {thongDiepMoiNhat.NgayGui.Value:dd/MM/yyyy HH:mm:ss} đã được CQT chấp nhận " +
                $"có thông tin phù hợp với Ký hiệu &lt;{model.KyHieu}&gt; nhưng chưa được liên hết với ký hiệu này. Vui lòng kiểm tra lại!</p>";
            return result;
        }

        public async Task<ToKhaiForBoKyHieuHoaDonViewModel> CheckToKhaiPhuHopAsync(BoKyHieuHoaDonViewModel model)
        {
            var query = from tk in _db.ToKhaiDangKyThongTins
                        join tdg in _db.ThongDiepChungs on tk.Id equals tdg.IdThamChieu
                        where (tk.NhanUyNhiem == (model.UyNhiemLapHoaDon == UyNhiemLapHoaDon.DangKy)) &&
                        (tdg.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan) &&
                        tdg.NgayThongBao > model.ThongDiepChung.NgayThongBao
                        orderby tdg.NgayThongBao descending
                        select new ToKhaiForBoKyHieuHoaDonViewModel
                        {
                            ToKhaiId = tk.Id,
                            ThongDiepId = tdg.ThongDiepChungId,
                            MaThongDiepGui = tdg.MaThongDiep,
                            ThoiGianGui = tdg.NgayGui,
                            TrangThaiGui = tdg.TrangThaiGui,
                            ThoiDiemChapNhan = tdg.NgayThongBao,
                            ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                            ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                        };

            if (model.UyNhiemLapHoaDon == UyNhiemLapHoaDon.DangKy)
            {
                query = query.Where(x => x.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSDKUNhiem.Any(y => y.KHMSHDon == model.KyHieuMauSoHoaDon && y.KHHDon == model.KyHieuHoaDon));
            }
            else
            {
                query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.HTHDon.CMa == (int)model.HinhThucHoaDon);

                switch (model.LoaiHoaDon)
                {
                    case LoaiHoaDon.HoaDonGTGT:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanHang:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanTaiSanCong:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1);
                        break;
                    case LoaiHoaDon.CacLoaiHoaDonKhac:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1);
                        break;
                    case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.CTu == 1);
                        break;
                    default:
                        break;
                }
            }

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Get thông tin tờ khai mới nhất vào bộ ký hiệu
        /// </summary>
        /// <returns></returns>
        public async Task<BoKyHieuHoaDonViewModel> GetThongTinTuToKhaiMoiNhatAsync()
        {
            BoKyHieuHoaDonViewModel result = new BoKyHieuHoaDonViewModel();

            // nếu có tờ khai được chấp nhận mới nhất thì lấy
            var toKhaiMoiNhat = await (from tk in _db.ToKhaiDangKyThongTins
                                       join tdg in _db.ThongDiepChungs on tk.Id equals tdg.IdThamChieu
                                       where tdg.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
                                       orderby tdg.NgayThongBao descending, tdg.NgayGui descending, tdg.ModifyDate descending
                                       select new ToKhaiForBoKyHieuHoaDonViewModel
                                       {
                                           ToKhaiId = tk.Id,
                                           ThongDiepId = tdg.ThongDiepChungId,
                                           MaThongDiepGui = tdg.MaThongDiep,
                                           ThoiGianGui = tdg.NgayGui,
                                           MaThongDiepNhan = tdg.MaThongDiepPhanHoi,
                                           TrangThaiGui = tdg.TrangThaiGui,
                                           ThoiDiemChapNhan = tdg.NgayThongBao,
                                           TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdg.TrangThaiGui).GetDescription(),
                                           ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                           ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                       }).FirstOrDefaultAsync();

            //// nếu ko có thì
            //if (toKhaiMoiNhat == null)
            //{
            //    // nếu theo tờ khai gửi đi mới nhất
            //    toKhaiMoiNhat = await queryToKhaiMoiNhat.OrderBy(x => x.ThoiGianGui).LastOrDefaultAsync();
            //}

            if (toKhaiMoiNhat == null)
            {
                return null;
            }

            result.ToKhaiForBoKyHieuHoaDon = toKhaiMoiNhat;

            // Trường hợp tờ khai không ủy nghiệm
            if (toKhaiMoiNhat.ToKhaiKhongUyNhiem != null)
            {
                var ndtKhai = toKhaiMoiNhat.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai;

                // get hình thức hóa đơn
                result.HinhThucHoaDon = ndtKhai.HTHDon.CMa == 1 ? HinhThucHoaDon.CoMa : HinhThucHoaDon.KhongCoMa;
                result.IsCMTMTTien = ndtKhai.HTHDon.CMTMTTien == 1 ? true : false;
                result.IsKhongCoMa = ndtKhai.HTHDon.KCMa == 1 ? true : false;
                if (ndtKhai.HTHDon.CMTMTTien == 1)
                {
                    var ThongDiep103TuongUng = await _db.HoSoHDDTs.FirstOrDefaultAsync();
                    if (ThongDiep103TuongUng != null)
                    {
                        result.MaCuaCQTToKhaiChapNhan = ThongDiep103TuongUng.MaCuaCQTToKhaiChapNhan ?? String.Empty;
                    }
                }

                // phương thức chuyển dữ liệu hóa đơn điện tử
                result.IsChuyenDayDuNoiDungTungHoaDon = ndtKhai.PThuc.CDDu == 1;
                result.IsChuyenBangTongHop = ndtKhai.PThuc.CBTHop == 1;

                // get loại hóa đơn
                result.LoaiHoaDons = new List<LoaiHoaDon>();
                if (ndtKhai.LHDSDung.HDGTGT == 1)
                {
                    result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonGTGT);

                    if (result.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                    {
                        var bkhGTGTs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT && x.HinhThucHoaDon == HinhThucHoaDon.CoMa).ToListAsync();
                        if (bkhGTGTs.Any())
                        {
                            foreach (var item in bkhGTGTs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhGTGTs);
                    }

                    if (result.IsKhongCoMa)
                    {
                        var bkhGTGTs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT && x.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa).ToListAsync();
                        if (bkhGTGTs.Any())
                        {
                            foreach (var item in bkhGTGTs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhGTGTs);
                    }
                }
                if (ndtKhai.LHDSDung.HDBHang == 1)
                {
                    result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonBanHang);

                    if (result.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                    {
                        var bkhBanHangs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && x.HinhThucHoaDon == HinhThucHoaDon.CoMa).ToListAsync();
                        if (bkhBanHangs.Any())
                        {
                            foreach (var item in bkhBanHangs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhBanHangs);
                    }

                    if (result.IsKhongCoMa)
                    {
                        var bkhBanHangs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && x.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa).ToListAsync();
                        if (bkhBanHangs.Any())
                        {
                            foreach (var item in bkhBanHangs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhBanHangs);
                    }
                }
                if (ndtKhai.LHDSDung.HDBTSCong == 1)
                {
                    result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonBanTaiSanCong);
                }
                if (ndtKhai.LHDSDung.HDBHDTQGia == 1)
                {
                    result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonBanHangDuTruQuocGia);
                }
                if (ndtKhai.LHDSDung.HDKhac == 1)
                {
                    if (result.HinhThucHoaDon == HinhThucHoaDon.CoMa || result.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa)
                    {
                        if (!result.LoaiHoaDons.Contains(LoaiHoaDon.CacLoaiHoaDonKhac)) result.LoaiHoaDons.Add(LoaiHoaDon.CacLoaiHoaDonKhac);
                        result.LoaiHoaDons.Add(LoaiHoaDon.TemVeGTGT);
                        result.LoaiHoaDons.Add(LoaiHoaDon.TemVeBanHang);
                        var bkhKhacs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.CacLoaiHoaDonKhac && x.HinhThucHoaDon == HinhThucHoaDon.CoMa).ToListAsync();
                        if (bkhKhacs.Any())
                        {
                            foreach (var item in bkhKhacs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhKhacs);
                    }


                }
                if (ndtKhai.LHDSDung.CTu == 1)
                {
                    result.LoaiHoaDons.Add(LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD);
                    result.LoaiHoaDons.Add(LoaiHoaDon.PXKKiemVanChuyenNoiBo);
                    result.LoaiHoaDons.Add(LoaiHoaDon.PXKHangGuiBanDaiLy);


                    var bkhKhacs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && (x.LoaiHoaDon == LoaiHoaDon.PXKKiemVanChuyenNoiBo || x.LoaiHoaDon == LoaiHoaDon.PXKHangGuiBanDaiLy || x.LoaiHoaDon == LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD) && x.HinhThucHoaDon == HinhThucHoaDon.CoMa).ToListAsync();
                    if (bkhKhacs.Any())
                    {
                        foreach (var item in bkhKhacs)
                        {
                            item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                        }
                    }

                    _db.BoKyHieuHoaDons.UpdateRange(bkhKhacs);
                }
                if (result.IsCMTMTTien == true)
                {
                    if (ndtKhai.LHDSDung.HDGTGT == 1)
                    {
                        result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonGTGTCMTMTT);

                        var bkhGTGTs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonGTGTCMTMTT && x.HinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien).ToListAsync();
                        if (bkhGTGTs.Any())
                        {
                            foreach (var item in bkhGTGTs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhGTGTs);

                    }
                    if (ndtKhai.LHDSDung.HDBHang == 1)
                    {
                        result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonBanHangCMTMTT);

                        var bkhBanHangs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonBanHangCMTMTT && x.HinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien).ToListAsync();
                        if (bkhBanHangs.Any())
                        {
                            foreach (var item in bkhBanHangs)
                            {
                                item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                            }
                        }

                        _db.BoKyHieuHoaDons.UpdateRange(bkhBanHangs);
                    }

                    if (ndtKhai.LHDSDung.HDKhac == 1)
                    {
                        result.LoaiHoaDons.Add(LoaiHoaDon.HoaDonKhacCMTMTT);
                    }

                    var bkhKhacs = await _db.BoKyHieuHoaDons.Where(x => (x.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || x.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc) && x.LoaiHoaDon == LoaiHoaDon.HoaDonKhacCMTMTT && x.HinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien).ToListAsync();
                    if (bkhKhacs.Any())
                    {
                        foreach (var item in bkhKhacs)
                        {
                            item.ThongDiepId = toKhaiMoiNhat.ThongDiepId;
                        }
                    }

                    _db.BoKyHieuHoaDons.UpdateRange(bkhKhacs);
                }
            }

            await _db.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// Hàm kiểm tra xem tờ khai trong bộ ký hiệu có tích chuyển theo bảng tổng hợp không
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True: có, False: không</returns>
        public async Task<bool> HasChuyenTheoBangTongHopDuLieuHDDTAsync(string id)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                        join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                        where bkhhd.BoKyHieuHoaDonId == id
                        select new BoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            ThongDiepId = bkhhd.ThongDiepId,
                            ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tk.Id,
                                ThongDiepId = bkhhd.ThongDiepId,
                                IsNhanUyNhiem = tk.NhanUyNhiem,
                            },
                            CreatedBy = bkhhd.CreatedBy,
                            CreatedDate = bkhhd.CreatedDate,
                            Status = bkhhd.Status,
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();
            if (result == null)
            {
                return false;
            }

            // get content xml of ToKhai
            var fileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.RefId == result.ToKhaiForBoKyHieuHoaDon.ToKhaiId);
            if (fileData != null && result.ToKhaiForBoKyHieuHoaDon.IsNhanUyNhiem != true)
            {
                result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(fileData.Content);
                return result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.PThuc.CBTHop == 1;
            }

            return false;
        }

        /// <summary>
        /// Kiểm tra trạng thái sử dụng trước khi sửa bộ ký hiệu hóa đơn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NhatKyXacThucBoKyHieuViewModel> CheckTrangThaiSuDungTruocKhiSuaAsync(string id)
        {
            // get bo ky hieu by id
            var model = await (from bkhhd in _db.BoKyHieuHoaDons
                               where bkhhd.BoKyHieuHoaDonId == id
                               select new BoKyHieuHoaDonViewModel
                               {
                                   BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                   HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                   LoaiHoaDon = bkhhd.LoaiHoaDon,
                                   TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                   NhatKyXacThucBoKyHieus = (from nk in _db.NhatKyXacThucBoKyHieus
                                                             where nk.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId
                                                             orderby nk.CreatedDate descending
                                                             select new NhatKyXacThucBoKyHieuViewModel
                                                             {
                                                                 MaThongDiepGui = nk.MaThongDiepGui,
                                                                 ThoiDiemChapNhan = nk.ThoiDiemChapNhan
                                                             })
                                                             .ToList()
                               })
                               .FirstOrDefaultAsync();

            var toKhaiMoiNhat = await (from tk in _db.ToKhaiDangKyThongTins
                                       join tdg in _db.ThongDiepChungs on tk.Id equals tdg.IdThamChieu
                                       where tdg.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
                                       orderby tdg.NgayThongBao descending, tdg.NgayGui descending, tdg.ModifyDate descending
                                       select new ToKhaiForBoKyHieuHoaDonViewModel
                                       {
                                           ToKhaiId = tk.Id,
                                           ThongDiepId = tdg.ThongDiepChungId,
                                           MaThongDiepGui = tdg.MaThongDiep,
                                           ThoiGianGui = tdg.NgayGui,
                                           MaThongDiepNhan = tdg.MaThongDiepPhanHoi,
                                           TrangThaiGui = tdg.TrangThaiGui,
                                           ThoiDiemChapNhan = tdg.NgayThongBao,
                                           TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdg.TrangThaiGui).GetDescription(),
                                           ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                           ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                       }).FirstOrDefaultAsync();

            LoaiThongTinChiTiet loaiHoaDon = LoaiThongTinChiTiet.HoaDonGTGT;
            switch (model.LoaiHoaDon)
            {
                case LoaiHoaDon.HoaDonBanHang:
                    loaiHoaDon = LoaiThongTinChiTiet.HoaDonBanHang;
                    break;
                case LoaiHoaDon.HoaDonBanTaiSanCong:
                    loaiHoaDon = LoaiThongTinChiTiet.HoaDonBanTaiSanCong;
                    break;
                case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                    loaiHoaDon = LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia;
                    break;
                case LoaiHoaDon.CacLoaiHoaDonKhac:
                    loaiHoaDon = LoaiThongTinChiTiet.CacLoaiHoaDonKhac;
                    break;
                case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                    loaiHoaDon = LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon;
                    break;
                default:
                    break;
            }

            LoaiThongTinChiTiet loaiHinhThucHoaDon = model.HinhThucHoaDon == HinhThucHoaDon.CoMa ?
                LoaiThongTinChiTiet.CoMaCuaCoQuanThue :
                LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue;

            // get thong tin hoa don by bo ky hieu
            var thongTinHoaDons = await _db.QuanLyThongTinHoaDons
                .Where(x => x.TrangThaiSuDung == TrangThaiSuDung2.NgungSuDung && (x.LoaiThongTinChiTiet == loaiHoaDon || x.LoaiThongTinChiTiet == loaiHinhThucHoaDon))
                .AsNoTracking()
                .ToListAsync();

            if (!thongTinHoaDons.Any())
            {
                return null;
            }
            if (model.HinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien)
            {
                return null;
            }
            string tenLoaiThongTin = string.Empty;
            // get tên loại thông tin
            if (thongTinHoaDons.Any(x => x.LoaiThongTinChiTiet == loaiHinhThucHoaDon) &&
                thongTinHoaDons.Any(x => x.LoaiThongTinChiTiet == loaiHoaDon))
            {
                tenLoaiThongTin = "Hình thức hóa đơn và Loại hóa đơn";
            }
            else
            {
                if (thongTinHoaDons.Any(x => x.LoaiThongTinChiTiet == loaiHinhThucHoaDon))
                {
                    tenLoaiThongTin = "Hình thức hóa đơn";
                }
                else
                {
                    tenLoaiThongTin = "Loại hóa đơn";
                }
            }

            var result = new NhatKyXacThucBoKyHieuViewModel
            {
                NoiDung = tenLoaiThongTin,
                MaThongDiepGui = toKhaiMoiNhat.MaThongDiepGui,
                ThoiDiemChapNhan = toKhaiMoiNhat.ThoiDiemChapNhan,
            };

            return result;
        }

        /// <summary>
        /// Lấy danh sách bộ ký hiệu để phát hành hóa đơn đồng loạt
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<BoKyHieuHoaDonViewModel>> GetListForPhatHanhDongLoatAsync(PagingParams param)
        {
            var yyOfNgayHoaDon = int.Parse(DateTime.Now.ToString("yy"));

            var result = await (from bkhhd in _db.BoKyHieuHoaDons
                                where (param.Permission == true || param.MauHoaDonDuocPQ.Contains(bkhhd.BoKyHieuHoaDonId)) &&
                                (bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || bkhhd.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    LoaiHoaDon = bkhhd.LoaiHoaDon,
                                    HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    SoBatDau = bkhhd.SoBatDau,
                                    SoToiDa = bkhhd.SoToiDa,
                                    SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                                    MauHoaDons = (from mhd in _db.MauHoaDons
                                                  where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                                  select new MauHoaDonViewModel
                                                  {
                                                      MauHoaDonId = mhd.MauHoaDonId,
                                                      Ten = mhd.Ten,
                                                      NgayKy = mhd.NgayKy,
                                                      LoaiHoaDon = mhd.LoaiHoaDon,
                                                      LoaiThueGTGT = mhd.LoaiThueGTGT
                                                  })
                                           .ToList(),
                                    SoBatDauCMCQT = bkhhd.SoBatDauCMCQT,
                                    MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan

                                })
                                .Where(x => x.KyHieu23Int == yyOfNgayHoaDon)
                                .OrderByDescending(x => x.KyHieu23Int)
                                .ThenBy(x => x.KyHieu)
                                .ToListAsync();

            if (result.Any())
            {
                result.Insert(0, new BoKyHieuHoaDonViewModel
                {
                    BoKyHieuHoaDonId = "-1",
                    KyHieu = "Tất cả"
                });
            }

            return result;
        }

        /// <summary>
        /// kiểm tra bộ ký hiệu có đang đc phát hành hay chưa
        /// </summary>
        /// <param name="boKyHieuHoaDonIds"></param>
        /// <returns></returns>
        public async Task<string> CheckBoKyHieuDangPhatHanhAsync(List<string> boKyHieuHoaDonIds)
        {
            var listResult = await (from bkhhd in _db.BoKyHieuHoaDons
                                    join bkhdph in _db.BoKyHieuDangPhatHanhs on bkhhd.BoKyHieuHoaDonId equals bkhdph.BoKyHieuHoaDonId
                                    where boKyHieuHoaDonIds.Contains(bkhhd.BoKyHieuHoaDonId)
                                    orderby bkhhd.KyHieu
                                    select bkhhd.KyHieu)
                                    .Distinct()
                                    .ToListAsync();

            if (listResult.Any())
            {
                return $"Bộ ký hiệu <span class='colorChuYTrongThongBao'><b>{string.Join(",", listResult)}</b></span> đang được phát hành đồng loạt. Vui lòng chờ!";
            }
            else
            {
                var listAdded = boKyHieuHoaDonIds.Distinct()
                    .Select(x => new BoKyHieuDangPhatHanh
                    {
                        BoKyHieuHoaDonId = x
                    })
                    .ToList();

                await _db.BoKyHieuDangPhatHanhs.AddRangeAsync(listAdded);
                await _db.SaveChangesAsync();
                return null;
            }
        }

        /// <summary>
        /// xóa hết bộ ký hiệu sau khi phát hành
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ClearBoKyHieuDaPhatHanhAsync()
        {
            var listAll = await _db.BoKyHieuDangPhatHanhs.ToListAsync();
            _db.BoKyHieuDangPhatHanhs.RemoveRange(listAll);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        /// Check xem đã hêt số lượng hóa đơn và xác thực bộ ký hiệu
        /// </summary>
        /// <param name="hoaDon"></param>
        /// <returns></returns>
        public async Task CheckDaHetSoLuongHoaDonVaXacThucAsync(HoaDonDienTuViewModel hoaDon)
        {
            HoaDonDienTuViewModel maxHoaDon = null;

            var hoaDonDienTuIds = hoaDon.Children.Select(x => x.HoaDonDienTuId).ToList();

            var hoaDonDienTus = await _db.HoaDonDienTus
                .Where(x => hoaDonDienTuIds.Contains(x.HoaDonDienTuId) && (x.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu))
                .Select(x => new HoaDonDienTuViewModel
                {
                    HoaDonDienTuId = x.HoaDonDienTuId
                })
                .ToDictionaryAsync(x => x.HoaDonDienTuId);

            foreach (var item in hoaDon.Children)
            {
                if (!hoaDonDienTus.ContainsKey(item.HoaDonDienTuId))
                {
                    continue;
                }

                if (maxHoaDon == null || item.SoHoaDon > maxHoaDon.SoHoaDon)
                {
                    maxHoaDon = item;
                }
            }

            if (maxHoaDon != null)
            {
                var checkDaDungHetSLHD = await CheckDaHetSoLuongHoaDonAsync(maxHoaDon.BoKyHieuHoaDonId, maxHoaDon.SoHoaDon);
                if (checkDaDungHetSLHD) // đã dùng hết
                {
                    await XacThucBoKyHieuHoaDonAsync(new NhatKyXacThucBoKyHieuViewModel
                    {
                        BoKyHieuHoaDonId = maxHoaDon.BoKyHieuHoaDonId,
                        TrangThaiSuDung = TrangThaiSuDung.HetHieuLuc,
                        LoaiHetHieuLuc = LoaiHetHieuLuc.XuatHetSoHoaDon,
                        SoLuongHoaDon = maxHoaDon.SoHoaDon,
                        NgayHoaDon = maxHoaDon.NgayHoaDon
                    });
                }
            }
        }

        /// <summary>
        /// Function Call Get List BoKyHieuHoaDon từ máy post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns
        public async Task<List<BoKyHieuForMayTinhTienViewModels>> GetListBoKyHieuForHoaDonCMMTTienAsync(BoKyHieuHoaDonViewModel model)
        {
            var keKhaiThueGTGT = await _tuyChonService.GetDetailAsync("KyKeKhaiThueGTGT");

            if (!model.NgayHoaDon.HasValue)
            {
                return new List<BoKyHieuForMayTinhTienViewModels>();
            }

            var yyOfNgayHoaDon = int.Parse(model.NgayHoaDon.Value.ToString("yy"));

            var result = await (from bkhhd in _db.BoKyHieuHoaDons
                                where (bkhhd.LoaiHoaDon == model.LoaiHoaDon || model.LoaiHoaDon == LoaiHoaDon.TatCa) &&
                                (bkhhd.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung || bkhhd.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                                orderby bkhhd.KyHieu
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    LoaiHoaDon = bkhhd.LoaiHoaDon,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                                    KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                    KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                    MauHoaDons = (from mhd in _db.MauHoaDons
                                                  where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                                  orderby mhd.LoaiThueGTGT
                                                  select new MauHoaDonViewModel
                                                  {
                                                      MauHoaDonId = mhd.MauHoaDonId,
                                                      Ten = mhd.Ten,
                                                      NgayKy = mhd.NgayKy,
                                                      LoaiHoaDon = mhd.LoaiHoaDon,
                                                      LoaiThueGTGT = mhd.LoaiThueGTGT,
                                                      TenLoaiThueGTGT = mhd.LoaiThueGTGT.GetDescription(),
                                                      TenLoaiHoaDonFull = mhd.LoaiHoaDon.GetTenLoaiHoaDonFull()
                                                  })
                                                .ToList(),
                                    NhatKyXacThucBoKyHieus = (from nk in _db.NhatKyXacThucBoKyHieus
                                                              where nk.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId
                                                              orderby nk.CreatedDate
                                                              select new NhatKyXacThucBoKyHieuViewModel
                                                              {
                                                                  TrangThaiSuDung = nk.TrangThaiSuDung,
                                                                  LoaiHetHieuLuc = nk.LoaiHetHieuLuc,
                                                                  ThoiGianXacThuc = nk.ThoiGianXacThuc
                                                              })
                                                              .ToList(),
                                    MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan,
                                    SoBatDauCMCQT = bkhhd.SoBatDauCMCQT
                                })
                                .Where(x => x.KyHieu23Int == yyOfNgayHoaDon)
                                .OrderByDescending(x => x.KyHieu23Int)
                                .ThenBy(x => x.KyHieu)
                                .ToListAsync();

            var yyOfCurrent = int.Parse(DateTime.Now.ToString("yy"));
            foreach (var item in result)
            {
                if (item.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc) // trường hợp trạng thái là hết hiệu lực
                {
                    // get last day of pre year
                    var lastDayOfPreYear = DateTime.Parse($"{DateTime.Now.Year - 1}-12-31 23:59:59");

                    if (item.NhatKyXacThucBoKyHieus.LastOrDefault(x => x.LoaiHetHieuLuc == LoaiHetHieuLuc.ThoiDiemCuoiNam) != null && // trường hợp hết hiệu lực do ngày giờ thưc tế đã qua thời điểm 31/12/XXXX
                        (item.KyHieu23Int + 1) == yyOfCurrent &&  // nếu năm trong bộ ký hiệu là năm trước của năm hiện tại
                        item.NhatKyXacThucBoKyHieus[item.NhatKyXacThucBoKyHieus.Count - 2].TrangThaiSuDung != TrangThaiSuDung.NgungSuDung && // trường hợp trạng không phải là ngừng sử dụng -> hết hiệu lực
                        !item.NhatKyXacThucBoKyHieus.Any(x => x.TrangThaiSuDung == TrangThaiSuDung.NgungSuDung && x.ThoiGianXacThuc > lastDayOfPreYear)) // không có ngừng sử dụng sau thời gian 31/12/XXXX
                    {
                        if (keKhaiThueGTGT.GiaTri == "Thang")
                        {
                            var thoiDiem = DateTime.Parse($"{DateTime.Now.Year}-01-20");

                            if (DateTime.Now.Date <= thoiDiem)
                            {
                                item.Checked = true;
                            }
                        }
                        else
                        {
                            var thoiDiem = DateTime.Parse($"{DateTime.Now.Year}-01-31");

                            if (DateTime.Now.Date <= thoiDiem)
                            {
                                item.Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    item.Checked = true;
                }
            }

            result = result.Where(x => x.Checked == true).ToList();
            List<BoKyHieuForMayTinhTienViewModels> query = new List<BoKyHieuForMayTinhTienViewModels>();

            foreach (var item in result)
            {
                BoKyHieuForMayTinhTienViewModels itemAdd = new BoKyHieuForMayTinhTienViewModels
                {
                    BoKyHieuHoaDonId = item.BoKyHieuHoaDonId,
                    MauHoaDonId = item.MauHoaDonId,
                    KyHieu = item.KyHieu,
                    MaCuaCQTToKhaiChapNhan = item.MaCuaCQTToKhaiChapNhan,
                    SoBatDauCMCQT = item.SoBatDauCMCQT,
                    MauHoaDons = item.MauHoaDons,
                };
                query.Add(itemAdd);
            }
            return query;
        }
        public async Task<List<BoKyHieuHoaDonViewModel>> PosGetBoKyHieuMTT()
        {
            var result = await (from bkhhd in _db.BoKyHieuHoaDons
                                where (bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung)
                                orderby bkhhd.KyHieu
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                    UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                    LoaiHoaDon = bkhhd.LoaiHoaDon,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                    KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                    KyHieu1 = bkhhd.KyHieu1,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                                    KyHieu4 = bkhhd.KyHieu4,
                                    KyHieu56 = bkhhd.KyHieu56,
                                    SoBatDau = bkhhd.SoBatDau,
                                    SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                                    SoToiDa = bkhhd.SoToiDa,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription(),
                                    MaSoThueBenUyNhiem = bkhhd.MaSoThueBenUyNhiem,
                                    IsTuyChinh = bkhhd.IsTuyChinh,
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    ThongDiepId = bkhhd.ThongDiepId,
                                    MauHoaDons = (from mhd in _db.MauHoaDons
                                                  where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                                  select new MauHoaDonViewModel
                                                  {
                                                      MauHoaDonId = mhd.MauHoaDonId,
                                                      Ten = mhd.Ten,
                                                      NgayKy = mhd.NgayKy,
                                                      LoaiThueGTGT = mhd.LoaiThueGTGT,
                                                      TenLoaiThueGTGT = mhd.LoaiThueGTGT.GetDescription()
                                                  })
                                          .ToList(),                                    
                                    CreatedBy = bkhhd.CreatedBy,
                                    CreatedDate = bkhhd.CreatedDate,
                                    PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL,
                                    Status = bkhhd.Status,
                                    MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan,
                                    SoBatDauCMCQT = bkhhd.SoBatDauCMCQT,
                                })
                               .OrderByDescending(x => x.KyHieu23Int)
                               .ThenBy(x => x.KyHieu)
                               .ToListAsync();
            return result;

        }
        public async Task<List<BoKyHieuHoaDonViewModel>> PosGetAllBoKyHieu()
        {
            var result = await (from bkhhd in _db.BoKyHieuHoaDons
                                where
                                (bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc || bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung)
                                orderby bkhhd.KyHieu
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                    UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                    LoaiHoaDon = bkhhd.LoaiHoaDon,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                    KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                    KyHieu1 = bkhhd.KyHieu1,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                                    KyHieu4 = bkhhd.KyHieu4,
                                    KyHieu56 = bkhhd.KyHieu56,
                                    SoBatDau = bkhhd.SoBatDau,
                                    SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                                    SoToiDa = bkhhd.SoToiDa,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription(),
                                    MaSoThueBenUyNhiem = bkhhd.MaSoThueBenUyNhiem,
                                    IsTuyChinh = bkhhd.IsTuyChinh,
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    ThongDiepId = bkhhd.ThongDiepId,
                                    MauHoaDons = (from mhd in _db.MauHoaDons
                                                  where bkhhd.MauHoaDonId.Contains(mhd.MauHoaDonId)
                                                  select new MauHoaDonViewModel
                                                  {
                                                      MauHoaDonId = mhd.MauHoaDonId,
                                                      Ten = mhd.Ten,
                                                      NgayKy = mhd.NgayKy,
                                                      LoaiThueGTGT = mhd.LoaiThueGTGT,
                                                      TenLoaiThueGTGT = mhd.LoaiThueGTGT.GetDescription()
                                                  })
                                          .ToList(),                                    
                                    CreatedBy = bkhhd.CreatedBy,
                                    CreatedDate = bkhhd.CreatedDate,
                                    PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL,
                                    Status = bkhhd.Status,
                                    MaCuaCQTToKhaiChapNhan = bkhhd.MaCuaCQTToKhaiChapNhan,
                                    SoBatDauCMCQT = bkhhd.SoBatDauCMCQT,
                                })
                               .OrderByDescending(x => x.KyHieu23Int)
                               .ThenBy(x => x.KyHieu)
                               .ToListAsync();
            return result;

        }
        public async Task<bool> XacThucMauHoaDonWhenChangeHopDong()
        {
            bool result = false;
            try
            {

                IQueryable<MauHoaDon> listMauHoaDon = _db.MauHoaDons.AsQueryable();

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        /// <summary>
        /// Lấy số lớn nhất của có mã của cơ quan thuế theo loại hóa đơn
        /// </summary>
        /// <param name="loaiHoaDon"></param>
        /// <returns></returns>
        private async Task<long> GetMaxNumberOfCMCQTByType(LoaiHoaDon loaiHoaDon)
        {
            BoKyHieuHoaDon query = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.LoaiHoaDon == loaiHoaDon);
            if (query != null)
            {
                return (long)query.SoBatDauCMCQT;
            }
            else
            {
                return 0;

            }
        }

        /// <summary>
        /// Check khách hàng đã dùng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool CheckUsedBoKyHieuHoaDon(string Id)
        {
            bool res = false;
            try
            {
                int count = 0;
                /// check hàng hóa đã dùng ở hóa đơn
                count += _db.HoaDonDienTus.Where(x => x.BoKyHieuHoaDonId == Id).ToList().Count;

                res = count > 0;
            }
            catch (Exception e)
            {
                Tracert.WriteLog("Bug check used  :" + e.ToString());
            }
            return res;

        }
    }
}