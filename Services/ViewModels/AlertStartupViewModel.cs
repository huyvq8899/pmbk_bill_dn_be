using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels
{
    public class AlertStartupViewModel
    {
        public AlertStartupViewModel()
        {
            this.Status = true;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Link { get; set;}
        public string Content { get; set; }
        public bool? Status { get; set; } = false;
        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
