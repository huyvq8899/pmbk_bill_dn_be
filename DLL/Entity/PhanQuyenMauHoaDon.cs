﻿using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;

namespace DLL.Entity
{
    public class PhanQuyenMauHoaDon
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public virtual BoKyHieuHoaDon BoKyHieuHoaDon { get; set; }
    }
}
