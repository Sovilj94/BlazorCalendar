namespace BlazorCalendar.Models.MonthViewModels
{
    public class MonthGridItemListViewModel
    {
        public DateTime Day { get; set; }
        public List<MonthGridItemViewModel> GridItemsViewModel { get; set; }

        public int EventCounter { get; set; }
        public string CSSGridPosition { get; set; }
        public string CSSClass { get; set; }
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
