using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h
{
    public partial class TTin
    {
        /// <summary>
        /// <para>Tên trường</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string TTruong { get; set; }

        /// <summary>
        /// <para>Kiểu dữ liệu</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục III kèm theo Quy định này)</para>
        /// <para>1. string (Chuỗi ký tự)</para>
        /// <para>2. numberic (Số)</para>
        /// <para>3. dateTime (Ngày giờ)</para>
        /// <para>4. date (Ngày)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string KDLieu { get; set; }

        /// <summary>
        /// <para>Dữ liệu</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string DLieu { get; set; }
    }
}
