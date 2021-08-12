using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Services.ViewModels.DanhMuc;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public static Document TaoMauHoaDonDoc(MauHoaDon mauHoaDon, HinhThucMauHoaDon loai, HoSoHDDT hoSoHDDT, IHostingEnvironment env, IHttpContextAccessor accessor, IConfiguration config, out int beginRow, bool hasReason = false)
        {
            string webRootPath = env.WebRootPath;
            string docPath = Path.Combine(webRootPath, $"docs/MauHoaDonAnhBH/{mauHoaDon.TenBoMau}/{loai.GetDescription()}.docx");
            string qrcode = Path.Combine(webRootPath, $"images/template/qrcode.png");
            string databaseName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string backgroundEmtpy = Path.Combine(webRootPath, $"images/background/empty.jpg");
            string linkSearch = config["Config:LinkSearchInvoice"];

            #region Logo
            var logo = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.Logo);
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.MauHoaDon);
            string logoPath = Path.Combine(webRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}/{mauHoaDon.MauHoaDonId}/FileAttach/{logo.GiaTri}");
            var giaTriBoSungLogos = logo.GiaTriBoSung.Split(";");
            float topLogo = float.Parse(giaTriBoSungLogos[0], CultureInfo.InvariantCulture.NumberFormat);
            float leftLogo = float.Parse(giaTriBoSungLogos[1], CultureInfo.InvariantCulture.NumberFormat);
            float widthLogo = float.Parse(giaTriBoSungLogos[2], CultureInfo.InvariantCulture.NumberFormat);
            float heightLogo = float.Parse(giaTriBoSungLogos[3], CultureInfo.InvariantCulture.NumberFormat);
            float positionLogo = int.Parse(giaTriBoSungLogos[4]);
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
            string bgDefaultPath = webRootPath + hinhNenMacDinh.GiaTriBoSung;
            #endregion

            #region Hình nền tải lên
            var bgUpload = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.HinhNenTaiLen);
            string bgUploadPath = Path.Combine(webRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}/{mauHoaDon.MauHoaDonId}/FileAttach/{bgUpload.GiaTri}");
            float topBgUpload = 0;
            float leftBgUpload = 0;
            float widthBgUpload = 0;
            float heightBgUpload = 0;
            float opacityBgUpload = 0;
            if (!string.IsNullOrEmpty(bgUpload.GiaTri))
            {
                var giaTriBoSungBgUploads = bgUpload.GiaTriBoSung.Split(";");
                topBgUpload = float.Parse(giaTriBoSungBgUploads[0], CultureInfo.InvariantCulture.NumberFormat);
                leftBgUpload = float.Parse(giaTriBoSungBgUploads[1], CultureInfo.InvariantCulture.NumberFormat);
                widthBgUpload = float.Parse(giaTriBoSungBgUploads[2], CultureInfo.InvariantCulture.NumberFormat);
                heightBgUpload = float.Parse(giaTriBoSungBgUploads[3], CultureInfo.InvariantCulture.NumberFormat);
                opacityBgUpload = float.Parse(giaTriBoSungBgUploads[4], CultureInfo.InvariantCulture.NumberFormat);
            }
            #endregion

            Document doc = new Document();
            doc.LoadFromFile(docPath, Spire.Doc.FileFormat.Docx);
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
                DocPicture picQRCode = tbl_nguoi_mua_first_page.Rows[0].Cells[1].Paragraphs[0].AppendPicture(Image.FromFile(qrcode));
                picQRCode.Width = 60;
                picQRCode.Height = 60;
                tbl_nguoi_mua_first_page.Rows[0].Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                tbl_nguoi_mua_first_page.Rows[0].Cells[1].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
            }
            if (tbl_nguoi_ban_first_page != null && !string.IsNullOrEmpty(logo.GiaTri))
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
                    DocPicture picQRCode = tbl_nguoi_mua.Rows[0].Cells[1].Paragraphs[0].AppendPicture(Image.FromFile(qrcode));
                    picQRCode.Width = 60;
                    picQRCode.Height = 60;
                    tbl_nguoi_mua.Rows[0].Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    tbl_nguoi_mua.Rows[0].Cells[1].Paragraphs[0].Format.HorizontalAlignment = HorizontalAlignment.Center;
                }
                if (tbl_nguoi_ban != null && !string.IsNullOrEmpty(logo.GiaTri))
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
                if (tbl_hhdv != null)
                {
                    TableRow cl_rowHeader = tbl_hhdv.Rows[0].Clone();
                    cl_rowHeader.Cells[0].Paragraphs[0].Text = "1";
                    cl_rowHeader.Cells[1].Paragraphs[0].Text = "2";
                    cl_rowHeader.Cells[2].Paragraphs[0].Text = "3";
                    cl_rowHeader.Cells[3].Paragraphs[0].Text = "4";
                    cl_rowHeader.Cells[4].Paragraphs[0].Text = "5";
                    cl_rowHeader.Cells[5].Paragraphs[0].Text = "6";
                    tbl_hhdv.Rows.Insert(1, cl_rowHeader);
                }
            }
            if (tbl_hhdv != null)
            {
                if (loai == HinhThucMauHoaDon.HoaDonMauCoChietKhau || loai == HinhThucMauHoaDon.HoaDonMauCoBan_CoChietKhau || loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau || loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
                {
                    int rowCount = tbl_hhdv.Rows.Count;
                    TableRow cl_rowTotalAmount = tbl_hhdv.Rows[rowCount - 4].Clone();
                    for (int i = 0; i < cl_rowTotalAmount.Cells.Count; i++)
                    {
                        TableCell cell = cl_rowTotalAmount.Cells[i];
                        if (i == 0)
                        {
                            Paragraph discountRatePar = cell.Paragraphs.Count > 0 ? cell.Paragraphs[0] : cell.AddParagraph();
                            discountRatePar.AppendText("Tỷ lệ CK: ");
                            TextRange trDiscountRate = discountRatePar.AppendText("<discountRate>");
                            trDiscountRate.CharacterFormat.Bold = true;
                        }
                        else if (i == 1)
                        {
                            cell.Paragraphs[0].Text = "Số tiền chiết khấu:";
                        }
                        else
                        {
                            cell.Paragraphs[0].Text = "<discountAmount>";
                        }
                    }
                    tbl_hhdv.Rows.Insert(rowCount - 4, cl_rowTotalAmount);
                }
                if (loai == HinhThucMauHoaDon.HoaDonMauNgoaiTe || loai == HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe || loai == HinhThucMauHoaDon.HoaDonMauCoBan_All ||
                    loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe || loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All)
                {
                    int rowCount = tbl_hhdv.Rows.Count;
                    for (int i = 0; i < 2; i++)
                    {
                        TableRow cl_lastRow = tbl_hhdv.Rows[rowCount - 1].Clone();
                        if (i == 0)
                        {
                            TableCell cell = cl_lastRow.Cells[0];
                            cell.CellFormat.Borders.Top.BorderType = BorderStyle.Cleared;
                            cell.CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                            cell.CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                            cell.CellFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;

                            Paragraph exchangePar = cell.Paragraphs[0];
                            exchangePar.ChildObjects.Clear();
                            exchangePar.AppendText("Quy đổi: ");
                            TextRange trExchange = exchangePar.AppendText("<exchangeAmount>");
                            trExchange.CharacterFormat.Bold = true;
                        }
                        else
                        {
                            TableCell cell = cl_lastRow.Cells[0];
                            cell.CellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
                            cell.CellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
                            cell.CellFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;

                            Paragraph exchangePar = cell.Paragraphs[0];
                            exchangePar.Format.BeforeSpacing = 5;
                            exchangePar.ChildObjects.Clear();
                            exchangePar.AppendText("Tỷ giá: ");
                            TextRange trExchange = exchangePar.AppendText("<exchangeRate>");
                            trExchange.CharacterFormat.Bold = true;
                        }

                        tbl_hhdv.Rows.Insert(rowCount, cl_lastRow);
                    }
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
            foreach (TextSelection seletion in txtLinkSearch)
            {
                seletion.GetAsOneRange().CharacterFormat.TextColor = ColorTranslator.FromHtml("#5406ee");
                seletion.GetAsOneRange().CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
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
            Image bgDefault = null;
            if (!string.IsNullOrEmpty(hinhNenMacDinh.GiaTri))
            {
                bgDefault = Image.FromFile(bgDefaultPath);
            }
            else
            {
                bgDefault = Image.FromFile(backgroundEmtpy);
            }
            Image bgUploaded = null;
            if (!string.IsNullOrEmpty(bgUpload.GiaTri))
            {
                bgUploaded = Image.FromFile(bgUploadPath).SetImageOpacity(opacityBgUpload);
                bgUploaded = bgUploaded.ResizeImage(widthBgUpload, heightBgUpload);
            }
            if (bgUploaded != null)
            {
                Graphics g = Graphics.FromImage(bgDefault);
                g.DrawImage(bgUploaded, leftBgUpload, topBgUpload);
            }
            doc.Background.Type = BackgroundType.Picture;
            doc.Background.Picture = bgDefault;
            #endregion

            #region test filldata
            if (tbl_hhdv != null)
            {
                // Check to insert to row detail order
                if (soDongTrang > 4)
                {
                    int _cnt_rows = soDongTrang - 4;

                    for (int i = 0; i < _cnt_rows; i++)
                    {
                        // Clone row
                        TableRow cl_row = tbl_hhdv.Rows[4].Clone();
                        // Add row
                        tbl_hhdv.Rows.Insert(4, cl_row);
                    }
                }
            }
            #endregion

            #region footer
            doc.Replace("<linkSearch>", linkSearch, true, true);
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

        /// <summary>
        /// Tạo trắng dữ liệu để preview
        /// </summary>
        public static FileReturn PreviewFilePDF(MauHoaDon mauHoaDon, HinhThucMauHoaDon loai, HoSoHDDT hoSoHDDT, IHostingEnvironment env, IHttpContextAccessor accessor, IConfiguration config)
        {
            Document doc = TaoMauHoaDonDoc(mauHoaDon, loai, hoSoHDDT, env, accessor, config, out int beginRow);
            CreatePreviewFileDoc(doc, mauHoaDon, accessor);
            string mauHoaDonImg = Path.Combine(env.WebRootPath, "images/template/mau.png");

            string folderPath = Path.Combine(env.WebRootPath, $"temp/preview_mau_hoa_don_{Guid.NewGuid()}");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // string docPath = Path.Combine(folderPath, $"doc_{DateTime.Now:HH-mm-ss}.docx");
            string pdfPath = Path.Combine(folderPath, $"{loai.GetTenFile()}.pdf");
            // doc.SaveToFile(docPath);
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

        public static void CreatePreviewFileDoc(Document doc, MauHoaDon mauHoaDon, IHttpContextAccessor accessor)
        {
            string fullName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;

            #region Replace
            doc.Replace("<dd>", string.Empty, true, true);
            doc.Replace("<mm>", string.Empty, true, true);
            doc.Replace("<yyyy>", string.Empty, true, true);

            doc.Replace("<numberSample>", mauHoaDon.MauSo, true, true);
            doc.Replace("<sign>", mauHoaDon.KyHieu, true, true);
            doc.Replace("<orderNumber>", "0000000", true, true);

            doc.Replace("<customerName>", string.Empty, true, true);
            doc.Replace("<customerCompany>", string.Empty, true, true);
            doc.Replace("<customerTaxCode>", string.Empty, true, true);
            doc.Replace("<customerAddress>", string.Empty, true, true);
            doc.Replace("<kindOfPayment>", string.Empty, true, true);
            doc.Replace("<accountNumber>", string.Empty, true, true);

            doc.Replace("<discountRate>", string.Empty, true, true);
            doc.Replace("<discountAmount>", string.Empty, true, true);
            doc.Replace("<totalAmount>", string.Empty, true, true);
            doc.Replace("<vatRate>", string.Empty, true, true);
            doc.Replace("<vatAmount>", string.Empty, true, true);
            doc.Replace("<totalPayment>", string.Empty, true, true);
            doc.Replace("<amountInWords>", string.Empty, true, true);
            doc.Replace("<exchangeRate>", string.Empty, true, true);
            doc.Replace("<exchangeAmount>", string.Empty, true, true);

            doc.Replace("<convertor>", fullName, true, true);
            doc.Replace("<conversionDate>", DateTime.Now.ToString("dd/MM/yyyy"), true, true);

            doc.Replace("<codeSearch>", string.Empty, true, true);
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
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauCoBan,
        [Description("Hoa_don_mau_dang_chuyen_doi")]
        HoaDonMauDangChuyenDoi,
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauCoChietKhau,
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauNgoaiTe,
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauCoBan_CoChietKhau,
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauCoBan_NgoaiTe,
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauCoBan_All,
        [Description("Hoa_don_mau_dang_chuyen_doi")]
        HoaDonMauDangChuyenDoi_CoChietKhau,
        [Description("Hoa_don_mau_dang_chuyen_doi")]
        HoaDonMauDangChuyenDoi_NgoaiTe,
        [Description("Hoa_don_mau_dang_chuyen_doi")]
        HoaDonMauDangChuyenDoi_All,
    }
}
