using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Input;
using EventAssistant.Database;
using System;

namespace EventAssistant
{
    class HomePageViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Contains the details of a new event.
        /// </summary>
        public EventModel EventModel { get; private set; } = new EventModel();

        private string mTask = "0";
        /// <summary>
        /// Contains the number of the tasks to create.
        /// </summary>
        public string TaskNumber
        {
            get { return mTask; }

            set
            {
                // Don't allow anything, but numbers.
                Regex regex = new Regex(@"[^0-9]");
                Match match = regex.Match(value);

                if (string.IsNullOrWhiteSpace(value))
                {
                    mTask = "0";
                    return;
                }

                if (match.Success)
                    return;

                mTask = value;
            }
        }

        /// <summary>
        /// Tasks of the new event.(Optional)
        /// </summary>
        public ObservableCollection<TaskModel> TasksList { get; private set; } = new ObservableCollection<TaskModel>();

        /// <summary>
        /// Datacontext of the selected event usercontrol.
        /// </summary>
        public EventDetailsViewModel DetailsDataContext { get; private set; } = new EventDetailsViewModel();

        /// <summary>
        /// Contains the events which happen today.
        /// </summary>
        public ObservableCollection<EventDataModel> TodayEventsList { get; private set; } = new ObservableCollection<EventDataModel>();

        /// <summary>
        /// Contains the selected event.
        /// </summary>
        public EventDataModel SelectedEvent { get; set; }

        /// <summary>
        /// Private dialogbox to inform user.
        /// </summary>
        private DialogBox dialogBox;
        #endregion

        #region Commands
        public ICommand IncreaseTaskCommand { get; private set; }
        public ICommand DecreaseTaskCommand { get; private set; }
        public ICommand AddEventCommand { get; private set; }
        public ICommand LoadSelectedEventCommand { get; private set; }
        public ICommand DirectDeleteCommand { get; private set; }
        #endregion

