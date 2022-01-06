namespace Services.Helper.Params.HoaDon
{
    public class KetQuaCapSoHoaDon
    {
        public KetQuaCapSoHoaDon()
        {
            IsYesNo = false;
            IsCheckToKhaiMoiNhat = false;
        }

        public bool? IsCheckToKhaiMoiNhat { get; set; }
        public bool? IsCheckHetHieuLuc { get; set; }
        public string TitleMessage { get; set; }
        public int? SoHoaDon { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool? IsYesNo { get; set; }
    }
}
