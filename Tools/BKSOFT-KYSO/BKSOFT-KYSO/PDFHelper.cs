using BKSOFT_KYSO.Modal;
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
    public class PDFHelper
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

        public MessageObj MsgObj { set; get; }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="source"></param>
        /// <param name="certificate"></param>
        public PDFHelper(MessageObj obj, PdfCertificate certificate, bool isTT32 = false)
        {
            MsgObj = obj;

            using (WebClient webClient = new WebClient())
            {
                byte[] data = null;
                if (isTT32)
                {
                    if (MsgObj.Type == 1004)        // Hóa đơn mẫu TT78
                    {
                        if (!string.IsNullOrEmpty(MsgObj.DataPDF))
                        {
                            data = Convert.FromBase64String(MsgObj.DataPDF);                            
                        }
                        else
                        {
                            data = webClient.DownloadData(MsgObj.UrlPDF);
                        }
                    }
                    else
                    {
                        data = webClient.DownloadData(MsgObj.DataPDF);
                    }
                }
                else
                {
                    data = webClient.DownloadData(MsgObj.UrlPDF);           // TT78
                }

                using (MemoryStream stream = new MemoryStream(data))
                {
                    this.pdfDoc = new PdfDocument(stream);
                }
            }
            this.certificate = certificate;
        }

        public bool Sign()
        {
            bool res = true;
            try
            {
                DateTime date = DateTime.Now;

                // Find page end
                int end = pdfDoc.Pages.Count - 1;
                pageBase = pdfDoc.Pages[end];

                // PdfSignature
                var signature = new PdfSignature(pdfDoc, pageBase, certificate, (MsgObj.TTNKy).Ten);
                signature.SignDetailsFont = new PdfTrueTypeFont(new Font("Times New Roman", 8f, FontStyle.Bold), true);         // Create a signature and set its position.
                signature.SignFontColor = Color.Red;

                // Add green tick
                string pathImg = AppDomain.CurrentDomain.BaseDirectory + "\\" + Constants.SIGNATURE_GREEN_TICK;
                PdfImage image = PdfImage.FromFile(pathImg);
                pageBase.Canvas.SetTransparency(0.5f);

                // Create object PDFSign
                if (string.IsNullOrEmpty((MsgObj.TTNKy).TenP1))
                {
                    CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + (MsgObj.TTNKy).TenP1);
                    pageBase.Canvas.DrawImage(image, markSign, new SizeF(30, 30));
                }
                else
                {
                    CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + (MsgObj.TTNKy).TenP1);
                    pageBase.Canvas.DrawImage(image, markSign, new SizeF(40, 40));
                }

                signature.DigitalSignerLable = Constants.SIGNATURE_DIGITAL_SIGNER_LABLE;
                if (string.IsNullOrEmpty((MsgObj.TTNKy).TenP2))
                {
                    signature.DigitalSigner = string.Format(Constants.SIGNATURE_DIGITAL_SIGNER_P01, (MsgObj.TTNKy).TenP1, date.ToString("dd/MM/yyyy"));
                }
                else
                {
                    signature.DigitalSigner = string.Format(Constants.SIGNATURE_DIGITAL_SIGNER_P02, (MsgObj.TTNKy).TenP1, (MsgObj.TTNKy).TenP2, date.ToString("dd/MM/yyyy"));
                }

                // Create bound
                signature.Bounds = new RectangleF(posSign, boundSign);
                signature.IsTag = true;
                signature.DistinguishedName = (MsgObj.TTNKy).Ten;
                signature.ReasonLabel = Constants.PDF_SIGNATURE_REASON_LABEL;
                signature.Reason = Constants.PDF_SIGNATURE_REASON;
                signature.DateLabel = Constants.PDF_SIGNATURE_DATE_LABEL;
                signature.Date = date;
                signature.ContactInfoLabel = Constants.PDF_SIGNATURE_CONTACT_INFO_LABEL;
                signature.ContactInfo = (MsgObj.TTNKy).SDThoai;
                signature.LocationInfo = (MsgObj.TTNKy).DChi;
                // Set the document permission of the signature.
                signature.Certificated = false;
                signature.DocumentPermissions = PdfCertificationFlags.AllowFormFill | PdfCertificationFlags.ForbidChanges;

                // Disables the incremental update
                // pdfDoc.FileInfo.IncrementalUpdate = false;
                // Sets the compression level to best
                pdfDoc.CompressionLevel = PdfCompressionLevel.Best;

                // Save memory stream
                Ms = new MemoryStream();
                pdfDoc.SaveToStream(Ms);

                // Write to disk
                string path = $"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\\log\\{Guid.NewGuid()}.pdf";
                if (!File.Exists(path))
                {
                    pdfDoc.SaveToFile(path);
                }

                if(MsgObj.MLTDiep == MLTDiep.BBCBenA)
                {
                    MsgObj.Cert = GetCertificate();
                }
                else
                {
                    MsgObj.CertB = GetCertificate();
                }

                res = true;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);

                res = false;
            }

            return res;
        }

        public byte[] GetCertificate()
        {
            return certificate.GetRawCertData();
        }

        private void CalculatePointSigned(string s_find, string title)
        {
            try
            {
                float w_find = 0;
                switch (MsgObj.MLTDiep)
                {
                    case MLTDiep.BBCBenA:
                        s_find = "ĐẠI DIỆN BÊN A";
                        break;
                    case MLTDiep.BBCBenB:
                        s_find = "ĐẠI DIỆN BÊN B";
                        break;
                    case MLTDiep.TDCDLHDKMDCQThue:
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
                if (string.IsNullOrEmpty((MsgObj.TTNKy).TenP2))
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
    }
}
