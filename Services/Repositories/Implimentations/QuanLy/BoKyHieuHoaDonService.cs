using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.Config;
using DLL.Entity.QuanLy;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.QuanLy;
using Services.Repositories.Interfaces.Config;
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
        private readonly ITuyChonService _tuyChonService;
        private readonly IThietLapTruongDuLieuService _thietLapTruongDuLieuService;

        public BoKyHieuHoaDonService(
            Datacontext dataContext,
            IMapper mp,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            ITuyChonService tuyChonService,
            IThietLapTruongDuLieuService thietLapTruongDuLieuService)
        {
            _db = dataContext;
            _mp = mp;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _tuyChonService = tuyChonService;
            _thietLapTruongDuLieuService = thietLapTruongDuLieuService;
            _thietLapTruongDuLieuService = thietLapTruongDuLieuService;
        }

        /// <summary>
        /// Kiểm tra xem số lượng hóa đơn đã dùng hết chưa
        /// </summary>
        /// <param name="boKyHieuHoaDonId"></param>
        /// <param name="soHoaDon"></param>
        /// <returns></returns>
        public async Task<bool> CheckDaHetSoLuongHoaDonAsync(string boKyHieuHoaDonId, string soHoaDon)
        {
            var result = false;
            var entity = await _db.BoKyHieuHoaDons
                .FirstOrDefaultAsync(x => x.BoKyHieuHoaDonId == boKyHieuHoaDonId);

            if (entity != null)
            {
                int currentSoHoaDon = int.Parse(soHoaDon);

                // nếu số lượng hiện tại ở bộ ký hiệu khác số hiện tại trên hóa đơn thì update
                if (entity.SoLonNhatDaLapDenHienTai != currentSoHoaDon && currentSoHoaDon > (entity.SoLonNhatDaLapDenHienTai ?? 0))
                {
                    entity.SoLonNhatDaLapDenHienTai = currentSoHoaDon;

                    if (entity.TrangThaiSuDung != TrangThaiSuDung.HetHieuLuc)
                    {
                        entity.TrangThaiSuDung = TrangThaiSuDung.DangSuDung;
                    }

                    await _db.SaveChangesAsync();
                }

                // nếu số hiện tại trên hóa đơn = số tối đa trong bộ ký hiệu thì báo đã dùng hết
                if (currentSoHoaDon == entity.SoToiDa)
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
                         join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                         join tdc in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdc.ThongDiepChungId
                         select new BoKyHieuHoaDonViewModel
                         {
                             BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                             KyHieu23Int = int.Parse(bkhhd.KyHieu23),
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
                                 TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                                 TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                                 NgayThongBao = tdc.NgayThongBao,
                             },
                             ModifyDate = bkhhd.ModifyDate,
                             SoBatDau = bkhhd.SoBatDau,
                             SoLonNhatDaLapDenHienTai = bkhhd.SoLonNhatDaLapDenHienTai,
                             SoToiDa = bkhhd.SoToiDa,
                             TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                             TenTrangThaiSuDung = bkhhd.TrangThaiSuDung.GetDescription()
                         })
                         .OrderByDescending(x => x.KyHieu23Int)
                         .ThenBy(x => x.KyHieu)
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
                                TenTrangThaiGui = ((TrangThaiGuiThongDiep)tdg.TrangThaiGui).GetDescription(),
                                IsNhanUyNhiem = tk.NhanUyNhiem,
                                ThoiDiemChapNhan = tdg.NgayThongBao,
                            },
                            CreatedBy = bkhhd.CreatedBy,
                            CreatedDate = bkhhd.CreatedDate,
                            Status = bkhhd.Status,
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            if (result == null)
            {
                return result;
            }

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

        public async Task<List<string>> GetChungThuSoByIdAsync(string id)
        {
            var query = from bkhhd in _db.BoKyHieuHoaDons
                        join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                        join tdg in _db.ThongDiepChungs on bkhhd.ThongDiepId equals tdg.ThongDiepChungId
                        join tk in _db.ToKhaiDangKyThongTins on tdg.IdThamChieu equals tk.Id
                        where bkhhd.BoKyHieuHoaDonId == id
                        select new BoKyHieuHoaDonViewModel
                        {
                            ToKhaiForBoKyHieuHoaDon = new ToKhaiForBoKyHieuHoaDonViewModel
                            {
                                ToKhaiId = tk.Id,
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

        /// <summary>
        /// Lấy danh sách bộ ký hiệu hóa đơn cho hóa đơn lựa chọn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<BoKyHieuHoaDonViewModel>> GetListForHoaDonAsync(BoKyHieuHoaDonViewModel model)
        {
            var keKhaiThueGTGT = await _tuyChonService.GetDetailAsync("KyKeKhaiThueGTGT");

            if (!model.NgayHoaDon.HasValue)
            {
                return new List<BoKyHieuHoaDonViewModel>();
            }

            var yyOfNgayHoaDon = int.Parse(model.NgayHoaDon.Value.ToString("yy"));

            var result = await (from bkhhd in _db.BoKyHieuHoaDons
                                join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                                where (bkhhd.LoaiHoaDon == model.LoaiHoaDon || model.LoaiHoaDon == LoaiHoaDon.TatCa) && (bkhhd.BoKyHieuHoaDonId == model.BoKyHieuHoaDonId ||
                                                                                                                        bkhhd.TrangThaiSuDung == TrangThaiSuDung.ChuaXacThuc ||
                                                                                                                        bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc ||
                                                                                                                        bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung ||
                                                                                                                        bkhhd.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                                orderby bkhhd.KyHieu
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    KyHieu23Int = int.Parse(bkhhd.KyHieu23),
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    MauHoaDon = new MauHoaDonViewModel
                                    {
                                        MauHoaDonId = mhd.MauHoaDonId,
                                        LoaiHoaDon = mhd.LoaiHoaDon,
                                        LoaiThueGTGT = mhd.LoaiThueGTGT
                                    },
                                    NhatKyXacThucBoKyHieus = (from nk in _db.NhatKyXacThucBoKyHieus
                                                              where nk.BoKyHieuHoaDonId == bkhhd.BoKyHieuHoaDonId
                                                              orderby nk.CreatedDate
                                                              select new NhatKyXacThucBoKyHieuViewModel
                                                              {
                                                                  TrangThaiSuDung = nk.TrangThaiSuDung
                                                              })
                                                              .ToList()
                                })
                                .Where(x => (x.KyHieu23Int == yyOfNgayHoaDon) || ((x.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc) && ((x.KyHieu23Int + 1) == yyOfNgayHoaDon)))
                                .OrderByDescending(x => x.KyHieu23Int)
                                .ThenBy(x => x.KyHieu)
                                .ToListAsync();

            var yyOfCurrent = int.Parse(DateTime.Now.ToString("yy"));

            foreach (var item in result)
            {
                if (item.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                {
                    // nếu năm trong bộ ký hiệu là năm trước của năm hiện tại
                    if ((item.KyHieu23Int + 1) == yyOfCurrent)
                    {
                        if (item.NhatKyXacThucBoKyHieus[item.NhatKyXacThucBoKyHieus.Count - 2].TrangThaiSuDung != TrangThaiSuDung.NgungSuDung)
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
                }
                else
                {
                    item.Checked = true;
                }
            }

            result = result.Where(x => x.Checked == true).ToList();
            return result;
        }

        public async Task<bool> KiemTraHieuLucBoKyHieu(string boKyHieuId)
        {
            var keKhaiThueGTGT = await _tuyChonService.GetDetailAsync("KyKeKhaiThueGTGT");

            var result = await (from bkhhd in _db.BoKyHieuHoaDons
                                join mhd in _db.MauHoaDons on bkhhd.MauHoaDonId equals mhd.MauHoaDonId
                                where (bkhhd.TrangThaiSuDung == TrangThaiSuDung.DaXacThuc ||
                                                                               bkhhd.TrangThaiSuDung == TrangThaiSuDung.DangSuDung ||
                                                                               bkhhd.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                                orderby bkhhd.KyHieu
                                select new BoKyHieuHoaDonViewModel
                                {
                                    BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                    TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                    KyHieu = bkhhd.KyHieu,
                                    KyHieu23 = bkhhd.KyHieu23,
                                    MauHoaDonId = bkhhd.MauHoaDonId,
                                    MauHoaDon = new MauHoaDonViewModel
                                    {
                                        MauHoaDonId = mhd.MauHoaDonId,
                                        LoaiHoaDon = mhd.LoaiHoaDon,
                                        LoaiThueGTGT = mhd.LoaiThueGTGT
                                    }
                                })
                                .ToListAsync();

            var yy = int.Parse(DateTime.Now.ToString("yy"));

            foreach (var item in result)
            {
                var intKyHieu23 = int.Parse(item.KyHieu23);

                if (item.TrangThaiSuDung == TrangThaiSuDung.HetHieuLuc)
                {
                    // nếu năm bộ ký hiệu = năm hiện tại
                    if (intKyHieu23 == yy)
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        // nếu năm trong bộ ký hiệu là năm trước của năm hiện tại
                        if ((intKyHieu23 + 1) == yy)
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
                }
                else
                {
                    item.Checked = true;
                }
            }

            result = result.Where(x => x.Checked == true).ToList();
            return result.Any(x => x.BoKyHieuHoaDonId == boKyHieuId);
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
                    };
                    await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyDaXacThuc);
                    break;
                case TrangThaiSuDung.DangSuDung:
                    break;
                case TrangThaiSuDung.HetHieuLuc:
                    var nhatKyHetHieuLuc = new NhatKyXacThucBoKyHieu
                    {
                        TrangThaiSuDung = model.TrangThaiSuDung,
                        BoKyHieuHoaDonId = model.BoKyHieuHoaDonId,
                        ThoiGianXacThuc = DateTime.Now,
                        IsHetSoLuongHoaDon = model.IsHetSoLuongHoaDon,
                        SoLuongHoaDon = model.IsHetSoLuongHoaDon == true ? model.SoLuongHoaDon : entity.SoLonNhatDaLapDenHienTai
                    };

                    await _db.NhatKyXacThucBoKyHieus.AddAsync(nhatKyHetHieuLuc);
                    break;
                default:
                    break;
            }

            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        private async Task InsertThietLapTruongDuLieus(BoKyHieuHoaDon entity)
        {
            var tltdlByMauHoaDonIds = await _thietLapTruongDuLieuService.GetListTruongMoRongByMauHoaDonIdAsync(entity.MauHoaDonId);

            var initData = new ThietLapTruongDuLieu().InitData();

            switch (entity.LoaiHoaDon)
            {
                case LoaiHoaDon.HoaDonGTGT:
                    if (true)
                    {

                    }
                    break;
                case LoaiHoaDon.HoaDonBanHang:
                    break;
                case LoaiHoaDon.HoaDonBanTaiSanCong:
                    break;
                case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                    break;
                case LoaiHoaDon.CacLoaiHoaDonKhac:
                    break;
                case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                    break;
                default:
                    break;
            }

            return;
        }
    }
}
