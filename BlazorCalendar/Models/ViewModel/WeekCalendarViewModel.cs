using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.ViewModel
{
    public class WeekCalendarViewModel : ICalendarView
    {
        public List<DayHeaderViewModel> DayHeader { get; set; }

        public AllDayViewModel AllDay { get; set; }

        public List<DayCalendarViewModel> DayCalendar { get; set; }

        public TimeSideBarViewModel TimeSideBar { get; set; }

        public List<ICalendarEvent> Tasks { get; set; }
    }
}
