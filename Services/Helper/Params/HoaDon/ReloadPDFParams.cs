namespace Services.Helper.Params.HoaDon
{
    public class ReloadPDFParams
    {
        public string Password { get; set; }
        public string KyHieu { get; set; }
        public string SoHoaDon { get; set; }
        public string MaSoThue { get; set; }
    }

    public class ReloadPDFResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
