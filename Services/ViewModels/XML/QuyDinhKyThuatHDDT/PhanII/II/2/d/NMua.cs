using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d
{
    public partial class NMua
    {
        /// <summary>
        /// <para>Tên (Người mua tài sản NN)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp  người mua là cơ quan, tổ chức, đơn vị, doanh nghiệp)</para>
        /// </summary>
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp  người mua là cơ quan, tổ chức, đơn vị, doanh nghiệp không có Mã đơn vị quan hệ ngân sách)</para>
        /// </summary>
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã đơn vị quan hệ ngân sách (Mã số đơn vị có quan hệ với ngân sách của đơn vị)</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp  người mua là cơ quan, tổ chức, đơn vị, doanh nghiệp không có Mã số thuế)</para>
        /// </summary>
        [MaxLength(7)]
        public string MDVQHNSach { get; set; }

        /// <summary>
        /// <para>Địa chỉ</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp người mua là cơ quan, tổ chức, đơn vị, doanh nghiệp)</para>
        /// </summary>
        [MaxLength(400)]
        public string DChi { get; set; }

        /// <summary>
        /// <para>Số điện thoại</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(20)]
        public string SDThoai { get; set; }

        /// <summary>
        /// <para>Địa chỉ thư điện tử</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string DCTDTu { get; set; }

        /// <summary>
        /// <para>Họ và tên người mua hàng</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp  người mua với tư cách cá nhân)</para>
        /// </summary>
        [MaxLength(100)]
        public string HVTNMHang { get; set; }

        /// <summary>
        /// <para>Số tài khoản ngân hàng</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(30)]
        public string STKNHang { get; set; }

        /// <summary>
        /// <para>Tên ngân hàng</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(400)]
        public string TNHang { get; set; }

        /// <summary>
        /// <para>Địa điểm vận chuyển hàng đến</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp  tài sản là hàng hóa nhập khẩu bị tịch thu)</para>
        /// </summary>
        [MaxLength(400)]
        public string DDVCHDen { get; set; }

        /// <summary>
        /// <para>Thời gian vận chuyển hàng đến từ</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (Đối với trường hợp  tài sản là hàng hóa nhập khẩu bị tịch thu)</para>
        /// </summary>
        public string TGVCHDTu { get; set; }

        /// <summary>
        /// <para>Thời gian vận chuyển hàng đến đến</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (Đối với trường hợp  tài sản là hàng hóa nhập khẩu bị tịch thu)</para>
        /// </summary>
        public string TGVCHDDen { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
