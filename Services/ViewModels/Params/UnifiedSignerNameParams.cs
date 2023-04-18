using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Params
{
    /// <summary>
    /// Lớp này thể hiện các tham số cho API đọc ra tên người ký từ các điều kiện khác nhau.
    /// </summary>
    public class UnifiedSignerNameParams
    {
        /// <summary>
        /// Id của bản ghi (có thể là id hóa đơn, id mẫu hóa đơn, id biên bản hủy, id biên bản điều chỉnh)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Phân loại dữ liệu cần phải xử lý (có thể là hóa đơn, mẫu hóa đơn, biên bản hủy, biên bản điều chỉnh)
        /// </summary>
        public string Type { get; set; }
    }
}