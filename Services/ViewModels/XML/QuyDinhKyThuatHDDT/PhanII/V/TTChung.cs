using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.V
{
    public partial class TTChung
    {
        /// <summary>
        /// <para>Phiên bản XML (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string PBan { get; set; } = "2.0.1";

        /// <summary>
        /// <para>Mẫu số (Mẫu số tờ khai)</para>
        /// <para>Độ dài tối đa: 15</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VIII kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string MSo { get; set; }

        /// <summary>
        /// <para>Tên (Tên tờ khai)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Loại kỳ tính thuế</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VII kèm theo Quyết định số 1450/QĐ-TCT ngày 7/10/2021 </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string LKTThue { get; set; }

        /// <summary>
        /// <para>Kỳ tính thuế</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục VII kèm theo Quyết định số 1450/QĐ-TCT ngày 7/10/2021) </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string KTThue { get; set; }

        /// <summary>
        /// <para>Số tờ khai</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para> Số </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public int STKhai { get; set; }

        /// <summary>
        /// <para>Địa danh</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para> Chuỗi ký tự </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string DDanh { get; set; }

        /// <summary>
        /// <para>Ngày lập</para>
        /// <para> Ngày </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NLap { get; set; }


        /// <summary>
        /// <para>Tên NNT</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para> Chuỗi ký tự </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string TNNT { get; set; }

        /// <summary>
        /// <para>Mã số thuế NNT</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para> Chuỗi ký tự </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Tên đại lý thuế</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para> Chuỗi ký tự </para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(400)]
        public string TDLThue { get; set; }
        /// <summary>
        /// <para>Mã số thuế đại lý thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para> Chuỗi ký tự </para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MSTDLThue { get; set; }

        /// <summary>
        /// <para>Đơn vị tiền tệ</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string DVTTe { get; set; }

    }
}
