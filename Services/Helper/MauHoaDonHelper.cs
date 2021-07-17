using DLL.Constants;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Services.Helper
{
    public static class MauHoaDonHelper
    {
        /// <summary>
        /// Tạo mẫu hóa đơn doc
        /// </summary>
        public static Document TaoMauHoaDonDoc(MauHoaDon mauHoaDon, bool isLapLaiThongTin, BoMauHoaDonEnum loai, string webRootPath, HoSoHDDT hoSoHDDT)
        {
            string docPath = Path.Combine(webRootPath, $"docs/MauHoaDonAnhBH/{mauHoaDon.TenBoMau}/{loai.GetDescription()}.docx");

            Document doc = new Document();
            doc.LoadFromFile(docPath, Spire.Doc.FileFormat.Docx);

            if (!isLapLaiThongTin)
            {
                Section section = doc.Sections[0];
                section.PageSetup.DifferentFirstPageHeaderFooter = false;
                int count = section.HeadersFooters.Header.Tables.Count;
                for (int i = 0; i < count; i++)
                {
                    section.HeadersFooters.Header.Tables.RemoveAt(0);
                }
                section.PageSetup.DifferentFirstPageHeaderFooter = true;
            }

            List<string> soTKNH = new List<string>();
            if (!string.IsNullOrEmpty(hoSoHDDT.SoTaiKhoanNganHang))
            {
                soTKNH.Add(hoSoHDDT.SoTaiKhoanNganHang);
            }
            if (!string.IsNullOrEmpty(hoSoHDDT.TenNganHang))
            {
                soTKNH.Add(hoSoHDDT.TenNganHang);
            }

            doc.Replace("<CompanyName>", hoSoHDDT.TenDonVi ?? string.Empty, true, true);
            doc.Replace("<taxcode>", hoSoHDDT.MaSoThue ?? string.Empty, true, true);
            doc.Replace("<Address>", hoSoHDDT.DiaChi ?? string.Empty, true, true);
            doc.Replace("<Tel>", hoSoHDDT.SoDienThoaiLienHe ?? string.Empty, true, true);
            doc.Replace("<Banknumber>", string.Join(" - ", soTKNH), true, true);

            return doc;
        }

        /// <summary>
        /// Tạo trắng dữ liệu để preview
        /// </summary>
        public static FileReturn PreviewFilePDF(MauHoaDon mauHoaDon, bool isLapLaiThongTin, BoMauHoaDonEnum loai, string webRootPath, HoSoHDDT hoSoHDDT, IHttpContextAccessor accessor)
        {
            Document doc = TaoMauHoaDonDoc(mauHoaDon, isLapLaiThongTin, loai, webRootPath, hoSoHDDT);
            CreatePreviewFileDoc(doc, mauHoaDon, accessor);
            string mauHoaDonImg = Path.Combine(webRootPath, "images/template/mau.png");

            string folderPath = Path.Combine(webRootPath, $"temp");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string pdf = Path.Combine(folderPath, $"pdf_{Guid.NewGuid()}.pdf");
            doc.SaveToFile(pdf, Spire.Doc.FileFormat.PDF);

            PdfDocument pdfDoc = new PdfDocument();
            pdfDoc.LoadFromFile(pdf);
            PdfPageBase page = pdfDoc.Pages[0];
            PdfImage image = PdfImage.FromFile(mauHoaDonImg);
            float width = image.Width * 0.75f;
            float x = (page.Canvas.ClientSize.Width - width) / 2;
            float y = (page.Canvas.ClientSize.Height - 300) / 2;
            page.Canvas.DrawImage(image, x, y);
            pdfDoc.SaveToFile(pdf);

            byte[] bytes = File.ReadAllBytes(pdf);
            File.Delete(pdf);
            return new FileReturn
            {
                Bytes = bytes,
                ContentType = MimeTypes.GetMimeType(pdf),
                FileName = Path.GetFileName(pdf)
            };
        }

        public static void CreatePreviewFileDoc(Document doc, MauHoaDon mauHoaDon, IHttpContextAccessor accessor)
        {
            string fullName = accessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.FULL_NAME)?.Value;
            string domain = accessor.GetDomain();

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

            //TextSelection txtLinkSeach = doc.FindString("linkSearch", true, true);
            //if (txtLinkSeach != null)
            //{
            //    txtLinkSeach.GetAsOneRange().CharacterFormat.TextColor = ColorTranslator.FromHtml("#5200EE");
            //}
            doc.Replace("<linkSearch>", domain, true, true);
            doc.Replace("<codeSearch>", string.Empty, true, true);
            #endregion
        }
    }

    public class FileReturn
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }

    public enum LoaiFileDownload
    {
        PDF,
        DOC,
        DOCX
    }

    /// <summary>
    /// Bộ mẫu hóa đơn
    /// </summary>
    public enum BoMauHoaDonEnum
    {
        [Description("Hoa_don_mau_co_ban")]
        HoaDonMauCoBan,
        [Description("Hoa_don_mau_dang_chuyen_doi")]
        HoaDonMauDangChuyenDoi,
        [Description("Hoa_don_mau_co_chiet_khau")]
        HoaDonMauCoChietKhau,
        [Description("Hoa_don_mau_ngoai_te")]
        HoaDonMauNgoaiTe
    }
}
