using ManagementServices.Helper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IHopDongHoaDonService
    {
        /// <summary>
        /// Hàm lấy toàn bộ hợp đồng không phân trang
        /// </summary>
        /// <returns></returns>
        Task<IList<HopDongHoaDonViewModel>> GetAllAsync();
        
        Task<List<HopDongHoaDonViewModel>> GetHopDongByTaxcodeAsync(string maSoThue);

        /// <summary>
        /// Hàm lấy toàn bộ hợp đồng theo phân trang
        /// </summary>
        /// <param name="pagingParams">Thông tin trong 1 page</param>
        /// <returns></returns>
        Task<PagedList<HopDongHoaDonViewModel>> GetAllPagingAsync(PagingParams pagingParams);

        /// <summary>
        /// Hàm lấy thông tin hợp đồng theo Id
        /// </summary>
        /// <param name="id">Hợp đồng hóa đơn Id</param>
        /// <returns></returns>
        Task<HopDongHoaDonViewModel> GetByIdAsync(string id);

        /// <summary>
        /// Hàm insert hợp đồng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> CreateAsync(HopDongHoaDonViewModel model);

        /// <summary>
        /// Hàm check hợp đồng có tồn tại hay không
        /// </summary>
        /// <param name="id">Hợp đồng hóa đơn Id</param>
        /// <returns></returns>
        Task<bool> CheckExistsAsync(string id);

        /// <summary>
        /// Hàm update thông tin hợp đồng hóa đơn
        /// </summary>
        /// <param name="model">Thông tin cần update</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(HopDongHoaDonViewModel model);

        /// <summary>
        /// Hàm tính tổng giá trị hợp đồng
        /// </summary>
        /// <returns></returns>
        Task<decimal> SumGiaTriHopDongAsync();


    }
}
