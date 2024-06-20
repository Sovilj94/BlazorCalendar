

namespace BlazorCalendar.Models.DayViewModels
{
    public class DayTimeSideBarViewModel
    {
        public string Title { get; set; } = "Vreme";
        public DateTime Time { get; set; }
        public TimeDivision TimeDivision { get; set; }
    }
}
