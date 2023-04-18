using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._6
{
    public partial class DSCKS
    {
        /// <summary>
        /// CQT chứa thông tin chữ ký số của cơ quan thuế (Ký trên thẻ TBao\DLTBao).
        /// </summary>
        public CQT CQT { get; set; }
        /// <summary>
        /// chứa các chữ ký số khác (nếu có)
        /// </summary>
        public CCKSKhac CCKSKhac { get; set; }
    }

    public partial class CQT
    {
        /// <summary>
        /// <para> Hình thức (Hình thức, chức danh của chữ k‎ý)</para>
        /// <para> Độ dài tối đa: 100</para>
        /// <para> Không bắt buộc </para>
        /// </summary>
        [MaxLength(100)]
        public string HThuc { get; set; }
    }

    public partial class CCKSKhac
    {

    }
}
