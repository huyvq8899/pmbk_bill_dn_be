using DLL.Enums;
using System;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class TrangThaiGuiToKhaiViewModel
    {
        public string Id { get; set; }
        public string IdToKhai { get; set; }
        public string MNGui { get; set; }

        /// <summary>
        /// <para>Mã nơi nhận</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        public string MNNhan { get; set; }

        /// <summary>
        /// <para>Mã loại thông điệp</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        public string MLTDiep { get; set; }

        /// <summary>
        /// <para>Mã thông điệp</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MTDiep { get; set; }

        /// <summary>
        /// <para> Mã thông điệp tham chiếu</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MTDTChieu { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST của NNT)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Số lượng</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public int SLuong { get; set; }

        public TrangThaiGuiToKhaiDenCQT TrangThaiGui { get; set; }

        public TrangThaiTiepNhanCuaCoQuanThue TrangThaiTiepNhan { get; set; }

        public DateTime NgayGioGui { get; set; }

        public string FileXMLGui { get; set; }

        public byte[] NoiDungFileGui { get; set; }
    }
}
