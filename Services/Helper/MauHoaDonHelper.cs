using DLL.Constants;
using DLL.Enums;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using Services.Helper.Constants;
using Services.ViewModels.DanhMuc;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Pdf;
using Spire.Pdf.AutomaticFields;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Widget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Services.Helper
{
    public static class MauHoaDonHelper
    {
        /// <summary>
        /// Tạo mẫu hóa đơn doc
        /// </summary>
        public static Document TaoMauHoaDonDoc(MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai, IHostingEnvironment env, IHttpContextAccessor accessor, out int beginRow, bool hasReason = false)
        {
            string webRootPath = env.WebRootPath;
            string docPath = Path.Combine(webRootPath, $"docs/MauHoaDon/01.CB.01.docx");
            string qrcode = Path.Combine(webRootPath, $"images/template/qrcode.png");
            string tempFolder = Path.Combine(webRootPath, $"temp");
            string databaseName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string backgroundEmtpy = Path.Combine(webRootPath, $"images/background/empty.jpg");
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.MauHoaDon);

            mauHoaDon.WebRootPath = webRootPath;
            mauHoaDon.DatabaseName = databaseName;
            mauHoaDon.LoaiNghiepVu = loaiNghiepVu;

            #region Kiểu chữ
            var kieuChu = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.KieuChu).GiaTri;
            #endregion

            #region Cỡ chữ
            var coChu = int.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.CoChu).GiaTri);
            #endregion

            #region Màu chữ
            var mauChu = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.MauChu).GiaTri;
            #endregion

            #region Hiện thị QRcode
            var isHienThiQRCode = bool.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.HienThiQRCode).GiaTri);
            #endregion

            #region Lặp lại thông tin khi hóa đơn có nhiều trang
            var isLapLaiThongTinHD = bool.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.LapLaiThongTinKhiHoaDonCoNhieuTrang).GiaTri);
            #endregion

            #region Thiết lập dòng ký hiệu cột
            var isThietLapDongKyHieuCot = bool.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.ThietLapDongKyHieuCot).GiaTri);
            #endregion

            #region Số dòng trắng
            var soDongTrang = int.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.SoDongTrang).GiaTri);
            #endregion

            #region Hình nền mặc định
            var hinhNenMacDinh = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.HinhNenMacDinh);
            string colorBgDefault = string.Empty;
            string bgDefaultPath = string.Empty;
            float opacityBgDefault = 0;
            bool isSVGBgDefalt = false;
            if (!string.IsNullOrEmpty(hinhNenMacDinh.GiaTri))
            {
                var giaTriBoSungBgDefaults = hinhNenMacDinh.GiaTriBoSung.Split(";");
                bgDefaultPath = webRootPath + giaTriBoSungBgDefaults[0];
                string ext = Path.GetExtension(bgDefaultPath);
                isSVGBgDefalt = ext == ".svg" || ext == ".SVG";
                if (giaTriBoSungBgDefaults.Count() > 1 && !string.IsNullOrEmpty(giaTriBoSungBgDefaults[1]))
                {
                    colorBgDefault = giaTriBoSungBgDefaults[1];
                }
                if (giaTriBoSungBgDefaults.Count() > 1 && !string.IsNullOrEmpty(giaTriBoSungBgDefaults[2]))
                {
                    opacityBgDefault = float.Parse(giaTriBoSungBgDefaults[2], CultureInfo.InvariantCulture.NumberFormat);
                }
            }
            #endregion

            #region Hình nền tải lên
            var bgUpload = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.HinhNenTaiLen);
            string bgUploadPath = Path.Combine(webRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.FILE_ATTACH}/{bgUpload.GiaTri}");
            float topBgUpload = 0;
            float leftBgUpload = 0;
            float widthBgUpload = 0;
            float heightBgUpload = 0;
            float opacityBgUpload = 0;
            int typeBgUpload = 1;
            ChuChimModel chuChimModel = null;
            if (!string.IsNullOrEmpty(bgUpload.GiaTri))
            {
                var giaTriBoSungBgUploads = bgUpload.GiaTriBoSung.Split(";");
                topBgUpload = float.Parse(giaTriBoSungBgUploads[0], CultureInfo.InvariantCulture.NumberFormat);
                leftBgUpload = float.Parse(giaTriBoSungBgUploads[1], CultureInfo.InvariantCulture.NumberFormat);
                widthBgUpload = float.Parse(giaTriBoSungBgUploads[2], CultureInfo.InvariantCulture.NumberFormat);
                heightBgUpload = float.Parse(giaTriBoSungBgUploads[3], CultureInfo.InvariantCulture.NumberFormat);
                opacityBgUpload = float.Parse(giaTriBoSungBgUploads[4], CultureInfo.InvariantCulture.NumberFormat);

                if (giaTriBoSungBgUploads.Count() > 5 && !string.IsNullOrEmpty(giaTriBoSungBgUploads[5]))
                {
                    typeBgUpload = int.Parse(giaTriBoSungBgUploads[5]);
                }
                if (giaTriBoSungBgUploads.Count() > 7 && !string.IsNullOrEmpty(giaTriBoSungBgUploads[7]))
                {
                    chuChimModel = JsonConvert.DeserializeObject<ChuChimModel>(Uri.UnescapeDataString(DataHelper.Base64Decode(giaTriBoSungBgUploads[7])));
                }
            }
            #endregion

            #region Khung viền mặc định
            var bdDefault = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.KhungVienMacDinh);
            string colorBdDefault = string.Empty;
            string bdDefaultPath = string.Empty;
            bool isSVGBdDefalt = false;
            if (bdDefault != null && !string.IsNullOrEmpty(bdDefault.GiaTri))
            {
                var giaTriBoSungBdDefaults = bdDefault.GiaTriBoSung.Split(";");
                bdDefaultPath = webRootPath + giaTriBoSungBdDefaults[0];
                string ext = Path.GetExtension(bdDefaultPath);
                isSVGBdDefalt = ext == ".svg" || ext == ".SVG";
                if (giaTriBoSungBdDefaults.Count() > 1 && !string.IsNullOrEmpty(giaTriBoSungBdDefaults[1]))
                {
                    colorBdDefault = giaTriBoSungBdDefaults[1];
                }
            }
            #endregion

            Document doc = new Document();
            doc.LoadFromFile(docPath, Spire.Doc.FileFormat.Docx);
            doc.CreateTuyChinhChiTiet(mauHoaDon, loai);
            Section section = doc.Sections[0];

            Table tbl_nguoi_mua_first_page = null;
            Table tbl_nguoi_ban_first_page = null;
            Table tbl_nguoi_mua = null;
            Table tbl_nguoi_ban = null;
            Table tbl_hhdv = null;

            #region Header
            foreach (Table tb in section.HeadersFooters.FirstPageHeader.Tables)
            {
                if (tb.Title == "tbl_nguoi_mua")
                {
                    tbl_nguoi_mua_first_page = tb;
                }
                if (tb.Title == "tbl_nguoi_ban")
                {
                    tbl_nguoi_ban_first_page = tb;
                }
            }
            if (tbl_nguoi_mua_first_page != null && isHienThiQRCode)
            {
                int lastCell = tbl_nguoi_mua_first_page.Rows[0].Cells.Count - 1;
                TableCell lastTableCell = tbl_nguoi_mua_first_page.Rows[0].Cells[lastCell];
                lastTableCell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                Paragraph par = lastTableCell.Paragraphs.Count > 0 ? lastTableCell.Paragraphs[0] : lastTableCell.AddParagraph();
                DocPicture picQRCode = par.AppendPicture(Image.FromFile(qrcode));
                picQRCode.Width = 46;
                picQRCode.Height = 46;
                par.Format.HorizontalAlignment = HorizontalAlignment.Center;
            }
            if (!isLapLaiThongTinHD)
            {
                section.PageSetup.DifferentFirstPageHeaderFooter = false;
                int count = section.HeadersFooters.Header.Tables.Count;
                for (int i = 0; i < count; i++)
                {
                    section.HeadersFooters.Header.Tables.RemoveAt(0);
                }
                section.PageSetup.DifferentFirstPageHeaderFooter = true;
            }
            else
            {
                section.PageSetup.DifferentFirstPageHeaderFooter = false;

                foreach (Table tb in section.HeadersFooters.Header.Tables)
                {
                    if (tb.Title == "tbl_nguoi_mua")
                    {
                        tbl_nguoi_mua = tb;
                    }
                    if (tb.Title == "tbl_nguoi_ban")
                    {
                        tbl_nguoi_ban = tb;
                    }
                }
                if (tbl_nguoi_mua != null && isHienThiQRCode)
                {
                    int lastCell = tbl_nguoi_mua.Rows[0].Cells.Count - 1;
                    TableCell lastTableCell = tbl_nguoi_mua.Rows[0].Cells[lastCell];
                    lastTableCell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    Paragraph par = lastTableCell.Paragraphs.Count > 0 ? lastTableCell.Paragraphs[0] : lastTableCell.AddParagraph();
                    DocPicture picQRCode = par.AppendPicture(Image.FromFile(qrcode));
                    picQRCode.Width = 46;
                    picQRCode.Height = 46;
                    par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                }
                section.PageSetup.DifferentFirstPageHeaderFooter = true;
            }
            #endregion

            #region thay thế
            if (hasReason == true)
            {
                Paragraph replacePar = section.AddParagraph();
                TextRange trExchange = replacePar.AppendText("<reason>");
                trExchange.CharacterFormat.FontSize = 10 + coChu;
                trExchange.CharacterFormat.FontName = kieuChu;
                trExchange.CharacterFormat.TextColor = ColorTranslator.FromHtml(mauChu);
                section.Paragraphs.Insert(0, section.Paragraphs[section.Paragraphs.Count - 1]);
            }
            #endregion

            #region hhdv
            beginRow = 1;
            foreach (Table tb in section.Tables)
            {
                if (tb.Title == "tbl_hhdv")
                {
                    tbl_hhdv = tb;
                    break;
                }
            }
            if (isThietLapDongKyHieuCot)
            {
                beginRow = 2;
            }
            #endregion

            #region style
            var tables = section.Tables;
            foreach (Table table in tables)
            {
                foreach (TableRow row in table.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Paragraph par in cell.Paragraphs)
                        {
                            var style = par.GetStyle();
                            style.CharacterFormat.FontName = kieuChu;
                        }
                    }
                }
            }
            #endregion

            #region background
            Image bgDefault = Image.FromFile(backgroundEmtpy);
            Graphics g = Graphics.FromImage(bgDefault);
            if (!string.IsNullOrEmpty(bdDefaultPath) && File.Exists(bdDefaultPath))
            {
                Image borderDefault = null;
                if (isSVGBdDefalt == true)
                {
                    borderDefault = ConvertSvgToImage(bdDefaultPath, tempFolder, colorBdDefault, 860, 1215);
                }
                else
                {
                    borderDefault = Image.FromFile(bdDefaultPath);
                }

                g.DrawImage(borderDefault, 0, 5);
            }
            if (!string.IsNullOrEmpty(bgUploadPath) && File.Exists(bgUploadPath))
            {
                Image backgroundUpload = Image.FromFile(bgUploadPath);
                backgroundUpload = backgroundUpload.ResizeImage(widthBgUpload, heightBgUpload);

                g.DrawImage(backgroundUpload.SetImageOpacity(opacityBgUpload), leftBgUpload, topBgUpload);
            }
            if (!string.IsNullOrEmpty(bgDefaultPath) && File.Exists(bgDefaultPath))
            {
                Image backgroundDefault = null;
                if (isSVGBgDefalt == true)
                {
                    backgroundDefault = ConvertSvgToImage(bgDefaultPath, tempFolder, colorBgDefault, 500, 500);
                }
                else
                {
                    backgroundDefault = Image.FromFile(bgDefaultPath);
                }

                int x = (bgDefault.Width / 2) - (backgroundDefault.Width / 2);
                int y = (bgDefault.Height / 2) - (backgroundDefault.Height / 2);

                g.DrawImage(backgroundDefault.SetImageOpacity(opacityBgDefault), x, y);
            }
            doc.Background.Type = BackgroundType.Picture;
            doc.Background.Picture = bgDefault;
            #endregion

            #region test filldata
            if (tbl_hhdv != null)
            {
                //soDongTrang = 40;
                // Check to insert to row detail order
                if (soDongTrang > 4)
                {
                    int _cnt_rows = soDongTrang - 4;

                    for (int i = 0; i < _cnt_rows; i++)
                    {
                        if (tbl_hhdv.Rows.Count >= 4)
                        {
                            // Clone row
                            TableRow cl_row = tbl_hhdv.Rows[4].Clone();
                            // Add row
                            tbl_hhdv.Rows.Insert(4, cl_row);
                        }
                    }
                }
            }
            #endregion

            return doc;
        }

        public static Image ConvertSvgToImage(string svgPath, string tempFolderPath, string color, int width, int height)
        {
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            var tempPngFullPath = Path.Combine(tempFolderPath, $"{Guid.NewGuid()}.png");

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(svgPath);
            xmlDocument.DocumentElement.SetAttribute("style", $"fill: {color};");
            MemoryStream xmlStream = new MemoryStream();
            xmlDocument.Save(xmlStream);
            xmlStream.Position = 0;

            // Write to stream
            var settings = new MagickReadSettings();
            settings.Width = width;
            settings.Height = height;

            // Read first frame of gif image
            using (var image = new MagickImage(xmlStream, settings))
            {
                // Save frame as jpg
                image.Write(tempPngFullPath, MagickFormat.Png);
                byte[] bytes = File.ReadAllBytes(tempPngFullPath);
                if (File.Exists(tempPngFullPath))
                {
                    File.Delete(tempPngFullPath);
                }

                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
        }

        public static Image ResizeImage(this Image image, float width, float height)
        {
            return new Bitmap(image, new Size((int)width, (int)height));
        }

        public static Image SetImageOpacity(this Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix
                {
                    Matrix33 = opacity
                };
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default,
                                                  ColorAdjustType.Bitmap);
                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                                   0, 0, image.Width, image.Height,
                                   GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }

        public static void CreateTuyChinhChiTiet(this Document doc, MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai)
        {
            if (mauHoaDon.MauHoaDonTuyChinhChiTiets != null && mauHoaDon.MauHoaDonTuyChinhChiTiets.Count > 0)
            {
                Section section = doc.Sections[0];

                Table tbl_nguoi_mua_first_page = null;
                Table tbl_nguoi_ban_first_page = null;
                Table tbl_tieu_de_first_page = null;
                Table tbl_nguoi_mua = null;
                Table tbl_nguoi_ban = null;
                Table tbl_tieu_de = null;
                Table tbl_hhdv = null;
                Table tbl_nguoi_ky = null;
                Table tbl_footer_first_page = null;
                Table tbl_footer = null;

                List<MauHoaDonTuyChinhChiTietViewModel> thongTinNguoiBans = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiBan && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinHoaDon_mauSoKyHieuSoHoaDon = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => (x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon || x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon || x.Loai == LoaiTuyChinhChiTiet.ThongTinMaCuaCoQuanThue) && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinNguoiMuas = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinHangHoaDichVus = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => (x.Loai == LoaiTuyChinhChiTiet.ThongTinVeHangHoaDichVu || x.Loai == LoaiTuyChinhChiTiet.ThongTinVeTongGiaTriHHDV || x.Loai == LoaiTuyChinhChiTiet.ThongTinNgoaiTe) && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinNguoiKys = mauHoaDon.MauHoaDonTuyChinhChiTiets
                       .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiKy && x.Checked == true)
                       .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinTraCuus = mauHoaDon.MauHoaDonTuyChinhChiTiets
                       .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinTraCuu && x.Checked == true)
                       .ToList();

                foreach (Table tb in section.HeadersFooters.FirstPageHeader.Tables)
                {
                    if (tb.Title == "tbl_nguoi_mua")
                    {
                        tbl_nguoi_mua_first_page = tb;
                    }
                    if (tb.Title == "tbl_nguoi_ban")
                    {
                        tbl_nguoi_ban_first_page = tb;
                    }
                    if (tb.Title == "tbl_tieu_de")
                    {
                        tbl_tieu_de_first_page = tb;
                    }
                }
                doc.StyleDataTable(tbl_nguoi_ban_first_page, thongTinNguoiBans, TableType.ThongTinNguoiBan, mauHoaDon, loai);
                doc.StyleDataTable(tbl_tieu_de_first_page, thongTinHoaDon_mauSoKyHieuSoHoaDon, TableType.ThongTinHoaDon, mauHoaDon, loai);
                doc.StyleDataTable(tbl_nguoi_mua_first_page, thongTinNguoiMuas, TableType.ThongTinNguoiMua, mauHoaDon, loai);

                section.PageSetup.DifferentFirstPageHeaderFooter = false;
                foreach (Table tb in section.HeadersFooters.Header.Tables)
                {
                    if (tb.Title == "tbl_nguoi_mua")
                    {
                        tbl_nguoi_mua = tb;
                    }
                    if (tb.Title == "tbl_nguoi_ban")
                    {
                        tbl_nguoi_ban = tb;
                    }
                    if (tb.Title == "tbl_tieu_de")
                    {
                        tbl_tieu_de = tb;
                    }
                }
                doc.StyleDataTable(tbl_nguoi_ban, thongTinNguoiBans, TableType.ThongTinNguoiBan, mauHoaDon, loai);
                doc.StyleDataTable(tbl_tieu_de, thongTinHoaDon_mauSoKyHieuSoHoaDon, TableType.ThongTinHoaDon, mauHoaDon, loai);
                doc.StyleDataTable(tbl_nguoi_mua, thongTinNguoiMuas, TableType.ThongTinNguoiMua, mauHoaDon, loai);
                section.PageSetup.DifferentFirstPageHeaderFooter = true;

                foreach (Table tb in section.Tables)
                {
                    if (tb.Title == "tbl_hhdv")
                    {
                        tbl_hhdv = tb;
                    }
                    if (tb.Title == "tbl_nguoi_ky")
                    {
                        tbl_nguoi_ky = tb;
                    }
                }
                doc.StyleDataTable(tbl_hhdv, thongTinHangHoaDichVus, TableType.ThongTinHangHoaDichVu, mauHoaDon, loai);
                doc.StyleDataTable(tbl_nguoi_ky, thongTinNguoiKys, TableType.ThongTinNguoiKy, mauHoaDon, loai);

                foreach (Table tb in section.HeadersFooters.FirstPageFooter.Tables)
                {
                    if (tb.Title == "tbl_footer")
                    {
                        tbl_footer_first_page = tb;
                    }
                }
                doc.StyleDataTable(tbl_footer_first_page, thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);

                section.PageSetup.DifferentFirstPageHeaderFooter = false;
                foreach (Table tb in section.HeadersFooters.Footer.Tables)
                {
                    if (tb.Title == "tbl_footer")
                    {
                        tbl_footer = tb;
                    }
                }
                doc.StyleDataTable(tbl_footer, thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);
                section.PageSetup.DifferentFirstPageHeaderFooter = true;
            }
        }

        private static void StyleDataTable(this Document doc, Table table, List<MauHoaDonTuyChinhChiTietViewModel> list, TableType tableType, MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai)
        {
            List<MauHoaDonTuyChinhChiTietViewModel> cloneList = CloneHelper.DeepClone(list);
            if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.CacLoaiHoaDonKhac)
            {
                mauHoaDon.LoaiHoaDon = LoaiHoaDon.HoaDonGTGT;
            }

            if (table != null)
            {
                if (tableType == TableType.ThongTinNguoiBan)
                {
                    var listThongTinNguoBan = new List<MauHoaDonTuyChinhChiTietViewModel>();

                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.PXKKiemVanChuyenNoiBo && mauHoaDon.TenBoMau == "01.CB.02")
                    {
                        var listTenDonViVaMaSoThue = cloneList.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan).ToList();
                        var listCanCuNgay = cloneList.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CanCuSo || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayCanCu).ToList();
                        var listCuaVeViec = cloneList.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.Cua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.VeViecDienGiai).ToList();
                        var listXuatKho = cloneList.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoXuatHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HoTenNguoiXuatHang).ToList();
                        var listVanChuyen = cloneList.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenNguoiVanChuyen || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HopDongVanChuyenSo).ToList();
                        var listOther = cloneList.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.CanCuSo &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.NgayCanCu &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.Cua &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DiaChiKhoXuatHang &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.HoTenNguoiXuatHang &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TenNguoiVanChuyen &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.HopDongVanChuyenSo &&
                                                            x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.VeViecDienGiai).ToList();

                        foreach (var item in listTenDonViVaMaSoThue)
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                            });
                        }

                        if (listCanCuNgay.Any())
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = listCanCuNgay
                            });
                        }

                        if (listCuaVeViec.Any())
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = listCuaVeViec
                            });
                        }

                        foreach (var item in listXuatKho)
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                            });
                        }

                        if (listVanChuyen.Any())
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = listVanChuyen
                            });
                        }

                        foreach (var item in listOther)
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                            });
                        }
                    }
                    else
                    {
                        foreach (var item in cloneList)
                        {
                            listThongTinNguoBan.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                            });
                        }
                    }

                    int canTieuDe = cloneList.SelectMany(x => x.Children).FirstOrDefault().TuyChonChiTiet.CanTieuDe.Value;
                    int doRong = cloneList.SelectMany(x => x.Children).Max(x => x.TuyChonChiTiet.DoRong ?? 0);
                    int row = listThongTinNguoBan.Count();
                    int maxCol = listThongTinNguoBan.Max(x => x.Children.Count);
                    int col = maxCol;
                    int startColWithoutLogo = 0;
                    int endColWithoutLogo = 0;

                    if (canTieuDe > 1)
                    {
                        col += 1;
                    }

                    #region Logo
                    var logo = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.Logo);
                    string logoPath = string.Empty;
                    float topLogo = 0;
                    float leftLogo = 0;
                    float widthLogo = 0;
                    float heightLogo = 0;
                    float positionLogo = 0;
                    bool hasLogo = false;

                    if (logo != null)
                    {
                        logoPath = Path.Combine(mauHoaDon.WebRootPath, $"FilesUpload/{mauHoaDon.DatabaseName}/{ManageFolderPath.FILE_ATTACH}/{logo.GiaTri}");
                        var giaTriBoSungLogos = logo.GiaTriBoSung.Split(";");
                        topLogo = float.Parse(giaTriBoSungLogos[0], CultureInfo.InvariantCulture.NumberFormat);
                        topLogo = float.Parse(giaTriBoSungLogos[0], CultureInfo.InvariantCulture.NumberFormat);
                        leftLogo = float.Parse(giaTriBoSungLogos[1], CultureInfo.InvariantCulture.NumberFormat);
                        widthLogo = float.Parse(giaTriBoSungLogos[2], CultureInfo.InvariantCulture.NumberFormat);
                        heightLogo = float.Parse(giaTriBoSungLogos[3], CultureInfo.InvariantCulture.NumberFormat);
                        positionLogo = int.Parse(giaTriBoSungLogos[4]);
                        if (!string.IsNullOrEmpty(logo.GiaTri) && File.Exists(logoPath))
                        {
                            //AddColumn(table, positionLogo == 1 ? 0 : (col + 1));
                            col += 1;
                            hasLogo = true;
                        }

                        widthLogo = widthLogo * 60 / 100;
                        heightLogo = heightLogo * 60 / 100;
                    }
                    startColWithoutLogo = hasLogo ? (positionLogo == 1 ? 1 : 0) : 0;
                    endColWithoutLogo = hasLogo ? (positionLogo == 1 ? (col - 1) : (col - 2)) : (col - 1);
                    #endregion

                    table.ResetCells(row, col);

                    ////////////////////
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        float widthOfTieuDe = doRong * (maxCol == 2 ? 50 : 40) / 100;

                        if (hasLogo)
                        {
                            table.Rows[i].Cells[positionLogo == 1 ? 0 : (col - 1)].Width = widthLogo * (maxCol == 2 ? 90 : 60) / 100;
                        }
                        else
                        {
                            widthOfTieuDe += 10;
                        }

                        if (canTieuDe > 1)
                        {
                            table.Rows[i].Cells[startColWithoutLogo].Width = widthOfTieuDe;
                            if (maxCol == 2)
                            {
                                // table.Rows[i].Cells[endColWithoutLogo].Width = (table.Width - (hasLogo ? (widthLogo * 60 / 100) : 0)) / 2;
                            }
                        }
                    }

                    if (hasLogo)
                    {
                        Paragraph paraLogo = null;
                        if (positionLogo == 1)
                        {
                            table.ApplyVerticalMerge(0, 0, row - 1);
                            paraLogo = table[0, 0].AddParagraph();
                        }
                        else
                        {
                            table.ApplyVerticalMerge(col - 1, 0, row - 1);
                            paraLogo = table[0, col - 1].AddParagraph();
                        }

                        Image logoImage = Image.FromFile(logoPath);
                        DocPicture picLogo = paraLogo.AppendPicture(logoImage);
                        picLogo.VerticalPosition = topLogo + (100 / heightLogo * 3) + (listThongTinNguoBan.Count * (listThongTinNguoBan.Count * 50 / 100f));
                        picLogo.HorizontalPosition = leftLogo;
                        picLogo.Width = widthLogo;
                        picLogo.Height = heightLogo;
                        picLogo.TextWrappingStyle = TextWrappingStyle.Through;
                    }

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        List<MauHoaDonTuyChinhChiTietViewModel> listChildren = listThongTinNguoBan[i].Children;

                        for (int j = 0; j < listChildren.Count; j++)
                        {
                            var itemChildren = listChildren[j];

                            if (maxCol == 2 && listChildren.Count() == 1)
                            {
                                table.ApplyHorizontalMerge(i, canTieuDe == 1 ? startColWithoutLogo : (startColWithoutLogo + 1), endColWithoutLogo);
                            }

                            if (j == 0)
                            {
                                TableCell tableCell = tableRow.Cells[startColWithoutLogo];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                if (itemChildren.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan)
                                {
                                    table.ApplyHorizontalMerge(i, startColWithoutLogo, endColWithoutLogo);
                                    tableCell = tableRow.Cells[startColWithoutLogo];
                                    par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                    par.AddStyleTextRange(itemChildren.Children[0]);
                                }
                                else
                                {
                                    if (itemChildren.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayCanCu)
                                    {
                                        MauHoaDonTuyChinhChiTietViewModel child = itemChildren.Children[0];
                                        child.GiaTri = mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? "Ngày <ddPXK> tháng <mmPXK> năm <yyyyPXK>" : "Ngày (Date) <ddPXK> tháng (month) <mmPXK> năm (year) <yyyyPXK>";

                                        if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                        {
                                            var splitNgayThangNams = child.GiaTri.Split(" ");
                                            foreach (var ntn in splitNgayThangNams)
                                            {
                                                var cloneNTN = CloneHelper.DeepClone(child);
                                                cloneNTN.GiaTri = ntn + " ";

                                                if (ntn.Contains("Date") || ntn.Contains("month") || ntn.Contains("year"))
                                                {
                                                    cloneNTN.LoaiContainer = LoaiContainerTuyChinh.TieuDeSongNgu;
                                                    par.AddStyleTextRange(cloneNTN);
                                                }
                                                else
                                                {
                                                    par.AddStyleTextRange(cloneNTN);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            par.AddStyleTextRange(child);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var child in itemChildren.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += canTieuDe == 3 ? "" : ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}{(canTieuDe == 3 ? "" : ":")} ";
                                            }
                                            else
                                            {
                                                if (canTieuDe > 1)
                                                {
                                                    tableCell = tableRow.Cells[startColWithoutLogo + 1];
                                                    par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                                                }

                                                if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiBan && child.TuyChonChiTiet.MaSoThue == true)
                                                {
                                                    if (canTieuDe == 3)
                                                    {
                                                        child.GiaTri = ":" + child.GiaTri;
                                                    }

                                                    CreateTableMST(doc, par, itemChildren.Children[0], child, canTieuDe == 1);
                                                }
                                                else
                                                {
                                                    child.GiaTri = (canTieuDe == 3 ? ": " : "") + (!string.IsNullOrEmpty(child.GiaTri) ? child.GiaTri : child.LoaiChiTiet.GenerateKeyTag());
                                                }
                                            }

                                            par.AddStyleTextRange(child);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                TableCell tableCell = tableRow.Cells[startColWithoutLogo + 1 + (canTieuDe > 1 ? 1 : 0)];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                if (itemChildren.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayCanCu)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = itemChildren.Children[0];
                                    child.GiaTri = mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? "Ngày <ddPXK> tháng <mmPXK> năm <yyyyPXK>" : "Ngày (Date) <ddPXK> tháng (month) <mmPXK> năm (year) <yyyyPXK>";

                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                    {
                                        var splitNgayThangNams = child.GiaTri.Split(" ");
                                        foreach (var ntn in splitNgayThangNams)
                                        {
                                            var cloneNTN = CloneHelper.DeepClone(child);
                                            cloneNTN.GiaTri = ntn + " ";

                                            if (ntn.Contains("Date") || ntn.Contains("month") || ntn.Contains("year"))
                                            {
                                                cloneNTN.LoaiContainer = LoaiContainerTuyChinh.TieuDeSongNgu;
                                                par.AddStyleTextRange(cloneNTN);
                                            }
                                            else
                                            {
                                                par.AddStyleTextRange(cloneNTN);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        par.AddStyleTextRange(child);
                                    }
                                }
                                else
                                {
                                    foreach (var child in itemChildren.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                        {
                                            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                            {
                                                child.GiaTri += ": ";
                                            }
                                        }
                                        else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                        {
                                            child.GiaTri = $" {child.GiaTri}: ";
                                        }
                                        else
                                        {
                                            child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                        }

                                        par.AddStyleTextRange(child);
                                    }
                                }
                            }
                        }
                    }

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                }

                if (tableType == TableType.ThongTinHoaDon)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinHoaDon = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon && (x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon || (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon && !string.IsNullOrEmpty(x.Children[0].GiaTri)))).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listMSKHSHD = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listMaCuaCQT = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinMaCuaCoQuanThue).ToList();

                    int row = listThongTinHoaDon.Count;

                    if (row > 3)
                    {
                        for (int i = 0; i < row - 3; i++)
                        {
                            TableRow cl_row = table.Rows[2].Clone();
                            table.Rows.Insert(2, cl_row);
                        }
                    }

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = listThongTinHoaDon[i];

                        TableCell tableCell = tableRow.Cells[1];
                        Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                        MauHoaDonTuyChinhChiTietViewModel child = item.Children[0];

                        switch (child.LoaiChiTiet)
                        {
                            case LoaiChiTietTuyChonNoiDung.TenMauHoaDon:
                                child.GiaTri = loai.GetDescription();
                                break;
                            case LoaiChiTietTuyChonNoiDung.NgayThangNamTieuDe:
                                child.GiaTri = mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? "Ngày <dd> tháng <mm> năm <yyyy>" : "Ngày (Date) <dd> tháng (month) <mm> năm (year) <yyyy>";
                                break;
                            default:
                                break;
                        }

                        if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayThangNamTieuDe && mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                        {
                            var splitNgayThangNams = child.GiaTri.Split(" ");
                            foreach (var ntn in splitNgayThangNams)
                            {
                                var cloneNTN = CloneHelper.DeepClone(child);
                                cloneNTN.GiaTri = ntn + " ";

                                if (ntn.Contains("Date") || ntn.Contains("month") || ntn.Contains("year"))
                                {
                                    cloneNTN.LoaiContainer = LoaiContainerTuyChinh.TieuDeSongNgu;
                                    par.AddStyleTextRange(cloneNTN);
                                }
                                else
                                {
                                    par.AddStyleTextRange(cloneNTN);
                                }
                            }
                        }
                        else
                        {
                            par.AddStyleTextRange(child);
                        }

                        if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA && item.Children.Any(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu))
                        {
                            Paragraph parSN = tableCell.AddParagraph();

                            MauHoaDonTuyChinhChiTietViewModel childSN = item.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);

                            switch (childSN.LoaiChiTiet)
                            {
                                case LoaiChiTietTuyChonNoiDung.TenMauHoaDon:
                                    if (loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi ||
                                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All ||
                                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau ||
                                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe)
                                    {
                                        childSN.GiaTri = "(Invoice converted from E-invoice)";
                                    }
                                    break;
                                default:
                                    break;
                            }

                            parSN.Format.HorizontalAlignment = HorizontalAlignment.Center;
                            parSN.AddStyleTextRange(childSN);
                        }
                    }

                    if (listMSKHSHD.Any())
                    {
                        TableCell tableCell = table.Rows[0].Cells[2];

                        for (int i = 0; i < listMSKHSHD.Count; i++)
                        {
                            Paragraph par = null;

                            if (i == 0)
                            {
                                par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                            }
                            else
                            {
                                par = tableCell.AddParagraph();
                                par.Format.BeforeSpacing = 5;
                            }

                            MauHoaDonTuyChinhChiTietViewModel item = listMSKHSHD[i];
                            foreach (var child in item.Children)
                            {
                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                {
                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                    {
                                        child.GiaTri += ": ";
                                    }
                                }
                                else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                {
                                    child.GiaTri = $" {child.GiaTri}: ";
                                }
                                else
                                {
                                    child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                }
                                par.AddStyleTextRange(child);
                            }
                        }
                    }

                    if (mauHoaDon.HinhThucHoaDon == HinhThucHoaDon.CoMa)
                    {
                        int rowTieuDe = table.Rows.Count;
                        int colTieuDe = table.Rows[0].Cells.Count;
                        var maCuaCQT = listMaCuaCQT.Any() ? listMaCuaCQT[0] : null;

                        if (maCuaCQT != null)
                        {
                            TableRow cl_row = table.Rows[rowTieuDe - 1].Clone();
                            table.Rows.Insert(rowTieuDe, cl_row);
                            table.ApplyHorizontalMerge(rowTieuDe, 0, 1);

                            TableRow rowMaCuaCQT = table.Rows[table.Rows.Count - 1];
                            rowMaCuaCQT.Cells[0].Paragraphs.Clear();
                            Paragraph par = rowMaCuaCQT.Cells[0].AddParagraph();
                            foreach (var child in maCuaCQT.Children)
                            {
                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                {
                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                    {
                                        child.GiaTri += ": ";
                                    }
                                }
                                else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                {
                                    child.GiaTri = $" {child.GiaTri}: ";
                                }
                                else
                                {
                                    child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                }
                                par.AddStyleTextRange(child);
                            }
                        }
                    }

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                    table.TableFormat.Borders.Top.BorderType = BorderStyle.Single;
                    table.TableFormat.Borders.Top.Color = ColorTranslator.FromHtml("#b7b7b7");
                    table.TableFormat.Borders.Bottom.BorderType = BorderStyle.Single;
                    table.TableFormat.Borders.Bottom.Color = ColorTranslator.FromHtml("#b7b7b7");
                }

                if (tableType == TableType.ThongTinNguoiMua)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinChung = new List<MauHoaDonTuyChinhChiTietViewModel>(); ;
                    List<MauHoaDonTuyChinhChiTietViewModel> listHHTT_STK = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HinhThucThanhToan || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listBoSung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && ((x.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung1 && x.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung10) || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.GhiChuBenMua)).ToList();

                    List<MauHoaDonTuyChinhChiTietViewModel> listCanCuNgaySo = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CanCuSo || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayCanCu)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listCuaVeViec = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.Cua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.VeViecDienGiai)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listNguoiVanChuyenHopDongSo = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenNguoiVanChuyen || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HopDongVanChuyenSo)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listNguoiNhanHangDongTienThanhToan = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HoTenNguoiNhanHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan)).ToList();

                    List<MauHoaDonTuyChinhChiTietViewModel> listCuaVoiMaSoThue = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.Cua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listPhuongTienVanChuyen = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhuongThucVanChuyen).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listXuatKho = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoXuatHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HoTenNguoiXuatHang)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listNhapKho = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoNhanHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HoTenNguoiNhanHang)).ToList();

                    // Nếu không là Ngoại tệ thì ẩn Đồng tiền thanh toán
                    if (!(loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All))
                    {
                        listHHTT_STK = listHHTT_STK.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DongTienThanhToan).ToList();
                        listNguoiNhanHangDongTienThanhToan = listNguoiNhanHangDongTienThanhToan.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DongTienThanhToan).ToList();
                    }

                    var listThongTinNguoiMua = new List<MauHoaDonTuyChinhChiTietViewModel>();
                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang)
                    {
                        listThongTinChung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.HoTenNguoiMua && x.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.DiaChiNguoiMua).ToList();

                        foreach (var item in listThongTinChung)
                        {
                            listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                            });
                        }

                        if (listHHTT_STK.Any())
                        {
                            listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                            {
                                Children = listHHTT_STK
                            });
                        }
                    }
                    else
                    {
                        if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.PXKKiemVanChuyenNoiBo)
                        {
                            if (mauHoaDon.TenBoMau == "01.CB.01")
                            {
                                listThongTinChung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhuongThucVanChuyen || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoXuatHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoNhanHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan)).ToList();

                                // Nếu không là Ngoại tệ thì ẩn Đồng tiền thanh toán
                                if (!(loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All))
                                {
                                    listThongTinChung = listThongTinChung.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DongTienThanhToan).ToList();
                                }

                                if (listCanCuNgaySo.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listCanCuNgaySo
                                    });
                                }

                                if (listCuaVeViec.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listCuaVeViec
                                    });
                                }

                                if (listNguoiVanChuyenHopDongSo.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listNguoiVanChuyenHopDongSo
                                    });
                                }

                                foreach (var item in listThongTinChung)
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                                    });
                                }
                            }
                            else
                            {
                                listThongTinChung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiMua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaSoThueNguoiMua || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoNhanHang)).ToList();

                                foreach (var item in listThongTinChung)
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                                    });
                                }

                                if (listNguoiNhanHangDongTienThanhToan.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listNguoiNhanHangDongTienThanhToan
                                    });
                                }
                            }
                        }
                        else
                        {
                            if (listCanCuNgaySo.Any())
                            {
                                listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                {
                                    Children = listCanCuNgaySo
                                });
                            }

                            foreach (var item in listCuaVoiMaSoThue)
                            {
                                listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                {
                                    Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                                });
                            }

                            if (listNguoiVanChuyenHopDongSo.Any())
                            {
                                listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                {
                                    Children = listNguoiVanChuyenHopDongSo
                                });
                            }

                            if (mauHoaDon.TenBoMau == "01.CB.01")
                            {
                                listThongTinChung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhuongThucVanChuyen || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoXuatHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DiaChiKhoNhanHang || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan)).ToList();

                                // Nếu không là Ngoại tệ thì ẩn Đồng tiền thanh toán
                                if (!(loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All))
                                {
                                    listThongTinChung = listThongTinChung.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DongTienThanhToan).ToList();
                                }

                                foreach (var item in listThongTinChung)
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                                    });
                                }
                            }
                            else
                            {
                                listThongTinChung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan).ToList();

                                // Nếu không là Ngoại tệ thì ẩn Đồng tiền thanh toán
                                if (!(loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All))
                                {
                                    listThongTinChung = listThongTinChung.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DongTienThanhToan).ToList();
                                }

                                if (listPhuongTienVanChuyen.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listPhuongTienVanChuyen
                                    });
                                }

                                if (listXuatKho.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listXuatKho
                                    });
                                }

                                if (listNhapKho.Any())
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = listNhapKho
                                    });
                                }

                                foreach (var item in listThongTinChung)
                                {
                                    listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                                    {
                                        Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                                    });
                                }
                            }
                        }
                    }

                    foreach (var item in listBoSung)
                    {
                        listThongTinNguoiMua.Add(new MauHoaDonTuyChinhChiTietViewModel
                        {
                            Children = new List<MauHoaDonTuyChinhChiTietViewModel> { item }
                        });
                    }

                    bool isHienThiQRCode = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.HienThiQRCode).GiaTri == "true";
                    int canTieuDe = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua).SelectMany(x => x.Children).FirstOrDefault().TuyChonChiTiet.CanTieuDe.Value;
                    int row = listThongTinNguoiMua.Count;
                    int maxCol = listThongTinNguoiMua.Max(x => x.Children.Count);
                    int col = maxCol;
                    if (canTieuDe > 1)
                    {
                        col += 1;
                    }
                    int colToText = col;
                    int colToMergeQRCode = -1;
                    int rowToBoSung = -1;
                    if (isHienThiQRCode)
                    {
                        col += 1;

                        if (row >= 4)
                        {
                            colToMergeQRCode = 4;
                        }
                        else
                        {
                            colToMergeQRCode = row;
                        }
                    }

                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang)
                    {
                        rowToBoSung = 5;
                    }

                    table.Rows[0].Cells[0].SplitCell(col, row);

                    // Set col width
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        var doRong = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua).SelectMany(x => x.Children).Max(x => x.TuyChonChiTiet.DoRong ?? 0);
                        var firstDoRong = doRong * ((canTieuDe == 2 ? 33 : 30) * (doRong / (isHienThiQRCode ? 150f : 130f))) / 100;

                        if (canTieuDe > 1)
                        {
                            table.Rows[i].Cells[0].Width = firstDoRong;
                        }

                        if (isHienThiQRCode)
                        {
                            table.Rows[i].Cells[col - 1].Width = 50;
                        }

                        if (listHHTT_STK.Any())
                        {
                            if (listHHTT_STK.Count == 2 && canTieuDe == 1)
                            {
                                for (int j = 0; j < listHHTT_STK.Count; j++)
                                {
                                    table.Rows[i].Cells[j].Width = (table.Width / 2) - ((j != 0 && isHienThiQRCode) ? 50 : 0);
                                }
                            }
                            else if (listHHTT_STK.Count == 3)
                            {
                                for (int j = 0; j < listHHTT_STK.Count; j++)
                                {
                                    if (j != 0 && listHHTT_STK[j].LoaiChiTiet == LoaiChiTietTuyChonNoiDung.DongTienThanhToan)
                                    {
                                        table.Rows[i].Cells[canTieuDe == 1 ? j : (j + 1)].Width = isHienThiQRCode ? 50 : 100;
                                    }
                                }
                            }
                        }
                    }

                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang)
                    {
                        for (int i = 0; i < listThongTinChung.Count; i++)
                        {
                            table.ApplyHorizontalMerge(i, canTieuDe > 1 ? 1 : 0, colToText - 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < row; i++)
                        {
                            var hasMerge = listThongTinNguoiMua[i].Children.Count == 1;
                            if (hasMerge)
                            {
                                table.ApplyHorizontalMerge(i, canTieuDe > 1 ? 1 : 0, colToText - 1);
                            }
                        }
                    }

                    if (isHienThiQRCode)
                    {
                        table.ApplyVerticalMerge(col - 1, 0, colToMergeQRCode - 1);
                    }

                    if (maxCol >= 2 && isHienThiQRCode)
                    {
                        if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang)
                        {
                            table.ApplyHorizontalMerge(4, col - 2, col - 1);
                        }
                    }

                    for (int i = rowToBoSung; i < (listBoSung.Count + rowToBoSung); i++)
                    {
                        table.ApplyHorizontalMerge(i, canTieuDe > 1 ? 1 : 0, col - 1);
                    }

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        List<MauHoaDonTuyChinhChiTietViewModel> listChildren = listThongTinNguoiMua[i].Children;

                        for (int j = 0; j < listChildren.Count; j++)
                        {
                            var itemChildren = listChildren[j];

                            if (j == 0)
                            {
                                TableCell tableCell = tableRow.Cells[0];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                if (itemChildren.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayCanCu)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = itemChildren.Children[0];
                                    child.GiaTri = mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? "Ngày <ddPXK> tháng <mmPXK> năm <yyyyPXK>" : "Ngày (Date) <ddPXK> tháng (month) <mmPXK> năm (year) <yyyyPXK>";

                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                    {
                                        var splitNgayThangNams = child.GiaTri.Split(" ");
                                        foreach (var ntn in splitNgayThangNams)
                                        {
                                            var cloneNTN = CloneHelper.DeepClone(child);
                                            cloneNTN.GiaTri = ntn + " ";

                                            if (ntn.Contains("Date") || ntn.Contains("month") || ntn.Contains("year"))
                                            {
                                                cloneNTN.LoaiContainer = LoaiContainerTuyChinh.TieuDeSongNgu;
                                                par.AddStyleTextRange(cloneNTN);
                                            }
                                            else
                                            {
                                                par.AddStyleTextRange(cloneNTN);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        par.AddStyleTextRange(child);
                                    }
                                }
                                else
                                {
                                    foreach (var child in itemChildren.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                        {
                                            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                            {
                                                child.GiaTri += canTieuDe == 3 ? "" : ": ";
                                            }
                                        }
                                        else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                        {
                                            child.GiaTri = $" {child.GiaTri}{(canTieuDe == 3 ? "" : ":")} ";
                                        }
                                        else
                                        {
                                            if (canTieuDe > 1)
                                            {
                                                tableCell = tableRow.Cells[1];
                                                par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                                            }

                                            child.GiaTri = (canTieuDe == 3 ? ": " : "") + child.LoaiChiTiet.GenerateKeyTag();
                                        }

                                        par.AddStyleTextRange(child);
                                    }
                                }
                            }
                            else
                            {
                                TableCell tableCell = tableRow.Cells[j + (canTieuDe > 1 ? 1 : 0)];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                if (itemChildren.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayCanCu)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = itemChildren.Children[0];
                                    child.GiaTri = mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? "Ngày <ddPXK> tháng <mmPXK> năm <yyyyPXK>" : "Ngày (Date) <ddPXK> tháng (month) <mmPXK> năm (year) <yyyyPXK>";

                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                    {
                                        var splitNgayThangNams = child.GiaTri.Split(" ");
                                        foreach (var ntn in splitNgayThangNams)
                                        {
                                            var cloneNTN = CloneHelper.DeepClone(child);
                                            cloneNTN.GiaTri = ntn + " ";

                                            if (ntn.Contains("Date") || ntn.Contains("month") || ntn.Contains("year"))
                                            {
                                                cloneNTN.LoaiContainer = LoaiContainerTuyChinh.TieuDeSongNgu;
                                                par.AddStyleTextRange(cloneNTN);
                                            }
                                            else
                                            {
                                                par.AddStyleTextRange(cloneNTN);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        par.AddStyleTextRange(child);
                                    }
                                }
                                else
                                {
                                    foreach (var child in itemChildren.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                        {
                                            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                            {
                                                child.GiaTri += ": ";
                                            }
                                        }
                                        else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                        {
                                            child.GiaTri = $" {child.GiaTri}: ";
                                        }
                                        else
                                        {
                                            child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                        }

                                        par.AddStyleTextRange(child);
                                    }
                                }
                            }
                        }
                    }

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                }

                if (tableType == TableType.ThongTinHangHoaDichVu)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listHangHoaDichVu = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinVeHangHoaDichVu).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listTongGiaTriHHDV = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinVeTongGiaTriHHDV).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listNgoaiTe = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNgoaiTe).ToList();

                    if (mauHoaDon.TenBoMau == "01.CB.04" && mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT)
                    {
                        listNgoaiTe = new List<MauHoaDonTuyChinhChiTietViewModel>();
                    }

                    var listColSpan = new List<GroupHeaderHHDVTable>();
                    var listNotSpan = new List<LoaiChiTietTuyChonNoiDung>();

                    if (listHangHoaDichVu.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoLuongNhapXuat))
                    {
                        var indexOfSoLuongNhapXuat = listHangHoaDichVu.FindIndex(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoLuongNhapXuat);
                        var itemSoLuongNhapXuat = listHangHoaDichVu[indexOfSoLuongNhapXuat];
                        listColSpan.Add(new GroupHeaderHHDVTable
                        {
                            Key = LoaiChiTietTuyChonNoiDung.SoLuongNhapXuat,
                            Item = itemSoLuongNhapXuat,
                            Span = 2,
                            Index = indexOfSoLuongNhapXuat
                        });

                        listNotSpan.Add(LoaiChiTietTuyChonNoiDung.SoLuong);
                        listNotSpan.Add(LoaiChiTietTuyChonNoiDung.SoLuongNhap);

                        listHangHoaDichVu = listHangHoaDichVu.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.SoLuongNhapXuat).ToList();
                    }

                    int col = listHangHoaDichVu.Count();
                    int row = 5;
                    int amountRow = 0;
                    int amountCol = 0;
                    bool hasCK = false;
                    bool hasNT = false;
                    int beginToFillData = 1;
                    bool isThietLapDongKyHieuCot = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.ThietLapDongKyHieuCot).GiaTri == "true";
                    if (isThietLapDongKyHieuCot)
                    {
                        row += 1;
                        beginToFillData += 1;
                    }

                    if (loai == HinhThucMauHoaDon.HoaDonMauCoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_CoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
                    {
                        hasCK = true;

                        if (mauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauMotThueSuat)
                        {
                            amountRow += 2;
                        }
                        else
                        {
                            amountRow += 1;
                        }
                    }

                    if ((loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All))
                    {
                        hasNT = true;
                        amountRow += listNgoaiTe.Count();
                    }
                    else
                    {
                        listHangHoaDichVu = listHangHoaDichVu.Where(x => x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TyGiaHHDV).ToList();
                        col = listHangHoaDichVu.Count();
                    }

                    table.Rows[0].Cells[0].SplitCell(0, 2);

                    switch (mauHoaDon.LoaiHoaDon)
                    {
                        case LoaiHoaDon.HoaDonGTGT:
                            if (mauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauMotThueSuat)
                            {
                                amountCol = 3;
                                amountRow += 4;
                            }
                            else
                            {
                                if (mauHoaDon.TenBoMau == "01.CB.01")
                                {
                                    amountCol = 4;
                                    amountRow += 3 + listTongGiaTriHHDV.Where(x => x.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue &&
                                                                                    x.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.TongTienChiuThueSuatKhac)
                                                                        .Count();
                                }
                                else
                                {
                                    amountCol = 3;
                                    amountRow += 4;
                                }

                                if (!listTongGiaTriHHDV.Any(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau ||
                                                                x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau))
                                {
                                    hasCK = false;
                                    amountRow -= 1;
                                }
                            }
                            break;
                        case LoaiHoaDon.HoaDonBanHang:
                            amountCol = 3;
                            amountRow += 2;
                            break;
                        case LoaiHoaDon.PXKKiemVanChuyenNoiBo:
                        case LoaiHoaDon.PXKHangGuiBanDaiLy:
                            amountCol = 1;
                            amountRow += 1;
                            break;
                        default:
                            break;
                    }

                    if (listColSpan.Any())
                    {
                        row += 1;
                        beginToFillData += 1;
                    }

                    table.Rows[0].Cells[0].SplitCell(col, row);
                    table.TableFormat.Paddings.All = 0;

                    PreferredWidth width = new PreferredWidth(WidthType.Percentage, 100);
                    table.PreferredWidth = width;

                    List<int> listColWidth = new List<int>();
                    for (int i = 0; i < listHangHoaDichVu.Count; i++)
                    {
                        int doRong = listHangHoaDichVu[i].Children[0].TuyChonChiTiet.DoRong ?? 0;
                        listColWidth.Add(doRong);
                    }

                    for (int i = 0; i < row; i++)
                    {
                        for (int j = 0; j < listColWidth.Count; j++)
                        {
                            table.Rows[i].Cells[j].SetCellWidth(listColWidth[j], CellWidthType.Percentage);
                        }
                    }

                    if (listColSpan.Any())
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            for (int j = 0; j < col; j++)
                            {
                                var item = listHangHoaDichVu[j];

                                if (i == 0)
                                {
                                    var itemColSpan = listColSpan.FirstOrDefault(x => x.Index == j);

                                    if (itemColSpan != null)
                                    {
                                        table.ApplyHorizontalMerge(i, j, j + itemColSpan.Span - 1);
                                    }
                                    else
                                    {
                                        if (!listNotSpan.Any(x => x == item.LoaiChiTiet))
                                        {
                                            table.ApplyVerticalMerge(j, 0, 1);
                                        }
                                    }
                                }
                            }
                        }
                    }


                    table[table.Rows.Count - 1, 0].Paragraphs.Clear();
                    table[table.Rows.Count - 1, 0].CellFormat.Borders.BorderType = BorderStyle.Cleared;
                    Table tblTotalAmount = table[table.Rows.Count - 1, 0].AddTable(true);
                    tblTotalAmount.ResetCells(amountRow, amountCol);
                    tblTotalAmount.TableFormat.Paddings.All = 0;
                    tblTotalAmount.TableFormat.Borders.Top.BorderType = BorderStyle.Cleared;
                    tblTotalAmount.TableFormat.Borders.LineWidth = 1F;
                    tblTotalAmount.TableFormat.Borders.Top.LineWidth = 0;
                    tblTotalAmount.TableFormat.Borders.Bottom.LineWidth = 0.5F;

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];

                        for (int j = 0; j < col; j++)
                        {
                            TableCell tableCell = tableRow.Cells[j];
                            Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                            par.ApplyStyleParHHDV();

                            if (i == 0 || (listColSpan.Any() && i == 1))
                            {
                                MauHoaDonTuyChinhChiTietViewModel child = null;

                                if (i == 0 && listColSpan.Any(x => x.Index == j))
                                {
                                    child = listColSpan.FirstOrDefault(x => x.Index == j).Item.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                }
                                else
                                {
                                    child = listHangHoaDichVu[j].Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                }

                                par.AddStyleTextRange(child);

                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel childSN = listHangHoaDichVu[j].Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);
                                    par = tableCell.AddParagraph();
                                    par.AddStyleTextRange(childSN);
                                }

                                if (!string.IsNullOrEmpty(child.TuyChonChiTiet.MauNenTieuDeBang) && child.TuyChonChiTiet.MauNenTieuDeBang.ToUpper() != "#FFFFFF")
                                {
                                    tableCell.CellFormat.BackColor = ColorTranslator.FromHtml(child.TuyChonChiTiet.MauNenTieuDeBang);
                                }
                            }
                            else if (isThietLapDongKyHieuCot == true && i == (listColSpan.Any() ? 2 : 1))
                            {
                                MauHoaDonTuyChinhChiTietViewModel child = listHangHoaDichVu[j].Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.KyHieuCot);
                                par.AddStyleTextRange(child);
                            }
                            else
                            {
                                if (beginToFillData <= i)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = listHangHoaDichVu[j].Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.NoiDung);
                                    child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                    par.AddStyleParagraph(doc, child);
                                }
                            }
                        }
                    }

                    table.Rows[0].IsHeader = true;
                    if (isThietLapDongKyHieuCot == true)
                    {
                        if (listColSpan.Any())
                        {
                            table.Rows[1].IsHeader = true;
                        }

                        table.Rows[listColSpan.Any() ? 2 : 1].IsHeader = true;
                    }

                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonGTGT || mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang)
                    {
                        if (mauHoaDon.LoaiThueGTGT == LoaiThueGTGT.MauMotThueSuat)
                        {
                            PreferredWidth widthAmount = new PreferredWidth(WidthType.Percentage, 100);
                            tblTotalAmount.PreferredWidth = widthAmount;

                            for (int i = 0; i < amountRow; i++)
                            {
                                tblTotalAmount.Rows[i].Cells[0].SetCellWidth(46, CellWidthType.Percentage);
                                tblTotalAmount.Rows[i].Cells[1].SetCellWidth(38, CellWidthType.Percentage);
                                tblTotalAmount.Rows[i].Cells[2].SetCellWidth(16, CellWidthType.Percentage);

                                var tableRow = tblTotalAmount.Rows[i];
                                for (int j = 0; j < amountCol; j++)
                                {
                                    var tableCell = tableRow.Cells[j];
                                    tableCell.Paragraphs.Clear();
                                }

                                MauHoaDonTuyChinhChiTietViewModel itemLeft = null;
                                MauHoaDonTuyChinhChiTietViewModel itemRight = null;

                                switch (mauHoaDon.LoaiHoaDon)
                                {
                                    case LoaiHoaDon.HoaDonGTGT:
                                        if (i == 0)
                                        {
                                            itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang);
                                        }
                                        else if (i == 1)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau);
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau);
                                            }
                                            else
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.ThueSuatGTGT);
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueGTGT);
                                            }
                                        }
                                        else if (i == 2)
                                        {
                                            if (hasCK)
                                            {
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHangDaTruCK);
                                            }
                                            else
                                            {
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                                            }
                                        }
                                        else if (i == 3)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.ThueSuatGTGT);
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueGTGT);
                                            }
                                            else
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu);
                                            }
                                        }
                                        else if (i == 4)
                                        {
                                            if (hasCK)
                                            {
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                                            }
                                            else
                                            {
                                                if (hasNT)
                                                {
                                                    itemLeft = listNgoaiTe[i - 4];

                                                }
                                            }
                                        }
                                        else if (i == 5)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu);
                                            }
                                            else
                                            {
                                                if (hasNT)
                                                {
                                                    itemLeft = listNgoaiTe[i - 4];
                                                }
                                            }
                                        }
                                        else if (i == 6)
                                        {
                                            itemLeft = listNgoaiTe[i - 6];
                                        }
                                        else
                                        {
                                            itemLeft = listNgoaiTe[i - 6];
                                        }
                                        break;
                                    case LoaiHoaDon.HoaDonBanHang:
                                        if (i == 0)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang);
                                            }
                                            else
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                                            }
                                        }
                                        else if (i == 1)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau);
                                                itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau);
                                            }
                                            else
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu);
                                            }
                                        }
                                        else if (i == 2)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                                            }
                                            else
                                            {
                                                if (hasNT)
                                                {
                                                    itemLeft = listNgoaiTe[i - 2];
                                                }
                                            }
                                        }
                                        else if (i == 3)
                                        {
                                            if (hasCK)
                                            {
                                                itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu);
                                            }
                                            else
                                            {
                                                if (hasNT)
                                                {
                                                    itemLeft = listNgoaiTe[i - 2];
                                                }
                                            }
                                        }
                                        else if (i == 4)
                                        {
                                            itemLeft = listNgoaiTe[i - 4];
                                        }
                                        else
                                        {
                                            itemLeft = listNgoaiTe[i - 4];
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                if (itemLeft != null && (itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu ||
                                                        itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyGia ||
                                                        itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.QuyDoi))
                                {
                                    tblTotalAmount.ApplyHorizontalMerge(i, 0, amountCol - 1);

                                    if (itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyGia ||
                                        itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.QuyDoi)
                                    {
                                        tableRow.Cells[0].CellFormat.Borders.BorderType = BorderStyle.Cleared;
                                    }
                                }
                                else
                                {
                                    tableRow.Cells[0].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                    tableRow.Cells[1].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                    tableRow.Cells[1].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                    tableRow.Cells[2].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;

                                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && (itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan ||
                                                                                           itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang))
                                    {
                                        tblTotalAmount.ApplyHorizontalMerge(i, 0, amountCol - 2);
                                    }
                                }

                                if (itemLeft != null)
                                {
                                    var par = tableRow.Cells[0].AddParagraph();
                                    par.ApplyStyleParHHDV();

                                    if (itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyGia)
                                    {
                                        par.Format.BeforeSpacing = 5;
                                    }

                                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && (itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan ||
                                                                                            itemLeft.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang))
                                    {
                                        var tieuDeItemLefts = itemLeft.Children
                                            .Where(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe || x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            .ToList();

                                        foreach (var child in tieuDeItemLefts)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang)
                                                {
                                                    child.GiaTri += " (Chưa trừ CK)";
                                                }

                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            par.AddStyleTextRange(child);
                                        }

                                        var noiDungItemLeft = itemLeft.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.NoiDung);
                                        par = tableRow.Cells[2].AddParagraph();
                                        par.ApplyStyleParHHDV();
                                        noiDungItemLeft.GiaTri = noiDungItemLeft.LoaiChiTiet.GenerateKeyTag();
                                        par.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                        par.AddStyleTextRange(noiDungItemLeft);
                                    }
                                    else
                                    {
                                        foreach (var child in itemLeft.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.HoaDonBanHang && child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang)
                                                {
                                                    child.GiaTri += " (Chưa trừ CK)";
                                                }

                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            else
                                            {
                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                            }

                                            par.AddStyleTextRange(child);
                                        }
                                    }
                                }
                                else
                                {
                                    var par = tableRow.Cells[0].AddParagraph();
                                    par.ApplyEmptyPar(doc);
                                }
                                if (itemRight != null)
                                {
                                    var par1 = tableRow.Cells[1].AddParagraph();
                                    par1.ApplyStyleParHHDV();
                                    var par2 = tableRow.Cells[2].AddParagraph();
                                    par2.ApplyStyleParHHDV();

                                    foreach (var child in itemRight.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe ||
                                            child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (hasCK)
                                                {
                                                    if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang)
                                                    {
                                                        child.GiaTri += " (Chưa trừ CK)";
                                                    }
                                                    else if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHangDaTruCK)
                                                    {
                                                        child.GiaTri += " (Đã trừ CK)";
                                                    }
                                                }

                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            par1.AddStyleTextRange(child);
                                        }
                                        else
                                        {
                                            child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                            par2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                            par2.AddStyleTextRange(child);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (mauHoaDon.TenBoMau == "01.CB.01")
                            {
                                PreferredWidth widthAmount = new PreferredWidth(WidthType.Percentage, 100);
                                tblTotalAmount.PreferredWidth = widthAmount;

                                for (int i = 0; i < amountRow; i++)
                                {
                                    tblTotalAmount.Rows[i].Cells[0].SetCellWidth(25, CellWidthType.Percentage);
                                    tblTotalAmount.Rows[i].Cells[1].SetCellWidth(25, CellWidthType.Percentage);
                                    tblTotalAmount.Rows[i].Cells[2].SetCellWidth(26, CellWidthType.Percentage);
                                    tblTotalAmount.Rows[i].Cells[3].SetCellWidth(24, CellWidthType.Percentage);

                                    var tableRow = tblTotalAmount.Rows[i];

                                    for (int j = 0; j < amountCol; j++)
                                    {
                                        var tableCell = tableRow.Cells[j];
                                        tableCell.Paragraphs.Clear();
                                    }
                                }

                                var idxRow = 0;
                                if (hasCK)
                                {
                                    tblTotalAmount.ApplyHorizontalMerge(0, 0, 1);

                                    var firstRow = tblTotalAmount.Rows[0];
                                    firstRow.Cells[0].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                    var tyLeCK = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau);
                                    var soTienCK = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau);
                                    idxRow += 1;

                                    var parTyLeCK = firstRow.Cells[0].AddParagraph();

                                    if (tyLeCK != null)
                                    {
                                        parTyLeCK.ApplyStyleParHHDV();

                                        foreach (var child in tyLeCK.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            else
                                            {
                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                            }

                                            parTyLeCK.AddStyleTextRange(child);
                                        }
                                    }
                                    else
                                    {
                                        parTyLeCK.ApplyEmptyPar(doc);
                                    }

                                    firstRow.Cells[2].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                    firstRow.Cells[2].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                    firstRow.Cells[3].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;

                                    var par1 = firstRow.Cells[2].AddParagraph();
                                    var par2 = firstRow.Cells[3].AddParagraph();

                                    if (soTienCK != null)
                                    {
                                        par1.ApplyStyleParHHDV();
                                        par2.ApplyStyleParHHDV();

                                        foreach (var child in soTienCK.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe ||
                                                child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                                {
                                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                    {
                                                        child.GiaTri += ": ";
                                                    }
                                                }
                                                else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                                {
                                                    child.GiaTri = $" {child.GiaTri}: ";
                                                }
                                                par1.AddStyleTextRange(child);
                                            }
                                            else
                                            {
                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                                par2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                                par2.AddStyleTextRange(child);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        par1.ApplyEmptyPar(doc);
                                        par2.ApplyEmptyPar(doc);
                                    }
                                }

                                var tongHopTien = listTongGiaTriHHDV.Where(x => x.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.TongHopThueGTGT && x.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT).ToList();
                                for (int i = 0; i < tongHopTien.Count; i++)
                                {
                                    var item = tongHopTien[i];
                                    var totalAmountRow = tblTotalAmount.Rows[idxRow];

                                    for (int j = 0; j < amountCol; j++)
                                    {
                                        var totalAmountCell = totalAmountRow.Cells[j];
                                        var par = totalAmountCell.AddParagraph();
                                        par.ApplyStyleParHHDV();

                                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongHopThueGTGT)
                                        {
                                            MauHoaDonTuyChinhChiTietViewModel itemClone = null;
                                            if (j == 0)
                                            {
                                                var child = item.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                                par.AddStyleTextRange(child);
                                                itemClone = item;
                                            }
                                            else if (j == 1)
                                            {
                                                itemClone = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang);
                                                var child = itemClone.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                                par.AddStyleTextRange(child);
                                            }
                                            else if (j == 2)
                                            {
                                                itemClone = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueGTGT);
                                                var child = itemClone.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                                par.AddStyleTextRange(child);
                                            }
                                            else
                                            {
                                                itemClone = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                                                var child = itemClone.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                                par.AddStyleTextRange(child);
                                            }

                                            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                            {
                                                var childSN = itemClone.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);
                                                par = totalAmountCell.AddParagraph();
                                                par.AddStyleTextRange(childSN);
                                            }
                                        }
                                        else
                                        {
                                            var child = item.Children.FirstOrDefault(x => j == 0 ? (x.LoaiContainer == LoaiContainerTuyChinh.TieuDe) : (x.LoaiContainer == LoaiContainerTuyChinh.NoiDung));
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT && mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                                {
                                                    par.AddStyleTextRange(child);
                                                    var childSN = item.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);
                                                    childSN.GiaTri = $" {childSN.GiaTri}:";
                                                    par.AddStyleTextRange(childSN);
                                                }
                                                else
                                                {
                                                    child.GiaTri += ":";
                                                    par.AddStyleTextRange(child);
                                                }
                                            }
                                            else
                                            {
                                                LoaiTongHopThueGTGT loaiTongHopThue;
                                                if (j == 1)
                                                {
                                                    loaiTongHopThue = LoaiTongHopThueGTGT.ThanhTienTruocThue;
                                                }
                                                else if (j == 2)
                                                {
                                                    loaiTongHopThue = LoaiTongHopThueGTGT.TienThue;
                                                }
                                                else
                                                {
                                                    loaiTongHopThue = LoaiTongHopThueGTGT.CongTienThanhToan;
                                                }

                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTagTongHopThueGTGT(loaiTongHopThue);
                                                par.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                                par.AddStyleParagraph(doc, child);
                                            }
                                        }
                                    }

                                    idxRow += 1;
                                }

                                #region số tiền bằng chữ
                                var soTienBangChu = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu);
                                tblTotalAmount.ApplyHorizontalMerge(idxRow, 0, amountCol - 1);
                                var parSoTienBangChu = tblTotalAmount.Rows[idxRow].Cells[0].AddParagraph();
                                parSoTienBangChu.ApplyStyleParHHDV();
                                foreach (var child in soTienBangChu.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                        {
                                            child.GiaTri += ": ";
                                        }
                                    }
                                    else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                    {
                                        child.GiaTri = $" {child.GiaTri}: ";
                                    }
                                    else
                                    {
                                        child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                    }

                                    parSoTienBangChu.AddStyleTextRange(child);
                                }
                                idxRow += 1;
                                #endregion

                                if (hasNT)
                                {
                                    for (int i = 0; i < listNgoaiTe.Count; i++)
                                    {
                                        var item = listNgoaiTe[i];

                                        tblTotalAmount.ApplyHorizontalMerge(idxRow, 0, amountCol - 1);
                                        var parNT = tblTotalAmount.Rows[idxRow].Cells[0].AddParagraph();
                                        tblTotalAmount.Rows[idxRow].Cells[0].CellFormat.Borders.BorderType = BorderStyle.Cleared;
                                        parNT.ApplyStyleParHHDV();

                                        if (i == 0)
                                        {
                                            parNT.Format.BeforeSpacing = 5;
                                        }

                                        foreach (var child in item.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            else
                                            {
                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                            }

                                            parNT.AddStyleTextRange(child);
                                        }
                                        idxRow += 1;
                                    }
                                }
                            }
                            else
                            {
                                var lisTongTienExceptCK = listTongGiaTriHHDV.Where(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang ||
                                                                                x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueGTGT ||
                                                                                x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan ||
                                                                                x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu).ToList();

                                for (int i = 0; i < amountRow; i++)
                                {
                                    tblTotalAmount.Rows[i].Cells[0].SetCellWidth(46, CellWidthType.Percentage);
                                    tblTotalAmount.Rows[i].Cells[1].SetCellWidth(38, CellWidthType.Percentage);
                                    tblTotalAmount.Rows[i].Cells[2].SetCellWidth(16, CellWidthType.Percentage);

                                    var tableRow = tblTotalAmount.Rows[i];

                                    for (int j = 0; j < amountCol; j++)
                                    {
                                        var tableCell = tableRow.Cells[j];
                                        tableCell.Paragraphs.Clear();
                                    }
                                }

                                var idxTongTienExceptCK = 0;
                                var totalRowOfTongTien = 4 + (hasCK ? 1 : 0);

                                for (int i = 0; i < totalRowOfTongTien; i++)
                                {
                                    var rowOfTongTien = tblTotalAmount.Rows[i];

                                    if (!(i == 1 && hasCK == true)) // Nếu không là dòng chiết khấu
                                    {
                                        var item = lisTongTienExceptCK[idxTongTienExceptCK];
                                        var isSoTienBangChu = false;

                                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu)
                                        {
                                            tblTotalAmount.ApplyHorizontalMerge(i, 0, amountCol - 1);
                                            isSoTienBangChu = true;
                                        }
                                        else
                                        {
                                            tblTotalAmount.ApplyHorizontalMerge(i, 0, 1);

                                            rowOfTongTien.Cells[0].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                            rowOfTongTien.Cells[1].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                            rowOfTongTien.Cells[1].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                            rowOfTongTien.Cells[2].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                        }

                                        var cell = rowOfTongTien.Cells[0];

                                        var par = cell.AddParagraph();
                                        par.ApplyStyleParHHDV();
                                        foreach (var child in item.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            else
                                            {
                                                if (!isSoTienBangChu == true)
                                                {
                                                    par = rowOfTongTien.Cells[2].AddParagraph();
                                                    par.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                                    par.ApplyStyleParHHDV();
                                                }

                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                            }

                                            par.AddStyleTextRange(child);
                                        }

                                        idxTongTienExceptCK++;
                                    }
                                    else
                                    {
                                        var rowCK = tblTotalAmount.Rows[i];

                                        rowCK.Cells[0].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                        rowCK.Cells[1].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                        rowCK.Cells[1].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                                        rowCK.Cells[2].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;

                                        var tyLeCK = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau);
                                        var soTienCK = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau);

                                        var parTyLeCK = rowCK.Cells[0].AddParagraph();

                                        if (tyLeCK != null)
                                        {
                                            parTyLeCK.ApplyStyleParHHDV();

                                            foreach (var child in tyLeCK.Children)
                                            {
                                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                                {
                                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                    {
                                                        child.GiaTri += ": ";
                                                    }
                                                }
                                                else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                                {
                                                    child.GiaTri = $" {child.GiaTri}: ";
                                                }
                                                else
                                                {
                                                    child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                                }

                                                parTyLeCK.AddStyleTextRange(child);
                                            }
                                        }
                                        else
                                        {
                                            parTyLeCK.ApplyEmptyPar(doc);
                                        }

                                        var par1 = rowCK.Cells[1].AddParagraph();
                                        var par2 = rowCK.Cells[2].AddParagraph();

                                        if (soTienCK != null)
                                        {
                                            par1.ApplyStyleParHHDV();
                                            par2.ApplyStyleParHHDV();

                                            foreach (var child in soTienCK.Children)
                                            {
                                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe ||
                                                    child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                                {
                                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                                    {
                                                        if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                        {
                                                            child.GiaTri += ": ";
                                                        }
                                                    }
                                                    else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                                    {
                                                        child.GiaTri = $" {child.GiaTri}: ";
                                                    }
                                                    par1.AddStyleTextRange(child);
                                                }
                                                else
                                                {
                                                    child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                                    par2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                                    par2.AddStyleTextRange(child);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            par1.ApplyEmptyPar(doc);
                                            par2.ApplyEmptyPar(doc);
                                        }
                                    }
                                }

                                // Ngoai te
                                if (hasNT)
                                {
                                    for (int i = totalRowOfTongTien; i < (listNgoaiTe.Count + totalRowOfTongTien); i++)
                                    {
                                        tblTotalAmount.ApplyHorizontalMerge(i, 0, amountCol - 1);
                                        tblTotalAmount.Rows[i].Cells[0].CellFormat.Borders.BorderType = BorderStyle.Cleared;

                                        var item = listNgoaiTe[i - totalRowOfTongTien];

                                        var cell = tblTotalAmount.Rows[i].Cells[0];

                                        var par = cell.AddParagraph();

                                        if (i == totalRowOfTongTien)
                                        {
                                            par.Format.BeforeSpacing = 5;
                                        }

                                        par.ApplyStyleParHHDV();
                                        foreach (var child in item.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                                {
                                                    child.GiaTri += ": ";
                                                }
                                            }
                                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                            {
                                                child.GiaTri = $" {child.GiaTri}: ";
                                            }
                                            else
                                            {
                                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                            }

                                            par.AddStyleTextRange(child);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var cell = tblTotalAmount.Rows[0].Cells[0];
                        cell.Paragraphs.Clear();
                        var par = cell.AddParagraph();
                        par.ApplyStyleParHHDV();

                        var item = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                        foreach (var child in item.Children)
                        {
                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                            {
                                if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                {
                                    child.GiaTri += ": ";
                                }
                            }
                            else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                            {
                                child.GiaTri = $" {child.GiaTri}: ";
                            }
                            else
                            {
                                child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                            }

                            par.AddStyleTextRange(child);
                        }

                        if (hasNT)
                        {
                            for (int i = 1; i <= listNgoaiTe.Count; i++)
                            {
                                var itemNT = listNgoaiTe[i - 1];

                                tblTotalAmount.ApplyHorizontalMerge(i, 0, amountCol - 1);
                                var parNT = tblTotalAmount.Rows[i].Cells[0].AddParagraph();
                                tblTotalAmount.Rows[i].Cells[0].CellFormat.Borders.BorderType = BorderStyle.Cleared;
                                parNT.ApplyStyleParHHDV();

                                if (i == 1)
                                {
                                    parNT.Format.BeforeSpacing = 5;
                                }

                                foreach (var child in itemNT.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
                                        {
                                            child.GiaTri += ": ";
                                        }
                                    }
                                    else if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                    {
                                        child.GiaTri = $" {child.GiaTri}: ";
                                    }
                                    else
                                    {
                                        child.GiaTri = child.LoaiChiTiet.GenerateKeyTag();
                                    }

                                    parNT.AddStyleTextRange(child);
                                }
                            }
                        }
                    }
                }

                if (tableType == TableType.ThongTinNguoiKy)
                {
                    var tieuDes = cloneList.Where(x => (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiChuyenDoi ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiMua ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiBan ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiLapPhieu ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiNhanHang ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyThuKhoXuat ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiVanChuyen ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyThuKhoNhap ||
                                                        x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyThuTruongDonVi) && x.Checked == true)
                                            .OrderBy(x => x.STT).ToList();

                    var groupThongTinNguoiKys = new List<GroupThongTinNguoiKy>();

                    foreach (var item in tieuDes)
                    {
                        var children = new List<MauHoaDonTuyChinhChiTietViewModel>();

                        // add tieu de nguoi ky
                        item.IsTieuDeKy = true;
                        children.Add(item);

                        // add ky ten
                        string strLoaiChiTiet = Enum.GetName(typeof(LoaiChiTietTuyChonNoiDung), item.LoaiChiTiet);
                        var keyKyGhiRoBHoTen = "KyGhiRoHoTen" + strLoaiChiTiet.Substring(8);
                        var findKyGhiRoHoTen = cloneList.FirstOrDefault(x => Enum.GetName(typeof(LoaiChiTietTuyChonNoiDung), x.LoaiChiTiet) == keyKyGhiRoBHoTen);
                        if (findKyGhiRoHoTen != null)
                        {
                            findKyGhiRoHoTen.IsTieuDeKy = false;
                            children.Add(findKyGhiRoHoTen);
                        }

                        groupThongTinNguoiKys.Add(new GroupThongTinNguoiKy { Key = item.LoaiChiTiet, Children = children });
                    }

                    if (!(loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All))
                    {
                        groupThongTinNguoiKys = groupThongTinNguoiKys.Where(x => x.Key != LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiChuyenDoi).ToList();
                    }

                    var col = groupThongTinNguoiKys.Count;

                    table.ResetCells(1, col == 1 ? 2 : col);

                    if (col >= 4)
                    {
                        PreferredWidth width = new PreferredWidth(WidthType.Percentage, 100);
                        table.PreferredWidth = width;
                        for (int i = 0; i < col; i++)
                        {
                            if (i == (col - 1))
                            {
                                table.Rows[0].Cells[i].SetCellWidth(mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? 30 : 35, CellWidthType.Percentage);
                            }
                            else
                            {
                                table.Rows[0].Cells[i].SetCellWidth((mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet ? 70 : 65) / (col - 1), CellWidthType.Percentage);
                            }
                        }
                    }


                    for (int i = 0; i < col; i++)
                    {
                        TableCell cell;
                        if (col == 1)
                        {
                            cell = table.Rows[0].Cells[col];
                            cell.Paragraphs.Clear();
                        }
                        else
                        {
                            cell = table.Rows[0].Cells[i];
                            cell.Paragraphs.Clear();
                        }

                        var group = groupThongTinNguoiKys[i];
                        var countGroup = group.Children.Count;

                        for (int j = 0; j < countGroup; j++)
                        {
                            var item = group.Children[j];
                            var par = cell.AddParagraph();

                            if (item.IsTieuDeKy == true)
                            {
                                foreach (var child in item.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                    {
                                        child.GiaTri = " " + child.GiaTri;
                                    }

                                    par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                    par.AddStyleTextRange(child);
                                }
                            }
                            else
                            {
                                foreach (var child in item.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
                                    {
                                        par = cell.AddParagraph();
                                    }

                                    par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                    par.AddStyleTextRange(child);
                                }
                            }

                            if (j == (countGroup - 1))
                            {
                                switch (group.Key)
                                {
                                    case LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiChuyenDoi:
                                        cell.AddParagraph();
                                        cell.AddParagraph();
                                        cell.AddParagraph();

                                        par = cell.AddParagraph();
                                        par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                        par.Format.AfterSpacing = 0;
                                        TextRange convertorTextRange = par.AppendText("<convertor>");
                                        convertorTextRange.CharacterFormat.Bold = true;
                                        convertorTextRange.CharacterFormat.FontSize = 10;

                                        par = cell.AddParagraph();
                                        par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                        par.Format.AfterSpacing = 0;
                                        TextRange conversionDateTitleTextRange = par.AppendText("<conversionDateTitle>");
                                        conversionDateTitleTextRange.CharacterFormat.FontSize = 10;
                                        TextRange conversionDateValueTextRange = par.AppendText("<conversionDateValue>");
                                        conversionDateValueTextRange.CharacterFormat.Italic = true;
                                        conversionDateValueTextRange.CharacterFormat.FontSize = 10;
                                        break;
                                    case LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiMua:
                                    case LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiBan:
                                        par = cell.AddParagraph();
                                        par.Text = group.Key == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiMua ? "<digitalSignature_Buyer>" : "<digitalSignature>";
                                        par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                        break;
                                    case LoaiChiTietTuyChonNoiDung.TieuDeKyThuKhoNhap:
                                    case LoaiChiTietTuyChonNoiDung.TieuDeKyThuTruongDonVi:
                                        par = cell.AddParagraph();

                                        par.Text = "<digitalSignature>";
                                        par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                }

                if (tableType == TableType.ThongTinFooter)
                {
                    int count = 0;
                    switch (mauHoaDon.LoaiHoaDon)
                    {
                        case LoaiHoaDon.HoaDonGTGT:
                        case LoaiHoaDon.PXKKiemVanChuyenNoiBo:
                        case LoaiHoaDon.PXKHangGuiBanDaiLy:
                            count = (cloneList.Count == 5 || cloneList.Count == 3) ? 3 : 2;
                            if (count == 2)
                            {
                                table.Rows.RemoveAt(0);
                            }
                            break;
                        case LoaiHoaDon.HoaDonBanHang:
                            count = 3;
                            break;
                        default:
                            break;
                    }

                    if (count == 0)
                    {
                        return;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        TableCell tableCell = table.Rows[i].Cells[0];
                        Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                        MauHoaDonTuyChinhChiTietViewModel child = new MauHoaDonTuyChinhChiTietViewModel();
                        MauHoaDonTuyChinhChiTietViewModel childSN = new MauHoaDonTuyChinhChiTietViewModel();
                        if (i == 0)
                        {
                            switch (mauHoaDon.LoaiHoaDon)
                            {
                                case LoaiHoaDon.HoaDonGTGT:
                                case LoaiHoaDon.PXKKiemVanChuyenNoiBo:
                                case LoaiHoaDon.PXKHangGuiBanDaiLy:
                                    CreateTraCuuTaiPar(par, cloneList, mauHoaDon);

                                    CreateMaTraCuuPar(par, cloneList, mauHoaDon);
                                    break;
                                case LoaiHoaDon.HoaDonBanHang:
                                    CreateMaTraCuuPar(par, cloneList, mauHoaDon);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (i == 1)
                        {
                            switch (mauHoaDon.LoaiHoaDon)
                            {
                                case LoaiHoaDon.HoaDonGTGT:
                                case LoaiHoaDon.PXKKiemVanChuyenNoiBo:
                                case LoaiHoaDon.PXKHangGuiBanDaiLy:
                                    child = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CanKiemTraDoiChieu)
                                        .Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                                    par.AddStyleTextRange(child);

                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
                                    {
                                        child = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CanKiemTraDoiChieu)
                                            .Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);

                                        child.GiaTri = " " + child.GiaTri;

                                        par.AddStyleTextRange(child);
                                    }
                                    break;
                                case LoaiHoaDon.HoaDonBanHang:
                                    CreateTraCuuTaiPar(par, cloneList, mauHoaDon);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            child = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhatHanhBoi).Children[0];
                            par.AddStyleTextRange(child);
                        }
                    }
                }
            }
        }

        private static void ApplyStyleParHHDV(this Paragraph par)
        {
            TableCell cell = par.Owner as TableCell;
            cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            par.Format.LeftIndent = 1;
            par.Format.RightIndent = 1;
            par.Format.BeforeSpacing = 1;
        }

        private static void CreateTraCuuTaiPar(Paragraph par, List<MauHoaDonTuyChinhChiTietViewModel> cloneList, MauHoaDonViewModel mauHoaDon)
        {
            var child = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TraCuuTai)
                                        .Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);

            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
            {
                child.GiaTri += ": ";
            }

            par.AddStyleTextRange(child);

            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.SongNguVA)
            {
                child = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TraCuuTai)
                    .Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);

                child.GiaTri = $" {child.GiaTri}: ";

                par.AddStyleTextRange(child);
            }

            MauHoaDonTuyChinhChiTietViewModel childLinkTraCuu = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.LinkTraCuu).Children[0];
            childLinkTraCuu.GiaTri += " ";
            par.AddStyleTextRange(childLinkTraCuu, true);
        }

        private static void CreateMaTraCuuPar(Paragraph par, List<MauHoaDonTuyChinhChiTietViewModel> cloneList, MauHoaDonViewModel mauHoaDon)
        {
            MauHoaDonTuyChinhChiTietViewModel childMaTraCuu = cloneList
                                        .FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaTraCuu)
                                        .Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);

            if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet)
            {
                childMaTraCuu.GiaTri += $": {childMaTraCuu.LoaiChiTiet.GenerateKeyTag()}";
                par.AddStyleTextRange(childMaTraCuu);
            }
            else
            {
                par.AddStyleTextRange(childMaTraCuu);

                MauHoaDonTuyChinhChiTietViewModel childMaTraCuuSN = cloneList
                   .FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.MaTraCuu)
                   .Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);
                childMaTraCuuSN.GiaTri = $" {childMaTraCuuSN.GiaTri}: ";
                par.AddStyleTextRange(childMaTraCuuSN);

                childMaTraCuu.GiaTri = childMaTraCuu.LoaiChiTiet.GenerateKeyTag();
                par.AddStyleTextRange(childMaTraCuu);
            }
        }

        private static void AddRow(Table table, int srcRowIndex, int totalRow, int? destRowIndex = null)
        {
            for (int i = 0; i < totalRow; i++)
            {
                TableRow cl_row = table.Rows[srcRowIndex].Clone();
                table.Rows.Insert(destRowIndex ?? srcRowIndex, cl_row);
            }
        }

        private static void CreateTableMST(Document doc, Paragraph par, MauHoaDonTuyChinhChiTietViewModel itemTieuDe, MauHoaDonTuyChinhChiTietViewModel itemNoiDung, bool hasTitle)
        {
            string mst = itemNoiDung.GiaTri;

            var mstArray = new List<string>();

            for (int i = 0; i < mst.Length; i++)
            {
                mstArray.Add(mst[i].ToString());
            }

            if (hasTitle)
            {
                mstArray.Insert(0, itemTieuDe.GiaTri);
            }

            Table tblMST = doc.Sections[0].AddTable(true);
            var col = mstArray.Count();
            tblMST.ResetCells(1, col);
            for (int i = 0; i < col; i++)
            {
                var s = mstArray[i].ToString();

                if (i == 0 && hasTitle)
                {
                    var coChu = (int)itemTieuDe.TuyChonChiTiet.CoChu;
                    tblMST[0, i].Width = (s.Length * (coChu * 20 / 90f)) + s.Length + (coChu * 50 / 100);
                }
                else
                {
                    tblMST[0, i].Width = s == ":" ? 5 : 15;
                }
            }
            if (hasTitle)
            {
                Paragraph parMstTitle = tblMST[0, 0].AddParagraph();
                parMstTitle.Format.LeftIndent = -6F;
                tblMST[0, 0].CellFormat.Borders.BorderType = BorderStyle.Cleared;
                parMstTitle.AddStyleTextRange(itemTieuDe);
            }
            for (int i = 0; i < col; i++)
            {
                if (i == 0 && hasTitle)
                {
                    continue;
                }

                var tableCell = tblMST[0, i];

                Paragraph _par = tableCell.AddParagraph();
                itemNoiDung.GiaTri = mstArray[i].ToString();

                if (itemNoiDung.GiaTri == ":")
                {
                    tableCell.CellFormat.Borders.BorderType = BorderStyle.Cleared;
                    _par.Format.LeftIndent = -5F;
                }

                _par.AddStyleTextRange(itemNoiDung);
            }

            tblMST.AutoFit(AutoFitBehaviorType.AutoFitToContents);

            var body = par.Owner;
            int index = body.ChildObjects.IndexOf(par);
            body.ChildObjects.Remove(par);
            body.ChildObjects.Insert(index, tblMST);
        }

        public static string GenerateKeyTag(this LoaiChiTietTuyChonNoiDung type)
        {
            string result = Enum.GetName(typeof(LoaiChiTietTuyChonNoiDung), type);
            return $"<{result}>";
        }

        public static string GenerateKeyTagTongHopThueGTGT(this LoaiChiTietTuyChonNoiDung type, LoaiTongHopThueGTGT loaiTongHopThue)
        {
            string result = Enum.GetName(typeof(LoaiChiTietTuyChonNoiDung), type);
            string result2 = Enum.GetName(typeof(LoaiTongHopThueGTGT), loaiTongHopThue);
            return $"<{result}_{result2}>";
        }

        public static void SetPdfMargins(PdfDocument doc)
        {
            var pageNumbers = doc.Pages.Count;
            if (pageNumbers > 1)
            {
                //PdfUnitConvertor unitCvtr = new PdfUnitConvertor();
                //PdfMargins margin = new PdfMargins
                //{
                //    Top = unitCvtr.ConvertUnits(2.54f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point),
                //};
                //margin.Bottom = margin.Top;
                //margin.Left = unitCvtr.ConvertUnits(3.17f, PdfGraphicsUnit.Centimeter, PdfGraphicsUnit.Point);
                //margin.Right = margin.Left;
                //draw page number
                DrawPageNumber(doc.Pages);
            }
        }

        private static void DrawPageNumber(PdfPageCollection collection)
        {
            foreach (PdfPageBase page in collection)
            {
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Arial", 8f, FontStyle.Regular), true);
                PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Left);

                PdfPageNumberField pageNumber = new PdfPageNumberField();
                PdfPageCountField pageCount = new PdfPageCountField();

                int x = Convert.ToInt32(page.Canvas.ClientSize.Width - 40);
                int y = Convert.ToInt32(page.Canvas.ClientSize.Height - 10);

                PdfCompositeField pageNumberLabel = new PdfCompositeField
                {
                    AutomaticFields = new PdfAutomaticField[] { pageNumber, pageCount },
                    Brush = PdfBrushes.Black,
                    Font = font,
                    StringFormat = format,
                    Text = "Trang {0}/{1}"
                };
                pageNumberLabel.Draw(page.Canvas, x, y);
            }
        }

        private static void AddStyleTextRange(this Paragraph par, MauHoaDonTuyChinhChiTietViewModel item, bool isLink = false)
        {
            TextRange textRange = par.AppendText(item.GiaTri);
            par.Format.AfterSpacing = 0;
            textRange.CharacterFormat.Italic = item.TuyChonChiTiet.ChuNghieng.Value;
            textRange.CharacterFormat.Bold = item.TuyChonChiTiet.ChuDam.Value;
            textRange.CharacterFormat.FontSize = item.TuyChonChiTiet.CoChu.GetFontSize(item.LoaiContainer);
            textRange.CharacterFormat.TextColor = ColorTranslator.FromHtml(item.TuyChonChiTiet.MauChu);
            if (isLink)
            {
                textRange.CharacterFormat.TextColor = ColorTranslator.FromHtml("#5406ee");
                textRange.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
            }
            if (item.TuyChonChiTiet.CanChu.HasValue)
            {
                if (item.TuyChonChiTiet.CanChu == 2)
                {
                    textRange.OwnerParagraph.Format.HorizontalAlignment = HorizontalAlignment.Center;
                }
                else if (item.TuyChonChiTiet.CanChu == 3)
                {
                    textRange.OwnerParagraph.Format.HorizontalAlignment = HorizontalAlignment.Right;
                }
            }
            if (item.TuyChonChiTiet.CanChuDoc.HasValue)
            {
                TableCell cell = textRange.Owner.Owner as TableCell;

                if (item.TuyChonChiTiet.CanChuDoc == 2)
                {
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                }
                else if (item.TuyChonChiTiet.CanChuDoc == 3)
                {
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Bottom;
                }
            }
        }

        public static void SetValuePar(this Paragraph par, string value)
        {
            par.ChildObjects.Clear();
            par.AppendText(value ?? string.Empty);
        }

        public static void SetValuePar2(this Paragraph par, string value, LoaiChiTietTuyChonNoiDung detailType)
        {
            if (par.Text.Trim() == detailType.GenerateKeyTag())
            {
                par.ChildObjects.Clear();
                value = value ?? string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    if (detailType == LoaiChiTietTuyChonNoiDung.ThueSuatHHDV)
                    {
                        if (value != "KKKNT" && value != "KCT" && value != "\\" && value != "X")
                        {
                            if (value.Contains("KHAC"))
                            {
                                value = value.Split(":")[1].Replace(".", ",") + "%";
                            }
                            else
                            {
                                value += "%";
                            }
                        }
                    }
                    else if (detailType == LoaiChiTietTuyChonNoiDung.TyLeChietKhauHHDV)
                    {
                        value += "%";
                    }
                }
                par.AppendText(value ?? string.Empty);
            }
        }

        public static bool IsTagTyGiaHHDV(this Paragraph par)
        {
            return par.Text.Trim() == LoaiChiTietTuyChonNoiDung.TyGiaHHDV.GenerateKeyTag();
        }

        private static void AddStyleParagraph(this Paragraph par, Document doc, MauHoaDonTuyChinhChiTietViewModel item)
        {
            ParagraphStyle style = new ParagraphStyle(doc)
            {
                Name = $"FontStyle-{Guid.NewGuid()}"
            };
            style.CharacterFormat.Italic = item.TuyChonChiTiet.ChuNghieng.Value;
            style.CharacterFormat.Bold = item.TuyChonChiTiet.ChuDam.Value;
            style.CharacterFormat.FontSize = item.TuyChonChiTiet.CoChu.GetFontSize(item.LoaiContainer);
            style.CharacterFormat.TextColor = ColorTranslator.FromHtml(item.TuyChonChiTiet.MauChu);
            style.ParagraphFormat.AfterSpacing = 0;
            if (item.TuyChonChiTiet.CanChu.HasValue)
            {
                if (item.TuyChonChiTiet.CanChu == 2)
                {
                    style.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                }
                else if (item.TuyChonChiTiet.CanChu == 3)
                {
                    style.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
                }
            }
            if (item.TuyChonChiTiet.CanChuDoc.HasValue)
            {
                TableCell cell = par.Owner as TableCell;

                if (item.TuyChonChiTiet.CanChuDoc == 2)
                {
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                }
                else if (item.TuyChonChiTiet.CanChuDoc == 3)
                {
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Bottom;
                }
            }
            doc.Styles.Add(style);

            par.ApplyStyle(style.Name);
            par.AppendText(item.GiaTri);
        }

        private static void ApplyEmptyPar(this Paragraph par, Document doc)
        {
            ParagraphStyle style = new ParagraphStyle(doc)
            {
                Name = $"FontStyle-{Guid.NewGuid()}"
            };
            style.CharacterFormat.FontSize = 1;
            style.CharacterFormat.TextColor = ColorTranslator.FromHtml("#05FF00FF");
            style.ParagraphFormat.AfterSpacing = 0;

            doc.Styles.Add(style);

            par.ApplyStyle(style.Name);
            par.AppendText("");
        }

        private static int GetFontSize(this int? input, LoaiContainerTuyChinh loaiContainer)
        {
            int result = (int)Math.Round((input ?? 0) * 67 / 100D);

            if (loaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu)
            {
                result -= 1;
            }

            return result;
        }

        public enum LoaiTongHopThueGTGT
        {
            ThanhTienTruocThue,
            TienThue,
            CongTienThanhToan
        }

        private enum TableType
        {
            ThongTinNguoiBan,
            ThongTinHoaDon,
            ThongTinNguoiMua,
            ThongTinHangHoaDichVu,
            ThongTinNguoiKy,
            ThongTinFooter
        }

        /// <summary>
        /// Tạo trắng dữ liệu để preview
        /// </summary>
        public static FileReturn PreviewFilePDF(MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai, HoSoHDDTViewModel hoSoHDDT, IHostingEnvironment env, IHttpContextAccessor accessor)
        {
            Document doc = new Document(mauHoaDon.FilePath);
            CreatePreviewFileDoc(doc, mauHoaDon, accessor);
            string mauHoaDonImg = Path.Combine(env.WebRootPath, "images/template/mau.png");

            string folderPath = Path.Combine(env.WebRootPath, $"temp/preview_mau_hoa_don_{Guid.NewGuid()}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (mauHoaDon.NgayKy.HasValue == true)
            {
                ImageHelper.CreateSignatureBox(doc, hoSoHDDT.TenDonVi, mauHoaDon.LoaiNgonNgu, mauHoaDon.NgayKy);
            }
            else
            {
                ImageHelper.CreateEmptySignatureBox(doc, mauHoaDon.LoaiNgonNgu);
            }

            doc.Replace("<digitalSignature_Buyer>", string.Empty, true, true);

            //string docPath = Path.Combine(folderPath, $"doc_{DateTime.Now:HH-mm-ss}.docx");
            string pdfPath = Path.Combine(folderPath, $"{loai.GetTenFile()}.pdf");
            //doc.SaveToFile(docPath);
            doc.SaveToPDF(pdfPath, env, mauHoaDon.LoaiNgonNgu, !mauHoaDon.NgayKy.HasValue);

            PdfDocument pdfDoc = new PdfDocument();
            pdfDoc.LoadFromFile(pdfPath);
            PdfPageBase page = pdfDoc.Pages[0];
            PdfImage image = PdfImage.FromFile(mauHoaDonImg);
            float width = image.Width * 0.75f;
            float x = (page.Canvas.ClientSize.Width - width) / 2;
            float y = (page.Canvas.ClientSize.Height - 300) / 2;
            page.Canvas.DrawImage(image, x, y);
            pdfDoc.SaveToFile(pdfPath);

            byte[] bytes = File.ReadAllBytes(pdfPath);
            string base64 = Convert.ToBase64String(bytes);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            return new FileReturn
            {
                Bytes = bytes,
                ContentType = MimeTypes.GetMimeType(pdfPath),
                FileName = Path.GetFileName(pdfPath),
                Base64 = base64
            };
        }

        /// <summary>
        /// save doc to pdf attach greentick image
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pdfPath"></param>
        /// <param name="greentickPath"></param>
        /// <param name="loaiNgonNgu"></param>
        public static void SaveToPDF(this Document doc, string pdfPath, IHostingEnvironment env, LoaiNgonNgu loaiNgonNgu, bool isEmptySignature = false)
        {
            bool isSongNgu = loaiNgonNgu == LoaiNgonNgu.SongNguVA;
            string greentickPath = Path.Combine(env.WebRootPath, "images/template/greentick.png");

            doc.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);

            // load pdfDoc from path
            PdfDocument pdfDoc = new PdfDocument();
            pdfDoc.LoadFromFile(pdfPath);

            if (!isEmptySignature)
            {
                foreach (PdfPageBase page in pdfDoc.Pages)
                {
                    // find text to add signature greentick
                    PdfTextFind[] results = page.FindText("Signature Valid", TextFindParameter.WholeWord).Finds;
                    foreach (PdfTextFind text in results)
                    {
                        PointF p = text.Position;

                        //Draw the image
                        PdfImage image = PdfImage.FromFile(greentickPath);
                        float width = image.Width * 0.2f;
                        float height = image.Height * 0.2f;
                        page.Canvas.SetTransparency(0.8f);
                        page.Canvas.DrawImage(image, p.X + (isSongNgu ? 60 : 40), p.Y, width, height);
                    }
                }
            }

            // add page number footer
            SetPdfMargins(pdfDoc);

            pdfDoc.SaveToFile(pdfPath);

            doc.Dispose();
            pdfDoc.Dispose();
        }

        public static void CreatePreviewFileDoc(Document doc, MauHoaDonViewModel mauHoaDon, IHttpContextAccessor accessor)
        {
            string fullName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;

            #region Replace
            doc.Replace("<dd>", string.Empty, true, true);
            doc.Replace("<mm>", string.Empty, true, true);
            doc.Replace("<yyyy>", string.Empty, true, true);
            doc.Replace("<ddPXK>", string.Empty, true, true);
            doc.Replace("<mmPXK>", string.Empty, true, true);
            doc.Replace("<yyyyPXK>", string.Empty, true, true);
            doc.Replace("<reason>", string.Empty, true, true);

            List<string> wordKeys = Enum.GetValues(typeof(LoaiChiTietTuyChonNoiDung))
                .Cast<LoaiChiTietTuyChonNoiDung>()
                .Select(v => $"<{v}>")
                .ToList();

            foreach (var key in wordKeys)
            {
                if (key == GenerateKeyTag(LoaiChiTietTuyChonNoiDung.KyHieu))
                {
                    doc.Replace(key, mauHoaDon.KyHieu, true, true);
                }
                else if (key == GenerateKeyTag(LoaiChiTietTuyChonNoiDung.SoHoaDon))
                {
                    doc.Replace(key, !string.IsNullOrEmpty(mauHoaDon.KyHieu) ? "0" : string.Empty, true, true);
                }
                else
                {
                    doc.Replace(key, "<none-value>", true, true);
                }
            }

            List<LoaiChiTietTuyChonNoiDung> tongTienThues = Enum.GetValues(typeof(LoaiChiTietTuyChonNoiDung))
                .Cast<LoaiChiTietTuyChonNoiDung>()
                .Where(x => x >= LoaiChiTietTuyChonNoiDung.TongTienKhongKeKhaiThue && x <= LoaiChiTietTuyChonNoiDung.TongCongTongHopThueGTGT)
                .ToList();

            List<LoaiTongHopThueGTGT> loaiTongHopThues = Enum.GetValues(typeof(LoaiTongHopThueGTGT))
                .Cast<LoaiTongHopThueGTGT>()
                .ToList();

            foreach (var tongTien in tongTienThues)
            {
                foreach (var loaiTongHop in loaiTongHopThues)
                {
                    string key = tongTien.GenerateKeyTagTongHopThueGTGT(loaiTongHop);
                    doc.Replace(key, "<none-value>", true, true);
                }
            }

            doc.Replace("<none-value>", string.Empty, true, true);
            if (!string.IsNullOrEmpty(fullName))
            {
                doc.Replace("<convertor>", fullName, true, true);
            }
            doc.Replace("<conversionDateTitle>", "Ngày chuyển đổi: ", true, true);
            doc.Replace("<conversionDateValue>", DateTime.Now.ToString("dd/MM/yyyy"), true, true);
            #endregion
        }

        public static void FintTextInPDFAndReplaceIt(PdfDocument documents, Dictionary<string, string> dictionary)
        {
            foreach (var word in dictionary)
            {
                foreach (PdfPageBase page in documents.Pages)
                {
                    PdfTextFind[] result = page.FindText(word.Key, TextFindParameter.WholeWord).Finds;
                    foreach (PdfTextFind find in result)
                    {
                        //replace word in pdf                   
                        find.ApplyRecoverString(word.Value, Color.White, true);
                    }
                }
            }
        }

        public static void ClearKeyTag(this Document doc)
        {
            List<string> wordKeys = Enum.GetValues(typeof(LoaiChiTietTuyChonNoiDung))
                .Cast<LoaiChiTietTuyChonNoiDung>()
                .Select(v => $"<{v}>")
                .ToList();

            foreach (var key in wordKeys)
            {
                doc.Replace(key, "<none-value>", true, true);
            }

            doc.Replace("<none-value>", string.Empty, true, true);
        }

        public static void OptimizePDF(this string pdfPath)
        {
            PdfDocument doc = new PdfDocument(pdfPath);
            doc.FileInfo.IncrementalUpdate = false;
            doc.CompressionLevel = PdfCompressionLevel.Best;

            //Traverses all pages
            foreach (PdfPageBase page in doc.Pages)
            {
                //Extracts images from page
                Image[] images = page.ExtractImages();
                if (images != null && images.Length > 0)
                {
                    //Traverses all images
                    for (int j = 0; j < images.Length; j++)
                    {
                        Image image = images[j];
                        PdfBitmap bp = new PdfBitmap(image)
                        {
                            Quality = 20
                        };
                        page.ReplaceImage(j, bp);
                    }
                }
            }

            doc.SaveToFile(pdfPath);
            doc.Close();
        }

        public static void AddColumn(Table table, int columnIndex)
        {
            //Get the total grid span
            int gridSpan = 0;
            for (int i = 0; i < columnIndex; i++)
            {
                gridSpan += table.Rows[0].Cells[i].GridSpan;
            }
            for (int r = 0; r < table.Rows.Count; r++)
            {
                //Add a new cell
                TableCell addCell = new TableCell(table.Document);
                int currentGridSpan = 0;
                for (int i = 0; i < table.Rows[r].Cells.Count; i++)
                {
                    //Calculate the current total grid span
                    currentGridSpan += table.Rows[r].Cells[i].GridSpan;
                    if (currentGridSpan == gridSpan)
                    {
                        table.Rows[r].Cells.Insert(i + 1, addCell);
                        break;
                    }
                    if (currentGridSpan > gridSpan)
                    {
                        table.Rows[r].Cells.Insert(i, addCell);
                        break;
                    }
                }
            }
        }

        public static string GetTenFile(this HinhThucMauHoaDon hinhThucMauHoaDon)
        {
            string fileName;

            if (hinhThucMauHoaDon == HinhThucMauHoaDon.HoaDonMauCoBan)
            {
                fileName = "Hoa_don_mau_dang_the_hien";
            }
            else if (hinhThucMauHoaDon == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi)
            {
                fileName = "Hoa_don_mau_dang_chuyen_doi";
            }
            else if (hinhThucMauHoaDon == HinhThucMauHoaDon.HoaDonMauCoChietKhau)
            {
                fileName = "Hoa_don_mau_co_chiet_khau";
            }
            else
            {
                fileName = "Hoa_don_mau_ngoai_te";
            }

            return fileName;
        }
    }

    public class FileReturn
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string Base64 { get; set; }
        public string RefId { get; set; }
    }

    public class GroupThongTinNguoiKy
    {
        public LoaiChiTietTuyChonNoiDung Key { get; set; }
        public List<MauHoaDonTuyChinhChiTietViewModel> Children { get; set; }
    }

    public class GroupHeaderHHDVTable
    {
        public LoaiChiTietTuyChonNoiDung Key { get; set; }
        public MauHoaDonTuyChinhChiTietViewModel Item { get; set; }
        public int Span { get; set; }
        public int Index { get; set; }
    }

    public enum DinhDangTepMau
    {
        PDF,
        DOC,
        DOCX,
        XML
    }
}