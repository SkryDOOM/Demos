using EventAssistant.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace EventAssistant
{
    class EventDetailsViewModel : BaseViewModel
    {
        #region Properties
        private string mTaskSelected = "0";
        /// <summary>
        /// Contains the number of the tasks of the selected event.
        /// </summary>
        public string TaskNumberSelected
        {
            get { return mTaskSelected; }

            set
            {
                // Don't allow anything, but numbers.
                Regex regex = new Regex(@"[^0-9]");
                Match match = regex.Match(value);

                if (string.IsNullOrWhiteSpace(value))
                {
                    mTaskSelected = "0";
                    return;
                }

                if (match.Success)
                    return;

                mTaskSelected = value;
            }
        }

        /// <summary>
        /// Set true to enable editing of the selected event.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// Used to restore the selected event if editing was aborted.
        /// </summary>
        private Event SelectedEventBackup;

        /// <summary>
        /// The selected event to show.
        /// </summary>
        public EventDataModel SelectedEvent { get; set; }

        /// <summary>
        /// Size of the progress bar.
        /// </summary>
        public int ProgressbarSize { get; set; }

        private int mValue;
        /// <summary>
        /// The current value of the progressbar.
        /// </summary>
        public int ProgressbarValue
        {
            get { return mValue; }
            set
            {
                ProgressbarText = value == ProgressbarSize ? "Completed" : "Progression";
                mValue = value;
            }
        }

        /// <summary>
        /// Progressbar text.
        /// </summary>
        public string ProgressbarText { get; set; } = "Progression";

        /// <summary>
        /// Tasks of the selected event.
        /// </summary>
        public ObservableCollection<Task> TodayTasksList { get; set; }

        /// <summary>
        /// Used to restore the tasks of the event if editing was aborted.
        /// </summary>
        private List<Task> TodayTasksListBackup;
        #endregion

        #region Commands
        public ICommand UpdateEventCommand { get; private set; }
        public ICommand CompleteEventCommand { get; private set; }
        public ICommand DeleteEventCommand { get; set; }
        public ICommand EditEventCommand { get; private set; }
        public ICommand CheckTaskCommand { get; private set; }
        public ICommand DecreaseSelectedTaskCommand { get; private set; }
        public ICommand IncreaseSelectedTaskCommand { get; private set; }
        #endregion

        #region Constructors
        public EventDetailsViewModel()
        {
            InitializeCommands();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes commands.
        /// </summary>
        private void InitializeCommands()
        {
            EditEventCommand = new RelayCommand(EditEvent);
            CompleteEventCommand = new RelayCommand(CompleteEvent);
            CheckTaskCommand = new RelayParamCommand<Task>(CheckTaskEvent);
            IncreaseSelectedTaskCommand = new RelayCommand(IncreaseSelectedTaskEvent);
            DecreaseSelectedTaskCommand = new RelayCommand(DecreaseSelectedTaskEvent);
            UpdateEventCommand = new RelayCommand(UpdateEvent);
        }

        /// <summary>
        /// Editing the currently selected event.
        /// </summary>
        private void EditEvent()
        {
            if (SelectedEvent == null || SelectedEvent.IsCompleted)
                return;

            if (IsEditable == false)
            {
                // Save the current state in case of a backup.
                SelectedEventBackup = new Event(SelectedEvent.CurrentEvent);
                TodayTasksListBackup = new List<Task>();

                foreach (var item in TodayTasksList)
                {
                    TodayTasksListBackup.Add(new Task
                    {
                        ID = item.ID,
                        EventID = item.EventID,
                        Completed = item.Completed,
                        Description = item.Description
                    });
                }
            }
            else
            {
                // Reset the items.
                SelectedEvent.CurrentEvent.Title = SelectedEventBackup.Title;
                SelectedEvent.CurrentEvent.Date = SelectedEventBackup.Date;
                SelectedEvent.CurrentEvent.Description = SelectedEventBackup.Description;
                TodayTasksList = new ObservableCollection<Task>(TodayTasksListBackup);
            }

            IsEditable = IsEditable ? false : true;
        }

        /// <summary>
        /// Completing the selected event and add it to the completed table.
        /// </summary>
        private void CompleteEvent()
        {
            if (SelectedEvent == null || SelectedEvent.IsCompleted)
                return;

            using (var context = new EventDatabaseEntities())
            {
                Completed completed = new Completed() { EventID = SelectedEvent.CurrentEvent.ID };
                context.Completeds.Add(completed);

                // Mark every task as completed.
                (from item in context.Tasks
                 where item.EventID == SelectedEvent.CurrentEvent.ID select item).ToList().ForEach(x => x.Completed = true);

                TodayTasksList.ToList().ForEach(x => x.Completed = true);
                SelectedEvent.IsCompleted = true;
                ProgressbarValue = ProgressbarSize;
                context.SaveChanges();
                MessageBox.Show("Event completed successfully!");
            }
        }

        /// <summary>
        /// Increases the number of the tasks of the selected event.
        /// </summary>
        private void IncreaseSelectedTaskEvent()
        {
            int current = int.Parse(TaskNumberSelected);
            if (current == 99)
                return;

            current++;
            TaskNumberSelected = current.ToString();
            TodayTasksList.Add(new Task { Description = "" });
        }

        /// <summary>
        /// Decreases the number of the tasks of the selected event.
        /// </summary>
        private void DecreaseSelectedTaskEvent()
        {
            int current = int.Parse(TaskNumberSelected);
            if (current == 0)
                return;


            current--;
            TaskNumberSelected = current.ToString();
            TodayTasksList.RemoveAt(current);
        }

        /// <summary>
        /// Updates the selected event.
        /// </summary>
        private void UpdateEvent()
        {
            if (SelectedEvent.IsCompleted)
                return;

            if (SelectedEvent.CurrentEvent.Date == null || string.IsNullOrWhiteSpace(SelectedEvent.CurrentEvent.Title))
            {
                MessageBox.Show("Wrong title or date!");
                return;
            }

            using (var context = new EventDatabaseEntities())
            {
                // Update the event.
                var update = context.Events.Where(x => x.ID == SelectedEvent.CurrentEvent.ID).Single();
                context.Entry(update).CurrentValues.SetValues(SelectedEvent);

                context.Tasks.RemoveRange(context.Tasks.Where(x => x.EventID == SelectedEvent.CurrentEvent.ID));
                context.SaveChanges();

                foreach (var item in TodayTasksList)
                {
                    if (!string.IsNullOrWhiteSpace(item.Description))
                        context.Tasks.Add(new Task { EventID = SelectedEvent.CurrentEvent.ID, Description = item.Description });
                }

                context.SaveChanges();

                //Remove every empty task item from the list if there any.
                TodayTasksList.Where(x => string.IsNullOrWhiteSpace(x.Description)).ToList().All(i => TodayTasksList.Remove(i));

                TaskNumberSelected = TodayTasksList.Count().ToString();
                ProgressbarSize = TodayTasksList.Count();
                ProgressbarValue = TodayTasksList.Where(r => r.Completed == true).Count();
                IsEditable = false;
            }
        }

        /// <summary>
        /// Check or uncheck a task.
        /// </summary>
        /// <param name="sender"></param>
        private void CheckTaskEvent(Task sender)
        {
            if (SelectedEvent.IsCompleted)
            {
                sender.Completed = true;
                return;
            }

            if (sender.Completed)
                ProgressbarValue++;
            else
                ProgressbarValue--;

            using (var context = new EventDatabaseEntities())
            {
                var item = context.Tasks.Where(r => r.ID == sender.ID).Single();
                item.Completed = sender.Completed;
                context.SaveChanges();
            }
        }
        #endregion
    }
}
