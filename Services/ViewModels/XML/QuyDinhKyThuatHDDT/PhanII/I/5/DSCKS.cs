using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5
{
    public partial class DSCKS
    {
        public CQT CQT { get; set; }
        public CCKSKhac CCKSKhac { get; set; }
    }

    public partial class CQT
    {
        [MaxLength(100)]
        public string HThuc { get; set; }
    }

    public partial class CCKSKhac
    {
    }
}
