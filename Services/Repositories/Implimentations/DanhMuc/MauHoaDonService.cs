using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.TienIch;
using Spire.Doc;
using Spire.Doc.Fields;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class MauHoaDonService : IMauHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public MauHoaDonService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.MauHoaDon, _db);

            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == id);
            _db.MauHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<MauHoaDonViewModel>> GetAllAsync(MauHoaDonParams @params = null)
        {
            var query = _db.MauHoaDons.AsQueryable();

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    string keyword = @params.Keyword.ToUpper().ToTrim();
                    //query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) ||
                    //                        x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()));
                }
            }

            var result = await query
                .ProjectTo<MauHoaDonViewModel>(_mp.ConfigurationProvider)
                .AsNoTracking()
                .OrderBy(x => x.MauSo)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<MauHoaDonViewModel>> GetAllPagingAsync(MauHoaDonParams @params)
        {
            var query = from mhd in _db.MauHoaDons
                        join u in _db.Users on mhd.CreatedBy equals u.UserId into tmpUsers
                        join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId into tmpTBPHCTs
                        from tbphct in tmpTBPHCTs.DefaultIfEmpty()
                        from u in tmpUsers.DefaultIfEmpty()
                        select new MauHoaDonViewModel
                        {
                            MauHoaDonId = mhd.MauHoaDonId,
                            Ten = mhd.Ten,
                            SoThuTu = mhd.SoThuTu,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            TenBoMau = mhd.TenBoMau,
                            Status = mhd.Status,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            TenQuyDinhApDung = mhd.QuyDinhApDung.GetDescription(),
                            Username = u != null ? u.UserName : string.Empty,
                            ModifyDate = mhd.ModifyDate,
                            NgayKy = mhd.NgayKy,
                            IsDaThongBaoPhatHanh = tbphct != null
                        };

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<MauHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<MauHoaDonViewModel> GetByIdAsync(string id)
        {
            var query = from mhd in _db.MauHoaDons
                        where mhd.MauHoaDonId == id
                        select new MauHoaDonViewModel
                        {
                            MauHoaDonId = mhd.MauHoaDonId,
                            Ten = mhd.Ten,
                            SoThuTu = mhd.SoThuTu,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            TenBoMau = mhd.TenBoMau,
                            NgayKy = mhd.NgayKy,
                            QuyDinhApDung = mhd.QuyDinhApDung,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            LoaiMauHoaDon = mhd.LoaiMauHoaDon,
                            LoaiThueGTGT = mhd.LoaiThueGTGT,
                            LoaiNgonNgu = mhd.LoaiNgonNgu,
                            LoaiKhoGiay = mhd.LoaiKhoGiay,
                            CreatedBy = mhd.CreatedBy,
                            CreatedDate = mhd.CreatedDate,
                            Status = mhd.Status,
                            MauHoaDonThietLapMacDinhs = (from tlmd in _db.MauHoaDonThietLapMacDinhs
                                                         where tlmd.MauHoaDonId == mhd.MauHoaDonId
                                                         select new MauHoaDonThietLapMacDinhViewModel
                                                         {
                                                             MauHoaDonThietLapMacDinhId = tlmd.MauHoaDonThietLapMacDinhId,
                                                             MauHoaDonId = tlmd.MauHoaDonId,
                                                             Loai = tlmd.Loai,
                                                             GiaTri = tlmd.GiaTri,
                                                             GiaTriBoSung = tlmd.GiaTriBoSung,
                                                         })
                                                         .ToList(),
                            MauHoaDonTuyChinhChiTiets = (from tcct in _db.MauHoaDonTuyChinhChiTiets
                                                         where tcct.MauHoaDonId == mhd.MauHoaDonId && tcct.IsParent == true
                                                         orderby tcct.STT
                                                         select new MauHoaDonTuyChinhChiTietViewModel
                                                         {
                                                             MauHoaDonTuyChinhChiTietId = tcct.MauHoaDonTuyChinhChiTietId,
                                                             MauHoaDonId = tcct.MauHoaDonId,
                                                             GiaTri = tcct.GiaTri,
                                                             TuyChinhChiTiet = tcct.TuyChinhChiTiet,
                                                             TenTiengAnh = tcct.TenTiengAnh,
                                                             KieuDuLieuThietLap = tcct.KieuDuLieuThietLap,
                                                             Loai = tcct.Loai,
                                                             LoaiChiTiet = tcct.LoaiChiTiet,
                                                             LoaiContainer = tcct.LoaiContainer,
                                                             IsParent = tcct.IsParent,
                                                             Checked = tcct.Checked,
                                                             Disabled = tcct.Disabled,
                                                             STT = tcct.STT,
                                                             Status = tcct.Status,
                                                             Children = (from child in _db.MauHoaDonTuyChinhChiTiets
                                                                         where tcct.LoaiChiTiet == child.LoaiChiTiet && child.IsParent == false
                                                                         orderby child.STT
                                                                         select new MauHoaDonTuyChinhChiTietViewModel
                                                                         {
                                                                             MauHoaDonTuyChinhChiTietId = child.MauHoaDonTuyChinhChiTietId,
                                                                             MauHoaDonId = child.MauHoaDonId,
                                                                             GiaTri = child.GiaTri,
                                                                             TuyChonChiTiet = JsonConvert.DeserializeObject<TuyChinhChiTietModel>(child.TuyChinhChiTiet),
                                                                             TuyChinhChiTiet = child.TuyChinhChiTiet,
                                                                             TenTiengAnh = child.TenTiengAnh,
                                                                             KieuDuLieuThietLap = child.KieuDuLieuThietLap,
                                                                             Loai = child.Loai,
                                                                             LoaiChiTiet = child.LoaiChiTiet,
                                                                             LoaiContainer = child.LoaiContainer,
                                                                             IsParent = child.IsParent,
                                                                             Checked = child.Checked,
                                                                             Disabled = child.Disabled,
                                                                             STT = child.STT,
                                                                             Status = child.Status,
                                                                         })
                                                                         .ToList()
                                                         })

                                                         .ToList()
                        };

            MauHoaDonViewModel result = await query.FirstOrDefaultAsync();
            return result;
        }

        public List<EnumModel> GetListLoaiHoaDon()
        {
            List<EnumModel> enums = ((LoaiHoaDon[])Enum.GetValues(typeof(LoaiHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiKhoGiay()
        {
            List<EnumModel> enums = ((LoaiKhoGiay[])Enum.GetValues(typeof(LoaiKhoGiay)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiMau()
        {
            List<EnumModel> enums = ((LoaiMauHoaDon[])Enum.GetValues(typeof(LoaiMauHoaDon)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiNgonNgu()
        {
            List<EnumModel> enums = ((LoaiNgonNgu[])Enum.GetValues(typeof(LoaiNgonNgu)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public List<EnumModel> GetListLoaiThueGTGT()
        {
            List<EnumModel> enums = ((LoaiThueGTGT[])Enum.GetValues(typeof(LoaiThueGTGT)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }


        public async Task<List<MauHoaDonViewModel>> GetListMauDaDuocChapNhanAsync()
        {
            var query = from mhd in _db.MauHoaDons
                        join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                        join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                        where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                        group mhd by new { mhd.LoaiHoaDon, mhd.MauSo } into g
                        select new MauHoaDonViewModel
                        {
                            LoaiHoaDon = g.Key.LoaiHoaDon,
                            MauSo = g.Key.MauSo,
                            MauHoaDonIds = g.Select(x => x.MauHoaDonId).ToList(),
                            KyHieus = g.OrderBy(x => x.KyHieu).Select(x => x.KyHieu).ToList()
                        };

            var result = await query.ToListAsync();
            var mauHoaDonIds = result.SelectMany(x => x.MauHoaDonIds).ToList();

            var thongBaoKetQuaHuyHDs = await _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => mauHoaDonIds.Contains(x.MauHoaDonId)).ToListAsync();
            var mauHoaDons = await _db.MauHoaDons.Where(x => mauHoaDonIds.Contains(x.MauHoaDonId)).ToListAsync();
            foreach (var group in result)
            {
                group.ThongTinChiTiets = new List<ThongTinChiTietKetQuaHuy>();

                foreach (var kyHieu in group.KyHieus)
                {
                    var mauHoaDon = mauHoaDons.FirstOrDefault(x => x.MauSo == group.MauSo && x.KyHieu == kyHieu);
                    int? tuSo = 1;
                    if (mauHoaDon != null)
                    {
                        int? maxTuSo = thongBaoKetQuaHuyHDs.Where(x => x.MauHoaDonId == mauHoaDon.MauHoaDonId).Max(x => x.DenSo);
                        if (maxTuSo.HasValue && maxTuSo > 0)
                        {
                            tuSo = maxTuSo + 1;
                        }
                    }

                    group.ThongTinChiTiets.Add(new ThongTinChiTietKetQuaHuy
                    {
                        KyHieu = kyHieu,
                        TuSo = tuSo
                    });
                }
            }

            return result;
        }

        public List<MauParam> GetListMauHoaDon(MauHoaDonParams @params)
        {
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");
            var list = new List<MauParam>().Deserialize(Path.Combine(jsonPath, "mau-hoa-don-anhbh.json")).ToList();
            list = list.Where(x => x.loaiHoaDon == @params.LoaiHoaDon && x.loaiMauHoaDon == @params.LoaiMau && x.loaiThueGTGT == @params.LoaiThueGTGT && x.loaiNgonNgu == @params.LoaiNgonNgu && x.loaiKhoGiay == @params.LoaiKhoGiay).ToList();
            return list;
        }

        public List<EnumModel> GetListQuyDinhApDung()
        {
            List<EnumModel> enums = ((QuyDinhApDung[])Enum.GetValues(typeof(QuyDinhApDung)))
               .Select(c => new EnumModel()
               {
                   Value = (int)c,
                   Name = c.GetDescription()
               }).ToList();
            return enums;
        }

        public List<ImageParam> GetMauHoaDonBackgrounds()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "template-background.json")).ToList();

            foreach (var item in list)
            {
                item.thumb = "/images/background-thumb/" + item.thumb;
                item.background = "/images/background/" + item.background;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public async Task<MauHoaDonViewModel> InsertAsync(MauHoaDonViewModel model)
        {
            var entity = _mp.Map<MauHoaDon>(model);
            await _db.MauHoaDons.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<MauHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(MauHoaDonViewModel model)
        {
            var mauHoaDonThietLapMacDinhs = await _db.MauHoaDonThietLapMacDinhs
                .Where(x => x.MauHoaDonId == model.MauHoaDonId)
                .ToListAsync();
            _db.MauHoaDonThietLapMacDinhs.RemoveRange(mauHoaDonThietLapMacDinhs);

            var mauHoaDonTuyChinhChiTiets = await _db.MauHoaDonTuyChinhChiTiets
                .Where(x => x.MauHoaDonId == model.MauHoaDonId)
                .ToListAsync();
            _db.MauHoaDonTuyChinhChiTiets.RemoveRange(mauHoaDonTuyChinhChiTiets);

            var entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            entity.MauHoaDonThietLapMacDinhs = _mp.Map<List<MauHoaDonThietLapMacDinh>>(model.MauHoaDonThietLapMacDinhs);
            entity.MauHoaDonTuyChinhChiTiets = _mp.Map<List<MauHoaDonTuyChinhChiTiet>>(model.MauHoaDonTuyChinhChiTiets);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ChiTietMauHoaDon> GetChiTietByMauHoaDon(string mauHoaDonId)
        {
            var result = new ChiTietMauHoaDon();
            try
            {
                var mhd = _mp.Map<MauHoaDonViewModel>(await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == mauHoaDonId));
                var listBanMau = new List<BanMauHoaDon>();
                string jsonFolder = Path.Combine(_hostingEnvironment.WebRootPath, "jsons/mau-hoa-don.json");
                using (StreamReader r = new StreamReader(jsonFolder))
                {
                    string json = r.ReadToEnd();
                    listBanMau = JsonConvert.DeserializeObject<List<BanMauHoaDon>>(json);
                }

                var banMau = listBanMau.FirstOrDefault(x => x.TenBanMau.Contains(mhd.TenBoMau));
                if (banMau != null) result = banMau.ChiTiets.FirstOrDefault();
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return result;
        }

        public async Task<List<string>> GetAllMauSoHoaDon()
        {
            return await _db.MauHoaDons.Select(x => x.MauSo).ToListAsync();
        }

        public async Task<List<string>> GetAllKyHieuHoaDon(string ms = "")
        {
            return await _db.MauHoaDons.Where(x => string.IsNullOrEmpty(ms) || x.MauSo == ms).Select(x => x.KyHieu).ToListAsync();
        }

        public async Task<FileReturn> PreviewPdfAsync(string id, HinhThucMauHoaDon loai)
        {
            var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

            var hoSoHDDT = await _db.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
            if (hoSoHDDT == null)
            {
                hoSoHDDT = new HoSoHDDT { MaSoThue = taxCode };
            }

            var mauHoaDon = await _db.MauHoaDons.AsNoTracking()
                .Include(x => x.MauHoaDonThietLapMacDinhs)
                .FirstOrDefaultAsync(x => x.MauHoaDonId == id);

            var result = MauHoaDonHelper.PreviewFilePDF(mauHoaDon, loai, hoSoHDDT, _hostingEnvironment, _httpContextAccessor, _configuration);
            return result;
        }

        public async Task<FileReturn> DownloadFileAsync(string id, HinhThucMauHoaDon loai, DinhDangTepMau loaiFile)
        {
            var fileReturn = await PreviewPdfAsync(id, loai);
            if (loaiFile == DinhDangTepMau.PDF)
            {
                return fileReturn;
            }
            else
            {
                string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp/download_mau_hoa_don_{Guid.NewGuid()}");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string pdfPath = Path.Combine(folderPath, $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(pdfPath, fileReturn.Bytes);

                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.LoadFromFile(pdfPath);
                Image bmp = pdfDoc.SaveAsImage(0);
                Image emf = pdfDoc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap);
                Image zoomImg = new Bitmap(emf.Size.Width * 2, emf.Size.Height * 2);
                using (Graphics gg = Graphics.FromImage(zoomImg))
                {
                    gg.ScaleTransform(2.0f, 2.0f);
                    gg.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
                }

                Document docEmpty = new Document(Path.Combine(_hostingEnvironment.WebRootPath, "docs/MauHoaDonAnhBH/Empty/Hoa_don_trang.docx"));
                DocPicture picture2 = docEmpty.Sections[0].Paragraphs[0].AppendPicture(bmp);
                picture2.Width = 580;
                picture2.Height = 800;
                string docPath = Path.Combine(folderPath, loai.GetTenFile() + (loaiFile == DinhDangTepMau.DOC ? ".doc" : ".docx"));
                docEmpty.SaveToFile(docPath, (loaiFile == DinhDangTepMau.DOC ? Spire.Doc.FileFormat.Doc : Spire.Doc.FileFormat.Docx));
                byte[] bytes = File.ReadAllBytes(docPath);
                Directory.Delete(folderPath, true);
                return new FileReturn
                {
                    Bytes = bytes,
                    ContentType = MimeTypes.GetMimeType(docPath),
                    FileName = Path.GetFileName(docPath)
                };
            }
        }

        public async Task<bool> CheckTrungMauSoAsync(MauHoaDonViewModel model)
        {
            bool result = await _db.MauHoaDons.AnyAsync(x => x.MauSo == model.MauSo);
            return result;
        }

        public async Task<string> CheckAllowUpdateAsync(MauHoaDonViewModel model)
        {
            var check1 = await (from tbph in _db.ThongBaoPhatHanhs
                                join tbphct in _db.ThongBaoPhatHanhChiTiets on tbph.ThongBaoPhatHanhId equals tbphct.ThongBaoPhatHanhId
                                where tbphct.MauHoaDonId == model.MauHoaDonId && tbph.TrangThaiNop != TrangThaiNop.ChuaNop
                                select new
                                {
                                    TenTrangThaiNop = tbph.TrangThaiNop.GetDescription()
                                })
                                .Select(x => x.TenTrangThaiNop).Distinct().ToListAsync();

            if (check1.Any())
            {
                return string.Join(", ", check1);
            }

            if (model.NgayKy.HasValue)
            {
                return "signed";
            }

            return null;
        }

        public async Task<FileReturn> ExportMauHoaDonAsync(ExportMauHoaDonParams @params)
        {
            var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
            var hoSoHDDT = await _db.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
            if (hoSoHDDT == null)
            {
                hoSoHDDT = new HoSoHDDT { MaSoThue = taxCode };
            }

            var mauHoaDon = await _db.MauHoaDons.AsNoTracking()
                .Include(x => x.MauHoaDonThietLapMacDinhs)
                .FirstOrDefaultAsync(x => x.MauHoaDonId == @params.MauHoaDonId);

            List<string> filePaths = new List<string>();
            string folderName = $"temp/export_mau_hoa_don_{Guid.NewGuid()}";
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var item in @params.HinhThucMauHoaDon)
            {
                var fileReturn = MauHoaDonHelper.PreviewFilePDF(mauHoaDon, item, hoSoHDDT, _hostingEnvironment, _httpContextAccessor, _configuration);
                string pdfPath = Path.Combine(folderPath, $"{item.GetTenFile()}.pdf");
                File.WriteAllBytes(pdfPath, fileReturn.Bytes);
                filePaths.Add(pdfPath);
            }

            if (@params.DinhDangTepMau != 0)
            {
                for (int i = 0; i < filePaths.Count(); i++)
                {
                    string path = filePaths[i];

                    PdfDocument pdfDoc = new PdfDocument();
                    pdfDoc.LoadFromFile(path);
                    Image bmp = pdfDoc.SaveAsImage(0);
                    Image emf = pdfDoc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Bitmap);
                    Image zoomImg = new Bitmap(emf.Size.Width * 2, emf.Size.Height * 2);
                    using (Graphics gg = Graphics.FromImage(zoomImg))
                    {
                        gg.ScaleTransform(2.0f, 2.0f);
                        gg.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
                    }

                    Document docEmpty = new Document(Path.Combine(_hostingEnvironment.WebRootPath, "docs/MauHoaDonAnhBH/Empty/Hoa_don_trang.docx"));
                    DocPicture picture2 = docEmpty.Sections[0].Paragraphs[0].AppendPicture(bmp);
                    picture2.Width = 580;
                    picture2.Height = 800;

                    filePaths[i] = path.Replace(".pdf", (@params.DinhDangTepMau == DinhDangTepMau.DOC) ? ".doc" : ".docx");
                    docEmpty.SaveToFile(filePaths[i], (@params.DinhDangTepMau == DinhDangTepMau.DOC) ? Spire.Doc.FileFormat.Doc : Spire.Doc.FileFormat.Docx);
                }
            }

            if (filePaths.Count() > 1)
            {
                using (var zipFileMemoryStream = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Create, leaveOpen: true))
                    {
                        foreach (var botFilePath in filePaths)
                        {
                            var botFileName = Path.GetFileName(botFilePath);
                            var entry = archive.CreateEntry(botFileName, CompressionLevel.Fastest);
                            using (var entryStream = entry.Open())
                            using (var fileStream = File.OpenRead(botFilePath))
                            {
                                await fileStream.CopyToAsync(entryStream);
                            }
                        }
                    }
                    zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
                    // use stream as needed
                    byte[] zipBytes = zipFileMemoryStream.ToArray(); //get all flushed data
                    string zipPath = Path.Combine(folderPath, "compressed.zip");
                    File.WriteAllBytes(Path.Combine(folderPath, "compressed.zip"), zipBytes);
                    Directory.Delete(folderPath, true);

                    return new FileReturn
                    {
                        Bytes = zipBytes,
                        ContentType = MimeTypes.GetMimeType(zipPath),
                        FileName = Path.GetFileName(zipPath)
                    };
                }
            }

            byte[] bytes = File.ReadAllBytes(filePaths[0]);
            Directory.Delete(folderPath, true);
            return new FileReturn
            {
                Bytes = bytes,
                ContentType = MimeTypes.GetMimeType(filePaths[0]),
                FileName = Path.GetFileName(filePaths[0])
            };
        }

        public async Task<bool> UpdateNgayKyAsync(MauHoaDonViewModel model)
        {
            MauHoaDon entity = await _db.MauHoaDons.FirstOrDefaultAsync(x => x.MauHoaDonId == model.MauHoaDonId);
            entity.NgayKy = model.NgayKy;
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<MauHoaDonViewModel> GetNgayKyByIdAsync(string id)
        {
            MauHoaDonViewModel result = await _db.MauHoaDons
                .Where(x => x.MauHoaDonId == id)
                .Select(x => new MauHoaDonViewModel
                {
                    MauHoaDonId = x.MauHoaDonId,
                    NgayKy = x.NgayKy
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<NhatKyTruyCapViewModel>> GetListNhatKyHoaDonAsync(string id)
        {
            List<NhatKyTruyCapViewModel> result = await (from nktc in _db.NhatKyTruyCaps
                                                         join u in _db.Users on nktc.CreatedBy equals u.UserId
                                                         where nktc.RefId == id
                                                         orderby nktc.CreatedDate descending
                                                         select new NhatKyTruyCapViewModel
                                                         {
                                                             NhatKyTruyCapId = nktc.NhatKyTruyCapId,
                                                             CreatedBy = nktc.CreatedBy,
                                                             CreatedByUserName = u.UserName,
                                                             HanhDong = nktc.HanhDong,
                                                             CreatedDate = nktc.CreatedDate,
                                                             MoTaChiTiet = nktc.MoTaChiTiet
                                                         })
                                                         .ToListAsync();

            return result;
        }

        public List<ImageParam> GetBackgrounds()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "hinh-nen-mau-hoa-don.json")).ToList();

            foreach (var item in list)
            {
                item.background = "/images/background/" + item.value;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }

        public List<ImageParam> GetBorders()
        {
            string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string jsonPath = Path.Combine(_hostingEnvironment.WebRootPath, "jsons");

            var list = new List<ImageParam>().Deserialize(Path.Combine(jsonPath, "khung-vien-mau-hoa-don.json")).ToList();

            foreach (var item in list)
            {
                item.background = "/images/border/" + item.value;
            }

            list = list.OrderBy(x => x.code).ToList();
            return list;
        }
    }
}
