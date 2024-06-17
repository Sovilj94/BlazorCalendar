using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.DayViewModels
{
    public class DDayViewModel
    {
        public List<DGridItemViewModel>? GridItemsViewModel { get; set; }

        public List<DTimeCellViewModel> TimeCellsViewModel { get; set; }

        public TimeDivision TimeDivision { get; set; }

        public DateTime Day { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }

        public int? MaxNumberOfColumns { get; set; }
    }
}
