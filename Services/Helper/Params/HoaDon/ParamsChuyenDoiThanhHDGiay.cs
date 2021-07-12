using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamsChuyenDoiThanhHDGiay
    {
        public string HoaDonDienTuId { get; set; }
        public DateTime? NgayChuyenDoi { get; set; }
        public string NguoiChuyenDoiId { get; set; }
    }
}
