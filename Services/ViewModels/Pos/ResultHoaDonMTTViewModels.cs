using System.Collections.Generic;

namespace Services.ViewModels.Pos
{
    public class ResultHoaDonMTTViewModels
    {
        public string MaTraCuu { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public int? TrangThaiQuyTrinh { get; set; }
        public string Error { get; set; }
        public long? SoHoaDon { get; set; }
        public long? BienLaiId { get; set; }
        public string PosCustomerURL { get; set; }
        public List<ResultHoaDonMTTViewModels> ListHoaDons { get; set; }

    }
}
