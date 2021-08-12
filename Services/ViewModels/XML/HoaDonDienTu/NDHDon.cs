using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.HoaDonDienTu
{
    /// <summary>
    /// Chứa nội dung hóa đơn, bao gồm: Thông tin người bán, người mua, danh sách hàng hóa, dịch vụ và thông tin thanh toán
    /// của hóa đơn
    /// </summary>
    public class NDHDon
    {
        public NBan NBan { set; get; }

        public NMua NMua { set; get; }

        //public DSHHDVu DSHHDVu { set; get; }
        public List<HHDVu> DSHHDVu { set; get; }
        public TToan TToan { set; get; }

        public TTKhac TTKhac { set; get; }
    }
}
