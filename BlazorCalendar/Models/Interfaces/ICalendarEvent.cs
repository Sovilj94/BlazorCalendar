namespace BlazorCalendar.Models.Interfaces
{
    public interface ICalendarEvent
    {
        public int ID { get; set; }
        public string? Key { get; set; }
        public string Caption { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public string? ForeColor { get; set; }
        public FillStyleEnum FillStyle { get; set; }
        public string? Comment { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public bool NotBeDraggable { get; set; }
        public int Type { get; set; }
    }
}
