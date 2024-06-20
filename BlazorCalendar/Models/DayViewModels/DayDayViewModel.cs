using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.DayViewModels
{
    public class DayDayViewModel
    {
        public List<DayGridItemViewModel>? GridItemsViewModel { get; set; }

        public List<DayTimeCellViewModel> TimeCellsViewModel { get; set; }

        public TimeDivision TimeDivision { get; set; }

        public DateTime Day { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }

        public int? MaxNumberOfColumns { get; set; }
    }
}
