using DLL.Entity;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface IXMLInvoiceService
    {
        Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model);
        //Task<bool> CreateXMLBienBan(string xmlFilePath, BienBanXoaBoViewModel model);
        Task CreateQuyDinhKyThuatTheoMaLoaiThongDiep(string xmlFilePath, ThongDiepChungViewModel model);
        Task<FileData> CreateBangTongHopDuLieu(string xmlPath, BangTongHopDuLieuParams @params);
        Task CreateQuyDinhKyThuat_PhanII_II_7(string xmlFilePath, ThongDiepChungViewModel model);
        void CreateQuyDinhKyThuat_PhanII_II_5(string xmlFilePath, ThongDiepChungViewModel model);
        //string ConvertToXML<T>(T obj);
        string CreateFileXML<T>(T obj, string folderName, string fileName, string ThongDiepId = null);
        void GenerateXML<T>(T data, string path);
        string PrintXML(string xml);
    }
}
