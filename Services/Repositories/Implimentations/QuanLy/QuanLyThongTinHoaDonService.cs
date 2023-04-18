using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using DLL.Entity.TienIch;
using DLL.Enums;
using ImageMagick;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math;
using Services.Enums;
using Services.Helper;
using Services.Repositories.Interfaces.QuanLy;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.QuanLy;
using Services.ViewModels.TienIch;
using Services.ViewModels.XML.HoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using BigInteger = System.Numerics.BigInteger;

namespace Services.Repositories.Implimentations.QuanLy
{
    public class QuanLyThongTinHoaDonService : IQuanLyThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;

        public QuanLyThongTinHoaDonService(
            Datacontext dataContext,
            IMapper mp,
            INhatKyTruyCapService nhatKyTruyCapService)
        {
            _db = dataContext;
            _mp = mp;
            _nhatKyTruyCapService = nhatKyTruyCapService;
        }

        public async Task<List<QuanLyThongTinHoaDonViewModel>> GetListByHinhThucVaLoaiHoaDonAsync(HinhThucHoaDon hinhThucHoaDon, LoaiHoaDon loaiHoaDon)
        {
            var loaiThongTinHinhThucHoaDon = hinhThucHoaDon == HinhThucHoaDon.CoMa ? LoaiThongTinChiTiet.CoMaCuaCoQuanThue : LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue;
            /// Fix tạm nếu là bộ ký hiệu của hóa đơn có mã từ máy tính tiền => open sửa + xác nhận
            if (hinhThucHoaDon == HinhThucHoaDon.CoMaTuMayTinhTien)
            {
                loaiThongTinHinhThucHoaDon = LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien;
            }
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
                case LoaiHoaDon.HoaDonGTGTCMTMTT:
                    loaiThongTinLoaiHoaDon = LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien;
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

        /// <summary>
        /// update trạng thái sử dụng cho khách hàng chưa có thông tin hóa đơn
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<bool> UpdateTrangThaiSuDungTruocDoAsync()
        {
            // get thong diep gui duoc chap nhan 100
            var thongDiep100s = await _db.ThongDiepChungs
                .Where(x => (x.MaLoaiThongDiep == 100) && (x.TrangThaiGui == (int)TrangThaiGuiThongDiep.ChapNhan))
                .OrderBy(x => x.NgayThongBao)
                .AsNoTracking()
                .ToListAsync();

            // get file of thong diep 100
            var fileDatas = await _db.FileDatas
                .Where(x => thongDiep100s.Select(y => y.IdThamChieu).Contains(x.RefId))
                .AsNoTracking()
                .ToListAsync();

            var thongTinHoaDons = await _db.QuanLyThongTinHoaDons.ToListAsync();

            // list sub
            List<QuanLyThongTinHoaDon> listCon = new List<QuanLyThongTinHoaDon>();

            // declare hình thức hóa đơn hoặc loại hóa đơn ngừng sử dụng
            List<HinhThucVaLoaiHoaDonNgungSuDung> listNgungSuDung = new List<HinhThucVaLoaiHoaDonNgungSuDung>();

            int length = thongDiep100s.Count;
            for (int i = 0; i < length; i++)
            {
                var tDiep = thongDiep100s[i];

                // get xml content of 100
                var xmlContent = fileDatas.FirstOrDefault(x => x.RefId == tDiep.IdThamChieu).Content;

                // convert xml to model
                var tDiep100 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(xmlContent);

                if (i == 0) // to khai dau tien duoc chap nhan
                {
                    foreach (var thongTinHoaDon in thongTinHoaDons)
                    {
                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CoMaCuaCoQuanThue)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonGTGT)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHang)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanTaiSanCong)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacLoaiHoaDonKhac)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1 && thongTinHoaDon.LoaiThongTinChiTiet == LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon)
                        {
                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                        }
                    }
                }
                else // nếu là tờ khai tiếp theo
                {
                    foreach (var thongTinHoaDon in thongTinHoaDons)
                    {
                        switch (thongTinHoaDon.LoaiThongTinChiTiet)
                        {
                            case LoaiThongTinChiTiet.TamNgungSuDung:
                                break;
                            case LoaiThongTinChiTiet.CoMaCuaCoQuanThue:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.CMa == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.HTHDon.KCMa == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.HoaDonGTGT:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.HoaDonBanHang:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.HoaDonBanTaiSanCong:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBTSCong == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDBHDTQGia == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.CacLoaiHoaDonKhac:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.HDKhac == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon:
                                switch (thongTinHoaDon.TrangThaiSuDung)
                                {
                                    case TrangThaiSuDung2.KhongSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayBatDauSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.DangSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 0)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.NgungSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = tDiep.NgayThongBao;
                                        }
                                        break;
                                    case TrangThaiSuDung2.NgungSuDung:
                                        if (tDiep100.DLTKhai.NDTKhai.LHDSDung.CTu == 1)
                                        {
                                            thongTinHoaDon.TrangThaiSuDung = TrangThaiSuDung2.DangSuDung;
                                            thongTinHoaDon.NgayNgungSuDung = null;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            // await _db.QuanLyThongTinHoaDons.AddRangeAsync(listCon);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<EnumModel>> GetHinhThucHoaDonDangSuDung()
        {
            var result = new List<EnumModel>();
            result = await _db.QuanLyThongTinHoaDons
                           .Where(x => x.TrangThaiSuDung != TrangThaiSuDung2.KhongSuDung && x.LoaiThongTin == 1)
                           .Select(x => new EnumModel
                           {
                               Value = (int)x.LoaiThongTinChiTiet,
                               Name = x.LoaiThongTinChiTiet.GetDescription(),
                               TrangThai = x.TrangThaiSuDung
                           })
                           .ToListAsync();
            return result;
        }

        public async Task<List<EnumModel>> GetLoaiHoaDonDangSuDung()
        {
            var result = new List<EnumModel>();
            var loaiHoaDons = await GetListByLoaiThongTinAsync(2);
            var hinhThucHoaDons = await GetListByLoaiThongTinAsync(1);
            var hinhThucDangSuDungs = hinhThucHoaDons.Where(x => x.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung).Select(x => x.LoaiThongTinChiTiet).ToList();
            var dangSuDungs = loaiHoaDons.Where(x => x.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung).Select(x => x.LoaiThongTinChiTiet).ToList();
            if (hinhThucDangSuDungs.Contains(LoaiThongTinChiTiet.CoMaCuaCoQuanThue) || hinhThucDangSuDungs.Contains(LoaiThongTinChiTiet.KhongCoMaCuaCoQuanThue))
            {
                if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonGTGT))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonGTGT,
                        Name = LoaiHoaDon.HoaDonGTGT.GetDescription()
                    });
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonBanHang))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonBanHang,
                        Name = LoaiHoaDon.HoaDonBanHang.GetDescription()
                    });
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonBanTaiSanCong))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonBanTaiSanCong,
                        Name = LoaiHoaDon.HoaDonBanTaiSanCong.GetDescription()
                    });
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonBanHangDuTruQuocGia))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonBanHangDuTruQuocGia,
                        Name = LoaiHoaDon.HoaDonBanHangDuTruQuocGia.GetDescription()
                    });
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.CacLoaiHoaDonKhac))
                {
                    if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonGTGT))
                    {
                        result.Add(new EnumModel
                        {
                            Value = (int)LoaiHoaDon.TemVeGTGT,
                            Name = LoaiHoaDon.TemVeGTGT.GetDescription()
                        });
                    }

                    if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonBanHang))
                    {
                        result.Add(new EnumModel
                        {
                            Value = (int)LoaiHoaDon.TemVeBanHang,
                            Name = LoaiHoaDon.TemVeBanHang.GetDescription()
                        });
                    }
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon))
                {
                    //result.Add(new EnumModel
                    //{
                    //    Value = (int)LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD,
                    //    Name = LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD.GetDescription()
                    //});
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.PXKKiemVanChuyenNoiBo,
                        Name = LoaiHoaDon.PXKKiemVanChuyenNoiBo.GetDescription()
                    });
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.PXKHangGuiBanDaiLy,
                        Name = LoaiHoaDon.PXKHangGuiBanDaiLy.GetDescription()
                    });
                }
            }

            if (hinhThucDangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonGTGTCMTMTTien))
            {
                if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonGTGT))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonGTGTCMTMTT,
                        Name = LoaiHoaDon.HoaDonGTGTCMTMTT.GetDescription()
                    });
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.HoaDonBanHang))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonBanHangCMTMTT,
                        Name = LoaiHoaDon.HoaDonBanHangCMTMTT.GetDescription()
                    });
                }

                if (dangSuDungs.Contains(LoaiThongTinChiTiet.CacLoaiHoaDonKhac))
                {
                    result.Add(new EnumModel
                    {
                        Value = (int)LoaiHoaDon.HoaDonKhacCMTMTT,
                        Name = LoaiHoaDon.HoaDonKhacCMTMTT.GetDescription()
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Get by LoaiThongTinChiTiet
        /// </summary>
        /// <param name="loaiThongTinChiTiet"></param>
        /// <returns></returns>
        public async Task<QuanLyThongTinHoaDonViewModel> GetByLoaiThongTinChiTietAsync(LoaiThongTinChiTiet loaiThongTinChiTiet)
        {
            var entity = await _db.QuanLyThongTinHoaDons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LoaiThongTinChiTiet == loaiThongTinChiTiet);

            var result = _mp.Map<QuanLyThongTinHoaDonViewModel>(entity);
            return result;
        }

        /// <summary>
        /// Get lịch sử sinh số của máy tính tiền theo năm
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<SinhSoHDMayTinhTien>> GetHistorySinhSoHoaDonCMMTTien(int year)
        {
            var result = new List<SinhSoHDMayTinhTien>();

            var entity = _db.SinhSoHDMayTinhTiens.Select(x => new SinhSoHDMayTinhTien
            {
                SinhSoHDMayTinhTienId = x.SinhSoHDMayTinhTienId,
                NamPhatHanh = x.NamPhatHanh,
                SoBatDau = x.SoBatDau,
                SoKetThuc = x.SoKetThuc,
                IsUpdateSoKetThuc = x.IsUpdateSoKetThuc,
                // IsUpdateSoBatDau = x.IsUpdateSoBatDau,
            }); ;
            if (year != -1)
            {
                result = entity.Where(x => x.NamPhatHanh == year).OrderBy(x => x.NamPhatHanh).ToList();
            }
            else
            {
                result = await entity.OrderBy(x => x.NamPhatHanh).ToListAsync();
            }
            foreach (var item in result)
            {
               // item.SoBatDau = await this.GetMinSoHoaDonByYear(item.NamPhatHanh);
                item.SoKetThuc = await this.GetMaxSoHoaDonByYear(item.NamPhatHanh);
            }
            return result;

        }

        /// <summary>
        /// Get lịch sử sinh số của máy tính tiền theo năm
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSoBatDauSinhSoHoaDonCMMTTien(long SoBatdau)
        {
            bool result = false;
            try
            {
                int CurentYear = DateTime.Now.Year;
                var entity = await _db.SinhSoHDMayTinhTiens.FirstOrDefaultAsync(x => x.NamPhatHanh == CurentYear);
                long SoBatdauOld = entity.SoBatDau;
                if (entity != null && entity.SoKetThuc < 1)
                {
                    entity.SoBatDau = SoBatdau;
                    entity.IsUpdateSoBatDau = true;
                    entity.ModifyDate = DateTime.Now;
                }
                result = await _db.SaveChangesAsync() > 0;
                if (result && SoBatdauOld != SoBatdau)
                {
                    // add to nhatky
                    await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                    {
                        LoaiHanhDong = LoaiHanhDong.CapNhatSoLonNhatDaSinh,
                        RefType = RefType.ThongTinHoaDon,
                        ThamChieu = $"Cập nhật số bắt đầu từ số: {GetSoChuanCQT(SoBatdau)}",
                        MoTaChiTiet = $"Cập nhật số bắt đầu từ số: {GetSoChuanCQT(SoBatdau)}",
                        RefId = entity.SinhSoHDMayTinhTienId.ToString(),
                    });
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public async Task<bool> UpdateSoDaSinhMoiNhatHoaDonCMMTTien(long SoCapNhat)
        {
            bool result = false;
            long SoCu = 0;
            try
            {
                int CurentYear = DateTime.Now.Year;
                var entity = await _db.SinhSoHDMayTinhTiens.FirstOrDefaultAsync(x => x.NamPhatHanh == CurentYear);
                if (entity != null)
                {
                    SoCu = entity.SoKetThuc;
                    entity.SoKetThuc = SoCapNhat;
                    entity.ModifyDate = DateTime.Now;
                    entity.IsUpdateSoKetThuc = true;
                }


                result = await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        public async Task<bool> UpdateSoDaSinhMoiNhatHoaDonCMMTTienByNSD(long SoCapNhat)
        {
            bool result = false;
            long SoCu = 0;
            try
            {
                int CurentYear = DateTime.Now.Year;
                var entity = await _db.SinhSoHDMayTinhTiens.FirstOrDefaultAsync(x => x.NamPhatHanh == CurentYear);
                if (entity != null)
                {
                    SoCu = entity.SoKetThuc;
                    entity.SoKetThuc = SoCapNhat;
                    entity.ModifyDate = DateTime.Now;
                    entity.IsUpdateSoKetThuc = true;
                }


                result = await _db.SaveChangesAsync() > 0;
                if (result)
                {
                    // add to nhatky
                    await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                    {
                        LoaiHanhDong = LoaiHanhDong.CapNhatSoLonNhatDaSinh,
                        RefType = RefType.ThongTinHoaDon,
                        ThamChieu = $"Cập nhật số lớn nhất đã tự sinh đến hiện tại là: {GetSoChuanCQT(SoCapNhat)}",
                        MoTaChiTiet = $"Cập nhật số lơn nhất đã sinh từ {GetSoChuanCQT(SoCu)} thành {GetSoChuanCQT(SoCapNhat)}",
                        RefId = entity.SinhSoHDMayTinhTienId.ToString(),
                    });
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        public async Task<List<NhatKyTruyCapViewModel>> GetHistorySinhSoHoaDonCMMTTienInNhatKyTruyCap(Guid Id)
        {

            var query = from nktc in _db.NhatKyTruyCaps
                        join u in _db.Users on nktc.CreatedBy equals u.UserId
                        where nktc.RefId.Contains(Id.ToString())
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

        public async Task<long> GetSoCuoiMaxMaCQTCapMTTAsync(int year)
        {
            var hoSoHDDT = await _db.HoSoHDDTs.FirstOrDefaultAsync();
            if (hoSoHDDT != null && !string.IsNullOrEmpty(hoSoHDDT.MaCuaCQTToKhaiChapNhan))
            {
                var maCQTToKhai = hoSoHDDT.MaCuaCQTToKhaiChapNhan != null ? hoSoHDDT.MaCuaCQTToKhaiChapNhan.Trim() : "";
                var hoaDonLonNhat = await _db.HoaDonDienTus.Where(x => !string.IsNullOrEmpty(x.MaCuaCQT) && x.MaCuaCQT.Contains(maCQTToKhai) && x.NgayHoaDon.Value.Year == year).OrderByDescending(x => long.Parse(x.MaCuaCQT.Substring(12))).FirstOrDefaultAsync();
                if (hoaDonLonNhat != null)
                {
                    return long.Parse(hoaDonLonNhat.MaCuaCQT.Substring(12));
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private async Task<long> GetMaxSoHoaDonByYear(int year)
        {
            try
            {
                var hoSoHDDT = await _db.HoSoHDDTs.FirstOrDefaultAsync();
                if (hoSoHDDT != null)
                {
                    var maCQTToKhai = hoSoHDDT.MaCuaCQTToKhaiChapNhan != null ? hoSoHDDT.MaCuaCQTToKhaiChapNhan.Trim() : "";
                    var hoaDonLonNhat = await _db.HoaDonDienTus.Where(x => !string.IsNullOrEmpty(x.MaCuaCQT) && x.MaCuaCQT.Contains(hoSoHDDT.MaCuaCQTToKhaiChapNhan.Trim()) && x.NgayHoaDon.Value.Year == year).OrderByDescending(x => long.Parse(x.MaCuaCQT.Substring(12))).FirstOrDefaultAsync();
                    if (hoaDonLonNhat != null)
                    {
                        return long.Parse(hoaDonLonNhat.MaCuaCQT.Substring(12));
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return 0;

            }
        }

            private async Task<long> GetMinSoHoaDonByYear(int year)
        {
            try
            {
                var hoSoHDDT = await _db.HoSoHDDTs.FirstOrDefaultAsync();
                if(hoSoHDDT != null)
                {
                    var maCQTToKhai = hoSoHDDT.MaCuaCQTToKhaiChapNhan != null ? hoSoHDDT.MaCuaCQTToKhaiChapNhan.Trim() : "";
                    var hoaDonLonNhat = await _db.HoaDonDienTus.Where(x => !string.IsNullOrEmpty(x.MaCuaCQT) && x.MaCuaCQT.Contains(hoSoHDDT.MaCuaCQTToKhaiChapNhan.Trim()) && x.NgayHoaDon.Value.Year == year).OrderBy(x => long.Parse(x.MaCuaCQT.Substring(12))).FirstOrDefaultAsync();
                    if (hoaDonLonNhat != null)
                    {
                        return long.Parse(hoaDonLonNhat.MaCuaCQT.Substring(12));
                    }
                    else
                    {
                        return 0;
                    }
                } else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return 0;
            }
        }

        private string GetSoChuanCQT(long input)
        {
            string stringInit = "00000000000";
            string numberOfZezo = stringInit.Substring(0, (11 - input.ToString().Length));
            return numberOfZezo + input.ToString();
        }
    }
}
