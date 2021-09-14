using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1
{
    public partial class TTChung
    {
        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string PBan { get; set; }

        /// <summary>
        /// <para>Số bảng tổng hợp dữ liệu (Số thứ tự bảng tổng hợp dữ liệu)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public int SBTHDLieu { get; set; }

        /// <summary>
        /// <para>Loại kỳ dữ liệu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (T: tháng, Q: quý, N: năm)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string LKDLieu { get; set; }

        /// <summary>
        /// <para>Lần đầu</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự(Định dạng trường kỳ theo tháng, quý: N1N2/Y1Y2Y3Y4, Định dạng trường kỳ theo ngày: N1N2/N3N4/Y1Y2Y3Y4)(Chú thích: KDLieu.cs)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string KDLieu { get; set; }

        /// <summary>
        /// <para>Lần đầu</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public LDau LDau { get; set; }

        /// <summary>
        /// <para>Bổ sung lần thứ)</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc nếu LDau = 0</para>
        /// </summary>
        public int? BSLThu { get; set; }

        /// <summary>
        /// <para>Ngày lập</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string NLap { get; set; }

        /// <summary>
        /// <para>Tên NNT</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TNNT { get; set; }

        /// <summary>
        /// <para>Mã số thuế NNT</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Hóa đơn đặt in</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public HDDIn HDDIn { get; set; }

        /// <summary>
        /// <para>Loại hàng hóa (Loại hàng hóa, dịch vụ kinh doanh)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public LHHoa LHHoa { get; set; }
    }
}
