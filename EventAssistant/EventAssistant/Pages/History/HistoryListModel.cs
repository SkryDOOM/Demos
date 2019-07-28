using EventAssistant.Database;
using System.Collections.Generic;

namespace EventAssistant
{
    /// <summary>
    /// Helper class that describes an event that happend at this day.
    /// </summary>
    class HistoryListModel : BaseViewModel
    {
        /// <summary>
        /// Details of the current event.
        /// </summary>
        public Event CurrentEvent { get; set; } = new Event();

        /// <summary>
        /// Contains the tasks of the current event.
        /// </summary>
        public List<Task> TaskList { get; set; } = new List<Task>();

        public HistoryListModel() { }

        public HistoryListModel(Event currentEvent, List<Task> taskList)
        {
            CurrentEvent = currentEvent;
            TaskList = taskList;
        }
    }
}
