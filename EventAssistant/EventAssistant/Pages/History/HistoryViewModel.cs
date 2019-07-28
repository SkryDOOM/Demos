using EventAssistant.Database;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace EventAssistant
{
    class HistoryViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Contains every event that happend before today.
        /// </summary>
        public ObservableCollection<HistoryModel> HistoryList { get; private set; } = new ObservableCollection<HistoryModel>();

        /// <summary>
        /// Contains the results of the searchbox.
        /// </summary>
        public ObservableCollection<Event> AutoFillList { get; private set; }

        /// <summary>
        /// Filter property that controls the state of the expanders.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Controls the visibility of the autofill listbox.
        /// </summary>
        public bool IsCollapsed { get; private set; }

        /// <summary>
        /// Controls the visibility of the HistoryList Itemscontrol.
        /// </summary>
        public bool EventListVisibility { get; set; } = true;

        /// <summary>
        /// Controls the visibility of the autosearch selected item box.
        /// </summary>
        public bool AutoSearchVisibility { get; private set; }

        /// <summary>
        /// Filter property to set the Date to start with.
        /// </summary>
        public string FromDate { get; private set; }

        /// <summary>
        /// Filter property to set the date to end with.
        /// </summary>
        public string ToDate { get; private set; }

        private string mSearch;
        /// <summary>
        /// Contains the search term that is binded to the search textbox.
        /// </summary>
        public string SearchTerm
        {
            get { return mSearch; } 
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    IsCollapsed = false;

                else if (mFlag == false)
                {
                    // Search for a matching item.
                    Regex regex = new Regex(value.ToLower());
                    var Result = HistoryList.SelectMany(x => x.EventList.Where(r => regex.IsMatch(r.CurrentEvent.Title.ToLower()))).Select(x => x.CurrentEvent).ToList();
                    
                    AutoFillList = new ObservableCollection<Event>(Result);
                    IsCollapsed = true;
                }

                mSearch = value;
            }
        }

        private Event mSearchItem;
        /// <summary>
        /// Contains the selected autofill item.
        /// </summary>
        public Event AutoSearchItem
        {
            get { return mSearchItem; }
            set
            {
                mFlag = true;

                if (value != null)
                {
                    SearchTerm = value.Title;
                    using (var context = new EventDatabaseEntities())
                    {
                        var result = context.Tasks.Where(r => r.EventID == value.ID).ToList();
                        AutoSearchTaskList = new ObservableCollection<Task>(result);
                    }
                    AutoSearchEvent();
                }

                mFlag = false;
                AutoFillList = null;
                IsCollapsed = false;
                
                mSearchItem = new Event(value);
            }
        }

        /// <summary>
        /// Indicates if an autofill item was selected.
        /// Prevents stackoverflow exception.
        /// </summary>
        private bool mFlag;

        /// <summary>
        /// Contains the tasks of the selected autosearch item if there any.
        /// </summary>
        public ObservableCollection<Task> AutoSearchTaskList { get; private set; }

        /// <summary>
        /// Private Dialogbox to inform user.
        /// </summary>
        private DialogBox dialogBox;
        #endregion

        #region Commands
        public ICommand DateFilterCommand { get; private set; }
        public ICommand DeleteEventCommand { get; private set; }
        public ICommand CloseEventCommand { get; private set; }
        #endregion

        #region Constructors
        public HistoryViewModel()
        {
            using (var context = new EventDatabaseEntities())
            {
                int CurrentIndex = 0;
                // Get every date when was an event.
                var dates = context.Events.Where(x => DbFunctions.TruncateTime(x.Date) < DateTime.Today)
                                          .GroupBy(r => DbFunctions.TruncateTime(r.Date)).OrderByDescending(x => x.Key).ToList();

                // Set the date indicators.
                ToDate = dates.ElementAt(0).Key.ToString();
                FromDate = dates.ElementAt(dates.Count() - 1).Key.ToString();

                foreach (var events in dates)
                {
                    // Every event that happend at this day.
                    var Events = context.Events.Where(x => DbFunctions.TruncateTime(x.Date) == events.Key).ToList();

                    // Add the date to the list.
                    HistoryList.Add(new HistoryModel() { EventDate = (DateTime)events.Key });

                    // Get the tasks for the events if there any.
                    foreach (var task in Events)
                    {
                        var tasks = context.Tasks.Where(x => x.EventID == task.ID).ToList();
                        HistoryList.ElementAt(CurrentIndex).EventList.Add(new HistoryListModel(task, tasks));
                    }

                    CurrentIndex++;
                }
            }

            DateFilterCommand = new RelayCommand(DateFilterEvent);
            DeleteEventCommand = new RelayParamCommand<Event>(DeleteEvent);
            CloseEventCommand = new RelayCommand(CloseEvent);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Applies Datefilters.
        /// </summary>
        private void DateFilterEvent()
        {
            if (string.IsNullOrWhiteSpace(ToDate))
                return;

            if(!EventListVisibility)
            CloseEvent();

            DateTime from = string.IsNullOrWhiteSpace(FromDate) ? DateTime.MinValue : DateTime.Parse(FromDate);
            DateTime to = DateTime.Parse(ToDate);

            if (from > to)
                return;

                foreach (var item in HistoryList)
                {
                    if (item.EventDate <  from || item.EventDate > to)
                        item.DateVisibility = false;
                    else
                     item.DateVisibility = true;
                }
        }

        /// <summary>
        /// Deletes the selected event.
        /// </summary>
        private void DeleteEvent(Event sender)
        {
            dialogBox = new DialogBox(DialogBoxType.WARNING, "DELETE!", "Would you like to delete this event?");
            dialogBox.ShowDialog();

            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new EventDatabaseEntities())
            {
                context.Events.Attach(sender);
                context.Events.Remove(sender);
                context.SaveChanges();

                dialogBox = new DialogBox("Event deleted successfully!");
                dialogBox.ShowDialog();

                if (sender.ID == AutoSearchItem.ID)
                    CloseEvent();
            }

            foreach (var item in HistoryList)
            {
                foreach (var del in item.EventList)
                {
                    if (del.CurrentEvent.ID == sender.ID)
                    {
                        item.EventList.Remove(del);

                        if (item.EventList.Count == 0)
                            HistoryList.Remove(item);

                        return;
                    }
                }                    
            }
        }

        /// <summary>
        /// Displays the autosearch selected itembox and hides the date filtered Itemscontrol.
        /// </summary>
        private void AutoSearchEvent()
        {
            EventListVisibility = false;
            AutoSearchVisibility = true;            
        }

        /// <summary>
        /// Closes the searched item and reopens the date filtered itemscontrol.
        /// </summary>
        private void CloseEvent()
        {
            AutoSearchVisibility = false;
            EventListVisibility = true;            
        }
        #endregion
    }
}