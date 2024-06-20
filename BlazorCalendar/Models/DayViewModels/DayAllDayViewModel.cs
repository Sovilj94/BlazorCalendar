
using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.DayViewModels
{
    public class DayAllDayViewModel
    {
        public List<DayGridItemViewModel> GridItemsViewModel { get; set; }

        public DayGridItemListViewModel GridItemListViewModel { get; set; } = new();

        public DayTimeCellViewModel TimeCellViewModel { get; set; }

        public List<ICalendarEvent> Events { get; set; }

        public DateTime Day { get; set; }

        public DateTime FirstDateWeek { get; set; }
    }
}
