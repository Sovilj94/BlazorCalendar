
namespace BlazorCalendar.Models.ViewModel
{
    public class DayViewModel
    {
        public List<GridItemViewModel>? GridItems { get; set; }

        public List<TimeCellViewModel> TimeCells { get; set; }

        public TimeDivision TimeDivision { get; set; }

        public DateTime Day { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }

        public int? MaxNumberOfColumns { get; set; }
    }
}
