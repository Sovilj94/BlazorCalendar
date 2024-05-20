
using System.Reflection.Metadata.Ecma335;

namespace BlazorCalendar.Models.ViewModel
{
    public class DayCalendarViewModel
    {
        public  List<Tasks> Tasks { get; set; }

        public GridItemViewModel GridItem { get; set; }

        public TimeCellViewModel TimeCell { get; set; } 

        public TimeDivision TimeDivision { get; set; }

        public DateTime Day { get; set; }

        public string SaturdayColor { get; set; }

        public string SundayColor { get; set; }

        public string WeekDaysColor { get; set; }
    }
}
