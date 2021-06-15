using Services.ViewModels;

namespace ManagementServices.Helper
{
    public class ResultParam
    {
        public string HinhAnh { get; set; }
        public bool Result { get; set; }
        public UserViewModel User { get; set; }
        public string FileError { get; set; }
    }
}
