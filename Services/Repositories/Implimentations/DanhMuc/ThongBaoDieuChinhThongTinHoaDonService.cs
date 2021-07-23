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
    public class ThongBaoDieuChinhThongTinHoaDonService : IThongBaoDieuChinhThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThongBaoDieuChinhThongTinHoaDonService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            bool result = await _db.ThongBaoDieuChinhThongTinHoaDons
               .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.ThongBaoDieuChinhThongTinHoaDon, _db);

            var entity = await _db.ThongBaoDieuChinhThongTinHoaDons.FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == id);
            _db.ThongBaoDieuChinhThongTinHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public Task<List<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllAsync(ThongBaoDieuChinhThongTinHoaDonParams @params = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllPagingAsync(ThongBaoDieuChinhThongTinHoaDonParams @params)
        {
            var query = _db.ThongBaoDieuChinhThongTinHoaDons
                .OrderByDescending(x => x.NgayThongBaoDieuChinh).OrderByDescending(x => x.So)
                .Select(x => new ThongBaoDieuChinhThongTinHoaDonViewModel
                {
                    ThongBaoDieuChinhThongTinHoaDonId = x.ThongBaoDieuChinhThongTinHoaDonId,
                    NgayThongBaoDieuChinh = x.NgayThongBaoDieuChinh,
                    NgayThongBaoPhatHanh = x.NgayThongBaoPhatHanh,
                    CoQuanThue = x.CoQuanThue,
                    So = x.So,
                    TrangThaiHieuLuc = x.TrangThaiHieuLuc,
                    TenTrangThai = x.TrangThaiHieuLuc.GetDescription(),
                    Status = x.Status,
                    NoiDungThayDoi = GetNoiDungThayDoi(x)
                });

            if (@params.Filter != null)
            {

            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {

            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        private string GetNoiDungThayDoi(ThongBaoDieuChinhThongTinHoaDon model)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(model.TenDonViCu) || !string.IsNullOrEmpty(model.TenDonViMoi))
            {
                list.Add($"Tên đơn vị từ <{model.TenDonViCu}> thành <{model.TenDonViMoi}>");
            }
            if (!string.IsNullOrEmpty(model.DiaChiCu) || !string.IsNullOrEmpty(model.DiaChiMoi))
            {
                list.Add($"Địa chỉ từ <{model.DiaChiCu}> thành <{model.DiaChiMoi}>");
            }
            if (!string.IsNullOrEmpty(model.DienThoaiCu) || !string.IsNullOrEmpty(model.DienThoaiMoi))
            {
                list.Add($"Điện thoại từ <{model.DienThoaiCu}> thành &lt;{model.DienThoaiMoi}>");
            }

            string result = "Thông tin thay đổi: " + string.Join(';', list.ToArray()) + ".";
            return result;
        }

        public async Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetBangKeHoaDonChuaSuDungAsync(string id)
        {
            var query = from mhd in _db.MauHoaDons
                        join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                        join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                        join tbdcct in _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == id)
                        on mhd.MauHoaDonId equals tbdcct.MauHoaDonId into tmpTBDCCTs
                        from tbdcct in tmpTBDCCTs.DefaultIfEmpty()
                        where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                        select new ThongBaoDieuChinhThongTinHoaDonChiTietViewModel
                        {
                            MauHoaDonId = mhd.MauHoaDonId,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            TuSo = tbphct.TuSo,
                            DenSo = tbphct.DenSo,
                            SoLuong = tbphct.SoLuong,
                            Checked = tbdcct != null
                        };

            var result = await query.OrderBy(x => x.MauSo).ThenBy(x => x.KyHieu).ToListAsync();
            return result;
        }

        public async Task<ThongBaoDieuChinhThongTinHoaDonViewModel> GetByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongBaoDieuChinhThongTinHoaDon);
            string folder = $@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{id}\FileAttach";

            var query = from tb in _db.ThongBaoDieuChinhThongTinHoaDons
                        where tb.ThongBaoDieuChinhThongTinHoaDonId == id
                        select new ThongBaoDieuChinhThongTinHoaDonViewModel
                        {
                            ThongBaoDieuChinhThongTinHoaDonId = tb.ThongBaoDieuChinhThongTinHoaDonId,
                            NgayThongBaoDieuChinh = tb.NgayThongBaoDieuChinh,
                            NgayThongBaoPhatHanh = tb.NgayThongBaoPhatHanh,
                            CoQuanThue = tb.CoQuanThue,
                            So = tb.So,
                            TrangThaiHieuLuc = tb.TrangThaiHieuLuc,
                            TenDonViCu = tb.TenDonViCu,
                            TenDonViMoi = tb.TenDonViMoi,
                            DiaChiCu = tb.DiaChiCu,
                            DiaChiMoi = tb.DiaChiMoi,
                            DienThoaiCu = tb.DienThoaiCu,
                            DienThoaiMoi = tb.DienThoaiMoi,
                            ThongBaoDieuChinhThongTinHoaDonChiTiets = (from tbct in _db.ThongBaoDieuChinhThongTinHoaDonChiTiets
                                                                       orderby tb.CreatedDate
                                                                       select new ThongBaoDieuChinhThongTinHoaDonChiTietViewModel
                                                                       {
                                                                           ThongBaoDieuChinhThongTinHoaDonChiTietId = tbct.ThongBaoDieuChinhThongTinHoaDonChiTietId,
                                                                           ThongBaoDieuChinhThongTinHoaDonId = tbct.ThongBaoDieuChinhThongTinHoaDonId,
                                                                           MauHoaDonId = tbct.MauHoaDonId,
                                                                       })
                                                                       .ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == tb.ThongBaoDieuChinhThongTinHoaDonId
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
                            CreatedDate = tb.CreatedDate,
                            CreatedBy = tb.CreatedBy,
                            Status = tb.Status
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public List<EnumModel> GetTrangThaiHieuLucs()
        {
            List<EnumModel> enums = ((TrangThaiHieuLuc[])Enum.GetValues(typeof(TrangThaiHieuLuc)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public async Task<ThongBaoDieuChinhThongTinHoaDonViewModel> InsertAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> detailVMs = model.ThongBaoDieuChinhThongTinHoaDonChiTiets;

            model.ThongBaoDieuChinhThongTinHoaDonChiTiets = null;
            model.ThongBaoDieuChinhThongTinHoaDonId = string.IsNullOrEmpty(model.ThongBaoDieuChinhThongTinHoaDonId) ? Guid.NewGuid().ToString() : model.ThongBaoDieuChinhThongTinHoaDonId;
            ThongBaoDieuChinhThongTinHoaDon entity = _mp.Map<ThongBaoDieuChinhThongTinHoaDon>(model);
            await _db.ThongBaoDieuChinhThongTinHoaDons.AddAsync(entity);

            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoDieuChinhThongTinHoaDonId = entity.ThongBaoDieuChinhThongTinHoaDonId;
                var detail = _mp.Map<ThongBaoDieuChinhThongTinHoaDonChiTiet>(item);
                await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongBaoDieuChinhThongTinHoaDonViewModel result = _mp.Map<ThongBaoDieuChinhThongTinHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> detailVMs = model.ThongBaoDieuChinhThongTinHoaDonChiTiets;

            model.ThongBaoDieuChinhThongTinHoaDonChiTiets = null;
            ThongBaoDieuChinhThongTinHoaDon entity = await _db.ThongBaoDieuChinhThongTinHoaDons.FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == model.ThongBaoDieuChinhThongTinHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            try
            {
                List<ThongBaoDieuChinhThongTinHoaDonChiTiet> details = await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == model.ThongBaoDieuChinhThongTinHoaDonId).ToListAsync();
                _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.RemoveRange(details);
                foreach (var item in detailVMs)
                {
                    item.Status = true;
                    item.CreatedDate = DateTime.Now;
                    item.ThongBaoDieuChinhThongTinHoaDonId = entity.ThongBaoDieuChinhThongTinHoaDonId;
                    var detail = _mp.Map<ThongBaoDieuChinhThongTinHoaDonChiTiet>(item);
                    await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.AddAsync(detail);
                }
            }
            catch (Exception e)
            {

                throw;
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetThongBaoDieuChinhThongTinChiTietByIdAsync(string id)
        {
            var result = await GetBangKeHoaDonChuaSuDungAsync(id);
            var chiTiets = await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets
                .Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == id)
                .ToListAsync();

            result = result.Where(x => chiTiets.Any(y => y.MauHoaDonId == x.MauHoaDonId)).ToList();
            return result;
        }
    }
}
