using DLL.Entity.QuanLyHoaDon;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.DanhMuc
{
    public interface IThongTinHoaDonService
    {
        Task<ThongTinHoaDon> InsertAsync(ThongTinHoaDon model);
        Task<ThongTinHoaDon> UpdateAsync(ThongTinHoaDon model);
    }
}
