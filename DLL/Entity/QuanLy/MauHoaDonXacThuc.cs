﻿using DLL.Enums;

namespace DLL.Entity.QuanLy
{
    public class MauHoaDonXacThuc : ThongTinChung
    {
        public string MauHoaDonXacThucId { get; set; }
        public string NhatKyXacThucBoKyHieuId { get; set; }
        public byte[] FileByte { get; set; }
        public LoaiTheHienHoaDon FileType { get; set; }

        public NhatKyXacThucBoKyHieu NhatKyXacThucBoKyHieu { get; set; }
    }
}
