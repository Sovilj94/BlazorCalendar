
namespace BlazorCalendar.Models.ViewModel
{
    public class AllDayViewModel
    {
        public GridItemViewModel GridItem { get; set; }

        public List<GridItemViewModel> GridItems { get; set; }

        public List<TimeCellViewModel> TimeCells { get; set; }

        public List<Tasks> Tasks{ get; set; }

        public DateTime Day { get; set; }

        public DateTime FirstDateWeek { get; set; }
    }
}
