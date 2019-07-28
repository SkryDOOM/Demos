using System;

namespace EventAssistant
{
    /// <summary>
    /// An event model to create a new event.
    /// </summary>
    class EventModel : BaseViewModel
    {
        /// <summary>
        /// Title of the event.
        /// </summary>
        public string EventTitle { get; set; }

        /// <summary>
        /// Time of the event.
        /// </summary>
        public string EventTime { get; set; }

        /// <summary>
        /// Date of the event.
        /// </summary>
        public string EventDate { get; set; } = DateTime.Today.ToString();

        /// <summary>
        /// Description of the event. (Optional)
        /// </summary>
        public string EventDescription { get; set; }

        /// <summary>
        /// Priority of the event.
        /// </summary>
        public Priority Priority { get; set; }
    }
}
