using ManagementServices.Helper;
using Services.ViewModels;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.TienIch
{
    /// <summary>
    /// Interface này định nghĩa các phương thức:
    /// - Đọc ra tên người ký theo các điều kiện/trường hợp khác nhau.
    /// - Gửi lệnh socket để đọc thông tin người ký từ chữ ký số.
    /// </summary>
    public interface IDigitalSignerNameReaderService
    {
        /// <summary>
        /// Đọc ra thông tin của người bán trên mẫu hóa đơn của hóa đơn điện tử.
        /// </summary>
        /// <param name="hoaDonDienTuIds">Là mảng các id của hóa đơn điện tử cần đọc thông tin.</param>
        /// <returns>Thông tin của người bán trên mẫu hóa đơn.</returns>
        Task<List<ThongTinNguoiBanTrenMauHoaDonViewModel>> GetThongTinNguoiBanTuHoaDonAsync(string[] hoaDonDienTuIds);

        /// <summary>
        /// Đọc ra thông tin của người bán trên mẫu hóa đơn cụ thể.
        /// </summary>
        /// <param name="mauHoaDonId">Id của mẫu hóa đơn cần đọc thông tin.</param>
        /// <returns>Thông tin của người bán trên mẫu hóa đơn.</returns>
        Task<ThongTinNguoiBanTrenMauHoaDonViewModel> GetThongTinNguoiBanTuMauHoaDonAsync(string mauHoaDonId);

        /// <summary>
        /// Đọc ra tên người ký theo các điều kiện/trường hợp khác nhau.
        /// </summary>
        /// <param name="signerNameParams">Tham số điều kiện.</param>
        /// <returns>Tên người ký.</returns>
        Task<string> GetUnifiedSignerNameAsync(UnifiedSignerNameParams signerNameParams);

        /// <summary>
        /// Đọc ra tên của người ký chữ ký số.
        /// </summary>
        /// <returns>Tên của người ký chữ ký số.</returns>
        Task<string> GetSignerNameAsync();

        /// <summary>
        /// Đọc ra tên người ký từ http-headers của client gửi lên.
        /// </summary>
        /// <param name="signerNameParams">Các tham số điều kiện.</param>
        /// <returns>Tên người ký của chữ ký số.</returns>
        Task<string> GetSignerNameByHttpHeaderAsync(UnifiedSignerNameParams signerNameParams);
    }
}
