using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class HoSoHDDTService : IHoSoHDDTService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HoSoHDDTService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HoSoHDDTViewModel> GetDetailAsync()
        {
            var entity = await _db.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
            if (entity == null)
            {
                var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
                entity = new HoSoHDDT { MaSoThue = taxCode };
            }

            var result = _mp.Map<HoSoHDDTViewModel>(entity);

            if (!string.IsNullOrEmpty(result.CoQuanThueQuanLy))
            {
                result.TenCoQuanThueQuanLy = GetListCoQuanThueQuanLy().FirstOrDefault(x => x.Code == result.CoQuanThueQuanLy)?.Name;
            }

            if (entity != null)
            {
                var diaDanh = await _db.CoQuanThueCapCuc_DiaDanhs.FirstOrDefaultAsync(x => x.MaCQT == entity.CoQuanThueCapCuc);
                result.MaDiaDanhCQTCapCuc = diaDanh?.MaDiaDanh;
            }

            return result;
        }

        public List<CityParam> GetListCoQuanThueCapCuc()
        {
            var list = _db.CoQuanThues.Where(x => string.IsNullOrEmpty(x.MaCQTCapCuc) || x.MaCQTCapCuc == x.Ma)
                                      .Select(x => new CityParam
                                      {
                                          code = x.Ma,
                                          name = x.Ten
                                      }).ToList();
            foreach (var item in list)
            {
                item.name = item.name.Replace("–", "-");
                item.parent_code = _db.CoQuanThueCapCuc_DiaDanhs.Where(x => x.MaCQT == item.code).Select(x => x.MaDiaDanh).FirstOrDefault();
            }
            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public List<DistrictsParam> GetListCoQuanThueQuanLy()
        {
            var list = _db.CoQuanThues.Where(x => !string.IsNullOrEmpty(x.MaCQTCapCuc))
                          .Select(x => new DistrictsParam
                          {
                              Code = x.Ma,
                              Name = x.Ten.Replace("–", "-"),
                              Parent_code = x.MaCQTCapCuc
                          })
                          .OrderBy(x => x.Parent_code)
                          .ToList();

            return list;
        }

        public async Task<HoSoHDDTViewModel> InsertAsync(HoSoHDDTViewModel model)
        {
            var entity = _mp.Map<HoSoHDDT>(model);
            await _db.HoSoHDDTs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<HoSoHDDTViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(HoSoHDDTViewModel model)
        {
            var entity = await _db.HoSoHDDTs.FirstOrDefaultAsync(x => x.HoSoHDDTId == model.HoSoHDDTId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public List<CityParam> GetListCity()
        {
            string path = _hostingEnvironment.WebRootPath + "\\jsons\\city.json";
            var list = _db.DiaDanhs.Select(x => new CityParam
            {
                code = x.Ma,
                name = x.Ten
            }).ToList();
            return list;
        }

        public async Task<List<ChungThuSoSuDungViewModel>> GetDanhSachChungThuSoSuDung()
        {
            var result = new List<ChungThuSoSuDungViewModel>();
            IQueryable<ToKhaiDangKyThongTinViewModel> query = from tdc in _db.ThongDiepChungs
                                                              join tk in _db.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id
                                                              join hs in _db.HoSoHDDTs on tdc.MaSoThue equals hs.MaSoThue
                                                              where (tdc.MaLoaiThongDiep == 100 || tdc.MaLoaiThongDiep == 101) && tdc.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
                                                              orderby tdc.CreatedDate descending
                                                              select new ToKhaiDangKyThongTinViewModel
                                                              {
                                                                  Id = tk.Id,
                                                                  NgayTao = tk.NgayTao,
                                                                  IsThemMoi = tk.IsThemMoi,
                                                                  FileXMLChuaKy = tk.FileXMLChuaKy,
                                                                  ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                                                  ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
                                                                  NhanUyNhiem = tk.NhanUyNhiem,
                                                                  LoaiUyNhiem = tk.LoaiUyNhiem,
                                                                  SignedStatus = tk.SignedStatus,
                                                                  NgayGui = tdc != null ? tdc.NgayGui : null,
                                                                  ModifyDate = tk.ModifyDate,
                                                                  PPTinh = tk.PPTinh
                                                              };
            var toKhai = await query.FirstOrDefaultAsync();
            if (toKhai != null)
            {
                if(toKhai.ToKhaiKhongUyNhiem != null)
                    result = toKhai.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung
                        .Select(x => new ChungThuSoSuDungViewModel
                        {
                            TTChuc = x.TTChuc,
                            Seri = x.Seri,
                            TNgay = x.TNgay,
                            DNgay = x.DNgay,
                            HThuc = x.HThuc,
                            TenHThuc = ((HThuc)x.HThuc).GetDescription(),
                        })
                        .ToList();
                else
                {
                    result = toKhai.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung
                    .Select(x => new ChungThuSoSuDungViewModel
                    {
                        TTChuc = x.TTChuc,
                        Seri = x.Seri,
                        TNgay = x.TNgay,
                        DNgay = x.DNgay,
                        HThuc = x.HThuc,
                        TenHThuc = ((HThuc)x.HThuc).GetDescription(),
                        IsAddInTTNNT = false
                    })
                    .ToList();
                }

                foreach(var item in result)
                {
                    item.Id = await _db.ChungThuSoSuDungs.Where(x => x.Seri == item.Seri && x.TNgay == item.TNgay && x.DNgay == item.DNgay && !x.IsAddInTTNNT).Select(x => x.Id).FirstOrDefaultAsync();
                    if(string.IsNullOrEmpty(item.Id))
                    {
                        item.Id = Guid.NewGuid().ToString();
                        await _db.ChungThuSoSuDungs.AddAsync(_mp.Map<ChungThuSoSuDung>(item));
                        await _db.SaveChangesAsync();
                    }
                }
            }

            var listThemTuTTNNT = await _db.ChungThuSoSuDungs
                            .Where(x => x.IsAddInTTNNT == true)
                            .Select(x => new ChungThuSoSuDungViewModel
                            {
                                Id = x.Id,
                                TTChuc = x.TTChuc,
                                Seri = x.Seri,
                                TNgay = x.TNgay,
                                DNgay = x.DNgay,
                                HThuc = x.HThuc,
                                TenHThuc = ((HThuc)x.HThuc).GetDescription(),
                                IsAddInTTNNT = x.IsAddInTTNNT
                            })
                            .ToListAsync();
            result = result.Union(listThemTuTTNNT).ToList();
            return result;
        }
    }
}
