using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Services.ViewModels;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Services.Repositories.Interfaces;
using DLL.Enums;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces.DanhMuc;

namespace Services.Repositories.Implimentations
{
    public class HopDongHoaDonService : IHopDongHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration iConfiguration;
        private readonly IMauHoaDonService _mauHoaDonService;

        public HopDongHoaDonService(
            Datacontext datacontext,
            IMapper mapper,
            IConfiguration IConfiguration,
            IHostingEnvironment hostingEnvironment,
            IMauHoaDonService mauHoaDonService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            iConfiguration = IConfiguration;
            _mauHoaDonService = mauHoaDonService;
        }

        /// <summary>
        /// Hàm check hợp đồng có tồn tại hay không
        /// </summary>
        /// <param name="id">Hợp đồng hóa đơn Id</param>
        /// <returns></returns>
        public async Task<bool> CheckExistsAsync(string id)
        {
            HopDongHoaDon result = await _db.HopDongHoaDons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.HopDongHoaDonId == id);
            return result != null ? true : false;
        }

        /// <summary>
        /// Hàm insert hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(HopDongHoaDonViewModel model)
        {
            try
            {
                model.HopDongHoaDonId = Guid.NewGuid().ToString();
                string nameConfig = string.Format("TTMstGp:MstGp{0}:MNGui", model.LoaiCongTy);
                model.MstGp = iConfiguration[nameConfig] ?? "0202029650";
                HopDongHoaDon hopDongHoaDon = _mp.Map<HopDongHoaDon>(model);
                HopDongHoaDon hopDongMoiNhat = await _db.HopDongHoaDons.Where(x => x.TrangThaiHopDong == 2 && x.NgayDuyet != null).OrderByDescending(x => x.NgayDuyet).FirstOrDefaultAsync();
                await _db.HopDongHoaDons.AddAsync(hopDongHoaDon);
                int result = await _db.SaveChangesAsync();
                /// Run Update Mau Hoa Don 
                if (hopDongMoiNhat != null)
                {
                    if (hopDongMoiNhat.MstGp != model.MstGp)
                    {
                        await _mauHoaDonService.XacThucMauHoaDonWhenChangeHopDong();

                    }
                }
                return result > 0;

            }
            catch (Exception ex)
            {
                Tracert.WriteLog("Bug Create Hop Dong" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm lấy toàn bộ hợp đồng không phân trang
        /// </summary>
        /// <returns></returns>
        public async Task<IList<HopDongHoaDonViewModel>> GetAllAsync()
        {
            IList<HopDongHoaDonViewModel> result = await _db.HopDongHoaDons
                 .OrderByDescending(x => x.CreatedDate)
                 .ThenByDescending(x => x.SoHopDong)
                 .ProjectTo<HopDongHoaDonViewModel>(_mp.ConfigurationProvider)
                 .ToListAsync();

            return result;
        }

        /// <summary>
        /// Hàm lấy toàn bộ hợp đồng theo mst truyền vào
        /// </summary>
        /// <returns></returns>
        public async Task<List<HopDongHoaDonViewModel>> GetHopDongByTaxcodeAsync(string taxcode)
        {
            var query = await (from main in _db.HopDongHoaDons
                               where main.MaSoThue == taxcode
                               select new HopDongHoaDonViewModel
                               {
                                   TongThanhTien = main.TongThanhTien,
                                   TongThanhToan = main.TongThanhToan,
                                   TienChietKhau = main.TienChietKhau,
                                   HopDongHoaDonId = main.HopDongHoaDonId,
                                   NgayLap = main.NgayLap,
                                   NgayDuyet = main.NgayDuyet,
                                   MauHopDongId = main.MauHopDongId,
                                   KhachHangId = main.KhachHangId,
                                   SoHopDong = main.SoHopDong ?? string.Empty,
                                   TenKhachHang = main.TenKhachHang ?? string.Empty,
                                   NguoiDaiDien = main.NguoiDaiDien ?? string.Empty,
                                   ChucVu = main.ChucVu ?? string.Empty,
                                   DiaChi = main.DiaChi ?? string.Empty,
                                   MaSoThue = main.MaSoThue ?? string.Empty,
                                   SoDienThoai = main.SoDienThoai ?? string.Empty,
                                   Fax = main.Fax ?? string.Empty,
                                   Email = main.Email ?? string.Empty,
                                   SoTaiKhoan = main.SoTaiKhoan ?? string.Empty,
                                   NganHangMo = main.NganHangMo ?? string.Empty,
                                   NguoiLienHe = main.NguoiLienHe ?? string.Empty,
                                   SoDienThoaiNguoiLienHe = main.SoDienThoaiNguoiLienHe ?? string.Empty,
                                   CreatedBy = main.CreatedBy.ToLower() ?? string.Empty,
                                   website = main.website ?? string.Empty,
                                   pathFilePDF = main.pathFilePDF ?? string.Empty,
                                   GiaiDoan = main.GiaiDoan,
                                   MstGp = main.MstGp,
                                   LoaiCongTy = main.LoaiCongTy,
                                   IsGiaHan = main.IsGiaHan,
                                   LoaiHopDong = main.LoaiHopDong
                               }).ToListAsync();

            return query;
        }

        /// <summary>
        /// Hàm lấy toàn bộ hợp đồng theo phân trang
        /// </summary>
        /// <param name="pagingParams">Thông tin trong 1 page</param>
        /// <returns></returns>
        public async Task<PagedList<HopDongHoaDonViewModel>> GetAllPagingAsync(PagingParams pagingParams)
        {

            var query = from main in _db.HopDongHoaDons
                        orderby main.NgayLap descending, main.SoHopDong descending
                        select new HopDongHoaDonViewModel
                        {
                            TongThanhTien = main.TongThanhTien,
                            TongThanhToan = main.TongThanhToan,
                            TienChietKhau = main.TienChietKhau,
                            HopDongHoaDonId = main.HopDongHoaDonId,
                            NgayLap = main.NgayLap,
                            MauHopDongId = main.MauHopDongId,
                            KhachHangId = main.KhachHangId,
                            SoHopDong = main.SoHopDong ?? string.Empty,
                            TenKhachHang = main.TenKhachHang ?? string.Empty,
                            NguoiDaiDien = main.NguoiDaiDien ?? string.Empty,
                            ChucVu = main.ChucVu ?? string.Empty,
                            DiaChi = main.DiaChi ?? string.Empty,
                            MaSoThue = main.MaSoThue ?? string.Empty,
                            SoDienThoai = main.SoDienThoai ?? string.Empty,
                            Fax = main.Fax ?? string.Empty,
                            Email = main.Email ?? string.Empty,
                            SoTaiKhoan = main.SoTaiKhoan ?? string.Empty,
                            NganHangMo = main.NganHangMo ?? string.Empty,
                            NguoiLienHe = main.NguoiLienHe ?? string.Empty,
                            SoDienThoaiNguoiLienHe = main.SoDienThoaiNguoiLienHe ?? string.Empty,
                            CreatedBy = main.CreatedBy.ToLower() ?? string.Empty,
                            website = main.website ?? string.Empty,
                            pathFilePDF = main.pathFilePDF ?? string.Empty,
                            GiaiDoan = main.GiaiDoan
                        };

            if (!string.IsNullOrEmpty(pagingParams.FromDate) && !string.IsNullOrEmpty(pagingParams.ToDate))
            {
                DateTime fromDate = DateTime.Parse(pagingParams.FromDate);
                DateTime toDate = DateTime.Parse(pagingParams.ToDate);
                query = query.Where(x => x.NgayLap >= fromDate && x.NgayLap <= toDate);
            }

            if (!string.IsNullOrEmpty(pagingParams.Keyword))
            {
                var keyword = pagingParams.Keyword.ToUpper().ToTrim();

                query = query.Where(x => (x.SoHopDong ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.SoHopDong ?? string.Empty).ToUpper().Contains(keyword) ||
                                        (x.TenKhachHang ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.TenKhachHang ?? string.Empty).ToUpper().Contains(keyword) ||
                                        (x.NguoiDaiDien ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.NguoiDaiDien ?? string.Empty).ToUpper().Contains(keyword) ||
                                        (x.DiaChi ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.DiaChi ?? string.Empty).ToUpper().Contains(keyword) ||
                                        (x.MaSoThue ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.MaSoThue ?? string.Empty).ToUpper().Contains(keyword) ||
                                        (x.SoDienThoai ?? string.Empty).ToUpper().ToUnSign().Contains(keyword.ToUnSign()) ||
                                        (x.SoDienThoai ?? string.Empty).ToUpper().Contains(keyword));
            }

            if (!string.IsNullOrEmpty(pagingParams.SortKey))
            {
                if (pagingParams.SortKey == "soHopDong" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoHopDong);
                }
                if (pagingParams.SortKey == "soHopDong" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoHopDong);
                }

                if (pagingParams.SortKey == "ngayLap" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.NgayLap);
                }
                if (pagingParams.SortKey == "ngayLap" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.NgayLap);
                }

                if (pagingParams.SortKey == "tenKhachHang" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.TenKhachHang);
                }
                if (pagingParams.SortKey == "tenKhachHang" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.TenKhachHang);
                }

                if (pagingParams.SortKey == "maSoThue" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.MaSoThue);
                }
                if (pagingParams.SortKey == "maSoThue" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.MaSoThue);
                }

                if (pagingParams.SortKey == "diaChi" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.DiaChi);
                }
                if (pagingParams.SortKey == "diaChi" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.DiaChi);
                }

                if (pagingParams.SortKey == "soDienThoai" && pagingParams.SortValue == "ascend")
                {
                    query = query.OrderBy(x => x.SoDienThoai);
                }
                if (pagingParams.SortKey == "soDienThoai" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.SoDienThoai);
                }
                if (pagingParams.SortKey == "website" && pagingParams.SortValue == "descend")
                {
                    query = query.OrderByDescending(x => x.website);
                }

            }

            return await PagedList<HopDongHoaDonViewModel>.CreateAsync(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        /// <summary>
        /// Hàm lấy thông tin hợp đồng theo Id
        /// </summary>
        /// <param name="id">Hợp đồng hóa đơn Id</param>
        /// <returns></returns>
        public async Task<HopDongHoaDonViewModel> GetByIdAsync(string id)
        {
            HopDongHoaDonViewModel query = (from hd in _db.HopDongHoaDons
                                            where hd.HopDongHoaDonId == id
                                            select new HopDongHoaDonViewModel
                                            {
                                                HopDongHoaDonId = hd.HopDongHoaDonId,
                                                NgayLap = hd.NgayLap,
                                                MauHopDongId = hd.MauHopDongId,
                                                KhachHangId = hd.KhachHangId,
                                                SoHopDong = hd.SoHopDong,
                                                TenKhachHang = hd.TenKhachHang,
                                                NguoiDaiDien = hd.NguoiDaiDien,
                                                ChucVu = hd.ChucVu,
                                                DiaChi = hd.DiaChi,
                                                MaSoThue = hd.MaSoThue,
                                                SoDienThoai = hd.SoDienThoai,
                                                Fax = hd.Fax,
                                                Email = hd.Email,
                                                SoTaiKhoan = hd.SoTaiKhoan,
                                                NganHangMo = hd.NganHangMo,
                                                NguoiLienHe = hd.NguoiLienHe ?? string.Empty,
                                                SoDienThoaiNguoiLienHe = hd.SoDienThoaiNguoiLienHe ?? string.Empty,
                                                GiaiDoan = hd.GiaiDoan,
                                                //Thông tin phụ lục
                                                GoiBan = hd.GoiBan,
                                                SoLuongBan = hd.SoLuongBan,
                                                DonGiaBan = hd.DonGiaBan,
                                                ThanhTienBan = hd.ThanhTienBan,
                                                TongThanhTien = hd.TongThanhTien,
                                                TienThueGTGT = hd.TienThueGTGT,
                                                TongThanhToan = hd.TongThanhToan,
                                                SoLuongTang = hd.SoLuongTang,
                                                DonGiaTang = hd.DonGiaTang,
                                                ThanhTienTang = hd.ThanhTienTang,
                                                TienChietKhau = hd.TienChietKhau,
                                                TongThanhToanBangChu = hd.TongThanhToanBangChu,
                                                SoftWareRef = hd.SoftWareRef,
                                                website = hd.website,
                                                KyHieu = hd.KyHieu,
                                                TuSo = hd.TuSo,
                                                DenSo = hd.DenSo,
                                                TongSoLuong = hd.TongSoLuong,
                                                pathFilePDF = hd.pathFilePDF,
                                                //Thông tin hỗ trợ
                                                NgayThiHanh = hd.NgayThiHanh.ToString("yyyy-MM-dd"),
                                                TenCucThue = hd.TenCucThue,
                                                CongTyPhatTrienKeToan = hd.CongTyPhatTrienKeToan,
                                                LoaiHoaDon = hd.LoaiHoaDon,
                                                NoiLap = hd.NoiLap,
                                                LoaiUSB = hd.LoaiUSB,
                                                SoSeriUSB = hd.SoSeriUSB,
                                                TuNgayUSB = hd.TuNgayUSB.ToString("yyyy-MM-dd"),
                                                DenNgayUSB = hd.DenNgayUSB.ToString("yyyy-MM-dd"),
                                                ChungThuSo = hd.ChungThuSo,
                                                MauSoHoaDon = hd.MauSoHoaDon,
                                                NganhKinhDoanh = hd.NganhKinhDoanh,
                                                CreatedBy = hd.CreatedBy.ToLower(),
                                                CreatedDate = hd.CreatedDate,
                                                ModifyBy = hd.ModifyBy,
                                                ModifyDate = hd.ModifyDate
                                            }).FirstOrDefault();
            return query;
        }


        /// <summary>
        /// Hàm tính tổng giá trị hợp đồng
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> SumGiaTriHopDongAsync()
        {
            decimal sum = await _db.HopDongHoaDons.AsNoTracking()
                .SumAsync(x => x.TongThanhToan);
            return sum;
        }

        /// <summary>
        /// Hàm update thông tin hợp đồng hóa đơn
        /// </summary>
        /// <param name="model">Thông tin cần update</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(HopDongHoaDonViewModel model)
        {
            try
            {
                HopDongHoaDon hopDongHoaDon = _mp.Map<HopDongHoaDon>(model);
                hopDongHoaDon.NgayLap = model.NgayLap.Value;
                _db.HopDongHoaDons.Update(hopDongHoaDon);
                int result = await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
