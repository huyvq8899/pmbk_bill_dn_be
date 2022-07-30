using DLL.Entity;
using DLL.Enums;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using Services.Helper;
using Services.ViewModels.Config;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ManagementServices.Helper
{
    public static class TextHelper
    {
        public static bool IsValidEmail(this string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string ToUnSign(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }
        public static bool CheckIsInteger(this string input)
        {
            return int.TryParse(input, out _);
        }

        public static bool CheckDecical(this string input)
        {
            return decimal.TryParse(input, out _);
        }

        public static bool CheckNegative(this string input)
        {
            var rs = decimal.TryParse(input, out decimal a);
            if (rs)
            {
                if (a < 0) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }
        public static List<string> ConvertDanhSachTaiKhoanToArray(this string multiCharString)
        {
            List<string> array = new List<string>();
            string[] multiArray = multiCharString.Split(new Char[] { ' ', ';', '.', '-', '\n', '\t' });
            foreach (string author in multiArray)
            {
                if (author.Trim() != "")
                {
                    array.Add(author.Trim());
                }
            }
            return array;
        }
        public static string ConvertArrayToDanhSachTaiKhoan(this List<string> array)
        {
            string str = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i < array.Count - 1)
                {
                    str += array[i] + ";";
                }
                else
                {
                    str += array[i];
                }
            }
            //foreach (var item in array)
            //{
            //    str += item + ";";
            //}
            return str;
        }
        public static string ToTitleCase(this string title)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }
        public static string PadZerro(this int input)
        {
            string result = input.ToString().PadLeft(7, '0');
            return result;
        }
        public static string ToTrim(this string value)
        {
            return Regex.Replace(value, @"\s+", " ").Trim();
        }
        public static string TrimToUpper(this string value)
        {
            if (value == null) return "";
            return value.Trim().ToUpper();
        }

        public static string ToUpperFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static string ToConvertFullDateFormat(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            DateTime dt = DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return dt.ToString("dd/MM/yyyy");
        }
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static string SendMail(this string FromMailAddress, string FromMailName, string Pass, string ToMailName, string ToMailAddress, string MesSubject, string MesBody, string fileUrl)
        {

            var mes = new MimeMessage();
            mes.From.Add(new MailboxAddress(FromMailName, FromMailAddress));
            mes.To.Add(new MailboxAddress(ToMailName, ToMailAddress));
            mes.Subject = MesSubject;
            // gửi text thông thường
            //mes.Body = new TextPart("plain")
            //{
            //    Text = MesBody
            //};
            // gửi html page
            var bodyBuilder = new BodyBuilder();
            if (!string.IsNullOrEmpty(fileUrl))
            {
                bodyBuilder.Attachments.Add(fileUrl);
            }
            bodyBuilder.HtmlBody = MesBody;
            mes.Body = bodyBuilder.ToMessageBody();
            //
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("mail9096.maychuemail.com", 465, true);
                //client.Authenticate(fromMail, pass);
                client.Authenticate(FromMailAddress, Pass); // mật khẩu ứng dụng được tạo bằng google thư
                client.Send(mes);
                client.Disconnect(true);
            }
            return "true";

        }

        public static string FormatQuanity(this decimal value)
        {
            string s_tmp;
            string many = string.Empty;
            double dec;

            try
            {
                UInt64 parts = (UInt64)value;

                double stool = (double)(value - parts);

                s_tmp = stool.ToString();
                if (s_tmp.Length == 3)
                {
                    many = value.ToString("N01", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                else if (s_tmp.Length == 4)
                {
                    many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                else
                {
                    many = value.ToString("N03", CultureInfo.CreateSpecificCulture("es-ES"));
                }

                int idx = many.IndexOf(",");
                if (idx > 0)
                {
                    s_tmp = many.Substring(idx + 1);
                    s_tmp = "0." + s_tmp;

                    // Get decimal value
                    dec = Convert.ToDouble(s_tmp);
                    if (dec == 0)
                    {
                        many = many.Substring(0, idx);
                    }
                }
            }
            catch (Exception)
            {
                // FileLog.WriteLog(string.Empty, ex);
            }

            return many;
        }

        public static string FormatNumberByTuyChon(this decimal value, List<TuyChonViewModel> tuyChons, string loai, bool showZerro = false, string maLoaiTien = null)
        {
            if (value == 0)
            {
                return showZerro ? "0" : string.Empty;
            }

            var tuyChon = tuyChons.FirstOrDefault(x => x.Ma == loai);
            string decimalFormat = "0";
            if (tuyChon != null)
            {
                decimalFormat = tuyChon.GiaTri;
            }

            var result = value.ToString("N0" + decimalFormat, CultureInfo.CreateSpecificCulture("es-ES"));

            if (!string.IsNullOrEmpty(maLoaiTien))
            {
                result += $" {maLoaiTien}";
            }

            return result;
        }

        public static decimal MathRoundNumberByTuyChon(this decimal value, List<TuyChonViewModel> tuyChons, string loai)
        {
            var tuyChon = tuyChons.FirstOrDefault(x => x.Ma == loai);
            int decimalFormat = 0;
            if (tuyChon != null)
            {
                decimalFormat = int.Parse(tuyChon.GiaTri);
            }

            var result = Math.Round(value, decimalFormat, MidpointRounding.AwayFromZero);
            return result;
        }

        public static string FormatPriceChenhLech(this decimal value, string defaultValue = "")
        {
            string s_tmp;
            string many = string.Empty;
            double dec;

            try
            {
                if (value > 0)
                {
                    UInt64 parts = (UInt64)value;

                    double stool = (double)(value - parts);

                    s_tmp = stool.ToString();
                    if (s_tmp.Length == 3)
                    {
                        many = value.ToString("N01", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else if (s_tmp.Length == 4)
                    {
                        many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else
                    {
                        many = value.ToString("N03", CultureInfo.CreateSpecificCulture("es-ES"));
                    }

                    int idx = many.IndexOf(",");
                    if (idx > 0)
                    {
                        s_tmp = many.Substring(idx + 1);
                        s_tmp = "0." + s_tmp;

                        // Get decimal value
                        dec = Convert.ToDouble(s_tmp);
                        if (dec == 0)
                        {
                            many = many.Substring(0, idx);
                        }
                    }
                }
                else if (value < 0)
                {
                    value = Math.Abs(value);
                    UInt64 parts = (UInt64)value;

                    double stool = (double)(value - parts);

                    s_tmp = stool.ToString();
                    if (s_tmp.Length == 3)
                    {
                        many = value.ToString("N01", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else if (s_tmp.Length == 4)
                    {
                        many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));
                    }
                    else
                    {
                        many = value.ToString("N03", CultureInfo.CreateSpecificCulture("es-ES"));
                    }

                    int idx = many.IndexOf(",");
                    if (idx > 0)
                    {
                        s_tmp = many.Substring(idx + 1);
                        s_tmp = "0." + s_tmp;

                        // Get decimal value
                        dec = Convert.ToDouble(s_tmp);
                        if (dec == 0)
                        {
                            many = many.Substring(0, idx);
                        }
                    }
                    many = "(" + many + ")";

                }
                else
                {
                    many = defaultValue;
                }
            }
            catch (Exception)
            {
                // FileLog.WriteLog(string.Empty, ex);
            }
            return many;
        }

        public static string FormatPrice2(this decimal value)
        {
            var result = value.ToString(CultureInfo.CreateSpecificCulture("es-ES"));
            return result;
        }

        public static string FormatPrice(this decimal value)
        {
            string s_tmp;
            string many = string.Empty;
            double dec;

            try
            {
                many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));

                int idx = many.IndexOf(",");
                if (idx > 0)
                {
                    s_tmp = many.Substring(idx + 1);
                    s_tmp = "0." + s_tmp;

                    // Get decimal value
                    dec = Convert.ToDouble(s_tmp);
                    if (dec == 0)
                    {
                        many = many.Substring(0, idx);
                    }
                }
            }
            catch (Exception)
            {
                // FileLog.WriteLog(string.Empty, ex);
            }

            return many;
        }

        public static string GetFirstCharFromString(string ctx)
        {
            string rsl = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(ctx))
                {
                    return rsl;
                }
                ctx = ctx.Trim();
                ctx = Regex.Replace(ctx, @"\s+", " ");

                string[] arrs = ctx.Split(" ");
                foreach (var it in arrs)
                {
                    if (it.Length > 1)
                    {
                        string first = it.Substring(0, 1);

                        first = first.ToUpper();

                        rsl += first;
                    }
                }
            }
            catch (Exception)
            {
                //FileLog.WriteLog(string.Empty, ex);
            }

            return rsl;
        }

        public static string ConvertToInWord(this decimal total, string cachDocSo0HangChuc, string cachDocSoHangNghin, bool hienThiSoChan, string maLoaiTien, int cachTheHienSoTienBangChu, HoaDonDienTuViewModel hd)
        {
            try
            {
                string rs = "";
                decimal totalDecimal = total;
                total = Math.Truncate(Math.Abs(total));
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "nghìn", "", "", "triệu", "", "", "tỷ", "", "", "nghìn", "", "", "triệu" };
                string nstr = total.ToString();
                string readNameOfCurrency = "đồng";

                if (maLoaiTien != "VND")
                {
                    if (maLoaiTien == "USD")
                    {
                        readNameOfCurrency = "đô la Mỹ";
                    }
                }


                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : " ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }

                if (!string.IsNullOrEmpty(maLoaiTien))
                {
                    if (rs[rs.Length - 1] != ' ')
                        rs += $" {readNameOfCurrency}";
                    else
                        rs += readNameOfCurrency;
                }

                if (maLoaiTien == "USD")
                {
                    if (totalDecimal % 1 != 0)
                    {
                        decimal dec = (int)(totalDecimal % 1 * 100);
                        rs += " và " + ConvertToInWord(dec, cachDocSo0HangChuc, cachDocSoHangNghin, false, null, cachTheHienSoTienBangChu, hd).ToLower() + " xu";
                    }
                }

                rs = rs.Trim();
                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 1);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(1);
                    rs = rs1 + rs;
                }
                rs = rs.Replace("lẻ", cachDocSo0HangChuc).Replace("mươi", "mươi").Replace("trăm", "trăm").Replace("mười", "mười").Replace("nghìn", cachDocSoHangNghin) + ((hienThiSoChan) ? " chẵn" : string.Empty);

                // nếu là hóa đơn điều chỉnh tăng hoặc giảm thì đoc theo tùy chọn
                if (!string.IsNullOrEmpty(maLoaiTien) && (hd.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && ((hd.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhTang) || (hd.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhGiam)))
                {
                    switch (cachTheHienSoTienBangChu)
                    {
                        case 1: // [Giảm/Tăng] [Tổng tiền thanh toán]
                            rs = (hd.LoaiDieuChinh == 1 ? "Tăng" : "Giảm") + " " + rs.ToLower();
                            break;
                        case 2: // [Điều chỉnh giảm/Điều chỉnh tăng] [Tổng tiền thanh toán]
                            rs = (hd.LoaiDieuChinh == 1 ? "Điều chỉnh tăng" : "Điều chỉnh giảm") + " " + rs.ToLower();
                            break;
                        case 3: // [Tổng tiền thanh toán] [(Giảm)/(Tăng)]
                            rs = rs + " " + (hd.LoaiDieuChinh == 1 ? "(Tăng)" : "(Giảm)");
                            break;
                        case 4: // [Tổng tiền thanh toán] [(Điều chỉnh giản)/(Điều chỉnh tăng)]
                            rs = rs + " " + (hd.LoaiDieuChinh == 1 ? "(Điều chỉnh tăng)" : "(Điều chỉnh giảm)");
                            break;
                        case 5: // [Âm/] [Tổng tiền thanh toán]
                            if (hd.LoaiDieuChinh == 2) // điều chỉnh giảm
                            {
                                rs = $"Âm {rs.ToLower()}";
                            }
                            break;
                        case 6: // [Giảm/] [Tổng tiền thanh toán]
                            if (hd.LoaiDieuChinh == 2) // điều chỉnh giảm
                            {
                                rs = $"Giảm {rs.ToLower()}";
                            }
                            break;
                        default:
                            break;
                    }
                }

                return rs + ".";
            }
            catch
            {
                return "";
            }
        }

        public static string DocTenLoaiTien(this string maLoaiTien)
        {
            string result = string.Empty;

            switch (maLoaiTien)
            {
                case "VND":
                    result = "đồng";
                    break;
                case "USD":
                    result = "đô la Mỹ";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static bool IsValidDate(this string value)
        {
            string[] dateFormats = {
                "dd/MM/yyyy",
                "d/MM/yyyy",
                "d/M/yyyy",
                "dd/M/yyyy",
                "dd/MM/yyyy hh:mm:ss tt",
                "d/MM/yyyy hh:mm:ss tt",
                "dd/M/yyyy hh:mm:ss tt",
                "d/M/yyyy hh:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy hh:mm:ss tt",
                "yyyy-MM-ddTHH:mm:ss"
            };
            bool validDate = DateTime.TryParseExact(value, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
            //bool validDate = DateTime.TryParseExact(value, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime temp);
            return validDate;
        }

        public static DateTime ParseExact(this string value)
        {
            string[] dateFormats = {
                "dd/MM/yyyy",
                "d/MM/yyyy",
                "d/M/yyyy",
                "dd/M/yyyy",
                "dd/MM/yyyy hh:mm:ss tt",
                "d/MM/yyyy hh:mm:ss tt",
                "dd/M/yyyy hh:mm:ss tt",
                "d/M/yyyy hh:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy hh:mm:ss tt",
                "yyyy-MM-ddTHH:mm:ss"
            };
            //DateTime result = DateTime.ParseExact(value, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            DateTime result = DateTime.ParseExact(value, dateFormats, CultureInfo.InvariantCulture);
            return result;
        }

        public static DateTime? ParseExactNullable(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            string[] dateFormats = {
                "dd/MM/yyyy",
                "d/MM/yyyy",
                "d/M/yyyy",
                "dd/M/yyyy",
                "dd/MM/yyyy hh:mm:ss tt",
                "d/MM/yyyy hh:mm:ss tt",
                "dd/M/yyyy hh:mm:ss tt",
                "d/M/yyyy hh:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy hh:mm:ss tt",
                "yyyy-MM-ddTHH:mm:ss"
            };
            DateTime result = DateTime.ParseExact(value, dateFormats, CultureInfo.InvariantCulture);
            return result;
        }

        public static DateTime? ParseExactSoCaiOngHieu(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            string[] dateFormats = {
                "dd/MM/yyyy",
                "d/MM/yyyy",
                "d/M/yyyy",
                "dd/M/yyyy",
                "dd/MM/yyyy hh:mm:ss tt",
                "d/MM/yyyy hh:mm:ss tt",
                "dd/M/yyyy hh:mm:ss tt",
                "d/M/yyyy hh:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy hh:mm:ss tt",
                "MM/d/yyyy hh:mm:ss tt",
                "MM/dd/yyyy hh:mm:ss tt",
                "yyyy-MM-dd"
            };
            DateTime result = DateTime.ParseExact(value, dateFormats, CultureInfo.InvariantCulture);
            return result;
        }

        public static bool IsValidCurrency(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                var tachChuoi = value.Split('.');
                var tachChuoi2 = value.Split(',');
                if ((tachChuoi.Length - 1) > 1 || (tachChuoi2.Length - 1) > 1) //nếu xuất hiện trên 2 lần ký tự . hoặc ,
                {
                    return false;
                }
            }

            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            return decimal.TryParse(value, NumberStyles.Currency, culture, out _);
        }

        public static bool IsValidCurrencyOutput(this string value, List<TuyChonViewModel> tuyChons, string loai, out decimal output)
        {
            if (string.IsNullOrEmpty(value))
            {
                output = 0;
                return true;
            }

            if (string.IsNullOrWhiteSpace(value) == false)
            {
                var tachChuoi = value.Split('.');
                var tachChuoi2 = value.Split(',');
                if ((tachChuoi.Length - 1) > 1 || (tachChuoi2.Length - 1) > 1) //nếu xuất hiện trên 2 lần ký tự . hoặc ,
                {
                    output = 0;
                    return false;
                }
            }

            var result = decimal.TryParse(value, out decimal outputDecimal);
            if (result)
            {
                var tuyChon = tuyChons.FirstOrDefault(x => x.Ma == loai);
                int decimalPlace = 0;
                if (tuyChon != null)
                {
                    decimalPlace = int.Parse(tuyChon.GiaTri);
                }

                output = Math.Round(outputDecimal, decimalPlace, MidpointRounding.AwayFromZero);
            }
            else
            {
                output = 0;
            }

            return result;
        }

        public static bool IsValidInt(this string value, out int output)
        {
            if (int.TryParse(value, out output))
                return true;
            else
                return false;
        }

        public static int ParseInt(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int ParseThueGTGT(this string value)
        {
            try
            {
                if (value == "0" || value == "5" || value == "10")
                {
                    return int.Parse(value);
                }
                else
                {
                    if (value == "KCT")
                    {
                        return -1;
                    }
                    else if (value == "KKKNT")
                    {
                        return -2;
                    }
                    else if (value == "KHAC")
                    {
                        return -3;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static decimal ParseDecimal(this string value)
        {
            try
            {
                return decimal.Parse(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string EncodeToken(this string token)
        {
            char a = (char)(Convert.ToUInt16(token[50]) + 1);
            char b = (char)(Convert.ToUInt16(token[51]) + 1);
            char c = (char)(Convert.ToUInt16(token[52]) + 1);
            char d = (char)(Convert.ToUInt16(token[63]) + 1);
            char e = (char)(Convert.ToUInt16(token[65]) + 1);
            char f = (char)(Convert.ToUInt16(token[68]) + 1);
            char g = (char)(Convert.ToUInt16(token[73]) + 1);
            char h = (char)(Convert.ToUInt16(token[81]) + 1);
            StringBuilder sb = new StringBuilder(token);
            sb[50] = a;
            sb[51] = b;
            sb[52] = c;
            sb[63] = d;
            sb[65] = e;
            sb[68] = f;
            sb[73] = g;
            sb[81] = h;
            return sb.ToString();
        }

        public static string GetLastNumberString(this string value)
        {
            Stack<char> number = new Stack<char>();
            bool hasNumber = false;
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return string.Empty;
                }

                int length = value.Length;
                while (--length > 0)
                {
                    if (char.IsDigit(value[length]))
                    {
                        number.Push(value[length]);

                        hasNumber = true;
                    }
                    else if (hasNumber)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }

            return new string(number.ToArray());
        }

        public static string FormatPriceTwoDecimal(this decimal value)
        {
            string many = string.Empty;
            try
            {
                if (value >= 0)
                {
                    many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));
                }
                else
                {
                    value = Math.Abs(value);
                    many = '(' + value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES")) + ')';
                }
            }
            catch (Exception)
            {
                // FileLog.WriteLog(string.Empty, ex);
            }

            return many;
        }

        public static byte[] ToByteArray(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public static string ToBase64(this string path)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(path);
            string result = Convert.ToBase64String(imageArray);
            return result;
        }

        public static string GetDomain(this IHttpContextAccessor accessor)
        {
            return $"{accessor.HttpContext.Request.Scheme}://{accessor.HttpContext.Request.Host}";
        }

        public static string GetTenHinhThucHoaDonCanThayThe(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var HinhThucHoaDonCanThayThe = JsonConvert.DeserializeObject<LyDoThayTheModel>(value).HinhThucHoaDonCanThayThe;
                if (HinhThucHoaDonCanThayThe != null)
                {
                    return string.Format("{0}. {1}", HinhThucHoaDonCanThayThe.GetValueOrDefault().ToString(), ((HinhThucHoaDonCanThayThe)HinhThucHoaDonCanThayThe).GetDescription());
                }
                else
                {
                    return "Hóa đơn điện tử";
                }
            }

            return "Hóa đơn điện tử";
        }

        public static string GetTenHinhThucHoaDonBiDieuChinh(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var HinhThucHoaDonBiDieuChinh = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(value).HinhThucHoaDonBiDieuChinh;
                if (HinhThucHoaDonBiDieuChinh != null)
                {
                    return string.Format("{0}. {1}", HinhThucHoaDonBiDieuChinh.GetValueOrDefault().ToString(), ((HinhThucHoaDonCanThayThe)HinhThucHoaDonBiDieuChinh).GetDescription());
                }
                return "Hóa đơn điện tử";
            }

            return "Hóa đơn điện tử";
        }

        public static string GetNoiDungLyDoDieuChinh(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var LyDoDieuChinh = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(value);
                return LyDoDieuChinh.LyDo ?? string.Empty;
            }

            return string.Empty;
        }

        public static HinhThucMauHoaDon GetBoMauHoaDonFromHoaDonDienTu(this HoaDonDienTuViewModel model, bool isBanTheHien = true)
        {
            bool isVND = model.IsVND ?? true;
            bool isChietKhau = model.TongTienChietKhauQuyDoi != 0 || model.TongTienChietKhau != 0;
            HinhThucMauHoaDon loai = HinhThucMauHoaDon.HoaDonMauCoBan;

            if (isBanTheHien)
            {
                if (isChietKhau)
                {
                    loai = HinhThucMauHoaDon.HoaDonMauCoBan_CoChietKhau;
                }

                if (isVND == false)
                {
                    loai = HinhThucMauHoaDon.HoaDonMauCoBan_NgoaiTe;
                }

                if (!isVND && isChietKhau)
                {
                    loai = HinhThucMauHoaDon.HoaDonMauCoBan_All;
                }
            }
            else
            {
                loai = HinhThucMauHoaDon.HoaDonMauDangChuyenDoi;

                if (isChietKhau)
                {
                    loai = HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_CoChietKhau;
                }

                if (isVND == false)
                {
                    loai = HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_NgoaiTe;
                }

                if (!isVND && isChietKhau)
                {
                    loai = HinhThucMauHoaDon.HoaDonMauDangChuyenDoi_All;
                }
            }

            return loai;
        }

        public static string NameOfEmum<T>(this T value) where T : Enum
        {
            string result = Enum.GetName(typeof(T), value);
            return result;
        }

        public static string LimitLine(this string value, int limit)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] array = value.Split("\n");
                return string.Join("\n", array.Count() > 2 ? array.Take(limit) : array);
            }
            return string.Empty;
        }

        public static bool IsOverLimit(this string value, int limit)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string[] array = value.Split("\n");
                return array.Count() > limit;
            }

            return false;
        }

        public static bool IsHoaDonCoMa(this string input)
        {
            var cha = input[1];
            return cha == 'C';
        }

        public static string GetThueHasPer(this string value)
        {
            if (value == "KCT" || value == "KKKNT")
            {
                return value;
            }
            else
            {
                if (value == "3.5" || value == "7")
                {
                    return $"KHAC:{value}%";
                }
            }

            return value + "%";
        }

        public static string GetThueGTGTByNgayHoaDon(DateTime ngayHoaDon, string thueGTGT)
        {
            if (string.IsNullOrEmpty(thueGTGT))
            {
                return string.Empty;
            }

            if (thueGTGT == "KCT" || thueGTGT == "KKKNT")
            {
                return thueGTGT;
            }
            else if (thueGTGT == "3.5" || thueGTGT == "7")
            {
                var thueDec = decimal.Parse(thueGTGT.Replace(".", ","));
                if ((thueDec == 3.5M || thueDec == 7) &&
                    (ngayHoaDon.Date.Month == 11 || ngayHoaDon.Date.Month == 12) &&
                    ngayHoaDon.Date.Year == 2021)
                {
                    thueDec = thueDec * 100 / 70;
                    return $"{thueDec:G29}% x 70%";
                }

                return $"{thueDec:G29}%";
            }
            else if (thueGTGT.Contains("KHAC"))
            {
                var thueVal = thueGTGT.Split(":")[1].Replace(".", ",");
                return $"{thueVal}%";
            }
            else
            {
                return thueGTGT + "%";
            }
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Compress(string uncompressedString)
        {
            byte[] compressedBytes;

            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using (var compressedStream = new MemoryStream())
                {
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }

                    compressedBytes = compressedStream.ToArray();
                }
            }

            return Convert.ToBase64String(compressedBytes);
        }

        public static string Decompress(string compressedString)
        {
            byte[] decompressedBytes;

            using (var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString)))
            {
                using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        decompressorStream.CopyTo(decompressedStream);

                        decompressedBytes = decompressedStream.ToArray();
                    }
                }
            }

            return Encoding.UTF8.GetString(decompressedBytes);
        }

        public static string GetTenHinhThucThanhToan(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return string.Empty;
            }

            if (Enum.TryParse(id, out Services.Enums.HinhThucThanhToan hinhThucThanhToan))
            {
                return hinhThucThanhToan.GetDescription();
            }
            else
            {
                return id;
            }
        }

        public static string GetBase64ImageMauHoaDon(this string value, LoaiThietLapMacDinh loai, string path, List<FileData> fileDatas)
        {
            if (!string.IsNullOrEmpty(value) && (loai == LoaiThietLapMacDinh.Logo || loai == LoaiThietLapMacDinh.HinhNenTaiLen))
            {
                var fullPath = Path.Combine(path, value);

                // Nếu file path is not exists then generate file from binary
                if (!File.Exists(fullPath))
                {
                    var fileData = fileDatas.FirstOrDefault(x => x.FileName == value);
                    if (fileData != null)
                    {
                        File.WriteAllBytes(fullPath, fileData.Binary);
                    }
                }

                if (File.Exists(fullPath))
                {
                    var contentType = $"data:{MimeTypes.GetMimeType(fullPath)};base64,";
                    byte[] imageArray = File.ReadAllBytes(fullPath);
                    string base64ImageRepresentation = contentType + Convert.ToBase64String(imageArray);
                    return base64ImageRepresentation;
                }
            }

            return value;
        }

        public static int? ParseIntNullable(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var result = int.Parse(value);
            return result;
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static Tuple<string, string> GetTenKySo(this string tenDonVi, LoaiNgonNgu loaiNgonNgu = LoaiNgonNgu.TiengViet)
        {
            if (string.IsNullOrEmpty(tenDonVi))
            {
                return new Tuple<string, string>(string.Empty, string.Empty);
            }

            List<string> ten1s = new List<string>();
            List<string> ten2s = new List<string>();

            var array = tenDonVi.Split(" ");
            int count = 0;
            foreach (var item in array)
            {
                count += item.Count();
                if (count > (loaiNgonNgu == LoaiNgonNgu.TiengViet ? 25 : 20))
                {
                    ten2s.Add(item);
                }
                else
                {
                    ten1s.Add(item);
                }
            }

            string ten1 = string.Join(" ", ten1s);
            string ten2 = string.Join(" ", ten2s);

            var resunt = new Tuple<string, string>(ten1, ten2);
            return resunt;
        }

        public static bool CheckValidKyHieuHoaDon(this string value)
        {
            if (string.IsNullOrEmpty(value) || (value.Length != 6))
            {
                return false;
            }

            char[] char4 = { 'T', 'D', 'L', 'M', 'N', 'B', 'G', 'H' };

            for (int i = 0; i < value.Length; i++)
            {
                var item = value[i];

                if (i == 0)
                {
                    if (item != 'C' && item != 'K')
                    {
                        return false;
                    }
                }
                else if (i == 1 || i == 2)
                {
                    if (item < 48 || item > 57)
                    {
                        return false;
                    }
                }
                else if (i == 3)
                {
                    if (!char4.Contains(item))
                    {
                        return false;
                    }
                }
                else
                {
                    if (item < 65 || item > 90)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool CheckValidSoHoaDon(this string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length > 8)
            {
                return false;
            }

            var result = int.TryParse(value, out _);
            return result;
        }

        public static bool CheckValidMaSoThue(this string value)
        {
            if (string.IsNullOrEmpty(value) || (value.Length != 10 && value.Length != 14))
            {
                return false;
            }

            if (value.IndexOf('-') != -1)
            {
                if (value.IndexOf('-') != 10)
                {
                    return false;
                }
                else
                {
                    var split = value.Split('-');
                    return split.All(x => x.CheckValidNumber());
                }
            }
            else
            {
                if (value.Length != 10 || !value.CheckValidNumber())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// check thuế có hợp lệ không
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckValidThueGTGT(this string value)
        {
            if (value != "0" && value != "5" && value != "8" && value != "10" && value != "KCT" && value != "KKKNT" && !value.CheckValidThueKhac())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// chech định dạng thuế khác
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckValidThueKhac(this string value)
        {
            if (!(value.StartsWith("KHAC:") && value.EndsWith("%")))
            {
                return false;
            }

            var splitThue = value.Substring(5, value.Length - 6).Split(".");
            if (splitThue.Length > 2)
            {
                return false;
            }

            return splitThue.All(x => CheckValidNumberThue(x));
        }

        /// <summary>
        /// convert thue DB to view
        /// </summary>
        public static string ConvertDBThueToView(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (value == "KCT" || value == "KKKNT")
            {
                return value;
            }
            else
            {
                return value + "%";
            }
        }

        /// <summary>
        /// convert value thue excel to thue db
        /// </summary>
        public static string ConvertThueExcetToDB(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (value.CheckValidThueKhac())
            {
                var thue = value.Substring(5, value.Length - 6).Replace(".", ",");
                var thueDec = decimal.Parse(thue, NumberStyles.Float, CultureInfo.CreateSpecificCulture("es-ES"));
                thue = thueDec.ToString("G29").Replace(",", ".");

                return "KHAC:" + thue;
            }

            return value;
        }

        public static decimal ConvertStringToDecimal(this string value)
        {
            var result = decimal.Parse(value.Replace(".", ","), NumberStyles.Float, CultureInfo.CreateSpecificCulture("es-ES"));
            return result;
        }

        public static bool CheckValidNumberThue(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return Regex.IsMatch(value, "^[0-9]{1,2}$");
        }

        public static bool CheckValidNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return Regex.IsMatch(value, "^[0-9]*$");
        }

        public static bool CheckExcelCurrencyFormat(this string value, int precision, int scale, out decimal output)
        {
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            var decimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator;
            var result = Regex.IsMatch(value, @"^(0|-?\d{0," + precision + @"}(\" + decimalSeparator + @"\d{0," + scale + @"})?)$");
            if (result)
            {
                decimal.TryParse(value, out output);
            }
            else
            {
                output = 0;
            }
            return result;
        }

        public static DateTime? ParseExactCellDate(this object input, out bool isValidDate)
        {
            if (string.IsNullOrEmpty((input + "").Trim()))
            {
                isValidDate = false;
                return null;
            }

            string output = input.ToString().Trim();
            DateTime? result = null;
            Type type = input.GetType();
            if (type == typeof(DateTime))
            {
                isValidDate = DateTime.TryParse(output, out DateTime outDateTryParse);
                if (isValidDate)
                {
                    result = outDateTryParse;
                }
            }
            else
            {
                string[] dateFormats = {
                    "dd/MM/yyyy",
                    "d/MM/yyyy",
                    "d/M/yyyy",
                    "dd/M/yyyy"
                };

                isValidDate = DateTime.TryParseExact(input.ToString(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outDateTryParse);
                if (isValidDate)
                {
                    result = outDateTryParse;
                }
            }
            return result;
        }

        public static string GeneratePassword()
        {
            string text;
            try
            {
                text = $"@{DateTime.Now:dd-MM-yyyy}#";
                // 1st
                text = Base64Encode(text);
                // 2st
                text = Base64Encode(text);
            }
            catch (Exception)
            {
                throw;
            }

            return text;
        }

        public static (string, string) GetErrorWhenSendEmailToClient(string message)
        {
            string moTa;
            string huongDanXuLy = "Vui lòng kiểm tra lại";
            if (message.Contains("may not exist"))
            {
                moTa = "Email người nhận không tồn tại";
                huongDanXuLy = "Vui lòng kiểm tra lại";
            }
            else if (message.Contains("Incorrect authentication data"))
            {
                moTa = "Email gửi hoặc mật khẩu không đúng";
            }
            else if (message.Contains("No such host is known"))
            {
                moTa = "Tên máy chủ không đúng";
            }
            else
            {
                moTa = "Lỗi hệ thống";
                huongDanXuLy = "Vui lòng liên hệ với bộ phẫn hỗ trợ để được trợ giúp";
            }

            return (moTa, huongDanXuLy);
        }
    }
}
