namespace E_Book.InterfacePages
{
    class DetailsModel 
    {
        /// <summary>
        /// The 6 character long book ID.
        /// </summary>
        public string BookCode { get; set; }

        /// <summary>
        /// Indicates if a copy is available to borrow.
        /// </summary>
        public string IsAvailable { get; set; } = "Igen";
    }
}
