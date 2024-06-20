
using Microsoft.AspNetCore.Components;

namespace BlazorCalendar.Models.WeekViewModels
{
    public class WeekDayViewModel
    {
        public List<WeekGridItemViewModel>? GridItems { get; set; }

        public List<WeekTimeCellViewModel> TimeCells { get; set; }

        public TimeDivision TimeDivision { get; set; }

        public DateTime Day { get; set; }

        public string WeekDaysColor { get; set; } = "#FFF";

        public string SaturdayColor { get; set; } = "#ECF4FD";

        public string SundayColor { get; set; } = "#DBE7F8";

        public int? MaxNumberOfColumns { get; set; }
    }
}
