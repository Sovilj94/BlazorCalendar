using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.MonthViewModels
{
    public class MonthGridItemViewModel
    {
        public string CSSGridPosition { get; set; }
        public string GridItemColor { get; set; }
        public string CSSClass { get; set; }
        public ICalendarEvent Event { get; set; }
        public DateTime Day { get; set; }
    }
}