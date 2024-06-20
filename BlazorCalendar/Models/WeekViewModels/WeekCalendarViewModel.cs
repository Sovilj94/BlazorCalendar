using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.WeekViewModels
{
    public class WeekCalendarViewModel : ICalendarView
    {
        public List<WeekDayHeaderViewModel> DayHeader { get; set; }

        public WeekAllDayViewModel AllDay { get; set; }

        public List<WeekDayViewModel> DayCalendar { get; set; }

        public WeekTimeSideBarViewModel TimeSideBar { get; set; }

        public List<ICalendarEvent> Tasks { get; set; }
    }
}
