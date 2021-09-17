namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a
{
    public partial class DSCKS
    {
        /// <summary>
        /// chứa thông tin chữ ký số người bán hoặc chữ ký số của đơn vị nhận ủy nhiệm  
        /// (ký trên thẻ HDon\DLHDon và thẻ HDon\DSCKS\Nban\Object\SignatureProperties\SignatureProperty)
        /// <para>Thẻ bên trong: Signature</para>
        /// <para>Chữ ký số người bán (Thực hiện theo quy định tại Điều 10 của Nghị định số 123/2020/NĐ-CP)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 của Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string NBan { get; set; }

        /// <summary>
        /// chứa thông tin chữ ký số người mua(nếu có) 
        /// (chỉ ký trên thẻ HDon\DLHDon hoặc các thẻ con của thẻ HDon\DLHDon(nếu cần) 
        /// hoặc các thẻ con thuộc thẻ HDon\DSCKS\NMua(nếu cần))
        /// <para>Thẻ bên trong: Signature</para>
        /// <para>Chữ ký số người mua (Thực hiện theo quy định tại Điều 10 của Nghị định số 123/2020/NĐ-CP)</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string NMua { get; set; }

        /// <summary>
        /// chứa các chữ ký số khác (nếu có).
        /// </summary>
        public string CCKSKhac { get; set; }

        /// <summary>
        /// Với hóa đơn điện tử đủ điều kiện cấp mã, 
        /// hệ thống của cơ quan thuế trả về hóa đơn điện tử và bổ sung thẻ CQT (đặt bên trong thẻ HDon\DSCKS) 
        /// chứa thông tin chữ ký số của cơ quan thuế 
        /// (ký trên thẻ HDon\MCCQT và thẻ TBao\DSCKS\CQT\Object\SignatureProperties\SignatureProperty)
        /// <para>Thẻ bên trong: Signature</para>
        /// <para>Chữ ký số cơ quan thuế</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        // public string CQT { get; set; }
    }
}
