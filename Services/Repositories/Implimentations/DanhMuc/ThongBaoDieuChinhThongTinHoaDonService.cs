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
using Services.ViewModels.FormActions;
using Services.ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon;
using Services.ViewModels.XML.ThongBaoPhatHanhHoaDon;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonService : IThongBaoDieuChinhThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public ThongBaoDieuChinhThongTinHoaDonService(
            Datacontext datacontext,
            IMapper mapper,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor,
            IHoSoHDDTService hoSoHDDTService)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            bool result = await _db.ThongBaoDieuChinhThongTinHoaDons
               .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.ThongBaoDieuChinhThongTinHoaDons.FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == id);
            _db.ThongBaoDieuChinhThongTinHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;

            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.ThongBaoDieuChinhThongTinHoaDon, _db);

            return result;
        }

        public async Task<PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllPagingAsync(ThongBaoDieuChinhThongTinHoaDonParams @params)
        {
            List<DistrictsParam> coQuanThueQuanLys = _hoSoHDDTService.GetListCoQuanThueQuanLy();

            var query = _db.ThongBaoDieuChinhThongTinHoaDons
                .OrderByDescending(x => x.NgayThongBaoDieuChinh).ThenByDescending(x => x.So)
                .Select(x => new ThongBaoDieuChinhThongTinHoaDonViewModel
                {
                    ThongBaoDieuChinhThongTinHoaDonId = x.ThongBaoDieuChinhThongTinHoaDonId,
                    NgayThongBaoDieuChinh = x.NgayThongBaoDieuChinh,
                    NgayThongBaoPhatHanh = x.NgayThongBaoPhatHanh,
                    CoQuanThue = x.CoQuanThue,
                    TenCoQuanThue = coQuanThueQuanLys.FirstOrDefault(y => y.Code == x.CoQuanThue).Name,
                    So = x.So,
                    TrangThaiHieuLuc = x.TrangThaiHieuLuc,
                    TenTrangThai = x.TrangThaiHieuLuc.GetDescription(),
                    Status = x.Status,
                    NoiDungThayDoi = GetNoiDungThayDoi(x)
                });

            if (@params.Filter != null)
            {

            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.NgayThongBaoDieuChinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayThongBaoDieuChinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayThongBaoDieuChinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.So))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.So);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.So);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TenCoQuanThue))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenCoQuanThue);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenCoQuanThue);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.NoiDungThayDoi))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NoiDungThayDoi);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NoiDungThayDoi);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        private string GetNoiDungThayDoi(ThongBaoDieuChinhThongTinHoaDon model)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(model.TenDonViCu) || !string.IsNullOrEmpty(model.TenDonViMoi))
            {
                list.Add($"Tên đơn vị từ <{model.TenDonViCu}> thành <{model.TenDonViMoi}>");
            }
            if (!string.IsNullOrEmpty(model.DiaChiCu) || !string.IsNullOrEmpty(model.DiaChiMoi))
            {
                list.Add($"Địa chỉ từ <{model.DiaChiCu}> thành <{model.DiaChiMoi}>");
            }
            if (!string.IsNullOrEmpty(model.DienThoaiCu) || !string.IsNullOrEmpty(model.DienThoaiMoi))
            {
                list.Add($"Điện thoại từ <{model.DienThoaiCu}> thành &lt;{model.DienThoaiMoi}>");
            }

            string result = "Thông tin thay đổi: " + string.Join(';', list.ToArray()) + ".";
            return result;
        }

        public async Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetBangKeHoaDonChuaSuDungAsync(string id)
        {
            var query = from mhd in _db.MauHoaDons
                        join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                        join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                        join tbdcct in _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == id)
                        on mhd.MauHoaDonId equals tbdcct.MauHoaDonId into tmpTBDCCTs
                        from tbdcct in tmpTBDCCTs.DefaultIfEmpty()
                        where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan
                        select new ThongBaoDieuChinhThongTinHoaDonChiTietViewModel
                        {
                            ThongBaoDieuChinhThongTinHoaDonChiTietId = tbdcct != null ? tbdcct.ThongBaoDieuChinhThongTinHoaDonChiTietId : null,
                            MauHoaDonId = mhd.MauHoaDonId,
                            LoaiHoaDon = mhd.LoaiHoaDon,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            TuSo = tbphct.TuSo,
                            DenSo = tbphct.DenSo,
                            SoLuong = tbphct.SoLuong,
                            Checked = tbdcct != null
                        };

            var result = await query.OrderBy(x => x.MauSo).ThenBy(x => x.KyHieu).ToListAsync();
            return result;
        }

        public async Task<ThongBaoDieuChinhThongTinHoaDonViewModel> GetByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongBaoDieuChinhThongTinHoaDon);
            string folder = $@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{id}\FileAttach";

            var query = from tb in _db.ThongBaoDieuChinhThongTinHoaDons
                        where tb.ThongBaoDieuChinhThongTinHoaDonId == id
                        select new ThongBaoDieuChinhThongTinHoaDonViewModel
                        {
                            ThongBaoDieuChinhThongTinHoaDonId = tb.ThongBaoDieuChinhThongTinHoaDonId,
                            NgayThongBaoDieuChinh = tb.NgayThongBaoDieuChinh,
                            NgayThongBaoPhatHanh = tb.NgayThongBaoPhatHanh,
                            CoQuanThue = tb.CoQuanThue,
                            So = tb.So,
                            TrangThaiHieuLuc = tb.TrangThaiHieuLuc,
                            TenDonViCu = tb.TenDonViCu,
                            TenDonViMoi = tb.TenDonViMoi,
                            DiaChiCu = tb.DiaChiCu,
                            DiaChiMoi = tb.DiaChiMoi,
                            DienThoaiCu = tb.DienThoaiCu,
                            DienThoaiMoi = tb.DienThoaiMoi,
                            ThongBaoDieuChinhThongTinHoaDonChiTiets = (from tbct in _db.ThongBaoDieuChinhThongTinHoaDonChiTiets
                                                                       orderby tb.CreatedDate
                                                                       select new ThongBaoDieuChinhThongTinHoaDonChiTietViewModel
                                                                       {
                                                                           ThongBaoDieuChinhThongTinHoaDonChiTietId = tbct.ThongBaoDieuChinhThongTinHoaDonChiTietId,
                                                                           ThongBaoDieuChinhThongTinHoaDonId = tbct.ThongBaoDieuChinhThongTinHoaDonId,
                                                                           MauHoaDonId = tbct.MauHoaDonId,
                                                                       })
                                                                       .ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == tb.ThongBaoDieuChinhThongTinHoaDonId
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
                            CreatedDate = tb.CreatedDate,
                            CreatedBy = tb.CreatedBy,
                            Status = tb.Status
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public List<EnumModel> GetTrangThaiHieuLucs()
        {
            List<EnumModel> enums = ((TrangThaiHieuLuc[])Enum.GetValues(typeof(TrangThaiHieuLuc)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public async Task<ThongBaoDieuChinhThongTinHoaDonViewModel> InsertAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> detailVMs = model.ThongBaoDieuChinhThongTinHoaDonChiTiets;

            model.ThongBaoDieuChinhThongTinHoaDonChiTiets = null;
            model.ThongBaoDieuChinhThongTinHoaDonId = string.IsNullOrEmpty(model.ThongBaoDieuChinhThongTinHoaDonId) ? Guid.NewGuid().ToString() : model.ThongBaoDieuChinhThongTinHoaDonId;
            ThongBaoDieuChinhThongTinHoaDon entity = _mp.Map<ThongBaoDieuChinhThongTinHoaDon>(model);
            await _db.ThongBaoDieuChinhThongTinHoaDons.AddAsync(entity);

            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoDieuChinhThongTinHoaDonId = entity.ThongBaoDieuChinhThongTinHoaDonId;
                var detail = _mp.Map<ThongBaoDieuChinhThongTinHoaDonChiTiet>(item);
                await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongBaoDieuChinhThongTinHoaDonViewModel result = _mp.Map<ThongBaoDieuChinhThongTinHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> detailVMs = model.ThongBaoDieuChinhThongTinHoaDonChiTiets;

            model.ThongBaoDieuChinhThongTinHoaDonChiTiets = null;
            ThongBaoDieuChinhThongTinHoaDon entity = await _db.ThongBaoDieuChinhThongTinHoaDons.FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == model.ThongBaoDieuChinhThongTinHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<ThongBaoDieuChinhThongTinHoaDonChiTiet> details = await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == model.ThongBaoDieuChinhThongTinHoaDonId).ToListAsync();
            _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.RemoveRange(details);
            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoDieuChinhThongTinHoaDonId = entity.ThongBaoDieuChinhThongTinHoaDonId;
                var detail = _mp.Map<ThongBaoDieuChinhThongTinHoaDonChiTiet>(item);
                await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.AddAsync(detail);
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel>> GetThongBaoDieuChinhThongTinChiTietByIdAsync(string id)
        {
            var result = await GetBangKeHoaDonChuaSuDungAsync(id);
            var chiTiets = await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets
                .Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == id)
                .ToListAsync();

            result = result.Where(x => chiTiets.Any(y => y.MauHoaDonId == x.MauHoaDonId)).ToList();
            return result;
        }

        public async Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau, int loai)
        {
            ThongBaoDieuChinhThongTinHoaDonViewModel model = await GetByIdAsync(id);
            model.TenCoQuanThue = _hoSoHDDTService.GetListCoQuanThueQuanLy().FirstOrDefault(x => x.Code == model.CoQuanThue).Name;

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            FileFormat fileFormat;
            string fileName;
            string filePath;

            if (dinhDangTepMau != DinhDangTepMau.XML)
            {
                Document doc = new Document();

                string srcPath = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/ThongBaoDieuChinhThongTinHoaDon/{(loai == 1 ? "Thong_bao_dieu_chinh_thong_tin_HDDT" : "Bang_ke_hoa_don_chua_su_dung")}.docx");

                doc.LoadFromFile(srcPath);
                Section section = doc.Sections[0];

                doc.Replace("<tenDonVi>", hoSoHDDTVM.TenDonVi ?? string.Empty, true, true);
                doc.Replace("<maSoThue>", hoSoHDDTVM.MaSoThue ?? string.Empty, true, true);
                doc.Replace("<diaChi>", hoSoHDDTVM.DiaChi ?? string.Empty, true, true);
                doc.Replace("<dienThoai>", hoSoHDDTVM.SoDienThoaiLienHe ?? string.Empty, true, true);
                doc.Replace("<DD>", model.NgayThongBaoPhatHanh.Value.ToString("dd") ?? string.Empty, true, true);
                doc.Replace("<MM>", model.NgayThongBaoPhatHanh.Value.ToString("MM") ?? string.Empty, true, true);
                doc.Replace("<YY>", model.NgayThongBaoPhatHanh.Value.ToString("yyyy") ?? string.Empty, true, true);

                if (loai == 1)
                {
                    model.ThongTinThayDois = model.GetThongTinThayDois();
                }
                else
                {
                    model.ThongBaoDieuChinhThongTinHoaDonChiTiets = await GetThongBaoDieuChinhThongTinChiTietByIdAsync(id);
                }

                int line = loai == 1 ? model.ThongTinThayDois.Count() : model.ThongBaoDieuChinhThongTinHoaDonChiTiets.Count();
                Table table = null;
                string stt = string.Empty;
                foreach (Table tb in doc.Sections[0].Tables)
                {
                    if (tb.Rows.Count > 0)
                    {
                        foreach (Paragraph par in tb.Rows[0].Cells[0].Paragraphs)
                        {
                            stt = par.Text;
                        }
                        if (stt.Contains("STT"))
                        {
                            table = tb;
                            break;
                        }
                    }
                }

                int beginRow = 1;
                for (int i = 0; i < line - 1; i++)
                {
                    // Clone row
                    TableRow cl_row = table.Rows[beginRow].Clone();
                    table.Rows.Insert(beginRow, cl_row);
                }

                for (int i = 0; i < line; i++)
                {
                    TableRow row = table.Rows[i + beginRow];

                    if (loai == 1)
                    {
                        ThongTinThayDoi item = model.ThongTinThayDois[i];

                        row.Cells[0].Paragraphs[0].Text = (i + 1).ToString();
                        row.Cells[1].Paragraphs[0].Text = item.TenThongTin;
                        row.Cells[2].Paragraphs[0].Text = item.ThongTinCu;
                        row.Cells[3].Paragraphs[0].Text = item.ThongTinMoi;
                    }
                    else
                    {
                        ThongBaoDieuChinhThongTinHoaDonChiTietViewModel item = model.ThongBaoDieuChinhThongTinHoaDonChiTiets[i];

                        row.Cells[0].Paragraphs[0].Text = (i + 1).ToString();
                        row.Cells[1].Paragraphs[0].Text = item.TenLoaiHoaDon;
                        row.Cells[2].Paragraphs[0].Text = item.MauSo;
                        row.Cells[3].Paragraphs[0].Text = item.KyHieu;
                        row.Cells[4].Paragraphs[0].Text = item.SoLuong.Value.ToString("N0");
                        row.Cells[5].Paragraphs[0].Text = item.TuSo.Value.PadZerro();
                        row.Cells[6].Paragraphs[0].Text = item.DenSo.Value.PadZerro();
                    }
                }

                doc.Replace("<tenCoQuanThue>", model.TenCoQuanThue ?? string.Empty, true, true);

                doc.Replace("<dd>", model.NgayThongBaoDieuChinh.Value.ToString("dd") ?? string.Empty, true, true);
                doc.Replace("<mm>", model.NgayThongBaoDieuChinh.Value.ToString("MM") ?? string.Empty, true, true);
                doc.Replace("<yyyy>", model.NgayThongBaoDieuChinh.Value.ToString("yyyy") ?? string.Empty, true, true);

                doc.Replace("<tenNguoiDaiDien>", (hoSoHDDTVM.HoTenNguoiDaiDienPhapLuat ?? string.Empty).ToUpper(), true, true);

                if (dinhDangTepMau == DinhDangTepMau.PDF)
                {
                    fileName = $"{Guid.NewGuid()}.pdf";
                    fileFormat = FileFormat.PDF;
                }
                else if (dinhDangTepMau == DinhDangTepMau.DOC)
                {
                    fileName = $"{Guid.NewGuid()}.doc";
                    fileFormat = FileFormat.Doc;
                }
                else
                {
                    fileName = $"{Guid.NewGuid()}.docx";
                    fileFormat = FileFormat.Docx;
                }

                filePath = Path.Combine(destPath, fileName);
                doc.SaveToFile(filePath, fileFormat);
            }
            else
            {
                fileName = $"{Guid.NewGuid()}.xml";
                filePath = Path.Combine(destPath, fileName);
                CreateXML(model, hoSoHDDTVM, filePath);
            }

            byte[] fileByte = File.ReadAllBytes(filePath);
            File.Delete(filePath);

            return new FileReturn
            {
                Bytes = fileByte,
                ContentType = MimeTypes.GetMimeType(filePath),
                FileName = Path.GetFileName(filePath)
            };
        }

        private void CreateXML(ThongBaoDieuChinhThongTinHoaDonViewModel model, HoSoHDDTViewModel hoSoHDDT, string filePath)
        {
            ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon.HSoKhaiThue hSoKhaiThue = new ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon.HSoKhaiThue
            {
                TTinChung = new TTinChung
                {
                    TTinDVu = new TTinDVu
                    {
                        MaDVu = "BKSoft",
                        TenDVu = "pmbk.vn",
                        PbanDVu = "1.0.0.0",
                        TtinNhaCCapDVu = "Công Ty Cổ Phần Phát Triển Và Ứng Dụng Phần Mềm Bách Khoa"
                    },
                    TTinTKhaiThue = new TTinTKhaiThue
                    {
                        TKhaiThue = new TKhaiThue
                        {
                            MaTKhai = "129",
                            TenTKhai = "Thông báo điều chỉnh thông tin (TB04/AC)",
                            MoTaBMau = "(Ban hành kèm theo Thông tư số 39/2014/TT-BTC ngày 31/3/2014 của Bộ Tài chính)",
                            PbanTKhaiXML = "1.0.0",
                            LoaiTKhai = "C",
                            SoLan = "0",
                            KyKKhaiThue = new KyKKhaiThue
                            {
                                KieuKy = "D",
                                KyKKhai = model.NgayThongBaoDieuChinh.Value.ToString("dd/MM/yyyy"),
                                KyKKhaiTuNgay = model.NgayThongBaoDieuChinh.Value.ToString("dd/MM/yyyy"),
                                KyKKhaiDenNgay = model.NgayThongBaoDieuChinh.Value.ToString("dd/MM/yyyy"),
                                KyKKhaiTuThang = string.Empty,
                                KyKKhaiDenThang = string.Empty
                            },
                            MaCQTNoiNop = hoSoHDDT.CoQuanThueQuanLy,
                            TenCQTNoiNop = hoSoHDDT.TenCoQuanThueQuanLy,
                            NgayLapTKhai = DateTime.Now.ToString("yyyy-MM-dd"),
                            GiaHan = new GiaHan
                            {
                                MaLyDoGiaHan = string.Empty,
                                LyDoGiaHan = string.Empty
                            },
                            NguoiKy = hoSoHDDT.HoTenNguoiDaiDienPhapLuat,
                            NgayKy = DateTime.Now.ToString("yyyy-MM-dd"),
                            NganhNgheKD = string.Empty,
                        },
                        NNT = new NNT
                        {
                            Mst = hoSoHDDT.MaSoThue,
                            TenNNT = hoSoHDDT.TenDonVi,
                            DchiNNT = hoSoHDDT.DiaChi,
                            PhuongXa = string.Empty,
                            MaHuyenNNT = string.Empty,
                            TenHuyenNNT = string.Empty,
                            MaTinhNNT = string.Empty,
                            TenTinhNNT = string.Empty,
                            DthoaiNNT = string.Empty,
                            FaxNNT = string.Empty,
                            EmailNNT = string.Empty,
                        }
                    }
                },
                CTieuTKhaiChinh = new ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon.CTieuTKhaiChinh
                {
                    NgayTBaoPHanhHDon = model.NgayThongBaoPhatHanh.Value.ToString("yyyy-MM-dd"),
                    TTinThayDoi = new List<ChiTietTTinThayDoi>(),
                    TTinDonViChuQuan = new TTinDonViChuQuan
                    {
                        MstDViChuQuan = string.Empty,
                        TenDViChuQuan = string.Empty
                    },
                    TenCQTTiepNhanTBao = model.TenCoQuanThue,
                    NgayThongBao = model.NgayThongBaoPhatHanh.Value.ToString("yyyy-MM-dd"),
                    NguoiDaiDien = hoSoHDDT.HoTenNguoiDaiDienPhapLuat
                }
            };

            if (!string.IsNullOrEmpty(model.TenDonViCu) || !string.IsNullOrEmpty(model.TenDonViMoi))
            {
                hSoKhaiThue.CTieuTKhaiChinh.TTinThayDoi.Add(new ChiTietTTinThayDoi
                {
                    ThongTinThayDoi = "01DVPH",
                    ThongTinCu = model.TenDonViCu,
                    ThongTinMoi = model.TenDonViMoi
                });
            }

            if (!string.IsNullOrEmpty(model.DiaChiCu) || !string.IsNullOrEmpty(model.DiaChiMoi))
            {
                hSoKhaiThue.CTieuTKhaiChinh.TTinThayDoi.Add(new ChiTietTTinThayDoi
                {
                    ThongTinThayDoi = "02DCTS",
                    ThongTinCu = model.DiaChiCu,
                    ThongTinMoi = model.DiaChiMoi
                });
            }

            if (!string.IsNullOrEmpty(model.DienThoaiCu) || !string.IsNullOrEmpty(model.DienThoaiMoi))
            {
                hSoKhaiThue.CTieuTKhaiChinh.TTinThayDoi.Add(new ChiTietTTinThayDoi
                {
                    ThongTinThayDoi = "03PHONE",
                    ThongTinCu = model.DienThoaiCu,
                    ThongTinMoi = model.DienThoaiMoi
                });
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon.HSoKhaiThue));

            using (TextWriter filestream = new StreamWriter(filePath))
            {
                serialiser.Serialize(filestream, hSoKhaiThue, ns);
            }
        }

        public async Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model)
        {
            TienLuiViewModel result = new TienLuiViewModel();
            if (string.IsNullOrEmpty(model.ChungTuId))
            {
                return result;
            }

            var list = await _db.ThongBaoDieuChinhThongTinHoaDons
                .OrderBy(x => x.NgayThongBaoDieuChinh).ThenBy(x => x.So)
                .Select(x => new TienLuiViewModel
                {
                    ChungTuId = x.ThongBaoDieuChinhThongTinHoaDonId,
                })
                .ToListAsync();

            var length = list.Count();
            var currentIndex = list.FindIndex(x => x.ChungTuId == model.ChungTuId);
            if (currentIndex != -1)
            {
                if (currentIndex > 0)
                {
                    result.TruocId = list[currentIndex - 1].ChungTuId;
                    result.VeDauId = list[0].ChungTuId;
                }
                if (currentIndex < (length - 1))
                {
                    result.SauId = list[currentIndex + 1].ChungTuId;
                    result.VeCuoiId = list[length - 1].ChungTuId;
                }
            }

            return result;
        }
    }
}
