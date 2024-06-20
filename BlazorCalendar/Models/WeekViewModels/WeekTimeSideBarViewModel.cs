

namespace BlazorCalendar.Models.WeekViewModels
{
    public class WeekTimeSideBarViewModel
    {
        public string Title { get; set; } = "Vreme";
        public DateTime Time { get; set; }
        public TimeDivision TimeDivision { get; set; }
    }
}
