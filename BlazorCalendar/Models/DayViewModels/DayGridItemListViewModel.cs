namespace BlazorCalendar.Models.DayViewModels
{
    public class DayGridItemListViewModel
    {
        public DateTime Day { get; set; }
        public List<DayGridItemViewModel> GridItemsViewModel { get; set; }

        public string CSSGridPosition { get; set; } = "grid-row-start:5; grid-column-start:1;";

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
