using DLL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class FunctionViewModel
    {
        public FunctionViewModel()
        {
            this.Status = true;
        }
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? Status { get; set; }
        public string Type { get; set; }
        public int? STT { get; set; }
    }

    public class FunctionByTreeViewModel
    {
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string Title { get; set; }
        public string ParentFunctionId { get; set; }
        public bool? SuDung { get; set; }
        public string Key { get; set; }
        public bool? IsRootTree { get; set; }
        public int? STT { get; set; }
        public List<FunctionByTreeViewModel> Children { get; set; }
        public List<ThaoTacViewModel> ThaoTacs { get; set; }
    }

    public class TreeOfFunction
    {
        public List<FunctionViewModel> SelectedFunctions { get; set; }
        public List<FunctionByTreeViewModel> FunctionByTreeViewModel { get; set; }

    }
}
