namespace E_Book.InterfacePages
{
    class BorrowModel
    {
        /// <summary>
        /// The foreign key of the 6 character long identifier.
        /// </summary>
        public int CopyID { get; set; }

        /// <summary>
        /// The 6 character long identifier.
        /// </summary>
        public string BookCode { get; set; }

        /// <summary>
        /// The title of the book.
        /// </summary>
        public string BookName { get; set; }
    }
}
