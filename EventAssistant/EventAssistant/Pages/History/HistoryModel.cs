using System;
using System.Collections.ObjectModel;

namespace EventAssistant
{
    /// <summary>
    /// A model class to create a Date based eventlist that contains every event happend this day.
    /// </summary>
    class HistoryModel : BaseViewModel
    {
        /// <summary>
        /// Date of the event.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Used to apply search filter between dates.
        /// </summary>
        public bool DateVisibility { get; set; } = true;

        /// <summary>
        /// Contains every event that happend at this.EventDate.
        /// </summary>
        public ObservableCollection<HistoryListModel> EventList { get; set; } = new ObservableCollection<HistoryListModel>();
    }
}
