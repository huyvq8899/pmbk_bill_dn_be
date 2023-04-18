using DLL.Constants;
using DLL.Enums;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
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


namespace Services.Helper.VeThamQuan
{
    public static class MauVeHelper
    {
        /// <summary>
        /// Tạo mẫu hóa đơn doc
        /// </summary>
        private static readonly Object obj = new Object();
        public static Document TaoMauVEDoc(MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai, IHostingEnvironment env, IHttpContextAccessor accessor, out int beginRow, bool hasReason = false)
        {
            lock (obj)
            {
                // critical section
                string folderPath = Path.Combine("D:\\project-pmbk\\bill-doanh-nghiep-kt\\bill-back-end\\API\\wwwroot\\", $"temp/1_Test{Guid.NewGuid()}");
              //  string docPathhuyvq = Path.Combine(folderPath, $"doc_{DateTime.Now:HH-mm-ss}.docx");
                string webRootPath = env.WebRootPath;
                string docPath = Path.Combine(webRootPath, $"docs/MauHoaDon/02.CB.02.docx");
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
                //    var isLapLaiThongTinHD = bool.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.LapLaiThongTinKhiHoaDonCoNhieuTrang).GiaTri);
                #endregion

                #region Thiết lập dòng ký hiệu cột
                var isThietLapDongKyHieuCot = bool.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.ThietLapDongKyHieuCot).GiaTri);
                #endregion

                #region Số dòng trắng
                //    var soDongTrang = int.Parse(mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.SoDongTrang).GiaTri);
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
                doc.CreateTuyChinhChiTietMauVe(mauHoaDon, loai, env);
                Section section = doc.Sections[0];
                Table tbl_nguoi_mua = null;
                Table tbl_nguoi_ban = null;
                Table tbl_tieu_de = null;

                foreach (Table tb in section.Tables)
                {
                    if (tb.Title == "tbl_nguoi_ban")
                    {
                        tbl_nguoi_ban = tb;
                    }
                    if (tb.Title == "tbl_tieu_de")
                    {
                        tbl_tieu_de = tb;
                    }
                }
                if (tbl_tieu_de != null && isHienThiQRCode)
                {
                    TableCell lastTableCell = tbl_tieu_de.Rows[0].Cells[0];
                    lastTableCell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    /// Tạo khoảng cách giữa ảnh QR và nội dung Qr code - Giống lệnh Enter trong word
                    lastTableCell.AddParagraph();
                    lastTableCell.AddParagraph();
                    lastTableCell.AddParagraph();
                    Paragraph par = lastTableCell.Paragraphs.Count > 0 ? lastTableCell.Paragraphs[lastTableCell.Paragraphs.Count-1] : lastTableCell.AddParagraph();
                    DocPicture picQRCode = par.AppendPicture(Image.FromFile(qrcode));
                    picQRCode.Width = 46;
                    picQRCode.Height = 46;
                    par.Format.HorizontalAlignment = HorizontalAlignment.Center;
                }

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

                beginRow = 1;

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
                        //borderDefault = ConvertSvgToImage(bdDefaultPath, tempFolder, colorBdDefault, 950, 554);
                        borderDefault = ConvertSvgToImage(bdDefaultPath, tempFolder, colorBdDefault, 900, 1200);
                    }
                    else
                    {
                        borderDefault = Image.FromFile(bdDefaultPath);
                    }

