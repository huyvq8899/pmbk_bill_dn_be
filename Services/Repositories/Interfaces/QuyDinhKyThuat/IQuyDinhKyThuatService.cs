using Services.ViewModels.QuyDinhKyThuat;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces.QuyDinhKyThuat
{
    public interface IQuyDinhKyThuatService
    {
        Task<bool> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai);
        Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai);
        Task<bool> LuuTrangThaiGuiToKhai(TrangThaiGuiToKhaiViewModel tThai);

    }
}
