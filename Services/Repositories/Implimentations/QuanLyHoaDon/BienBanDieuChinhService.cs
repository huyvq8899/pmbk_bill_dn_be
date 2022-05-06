using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Formatting;
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
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly IThongTinHoaDonService _thongTinHoaDonService;

        public BienBanDieuChinhService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor
            , IHoaDonDienTuService hoaDonDienTuService, IThongTinHoaDonService thongTinHoaDonService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _hoaDonDienTuService = hoaDonDienTuService;
            _thongTinHoaDonService = thongTinHoaDonService;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == id);
            _db.BienBanDieuChinhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;

            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, _db);

            return result;
        }

        /// <summary>
        /// Hàm xử lí thao tác ký biên bản điều chỉnh
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<BienBanDieuChinhViewModel> GateForWebSocket(ParamPhatHanhBBDC param)
        {
            if (!string.IsNullOrEmpty(param.BienBanDieuChinhId))
            {
                var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string assetsFolder = $"FilesUpload/{databaseName}";

                var _objBBDC = await GetByIdAsync(param.BienBanDieuChinhId);
                if (_objBBDC != null)
                {
                    string oldSignedPdfPath = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, $"{ManageFolderPath.PDF_SIGNED}/{_objBBDC.FileDaKy}");
                    if (File.Exists(oldSignedPdfPath))
                    {
                        File.Delete(oldSignedPdfPath);
                    }

                    string newPdfFileName = $"BBDC-{Guid.NewGuid()}.pdf";
                    string newSignedPdfFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder, ManageFolderPath.PDF_SIGNED);
                    if (!Directory.Exists(newSignedPdfFolder))
                    {
                        Directory.CreateDirectory(newSignedPdfFolder);
                    }
                    else
                    {
                        //FileHelper.ClearFolder(newSignedPdfFolder);
                    }

                    _objBBDC.FileDaKy = newPdfFileName;
                    if (param.IsKyBenB == true)
                    {
                        _objBBDC.NgayKyBenB = DateTime.Now;
                        _objBBDC.TrangThaiBienBan = (int)LoaiTrangThaiBienBanDieuChinhHoaDon.KhachHangDaKy;
                        _objBBDC.CertB = param.CertB;
                    }
                    else
                    {
                        _objBBDC.NgayKyBenA = DateTime.Now;
                        _objBBDC.TrangThaiBienBan = (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaGuiKhachHang;
                        _objBBDC.CertA = param.Cert;
                    }
                    await UpdateAsync(_objBBDC);

                    var fileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == param.BienBanDieuChinhId);
                    if (fileData == null)
                    {
                        fileData = new FileData
                        {
                            RefId = param.BienBanDieuChinhId,
                            Type = 2,
                            DateTime = DateTime.Now,
                            FileName = newPdfFileName,
                            IsSigned = true
                        };
                    }

                    // PDF 
                    byte[] bytePDF = DataHelper.StringToByteArray(@param.DataPDF);
                    fileData.Binary = bytePDF;
                    File.WriteAllBytes(Path.Combine(newSignedPdfFolder, newPdfFileName), bytePDF);

                    if (string.IsNullOrEmpty(fileData.FileDataId))
                    {
                        await _db.FileDatas.AddAsync(fileData);
                    }
                    else _db.Update(fileData);

                    await _db.SaveChangesAsync();
                    return _objBBDC;
                }
            }

            return null;
        }

        public async Task<BienBanDieuChinhViewModel> GetByIdAsync(string id)
        {
            try
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

                var query = from bbdc in _db.BienBanDieuChinhs
                            join hddt in _db.HoaDonDienTus on bbdc.HoaDonBiDieuChinhId equals hddt.HoaDonDienTuId into tmpDieuChinhs
                            from hddt in tmpDieuChinhs.DefaultIfEmpty()
                            join bkh in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId into tmpBoKyHieus
                            from bkh in tmpBoKyHieus.DefaultIfEmpty()
                            join tthd in _db.ThongTinHoaDons on bbdc.HoaDonBiDieuChinhId equals tthd.Id into tmpTT
                            from tthd in tmpTT.DefaultIfEmpty()
                            where bbdc.BienBanDieuChinhId == id
                            select new BienBanDieuChinhViewModel
                            {
                                BienBanDieuChinhId = bbdc.BienBanDieuChinhId,
                                SoBienBan = bbdc.SoBienBan,
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
                                IsCheckNgay = bbdc.IsCheckNgay,
                                HoaDonBiDieuChinh = new HoaDonDienTuViewModel
                                {
                                    HoaDonDienTuId = hddt != null ? hddt.HoaDonDienTuId : tthd.Id,
                                    NgayHoaDon = hddt != null ? hddt.NgayHoaDon : tthd.NgayHoaDon,
                                    SoHoaDon = hddt != null ? hddt.SoHoaDon : null,
                                    StrSoHoaDon = hddt != null ? hddt.SoHoaDon.ToString() : tthd.SoHoaDon,
                                    MauHoaDonId = hddt != null ? hddt.MauHoaDonId : string.Empty,
                                    MauSo = hddt != null ? bkh.KyHieuMauSoHoaDon.ToString() : tthd.MauSoHoaDon,
                                    KyHieu = hddt != null ? bkh.KyHieuHoaDon : tthd.KyHieuHoaDon,
                                    MaTraCuu = hddt != null ? hddt.MaTraCuu : tthd.MaTraCuu,
                                    NgayKy = hddt.NgayKy
                                },
                                HoaDonDieuChinhId = bbdc.HoaDonDieuChinhId,
                                CertA = bbdc.CertA,
                                CertB = bbdc.CertB,
                                CreatedBy = bbdc.CreatedBy,
                                CreatedDate = bbdc.CreatedDate,
                                Status = bbdc.Status,
                                TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                                   where tldk.NghiepVuId == bbdc.BienBanDieuChinhId
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
                            };

                var result = await query.AsNoTracking().FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }
        }

        public async Task<BienBanDieuChinhViewModel> InsertAsync(BienBanDieuChinhViewModel model)
        {
            try
            {
                model.BienBanDieuChinhId = Guid.NewGuid().ToString();

                model.HoaDonBiDieuChinh = null;
                model.TrangThaiBienBan = (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaKyBienBan;
                var entity = _mp.Map<BienBanDieuChinh>(model);
                await _db.BienBanDieuChinhs.AddAsync(entity);
                if (await _db.SaveChangesAsync() > 0)
                {
                    var result = _mp.Map<BienBanDieuChinhViewModel>(entity);

                    return result;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Xem biên bản điều chỉnh dưới dạng pdf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> PreviewBienBanAsync(string id)
        {
            var model = await GetByIdAsync(id);

            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}";
            string filePath = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}/{model.FileDaKy}";
            if (model.TrangThaiBienBan >= 2)
            {
                string fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
                if (!Directory.Exists(fullFolder))
                {
                    Directory.CreateDirectory(fullFolder);
                }

                string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);
                if (!File.Exists(fullPath))
                {
                    var fileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.RefId == model.BienBanDieuChinhId);
                    filePath = fileData != null ? filePath : null;
                   if(fileData != null) File.WriteAllBytes(fullPath, fileData.Binary);
                }
                if(filePath != null)  return filePath;
            }

            Document doc = new Document();

            string srcPath = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/HoaDonDieuChinh/Bien_ban_dieu_chinh_hoa_don.docx");
            string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_UNSIGN}";
            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            else
            {
                if (!string.IsNullOrEmpty(model.FileChuaKy))
                {
                    var oldFilePath = Path.Combine(destPath, model.FileChuaKy);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }
            }

            var signA = model.NgayKyBenA == null ? "(Ký, đóng dấu, ghi rõ họ và tên)" : "(Chữ ký số, chữ ký điện tử)";
            var signB = model.NgayKyBenB == null ? "(Ký, đóng dấu, ghi rõ họ và tên)" : "(Chữ ký số, chữ ký điện tử)";
            string tenDonViA = model.TenDonViBenA ?? string.Empty;
            string tenDonViB = model.TenDonViBenB ?? string.Empty;

            doc.LoadFromFile(srcPath);

            doc.Replace("<content>", model.NoiDungBienBan ?? string.Empty, true, true);
            doc.Replace("<date>", model.NgayBienBan.Value.ToString("dd") ?? string.Empty, true, true);
            doc.Replace("<month>", model.NgayBienBan.Value.ToString("MM") ?? string.Empty, true, true);
            doc.Replace("<year>", model.NgayBienBan.Value.ToString("yyyy") ?? string.Empty, true, true);

            doc.Replace("<CompanyName>", model.TenDonViBenA ?? string.Empty, true, true);
            doc.Replace("<Address>", model.DiaChiBenA ?? string.Empty, true, true);
            doc.Replace("<Taxcode>", model.MaSoThueBenA ?? string.Empty, true, true);
            doc.Replace("<Tel>", model.SoDienThoaiBenA ?? string.Empty, true, true);
            doc.Replace("<Representative>", model.DaiDienBenA ?? string.Empty, true, true);
            doc.Replace("<Position>", model.ChucVuBenA ?? string.Empty, true, true);

            doc.Replace("<CustomerCompany>", model.TenDonViBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerAddress>", model.DiaChiBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerTaxcode>", model.MaSoThueBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerTel>", model.SoDienThoaiBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerRepresentative>", model.DaiDienBenB ?? string.Empty, true, true);
            doc.Replace("<CustomerPosition>", model.ChucVuBenB ?? string.Empty, true, true);

            model.HoaDonBiDieuChinh = await _hoaDonDienTuService.GetByIdAsync(model.HoaDonBiDieuChinhId);
            if (model.HoaDonBiDieuChinh == null)
            {
                model.HoaDonBiDieuChinh = await _thongTinHoaDonService.GetById(model.HoaDonBiDieuChinhId);
            }
            string tempPath = Path.Combine(_hostingEnvironment.WebRootPath, "docs/temp.docx");
            model.HoaDonBiDieuChinh.GetMoTaBienBanDieuChinh(tempPath);
            Document destinationDoc = new Document();
            destinationDoc.LoadFromFile(tempPath);
            doc.Replace("<Description>", destinationDoc, false, true);
            destinationDoc.Close();
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
            doc.Replace("<reason>", model.LyDoDieuChinh ?? string.Empty, true, true);

            doc.Replace("<txtSignA>", signA, true, true);
            doc.Replace("<txtSignB>", signB, true, true);

            if (model.NgayKyBenA != null)
            {
                //var tenKySo = tenDonViA.GetTenKySo();
                //var signatureImage = ImageHelper.CreateImageSignature(tenKySo.Item1, tenKySo.Item2, LoaiNgonNgu.TiengViet, model.NgayKyBenA);

                //TextSelection selection = doc.FindString("<digitalSignatureA>", true, true);
                //if (selection != null)
                //{
                //    DocPicture pic = new DocPicture(doc);
                //    pic.LoadImage(signatureImage);
                //    pic.Width = pic.Width * 48 / 100;
                //    pic.Height = pic.Height * 48 / 100;

                //    var range = selection.GetAsOneRange();
                //    var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                //    range.OwnerParagraph.ChildObjects.Insert(index, pic);
                //    range.OwnerParagraph.ChildObjects.Remove(range);
                //}

                ImageHelper.CreateSignatureBox(doc, tenDonViA, model.NgayKyBenA, "<digitalSignatureA>");
            }
            else
            {
                doc.Replace("<digitalSignatureA>", string.Empty, true, true);
            }

            if (model.NgayKyBenB != null)
            {
                //var tenKySo = tenDonViB.GetTenKySo();
                //var signatureImage = ImageHelper.CreateImageSignature(tenKySo.Item1, tenKySo.Item2, LoaiNgonNgu.TiengViet, model.NgayKyBenB);

                //TextSelection selection = doc.FindString("<digitalSignatureB>", true, true);
                //if (selection != null)
                //{
                //    DocPicture pic = new DocPicture(doc);
                //    pic.LoadImage(signatureImage);
                //    pic.Width = pic.Width * 48 / 100;
                //    pic.Height = pic.Height * 48 / 100;

                //    var range = selection.GetAsOneRange();
                //    var index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                //    range.OwnerParagraph.ChildObjects.Insert(index, pic);
                //    range.OwnerParagraph.ChildObjects.Remove(range);
                //}

                ImageHelper.CreateSignatureBox(doc, tenDonViB, model.NgayKyBenB, "<digitalSignatureB>");
            }
            else
            {
                doc.Replace("<digitalSignatureB>", string.Empty, true, true);
            }

            string fileName = $"BBDC-{Guid.NewGuid()}.pdf";
            filePath = Path.Combine(destPath, fileName);
            //doc.SaveToFile(filePath, FileFormat.PDF);
            doc.SaveToPDF(filePath, _hostingEnvironment, LoaiNgonNgu.TiengViet);

            if (model.TrangThaiBienBan < 2)
                model.FileChuaKy = fileName;
            else model.FileDaKy = fileName;
            await UpdateAsync(model);

            return Path.Combine(folderPath, fileName);
        }

        public async Task<bool> UpdateAsync(BienBanDieuChinhViewModel model)
        {
            model.HoaDonBiDieuChinh = null;
            var entity = await _db.BienBanDieuChinhs.FirstOrDefaultAsync(x => x.BienBanDieuChinhId == model.BienBanDieuChinhId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
