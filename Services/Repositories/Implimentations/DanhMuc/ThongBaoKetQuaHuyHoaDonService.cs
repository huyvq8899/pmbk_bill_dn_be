using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.XML.ThongBaoPhatHanhHoaDon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonService : IThongBaoKetQuaHuyHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThongBaoKetQuaHuyHoaDonService(Datacontext datacontext, IMapper mapper, IHoSoHDDTService hoSoHDDTService, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> CheckAllowDeleteWhenChuaNopAsync(string id)
        {
            var check1 = await (from tbct in _db.ThongBaoKetQuaHuyHoaDonChiTiets
                                join hhdt in _db.HoaDonDienTus on tbct.MauHoaDonId equals hhdt.MauHoaDonId
                                where tbct.ThongBaoKetQuaHuyHoaDonId == id
                                select new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                {
                                    MauHoaDonId = tbct.MauHoaDonId,
                                    SoLuong = tbct.DenSo,
                                    SoLuongOther = int.Parse(hhdt.SoHoaDon)
                                })
                                .GroupBy(x => x.MauHoaDonId)
                                .Select(x => new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                {
                                    MauHoaDonId = x.Key,
                                    BlockDelete = x.Max(y => y.SoLuongOther) > x.First().SoLuong
                                })
                                .AnyAsync(x => x.BlockDelete == true);

            if (check1)
            {
                return false;
            }

            var check2 = await (from tbct in _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => x.ThongBaoKetQuaHuyHoaDonId == id)
                                join tbctOther in _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => x.ThongBaoKetQuaHuyHoaDonId != id)
                                on tbct.MauHoaDonId equals tbctOther.MauHoaDonId
                                select new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                {
                                    MauHoaDonId = tbct.MauHoaDonId,
                                    SoLuong = tbct.DenSo,
                                    SoLuongOther = tbctOther.TuSo
                                })
                                .GroupBy(x => x.MauHoaDonId)
                                .Select(x => new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                {
                                    MauHoaDonId = x.Key,
                                    BlockDelete = x.Max(y => y.SoLuongOther) > x.First().SoLuong
                                })
                                .AnyAsync(x => x.BlockDelete == true);

            return !check2;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            bool result = await _db.ThongBaoKetQuaHuyHoaDons
               .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.ThongBaoKetQuaHuyHoaDon, _db);

            var entity = await _db.ThongBaoKetQuaHuyHoaDons.FirstOrDefaultAsync(x => x.ThongBaoKetQuaHuyHoaDonId == id);
            _db.ThongBaoKetQuaHuyHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau)
        {
            ThongBaoKetQuaHuyHoaDonViewModel model = await GetByIdAsync(id);
            model.TenCoQuanThue = _hoSoHDDTService.GetListCoQuanThueQuanLy().FirstOrDefault(x => x.code == model.CoQuanThue).name;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            string fileName = $"{Guid.NewGuid()}.xml";
            string filePath = Path.Combine(destPath, fileName);
            CreateXML(model, hoSoHDDTVM, filePath);

            byte[] fileByte = File.ReadAllBytes(filePath);
            File.Delete(filePath);

            return new FileReturn
            {
                Bytes = fileByte,
                ContentType = MimeTypes.GetMimeType(filePath),
                FileName = Path.GetFileName(filePath)
            };
        }

        private void CreateXML(ThongBaoKetQuaHuyHoaDonViewModel model, HoSoHDDTViewModel hoSoHDDT, string filePath)
        {
            ViewModels.XML.ThongBaoKetQuaHuyHoaDon.HSoKhaiThue hSoKhaiThue = new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.HSoKhaiThue
            {
                TTinChung = new TTinChung
                {
                    TTinDVu = new TTinDVu
                    {
                        maDVu = "HTKK",
                        tenDVu = "HỖ TRỢ KÊ KHAI THUẾ",
                        pbanDVu = "4.1.6",
                        ttinNhaCCapDVu = "87CBA5FE8525AE3F361DEA2375F4A39F"
                    },
                    TTinTKhaiThue = new TTinTKhaiThue
                    {
                        TKhaiThue = new TKhaiThue
                        {
                            maTKhai = "107",
                            tenTKhai = "Thông báo kết quả hủy hóa đơn (TB03/AC)",
                            moTaBMau = "(Ban hành kèm theo Thông tư số 39/2014/TT-BTC ngày 31/3/2014 của Bộ Tài chính)",
                            pbanTKhaiXML = "1.0.0",
                            loaiTKhai = "C",
                            soLan = "0",
                            KyKKhaiThue = new KyKKhaiThue
                            {
                                kieuKy = "D",
                                kyKKhai = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                                kyKKhaiTuNgay = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                                kyKKhaiDenNgay = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                                kyKKhaiTuThang = string.Empty,
                                kyKKhaiDenThang = string.Empty
                            },
                            maCQTNoiNop = hoSoHDDT.CoQuanThueQuanLy,
                            tenCQTNoiNop = hoSoHDDT.TenCoQuanThueQuanLy,
                            ngayLapTKhai = model.CreatedDate.Value.ToString("dd/MM/yyyy"),
                            GiaHan = new GiaHan
                            {
                                maLyDoGiaHan = string.Empty,
                                lyDoGiaHan = string.Empty
                            },
                            nguoiKy = string.Empty,
                            ngayKy = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                            nganhNgheKD = string.Empty,
                        },
                        NNT = new NNT
                        {
                            mst = hoSoHDDT.MaSoThue,
                            tenNNT = hoSoHDDT.TenDonVi,
                            dchiNNT = hoSoHDDT.DiaChi,
                            phuongXa = string.Empty,
                            maHuyenNNT = string.Empty,
                            tenHuyenNNT = string.Empty,
                            maTinhNNT = string.Empty,
                            tenTinhNNT = string.Empty,
                            dthoaiNNT = string.Empty,
                            faxNNT = string.Empty,
                            emailNNT = string.Empty,
                        }
                    }
                },
                CTieuTKhaiChinh = new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.CTieuTKhaiChinh
                {
                    kinhGui = model.TenCoQuanThue,
                    phuongPhapHuy = model.PhuongPhapHuy,
                    thoiGian = model.NgayGioHuy.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z",
                    HoaDon = new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.HoaDon
                    {
                        ChiTiet = new List<ViewModels.XML.ThongBaoKetQuaHuyHoaDon.ChiTiet>()
                    },
                    nguoiLapBieu = model.TenNguoiTao,
                    nguoiDaiDien = hoSoHDDT.HoTenNguoiDaiDienPhapLuat,
                    ngayBCao = model.NgayGioHuy.Value.ToString("yyyy-MM-dd")
                }
            };

            foreach (var item in model.ThongBaoKetQuaHuyHoaDonChiTiets)
            {
                hSoKhaiThue.CTieuTKhaiChinh.HoaDon.ChiTiet.Add(new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.ChiTiet
                {
                    tenHDon = item.TenLoaiHoaDon,
                    mauHDon = item.MauSo,
                    kyHieu = item.KyHieu,
                    tuSo = item.TuSo.Value.PadZerro(),
                    denSo = item.DenSo.Value.PadZerro(),
                    soLuong = item.SoLuong.Value.ToString("N0")
                });
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.ThongBaoKetQuaHuyHoaDon.HSoKhaiThue));

            using (TextWriter filestream = new StreamWriter(filePath))
            {
                serialiser.Serialize(filestream, hSoKhaiThue, ns);
            }
        }

        public Task<List<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllAsync(ThongBaoKetQuaHuyHoaDonParams @params = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllPagingAsync(ThongBaoKetQuaHuyHoaDonParams @params)
        {
            var query = _db.ThongBaoKetQuaHuyHoaDons
                .OrderByDescending(x => x.NgayThongBao).OrderByDescending(x => x.So)
                .Select(x => new ThongBaoKetQuaHuyHoaDonViewModel
                {
                    ThongBaoKetQuaHuyHoaDonId = x.ThongBaoKetQuaHuyHoaDonId,
                    NgayThongBao = x.NgayThongBao,
                    So = x.So,
                    PhuongPhapHuy = x.PhuongPhapHuy,
                    TrangThaiNop = x.TrangThaiNop,
                    TenTrangThaiNop = x.TrangThaiNop.GetDescription()
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

            return await PagedList<ThongBaoKetQuaHuyHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ThongBaoKetQuaHuyHoaDonViewModel> GetByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongBaoKetQuaHuyHoaDon);
            string folder = $@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{id}\FileAttach";

            var query = from tb in _db.ThongBaoKetQuaHuyHoaDons
                        join u in _db.Users on tb.CreatedBy equals u.UserId
                        where tb.ThongBaoKetQuaHuyHoaDonId == id
                        select new ThongBaoKetQuaHuyHoaDonViewModel
                        {
                            ThongBaoKetQuaHuyHoaDonId = tb.ThongBaoKetQuaHuyHoaDonId,
                            CoQuanThue = tb.CoQuanThue,
                            NgayGioHuy = tb.NgayGioHuy,
                            PhuongPhapHuy = tb.PhuongPhapHuy,
                            So = tb.So,
                            NgayThongBao = tb.NgayThongBao,
                            TrangThaiNop = tb.TrangThaiNop,
                            ThongBaoKetQuaHuyHoaDonChiTiets = (from tbct in _db.ThongBaoKetQuaHuyHoaDonChiTiets
                                                               join mhd in _db.MauHoaDons on tbct.MauHoaDonId equals mhd.MauHoaDonId
                                                               where tbct.ThongBaoKetQuaHuyHoaDonId == tb.ThongBaoKetQuaHuyHoaDonId
                                                               orderby tbct.CreatedDate
                                                               select new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                                               {
                                                                   ThongBaoKetQuaHuyHoaDonChiTietId = tbct.ThongBaoKetQuaHuyHoaDonChiTietId,
                                                                   ThongBaoKetQuaHuyHoaDonId = tbct.ThongBaoKetQuaHuyHoaDonId,
                                                                   LoaiHoaDon = tbct.LoaiHoaDon,
                                                                   TenLoaiHoaDon = tbct.LoaiHoaDon.GetDescription(),
                                                                   MauHoaDonId = tbct.MauHoaDonId,
                                                                   MauSo = mhd.MauSo,
                                                                   KyHieu = mhd.KyHieu,
                                                                   TuSo = tbct.TuSo,
                                                                   DenSo = tbct.DenSo,
                                                                   SoLuong = tbct.SoLuong
                                                               })
                                                               .ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == tb.ThongBaoKetQuaHuyHoaDonId
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
                            CreatedBy = tb.CreatedBy,
                            TenNguoiTao = u.UserName,
                            CreatedDate = tb.CreatedDate,
                            Status = tb.Status,
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ThongBaoKetQuaHuyHoaDonChiTietViewModel>> GetThongBaoKetQuaHuyChiTietByIdAsync(string id)
        {
            var query = from tbct in _db.ThongBaoKetQuaHuyHoaDonChiTiets
                        join mhd in _db.MauHoaDons on tbct.MauHoaDonId equals mhd.MauHoaDonId
                        where tbct.ThongBaoKetQuaHuyHoaDonId == id
                        orderby tbct.CreatedDate
                        select new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                        {
                            ThongBaoKetQuaHuyHoaDonChiTietId = tbct.ThongBaoKetQuaHuyHoaDonChiTietId,
                            ThongBaoKetQuaHuyHoaDonId = tbct.ThongBaoKetQuaHuyHoaDonId,
                            LoaiHoaDon = tbct.LoaiHoaDon,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            MauHoaDonId = mhd.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            SoLuong = tbct.SoLuong,
                            TuSo = tbct.TuSo,
                            DenSo = tbct.DenSo
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<ThongBaoKetQuaHuyHoaDonViewModel> InsertAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            List<ThongBaoKetQuaHuyHoaDonChiTietViewModel> detailVMs = model.ThongBaoKetQuaHuyHoaDonChiTiets;

            model.ThongBaoKetQuaHuyHoaDonChiTiets = null;
            model.ThongBaoKetQuaHuyHoaDonId = string.IsNullOrEmpty(model.ThongBaoKetQuaHuyHoaDonId) ? Guid.NewGuid().ToString() : model.ThongBaoKetQuaHuyHoaDonId;
            ThongBaoKetQuaHuyHoaDon entity = _mp.Map<ThongBaoKetQuaHuyHoaDon>(model);
            await _db.ThongBaoKetQuaHuyHoaDons.AddAsync(entity);

            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoKetQuaHuyHoaDonId = entity.ThongBaoKetQuaHuyHoaDonId;
                var detail = _mp.Map<ThongBaoKetQuaHuyHoaDonChiTiet>(item);
                await _db.ThongBaoKetQuaHuyHoaDonChiTiets.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongBaoKetQuaHuyHoaDonViewModel result = _mp.Map<ThongBaoKetQuaHuyHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            List<ThongBaoKetQuaHuyHoaDonChiTietViewModel> detailVMs = model.ThongBaoKetQuaHuyHoaDonChiTiets;

            model.ThongBaoKetQuaHuyHoaDonChiTiets = null;
            ThongBaoKetQuaHuyHoaDon entity = await _db.ThongBaoKetQuaHuyHoaDons.FirstOrDefaultAsync(x => x.ThongBaoKetQuaHuyHoaDonId == model.ThongBaoKetQuaHuyHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<ThongBaoKetQuaHuyHoaDonChiTiet> details = await _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => x.ThongBaoKetQuaHuyHoaDonId == model.ThongBaoKetQuaHuyHoaDonId).ToListAsync();
            _db.ThongBaoKetQuaHuyHoaDonChiTiets.RemoveRange(details);
            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoKetQuaHuyHoaDonId = entity.ThongBaoKetQuaHuyHoaDonId;
                var detail = _mp.Map<ThongBaoKetQuaHuyHoaDonChiTiet>(item);
                await _db.ThongBaoKetQuaHuyHoaDonChiTiets.AddAsync(detail);
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
