using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.DayViewModels
{
    public class DayCalendarViewModel : ICalendarView
    {
        public DDayHeaderViewModel DayHeaderViewModel { get; set; }

        public DAllDayViewModel AllDayViewModel { get; set; }

        public DTimeSideBarViewModel TimeSideBarViewModel { get; set; }

        public DDayViewModel DayViewModel { get; set; }

        public List<ICalendarEvent> Events { get; set; }
    }
}
