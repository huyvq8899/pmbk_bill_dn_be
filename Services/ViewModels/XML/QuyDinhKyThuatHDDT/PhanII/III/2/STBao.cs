using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2
{
    public partial class STBao
    {
        /// <summary>
        /// <para>Số (Số thông báo)</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string So { get; set; }

        /// <summary>
        /// <para>Ngày thông báo</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NTBao { get; set; }
    }
}
