using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IXMLInvoiceService
    {
        Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model);
        Task<bool> CreateXMLBienBan(string xmlFilePath, BienBanXoaBoViewModel model);
        void CreateQuyDinhKyThuat_PhanII_II_7(string xmlFilePath, ThongDiepGuiHDDTKhongMaViewModel model);
        string ConvertToXML<T>(T obj);
        string CreateFileXML<T>(T obj, string folderName);
        void GenerateXML<T>(T data, string path);
    }
}
