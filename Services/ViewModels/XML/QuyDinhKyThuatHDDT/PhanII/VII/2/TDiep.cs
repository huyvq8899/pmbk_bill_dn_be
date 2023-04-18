using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VII._2
{
    public partial class TDiep
    {
        public TTChungThongDiep1510 TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }
    public partial class DLieu
    {
        /// <summary>
        /// <para>MST của người nộp thuế trên quyết định/thông báo</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã CQT ban hành</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        [MaxLength(5)]
        public string MCQT { get; set; }

        /// <summary>
        /// <para>Thông báo ngừng sử dụng</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        [MaxLength(3)]
        ///Thông báo ngừng sử dụng : 1, Thông báo tiếp tục sử dụng : 2, Quyết định cưỡng chế : 3, Quyết định chấm dứt hiệu lực : 4
        public string LTBao { get; set; }

        /// <summary>
        /// <para>Số thông báo</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string STBao { get; set; }

        /// <summary>
        /// <para>Ngày thông báo</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        [Required]
        public DateTime NTBao { get; set; }

        /// <summary>
        /// <para>Ngày hiệu lực từ</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NHLTu { get; set; }

        /// <summary>
        /// <para>Ngày hiệu lực đến</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NHLDen { get; set; }

        /// <summary>
        /// <para>Số thông báo liên quan</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// Bắt buộc với Loại: Quyết định chấm dứt hiệu lực quyết định cưỡng chế, Thông báo tiếp tục sử dụng
        [Required]
        [MaxLength(50)]
        public string STBLQuan { get; set; }

        /// <summary>
        /// <para>Ngày thông báo liên quan</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// Bắt buộc với Loại: Quyết định chấm dứt hiệu lực quyết định cưỡng chế, Thông báo tiếp tục sử dụng
        [Required]
        public DateTime NTBLQuan { get; set; }

        /// <summary>
        /// <para>Ngày cập nhật</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NCNhat { get; set; }
    }
}
