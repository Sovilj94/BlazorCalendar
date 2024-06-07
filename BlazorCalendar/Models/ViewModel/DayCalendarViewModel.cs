using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.ViewModel
{
    public class DayCalendarViewModel : ICalendarView
    {
        public GridItemViewModel GridItem { get; set; }

        public List<GridItemViewModel> GRIDITEMs2 { get; set; }

        public List<TimeCellViewModel> TimeCells { get; set; }

        public TimeDivision TimeDivision { get; set; }

        public List<Tasks>? DayTasks { get; set; }

        public DateTime Day { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }

        public int? MaxNumberOfColumns { get; set; }
    }
}
