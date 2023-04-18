using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VII._3
{
    public partial class TDiep
    {
        public TTChungThongDiep1510 TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }
    public partial class DLieu
    {
        /// <summary>
        /// <para>MST của người nộp thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã cơ quan thuế quản lý</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu:string</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [MaxLength(5)]
        [Required]
        public string MCQTQLy { get; set; }

        /// <summary>
        /// <para>Hình thức hóa đơn đăng ký sử dụng: Có mã </para>
        /// <para>Kiểu dữ liệu:Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int CMa { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn sử dụng là hóa đơn GTGT </para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int HDGTGT { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn sử dụng là hóa đơn bán hàng </para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int HDBHang { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn sử dụng là hóa đơn bán tài sản công</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int HDBTSCong { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn sử dụng là hóa đơn bán hàng dự trữ quốc gia</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int HDBHDTQGia { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn sử dụng là các loại hóa đơn khác</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int HDKhac { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn sử dụng là các chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        /// 0 : Không, 1 : Có
        //[Required]
        [Required]
        public int CTu { get; set; }

        /// <summary>
        /// <para>Ngày cập nhật</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [Required]
        public DateTime NCNhat { get; set; }
    }
}
