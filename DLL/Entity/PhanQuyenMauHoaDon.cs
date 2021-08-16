using DLL.Entity.DanhMuc;

namespace DLL.Entity
{
    public class PhanQuyenMauHoaDon
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string MauHoaDonId { get; set; }
        public virtual MauHoaDon MauHoaDon { get; set; }
    }
}
