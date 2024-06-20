using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.DayViewModels
{
    public class DayCalendarViewModel : ICalendarView
    {
        public DayDayHeaderViewModel DayHeaderViewModel { get; set; }

        public DayAllDayViewModel AllDayViewModel { get; set; }

        public DayTimeSideBarViewModel TimeSideBarViewModel { get; set; }

        public DayDayViewModel DayViewModel { get; set; }

        public List<ICalendarEvent> Events { get; set; }
    }
}
