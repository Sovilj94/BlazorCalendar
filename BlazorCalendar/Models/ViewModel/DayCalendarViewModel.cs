
namespace BlazorCalendar.Models.ViewModel
{
    public class DayCalendarViewModel
    {
        public decimal NumberOfTimeCells { get; set; }

        public int column { get; set; } = 1;

        List<GridItemViewModel> GridItem { get; set; }

        List<TimeCellViewModel> TimeCells { get; set; }

        public decimal Multiplyer { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }
    }
}
