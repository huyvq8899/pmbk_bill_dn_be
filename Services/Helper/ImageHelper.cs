using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Text;

namespace Services.Helper
{
    public class ImageHelper
    {
        public static Bitmap CreateImg(string TenP1, string TenP2)
        {
            Bitmap bitmap = null;

            try
            {
                // Calulcate image size
                SizeF imageF = CalculateSizeImage(TenP1, TenP2);

                // Initialize new image from scratch
                bitmap = new Bitmap((int)imageF.Width + 10, (int)imageF.Height + 10, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                graphics.Clear(Color.FromKnownColor(KnownColor.White));

                // Initialize Brush class object
                Brush brush = new SolidBrush(Color.FromKnownColor(KnownColor.Black));
                Pen pen = new Pen(Color.FromKnownColor(KnownColor.Blue), 1);

                // Set font style, size, etc.
                Font arial = new Font("Times New Roman", 16, FontStyle.Regular);
                string measureString = string.Empty;
                if (!string.IsNullOrWhiteSpace(TenP2))
                {
                    measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\n{TenP2}\r\nKý ngày: {DateTime.Now.ToString("yyyy-MM-dd")}";
                }
                else
                {
                    measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\nKý ngày: {DateTime.Now.ToString("yyyy-MM-dd")}";
                }

                // Measure string.
                SizeF stringSize = new SizeF();
                stringSize = graphics.MeasureString(measureString, arial);

                // Draw string
                graphics.DrawRectangle(new Pen(Color.Green, 2), 5.0F, 5.0F, stringSize.Width, stringSize.Height);
                graphics.DrawString(measureString, arial, Brushes.Red, new PointF(5, 5));
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return bitmap;
        }

        public static SizeF CalculateSizeImage(string TenP1, string TenP2)
        {
            // Initialize new image from scratch
            Bitmap bitmap = new Bitmap(1024, 960, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.Clear(Color.FromKnownColor(KnownColor.White));

            // Set font style, size, etc.
            Font roman = new Font("Times New Roman", 16, FontStyle.Regular);
            string measureString = string.Empty;
            if (!string.IsNullOrWhiteSpace(TenP2))
            {
                measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\n{TenP2}\r\nKý ngày: {DateTime.Now.ToString("yyyy-MM-dd")}";
            }
            else
            {
                measureString = $"Signature Valid\r\nKý bởi: {TenP1}\r\nKý ngày: {DateTime.Now.ToString("yyyy-MM-dd")}";
            }

            // Measure string.            
            return graphics.MeasureString(measureString, roman);
        }
    }
}
