namespace EventAssistant
{
    /// <summary>
    /// Public class to add new task to an event.
    /// </summary>
    class TaskModel : BaseViewModel
    {
        /// <summary>
        /// Description of the task.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The indexes of the tasks.
        /// </summary>
        public int TaskIndex { get; set; }
    }
}
