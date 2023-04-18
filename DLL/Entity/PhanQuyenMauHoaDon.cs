using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using System.Collections.Generic;

namespace DLL.Entity
{
    public class PhanQuyenMauHoaDon
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public virtual BoKyHieuHoaDon BoKyHieuHoaDon { get; set; }
        public string MauHoaDonIds { get; set; }
        //public virtual List<MauHoaDon> MauHoaDons { get; set; }
    }
}
