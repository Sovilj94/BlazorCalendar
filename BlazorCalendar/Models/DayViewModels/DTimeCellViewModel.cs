﻿
namespace BlazorCalendar.Models.DayViewModels
{
    public class DTimeCellViewModel
    {
        public string CSSGridPosition { get; set; }
        public string CSSbackground { get; set; }
        public DateTime Time { get; set; }
        public bool IsAllDayTimesCell { get; set; } = false;
        public int? ColumnsSpan { get; set; }
        public int? Row { get; set; }
        public int? Column { get; set; }
    }
}
