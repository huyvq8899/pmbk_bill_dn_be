//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BKSOFT.TCT.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class QueueOut
    {
        public System.Guid Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public bool Status { get; set; }
        public string MTDTChieu { get; set; }
        public string DataXML { get; set; }
    }
}
