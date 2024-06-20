
namespace BlazorCalendar.Models.WeekViewModels
{
    public class WeekDayViewModel
    {
        public List<WeekGridItemViewModel>? GridItems { get; set; }

        public List<WeekTimeCellViewModel> TimeCells { get; set; }

        public TimeDivision TimeDivision { get; set; }

        public DateTime Day { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }

        public int? MaxNumberOfColumns { get; set; }
    }
}
