using System.Collections.Generic;

namespace Services.ViewModels.Ticket
{
    public class User_XeViewModel : ThongTinChungViewModel
    {
        public string UserId { get; set; }
        public string XeId { get; set; }

        public UserViewModel User { get; set; }
        public XeViewModel Xe { get; set; }

        public List<string> UserIds { get; set; }
    }
}
