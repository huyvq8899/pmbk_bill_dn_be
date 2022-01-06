using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class ChungThuSoSuDungViewModel
    {
        public string Id { get; set; }
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
        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

        /// <summary>
        /// <para>Seri (Số sê-ri chứng thư số)</para>
        /// <para>Độ dài tối đa: 40</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(40)]
        public string Seri { get; set; }

        /// <summary>
        /// <para>Từ ngày (Thời hạn sử dụng chứng thư số từ ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn sử dụng chứng thư số đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string DNgay { get; set; }

        /// <summary>
        /// <para>Hình thức (Hình thức đăng ký)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(1: Thêm mới, 2: Gia hạn, 3: Ngừng sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public int HThuc { get; set; }
        public bool IsAddInTTNNT { get; set; } = false;
    }
}
