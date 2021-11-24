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
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
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
                                join hddt in _db.HoaDonDienTus on tbct.MauHoaDonId equals hddt.MauHoaDonId
                                where (tbct.ThongBaoKetQuaHuyHoaDonId == id) && (hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu)
                                select new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                {
                                    MauHoaDonId = tbct.MauHoaDonId,
                                    SoLuong = tbct.DenSo,
                                    SoLuongOther = int.Parse(hddt.SoHoaDon)
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
            var entity = await _db.ThongBaoKetQuaHuyHoaDons.FirstOrDefaultAsync(x => x.ThongBaoKetQuaHuyHoaDonId == id);
            _db.ThongBaoKetQuaHuyHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau)
        {
            ThongBaoKetQuaHuyHoaDonViewModel model = await GetByIdAsync(id);
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

                string srcPath = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/ThongBaoKetQuaHuyHoaDon/Thong_bao_ket_qua_huy_HDDT.docx");

                doc.LoadFromFile(srcPath);
                Section section = doc.Sections[0];

                doc.Replace("<tenDonVi>", hoSoHDDTVM.TenDonVi ?? string.Empty, true, true);
                doc.Replace("<maSoThue>", hoSoHDDTVM.MaSoThue ?? string.Empty, true, true);
                doc.Replace("<diaChi>", hoSoHDDTVM.DiaChi ?? string.Empty, true, true);
                doc.Replace("<phuongPhapHuy>", model.PhuongPhapHuy ?? string.Empty, true, true);
                doc.Replace("<HH>", model.NgayGioHuy.Value.ToString("HH") ?? string.Empty, true, true);
                doc.Replace("<pp>", model.NgayGioHuy.Value.ToString("mm") ?? string.Empty, true, true);
                doc.Replace("<DD>", model.NgayGioHuy.Value.ToString("dd") ?? string.Empty, true, true);
                doc.Replace("<MM>", model.NgayGioHuy.Value.ToString("MM") ?? string.Empty, true, true);
                doc.Replace("<YY>", model.NgayGioHuy.Value.ToString("yyyy") ?? string.Empty, true, true);

                int line = model.ThongBaoKetQuaHuyHoaDonChiTiets.Count();
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
                    ThongBaoKetQuaHuyHoaDonChiTietViewModel item = model.ThongBaoKetQuaHuyHoaDonChiTiets[i];

                    row.Cells[0].Paragraphs[0].Text = (i + 1).ToString();
                    row.Cells[1].Paragraphs[0].Text = item.TenLoaiHoaDon;
                    row.Cells[2].Paragraphs[0].Text = item.MauSo;
                    row.Cells[3].Paragraphs[0].Text = item.KyHieu;
                    row.Cells[4].Paragraphs[0].Text = item.TuSo.Value.PadZerro();
                    row.Cells[5].Paragraphs[0].Text = item.DenSo.Value.PadZerro();
                    row.Cells[6].Paragraphs[0].Text = item.SoLuong.Value.ToString("N0");
                }

                doc.Replace("<tenCoQuanThue>", model.TenCoQuanThue ?? string.Empty, true, true);

                doc.Replace("<dd>", model.NgayThongBao.Value.ToString("dd") ?? string.Empty, true, true);
                doc.Replace("<mm>", model.NgayThongBao.Value.ToString("MM") ?? string.Empty, true, true);
                doc.Replace("<yyyy>", model.NgayThongBao.Value.ToString("yyyy") ?? string.Empty, true, true);

                doc.Replace("<tenNguoiLap>", model.TenNguoiTao ?? string.Empty, true, true);
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

        private void CreateXML(ThongBaoKetQuaHuyHoaDonViewModel model, HoSoHDDTViewModel hoSoHDDT, string filePath)
        {
            ViewModels.XML.ThongBaoKetQuaHuyHoaDon.HSoKhaiThue hSoKhaiThue = new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.HSoKhaiThue
            {
                TTinChung = new TTinChung
                {
                    TTinDVu = new TTinDVu
                    {
                        MaDVu = "HTKK",
                        TenDVu = "HỖ TRỢ KÊ KHAI THUẾ",
                        PbanDVu = "4.1.6",
                        TtinNhaCCapDVu = "87CBA5FE8525AE3F361DEA2375F4A39F"
                    },
                    TTinTKhaiThue = new TTinTKhaiThue
                    {
                        TKhaiThue = new TKhaiThue
                        {
                            MaTKhai = "107",
                            TenTKhai = "Thông báo kết quả hủy hóa đơn (TB03/AC)",
                            MoTaBMau = "(Ban hành kèm theo Thông tư số 39/2014/TT-BTC ngày 31/3/2014 của Bộ Tài chính)",
                            PbanTKhaiXML = "1.0.0",
                            LoaiTKhai = "C",
                            SoLan = "0",
                            KyKKhaiThue = new KyKKhaiThue
                            {
                                KieuKy = "D",
                                KyKKhai = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                                KyKKhaiTuNgay = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                                KyKKhaiDenNgay = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
                                KyKKhaiTuThang = string.Empty,
                                KyKKhaiDenThang = string.Empty
                            },
                            MaCQTNoiNop = hoSoHDDT.CoQuanThueQuanLy,
                            TenCQTNoiNop = hoSoHDDT.TenCoQuanThueQuanLy,
                            NgayLapTKhai = model.CreatedDate.Value.ToString("dd/MM/yyyy"),
                            GiaHan = new GiaHan
                            {
                                MaLyDoGiaHan = string.Empty,
                                LyDoGiaHan = string.Empty
                            },
                            NguoiKy = string.Empty,
                            NgayKy = model.NgayGioHuy.Value.ToString("dd/MM/yyyy"),
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
                CTieuTKhaiChinh = new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.CTieuTKhaiChinh
                {
                    KinhGui = model.TenCoQuanThue,
                    PhuongPhapHuy = model.PhuongPhapHuy,
                    ThoiGian = model.NgayGioHuy.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") + "Z",
                    HoaDon = new List<ViewModels.XML.ThongBaoKetQuaHuyHoaDon.ChiTiet>(),
                    NguoiLapBieu = model.TenNguoiTao,
                    NguoiDaiDien = hoSoHDDT.HoTenNguoiDaiDienPhapLuat,
                    NgayBCao = model.NgayGioHuy.Value.ToString("yyyy-MM-dd")
                }
            };

            foreach (var item in model.ThongBaoKetQuaHuyHoaDonChiTiets)
            {
                hSoKhaiThue.CTieuTKhaiChinh.HoaDon.Add(new ViewModels.XML.ThongBaoKetQuaHuyHoaDon.ChiTiet
                {
                    TenHDon = item.TenLoaiHoaDon,
                    MauHDon = item.MauSo,
                    KyHieu = item.KyHieu,
                    TuSo = item.TuSo.Value.PadZerro(),
                    DenSo = item.DenSo.Value.PadZerro(),
                    SoLuong = item.SoLuong.Value.ToString("N0")
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

        public async Task<PagedList<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllPagingAsync(ThongBaoKetQuaHuyHoaDonParams @params)
        {
            var query = _db.ThongBaoKetQuaHuyHoaDons
                .OrderByDescending(x => x.NgayThongBao).ThenByDescending(x => x.So)
                .Select(x => new ThongBaoKetQuaHuyHoaDonViewModel
                {
                    ThongBaoKetQuaHuyHoaDonId = x.ThongBaoKetQuaHuyHoaDonId,
                    NgayThongBao = x.NgayThongBao,
                    So = x.So,
                    PhuongPhapHuy = x.PhuongPhapHuy,
                    TrangThaiNop = x.TrangThaiNop,
                    TenTrangThaiNop = x.TrangThaiNop.GetDescription()
                });

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.NgayThongBao))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayThongBao);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayThongBao);
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

                if (@params.SortKey == nameof(@params.Filter.PhuongPhapHuy))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.PhuongPhapHuy);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.PhuongPhapHuy);
                    }
                }
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
            string folder = $@"\FilesUpload\{databaseName}\{id}\FileAttach";

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
                            TenNguoiTao = u.FullName,
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

        public async Task<TienLuiViewModel> TienLuiChungTuAsync(TienLuiViewModel model)
        {
            TienLuiViewModel result = new TienLuiViewModel();
            if (string.IsNullOrEmpty(model.ChungTuId))
            {
                return result;
            }

            var list = await _db.ThongBaoKetQuaHuyHoaDons
                .OrderBy(x => x.NgayThongBao).ThenBy(x => x.So)
                .Select(x => new TienLuiViewModel
                {
                    ChungTuId = x.ThongBaoKetQuaHuyHoaDonId,
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
