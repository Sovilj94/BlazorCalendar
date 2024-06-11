
using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.ViewModel
{
    public class GridItemViewModel
    {
        public DateTime Day { get; set; }

        public ICalendarEvent Event { get; set; }

        public string CSSGridPosition { get; set; }

        public string GridItemColor { get; set; }

        public string EventColor { get; set; }

        public string ClassPin { get; set; }

        public string ClassPointer { get; set; }

        public int ColumnStart { get; set; }

        public int ColumnEnd { get; set; }

        public int RowStart { get; set; }

        public int RowEnd { get; set; }
    }
}
