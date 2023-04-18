using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VII._1
{
    public partial class TDiep
    {
        public TTChungThongDiep1510 TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }
    public partial class DLieu
    {
        /// <summary>
        /// <para>MST của người nộp thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Trạng thái MST</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        [MaxLength(3)]
        public string TThai { get; set; }

        /// <summary>
        /// <para>Ngày cập nhật</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        public DateTime NCNhat { get; set; }
    }

}
