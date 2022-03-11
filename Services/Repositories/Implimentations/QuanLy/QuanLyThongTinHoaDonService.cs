using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels.QuanLy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLy
{
    public class QuanLyThongTinHoaDonService : IQuanLyThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public QuanLyThongTinHoaDonService(
            Datacontext dataContext,
            IMapper mp)
        {
            _db = dataContext;
            _mp = mp;
        }

        public async Task<List<QuanLyThongTinHoaDonViewModel>> GetListByHinhThucVaLoaiHoaDonAsync(HinhThucHoaDon hinhThucHoaDon, LoaiHoaDon loaiHoaDon)
        {
            var loaiThongTinHinhThucHoaDon = hinhThucHoaDon == HinhThucHoaDon.CoMa ? LoaiThongTinChiTiet.CoMaCuaCoQuanThue : LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue;

            var loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.HoaDonGTGT;
            switch (loaiHoaDon)
            {
                case LoaiHoaDon.HoaDonBanHang:
                    loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.HoaDonBanHang;
                    break;
                case LoaiHoaDon.HoaDonBanTaiSanCong:
                    loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.HoaDonBanTaiSanCong;
                    break;
                case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                    loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia;
                    break;
                case LoaiHoaDon.CacLoaiHoaDonKhac:
                    loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.CacLoaiHoaDonKhac;
                    break;
                case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                    loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon;
                    break;
                default:
                    break;
            }

            var result = await _db.QuanLyThongTinHoaDons
                .Where(x => x.LoaiThongTinChiTiet == loaiThongTinHinhThucHoaDon || x.LoaiThongTinChiTiet == loaiThongTinLoaiHoaDon)
                .ProjectTo<QuanLyThongTinHoaDonViewModel>(_mp.ConfigurationProvider)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Get danh sách thông tin hóa đơn theo loại
        /// </summary>
        /// <param name="loaiThongTin">1: Hình thức hóa đơn, 2: Loại hóa đơn</param>
        /// <returns></returns>
        public async Task<List<QuanLyThongTinHoaDonViewModel>> GetListByLoaiThongTinAsync(int? loaiThongTin)
        {
            var result = await _db.QuanLyThongTinHoaDons
                .Where(x => loaiThongTin.HasValue == false || x.LoaiThongTin == loaiThongTin)
                .Select(x => new QuanLyThongTinHoaDonViewModel
                {
                    QuanLyThongTinHoaDonId = x.QuanLyThongTinHoaDonId,
                    STT = x.STT,
                    LoaiThongTin = x.LoaiThongTin,
                    LoaiThongTinChiTiet = x.LoaiThongTinChiTiet,
                    TenLoaiThongTinChiTiet = x.LoaiThongTinChiTiet.GetDescription(),
                    TrangThaiSuDung = x.TrangThaiSuDung,
                    TenTrangThaiSuDung = x.TrangThaiSuDung.GetDescription(),
                    NgayBatDauSuDung = x.NgayBatDauSuDung,
                    TuNgayTamNgungSuDung = x.TuNgayTamNgungSuDung,
                    DenNgayTamNgungSuDung = x.DenNgayTamNgungSuDung,
                    NgayNgungSuDung = x.NgayNgungSuDung
                })
                .OrderBy(x => x.STT)
                .ToListAsync();

            return result;
        }
    }
}
