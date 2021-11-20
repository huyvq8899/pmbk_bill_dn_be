using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO.Modal
{
    public class FeedBackFunction
    {
        public Status Status { get; set; }
        public string Content { get; set; }
    }

    public enum Status
    {
        Erro,
        Success
    }
}
