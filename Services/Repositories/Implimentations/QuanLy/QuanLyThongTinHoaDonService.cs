using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.QuanLy;
using DLL.Enums;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels.QuanLy;
using System;
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

        public async Task<List<EnumModel>> GetLoaiHoaDonDangSuDung()
        {
            var result = new List<EnumModel>();
            var loaiHoaDons = await GetListByLoaiThongTinAsync(2);
            var dangSuDungs = loaiHoaDons.Where(x => x.TrangThaiSuDung == TrangThaiSuDung2.DangSuDung).Select(x => x.LoaiThongTinChiTiet).ToList();
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
                result.Add(new EnumModel
                {
                    Value = (int)LoaiHoaDon.CacLoaiHoaDonKhac,
                    Name = LoaiHoaDon.CacLoaiHoaDonKhac.GetDescription()
                });
            }

            if (dangSuDungs.Contains(LoaiThongTinChiTiet.CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon))
            {
                result.Add(new EnumModel
                {
                    Value = (int)LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD,
                    Name = LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD.GetDescription()
                });
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
    }
}
