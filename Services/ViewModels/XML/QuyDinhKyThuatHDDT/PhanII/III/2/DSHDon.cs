using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2.DSLDKTNhanHDon;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2
{
    public partial class HDon
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(4)]
        public int? STT { get; set; }

        /// <summary>
        /// <para>Mã CQT cấp</para>
        /// <para>Độ dài tối đa: 34</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp là hóa đơn không có mã của CQT)</para>
        /// </summary>
        [MaxLength(34)]
        public string MCQTCap { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)(Chú thích: KHMSHDon.cs)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(11)]
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự/para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(8)]
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn (Số hóa đơn điện tử)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự/para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP))</para>
        /// </summary>
        [MaxLength(8)]
        public string SHDon { get; set; }

        /// <summary>
        /// <para>Ngày lập (Ngày lập hóa đơn)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NLap { get; set; }

        /// <summary>
        /// <para>Loại áp dụng hóa đơn điện tử</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public LADHDDT LADHDDT { get; set; }

        /// <summary>
        /// <para>Tính chất thông báo (Hủy/Điều chỉnh/Thay thế/Giải trình)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public TCTBao TCTBao { get; set; }

        /// <summary>
        /// <para>Trạng thái tiếp nhận của cơ quan thuế</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (1: Tiếp nhận, 2: Không tiếp nhận)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public TTTNCCQT TTTNCCQT { get; set; }

        public List<LDo> DSLDKTNhan { get; set; }
    }

}
