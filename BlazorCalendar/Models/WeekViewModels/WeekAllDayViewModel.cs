
using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.WeekViewModels
{
    public class WeekAllDayViewModel
    {
        public List<WeekGridItemViewModel> GridItems { get; set; }

        public List<WeekTimeCellViewModel> TimeCells { get; set; }

        public List<ICalendarEvent> Events { get; set; }

        public DateTime Day { get; set; }

        public DateTime FirstDateWeek { get; set; }
    }
}
