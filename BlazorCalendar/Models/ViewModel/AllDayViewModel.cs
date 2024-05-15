
namespace BlazorCalendar.Models.ViewModel
{
    public class AllDayViewModel
    {
        List<GridItemViewModel> GridItems { get; set; }

        List<TimeCellViewModel> TimeCells { get; set; }

        public int Column { get; set; }

        public DateTime Day { get; set; }

        public DateTime FirstDateWeek { get; set; }
    }
}
