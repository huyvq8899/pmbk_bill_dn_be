using DLL.Enums;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    [Serializable]
    public class MauHoaDonTuyChinhChiTietViewModel : ThongTinChungViewModel
    {
        public string MauHoaDonTuyChinhChiTietId { get; set; }
        public string MauHoaDonId { get; set; }
        public string GiaTri { get; set; }
        public TuyChinhChiTietModel TuyChonChiTiet { get; set; }
        public string TuyChinhChiTiet { get; set; }
        public string TenTiengAnh { get; set; }
        public string GiaTriMacDinh { get; set; }
        public KieuDuLieuThietLapTuyChinh KieuDuLieuThietLap { get; set; }
        public LoaiTuyChinhChiTiet Loai { get; set; }
        public LoaiChiTietTuyChonNoiDung LoaiChiTiet { get; set; }
        public LoaiContainerTuyChinh LoaiContainer { get; set; }
        public bool? IsParent { get; set; }
        public bool? Checked { get; set; }
        public bool? Disabled { get; set; }
        public int? CustomKey { get; set; }

        public List<MauHoaDonTuyChinhChiTietViewModel> Children { get; set; }
    }
}
