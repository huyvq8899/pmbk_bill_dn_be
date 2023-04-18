using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV.IV
{
    public partial class NNT
    {
        /// <summary>
        /// <para>Tên</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Quốc tịch</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        
        [MaxLength(400)]
        public string QTich { get; set; }


        /// <summary>
        /// <para>Cá nhân cư trú</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số </para>
        /// <para>Bắt buộc</para>
        /// </summary>


        public int? CNCTru { get; set; }


        /// <summary>
        /// <para>CMND (Số CMND /CCCD/Hộ chiếu)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        [MaxLength(20)]
        public string CMND { get; set; }


        /// <summary>
        /// <para>Ngày cấp CMND (Ngày cấp CMND /CCCD/Hộ chiếu)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

       
        public string NgCCMND { get; set; }


        /// <summary>
        /// <para>Nơi cấp CMND (Nơi cấp CMND /CCCD/Hộ chiếu)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        [MaxLength(100)]
        public string NCCMND { get; set; }


        /// <summary>
        /// <para>Số điện thoại</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        [MaxLength(20)]
        public string SDThoai { get; set; }


        /// <summary>
        /// <para>Địa chỉ thư điện tử </para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        [MaxLength(50)]
        public string DCTDTu { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
