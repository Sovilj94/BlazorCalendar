
using BlazorCalendar.Models.Interfaces;

namespace BlazorCalendar.Models.DayViewModels
{
    public class DAllDayViewModel
    {
        public List<DGridItemViewModel> GridItemsViewModel { get; set; }

        public DGridItemListViewModel GridItemListViewModel { get; set; } = new();

        public DTimeCellViewModel TimeCellViewModel { get; set; }

        public List<ICalendarEvent> Events { get; set; }

        public DateTime Day { get; set; }

        public DateTime FirstDateWeek { get; set; }
    }
}