                    g.DrawImage(borderDefault, 0, 5, 863, 1212);
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
                        backgroundDefault = ConvertSvgToImage(bgDefaultPath, tempFolder, colorBgDefault, 250, 250);
                    }
                    else
                    {
                        backgroundDefault = Image.FromFile(bgDefaultPath);
                    }
                        
                    int x = (bgDefault.Width / 2) - ((backgroundDefault.Width + 160) / 4);
                    int y = (bgDefault.Height / 2) - ((backgroundDefault.Height + 100) / 2);

                    g.DrawImage(backgroundDefault.SetImageOpacity(opacityBgDefault), x, y, 400, 600);
                }
                doc.Background.Type = BackgroundType.Picture;
                doc.Background.Picture = bgDefault;
                #endregion

                #region test filldata
                #endregion
                //   doc.SaveToFile(docPathhuyvq);

                return doc;
            }


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
           // settings.Format = MagickFormat.Png;

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

        public static void CreateTuyChinhChiTietMauVe(this Document doc, MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai, IHostingEnvironment env)
        {
          //  string folderPath = Path.Combine(env.WebRootPath, $"temp/1_Test{Guid.NewGuid()}");
         //   string docPath = Path.Combine(folderPath, $"doc_{DateTime.Now:HH-mm-ss}.docx");
            if (mauHoaDon.MauHoaDonTuyChinhChiTiets != null && mauHoaDon.MauHoaDonTuyChinhChiTiets.Count > 0)
            {
                Section section = doc.Sections[0];

                Table tbl_nguoi_mua = null;
                Table tbl_nguoi_ban = null;
                Table tbl_tieu_de = null;
                Table tbl_nguoi_ky = null;
                Table tbl_footer = null;

                List<MauHoaDonTuyChinhChiTietViewModel> thongTinNguoiBans = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiBan && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinHoaDon_mauSoKyHieuSoHoaDon = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => (x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon || x.Loai == LoaiTuyChinhChiTiet.ThongTinMaCuaCoQuanThue) && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinHoaDon = mauHoaDon.MauHoaDonTuyChinhChiTiets
        .Where(x => (x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon && x.Checked == true)).ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinHoaDon_QRcode = mauHoaDon.MauHoaDonTuyChinhChiTiets
.Where(x => (x.Loai == LoaiTuyChinhChiTiet.MaQRCode && x.Checked == true)).ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinNguoiMuas = mauHoaDon.MauHoaDonTuyChinhChiTiets
                        .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiMua && x.Checked == true)
                        .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinNguoiKys = mauHoaDon.MauHoaDonTuyChinhChiTiets
                       .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinNguoiKy && x.Checked == true)
                       .ToList();
                List<MauHoaDonTuyChinhChiTietViewModel> thongTinTraCuus = mauHoaDon.MauHoaDonTuyChinhChiTiets
                       .Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinTraCuu && x.Checked == true)
                       .ToList();

                foreach (Table tb in section.Tables)
                {
                    if (tb.Title == "tbl_nguoi_ban")
                    {
                        tbl_nguoi_ban = tb;
                    }
                    if (tb.Title == "tbl_tieu_de")
                    {
                        tbl_tieu_de = tb;
                    }
                }

                /// Bảng 1 - Thông tin người bán + Mẫu số ký hiệu
                doc.StyleDataTable(tbl_nguoi_ban, thongTinNguoiBans, TableType.ThongTinNguoiBan, mauHoaDon, loai, env);

                doc.StyleDataTable(tbl_nguoi_ban, thongTinHoaDon_mauSoKyHieuSoHoaDon, TableType.ThongTinMauSoKyHieu, mauHoaDon, loai);

                doc.StyleDataTable(tbl_tieu_de, thongTinHoaDon, TableType.ThongTinHoaDon, mauHoaDon, loai);

                doc.StyleDataTable(tbl_tieu_de, thongTinHoaDon_QRcode, TableType.ThongTinQrCode, mauHoaDon, loai);


                foreach (Table tb in section.Tables)
                {

                    if (tb.Title == "tbl_nguoi_ky")
                    {
                        tbl_nguoi_ky = tb;
                    }
                    if (tb.Title == "tbl_tra_cuu")
                    {
                        tbl_footer = tb;
                    }
                }
                doc.StyleDataTable(tbl_nguoi_ky, thongTinNguoiKys, TableType.ThongTinNguoiKy, mauHoaDon, loai);
                doc.StyleDataTable(tbl_footer, thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);
              //  doc.SaveToFile(docPath);

                /* foreach (Table tb in section.HeadersFooters.FirstPageFooter.Tables)
                 {
                     if (tb.Title == "tbl_footer")
                     {
                          tbl_footer_first_page = tb;
                     }
                 }
                 //  doc.StyleDataTable(tbl_footer_first_page, thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);

                 section.PageSetup.DifferentFirstPageHeaderFooter = false;
                 foreach (Table tb in section.HeadersFooters.Footer.Tables)
                 {
                     if (tb.Title == "tbl_footer")
                     {
                         tbl_footer = tb;
                     }
                 }
                 doc.StyleDataTable(tbl_footer, thongTinTraCuus, TableType.ThongTinFooter, mauHoaDon, loai);
                 section.PageSetup.DifferentFirstPageHeaderFooter = true;*/
            }
        }

        private static void StyleDataTable(this Document doc, Table table, List<MauHoaDonTuyChinhChiTietViewModel> list, TableType tableType, MauHoaDonViewModel mauHoaDon, HinhThucMauHoaDon loai, IHostingEnvironment env = null)
        {
            string folderPath = Path.Combine("D:\\project-pmbk\\bill-doanh-nghiep-kt\\bill-back-end\\API\\wwwroot\\", $"temp/1_Test{Guid.NewGuid()}");
            string docPath = Path.Combine(folderPath, $"doc_{DateTime.Now:HH-mm-ss}.docx");
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
                    int col = 2;
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

                        widthLogo = widthLogo * 45 / 100;
                        heightLogo = heightLogo * 45 / 100;
                    }
                    startColWithoutLogo = hasLogo ? (positionLogo == 1 ? 1 : 0) : 0;
                    endColWithoutLogo = hasLogo ? (positionLogo == 1 ? (col - 1) : (col - 2)) : (col - 1);
                    #endregion

                    table.ResetCells(row, col);

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        float widthOfTieuDe = doRong * (maxCol == 2 ? 50 : 45) / 100;

                        if (hasLogo)
                        {
                            table.Rows[i].Cells[0].Width = widthLogo * (45) / 100;
                            table.Rows[i].Cells[1].Width = 400;
                        }
                        else
                        {
                            table.Rows[i].Cells[0].Width = 450;
                        }
                    }

                    if (hasLogo)
                    {
                        Paragraph paraLogo = null;
                            table.ApplyVerticalMerge(0, 0, 2);
                            paraLogo = table[0, 0].AddParagraph();

                        Image logoImage = Image.FromFile(logoPath);
                        DocPicture picLogo = paraLogo.AppendPicture(logoImage);
                        picLogo.VerticalPosition = topLogo;
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
                            if (j == 0)
                            {
                                TableCell tableCell = tableRow.Cells[startColWithoutLogo];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                if (itemChildren.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDonViNguoiBan)
                                {
                                    //table.ApplyHorizontalMerge(i, startColWithoutLogo, endColWithoutLogo);
                                    tableCell = tableRow.Cells[startColWithoutLogo];
                                    par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                                    par.AddStyleTextRange(itemChildren.Children[0]);
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
                            else
                            {
                                TableCell tableCell = tableRow.Cells[startColWithoutLogo + 1 + (canTieuDe > 1 ? 1 : 0)];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

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

                    //table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                    //   doc.SaveToFile(docPath);
                }
                if (tableType == TableType.ThongTinMauSoKyHieu)
                {
                    var listThongTinNguoBan = new List<MauHoaDonTuyChinhChiTietViewModel>();

                    if (mauHoaDon.LoaiHoaDon == LoaiHoaDon.PXKKiemVanChuyenNoiBo && mauHoaDon.TenBoMau == "01.CB.02")
                    {

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

                    int canTieuDe = cloneList.SelectMany(x => x.Children).FirstOrDefault().TuyChonChiTiet.CanTieuDe ?? 0;
                    int doRong = cloneList.SelectMany(x => x.Children).Max(x => x.TuyChonChiTiet.DoRong ?? 0);
                    int row = listThongTinNguoBan.Count();
                    int maxCol = listThongTinNguoBan.Max(x => x.Children.Count);
                    int col = 2;
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
                    startColWithoutLogo = hasLogo ? (positionLogo == 1 ? 1 : 0) : 0;
                    endColWithoutLogo = hasLogo ? (positionLogo == 1 ? (col - 1) : (col - 2)) : (col - 1);
                    #endregion

                    //table.ResetCells(row, col);

                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        List<MauHoaDonTuyChinhChiTietViewModel> listChildren = listThongTinNguoBan[i].Children;

                        for (int j = 0; j < listChildren.Count; j++)
                        {
                            var itemChildren = listChildren[j];

                            /*               if (maxCol == 2 && listChildren.Count() == 1)
                                           {
                                               table.ApplyHorizontalMerge(i, canTieuDe == 1 ? startColWithoutLogo : (startColWithoutLogo + 1), endColWithoutLogo);
                                           }*/

                            if (j == 0)
                            {
                                logoPath = Path.Combine(mauHoaDon.WebRootPath, $"FilesUpload/{mauHoaDon.DatabaseName}/{ManageFolderPath.FILE_ATTACH}/{logo.GiaTri}");
                                if (!string.IsNullOrEmpty(logo.GiaTri) && File.Exists(logoPath))
                                {
                                    hasLogo = true;
                                }
                                TableCell tableCell = tableRow.Cells[hasLogo == true ? 2 : 1];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();


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
                            else
                            {
                                TableCell tableCell = tableRow.Cells[startColWithoutLogo + 1 + (canTieuDe > 1 ? 1 : 0)];
                                Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

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

                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;
                    // doc.SaveToFile(docPath);
                }
                if (tableType == TableType.ThongTinHoaDon)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinHoaDon = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon && (x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon || (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon && !string.IsNullOrEmpty(x.Children[0].GiaTri)))).ToList();
                    // List<MauHoaDonTuyChinhChiTietViewModel> listMSKHSHD = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon).ToList();
                    // List<MauHoaDonTuyChinhChiTietViewModel> listMaCuaCQT = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinMaCuaCoQuanThue).ToList();
                    /// Fix cứng tale thông tin hóa đơn c
                    int row = listThongTinHoaDon.Count;

                    if (row > 3)
                    {
                        for (int i = 0; i < row - 1; i++)
                        {
                            TableRow cl_row = table.Rows[0].Clone();
                            table.Rows.Insert(0, cl_row);
                        }
                    }
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        /// Gán cứng giá trị width khi đã chia cột
                        table.Rows[i].Cells[0].Width = 150;
                    }


                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = listThongTinHoaDon[i];

                        TableCell tableCell = tableRow.Cells[1];
                        Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenLoaiHoaDon || item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NgayThangNamTieuDe || item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenDichVu)
                        {
                            MauHoaDonTuyChinhChiTietViewModel child = item.Children[0];

                            bool isChuyenDoi = loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi || loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau || loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe || loai == HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All;

                            switch (child.LoaiChiTiet)
                            {
                                case LoaiChiTietTuyChonNoiDung.TenMauHoaDon:
                                    child.GiaTri = loai.GetDescription();

                                    switch (mauHoaDon.LoaiHoaDon)
                                    {
                                        case LoaiHoaDon.PXKKiemVanChuyenNoiBo:
                                            child.GiaTri = isChuyenDoi ? "(Bản chuyển đổi của phiếu xuất kho kiêm vận chuyển nội bộ điện tử)" : "(Bản thể hiện của phiếu xuất kho kiêm vận chuyển nội bộ điện tử)";
                                            break;
                                        case LoaiHoaDon.PXKHangGuiBanDaiLy:
                                            child.GiaTri = isChuyenDoi ? "(Bản chuyển đổi của phiếu xuất kho hàng gửi bán đại lý điện tử)" : "(Bản thể hiện của phiếu xuất kho hàng gửi bán đại lý điện tử)";
                                            break;
                                        default:
                                            break;
                                    }
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

                            if (child.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenLoaiHoaDon && item.Children.Any(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu))
                            {
                                Paragraph parSN = tableCell.AddParagraph();

                                MauHoaDonTuyChinhChiTietViewModel childSN = item.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDeSongNgu);

                                switch (childSN.LoaiChiTiet)
                                {
                                    case LoaiChiTietTuyChonNoiDung.TenMauHoaDon:

                                        childSN.GiaTri = "(BAYS OF CAT BA AECHIPELAGO)";
                                        break;
                                    default:
                                        break;
                                }

                                parSN.Format.HorizontalAlignment = HorizontalAlignment.Center;
                                par.AddStyleTextRange(child);
                                /// Gán cứng giá trị mặc định của vé tham quan Cát Bà
                                parSN.AddStyleTextRange(childSN);
                            }
                            else
                            {
                                    par.AddStyleTextRange(child);
                            }

                        }
                        else
                        {
                            foreach (var child in item.Children)
                            {
                                if (child.LoaiContainer == LoaiContainerTuyChinh.TieuDe)
                                {
                                    if (mauHoaDon.LoaiNgonNgu == LoaiNgonNgu.TiengViet && child.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.DaBaoGom)
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
                    // doc.SaveToFile(docPath);
                }
                if (tableType == TableType.ThongTinQrCode)
                {
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinHoaDon = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinHoaDon && (x.LoaiChiTiet != LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon || (x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.TenKhacLoaiHoaDon && !string.IsNullOrEmpty(x.Children[0].GiaTri)))).ToList();
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinHoaDon_QrCode = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.MaQRCode).ToList();
                    bool isHienThiQRCode = mauHoaDon.MauHoaDonThietLapMacDinhs.FirstOrDefault(x => x.Loai == LoaiThietLapMacDinh.HienThiQRCode).GiaTri == "true";
                    if (isHienThiQRCode)
                    {
                        int row = listThongTinHoaDon.Count;
                        if (listThongTinHoaDon_QrCode.Count > 0)
                        {
                            /// Merger vertical để chèn ảnh của QR code

                            table.ApplyVerticalMerge(0, 0, 4);

                            /// Nội dung hiển thị phía dưới của QR code
                             table.ApplyVerticalMerge(0, 4, 5);
                            TableRow tableRow = table.Rows[4];
                            TableCell tableCell = tableRow.Cells[0];
                            MauHoaDonTuyChinhChiTietViewModel noiDungThongTinQRCode = listThongTinHoaDon_QrCode.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NoiDungQrCode);
                            Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();
                            foreach (var child in noiDungThongTinQRCode.Children)
                            {
                                
                                par.AddStyleTextRange(child);
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
                    List<MauHoaDonTuyChinhChiTietViewModel> listThongTinTraCuu = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinTraCuu).ToList();
                    // List<MauHoaDonTuyChinhChiTietViewModel> listMSKHSHD = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.MauSoKyHieuSoHoaDon).ToList();
                    // List<MauHoaDonTuyChinhChiTietViewModel> listMaCuaCQT = cloneList.Where(x => x.Loai == LoaiTuyChinhChiTiet.ThongTinMaCuaCoQuanThue).ToList();
                    /// Fix cứng tale thông tin hóa đơn c
                    int row = 4;

                    for (int i = 0; i < row; i++)
                    {
                        TableRow cl_row = table.Rows[0].Clone();
                        table.Rows.Insert(0, cl_row);
                    }
                    for (int i = 0; i < row; i++)
                    {
                        TableRow tableRow = table.Rows[i];
                        MauHoaDonTuyChinhChiTietViewModel item = listThongTinTraCuu[i];

                        TableCell tableCell = tableRow.Cells[0];
                        Paragraph par = tableCell.Paragraphs.Count > 0 ? tableCell.Paragraphs[0] : tableCell.AddParagraph();

                        if (item.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.GhiChuChanTrang)
                        {
                            var itemBoiDung = item.Children.FirstOrDefault(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe);
                            par.AddStyleTextRange(itemBoiDung);
                        }
                        else if (i == 1)
                        {
                            CreateTraCuuTaiPar(par, cloneList, mauHoaDon);

                            CreateMaTraCuuPar(par, cloneList, mauHoaDon);
                        }
                        else if (i == 2)
                        {
                            var itemNguoiLap = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.NguoiLap);
                            foreach (var child in itemNguoiLap.Children)
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
                        else if (i == 3)
                        {
                            item = cloneList.FirstOrDefault(x => x.LoaiChiTiet == LoaiChiTietTuyChonNoiDung.PhatHanhBoi).Children[0];
                            par.AddStyleTextRange(item);
                        }

                    }
                    table.TableFormat.Borders.BorderType = BorderStyle.Cleared;

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
            TextRange textRange = isLink ? par.AppendHyperlink(item.GiaTri, item.GiaTri, HyperlinkType.WebLink) : par.AppendText(item.GiaTri);
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
            ThongTinMauSoKyHieu,
            ThongTinHoaDon,
            ThongTinQrCode,
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


        public static Document FormatBeforeSavePdf(this Document doc, string fontName, int fontSize)
        {
            foreach (Section sec in doc.Sections)
            {
                foreach (DocumentObject obj in sec.Body.ChildObjects)
                {
                    if (obj is Paragraph)
                    {
                        var para = obj as Paragraph;
                        foreach (DocumentObject Pobj in para.ChildObjects)
                        {
                            if (Pobj is TextRange)
                            {
                                TextRange textRange = Pobj as TextRange;
                                //set the font
                                textRange.CharacterFormat.FontName = fontName;
                                //set the font size
                                if (fontSize != 0)
                                    textRange.CharacterFormat.FontSize = fontSize;
                            }
                        }
                    }
                }
                /// set font cho table
                int check = sec.Tables.Count;
                for (int k = 0; k < check; k++)
                {
                    Table table = sec.Tables[k] as Table;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        for (int j = 0; j < table.Rows[i].Cells.Count; j++)
                        {
                            TableCell cell = table.Rows[i].Cells[j];
                            foreach (Paragraph para in cell.Paragraphs)
                            {
                                //apply style
                                foreach (DocumentObject Pobj in para.ChildObjects)
                                {
                                    if (Pobj is TextRange)
                                    {
                                        TextRange textRange = Pobj as TextRange;
                                        //set the font
                                        textRange.CharacterFormat.FontName = fontName;
                                        //set the font size
                                        if (fontSize != 0)
                                            textRange.CharacterFormat.FontSize = fontSize;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return doc;
        }

    }
}
