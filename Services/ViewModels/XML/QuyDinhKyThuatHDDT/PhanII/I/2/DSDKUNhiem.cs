﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2
{
    public partial class DSDKUNhiem
    {
        public List<DKUNhiem> DKUNhiem { get; set; }
    }

    public partial class DKUNhiem
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

        /// <summary>
        /// <para>Tên loại hóa đơn</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TLHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public int? KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Tên tổ chức (Tên tổ chức ủy nhiệm/nhận ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TTChuc { get; set; }

        /// <summary>
        /// <para>Mục đích (Mục đích ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string MDich { get; set; }

        /// <summary>
        /// <para>Từ ngày (Thời hạn ủy nhiệm từ ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string TNgay { get; set; }

        /// <summary>
        /// <para>Đến ngày (Thời hạn ủy nhiệm đến ngày)</para>
        /// <para>Kiểu dữ liệu: Ngày giờ</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string DNgay { get; set; }

        /// <summary>
        /// <para>Phương thức (Phương thức thanh toán hóa đơn ủy nhiệm)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục XI kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public PThuc PThuc { get; set; }

        /// <summary>
        /// <para>Tên hình thức thanh toán khác</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Bắt buộc trong trường hợp hình thức thanh toán là khác)</para>
        /// </summary>
        public string THTTTKhac { get; set; }
    }
}
