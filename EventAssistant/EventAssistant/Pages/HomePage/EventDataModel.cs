using EventAssistant.Database;

namespace EventAssistant
{
    /// <summary>
    /// Describes an event.
    /// </summary>
    class EventDataModel : BaseViewModel
    {
        /// <summary>
        /// The model of an event.
        /// </summary>
        public Event CurrentEvent { get; set; }

        /// <summary>
        /// Datatrigger property to show if an event is completed or not.
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
