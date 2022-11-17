using System.ComponentModel.DataAnnotations;

namespace DLL.Entity.Config
{
    public class TaiKhoanSmartCA : ThongTinChung
    {
        [Key]
        [MaxLength(36)]
        public string TaiKhoanSmartCAId { get; set; }
        public string UserNameSmartCA { get; set; }
        public string PasswordSmartCA { get; set; }
        public string UserId { get; set; }
    }
}
