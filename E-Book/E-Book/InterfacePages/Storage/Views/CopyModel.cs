using System.Text.RegularExpressions;

namespace E_Book.InterfacePages
{
    class CopyModel : BaseViewModel
    {
        private string mBook = ""; 
        public string BookCode
        {
            get { return mBook; }
            set
            {
                Regex regex = new Regex(@"[^A-z0-9]");
                Match match = regex.Match(value);

                if (match.Success)
                    return;

                mBook = value;
            }
        }
        public string ItemCount { get; set; }

        public CopyModel() { }

        public CopyModel(int itemCount)
        {
            ItemCount = itemCount.ToString() + ". másolat";
        }

    }
}
