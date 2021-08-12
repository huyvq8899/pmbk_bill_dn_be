using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class ThongBaoPhatHanhService : IThongBaoPhatHanhService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ThongBaoPhatHanhService(Datacontext datacontext, IMapper mapper, IHoSoHDDTService hoSoHDDTService, IHostingEnvironment hostingEnvironment)
        {
            _db = datacontext;
            _mp = mapper;
            _hoSoHDDTService = hoSoHDDTService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> CheckAllowUpdateDeleteAsync(string id)
        {
            string result = string.Empty;

            result = await (from tbphct in _db.ThongBaoPhatHanhChiTiets
                            join hddt in _db.HoaDonDienTus on tbphct.MauHoaDonId equals hddt.MauHoaDonId
                            join mhd in _db.MauHoaDons on tbphct.MauHoaDonId equals mhd.MauHoaDonId
                            where (TrangThaiPhatHanh)hddt.TrangThaiPhatHanh == TrangThaiPhatHanh.DaPhatHanh &&
                            tbphct.ThongBaoPhatHanhId == id
                            select new
                            {
                                Message = $"Mẫu hóa đơn &lt;{mhd.MauSo}&gt;, &lt;{tbphct.KyHieu}&gt;, Từ số &lt;{tbphct.TuSo.Value.PadZerro()}&gt; đến số &lt;{tbphct.DenSo.Value.PadZerro()}&gt; đã có phát sinh trên &lt;{mhd.LoaiHoaDon.GetDescription()}&gt;"
                            })
                            .Select(x => x.Message)
                            .Distinct()
                            .FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoPhatHanhViewModel model)
        {
            bool result = await _db.ThongBaoPhatHanhs
                .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.ThongBaoPhatHanhs.FirstOrDefaultAsync(x => x.ThongBaoPhatHanhId == id);
            _db.ThongBaoPhatHanhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau)
        {
            ThongBaoPhatHanhViewModel model = await GetByIdAsync(id);

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            Document doc = new Document();

            string srcPath = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/ThongBaoPhatHanhHDDT/Thong_bao_phat_hanh_HDDT.docx");
            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            doc.LoadFromFile(srcPath);
            Section section = doc.Sections[0];

            doc.Replace("<tenDonVi>", hoSoHDDTVM.TenDonVi ?? string.Empty, true, true);
            doc.Replace("<maSoThue>", hoSoHDDTVM.MaSoThue ?? string.Empty, true, true);
            doc.Replace("<diaChi>", hoSoHDDTVM.DiaChi ?? string.Empty, true, true);
            doc.Replace("<dienThoai>", model.DienThoai ?? string.Empty, true, true);

            int line = model.ThongBaoPhatHanhChiTiets.Count();
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
                ThongBaoPhatHanhChiTietViewModel item = model.ThongBaoPhatHanhChiTiets[i];

                row.Cells[0].Paragraphs[0].Text = (i + 1).ToString();
                row.Cells[1].Paragraphs[0].Text = item.TenLoaiHoaDon;
                row.Cells[2].Paragraphs[0].Text = item.MauSoHoaDon;
                row.Cells[3].Paragraphs[0].Text = item.KyHieu;
                row.Cells[4].Paragraphs[0].Text = item.SoLuong.Value.ToString("N0");
                row.Cells[5].Paragraphs[0].Text = item.TuSo.Value.PadZerro();
                row.Cells[6].Paragraphs[0].Text = item.DenSo.Value.PadZerro();
                row.Cells[7].Paragraphs[0].Text = item.NgayBatDauSuDung.Value.ToString("dd/MM/yyyy");
            }

            string tenCoQuanThue = _hoSoHDDTService.GetListCoQuanThueQuanLy().FirstOrDefault(x => x.code == model.CoQuanThue).name;
            doc.Replace("<tenCoQuanThue>", tenCoQuanThue ?? string.Empty, true, true);

            doc.Replace("<dd>", model.Ngay.ToString("dd") ?? string.Empty, true, true);
            doc.Replace("<mm>", model.Ngay.ToString("MM") ?? string.Empty, true, true);
            doc.Replace("<yyyy>", model.Ngay.ToString("yyyy") ?? string.Empty, true, true);

            doc.Replace("<tenNguoiDaiDien>", model.NguoiDaiDienPhapLuat.ToUpper() ?? string.Empty, true, true);

            FileFormat fileFormat;
            string fileName;
            string filePath;

            if (dinhDangTepMau != DinhDangTepMau.XML)
            {
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

        public Task<List<ThongBaoPhatHanhViewModel>> GetAllAsync(ThongBaoPhatHanhParams @params = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ThongBaoPhatHanhViewModel>> GetAllPagingAsync(ThongBaoPhatHanhParams @params)
        {
            var query = _db.ThongBaoPhatHanhs
                .OrderByDescending(x => x.Ngay).OrderByDescending(x => x.So)
                .Select(x => new ThongBaoPhatHanhViewModel
                {
                    ThongBaoPhatHanhId = x.ThongBaoPhatHanhId,
                    Ngay = x.Ngay,
                    So = x.So,
                    TrangThaiNop = x.TrangThaiNop,
                    TenTrangThaiNop = x.TrangThaiNop.GetDescription()
                });

            if (@params.Filter != null)
            {
                //if (!string.IsNullOrEmpty(@params.Filter.Ma))
                //{
                //    var keyword = @params.Filter.Ma.ToUpper().ToTrim();
                //    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword));
                //}
                //if (!string.IsNullOrEmpty(@params.Filter.Ten))
                //{
                //    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                //    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                //}
                //if (!string.IsNullOrEmpty(@params.Filter.MaSoThue))
                //{
                //    var keyword = @params.Filter.MaSoThue.ToUpper().ToTrim();
                //    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                //}
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.Ngay))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ngay);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ngay);
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
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<ThongBaoPhatHanhViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ThongBaoPhatHanhViewModel> GetByIdAsync(string id)
        {
            var query = from tbph in _db.ThongBaoPhatHanhs
                        where tbph.ThongBaoPhatHanhId == id
                        select new ThongBaoPhatHanhViewModel
                        {
                            ThongBaoPhatHanhId = tbph.ThongBaoPhatHanhId,
                            DienThoai = tbph.DienThoai,
                            CoQuanThue = tbph.CoQuanThue,
                            NguoiDaiDienPhapLuat = tbph.NguoiDaiDienPhapLuat,
                            Ngay = tbph.Ngay,
                            So = tbph.So,
                            TrangThaiNop = tbph.TrangThaiNop,
                            IsXacNhan = tbph.IsXacNhan,
                            CreatedDate = tbph.CreatedDate,
                            CreatedBy = tbph.CreatedBy,
                            Status = tbph.Status,
                            ThongBaoPhatHanhChiTiets = (from tbphct in _db.ThongBaoPhatHanhChiTiets
                                                        join mhd in _db.MauHoaDons on tbphct.MauHoaDonId equals mhd.MauHoaDonId
                                                        where tbphct.ThongBaoPhatHanhId == tbph.ThongBaoPhatHanhId
                                                        orderby tbphct.CreatedDate
                                                        select new ThongBaoPhatHanhChiTietViewModel
                                                        {
                                                            ThongBaoPhatHanhChiTietId = tbphct.ThongBaoPhatHanhChiTietId,
                                                            ThongBaoPhatHanhId = tbphct.ThongBaoPhatHanhId,
                                                            MauHoaDonId = tbphct.MauHoaDonId,
                                                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                                                            MauSoHoaDon = mhd.MauSo,
                                                            KyHieu = tbphct.KyHieu,
                                                            SoLuong = tbphct.SoLuong,
                                                            TuSo = tbphct.TuSo,
                                                            DenSo = tbphct.DenSo,
                                                            NgayBatDauSuDung = tbphct.NgayBatDauSuDung
                                                        })
                                                        .ToList()
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ThongBaoPhatHanhChiTietViewModel>> GetCacLoaiHoaDonPhatHanhsAsync(string id)
        {
            List<ThongBaoPhatHanhChiTietViewModel> result = new List<ThongBaoPhatHanhChiTietViewModel>();

            var leftJoin = await (from mhd in _db.MauHoaDons
                                  select new ThongBaoPhatHanhChiTietViewModel
                                  {
                                      MauHoaDonId = mhd.MauHoaDonId,
                                      TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                                      MauSoHoaDon = mhd.MauSo,
                                      KyHieu = mhd.KyHieu,
                                      Status = true
                                  })
                                  .ToListAsync();

            var rightJoin = await (from tbphct in _db.ThongBaoPhatHanhChiTiets
                                   join mhd in _db.MauHoaDons on tbphct.MauHoaDonId equals mhd.MauHoaDonId
                                   select new ThongBaoPhatHanhChiTietViewModel
                                   {
                                       ThongBaoPhatHanhChiTietId = tbphct.ThongBaoPhatHanhChiTietId,
                                       ThongBaoPhatHanhId = tbphct.ThongBaoPhatHanhId,
                                       MauHoaDonId = tbphct.MauHoaDonId,
                                       TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                                       MauSoHoaDon = mhd.MauSo,
                                       KyHieu = tbphct.KyHieu,
                                       SoLuong = tbphct.SoLuong,
                                       TuSo = tbphct.TuSo,
                                       DenSo = tbphct.DenSo,
                                       NgayBatDauSuDung = tbphct.NgayBatDauSuDung,
                                       Checked = true,
                                       Status = true
                                   })
                                   .ToListAsync();

            var fullJoin = new List<ThongBaoPhatHanhChiTietViewModel>();
            if (string.IsNullOrEmpty(id))
            {
                fullJoin = leftJoin.ToList();
            }
            else
            {
                fullJoin = leftJoin.Union(rightJoin).ToList();
            }

            fullJoin = fullJoin.GroupBy(x => new { x.MauHoaDonId, x.KyHieu })
                .Select(x => new ThongBaoPhatHanhChiTietViewModel
                {
                    MauHoaDonId = x.Key.MauHoaDonId,
                    TenLoaiHoaDon = x.First().TenLoaiHoaDon,
                    MauSoHoaDon = x.First().MauSoHoaDon,
                    KyHieu = x.Key.KyHieu,
                    Status = true
                })
                .ToList();

            foreach (var full in fullJoin)
            {
                var right = rightJoin.FirstOrDefault(x => x.MauHoaDonId == full.MauHoaDonId && x.KyHieu == full.KyHieu && x.ThongBaoPhatHanhId == id);
                if (right == null)
                {
                    var maxRight = rightJoin.Where(x => x.MauHoaDonId == full.MauHoaDonId && x.KyHieu == full.KyHieu).OrderByDescending(x => x.TuSo).FirstOrDefault();

                    full.SoLuong = 0;
                    full.TuSo = maxRight != null ? (maxRight.DenSo + 1) : 1;
                    full.NgayBatDauSuDung = DateTime.Now.AddDays(2);
                    result.Add(full);
                }
                else
                {
                    result.Add(right);
                }
            }

            return result;
        }

        public async Task<List<ThongBaoPhatHanhChiTietViewModel>> GetListChiTietThongBaoPhatHanhByMauHoaDonIdAsync(string mauHoaDonId)
        {
            List<ThongBaoPhatHanhChiTietViewModel> result = await (from tbphct in _db.ThongBaoPhatHanhChiTiets
                                                                   join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                                                                   join mhd in _db.MauHoaDons on tbphct.MauHoaDonId equals mhd.MauHoaDonId
                                                                   where tbphct.MauHoaDonId == mauHoaDonId
                                                                   orderby tbph.Ngay
                                                                   select new ThongBaoPhatHanhChiTietViewModel
                                                                   {
                                                                       NgayTao = tbph.Ngay,
                                                                       So = tbph.So,
                                                                       TenTrangThai = tbph.TrangThaiNop.GetDescription(),
                                                                       MauSoHoaDon = mhd.MauSo,
                                                                       KyHieu = tbphct.KyHieu,
                                                                       SoLuong = tbphct.SoLuong,
                                                                       TuSo = tbphct.TuSo,
                                                                       DenSo = tbphct.DenSo,
                                                                       NgayBatDauSuDung = tbphct.NgayBatDauSuDung
                                                                   })
                                                                    .ToListAsync();

            return result;
        }

        public async Task<List<ThongBaoPhatHanhChiTietViewModel>> GetThongBaoPhatHanhChiTietByIdAsync(string id)
        {
            var query = from tbphct in _db.ThongBaoPhatHanhChiTiets
                        join mhd in _db.MauHoaDons on tbphct.MauHoaDonId equals mhd.MauHoaDonId
                        where tbphct.ThongBaoPhatHanhId == id
                        orderby tbphct.CreatedDate
                        select new ThongBaoPhatHanhChiTietViewModel
                        {
                            ThongBaoPhatHanhId = tbphct.ThongBaoPhatHanhId,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            MauHoaDonId = mhd.MauHoaDonId,
                            MauSoHoaDon = mhd.MauSo,
                            KyHieu = tbphct.KyHieu,
                            SoLuong = tbphct.SoLuong,
                            TuSo = tbphct.TuSo,
                            DenSo = tbphct.DenSo,
                            NgayBatDauSuDung = tbphct.NgayBatDauSuDung,
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public List<EnumModel> GetTrangThaiNops()
        {
            List<EnumModel> enums = ((TrangThaiNop[])Enum.GetValues(typeof(TrangThaiNop)))
                .Select(c => new EnumModel()
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            return enums;
        }

        public async Task<ThongBaoPhatHanhViewModel> InsertAsync(ThongBaoPhatHanhViewModel model)
        {
            List<ThongBaoPhatHanhChiTietViewModel> thongBaoPhatHanhChiTiets = model.ThongBaoPhatHanhChiTiets;

            model.ThongBaoPhatHanhChiTiets = null;
            model.ThongBaoPhatHanhId = Guid.NewGuid().ToString();
            ThongBaoPhatHanh entity = _mp.Map<ThongBaoPhatHanh>(model);
            await _db.ThongBaoPhatHanhs.AddAsync(entity);

            foreach (var item in thongBaoPhatHanhChiTiets)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoPhatHanhId = entity.ThongBaoPhatHanhId;
                var detail = _mp.Map<ThongBaoPhatHanhChiTiet>(item);
                await _db.ThongBaoPhatHanhChiTiets.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongBaoPhatHanhViewModel result = _mp.Map<ThongBaoPhatHanhViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoPhatHanhViewModel model)
        {
            List<ThongBaoPhatHanhChiTietViewModel> thongBaoPhatHanhChiTiets = model.ThongBaoPhatHanhChiTiets;

            model.ThongBaoPhatHanhChiTiets = null;
            ThongBaoPhatHanh entity = await _db.ThongBaoPhatHanhs.FirstOrDefaultAsync(x => x.ThongBaoPhatHanhId == model.ThongBaoPhatHanhId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<ThongBaoPhatHanhChiTiet> details = await _db.ThongBaoPhatHanhChiTiets.Where(x => x.ThongBaoPhatHanhId == model.ThongBaoPhatHanhId).ToListAsync();
            _db.ThongBaoPhatHanhChiTiets.RemoveRange(details);
            foreach (var item in thongBaoPhatHanhChiTiets)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoPhatHanhId = entity.ThongBaoPhatHanhId;
                var detail = _mp.Map<ThongBaoPhatHanhChiTiet>(item);
                await _db.ThongBaoPhatHanhChiTiets.AddAsync(detail);
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
