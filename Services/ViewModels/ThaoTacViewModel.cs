using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class ThaoTacViewModel
    {
        public string ThaoTacId { get; set; }
        public string PemissionId { get; set; }
        public string FunctionId { get; set; }
        public string RoleId { get; set;}
        public string UserId { get; set; }
        public string FTID { get; set; }
        public string UTID { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int STT { get; set; }
        public bool? Active { get; set; } = false;
    }
}
