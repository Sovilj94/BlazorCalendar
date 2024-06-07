﻿
namespace BlazorCalendar.Models.ViewModel
{
    public class GridItemViewModel
    {
        public DateTime Day { get; set; }

        public Tasks Task { get; set; }

        public string CSSGridPosition { get; set; }

        public string GridItemColor { get; set; }

        public string TaskColor { get; set; }

        public string ClassPin { get; set; }

        public string ClassPointer { get; set; }

        public int ColumnStart { get; set; }

        public int ColumnEnd { get; set; }

        public int RowStart { get; set; }

        public int RowEnd { get; set; }
    }
}
