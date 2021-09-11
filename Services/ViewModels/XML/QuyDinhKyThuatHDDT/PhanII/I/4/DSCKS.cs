namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4
{
    public partial class DSCKS
    {
        public CQT CQT { get; set; }
        public CCKSKhac CCKSKhac { get; set; }
    }

    public partial class CQT
    {
        /// <summary>
        /// <para>Hình thức (Hình thức, chức danh của chữ k‎ý)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string HTHuc { get; set; }
    }

    public partial class CCKSKhac
    {
    }
}
