

namespace BlazorCalendar.Models.DayViewModels
{
    public class DTimeSideBarViewModel
    {
        public string Title { get; set; } = "Vreme";
        public DateTime Time { get; set; }
        public TimeDivision TimeDivision { get; set; }
    }
}
