

namespace BlazorCalendar.Models.ViewModel
{
    public class TimeSideBarViewModel
    {
        public string Title { get; set; } = "Vreme";
        public DateTime Time { get; set; }
        public TimeDivision TimeDivision { get; set; }
    }
}
