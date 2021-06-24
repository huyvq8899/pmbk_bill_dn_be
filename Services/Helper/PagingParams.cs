﻿namespace ManagementServices.Helper
{
    /// <summary>
    /// Lớp này dữ nguyên, ko thêm trường gì, muốn thêm thì tạo lớp khác rồi kế thừa lớp này
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
    }

	public HoaDonParams : PagingParams{
        public int? LoaiHoaDon { get; set; }
        public int? TrangThaiHoaDonDienTu { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public bool? TrangThaiChuyenDoi { get; set; }
	}
}
