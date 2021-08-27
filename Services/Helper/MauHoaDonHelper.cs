using DLL.Constants;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using Services.ViewModels.DanhMuc;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Services.Helper
{
    public static class MauHoaDonHelper
    {
        /// <summary>
        /// Tạo mẫu hóa đơn doc
        /// </summary>
        public static Document TaoMauHoaDonDoc(MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai, HoSoHDDTViewModel hoSoHDDT, IHostingEnvironment env, IHttpContextAccessor accessor, out int beginRow, bool hasReason = false)
        {
            string webRootPath = env.WebRootPath;
            string docPath = Path.Combine(webRootPath, $"docs/MauHoaDon/{mauHoaDon.TenBoMau}.docx");
            string qrcode = Path.Combine(webRootPath, $"images/template/qrcode.png");
            string databaseName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string backgroundEmtpy = Path.Combine(webRootPath, $"images/background/empty.jpg");

            #region Logo
            var logo = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.Logo);
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.MauHoaDon);

            #endregion

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
            string bgUploadPath = Path.Combine(webRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}/{mauHoaDon.MauHoaDonId}/FileAttach/{bgUpload.GiaTri}");
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
                    chuChimModel = JsonConvert.DeserializeObject<ChuChimModel>(DataHelper.Base64Decode(giaTriBoSungBgUploads[7]));
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
                Paragraph par = lastTableCell.Paragraphs.Count > 0 ? lastTableCell.Paragraphs[0] : lastTableCell.AddParagraph();
                DocPicture picQRCode = par.AppendPicture(Image.FromFile(qrcode));
                picQRCode.Width = 60;
                picQRCode.Height = 60;
                lastTableCell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                par.Format.HorizontalAlignment = HorizontalAlignment.Center;
            }

            if (logo != null)
            {
                string logoPath = Path.Combine(webRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}/{mauHoaDon.MauHoaDonId}/FileAttach/{logo.GiaTri}");
                var giaTriBoSungLogos = logo.GiaTriBoSung.Split(";");
                float topLogo = float.Parse(giaTriBoSungLogos[0], CultureInfo.InvariantCulture.NumberFormat);
                float leftLogo = float.Parse(giaTriBoSungLogos[1], CultureInfo.InvariantCulture.NumberFormat);
                float widthLogo = float.Parse(giaTriBoSungLogos[2], CultureInfo.InvariantCulture.NumberFormat);
                float heightLogo = float.Parse(giaTriBoSungLogos[3], CultureInfo.InvariantCulture.NumberFormat);
                float positionLogo = int.Parse(giaTriBoSungLogos[4]);
                if (tbl_nguoi_ban_first_page != null && !string.IsNullOrEmpty(logo.GiaTri) && File.Exists(logoPath))
                {
                    AddColumn(tbl_nguoi_ban_first_page, positionLogo == 1 ? 0 : 1);
                    Paragraph paraLogo = null;
                    if (positionLogo == 1)
                    {
                        tbl_nguoi_ban_first_page.ApplyVerticalMerge(0, 0, 4);
                        paraLogo = tbl_nguoi_ban_first_page[0, 0].AddParagraph();
                    }
                    else
                    {
                        tbl_nguoi_ban_first_page.ApplyVerticalMerge(1, 0, 4);
                        paraLogo = tbl_nguoi_ban_first_page[0, 1].AddParagraph();
                    }

                    Image logoImage = Image.FromFile(logoPath);
                    DocPicture picLogo = paraLogo.AppendPicture(logoImage);
                    picLogo.VerticalPosition = topLogo + ((100 / heightLogo) * 5);
                    picLogo.HorizontalPosition = leftLogo;
                    picLogo.Width = (widthLogo * 65) / 100;
                    picLogo.Height = (heightLogo * 65) / 100;
                    picLogo.TextWrappingStyle = TextWrappingStyle.Through;
                }
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
                    Paragraph par = lastTableCell.Paragraphs.Count > 0 ? lastTableCell.Paragraphs[0] : lastTableCell.AddParagraph();
                    DocPicture picQRCode = par.AppendPicture(Image.FromFile(qrcode));
                    picQRCode.Width = 60;
                    picQRCode.Height = 60;
                    lastTableCell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                }

                if (logo != null)
                {
                    string logoPath = Path.Combine(webRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}/{mauHoaDon.MauHoaDonId}/FileAttach/{logo.GiaTri}");
                    var giaTriBoSungLogos = logo.GiaTriBoSung.Split(";");
                    float topLogo = float.Parse(giaTriBoSungLogos[0], CultureInfo.InvariantCulture.NumberFormat);
                    float leftLogo = float.Parse(giaTriBoSungLogos[1], CultureInfo.InvariantCulture.NumberFormat);
                    float widthLogo = float.Parse(giaTriBoSungLogos[2], CultureInfo.InvariantCulture.NumberFormat);
                    float heightLogo = float.Parse(giaTriBoSungLogos[3], CultureInfo.InvariantCulture.NumberFormat);
                    float positionLogo = int.Parse(giaTriBoSungLogos[4]);

                    if (tbl_nguoi_ban != null && !string.IsNullOrEmpty(logo.GiaTri) && File.Exists(logoPath))
                    {
                        AddColumn(tbl_nguoi_ban, positionLogo == 1 ? 0 : 1);
                        Paragraph paraLogo = null;
                        if (positionLogo == 1)
                        {
                            tbl_nguoi_ban.ApplyVerticalMerge(0, 0, 4);
                            paraLogo = tbl_nguoi_ban[0, 0].AddParagraph();
                        }
                        else
                        {
                            tbl_nguoi_ban.ApplyVerticalMerge(1, 0, 4);
                            paraLogo = tbl_nguoi_ban[0, 1].AddParagraph();
                        }

                        Image logoImage = Image.FromFile(logoPath);
                        DocPicture picLogo = paraLogo.AppendPicture(logoImage);
                        picLogo.VerticalPosition = topLogo + ((100 / heightLogo) * 5);
                        picLogo.HorizontalPosition = leftLogo;
                        picLogo.Width = (widthLogo * 65) / 100;
                        picLogo.Height = (heightLogo * 65) / 100;
                        picLogo.TextWrappingStyle = TextWrappingStyle.Through;
                    }
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
                            style.CharacterFormat.TextColor = ColorTranslator.FromHtml(mauChu);
                            style.CharacterFormat.FontName = kieuChu;
                            style.CharacterFormat.FontSize = 10 + coChu;
                        }
                    }
                }
            }

            foreach (Table table in section.HeadersFooters.FirstPageHeader.Tables)
            {
                foreach (TableRow row in table.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Paragraph par in cell.Paragraphs)
                        {
                            foreach (DocumentObject docObject in par.ChildObjects)
                            {
                                if (docObject.DocumentObjectType == DocumentObjectType.TextRange)
                                {
                                    TextRange text = docObject as TextRange;

                                    if (text.Text == "<CompanyName>" || text.Text.ToUpper() == "HÓA ĐƠN GIÁ TRỊ GIA TĂNG")
                                    {
                                        text.CharacterFormat.FontSize = 13 + (coChu * 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //foreach (Table table in section.HeadersFooters.FirstPageFooter.Tables)
            //{
            //    foreach (TableRow row in table.Rows)
            //    {
            //        foreach (TableCell cell in row.Cells)
            //        {
            //            foreach (Paragraph par in cell.Paragraphs)
            //            {
            //                foreach (DocumentObject docObject in par.ChildObjects)
            //                {
            //                    if (docObject.DocumentObjectType == DocumentObjectType.TextRange)
            //                    {
            //                        TextRange text = docObject as TextRange;
            //                        text.CharacterFormat.FontSize = 9 + coChu;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            section.PageSetup.DifferentFirstPageHeaderFooter = false;
            foreach (Table table in section.HeadersFooters.Header.Tables)
            {
                foreach (TableRow row in table.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Paragraph par in cell.Paragraphs)
                        {
                            foreach (DocumentObject docObject in par.ChildObjects)
                            {
                                if (docObject.DocumentObjectType == DocumentObjectType.TextRange)
                                {
                                    TextRange text = docObject as TextRange;

                                    if (text.Text == "<CompanyName>" || text.Text.ToUpper() == "HÓA ĐƠN GIÁ TRỊ GIA TĂNG")
                                    {
                                        text.CharacterFormat.FontSize = 13 + (coChu * 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //foreach (Table table in section.HeadersFooters.Footer.Tables)
            //{
            //    foreach (TableRow row in table.Rows)
            //    {
            //        foreach (TableCell cell in row.Cells)
            //        {
            //            foreach (Paragraph par in cell.Paragraphs)
            //            {
            //                foreach (DocumentObject docObject in par.ChildObjects)
            //                {
            //                    if (docObject.DocumentObjectType == DocumentObjectType.TextRange)
            //                    {
            //                        TextRange text = docObject as TextRange;
            //                        text.CharacterFormat.FontSize = 9 + coChu;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            TextSelection[] txtLinkSearch = doc.FindAllString("<linkSearch>", false, true);
            if (txtLinkSearch != null && txtLinkSearch.Length > 0)
            {
                foreach (TextSelection seletion in txtLinkSearch)
                {
                    seletion.GetAsOneRange().CharacterFormat.TextColor = ColorTranslator.FromHtml("#5406ee");
                    seletion.GetAsOneRange().CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
                }
            }
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            #endregion

            #region set người bán
            List<string> soTKNH = new List<string>();
            if (!string.IsNullOrEmpty(hoSoHDDT.SoTaiKhoanNganHang))
            {
                soTKNH.Add(hoSoHDDT.SoTaiKhoanNganHang);
            }
            if (!string.IsNullOrEmpty(hoSoHDDT.TenNganHang))
            {
                soTKNH.Add(hoSoHDDT.TenNganHang);
            }

            doc.Replace("<CompanyName>", (hoSoHDDT.TenDonVi ?? string.Empty).ToUpper(), true, true);
            doc.Replace("<taxcode>", hoSoHDDT.MaSoThue ?? string.Empty, true, true);
            doc.Replace("<Address>", hoSoHDDT.DiaChi ?? string.Empty, true, true);
            doc.Replace("<Tel>", hoSoHDDT.SoDienThoaiLienHe ?? string.Empty, true, true);
            doc.Replace("<Banknumber>", string.Join(" - ", soTKNH), true, true);
            #endregion

            #region background
            Image bgDefault = Image.FromFile(backgroundEmtpy);
            Graphics g = Graphics.FromImage(bgDefault);
            if (!string.IsNullOrEmpty(bdDefaultPath) && File.Exists(bdDefaultPath))
            {
                Image borderDefault = null;
                if (isSVGBdDefalt == true)
                {
                    var svgDoc = SvgDocument.Open(bdDefaultPath);
                    svgDoc.Fill = new SvgColourServer(ColorTranslator.FromHtml(colorBdDefault));
                    borderDefault = svgDoc.Draw(860, 1220);
                }
                else
                {
                    borderDefault = Image.FromFile(bdDefaultPath);
                }

                g.DrawImage(borderDefault, 0, 0);
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
                    var svgDoc = SvgDocument.Open(bgDefaultPath);
                    svgDoc.Fill = new SvgColourServer(ColorTranslator.FromHtml(colorBgDefault));
                    backgroundDefault = svgDoc.Draw(500, 500);
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
                //soDongTrang = 50;
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

        public static Image ResizeImage(this Image image, float width, float height)
        {
            return new Bitmap(image, new Size((int)width, (int)height));
        }

        public static Image SetImageOpacity(this Image image, float opacity)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix();
                matrix.Matrix33 = opacity;
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
                        .Where(x => (x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon || x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon) && x.Checked == true)
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
                tbl_nguoi_ban_first_page.StyleDataTable(thongTinNguoiBans, TableType.ThongTinNguoiBan, mauHoaDon, loai);
                tbl_tieu_de_first_page.StyleDataTable(thongTinHoaDon_mauSoKyHieuSoHoaDon, TableType.ThongTinHoaDon, mauHoaDon, loai);
                tbl_nguoi_mua_first_page.StyleDataTable(thongTinNguoiMuas, TableType.ThongTinNguoiMua, mauHoaDon, loai);

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
                tbl_nguoi_ban.StyleDataTable(thongTinNguoiBans, TableType.ThongTinNguoiBan, mauHoaDon, loai);
                tbl_tieu_de.StyleDataTable(thongTinHoaDon_mauSoKyHieuSoHoaDon, TableType.ThongTinHoaDon, mauHoaDon, loai);
                tbl_nguoi_mua.StyleDataTable(thongTinNguoiMuas, TableType.ThongTinNguoiMua, mauHoaDon, loai);
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
                tbl_hhdv.StyleDataTable(thongTinHangHoaDichVus, TableType.ThongTinHangHoaDichVu, mauHoaDon, loai);
                tbl_nguoi_ky.StyleDataTable(thongTinNguoiKys, TableType.ThongTinNguoiKy, mauHoaDon, loai);

                foreach (Table tb in section.HeadersFooters.FirstPageFooter.Tables)
                {
                    if (tb.Title == "tbl_footer")
                    {
                        tbl_footer_first_page = tb;
                    }
                }
                tbl_footer_first_page.StyleDataTable(thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);

                section.PageSetup.DifferentFirstPageHeaderFooter = false;
                foreach (Table tb in section.HeadersFooters.Footer.Tables)
                {
                    if (tb.Title == "tbl_footer")
                    {
                        tbl_footer = tb;
                    }
                }
                tbl_footer.StyleDataTable(thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);
                section.PageSetup.DifferentFirstPageHeaderFooter = true;
            }
        }

        private static void StyleDataTable(this Table table, List<MauHoaDonTuyChinhChiTietViewModel> list, TableType tableType, MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai)
        {
            List<MauHoaDonTuyChinhChiTietViewModel> cloneList = CloneHelper.DeepClone(list);

            if (table != null)
            {
                if (tableType == TableType.ThongTinNguoiBan)
                {
                    int canTieuDe = cloneList.SelectMany(x => x.Children).FirstOrDefault().TuyChonChiTiet.CanTieuDe.Value;
                    int idxTenDonViNguoiBan = cloneList.FindIndex(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan);
                    int row = cloneList.Count();
                    int col = canTieuDe > 1 ? 2 : 1;

                    table.Rows[0].Cells[0].SplitCell(col, row);

                    if (canTieuDe > 1)
                    {
                        PreferredWidth width = new PreferredWidth(WidthType.Percentage, 100);
                        table.PreferredWidth = width;
                        int doRong = cloneList.SelectMany(x => x.Children).Max(x => x.TuyChonChiTiet.DoRong ?? 0);

                        for (int i = 0; i < row; i++)
                        {
                            table.Rows[i].Cells[0].SetCellWidth(doRong * 68 / 674, CellWidthType.Percentage);
                        }
                    }

                    table.ApplyHorizontalMerge(idxTenDonViNguoiBan, 0, col - 1);

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = cloneList[i];

                        for (int j = 0; j < tableRow.Cells.Count; j++)
                        {
                            TableCell tableCell = tableRow.Cells[j];
                            Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                            if (col == 1)
                            {
                                foreach (var child in item.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        child.GiaTri += (idxTenDonViNguoiBan == i) ? "" : ": ";
                                    }

                                    par.AddStyleTextRange(child);
                                }
                            }
                            else
                            {
                                MauHoaDonTuyChinhChiTietViewModel child = new MauHoaDonTuyChinhChiTietViewModel();
                                if (idxTenDonViNguoiBan == i)
                                {
                                    child = item.Children[0];
                                    par.Format.AfterSpacing = 0;
                                }
                                else
                                {
                                    child = item.Children[j];
                                }
                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                {
                                    child.GiaTri += (canTieuDe == 2 ? ": " : "");
                                }
                                else if (child.LoaiContainer == LoaiContainerTuyChinh.NoiDung)
                                {
                                    child.GiaTri = (canTieuDe == 2 ? "" : (idxTenDonViNguoiBan == i ? "" : ": ")) + child.GiaTri;
                                }

                                par.AddStyleTextRange(child);
                            }
                        }
                    }

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                }

                if (tableType == TableType.ThongTinHoaDon)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinHoaDon = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon && (x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon || (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon && !string.IsNullOrEmpty(x.Children[0].GiaTri)))).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listMSKHSHD = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon).ToList();
                    int row = listThongTinHoaDon.Count;

                    if (row > 3)
                    {
                        TableRow cl_row = table.Rows[row - 2].Clone();
                        table.Rows.Insert(row - 2, cl_row);
                    }

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = listThongTinHoaDon[i];

                        TableCell tableCell = tableRow.Cells[1];
                        Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                        MauHoaDonTuyChinhChiTietViewModel child = item.Children[0];
                        if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayThangNamTieuDe)
                        {
                            child.GiaTri = "Ngày <dd> tháng <mm> năm <yyyy>";
                        }
                        par.AddStyleTextRange(child);
                    }
                    for (int i = 0; i < listMSKHSHD.Count; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = listMSKHSHD[i];

                        for (int j = 2; j <= 3; j++)
                        {
                            TableCell tableCell = tableRow.Cells[j];
                            Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                            MauHoaDonTuyChinhChiTietViewModel child = item.Children[j == 2 ? 0 : 1];
                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                            {
                                child.GiaTri += ": ";
                            }
                            else
                            {
                                child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                            }
                            par.AddStyleTextRange(child);
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
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinChung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.HinhThucThanhToan && x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua && x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.CustomNguoiMua).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listHHTT_STK = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.HinhThucThanhToan || x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTaiKhoanNguoiMua)).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listBoSung = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CustomNguoiMua).ToList();

                    int canTieuDe = listThongTinChung.SelectMany(x => x.Children).FirstOrDefault().TuyChonChiTiet.CanTieuDe.Value;
                    int row = cloneList.Count() + (listHHTT_STK.Count == 2 ? (-1) : 0);
                    int col = canTieuDe > 1 ? 2 : 1;
                    if (listHHTT_STK.Count == 2)
                    {
                        col += 1;
                    }

                    table.Rows[0].Cells[0].SplitCell(col, row);

                    if (canTieuDe > 1)
                    {
                        PreferredWidth width = new PreferredWidth(WidthType.Percentage, 100);
                        table.PreferredWidth = width;
                        int doRong = listThongTinChung.SelectMany(x => x.Children).Max(x => x.TuyChonChiTiet.DoRong ?? 0);

                        for (int i = 0; i < row; i++)
                        {
                            table.Rows[i].Cells[0].SetCellWidth(doRong * 110 / 674, CellWidthType.Percentage);

                            if (listHHTT_STK.Count == 2)
                            {
                                table.Rows[i].Cells[col - 1].SetCellWidth(50, CellWidthType.Percentage);
                            }
                        }
                    }

                    for (int i = 0; i < listThongTinChung.Count; i++)
                    {
                        table.ApplyHorizontalMerge(i, canTieuDe > 1 ? 1 : 0, col - 1);
                    }

                    for (int i = 0; i < listThongTinChung.Count; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = listThongTinChung[i];

                        for (int j = 0; j < col; j++)
                        {
                            TableCell tableCell = tableRow.Cells[j];
                            Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                            if (canTieuDe == 1)
                            {
                                if (j == 0)
                                {
                                    foreach (var child in item.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                        {
                                            child.GiaTri += ": ";
                                        }
                                        else
                                        {
                                            child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                        }

                                        par.AddStyleTextRange(child);
                                    }
                                }
                            }
                            else
                            {
                                if (j < 2)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = new MauHoaDonTuyChinhChiTietViewModel();
                                    child = item.Children[j];
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        child.GiaTri += (canTieuDe == 2 ? ": " : "");
                                    }
                                    else if (child.LoaiContainer == LoaiContainerTuyChinh.NoiDung)
                                    {
                                        child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                        child.GiaTri = (canTieuDe == 2 ? "" : ": ") + child.GiaTri;
                                    }

                                    par.AddStyleTextRange(child);
                                }
                            }
                        }
                    }

                    if (listHHTT_STK.Any())
                    {
                        TableRow tableRow = table.Rows[4];
                        if (canTieuDe == 1)
                        {
                            for (int i = 0; i < listHHTT_STK.Count; i++)
                            {
                                TableCell tableCell = tableRow.Cells[i];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                MauHoaDonTuyChinhChiTietViewModel item = listHHTT_STK[i];
                                foreach (var child in item.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        child.GiaTri += ": ";
                                    }
                                    else
                                    {
                                        child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                    }

                                    par.AddStyleTextRange(child);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < col; i++)
                            {
                                TableCell tableCell = tableRow.Cells[i];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                if (i > 1)
                                {
                                    MauHoaDonTuyChinhChiTietViewModel item = listHHTT_STK[1];
                                    foreach (var child in item.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                        {
                                            child.GiaTri += ": ";
                                        }
                                        else
                                        {
                                            child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                        }

                                        par.AddStyleTextRange(child);
                                    }
                                }
                                else
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = listHHTT_STK[0].Children[i];
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        child.GiaTri += (canTieuDe == 2 ? ": " : "");
                                    }
                                    else if (child.LoaiContainer == LoaiContainerTuyChinh.NoiDung)
                                    {
                                        child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                        child.GiaTri = (canTieuDe == 2 ? "" : ": ") + child.GiaTri;
                                    }

                                    par.AddStyleTextRange(child);
                                }
                            }
                        }
                    }

                    AddColumn(table, col);
                    table.ApplyVerticalMerge(col, 0, row - 1);
                    table.Rows[0].Cells[col].SetCellWidth(15, CellWidthType.Percentage);

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                }

                if (tableType == TableType.ThongTinHangHoaDichVu)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listHangHoaDichVu = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinVeHangHoaDichVu).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listTongGiaTriHHDV = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinVeTongGiaTriHHDV).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listNgoaiTe = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNgoaiTe).ToList();

                    int col = listHangHoaDichVu.Count();
                    int row = 5;
                    bool isThietLapDongKyHieuCot = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.ThietLapDongKyHieuCot).GiaTri == "true";
                    if (isThietLapDongKyHieuCot)
                    {
                        row += 1;
                    }

                    table.Rows[0].Cells[0].SplitCell(col, row + 4);

                    PreferredWidth width = new PreferredWidth(WidthType.Percentage, 100);
                    table.PreferredWidth = width;

                    int leftTotalCol = 0;
                    int? idxToMergeThongTinTongTien = null;
                    List<int> listColWidth = new List<int>();
                    for (int i = 0; i < listHangHoaDichVu.Count; i++)
                    {
                        int doRong = listHangHoaDichVu[i].Children[0].TuyChonChiTiet.DoRong ?? 0;
                        listColWidth.Add(doRong);
                    }

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        for (int j = 0; j < listColWidth.Count; j++)
                        {
                            leftTotalCol += listColWidth[j];
                            table.Rows[i].Cells[j].SetCellWidth(listColWidth[j], CellWidthType.Percentage);

                            if (leftTotalCol > 40 && idxToMergeThongTinTongTien == null)
                            {
                                idxToMergeThongTinTongTien = j;
                            }
                        }
                    }

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];

                        for (int j = 0; j < col; j++)
                        {
                            TableCell tableCell = tableRow.Cells[j];

                            if (i == 0)
                            {
                                MauHoaDonTuyChinhChiTietViewModel child = listHangHoaDichVu[j].Children[0];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                                par.AddStyleTextRange(child);

                                if (!string.IsNullOrEmpty(child.TuyChonChiTiet.MauNenTieuDeBang))
                                {
                                    tableCell.CellFormat.BackColor = ColorTranslator.FromHtml(child.TuyChonChiTiet.MauNenTieuDeBang);
                                }
                            }
                            else if (isThietLapDongKyHieuCot == true && i == 1)
                            {
                                MauHoaDonTuyChinhChiTietViewModel child = listHangHoaDichVu[j].Children[2];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                                par.AddStyleTextRange(child);
                            }
                            else
                            {
                                if ((isThietLapDongKyHieuCot == false && i >= 1) || (isThietLapDongKyHieuCot == true && i >= 2))
                                {
                                    MauHoaDonTuyChinhChiTietViewModel child = listHangHoaDichVu[j].Children[1];
                                    Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                                    par.AddStyleTextRange(child);
                                }
                            }
                        }
                    }

                    table.Rows[0].IsHeader = true;
                    if (isThietLapDongKyHieuCot == true)
                    {
                        table.Rows[1].IsHeader = true;
                    }

                    if (idxToMergeThongTinTongTien.HasValue)
                    {
                        for (int i = row; i < table.Rows.Count; i++)
                        {
                            TableRow tableRow = table.Rows[i];

                            if (i == (table.Rows.Count - 1))
                            {
                                table.ApplyHorizontalMerge(i, 0, col - 1);
                                Paragraph par = tableRow.Cells[0].Paragraphs.Count > 0 ? tableRow.Cells[0].Paragraphs[0] : tableRow.Cells[0].AddParagraph();
                                MauHoaDonTuyChinhChiTietViewModel item = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienBangChu);
                                foreach (var child in item.Children)
                                {
                                    if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                    {
                                        child.GiaTri += ": ";
                                    }
                                    else
                                    {
                                        child.GiaTri = child.LoaiChiTiet.GenerateWordKey() + ".";
                                    }

                                    par.AddStyleTextRange(child);
                                }
                            }
                            else
                            {
                                table.ApplyHorizontalMerge(i, 0, idxToMergeThongTinTongTien.Value);
                                Paragraph par = tableRow.Cells[0].Paragraphs.Count > 0 ? tableRow.Cells[0].Paragraphs[0] : tableRow.Cells[0].AddParagraph();
                                tableRow.Cells[0].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;

                                MauHoaDonTuyChinhChiTietViewModel itemLeft = null;
                                MauHoaDonTuyChinhChiTietViewModel itemRight = null;
                                if (i == row)
                                {
                                    par.Format.AfterSpacing = 0;
                                    itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.CongTienHang);
                                }
                                else if (i == (row + 1))
                                {
                                    itemLeft = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.ThueSuatGTGT);
                                    itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TienThueGTGT);
                                }
                                else if (i == (row + 2))
                                {
                                    par.Format.AfterSpacing = 0;
                                    itemRight = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TongTienThanhToan);
                                }

                                if (itemLeft != null)
                                {
                                    foreach (var child in itemLeft.Children)
                                    {
                                        if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                        {
                                            child.GiaTri += ": ";
                                        }
                                        else
                                        {
                                            child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                        }

                                        par.AddStyleTextRange(child);
                                    }
                                }

                                if (itemRight != null)
                                {
                                    if ((idxToMergeThongTinTongTien + 1) == (col - 1))
                                    {
                                        foreach (var child in itemRight.Children)
                                        {
                                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                            {
                                                child.GiaTri += ": ";
                                            }
                                            else
                                            {
                                                child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                                par.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                            }

                                            par.AddStyleTextRange(child);
                                        }
                                    }
                                    else
                                    {
                                        table.ApplyHorizontalMerge(i, idxToMergeThongTinTongTien.Value + 1, col - 2);
                                        int celIdx1 = idxToMergeThongTinTongTien.Value + 1;
                                        int celIdx2 = col - 1;
                                        Paragraph par1 = tableRow.Cells[celIdx1].Paragraphs.Count > 0 ? tableRow.Cells[celIdx1].Paragraphs[0] : tableRow.Cells[celIdx1].AddParagraph();
                                        MauHoaDonTuyChinhChiTietViewModel child1 = itemRight.Children[0];
                                        child1.GiaTri += ": ";
                                        par1.AddStyleTextRange(child1);
                                        tableRow.Cells[celIdx1].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                        tableRow.Cells[celIdx1].CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;

                                        Paragraph par2 = tableRow.Cells[celIdx2].Paragraphs.Count > 0 ? tableRow.Cells[celIdx2].Paragraphs[0] : tableRow.Cells[celIdx2].AddParagraph();
                                        MauHoaDonTuyChinhChiTietViewModel child2 = itemRight.Children[1];
                                        child2.GiaTri = child2.LoaiChiTiet.GenerateWordKey();
                                        par2.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                        par2.AddStyleTextRange(child2);
                                        tableRow.Cells[celIdx2].CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                                    }
                                }
                            }
                        }
                    }

                    if (loai == HinhThucMauHoaDon.HoaDonMauCoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_CoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
                    {
                        TableRow cl_row = table.Rows[row].Clone();

                        #region tỷ lệ chiết khấu
                        cl_row.Cells[0].Paragraphs.Clear();
                        Paragraph parTyLeChietKhau = cl_row.Cells[0].AddParagraph();
                        MauHoaDonTuyChinhChiTietViewModel itemTyLeChietKhau = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyLeChietKhau);
                        foreach (var child in itemTyLeChietKhau.Children)
                        {
                            if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                            {
                                child.GiaTri += ": ";
                            }
                            else
                            {
                                child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                            }

                            parTyLeChietKhau.AddStyleTextRange(child);
                        }
                        #endregion

                        #region số tiền chiết khấu
                        MauHoaDonTuyChinhChiTietViewModel itemSoTienChietKhau = listTongGiaTriHHDV.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.SoTienChietKhau);
                        if ((idxToMergeThongTinTongTien + 1) == (col - 1))
                        {
                            cl_row.Cells[col - 1].Paragraphs.Clear();
                            Paragraph parSoTienchietKhau = cl_row.Cells[col - 1].AddParagraph();

                            foreach (var child in itemSoTienChietKhau.Children)
                            {
                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                {
                                    child.GiaTri += ": ";
                                }
                                else
                                {
                                    child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                    parSoTienchietKhau.Format.HorizontalAlignment = HorizontalAlignment.Right;
                                }

                                parSoTienchietKhau.AddStyleTextRange(child);
                            }
                        }
                        else
                        {
                            int celIdx1 = idxToMergeThongTinTongTien.Value + 1;
                            int celIdx2 = col - 1;
                            cl_row.Cells[celIdx1].Paragraphs.Clear();
                            cl_row.Cells[celIdx2].Paragraphs.Clear();

                            Paragraph par1 = cl_row.Cells[celIdx1].AddParagraph();
                            MauHoaDonTuyChinhChiTietViewModel child1 = itemSoTienChietKhau.Children[0];
                            child1.GiaTri += ": ";
                            par1.AddStyleTextRange(child1);

                            Paragraph par2 = cl_row.Cells[celIdx2].AddParagraph();
                            MauHoaDonTuyChinhChiTietViewModel child2 = itemSoTienChietKhau.Children[1];
                            child2.GiaTri = child2.LoaiChiTiet.GenerateWordKey();
                            par2.AddStyleTextRange(child2);
                        }
                        #endregion

                        table.Rows.Insert(row, cl_row);
                    }

                    if (loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TableRow cl_row = table.Rows[table.Rows.Count - 1].Clone();
                            table.Rows.Insert(table.Rows.Count - 1, cl_row);
                        }

                        for (int i = table.Rows.Count - 2; i <= table.Rows.Count - 1; i++)
                        {
                            TableCell tableCell = table.Rows[i].Cells[0];
                            tableCell.CellFormat.Borders.BorderType = BorderStyle.Cleared;

                            tableCell.Paragraphs.Clear();
                            Paragraph par = tableCell.AddParagraph();
                            MauHoaDonTuyChinhChiTietViewModel item = new MauHoaDonTuyChinhChiTietViewModel();
                            if (i == table.Rows.Count - 2)
                            {
                                par.Format.BeforeSpacing = 5;
                                item = listNgoaiTe.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TyGia);
                            }
                            else
                            {
                                item = listNgoaiTe.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.QuyDoi);
                            }

                            foreach (var child in item.Children)
                            {
                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                {
                                    child.GiaTri += ": ";
                                }
                                else
                                {
                                    child.GiaTri = child.LoaiChiTiet.GenerateWordKey();
                                }

                                par.AddStyleTextRange(child);
                            }
                        }
                    }
                }

                if (tableType == TableType.ThongTinNguoiKy)
                {
                    bool hasChuyenDoi = false;
                    if (loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe ||
                        loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
                    {
                        PreferredWidth width = new PreferredWidth(WidthType.Percentage, 100);
                        table.PreferredWidth = width;
                        table.Rows[0].Cells[0].SetCellWidth(33, CellWidthType.Percentage);
                        table.Rows[0].Cells[1].SetCellWidth(34, CellWidthType.Percentage);
                        table.Rows[0].Cells[2].SetCellWidth(33, CellWidthType.Percentage);
                        hasChuyenDoi = true;
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        TableCell tableCell = table.Rows[0].Cells[i];
                        var paragraphs = tableCell.Paragraphs;
                        foreach (Paragraph par in paragraphs)
                        {
                            switch (par.Text)
                            {
                                case "<signNameTitle1>":
                                    MauHoaDonTuyChinhChiTietViewModel itemTieuDeKyTrai = cloneList.FirstOrDefault(x => x.LoaiChiTiet == (hasChuyenDoi == true ? LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiChuyenDoi : LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiMua)).Children[0];
                                    par.ChildObjects.Clear();
                                    par.AddStyleTextRange(itemTieuDeKyTrai);
                                    break;
                                case "<signNameSubTitle1>":
                                    MauHoaDonTuyChinhChiTietViewModel itemKyGhiRoHoTenTrai = cloneList.FirstOrDefault(x => x.LoaiChiTiet == (hasChuyenDoi == true ? LoaiChiTietTuyChonNoiDung.KyGhiRoHoTenNguoiChuyenDoi : LoaiChiTietTuyChonNoiDung.KyGhiRoHoTenNguoiMua)).Children[0];
                                    par.ChildObjects.Clear();
                                    par.AddStyleTextRange(itemKyGhiRoHoTenTrai);
                                    break;
                                case "<convertor>":
                                    if (hasChuyenDoi != true)
                                    {
                                        par.ChildObjects.Clear();
                                    }
                                    break;
                                case "<conversionDate>":
                                    if (hasChuyenDoi != true)
                                    {
                                        par.ChildObjects.Clear();
                                    }
                                    break;
                                case "<signNameTitle2>":
                                    if (hasChuyenDoi == true)
                                    {
                                        MauHoaDonTuyChinhChiTietViewModel itemTieuDeKyGiua = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiMua).Children[0];
                                        par.ChildObjects.Clear();
                                        par.AddStyleTextRange(itemTieuDeKyGiua);
                                    }
                                    else
                                    {
                                        par.ChildObjects.Clear();
                                    }
                                    break;
                                case "<signNameSubTitle2>":
                                    if (hasChuyenDoi == true)
                                    {
                                        MauHoaDonTuyChinhChiTietViewModel itemKyGhiRoHoTenGiua = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.KyGhiRoHoTenNguoiMua).Children[0];
                                        par.ChildObjects.Clear();
                                        par.AddStyleTextRange(itemKyGhiRoHoTenGiua);
                                    }
                                    else
                                    {
                                        par.ChildObjects.Clear();
                                    }
                                    break;
                                case "<signNameTitle3>":
                                    MauHoaDonTuyChinhChiTietViewModel itemTieuDeKyPhai = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TieuDeKyNguoiBan).Children[0];
                                    par.ChildObjects.Clear();
                                    par.AddStyleTextRange(itemTieuDeKyPhai);
                                    break;
                                case "<signNameSubTitle3>":
                                    MauHoaDonTuyChinhChiTietViewModel itemKyGhiRoHoTenPhai = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.KyGhiRoHoTenNguoiBan).Children[0];
                                    par.ChildObjects.Clear();
                                    par.AddStyleTextRange(itemKyGhiRoHoTenPhai);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (tableType == TableType.ThongTinFooter)
                {
                    int count = cloneList.Count;
                    if (count == 2)
                    {
                        table.Rows.RemoveAt(0);
                    }

                    for (int i = 0; i < count; i++)
                    {
                        TableCell tableCell = table.Rows[i].Cells[0];
                        Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                        MauHoaDonTuyChinhChiTietViewModel child = cloneList[i].Children[0];

                        if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TraCuuTai)
                        {
                            string textValue = child.GiaTri;
                            int firstIndexOfTwoDot = textValue.IndexOf(": ");
                            int lastIndexOfMinus = textValue.LastIndexOf(" - ");
                            int countText = child.GiaTri.Length;
                            List<string> texts = new List<string>();
                            texts.Add(textValue.Substring(0, firstIndexOfTwoDot + 2));
                            texts.Add(textValue.Substring(firstIndexOfTwoDot + 2, lastIndexOfMinus - 2 - firstIndexOfTwoDot));
                            texts.Add(textValue.Substring(lastIndexOfMinus, countText - 1 - lastIndexOfMinus));

                            for (int j = 0; j < texts.Count; j++)
                            {
                                child.GiaTri = texts[j];
                                par.AddStyleTextRange(child, j == 1);
                            }
                        }
                        else
                        {
                            par.AddStyleTextRange(child);
                        }

                    }
                }
            }
        }

        public static string GenerateWordKey(this LoaiChiTietTuyChonNoiDung type)
        {
            string result = Enum.GetName(typeof(LoaiChiTietTuyChonNoiDung), type);
            return $"<{result}>";
        }

        private static void AddStyleTextRange(this Paragraph par, MauHoaDonTuyChinhChiTietViewModel item, bool isLink = false)
        {
            TextRange textRange = par.AppendText(item.GiaTri);
            par.Format.AfterSpacing = 0;
            textRange.CharacterFormat.Italic = item.TuyChonChiTiet.ChuNghieng.Value;
            textRange.CharacterFormat.Bold = item.TuyChonChiTiet.ChuDam.Value;
            textRange.CharacterFormat.FontSize = item.TuyChonChiTiet.CoChu.GetFontSize();
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
        }

        private static int GetFontSize(this int? input)
        {
            int result = (int)Math.Round((input ?? 0) * 70 / 100D);
            return result;
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
            Document doc = TaoMauHoaDonDoc(mauHoaDon, loai, hoSoHDDT, env, accessor, out _);
            CreatePreviewFileDoc(doc, mauHoaDon, accessor);
            string mauHoaDonImg = Path.Combine(env.WebRootPath, "images/template/mau.png");

            string folderPath = Path.Combine(env.WebRootPath, $"temp/preview_mau_hoa_don_{Guid.NewGuid()}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //string docPath = Path.Combine(folderPath, $"doc_{DateTime.Now:HH-mm-ss}.docx");
            string pdfPath = Path.Combine(folderPath, $"{loai.GetTenFile()}.pdf");
            //doc.SaveToFile(docPath);
            doc.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);

            PdfDocument pdfDoc = new PdfDocument();
            pdfDoc.LoadFromFile(pdfPath);
            PdfPageBase page = pdfDoc.Pages[0];
            PdfImage image = PdfImage.FromFile(mauHoaDonImg);
            float width = image.Width * 0.75f;
            float x = (page.Canvas.ClientSize.Width - width) / 2;
            float y = (page.Canvas.ClientSize.Height - 300) / 2;
            page.Canvas.DrawImage(image, x, y);
            pdfDoc.SaveToFile(pdfPath);

            if (mauHoaDon.NgayKy.HasValue == true)
            {
                USBTokenSign uSBTokenSign = new USBTokenSign(new HoSoHDDTViewModel
                {
                    MaSoThue = hoSoHDDT.MaSoThue,
                    TenDonVi = hoSoHDDT.TenDonVi,
                    DiaChi = hoSoHDDT.DiaChi,
                    SoDienThoaiLienHe = hoSoHDDT.SoDienThoaiLienHe
                }, env);
                uSBTokenSign.DigitalSignaturePDF(pdfPath, mauHoaDon.NgayKy.Value);
            }

            byte[] bytes = File.ReadAllBytes(pdfPath);
            Directory.Delete(folderPath, true);

            return new FileReturn
            {
                Bytes = bytes,
                ContentType = MimeTypes.GetMimeType(pdfPath),
                FileName = Path.GetFileName(pdfPath)
            };
        }

        public static void CreatePreviewFileDoc(Document doc, MauHoaDonViewModel mauHoaDon, IHttpContextAccessor accessor)
        {
            string fullName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;

            #region Replace
            doc.Replace("<dd>", string.Empty, true, true);
            doc.Replace("<mm>", string.Empty, true, true);
            doc.Replace("<yyyy>", string.Empty, true, true);
            doc.Replace("<reason>", string.Empty, true, true);

            List<string> wordKeys = Enum.GetValues(typeof(LoaiChiTietTuyChonNoiDung))
                .Cast<LoaiChiTietTuyChonNoiDung>()
                .Select(v => $"<{v}>")
                .ToList();

            foreach (var key in wordKeys)
            {
                if (key == GenerateWordKey(LoaiChiTietTuyChonNoiDung.MauSo))
                {
                    doc.Replace(key, mauHoaDon.MauSo, true, true);
                }
                else if (key == GenerateWordKey(LoaiChiTietTuyChonNoiDung.KyHieu))
                {
                    doc.Replace(key, mauHoaDon.KyHieu, true, true);
                }
                else if (key == GenerateWordKey(LoaiChiTietTuyChonNoiDung.SoHoaDon))
                {
                    doc.Replace(key, "0000000", true, true);
                }
                else
                {
                    doc.Replace(key, string.Empty, true, true);
                }
            }

            doc.Replace("<convertor>", fullName, true, true);
            doc.Replace("<conversionDate>", DateTime.Now.ToString("dd/MM/yyyy"), true, true);
            #endregion
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
                        PdfBitmap bp = new PdfBitmap(image);
                        bp.Quality = 20;
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
                fileName = "Hoa_don_mau_co_ban";
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
    }

    public enum DinhDangTepMau
    {
        PDF,
        DOC,
        DOCX,
        XML
    }

    public enum HinhThucMauHoaDon
    {
        HoaDonMauCoBan,
        HoaDonMauDangChuyenDoi,
        HoaDonMauCoChietKhau,
        HoaDonMauNgoaiTe,
        HoaDonMauCoBan_CoChietKhau,
        HoaDonMauCoBan_NgoaiTe,
        HoaDonMauCoBan_All,
        HoaDonMauDangChuyenDoi_CoChietKhau,
        HoaDonMauDangChuyenDoi_NgoaiTe,
        HoaDonMauDangChuyenDoi_All,
    }
}
