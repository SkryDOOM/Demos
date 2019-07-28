using System;
using System.Collections.ObjectModel;

namespace EventAssistant
{
    /// <summary>
    /// Model class to create a date based eventlist, that contains every event happend this day.
    /// </summary>
    class NextEvents : BaseViewModel
    {
        /// <summary>
        /// Contains every event of a day.
        /// </summary>
        public ObservableCollection<EventDataModel> EventModelList{ get; set; }

        /// <summary>
        /// Contains the date of the EventModelList.
        /// </summary>
        public DateTime EventDate { get; set; }
    }
}
