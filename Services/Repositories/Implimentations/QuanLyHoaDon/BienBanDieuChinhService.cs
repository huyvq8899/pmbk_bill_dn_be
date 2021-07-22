using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Spire.Doc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class BienBanDieuChinhService : IBienBanDieuChinhService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BienBanDieuChinhService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.BienBanDieuChinh);
            string destDocPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/FileAttach/{loaiNghiepVu}/{id}/Bien_ban_dieu_chinh_hoa_don.doc");
            if (File.Exists(destDocPath))
            {
                File.Delete(destDocPath);
            }

            var entity = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == id);
            _db.BienBanDieuChinhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<BienBanDieuChinhViewModel> GetByIdAsync(string id)
        {
            var query = from bbdc in _db.BienBanDieuChinhs
                        join hddt in _db.HoaDonDienTus on bbdc.HoaDonBiDieuChinhId equals hddt.HoaDonDienTuId
                        where bbdc.BienBanDieuChinhId == id
                        select new BienBanDieuChinhViewModel
                        {
                            BienBanDieuChinhId = bbdc.BienBanDieuChinhId,
                            NoiDungBienBan = bbdc.NoiDungBienBan,
                            NgayBienBan = bbdc.NgayBienBan,
                            TenDonViBenA = bbdc.TenDonViBenA,
                            DiaChiBenA = bbdc.DiaChiBenA,
                            MaSoThueBenA = bbdc.MaSoThueBenA,
                            SoDienThoaiBenA = bbdc.SoDienThoaiBenA,
                            DaiDienBenA = bbdc.DaiDienBenA,
                            ChucVuBenA = bbdc.ChucVuBenA,
                            NgayKyBenA = bbdc.NgayKyBenA,
                            TenDonViBenB = bbdc.TenDonViBenB,
                            DiaChiBenB = bbdc.DiaChiBenB,
                            MaSoThueBenB = bbdc.MaSoThueBenB,
                            SoDienThoaiBenB = bbdc.SoDienThoaiBenB,
                            DaiDienBenB = bbdc.DaiDienBenB,
                            ChucVuBenB = bbdc.ChucVuBenB,
                            NgayKyBenB = bbdc.NgayKyBenB,
                            LyDoDieuChinh = bbdc.LyDoDieuChinh,
                            TrangThaiBienBan = bbdc.TrangThaiBienBan,
                            FileDaKy = bbdc.FileDaKy,
                            FileChuaKy = bbdc.FileChuaKy,
                            XMLChuaKy = bbdc.XMLChuaKy,
                            XMLDaKy = bbdc.XMLDaKy,
                            HoaDonBiDieuChinhId = bbdc.HoaDonBiDieuChinhId,
                            HoaDonBiDieuChinh = new HoaDonDienTuViewModel
                            {
                                HoaDonDienTuId = hddt.HoaDonDienTuId,
                                NgayHoaDon = hddt.NgayHoaDon,
                                SoHoaDon = hddt.SoHoaDon,
                                MauHoaDonId = hddt.MauHoaDonId,
                                MauSo = hddt.MauSo,
                                KyHieu = hddt.KyHieu,
                                MaTraCuu = hddt.MaTraCuu
                            },
                            HoaDonDieuChinhId = bbdc.HoaDonDieuChinhId,
                            CreatedBy = bbdc.CreatedBy,
                            CreatedDate = bbdc.CreatedDate,
                            Status = bbdc.Status
                        };

            var result = await query.AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public async Task<BienBanDieuChinhViewModel> InsertAsync(BienBanDieuChinhViewModel model)
        {
            var hoaDonBiDieuChinh = model.HoaDonBiDieuChinh;
            model.HoaDonBiDieuChinh = null;

            var entity = _mp.Map<BienBanDieuChinh>(model);
            await _db.BienBanDieuChinhs.AddAsync(entity);
            await _db.SaveChangesAsync();

            var result = _mp.Map<BienBanDieuChinhViewModel>(entity);
            result.HoaDonBiDieuChinh = hoaDonBiDieuChinh;
            SaveBienBanDoc(result);

            return result;
        }

        public async Task<bool> UpdateAsync(BienBanDieuChinhViewModel model)
        {
            var hoaDonBiDieuChinh = model.HoaDonBiDieuChinh;
            model.HoaDonBiDieuChinh = null;

            var entity = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == model.BienBanDieuChinhId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            model.HoaDonBiDieuChinh = hoaDonBiDieuChinh;
            SaveBienBanDoc(model);

            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        private void SaveBienBanDoc(BienBanDieuChinhViewModel bienBanDieuChinh)
        {
            Document doc = new Document();
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.BienBanDieuChinh);
            string srcDocPath = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/HoaDonDieuChinh/Bien_ban_dieu_chinh_hoa_don.doc");
            string destDocPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/FileAttach/{loaiNghiepVu}/{bienBanDieuChinh.BienBanDieuChinhId}");
            if (!Directory.Exists(destDocPath))
            {
                Directory.CreateDirectory(destDocPath);
            }
            doc.LoadFromFile(srcDocPath);

            doc.Replace("<content>", bienBanDieuChinh.NoiDungBienBan ?? string.Empty, true, true);
            doc.Replace("<date>", bienBanDieuChinh.NgayBienBan.Value.ToString("dd") ?? string.Empty, true, true);
            doc.Replace("<month>", bienBanDieuChinh.NgayBienBan.Value.ToString("MM") ?? string.Empty, true, true);
            doc.Replace("<year>", bienBanDieuChinh.NgayBienBan.Value.ToString("yyyy") ?? string.Empty, true, true);

            doc.Replace("<CompanyName>", bienBanDieuChinh.TenDonViBenA ?? string.Empty, true, true);
            doc.Replace("<Address>", bienBanDieuChinh.DiaChiBenA ?? string.Empty, true, true);
            doc.Replace("<Taxcode>", bienBanDieuChinh.MaSoThueBenA ?? string.Empty, true, true);
            doc.Replace("<Tel>", bienBanDieuChinh.SoDienThoaiBenA ?? string.Empty, true, true);
            doc.Replace("<Representative>", bienBanDieuChinh.DiaChiBenA ?? string.Empty, true, true);
            doc.Replace("<Position>", bienBanDieuChinh.ChucVuBenA ?? string.Empty, true, true);

            doc.Replace("<CustomerCompany>", bienBanDieuChinh.TenDonViBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerAddress>", bienBanDieuChinh.DiaChiBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerTaxcode>", bienBanDieuChinh.MaSoThueBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerTel>", bienBanDieuChinh.SoDienThoaiBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerRepresentative>", bienBanDieuChinh.DiaChiBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerPosition>", bienBanDieuChinh.ChucVuBenB ?? string.Empty, true, true);

            doc.Replace("<Description>", bienBanDieuChinh.HoaDonBiDieuChinh.GetMoTaBienBanDieuChinh(), true, true);
            doc.Replace("<reason>", bienBanDieuChinh.LyDoDieuChinh ?? string.Empty, true, true);

            doc.SaveToFile(Path.Combine(destDocPath, "Bien_ban_dieu_chinh_hoa_don.doc"));
        }
    }
}
