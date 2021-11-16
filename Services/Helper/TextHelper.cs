using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using Services.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
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
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
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

        public static string ConvertToInWord(this decimal total, string cachDocSo0HangChuc, string cachDocSoHangNghin, bool hienThiSoChan)
        {

            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "nghìn", "", "", "triệu", "", "", "tỷ", "", "", "nghìn", "", "", "triệu" };
                string nstr = total.ToString();

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
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ", cachDocSo0HangChuc).Replace("mươi", "mươi").Replace("trăm", "trăm").Replace("mười", "mười").Replace("nghìn", cachDocSoHangNghin) + ((total % 1000 == 0 && hienThiSoChan) ? " chẵn" : string.Empty);

            }
            catch
            {
                return "";
            }
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
            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            if (decimal.TryParse(value, NumberStyles.Currency, culture, out _))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidInt(this string value)
        {
            if (Int32.TryParse(value, out _))
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

        public static string FormatPriceNew(this decimal value)
        {
            string many = string.Empty;
            try
            {
                if (value >= 0)
                {
                    value = Math.Round(value);
                    many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));
                    int idx = many.IndexOf(",");
                    if (idx > 0)
                    {
                        many = many.Substring(0, idx);
                    }
                }
                else
                {
                    value = Math.Abs(value);
                    value = Math.Round(value);
                    many = value.ToString("N02", CultureInfo.CreateSpecificCulture("es-ES"));
                    int idx = many.IndexOf(",");
                    if (idx > 0)
                    {
                        many = '(' + many.Substring(0, idx) + ')';
                    }
                }
            }
            catch (Exception)
            {
                // FileLog.WriteLog(string.Empty, ex);
            }

            return many;
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
            var cha = input[0];
            return cha == 'C';
        }

        public static string GetThueHasPer(this string value)
        {
            if (value == "KCT" || value == "KKKNT")
            {
                return value;
            }

            return value + "%";
        }

        public static string GetThueGTGTByNgayHoaDon(DateTime ngayHoaDon, string thueGTGT)
        {
            if (thueGTGT == "KCT" || thueGTGT == "KKKNT")
            {
                return "\\";
            }
            else if (thueGTGT == "3.5" || thueGTGT == "7")
            {
                var thueDec = decimal.Parse(thueGTGT.Replace(".", ","));
                if ((thueDec == 3.5M || thueDec == 7) &&
                    (ngayHoaDon.Date.Month == 11 || ngayHoaDon.Date.Month == 12) &&
                    ngayHoaDon.Date.Year == 2021)
                {
                    thueDec = thueDec * 100 / 70;
                    return $"{thueDec:G29}% * 70%";
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

        /// <summary>
        /// get thuế gtgt chung từ thuế chi tiết
        /// </summary>
        /// <param name="thueGTGTs"></param>
        /// <returns></returns>
        public static string GetThueChungFromChiTiet(DateTime ngayHoaDon, List<string> thueGTGTs)
        {
            var thueSos = thueGTGTs.Where(x => x != "KCT" && x != "KKKNT")
                .Select(x => new HoaDonDienTuChiTietViewModel
                {
                    ThueGTGT = x
                })
                .ToList();

            foreach (var item in thueSos)
            {
                if (item.ThueGTGT.Contains("KHAC"))
                {
                    var sKhacVal = item.ThueGTGT.Split(":")[1];
                    item.ThueGTGT = sKhacVal.Replace(".", ",");
                }
            }

            if (thueSos.Any())
            {
                var max = thueSos.Select(x => decimal.Parse(x.ThueGTGT)).Max();

                /// trong khoảng 11/2021 -> 12/2021 nếu thuế = 3,5 hoặc 7 thì giảm 70%
                if ((max == 3.5M || max == 7) &&
                    (ngayHoaDon.Date.Month == 11 || ngayHoaDon.Date.Month == 12) &&
                    ngayHoaDon.Date.Year == 2021)
                {
                    max = max * 100 / 70;
                    return $"{max:G29}% * 70%";
                }
                else
                {
                    return $"{max:G29}%";
                }
            }

            return "\\";
        }
    }
}
