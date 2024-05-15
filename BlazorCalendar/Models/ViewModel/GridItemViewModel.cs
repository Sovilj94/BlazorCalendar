
namespace BlazorCalendar.Models.ViewModel
{
    public class GridItemViewModel
    {
        public DateTime Day { get; set; }

        public Tasks Task { get; set; }

        public string CSSGridPosition { get; set; }

        public string TaskColor { get; set; }

        public string ClassPin { get; set; }

        public string ClassPointer { get; set; }
    }
}
