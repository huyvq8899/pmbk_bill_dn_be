using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class DSCTSSDung
    {
        public List<CTS> CTS { get; set; }
    }

    public partial class CTS
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

        /// <summary>
        /// <para>Tên tổ chức (Cơ quan chứng thực/cấp/công nhận chữ ký số, chữ ký điện tử)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TTChuc { get; set; }

        /// <summary>
        /// <para>Seri (Số sê-ri chứng thư số)</para>
        /// <para>Độ dài tối đa: 40</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string Seri { get; set; }

        /// <summary>
        /// <para>Từ ngày (Thời hạn sử dụng chứng thư số từ ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn sử dụng chứng thư số đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string DNgay { get; set; }

        /// <summary>
        /// <para>Hình thức (Hình thức đăng ký)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(1: Thêm mới, 2: Gia hạn, 3: Ngừng sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public HThuc2 HThuc { get; set; }
    }
}
