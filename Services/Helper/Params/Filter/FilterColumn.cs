namespace Services.Helper.Params.Filter
{
    public class FilterColumn
    {
        public string ColKey { get; set; }
        public string ColValue { get; set; }
        public FilterCondition FilterCondition { get; set; }
        public bool? IsFilter { get; set; }
    }

    public enum FilterCondition
    {
        Chua = 1,
        KhongChua = 2,
        Bang = 3,
        Khac = 4,
        BatDau = 5,
        KetThuc = 6,
        NhoHon = 7,
        NhoHonHoacBang = 8,
        LonHon = 9,
        LonHonHoacBang = 10,
        Trong = 11,
        KhongTrong = 12
    }
}
