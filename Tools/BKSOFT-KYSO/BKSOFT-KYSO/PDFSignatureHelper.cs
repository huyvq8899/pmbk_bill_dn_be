﻿using BKSOFT_KYSO.Modal;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Graphics.Fonts;
using Spire.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO
{
    public class PDFSignatureHelper
    {
        private const string STR_FIND_TEXT = "Người bán";

        private const string STR_FULL_NAME = "full name";

        private const string STR_FULL_NAME_UP = "Full name";

        private const string STR_SIGN_BY = "Ký bởi: ";

        private PdfDocument pdfDoc;

        private PdfPageBase pageBase;

        private PdfCertificate certificate;

        private Size boundSign = new Size(0, 0);

        private PointF posSign = new PointF(0, 0);

        private PointF markSign = new PointF(0, 0);

        public MemoryStream Ms { set; get; }

        public NBan NBan { set; get; }

        public TYPE_MESSAGE TypeFindPositionSign { set; get; }

        public PDFSignatureHelper(byte[] pdfIn, PdfCertificate certificate)
        {
            // Load a PDF file and certificate
            this.pdfDoc = new PdfDocument(pdfIn);
            this.certificate = certificate;
        }

        public PDFSignatureHelper(string source, PdfCertificate certificate)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(source);

                using (MemoryStream stream = new MemoryStream(data))
                {
                    this.pdfDoc = new PdfDocument(stream);
                }
            }
            this.certificate = certificate;
        }

        public FeedBackFunction Sign()
        {
            bool res = true;
            try
            {
                DateTime date = DateTime.Now;

                // Find page end
                int end = pdfDoc.Pages.Count - 1;
                pageBase = pdfDoc.Pages[end];

                // PdfSignature
                var signature = new PdfSignature(pdfDoc, pageBase, certificate, NBan.Ten);
                signature.SignDetailsFont = new PdfTrueTypeFont(new Font("Times New Roman", 8f, FontStyle.Bold), true);         // Create a signature and set its position.
                signature.SignFontColor = Color.Red;

                // Add green tick
                PdfImage image = PdfImage.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\" + Constants.SIGNATURE_GREEN_TICK);
                pageBase.Canvas.SetTransparency(0.5f);

                // Create object PDFSign
                if (string.IsNullOrEmpty(NBan.TenP1))
                {
                    CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + NBan.TenP1);
                    pageBase.Canvas.DrawImage(image, markSign, new SizeF(30, 30));
                }
                else
                {
                    CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + NBan.TenP1);
                    pageBase.Canvas.DrawImage(image, markSign, new SizeF(40, 40));
                }

                signature.DigitalSignerLable = Constants.SIGNATURE_DIGITAL_SIGNER_LABLE;
                if (string.IsNullOrEmpty(NBan.TenP2))
                {
                    signature.DigitalSigner = string.Format(Constants.SIGNATURE_DIGITAL_SIGNER_P01, NBan.TenP1, date.ToString("dd/MM/yyyy"));
                }
                else
                {
                    signature.DigitalSigner = string.Format(Constants.SIGNATURE_DIGITAL_SIGNER_P02, NBan.TenP1, NBan.TenP2, date.ToString("dd/MM/yyyy"));
                }

                // Create bound
                signature.Bounds = new RectangleF(posSign, boundSign);
                signature.IsTag = true;
                signature.DistinguishedName = NBan.Ten;
                signature.ReasonLabel = Constants.PDF_SIGNATURE_REASON_LABEL;
                signature.Reason = Constants.PDF_SIGNATURE_REASON;
                signature.DateLabel = Constants.PDF_SIGNATURE_DATE_LABEL;
                signature.Date = date;
                signature.ContactInfoLabel = Constants.PDF_SIGNATURE_CONTACT_INFO_LABEL;
                signature.ContactInfo = NBan.SDThoai;
                signature.LocationInfo = NBan.DChi;
                if (this.TypeFindPositionSign == TYPE_MESSAGE.SIGN_INVOICE)
                {
                    signature.Certificated = true;
                }

                // Set the document permission of the signature.
                signature.DocumentPermissions = PdfCertificationFlags.ForbidChanges;

                //Disables the incremental update
                pdfDoc.FileInfo.IncrementalUpdate = false;
                //Sets the compression level to best
                pdfDoc.CompressionLevel = PdfCompressionLevel.Best;

                // Save memory stream
                Ms = new MemoryStream();
                pdfDoc.SaveToStream(Ms);

                //pdfDoc.SaveToFile(string.Format("sign_{0}_{1}.pdf", date.Hour, date.Second));
                return new FeedBackFunction { Status = Status.Success, Content = "" };
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);

                return new FeedBackFunction { Status = Status.Erro, Content = ex.Message };
            }
        }

        private void CalculatePointSigned(string s_find, string title)
        {
            try
            {
                float w_find = 0;
                switch (this.TypeFindPositionSign)
                {
                    case TYPE_MESSAGE.SIGN_RECORD:
                        s_find = "ĐẠI DIỆN BÊN B";
                        break;
                    case TYPE_MESSAGE.SIGN_RECORD_FOR_A:
                        s_find = "ĐẠI DIỆN BÊN A";
                        break;
                    case TYPE_MESSAGE.SIGN_RECORD_FOR_B:
                        s_find = "ĐẠI DIỆN BÊN B";
                        break;
                    default:
                        break;
                }

                // Find text
                PointF p_find = GetPositionFindText(s_find, ref w_find);
                if (p_find.Y < 50)
                {
                    return;
                }
                //p_find.X += 80;

                // Calculate with title sign
                float w_sign = GetWithSignedTitle(title);

                // Calculate size for sign
                boundSign.Width = (int)w_sign + 40;
                if (string.IsNullOrEmpty(NBan.TenP2))
                {
                    boundSign.Height = 30;
                }
                else
                {
                    boundSign.Height = 40;
                }

                // Calculte position for sign
                posSign.X = p_find.X + (w_find / 2) - (w_sign / 2);
                posSign.Y = p_find.Y + 25;
                if ((posSign.X + w_sign) > pageBase.ActualSize.Width - 30)
                {
                    posSign.X -= (posSign.X + w_sign) - (pageBase.ActualSize.Width - 30);
                }

                // Check dual languages
                p_find = GetPositionFindText(STR_FULL_NAME, ref w_find);
                if (p_find.Y > 50)
                {
                    posSign.Y += 15;
                }

                // Check dual languages
                p_find = GetPositionFindText(STR_FULL_NAME_UP, ref w_find);
                if (p_find.Y > 50)
                {
                    posSign.Y += 15;
                }

                // Calculate position mark
                markSign.X = posSign.X + (w_sign / 2);
                markSign.Y = posSign.Y;
                //_mark.Y = p_find.Y + 25;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private float GetWithSignedTitle(string title)
        {
            SizeF res = new SizeF(0, 0);

            try
            {
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Times New Roman", 8f, FontStyle.Bold), true);

                res = font.MeasureString(title);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return res.Width;
        }

        private PointF GetPositionFindText(string s_find, ref float w_find)
        {
            // Create default
            PointF point = new PointF(0, 0);

            try
            {
                PdfTextFind[] result = pageBase.FindText(s_find).Finds;

                if (result != null && result.Count() > 0)
                {
                    w_find = result[0].Bounds.Width;

                    point = result[0].Position;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return point;
        }

        public static PointF GetPositionFindText(string path, string s_find, ref float w_find)
        {
            // Create default
            PointF point = new PointF(0, 0);

            try
            {
                PdfDocument doc = new PdfDocument(path);

                PdfUsedFont[] usedfont = doc.UsedFonts;
                foreach (PdfUsedFont font in usedfont)
                {
                    string fontInfo = string.Format("{0}, {1}, {2}, {3}", font.Name, font.Size, font.Type, font.Style);

                    bool res = IsFontInstalled(font.Name, font.Size, (FontStyle)font.Style);

                }

                PdfPageBase page = doc.Pages[0];
                PdfTextFind[] result = page.FindText(s_find).Finds;
                if (result != null && result.Count() > 0)
                {
                    w_find = result[0].Bounds.Width;
                    point = result[0].Position;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return point;
        }

        public static bool IsFontInstalled(string fontName, float size, FontStyle style)
        {
            bool installed = false;
            float emSize = size;

            try
            {
                using (var testFont = new Font(fontName, emSize, style))
                {
                    installed = (0 == string.Compare(fontName, testFont.Name, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            catch
            {
            }

            return installed;
        }
    }
}