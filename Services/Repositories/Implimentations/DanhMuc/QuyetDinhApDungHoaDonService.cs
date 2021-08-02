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
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class QuyetDinhApDungHoaDonService : IQuyetDinhApDungHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public QuyetDinhApDungHoaDonService(Datacontext datacontext,
            IMapper mapper,
            IHoSoHDDTService hoSoHDDTService,
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hoSoHDDTService = hoSoHDDTService;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckTrungMaAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            bool result = await _db.QuyetDinhApDungHoaDons
               .AnyAsync(x => x.SoQuyetDinh.ToUpper() == model.SoQuyetDinh.ToUpper());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteFileRefTypeById(id, RefType.QuyetDinhApDungHoaDon, _db);

            var entity = await _db.QuyetDinhApDungHoaDons.FirstOrDefaultAsync(x => x.QuyetDinhApDungHoaDonId == id);
            _db.QuyetDinhApDungHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<FileReturn> ExportFileAsync(string id, DinhDangTepMau dinhDangTepMau)
        {
            QuyetDinhApDungHoaDonViewModel model = await GetByIdAsync(id);

            HoSoHDDTViewModel hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            Document doc = new Document();

            string srcPath = Path.Combine(_hostingEnvironment.WebRootPath, $"docs/QuyetDinhApDungHDDT/Quyet_dinh_ap_dung_HDDT.docx");
            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            doc.LoadFromFile(srcPath);
            Section section = doc.Sections[0];

            doc.Replace("<TenDonVi>", hoSoHDDTVM.TenDonVi ?? string.Empty, true, true);
            doc.Replace("<DiaChi>", hoSoHDDTVM.DiaChi ?? string.Empty, true, true);
            doc.Replace("<dd>", model.NgayQuyetDinh.Value.ToString("dd") ?? string.Empty, true, true);
            doc.Replace("<mm>", model.NgayQuyetDinh.Value.ToString("MM") ?? string.Empty, true, true);
            doc.Replace("<yyyy>", model.NgayQuyetDinh.Value.ToString("yyyy") ?? string.Empty, true, true);

            #region Căn cứ để ban hành QĐ
            ListStyle lsCanCuDeBanHanhQD = new ListStyle(doc, ListType.Bulleted);
            lsCanCuDeBanHanhQD.Name = "lsCanCuDeBanHanhQD";
            lsCanCuDeBanHanhQD.Levels[0].BulletCharacter = "\x002D";
            lsCanCuDeBanHanhQD.Levels[0].ParagraphFormat.AfterSpacing = 6;
            lsCanCuDeBanHanhQD.Levels[0].CharacterFormat.FontName = "Times New Roman";
            lsCanCuDeBanHanhQD.Levels[0].ParagraphFormat.LeftIndent = 30F;
            doc.ListStyles.Add(lsCanCuDeBanHanhQD);

            string[] canCuDeBanHanhQDs = model.CanCuDeBanHanhQuyetDinh.Split("\n");
            foreach (string item in canCuDeBanHanhQDs)
            {
                Paragraph par = section.AddParagraph();
                par.AppendText(item);
                par.ListFormat.ApplyStyle(lsCanCuDeBanHanhQD.Name);
            }
            #endregion

            #region Quyết định
            Paragraph paraQD = section.AddParagraph();
            paraQD.Format.HorizontalAlignment = HorizontalAlignment.Center;
            TextRange trQD = paraQD.AppendText("QUYẾT ĐỊNH");
            trQD.CharacterFormat.Bold = true;
            trQD.CharacterFormat.FontSize = 14;
            section.AddParagraph();
            #endregion

            #region Điều 1
            Paragraph paraDieu1 = section.AddParagraph();
            paraDieu1.Format.AfterSpacing = 6;
            TextRange trDieu1 = paraDieu1.AppendText($"Điều 1. {model.Dieu1}");
            trDieu1.CharacterFormat.Bold = true;

            Paragraph paraHeThongThietBi = section.AddParagraph();
            paraHeThongThietBi.AppendText("1. Hệ thống thiết bị:");
            paraHeThongThietBi.Format.BeforeSpacing = 6;
            paraHeThongThietBi.Format.AfterSpacing = 6;

            var list123 = Enum.GetValues(typeof(LoaiDieu1))
               .Cast<LoaiDieu1>()
               .Where(x => x >= LoaiDieu1.MayTinh && x <= LoaiDieu1.ChungThuSo)
               .Select(v => new EnumModel
               {
                   Value = v,
                   Name = v.GetDescription()
               })
               .ToList();

            ListStyle listLoaiDieu123 = new ListStyle(doc, ListType.Bulleted);
            listLoaiDieu123.Name = "listLoaiDieu123";
            listLoaiDieu123.Levels[0].ParagraphFormat.AfterSpacing = 3.2f;
            listLoaiDieu123.Levels[0].ParagraphFormat.BeforeSpacing = 3.2f;
            doc.ListStyles.Add(listLoaiDieu123);

            foreach (var type in list123)
            {
                Paragraph paraBulletTitle = section.AddParagraph();
                paraBulletTitle.Format.BeforeSpacing = 3.2F;
                paraBulletTitle.Format.AfterSpacing = 3.2F;
                TextRange trBulletTitle = paraBulletTitle.AppendText(type.Name);
                trBulletTitle.CharacterFormat.Bold = true;

                if (model.QuyetDinhApDungHoaDonDieu1s.Any(x => x.LoaiDieu1 == (LoaiDieu1)type.Value))
                {
                    var list = model.QuyetDinhApDungHoaDonDieu1s
                        .Where(x => x.LoaiDieu1 == (LoaiDieu1)type.Value)
                        .ToList();

                    foreach (QuyetDinhApDungHoaDonDieu1ViewModel item in list)
                    {
                        Paragraph para = section.AddParagraph();
                        para.AppendText($"{item.Ten} {item.GiaTri}");
                        para.ListFormat.ApplyStyle(listLoaiDieu123.Name);
                    }
                }
            }

            if (model.QuyetDinhApDungHoaDonDieu1s.Any(x => x.LoaiDieu1 == LoaiDieu1.ThietBi))
            {
                var list = model.QuyetDinhApDungHoaDonDieu1s
                    .Where(x => x.LoaiDieu1 == LoaiDieu1.ThietBi)
                    .ToList();

                foreach (QuyetDinhApDungHoaDonDieu1ViewModel item in list)
                {
                    Paragraph paraThietBi = section.AddParagraph();
                    paraThietBi.AppendText(item.Ten + ":");
                    paraThietBi.Format.BeforeSpacing = 3.2F;
                    paraThietBi.Format.AfterSpacing = 3.2F;

                    Paragraph paraThongTinThietBi = section.AddParagraph();
                    paraThongTinThietBi.AppendText(item.GiaTri);
                    paraThongTinThietBi.ListFormat.ApplyStyle(listLoaiDieu123.Name);
                }
            }

            Paragraph paraPhanMemUngDung = section.AddParagraph();
            paraPhanMemUngDung.AppendText("2. Phần mềm ứng dung:");
            paraPhanMemUngDung.Format.BeforeSpacing = 6;
            paraPhanMemUngDung.Format.AfterSpacing = 6;

            ListStyle listPhanMemUngDung = new ListStyle(doc, ListType.Bulleted);
            listPhanMemUngDung.Name = "listPhanMemUngDung";
            listPhanMemUngDung.Levels[0].ParagraphFormat.AfterSpacing = 3.2f;
            listPhanMemUngDung.Levels[0].ParagraphFormat.BeforeSpacing = 3.2f;
            listPhanMemUngDung.Levels[0].NumberPosition = -40;
            doc.ListStyles.Add(listPhanMemUngDung);

            if (model.QuyetDinhApDungHoaDonDieu1s.Any(x => x.LoaiDieu1 == LoaiDieu1.PhanMemUngDung))
            {
                var list = model.QuyetDinhApDungHoaDonDieu1s
                    .Where(x => x.LoaiDieu1 == LoaiDieu1.PhanMemUngDung)
                    .ToList();

                foreach (QuyetDinhApDungHoaDonDieu1ViewModel item in list)
                {
                    Paragraph para = section.AddParagraph();
                    para.AppendText($"{item.Ten} {item.GiaTri}");
                    para.ListFormat.ApplyStyle(listPhanMemUngDung.Name);
                }
            }

            Paragraph para3 = section.AddParagraph();
            para3.AppendText($"3. Bộ phận kỹ thuật hoặc tên nhà cung ứng dịch vụ chịu trách nhiệm về mặt kỹ thuật hoá đơn điện tử, phần mềm ứng dụng: Công ty Cổ phần Thiết bị điện - điện tử Bách Khoa (MST: 0200784873).");
            para3.Format.BeforeSpacing = 6;
            para3.Format.AfterSpacing = 6;
            #endregion

            #region Điều 2
            section.AddParagraph();
            Paragraph paraDieu2 = section.AddParagraph();
            paraDieu2.Format.AfterSpacing = 6;
            TextRange trDieu2 = paraDieu2.AppendText($"Điều 2. {model.Dieu2}");
            trDieu2.CharacterFormat.Bold = true;

            Table table = section.AddTable(true);
            string[] Header = { "STT", "Tên hóa đơn", "Mẫu hóa đơn", "Ký hiệu", "Mục đích sử dụng" };
            //Add Cells
            table.ResetCells(model.QuyetDinhApDungHoaDonDieu2s.Count() + 1, Header.Length);

            //Header Row
            TableRow FRow = table.Rows[0];
            FRow.Height = 23;
            for (int i = 0; i < Header.Length; i++)
            {
                //Cell Alignment
                Paragraph p = FRow.Cells[i].AddParagraph();
                FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                //Data Format
                TextRange TR = p.AppendText(Header[i]);
                TR.CharacterFormat.Bold = true;
            }
            //Data Row
            for (int r = 0; r < model.QuyetDinhApDungHoaDonDieu2s.Count(); r++)
            {
                TableRow DataRow = table.Rows[r + 1];
                QuyetDinhApDungHoaDonDieu2ViewModel item = model.QuyetDinhApDungHoaDonDieu2s[r];

                Paragraph parSTT = DataRow.Cells[0].AddParagraph();
                DataRow.Cells[0].SetCellWidth(10, CellWidthType.Percentage);
                parSTT.AppendText((r + 1).ToString());
                parSTT.Format.HorizontalAlignment = HorizontalAlignment.Center;
                parSTT.Format.AfterSpacing = 2;
                parSTT.Format.BeforeSpacing = 2;

                Paragraph parTenHoaDon = DataRow.Cells[1].AddParagraph();
                DataRow.Cells[1].SetCellWidth(25, CellWidthType.Percentage);
                parTenHoaDon.AppendText(item.TenLoaiHoaDon);
                parTenHoaDon.Format.AfterSpacing = 2;
                parTenHoaDon.Format.BeforeSpacing = 2;

                Paragraph parMauSo = DataRow.Cells[2].AddParagraph();
                DataRow.Cells[2].SetCellWidth(20, CellWidthType.Percentage);
                parMauSo.AppendText(item.MauSo);
                parMauSo.Format.AfterSpacing = 2;
                parMauSo.Format.BeforeSpacing = 2;

                Paragraph parKyHieu = DataRow.Cells[3].AddParagraph();
                DataRow.Cells[3].SetCellWidth(15, CellWidthType.Percentage);
                parKyHieu.AppendText(item.KyHieu);
                parKyHieu.Format.AfterSpacing = 2;
                parKyHieu.Format.BeforeSpacing = 2;

                Paragraph parMucDichSuDung = DataRow.Cells[4].AddParagraph();
                DataRow.Cells[4].SetCellWidth(30, CellWidthType.Percentage);
                parMucDichSuDung.AppendText(item.MucDichSuDung);
                parMucDichSuDung.Format.AfterSpacing = 2;
                parMucDichSuDung.Format.BeforeSpacing = 2;
            }
            #endregion

            #region Điều 3
            section.AddParagraph();
            Paragraph paraDieu3 = section.AddParagraph();
            paraDieu3.Format.AfterSpacing = 6;
            TextRange trDieu3 = paraDieu3.AppendText($"Điều 3. {model.Dieu3}");
            trDieu3.CharacterFormat.Bold = true;

            section.AddParagraph().AppendText(model.NoiDungDieu3);

            //if (!string.IsNullOrEmpty(model.NoiDungDieu3))
            //{
            //    string[] splitDieu3 = model.NoiDungDieu4.Split("\n");
            //    foreach (string item in splitDieu3)
            //    {
            //        string value = item;
            //        if (item[0] == '-')
            //        {
            //            value = item.Substring(1).Trim();
            //        }

            //        Paragraph para = section.AddParagraph();
            //        para.AppendText(value);
            //        para.ListFormat.ApplyStyle(listLoaiDieu123.Name);
            //    }
            //}
            //else
            //{
            //    Paragraph para = section.AddParagraph();
            //}
            #endregion

            #region Điều 4
            section.AddParagraph();
            Paragraph paraDieu4 = section.AddParagraph();
            paraDieu4.Format.AfterSpacing = 6;
            TextRange trDieu4 = paraDieu4.AppendText($"Điều 4. {model.Dieu4}");
            trDieu4.CharacterFormat.Bold = true;

            section.AddParagraph().AppendText(model.NoiDungDieu4);

            //if (!string.IsNullOrEmpty(model.NoiDungDieu4))
            //{
            //    string[] splitDieu4 = model.NoiDungDieu4.Split("\n");
            //    foreach (string item in splitDieu4)
            //    {
            //        string value = item;
            //        if (item[0] == '-')
            //        {
            //            value = item.Substring(1).Trim();
            //        }

            //        Paragraph para = section.AddParagraph();
            //        para.AppendText(value);
            //        para.ListFormat.ApplyStyle(listLoaiDieu123.Name);
            //    }
            //}
            //else
            //{
            //    Paragraph para = section.AddParagraph();
            //}
            #endregion

            #region Điều 5
            section.AddParagraph();
            Paragraph paraDieu5 = section.AddParagraph();
            paraDieu5.Format.AfterSpacing = 6;
            TextRange trDieu5 = paraDieu5.AppendText($"Điều 5. ");
            trDieu5.CharacterFormat.Bold = true;
            paraDieu5.AppendText($"Quyết định này có hiệu lực thi hành kể từ ngày {model.NgayHieuLuc.Value.ToString("dd/MM/yyyy")}");

            section.AddParagraph().AppendText($"Lãnh đạo các bộ phận kế toán, bộ phận bán hàng, bộ phận kỹ thuật và các cá nhân, bộ phận liên quan chịu trách nhiệm triển khai, thực hiện Quyết định này./.");
            #endregion

            #region Footer
            section.AddParagraph();
            Table table2 = section.AddTable(true);
            //Add Cells
            table2.ResetCells(1, 2);
            table2[0, 0].CellFormat.Borders.BorderType = BorderStyle.Cleared;
            table2[0, 1].CellFormat.Borders.BorderType = BorderStyle.Cleared;
            Paragraph parNoiNhan = table2[0, 0].AddParagraph();
            parNoiNhan.Format.LineSpacing = 15f;
            TextRange trNoiNhan = parNoiNhan.AppendText("Nơi nhận:");
            trNoiNhan.CharacterFormat.Bold = true;
            string tenCoQuanThue = _hoSoHDDTService.GetListCoQuanThueQuanLy().FirstOrDefault(x => x.code == model.CoQuanThue).name;

            string[] footer1s = {
                "- " + tenCoQuanThue,
                "- Như Điều 4 (để thực hiện);",
                "- Lãnh đạo đơn vị;",
                "- Lưu." };

            foreach (var item in footer1s)
            {
                Paragraph par = table2[0, 0].AddParagraph();
                par.Format.LineSpacing = 15f;
                TextRange tr = par.AppendText(item);
                tr.CharacterFormat.Italic = true;
            }

            Paragraph parGiamDoc = table2[0, 1].AddParagraph();
            parGiamDoc.Format.HorizontalAlignment = HorizontalAlignment.Center;
            parGiamDoc.Format.LineSpacing = 15f;
            TextRange trGiamDoc = parGiamDoc.AppendText(model.ChucDanh);
            trGiamDoc.CharacterFormat.Bold = true;
            trGiamDoc.CharacterFormat.FontSize = 14;

            Paragraph parKyDongDau = table2[0, 1].AddParagraph();
            parKyDongDau.Format.HorizontalAlignment = HorizontalAlignment.Center;
            parKyDongDau.Format.LineSpacing = 15f;
            TextRange trKyDongDau = parKyDongDau.AppendText("(Ký, đóng dấu và ghi rõ họ, tên)");
            trKyDongDau.CharacterFormat.Italic = true;

            table2[0, 1].AddParagraph();
            table2[0, 1].AddParagraph();
            table2[0, 1].AddParagraph();

            Paragraph parTenGiamDoc = table2[0, 1].AddParagraph();
            parTenGiamDoc.Format.HorizontalAlignment = HorizontalAlignment.Center;
            TextRange trTenGiamDoc = parTenGiamDoc.AppendText(model.NguoiDaiDienPhapLuat);
            trTenGiamDoc.CharacterFormat.Bold = true;
            trTenGiamDoc.CharacterFormat.FontSize = 14;
            #endregion

            FileFormat fileFormat;
            string fileName;

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

            string filePath = Path.Combine(destPath, fileName);
            doc.SaveToFile(filePath, fileFormat);

            byte[] fileByte = File.ReadAllBytes(filePath);
            File.Delete(filePath);

            return new FileReturn
            {
                Bytes = fileByte,
                ContentType = MimeTypes.GetMimeType(filePath),
                FileName = Path.GetFileName(filePath)
            };
        }

        public async Task<PagedList<QuyetDinhApDungHoaDonViewModel>> GetAllPagingAsync(QuyetDinhApDungHoaDonParams @params)
        {
            var query = from qd in _db.QuyetDinhApDungHoaDons
                        join u in _db.Users on qd.CreatedBy equals u.UserId
                        orderby qd.NgayQuyetDinh descending
                        select new QuyetDinhApDungHoaDonViewModel
                        {
                            QuyetDinhApDungHoaDonId = qd.QuyetDinhApDungHoaDonId,
                            NgayQuyetDinh = qd.NgayQuyetDinh,
                            SoQuyetDinh = qd.SoQuyetDinh,
                            NgayHieuLuc = qd.NgayHieuLuc,
                            NguoiTao = u.UserName
                        };

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.NgayQuyetDinhFilter))
                {
                    var keyword = @params.Filter.NgayQuyetDinhFilter.ToUpper().ToTrim();
                    query = query.Where(x => x.NgayQuyetDinhFilter.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.SoQuyetDinh))
                {
                    var keyword = @params.Filter.SoQuyetDinh.ToUpper().ToTrim();
                    query = query.Where(x => x.SoQuyetDinh.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.NgayHieuLucFilter))
                {
                    var keyword = @params.Filter.NgayHieuLucFilter.ToUpper().ToTrim();
                    query = query.Where(x => x.NgayHieuLucFilter.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.NguoiTao))
                {
                    var keyword = @params.Filter.NguoiTao.ToUpper().ToTrim();
                    query = query.Where(x => x.NguoiTao.ToUpper().ToTrim().Contains(keyword));
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.NgayQuyetDinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayQuyetDinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayQuyetDinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.SoQuyetDinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoQuyetDinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoQuyetDinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.NgayHieuLuc))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NgayHieuLuc);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NgayHieuLuc);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.NguoiTao))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.NguoiTao);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.NguoiTao);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<QuyetDinhApDungHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<QuyetDinhApDungHoaDonViewModel> GetByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyetDinhApDungHoaDon);
            string folder = $@"\FilesUpload\{databaseName}\{loaiNghiepVu}\{id}\FileAttach";

            var query = from qd in _db.QuyetDinhApDungHoaDons
                        where qd.QuyetDinhApDungHoaDonId == id
                        select new QuyetDinhApDungHoaDonViewModel
                        {
                            QuyetDinhApDungHoaDonId = qd.QuyetDinhApDungHoaDonId,
                            NguoiDaiDienPhapLuat = qd.NguoiDaiDienPhapLuat ?? string.Empty,
                            ChucDanh = qd.ChucDanh,
                            SoQuyetDinh = qd.SoQuyetDinh,
                            NgayQuyetDinh = qd.NgayQuyetDinh,
                            CanCuDeBanHanhQuyetDinh = qd.CanCuDeBanHanhQuyetDinh,
                            HasMayTinh = qd.HasMayTinh,
                            HasMayIn = qd.HasMayIn,
                            HasChungThuSo = qd.HasChungThuSo,
                            Dieu1 = qd.Dieu1,
                            Dieu2 = qd.Dieu2,
                            Dieu3 = qd.Dieu3,
                            NoiDungDieu3 = qd.NoiDungDieu3,
                            Dieu4 = qd.Dieu4,
                            NoiDungDieu4 = qd.NoiDungDieu4,
                            Dieu5 = qd.Dieu5,
                            NgayHieuLuc = qd.NgayHieuLuc,
                            CoQuanThue = qd.CoQuanThue,
                            CreatedBy = qd.CreatedBy,
                            CreatedDate = qd.CreatedDate,
                            Status = qd.Status,
                            QuyetDinhApDungHoaDonDieu1s = (from qdd1 in _db.QuyetDinhApDungHoaDonDieu1s
                                                           where qdd1.QuyetDinhApDungHoaDonId == qd.QuyetDinhApDungHoaDonId
                                                           orderby qdd1.CreatedDate
                                                           select new QuyetDinhApDungHoaDonDieu1ViewModel
                                                           {
                                                               QuyetDinhApDungHoaDonDieu1Id = qdd1.QuyetDinhApDungHoaDonDieu1Id,
                                                               QuyetDinhApDungHoaDonId = qdd1.QuyetDinhApDungHoaDonId,
                                                               Ten = qdd1.Ten,
                                                               GiaTri = qdd1.GiaTri,
                                                               Checked = qdd1.Checked,
                                                               LoaiDieu1 = qdd1.LoaiDieu1,
                                                               Disabled = qdd1.Disabled,
                                                           })
                                                           .ToList(),
                            QuyetDinhApDungHoaDonDieu2s = (from qdd2 in _db.QuyetDinhApDungHoaDonDieu2s
                                                           join mhd in _db.MauHoaDons on qdd2.MauHoaDonId equals mhd.MauHoaDonId
                                                           where qdd2.QuyetDinhApDungHoaDonId == qd.QuyetDinhApDungHoaDonId
                                                           orderby qdd2.CreatedDate
                                                           select new QuyetDinhApDungHoaDonDieu2ViewModel
                                                           {
                                                               QuyetDinhApDungHoaDonDieu2Id = qdd2.QuyetDinhApDungHoaDonDieu2Id,
                                                               QuyetDinhApDungHoaDonId = qdd2.QuyetDinhApDungHoaDonId,
                                                               MauHoaDonId = qdd2.MauHoaDonId,
                                                               TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                                                               MauSo = mhd.MauSo,
                                                               KyHieu = mhd.KyHieu,
                                                               MucDichSuDung = qdd2.MucDichSuDung,
                                                           })
                                                           .ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == qd.QuyetDinhApDungHoaDonId
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

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<QuyetDinhApDungHoaDonDieu2ViewModel>> GetListMauHoaDonByIdAsync(string id)
        {
            var query = from d2 in _db.QuyetDinhApDungHoaDonDieu2s
                        join mhd in _db.MauHoaDons on d2.MauHoaDonId equals mhd.MauHoaDonId
                        where d2.QuyetDinhApDungHoaDonId == id
                        orderby d2.CreatedDate
                        select new QuyetDinhApDungHoaDonDieu2ViewModel
                        {
                            QuyetDinhApDungHoaDonDieu2Id = d2.QuyetDinhApDungHoaDonDieu2Id,
                            QuyetDinhApDungHoaDonId = d2.QuyetDinhApDungHoaDonId,
                            MauHoaDonId = d2.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            MucDichSuDung = mhd.KyHieu
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<List<QuyetDinhApDungHoaDonDieu2ViewModel>> GetMauCacLoaiHoaDonAsync(string id)
        {
            var query = from mhd in _db.MauHoaDons
                        join d2 in _db.QuyetDinhApDungHoaDonDieu2s on mhd.MauHoaDonId equals d2.MauHoaDonId into tmpDieu2
                        from d2 in tmpDieu2.DefaultIfEmpty()
                        where (string.IsNullOrEmpty(id) && d2 == null) || (!string.IsNullOrEmpty(id) && (d2 == null || d2.QuyetDinhApDungHoaDonId == id))
                        select new QuyetDinhApDungHoaDonDieu2ViewModel
                        {
                            QuyetDinhApDungHoaDonDieu2Id = d2 != null ? d2.QuyetDinhApDungHoaDonDieu2Id : null,
                            QuyetDinhApDungHoaDonId = d2 != null ? d2.QuyetDinhApDungHoaDonId : null,
                            TenLoaiHoaDon = mhd.LoaiHoaDon.GetDescription(),
                            MauHoaDonId = mhd.MauHoaDonId,
                            MauSo = mhd.MauSo,
                            KyHieu = mhd.KyHieu,
                            MucDichSuDung = d2 != null ? d2.MucDichSuDung : "Bán hàng",
                            Status = true,
                            Checked = d2 != null
                        };

            var result = await query.OrderBy(x => x.MauSo).ToListAsync();
            return result;
        }

        public async Task<QuyetDinhApDungHoaDonViewModel> InsertAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            List<QuyetDinhApDungHoaDonDieu1ViewModel> quyetDinhApDungHoaDonDieu1s = model.QuyetDinhApDungHoaDonDieu1s;
            List<QuyetDinhApDungHoaDonDieu2ViewModel> quyetDinhApDungHoaDonDieu2s = model.QuyetDinhApDungHoaDonDieu2s;

            model.QuyetDinhApDungHoaDonDieu1s = null;
            model.QuyetDinhApDungHoaDonDieu2s = null;
            var entity = _mp.Map<QuyetDinhApDungHoaDon>(model);
            await _db.QuyetDinhApDungHoaDons.AddAsync(entity);

            foreach (var item in quyetDinhApDungHoaDonDieu1s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu1>(item);
                await _db.QuyetDinhApDungHoaDonDieu1s.AddAsync(detail);
            }

            foreach (var item in quyetDinhApDungHoaDonDieu2s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu2>(item);
                await _db.QuyetDinhApDungHoaDonDieu2s.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            var result = _mp.Map<QuyetDinhApDungHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(QuyetDinhApDungHoaDonViewModel model)
        {
            List<QuyetDinhApDungHoaDonDieu1ViewModel> quyetDinhApDungHoaDonDieu1s = model.QuyetDinhApDungHoaDonDieu1s;
            List<QuyetDinhApDungHoaDonDieu2ViewModel> quyetDinhApDungHoaDonDieu2s = model.QuyetDinhApDungHoaDonDieu2s;

            model.QuyetDinhApDungHoaDonDieu1s = null;
            model.QuyetDinhApDungHoaDonDieu2s = null;
            var entity = await _db.QuyetDinhApDungHoaDons.FirstOrDefaultAsync(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<QuyetDinhApDungHoaDonDieu1> removedDieu1s = await _db.QuyetDinhApDungHoaDonDieu1s.Where(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId).ToListAsync();
            List<QuyetDinhApDungHoaDonDieu2> removedDieu2s = await _db.QuyetDinhApDungHoaDonDieu2s.Where(x => x.QuyetDinhApDungHoaDonId == model.QuyetDinhApDungHoaDonId).ToListAsync();
            _db.QuyetDinhApDungHoaDonDieu1s.RemoveRange(removedDieu1s);
            _db.QuyetDinhApDungHoaDonDieu2s.RemoveRange(removedDieu2s);

            foreach (var item in quyetDinhApDungHoaDonDieu1s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu1>(item);
                await _db.QuyetDinhApDungHoaDonDieu1s.AddAsync(detail);
            }

            foreach (var item in quyetDinhApDungHoaDonDieu2s)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.QuyetDinhApDungHoaDonId = entity.QuyetDinhApDungHoaDonId;
                var detail = _mp.Map<QuyetDinhApDungHoaDonDieu2>(item);
                await _db.QuyetDinhApDungHoaDonDieu2s.AddAsync(detail);
            }

            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
