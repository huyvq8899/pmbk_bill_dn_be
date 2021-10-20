using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ThongDiepChung : ThongTinChung
    {
        public string ThongDiepChungId { get; set; }
        public string MaThongDiep { get; set; }
        public int MaLoaiThongDiep { get; set; }
        public bool ThongDiepGuiDi { get; set; }
        public int? HinhThuc { get; set; }
        public int LanThu { get; set; } = 0;
        public int LanGui { get; set; } = 0;
        public int TrangThaiGui { get; set; }
        public int TrangThaiTiepNhan { get; set; }
        public string NoiNhan { get; set; }
        public DateTime? NgayGui { get; set; }
        public string IdThamChieu { get; set; } // tham chiếu đến thực thể được đóng gói trong thông điệp (thông báo, tờ khai, etc...)
        public string IdThongDiepGoc { get; set; } // trường hợp thông điệp trả về từ cơ quan thuế, chỉ đến thông điệp gốc đã gửi
    }
}
