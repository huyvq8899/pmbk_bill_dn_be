using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IXMLInvoiceService
    {
        Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model);
        Task<bool> CreateXMLBienBan(string xmlFilePath, BienBanXoaBoViewModel model);
    }
}
