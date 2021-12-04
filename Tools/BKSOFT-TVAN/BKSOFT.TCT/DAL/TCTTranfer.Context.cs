﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TCTTranferEntities : DbContext
    {
        public TCTTranferEntities()
            : base("name=TCTTranferEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TIVan> TIVans { get; set; }
    
        public virtual int usp_InsertMessage(Nullable<System.DateTime> dateTime, string mNGui, string mNNhan, Nullable<int> mLTDiep, string mTDiep, string mTDTChieu, string mST, string dataXML, Nullable<bool> status)
        {
            var dateTimeParameter = dateTime.HasValue ?
                new ObjectParameter("DateTime", dateTime) :
                new ObjectParameter("DateTime", typeof(System.DateTime));
    
            var mNGuiParameter = mNGui != null ?
                new ObjectParameter("MNGui", mNGui) :
                new ObjectParameter("MNGui", typeof(string));
    
            var mNNhanParameter = mNNhan != null ?
                new ObjectParameter("MNNhan", mNNhan) :
                new ObjectParameter("MNNhan", typeof(string));
    
            var mLTDiepParameter = mLTDiep.HasValue ?
                new ObjectParameter("MLTDiep", mLTDiep) :
                new ObjectParameter("MLTDiep", typeof(int));
    
            var mTDiepParameter = mTDiep != null ?
                new ObjectParameter("MTDiep", mTDiep) :
                new ObjectParameter("MTDiep", typeof(string));
    
            var mTDTChieuParameter = mTDTChieu != null ?
                new ObjectParameter("MTDTChieu", mTDTChieu) :
                new ObjectParameter("MTDTChieu", typeof(string));
    
            var mSTParameter = mST != null ?
                new ObjectParameter("MST", mST) :
                new ObjectParameter("MST", typeof(string));
    
            var dataXMLParameter = dataXML != null ?
                new ObjectParameter("DataXML", dataXML) :
                new ObjectParameter("DataXML", typeof(string));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_InsertMessage", dateTimeParameter, mNGuiParameter, mNNhanParameter, mLTDiepParameter, mTDiepParameter, mTDTChieuParameter, mSTParameter, dataXMLParameter, statusParameter);
        }
    }
}
