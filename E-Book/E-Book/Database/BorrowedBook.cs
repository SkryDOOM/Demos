//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Book.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class BorrowedBook : BaseViewModel
    {
        public int ID { get; set; }
        public int LibID { get; set; }
        public int CopyID { get; set; }
        public System.DateTime ExpDate { get; set; }
    
        public virtual BookCopy BookCopy { get; set; }
        public virtual Member Member { get; set; }
    }
}
