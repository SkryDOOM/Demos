namespace E_Book.InterfacePages
{
    /// <summary>
    /// A helper model class to get every borrowed book of a person.
    /// </summary>
    class MemberModel
    {
        /// <summary>
        /// the title of a book.
        /// </summary>
        public string BookName { get; set; }

        /// <summary>
        /// The 6 character length Id of a book copy.
        /// </summary>
        public string CopyID { get; set; }
    }
}
