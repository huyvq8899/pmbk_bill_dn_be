namespace Services.Helper.Params.HoaDon
{
    public class KetQuaCapSoHoaDon
    {
        public KetQuaCapSoHoaDon()
        {
            IsYesNo = false;
        }

        public string TitleMessage { get; set; }
        public int? SoHoaDon { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool? IsYesNo { get; set; }
    }
}
