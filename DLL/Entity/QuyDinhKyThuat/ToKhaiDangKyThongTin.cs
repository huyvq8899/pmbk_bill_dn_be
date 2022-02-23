using System;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ToKhaiDangKyThongTin : ThongTinChung
    {
        /// <summary>
        /// Id tờ khai
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Phương pháp tính thuế (lấy từ trang ttnnt)
        /// </summary>
        public string PPTinh { get; set; }

        /// <summary>
        /// Tương ứng với hình thức đăng ký (true) hoặc thay đổi thông tin (false)
        /// </summary>
        public bool IsThemMoi { get; set; }

        /// <summary>
        /// Có đăng ký ủy nhiệm lập hóa đơn hay không.
        /// Chỉ sử dụng khi chọn hình thức thay đổi thông tin
        /// </summary>
        public bool NhanUyNhiem { get; set; }

        /// <summary>
        /// Loại đăng ký ủy nhiệm (1: ủy nhiệm, 2: nhận ủy nhiệm)
        /// </summary>
        public int? LoaiUyNhiem { get; set; }

        /// <summary>
        /// Tên file xml ở trạng thái chưa ký
        /// </summary>
        public string FileXMLChuaKy { get; set; }

        /// <summary>
        /// Trạng thái ký tờ khai (đã ký: true, chưa ký: false)
        /// </summary>
        public bool SignedStatus { get; set; }

        /// <summary>
        /// Ngày tạo tờ khai
        /// </summary>
        public DateTime NgayTao { get; set; }
    }
}
