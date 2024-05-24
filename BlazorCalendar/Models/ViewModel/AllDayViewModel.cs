
namespace BlazorCalendar.Models.ViewModel
{
    public class AllDayViewModel
    {
        public GridItemViewModel GridItem { get; set; }

        public TimeCellViewModel TimeCell { get; set; }

        public List<Tasks> Tasks{ get; set; }

        public int Column { get; set; }

        public DateTime Day { get; set; }

        public DateTime FirstDateWeek { get; set; }
    }
}
