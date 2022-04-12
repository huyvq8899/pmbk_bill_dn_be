using ManagementServices.Helper;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HeThong
{
    public class UserParams : PagingParams
    {
        public UserViewModel Filter { get; set; }
    }
}
