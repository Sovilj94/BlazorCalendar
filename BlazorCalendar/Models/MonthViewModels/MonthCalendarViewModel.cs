using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.MonthViewModels
{
    public class MonthCalendarViewModel : ICalendarView
    {
        public List<MonthDayHeaderViewModel> MonthHeaderViewModels { get; set; }
        public List<MonthTimeCellViewModel> MonthTimeCellViewModels { get; set; }
        public List<MonthGridItemViewModel> MonthGridItemViewModels { get; set; }
        public List<ICalendarEvent> CalendarEvents { get; set; }
        public DateTime Date { get; set; }
    }
}