        #region Constructors
        public HomePageViewModel()
        {
            LoadTodayEvents();
            InitializeCommands();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads every event that occours today.
        /// </summary>
        private void LoadTodayEvents()
        {
            using (var context = new EventDatabaseEntities())
            {
                var item = context.Events.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.Date) == DateTime.Today).ToList();
                foreach (var query in item)
                {
                    var completed = context.Completeds.Any(r => r.EventID == query.ID);
                    TodayEventsList.Add(new EventDataModel
                    {
                        CurrentEvent = query,
                        IsCompleted = completed
                    });
                }
            }
        }

        /// <summary>
        /// Initializes commands.
        /// </summary>
        private void InitializeCommands()
        {
            AddEventCommand = new RelayCommand(AddEvent);
            LoadSelectedEventCommand = new RelayCommand(LoadSelectedEvent);
            DetailsDataContext.DeleteEventCommand = new RelayCommand(DeleteEvent);
            DirectDeleteCommand = new RelayCommand(DirectDelete);
            IncreaseTaskCommand = new RelayCommand(IncreaseTaskEvent);
            DecreaseTaskCommand = new RelayCommand(DecreaseTaskEvent);        
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        private void AddEvent()
        {
            if (string.IsNullOrWhiteSpace(EventModel.EventTitle) || EventModel.EventDate == null)
            {
                dialogBox = new DialogBox("ERROR!", "Invalid inputs!");
                dialogBox.Show();
                return;
            }
       
            dialogBox = new DialogBox(DialogBoxType.WARNING, "CONFIRM!", "Would you like to add this event?");
            dialogBox.ShowDialog();

            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            if (!string.IsNullOrWhiteSpace(EventModel.EventTime))
            {
                EventModel.EventDate += " " + EventModel.EventTime;
            }

            using (var context = new EventDatabaseEntities())
            {
                try
                {
                    var item = new Event()
                    {
                        Title = EventModel.EventTitle,
                        Priority = EventModel.Priority.ToString(),
                        Date = DateTime.Parse(EventModel.EventDate),
                        Description = EventModel.EventDescription,
                    };

                    context.Events.Add(item);

                    context.SaveChanges();

                    if (item.Date.Date == DateTime.Today.Date)
                        TodayEventsList.Add(new EventDataModel {  CurrentEvent = item,  IsCompleted = false });

                    if (TasksList.Count != 0)
                    {
                        foreach (var task in TasksList)
                        {
                            if (!string.IsNullOrWhiteSpace(task.Description))
                                context.Tasks.Add(new Task { EventID = item.ID, Description = task.Description });
                        }
                        context.SaveChanges();
                    }

                    dialogBox = new DialogBox("Event added successfully!");
                    dialogBox.Show();
                }
                catch
                {
                    dialogBox = new DialogBox("ERROR!", "There was an error!");
                    dialogBox.Show();
                }

            }
        }

        /// <summary>
        /// Loads the selected event.
        /// </summary>
        private void LoadSelectedEvent()
        {
            if (TodayEventsList.Count == 0)
                return;
                     
            using (var context = new EventDatabaseEntities())
            {
                DetailsDataContext.SelectedEvent = SelectedEvent;
                var item = context.Events.Where(r => r.ID == DetailsDataContext.SelectedEvent.CurrentEvent.ID).FirstOrDefault().Tasks.ToList();

                DetailsDataContext.TodayTasksList = new ObservableCollection<Task>(item);
                DetailsDataContext.ProgressbarSize = DetailsDataContext.TodayTasksList.Count();
                DetailsDataContext.ProgressbarValue = DetailsDataContext.TodayTasksList.Where(r => r.Completed == true).Count();
                DetailsDataContext.TaskNumberSelected = DetailsDataContext.ProgressbarSize.ToString();
                DetailsDataContext.IsEditable = false;
            }
        }

        /// <summary>
        /// Increases the number of the tasks of a new event.
        /// </summary>
        private void IncreaseTaskEvent()
        {
            int current = int.Parse(TaskNumber);
            if (current == 99)
                return;

            current++;
            TaskNumber = current.ToString();
            TasksList.Add(new TaskModel { Description = "", TaskIndex = current });
        }

        /// <summary>
        /// Decreases the number of the tasks of a new event.
        /// </summary>
        private void DecreaseTaskEvent()
        {
            int current = int.Parse(TaskNumber);
            if (current == 0)
                return;

            current--;
            TaskNumber = current.ToString();
            TasksList.RemoveAt(current);
        }

        /// <summary>
        /// Directly deletes an event from the today's list.
        /// </summary>
        /// <param name="sender"></param>
        private void DirectDelete()
        {
            dialogBox = new DialogBox(DialogBoxType.WARNING, "DELETE!", "Would you like to delete this event?");
            dialogBox.ShowDialog();

            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            // GOT A BIG ERROR HERE AFTER EDITING...
            using (var context = new EventDatabaseEntities())
            {
                // Create a new object to avoid DbUpdateConcurrencyException.
                var deleteItem = context.Events.FirstOrDefault(x => x.ID == SelectedEvent.CurrentEvent.ID);

                context.Events.Remove(deleteItem);                
                context.SaveChanges();

                // Remove deleted event from the UI.
                DetailsDataContext.TodayTasksList = new ObservableCollection<Task>();
                TodayEventsList.Remove(SelectedEvent);
                DetailsDataContext.SelectedEvent = null;
                DetailsDataContext.ProgressbarValue = 0;
            }
        }

        /// <summary>
        /// Deletes the currently selected event.
        /// </summary>
        private void DeleteEvent()
        {
            if (DetailsDataContext.SelectedEvent == null)
                return;

            dialogBox = new DialogBox(DialogBoxType.WARNING, "DELETE!", "Would you like to delete this event?");
            dialogBox.ShowDialog();

            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new EventDatabaseEntities())
            {
                var deleteItem = context.Events.FirstOrDefault(x => x.ID == SelectedEvent.CurrentEvent.ID);
                context.Events.Remove(deleteItem);

                context.SaveChanges();

                // Remove the deleted event from the UI.
                var delete = TodayEventsList.FirstOrDefault(r => r.CurrentEvent.ID == SelectedEvent.CurrentEvent.ID);
                TodayEventsList.Remove(delete);
                DetailsDataContext.TodayTasksList = null;
                DetailsDataContext.SelectedEvent = null;
                DetailsDataContext.ProgressbarValue = 0;

                dialogBox = new DialogBox("Event deleted successfully!");
                dialogBox.Show();
            }
        }
        #endregion
    }
}