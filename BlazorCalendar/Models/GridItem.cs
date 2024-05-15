
namespace BlazorCalendar.Models
{
    public abstract class GridItem
    {
        public int ID { get; set; }
        public string? Key { get; set; }
        public string Caption { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }
        public int ColumnStart { get; set; }
        public int ColumnEnd { get; set; }
        public int RowStart { get; set; }
        public int RowEnd { get; set; }
    }
}
