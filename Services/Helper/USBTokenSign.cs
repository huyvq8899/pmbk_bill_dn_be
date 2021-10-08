using Microsoft.AspNetCore.Hosting;
using Services.ViewModels.DanhMuc;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Security;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Services.Helper
{
    public class USBTokenSign
    {
        private readonly HoSoHDDTViewModel _hoSoHDDTViewModel;
        private readonly IHostingEnvironment _hostingEnvironment;

        public USBTokenSign(
            HoSoHDDTViewModel hoSoHDDTViewModel,
            IHostingEnvironment hostingEnvironment)
        {
            _hoSoHDDTViewModel = hoSoHDDTViewModel;
            _hostingEnvironment = hostingEnvironment;
        }

        public const string STR_FIND_TEXT = "Người bán ";

        public const string STR_SIGN_BY = "Ký bởi: ";

        public void DigitalSignaturePDF(string pdfPath, DateTime ngayKy)
        {
            try
            {
                // Load a PDF file and certificate
                PdfDocument doc = new PdfDocument();
                doc.LoadFromFile(pdfPath);

                // Find page end
                int end = doc.Pages.Count - 1;
                PdfPageBase page = doc.Pages[end];

                // Add green tick
                string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/template/greentick.png");
                PdfImage image = PdfImage.FromFile(imagePath);
                page.Canvas.SetTransparency(0.5f);

                // Create object PDFSign
                PDFSign _pdf = new PDFSign(page, _hoSoHDDTViewModel);
                _pdf.CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + _hoSoHDDTViewModel.TenDonVi.ToUpper());

                // Create object PDFSign
                _pdf.CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + _hoSoHDDTViewModel.TenDonVi);
                page.Canvas.DrawImage(image, _pdf._mark, new SizeF(30, 30));
                //if (string.IsNullOrEmpty(_salerViewModel.SignPartName02))
                //{
                //    _pdf.CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + _salerViewModel.CompanyName);
                //    page.Canvas.DrawImage(image, _pdf._mark, new SizeF(30, 30));
                //}
                //else
                //{
                //    _pdf.CalculatePointSigned(STR_FIND_TEXT, STR_SIGN_BY + _salerViewModel.SignPartName01);
                //    page.Canvas.DrawImage(image, _pdf._mark, new SizeF(40, 40));
                //}

                // PdfCertificate object with  X509Certificate2 class object
                //PdfCertificate cert = new PdfCertificate(x509);
                string pfxFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "pfx/dtbk.pfx");
                PdfCertificate cert = new PdfCertificate(pfxFilePath, "DienTuBachKhoa");

                // Create a signature and set its position.
                var signature = new PdfSignature(doc, page, cert, _hoSoHDDTViewModel.TenDonVi.ToUpper());
                signature.SignDetailsFont = new PdfTrueTypeFont(new Font("Times New Roman", 8f, FontStyle.Bold), true);
                signature.SignFontColor = Color.Red;

                signature.Bounds = new RectangleF(_pdf._pos, _pdf._bound);
                // Fill the contents of the signature.
                signature.Name = string.Format("Ký bởi: {0}\nNgày ký: {1}\n\n\n", _hoSoHDDTViewModel.TenDonVi, ngayKy.ToString("dd/MM/yyyy"));
                //if (string.IsNullOrEmpty(_salerViewModel.SignPartName02))
                //{
                //    signature.Name = string.Format("Ký bởi: {0}\nNgày ký: {1}\n\n\n", _salerViewModel.SignPartName01, ngayKy.ToString("dd/MM/yyyy"));
                //}
                //else
                //{
                //    signature.Name = string.Format("Ký bởi: {0}\n{1}\nNgày ký: {2}\n\n\n", _salerViewModel.SignPartName01, _salerViewModel.SignPartName02, ngayKy.ToString("dd/MM/yyyy"));
                //}
                signature.DistinguishedName = _hoSoHDDTViewModel.TenDonVi.ToUpper();

                signature.ReasonLabel = "\nReason: ";
                signature.Reason = "Hóa đơn giá trị gia tăng";       // Add
                signature.DateLabel = "\nNgày ký: ";
                signature.Date = DateTime.Now;

                signature.ContactInfoLabel = "Điện thoại: ";
                signature.ContactInfo = _hoSoHDDTViewModel.SoDienThoaiLienHe;             // Add
                signature.LocationInfo = _hoSoHDDTViewModel.DiaChi;                // Add         
                signature.Certificated = false;

                // Set the document permission of the signature.
                signature.DocumentPermissions = PdfCertificationFlags.ForbidChanges;
                doc.SaveToFile(pdfPath);
                doc.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class PDFSign
    {
        public Size _bound = new Size(0, 0);

        public PointF _pos = new PointF(0, 0);

        public PointF _mark = new PointF(0, 0);
        public HoSoHDDTViewModel _hoSoHDDTViewModel;

        private PdfPageBase Page { set; get; }

        public PDFSign(PdfPageBase page, HoSoHDDTViewModel hoSoHDDTViewModel)
        {
            Page = page;
            _hoSoHDDTViewModel = hoSoHDDTViewModel;
        }

        public PointF GetPositionFindText(string s_find, ref float w_find)
        {
            // Create default
            PointF point = new PointF(0, 0);

            try
            {
                PdfTextFind[] result = Page.FindText(s_find, TextFindParameter.IgnoreCase).Finds;
                PdfTextFind[] result1 = Page.FindText("full name)", TextFindParameter.IgnoreCase).Finds;
                if (result1.Length > 0)
                {
                    if (result != null && result.Count() > 0)
                    {
                        w_find = result[0].Bounds.Width;

                        point = result[0].Position;
                        point.X += 30;
                        point.Y += 15;
                    }
                }
                else if (result.Length > 0)
                {
                    if (result != null && result.Count() > 0)
                    {
                        w_find = result[0].Bounds.Width;
                        point = result[0].Position;
                    }
                }
                else
                {
                    if (result != null && result.Count() > 0)
                    {
                        w_find = result[0].Bounds.Width;

                        point = result[0].Position;
                    }
                }

            }
            catch (Exception)
            {
            }

            return point;
        }

        public float GetWithSignedTitle(string title)
        {
            SizeF res = new SizeF(0, 0);

            try
            {
                PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Times New Roman", 8f, FontStyle.Bold), true);

                res = font.MeasureString(title);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return res.Width;
        }

        public void CalculatePointSigned(string s_find, string title)
        {
            try
            {
                float w_find = 0;

                // Find text
                PointF p_find = GetPositionFindText(s_find, ref w_find);
                if (p_find.Y < 50)
                {
                    return;
                }

                // Calculate with title sign
                float w_sign = GetWithSignedTitle(title);

                // Calculate size for sign
                _bound.Width = (int)w_sign + 20;
                _bound.Height = 30;
                //if (string.IsNullOrEmpty(_salerVM.SignPartName02))
                //{
                //    _bound.Height = 30;
                //}
                //else
                //{
                //    _bound.Height = 40;
                //}

                // Calculte position for sign
                _pos.X = p_find.X + (w_find / 2) - (w_sign / 2);
                _pos.Y = p_find.Y + 25;
                if ((_pos.X + w_sign) > Page.ActualSize.Width - 30)
                {
                    _pos.X -= (_pos.X + w_sign) - (Page.ActualSize.Width - 30);
                }

                // Calculate position mark
                _mark.X = _pos.X + (w_sign / 2);
                _mark.Y = p_find.Y + 25;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
