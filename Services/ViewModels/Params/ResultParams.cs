using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.Params
{
    public class ResultParams
    {
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
    }
    public class ResultParams2
    {
        public bool Success { get; set; } = true;
        public List<string> ErrorMessage { get; set; } = null;
    }
}