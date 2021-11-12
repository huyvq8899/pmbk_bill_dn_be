using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuanLy;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.QuanLy;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLy
{
    public class BoKyHieuHoaDonService : IBoKyHieuHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BoKyHieuHoaDonService(
            Datacontext dataContext,
            IMapper mp,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = dataContext;
            _mp = mp;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public string CheckSoSeriChungThu(BoKyHieuHoaDonViewModel model)
        {
            var maThongDiepGui = model.ToKhaiForBoKyHieuHoaDon.MaThongDiepGui;

            if (model.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem != null)
            {
                var cts = model.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung;
                if (!cts.Any(x => x.Seri == model.SerialNumber))
                {
                    return $"Chứng thư số &lt;{model.SerialNumber}&gt; không thuộc danh sách chứng thư số sử dụng tại mục 5" +
                           $" của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                           $"Vui lòng kiểm tra lại.";
                }
                else
                {
                    var hinhThucDangKy = cts.FirstOrDefault(x => x.Seri == model.SerialNumber).HThuc;
                    if (hinhThucDangKy == 3)
                    {
                        return $"Chứng thư số &lt;{model.SerialNumber}&gt; có hình thức đăng ký là &lt;Ngừng sử dụng&gt; trên danh sách chứng thư số" +
                               $" sử dụng tại mục 5 của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                               $"Vui lòng kiểm tra lại.";
                    }
                }
            }

            if (model.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem != null)
            {
                var cts = model.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung;
                if (!cts.Any(x => x.Seri == model.SerialNumber))
                {
                    return $"Chứng thư số &lt;{model.SerialNumber}&gt; không thuộc danh sách chứng thư số sử dụng tại mục 5" +
                           $" của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                           $"Vui lòng kiểm tra lại.";
                }
                else
                {
                    var hinhThucDangKy = cts.FirstOrDefault(x => x.Seri == model.SerialNumber).HThuc;
                    if (hinhThucDangKy == 3)
                    {
                        return $"Chứng thư số &lt;{model.SerialNumber}&gt; có hình thức đăng ký là &lt;Ngừng sử dụng&gt; trên danh sách chứng thư số" +
                               $" sử dụng tại mục 5 của Tờ khai đăng ký/thay đổi thông tin sử dụng dịch vụ hóa đơn điện tử &lt;<b>{maThongDiepGui}</b>&gt;." +
                               $"Vui lòng kiểm tra lại.";
                    }
                }
            }

            return null;
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
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                        join tdc in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdc.ThongDiepChungId
                        orderby bkhhd.KyHieu
                        select new BoKyHieuHoaDonViewModel
                        {
                            BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                            KyHieu = bkhhd.KyHieu,
                            UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                            MauHoaDon = new MauHoaDonViewModel
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                Ten = mhd.Ten,
                                NgayKy = mhd.NgayKy
                            },
                            ThongDiepChung = new ThongDiepChungViewModel
                            {
                                ThongDiepChungId = tdc.ThongDiepChungId,
                                MaThongDiep = tdc.MaThongDiep,
                                TrangThaiGui = (TrangThaiGuiToKhaiDenCQT)tdc.TrangThaiGui,
                                TenTrangThaiGui = ((TrangThaiGuiToKhaiDenCQT)tdc.TrangThaiGui).GetDescription(),
                                NgayThongBao = tdc.NgayThongBao,
                            },
                            ModifyDate = bkhhd.ModifyDate,
                            SoBatDau = bkhhd.SoBatDau,
                            SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                            SoToiDa = bkhhd.SoToiDa,
                            TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                            TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription()
                        };

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

            if (@params.TimKiemTheo != null)
            {
                var timKiemTheo = @params.TimKiemTheo;
                if (!string.IsNullOrEmpty(timKiemTheo.KyHieu))
                {
                    var keyword = timKiemTheo.KyHieu.ToUpper().ToTrim();
                    query = query.Where(x => x.KyHieu.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenUyNhiemLapHoaDon))
                {
                    var keyword = timKiemTheo.TenUyNhiemLapHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.TenUyNhiemLapHoaDon.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.TenMauHoaDon))
                {
                    var keyword = timKiemTheo.TenMauHoaDon.ToUpper().ToTrim();
                    query = query.Where(x => x.MauHoaDon.Ten.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiep))
                {
                    var keyword = timKiemTheo.MaThongDiep.ToUpper().ToTrim();
                    query = query.Where(x => x.ThongDiepChung.MaThongDiep.ToUpper().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.ThoiDiemChapNhanFilter))
                {
                    var keyword = timKiemTheo.ThoiDiemChapNhanFilter.ToTrim();
                    query = query.Where(x => x.ThoiDiemChapNhan.HasValue && x.ThoiDiemChapNhan.Value.ToString("dd/MM/yyyy HH:mm:ss").Contains(keyword));
                }
                if (!string.IsNullOrEmpty(timKiemTheo.NgayCapNhatFilter))
                {
                    var keyword = timKiemTheo.NgayCapNhatFilter.ToTrim();
                    query = query.Where(x => x.ModifyDate.HasValue && x.ModifyDate.Value.ToString("dd/MM/yyyy").Contains(keyword));
                }
            }

            return await PagedList<BoKyHieuHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<BoKyHieuHoaDonViewModel> GetByIdAsync(string id)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                        join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                        join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                        join tdn in _db.ThongDiepChungs on tdg.MaThongDiep equals tdn.MaThongDiepThamChieu into tmpThongDiepNhans
                        from tdn in tmpThongDiepNhans.DefaultIfEmpty()
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
                            IsTuyChinh = bkhhd.IsTuyChinh,
                            MauHoaDonId = bkhhd.MauHoaDonId,
                            ThongDiepId = bkhhd.ThongDiepId,
                            MauHoaDon = new MauHoaDonViewModel
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                Ten = mhd.Ten,
                                NgayKy = mhd.NgayKy
                            },
                            ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tk.Id,
                                ThongDiepId = bkhhd.ThongDiepId,
                                MaThongDiepGui = tdg.MaThongDiep,
                                ThoiGianGui = tdg.NgayGui,
                                MaThongDiepNhan = tdn != null ? tdn.MaThongDiep : string.Empty,
                                TrangThaiGui = tdg.TrangThaiGui,
                                TenTrangThaiGui = ((TrangThaiGuiToKhaiDenCQT)tdg.TrangThaiGui).GetDescription(),
                                ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromTKhai<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(tk, _hostingEnvironment.WebRootPath),
                                ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromTKhai<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(tk, _hostingEnvironment.WebRootPath),
                                ThoiGianChapNhan = tdg.NgayThongBao,
                            },
                            CreatedBy = bkhhd.CreatedBy,
                            CreatedDate = bkhhd.CreatedDate,
                            Status = bkhhd.Status,
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            if (result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem != null)
            {
                var cts = result.ToKhaiForBoKyHieuHoaDon.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung[0];
                result.ToKhaiForBoKyHieuHoaDon.TenToChucChungThuc = cts.TTChuc;
                result.ToKhaiForBoKyHieuHoaDon.SoSeriChungThu = cts.Seri;
                result.ToKhaiForBoKyHieuHoaDon.ThoiGianSuDungTu = DateTime.Parse(cts.TNgay);
                result.ToKhaiForBoKyHieuHoaDon.ThoiGianSuDungDen = DateTime.Parse(cts.DNgay);
            }

            if (result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem != null)
            {
                var cts = result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung[0];
                result.ToKhaiForBoKyHieuHoaDon.TenToChucChungThuc = cts.TTChuc;
                result.ToKhaiForBoKyHieuHoaDon.SoSeriChungThu = cts.Seri;
                result.ToKhaiForBoKyHieuHoaDon.ThoiGianSuDungTu = DateTime.Parse(cts.TNgay);
                result.ToKhaiForBoKyHieuHoaDon.ThoiGianSuDungDen = DateTime.Parse(cts.DNgay);
            }

            if (result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem != null)
            {
                var dkun = result.ToKhaiForBoKyHieuHoaDon.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSDKUNhiem.FirstOrDefault(x => x.KHMSHDon == result.KyHieuMauSoHoaDon && x.KHHDon == result.KyHieuHoaDon);
                if (dkun != null)
                {
                    result.ToKhaiForBoKyHieuHoaDon.STT = dkun.STT;
                    result.ToKhaiForBoKyHieuHoaDon.TenLoaiHoaDonUyNhiem = dkun.TLHDon;
                    result.ToKhaiForBoKyHieuHoaDon.KyHieuMauHoaDon = dkun.KHMSHDon;
                    result.ToKhaiForBoKyHieuHoaDon.KyHieuHoaDonUyNhiem = dkun.KHHDon;
                    result.ToKhaiForBoKyHieuHoaDon.TenToChucDuocUyNhiem = dkun.TTChuc;
                    result.ToKhaiForBoKyHieuHoaDon.MaSoThueDuocUyNhiem = dkun.MST;
                    result.ToKhaiForBoKyHieuHoaDon.MucDichUyNhiem = dkun.MDich;
                    result.ToKhaiForBoKyHieuHoaDon.ThoiGianUyNhiem = DateTime.Parse(dkun.DNgay);
                    result.ToKhaiForBoKyHieuHoaDon.PhuongThucThanhToan = (HTTToan)dkun.PThuc;
                    result.ToKhaiForBoKyHieuHoaDon.TenPhuongThucThanhToan = (((HTTToan)dkun.PThuc).GetDescription());
                }
            }

            return result;
        }

        public async Task<List<BoKyHieuHoaDonViewModel>> GetListByMauHoaDonIdAsync(string mauHoaDonId)
        {
            var result = await _db.BoKyHieuHoaDons
                .Where(x => x.MauHoaDonId == mauHoaDonId && x.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc)
                .Select(x => new BoKyHieuHoaDonViewModel
                {
                    BoKyHieuHoaDonId = x.BoKyHieuHoaDonId,
                    KyHieu = x.KyHieu,
                    TrangThaiSuDung = x.TrangThaiSuDung,
                    TenTrangThaiSuDung = x.TrangThaiSuDung.GetDescription(),
                    SoBatDau = x.SoBatDau,
                    SoLonNhatDaLapDenHienTai = x.SoLonNhatDaLapDenHienTai,
                    SoToiDa = x.SoToiDa,
                    MauHoaDonId = x.MauHoaDonId
                })
                .OrderByDescending(x => x.KyHieu)
                .ToListAsync();

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
                    CreatedBy = x.CreatedBy,
                })
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();

            return result;
        }

        public async Task<BoKyHieuHoaDonViewModel> InsertAsync(BoKyHieuHoaDonViewModel model)
        {
            model.BoKyHieuHoaDonId = Guid.NewGuid().ToString();
            var entity = _mp.Map<BoKyHieuHoaDon>(model);
            await _db.BoKyHieuHoaDons.AddAsync(entity);

            var nhatKyXacThuc = new NhatKyXacThucBoKyHieu
            {
                BoKyHieuHoaDonId = entity.BoKyHieuHoaDonId,
                TrangThaiSuDung = TrangThaiSuDung.ChuaXacThuc
            };
            await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyXacThuc);

            await _db.SaveChangesAsync();
            var result = _mp.Map<BoKyHieuHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(BoKyHieuHoaDonViewModel model)
        {
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> XacThucBoKyHieuHoaDonAsync(NhatKyXacThucBoKyHieuViewModel model)
        {
            var fullName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;
            var entity = await _db.BoKyHieuHoaDons.FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId);
            entity.TrangThaiSuDung = model.TrangThaiSuDung;

            switch (model.TrangThaiSuDung)
            {
                case TrangThaiSuDung.DaXacThuc:
                    var nhatKyDaXacThuc = new NhatKyXacThucBoKyHieu
                    {
                        TrangThaiSuDung = TrangThaiSuDung.DaXacThuc,
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
                    };
                    await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyDaXacThuc);
                    break;
                case TrangThaiSuDung.DangSuDung:
                    break;
                case TrangThaiSuDung.NgungSuDung:
                    var nhatKyNgungSuDung = new NhatKyXacThucBoKyHieu
                    {
                        TrangThaiSuDung = TrangThaiSuDung.NgungSuDung,
                        BoKyHieuHoaDonId = model.BoKyHieuHoaDonId,
                        MauHoaDonId = model.MauHoaDonId,
                        TenNguoiXacThuc = fullName,
                        ThongDiepId = model.ThongDiepId,
                        ThoiGianXacThuc = DateTime.Now,
                    };
                    await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyNgungSuDung);
                    break;
                case TrangThaiSuDung.HetHieuLuc:
                    break;
                default:
                    break;
            }

            var result = await _db.SaveChangesAsync();
            return result > 0;
        }
    }
}
