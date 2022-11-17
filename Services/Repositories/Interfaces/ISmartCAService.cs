using Services.ViewModels.Config;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface ISmartCAService
    {
        Task<string> AccessSmartCA(string type, string args);

        Task<string> GetDataXMLUnsign(string idThongDiep);

        Task<string> GetDataXMLHoaDonUnsign(string idHoaDon);

        string ReadFileHoaDonThayThe(string url);

        Task<TaiKhoanSmartCAViewModel> GetChuKiMemMoiNhat();

        Task<TaiKhoanSmartCAViewModel> InsertAsync(TaiKhoanSmartCAViewModel model);

        Task<string> SignSmartCAXML(TaiKhoanSmartCAViewModel model);

        Task<string> SignSmartCAXMLToKhai(TaiKhoanSmartCAViewModel model);

        Task<string> SignSmartCAXMLSaiSot(TaiKhoanSmartCAViewModel model);
    }
}
