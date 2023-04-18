using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ImageMagick;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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
            string stringMSTNhaPhatHanh = await GetPhatHanhBoiFromHopDong();
            if (entity == null)
            {
                var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
                entity = new HoSoHDDT { MaSoThue = taxCode, KyTinhThue = KyKeKhaiThue.Thang };
            }

            var result = _mp.Map<HoSoHDDTViewModel>(entity);

            if (!string.IsNullOrEmpty(result.CoQuanThueQuanLy) && string.IsNullOrEmpty(result.TenCoQuanThueQuanLy))
            {
                result.TenCoQuanThueQuanLy = GetListCoQuanThueQuanLy().FirstOrDefault(x => x.Code == result.CoQuanThueQuanLy)?.Name;
            }

            if (!string.IsNullOrEmpty(result.CoQuanThueCapCuc) && string.IsNullOrEmpty(result.TenCoQuanThueCapCuc))
            {
                result.TenCoQuanThueCapCuc = GetListCoQuanThueCapCuc().FirstOrDefault(x => x.code == result.CoQuanThueCapCuc)?.name;
            }


            if (entity != null)
            {
                var diaDanh = await _db.CoQuanThueCapCuc_DiaDanhs.FirstOrDefaultAsync(x => x.MaCQT == entity.CoQuanThueCapCuc);
                result.MaDiaDanhCQTCapCuc = diaDanh?.MaDiaDanh;
            }
            if (!string.IsNullOrEmpty(stringMSTNhaPhatHanh))
            {
                result.PhatHanhBoi = stringMSTNhaPhatHanh;
            }
            return result;
        }

        public List<CityParam> GetListCoQuanThueCapCuc()
        {
            var list = _db.CoQuanThues.Where(x => string.IsNullOrEmpty(x.MaCQTCapCuc) || (!_db.CoQuanThues.Any(o => o.Ma == x.MaCQTCapCuc && string.IsNullOrEmpty(o.MaCQTCapCuc)) && x.MaCQTCapCuc == x.Ma))
                                      .Select(x => new CityParam
                                      {
                                          code = x.Ma,
                                          name = x.Ten
                                      }).DistinctBy(x => x.code).ToList();
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
            try
            {
                var entity = _mp.Map<HoSoHDDT>(model);
                entity.HoSoHDDTId = Guid.NewGuid().ToString();
                entity.TenCoQuanThueCapCuc = (await _db.CoQuanThues.FirstOrDefaultAsync(x => x.Ma == entity.CoQuanThueCapCuc)).Ten;
                entity.TenCoQuanThueQuanLy = (await _db.CoQuanThues.FirstOrDefaultAsync(x => x.Ma == entity.CoQuanThueQuanLy)).Ten;
                await _db.HoSoHDDTs.AddAsync(entity);
                await _db.SaveChangesAsync();
                var result = _mp.Map<HoSoHDDTViewModel>(entity);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(HoSoHDDTViewModel model)
        {
            var entity = await _db.HoSoHDDTs.FirstOrDefaultAsync(x => x.HoSoHDDTId == model.HoSoHDDTId);
            model.TenCoQuanThueCapCuc = (await _db.CoQuanThues.FirstOrDefaultAsync(x => x.Ma == model.CoQuanThueCapCuc)).Ten;
            model.TenCoQuanThueQuanLy = (await _db.CoQuanThues.FirstOrDefaultAsync(x => x.Ma == model.CoQuanThueQuanLy)).Ten;
            model.TenDonVi = TextHelper.TrimSpaceFristAndLastString(model.TenDonVi);
            model.HoTenNguoiDaiDienPhapLuat = TextHelper.TrimSpaceFristAndLastString(model.HoTenNguoiDaiDienPhapLuat);
            model.DiaChi = TextHelper.TrimSpaceFristAndLastString(model.DiaChi);
            /// Những cột ẩn
            model.MaCuaCQTToKhaiChapNhan = entity.MaCuaCQTToKhaiChapNhan;
            model.FileId103Import = entity.FileId103Import;
            model.IsNhapKhauMaCQT = entity.IsNhapKhauMaCQT;
            model.MaThongDiepChuaMCQT = entity.MaThongDiepChuaMCQT;
            model.ThongBaoChiTietMaCuaCQT = entity.ThongBaoChiTietMaCuaCQT;

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
            //IQueryable<ToKhaiDangKyThongTinViewModel> query = from tdc in _db.ThongDiepChungs
            //                                                  join tk in _db.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id
            //                                                  join hs in _db.HoSoHDDTs on tdc.MaSoThue equals hs.MaSoThue
            //                                                  where (tdc.MaLoaiThongDiep == 100 || tdc.MaLoaiThongDiep == 101) && tdc.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan
            //                                                  orderby tdc.CreatedDate descending
            //                                                  select new ToKhaiDangKyThongTinViewModel
            //                                                  {
            //                                                      Id = tk.Id,
            //                                                      NgayTao = tk.NgayTao,
            //                                                      IsThemMoi = tk.IsThemMoi,
            //                                                      FileXMLChuaKy = tk.FileXMLChuaKy,
            //                                                      ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
            //                                                      ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(_db.FileDatas.FirstOrDefault(x => x.RefId == tk.Id).Content),
            //                                                      NhanUyNhiem = tk.NhanUyNhiem,
            //                                                      LoaiUyNhiem = tk.LoaiUyNhiem,
            //                                                      SignedStatus = tk.SignedStatus,
            //                                                      NgayGui = tdc != null ? tdc.NgayGui : null,
            //                                                      ModifyDate = tk.ModifyDate,
            //                                                      PPTinh = tk.PPTinh
            //                                                  };
            //var toKhai = await query.FirstOrDefaultAsync();
            //if (toKhai != null)
            //{
            //    if(toKhai.ToKhaiKhongUyNhiem != null)
            //        result = toKhai.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.DSCTSSDung
            //            .Select(x => new ChungThuSoSuDungViewModel
            //            {
            //                TTChuc = x.TTChuc,
            //                Seri = x.Seri,
            //                TNgay = x.TNgay,
            //                DNgay = x.DNgay,
            //                HThuc = x.HThuc,
            //                TenHThuc = ((HThuc)x.HThuc).GetDescription(),
            //            })
            //            .ToList();
            //    else
            //    {
            //        result = toKhai.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSCTSSDung
            //        .Select(x => new ChungThuSoSuDungViewModel
            //        {
            //            TTChuc = x.TTChuc,
            //            Seri = x.Seri,
            //            TNgay = x.TNgay,
            //            DNgay = x.DNgay,
            //            HThuc = x.HThuc,
            //            TenHThuc = ((HThuc)x.HThuc).GetDescription(),
            //            IsAddInTTNNT = false
            //        })
            //        .ToList();
            //    }

            //    foreach(var item in result)
            //    {
            //        item.Id = await _db.ChungThuSoSuDungs.Where(x => x.Seri == item.Seri && x.TNgay == item.TNgay && x.DNgay == item.DNgay && !x.IsAddInTTNNT).Select(x => x.Id).FirstOrDefaultAsync();
            //        if(string.IsNullOrEmpty(item.Id))
            //        {
            //            item.Id = Guid.NewGuid().ToString();
            //            await _db.ChungThuSoSuDungs.AddAsync(_mp.Map<ChungThuSoSuDung>(item));
            //            await _db.SaveChangesAsync();
            //        }
            //    }
            //}

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
                                IsAddInTTNNT = x.IsAddInTTNNT,
                                TypeSign=x.TypeSign
                            })
                            .ToListAsync();
            result = listThemTuTTNNT;
            return result;
        }
        /// <summary>
        /// Insert mã cqt khi nhận đc ở thông điệp 103 có chọn hình thức hóa đơn là có mã từ máy tính tiền lần đầu
        /// </summary>
        /// <param name="MaCuaCQTToKhaiChapNhan"></param>
        /// <returns></returns>
        public async Task<bool> InsertMCCQTToKhaiAsync(DLieu DLieu, string ThongDiepChung100)
        {
            try
            {
                var entity = _db.HoSoHDDTs.FirstOrDefault();
                entity.MaCuaCQTToKhaiChapNhan = DLieu.TBao.DLTBao.MCCQT ?? string.Empty;
                entity.ThongBaoChiTietMaCuaCQT = String.Format("Thông báo {0} số {1} ngày {2}", DLieu.TBao.DLTBao.MSo, DLieu.TBao.STBao.So, DateTime.Parse(DLieu.TBao.STBao.NTBao).ToString("dd/MM/yyyy"));
                entity.MaThongDiepChuaMCQT = ThongDiepChung100;
                _db.HoSoHDDTs.Update(entity);
                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get Phat Hanh Boi From Hop Dong
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPhatHanhBoiFromHopDong()
        {
            string result = "0202029650";
            try
            {
                List<HopDongHoaDon> listHopDongMoiNhat = await _db.HopDongHoaDons.Where(x => x.TrangThaiHopDong == 2 && x.NgayDuyet != null).OrderByDescending(x => x.NgayDuyet).ToListAsync();
                if (listHopDongMoiNhat.Count() > 0)
                {
                    result = listHopDongMoiNhat[0].MstGp;
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog("Error Phat Hanh Boi From Hop Dpng" + ex.Message);
            }
            return result;


        }
    }
}
