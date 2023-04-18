using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6
{
    public partial class TBao
    {
        /// <summary>
        /// <para>Mã thông điệp (Mã thông điệp gốc)</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp hệ thống của bên nhận không bóc tách và lấy được thông điệp)</para>
        /// </summary>
        public string MTDiep { get; set; }

        /// <summary>
        /// <para>Mã hồ sơ gốc</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(46)]
        public string MHSGoc { get; set; }
        /// <summary>
        /// <para>Mã nơi gửi</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MNGui { get; set; }

        /// <summary>
        /// <para>Ngày nhận (Ngày nhận thông điệp)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string NNhan { get; set; }

        /// <summary>
        /// <para>Trạng thái tiếp nhận</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (0: Không lỗi, 1: Có lỗi)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public TTTNhan TTTNhan { get; set; }

        public List<LDo> DSLDo { get; set; }
    }

    public partial class LDo
    {
        /// <summary>
        /// <para>Mã lỗi</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MLoi { get; set; }

        /// <summary>
        /// <para>Mô tả</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MTa { get; set; }
    }
}
