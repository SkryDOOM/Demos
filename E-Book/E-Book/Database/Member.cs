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
    
    public partial class Member : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            this.BorrowedBooks = new HashSet<BorrowedBook>();
        }
    
        public int ID { get; set; }
        public string FullName { get; set; }
        public string LibID { get; set; }
        public System.DateTime ExpData { get; set; }
        public string PlaceOfBirth { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; }
    }
}