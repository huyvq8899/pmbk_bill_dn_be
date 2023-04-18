using DLL.Enums;
using Services.Helper.Params.HeThong;

namespace Services.Helper
{
    public class EnumModel
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public string NameOfKey { get; set; }
        public TrangThaiSuDung2 TrangThai { get; set; }
    }
}
