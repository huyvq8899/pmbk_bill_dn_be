using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper
{
    public class ParamFileAttach
    {
        public string FileAttachDetailId { get; set; }
        public string FileAttachId { get; set; }
        public string Type { get; set; } // task, subtask, discuss
        public string TaskId { get; set; }
        public string SubTaskId { get; set; }
        public string CommentId { get; set; }
        public string DiscussId { get; set; }
        public string OfferId { get; set; }
    }
}
