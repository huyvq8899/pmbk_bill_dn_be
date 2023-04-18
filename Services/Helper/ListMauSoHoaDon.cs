using System.Collections.Generic;
using System.Linq;

namespace Services.Helper
{
    public static class ListMauSoHoaDon
    {
        public static List<MauSoHoaDon> GetList()
        {
            List<MauSoHoaDon> result = new List<MauSoHoaDon>
            {
               new MauSoHoaDon { MauSo = "01GTKT0/001", TenMauSo = "Hóa đơn giá trị gia tăng (HĐ điện tử)", Status = true },
               new MauSoHoaDon { MauSo = "01GTKT3/001", TenMauSo = "Hóa đơn giá trị gia tăng", Status = true },
               new MauSoHoaDon { MauSo = "02GTTT0/001", TenMauSo = "Hóa đơn bán hàng (HĐ điện tử)", Status = true },
               new MauSoHoaDon { MauSo = "02GTTT3/001", TenMauSo = "Hóa đơn bán hàng", Status = true },
               new MauSoHoaDon { MauSo = "03XKNB3/001", TenMauSo = "Phiếu xuất kho kiêm vận chuyển hàng hóa nội bộ", Status = true },
               new MauSoHoaDon { MauSo = "04HGDL3/001", TenMauSo = "Phiếu xuất kho gửi bán hàng đại lý", Status = true },
               new MauSoHoaDon { MauSo = "07KPTQ3/001", TenMauSo = "Hóa đơn bán hàng (dành cho tổ chức, cá nhân trong khu phi thuế quan)", Status = true }
            };

            return result;
        }
    }

    public class MauSoHoaDon
    {
        public string MauSo { get; set; }
        public string TenMauSo { get; set; }
        public bool? Status { get; set; }
    }
}
