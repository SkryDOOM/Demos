using System;

namespace E_Book.InterfacePages
{
    class ReturnModel
    {
        /// <summary>
        /// The full name of a member.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// The 5 character long ID of a member.
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// the title of a book.
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// The 6 character long Id of a book copy.
        /// </summary>
        public string CopyID { get; set; }

        /// <summary>
        /// The expiration date of the book borrowing.
        /// </summary>
        public DateTime ExpDate { get; set; } 
    }
}
