namespace BlazorCalendar.Models.Interfaces
{
    public interface ICalendarViewFactory
    {
        ICalendarView CreateCalendarView(DateTime firstDate, TimeDivisionEnum timeDivision, List<ICalendarEvent> calendrEvents);
    }
}
