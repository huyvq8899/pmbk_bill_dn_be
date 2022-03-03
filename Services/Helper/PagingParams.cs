using Services.Helper.Params.Filter;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.QuyDinhKyThuat;
using System.Collections.Generic;

namespace ManagementServices.Helper
{
    /// <summary>
    /// Lớp này giữ nguyên, ko thêm trường gì, muốn thêm thì tạo lớp khác rồi kế thừa lớp này
    /// vd: DoiTuongParams : PagingParams
    /// </summary>
    public class PagingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string SortValue { get; set; }
        public string SortKey { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool? IsActive { get; set; }
        public List<FilterColumn> FilterColumns { get; set; }
        public List<string> MauHoaDonDuocPQ { get; set; }
    }

    public class ThongDiepChungParams : PagingParams
    {
        public int LoaiThongDiep { get; set; }
        public int? TrangThaiGui { get; set; }
        public bool? IsThongDiepGui { get; set; }
        public bool? IsPrint { get; set; }
        public ThongDiepChungViewModel Filter { get; set; }
        public ThongDiepSearch TimKiemTheo { get; set; }
        public string GiaTri { get; set; }
    }

    public class BangTongHopDuLieuHoaDonParams : PagingParams
    {
        public int? LoaiHangHoa { get; set; }
        public int? TrangThaiGui { get; set; }
        public BangTongHopSearch TimKiemTheo { get; set; }
        public BangTongHopDuLieuHoaDonViewModel Filter { get; set; }
    }
}
