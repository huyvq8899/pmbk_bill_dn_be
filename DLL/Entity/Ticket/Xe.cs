using DLL.Entity.QuanLyHoaDon;
using System.Collections.Generic;

namespace DLL.Entity.Ticket
{
    public class Xe : ThongTinChung
    {
        public string XeId { get; set; }
        public string MaXe { get; set; }
        public string SoXe { get; set; }
        public string LoaiXe { get; set; }

        public List<User_Xe> User_Xes { get; set; }
        public List<HoaDonDienTu> HoaDonDienTus { get; set; }
    }
}
