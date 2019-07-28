using EventAssistant.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace EventAssistant
{
    class UpComingViewModel : BaseViewModel
    {
        #region Properties
        /// <summary>
        /// Contains the selected event.
        /// </summary>
        public EventDataModel SelectedEvent { get; set; }

        /// <summary>
        /// A list that contains every upcoming events.
        /// </summary>
        public ObservableCollection<NextEvents> UpcomingEventsList { get; private set; }

        /// <summary>
        /// Datacontext of the selected event.
        /// </summary>
        public EventDetailsViewModel DetailsDataContext { get; private set; } = new EventDetailsViewModel();

        /// <summary>
        /// Private dialogbox to inform user.
        /// </summary>
        private DialogBox dialogBox;
        #endregion

        #region Commands
        public ICommand LoadSelectedEventCommand { get; private set; }
        public ICommand DirectDeleteCommand { get; private set; }
        #endregion

        #region Constructors
        public UpComingViewModel()
        {
            using (var context = new EventDatabaseEntities())
            {
                UpcomingEventsList = new ObservableCollection<NextEvents>();

                var result = context.Events.Where(x => System.Data.Entity.DbFunctions.TruncateTime(x.Date) > DateTime.Today).GroupBy(r => System.Data.Entity.DbFunctions.TruncateTime(r.Date));

                foreach (var query in result)
                {
                    // Get every events that happen on the same day.
                    var events = context.Events.Where(r => System.Data.Entity.DbFunctions.TruncateTime(r.Date) == query.Key).ToList();

                    // Create a new list to add the IsCompleted property.
                    List<EventDataModel> list = new List<EventDataModel>();
                    foreach (var collection in events)
                    {
                        var completed = context.Completeds.Any(r => r.EventID == collection.ID);
                        list.Add(new EventDataModel { CurrentEvent = collection, IsCompleted = completed});
                    }

                    UpcomingEventsList.Add(new NextEvents
                    {
                        EventDate = (DateTime)query.Key,
                        EventModelList = new ObservableCollection<EventDataModel>(list)
                    });
                }
            }
            InitializeCommands();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes commands.
        /// </summary>
        private void InitializeCommands()
        {
            LoadSelectedEventCommand = new RelayCommand(LoadSelectedEvent);
            DirectDeleteCommand = new RelayParamCommand<EventDataModel>(DirectDelete);
            DetailsDataContext.DeleteEventCommand = new RelayCommand(DeleteEvent);
        }

        /// <summary>
        /// Loads the selected event.
        /// </summary>
        private void LoadSelectedEvent()
        {

            using (var context = new EventDatabaseEntities())
            {
                DetailsDataContext.SelectedEvent = SelectedEvent;
                var item = context.Events.Where(r => r.ID == DetailsDataContext.SelectedEvent.CurrentEvent.ID).FirstOrDefault().Tasks.ToList();

                DetailsDataContext.TodayTasksList = new ObservableCollection<Database.Task>(item);
                DetailsDataContext.ProgressbarSize = DetailsDataContext.TodayTasksList.Count();
                DetailsDataContext.ProgressbarValue = DetailsDataContext.TodayTasksList.Where(r => r.Completed == true).Count();
                DetailsDataContext.TaskNumberSelected = DetailsDataContext.ProgressbarSize.ToString();
                DetailsDataContext.IsEditable = false;
            }
        }

        /// <summary>
        /// Directly deletes an event from the today's list.
        /// </summary>
        /// <param name="sender"></param>
        private void DirectDelete(EventDataModel sender)
        {
            dialogBox = new DialogBox(DialogBoxType.WARNING, "DELETE!", "Would you like to delete this event?");
            dialogBox.ShowDialog();

            if (dialogBox.Answer == DialogAnswer.NO)
                return;

            using (var context = new EventDatabaseEntities())
            {
               
                context.Events.Attach(sender.CurrentEvent);
                context.Events.Remove(sender.CurrentEvent);
                context.SaveChanges();
          
                var delete = UpcomingEventsList.Where(p => p.EventModelList.Any(x => x.CurrentEvent.ID == sender.CurrentEvent.ID));
                delete.ElementAt(0).EventModelList.Remove(sender);

                DetailsDataContext.SelectedEvent = null;
                DetailsDataContext.ProgressbarValue = 0;
                DetailsDataContext.TodayTasksList = new ObservableCollection<Task>();

                // Remove the empy date object.
                var item = UpcomingEventsList.FirstOrDefault(x => x.EventModelList.Count == 0);
                if (item != null)
                    UpcomingEventsList.Remove(item);
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
                context.Events.Attach(DetailsDataContext.SelectedEvent.CurrentEvent);
                context.Events.Remove(DetailsDataContext.SelectedEvent.CurrentEvent);

                context.SaveChanges();
                var delete = UpcomingEventsList.SelectMany(x => x.EventModelList.Where( r => r.CurrentEvent.ID == SelectedEvent.CurrentEvent.ID)).Single();
                UpcomingEventsList.ToList().RemoveAll(x => x.EventModelList.Remove(delete));
                DetailsDataContext.TodayTasksList = null;
                DetailsDataContext.SelectedEvent = null;
                DetailsDataContext.ProgressbarValue = 0;

                // Remove the empy date object.
                var item = UpcomingEventsList.FirstOrDefault(x => x.EventModelList.Count == 0);
                if (item != null)
                    UpcomingEventsList.Remove(item);



                dialogBox = new DialogBox("Event deleted successfully!");
                dialogBox.Show();
            }
        }
        #endregion
    }
}
